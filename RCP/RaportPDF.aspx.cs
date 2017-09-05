using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.Controls.Reports;
using HRRcp.Scorecards.App_Code;
using AjaxControlToolkit;

/*
UWAGA !!! po zmianach nadpisać RaportPDF !!! zmieniając EnableEventValidation="true" -> "false"

//r - te linie zostały zaremowane zeby wywalić cntReport2 
*/

namespace HRRcp
{
    public partial class RaportPDF : System.Web.UI.Page
    {
        //public const string RAPORTY = "RAPORTY";  //b2b
        public const string RAPORTY = "REPORT";

        
        
        
        // żeby pdf działał ... <<< docelowo zmiana na RaportPDF        
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        


        
        public bool AdminEdit
        {
            get { return App.User.HasRight(AppUser.rSuperuser); }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if SCARDS
            this.MasterPageFile = App.ScReportMaster;
#elif MS
            this.MasterPageFile = App.MSMaster;
#elif RCP
            MasterPageFile = App.GetMasterPage();
#endif
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvReport);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SCARDS
                jsScards.Visible = true;
#endif
                bool adm = AdminEdit;

                //if (!App.User.HasRight(AppUser.rRaporty2))
                //    App.Redirect(App.DefaultForm);
                ////if (!User.IsInRole(Utils.rRaportyWidok))
                ////    Response.Redirect("/");

                Prepare();
                
                //App.SetAsReport(null);
                
                cntFilter.DataBind();

                //if (!User.IsInRole(Utils.rRaportyAdm))
                if (!adm)
                    cntFilter.Visible = cntFilter.Present;   // wyłączenie dla nie Admina                               
                Tools.MakeConfirmButton(btDelete, "Potwierdź usunięcie raportu.", "Raport zostanie permanentnie usunięty i nie będzie można go przywrócić.\\nPotwierdź ponownie usunięcie raportu.");
            }
#if MS
            cntReportHeader.Title = Title;
            cntReportHeader.Title2 = Title2;

            //lbTitle.Text = Title;
            
            //cntReportHeader.Visible = false;
            cntReportHeaderM.Visible = true;
#else
            cntReportHeader.Title = Title;
            cntReportHeader.Title2 = Title2;
#endif
#if RCP
            //paPrintFooter.Visible = true;
            //lbPrintFooter.Text = String.Format("Wydrukowano z systemu {0} v. {1}", Tools.GetAppName(), Tools.GetAppVersion()); 
            //lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + App.User.ImieNazwisko;
#endif
            UpdateReport();


            cntReportHeader.DataBind();
            DownloadPDF();
        }

        private void HandleError(string msg)
        {
            //zapis do log jeszcze dodac
            SQL = String.Format("select '{0}' as [{1}]", String.IsNullOrEmpty(msg) ? null : msg.Replace("'", "''"), HRRcp.App_Code.L.p("BŁĄD STRUKTURY RAPORTU"));
            SqlDataSource1.SelectCommand = SQL;
            gvReport.DataBind();
            //r cntReport2.SQL = SQL;
        }

        public void UpdateReport()
        {
            //r if (cntFilter.ApplyTo(cntReport2.DataSourceSql))       
            //r     cntReport2.ReportGrid.DataBind();

            SqlDataSource1.SelectCommand = SQL;
            SqlDataSource1.UpdateCommand = FooterSQL;
            if (cntFilter.ApplyTo(SqlDataSource1))    //true jak parametry się różnią więc trzeba przebindować
                try
                {
                    gvReport.DataBind();
                }
                catch (Exception ex)
                {
                    HandleError(ex.Message);
                }
        }

        public static void Show(string id)
        {
            //string d = Tools.DateTimeToStr(DateTime.Now);
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            App.Redirect("RaportF.aspx" + "?p=" + e);
        }

        private bool Prepare()
        {
            if (Grid.cryptParams)
            {
                string p = Tools.GetStr(Request.QueryString["p"]);
                if (!String.IsNullOrEmpty(p))
                {
                    int pp;
                    if (Int32.TryParse(p, out pp))   // plomb, coś więcej niż plomba ... żeby można było przekazać nieszyfrowane
                        p = pp.ToString();
                    else
                        p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                    if (!String.IsNullOrEmpty(p))
                    {
                        string[] par = Tools.GetLineParams(p);   // 0-repId
                        string repId = par[0];
                        ReportId = repId;
                        if (Prepare(repId))
                            return true;
                        else
                        {
                            const string Err1 = "Niepoprawna definicja raportu";
                            Log.Error(Log.RAPORTY, Err1, String.Format("Brak definicji raportu: {0}", repId));
                            AppError.Show(Err1);
                            return false;
                        }

                        /*
                        for (int i = 0; i < par.Length - 1; i++)   // ostatni parametr to 0x0
                        {
                            string pname = "p" + (i + 1).ToString();   // p1 p2 ...
                            sql = sql.Replace("@" + pname, par[i]);
                        }
                        */ 
                    }
                }
            }
            const string Err2 = "Niepoprawne wywołanie raportu";
            Log.Error(Log.RAPORTY, Err2, null);
            AppError.Show(Err2);
            return false;   // make compiler happy
        }

        public bool x_Prepare(string repId)
        {
            DataRow dr = db.getDataRow(String.Format("select * from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, repId));
            if (dr != null)
            {
                if (App.User.HasRight(AppUser.rSuperuser))
                //if (User.IsInRole(Utils.rRaportyAdm))
                {
                    //paEdit.Visible = true;
                    //lbInfo.Text = String.Format("Raport Id: {0}", repId);
                    btEdit.Text = String.Format("Edycja ({0})", repId);
                    cntSqlReportEdit.Visible = true;
                }
                cntReportScheduler.Prepare(App.User.Id, repId);

                string title = db.getValue(dr, "MenuText");
                //string filter = db.getValue(dr, "SqlParams");
                string sql = db.getValue(dr, "Sql");
                sql = MakeParameters(sql);
                SQL = sql;
                //-----testy---------------
                cntFilter.ReportId = repId;
                //cntReportHeader.Caption = title;
                cntReportHeader.Title = title;
                //cntReport2.SQL = MakeParameters(sql, cntFilter);
                //r cntReport2.SQL = sql;
                //-------------------------
                //cntFilter.MakeParameters(SqlDataSource1);
                //MakeParameters(sql, SqlDataSource1, cntFilter);
                //SqlDataSource1.SelectCommand = sql;
                return true;
            }
            else
                return false;
        }

        public bool Prepare(string repId)
        {
            string rights = AppUser.SetRights(App.User.Rights, App.User.OriginalRights, // zeruję uprawnienia admina jeżeli sam ich nie mam
                                                AppUser.rScorecardsRestricted);

            DataRow dr = db.getDataRow(String.Format("select *, case when Rights is null then 1 else dbo.CheckRightsExpr('{2}', Rights) end as MaPrawo from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, repId, rights));
            if (dr != null)
            {
                if (db.getValue(dr, "MaPrawo") == "1")
                {
                    if (App.User.HasRight(AppUser.rSuperuser))
                    //if (User.IsInRole(Utils.rRaportyAdm))
                    {
                        //paEdit.Visible = true;
                        //lbInfo.Text = String.Format("Raport Id: {0}", repId);
                        btEdit.Text = String.Format("Edycja ({0})", repId);
                        btEdit.Visible = true;
                        btNew.Visible = true;
                        btDelete.Visible = true;
                        cntSqlReportEdit.Visible = true;
                    }
                    cntReportScheduler.Prepare(App.User.Id, repId);

                    
                    string par1 = db.getValue(dr, "Par1");
                    string par2 = db.getValue(dr, "Par2");

                    Title = String.IsNullOrEmpty(par1) ? db.getValue(dr, "MenuText") : par1;
                    Title2 = String.IsNullOrEmpty(par2) ? String.Empty : par2;
                    string filter = db.getValue(dr, "SqlParams");
                    string sql = db.getValue(dr, "Sql");
                    sql = MakeParameters(sql);
                    SQL = sql;
                    FooterSQL = filter;

                    cntFilter.ReportId = repId;

                    //cntReportHeader.Title = title;
                    //cntReportHeader.Title2 = title2;
                    //r cntReport2.SQL = sql;
                    //r cntReport2.FooterSql = filter;   //<<<<<<< tymczas, filter będzie inaczej definiowany
                    
                    return true;
                }
                else
                {
                    App.ShowNoAccess(db.getValue(dr, "MenuText"), App.User);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }





        public static string MakeParameters(string sql)  // stałe parametry
        {
            if (!String.IsNullOrEmpty(sql))
            {
                string p = Tools.GetStr(HttpContext.Current.Request.QueryString["p"]);   // z cntReport2
                if (!String.IsNullOrEmpty(p))
                {
                    p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                    if (!String.IsNullOrEmpty(p))
                    {
                        string[] par = Tools.GetLineParams(p);
                        for (int i = 0; i < par.Length - 1; i++)   // ostatni parametr to 0x0
                        {
                            string pname = "p" + (i + 1).ToString();   // p1 p2 ...
                            sql = sql.Replace("@" + pname, par[i]);
                        }
                    }
                }
            }
            //----- zmienne predefiniowane -----
            AppUser user = AppUser.CreateOrGetSession();
            sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
            sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));  // to najpierw
            sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
            sql = sql.Replace("@Login", db.strParam(user.Login));
            //sql = sql.Replace("@lang", db.strParam(L.Lang));
            sql = sql.Replace("@lang", db.strParam("PL"));
            //------------
            return sql;
        }

        /*
        public static string MakeParameters(string sql, cntFilter2 filter)
        {
            sql = MakeParameters(sql);
            sql = filter.ApplyTo(sql);
            return sql;
        }
        
        public static string MakeParameters(string sql, SqlDataSource ds, cntFilter2 filter)
        {
            sql = MakeParameters(sql);
            ds.SelectCommand = sql;
            filter.ApplyTo(ds);
            return sql;
        }
        */


        //----------------------------
        protected void btEdit_Click(object sender, EventArgs e)
        {
            cntSqlReportEdit.Show(ReportId, RAPORTY);
        }

        protected void btNew_Click(object sender, EventArgs e)
        {
            cntSqlReportEdit.Show(null, RAPORTY);
        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (cntSqlReportEdit.Delete(ReportId))
                Tools.Back();
        }
        //-----
        protected void cntFilter_Clear(object sender, EventArgs e)
        {
            UpdateReport();
        }

        protected void cntFilter_Edit(object sender, EventArgs e)
        {
            UpdateReport();
        }

        protected void cntFilter_EndEdit(object sender, EventArgs e)
        {
            UpdateReport();
        }
        //-----        
        protected void cntSqlReportEdit_Save(object sender, EventArgs e)
        {
            Prepare();
            UpdateReport();
            upMain.Update();
        }
        //-----
        string exMsg = null;
        int exCnt = 0;

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                if (exCnt == 0)  // jeszcze zabezpieczenie ze tylko za pierwszym razem
                {
                    //HandleError(e.Exception);
                    //upMain.Update();
                    exMsg = e.Exception.Message;
                    e.ExceptionHandled = true;
                }
                exCnt++;
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(exMsg))
            {
                string msg = exMsg;
                exMsg = null;
                HandleError(msg);
            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            int ccc = Controls.Count;

            string filename = null;
            foreach (Control cnt in upMain.Controls)
                if (cnt is cntReport)
                {
                    filename = ((cntReport)cnt).Title;
                    break;
                }

            if (String.IsNullOrEmpty(filename))
            {
                filename = hidReportTitle.Value;
                if (String.IsNullOrEmpty(filename))
                    //filename = "report.csv";
                    filename = "report";
            }

            App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }

        //---------------
        public String GetFilename(Control cnt)
        {
            if (cnt is cntReport)
            {
                return ((cntReport)cnt).Title;
            }
            else if (cnt is cntReport2)
            {
                return ((cntReport2)cnt).Title;
            }
            else if (cnt is HRRcp.Controls.EliteReports.cntReport)
            {
                return ((HRRcp.Controls.EliteReports.cntReport)cnt).Title;
            }
            return null;
        }

        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }

        public static void SetExtVisible(Control cnt)  //przeszukuje dzieci
        {
            if (cnt is ExtenderControlBase)
            {
                //cnt.Visible = false;
                //cnt.Parent.Controls.Remove(cnt);
            }
            else
                foreach (Control cc in cnt.Controls)
                    SetExtVisible(cc);
        }

        public static void RemoveExtenders(Control cnt)
        {
            List<Control> list = new List<Control>();
            GetExtenders(cnt, ref list);
            foreach (Control c in list)
                c.Parent.Controls.Remove(c);
        }

        public static void GetExtenders(Control cnt, ref List<Control> list)  //przeszukuje dzieci
        {
            if (cnt is ExtenderControlBase)
                list.Add(cnt);
            else
                foreach (Control cc in cnt.Controls)
                    GetExtenders(cc, ref list);
        }

        public void DownloadPDF()
        {
            List<Control> Controls = new List<Control>();
            foreach (Control Cnt in GetAllControls(this))
            {
                if (Cnt is HRRcp.Controls.EliteReports.cntReport
                 || Cnt is HRRcp.Controls.Reports.cntReport
                 || Cnt is HRRcp.Controls.Reports.cntReport2
                 || Cnt is HRRcp.Controls.Reports.cntReport3    // Control\Portal\WniosekZmianyDanych\cntReport2
                    //|| Cnt is HRRcp.Controls.Reports.cntFilter2 
                 || (Cnt is WebControl && (((WebControl)Cnt).Attributes["pdf"] ?? "").ToLower() == "true" || Cnt.ID == "pdfwrapper")
                )
                {
                    if (Cnt.Visible)
                    {
                        RemoveExtenders(Cnt);
                        Controls.Add(Cnt);
                    }
                }
            }

            if (Controls.Count > 0)
            {
                PDF PDF = new PDF();
                /*
                List<PDF.ReplaceObject> ReplaceObjects = new List<PDF.ReplaceObject>();
                ReplaceObjects.Add(new PDF.ReplaceObject("_hap", "<img src='../Scorecards/images/Happy64px.png' style='border-width:0px; width: 16px;'>"));
                ReplaceObjects.Add(new PDF.ReplaceObject("_sad", "<img src='../Scorecards/images/Sad64px.png' style='border-width:0px; width: 16px;'>"));
                ReplaceObjects.Add(new PDF.ReplaceObject("_spook", "<img src='../Scorecards/images/Skeleton64px.png' style='border-width:0px; width: 16px;'>"));
                PDF.ReplaceObjects = ReplaceObjects;
                */
                if (PDF.Download(Controls, Server, Response, Request, db.RemovePL(Tools.PrepareFilename(GetFilename(Controls[0])))) != 0)
                    Tools.ShowError("Wystąpił błąd podczas pobierania pliku.");
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas generowania pliku.");
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            DownloadPDF();
        }
        //-------------------
        public string SQL
        {
            set { ViewState["rapsql"] = value; }
            get { return Tools.GetStr(ViewState["rapsql"]); }
        }

        public string FooterSQL
        {
            set { ViewState["footsql"] = value; }
            get { return Tools.GetStr(ViewState["footsql"]); }
        }
        
        public string Title
        {
            set { ViewState["rtitle"] = value; }
            get { return Tools.GetStr(ViewState["rtitle"]); }
        }

        public string Title2
        {
            set { ViewState["rtitle2"] = value; }
            get { return Tools.GetStr(ViewState["rtitle2"]); }
        }

        public string ReportId
        {
            set { ViewState["repid"] = value; }
            get { return Tools.GetStr(ViewState["repid"]); }
        }

    }
}
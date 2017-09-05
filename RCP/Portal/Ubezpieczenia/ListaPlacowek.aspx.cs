using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia
{
    public partial class ListaPlacowek : System.Web.UI.Page
    {
        string typ = null;
        const string RAPORTY = "UBEZP_RAPORTY";
        const string ReportId = "2";

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvReport);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            typ = Request.QueryString["t"];
            if (!IsPostBack)
            {
                bool adm = AdminEdit;
                Prepare(ReportId);
                cntFilter.DataBind();
                if (adm)
                {
                    btEdit.Visible = true;
                    cntSqlReportEdit.Visible = true;
                }
                if (!adm)
                    cntFilter.Visible = cntFilter.Present;   // wyłączenie dla nie Admina   
            }
            UpdateReport();
        }

        public bool AdminEdit
        {
            get { return App.User.HasRight(AppUser.rSuperuser); }
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

        public bool Prepare(string repId)
        {
            DataRow dr = db.getDataRow(String.Format("select * from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, repId));
            if (dr != null)
            {
                string par1 = db.getValue(dr, "Par1");
                string par2 = db.getValue(dr, "Par2");

                //Title = String.IsNullOrEmpty(par1) ? db.getValue(dr, "MenuText") : par1;
                //Title2 = String.IsNullOrEmpty(par2) ? String.Empty : par2;
                string filter = db.getValue(dr, "SqlParams");
                string sql = db.getValue(dr, "Sql");

                sql = MakeParameters(sql);
                SQL = sql;
                SqlDataSource1.SelectCommand = SQL;

                cntFilter.ReportId = repId;

                //cntReportHeader.Title = title;
                //cntReportHeader.Title2 = title2;
                //r cntReport2.SQL = sql;
                //r cntReport2.FooterSql = filter;   //<<<<<<< tymczas, filter będzie inaczej definiowany

                return true;
            }
            else
            {
                return false;
            }
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
            SqlDataSource1.SelectCommand = SQL;
            //SqlDataSource1.UpdateCommand = FooterSQL;
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

        //-----
        public string SQL
        {
            set { ViewState["rapsql"] = value; }
            get { return Tools.GetStr(ViewState["rapsql"]); }
        }

        protected void btEdit_Click(object sender, EventArgs e)
        {
            cntSqlReportEdit.Show(ReportId, RAPORTY);
        }

        protected void cntSqlReportEdit_Save(object sender, EventArgs e)
        {
            Prepare(ReportId);
            UpdateReport();
            upMain.Update();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.Scorecards
{
    public partial class Raport : System.Web.UI.Page
    {
        //public const string RAPORTY = "RAPORTY";
        public const string RAPORTY = "REPORT";

        public static bool F
        {
            set { HttpContext.Current.Session["rapFilt"] = value; }
            get { return Tools.GetBool(HttpContext.Current.Session["rapFilt"], false); }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (F)
                    App.Redirect("RaportF.aspx" + Request.Url.Query);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
                //App.SetAsReport(null);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public static void Show(params string[] par) // na pierwszym miejscu id
        {
            string p = Tools.SetLineParams(par); 
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            App.Redirect("Scorecards/Raport.aspx" + "?p=" + e);
        }

        public static void Show(string id)
        {
            //string d = Tools.DateTimeToStr(DateTime.Now);
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            App.Redirect("Scorecards/Raport.aspx" + "?p=" + e);
        }

        private bool UserHasAccess(string rights)
        {
            return true;  // formuła !!!
        }

        private bool Prepare()
        {
            if (Grid.cryptParams)
            {
                string p = Tools.GetStr(Request.QueryString["p"]);
                if (!String.IsNullOrEmpty(p))
                {
                    p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                    if (!String.IsNullOrEmpty(p))
                    {
                        string[] par = Tools.GetLineParams(p);
                        string p0 = par[0];
                        string rights = AppUser.SetRights(App.User.Rights, App.User.OriginalRights, // zeruję uprawnienia admina jeżeli sam ich nie mam
                                                            AppUser.rScorecardsRestricted);

                        DataRow dr = db.getDataRow(String.Format("select *, case when Rights is null then 1 else dbo.CheckRightsExpr('{2}', Rights) end as MaPrawo from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, p0, rights));
                        if (dr != null)
                        {
                            if (db.getValue(dr, "MaPrawo") == "1")
                            {
                                string par1 = db.getValue(dr, "Par1");
                                string par2 = db.getValue(dr, "Par2");

                                string title = String.IsNullOrEmpty(par1) ? db.getValue(dr, "MenuText") : par1;
                                string title2 = String.IsNullOrEmpty(par2) ? String.Empty : par2;
                                string filter = db.getValue(dr, "SqlParams");
                                string sql = db.getValue(dr, "Sql");

                                cntReportHeader.Title = title;
                                cntReportHeader.Title2 = title2;
                                cntReport2.SQL = sql;
                                cntReport2.FooterSql = filter;   //<<<<<<< tymczas, filter będzie inaczej definiowany
 
                                return true;
                            }
                            else
                            {
                                App.ShowNoAccess(db.getValue(dr, "MenuText"), App.User);
                            }
                        }
                        else
                        {
                            Log.Error(Log.RAPORTY, "Brak definicji raportu.", p0);
                            AppError.Show("Niepoprawna definicja raportu");
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
            return false;
        }



        public static bool HasRight(int repId)
        {             
            string rights = AppUser.SetRights(App.User.Rights, App.User.OriginalRights, // zeruję uprawnienia admina jeżeli sam ich nie mam 
                                    AppUser.rScorecardsRestricted);
            string b = db.Select.Scalar("select case when Rights is null then 1 else dbo.CheckRightsExpr('{2}', Rights) end as MaPrawo from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, repId, rights);
            return b == "1";
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            /*
            Rep1.SQL1 = String.IsNullOrEmpty(fltrProgram.GetFilter()) ? AT : fltrProgram.GetFilter();
            Rep1.SQL2 = String.IsNullOrEmpty(fltrLeaders.GetFilter()) ? AT : fltrLeaders.GetFilter();
            Rep1.SQL3 = String.IsNullOrEmpty(ddlStructureFilter.SelectedValue) ? AT : ddlStructureFilter.SelectedValue;
            Rep1.ReloadTable();
             */ 
        }
    }
}
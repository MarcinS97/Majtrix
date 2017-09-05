using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp
{
    public partial class Raport : System.Web.UI.Page
    {
        public const string RAPORTY = "RAPORTY";

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if SCARDS
            this.MasterPageFile = App.ScReportMaster;
#endif
        }

        protected void Page_Init(object sender, EventArgs e)
        {
#if SCARDS
            if (!IsPostBack)
                if (Scorecards.Raport.F)
                    App.Redirect("RaportF.aspx" + Request.Url.Query);
#endif
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Prepare();
                //App.SetAsReport(null);
            }
        }

        public static void Show(string id)
        {
            //string d = Tools.DateTimeToStr(DateTime.Now);
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            //string d = Report.DecryptQueryString(e, key, salt);
            App.Redirect("Raport.aspx" + "?p=" + e);
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
                        DataRow dr = db.getDataRow(String.Format("select * from SqlMenu where Grupa = '{0}' and Id = {1}", RAPORTY, p0));
                        if (dr != null)
                        {
                            string title = db.getValue(dr, "MenuText");
                            string filter = db.getValue(dr, "SqlParams");
                            string sql = db.getValue(dr, "Sql");

                            cntReportHeader.Caption = title;
                            cntReport2.SQL = sql;

                            return true;
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
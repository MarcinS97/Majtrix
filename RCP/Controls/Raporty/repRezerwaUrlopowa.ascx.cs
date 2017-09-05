using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;

namespace HRRcp.Controls.Raporty
{
    public partial class repRezerwaUrlopowa : System.Web.UI.UserControl
    {
        const string active_tab = "tab_rezu";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Tools.SelectMenuFromSession(tabRaporty, active_tab);
                SelectTab();
            }
        }

        public void Prepare()
        {
            if (!Initialized)
            {
                Initialized = true;
                cntSelectOkres.Prepare(DateTime.Today, true);
                //cntSelectOkres.Prepare(DateTime.Parse("2013-11-30"), true);
                
                PrepareOkres();
                //cntReport1.GridDataBind();
            }
        }

        //---------------------
        private void PrepareOkres()
        {
            //cntReportHeader1.Caption = String.Format("Osoby korzystające ze stołówki w okresie: {0} - {1}", cntSelectOkres.DateFrom, cntSelectOkres.DateTo);

            cntReport1.Title = String.Format("Rezerwa urlopowa za miesiąc: {0}", cntSelectOkres.DateTo.Substring(0, 7));
            cntReport1.SQL1 = cntSelectOkres.DateFrom;
            cntReport1.SQL2 = cntSelectOkres.DateTo;
            cntReport1.Prepare();
            /*
            cntReport2.SQL1 = cntSelectOkres.DateFrom;
            cntReport2.SQL2 = cntSelectOkres.DateTo;
            cntReport2.Prepare();
            
            cntReport3.SQL1 = cntSelectOkres.DateFrom;
            cntReport3.SQL2 = cntSelectOkres.DateTo;
            cntReport3.Prepare();

            cntReport4.SQL1 = cntSelectOkres.DateFrom;
            cntReport4.SQL2 = cntSelectOkres.DateTo;
            cntReport4.Prepare();

            cntReport5.SQL1 = cntSelectOkres.DateFrom;
            cntReport5.SQL2 = cntSelectOkres.DateTo;
            cntReport5.Prepare();

            cntReport6.SQL1 = cntSelectOkres.DateFrom;
            cntReport6.SQL2 = cntSelectOkres.DateTo;
            cntReport6.Prepare();

            cntReport7.SQL1 = cntSelectOkres.DateFrom;
            cntReport7.SQL2 = cntSelectOkres.DateTo;
            cntReport7.Prepare();
             */ 
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            PrepareOkres();
        }

        private void SelectTab()
        {
            //Tools.SelectTab(tabRaporty, mvRaporty, active_tab, false);
        }

        protected void tabRaporty_MenuItemClick(object sender, EventArgs e)
        {
            SelectTab();
            RepDataBind();      // po zmianie SelectOkres nie zmieniał na pozostałych zakładkach
        }

        private void RepDataBind()
        {
            /*
            switch (tabRaporty.SelectedValue)
            {
                case "vDni":
                    cntReport1.GridDataBind();
                    break;
                case "vPodwojnie":
                    cntReport5.GridDataBind();
                    cntReport6.GridDataBind();
                    break;
                case "vPracownicy":
                    cntReport4.GridDataBind();
                    break;
                case "vDniPrac":
                    cntReport2.GridDataBind();
                    break;
                case "vSzczegoly":
                    cntReport3.GridDataBind();
                    break;
                case "vDuplikatyROGER":
                    cntReport7.GridDataBind();
                    break;
            }
             */ 
        }

        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            Report.ExportExcel(hidRepValue, cntReport1.Title, null);
            /*
            switch (tabRaporty.SelectedValue)
            {
                case "vDni":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
                case "vPodwojnie":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Podwojnie", null);
                    break;
                case "vPracownicy":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Pracownicy", null);
                    break;
                case "vDniPrac":
                    cntReport2.ExportCSV(cntReportHeader1.Caption + " - Dni", false);
                    break;
                case "vSzczegoly":
                    cntReport3.ExportCSV(cntReportHeader1.Caption + " - Szczegóły", false);
                    break;
                case "vDuplikatyROGER":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Duplikaty ROGER", null);
                    break;
            }
             */ 
        }
        //---------------------
        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }
    }
}
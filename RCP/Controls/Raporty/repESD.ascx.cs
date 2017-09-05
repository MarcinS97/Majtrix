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
    public partial class repESD : System.Web.UI.UserControl
    {
        const string active_tab = "tab_esd";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabRaporty, active_tab);
                SelectTab();
            }
        }

        public void Prepare()
        {
            if (!Initialized)
            {
                Initialized = true;
                deData.Date = DateTime.Today;
                
                PrepareZakres();
                //cntReport1.GridDataBind();
            }
        }

        //---------------------
        private void PrepareZakres()
        {
            cntReportHeader1.Caption = String.Format("Testy ESD na dzień: {0}", deData.DateStr);
            string data = deData.Date == null ? "19000101" : Tools.DateToStrDb((DateTime)deData.Date);

            cntReport1.SQL1 = data;
            cntReport1.Prepare();
            //cntReport2.SQL1 = rok;
            //cntReport2.Prepare();
            //cntReport3.SQL1 = rok;
            //cntReport3.Prepare();
        }

        protected void deData_Changed(object sender, EventArgs e)
        {
            PrepareZakres();
        }

        private void SelectTab()
        {
            Tools.SelectTab(tabRaporty, mvRaporty, active_tab, false);
        }

        protected void tabRaporty_MenuItemClick(object sender, EventArgs e)
        {
            SelectTab();
            RepDataBind();      // po zmianie SelectOkres nie zmieniał na pozostałych zakładkach
        }

        private void RepDataBind()
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vPracownicy":
                    cntReport1.GridDataBind();
                    break;
                    /*
                case "vError":
                    cntReport4.GridDataBind();
                    break;
                case "vOk":
                    cntReport6.GridDataBind();
                    break;
                case "vAll":
                    cntReport8.GridDataBind();
                    break;
                case "vPogonic":
                    cntReport9.GridDataBind();
                    break;
                case "vRealizacja":
                    cntReport5.GridDataBind();
                    //cntReport6.GridDataBind();
                    break;
                     */
            }
        }

        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vPracownicy":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Pracownicy", null);
                    break;
                    /*
                case "vError":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Plan niepoprawny", null);
                    break;
                case "vOk":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Plan poprawny", null);
                    break;
                case "vAll":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
                case "vPogonic":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Plan do uzupełnienia", null);
                    break;
                case "vRealizacja":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Realizacja", null);
                    break;
                    */
                //case "vDniPrac":
                //    cntReport2.ExportCSV(cntReportHeader1.Caption + " - Dni", false);
                //    break;
                //case "vSzczegoly":
                //    cntReport3.ExportCSV(cntReportHeader1.Caption + " - Szczegóły", false);
                //    break;
                //case "vDuplikatyROGER":
                //    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Duplikaty ROGER", null);
                //    break;
            }
        }
        //---------------------
        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }

    }
}
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
    public partial class repPlanUrlopow : System.Web.UI.UserControl
    {
        const string active_tab = "tab_pu";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabRaporty, active_tab);
                SelectTab();
            }
        }

        public void _Prepare()
        {
            if (!Initialized)
            {
                Initialized = true;
                if (ddlRok.Items.Count == 0) ddlRok.DataBind();
                Tools.SelectItem(ddlRok, DateTime.Today.Year);
                
                PrepareZakres();
                //cntReport1.GridDataBind();
            }
        }

        //---------------------
        private void PrepareZakres()
        {
            if (ddlRok.SelectedItem != null)
                cntReportHeader1.Caption = String.Format("Plan urlopów na rok {0}", ddlRok.SelectedItem.Text);
            //-----------------------------
            string rok = Tools.StrToInt(ddlRok.SelectedValue, 0).ToString();   // zabezpieczenie
            if (rok == "0")
            {
                Log.Error(Log.HAKER, "repPlanUrlopow.ddlRok", ddlRok.SelectedValue);
                App.ShowNoAccess("Raporty", null);
            }
            //-----------------------------
            cntReport1.SQL1 = rok;          //vBrak
            cntReport1.Prepare();
            cntReport2.SQL1 = rok;          //vRealizacjaMies
            cntReport2.Prepare();
            //cntReport3.SQL1 = rok;
            //cntReport3.Prepare();
            cntReport4.SQL1 = rok;          //vError
            cntReport4.Prepare();
            cntReport5.SQL1 = rok;          //vRealizacja
            cntReport5.Prepare();
            cntReport6.SQL1 = rok;          //vOK
            cntReport6.Prepare();
            //cntReport7.SQL1 = rok;
            //cntReport7.Prepare();
            cntReport8.SQL1 = rok;          //vAll
            cntReport8.Prepare();
            cntReport9.SQL1 = rok;          //vPogonic
            cntReport9.Prepare();
        }

        protected void ddlRok_SelectedIndexChanged(object sender, EventArgs e)
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
                case "vBrak":
                    cntReport1.GridDataBind();
                    break;
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
            }
        }

        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vBrak":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Brak planu", null);
                    break;
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
                case "vRealizacjaMies":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption + " - Realizacja miesięczna", null);
                    break;
 
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
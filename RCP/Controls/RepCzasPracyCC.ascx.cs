using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

/*
czas pracy na cc - porównanie z RCP dla Rafała
 * 
 * 
 * na razie w przygotowaniu, nie właczone, wymaga zmian w RepNadgodziny u czas dni
 * 
 * 
 * */

namespace HRRcp.Controls
{
    public partial class RepCzasPracyCC : System.Web.UI.UserControl
    {
        const string active_tab = "tab_czaspr";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabRaporty, active_tab);
                SelectTab();
            }
        }

        public void Prepare(DataSet dsKierownicy)
        {
            Tools.BindData(ddlKierownicy, dsKierownicy, "NI", "Id", true, null);
            cntSelectOkres.Prepare(DateTime.Today, true);
            repNadgodziny._Prepare(null, null, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny3.nmoRcp);
            
            repDni.Prepare(null, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
        }

        public void Prepare(string kierId)
        {
            ddlKierownicy.Visible = false;
            lbKierownik.Visible = false;
            if (String.IsNullOrEmpty(kierId)) kierId = App.User.Id;
            cntSelectOkres.Prepare(DateTime.Today, true);
            repNadgodziny._Prepare(null, kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny3.nmoRcp);
            //Execute();

            repDni.Prepare(kierId, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
        }
        //------------------
        private void SetHeader()
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vCzasSuma":
                    repHeader.Caption = "Porównanie czasu pracy z RCP za miesiąc " + cntSelectOkres.FriendlyName(4);
                    break;
                case "vCzasDni":
                    repHeader.Caption = "Czas pracy z RCP za miesiąc " + cntSelectOkres.FriendlyName(4);
                    break;
            }
            if (String.IsNullOrEmpty(ddlKierownicy.SelectedValue) || ddlKierownicy.SelectedValue == "-100") //"ALL")
                repHeader.Caption2 = null;
            else
                if (ddlKierownicy.Visible)
                    repHeader.Caption2 = ddlKierownicy.SelectedItem.Text.Replace(App.OldMarker, "");
                else
                    repHeader.Caption2 = AppUser.GetNazwiskoImie(repNadgodziny.KierId);
        }

        public void Execute()
        {
            SetHeader();

            switch (tabRaporty.SelectedValue)
            {
                case "vCzasSuma":
                    if (ddlKierownicy.Visible)
                        repNadgodziny._DataBindKier(ddlKierownicy.SelectedValue);
                    else
                        repNadgodziny._DataBindKier(repNadgodziny.KierId);
                    break;
                case "vCzasDni":
                    if (ddlKierownicy.Visible)
                        repDni.Prepare(ddlKierownicy.SelectedValue, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
                    else
                        repDni.Prepare(App.User.Id, cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
                    break;
            }
        }

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            Execute();
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            //SetHeader();
            Execute();
        }
        //------------------
        private void SelectTab()
        {
            Tools.SelectTab(tabRaporty, mvRaporty, active_tab, false);
        }

        protected void tabRaporty_MenuItemClick(object sender, EventArgs e)
        {
            SelectTab();
            //RepDataBind();      // po zmianie SelectOkres nie zmieniał na pozostałych zakładkach
            Execute();
        }

        /*
        private void RepDataBind()
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vCzasSuma":
                    //cntReport1.GridDataBind();
                    break;
                case "vCzasDni":
                    string kierId = 
                    cntRepDni.Prepare();
                    break;
            }
        }
        */
 
        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            /*
            switch (tabRaporty.SelectedValue)
            {
                case "vCzasSuma":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
                case "vCzasDni":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
            }
             */ 
        }

        
        
        
        //------------------
        public RepHeader Header
        {
            get { return repHeader; }
        }
















        /*

        //---------------------
        private void PrepareReport()
        {
            string kierId, kierNazwisko;
            if (ddlKierownicy.Items.Count > 1)
            {
                kierId = db.ISNULL(ddlKierownicy.SelectedValue, db.NULL);
                kierNazwisko = ddlKierownicy.SelectedItem.Text.Replace(App.OldMarker, "");
            }
            else
            {
                kierId = App.User.Id;
                kierNazwisko = App.User.ImieNazwisko;
            }

            cntReportHeader1.Caption = String.Format("Spóźnienia w okresie: {0} - {1}", cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            if (kierId == db.NULL)
                cntReportHeader1.Caption2 = null;
            else if (kierId == "-100") //"ALL")
                cntReportHeader1.Caption2 = "Wszyscy pracownicy";
            else
                cntReportHeader1.Caption2 = "Pracownicy przełożonego: " + kierNazwisko;

            cntReport1.SQL1 = cntSelectOkres.DateFrom;
            cntReport1.SQL2 = cntSelectOkres.DateTo;
            cntReport1.SQL3 = kierId;
            cntReport1.Prepare();

            cntReport4.SQL1 = cntSelectOkres.DateFrom;
            cntReport4.SQL2 = cntSelectOkres.DateTo;
            cntReport4.SQL3 = kierId;
            cntReport4.Prepare();
        }







        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            PrepareReport();
        }

        //---------------------
        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }



        */












    }
}
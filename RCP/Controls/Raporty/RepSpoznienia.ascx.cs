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
    public partial class RepSpoznienia : System.Web.UI.UserControl
    {
        const string active_tab = "tab_spozn";

        const int moKier = 0;
        const int moAdmin = 1;
        int FMode = moKier;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabRaporty, active_tab);
                SelectTab();
            }
        }

        public void PrepareAdm(DataSet dsKier, string nameField, string valueField)
        {
            switch (FMode)
            {
                default:
                case moKier:
                    PrepareKier();
                    break;
                case moAdmin:
                    Tools.BindData(ddlKierownicy, dsKier, nameField, valueField, true, null);
                    break;
            }
        }

        public void PrepareKier()
        {
            DataSet ds = db.getDataSet(String.Format(@"
declare @kierId int
declare @data datetime
set @kierId = {0}
set @data = GETDATE()
select Id, KadryId,
'Moi pracownicy' as Kierownik, 
--Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Kierownik, 
1 as Sort from Pracownicy where Id = @kierId 
union
select IdPracownika, KadryId, Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Kierownik, 2 as Sort 
from dbo.fn_GetTree(@kierId, @data) where Kierownik = 1
order by Sort, 3, KadryId                
                ", App.User.Id));
            Tools.BindData(ddlKierownicy, ds, "Kierownik", "Id", false, null);
            paSelectKier.Visible = db.getCount(ds) > 1;
        }

        public void Prepare()
        {
            if (!Initialized)
            {
                Initialized = true;
                cntSelectOkres.Prepare(DateTime.Today, true);
                //cntSelectOkres.Prepare(DateTime.Parse("2013-11-30"), true);

                PrepareReport();

                //cntReport1.GridDataBind();
            }
        }

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

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareReport();
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
                case "vCzas":
                    cntReport1.GridDataBind();
                    break;
                case "vSpoznienia":
                    cntReport4.GridDataBind();
                    break;
            }
        }

        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vCzas":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
                case "vSpoznienia":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
            }
        }
        //---------------------
        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }
    }
}
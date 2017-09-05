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
    public partial class RepRozlNadg : System.Web.UI.UserControl
    {
        const string active_tab = "tab_rona";

        //const string vRozlNadg = "vRozlNadg";
        //const string vSumyMies = "vSumyMies";

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
                
                //cntSelectOkres.Prepare(DateTime.Today, true);
                //cntSelectOkres.Prepare(DateTime.Parse("2013-11-30"), true);

                PrepareReport(false);

                //cntReport1.GridDataBind();
            }
        }

        //---------------------
        private bool CheckOkres()
        {
            bool b1 = deOd.Validate();
            bool b2 = deDo.Validate();
            if (b1 && b2)
            {
                DateTime d1 = (DateTime)deOd.Date;
                DateTime d2 = (DateTime)deDo.Date;
                if (d1 <= d2)
                {
                    if (d2.Subtract(d1).TotalDays < 366)
                        return true;
                    else deDo.Error = "Zbyt długi okres";
                }
                else deDo.Error = "Data wcześniejsza niż początkowa";
            }
            return false;
        }

        private void PrepareTitle()
        {
            View v = mvRaporty.GetActiveView();
            if (v == vRozlOkres)
                cntReportHeader1.Caption = String.Format("Rozliczenie nadgodzin w okresie: {0} - {1}", deOd.DateStr, deDo.DateStr);
            else if (v == vSumyMies)
            {
                cntReportHeader1.Caption = String.Format("Rozliczenie nadgodzin w miesiącach: {0} - {1}", deOd.YearMonthStr, deDo.YearMonthStr);
            }
        }

        private void PrepareReport(bool fromExec)
        {
            string dOd = null;
            string dDo = null;
            if (fromExec || deOd.Date != null || deDo.Date != null)
            {
                if (CheckOkres())
                {
                    dOd = deOd.DateStr;
                    dDo = deDo.DateStr;
                }
            }
            string kierId, kierNazwisko;
            switch (FMode)
            {
                case moAdmin:
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
                    break;
                default:
                case moKier:
                    kierId = App.User.Id;
                    kierNazwisko = App.User.ImieNazwisko;
                    break;
            }
            //cntReportHeader1.Caption = String.Format("Rozliczenie nadgodzin w okresie: {0} - {1}", cntSelectOkres.DateFrom, cntSelectOkres.DateTo);
            //cntReportHeader1.Caption = String.Format("Rozliczenie nadgodzin w okresie: {0} - {1}", deOd.DateStr, deDo.DateStr);
            
            PrepareTitle();

            if (kierId == db.NULL)
                cntReportHeader1.Caption2 = null;
            else if (kierId == "-100") //"ALL")
                cntReportHeader1.Caption2 = "Wszyscy pracownicy";
            else
                cntReportHeader1.Caption2 = "Pracownicy przełożonego: " + kierNazwisko;

            string pOd = db.nullStrParam(dOd);
            string pDo = db.nullStrParam(dDo);
            string pKier = db.nullParam(kierId);

            cntReport1.Prepare(pOd, pDo, pKier);
            cntReport2.Prepare(pOd, pDo, pKier);
        }
        
        











        /*
        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            PrepareReport(true);
        }
        */




        protected void btExec_Click(object sender, EventArgs e)
        {
            PrepareReport(true);
        }



        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareReport(true);
        }

        private void SelectTab()
        {
            Tools.SelectTab(tabRaporty, mvRaporty, active_tab, false);
            PrepareTitle();
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
                case "vRozlNadg":
                    cntReport1.GridDataBind();
                    break;
                case "vSumyMies":
                    cntReport2.GridDataBind();
                    break;
            }
        }

        public void ExportExcel(string hidRepValue)   // co co js zbierze
        {
            switch (tabRaporty.SelectedValue)
            {
                case "vRozlNadg":
                    Report.ExportExcel(hidRepValue, cntReportHeader1.Caption, null);
                    break;
                case "vSumyMies":
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
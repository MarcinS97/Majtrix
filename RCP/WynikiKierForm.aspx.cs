using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp
{
    /*
    UWAGA - nie moze leżeć na update panelu bo gubi się sortowanie jak zoom i back robię
    selected tab nie moze być zapisany w sesji bo przy back źle zaznacza
    */

    public partial class WynikiKierForm : System.Web.UI.Page
    {
        //private const string active_tab = "atabW";

#if SIEMENS
        const bool kartaRoczna = true;
#else
        const bool kartaRoczna = false;
#endif

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                if (App.User.IsKierownik && App.User.IsRaporty)
                {
                    Info.SetHelp(Info.HELP_KIERRAPORTY);

                    Prepare();
                    SelectTab();

                    lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + App.User.ImieNazwisko;
                    lbPrintVersion.Text = Tools.GetAppVersion();
                }
                else
                    App.ShowNoAccess("Raporty Kierownika", App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Panel Kierownika - Raporty");
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            switch (tabWyniki.SelectedValue)
            {
                default:
                    //((MasterPage)Master).SetWideJs(0);
                    break;
#if SIEMENS
                case "0":   //nadgodziny
                case "1":   //nadgodziny
                    ((MasterPage)Master).SetWideJs(2);  //wide2
                    break;
#else
                case "0":   //nadgodziny
                case "1":   //nadgodziny
                    //((MasterPage)Master).SetWideJs(1);
                    break;
#endif
                case "3":   //czaspracy
                case "4":   //podział na cc
                case "6":   //karta roczna
                case "8":   //spóźnienia
                case "9":   //esd
                    //((MasterPage)Master).SetWideJs(1);
                    break;
            }
            base.OnPreRender(e);
        }

        //--------------------
        private void SelectTab()
        {
            switch (tabWyniki.SelectedValue)
            {
                case "0":
                    mvWyniki.SetActiveView(pgNadgodziny);
                    break;
                case "1":
                    mvWyniki.SetActiveView(pgNadgodziny7);
                    break;
                case "2":
                    mvWyniki.SetActiveView(View1);
                    break;
                case "3":   //czaspracy
                    mvWyniki.SetActiveView(View2);
                    if (Tools.FirstExecute(ViewState, "czaspracy"))
                        repCzasPracy.Execute();
                    break;
                case "4":   //podzial na cc
                    mvWyniki.SetActiveView(pgPodzialCC);
                    /*
                    bool f = Tools.FirstExecute(ViewState, "podzialcc"); 
                    if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))
                    {
                        mvWyniki.SetActiveView(pgPodzialCCKwoty);
                        //if (f) cntReport2.Execute();
                    }
                    else
                    {
                        mvWyniki.SetActiveView(pgPodzialCC);
                        //if (f) cntReport1.Execute();
                    }
                     */ 
                    break;
                case "5":
                    mvWyniki.SetActiveView(vStolowka);
                    break;
                case "6":
                    mvWyniki.SetActiveView(pgKartaPracy);
                    break;
                case "8":
                    mvWyniki.SetActiveView(vSpoznienia);
                    break;
                case "9":
                    mvWyniki.SetActiveView(vESD);
                    break;
                case "12":
                    mvWyniki.SetActiveView(vRezerwaUrlopowa);
                    break;

            }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            switch (tabWyniki.SelectedValue)
            {
                default:
                case "0":
                    Report.ExportExcel(hidReport.Value, repHeader1.Caption, null);
                    break;
                case "1":
                    Report.ExportExcel(hidReport.Value, repHeader5.Caption, null);
                    break;
                case "2":
                    Report.ExportExcel(hidReport.Value, repHeader2.Caption, null);
                    break;
                case "3":
                    Report.ExportExcel(hidReport.Value, repCzasPracy.Header.Caption, null);
                    break;
                case "4":
                    Report.ExportExcel(hidReport.Value, repPodzialCC.Title, null);
                    /*
                    if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))
                        Report.ExportExcel(hidReport.Value, cntReport2.Title, null);
                    else
                        Report.ExportExcel(hidReport.Value, cntReport1.Title, null);                    
                     */ 
                    break;
                case "5":
                    repStolowka.ExportExcel(hidReport.Value);
                    break;    
                case "6":
                    Report.ExportExcel(hidReport.Value, repHeaderKarta.Caption, null);
                    break;
                case "8":
                    repSpoznienia.ExportExcel(hidReport.Value);
                    break;
                case "9":
                    repESD.ExportExcel(hidReport.Value);
                    break;
                case "12":
                    repRezerwaUrlopowa.ExportExcel(hidReport.Value);
                    break;
            }
        }
        //-----------------------------------------------
        public static string Url(int mode)
        {
            return Report.Url(App.WynikiForm, mode.ToString());
        }
        //--------------------
        private string OkresStatusOpened(int st)
        {
            switch (st)
            {
                default:
                    return " (okres rozliczeniowy otwarty)";
                case Okres.stClosed:
                    return null;
            }
        }

        private void SetHeaderNadgodziny()
        {
            repHeader1.Caption = "Nadgodziny za miesiąc " + cntSelectOkres.FriendlyName(1) + OkresStatusOpened(cntSelectOkres.Status);
        }

        private void SetHeaderNadgodziny7()
        {
            repHeader5.Caption = String.Format("Nadgodziny w okresie od: {0} do: {1}", deOd.DateStr, deDo.DateStr);
        }

        private void SetHeaderUrlopy()
        {
            string doD = deUrlopDo.DateStr;
            string odD = String.Format("{0}-01-01", doD.Substring(0, 4));            
            repHeader2.Caption = String.Format("Wykorzystanie urlopu w okresie: {0} do: {1}", odD, doD);
        }

        public void ExecuteNadgodziny7()
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
                    {
                        SetHeaderNadgodziny7();
                        Okres ok = new Okres((DateTime)deOd.Date);
                        repNadgodziny7._Prepare(null, App.User.Id, deOd.DateStr, deDo.DateStr, ok.Id.ToString(), ok.Status, ok.IsArch(), ok.StawkaNocna, RepNadgodziny.nmoKier);
                    }
                    else deDo.Error = "Zbyt długi okres";
                }
                else deDo.Error = "Data wcześniejsza niż początkowa";
            }
        }
        /*
        public void ExecuteUrlopy()
        {
            if (deUrlopDo.Validate())
            {
                SetHeaderUrlopy();
                repUrlopy.Prepare(user.Id, deDo.DateStr);
            }
        }
        */
        public void ExecuteUrlopy()
        {
            if (!String.IsNullOrEmpty(deUrlopDo.DateStr))
            {
                //if (deUrlopDo.Validate())
                if (deUrlopDo.IsValid)
                {
                    /*
                    string doD = deUrlopDo.DateStr.Substring(0,4);
                    if (doD == "2013")
                        Tools.ShowMessage("Ze względu na trwającą weryfikację danych po imporcie do nowego systemu HR, zestawienie na rok 2013 zostanie udostępnione wkrótce. Przepraszamy za utrudnienia.");
                    else
                    {
                        SetHeaderUrlopy();
                        repUrlopy.Prepare(App.User.Id, deUrlopDo.DateStr);
                    }
                    */
                    SetHeaderUrlopy();
                    repUrlopy.Prepare(App.User.Id, deUrlopDo.DateStr);
                }
                else Tools.ShowMessage("Niepoprawny format daty.");
            }
            else Tools.ShowMessage("Proszę wybrać datę końcową okresu zestawienia.");
        }

        //--------------------
        private void Prepare()
        {
            //----- nadgodziny -----
            cntSelectOkres.Prepare(DateTime.Today, true);
            repNadgodziny._Prepare(null, App.User.Id, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny.nmoKier);  // od razu się wykona !!!
            SetHeaderNadgodziny();
            //-------------------
            deOd.DateStr = cntSelectOkres.DateFrom;
            deDo.DateStr = cntSelectOkres.DateTo;
            //----- urlopy -----
            deUrlopDo.Date = Tools.eom(DateTime.Today);
            
            //repNadgodziny7.Prepare(null, deOd.DateStr, deDo.DateStr);
            //SetHeaderNadgodziny7();
            repCzasPracy.Prepare(App.User.Id);
            if (!App.User.HasRight(AppUser.rRepPodzialCC))  Tools.RemoveMenu(tabWyniki, "4"); 
            if (!App.User.HasRight(AppUser.rRepCzasPracy))  Tools.RemoveMenu(tabWyniki, "3");
            if (!App.User.HasRight(AppUser.rRepStolowka))   Tools.RemoveMenu(tabWyniki, "5");
            if (!App.User.HasRight(AppUser.rRepESD))        Tools.RemoveMenu(tabWyniki, "9");
            if (!App.User.HasRight(AppUser.rRepRezerwaUrlopowa)) Tools.RemoveMenu(tabWyniki, "12");

            //----- Karta roczna -----
            if (kartaRoczna)
            {
                PrepareRoki();
                PreparePracownicy();
            }
            else
            {
                Tools.RemoveMenu(tabWyniki, "6");
            }
            repSpoznienia.PrepareKier();

#if DBW || VICIM || VC
            Tools.RemoveMenu(tabWyniki, "4");
            Tools.RemoveMenu(tabWyniki, "5");
            Tools.RemoveMenu(tabWyniki, "12");
            Tools.RemoveMenu(tabWyniki, "9");
#endif

        }

        //--------------------
        protected void tabWyniki_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
            //Session[active_tab] = tabWyniki.SelectedValue;
        }

        protected void pgNadgodziny_Activate(object sender, EventArgs e)
        {
            //((MasterPage)Master).SetWideJs(false);
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            SetHeaderNadgodziny();
        }

        //------------------------    
        protected void btExec7_Click(object sender, EventArgs e)
        {
            ExecuteNadgodziny7();
        }

        protected void btExecUrlop_Click(object sender, EventArgs e)
        {
            ExecuteUrlopy();
        }
        //------------------------
        protected void vStolowka_Activate(object sender, EventArgs e)
        {
            repStolowka.Prepare();
        }
        //------------------------
        protected void vSpoznienia_Activate(object sender, EventArgs e)
        {
            repSpoznienia.Prepare();
        }
        //------------------------
        public void ExecuteKartaRoczna(bool fShowError)
        {
            string pracId = Tools.GetLineParam(ddlPracownicy.SelectedValue, 0);
            if (String.IsNullOrEmpty(pracId))
            {
                if (fShowError) Tools.ShowMessage("Proszę wybrać pracownika.");
            }
            else
            {
                string c = repHeaderKarta.Caption;
                int cut = c.EndsWith(".") ? 8 : 4;
                repHeaderKarta.Caption = c.Substring(0, c.Length - cut) + ddlRok.SelectedItem.Text;
                cntKartaRoczna.Prepare(ddlRok.SelectedValue, pracId);
            }
        }

        private void PrepareRoki()
        {
            //----- rok -----
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            int start_year = settings.SystemStartDate.Year;
            DateTime dt = DateTime.Today;
            int sel_year = dt.Year;
            for (int r = start_year; r <= dt.Year; r++)
            {
                ListItem li = new ListItem(r.ToString(), r.ToString());
                li.Selected = r == sel_year;
                ddlRok.Items.Add(li);
            }
        }

        private void PreparePracownicy()
        {
            string pid = ddlPracownicy.SelectedValue;

            DataSet dsP = Base.getDataSet(String.Format(@"
declare @rootId int
declare @boy datetime
declare @eoy datetime
declare @nadzien datetime
set @rootId = {0}
set @boy = '{1}0101'
set @eoy = '{1}1231'
set @nadzien = dbo.getdate(GETDATE()) 

select IdPracownika,
replicate('&nbsp;', (Hlevel - 1) * 4) +
Nazwisko + ' ' + Imie + ' (' + KadryId + ')' as Pracownik, SortPath
from dbo.fn_GetTreeOkres(@rootId, @boy, @eoy, case when @nadzien between @eoy and @eoy then @nadzien else @eoy end)    
order by SortPath", App.User.Id, ddlRok.SelectedValue));

            foreach (DataRow dr in db.getRows(dsP))
                dr[1] = HttpUtility.HtmlDecode(dr[1].ToString());

            Tools.BindData(ddlPracownicy, dsP, "Pracownik", "IdPracownika", true, pid);
        }

        protected void pgKartaPracy_Activate(object sender, EventArgs e)
        {
            //int x = 0;
        }

        protected void btExecute_Click(object sender, EventArgs e)
        {
            ExecuteKartaRoczna(true);
        }

        protected void ddlRok_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteKartaRoczna(true);
        }

        protected void ddlPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteKartaRoczna(false);
        }

        protected void vESD_Activate(object sender, EventArgs e)
        {
            repESD.Prepare();
        }

        protected void vRezerwaUrlopowa_Activate(object sender, EventArgs e)
        {
            repRezerwaUrlopowa.Prepare();
        }

    }
}

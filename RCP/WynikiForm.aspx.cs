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


    /* ----------------------------------------------------
    Resend przy Back - ajaxcontroltoolkit w nowej wersji powoduje ze sie takie okienko pojawia:
    do web.config dodać:
        1. 
        <compilation debug="false">
     
     * jak jest włączony debug to nadal będzie formatka tak działać !!!
     * Ctrl-F5 uruchamia bez debugu
     
        2. 
        <system.web>
            <httpHandlers>
                <add verb="*" path="AjaxFileUploadHandler.axd" type="AjaxControlToolkit.AjaxFileUploadHandler, AjaxControlToolkit"/>
                <add verb="*" path="CombineScriptsHandler.axd" type="AjaxControlToolkit.CombineScriptsHandler, AjaxControlToolkit"/>          
        3.
	    <system.webServer>
		    <validation validateIntegratedModeConfiguration="false"/>
		    <handlers>
                <add name="AjaxFileUploadHandler" verb="*" path="AjaxFileUploadHandler.axd" type="AjaxControlToolkit.AjaxFileUploadHandler, AjaxControlToolkit"/>
                <add name="CombineScriptsHandler" verb="*" path="CombineScriptsHandler.axd" type="AjaxControlToolkit.CombineScriptsHandler, AjaxControlToolkit" />
    ------------------------------------------------------- */

    public partial class WynikiForm : System.Web.UI.Page
    {
        //private const string active_tab = "atabW";

#if SIEMENS
        const bool rozlNadg = true;
        const bool planUrlopow = false;
#else
        const bool rozlNadg = true;   // od uprawnienia zależy
        const bool planUrlopow = true;
#endif

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //ScriptManager.GetCurrent(this).EnableHistory = true;
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.CacheControl = "Public";


            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            //Response.Cache.SetNoStore();
            //Response.AppendHeader("Pragma", "no-cache");


            //Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //Response.Cache.SetValidUntilExpires(false);
            //Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //Response.Cache.SetAllowResponseInBrowserHistory(true);

            if (!IsPostBack)
            {
                Tools.SetNoCache();
                
                AppUser user = AppUser.CreateOrGetSession();
                if (user.IsAdmin && user.IsRaporty)
                {
                    Info.SetHelp(Info.HELP_ADMRAPORTY);

                    Prepare();

                    string rep = (string)HttpContext.Current.Request.QueryString["p"];
                    if (!String.IsNullOrEmpty(rep))
                    {
                        Tools.SelectMenu(tabWyniki, "1");
                        SelectTab();
                        /*aa
                        repRealizacja.Mode = rep;
                        repHeader2.Caption = PracownicyControl.GetTitle(rep);
                         */
                    }
                    else
                    {
                        Tools.SelectMenu(tabWyniki, "0");
                        //Tools.SelectMenuFromSession(tabWyniki, active_tab);
                        SelectTab();
                        /*aa
                        repHeader2.Caption = PracownicyControl.GetTitle(repRealizacja.Mode);
                         */
                    }
                    lbPrintTime.Text = Base.DateTimeToStr(DateTime.Now) + " " + user.ImieNazwisko;
                    lbPrintVersion.Text = Tools.GetAppVersion();
                }
                else
                    App.ShowNoAccess("Raporty", user);
            }
        }


        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            /*
            if (!ScriptManager.GetCurrent(this).IsNavigating && (IsCallback || IsAsync))
                ScriptManager.GetCurrent(this).AddHistoryPoint("aaa", "bbb");
            */
            
            /*
            
            if (!ScriptManager.GetCurrent(this).IsNavigating && (IsCallback || IsInAsyncPostback()))
            {
                var state = new NameValueCollection();
                //OnCallbackHistory(state); // this gets state for all interested parties
                if (state.Count != 0)
                {
                    string key = ViewState["HistoryStateKey"] as string;   // empty on first AJAX call
                    if (string.IsNullOrEmpty(key) || ScriptManager.GetCurrent(this).EnableHistory)
                    {
                        key = CallbackHistoryKeyRoot + Interlocked.Increment(ref callbackHashKey).ToString();
                        ViewState["HistoryStateKey"] = key;
                        ScriptManager.GetCurrent(this).AddHistoryPoint("", key);
                    }
                    Session[key] = state;
                }
            }
            */
        }



        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                
                AppUser user = AppUser.CreateOrGetSession();
                if (user.IsAdmin)
                {
                    Info.SetHelp(Info.HELP_WYNIKI);

                    string rep = (string)HttpContext.Current.Request.QueryString["p"];
                    if (!String.IsNullOrEmpty(rep))
                    {
                        Tools.SelectMenu(tabWyniki, "1");
                        SelectTab();
                        repRealizacja.Mode = rep;
                        repHeader2.Caption = RepPodsumowanie.GetTitle(rep);
                    }
                    else
                    {
                        Tools.SelectMenuFromSession(tabWyniki, active_tab);
                        SelectTab();
                        /*if (tabWyniki.SelectedValue == "1")
                            ((MasterPage)Master).SetWideJs(true); // jak klikam w zakładkę                
                         * /
                        repHeader2.Caption = RepPodsumowanie.GetTitle(repRealizacja.Mode);
                    }
                }
                else
                    App.ShowNoAccess("Wyniki", user);
            }
        }
         */

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            switch (tabWyniki.SelectedValue)
            {
                case "0":
                case "1":
                case "4":
                case "9":
                case "10":  // Plan Urlopów
                case "11":  // ESD
#if RCP
                    //((MasterPage3)Master).SetWideJs(2);
#elif KDR
                    ((MasterPage2)Master).SetWideJs(2);
#else
                    ((MasterPage)Master).SetWideJs(2);  //wide2
#endif
                    break;
                default:
#if RCP
                    //((MasterPage3)Master).SetWideJs(2);
#elif KDR
                    ((MasterPage2)Master).SetWideJs(2);
#else
                    ((MasterPage)Master).SetWideJs(2);  //wide2
#endif
                    break;
            }


            /*
            if (IsPostBack)
            {
                Response.Write("<html><head><script>location.replace('" + Request.Path + "');\n" + "</script></head><body></body></html>\n");
                Response.End();
            }
            */            
            
            
            base.OnPreRender(e);
        }



        /*
        private void WebForm1_PreRender(object sender, System.EventArgs e)
        {

            if (IsPostBack)
            {

                Response.Write("<html><head><script>location.replace('" + Request.Path + "');\n" + "</script></head><body></body></html>\n");

                Response.End();

            }

        }
        */




        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Raporty");
        }
        //--------------------

        private void SelectTab()
        {
            switch (tabWyniki.SelectedValue)
            {
                case "0":
                    mvWyniki.SetActiveView(pgNadgodzinyKier);
                    break;
                case "1":
                    mvWyniki.SetActiveView(pgNadgodzinyProj);
                    break;
                case "4":
                    mvWyniki.SetActiveView(pgNadgodziny7);
                    break;
                case "2":
                    mvWyniki.SetActiveView(pgKartaPracy);
                    break;
                case "3":
                    mvWyniki.SetActiveView(pgUrlopy);
                    break;
                case "6":
                    mvWyniki.SetActiveView(pgPodzialCC);
                    repPodzialCC.Execute();
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
                case "7":
                    mvWyniki.SetActiveView(vStolowka);
                    break;
                case "8":
                    mvWyniki.SetActiveView(vSpoznienia);
                    break;
                case "9":
                    mvWyniki.SetActiveView(vRozlNadg);
                    break;
                case "10":
                    mvWyniki.SetActiveView(vPlanUrlopow);
                    break;
                case "11":
                    mvWyniki.SetActiveView(vESD);
                    break;
                case "12":
                    mvWyniki.SetActiveView(vRezerwaUrlopowa);
                    break;

                default:
                    int idx = Tools.StrToInt(tabWyniki.SelectedValue, -1);
                    mvWyniki.ActiveViewIndex = idx;
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
                    /*aa
                    Report.ExportExcel(hidReport.Value, repHeader2.Caption, repRealizacja.GetRowItemsList);
                     */
                    break;
                case "2":
                    Report.ExportExcel(hidReport.Value, repHeader2.Caption, null);
                    break;
                case "3":
                    Report.ExportExcel(hidReport.Value, repHeader3.Caption, null);
                    break;
                case "4":
                    Report.ExportExcel(hidReport.Value, repHeader4.Caption, null);
                    break;
                case "5":
                    Report.ExportExcel(hidReport.Value, repCzasPracy.Header.Caption, null);
                    break;
                case "6":
                    Report.ExportExcel(hidReport.Value, repPodzialCC.Title, null);
                    /*
                    if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))
                        Report.ExportExcel(hidReport.Value, cntReport2.Title, null);
                    else
                        Report.ExportExcel(hidReport.Value, cntReport1.Title, null);
                     */ 
                    break;
                case "7":
                    repStolowka.ExportExcel(hidReport.Value);
                    break;
                case "8":
                    repSpoznienia.ExportExcel(hidReport.Value);
                    break;
                case "9":
                    repRozlNadg.ExportExcel(hidReport.Value);
                    break;
                case "10":
                    repPlanUrlopow.ExportExcel(hidReport.Value);
                    break;
                case "11":
                    repESD.ExportExcel(hidReport.Value);
                    break;
                case "12":
                    repRezerwaUrlopowa.ExportExcel(hidReport.Value);
                    break;
            }
        }
        //--------------------------------------
        public static string Url(int mode)
        {
            return Report.Url(App.WynikiForm, mode.ToString());
        }
        //--------------------
        public void ExecuteKartaRoczna(bool fShowError)
        {
            string pracId = Tools.GetLineParam(ddlPracownicy.SelectedValue, 0);
            if (String.IsNullOrEmpty(pracId))
            {
                if (fShowError) Tools.ShowMessage("Proszę wybrać pracownika.");
            }
            else 
            {
                string c = repHeader2.Caption;
                int cut = c.EndsWith(".") ? 8 : 4;
                repHeader2.Caption = c.Substring(0, c.Length - cut) + ddlRok.SelectedItem.Text;
                cntKartaRoczna.Prepare(ddlRok.SelectedValue, pracId);
            }
        }

        private string OkresStatusOpened(int st)
        {
            switch (st)
            {
                default:
                    return " (okres rozliczeniowy otwarty)";
                case Okres.stClosed:
                    //return "Okres rozliczeniowy zamknięty";
                    return null;
            }
        }

        private void SetHeaderNadgodziny()
        {
            repHeader1.Caption = "Nadgodziny za miesiąc " + cntSelectOkres.FriendlyName(1) + OkresStatusOpened(cntSelectOkres.Status);
            if (String.IsNullOrEmpty(ddlKierownicy.SelectedValue) || ddlKierownicy.SelectedValue == "-100") //"ALL")
                repHeader1.Caption2 = null;
            else
                repHeader1.Caption2 = ddlKierownicy.SelectedItem.Text.Replace(App.OldMarker, "");
        }

        private void SetHeaderNadgodziny7()
        {
            repHeader4.Caption = String.Format("Nadgodziny w okresie od: {0} do: {1}", deOd.DateStr, deDo.DateStr);
        }

        private void SetHeaderNadgodzinyProj()
        {
            repHeader5.Caption = "Nadgodziny za miesiąc " + cntSelectOkresProj.FriendlyName(1) + OkresStatusOpened(cntSelectOkres.Status);
            if (String.IsNullOrEmpty(ddlProjekty.SelectedValue))
                repHeader5.Caption2 = null;
            else
                repHeader5.Caption2 = ddlProjekty.SelectedItem.Text.Replace(App.OldMarker,"");
        }

        private void SetHeaderUrlopy()
        {
            string doD = deUrlopDo.DateStr;
            string odD = String.Format("{0}-01-01", doD.Substring(0, 4));
            repHeader3.Caption = String.Format("Wykorzystanie urlopu w okresie: {0} do: {1}", odD, doD);
        }

        public void ExecuteNadgodziny()
        {
            SetHeaderNadgodziny();
            repNadgodziny._DataBindKier(ddlKierownicy.SelectedValue);            
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
                        repNadgodziny7._Prepare(null, 
                            ddlKierownicy7.SelectedValue, 
                            deOd.DateStr, deDo.DateStr, 
                            ok.Id.ToString(), ok.Status, ok.IsArch(), ok.StawkaNocna, 
                            //RepNadgodziny.nmoKier);
                            RepNadgodziny.nmoKierStawki);
                    }
                    else deDo.Error = "Zbyt długi okres";
                }
                else deDo.Error = "Data wcześniejsza niż początkowa";
            }
        }

        public void ExecuteNadgodzinyProj()
        {
            SetHeaderNadgodzinyProj();
            repNadgodzinyProj._DataBindProj(ddlProjekty.SelectedValue);
        }

        public void ExecuteUrlopy(bool validate)
        {
            if (!String.IsNullOrEmpty(deUrlopDo.DateStr))
            {
                //if (deUrlopDo.Validate())
                if (deUrlopDo.IsValid)
                    if (!String.IsNullOrEmpty(ddlKierUrlopy.SelectedValue))
                    {
                        SetHeaderUrlopy();
                        repUrlopy.Prepare(ddlKierUrlopy.SelectedValue, deUrlopDo.DateStr);
                    }
                    else
                        Tools.ShowMessage("Proszę wybrać kierownika.");
                else
                    Tools.ShowMessage("Niepoprawny format daty.");
            }
            else
                if (validate)
                    Tools.ShowMessage("Proszę wybrać datę końcową okresu zestawienia.");
        }
        //--------------------
        private void Prepare()
        {
            //----- nadgodziny -----
            //cntKierParams.Prepare(user.Id);

            //PlanPracyZmiany.Prepare(user.Id, DateTime.Now);
            //PlanPracyAccept.Prepare(user.Id, DateTime.Now);

            cntSelectOkres.Prepare(DateTime.Today, true);
            DataSet dsK = Base.getDataSet(
                /*
                "select 'ALL' as Id, 'Pokaż wszystkich pracowników' as NI, 0 as Sort union " +
                "select '0' as Id, 'Poziom główny struktury' as NI, 2 as Sort union " +
                "select Nazwisko + ' ' + Imie as NI, convert(varchar,Id) as Id, 1 as Sort from Pracownicy where Kierownik = 1 " + 
                "order by Sort,NI");
                */
                "select -100 as Id, 'Pokaż wszystkich pracowników' as NI, 0 as Sort, null as N1, null as Nazwisko, null as Imie union " +
                "select 0 as Id, 'Poziom główny struktury' as NI, 2 as Sort, null as N1, null as Nazwisko, null as Imie union " +
                //"select distinct P.Id, P.Nazwisko + ' ' + P.Imie + case when P.Kierownik = 0 then ' (stary)' else '' end as NI, 1 as Sort, P.Nazwisko, P.Imie " +
                "select distinct P.Id, case when P.Kierownik = 0 or P.Status < 0 then '" + App.OldMarker + "' else '' end + P.Nazwisko + ' ' + P.Imie as NI, 1 as Sort, " +
                    "case when P.Kierownik = 0 or P.Status < 0 then '*' else 'z' end as N1, " +
                    "P.Nazwisko, P.Imie " +
                "from Pracownicy P " +
                    "left outer join PracownicyOkresy O on O.Id = P.Id and O.Kierownik = 1 " +
                "where P.Kierownik = 1 or O.Id is not null " +
                "order by Sort,N1 desc,Nazwisko,Imie");
            Tools.BindData(ddlKierownicy, dsK, "NI", "Id", true, null);
            Tools.BindData(ddlKierownicy7, dsK, "NI", "Id", true, null);
            Tools.BindData(ddlKierUrlopy, dsK, "NI", "Id", true, null);
            repNadgodziny._Prepare(null, null, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny.nmoKierStawki);
            SetHeaderNadgodziny();

            repSpoznienia.PrepareAdm(dsK, "NI", "Id");
            repRozlNadg.PrepareAdm(dsK, "NI", "Id");
            //-------------------
            cntSelectOkresProj.Prepare(DateTime.Today, true);
            DataSet dsD = Base.getDataSet(
                //"select Nazwa, Id from Dzialy order by Nazwa");
                "select case when Status = -1 then '" + App.OldMarker + "' else '' end + Nazwa as Dzial, Id from Dzialy order by Status desc, Nazwa");
            Tools.BindData(ddlProjekty, dsD, "Dzial", "Id", true, null);

            repNadgodzinyProj._Prepare(null, null, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, cntSelectOkres.OkresId, cntSelectOkres.Status, cntSelectOkres.IsArch, cntSelectOkres.StawkaNocna, RepNadgodziny.nmoProjStawki);
            SetHeaderNadgodzinyProj();

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
            //----- pracownicy -----
            /*
            DataSet dsP = Base.getDataSet(
                //"select Nazwisko + ' ' + Imie as Text, " +
                "select case when Status < 0 then '" + App.OldMarker + "' else '' end + Nazwisko + ' ' + Imie as Text, " +
                       "case when Status < 0 then '*' else 'z' end as N1, " + 
                       "convert(varchar, Id) + '|' + convert(varchar, RcpId) as Value " + 
                "from Pracownicy order by N1 desc, Text");
            */
            DataSet dsP = Base.getDataSet(String.Format(@"
select case when Status <= {0} then '{1}' else '' end + Nazwisko + ' ' + Imie as Text, 
       case when Status <= {0} then 2 else 1 end as N1, 
       convert(varchar, Id) + '|' + convert(varchar, ISNULL(RcpId, -1)) as Value 
from Pracownicy where Status <> {2} order by N1, Text",
                App.stOld, App.OldMarker, App.stPomin));  // bez pomiń
                            
            Tools.BindData(ddlPracownicy, dsP, "Text", "Value", true, null);
            //----- urlopy -----
            deUrlopDo.Date = Tools.eom(DateTime.Today);
            //----- czas pracy rcp - zaakceptowany -----
            repCzasPracy.Prepare(dsK);
            //----- podział na cc ----------------------
            if (!App.User.HasRight(AppUser.rRepPodzialCC)) Tools.RemoveMenu(tabWyniki, "6");
            if (!App.User.HasRight(AppUser.rRepStolowka)) Tools.RemoveMenu(tabWyniki, "7");


            if (!rozlNadg || !App.User.HasRight(AppUser.rRozlNadg)) Tools.RemoveMenu(tabWyniki, "9");

            if (!planUrlopow) Tools.RemoveMenu(tabWyniki, "10"); //- plan urlpoów   // SIEMENS ma wyłączone
            if (!App.User.HasRight(AppUser.rRepESD)) Tools.RemoveMenu(tabWyniki, "11");
            if (!App.User.HasRight(AppUser.rRepRezerwaUrlopowa)) Tools.RemoveMenu(tabWyniki, "12");

#if DBW || VICIM || VC
            Tools.RemoveMenu(tabWyniki, "11");
            Tools.RemoveMenu(tabWyniki, "10");
            Tools.RemoveMenu(tabWyniki, "12");
            Tools.RemoveMenu(tabWyniki, "7");
            Tools.RemoveMenu(tabWyniki, "6");
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

        protected void pgKartaPracy_Activate(object sender, EventArgs e)
        {
            //int x = 0;
        }

        protected void cntSelectOkres_Changed(object sender, EventArgs e)
        {
            SetHeaderNadgodziny();
        }

        protected void cntSelectOkresProj_Changed(object sender, EventArgs e)
        {
            SetHeaderNadgodzinyProj();
        }
        //------------------------    
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

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteNadgodziny();
        }
        //-----
        protected void ddlKierownicy7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteNadgodziny7();
        }

        protected void btExec7_Click(object sender, EventArgs e)
        {
            ExecuteNadgodziny7();
        }

        protected void btExecCC_Click(object sender, EventArgs e)
        {
            //ExecuteNadgodziny7();
        }
        //-----
        protected void ddlProjekty_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteNadgodzinyProj();
        }
        //-----
        protected void ddlKierUrlopy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteUrlopy(false);
        }

        protected void btExecUrlop_Click(object sender, EventArgs e)
        {
            ExecuteUrlopy(true);
        }

        protected void pgPodzialCC_Activate(object sender, EventArgs e)
        {
            //Tools.SetNoCache(false);
        }

        protected void pgPodzialCC_Deactivate(object sender, EventArgs e)
        {
            //Tools.SetNoCache(true);
        }

        protected void vStolowka_Activate(object sender, EventArgs e)
        {
            repStolowka.Prepare();
        }

        protected void vSpoznienia_Activate(object sender, EventArgs e)
        {
            repSpoznienia.Prepare();
        }

        protected void vRozlNadg_Activate(object sender, EventArgs e)
        {
            repRozlNadg.Prepare();
        }

        protected void vESD_Activate(object sender, EventArgs e)
        {
            repESD.Prepare();
        }
        
        protected void vPlanUrlopow_Activate(object sender, EventArgs e)
        {
            repPlanUrlopow._Prepare();
        }

        protected void vRezerwaUrlopowa_Activate(object sender, EventArgs e)
        {
            repRezerwaUrlopowa.Prepare();
        }
    }
}

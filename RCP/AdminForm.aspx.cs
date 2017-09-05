using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class AdminForm : System.Web.UI.Page
    {
        private const string active_tab = "atabA";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        AppUser user;

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
            user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                if (user.IsAdmin)
                {
                    //Session[active_tab] = "7";
#if SIEMENS
                    Button6.Text    = "Import danych z KOMAX'a";
                    Button6.ToolTip = "Import absencji z systemu kadrowego";
                    Button6.Visible = false; // już nie używają
                    
                    Tools.MakeConfirmButton(Button3, "Potwierdź import danych z systemu KP.");              // te są zawsze
                    Tools.MakeConfirmButton(Button6, "Potwierdź import danych z systemu KP.");

                    Button5.Visible = false;
                    //Okres ok = GetOkresToExport();
                    //Button5.Text = String.Format("Eksport do systemu KP ({0})", Base.MonthName(ok.DateTo.Month));
                    //Tools.MakeConfirmButton(Button5, String.Format("Potwierdź eksport danych do systemu KP za okres:\\n{0}.", ok.OdDoStr));
                    //Button5.ToolTip = "Eksport danych czasu pracy do systemu KP";
#elif KDR
                    Button3.Visible = false;
                    Button5.Visible = false;
                    Button6.Visible = false;
                    paPodzialLudzi.Visible = App.User.HasRight(AppUser.rPodzialLudziAdm);
#else
                    Tools.MakeConfirmButton(Button3, "Potwierdź import danych z systemu KP.");              // te są zawsze
                    Tools.MakeConfirmButton(Button6, "Potwierdź import danych z systemu Asseco HR.");

                    Okres ok = GetOkresToExport();
                    Button5.Text = String.Format("Eksport do Asseco HR ({0})", Base.MonthName(ok.DateTo.Month));
                    Tools.MakeConfirmButton(Button5, String.Format(
                            "Potwierdź eksport danych do systemu Asseco za okres:\\n{0}.",
                            ok.OdDoStr));
                    paPodzialLudzi.Visible = App.User.HasRight(AppUser.rPodzialLudziAdm);
#endif                    
                    PrepareConfirmButtons();                                                                // opisy dynamiczne

                    Tools.SelectMenuFromSession(tabAdmin, active_tab);
                    SelectTab();

                    /*
                    string dt = (string)Session["enddate1"];
                    tbDataDo.Text = String.IsNullOrEmpty(dt) ? Base.DateToStr(DateTime.Now) : dt;
                    dt = (string)Session["startdate1"];
                    //tbDataOd.Text = String.IsNullOrEmpty(dt) ? Base.DateToStr(DateTime.Now.AddMonths(-1).AddDays(-5)) : dt;
                    tbDataOd.Text = "2011-12-01";
                    tbDataDo1.Text = tbDataDo.Text;
                    tbDataOd1.Text = tbDataOd.Text;
                    */

                    PrepareKierownicy();
                    //ddlKierownicy.SelectedIndex = 10;
                    //ddlKierownicy2.SelectedIndex = 10;

                    cntSelectOkresStruct._Prepare(null);
                    cntSelectOkres._Prepare(null);
                    cntSelectOkres1._Prepare(null);

                    //PrepareAcc();

                    //Tools.EnableUpload();
                    Page.Form.Attributes.Add("enctype", "multipart/form-data");  // ponieważ mamy FileUpload1 na formatce to musze to dodać bo za pierwszym razem nie widzi wpisanego pliku

                    if (!Lic.wnioskiUrlopowe || !App.User.HasRight(AppUser.rWnioskiUrlopoweAdm)) Tools.RemoveMenu(tabAdmin, "10");
                    if (!Lic.rozlNadg || !App.User.HasRight(AppUser.rRozlNadg)) Tools.RemoveMenu(tabAdmin, "12");
                    if (!Lic.planUrlopow || !App.User.HasRight(AppUser.rPlanUrlopow)) Tools.RemoveMenu(tabAdmin, "13");
                    
                    Tools.RemoveMenu(tabAdmin, "1");

#if SIEMENS || IQOR
                    //cntImportALARMUS1.Visible = true;  20151019 wyłączam
#endif
#if SIEMENS
                    cntImportABSENCJA.Visible = true;
                    cntImportABSENCJA2.Visible = true;
                    cntImportUmowy.Visible = true;
#endif

                    if (tabAdmin.Items.Count > 12)
                    {
                        tabAdmin.Items[0].Text = "Struktura org.";
                    }

                    if (Lic.ppPrint)
                        headPrint.Visible = true;
                }
                else
                    App.ShowNoAccess("Panel Administratora", user);
            }
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            switch (tabAdmin.SelectedValue)
            {
                case "0":
                    /*aa
                    bool b = cntPracToVerify.List.Items.Count > 0;
                    Ustawienia settings = Ustawienia.CreateOrGetSession();
                    lbInfo.Text = b ? "(termin zakończenia weryfikacji: " + settings.CzasWeryfikacji.ToString() + " dni, do: " + settings.TerminWeryfikacji(DateTime.Today) + ")" : "&nbsp;";
                    btStart.Enabled = b;
                     */ 
                    //((MasterPage)Master).SetWideJs(false);
                    //((MasterPage)Master).SetWideJs(true);
                    break;
                case "2":
                    //((MasterPage)Master).SetWideJs(true);
                    break;
                case "1":
                case "3":   // działy
                case "6":
                case "7":
                    //((MasterPage)Master).SetWideJs(true);
                    break;
                default:
                    //((MasterPage)Master).SetWideJs(false);
                    //((MasterPage)Master).SetWideJs(true);
                    break;
            }
            base.OnPreRender(e);
        }
        
        protected void Page_Error(object sender, System.EventArgs e)
        {
            Session[active_tab] = null;
            AppError.Show("Administrator Form");
        }        
        //----------------------------------        
        private void PrepareConfirmButtons()
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok != null)
            {
                Button2.Text = String.Format("Zamknięcie miesiąca ({0})", Base.MonthName(ok.DateTo.Month));
                Tools.MakeConfirmButton(Button2, String.Format(
                    "Potwierdź zamknięcie okresu rozliczeniowego:\\n{0}.",
                    ok.OdDoStr));
            }
            else
            {
                ok = Okres.Current(null);
                Button2.Text = String.Format("Zamknięcie miesiąca ({0})", Base.MonthName(ok.DateTo.Month));
                Tools.MakeInfoButton(Button2, String.Format(
                    "Zamknięcie okresu rozliczeniowego {0} niemożliwe przed datą końca okresu.",
                    ok.OdDoStr));
            }

            Okres ok2 = Okres.LastClosed(null);
            Button4.Enabled = ok2 != null;
            if (ok2 != null)
            {
                Button4.Text = String.Format("Odblokowanie miesiąca ({0})", Base.MonthName(ok2.DateTo.Month));
                Tools.MakeConfirmButton(Button4, String.Format(
                    "Potwierdź odblokowanie okresu rozliczeniowego:\\n{0}.",
                    ok2.OdDoStr));
            }
            else
                Tools.MakeConfirmButton(Button4, null);
            //-------------------
            lbInfo.Text = "Okres rozliczeniowy zamknięty do: " + 
                (ok.LockedTo < ok.DateFrom ? "odblokowany" : Tools.DateToStr(ok.LockedTo));
            /*
            lbInfo.Text += "<br />" + String.Format("{0} - {1}, {2}, {3} - {4}", 
                Tools.DateToStr(ok.DateFrom), 
                Tools.DateToStr(ok.DateTo), 
                Tools.DateToStr(ok.LockedTo), 
                Tools.DateToStr(ok.NextLockTo), 
                Tools.DateToStr(ok.UnlockTo)); /**/
            btWeekClose.Text = String.Format("Zamknięcie tygodnia do: {0}", Tools.DateToStr(ok.NextLockTo));
            btWeekClose.Enabled = ok.NextLockTo < DateTime.Today && ok.NextLockTo < ok.DateTo;
            Tools.MakeConfirmButton(btWeekClose, String.Format(
                "Potwierdź zamknięcie tygodnia do: {0}",
                Tools.DateToStr(ok.NextLockTo)));
            if (ok.LockedTo < ok.DateFrom) 
            {
                //btWeekOpen.Text = String.Format("Odblokowanie tygodnia od: {0}", Tools.DateToStr(ok.DateFrom));
                btWeekOpen.Text = "Odblokowanie tygodnia"; 
                btWeekOpen.Enabled = false;
            }
            else
            {
                //btWeekOpen.Text = String.Format("Odblokowanie tygodnia od: {0}", Tools.DateToStr(ok.UnlockTo));
                btWeekOpen.Text = "Odblokowanie tygodnia";
                btWeekOpen.Enabled = true;
                Tools.MakeConfirmButton(btWeekOpen, String.Format(
                    //"Potwierdź odblokowanie tygodnia od: {0}",
                    "Potwierdź odblokowanie tygodnia.\\nOkres rozliczeniowy zostanie odblokowany od dnia: {0}.",
                    Tools.DateToStr(ok.UnlockTo)));
            }
        }

        public Okres GetOkresToExport()
        {
            //return new Okres(DateTime.Today.AddDays(-15));
            return new Okres(DateTime.Today.AddDays(-10));  // do 10 musza pojsc wypłaty wiec nie ma sensu dłuzej 
        }
        //-----------------------------------
        protected void tabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            /*aa
            const string msg = "Trwa edycja danych pracownika.\\nPrzed zmianą zakładki proszę ją zakończyć.";

            switch (mvPracownicy.ActiveViewIndex)
            {
                case 1:
                    if (cntPracownicy.List.EditIndex != -1)
                    {
                        Tools.SelectMenu(tabAdmin, mvPracownicy.ActiveViewIndex.ToString()); // nie zmieniam taba
                        Tools.ShowMessage(msg);
                        return;
                    }
                    break;
            }
             */
            SelectTab();
        }

        private void SelectTab()
        {
            Session[active_tab] = tabAdmin.SelectedValue;
            mvAdministracja.ActiveViewIndex = Int32.Parse(tabAdmin.SelectedValue);
            switch (mvAdministracja.ActiveViewIndex)
            {
                case 0:
                    Info.SetHelp(Info.HELP_ADMSTRUKTURA);
                    break;
                case 1:
                case 11:
                    Info.SetHelp(Info.HELP_ADMPRACOWNICY);
                    break;
                case 2:
                    Info.SetHelp(Info.HELP_ADMSTREFY);
                    break;
                case 3:
                    Info.SetHelp(Info.HELP_ADMDZIALY);
                    break;
                case 4:
                    Info.SetHelp(Info.HELP_ADMZMIANY);
                    break;
                case 5:
                    Info.SetHelp(Info.HELP_ADMPARAMETRY);
                    break;
                case 6:
                    Info.SetHelp(Info.HELP_ADMPLANPRACY);
                    break;
                case 7:
                    Info.SetHelp(Info.HELP_ADMACCEPTPP);
                    break;
                case 8:
                    Info.SetHelp(Info.HELP_ADMZASTEPSTWA);
                    break;
                case 9:
                    Info.SetHelp(Info.HELP_ADMODDELEGOWANIA);
                    break;
                case 10:
                    Info.SetHelp(Info.HELP_ADMWNIOSKIURLOPOWE);
                    break;
                case 12:
                    Info.SetHelp(Info.HELP_ADMROZLNADG);
                    break;
                case 13:
                    Info.SetHelp(Info.HELP_ADMPLANURLOP);
                    break;
            }
        }

        //-----------------------------------
        /*
        private bool ReloadPlan
        {
            set { ViewState["rplan"] = value; }
            get { return Tools.GetBool(ViewState["rplan"], false); }
        }

        private bool ReloadAcc
        {
            set { ViewState["racc"] = value; }
            get { return Tools.GetBool(ViewState["racc"], false); }
        }
        
        private bool ReloadPrzes
        {
            set { ViewState["rprzes"] = value; }
            get { return Tools.GetBool(ViewState["rprzes"], false); }
        }
        */
        private string SelectPracIdPrzes
        {
            set { ViewState["pidprzes"] = value; }
            get { return Tools.GetStr(ViewState["pidprzes"]); }
        }
        
        private string SelectPracIdPP
        {
            set { ViewState["pidpp"] = value; }
            get { return Tools.GetStr(ViewState["pidpp"]); }
        }

        private string SelectPracIdAcc
        {
            set { ViewState["pidacc"] = value; }
            get { return Tools.GetStr(ViewState["pidacc"]); }
        }

        private string SelectPracIdRozl
        {
            set { ViewState["pidrozl"] = value; }
            get { return Tools.GetStr(ViewState["pidrozl"]); }
        }

        private string SelectPracIdPlanUrlop
        {
            set { ViewState["pidplurlop"] = value; }
            get { return Tools.GetStr(ViewState["pidplurlop"]); }
        }

        private string GetKierId(string pracId)
        {
            return db.getScalar("select IdKierownika from Przypisania where Status = 1 and GETDATE() between Od and ISNULL(Do, '20990909') and IdPracownika = " + pracId);
        }
        //-----------------------------------
        protected void pgStruktura_Activate(object sender, EventArgs e)
        {
            cntStruktura.PrepareIfEmpty(null);
            cntStruktura.InitScript();
        }

        protected void pgPracownicy_Activate(object sender, EventArgs e)
        {

        }

        protected void pgPracownicy_Deactivate(object sender, EventArgs e)
        {

        }

        protected void pgPlanPracy_Activate(object sender, EventArgs e)
        {
            PlanPracyAccept.InitScripts();

            string pracId = SelectPracIdPP;
            if (!String.IsNullOrEmpty(pracId))
            {
                SelectPracIdPP = null;
                string kierId = GetKierId(pracId);
                if (!String.IsNullOrEmpty(kierId))
                {
                    Tools.SelectItem(ddlKierownicy, kierId);
                    ddlKierownicy_SelectedIndexChanged(ddlKierownicy, EventArgs.Empty);
                }
            }
        }

        protected void pgAkceptacjaCzasu_Activate(object sender, EventArgs e)
        {
            PlanPracyAccept.InitScripts();

            string pracId = SelectPracIdAcc;
            if (!String.IsNullOrEmpty(pracId))
            {
                SelectPracIdAcc = null;
                string kierId = GetKierId(pracId);
                if (!String.IsNullOrEmpty(kierId))
                {
                    Tools.SelectItem(ddlKierownicy2, kierId);
                    ddlKierownicy2_SelectedIndexChanged(ddlKierownicy2, EventArgs.Empty);
                }
            }
        }

        protected void pgRozliczenieCzasu_Activate(object sender, EventArgs e)
        {
            PlanPracyRozliczenie._InitScripts();

            string pracId = SelectPracIdRozl;
            if (!String.IsNullOrEmpty(pracId))
            {
                SelectPracIdRozl = null;
                string kierId = GetKierId(pracId);
                if (!String.IsNullOrEmpty(kierId))
                {
                    Tools.SelectItem(ddlKierownicy3, kierId);
                    ddlKierownicy3_SelectedIndexChanged(ddlKierownicy3, EventArgs.Empty);
                }
            }
        }

        protected void pgPlanUrlopow_Activate(object sender, EventArgs e)
        {
            //cntPlanUrlopow.InitScripts();

            string pracId = SelectPracIdPlanUrlop;
            if (!String.IsNullOrEmpty(pracId))
            {
                SelectPracIdPlanUrlop = null;
                string kierId = GetKierId(pracId);
                if (!String.IsNullOrEmpty(kierId))
                {
                    Tools.SelectItem(ddlKierownicy4, kierId);
                    ddlKierownicy4_SelectedIndexChanged(ddlKierownicy4, EventArgs.Empty);
                }
            }
        }

        protected void pgZastepstwa_Activate(object sender, EventArgs e)
        {

        }

        protected void pgPrzesuniecia_Activate(object sender, EventArgs e)
        {
            cntPrzesunieciaAdm.Prepare();

            string pracId = SelectPracIdPrzes;
            if (!String.IsNullOrEmpty(pracId))
            {
                SelectPracIdPrzes = null;
                cntPrzesunieciaAdm.SelectPrac(pracId);
            }
        }

        protected void pgWnioskiUrlopowe_Activate(object sender, EventArgs e)
        {
            paWnioskiUrlopowe.Prepare(true);
        }
        //--------------------------------------
        protected void ProgressTimer_Tick(object sender, EventArgs e)
        {
            ShowProgress(false);
        }
        //------------------------------------
        const string iprocAD = "iprocAD";
        const string iprocKP = "iprocKP";
        const string importInfo = "Trwa import danych.\\nProszę zobaczyć stan importu w podglądzie zdarzeń.";

        private bool ImportInProgress()
        {
            return Session[iprocAD] != null || Session[iprocKP] != null;  // powinno sie sprawdzać Log i czas, ale na razie tak wystarczy
        }

        private void ShowProgress(bool start)
        {
            /*aa
            if (start)
            {
                btImport.Enabled = false;
                btStart.Enabled = false;
                hidError.Value = null;
                cntProgressAD.Visible = true;
                cntProgressAD.Pcent = 0;
                cntProgressKP.Visible = true;
                cntProgressKP.Pcent = 0;
            }
            //----- AD ----- 
            App.ProcessClass ad = (App.ProcessClass)Session[iprocAD];
            bool f = true;      // czy oba ukończone
            if (ad != null)
            {
                lbProgressAD.Text = "Import AD:";
                cntProgressAD.Pcent = ad.Pcent;
                if (ad.Finished)
                {
                    Session[iprocAD] = null;
                    Log.Update(ad.LogId, Log.OK);
                    Log.Info(Log.t2APP_IMPORTAD, "Import AD zakończony", null, Log.OK);
                }
                else f = false;
                if (ad.Error == 0)
                    lbProgressAD.Text += " " + ad.Progress.ToString() + "/" + ad.Max.ToString() + " - " + ad.Pcent.ToString() + "%";
                else
                {
                    lbProgressAD.Text += " Błąd!";
                    hidError.Value = "1";
                }
            }
            //----- KP ----- 
            App.ProcessClass kp = (App.ProcessClass)Session[iprocKP];
            if (kp != null)
            {
                lbProgressKP.Text = "Import KP:";
                cntProgressKP.Pcent = kp.Pcent;
                if (kp.Finished) 
                {
                    Session[iprocKP] = null;
                    int cnt = kp.Progress;
                    Log.Update(kp.LogId, Log.OK);
                    Log.Info(Log.t2APP_IMPORTKP, "Import KP zakończony", null 
                                //"Ilość dodanych pracowników: " + cnt.ToString() 
                     ,Log.OK); // ilosc - zawsze jest całość
                }
                else f = false;
                if (kp.Error == 0)
                    lbProgressKP.Text += " " + kp.Progress.ToString() + "/" + kp.Max.ToString() + " - " + kp.Pcent.ToString() + "%";
                else 
                {
                    lbProgressKP.Text += " Błąd!";
                    hidError.Value = "1";
                }
            }
            //----- AD,KP ----
            if (f)  // oba ukończone
            {
                ProgressTimer.Enabled = false;
                btImport.Enabled = true;
                btStart.Enabled = true;

                cntPracToVerify.DataBind();
                cntPracToVerify.PrepareListView(true);
                cntPracownicy.List.DataBind();
                cntPracownicy.PrepareListView(true);

                if (String.IsNullOrEmpty(hidError.Value)) 
                {
                    Tools.ShowMessage("Import danych zakończony poprawnie.");
                    //lbProgressAD.Text = null;  niech zostanie
                    //lbProgressKP.Text = null;
                    //cntProgressAD.Visible = false;
                    //cntProgressKP.Visible = false; 
                }
                else 
                    Tools.ShowMessage("Wystąpił błąd podczas importu danych.\\nSzczegóły są dostępne podglądzie zdarzeń.");
            }
            */
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            /*
            //int cnt = App.ImportData_test();
            string cnt = App.ImportData_test();
            Label1.Text = cnt;
            */
            if (ImportInProgress()) Tools.ShowMessage(importInfo);
            else
            {
                int id1 = Log.Info(Log.APP_IMPORTAD, "Import AD", "", Log.PENDING);   // zmianna do statusu postępu AD
                Session[iprocAD] = App.ExecProcess(id1, 1);  
                int id2 = Log.Info(Log.t2APP_IMPORTKP, "Import KP", "", Log.PENDING);
                Session[iprocKP] = App.ExecProcess(id2, 2);
                ShowProgress(true);
                ProgressTimer.Enabled = true;
            }
        }

        /*
        protected void btImport_Click(object sender, EventArgs e)
        {
            int cnt = App.ImportData();
            cntPracToVerify.DataBind();
            if (cnt > 0)
            {
                cntPracownicy.List.DataBind();
                Tools.ShowMessage("Import danych zakończony poprawnie.\\nIlość nowych pracowników: " + cnt.ToString() + ".");
            }
            else Tools.ShowMessage("Wystąpił błąd podczas importu danych.");
        }
         */

        protected void btStart_Click(object sender, EventArgs e)
        {
            /*aa
            if (ImportInProgress()) Tools.ShowMessage(importInfo);
            else
            {
                bool ok = App.StartVerify();
                cntPracToVerify.DataBind();
                if (ok)
                {
                    cntPracownicy.List.DataBind();
                    Tools.ShowMessage("Uruchomienie weryfikacji przebiegło poprawnie.");
                }
                else Tools.ShowMessage("Wystąpił błąd podczas uruchamiania weryfikacji.");
            }
             */
        }

        protected void cntImportALARMUS1_ImportFinished(object sender, EventArgs e)
        {
            if (cntImportALARMUS1.UpdateReaders)
            {
                StrefyControl1.DataBind();
            }
        }

        protected void cntImportABSENCJA_ImportFinished(object sender, EventArgs e)
        {
        }

        //----- struktura ---------------------------------------
        protected void ShowCzas1()
        {
            string pid = cntStruktura.SelectedPracId;
            string id = cntStruktura.SelectedRcpId;
            //if (String.IsNullOrEmpty(id)) id = "-1";
            //cntPracInfo.Prepare(pid);
            if (!String.IsNullOrEmpty(pid))
            {
                lbPracName.Text = cntStruktura.SelectedNI;
                string strefaId, algId, algPar;
                Worktime.GetStrefaRCP2(pid, cntSelectOkres.DateTo, out strefaId, out algId, out algPar);
                cntRCPStruct.Prepare(pid, id, cntSelectOkres.DateFrom, cntSelectOkres.DateTo, strefaId);
            }
        }

        protected void cntStruktura_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            ShowCzas1();
        }

        protected void btRefresh1_Click(object sender, EventArgs e)
        {
            ShowCzas1();
        }

        protected void cntPracownicy2_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "przes":
                    Tools.SelectMenu(tabAdmin, "9");
                    SelectTab();
                    UpdatePanel1.Update();
                    break;
                case "plan":
                    Tools.SelectMenu(tabAdmin, "6");
                    SelectTab();
                    UpdatePanel1.Update();
                    break;
                case "ppacc":
                    Tools.SelectMenu(tabAdmin, "7");
                    SelectTab();
                    UpdatePanel1.Update();
                    break;
            }
        }

        protected void cntPracownicy2_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            string pracId = cntPracownicy2.SelectedPracId;
            SelectPracIdPrzes = pracId;
            SelectPracIdPP = pracId;
            SelectPracIdAcc = pracId;
            SelectPracIdRozl = pracId;
            SelectPracIdPlanUrlop = pracId;
        }

        //----- lista pracowników -------------------------------
        protected void ShowCzas()
        {
            string pid = cntPracownicy.SelectedPracId;
            string id = cntPracownicy.SelectedRcpId;
            //if (String.IsNullOrEmpty(id)) id = "-1";
            cntRCPPrac1.Prepare(pid, id, cntSelectOkres1.DateFrom, cntSelectOkres1.DateTo, cntPracownicy.SelectedStrefaId);
            cntRCPPrac2.Prepare(pid, id, cntSelectOkres1.DateFrom, cntSelectOkres1.DateTo, cntPracownicy.SelectedStrefaId);
        }

        protected void cntPracownicy_SelectedChanged(object sender, EventArgs e)
        {
            ShowCzas();
        }

        protected void btRefresh_Click(object sender, EventArgs e)
        {
            ShowCzas();
        }
        //--------------------------------------------
        private void PrepareKierownicy()
        {
            DataSet ds = Base.getDataSet(
                /*
                "select 'Poziom główny struktury' as NI, 0 as Id, 1 as Sort union " +
                "select Nazwisko + ' ' + Imie as NI, Id, 0 as Sort from Pracownicy where Kierownik = 1 " +
                "order by Sort, NI");
                */
                "select 0 as Id, 'Poziom główny struktury' as NI, 1 as Sort, null as N1, null as Nazwisko, null as Imie union " +
                //"select distinct P.Id, P.Nazwisko + ' ' + P.Imie + case when P.Kierownik = 0 then ' (stary)' else '' end as NI, 0 as Sort " +
                "select distinct P.Id, case when P.Kierownik = 0 or P.Status < 0 then '" + App.OldMarker + "' else '' end + P.Nazwisko + ' ' + P.Imie as NI, 0 as Sort, " +
                    "case when P.Kierownik = 0 or P.Status < 0 then '*' else 'z' end as N1, " +
                    "P.Nazwisko, P.Imie " +                
                "from Pracownicy P " +
                    "left outer join PracownicyOkresy O on O.Id = P.Id and O.Kierownik = 1 " +
                    "where P.Kierownik = 1 or O.Id is not null " +
                "order by Sort, N1 desc, Nazwisko, Imie");

            string kid = (string)Session["admKier"];
            Tools.BindData(ddlKierownicy, ds, "NI", "Id", true, kid);
            Tools.BindData(ddlKierownicy2, ds, "NI", "Id", true, kid);

            PlanPracyZmiany.Prepare(kid, DateTime.Today, true);
            PlanPracyAccept.Prepare(kid, DateTime.Today, true);

            if (Lic.rozlNadg && App.User.HasRight(AppUser.rRozlNadg))
            {
                Tools.BindData(ddlKierownicy3, ds, "NI", "Id", true, kid);
                PlanPracyRozliczenie.Prepare(kid, DateTime.Today, true);
            }

            if (Lic.planUrlopow && App.User.HasRight(AppUser.rPlanUrlopow))
            {
                Tools.BindData(ddlKierownicy4, ds, "NI", "Id", true, kid);
                //ddlKierownicy4.Items.Add(new ListItem("Wszyscy pracownicy", "-1"));  // trzba by paginator włączyć bo za wolne
                cntPlanUrlopow.Prepare(kid, DateTime.Today, true);
            }
        }

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlanPracyZmiany.Prepare(ddlKierownicy.SelectedValue);
            Session["admKier"] = ddlKierownicy.SelectedValue;
        }

        protected void ddlKierownicy2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlanPracyAccept.Prepare(ddlKierownicy2.SelectedValue);
            Session["admKier"] = ddlKierownicy2.SelectedValue;
        }

        protected void ddlKierownicy3_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlanPracyRozliczenie.Prepare(ddlKierownicy3.SelectedValue);
            Session["admKier"] = ddlKierownicy3.SelectedValue;
        }

        protected void ddlKierownicy4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cntPlanUrlopow.Prepare(ddlKierownicy4.SelectedValue);
            Session["admKier"] = ddlKierownicy4.SelectedValue;
        }
        //------------------------------------------------

        //------------------------------------------------
        protected void ImportFinished(object sender, EventArgs e)
        {
            // err ?
            cntStruktura.PrepareOkres(null);
            //cntStruktura.Prepare(null);
            cntPracownicy.List.DataBind();
        }
        //----- KP -------------------------------------------
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Import danych z systemu KP";
                    int lid = Log.Info(Log.t2APP_IMPORTKP, info, null, Log.PENDING);

                    OdbcConnection conKP = Base.odbcConnect(KP.conStrKP);
                    SqlConnection con = Base.Connect();

                    int c1 = KP.ImportABSENCJA(conKP, con);
                    int c2 = KP.ImportKODYABS(conKP, con);
                    int c3 = KP.ImportKALENDAR(conKP, con);
                    int c4 = KP.ImportCZASNOM(conKP, con);
                    int c5 = KP.ImportSTAWKI(conKP, con);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    int c6 = KP.ImportZBIOR(conKP, con);

                    Base.Disconnect(con);
                    Base.Disconnect(conKP);

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.t2APP_IMPORTKP, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.t2APP_IMPORTKP, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5}", c1, c2, c3, c4, c5, c6), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już import danych z systemu KP.\\n\\n" + user.Imie + ", poczekaj na jego zakończenie.");
        }

        //----- ASSECO ------------------------------------------
        protected void Button6_Click(object sender, EventArgs e)
        {
#if SIEMENS
            Komax.ImportAll();
#else
            Asseco.ImportAll();
#endif
        }

        /*
        protected void Button6_Click(object sender, EventArgs e)
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Import danych z systemu Asseco HR";
                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    SqlConnection conAsseco = db.Connect(Asseco.ASSECO);

                    int c1 = Asseco.ImportABSENCJA(conAsseco);
                    int c2 = Asseco.ImportKODYABS(conAsseco);
                    int c3 = Asseco.ImportKALENDAR(conAsseco);
                    int c4 = Asseco.ImportCZASNOM(conAsseco);
                    int c5 = Asseco.ImportSTAWKI(conAsseco);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    int c6 = Asseco.ImportZBIOR(conAsseco);

                    db.Disconnect(conAsseco);

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5}", c1, c2, c3, c4, c5, c6), Log.OK);
                        //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już import danych.\\n\\n" + user.Imie + ", poczekaj na jego zakończenie.");
        }
        
         */
        //-----------------------
        protected void Button5_Click(object sender, EventArgs e)
        {
            //Tools.Delay(5000);   testy
            Okres ok = GetOkresToExport(); 
            if (ok != null)
                if (Asseco.ExportRCP(ok, false, true, true, true, true))
                    Tools.ShowMessage("Dane z okresu rozliczeniowego {0} zostały wyeksportowane poprawnie.", ok.OdDoStr);
                else
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            else Tools.ShowMessage("Brak zamkniętego okresu rozliczeniowego.");
            /**/
        }

        private string GetKierNotClosedSql()
        {
#if IQOR
            return dsGetKierNotClosed.SelectCommand;
#else
            return dsGetKierNotClosedOld.SelectCommand;
#endif
        }

        protected void Button2_Click(object sender, EventArgs e)    // zamykanie miesiąca
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok != null)
            {
                if (ok.CheckClose(ok.DateTo, GetKierNotClosedSql()))
                    if (ok._Close())
                    {
                        Tools.ShowMessage("Okres rozliczeniowy {0} został zamknięty.", ok.OdDoStr);
                        PrepareConfirmButtons();
                    }
                    else
                        Tools.ShowMessage("Wystąpił błąd podczas zamykania okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            }
            else
            {
                ok = Okres.Current(null);
                Tools.ShowMessage("Zamknięcie okresu rozliczeniowego {0} niemożliwe przed datą końca okresu.", ok.OdDoStr);
            }
        }

        protected void Button4_Click(object sender, EventArgs e)    // odblokowywanie miesiąca
        {
            Okres ok = Okres.LastClosed(null);
            if (ok != null)
                if (ok._Reopen())
                {
                    Tools.ShowMessage("Okres rozliczeniowy {0} został odblokowany.", ok.OdDoStr);
                    PrepareConfirmButtons();
                }
                else
                    Tools.ShowMessage("Wystąpił błąd podczas odblokowywania okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
            else Tools.ShowMessage("Brak zamkniętego okresu rozliczeniowego.");
        }
        //--------------------
        protected void btWeekClose_Click(object sender, EventArgs e)    // zamknięcie/zablokowanie tygodnia
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok == null) ok = Okres.Current(null);
            DateTime dt = ok.NextLockTo;

            if (ok.CheckClose(dt, GetKierNotClosedSql()))
                if (ok.SetLockedTo(dt))
                {
                    if (Asseco.ExportRCP(ok, true, true, true, true, false))  // bez struktury
                    {
                        Tools.ShowMessage("Okres rozliczeniowy {0} został zamknięty do dnia: {1}.", ok.OdDoStr, Tools.DateToStr(ok.LockedTo));
                        PrepareConfirmButtons();
                    }
                    else Tools.ShowMessage("Wystąpił błąd podczas eksportu danych z okresu rozliczeniowego:\\n{0}.", ok.OdDoStr);
                }
                else Tools.ShowMessage("Wystąpił błąd podczas zamykania tygodnia:\\n{0} do dnia: {1}.", ok.OdDoStr, Tools.DateToStr(dt));
        }

        protected void btWeekOpen_Click(object sender, EventArgs e)     // odblokowywanie tygodnia
        {
            Okres ok = Okres.FirstToClose(null);
            if (ok == null) ok = Okres.Current(null);
            DateTime dt = ok.UnlockTo;
            if (ok.SetLockedTo(dt.AddDays(-1)))
            {
                Tools.ShowMessage("Okres rozliczeniowy {0} został odblokowany od dnia: {1}.", ok.OdDoStr, Tools.DateToStr(ok.LockedTo.AddDays(1)));
                PrepareConfirmButtons();
            }
            else
                Tools.ShowMessage("Wystąpił błąd podczas odblokowywania tygodnia:\\n{0} od dnia: {1}.", ok.OdDoStr, Tools.DateToStr(dt));
        }
        //-----------------------------------------

        protected void btAbsDl_Click(object sender, EventArgs e)     // odblokowywanie tygodnia
        {
            AbsencjeDlugotrwale.Show();
        }

        //-----------------------
        protected void btPodzial_Click(object sender, EventArgs e)
        {
            App.Redirect(App.PodzialLudziAdmForm);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class PortalAdmin : System.Web.UI.Page
    {
        private const string active_tab = "atabAdmPortal";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
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
                if (user.IsAdmin)
                {
                    Tools.SetNoCache();
                    Tools.EnableUpload();
                    
                    Tools.RemoveMenu(tabAdmin, "pgPliki");
                    Tools.RemoveMenu(tabAdmin, "pgArtykuly");
                    Tools.RemoveMenu(tabAdmin, "pgGazetka");

                    //  20161123 DP
#if IQOR
                    Tools.RemoveMenu(tabAdmin, "pgPracownicy");
#endif
                    if (!Lic.wnZmianaDanych)
                        Tools.RemoveMenu(tabAdmin, "pgWnioskiZmianaDanych");

                    Tools.SelectMenuFromSession(tabAdmin, active_tab);
                    SelectTab();

#if PORTAL
                    Tools.RemoveMenu(tabAdmin, "pgTeksty");

#endif

                    Tools.ExecOnStart2("resize2", "resize();");
                }
                else
                    App.ShowNoAccess("Portal - Administracja", user);
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
            AppError.Show("Portal Administracja Form");
        }
        //----------------------------------        
        private void PrepareConfirmButtons()
        {
        }

        public Okres GetOkresToExport()
        {
            return new Okres(DateTime.Today.AddDays(-15));
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
            Tools.SelectTab(tabAdmin, mvAdministracja, active_tab, false);
            
            /*
            mvAdministracja.ActiveViewIndex = Int32.Parse(tabAdmin.SelectedValue);
            switch (mvAdministracja.ActiveViewIndex)
            {
                case 0:
                    Info.SetHelp(Info.HELP_ADMSTRUKTURA);
                    break;
                case 1:
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
            }
             */
        }

        //-----------------------------------
        protected void pgStruktura_Activate(object sender, EventArgs e)
        {
        }

        protected void pgPracownicy_Activate(object sender, EventArgs e)
        {

        }

        protected void pgPracownicy_Deactivate(object sender, EventArgs e)
        {

        }

        protected void pgAkceptacjaCzasu_Activate(object sender, EventArgs e)
        {
        }

        protected void pgZastepstwa_Activate(object sender, EventArgs e)
        {

        }

        protected void pgPrzesuniecia_Activate(object sender, EventArgs e)
        {
        }

        protected void pgWnioskiUrlopowe_Activate(object sender, EventArgs e)
        {

        }

        protected void pgPliki_Activate(object sender, EventArgs e)
        {

        }

        protected void pgArtykuly_Activate(object sender, EventArgs e)
        {

        }

        protected void pgGazetka_Activate(object sender, EventArgs e)
        {

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
        //----- struktura ---------------------------------------
        protected void ShowCzas1()
        {
        }

        protected void cntStruktura_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            ShowCzas1();
        }

        protected void btRefresh1_Click(object sender, EventArgs e)
        {
            ShowCzas1();
        }
        //----- lista pracowników -------------------------------
        protected void ShowCzas()
        {
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
        }

        protected void ddlKierownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlKierownicy2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //------------------------------------------------

        //------------------------------------------------
        protected void ImportFinished(object sender, EventArgs e)
        {
        }
        //----- KP -------------------------------------------
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        protected void Button3_Click(object sender, EventArgs e)
        {
        }

        //----- ASSECO ------------------------------------------
        protected void Button6_Click(object sender, EventArgs e)
        {
        }

        //-----------------------
        protected void Button5_Click(object sender, EventArgs e)
        {
        }

        protected void Button2_Click(object sender, EventArgs e)    // zamykanie miesiąca
        {
        }

        protected void Button4_Click(object sender, EventArgs e)    // odblokowywanie miesiąca
        {
        }
        //--------------------
        protected void btWeekClose_Click(object sender, EventArgs e)    // zamknięcie/zablokowanie tygodnia
        {
        }

        protected void btWeekOpen_Click(object sender, EventArgs e)     // odblokowywanie tygodnia
        {
        }
        //--------------------------------------------
        protected void btExportTeksty_Click(object sender, EventArgs e)
        {
            //ExportExcel(DateTime.Today.ToString("yyyymmdd") + "_prp_teksty");
            Report.ExportCSV(DateTime.Today.ToString("yyyyMMdd") + "_rcp_teksty", "select * from Teksty", null, null);
        }

        protected void btImportTeksty_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = FileUpload1.FileName;
                string savePath = Server.MapPath(@"uploads\") + fileName;  //@ oznacza nie interpretuj - pozwala na umieszczenie \ na końcu
                FileUpload1.SaveAs(savePath);
                AdmInfoControl.ImportData(savePath);
                AdmInfoControl.List.DataBind();
                Tools.ShowMessage("Import zakończony.");
            }
            else
            {
                Tools.ShowMessage("Brak pliku do importu.");  // załatwia to javascript, ale na wszelki wypadek, script dlatego ze postbacktrigger musi być na buttonie i msg pojawiał sie po przeładowaniu strony przed jej wyswietleniem
            }
        }
        //--------------------------------------------
        protected void cntPracownicy2_Command(object sender, CommandEventArgs e)
        {
            /*
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
             */ 
        }

        protected void cntPracownicy2_SelectedChanged(object sender, EventArgs e)    // selectOkres
        {
            /*
            string pracId = cntPracownicy2.SelectedPracId;
            SelectPracIdPrzes = pracId;
            SelectPracIdPP = pracId;
            SelectPracIdAcc = pracId;
            SelectPracIdRozl = pracId;
            SelectPracIdPlanUrlop = pracId;
             */ 
        }


    }
}

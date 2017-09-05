using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Adm
{
    public partial class cntPracownicy4 : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        public event CommandEventHandler Command;

        const int maxRights = 60;   // 1..21 kolumnmy, checkboxy, linkbuttony, 0-mailing w osobnej kolumnie

        bool canSetRights = false;
        bool ro = false;
#if SCARDS
        bool scAdm          = false;
        bool scHR           = false;
        bool scKwoty        = false;
        bool scControlling  = false;
        bool scZoom         = false;
#endif



        //        const int rightsCount = 30
        ///* SPX */
        //#if SPX
        //            * 0 + 32
        //#endif
        ///* PORTAL */
        //#if PORTAL
        //            * 0 + 38
        //#endif
        ///* MATRYCA SZKOLEŃ */
        //#if MS
        //            * 0 + 33
        //#endif
        ///* RCP */
        //#if IQOR || KDR || DBW
        // + 27
        //#endif
        //#if SIEMENS
        //            + 14
        //    #if ESD
        //            + 1
        //    #endif
        //#endif
        //            ;

        //#if SIEMENS || MS
        //        const int rightsCount = 30 /* to nie był mój pomysł - juan */
        //    #if MS
        //                * 0 + 33 /* to nie był mój pomysł - irson */
        //    #endif
        //    #if ESD 
        //            + 1        
        //    #endif
        //; // WnDstr + 1
        //#elif SPX
        //        const int rightsCount = 32;     // PORTAL
        //#else
        //    #if PORTAL
        //            const int rightsCount = 38;
        //    #else
        //            const int rightsCount = 57;
        //    #endif
        //#endif

        const int rightsCount = 0

            /**********
             * COMMON *
             **********/
            + 9

            /*******
             * RCP *
             *******/
#if RCP
            + 35 + 2
#if IQOR
            + 13
#endif
#if SIEMENS
            - 27
            + 13 //scards
#endif
#if KDR
            + 13
#endif
#if DBW
            + 1
            - 4 - 1 - 8 - 14
#endif
#endif

            /**********
             * PORTAL *
             **********/
#if PORTAL
            + 23
#if IQOR
            + 6
#endif
#if KDR && !IQOR
            + 6 
#endif
            + 0
#endif

            /**********
             * SCARDS *
             **********/
#if SCARDS
            + 13
#if SIEMENS
            - 13 //uprawnienia od scorecardow sa w RCP-SIEMENS
#endif
#endif

            /******
             * MS *
             ******/
#if MS
            + 17
#endif
            ;

        public static object[,] rights = new object[rightsCount, 2] {                     
            /* COMMON */
          // {AppUser.rAdmin,               "A - Administracja"},                                      // rAdmin                 = 0; // administrator
             {AppUser.rRights,              "U - Nadawanie uprawnień"}                                 // rRights                = 1; // prawo do nadawania uprawnień        
            ,{AppUser.rTester,              "T - Testowanie nowych funkcjonalności"}                   // _rTester               = 2; //<<<<<<<< testy cc
            /* RCP */
#if ((RCP || PORTAL) && (!SIEMENS && !DBW))
            ,{AppUser.rKwitekAdm,           "AK - Administracja kwitkiem płacowym"}                      // rKwitekAdm             = 3; //<<<<<<<< podglad pracownika z listy
            ,{AppUser.rPortalAdmin,         "PA - Administracja portalem"}                               // 
            ,{AppUser.rPortalArticles,      "PB - Administracja portalem - artykuły i pliki"}            // artykuły, gazetka, pliki
            ,{AppUser.rWnioskiUrlopoweAdm,  "WX - Administracja wniosków urlopowych"}
#endif
            /* COMMON */
            ,{AppUser.rPrzesunieciaAdm,     "PM - Administracja ustawieniami przesunięć"}
            /* RCP */
#if ((RCP || PORTAL) && (!SIEMENS && !DBW))
            ,{AppUser.rBadaniaMailing,      "MB - Mailing wszystkich kończących się badań"}
#endif
            /* RCP */
#if RCP
            ,{AppUser.rPlanPracy,           "HP - Planowanie czasu pracy"}
            ,{AppUser.rPlanPracyAcc,        "HA - Akceptacja planu pracy"}
#endif

#if (RCP || PORTAL)
            ,{AppUser.rDostepniKierownicy,  "PO - Dostęp do obcych pracowników"}                       // rDostepniKierownicy    = 5; // mozliwosc wyboru kierownika i jego pracowników
            ,{AppUser.rPlanPracySwoj,       "S1 - Dostęp do swojego czasu pracy"}                      // 
            ,{AppUser.rUnlockPP,            "OD - Odblokowywanie zaakceptowanych dni"}                 // 20
            ,{AppUser.rKierParams,          "KP - Dostęp do konfiguracji czasów przerw"}
          //,{AppUser.rKartyTmp,            "KT - Wydawanie kart tymczasowych"}
          /* RCP IQOR */
    #if (IQOR || KDR)
            ,{AppUser.rRepPodzialCC,        "CC - Raporty podziału czasu na cc"}                         // rRepPodzialCC          = 6; // raport poczuału czasu na cc, uprawnienia do cc i class
            ,{AppUser.rRepPodzialCCKwoty,   "CK - Raporty podziału czasu na cc - kwoty"}                 // rRepPodzialCCKwoty     = 7; // raport poczuału czasu na cc z kwotami, uprawnienia do cc i class
            ,{AppUser.rRepPodzialCCAll,     "CA - Raporty podziału czasu na cc - cała klasyfikacja"}     // rRepPodzialCCAll       = 8; // pełen wgląd w dane, bez konieczności przypisywania cc i class - dla adminiów
    #endif
            /* RCP */
            ,{AppUser.rRepCzasPracy,        "R1 - Raport czasu pracy RCP"}                             // rRepCzasPracy          = 4; // raport czasu pracy do testów dla Adama Peplinskiego, tymczas poźniej zmienić 
            /* RCP IQOR */
    #if (IQOR || KDR)
            ,{AppUser.rRepStolowka,         "R2 - Raport stołówka"}                                      // 
            ,{AppUser.rRepESD,              "R3 - Raport zaliczenia testów ESD"}                         // 
            ,{AppUser.rRepRezerwaUrlopowa,  "R4 - Raport rezerwy urlopowej"}                             // 
    #endif
#endif
            /* COMMON */
            ,{AppUser.rRaporty2,            "R2 - Raporty dodatkowe"}                                  // na tymczas - zmienić to trzeba będzie docelowo na matryce dostepu do raportów 

            ,{AppUser.rPrzesuniecia,        "PP - Przesuwanie pracowników"}
            ,{AppUser.rPrzesunieciaAcc,     "PA - Akceptowanie przesunięć pracowników"}
            ,{AppUser.rPrzesunieciaAccSub,  "PS - Akceptowanie z całej podstruktury"}
#if (RCP || PORTAL)
            ,{AppUser.rRozlNadg,            "RN - Rozliczanie nadgodzin"}                              // 
            ,{AppUser.rRozlNadgPo,          "RA - Rozliczanie nadgodzin po zamknięciu okresu"}         // 
#endif
            /* SIEMENS */
#if (SCARDS || ((RCP || PORTAL) && SIEMENS))
            ,{AppUser._rScorecardsAdmin,     "SA - Scorecards - Administracja"}                        // 
            ,{AppUser.rScorecardsTLProd,     "TL - Scorecards - Team Leader produktywny"}              // 
            ,{AppUser.rScorecardsTLNieprod,  "TN - Scorecards - Team Leader nieproduktywny"}           // 
            ,{AppUser.rScorecardsKier,       "KI - Scorecards - Kierownik"}                            // 
            ,{AppUser.rScorecardsZarz,       "ZA - Scorecards - Zarząd"}                               // 
            ,{AppUser.rScorecardsWnAcc,      "SW - Scorecards - Akceptacja wniosków premiowych"}       // 
            ,{AppUser.rScorecardsHR,         "SH - Scorecards - HR"}                                   //      
            ,{AppUser.rScorecardsControlling,"SC - Scorecards - Controlling"}                          //      
            ,{AppUser._rScorecardsKwoty,     "SK - Scorecards - Raporty - podgląd kwot"}
          //,{AppUser.rScorecardsZoom,       "SZ - Scorecards - Raporty - szczegóły"}
            ,{AppUser.rScorecardsWnRej,      "SD - Scorecards - Deakceptacja arkuszy"}
            ,{AppUser.rScorecardsWnDstr,     "SO - Scorecards - Odrzucanie wniosków"}
            ,{AppUser.rScorecardsWysPowAdm,  "SQ - Scorecards - Otrzymywanie powiadomień administracyjnych"}
            ,{AppUser.rScorecardsArkAll,     "S7 - Scorecards - Dostęp do wszystkich arkuszy"}
#endif
#if ESD
            ,{AppUser.rESD,                  "ES - ESD"}
#endif
            /* MS */
#if (MS)
            ,{AppUser.rMSAdmin,             "M0 - Matryca Szkoleń - Administracja"}
            ,{AppUser.rMSMeister,           "M1 - Matryca Szkoleń - Mistrz"}
            ,{AppUser.rMSTrener,            "M2 - Matryca Szkoleń - Trener"}
            ,{AppUser.rMSBHP,               "M3 - Matryca Szkoleń - BHP"}
            ,{AppUser.rMSHR,                "M4 - Matryca Szkoleń - HR"}
            ,{AppUser.rMSKorekty,           "M5 - Matryca Szkoleń - Korekta ocen"}
            ,{AppUser.rMSKorektyAcc,        "M6 - Matryca Szkoleń - Akceptacja korekt"}
            ,{AppUser.rMSCertyfikatyAcc,    "M7 - Matryca Szkoleń - Akceptacja certyfikatów"}
            ,{AppUser.rMSAnkietyPodgladP,   "M8 - Matryca Szkoleń - Podgląd ankiet (Pracownicy)"}
            ,{AppUser.rMSAnkietyPodgladK,   "M9 - Matryca Szkoleń - Podgląd ankiet (Przełożeni)"}
            ,{AppUser.rMSAnkietyEdycjaP,    "MA - Matryca Szkoleń - Edycja ankiet (Pracownicy)"}
            ,{AppUser.rMSAnkietyEdycjaK,    "MB - Matryca Szkoleń - Edycja ankiet (Przełożeni)"}
            ,{AppUser.rMSAnkietyAcc,        "MC - Matryca Szkoleń - Akceptacja ankiet"}
            ,{AppUser.rMSPracownicyAdm,     "MD - Matryca Szkoleń - Pracownicy administracji"}
            ,{AppUser.rMSPracownicyProd,    "ME - Matryca Szkoleń - Pracownicy produkcji"}
            ,{AppUser.rMSWozki,             "MF - Matryca Szkoleń - Aktywna jazda wózkiem widłowym"}
            ,{AppUser.rMSBadaniaPodglad,    "MG - Matryca Szkoleń - Podgląd badań"}
#endif
            /* RCP */
#if ((RCP || PORTAL) && (!SIEMENS && !DBW))
            ,{AppUser.rPlanUrlopow,         "PU - Plan urlopów"}                                       // 
            ,{AppUser.rPlanUrlopowSwoj,     "PS - Edycja swojego planu urlopów"}                       //
            ,{AppUser.rPlanUrlopowEditPo,   "PE - Odblokuj edycję po zamknięciu"}                      // 1 miesiac na planowanie, potem korekty, plan z poprzedniego roku jest zablokowany do edycji, chyba ze nadam to uprawnienie
            ,{AppUser.rAbsencjeDlugotrwale, "PD - Absencje długotrwałe"}                               // wprowadzanie

            ,{AppUser.rWnioskiUrlopowe,     "WU - Wnioski urlopowe"}
            ,{AppUser.rWnioskiUrlopoweAcc,  "WA - Akceptowanie wniosków urlopowych"}
            ,{AppUser.rWnioskiUrlopoweSub,  "WS - Dostęp do wniosków z podstuktury"}
            ,{AppUser.rWnioskiUrlopoweNoAccMail, "WM - Mailing wniosków urlopowych"}                   // przy wyłączonym mailingu głównym 
#endif
#if (RCP || PORTAL)
            ,{AppUser.rPortalTmp,           "PO - Dostęp do portalu"}                                  //
#endif
            /* PORTAL */
#if (PORTAL)
            ,{AppUser.rPortalAdmin,         "PA - Portal - administracja"}                              //
            ,{AppUser.rPortalArticles,      "PN - Portal - artykuły"}                                   //
#endif
            /* RCP IQOR */
#if ((RCP) && (!SIEMENS && !DBW))
            ,{AppUser.rPodzialLudzi,            "PL - Podział Ludzi"}                                       //
            ,{AppUser.rPodzialLudziPM,          "PM - Podział Ludzi - Raporty PM"}                          //
            ,{AppUser.rPodzialLudziPMZoom,      "PZ - Podział Ludzi - Raporty PM - szczegóły"}              //
            ,{AppUser.rRaportIlDL,              "PD - Podział Ludzi - Raport ilości DL na CC"}              //
            ,{AppUser.rRaportCCLim,             "PI - Podział Ludzi - Raport przekroczeń limitów na CC"}    //
            ,{AppUser.rRaportCCLimMailing,      "PM - Podział Ludzi - Mailing przekroczeń limitów na CC"}   //            
            ,{AppUser.rPLMailingPrzesCC,        "PC - Podział Ludzi - Mailing przesunięć na CC"}            //            
            ,{AppUser.rEditCCLim,               "PE - Podział Ludzi - Edycja limitów nadgodzin na CC"}      //

            ,{AppUser.rPodzialLudziEditS,       "PS - Podział Ludzi - Edycja splitów"}                      //
            ,{AppUser.rPodzialLudziEditSGrupy,  "PG - Podział Ludzi - Edycja splitów dla grup"}             //
            ,{AppUser.rPodzialLudziEditCAP,     "PC - Podział Ludzi - Edycja CC,Comm,Area,Pos"}             //
            ,{AppUser.rHideSalary,              "PH - Podział Ludzi - Ukryj kwotę wynagrodzenia"}           //
            ,{AppUser.rShowHiddenSalary,        "PV - Podział Ludzi - Pokaż ukryte kwoty wynagrodzenia"}    //
            ,{AppUser.rPodzialLudziAdm,         "PA - Podział Ludzi - Administracja"}                       //
#endif
            /* RCP */
#if ((RCP) && (IQOR || KDR))
            ,{AppUser.rBadaniaWstepne,          "BW - Rejestr skierowań na badania wstępne"}                //
            ,{AppUser.rBadaniaWstepneAddDel,    "BA - Dodawanie i usuwanie osób"}                           //
            ,{AppUser.rRekruter,                "BR - Rekruter"}                                            //
            ,{AppUser.rOpiekun,                 "BO - Opiekun"}                                             //

            ,{AppUser.rSzkoleniaBHP,            "SB - Szkolenia BHP"}                                       // ukrywam na prośbę Oliwii
            ,{AppUser.rSzkoleniaBHPAdm,         "SA - Szkolenia BHP - Administracja"}                       //
            ,{AppUser.rSzkoleniaBHPMailing,     "SM - Szkolenia BHP - Mailing kończących się szkoleń"}      //
#endif

#if (RCP && DBW)
            ,{AppUser.rWnNadgAccAll,            "WA - Akceptacja wniosków o nadgodziny"} 
#endif
#if SIEMENS
            uwaga !!! zmienić id i przekonwertować w bazie, bo nachodzą na uprawnienia MS !!!

            ,{AppUser.rESD_Nadgarstek,          "EN - Wymagany test ESD - Nadgarstek"}                  //
            ,{AppUser.rESD_Stopa,               "ES - Wymagany test ESD - Stopa"}                       //
#endif
            /* COMMON */
            ,{AppUser.rInfokiosk,               "IK - Użytkownik - Infokiosk"}                              //
            ,{AppUser.rSuperuser,               "SU - Superuser (tryb developerski)"}
        };        
        




        //-----------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPracownicy);
            Tools.PrepareRights(null, rights, AppUser.maxRight, 2);
        }

        bool v = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidNaDzien.Value = Tools.DateToStr(DateTime.Today);
                DoSearch(true);     // ustawienie filtra domyślnego

                //FilterExpression = "Pracownik like '%'";

            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
            PrepareSql();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }
        //-----------------------------------------------
        private void PrepareSql()
        {
#if IQOR
            dsPracownicy.SelectCommand = String.Format(dsPracownicy.SelectCommand, dsFilterIQOR.SelectCommand);
#else
            dsPracownicy.SelectCommand = String.Format(dsPracownicy.SelectCommand, "");
#endif
            if (dsPracownicy.SelectCommand.StartsWith("\r\nselect\r\n"))
                dsPracownicy.SelectCommand = "select " + Tools.RightsToSelectSql("P.Rights") + dsPracownicy.SelectCommand.Substring(6+2);  //select do sortowania po prawach
             dsPracownicy.FilterExpression = _FilterExpression;
        }
        //-----------------------------------------------
        #region FILTER
        private void PrepareSearch()
        {
            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            SetFilterExpr(!init);
            if (!init)
            {
                //lvPracownicy.DataBind();
                Deselect();
                /*
                if (lvPracownicy.Items.Count == 1) Select(0);
                else if (lvPracownicy.SelectedIndex != -1) Select(-1);
                 */
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            /*
            string x = _FilterExpression;
            x = Test;
            DoSearch(false);
            x = _FilterExpression;
            */
            
            string x = Test;
            Label1.Text = x;
            Test = tbSearch.Text;
            

            /*
            if (String.IsNullOrEmpty(Test))
                Test = "0";
            else
                Test = (Tools.StrToInt(Test) + 1).ToString();
            */
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        private string _FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                Deselect();
                //lvPracownicy.EditIndex = -1;
                //lvPracownicy.InsertItemPosition = InsertItemPosition.None;

                //dsPracownicy.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        private string Test
        {
            set { ViewState["test"] = value; }
            get { return Tools.GetStr(ViewState["test"]); }
        }

        private void Deselect()
        {
            //SelectedRcpId = null;
            //SelectedStrefaId = null;
            //if (lvPracownicy.SelectedIndex != -1)
            //{
            //    lvPracownicy.SelectedIndex = -1;
            //    TriggerSelectedChanged();
            //}
        }

        private string SetFilterExpr(bool resetPager)
        {
            string filter;
            //string f1 = tabFilter.SelectedValue.Trim();   
            string f1 = null;   // w nowej wersji do wywalenia
            dsPracownicy.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = f1;
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    /*
                    string search = tbSearch.Text;
                    bool s1 = search.StartsWith(" ");
                    bool s2 = search.EndsWith(" ");
                    int len = search.Length;
                    */
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or NrKarty like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#else
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or RcpIdTxt like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#endif
                    dsPracownicy.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or NrKarty like '{0}%' or NrKarty like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#else
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or RcpIdTxt like '{0}%' or RcpIdTxt like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#endif
                    dsPracownicy.FilterParameters.Add("par0", words[0]);
                    dsPracownicy.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
#if SIEMENS
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or NrKarty like '{{{0}}}%')", i);
#else
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or RcpIdTxt like '{{{0}}}%')", i);
#endif
                        dsPracownicy.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            _FilterExpression = filter;
            return filter;
        }

        #endregion
        //-----
        protected void gvPracownicyCmd_Click(object sender, EventArgs e)
        {

        }

        public string SelectedPracId = null;

        protected void cntFilter_Filter(object sender, EventArgs e)
        {
            cntFilter.ApplyTo(dsPracownicy);
        }

        protected void dsPracownicy_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void gvPracownicy_Init(object sender, EventArgs e)
        {

        }

        protected void gvPracownicy_Load(object sender, EventArgs e)
        {

        }

        protected void gvPracownicy_DataBinding(object sender, EventArgs e)
        {

        }

        protected void gvPracownicy_DataBound(object sender, EventArgs e)
        {

        }

        protected void btSave_Click(object sender, EventArgs e)
        {

        }

        protected void dsPracownicy_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
        }

        protected void tabFilter_DataBound(object sender, EventArgs e)
        {
#if !IQOR
            Tools.RemoveMenu(tabFilter.Tabs, "11", "12", "13", "31", "32");   //BYD, ZG, CETOR
#endif
        }

        protected void dsPracownicy_Load(object sender, EventArgs e)
        {
        }


    }
}
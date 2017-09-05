using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.Security;
using HRRcp.App_Code;
using HRRcp.Controls.Przypisania;
using HRRcp.Controls.Adm;
using HRRcp.Controls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntPracownicy3: System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        public event CommandEventHandler Command;

        const string sesPageIdx = "_ADMpidxt";

        public int FMode = 0;
        const int moEdit = 0;   // do edycji w konfiguracji
        const int moSelect = 1; // do podpinania do struktury

        const string lcPrzypisania = "0";
        const string lcUprawnienia = "1";

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
            + 35
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
#if ((RCP || PORTAL) && !SIEMENS)
            ,{AppUser.rKwitekAdm,           "AK - Administracja kwitkiem płacowym"}                      // rKwitekAdm             = 3; //<<<<<<<< podglad pracownika z listy
            ,{AppUser.rPortalAdmin,         "PA - Administracja portalem"}                               // 
            ,{AppUser.rPortalArticles,      "PB - Administracja portalem - artykuły i pliki"}            // artykuły, gazetka, pliki
            ,{AppUser.rWnioskiUrlopoweAdm,  "WX - Administracja wniosków urlopowych"}
#endif
            /* COMMON */
            ,{AppUser.rPrzesunieciaAdm,     "PM - Administracja ustawieniami przesunięć"}
            /* RCP */
#if ((RCP || PORTAL) && !SIEMENS)
            ,{AppUser.rBadaniaMailing,      "MB - Mailing wszystkich kończących się badań"}
#endif
            /* RCP */
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
#if ((RCP || PORTAL) && (!SIEMENS))
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
#if ((RCP) && !SIEMENS)
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
            /* COMMON */
            ,{AppUser.rInfokiosk,               "IK - Użytkownik - Infokiosk"}                         //
            ,{AppUser.rSuperuser,               "SU - Superuser (tryb developerski)"}
        };


        //        public static object[,] rights = new object[rightsCount, 2] {                     
        //          // {AppUser.rAdmin,               "A - Administracja"},                                      // rAdmin                 = 0; // administrator
        //             {AppUser.rRights,              "U - Nadawanie uprawnień"}                                 // rRights                = 1; // prawo do nadawania uprawnień        
        //            ,{AppUser.rTester,              "T - Testowanie nowych funkcjonalności"}                   // _rTester               = 2; //<<<<<<<< testy cc
        //#if SIEMENS || MS
        //            ,{AppUser.rPrzesunieciaAdm,     "PM - Administracja ustawieniami przesunięć"}
        //#else
        //            ,{AppUser.rKwitekAdm,           "AK - Administracja kwitkiem płacowym"}                      // rKwitekAdm             = 3; //<<<<<<<< podglad pracownika z listy
        //            ,{AppUser.rPortalAdmin,         "PA - Administracja portalem"}                               // 
        //            ,{AppUser.rPortalArticles,      "PB - Administracja portalem - artykuły i pliki"}            // artykuły, gazetka, pliki
        //            ,{AppUser.rWnioskiUrlopoweAdm,  "WX - Administracja wniosków urlopowych"}
        //            ,{AppUser.rPrzesunieciaAdm,     "PM - Administracja ustawieniami przesunięć"}
        //            ,{AppUser.rBadaniaMailing,      "MB - Mailing wszystkich kończących się badań"}
        //#endif

        //            ,{AppUser.rDostepniKierownicy,  "PO - Dostęp do obcych pracowników"}                       // rDostepniKierownicy    = 5; // mozliwosc wyboru kierownika i jego pracowników
        //            ,{AppUser.rPlanPracySwoj,       "S1 - Dostęp do swojego czasu pracy"}                      // 
        //            ,{AppUser.rUnlockPP,            "OD - Odblokowywanie zaakceptowanych dni"}                 // 20
        //            ,{AppUser.rKierParams,          "KP - Dostęp do konfiguracji czasów przerw"}
        //          //,{AppUser.rKartyTmp,            "KT - Wydawanie kart tymczasowych"}
        //#if IQOR || KDR || DBW
        //            ,{AppUser.rRepPodzialCC,        "CC - Raporty podziału czasu na cc"}                         // rRepPodzialCC          = 6; // raport poczuału czasu na cc, uprawnienia do cc i class
        //            ,{AppUser.rRepPodzialCCKwoty,   "CK - Raporty podziału czasu na cc - kwoty"}                 // rRepPodzialCCKwoty     = 7; // raport poczuału czasu na cc z kwotami, uprawnienia do cc i class
        //            ,{AppUser.rRepPodzialCCAll,     "CA - Raporty podziału czasu na cc - cała klasyfikacja"}     // rRepPodzialCCAll       = 8; // pełen wgląd w dane, bez konieczności przypisywania cc i class - dla adminiów
        //#endif

        //            ,{AppUser.rRepCzasPracy,        "R1 - Raport czasu pracy RCP"}                             // rRepCzasPracy          = 4; // raport czasu pracy do testów dla Adama Peplinskiego, tymczas poźniej zmienić 
        //#if IQOR || KDR || DBW
        //            ,{AppUser.rRepStolowka,         "R2 - Raport stołówka"}                                      // 
        //            ,{AppUser.rRepESD,              "R3 - Raport zaliczenia testów ESD"}                         // 
        //            ,{AppUser.rRepRezerwaUrlopowa,  "R4 - Raport rezerwy urlopowej"}                             // 
        //#endif
        //            ,{AppUser.rRaporty2,            "R2 - Raporty dodatkowe"}                                  // na tymczas - zmienić to trzeba będzie docelowo na matryce dostepu do raportów 

        //            ,{AppUser.rPrzesuniecia,        "PP - Przesuwanie pracowników"}
        //            ,{AppUser.rPrzesunieciaAcc,     "PA - Akceptowanie przesunięć pracowników"}
        //            ,{AppUser.rPrzesunieciaAccSub,  "PS - Akceptowanie z całej podstruktury"}

        //            ,{AppUser.rRozlNadg,            "RN - Rozliczanie nadgodzin"}                              // 
        //            ,{AppUser.rRozlNadgPo,          "RA - Rozliczanie nadgodzin po zamknięciu okresu"}         // 
        //#if SIEMENS || MS
        // #if SIEMENS
        //            ,{AppUser._rScorecardsAdmin,     "SA - Scorecards - Administracja"}                        // 
        //            ,{AppUser.rScorecardsTLProd,    "TL - Scorecards - Team Leader produktywny"}               // 
        //            ,{AppUser.rScorecardsTLNieprod, "TN - Scorecards - Team Leader nieproduktywny"}            // 
        //            ,{AppUser.rScorecardsKier,      "KI - Scorecards - Kierownik"}                             // 
        //            ,{AppUser.rScorecardsZarz,      "ZA - Scorecards - Zarząd"}                                // 
        //            ,{AppUser.rScorecardsWnAcc,     "SW - Scorecards - Akceptacja wniosków premiowych"}        // 
        //            ,{AppUser.rScorecardsHR,        "SH - Scorecards - HR"}                                   //      
        //            ,{AppUser.rScorecardsControlling,"SC - Scorecards - Controlling"}                          //      
        //            ,{AppUser._rScorecardsKwoty,     "SK - Scorecards - Raporty - podgląd kwot"}
        //            //,{AppUser.rScorecardsZoom,      "SZ - Scorecards - Raporty - szczegóły"}
        //            ,{AppUser.rScorecardsWnRej,     "SD - Scorecards - Deakceptacja arkuszy"}
        //            ,{AppUser.rScorecardsWnDstr,    "SO - Scorecards - Odrzucanie wniosków"}
        //            ,{AppUser.rScorecardsWysPowAdm, "SQ - Scorecards - Otrzymywanie powiadomień administracyjnych"}
        //            ,{AppUser.rScorecardsArkAll,    "S7 - Scorecards - Dostęp do wszystkich arkuszy"}
        // #endif
        // #if ESD
        //            ,{AppUser.rESD,                 "ES - ESD"}
        // #endif
        // #if MS
        //            ,{AppUser.rMSAdmin,             "M0 - Matryca Szkoleń - Administracja"}
        //            ,{AppUser.rMSMeister,           "M1 - Matryca Szkoleń - Mistrz"}
        //            ,{AppUser.rMSTrener,            "M2 - Matryca Szkoleń - Trener"}
        //            ,{AppUser.rMSBHP,               "M3 - Matryca Szkoleń - BHP"}
        //            ,{AppUser.rMSHR,                "M4 - Matryca Szkoleń - HR"}
        //            ,{AppUser.rMSKorekty,           "M5 - Matryca Szkoleń - Korekta ocen"}
        //            ,{AppUser.rMSKorektyAcc,        "M6 - Matryca Szkoleń - Akceptacja korekt"}
        //            ,{AppUser.rMSCertyfikatyAcc,    "M7 - Matryca Szkoleń - Akceptacja certyfikatów"}
        //            ,{AppUser.rMSAnkietyPodgladP,   "M8 - Matryca Szkoleń - Podgląd ankiet (Pracownicy)"}
        //            ,{AppUser.rMSAnkietyPodgladK,   "M9 - Matryca Szkoleń - Podgląd ankiet (Przełożeni)"}
        //            ,{AppUser.rMSAnkietyEdycjaP,    "MA - Matryca Szkoleń - Edycja ankiet (Pracownicy)"}
        //            ,{AppUser.rMSAnkietyEdycjaK,    "MB - Matryca Szkoleń - Edycja ankiet (Przełożeni)"}
        //            ,{AppUser.rMSAnkietyAcc,        "MC - Matryca Szkoleń - Akceptacja ankiet"}
        //            ,{AppUser.rMSPracownicyAdm,     "MD - Matryca Szkoleń - Pracownicy administracji"}
        //            ,{AppUser.rMSPracownicyProd,    "ME - Matryca Szkoleń - Pracownicy produkcji"}
        //            ,{AppUser.rMSWozki,             "MF - Matryca Szkoleń - Aktywna jazda wózkiem widłowym"}
        // #endif
        //#else
        //            ,{AppUser.rPlanUrlopow,         "PU - Plan urlopów"}                                       // 
        //            ,{AppUser.rPlanUrlopowSwoj,     "PS - Edycja swojego planu urlopów"}                       //
        //            ,{AppUser.rPlanUrlopowEditPo,   "PE - Odblokuj edycję po zamknięciu"}                      // 1 miesiac na planowanie, potem korekty, plan z poprzedniego roku jest zablokowany do edycji, chyba ze nadam to uprawnienie
        //            ,{AppUser.rAbsencjeDlugotrwale, "PD - Absencje długotrwałe"}                               // wprowadzanie

        //            ,{AppUser.rWnioskiUrlopowe,     "WU - Wnioski urlopowe"}
        //            ,{AppUser.rWnioskiUrlopoweAcc,  "WA - Akceptowanie wniosków urlopowych"}
        //            ,{AppUser.rWnioskiUrlopoweSub,  "WS - Dostęp do wniosków z podstuktury"}
        //            ,{AppUser.rWnioskiUrlopoweNoAccMail, "WM - Mailing wniosków urlopowych"}                   // przy wyłączonym mailingu głównym 
        //#endif

        //            ,{AppUser.rPortalTmp,           "PO - Dostęp do portalu"}                                  //
        //#if PORTAL
        //            ,{AppUser.rPortalAdmin,         "PA - Portal - administracja"}                              //
        //            ,{AppUser.rPortalArticles,      "PN - Portal - artykuły"}                                   //
        //#endif
        //#if !PORTAL && (IQOR || KDR || DBW)
        //            ,{AppUser.rPodzialLudzi,            "PL - Podział Ludzi"}                                       //
        //            ,{AppUser.rPodzialLudziPM,          "PM - Podział Ludzi - Raporty PM"}                          //
        //            ,{AppUser.rPodzialLudziPMZoom,      "PZ - Podział Ludzi - Raporty PM - szczegóły"}              //
        //            ,{AppUser.rRaportIlDL,              "PD - Podział Ludzi - Raport ilości DL na CC"}              //
        //            ,{AppUser.rRaportCCLim,             "PI - Podział Ludzi - Raport przekroczeń limitów na CC"}    //
        //            ,{AppUser.rRaportCCLimMailing,      "PM - Podział Ludzi - Mailing przekroczeń limitów na CC"}   //            
        //            ,{AppUser.rPLMailingPrzesCC,        "PC - Podział Ludzi - Mailing przesunięć na CC"}            //            
        //            ,{AppUser.rEditCCLim,               "PE - Podział Ludzi - Edycja limitów nadgodzin na CC"}      //

        //            ,{AppUser.rPodzialLudziEditS,       "PS - Podział Ludzi - Edycja splitów"}                      //
        //            ,{AppUser.rPodzialLudziEditSGrupy,  "PG - Podział Ludzi - Edycja splitów dla grup"}             //
        //            ,{AppUser.rPodzialLudziEditCAP,     "PC - Podział Ludzi - Edycja CC,Comm,Area,Pos"}             //
        //            ,{AppUser.rHideSalary,              "PH - Podział Ludzi - Ukryj kwotę wynagrodzenia"}           //
        //            ,{AppUser.rShowHiddenSalary,        "PV - Podział Ludzi - Pokaż ukryte kwoty wynagrodzenia"}    //
        //            ,{AppUser.rPodzialLudziAdm,         "PA - Podział Ludzi - Administracja"}                       //
        //#endif
        //#if IPO
        //#endif
        //#if !PORTAL && (IQOR || KDR || DBW)
        //            ,{AppUser.rBadaniaWstepne,          "BW - Rejestr skierowań na badania wstępne"}                //
        //            ,{AppUser.rBadaniaWstepneAddDel,    "BA - Dodawanie i usuwanie osób"}                           //
        //            ,{AppUser.rRekruter,                "BR - Rekruter"}                                            //
        //            ,{AppUser.rOpiekun,                 "BO - Opiekun"}                                             //

        //            ,{AppUser.rSzkoleniaBHP,            "SB - Szkolenia BHP"}                                       // ukrywam na prośbę Oliwii
        //            ,{AppUser.rSzkoleniaBHPAdm,         "SA - Szkolenia BHP - Administracja"}                       //
        //            ,{AppUser.rSzkoleniaBHPMailing,     "SM - Szkolenia BHP - Mailing kończących się szkoleń"}      //
        //#endif
        //            ,{AppUser.rInfokiosk,               "IK - Użytkownik - Infokiosk"}                         //
        //            ,{AppUser.rSuperuser,               "SU - Superuser (tryb developerski)"}


        //            /*
        //            {AppUser.rMailing,            "Otrzymywanie powiadomień"}
        //            ,{AppUser.rAdmin,               "A - Administracja"}
        //            ,{AppUser.rRights,              "U - Nadawanie uprawnień"}

        //            ,{AppUser.rSlowniki,            "S - Edycja słowników"}

        //            ,{AppUser.rRaporty,             "R - Raporty"}
        //            ,{AppUser.rZastepstwo,          "Z - Ustalenie zastępstwa"}

        //            ,{AppUser.rWnioski,             "WN - Składanie wniosków"}
        //            ,{AppUser.rWnioskiAccNew,       "AN - Akceptowanie wniosków o dodanie jorg."}
        //            ,{AppUser.rWnioskiAccDel,       "AD - Akceptowanie wniosków o usunięcie jorg."}
        //            ,{AppUser.rWnioskiAccMod,       "AM - Akceptowanie wniosków o modyfikację jorg."}

        //            ,{AppUser.rReadOnly,            "RO - Tryb podglądu danych"}
        //            ,{AppUser.rSuperuser,           "K - Konfiguracja aplikacji"}
        //             */
        //        };


        //-----------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
#if MS
            Tools.PrepareDicListView(lvPracownicy, Tools.ListViewMode.Bootstrap);
#else
            Tools.PrepareDicListView(lvPracownicy, Tools.ListViewMode.Default);
#endif
            Tools.PrepareSorting(lvPracownicy, 101, 120);
            Tools.PrepareRights(null, rights, AppUser.maxRight, 2);
        }

        bool v = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidNaDzien.Value = Tools.DateToStr(DateTime.Today);
                DoSearch(true);     // ustawienie filtra domyślnego
#if !IQOR
                for (int i = tabFilter.Items.Count - 1; i > 0; i--)
                {
                    string s = tabFilter.Items[i].Text;
                    if (s == "ZG" || s == "BYD" || s == "CETOR")
                        tabFilter.Items.RemoveAt(i);
                }
#endif
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }

        public void Prepare(DateTime nadzien)
        {
            hidNaDzien.Value = Tools.DateToStr(nadzien);
            lvPracownicy.DataBind();
        }
        //---------------------------
        public string PrepareName(object name)
        {
            return Tools.PrepareName(name.ToString());
        }

        public string GetStatus(object status)
        {
            int st = Base.getInt(status, -999);
            return App.GetStatus(st);
        }

        public string GetOdDo(object dOd, object dDo)
        {
            return Tools.GetOdDo(dOd, dDo);
        }

        public string GetEditColSpan()   // celka na tabelkę przy edycji 
        {
            switch (ListContent)
            {
                default:
                case lcPrzypisania:
                    return (7).ToString();
                case lcUprawnienia:
                    return (rightsCount + 6).ToString();
            }
        }

        private void TriggerSelectedChanged()
        {
            bool s = lvPracownicy.SelectedIndex != -1;
            btPrzesuniecia.Enabled = s;
            btPlanPracy.Enabled = s;
            btAkceptacja.Enabled = s;

            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        private void TriggerCommand(string cmd, string arg)
        {
            if (Command != null)
            {
                CommandEventArgs e = new CommandEventArgs(cmd, arg);
                Command(this, e);
            }
        }












        //----- FILTER ----------------------------------
        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                Deselect();
                lvPracownicy.EditIndex = -1;
                lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        private void Deselect()
        {
            SelectedRcpId = null;
            SelectedStrefaId = null;
            if (lvPracownicy.SelectedIndex != -1)
            {
                lvPracownicy.SelectedIndex = -1;
                TriggerSelectedChanged();
            }
        }
        //-----
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

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {
        }

        /*
        private string GetSearch(string field, string search)
        {
            bool s1 = search.StartsWith(" ");
            bool s2 = search.EndsWith(" ");
            int len = search.Length;

            if (s1 && s2 && len > 2) return String.Format("{0}='{1}'", field, search.Substring(1, len-2));
            else
                if (s1 && len > 1) 
                    return String.Format("{0} like '{1}%'", field, search.Substring(1, len-1));
                else if (s2 && len > 1) 
                    return String.Format("{0} like '%{1}'", field, search.Substring(0, len-1));
                else
                    return String.Format("{0} like '%{1}%'", field, search.Trim());
        }
        */



        private string SetFilterExpr(bool resetPager)
        {
            string filter;
            string f1 = tabFilter.SelectedValue.Trim();
            SqlDataSource1.FilterParameters.Clear();
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
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or NrKarty like '{0}%' or NrKarty like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#else
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or RcpIdTxt like '{0}%' or RcpIdTxt like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#endif
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
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
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvPracownicy);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
        }

        private string SetFilterExpr_1(bool resetPager)
        {
            string filter;
            string f1 = tabFilter.SelectedValue.Trim();
            SqlDataSource1.FilterParameters.Clear();
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
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvPracownicy);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
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
            DoSearch(false);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(false);
        }


        
        /*      
        public void Select(int index)
        {
            lvPrzypisania.SelectedIndex = index;
            CheckSelectedChanged();
        }

        private bool CheckSelectedChanged()
        {
            string oldId = SelectedId;
            string newId = lvPrzypisania.SelectedDataKey == null ? null : lvPrzypisania.SelectedDataKey.Value.ToString();
            if (oldId != newId)
            {
                SelectedId = newId;
                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);
                return true;
            }
            else
                return false;
        }
        */





        //---------------------------------------
        private void ShowContentHeader()
        {
            switch (ListContent)
            {
                case lcPrzypisania:
                    break;
                case lcUprawnienia:
                    break;
            }
            bool cp = ListContent == lcPrzypisania;
            Tools.SetControlVisible(lvPracownicy, "thStrefa", cp);
            Tools.SetControlVisible(lvPracownicy, "thKierownik", cp);
            Tools.SetControlVisible(lvPracownicy, "thDzial", cp);

            bool cu = ListContent == lcUprawnienia;
            if (cu)
            {
                Tools.PrepareRightsTh(lvPracownicy);
            }
            else
            {
                Tools.SetControlVisible(lvPracownicy, "thRights", false);
                for (int i = 0; i < rightsCount; i++)
                {
                    string c = (i + 1).ToString();
                    Tools.SetControlVisible(lvPracownicy, "thR" + c, false);
                }
            }
            Tools.SetControlVisible(lvPracownicy, "th111", cu);  // adm
            Tools.SetControlVisible(lvPracownicy, "th112", cu);  // raporty
        }

        private void ShowContentLine(ListViewItemEventArgs e)
        {
            bool cp = ListContent == lcPrzypisania;
            Tools.SetControlVisible(e.Item, "tdStrefa", cp);
            Tools.SetControlVisible(e.Item, "tdKierownik", cp);
            Tools.SetControlVisible(e.Item, "tdDzial", cp);

            bool cu = ListContent == lcUprawnienia;     
            if (!cu)
                for (int i = 0; i < rightsCount; i++)
                {
                    string c = (i + 1).ToString();
                    Tools.SetControlVisible(e.Item, "tdR" + c, false);
                }
            Tools.SetControlVisible(e.Item, "td111", cu);  // adm
            Tools.SetControlVisible(e.Item, "td112", cu);  // raporty
        }

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            lvPracownicy.DataBind();
        }

        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {
            //FilterExpression = tabFilter.SelectedValue;
            SetFilterExpr(true);
        }















        //---------------------------------------
        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    string p1, p2;
                    Tools.GetLineParams(e.CommandArgument.ToString(), out p1, out p2);
                    SelectedRcpId = p1;
                    SelectedStrefaId = p2;
                    //TriggerSelectedChanged();
                    break;
                case "DeSelect":
                    Deselect();
                    break;
                case "Edit":
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    break;
                case "Update":
                    break;
                case "NewRecord":
                    lvPracownicy.EditIndex = -1;
                    Deselect();
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", false);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "Insert":  // <<<< dodać odświeżenie LetterPagera !!!
                    break;
                case "CancelInsert":
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("NazwiskoImie", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);

                    Deselect();
                    break;
                    //---------------
                    /*
                case "passreset":
                    string id = e.CommandArgument.ToString();
                    //string pesel = db.getScalar("select Nick from Pracownicy where Id = " + id);
                    DataRow dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, Nick from Pracownicy where Id = " + id);
                    string nazwisko = db.getValue(dr, 0);
                    string pesel = db.getValue(dr, 1);
                    if (!String.IsNullOrEmpty(pesel)) 
                    {
#if SPX
                        if (AppUser.UpdatePass(id, pesel))
                        //if (true)
#else
                        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod);
                        if (db.update("Pracownicy", 0, "Pass", "Id=" + id, db.strParam(hash)))
#endif
                        {
                            Tools.ShowMessage("Hasło zostało ustawione.");
                            Log.Info(Log.PASSRESET, "Reset hasła pracownika", nazwisko + ' ' + id);
                        }
                    }
                    else
                    {
                        Tools.ShowMessage("Wystąpił błąd podczas ustawiania hasła.");
                        Log.Error(Log.PASSRESET, "Reset hasła pracownika - BŁĄD", nazwisko + ' ' + id);
                    }
                    break;
                     */
                case "passreset":
                    string id = e.CommandArgument.ToString();
                    //string pesel = db.getScalar("select Nick from Pracownicy where Id = " + id);
                    DataRow dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, Nick, Pass, EMail, Mailing from Pracownicy where Id = " + id);
                    string nazwisko = db.getValue(dr, 0);
                    string oldpass = db.getValue(dr, 2);
                    string email = db.getValue(dr, 3);
                    bool mailing = db.getBool(dr, 4, false);

                    if (mailing)
                    {
                        //string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod);   // hasło domyślne - pesel
                        string pass = AppUser.GeneratePass(1);
                        if (AppUser.UpdatePass(id, pass, true))
                        {
                            Log.Info(Log.PASSRESET, "Reset hasła pracownika", nazwisko + ' ' + id);
                            if (Mailing.EventPassReset(id, pass))
                            {
                                Tools.SetText2(e.Item, "tbPass", null);
                                Tools.ShowMessage("Hasło zostało ustawione.\\nMail z nowym hasłem został wysłany na adres: {0}.", email);
                            }
                            else
                            {
                                DataRow mail = Mailing.GetData(Mailing.maSYS_PASSRESET);        // pobieramy dane maila
                                bool active = db.getBool(mail, "Aktywny", false);               // jak nie ma definicji maila to null->false
                                if (active)
                                    Tools.ShowError("Wystąpił błąd podczas wysyłania do pracownika maila z hasłem.\\nProszę zweryfikować poprawność adresu e-mail ({0}) i powtórzyć procedurę.", mail);
                                else
                                    Tools.ShowMessage("Mail nie został wysłany - nie jest aktywny.\\nProszę włączyć mail w Ustawieniach powiadomień automatycznych.");
                                AppUser.UpdatePass(id, oldpass, false); // przywracam juz bez kontrli błędów, jak mail do pracownika ma wyjść to już powinno być wszystko ustawione, dlatego tak
                            }
                        }
                        else
                        {
                            Log.Error(Log.PASSRESET, "Reset hasła pracownika - BŁĄD", nazwisko + ' ' + id);
                            Tools.ShowMessage("Wystąpił błąd podczas ustawiania hasła.");
                        }
                    }
                    else
                        Tools.ShowMessage("Pracownik ma wyłączone wysyłanie powiadomień mailowych.\\nProszę włączyć opcję Mailing, zapisać zmiany i powtórzyć procedurę.");
                    break;
                case "passset":
                    string newpass = Tools.GetText(e.Item, "tbPass").Trim();
                    if (AppUser.CheckPass(newpass, App.PassType))
                    {
                        id = e.CommandArgument.ToString();
                        dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, Nick, Pass, EMail, Mailing from Pracownicy where Id = " + id);
                        nazwisko = db.getValue(dr, 0);

                        if (AppUser.UpdatePass(id, newpass, true))
                        {
                            Log.Info(Log.PASSRESET, "Ustawienie hasła pracownika", nazwisko + ' ' + id);
                            Tools.ShowMessage("Hasło zostało ustawione.\\n\\nUwaga! Hasło nie jest nigdzie zapamiętywane, proszę przekazać je pracownikowi.");
                        }
                        else
                        {
                            Log.Error(Log.PASSRESET, "Ustawienie hasła pracownika - BŁĄD", nazwisko + ' ' + id);
                            Tools.ShowMessage("Wystąpił błąd podczas ustawiania hasła.");
                        }
                    }
                    break;
                case "passgen":
                    newpass = AppUser.GeneratePass(1);
                    Tools.SetText2(e.Item, "tbPass", newpass);
                    break;
                case "editrcpid":
                    id = Tools.GetDataKey(lvPracownicy, e);     // wywala się jak z InsertItem, bo nie ma tam DataListItem
                    cntKartyRcpPopup.Show(id);
                    break;
                case "editalg":
                    id = Tools.GetDataKey(lvPracownicy, e);
                    cntAlgorytmyPopup.Show(id);
                    break;
                case "editstan":
                    id = Tools.GetDataKey(lvPracownicy, e);
                    cntStanowiskaPopup.Show(id);
                    break;
            }
        }

        protected void lvPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }
        //------------------------------------------
        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            canSetRights = App.User.HasRight(AppUser.rRights);
            ro = App.User.HasRight(AppUser.rReadOnly);
#if MS
            ro = false;
#endif
#if SCARDS
            scAdm = App.User.IsScAdmin;
            //sckwoty = App.User.HasRight(AppUser.rScorecardsKwoty); 
            scHR    = AppUser.HasRight(App.User.OriginalRights, AppUser.rScorecardsHR);    // czy user lub oryginalny user w zastepstwie ma prawo
            scKwoty = AppUser.HasRight(App.User.OriginalRights, AppUser._rScorecardsKwoty); 
            scZoom  = AppUser.HasRight(App.User.OriginalRights, AppUser._rScorecardsZoom); 
            scControlling = AppUser.HasRight(App.User.OriginalRights, AppUser.rScorecardsControlling); 
#endif
            if (SqlDataSource1.SelectCommand.StartsWith("select\r\n"))
                SqlDataSource1.SelectCommand = "select " + Tools.RightsToSelectSql("P.Rights") + SqlDataSource1.SelectCommand.Substring(6);  //select do sortowania po prawach
            SqlDataSource1.FilterExpression = FilterExpression;
        }
        
        protected void lvPracownicy_DataBinding(object sender, EventArgs e)
        {

        }

        protected void lvPracownicy_DataBound(object sender, EventArgs e)
        {
            ShowContentHeader();
            Tools.UpdateLetterPager(lvPracownicy);    // może być wywoływane automatycznie przy Tools.PrepareDicListView, ale szkoda czasu bo przy każdym dic
#if SIEMENS
            //Tools.SetControlVisible(lvPracownicy, "thKierownikBr", false);
            Tools.SetControlVisible(lvPracownicy, "thKierownikLine2", false);
            Tools.SetControlVisible(lvPracownicy, "thArkusz", Lic.ScoreCards);

            LinkButton lbt = lvPracownicy.FindControl("LinkButton103") as LinkButton;
            if (lbt != null) lbt.CommandArgument = "NrKarty";
#else
            switch (cntKartyRcp.defMode)
            {
                case cntKartyRcp.moNoNrKarty:
                    break;
                case cntKartyRcp.moNoRcpId:
                    LinkButton lbt = lvPracownicy.FindControl("LinkButton103") as LinkButton;
                    if (lbt != null) lbt.CommandArgument = "NrKarty";
                    break;
                case cntKartyRcp.moShowAll:
                    Tools.SetControlVisible(lvPracownicy, "Literal116", true);
                    Tools.SetControlVisible(lvPracownicy, "LinkButton116", true);
                    break;
            }
#endif
            if (lvPracownicy.Items.Count == 1)
            {
                lvPracownicy.SelectedIndex = 0;
                TriggerSelectedChanged();
            }
        }

        protected void lvPracownicy_PreRender(object sender, EventArgs e)   //tylko tu mogę modyfikować kontrolki na EditItemTemplate, w innych funkcjach nawet jak znajdzie to nie potrafi ustawić visible
        {
        }
        //--------------------------------------------
        private void SetTB(Control item, string name, int maxlen, int width)
        {
            TextBox tb;
            if  (Tools.FindTextBox(item, name, out tb))
            {
                tb.MaxLength = maxlen;
                if (width != -1)
                    tb.Width = width;
            }
        }

        private void PrepareEditControls(Control item)
        {
#if SIEMENS
            SetTB(item, "KadryIdTextBox", 8, 80);
            SetTB(item, "tbKadryId2", 8, 80);
            SetTB(item, "RcpIdTextBox", 20, 80);
#else
            SetTB(item, "KadryIdTextBox", 5, 60);
            SetTB(item, "RcpIdTextBox", 5, 60);
#endif
            SetTB(item, "LoginTextBox", 20, 200);
            SetTB(item, "EmailTextBox", 200, -1);
            //SetTB(item, "StatusTextBox", 3, 50);
            DropDownList ddl = (DropDownList)item.FindControl("ddlStatus");
            App.FillStatus(ddl, null, true);
        }

        private void PrepareRights(ListViewItemEventArgs e, bool insert, bool edit)
        {
            string dbRights;
            if (insert)
                dbRights = new String('0', maxRights);  // domyślne
            else
            {
                DataRowView drv = Tools.GetDataRowView(e);
                dbRights = drv["Rights"].ToString();
            }
            
            Tools.SetText(e.Item, "lbRightsTitle", canSetRights ? "Uprawnienia" : "Uprawnienia - brak prawa do nadawania");

            char[] ra = dbRights.ToCharArray();

            bool ei = edit || insert;

            //bool nselect = liMode != Tools.limSelect;
            bool ie = insert || edit;
            bool enabled = ie && canSetRights && !ro;
            
            for (int i = 0; i < rightsCount; i++)
            {
                int r = (int)rights[i, 0];
                if (r == AppUser._rMailing)
                    Tools.prepareRightTd(e.Item, ei, i, ra, ie);     // mailing niech zawsze będzie można ustawić
#if SCARDS
                else if (!enabled && scAdm && (
                       r == AppUser._rScorecardsAdmin
                    || r == AppUser.rScorecardsTLNieprod
                    || r == AppUser.rScorecardsTLProd
                    || r == AppUser.rScorecardsKier
                    || r == AppUser.rScorecardsZarz
                    || r == AppUser.rScorecardsWnAcc
                    || r == AppUser.rScorecardsHR && scHR
                    || r == AppUser.rScorecardsControlling && scControlling
                    || r == AppUser._rScorecardsKwoty && scKwoty      // jeżeli sam mam prawo do kwot 
                    || r == AppUser._rScorecardsZoom && scZoom
                    //|| r == AppUser.rScorecard
                    )) 
                    Tools.prepareRightTd(e.Item, ei, i, ra, ie);     // mailing niech zawsze będzie można ustawić
#endif
                else
                    Tools.prepareRightTd(e.Item, ei, i, ra, enabled);
            }

            Tools.SetControlEnabled(e.Item, "AdminCheckBox", enabled);
            Tools.SetControlEnabled(e.Item, "RaportyCheckBox", enabled);
        }
        //-------------------------------
        private void ddlSelect(Control item, string ddlName, object selValue, SqlDataSource sqlDS)
        {
            DropDownList ddl = item.FindControl(ddlName) as DropDownList;
            if (ddl != null)
            {
                string sv = selValue == null ? null : selValue.ToString();
                sqlDS.SelectParameters["selId"].DefaultValue = sv;
                ddl.DataSourceID = null;
                ddl.DataSource = sqlDS;
                ddl.DataBind();
            }
            Tools.SelectItem(ddl, selValue);
        }

        private void _PrepareButtons(ListViewItem item, string tr, string btSelect, string btEdit, string btEditPostback)
        {
            //Control cntS = item.FindControl(cntSelectOnClick) as Control;
            //Control cntE = item.FindControl(cntEditOnClick) as Control;
            //if (cntS != null && btS != null && cntE != null && btE != null)
            Control btS = item.FindControl(btSelect);
            Control btE = item.FindControl(btEditPostback);
            if (btS != null && btE != null)
            {
                //Tools.OnClick(item, tr, "javascript:doClickSelectFFfix('{0}');", btS.ClientID);
                Tools.OnClick(item, btEdit, "javascript:doClickEditFFfix('{0}');return false;", btE.ClientID);
            }
        }

        private void PrepareButtons(ListViewItem item, string tr, string btSelect, string btEdit)
        {
            Control btS = item.FindControl(btSelect);
            if (btS != null)
            {
                Tools.OnClick(item, tr, "javascript:doClickSelectFFfix('{0}');", btS.ClientID);
                Tools.OnClick(item, btEdit, "javascript:doClickEditFFfix2();", "");
            }
        }

        private void PrepareItem(ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, lvPracownicy, out select, out edit, out insert);
            switch (lim)
            {
                case Tools.limSelect:
                    if (!create)
                    {
                        //Tools.OnClick(e.Item, "trLine", "NazwiskoLinkButton"); //"SelectButton");
                        //_PrepareButtons(e.Item, "trLine", "NazwiskoLinkButton", "EditButton", "btEdit");
                        PrepareButtons(e.Item, "trLine", "NazwiskoLinkButton", "EditButton");

                        ShowContentLine(e);
                        if (ListContent == lcUprawnienia)
                            PrepareRights(e, false, false);
/*
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "tdKierownikLine2", false);
                        Tools.SetControlVisible(e.Item, "RcpLabel", false);
                        Tools.SetControlVisible(e.Item, "RcpLabel2", true);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#else
*/
                        switch (cntKartyRcp.defMode)
                        {
                            default:
                            case cntKartyRcp.moNoNrKarty:
                                break;
                            case cntKartyRcp.moNoRcpId:  // SIEMENS
                                Tools.SetControlVisible(e.Item, "RcpLabel", false);
                                Tools.SetControlVisible(e.Item, "RcpLabel2", true);
                                break;
                            case cntKartyRcp.moShowAll:
                                Tools.SetControlVisible(e.Item, "RcpLabel", true);
                                Tools.SetControlVisible(e.Item, "RcpLabel1", true);
                                Tools.SetControlVisible(e.Item, "RcpLabel2", true);
                                break;
                        }
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
                        Tools.SetControlVisible(e.Item, "tdKierownikLine2", false);
                        Tools.SetControlVisible(e.Item, "tdArkusz", Lic.ScoreCards);
#endif
                    }
                    break;
                case Tools.limEdit:
                    if (!create)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);

                        //string kid = drv["KadryId"].ToString();
                        //bool spoza = String.IsNullOrEmpty(kid);
                        //bool v = spoza || kid == "00000";   //zeby mozna było skasowac pusty root-rekord po imporcie

                        bool v = true;
                        Button bt = (Button)Tools.SetControlVisible(e.Item, "DeleteButton", v);   // zawsze można usunąć
                        if (bt != null && v) Tools.MakeConfirmDeleteRecordButton(bt);

                        PrepareEditControls(e.Item);

                        /*
                        object kid = drv["IdKierownika"];
                        DropDownList ddl = e.Item.FindControl("ddlKierownik") as DropDownList;
                        if (ddl != null)
                        {
                            SqlDataSourceKier.SelectParameters["IdKierownika"].DefaultValue = Tools.GetInt(kid, -1).ToString();
                            ddl.DataBind();
                        }
                        Tools.SelectItem(e.Item, "ddlKierownik", kid);
                         */

                        ddlSelect(e.Item, "ddlKierownik", drv["IdKierownika"], SqlDataSourceKierEdit);
                        ddlSelect(e.Item, "ddlStrefa", drv["RcpStrefaId"], SqlDataSourceStrefaEdit);
                        ddlSelect(e.Item, "ddlCommodity", drv["IdCommodity"], SqlDataSourceCommEdit);
                        ddlSelect(e.Item, "ddlArea", drv["IdArea"], SqlDataSourceAreaEdit);
                        ddlSelect(e.Item, "ddlPosition", drv["IdPosition"], SqlDataSourcePosEdit);

                        //Tools.SelectItem(e.Item, "ddlKierownik", drv["IdKierownika"]);
                        //Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
                        //Tools.SelectItem(e.Item, "ddlCommodity", drv["IdCommodity"]);
                        //Tools.SelectItem(e.Item, "ddlArea", drv["IdArea"]);
                        //Tools.SelectItem(e.Item, "ddlPosition", drv["IdPosition"]);
                        
                        
                        /*
                        Tools.SelectItem(e.Item, "ddlDzial", drv["IdDzialu"]);
                        Tools.SelectItem(e.Item, "ddlStanowisko", drv["IdStanowiska"]);
                        Tools.SelectItem(e.Item, "ddlKierownik", drv["IdKierownika"]);
                        Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
                        Tools.SelectItem(e.Item, "ddlAlgorytm", drv["RcpAlgorytm"]);
                        
                        Tools.SelectItem(e.Item, "ddlLinia", drv["IdLinii"]);
                        Tools.SelectItem(e.Item, "ddlSplit", drv["GrSplitu"]);
                        */


                        object st = drv["Status"];
                        Tools.SelectItem(e.Item, "ddlStatus", st == null ? App.stPomin.ToString() : st.ToString());
#if SIEMENS
                        bool _kadm = App.User.HasRight(AppUser.rKwitekAdm) || App.User.HasRight(AppUser.rPortalAdmin);
#else
                        bool _kadm = App.User.HasRight(AppUser.rKwitekAdm) || App.User.HasRight(AppUser.rPortalAdmin) || App.User.IsAdmin;
#endif
                        Tools.SetControlVisible(e.Item, "paPass", _kadm);
                        if (_kadm)
                        {
                            bt = Tools.FindButton(e.Item, "btKwitekPassReset");
                            if (bt != null)
                            {
#if PORTAL  
                                bt.Text = "Resetuj hasło";
#endif
                                //Tools.MakeConfirmButton(bt, "Zmienić hasło na domyślne ?");
                                string email = Tools.GetText(e.Item, "EmailTextBox");
                                Tools.MakeConfirmButton(bt, String.Format(
                                    "Potwierdź reset hasła.\\n\\nNowe hasło zostanie wysłane na adres e-mail zapisany w bazie:\\n{0}.\\n\\nJeżeli właśnie został on zmieniony, należy najpierw zapisać zmiany."
                                    , email));
                            }
                            bt = Tools.FindButton(e.Item, "btKwitekPassSet");
                            if (bt != null)
                            {
#if PORTAL  
                                bt.Text = "Ustaw hasło";
#endif
                                Tools.MakeConfirmButton(bt, "Potwierdź ustawienie hasła.\\n\\nUwaga! W tej opcji nie jest wysyłane powiadomienie mailowe do pracownika.");
                            }
                        }
                        /*                        
                                                #if xxSIEMENS
                                                                        Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                                                                        Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                                                                        Tools.OnClick(e.Item, "RcpIdTextBox2", "ibtEditRcpId");
                                                                        Tools.SetControlVisible(e.Item, "paCommodity", false);
                                                                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
                                                #else
                                                */
                        Tools.OnClick(e.Item, "RcpIdTextBox", "ibtEditRcpId");
                        switch (cntKartyRcp.defMode)
                        {
                            case cntKartyRcp.moNoNrKarty:
                                break;
                            case cntKartyRcp.moNoRcpId:
                                Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                                Tools.SetControlVisible(e.Item, "ibtEditRcpId", false);
                                Tools.SetControlVisible(e.Item, "brRcpKarta", false);
                                
                                Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                                Tools.SetControlVisible(e.Item, "ibtEditRcpId2", true);
                                Tools.SetControlVisible(e.Item, "brRcpKarta2", true);
                                Tools.OnClick(e.Item, "RcpIdTextBox2", "ibtEditRcpId2");
                                break;
                            case cntKartyRcp.moShowAll:
                                Tools.SetControlVisible(e.Item, "lbRcpKarta2", true);
                                Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                                Tools.SetControlVisible(e.Item, "brRcpKarta2", true);
                                
                                Tools.OnClick(e.Item, "RcpIdTextBox2", "ibtEditRcpId");
                                break;
                        }
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "paCommodity", Lic.ScoreCards);
                        if (Lic.ScoreCards) Tools.SetText(e.Item, "lbCommodity", "Arkusz:");
                        Tools.SetControlVisible(e.Item, "paArea", false);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#endif                        
                        PrepareRights(e, false, true);

                        cntPrzypisaniaParametry pp = e.Item.FindControl("cntParametry1") as cntPrzypisaniaParametry;
                        if (pp != null)
                        {
                            string pracId =  Tools.GetDataKeyEdited(lvPracownicy);
                            pp.Prepare(pracId, Tools.DateToStr(DateTime.Today));
                        }

                    }
                    break;
                case Tools.limInsert:
                    if (create)
                    {
                        PrepareEditControls(e.Item);
                        Tools.SelectItem(e.Item, "ddlStatus", App.stNew.ToString()); //App.stPomin.ToString());
/*
#if xxSIEMENS
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                        Tools.SetControlVisible(e.Item, "paCommodity", false);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#else
*/ 
                        switch (cntKartyRcp.defMode)
                        {
                            case cntKartyRcp.moNoNrKarty:
                                break;
                            case cntKartyRcp.moNoRcpId:
                                Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                                Tools.SetControlVisible(e.Item, "brRcpKarta", false);

                                Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                                Tools.SetControlVisible(e.Item, "brRcpKarta2", true);
                                break;
                            case cntKartyRcp.moShowAll:
                                Tools.SetControlVisible(e.Item, "lbRcpKarta2", true);
                                Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                                Tools.SetControlVisible(e.Item, "brRcpKarta2", true);
                                break;
                        }
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "paCommodity", Lic.ScoreCards);
                        if (Lic.ScoreCards) Tools.SetText(e.Item, "lbCommodity", "Arkusz:");
                        Tools.SetControlVisible(e.Item, "paArea", false);

                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#endif
                        TimeEdit te = e.Item.FindControl("teWymiar") as TimeEdit;
                        if (te != null) te.Seconds = 8 * 3600;
                        te = e.Item.FindControl("tePrzerwaWliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;
                        te = e.Item.FindControl("tePrzerwaNiewliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;

                        PrepareRights(e, true, false);
                    }
                    break;
            }
            //-----
            if (!create)
            {
                if (select || edit)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trLine");
                    if (tr != null)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        bool kier = db.getBool(drv["Kierownik"], false);
                        string status = drv["Status"].ToString();
                        Tools.AddClass(tr, (kier ? "kier " : "") + "status" + status);
                    }
                }
            }
        }

        protected void lvPracownicy_ItemCreated(object sender, ListViewItemEventArgs e) // tylko tu jest dostęp do InserItemTemplate
        {
            PrepareItem(e, true);
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, false);
        }






        //--------------------------------------------
        private bool UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values, EventArgs ea)
        {
            values["IdDzialu"] = Tools.GetDdlSelectedValueInt(item, "ddlDzial");
            values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");
            values["RcpAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlAlgorytm");
            values["IdLinii"] = Tools.GetDdlSelectedValueInt(item, "ddlLinia");
            values["GrSplitu"] = Tools.GetDdlSelectedValueInt(item, "ddlSplit");
            




            string p1 = Tools.GetText(item, "hidIdPrzypisania");
            int? p2 = Tools.GetDdlSelectedValueInt(item, "ddlKierownik");
            int? p3 = Tools.GetDdlSelectedValueInt(item, "ddlStrefa");
            int? p4 = Tools.GetDdlSelectedValueInt(item, "ddlCommodity");
            int? p5 = Tools.GetDdlSelectedValueInt(item, "ddlArea");
            int? p6 = Tools.GetDdlSelectedValueInt(item, "ddlPosition");

            /* trzeba by jeszcze wymusić zeby validator się wyświetlił, na razie włączam na sztywno
            bool ppv = p1 == null && (p3 != null || p4 != null || p5 != null || p6 != null);   // coś z przypisania wypełnione oprócz kierownika
            Tools.SetControlEnabled(item, "rfvKierownik", ppv);
            if (ppv) 
                return false;
            */
            values["IdPrzypisania"] = p1;
            values["IdKierownika"]  = p2;
            values["RcpStrefaId"]   = p3;
            values["IdCommodity"]   = p4;
            values["IdArea"]        = p5;
            values["IdPosition"]    = p6;




            DateEdit de1 = item.FindControl("deZatr") as DateEdit;
            DateEdit de2 = item.FindControl("deZwol") as DateEdit;
            if (de1 != null && de2 != null)
            {
                if (de1.Date != null && de2.Date != null)
                {
                    DateTime d1 = (DateTime)de1.Date;
                    DateTime d2 = (DateTime)de2.Date;
                    if (d2 < d1)
                    {
                        Tools.ShowError("Data zwolnienia wcześniejsza od daty zatrudnienia.");
                        return false;
                    }
                }
            }


            //values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");    // Tools.UpdateDdlValues przy prepare Dic to zrobi
            //values["Grupa"] = Tools.GetDdlSelectedValueInt(item, "ddlGrupa");
            //values["Klasyfikacja"] = Tools.GetDdlSelectedValueInt(item, "ddlKlasyfikacja");
            
            int? status = Tools.GetDdlSelectedValueInt(item, "ddlStatus");
            values["Status"] = status;
            string r0 = Tools.GetStr(values["Rights"]);        //20160221 zeby nie znimały prawa jak ich nie ma na liście np przy SzkoleniachBHP; moze byc null jak zakładam nowego usera
            string rights = Tools.applyRights(item, r0);
            values["Rights"] = rights;

            AppUser user = AppUser.CreateOrGetSession();
            bool r = (bool)values["Raporty"];
            bool or = oldValues != null ? (bool)oldValues["Raporty"] : false;  // jak dopisuję nowego to zawsze odnoszę do false 
            if (r && !or && !user.IsRaporty)    // chcę ustawić i nowy lub nie miał ustawione
            {
                string id = Base.getScalar("select top 1 Id from Pracownicy where Raporty=1");
                if (!String.IsNullOrEmpty(id))  // jezeli juz ktos jest z uprawnieniem to kontroluje dostep
                {
                    values["Raporty"] = false;
                    Log.Info(Log.t2APP_ADMREPORTS, "Próba ustawienia dostępu do raportów dla " + values["Nazwisko"] + " " + values["Imie"] + " bez wymaganych uprawnień", null, Log.OK);
                    Tools.ShowMessage("Uprawnienia dostępu do raportów można nadać jedynie jeżeli samemu się je posiada.");
                    return false;
                }
            }
            if (lvPracownicy.EditIndex >= 0 && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)
            {
                bool a = (bool)values["Admin"];
                if (user.IsAdmin && !a)
                {
                    values["Admin"] = true;
                    Tools.ShowMessage("Nie można usunąć sobie uprawnień administratora.");
                    return false;
                }
            }
            bool k = (bool)values["Kierownik"];
            if (k && (int)status == App.stPomin && !AppUser.HasRight(rights, AppUser.rSuperuser))
            {
                Tools.ShowMessage("Nie można pominąć kierownika.");
                return false;
            }

            //----- kadryid, login i email -----
            string pid = oldValues != null ? ((ListViewUpdateEventArgs)ea).Keys[0].ToString() : null;  // albo jak wyżej: if (lvPracownicy.EditIndex >= 0) && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)            
            string nrew = Tools.GetStr(values["KadryId"]);
            string login = Tools.GetStr(values["Login"]);
            string email = Tools.GetStr(values["Email"]);
            //-----
            string e = db.getScalar(String.Format(@"
select Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') + case when Status = -1 then ' - zwolniony' else '' end 
from Pracownicy where KadryId = '{0}' and ({1} is null or Id != {1})", nrew, db.nullParam(pid)));
            if (!String.IsNullOrEmpty(e))
            {
                Tools.ShowError("Numer ewidencyjny jest już przypisany do pracownika:\\n{0}.", e, nrew);
                return false;
            }
            //-----
            if (String.IsNullOrEmpty(login)) values["Login"] = String.Format("login_{0}", nrew);
            else
            {
                e = db.getScalar(String.Format(@"
select Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') + case when Status = -1 then ' - zwolniony' else '' end 
from Pracownicy where Login = '{0}' and ({1} is null or Id != {1})", login, db.nullParam(pid)));
                if (!String.IsNullOrEmpty(e))
                {
                    Tools.ShowError("Login [{1}] jest już przypisany do pracownika:\\n{0}.", e, login);
                    return false;
                }
            }
            //-----
            if (String.IsNullOrEmpty(email)) values["Email"] = String.Format("email_{0}", nrew);
            else
            {
                e = db.getScalar(String.Format(@"
select Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','') + case when Status = -1 then ' - zwolniony' else '' end 
from Pracownicy where Email = '{0}' and ({1} is null or Id != {1})", email, db.nullParam(pid)));
                if (!String.IsNullOrEmpty(e))
                {
                    Tools.ShowError("Adres email jest już przypisany do pracownika:\\n{0}.", e, email);
                    return false;
                }
            }
            //----------------------------

            //string pid = lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString();
            //Log.LogChanges(Log.PRACOWNIK, "Pracownik: " + AppUser.GetNazwiskoImieNREW(pid), ea);
            return true;
        }

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            SqlDataSource1.UpdateParameters["AutorId"].DefaultValue = App.User.OriginalId;
            e.Cancel = !UpdateItem(lvPracownicy.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvPracownicy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            SqlDataSource1.InsertParameters["AutorId"].DefaultValue = App.User.OriginalId;
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        protected void lvPracownicy_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string pracId = Tools.GetDataKey(lvPracownicy, e);
            DataRow dr = db.getDataRow("select top 1 Data from PlanPracy where IdPracownika = " + pracId);
            if (dr != null)
            {
                Tools.ShowError("Dla pracownika istnieje plan pracy: {0}. Usunięcie niemożliwe.", (DateTime)db.getDateTime(dr, "Data"));
                e.Cancel = true;
            }
        }
        //-----
        protected void lvPracownicy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();

            cntSplityWsp splity = lvPracownicy.EditItem.FindControl("cntSplityWsp2") as cntSplityWsp;
            if (splity != null)
                splity.Update();            
            
            if (lvPracownicy.EditIndex >= 0 && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)
                user.Reload(false);
        }

        protected void lvPracownicy_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
            lvPracownicy.InsertItemPosition = InsertItemPosition.None;
        }
        //-----
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                cntSplityWsp splity = lvPracownicy.InsertItem.FindControl("cntSplityWsp2") as cntSplityWsp;
                if (splity != null)
                {
                    System.Data.Common.DbCommand command = e.Command;
                    //string pracId = command.Parameters["@IdPracownika"].Value.ToString();
                    string przId = command.Parameters["@IdPrzypisania"].Value.ToString();
                    splity.IdPrzypisania = przId;
                    splity.Update();
                }
            }
        }

        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                cntSplityWsp splity = lvPracownicy.EditItem.FindControl("cntSplityWsp2") as cntSplityWsp;
                if (splity != null)
                {
                    if (String.IsNullOrEmpty(splity.IdPrzypisania)) // == "-1")  // jak się okazuje po zainicjowaniu na null
                    {
                        System.Data.Common.DbCommand command = e.Command;
                        string przId = command.Parameters["@IdPrzypisaniaNew"].Value.ToString();
                        if (!String.IsNullOrEmpty(przId))
                            splity.IdPrzypisania = przId;
                        //splity.Update();   // tylko ustawiam - zapis wyżej
                    }
                }
            }
        }



        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Tools.DateOk(args.Value);
        }





        protected void lvPracownicy_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvPracownicy.SelectedIndex = e.NewEditIndex;
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }
        //---------------------------------------
        protected void cntKartyRcpPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
                TextBox tb1,tb2;
/*
#if xxSIEMENS
                tb1 = null;
                tb2 = lvPracownicy.EditItem.FindControl("RcpIdTextBox2") as TextBox;
#else
*/ 
                switch (cntKartyRcp.defMode)
                {
                    case cntKartyRcp.moNoRcpId:
                        tb1 = null;
                        tb2 = lvPracownicy.EditItem.FindControl("RcpIdTextBox2") as TextBox;
                        break;
                    case cntKartyRcp.moNoNrKarty:
                        tb1 = lvPracownicy.EditItem.FindControl("RcpIdTextBox") as TextBox;
                        tb2 = null;
                        break;
                    case cntKartyRcp.moShowAll:
                        tb1 = lvPracownicy.EditItem.FindControl("RcpIdTextBox") as TextBox;
                        tb2 = lvPracownicy.EditItem.FindControl("RcpIdTextBox2") as TextBox;
                        break;
                }

                if (tb1 != null)
                {
                    //DataRow dr = db.getDataRow("select * from PracownicyKarty where "
                    tb1.Text = cntKartyRcpPopup.CurrentRcpId;
                    tb1.ToolTip = null;   // później zmienić ...
                }
                if (tb2 != null)
                {
                    //DataRow dr = db.getDataRow("select * from PracownicyKarty where "
                    tb2.Text = cntKartyRcpPopup.CurrentNrKarty;
                    tb2.ToolTip = null;   // później zmienić ...
                }
            }
        }

        protected void cntAlgorytmyPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
                TextBox tb = Tools.SetTextBox(lvPracownicy.EditItem, "AlgorytmTextBox", cntAlgorytmyPopup.CurrentAlgorytm);
                if (tb != null) tb.ToolTip = cntAlgorytmyPopup.CurrentOdDo;
                /*
                tb = Tools.SetTextBox(lvPracownicy.EditItem, "StrefaTextBox", cntAlgorytmyPopup.CurrentStrefa);
                if (tb != null) tb.ToolTip = cntAlgorytmyPopup.CurrentOdDo;
                */

                TimeEdit te = lvPracownicy.EditItem.FindControl("teWymiar") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.WymiarCzasuSec;
                te = lvPracownicy.EditItem.FindControl("tePrzerwaWliczona") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.PrzerwaWliczonaSec;
                te = lvPracownicy.EditItem.FindControl("tePrzerwaNiewliczona") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.PrzerwaNiewliczonaSec;
            }
        }

        protected void cntStanowiskaPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
                DataRow dr = cntStanowiskaPopup.Current;
                Tools.SetTextBox(lvPracownicy.EditItem, "DzialTextBox", db.getValue(dr, "Dzial"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "StanowiskoTextBox", db.getValue(dr, "Stanowisko"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "GrupaTextBox", db.getValue(dr, "Grupa"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "KlasyfikacjaTextBox", db.getValue(dr, "Klasyfikacja"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "GradeTextBox", db.getValue(dr, "Grade"), null);
            }
        }

        //---------------------------------------
        protected void btPrzesuniecia_Click(object sender, EventArgs e)
        {
            TriggerCommand("przes", SelectedPracId);
        }

        protected void btPlanPracy_Click(object sender, EventArgs e)
        {
            TriggerCommand("plan", SelectedPracId);
        }

        protected void btAkceptacja_Click(object sender, EventArgs e)
        {
            TriggerCommand("ppacc", SelectedPracId);
        }

        //---------------------------------------
        public ListView List
        {
            get { return lvPracownicy; }
        }

        /* pozostałość z pwd
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }
        */
        //-------------------------

        /*
        protected void btJump_Click(object sender, EventArgs e)
        {

        }

        protected void btJump_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "JUMP")
            {
                DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                switch (e.CommandArgument.ToString())
                {
                    case "K":
                        dp.SetPageProperties(100, dp.PageSize, true);        
                        break;
                    case "W":
                        dp.SetPageProperties(300, dp.PageSize, true);
                        break;
                }
            }
        }
         */
        //---------------------------
        public string SelectedPracId
        {
            get 
            {
                if (lvPracownicy.SelectedIndex != -1)
                    return lvPracownicy.SelectedDataKey.Value.ToString();
                else
                    return null;
            }
        }

        public string SelectedRcpId
        {
            get { return hidSelectedRcpId.Value; }
            set { hidSelectedRcpId.Value = value; }
        }

        public string SelectedStrefaId
        {
            get { return hidSelectedStrefaId.Value; }
            set { hidSelectedStrefaId.Value = value; }
        }
        //----------------------------
        public string ListContent
        {
            set 
            {
                if (value != ListContent)
                {
                    Tools.SelectMenu(tabContent, value);
                    lvPracownicy.DataBind();
                }
            }
            get { return tabContent.SelectedValue; }
        }

        protected void SqlDataSourceKierEdit_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void SqlDataSourceKierEdit_DataBinding(object sender, EventArgs e)
        {

        }
        //-----------------------
        protected void cvLogin_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
        }

        protected void cvEmail_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
        }


    }
}
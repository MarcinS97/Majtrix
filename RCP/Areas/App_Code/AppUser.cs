using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.App_Code
{
    public class AppUser
    {
        public const string sesUser             = "user";    // identyfikator do sesji po zalogowaniu przez hasło
        public const string sesUserId           = "userId";  // identyfikator do sesji po lologowaniu, dla zastepstwa - zastepowany
#if PORTAL
        private const string selectUsers        = @"
select P.Id, P.Login, P.Email, P.Mailing, P.Imie, P.Nazwisko, P.Admin, P.Kierownik, P.Raporty, P.KadryId, P.KadryId2, P.Rights, P.Nick, P.Pass, P.RightsPortal, P.LoginType, P.PassExpire
from {0}..Pracownicy P
        ";
        //private const string _selectUsers        = "select Id, Login, Email, Mailing, Imie, Nazwisko, Admin, Kierownik, Raporty, KadryId, KadryId2, Rights, Nick, Pass, RightsPortal, LoginType from HR_PORTAL..Pracownicy";
#else
        private const string _selectUsers        = @"
select P.Id, P.Login, P.Email, P.Mailing, P.Imie, P.Nazwisko, P.Admin, P.Kierownik, P.Raporty, P.KadryId, P.KadryId2, P.Rights, P.Nick, P.Pass, P.PassExpire 
from Pracownicy P
        ";
#endif
        private const string _selectKierParams   = "select IdKierownika, PrzerwaMM, Przerwa2MM, MarginesMM, DataAccDo from KierParams";

        public const string KioskLogin          = "kiosk";   // login użytkownika kiosk



        public const int _rAdmin                 = 0;   // 1 administrator
        public const int rRights                = 1;    // 2 prawo do nadawania uprawnień        
        public const int rTester                = 2;    // 3 <<<<<<<< testy Przesuniec // cc
        public const int rKwitekAdm             = 3;    // 4 <<<<<<<< podglad pracownika z listy

        public const int rBadaniaMailing        = 35;   // maile o kończących sie badaniach do admina

        public const int rRepCzasPracy          = 4;    // 5 raport czasu pracy do testów dla Adama Peplinskiego, tymczas poźniej zmienić 
        public const int rDostepniKierownicy    = 5;    // 6 mozliwosc wyboru kierownika i jego pracowników
        public const int rRepPodzialCC          = 6;    // 7 raport poczuału czasu na cc, uprawnienia do cc i class
        public const int rRepPodzialCCKwoty     = 7;    // 8 raport poczuału czasu na cc z kwotami, uprawnienia do cc i class
        public const int rRepPodzialCCAll       = 8;    // 9 pełen wgląd w dane, bez konieczności przypisywania cc i class - dla adminiów

        public const int rRepESD                = 34;   // 
        //public const int rRepPodzialCCPrac      = 9;  // 10 dostep do podległych pracowników - płaska struktura

        public const int rRaportyAll            = 51;   // dostęp do wszystkich danych raportów Sql
        public const int rRaporty2              = 72;   // raporty dodatkowe

        public const int rKierParams            = 10;   // 11 dostep do konfiguracji czasów przerw
        public const int rPrzesuniecia          = 11;   // 12 przesuwanie pracowników
        public const int rPrzesunieciaAcc       = 12;   // 13 akceptowanie przesunięć pracowników
        public const int rPrzesunieciaAccSub    = 30;   // akceptowanie przesunięć pracowników z całej podstruktury = też info o przesunięciach
        public const int rPrzesunieciaAdm       = 18;   // administracja słownikami i parametrami przesunięc - dla kierownika

        public const int rWnioskiUrlopowe       = 28;   // wypełnianie swoich wniosków urlopowych  <<< tymczas później kązdy będzie mieć dostęp
        //public const int rWnioskiUrlopoweDlaPrac = ;  // wypełnianie wniosków urlopowych dla pracowników - domyslnie każdy przełożony dla swoich ludzi (wszystkich z podstruktury) xxx >>> chyba że sub to wtedy wszystkich widzi
        public const int rWnioskiUrlopoweAcc    = 13;   // 14 akceptowanie wniosków urlopowych
        public const int rWnioskiUrlopoweAdm    = 14;   // 15 administracja wniosków
        public const int rWnioskiUrlopoweSub    = 17;   // 17 dostęp do wniosków z podstruktury / nie, to tylko moi bezpośredni podwładni

        public const int rReadOnly              = 15;
        public const int _rMailing               = 16;
        public const int rSuperuser             = 19;
        public const int rUnlockPP              = 20;   // odblokowywanie dni (cofnięcie akceptacji) w zamkniętym tygodniu, ale nie okresie
        public const int rRepStolowka           = 21;   // raport wykorzystania stołówki
        public const int rRepRezerwaUrlopowa    = 46;   // 


        public const int rKartyTmp              = 22;   // wydawanie kart tymczasowych
        public const int rRozlNadg              = 23;   // rozliczanie nadgodzin
        public const int rRozlNadgPo            = 36;   // rozliczanie nadgodzin - edycja po zamknięciu
        
        public const int rPlanUrlopow           = 24;   // plan urlopów
        public const int rPlanUrlopowSwoj       = 26;   // edycja swojego planu urlopów
        public const int rPlanUrlopowEditPo     = 29;   // plan urlopów - edycja po zamknięciu, korekty wszyscy mogą
        public const int rPlanUrlopowAcc        = 37;   //"PB - Akceptacja plau urlopów"}                              // <<<<<<<<<<<<<<<<<<<<<<< jeszcze nie ma 

        public const int rPlanPracy             = 96;   // edycja planu pracy
        public const int rPlanPracyAcc          = 97;   // akceptacja planu pracy
        public const int rPlanPracySwoj         = 27;   // podgląd swojego planu pracy

        public const int rPortalAdmin           = 25;   // portal
        public const int rPortalArticles        = 32;   // portal administracja artykułami, plikami i gazetką
        public const int rPortalTmp             = 31;   // portal dostęp


        public const int rWnioskiUrlopoweNoAccMail = 33;

        public const int rPodzialLudzi          = 38;   // podgląd podziału
        public const int rPodzialLudziAdm       = 39;   // dostęp do wszystkiego, import splitów 
        public const int rPodzialLudziPM        = 40;   // Program Manager - ma dostep do raportów

        public const int rPodzialLudziPMZoom    = 66;   // Program Manager - ma dostep do raportów ZOOM

        public const int rPodzialLudziEditS     = 41;   // edycja splitów, 
        public const int rPodzialLudziEditCAP   = 42;   // edycja comm, area, pos ???
        public const int rPodzialLudziEditSGrupy= 43;   // edycja splitów 019 itp
        public const int rPLMailingPrzesCC      = 44;   // mailing o przesunięciach na cc 
        
        public const int rHideSalary            = 47;   // ukryj wynagrodzenie danej osoby w PodzialeLudzi - podział na cc kolumna Brutto
        public const int rShowHiddenSalary      = 48;   // podgląd ukrytych wynagrodzeń, głównie admin

        public const int rInfokiosk             = 45;   // uzytkownik infokiosk

        public const int rIPOGlobalAdmin        = 49;

        public const int rEditCCLim             = 50;   // edycja Limitow CC
        public const int rRaportCCLim           = 52;   // dostęp do raportu Limitow CC zgodnie z uprawnieniami PM z Podziału Ludzi oraz wysyłka maila z powiadomieniem
        public const int rRaportCCLimMailing    = 62;   // mailing dla raportu

        public const int rRaportIlDL            = 63;   // raport ilości DL - tymczasowe !!! po testach dać dostęp jak jest się PM !!! 
        

        public const int rBadaniaWstepne        = 53;   // arkusz pracowników skierowanych na badania wstępne
        public const int rBadaniaWstepneAddDel  = 61;   // arkusz pracowników skierowanych na badania wstępne - dodawanie i usuwanie pracowników
        public const int rRekruter              = 64;   // 
        public const int rOpiekun               = 67;   // 

        public const int rSzkoleniaBHP          = 54;   // szkolenia BHP (podgląd - tylko w ramach podległości)
        public const int rSzkoleniaBHPAdm       = 55;   // szkolenia BHP Administracja (wprowadzanie)
        public const int rSzkoleniaBHPMailing   = 71;   // szkolenia BHP Mailing (Admin - wszystko, wg podległości)

        public const int _rScorecardsAdmin       = 56;   // Admin Scorecards
        public const int rScorecardsTLProd      = 57;   // TL
        public const int rScorecardsTLNieprod   = 65;   // TL
        public const int rScorecardsKier        = 58;   // Kierownik
        public const int rScorecardsZarz        = 59;   // Zarząd
        public const int rScorecardsWnAcc       = 60;   // akceptacja wniosków
        public const int rScorecardsHR          = 68;   // dostęp dla HR
        public const int rScorecardsControlling = 73;   // dostęp dla Kontrolingu
        //public const int rScorecardsKsiegowosc  = ;   // dostęp dla Ksiegowosci <<< na razie nie
        public const int _rScorecardsKwoty       = 69;   // raporty - podgląd kwot
        public const int _rScorecardsZoom        = 70;   // raporty <<<<< chyba nie jest wykorzystywane
        public const int rScorecardsWnRej       = 74;   // deakceptacja arkuszy
        public const int rScorecardsWnDstr      = 75;   // odrzucanie wniosków
        public const int rScorecardsWysPowAdm   = 76;   // wysyłka powiadomień administracyjnych 
        public const int rScorecardsArkAll      = 77;   // dostęp do wszystkich arkuszy

        public const int rScorecardsAdminOdczyty = 129;

        public const int rAbsencjeDlugotrwale   = 78;   // dostęp do absencji długotrwałych

        public const int rESD                   = 79;   // ESD Sivnatos

        public const int rMSAdmin               = 80;
        public const int rMSMeister             = 81;
        public const int rMSTrener              = 82;
        public const int rMSKorekty             = 83;
        public const int rMSKorektyAcc          = 84; // Korekty ocen
        public const int rMSCertyfikatyAcc      = 85;

        public const int rMSAnkietyPodgladP     = 86;
        public const int rMSAnkietyPodgladK     = 87;
        public const int rMSAnkietyEdycjaP      = 88;
        public const int rMSAnkietyEdycjaK      = 89;
        public const int rMSAnkietyAcc          = 90;

        public const int rMSPracownicyAdm       = 91;
        public const int rMSPracownicyProd      = 92;
        
        public const int rMSBHP                 = 93;
        public const int rMSHR                  = 94;

        public const int rMSWozki               = 95;

        public const int rWnNadgAccAll = 98;   // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< konflikt !!!!!!!!!!!!!!!!!!!!!!!!!!!


        public const int rAdminKwoty = 99;   // RCP - podgląd kwot


#if IQOR
        public const int rOgloszeniaAdm         = 89;   // uwaga !!! wchodzi na matrycę szkoleń !!!
        public const int rOgloszeniaWyst        = 90;   // uwaga !!! wchodzi na matrycę szkoleń !!! <<<< w kodzie teraz wszyscy mogą
        public const int rOgloszeniaBlokada     = 91;   // uwaga !!! wchodzi na matrycę szkoleń !!!
        public const int rUbezpieczeniaAdm      = 92;

        public const int rZastWithAllRights     = 98;  // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<< konflikt !!!!!!!!!!!!!!!!!!!!!!!!!!!
#else
        public const int rOgloszeniaAdm         = 111;   // uwaga !!! w starym 89 <<< remapować!
        public const int rOgloszeniaWyst        = 112;   // uwaga !!! w starym 90 <<< remapować!, do zastanowienia czy w ogóle je dawać, ustawione na sztywno na true teraz
        public const int rOgloszeniaBlokada     = 113;   // uwaga !!! w starym 91 <<< remapować!


        public const int rUbezpieczeniaAdm      = 114;

        public const int rZastWithAllRights     = 115;
#endif

        public const int rWnioskiZdalna         = 116; // czy może wnioskować
        public const int rWnioskiZdalnaAcc      = 117;
        public const int rWnioskiZdalnaPodleg   = 118; // czy podlega

        public const int rOpieka188Godziny      = 119; // czy może wnioskować o godzinową
        public const int rPodzialLudziKalkulator = 120; // 

        public const int rScorecardsOverride    = 121;

        public const int rMSBadaniaPodglad      = 122;

        public const int rPremieAdmin           = 123;

        public const int rKomputer              = 124;

        public const int rDanePersonalne        = 125;
        public const int rPortalLoginAllowed    = 126;  // mogę zalogować się do danych spersonalizowanych na innego użytkownika niż przekazany z Loginu
        public const int rPlanPracyAccMimoBl    = 127;  // mogę zalogować się do danych spersonalizowanych na innego użytkownika niż przekazany z Loginu
        public const int rUrlopKonczacyAcc      = 128;  // dla CO jeżeli kier ma to uprawnienie, to nie jest wymagana acc poziom wyżej, jeżeli nie to ten i poziom następny
        public const int maxRight               = 128;   // UWAGA !!! ustawić na ostatni, max 100 !!!

        //----- SCORECARDS -----
        public static int[] rScorecardsRestricted = { _rScorecardsAdmin, 
                                               rScorecardsHR, 
                                               rScorecardsControlling, 
                                               _rScorecardsKwoty, 
                                               _rScorecardsZoom
                                             };

        //----- PORTAL -----
        public const int x_rPortal               = 0;    // dostęp do portalu


        //public static string[] RightName = {
        public static readonly string[] _RightName = {
            "Administracja",                                        // rAdmin                 = 0; // administrator
            "Nadawanie uprawnień",                                  // rRights                = 1; // prawo do nadawania uprawnień        
            "Testowanie",                                           // _rTester               = 2; //<<<<<<<< testy cc
            "Administracja kwitkiem płacowym",                      // rKwitekAdm             = 3; //<<<<<<<< podglad pracownika z listy

            "Raport czasu pracy RCP",                               // rRepCzasPracy          = 4; // raport czasu pracy do testów dla Adama Peplinskiego, tymczas poźniej zmienić 
            "Dostęp do obcych pracowników",                         // rDostepniKierownicy    = 5; // mozliwosc wyboru kierownika i jego pracowników
            "Raporty podziału czasu na cc",                         // rRepPodzialCC          = 6; // raport poczuału czasu na cc, uprawnienia do cc i class
            "Raporty podziału czasu na cc - kwoty",                 // rRepPodzialCCKwoty     = 7; // raport poczuału czasu na cc z kwotami, uprawnienia do cc i class
            "Raporty podziału czasu na cc - cała klasyfikacja",     // rRepPodzialCCAll       = 8; // pełen wgląd w dane, bez konieczności przypisywania cc i class - dla adminiów
                                          
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        //----- wartości - nie ma/ma prawo -----
        public const char chNoRight     = '0';
        public const char chHasRight    = '1';

        //--------------------------------------
        private string FLogin = null;               
        private DataRow drData = null;

        private string FOriginalLogin = null;           // wartosci są null jezeli nie jest to zastępstwo !!!
        private string FOriginalImieNazwisko = null;    // do info Witaj
        private string FOriginalNazwiskoImie = null;
        private string FOriginalImie = null;            // do okienek spersonalizowanych
        private string FOriginalId = null;
        private string _FOriginalRights = null;
#if R2
        private string FMergedRights = null;
        private bool? FOriginalIsAdmin = null;
        private bool? FOriginalIsRaporty = null;
#endif

        public AppUser(int no)
        {
            LogLogin(no);
        }

        public static AppUser CreateOrGetSession(string sesName)
        {
            if (HttpContext.Current.Session[sesName] == null)
            {
                AppUser u = new AppUser(1);
                HttpContext.Current.Session[sesName] = u;
                return u;
            }
            else
            {
                AppUser u = (AppUser)HttpContext.Current.Session[sesName];
                // sprawdzenie na wywalanie sesji
                if (String.IsNullOrEmpty(u.Id))
                {
                    u = new AppUser(2);
                    u.Reload(true);
                    HttpContext.Current.Session[sesName] = u;
                }
                return u;
            }
        }

        public static AppUser CreateOrGetSession()
        {
            return CreateOrGetSession(sesUser);
        }

        //----------------------------------------------
        public static string GetLogin()
        {
            String user = HttpContext.Current.User.Identity.Name;

            #if TOMEKW
            if (String.IsNullOrEmpty(user))     // debug !!!
                user = "tomekw";  
            #endif
    
            if (!String.IsNullOrEmpty(user))
            {
                int i = user.LastIndexOf('\\');     // KOMPUTER\\User -> \User -> User
                if (i >= 0)
                    user = user.Remove(0, i + 1);
            }



            #if JABIL
            if (user == "tomekw") 
                user = "WojciowT";
                //user = "zabiegld";
            #endif

            

            
            return user;
        }
        //----------------------------------------------
        public void Reload(bool withLogin)
        {
            if (withLogin)
                FLogin = null;
            drData = null;
            drData = getData();  //2012-06-22 od razu, Punkty sie bez teg wywalaly, moze poprawi wywalanie sesji
        }
        //----------------------------------------------
        public bool HasRight(int right)
        {
            return HasRight(Rights, right);
        }

        public bool HasOriginalRight(int right)
        {
            return HasRight(OriginalRights, right);
        }

        public bool _HasRightPortal(int right)
        {
            return HasRight(_RightsPortal, right);
        }

        public static bool HasRight(string rights, int right)
        {
            if (!String.IsNullOrEmpty(rights) && rights.Length > right)
            {
                char r = rights.ToCharArray()[right];
                return r == chHasRight;
            }
            else
                return false;
        }

        public static string GetRightName(int right)
        {
            if (0 <= right && right < _RightName.Length)
                return _RightName[right];
            else
                return null;
        }

        public static string SetRights(string rights, string rightsOriginal, params int[] rlist)
        {
            char[] ra = rights.ToCharArray();
            foreach (int r in rlist)
            {
                if (r >= 0 && r < ra.Length)   // zeby mozna bylo -1 przekazac jak ma być skip
                    if (r < rightsOriginal.Length)
                        ra[r] = rightsOriginal[r];
                    else
                        ra[r] = '0';
            }
            return new string(ra);
        }

        //----------------------------------------------
        public string MergeRights(string userRights, string myRights, params int[] keepRights)   // zastępuję usera i zmieniam sobie prawa na jego = muszę mieć te same prawa, żeby w pełni zastapić
        {
            if (userRights == null) userRights = "";
            if (myRights == null) myRights = "";
            int len1 = userRights.Length;
            int len2 = myRights.Length;
            int len = len1 > len2 ? len1 : len2;
            char[] ret = new char[len];
            for (int i = 0; i < len; i++)
            {
                //int rr = userRights[i];  0-48 / 1-49
                char r1 = i < len1 ? userRights[i] : '0';

                if (keepRights != null)
                    for (int k = 0; k < keepRights.Length; k++)
                        if (keepRights[k] == i)
                        {
                            r1 = '1';
                            break;
                        }

                char r2 = i < len2 ? myRights[i] : '0';
                ret[i] = r1 == '1' && r2 == '1' ? '1' : '0';    // funkcja and
            }
            return new string(ret);
        }
        //----------------------------------------------


        //----------------------------------------------
        public void LoginAsUser(string login)
        {
            if (String.IsNullOrEmpty(login) ||
                !String.IsNullOrEmpty(FOriginalLogin) && login.ToUpper() == FOriginalLogin.ToUpper()) // przywracam
            {
                FOriginalLogin = null;
                FOriginalImieNazwisko = null;
                FOriginalNazwiskoImie = null;
                FOriginalImie = null;
                FOriginalId = null;
                _FOriginalRights = null;
                string lll = Login;
                Login = GetLogin();     // tu się zrobi Reload
                Log.Info(Log.ZASTEPSTWO, null, 0, "Zakończenie zastępstwa", String.Format("{0} -> {1}", Login, lll), Log.OK);
            }
            else
            {
                string lll = GetLogin();
                if (String.IsNullOrEmpty(FOriginalLogin) || FOriginalLogin != lll)
                {
                    FOriginalLogin = lll;
                    FOriginalImieNazwisko = ImieNazwisko;
                    FOriginalNazwiskoImie = NazwiskoImie;
                    FOriginalImie = Imie;
                    FOriginalId = Id;
                    _FOriginalRights = Rights;
                }
                Login = login;          // tu się zrobi Reload
                Log.Info(Log.ZASTEPSTWO, null, 1, "Rozpoczęcie zastępstwa", String.Format("{0} -> {1}", lll, login), Log.OK);
            }
        }

        public void _LoginAsUserId(string id)
        {
            if (String.IsNullOrEmpty(id) ||
                !String.IsNullOrEmpty(FOriginalId) && id == FOriginalId) // przywracam
            {
                FOriginalLogin = null;
                FOriginalImieNazwisko = null;
                FOriginalNazwiskoImie = null;
                FOriginalImie = null;
                //FOriginalId = null;
                _FOriginalRights = null;
                string iii = Id;
                drData = GetData(FOriginalId);
                
                Log.Info(Log.ZASTEPSTWO, null, 0, "Zakończenie zastępstwa ID", String.Format("{0} -> {1}", iii, Id), Log.OK);
            }
            else
            {
                if (String.IsNullOrEmpty(FOriginalId) || FOriginalId != id)
                {
                    FOriginalImieNazwisko = ImieNazwisko;
                    FOriginalNazwiskoImie = NazwiskoImie;
                    FOriginalLogin = Login;
                    FOriginalImie = Imie;
                    FOriginalId = Id;
                    _FOriginalRights = Rights;

                    drData = GetData(id);
                }
                Log.Info(Log.ZASTEPSTWO, null, 1, "Rozpoczęcie zastępstwa ID", String.Format("{0} -> {1}", id, FOriginalId), Log.OK);
            }
        }







        public void LoginAsUserId_Portal(string id)
        {
            if (String.IsNullOrEmpty(id) ||
                !String.IsNullOrEmpty(FOriginalId) && id == FOriginalId) // przywracam
            {
                FOriginalLogin = null;
                FOriginalImieNazwisko = null;
                FOriginalNazwiskoImie = null;
                FOriginalImie = null;
                //FOriginalId = null;
                _FOriginalRights = null;

                string iii = Id;
                drData = GetData(FOriginalId);

                Log.Info(Log.ZASTEPSTWO, null, 0, "Zakończenie zastępstwa ID", String.Format("{0} -> {1}", iii, Id), Log.OK);
            }
            else
            {
                if (String.IsNullOrEmpty(FOriginalId) || FOriginalId != id)
                {
                    FOriginalImieNazwisko = ImieNazwisko;
                    FOriginalNazwiskoImie = NazwiskoImie;
                    FOriginalLogin = Login;
                    FOriginalImie = Imie;
                    FOriginalId = Id;
                    _FOriginalRights = Rights;

                    drData = GetData(id);



                    Login = db.getValue(drData, "Login");
                }
                Log.Info(Log.ZASTEPSTWO, null, 1, "Rozpoczęcie zastępstwa ID", String.Format("{0} -> {1}", id, FOriginalId), Log.OK);
            }
        }

        public void LoginAsUserId2(string id)  // nadpisuje FLogin co powinno mieć miejsce...
        {
            if (String.IsNullOrEmpty(id) ||
                !String.IsNullOrEmpty(FOriginalId) && id == FOriginalId) // przywracam
            {
                FOriginalLogin = null;
                FOriginalImieNazwisko = null;
                FOriginalNazwiskoImie = null;
                FOriginalImie = null;
                //FOriginalId = null;
                _FOriginalRights = null;
                string iii = Id;
                drData = GetData(FOriginalId);

                Log.Info(Log.ZASTEPSTWO, null, 0, "Zakończenie zastępstwa ID 2", String.Format("{0} -> {1}", iii, Id), Log.OK);
            }
            else
            {
                if (String.IsNullOrEmpty(FOriginalId) || FOriginalId != id)
                {
                    if (IsOriginalUser)
                    {
                        FOriginalImieNazwisko = ImieNazwisko;
                        FOriginalNazwiskoImie = NazwiskoImie;
                        FOriginalLogin = Login;
                        FOriginalImie = Imie;
                        FOriginalId = Id;
                        _FOriginalRights = Rights;
                    }
                    drData = GetData(id);
                    FLogin = db.getValue(drData, "Login");
                }
                Log.Info(Log.ZASTEPSTWO, null, 1, "Rozpoczęcie zastępstwa ID 2", String.Format("{0} -> {1}", id, FOriginalId), Log.OK);
            }
        }
        
        //----------------------------------------------
        private const string ses_user = "_passuser_";
        public const string hashMethod = "sha1";  // "md5"

        public bool _CheckPassLogin(string login, string pass, bool dologin)
        {
            Reload(true);
            if (!String.IsNullOrEmpty(login))
            {
                if (login != Nick)
                {
                    string l = Login;
                    string ni = NazwiskoImie;
                    string id = Id;
                    string nick = Nick;
                    DataRow dr = GetDataNick(login);
                    if (dr != null)
                    {
                        drData = dr;
                        HttpContext.Current.Session[sesUser] = this;

                        string n = Nick;
                        if (!String.IsNullOrEmpty(n))
                        {
                            int p = n.Length - 4;
                            n = n.Substring(p < 0 ? 0 : p);
                        }
                        Log.Info(Log.PRACLOGIN, App.APP + "Logowanie pracownika z innego loginu", String.Format("ID: {0}, {1}, (...{2}), (ID: {3}, login: {4}, prac: {5})", Id, NazwiskoImie, n, id, l, ni), Log.OK);
                    }
                    else
                    {
                        Log.Info(Log.PRACLOGIN, App.APP + "Brak użytkownika o podanym loginie", "Login: " + login, Log.OK);
                        return false;     // nie ma usera o nicku
                    }
                }
                else
                {
                    string n = Nick;
                    if (!String.IsNullOrEmpty(n))
                    {
                        int p = n.Length - 4;
                        n = n.Substring(p < 0 ? 0 : p);
                    }
                    Log.Info(Log.PRACLOGIN, App.APP + "Logowanie pracownika", String.Format("ID: {0}, {1}, (...{2})", Id, NazwiskoImie, n), Log.OK);
                }

                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pass, hashMethod);
                bool ok = hash == PassHash;
                if (dologin)
                    if (ok)
                        DoPassLogin();
                    else
                        DoPassLogout();  //na wszelki wypadek

                //-------------------------
                if (!ok)   // 20150217 zmiany dla Piotra logowanie PESEL zostawiało użytkownika
                {
                    drData = null;
                    HttpContext.Current.Session[sesUser] = this;
                }
                //-------------------------

                return ok;
            }
            return false;
        }







        public bool CheckPassLoginTest(string kadryId)
        {
            DataRow dr = GetDataKadryId(kadryId);
#if R2
            FMergedRights = null;
#endif
            if (dr != null)
            {
                drData = dr;
                HttpContext.Current.Session[sesUser] = this;
                DoPassLogin();
                return true;
            }
            return false;
        }






        
        
        public void DoPassLogin()
        {
            HttpContext.Current.Session[ses_user] = Id;
        }

        public void DoPassLogout()
        {
            HttpContext.Current.Session[ses_user] = null;
        }

        public bool IsPassLogged
        {
            get
            {
                object u = HttpContext.Current.Session[ses_user];
                if (u != null)
                {
                    string sid = u.ToString();
                    return sid == Id || sid == OriginalId;
                }
                else
                    return false;
            }
        }

        public bool IsPassLogged_VC   //dla VC
        {
            get
            {
                object u = HttpContext.Current.Session[ses_user];
                if (u != null)
                    //return u.ToString() == Id;
                    return true;
                else
                    return false;
            }
        }

        public bool NeedPassChange()
        {
            DateTime? exd = PassExpire;
            if (exd != null)
            {
                DateTime dt = (DateTime)exd;
                return dt <= DateTime.Now;
            }
            return false;
        }

        public static bool UpdatePass(string id, string newPass, bool encrypt)
        {
            try
            {
                string hash;
                if (String.IsNullOrEmpty(newPass))
                    hash = null;
                else
                    if (encrypt)
                        hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass, hashMethod);
                    else
                        hash = newPass;
#if SPX
                int i = Tools.StrToInt(id, 0);
                string dbMatryca = "SPX_Matryca2";
                if (i < 0)
                    return db.update(String.Format("{0}..Przelozeni", dbMatryca), 0, "Pass", "Id=" + (-i).ToString(), db.nullStrParam(hash));
                else
                    return db.update(String.Format("{0}..Pracownicy", dbMatryca), 0, "Pass", "Id=" + id, db.nullStrParam(hash));
#else
#if VICIM || VC
                db.execSQL(String.Format("update Pracownicy set Nick = Login where Id = {0}", id));
                return db.update("Pracownicy", 0, "Pass,PassExpire", "Id=" + id, db.nullStrParam(hash), db.NULL);   // bez terminu albo data nastepnej zmiany
#else
                return db.update("Pracownicy", 0, "Pass,PassExpire", "Id=" + id, db.nullStrParam(hash), db.NULL);   // bez terminu albo data nastepnej zmiany
#endif
#endif
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdatePass(string id, string newPass, bool encrypt, SqlConnection sq)
        {
            try
            {
                string hash;
                if (String.IsNullOrEmpty(newPass))
                    hash = null;
                else
                    if (encrypt)
                        hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass, hashMethod);
                    else
                        hash = newPass;
#if SPX
                int i = Tools.StrToInt(id, 0);
                string dbMatryca = "SPX_Matryca2";
                if (i < 0)
                    return db.update(String.Format("{0}..Przelozeni", dbMatryca), 0, "Pass", "Id=" + (-i).ToString(), db.nullStrParam(hash));
                else
                    return db.update(String.Format("{0}..Pracownicy", dbMatryca), 0, "Pass", "Id=" + id, db.nullStrParam(hash));
#else
#if VICIM || VC
                db.execSQL(String.Format("update Pracownicy set Nick = Login where Id = {0}", id));
                return db.update("Pracownicy", 0, "Pass,PassExpire", "Id=" + id, db.nullStrParam(hash), db.NULL);   // bez terminu albo data nastepnej zmiany
#else
                return db.update(sq, "Pracownicy", 0, "Pass,PassExpire", "Id=" + id, db.nullStrParam(hash), db.NULL);   // bez terminu albo data nastepnej zmiany
#endif
#endif
            }
            catch
            {
                return false;
            }
        }



        /*
        private const string ses_user = "sppuser";

        public void DoLogin()
        {
            HttpContext.Current.Session[ses_user] = FLogin;
        }

        public static void Logout()
        {
            HttpContext.Current.Session[ses_user] = null;
        }

        public bool IsLogged
        {
            get
            {
                object u = HttpContext.Current.Session[ses_user];
                if (u != null)
                    return u.ToString() == FLogin;
                else
                    return false;
            }
        }
        //----------------------------------------------
        public void Reload(bool withLogin)
        {
            if (withLogin)
                FLogin = null;
            drData = null;
            drData = getData();
        }

        public bool PassOk(string pass)
        {
            //string hash = pass;
            if (String.IsNullOrEmpty(pass))
                return String.IsNullOrEmpty(PassHash);
            else
            {
                string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pass, hashMethod);
                return PassHash == hash;
            }
        }

        public bool UpdatePass(string newPass)
        {
            bool ok = UpdatePass(Id, newPass);
            if (ok) Reload(false);
            return ok;
        }

        public static bool UpdatePass(string id, string newPass)
        {
            //string hash = newPass;
            try
            {
                string hash;
                if (String.IsNullOrEmpty(newPass))
                    hash = null;
                else
                    hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass, hashMethod);
                return db.execSQL(db.updateSql("Pracownicy", 0, "PassHash", "Id=" + id, db.nullStrParam(hash)));
            }
            catch
            {
                return false;
            }
        }

        public static bool ResetPass(string id)
        {
            Log.Info("Resetowanie hasła", String.Format("{0} ({1})", AppUser.GetNazwiskoImie(id), id));
            return UpdatePass(id, null);
        }
         */ 
        //----------------------------------------------
        public static DataRow GetData(string pracId)
        {
#if PORTAL
            return Base.getDataRow(String.Format(selectUsers, App.dbPORTAL) + " where Id = " + pracId);
#else
            return Base.getDataRow(_selectUsers + " where P.Id = " + pracId);
#endif
        }

        public static DataSet GetDataDs(string pracId)
        {
#if PORTAL
            return Base.getDataSet(String.Format(selectUsers, App.dbPORTAL) + " where Id = " + pracId);
#else
            return Base.getDataSet(_selectUsers + " where P.Id = " + pracId);
#endif
        }

        public static DataRow GetDataNick(string nick)
        {
#if PORTAL
            return Base.getDataRow(String.Format(selectUsers, App.dbPORTAL) + " where Nick = " + db.strParam(nick));
#else
            return Base.getDataRow(_selectUsers + " where P.Nick = " + db.strParam(nick));
#endif
        }

        public static DataRow GetDataKadryId(string kadryId)
        {
#if PORTAL
            return Base.getDataRow(String.Format(selectUsers, App.dbPORTAL) + " where KadryId = " + db.strParam(kadryId));
#else
            return Base.getDataRow(_selectUsers + " where P.KadryId = " + db.strParam(kadryId));
#endif
        }

        private DataRow getData()
        {
            if (drData == null && !String.IsNullOrEmpty(Login))
            {
#if PORTAL
                drData = Base.getDataRow(
                    String.Format(selectUsers, App.dbPORTAL) + " where UPPER(Login) = " +
                    Base.strParam(Login.ToUpper())
                    //Base.strParam(GetLogin().ToUpper())
                    );
#else
                drData = Base.getDataRow(
                    _selectUsers + " where UPPER(P.Login) = " +
                    Base.strParam(Login.ToUpper())
                    //Base.strParam(GetLogin().ToUpper())
                    );
#endif
#if R2
                FMergedRights = null;   // żeby zrobił merge uprawnień
#endif
            }
            return drData;
        }

        /*
        private DataRow getData()
        {
            if (drData == null)
                drData = Base.getDataRow(
                    "select Id, Login, Email, Imie, Nazwisko, Email, Checked, Operator, Kontroler1, Kontroler2, Admin from Pracownicy P " +
                    "where UPPER(Login) = " + 
                    Base.strParam(Login.ToUpper())
                    //Base.strParam(GetLogin().ToUpper())
                    );
            return drData;
        }
         */
        public static string GetNazwiskoImie(string pracId)
        {
            DataRow dr = GetData(pracId);
            if (dr != null)
                return Base.getValue(dr, "Nazwisko") + " " + Base.getValue(dr, "Imie");
            else
                return null;
        }

        public static string GetNazwiskoImieNREW(string pracId)
        {
            DataRow dr = GetData(pracId);
            return GetNazwiskoImieNREW(dr, pracId);
        }

        public static string GetNazwiskoImieNREW(DataRow dr, string pracId)
        {
            if (dr != null)
                return String.Format("{0} {1} ({2})", Base.getValue(dr, "Nazwisko"), Base.getValue(dr, "Imie"), Base.getValue(dr, "KadryId"));
            else
                return null;
        }

        public static bool GetIsKierownik(string pracId)
        {
            DataRow dr = GetData(pracId);
            if (dr != null)
                return Base.getBool(dr, "Kierownik", false);
            else
                return false;
        }
        //----------------------------------------------
        private static string GetUserIP()
        {
            string host = HttpContext.Current.Request.UserHostName;
            string ip = HttpContext.Current.Request.UserHostAddress;
            string hostip = host == ip ? host : host + " (" + ip + ")";
            return hostip;
        }

        private static string GetSesID()
        {
            string sesID = HttpContext.Current.Session.SessionID;
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(sesID, "md5");
            return hash;
        }

        public static string UserIP
        {
            set { HttpContext.Current.Session[App.ID1 + "userIP"] = value; }
            get
            {
                object o = HttpContext.Current.Session[App.ID1 + "userIP"];
                if (o == null)
                {
                    string hostip = GetUserIP();
                    UserIP = hostip;
                    return hostip;
                }
                else
                    return o.ToString();
            }
        }

        public static string SesID
        {
            set { HttpContext.Current.Session[App.ID1 + "sesID"] = value; }
            get
            {
                object o = HttpContext.Current.Session[App.ID1 + "sesID"];
                if (o == null)
                {
                    string sid = GetSesID();
                    SesID = sid;
                    return sid;
                }
                else
                    return o.ToString();
            }
        }

        //private void LogLogin(int no)
        public void LogLogin(int no)
        {
            Uri r = HttpContext.Current.Request.UrlReferrer;
            string browser = String.Format("{0} {1}",
                                           HttpContext.Current.Request.Browser.Browser,
                                           HttpContext.Current.Request.Browser.Version);
            string uip = GetUserIP();
            string info = (r != null ? r.ToString() + " " + (char)13 : "") +
                          uip + String.Format(" [{0}]", Login) + (char)13 +
                          browser;
            UserIP = uip;
            SesID = GetSesID();

            Log.Info(Log.LOGIN, App.APP + "Logowanie użytkownika " + no.ToString(), info);
        }
        //----------------------------------------------
        public static string GeneratePass(int typ)
        {
            switch (typ)
            {
                case 1:
                    return db.getScalar("select dbo.GeneratePass2(8,1,1,1,0)");   // len, lower, upper, num, special
                default:
                    return db.getScalar("select dbo.GeneratePass2(10,1,1,1,1)");
            }
        }

        public static bool CheckPass(string pass, int typ)
        {
            int minlen, mindig, minup;
            switch (typ)
            {
                case 1:
                    minlen = 8;
                    break;
                default:
                    minlen = 10;
                    break;
            }
            if (String.IsNullOrEmpty(pass) || pass.Trim().Length == 0)
                Tools.ShowMessage("Hasło nie może byc puste.");
            else if (pass.Length < minlen)
                Tools.ShowMessage2(String.Format("Hasło zbyt krótkie. Minimalna długość hasła to {0} znaków.", minlen));   // zeby sie odswiezylo dlatego ShowMessage2 
            else
                return true;
            return false;                 
        }

        public static string GatPassComplexityMsg(int typ)
        {
            const string p = "Minimalna długość hasła to {0} znaków.";
            switch (typ)
            {
                case 1:
                    return String.Format(p, 8);
                default:
                    return String.Format(p, 10);
            }
        }


        //----------------------------------------------
        public DataRow Data
        {
            get { return getData(); }
        }

        public string Login
        {
            get 
            {
                if (FLogin == null)
                {
                    FLogin = GetLogin();
                    //----- -----
                    //LogLogin();



                    //AppUser uu = (AppUser)HttpContext.Current.Session[sesUser]; //spr


                }
                return FLogin;
            }
            set 
            { 
                FLogin = value;
                Reload(false);
            }
        }
        //-----
        public bool IsOriginalUser
        {
            get { return String.IsNullOrEmpty(FOriginalLogin) || FOriginalLogin.ToUpper() == Login.ToUpper(); }
        }

        public string OriginalLogin
        {
            get { return IsOriginalUser ? Login : FOriginalLogin; }
        }

        public string OriginalId
        {
            get { return IsOriginalUser ? Id : FOriginalId; }
        }

        public string OriginalImieNazwisko
        {
            get { return IsOriginalUser ? ImieNazwisko : FOriginalImieNazwisko; }
        }

        public string OriginalImie
        {
            get { return IsOriginalUser ? Imie : FOriginalImie; }
        }

        public string OriginalNazwiskoImie
        {
            get { return IsOriginalUser ? NazwiskoImie : FOriginalNazwiskoImie; }
        }

        public string OriginalRights
        {
            get { return IsOriginalUser ? Rights : _FOriginalRights; }
        }
        //-----
        public string EMail
        {
            get { return Base.getValue(Data, "Email"); }
        }

        public string Id
        {
            get { return Base.getValue(Data, "Id");}
        }

        public string ImieNazwisko
        {
            get { return Base.getValue(Data, "Imie") + " " + Base.getValue(Data, "Nazwisko"); }
        }

        public string NazwiskoImie
        {
            get { return Base.getValue(Data, "Nazwisko") + " " + Base.getValue(Data, "Imie"); }
        }

        public string ImieNazwiskoOrLogin
        {
            get 
            {
                string name = ImieNazwisko.Trim();  // spacja rozdzielająca ;)
                if (String.IsNullOrEmpty(name))
                    return "<b>[" + Login + "]</b>";
                else 
                    return Tools.PrepareName(name);
            }
        }

        public string Imie
        {
            get { return Base.getValue(Data, "Imie"); }
        }

        public string Nazwisko
        {
            get { return Base.getValue(Data, "Nazwisko"); }
        }
        //-----
        public string NR_EW
        {
            get { return Base.getValue(Data, "KadryId"); }
        }

        public string KadryId2
        {
            get { return Base.getValue(Data, "KadryId2"); }
        }
        //-----
        public string Nick
        {
            get { return Base.getValue(Data, "Nick"); }
        }

        public string PassHash
        {
            get { return Base.getValue(Data, "Pass"); }
        }
        //-----
        public bool HasAccess  //nie znaleziony po Loginie, czyli moze byc !first login 
        { 
            get { return Data != null; }  // za każdym odwołaniem będzie sprawdzać ...
        }

        public bool IsKierownik
        { 
            get { return Base.getBool(Data, "Kierownik", false); }
        }

        public bool IsAdmin
        {
            get 
            { 
#if R2
                bool adm = Base.getBool(Data, "Admin", false);
                return IsOriginalUser ? adm : adm && (bool)FOriginalIsAdmin;
#else
                return Base.getBool(Data, "Admin", false); 
#endif
            }
        }

        public bool IsPortalAdmin
        {
            get { return HasRight(rPortalAdmin); }
        }
        //-----
        public bool IsScAdmin
        {
            //get { return HasRight(rScorecardsAdmin); }
            get 
            { 
                if (IsOriginalUser)
                    return HasRight(_rScorecardsAdmin); 
                else
                    return HasRight(_rScorecardsAdmin) && HasRight(OriginalRights, _rScorecardsAdmin); 
            }
        }

        public bool IsMSAdmin
        {
            get { return HasRight(rMSAdmin); }
        }

        public bool HasScAccess
        {
            get
            {
                return HasRight(rScorecardsTLProd)
                    || HasRight(rScorecardsTLNieprod)
                    || HasRight(rScorecardsKier)
                    || HasRight(rScorecardsZarz)
                    || HasRight(rScorecardsWnAcc)
                    || HasRight(rScorecardsHR)
                    || HasRight(rScorecardsControlling)
                    ;
            }
        }

        public bool IsScTL
        {
            get { return IsScTLProd || IsScTLNieprod; }
        }

        public bool IsScTLProd
        {
            get { return HasRight(rScorecardsTLProd); }
        }

        public bool IsScTLNieprod
        {
            get { return HasRight(rScorecardsTLNieprod); }
        }

        public bool IsScKier
        {
            get { return HasRight(rScorecardsKier); }
        }

        public bool IsScZarz
        {
            get { return HasRight(rScorecardsZarz); }
        }

        public bool HasScAccessAdm
        {
            get { return HasScAccess || IsScAdmin; }
        }

        public bool _IsScHR
        {
            get { return HasRight(rScorecardsHR); }
        }

        public bool IsScControlling
        {
            get { return HasRight(rScorecardsControlling); }
        }

        public bool IsScWnRej
        {
            get { return HasRight(rScorecardsWnRej); }
        }
        //-----
        public bool IsMailing
        {
            get 
            { 
                return Base.getBool(Data, "Mailing", false);
                //return HasRight(_rMailing);
            }
        }

        public bool IsSuperuser
        {
            get { return HasRight(rSuperuser); }
        }

        public bool IsRaporty
        {
            get 
            {
#if R2
                bool rap = Base.getBool(Data, "Raporty", false);
                return IsOriginalUser ? rap : rap && (bool)FOriginalIsRaporty;
#else
                return Base.getBool(Data, "Raporty", false); 
#endif
            }
        }

        public bool IsRaporty2
        {
            get 
            {
                //return false;
                return App.User.HasRight(AppUser.rRaporty2);
                //return IsRaporty; 
            }
        }

        public bool IsKomputer
        {
            get
            {
                return HasRight(rKomputer);
            }
        }

        //------
        public bool HasZastepstwo       // czy może kogoś zastąpić - czy ma jakieś zastępstwa
        {
            get
            {
                if (!String.IsNullOrEmpty(Id))
                {
                    DataRow dr = Base.getDataRow(String.Format(    // nie moze byc db bo wywoływana w Default.aspx a tam nie ma mastera
                        "select Id from Zastepstwa where {0} between Od and Do and IdZastepujacy = {1}",
                        db.sqlGetDate("GETDATE()"),
                        Id));
                    return dr != null;
                }
                else return false;
            }
        }

        public bool IsSetZastepstwa
        {
            get { return IsKierownik; }  // poki co ..., później dodać uprawnienia
        }

        public bool IsZastepuje         // odnosi sie tylko do kierowników ..., pracownicy moga zawsze zastapic
        {
            get { return IsKierownik; }  // poki co ...
        }

        public string Rights
        {
            //get { return db.getValue(Data, "Rights"); }
            get
            {
#if R2
                if (String.IsNullOrEmpty(FMergedRights))   // żeby troszkę optymalniej było ...
                {
                    string rights = db.getValue(Data, "Rights");
                    if (!IsOriginalUser && !HasRight(_FOriginalRights, rZastWithAllRights))   // nie jestem oryginalny, ale mam uprawnienie
                    {
                        // tu filter (maska) jeżeli jakieś uprawnienia powinny zostać 
                        FMergedRights = MergeRights(rights, _FOriginalRights, null);
                    }
                    else
                        FMergedRights = rights;
                }
                return FMergedRights;
#else
                return db.getValue(Data, "Rights");
#endif
            }
        }

        public string _RightsPortal
        {
            get { return db.getValue(Data, "RightsPortal"); }
        }

        public DateTime? PassExpire
        { 
            get { return db.getDateTime(Data, "PassExpire"); }
        }


        //-------------------------------------
        public const int ltPass         = 0;     // auth pass   
        public const int ltAuthWinPass  = 1;     // do app1 auth win, do app2 auth pass; app1 - poziom I zabezpieczeń, app2 - poziom II zabezpieczeń
        public const int ltAuthWin      = 2;     // do app1 auth win, do app2 auth pass

        public int LoginType
        {
            get
            {
#if PORTAL                
    #if SPX || ZELMER || OKT || APATOR
                return ltAuthWin;   // 
    #else
                return db.getInt(Data, "LoginType", ltPass);
    #endif
#else
                return ltAuthWin;   // RCP
#endif
            }
        }

        //---------------------------------

        
        public static bool LoginKarta_1(string idKarty)
        {
            SqlConnection con = null;  // >>> później zmienić na MasterPage 
            db.DoConnect(ref con);
            DataRow dr = db.getDataRow(con, String.Format("select * from Pracownicy where NrKarty1 = '{0}'", idKarty));
            db.Disconnect(con);
            if (dr != null)
            {
                string pid = db.getValue(dr, "Id");
                App.User._LoginAsUserId(pid);
                bool ok = App.User.LoginKartaByPracId(pid);
                if (ok)
                {
                    x_IdKarty = idKarty;
                    App.KwitekKadryId = App.User.NR_EW;
                    App.KwitekPracId = App.User.Id;
                }
                return ok;
            }
            else
                return false;
        }




        public static bool LoginKarta(string idKarty)
        {
            SqlConnection con = null;  // >>> później zmienić na MasterPage 
//#if PORTAL
 //           con = db.Connect(db.PORTAL);
//#else
            db.DoConnect(ref con);
//#endif       

            DataRow dr = db.getDataRow(con, String.Format("select * from Pracownicy where NrKarty1 = '{0}'", idKarty));
            db.Disconnect(con);
            if (dr != null)
            {
                string pid = db.getValue(dr, "Id");
                App.User._LoginAsUserId(pid);
                App.User.CheckPassLoginTest(App.User.NR_EW);
                dr = GetData(pid);
                App.User.LoginKartaByPracId(dr);    // <<<< nadpisuje FLogin
                App.User.DoPassLogin();             // zaznacza ze zalogowany hasłem i nie trzeba pytac

                App.KwitekKadryId = App.User.NR_EW;
                App.KwitekPracId = App.User.Id;

                AppUser.x_IdKarty = App.User.NR_EW;
                return true;
            }
            else
                return false;
        }




        /*
        public static bool LoginKarta(string idKarty)
        {
            SqlConnection con = null;  // >>> później zmienić na MasterPage 
            db.DoConnect(ref con);
            DataRow dr = db.getDataRow(con, String.Format("select * from Pracownicy where NrKarty1 = '{0}'", idKarty));
            db.Disconnect(con);
            if (dr != null)
            {
                IdKarty = idKarty;
                //Data = dr;
                //return IsLogged;
                return true;
            }
            else
                return false;
        }
        */
        
        public bool LoginKartaByPracId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                drData = GetData(id);
#if R2
                FMergedRights = null;
#endif
                if (drData != null)
                {
                    FLogin = db.getValue(drData, "Login");
                    
                    Log.Info(Log.KIOSKLOGIN, null, 1, "Logowanie użytkownika za pomocą karty", String.Format("{0} {1}", NazwiskoImie, NR_EW), Log.OK);
                    return true;
                }
                else
                    Log.Error(Log.KIOSKLOGIN, "Nieudane logowanie użytkownika za pomocą karty", String.Format("{0}", id));
            }
            return false;
        }

        public bool LoginKartaByPracId(DataRow drP)   // kiosk portal
        {
            drData = drP;
#if R2
            FMergedRights = null;
#endif
            if (drData != null)
            {
                FLogin = db.getValue(drData, "Login");
                
                Log.Info(Log.KIOSKLOGIN, null, 1, "Logowanie użytkownika za pomocą karty", String.Format("{0} {1}", NazwiskoImie, NR_EW), Log.OK);
                return true;
            }
            else
            {
                Log.Error(Log.KIOSKLOGIN, "Nieudane logowanie użytkownika za pomocą karty", null);
                return false;
            }
        }

        //-----------
        public static bool UpdateNrKartyRCP(string pracId, string kartaRCP)
        {
            string data = Tools.DateToStrDb(DateTime.Today);
            string curr = db.getScalar(String.Format("select top 1 NrKarty from PracownicyKarty where IdPracownika = {0} and '{1}' between Od and ISNULL(Do,'20990909')", pracId, data));
            string other = db.getScalar(String.Format("select top 1 IdPracownika from PracownicyKarty where NrKarty = '{0}' and '{1}' between Od and ISNULL(Do,'20990909')", kartaRCP, data));
            SqlTransaction tr = db.con.BeginTransaction("trRCP");
            bool ok = db.execSQL(tr, db.insertCmd("PrzypisaniaRCP", 0,
                "IdPracownika,NrKarty,Data,AdminId,PrevNrKarty,OtherPracId",
                pracId,
                db.strParam(kartaRCP),
                "GETDATE()",
                String.IsNullOrEmpty(App.User.OriginalId) ? App.User.Id : App.User.OriginalId,   // uwaga !!! logowanie zastępstwa z infokiosku - ap_infokiosk nie występuje na liscie userów
                db.strParam(curr),
                db.nullParam(other)
                ));
            if (ok)
            {
                ok = db.execSQL(tr, db.updateCmd("PracownicyKarty", 0, "NrKarty", String.Format("IdPracownika = {0} and Od = '{1}'", pracId, data), db.strParam(kartaRCP)));   // aktualizacja w tym samym dniu
                if (!ok)
                    ok = db.execSQL(tr, db.insertCmd("PracownicyKarty", 0, "IdPracownika,Od,Do,RcpId,NrKarty", pracId, db.strParam(data), db.NULL, pracId, db.strParam(kartaRCP)));
            }
            tr.Commit();
            return ok;
        }

        public static void x_Logout()
        {

        }

        public static string x_IdKarty    // tylko do ustawiania !!! <<< w App jest KartaRCP
        {
            get { return Tools.GetViewStateStr(HttpContext.Current.Session["_prac_idkarty"]); }
            set { HttpContext.Current.Session["_prac_idkarty"] = value; }
        }

        public bool IsKiosk
        {
            get 
            {
                //return true;



                //return GetLogin() == "kiosk";   //<<<<<, tymczas
                return HasRight(OriginalRights, AppUser.rInfokiosk); 
            }
        }

        public bool IsKioskLogged    // true tylko do czasu zalogowania usera
        {
            //get { return GetLogin().ToLower() == KioskLogin; }
            //get { return HasRight(OriginalRights, AppUser.rInfokiosk); }
            get 
            {
                //return !String.IsNullOrEmpty(IdKarty);
                //return App.User.HasRight(AppUser.rInfokiosk); 
                return IsKiosk && IsPassLogged;
            }
        }


    }
}

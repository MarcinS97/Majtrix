using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRRcp.App_Code
{
    public class Lic
    {
        public const bool lang = false;

/* SWITCHE LICENCJI: KLIENT, wewnątrz ewentualnie PRODUKT (po nowemu) */
/* docelowo znaczniki będą zmieszczone w podpisanym cyfrowo pliku "licencji" (lub w bazie ;), wgrywanym do katalogu www u użytkownika */
 
#if DEMO   // uwaga!!! na górze, żeby nie włączył się IQOR, który teraz funkcjonuje też jako switch włączony - docelowo przejrzeć wszystkie miejsca w kodzie ^F IQOR i dodać DEMO
        public const bool rozlNadg          = true;                    
        public const bool planUrlopow       = true;
        public const bool ppPrint           = true;         // wydruk planu pracy (stara formatka)
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;

        public const bool wnZmianaDanych    = false;

        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = true;             // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        
        //public const bool badania           = true;             // badania
        //public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;

        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF               = true;
        public const bool PDF2PNG           = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;     
        public const bool Portal            = true;     
        public const bool BadaniaWstepne    = true;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = true;       // 
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = false;

        public const bool HarmAcc           = true;    

        public const bool Calendar          = false;            // doczego to ???
        public const bool Przydelegowania   = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = false;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj          = false;
        public const bool DzialStanowisko   = false;            // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow    = false;
        public const bool Chat              = true;

        public const bool Social            = false;
        public const bool GrupyUprawnien    = false;
        public const bool SocialAvatarEdit  = false;             // pracownik może sam edytować swój avatar
#elif CO
        public const bool rozlNadg = true;
        public const bool planUrlopow = true;
        public const bool ppPrint = false;
        public const bool wnioskiUrlopowe = true;
        public const bool podzialLudzi = true;
        public const bool zastJaNoEdit = true;

        public const bool wnZmianaDanych = false;

        public const bool portalPrint = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC = true;             // limity nadgodzin na cc
        public const bool kierAbs = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS

        //public const bool badania           = true;             // badania
        //public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;

        public const bool UprExcel = false;
        public const bool Szkolenia = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP = true;
        public const bool Portal = true;
        public const bool BadaniaWstepne = true;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP = true;       // 
        public const bool ScoreCards = false;
        public const bool MatrycaSzkolen = false;

        public const bool HarmAcc = false;

        public const bool Calendar = false;            // doczego to ???
        public const bool Przydelegowania = false;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = false;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj = false;
        public const bool DzialStanowisko = false;             // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow = false;
        public const bool Chat = false;
        public const bool Social = false;
        public const bool GrupyUprawnien = false;
#elif PRON
        public const bool rozlNadg          = true;                    
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;

        public const bool wnZmianaDanych    = false;

        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = true;             // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        
        //public const bool badania           = true;             // badania
        //public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;

        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF               = true;
        public const bool PDF2PNG           = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;     
        public const bool Portal            = true;     
        public const bool BadaniaWstepne    = true;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = true;       // 
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = false;

        public const bool HarmAcc           = false;    

        public const bool Calendar          = false;            // doczego to ???
        public const bool Przydelegowania   = false;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = false;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj          = false;
        public const bool DzialStanowisko   = true;             // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow    = false;
        public const bool Chat              = false;
        public const bool Social            = false;
        public const bool GrupyUprawnien    = false;
#elif IQOR
        public const bool rozlNadg          = true;                    
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;
#if PORTAL
        public const bool wnZmianaDanych    = false;
#else
        public const bool wnZmianaDanych    = true;
#endif
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = true;             // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        
        //public const bool badania           = true;             // badania
        //public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;

        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF               = true;
        public const bool PDF2PNG           = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;     
        public const bool Portal            = true;     
        public const bool BadaniaWstepne    = true;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = true;       // 
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = false;

        public const bool HarmAcc           = false;    

        public const bool Calendar          = false;            // doczego to ???
        public const bool Przydelegowania   = false;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = false;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj          = false;
        public const bool DzialStanowisko   = true;             // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow    = false;
        public const bool Chat              = false;
        public const bool Social            = false;
        public const bool GrupyUprawnien    = false;
#elif SIEMENS
        public const bool rozlNadg          = true;             // moduł rozliczania nadgodzin
        public const bool planUrlopow       = false;
        public const bool ppPrint           = true;             // wydruk planu pracy
        public const bool wnioskiUrlopowe   = false;
        public const bool podzialLudzi      = false;
        public const bool zastJaNoEdit      = false;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = false;            // limity nadgodzin na cc
        public const bool kierAbs           = true;             // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        public const bool badania           = false;            // badania
        public const bool szkoleniaBHP      = false;            // 
        //public const bool scoreCards        = true;             // scoreCardy
        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli= false;
        public const bool PDF               = true;
        public const bool PDF2PNG           = false;

        public const bool RCP               = true;     
        public const bool Portal            = false;     
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // 
        public const bool ScoreCards        = true;
        public const bool MatrycaSzkolen    = false;    

        public const bool Calendar          = false;            // doczego to ???
        public const bool Przydelegowania   = false;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = false;            // automatyczne zakończenie oddelegowania

        public const bool GrupyUprawnien    = false;
#elif VC
        public const bool rozlNadg = true;
        public const bool planUrlopow = false;
        public const bool ppPrint = false;
        public const bool wnioskiUrlopowe = false;
        public const bool podzialLudzi = false;
        public const bool zastJaNoEdit = true;
        public const bool wnZmianaDanych = false;
        public const bool portalPrint = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC = false;            // limity nadgodzin na cc
        public const bool kierAbs = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS

        public const bool UprExcel = false;            // ???
        public const bool Szkolenia = false;            // ???
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;
        public const bool Portal            = false;
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // Szkolenia BHP RCP
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = true;
        
        public const bool HarmAcc           = true;
        
        public const bool Calendar          = true;            // doczego to ???
        public const bool Przydelegowania   = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania


        public const bool AccZeruj          = false;            // widoczna opcja zerwanie nadminut
        public const bool DzialStanowisko   = false;            // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow = true;            // czy wyswietlac podzial kosztow w akceptacji dnia (ppacc)
        public const bool Chat = false;
        public const bool Social = false;

        public const bool GrupyUprawnien    = false;
#elif VICIM
        public const bool rozlNadg = true;
        public const bool planUrlopow = false;
        public const bool ppPrint = false;
        public const bool wnioskiUrlopowe = false;
        public const bool podzialLudzi = true;
        public const bool zastJaNoEdit = true;
        public const bool wnZmianaDanych = false;
        public const bool portalPrint = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC = true;            // limity nadgodzin na cc
        public const bool kierAbs = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS

        public const bool UprExcel = false;            // ???
        public const bool Szkolenia = false;            // ???
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;
        public const bool Portal            = false;
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // Szkolenia BHP RCP
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = false;  //true; T:????
        
        public const bool HarmAcc           = true;
        
        public const bool Calendar          = true;            // doczego to ???
        public const bool Przydelegowania   = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania


        public const bool AccZeruj          = false;            // widoczna opcja zerwanie nadminut
        public const bool DzialStanowisko   = false;            // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow = true;            // czy wyswietlac podzial kosztow w akceptacji dnia (ppacc)
        public const bool Chat = false;
        public const bool Social = false;
        public const bool GrupyUprawnien    = false;
#elif KDR
        public const bool rozlNadg          = true;                    
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = true;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = true;             // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        public const bool badania           = true;             // badania
        public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;

        public const bool UprExcel = true;
        public const bool Szkolenia = true;
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;     
        public const bool Portal            = true;     
        public const bool BadaniaWstepne    = true;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = true;       // 
        public const bool ScoreCards        = true;
        public const bool MatrycaSzkolen    = true;

        public const bool HarmAcc           = false;
        
        public const bool Calendar          = true;            // do czego to ???
        public const bool Przydelegowania   = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj = false;            // widoczna opcja zerwanie nadminut
        public const bool DzialStanowisko = false;            // przy wyborze razem, false - można wybrac osobno
        public const bool PodzialKosztow = true;            // czy wyswietlac podzial kosztow w akceptacji dnia (ppacc)
        public const bool Chat = false;
        public const bool Social = false;
        public const bool GrupyUprawnien    = false;
#elif SPX
        public const bool rozlNadg          = true;
        public const bool planUrlopow       = false;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = false;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = true;             // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        //public const bool wnZmianaDanych  = false;
        public const bool limityNCC         = false;            // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        //public const bool badania           = false;            // badania
        //public const bool scoreCards        = false;
        //public const bool szkoleniaBHP      = false;            // 
        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF               = false;            // raporty wydruk do PDF

        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP = false;
        public const bool Portal = true;
        public const bool BadaniaWstepne = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP = false;       // 
        public const bool ScoreCards = false;
        public const bool MatrycaSzkolen = false;

        public const bool HarmAcc = false;

        public const bool Calendar = true;            // do czego to ???
        public const bool Przydelegowania = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania

        public const bool AccZeruj = false;            // widoczna opcja zerwanie nadminut
        public const bool DzialStanowisko = false;            // przy wyborze razem, false - można wybrac osobno

        public const bool PodzialKosztow = false;            // czy wyswietlac podzial kosztow w akceptacji dnia (ppacc)
        public const bool Chat = true;
        public const bool Social = true;
        public const bool GrupyUprawnien    = false;
#elif DBW
        public const bool rozlNadg = true;
        public const bool planUrlopow = false;
        public const bool ppPrint = false;
        public const bool wnioskiUrlopowe = false;
        public const bool podzialLudzi = false;
        public const bool zastJaNoEdit = true;
        public const bool wnZmianaDanych = false;
        public const bool portalPrint = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC = false;            // limity nadgodzin na cc
        public const bool kierAbs = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS

        public const bool UprExcel = false;            // ???
        public const bool Szkolenia = false;            // ???
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP               = true;
        public const bool Portal            = false;
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // Szkolenia BHP RCP
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = true;
        
        public const bool HarmAcc           = true;
        
        public const bool Calendar          = true;            // doczego to ???
        public const bool Przydelegowania   = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania


        public const bool AccZeruj          = false;            // widoczna opcja zerwanie nadminut
        public const bool DzialStanowisko   = false;            // przy wyborze razem, false - można wybrac osobno

        public const bool PodzialKosztow    = false;            // czy wyswietlac podzial kosztow w akceptacji dnia (ppacc)
        public const bool Chat = false;
        public const bool Social = false;
        public const bool GrupyUprawnien    = false;
#elif OKT
        /* skids, ogarnij to */
        public const bool rozlNadg = true;
        public const bool planUrlopow = false;
        public const bool ppPrint = false;
        public const bool wnioskiUrlopowe = false;
        public const bool podzialLudzi = false;
        public const bool zastJaNoEdit = true;
        public const bool wnZmianaDanych = false;
        public const bool portalPrint = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC = false;            // limity nadgodzin na cc
        public const bool kierAbs = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS

        public const bool UprExcel = false;            // ???
        public const bool Szkolenia = false;            // ???
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...

        public const bool RCP = true;
        public const bool Portal = false;
        public const bool BadaniaWstepne = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP = false;       // Szkolenia BHP RCP
        public const bool ScoreCards = false;
        public const bool MatrycaSzkolen = true;

        public const bool HarmAcc = true;

        public const bool Calendar = true;            // doczego to ???
        public const bool Przydelegowania = true;            // możliwość przydelegowania pracownika 
        public const bool OddelegAutoPowrot = true;            // automatyczne zakończenie oddelegowania
        public const bool DzialStanowisko = true;            // przy wyborze razem, false - można wybrac osobno
        public const bool AccZeruj = false;
        public const bool PodzialKosztow = false;

        public const bool Chat = false;
        public const bool Social = false;
        public const bool GrupyUprawnien    = false;
#endif











        /* wersja RCPII 
#if PORTAL
    #if IQOR || KDR
    #elif SPX
    #else
        public const bool rozlNadg          = true;
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
    #endif
#else   //----- RCP -----
#if SIEMENS
#else
#endif
#endif
        */


                /* stara wersja 
#if PORTAL
#if IQOR || KDR
#elif SPX
        public const bool rozlNadg          = true;
        public const bool planUrlopow       = false;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = false;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = true;             // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        //public const bool wnZmianaDanych  = false;
        public const bool limityNCC         = false;            // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        //public const bool badania           = false;            // badania
        //public const bool scoreCards        = false;

        //public const bool szkoleniaBHP      = false;            // 
        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli = false;

        public const bool PDF               = false;            // raporty wydruk do PDF
#else
        public const bool rozlNadg          = true;
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
#endif
#else   //----- RCP -----
#if SIEMENS
        public const bool rozlNadg          = true;             // moduł rozliczania nadgodzin
        public const bool planUrlopow       = false;
        public const bool ppPrint           = true;             // wydruk planu pracy
        public const bool wnioskiUrlopowe   = false;
        public const bool podzialLudzi      = false;
        public const bool zastJaNoEdit      = false;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = false;            // limity nadgodzin na cc
        public const bool kierAbs           = true;             // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        public const bool badania           = false;            // badania
        public const bool szkoleniaBHP      = false;            // 
        public const bool scoreCards        = true;             // scoreCardy
        public const bool UprExcel          = false;
        public const bool Szkolenia         = false;
        public const bool StatusSamokontroli= false;
        public const bool PDF               = true;
        public const bool PDF2PNG = false;
#elif DBW
#elif OKT
        public const bool rozlNadg          = false;                    
        public const bool planUrlopow       = false;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = false;
        public const bool podzialLudzi      = false;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = false;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = false;            // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        

        
        public const bool UprExcel          = false;            // ???
        public const bool Szkolenia         = false;            // ???
        public const bool StatusSamokontroli = false;

        public const bool PDF               = true;
        public const bool PDF2PNG           = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...
    
        public const bool HarmAcc           = true;
        public const bool scoreCards        = false;


#else
        public const bool planUrlopow       = true;
        public const bool ppPrint           = false;
        public const bool wnioskiUrlopowe   = true;
        public const bool podzialLudzi      = true;
        public const bool zastJaNoEdit      = true;
        public const bool wnZmianaDanych    = true;
        public const bool portalPrint       = false;            // wydruki domumentów z Portalu na kiosku
        public const bool portalPrintWU     = false;            // wydruki wniosków urlopowych na kiosku
        public const bool limityNCC         = true;             // limity nadgodzin na cc
        public const bool kierAbs           = false;            // absencje wpisane przez kierownika nie znikają po zamknięciu miesiąca SIEMENS
        //public const bool badania           = true;             // badania
        //public const bool szkoleniaBHP      = true;             // 
        //public const bool scoreCards        = false;



        public const bool UprExcel = true;
        public const bool Szkolenia = true;
        public const bool StatusSamokontroli = false;

        public const bool PDF = true;
        public const bool PDF2PNG = false;            // zapis PDF do png przy upload plików 20160514 nie działa bez .net 4 w iQor <<< w IQOR podmieniłem plik na Nop.exe żeby nie trzeba było uaktualniać aplikacji ...
        public const bool HarmAcc = false;
#endif
#endif
         */
        /*
#if MS || DBW || KDR || OKT
        public const bool Calendar = true;
        public const bool Przydelegowania = true;
        public const bool OddelegAutoPowrot = true;
#else
        public const bool Calendar = false;
        public const bool Przydelegowania = false;
        public const bool OddelegAutoPowrot = false;
#endif
        */
        /*
        // moduły
#if DBW
        public const bool RCP               = true;     
        public const bool Portal            = false;     
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // 
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = true;    
#elif IQOR

#elif SIEMENS

#elif VC

#elif VICIM

#elif SPX
#else
        public const bool RCP               = false;
        public const bool Portal            = false;
        public const bool BadaniaWstepne    = false;       // BadaniaWstępne RCP iQor
        public const bool SzkoleniaBHP      = false;       // 
        public const bool ScoreCards        = false;
        public const bool MatrycaSzkolen    = false;    
#endif
        */


    }
}

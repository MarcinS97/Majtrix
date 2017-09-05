using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if KWITEK
[assembly: AssemblyTitle("Panel Pracownika")]
[assembly: AssemblyProduct("KDR.KWITEK")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
#elif PORTAL
[assembly: AssemblyTitle("Portal Pracownika / Kierownika")]
[assembly: AssemblyProduct("KDR.PORTAL")]
[assembly: AssemblyVersion("2.0.0.6")]
[assembly: AssemblyFileVersion("2.0.0.6")]
#elif SCARDS  // Scorecards
[assembly: AssemblyTitle("Scorecards")]
[assembly: AssemblyProduct("KDR.SC")]
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
#elif MS  // Matryca Szkoleń
[assembly: AssemblyTitle("Matryca Szkoleń")]
[assembly: AssemblyProduct("KDR.MS")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
#elif RCP
[assembly: AssemblyTitle("Rejestracja Czasu Pracy")]
[assembly: AssemblyProduct("KDR.RCP")]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]
#else  //old RCP
[assembly: AssemblyTitle("Rejestracja Czasu Pracy")]
[assembly: AssemblyProduct("HRRCP")]
[assembly: AssemblyVersion("2.0.5.7")]
[assembly: AssemblyFileVersion("2.0.5.7")]
#endif

[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("KDR Solutions Sp. z o.o.")]
[assembly: AssemblyCopyright("KDR Solutions Sp. z o.o.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("35D77D60-237D-11E1-A709-42E04824019B")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

/*
---------------------------------------
 * SCORECARDS
---------------------------------------
1.0.0.1
 * rozdzielnik z czasami
---------------------------------------
 * PORTAL
---------------------------------------
2.0.0.6 20170801 CO
 * wersja dla CO, formatka rejestracji w RCP, poprawki
2.0.0.5 20170226 IQOR
 * Kalkulator Podział Ludzi, poprawki Identyfikatorów tymczasowych
2.0.0.4 20170120,20170205 IQOR
 * Wnioski o pracę zdalną
 * Identyfikatory tymczasowe
2.0.0.3 20160725 IQOR
 * dane wymiarów urlopu do wniosku z Asseco
 * poprawiona kontrola ilości dostępnego + blokada
2.0.0.2 20150108 SPX PORTAL
 * logowanie
---------------------------------------
 * RCP
---------------------------------------
2.0.5.7 20160503 IQOR
 * opieka art 188 na godziny
2.0.5.6 20160425 IQOR
 * urlop zaległy 
 * brak pesela -> nrewid + 0000... 
2.0.5.5 20160228 IQOR
 * autoimport PL w transakcji
 * podział kosztów na cc wg przypisania (ciut wczesniej)
 * 
2.0.5.3 20151117 SIEM
 * poprawki scorecard - wejście w menu
2.0.5.2 20151019 iQor, SIEM
 * Szkolenia BHP
 * zmiana exportu Asseco - zerowanie czasu Zm dla zmian S,N,N1
 * zoomy z raportów PM
2.0.5.1 20150930 iQor
 * BadaniaWstępne
 * Przekroczenia Limitów CC
 * Mailin porzesunięcia CC
 * Raport DL w dniu
2.0.5.0 20150823 Sie, iQor
 * import AUTOID
 * przygotowanie scorecards
 * badania wstępne
2.0.4.18 20150730 Sie
 * import absencji 2
 * 
2.0.4.17 20150719 iQor
 * wniosek urlopowy UD - dodatkowy 
2.0.4.16 20150616 Sie
 * funkcjonalności
2.0.4.16 20150608 iQor
 * submenu
 * limity do testów
2.0.4.15 20150411 iQor
 * optymalizacja czasu przesunięć
 * kontrola użycia RcpId
 * poprawki cntSplityWsp przy ustawianiu w Pracownikach - insert nie zapisywał

2.0.4.14 20150217 iQor
 * poprawka liczenia FTE - dodanie CHwKosztach dla Kolca

2.0.4.13 20150211 iQor
 * poprawka liczenia FTE - dodanie obsługi KodZUSChoroby = B -> cały miesiac w kosztach, bo po B zawsze jest M

2.0.4.12 20150209 iQor
 * poprawka liczenia FTE - dodanie ABS.ZUS.PLACI dla urlopow
 * 
2.0.4.11 20150201 iQor,Siemens
 * rozbudowa PL, import, statusy
 * ok.GetKierNotClosed
 * 
2.0.4.10 20150118 iQor
 * kodZUS 151 do liczenia FTE
 * 
2.0.4.9 20150104 iQor 
 * nr karty
 * poprawki exportu do asseco - brak pp mies
 * 
 * mailing jak w Matrycy - spr w SIEMENS !!!
 * 
 * PRZETESTOWA DLA SIEMENSA nr karty !!!
 * 
2.0.4.8 20141205 iQor, 20141215 Siemens
 * Podział Ludzi
 * Siemens - import absencji z pliku txt
 * 
2.0.4.7 20141120 iQor 
 * poprawki wniosków OD
 * 
 * 
 * 
 * 
2.0.4.6 20141111 iQor
 * 019 i coś przy eksporcie splitów macierzystych Asseco.cs <<< przetestować !!!
 * 
 * 
2.0.4.5 20141020 iQor
    1. Włączyłem już automatyczną synchronizację planu urlopów
    2. Prezentacja zaakceptowanych, ale jeszcze nie wprowadzonych, wniosków urlopowych na planie urlopów
    3. Dołożona kolumna cc na raportach administracyjnych i u przełożonych (raporty będą się dłużej generować niestety)
    4. Aktualizacja dodatkowego urlopu zaległego - jest dodawany do zaległego, a nie jak mówiliśmy do dodatkowego - w necie znalazłem interpretację, że "niewykorzystany dodatkowy staje się zaległym" - to byśmy musieli jeszcze omówić
    5. Dodatkowy wniosek o odbiór dni wolnych za święto - zerknij czy taka opcja jest do przyjęcia, mniej wklepywania i nie usuwam zabezpieczeń z normalnego wniosku OD, trzeba mu jeszcze uzupełnić opis
    6. Blokada możliwości akceptacji czasu pracy i wniosków urlopowych dla samego siebie
    7. Aktualizacja wyglądu (to już w sumie było)  
  
  
  
2.0.4.4 20140926
 * poprawki 11h i akceptacji - jabil, siemens jeszcze nie wgrywam
2.0.4.3 20140826 rcp2, 0903, jeszcze nie wgrane
 * siemens - zamykanie okresu rozliczeniowego, uprawnienie do edycji
 * jabil - optymalizacja, poprawki importu Urlopów
 * siemens - praca w sobote - dień do wybrania - wniosek z odwrotnymi datami
 * siemens - nie pokazuje na rozliczeniu nadgodzin niedomiarów przy wyborze odpracowanie wczesniejszego wyjscia w dniu - nie uwzglednial zmiany Poniedzialek < 6
 
2.0.4.1 20140818
jabil - plan urlopów z planowaniem do 0 - poprawki
siemens - optymalizacja - wersja bez nowego AjaxControlToolkit
 
2.0.4.0 20140818
jabil - plan urlopów z planowaniem do 0
 
2.0.3.0 20140807
jabil - import wniosków urlopowych, prezentacja zaakceptowanych na pp, ostrzezenia 11h
 
2.0.2.0 20140726
siemens - pracownicy do PP i PPAcc wg przypisania, a nie statusu Zatrudniony, brak migania po zwolnieniu (poza przypisaniem do mnie)

2.0.0.12 20140714
siemens - zmiana liczenia dodatku 100 poniedziałek before6, protokół
jabil - maile od urlopów dw do zastępujących kierowników, eksport wniosków urlopowych asseco
 
2.0.0.11 20140601  
siemens - mailingi przy przesunięciach do admin, insert pracownicy 
jabil - czas nom 05 11 zatrudnianie i zwalnianie w ciągu mies 
 
2.0.0.10 20140521
zatrudnianie i zwalnianie w ciągu miesiąca

2.0.0.9 20140415
KOMAX: roliczanie - button wolne za sobote
JABIL: czas rcp
2.0.0.5
JABIL: raport stołówka
2.0.0.4
SIEMENS: odczyt z Alarmus, Komax
2.0.0.3
poprawki przesunieć, mailingi z DW
2.0.0.2
przesuniecia, akceptacja kosmetyka i czas "w strefie"
 2.0.0.0
 1.2.0.4 - 20120702
 * import danych z plików CSV powiązanych w oparciu o RogerId, a nie po nazwie
 * 
 1.2.0.5 - 20120725
 * dodanie działu do raportu nadgodzin
 * poprawka wyswietlania raportow jak pracownik bez stawki
 * poprawka confirm delete przy pracid = '00000'
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp.App_Code
{
    public class Worktime : 
#if WT2
        Worktime2
#else
        Worktime1
#endif
    {
        public const int alNoWork           = 0x00000001;       // isZmiana && !isWTime && !isAbsencja 
        public const int alNoShift          = 0x00000002;       // isWTime && !isZmiana 
        public const int alWorkAbsence      = 0x00000004;       // isWTime && isAbsencja
        public const int _alNoNominal        = 0x00000008;       // nie osiągnięto czasu nominalnego, czas pracy < czas zmiany - margines tolerancji
        public const int alOver16           = 0x00000010;       // czas pracy > 16h

        public const int alLate             = 0x00000020;       // spóźnienie
        public const int alPrzWliczNoNadg   = 0x00000040;       // przerwa wliczona w trakcie nadgodzin, a brak zgody
        //public const int al2                = 0x00000050;       // 5 ???
        public const int al_1               = 0x00000080;       // 

        public const int alNoIn             = 0x00000100;       // out, but no int
        public const int alNoOut            = 0x00000200;       // in, but no out

        public const int alCheckInOut       = 0x00000400;       // spr. in, out odczytów pomiędzy <<< NOWY WT2
        public const int alBreakPass        = 0x00000800;       // doliczenie nominalnego czasu przerwy zaliczyło 8h <<< NOWY WT2a
        
        public const int alAlgError         = 0x00001000;       // bład w definicji algorytmu naliczania czasu pracy lub niepoprawny algorytm
        public const int alPrzerwa11        = 0x00002000;       // KONTROLA - przerwa 11h
        public const int alPrzerwa35        = 0x00004000;       // KONTROLA - przerwa 35h
        public const int al_4               = 0x00008000;       // 


        public const int alZmNoNadg         = 0x00010000;       // zmiana nie ma nadgodzin, >>> a wprowadzona korekta ma! - nie będzie jak tego policzyć 
        public const int alShortShift       = 0x00020000;       // zmiana nie jest 8h, a czas > czasu zmiany, moga być nadgodziny - niepoprawna zmiana
        public const int al_6               = 0x00040000;       // 
        public const int al_7               = 0x00080000;       // 

        
        public const int alKNadgNoc         = 0x00100000;       // zmiany kierownika: ilość nadgodzin w nocy > czasu pracy w nocy
        public const int alKCzasZm          = 0x00200000;       // zm.kier: czas pracy na zmianie > czasu zmiany
        public const int alKCzasNoc         = 0x00400000;       // zm.kier: czas nocny > czasu zmiany + nadgodziny noc
        public const int alKCzasNoc8        = 0x00800000;       // zm.kier: czas nocny > 8h
        
        public const int alKNadgNoc8        = 0x01000000;       // ilość nadgodzin w nocy > 8h
        public const int al_8               = 0x02000000;       // 
        public const int al_9               = 0x04000000;       // 
        public const int al_10              = 0x08000000;       // 
        
        public const int alLockError        = 0x10000000;       // niezgodność wartości rcp i zatrzaśniętych
        public const int alMPKError         = 0x20000000;       // niezgodność czasów i podziału na mpk

        public const int _alTester          = 0x40000000;       // tester
        
        //public const int al_11            = 0x80000000;       // ti już jest uint


        //----- stałe Rodzaj z tabeli Kalendarz -----
        public const int dayPracujacy   = -1;
        public const int daySobota      = 0;
        public const int dayNiedziela   = 1;
        public const int daySwieto      = 2;

        
        //----- common functions ----------------------------------------------
        
        public static TimeSpan CountTimeSpan(DateTime czasOd, DateTime czasDo)  // róznica nie moze przekroczyć doby !!!
        {
            TimeSpan dt1 = czasOd.TimeOfDay;
            TimeSpan dt2 = czasDo.TimeOfDay;
            if (dt2 > dt1)
                return dt2.Subtract(dt1);
            else
            {
                TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
                return dt2.Add(oneDay).Subtract(dt1);
            }
        }

        public static int CountTime(DateTime czasOd, DateTime czasDo)  // sec.
        {
            int dt1 = Convert.ToInt32(czasOd.TimeOfDay.TotalSeconds);  // róznica nie moze przekroczyć doby !!!
            int dt2 = Convert.ToInt32(czasDo.TimeOfDay.TotalSeconds);
            if (dt2 >= dt1)   // = dla zmian bez czasu pracy - same nadgodziny np sobota
                return dt2 - dt1;
            else
                return dt2 + 86400 - dt1;
        }

        public static int CountDateTimeSec(DateTime czasOd, DateTime czasDo)  // sec.
        {
            TimeSpan ts = czasDo.Subtract(czasOd);
            return Convert.ToInt32(ts.TotalSeconds);
        }

        public static void x_UpdateZmiana(DataRow wtdr, string kzmid)   // aktualizacja danych zmiany po korekcie w KAcc, kzmid jest brane z ddlZmana
        {
            if (kzmid == "-1")
            {
            }
            else
            {
            }
        }

        public static int TimeToSec(DateTime dt)
        {
            return dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
        }

        //---------------------------------------------------------------------------
        public static double Round05(double d, int decimals)   // Math.Round std zaokrągla 2.35 -> 2.3, podanie MidpointRounding.AwayFromZero 2.35 -> 2.4 a i tak podobno się myli Math.Round(1123.485, 2, MidpointRounding.AwayFromZero) http://stackoverflow.com/questions/977796/in-c-math-round2-5-result-is-2-instead-of-3-are-you-kidding-me
        {     
            double multiplier = Math.Pow(10, decimals);      
            if (d < 0) multiplier *= -1;      
            return Math.Floor((d * multiplier) + 0.5) / multiplier;  
        }

        public static float Round05(float d, int decimals)   // Math.Round std zaokrągla 2.35 -> 2.3, podanie MidpointRounding.AwayFromZero 2.35 -> 2.4 a i tak podobno się myli Math.Round(1123.485, 2, MidpointRounding.AwayFromZero) http://stackoverflow.com/questions/977796/in-c-math-round2-5-result-is-2-instead-of-3-are-you-kidding-me
        {
            double multiplier = Math.Pow(10, decimals);
            if (d < 0) multiplier *= -1;
            return (float)(Math.Floor((d * multiplier) + 0.5) / multiplier);
        }

        public static int RoundSec(int sec, int round, int rtype)
        {
            if (round == 0) return sec;
            else
            {
                int r = 60 * round;
                int rr;
                switch (rtype)
                {
                    case 0:
                        rr = 0;
                        break;
                    case 1:
                        rr = 30 * round;
                        break;  
                    default:    // 2,...
                        rr = 60 * round - 1;
                        break;
                }
                switch (rtype)
                {
                    case 3:
                        sec = RoundSec(sec, 1, 1);
                        break;
                    case 4:
                        sec = RoundSec(sec, 5, 1);
                        break;
                    case 5:
                        sec = RoundSec(sec, 15, 1);
                        break;
                    case 6:
                        sec = RoundSec(sec, 30, 1);
                        break;
                }
                int s = ((sec + rr) / r) * @r;  // div
                return s;                                          
            }
        }

        /*
        public static int RoundSec(int sec, int round, int rtype)
        {
            if (round == 0) return sec;
            else
            {
                int r = 60 * round;
                int s = (int)(Math.Round((double)((sec + (30 * round)) / r), 0, MidpointRounding.AwayFromZero) * r);
                return s;                                          // div
            }
        }
         */

        public static int testRoundSec(int sec, int round)
        {
            if (round == 0) return sec;
            else
            {
                int r = 60 * round;
                int d1 = Convert.ToInt32(
                                Math.Round(
                                    Convert.ToDouble((sec + (30 * round)) / r), 
                                    0, MidpointRounding.AwayFromZero
                                ) * r
                            );
                int d2 = Convert.ToInt32(
                                Round05(
                                    Convert.ToDouble((sec + (30 * round)) / r), 
                                    0
                                ) * r
                            );
                int d3 = (int)(
                                Math.Round(
                                    (double)((sec + (30 * round)) / r), 
                                    0, MidpointRounding.AwayFromZero
                                ) * r
                            );
                int d4 = (int)(
                                Round05(
                                    (double)((sec + (30 * round)) / r), 
                                    0
                                ) * r
                            );
                //testy !!!
                if (d1 != d2 || d1 != d3 || d1 != d4 || d2 != d3 || d2 != d4 || d3 != d4)
                {
                    int x = 0;
                }
                return d2;
            }
        }

        //-----------------------------------------------------------
        public static bool zmNadgodziny(DataRow wtdr)    // czy zmiana ma nadgodziny
        {
            string tz = Base.getValue(wtdr, "TypZmiany");
            switch (tz)
            {
                default:
                    return false;
                case App.zmNadgKolejne:
                    return !Base.isNull(wtdr, "Nadgodziny");
                case App.zmNadgZwykleNocne:
                    return true;
            }
        }

        public static int zmCzas(DataRow wtdr)  // -1 jak nie ma zmiany
        {
            object zmOd = wtdr["ZmianaOd"];
            object zmDo = wtdr["ZmianaDo"];
            if (!Base.isNull(zmOd) && !Base.isNull(zmDo))   // jest zmiana przed lub po korekcie!
                return CountTime((DateTime)zmOd, (DateTime)zmDo);
            else 
                return -1;  // brak zmiany
        }

        public static int GetKierAlert(string pracId, string data, DataRow dr, DateTime? timeIn, DateTime? timeOut, int? zt, int? otD, int? otN, int? nt)
        {
            int zm = 0;
            int zt0  = zt  == null ? 0 : (int)zt;   // uwaga !!! może być -1
            if (zt0 == -1) zt0 = 0;                 // koryguję
            int otD0 = otD == null ? 0 : (int)otD;
            int otN0 = otN == null ? 0 : (int)otN;
            int nt0  = nt  == null ? 0 : (int)nt;
            bool isWTime  = zt0 > 0;                // jest czas po acc lub korekcie K SolveWorktime2 zwraca czy jest czas rcp !!!
            bool isZmiana = !Base.isNull(dr, "ZmianaId");

            if (isZmiana)
            {
                object zmOd = dr["ZmianaOd"];
                object zmDo = dr["ZmianaDo"];
                if (!Base.isNull(zmOd) && !Base.isNull(zmDo))
                    try
                    {
                        zm = CountTime((DateTime)zmOd, (DateTime)zmDo);
                    }
                    catch 
                    {
                        zm = 0;
                    }
            }

            int err = 0;
            if (!isZmiana && (zt0 > 0 || otD0 > 0 || otN0 > 0 || nt0 > 0))          // brak zmiany
                err |= Worktime.alNoShift;  
            if (isZmiana && !Worktime.zmNadgodziny(dr) && (otD0 > 0 || otN0 > 0))   // zmiana nie ma nadgodzin a są
                err |= Worktime.alZmNoNadg;
            if (otN0 > nt0)                                                         // czas nadgodzin w nocy > od czasu pracy w nocy
                err |= Worktime.alKNadgNoc;
            if (isZmiana && zt0 > zm)                                               // czas dłuższy niż zmiany
                err |= Worktime.alKCzasZm;
            if (isZmiana && nt0 > zt0 + otN0)    
                err |= alKCzasNoc;                                                  // czas nocny > czasu zmiany + nadgodziny noc
            if (nt0 > 8 * 3600)
                err |= alKCzasNoc8;                                                 // czas nocny > 8h
            if (isZmiana && otN0 > 8 * 3600)
                err |= alKNadgNoc8;                                                 // czas nadgodzin nocnych > 8h

            /* na razie nie daję, wymaga testów, sprawdzić też przygotowanie wyżej czasów in i out
            if (isZmiana && (zt0 > 0 || otD0 > 0 || otN0 > 0 || nt0 > 0))           // jest czas nie ma we lub wy
            {
                if (timeIn == null)
                    err |= alNoIn;
                if (timeOut == null)
                    err |= alNoOut;
            }
            */
            // i inne sprawdzenia ...     
            //----- MPK ------------------------
            int m = MPK2.IsValid(pracId, data, null, zt0, otD0, otN0, nt0, null, null, null, null);
            if (m > 0) err |= alMPKError;

            return err;
        }

        /* 
         * sprawdzanie alertów - tu zawsze dla danych z RCP
         * alertCheck - czy sprawdzać alerty np. nie ma sensu dla dat > today
         * useKier - true: zwraca wartosci skorygowane lub po akceptacji, false: oryginalnych z RCP
         */


        
        public static int GetBefore6(object timeIn, int max, DateTime zmianaOd, int zmianaMargin)   // max - czas pracy w dniu, jak 0 to nie ma z czego wziąć, bo nadgodziny już są policzone ... 
        {
            return GetBefore6(timeIn, max, TimeToSec(zmianaOd), zmianaMargin);
        }

        public static int GetBefore6(object timeIn, int max, int zmianaOdSec, int zmianaMargin)   // max - czas pracy w dniu, jak 0 to nie ma z czego wziąć, bo nadgodziny już są policzone ... 
        {
#if SIEMENS
            if (max > 0)
            {
                int tIn;
                if (!db.isNull(timeIn))
                {
                    tIn = TimeToSec((DateTime)timeIn);
                    if (zmianaMargin >= 0)
                    {
                        int zOd = zmianaOdSec - zmianaMargin;
                        if (tIn < zOd)
                            tIn = zOd;
                    }
                }
                else tIn = zmianaOdSec;

                const int t6 = 6 * 3600;
                int b6 = t6 - tIn;
                if (b6 > 0)
                    return b6 > max ? max : b6;
            }
            return 0;
#else
            return 0;
#endif
        }
        


        /*
                public static int GetBefore6(object timeIn, int max, DateTime zmianIn, int zmianaMargin)   // max - czas pracy w dniu, jak 0 to nie ma z czego wziąć, bo nadgodziny już są policzone ... 
                {
        #if SIEMENS
                    if (max > 0)
                    {
                        if (!db.isNull(timeIn))
                        {
                            const int t6 = 6 * 3600;
                            int b6 = t6 - TimeToSec((DateTime)timeIn);
                            if (b6 > 0)
                                return b6 > max ? max : b6;
                        }
                        else return -1;  // wyznacz wg zmiany
                    }
                    return 0;
        #else
                    return 0;
        #endif
                }
         */


        public static bool SolveWorktime2(DataRow wtdr,     // na dany dzień wtdr["Data"], zwaraca zaokrąglone warości 
                                        
                                        string x_algRCP, string x_algPar,                                        
                            
                                        int breakTimeMin,   // czas przerwy
                                        int breakTimeMin2,  // czas przerwy [min] - przemnożyć * 60 !!! do sekund
                                        int timeMarginMin,  // margines poniżej którego generuje alert ze nie osiągnięto czasu nominalnego                                         
                                        int zaokr, int zaokrType,
                                        
                                        bool useKier,       // czy dane RPC czy nadpisywać zmianami kierownika

                                        out int _wtime,      // czas łączny - z rcp, bez zaokrąglania !, to nie to samo co różnica Wy-We dla zmian 2 i 12
                                        out int _ztime,      // czas zmiany - wg zmiany jesli przepracowany, moze byc = -1 !!!
                                        out int _otimeD,     // overtime w dzień
                                        out int _otimeN,     // overtime w nocy 
                                        out int _ntime,      // nighttime
                                        
                                        out int rcp_ztime,  // dane zawsze z rcp, na podstawie zmiany
                                        out int rcp_otimeD,
                                        out int rcp_otimeN,
                                        out int rcp_ntime,

                                        //out int before6,
                                        //out object czasIn,

                                        bool alertCheck,    // czy sprawdzać alerty data < today
                                        ref int wtAlert)    // true jesli jest zarejestrowany        
        {
            const int maxWorktimeHH = 16;                               // poki co w kodzie
            const int maxWorktime = maxWorktimeHH * 3600;
            //----- wartości kierownika -----
            //object oKierTimeIn = null;
            //object oKierTimeOut = null;
            object oKierCzas = null;
            object oKierNocne = null;
            object oKierNadgDzien = null;
            object oKierNadgNoc = null;



            int otimeN = 0;
            int otimeD = 0;


            if (useKier)
            {
                bool acc = Base.getInt(wtdr, "Akceptacja", 0) == 1;                
                bool k_CzasIn    = Base.getBool(wtdr, "k_CzasIn",          false);
                bool k_CzasOut   = Base.getBool(wtdr, "k_CzasOut",         false);
                bool k_CzasZm    = Base.getBool(wtdr, "k_CzasZm",          false);
                bool k_NadgDzien = Base.getBool(wtdr, "k_NadgodzinyDzien", false);
                bool k_NadgNoc   = Base.getBool(wtdr, "k_NadgodzinyNoc",   false);
                bool k_Nocne     = Base.getBool(wtdr, "k_Nocne",           false);
                //oKierTimeIn        = acc || k_CzasIn    ? wtdr["CzasIn"]          : null;
                //oKierTimeOut       = acc || k_CzasOut   ? wtdr["CzasOut"]         : null;
                oKierCzas      = acc || k_CzasZm    ? wtdr["CzasZm"]          : null;
                oKierNadgDzien = acc || k_NadgDzien ? wtdr["NadgodzinyDzien"] : null;
                oKierNadgNoc   = acc || k_NadgNoc   ? wtdr["NadgodzinyNoc"]   : null;
                oKierNocne     = acc || k_Nocne     ? wtdr["Nocne"]           : null;

                if (acc)            //20120503 jak null to mają byc zera, jak null to brał wartości rcp co nie powinno mieć miejsca np accpp po akceptacji - zmieniła sie konfiguracja i raptem pokazuje czas, przed akceptacją jak było 
                {
                    if (Base.isNull(oKierCzas)) oKierCzas = -1; 
                    if (Base.isNull(oKierNadgDzien)) oKierNadgDzien = 0;
                    if (Base.isNull(oKierNadgNoc)) oKierNadgNoc = 0;
                    if (Base.isNull(oKierNocne)) oKierNocne = 0;
                }
            }
            //----- parametry zmiany, zaplanowana lub skorygowana zawsze niezaleznie od useKier -----
            int _czas3 = -1;   // do alg obecność 8h
            int zt = 0;
            //int ztMinusBreak = 0;
            int ztMinusMargin = 0;
            object zmOd = wtdr["ZmianaOd"];
            object zmDo = wtdr["ZmianaDo"];
            bool isZmiana = !Base.isNull(zmOd) && !Base.isNull(zmDo);   // jest zmiana przed lub po korekcie!
            bool hasZmNadgodziny = false;
            if (isZmiana)
                try
                {
                    zt = CountTime((DateTime)zmOd, (DateTime)zmDo);
                    hasZmNadgodziny = zmNadgodziny(wtdr);
                }
                catch
                {
                    isZmiana = false;
                }
            //----- wyznaczam łączny czas pracy i nocny -----
            object oCzas = null;        // łaczny czas pracy w dniu
            object oNocne = null;
            object _oPoNocy = null;

            
            
            
            
            string algRCP = db.getValue(wtdr, "RcpAlgorytm");


            switch (algRCP)
            {
                case Worktime2.algBezLiczenia:  //"0":   // nie liczę pracownika, null -> 0 ale później
                    break;
                case Worktime2.algWeWy:         //"1":   //ALG 10	We-Wy	                    1	NULL
                case Worktime2.algWeWyNadg:     //"11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    oCzas    = wtdr["Czas1sec"];
                    oNocne   = wtdr["Nocne1sec"];
                    _oPoNocy = wtdr["ponocy"];
                    if (isZmiana && zt > 0)                                     // zt > 0 zmiana tylko z nadgodzinami
                    {
                        //ztMinusBreak = zt;                                      // przerwa jest wliczona w czas pracy, pracownik moze ją wykorzystać ale nie musi, nadgodziny po czasie zmiany, uzgodnione z p.prawnik 2012-03-01
                        ztMinusMargin = zt - timeMarginMin * 60;                // tak jest bardzie jednoznacznie, muszą sami uwzglednić przerwę
                        //ztMinusMargin = ztMinusBreak - timeMarginMin * 60;    // czas krótszy o margin od czasu nominalnego bez przerwy - pracownik moze wyjść 7:45 jak nie robił sobie przerwy i jest ok
                    }
                    break;
                case Worktime2.algSuma:         //"2":   //ALG 30	Suma w strefie	            2	NULL
                case Worktime2.algSumaNadg:     //"12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                    oCzas = wtdr["Czas2sec"];
                    if (isZmiana && zt > 0)                                     // zt > 0 zmiana tylko z nadgodzinami
                    {
                        ztMinusMargin = zt - timeMarginMin * 60;                // tak jest bardzie jednoznacznie, muszą sami uwzglednić przerwę
                    }
                    break;
                default:    //3 i inne: ALG 50	Obecność 8h          3	8 
                    string algPar = "8";                               // na razie sztywno, i tak nie ma nic innego
                    oCzas = wtdr["czasZm2"];                           // ten wybieram do pokazania, od we do wy
                    int c;

                    if (Int32.TryParse(algPar, out c))
                        _czas3 = c * 3600;
                    else 
                        wtAlert |= alAlgError;                         // czas rzeczywisty biorę
                    break;
            }
            
            //----- czas łączny -----
            _wtime = Base.isNull(oCzas) ? 0 : (int)oCzas;                // łączny czas zawsze z RCP!
            int wtimeR = RoundSec(_wtime, zaokr, zaokrType);

            //----- podział na czas zmiany i nadgodziny -----
            int NadgodzinyAll;
            if (!Base.isNull(oCzas))                                    // o.. -> object - jest czas pracy z rcp
            {
                switch (algRCP)
                {
                    case Worktime2.algBezLiczenia:  //"0":   //ALG nie liczę pracownika
                        NadgodzinyAll = 0;
                        break;
                    case Worktime2.algWeWy:         //"1":   //ALG 10	We-Wy	                    1	NULL

                        /*
                        if (isZmiana)
                        {
                            //if (_wtime < ztMinusMargin) wtAlert |= alNoNominal;
                            if (wtimeR < ztMinusMargin) wtAlert |= alNoNominal;
                            if (_wtime > ztMinusBreak) oCzas = zt;       // czas wynikający ze zmiany z uwzględnieniem przerwy               
                            if (!Base.isNull(oNocne))
                                if ((int)oNocne > zt) oNocne = oCzas;   // ograniczam czas nocnych do długości pracy <<< zweryfikować w HR !!! - czas pracy bez nadgodzin tu więc nocnych też nie powinno być więcej
                        }
                         */

                        if (isZmiana)  //20131111 jak algSum
                        {
                            int czasZm = (int)wtdr["czasZm2"];
                            //int przerwaZm = (int)wtdr["przerwaZm2"];
                            int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                            //int nadgDzien = (int)wtdr["nadgDzien2"];
                            //int nadgNoc= (int)wtdr["nadgNoc2"];
                            //int przerwaNadg = (int)wtdr["przerwaN2"];
                            //int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                            int nocne = (int)wtdr["nocne2"];


                            //if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            int czasZmR = RoundSec(czasZm, zaokr, zaokrType);
                            //if (czasZmR + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            if (czasZmR < ztMinusMargin) wtAlert |= _alNoNominal;    //20131111

                            oCzas = czasZm + przerwaZmNom;
                            oNocne = nocne;
                        }
                          
                        //if (_wtime > maxWorktime) wtAlert |= alOver16;                        
                        if (wtimeR > maxWorktime) wtAlert |= alOver16;
                        NadgodzinyAll = 0;
                        break;

                    case Worktime2.algSuma:         //"2":   //ALG 30	Suma w strefie	            2	NULL
                        if (isZmiana)
                        {
                            int czasZm = (int)wtdr["czasZm2"];
                            //int przerwaZm = (int)wtdr["przerwaZm2"];
                            int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                            //int nadgDzien = (int)wtdr["nadgDzien2"];
                            //int nadgNoc= (int)wtdr["nadgNoc2"];
                            //int przerwaNadg = (int)wtdr["przerwaN2"];
                            //int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                            int nocne = (int)wtdr["nocne2"];


                            //if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            int czasZmR = RoundSec(czasZm, zaokr, zaokrType); 
                            //if (czasZmR + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            if (czasZmR + przerwaZmNom < ztMinusMargin) wtAlert |= _alNoNominal;    //20131111

                            oCzas = czasZm + przerwaZmNom;
                            oNocne = nocne;
                        }
                        if (_wtime > maxWorktime) wtAlert |= alOver16;
                        NadgodzinyAll = 0;
                        break;

                    case Worktime2.algWeWyNadg:      //"11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                        /*
                        if (isZmiana)
                        {
                            //if (_wtime < ztMinusMargin) wtAlert |= alNoNominal;
                            if (wtimeR < ztMinusMargin) wtAlert |= alNoNominal;
                            if (_wtime < ztMinusBreak)
                            {
                                NadgodzinyAll = 0;
                            }
                            else
                            {
                                oCzas = zt;
                                if (hasZmNadgodziny)
                                    NadgodzinyAll = _wtime - ztMinusBreak;
                                    //NadgodzinyAll = _wtime - ztMinusBreak + breakTimeMin2 * 60;
                                else
                                    NadgodzinyAll = 0;
                            }
                        }
                        else
                        {
                            NadgodzinyAll = 0;
                        }
                        */


                        NadgodzinyAll = 0;  //20131111 jak algSumNadg
                        otimeD = 0;
                        otimeN = 0;
                        if (isZmiana)
                        {
                            int czasZm = (int)wtdr["czasZm2"];
                            //int przerwaZm = (int)wtdr["przerwaZm2"];
                            int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                            int nadgDzien = (int)wtdr["nadgDzien2"];
                            int nadgNoc = (int)wtdr["nadgNoc2"];
                            //int przerwaNadg = (int)wtdr["przerwaN2"];
                            int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                            int nocne = (int)wtdr["nocne2"];

                            //if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            int czasZmR = RoundSec(czasZm, zaokr, zaokrType);
                            //if (czasZmR + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            if (czasZmR < ztMinusMargin) wtAlert |= _alNoNominal;    //20131111

                            oCzas = czasZm + przerwaZmNom;
                            oNocne = nocne;
                            if (hasZmNadgodziny)
                            {
                                NadgodzinyAll = nadgDzien + nadgNoc + przerwaNadgNom;
                                otimeD = RoundSec(nadgDzien, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                                otimeN = RoundSec(nadgNoc, zaokr, zaokrType);
                            }
                        }

                        
                        if (_wtime > maxWorktime) wtAlert |= alOver16;
                        break;

                    case Worktime2.algSumaNadg:     //"12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                        NadgodzinyAll = 0;
                        otimeD = 0;
                        otimeN = 0;
                        if (isZmiana)
                        {
                            int czasZm = (int)wtdr["czasZm2"];
                            //int przerwaZm = (int)wtdr["przerwaZm2"];
                            int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                            int nadgDzien = (int)wtdr["nadgDzien2"];
                            int nadgNoc = (int)wtdr["nadgNoc2"];
                            //int przerwaNadg = (int)wtdr["przerwaN2"];
                            int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                            int nocne = (int)wtdr["nocne2"];

                            //if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            int czasZmR = RoundSec(czasZm, zaokr, zaokrType);
                            //if (czasZmR + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            if (czasZmR + przerwaZmNom < ztMinusMargin) wtAlert |= _alNoNominal;    //20131111
                            
                            oCzas = czasZm + przerwaZmNom;
                            oNocne = nocne;
                            if (hasZmNadgodziny)
                            {
                                NadgodzinyAll = nadgDzien + nadgNoc + przerwaNadgNom;
                                otimeD = RoundSec(nadgDzien, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                                otimeN = RoundSec(nadgNoc, zaokr, zaokrType);
                            }
                        }
                        if (_wtime > maxWorktime) wtAlert |= alOver16;
                        break;

                    default:    //3 i inne: ALG 50	Obecność 8h          3	8 

                        NadgodzinyAll = 0;
                        otimeD = 0;
                        otimeN = 0;
                        if (isZmiana)
                        {
                            int czasZm = (int)wtdr["czasZm2"];
                            //int przerwaZm = (int)wtdr["przerwaZm2"];
                            int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                            int nadgDzien = (int)wtdr["nadgDzien2"];
                            int nadgNoc = (int)wtdr["nadgNoc2"];
                            //int przerwaNadg = (int)wtdr["przerwaN2"];
                            int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                            int nocne = (int)wtdr["nocne2"];

                            //if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            int czasZmR = RoundSec(czasZm, zaokr, zaokrType);
                            //if (czasZmR + przerwaZmNom < zt) wtAlert |= alNoNominal;
                            if (czasZmR + przerwaZmNom < ztMinusMargin) wtAlert |= _alNoNominal; //20131111
                            
                            oCzas = czasZm + przerwaZmNom;
                            oNocne = nocne;
                            if (hasZmNadgodziny)
                            {
                                NadgodzinyAll = nadgDzien + nadgNoc + przerwaNadgNom;
                                otimeD = RoundSec(nadgDzien, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                                otimeN = RoundSec(nadgNoc, zaokr, zaokrType);
                            }
                        }
                        break;
                }

                //----- nadgodziny zwykłe i nocne z RCP ------
                /*
                //if (algRCP != "2" && algRCP != "12")
                if (algRCP != Worktime2.algSuma && algRCP != Worktime2.algSumaNadg)
                {
                    int ztt = (int)oCzas;
                    int ntt = Base.isNull(oNocne) ? 0 : (int)oNocne;
                    int _pott = Base.isNull(_oPoNocy) || (int)_oPoNocy < 0 ? 0 : (int)_oPoNocy;

                    if (ntt > 0)        // był czas pracy w nocy
                    {
                        if (_pott > 0)   // przekroczony czas nocny - nadgodziny mogą być znowu w stawce 150%
                        {
                            if (NadgodzinyAll > _pott) otimeN = NadgodzinyAll - _pott;    // nadgodziny w nocy
                            else otimeN = 0;                                            // nie ma
                            //else otimeN = -1;                                         // nie ma 20120503, późniejsze zaokrąglenie i tak zerwało
                        }
                        else            // nie było przekroczenia czasu nocnego
                        {
                            if (NadgodzinyAll <= ntt) otimeN = NadgodzinyAll;           // wszystkie nadgodziny są w nocy
                            else otimeN = ntt;                                          // tylko te nadgodziny które są w nocy
                        }
                        otimeN = RoundSec(otimeN, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                        otimeD = RoundSec(NadgodzinyAll, zaokr, zaokrType) - otimeN;
                        if (otimeD < 0)                                                 // zabezpieczenie, nie powinno miec miejsca
                        {
                            otimeD = 0;
                            Log.Info(Log.t2APP, "SolveWorktime2", "otimeD < 0, data=" +
                                                Base.getDateTime(wtdr, "Data", DateTime.MinValue) + ", zmiana=" +
                                                Base.getValue(wtdr, "ZmianaId"), Log.OK);
                        }
                    }
                    else
                    {
                        otimeN = 0;
                        otimeD = RoundSec(NadgodzinyAll, zaokr, zaokrType);
                    }
                }
                */


                /*
                jest:
                Nall = czas łączny - (czas zmiany - przerwa*) + przerwaN; *jeżeli alg = suma w strefie
                jeśli nie ma czasu po nocy i Nall < nocny -> wszystko jest NN
                jeśli 
                  
                zmienić na:  
                liczenie czasu nadgodzin - od końca czasu na zmianie policzyć sumę czasu pracy i przerw w dzień i w nocy 
                czasem przerwy podczas nadgodzin wypełnić pierwszy czas przerwy (nocny albo dzienny) i jeśli starczy go to pozostałą przerwę
                jeżeli jeszcze zostanie czasu przerwy to dodać do czasu pracy i przeliczyć czas pracy w nocy !

                */
            }
            else    // brak czasu pracy z RCP
            {
                otimeN = 0;
                otimeD = 0;
            }

            //----- alerty sprawdzam dla danych przed korektą kierownika -----
            bool isWTime = _wtime > 0;   // jest czas z rcp
            if (alertCheck)
            {

                int? a = Base.getInt(wtdr, "Alert");
                if (a != null)
                    wtAlert |= (int)a;

                object oTimeIn = wtdr["TimeIn"];
                object oTimeOut = wtdr["TimeOut"];
                if (Base.isNull(oTimeIn) && !Base.isNull(oTimeOut)) wtAlert |= alNoIn;
                if (!Base.isNull(oTimeIn) && Base.isNull(oTimeOut)) wtAlert |= alNoOut;

                bool isAbsencja = !Base.isNull(wtdr, "AbsencjaKod") || !Base.isNull(wtdr, "AbsencjaKodKier") || !Base.isNull(wtdr, "AbsencjaKodWniosek"); // zeby zgasic alert ...
                //xxx bool isAbsencja = !Base.isNull(wtdr, "AbsencjaKod");  // tylko absencja z KP się tu liczy, po zamknięciu okresu absencja K znika

                if (isZmiana && !isWTime && !isAbsencja) wtAlert |= alNoWork;
                if (isWTime && !isZmiana) wtAlert |= alNoShift;
                if (isWTime && isAbsencja) wtAlert |= alWorkAbsence;
       




                if (!hasZmNadgodziny && (otimeD > 0 || otimeN > 0)) wtAlert |= alZmNoNadg;
            
            
            
            


            }
            else wtAlert = 0;
            //----- wartości rcp -----
            rcp_ztime = Base.isNull(oCzas) ? -1 : RoundSec((int)oCzas, zaokr, zaokrType); // -1 - zeby nie pokazywał na acc pp, jak 0 to pokaze
            rcp_otimeD = otimeD;
            rcp_otimeN = otimeN;
            rcp_ntime = Base.isNull(oNocne) ? 0 : RoundSec((int)oNocne, zaokr, zaokrType);
            //----- nadpisanie wartosci ustawionych przez kier lub zamkniecie okresu lub zaokrąglenia -----           
            if (!Base.isNull(oKierCzas)) _ztime = (int)oKierCzas;
            else _ztime = rcp_ztime;
                //if (Base.isNull(oCzas))
                //    ztime = -1;                                         // zeby nie pokazywał na acc pp, jak 0 to pokaze
                //else 
                //    ztime = RoundSec((int)oCzas, zaokr, zaokrType);

            if (!Base.isNull(oKierNadgDzien)) otimeD = (int)oKierNadgDzien;
            if (!Base.isNull(oKierNadgNoc)) otimeN = (int)oKierNadgNoc;

            if (!Base.isNull(oKierNocne)) _ntime = (int)oKierNocne;
            else _ntime = rcp_ntime;
            //else _ntime = Base.isNull(oNocne) ? 0 : RoundSec((int)oNocne, zaokr, zaokrType);
            //.else ntime = Base.isNull(oNocne) ? -1 : RoundSec((int)oNocne, zaokr, zaokrType);  20120503 zmieniam bo WT2 wstawia null jak nie znajdzie czasu pracy do dnia 



            /*
            if (ztime == -1 || otimeD == -1 || otimeN == -1 || _ntime == -1)
            {
                int x = 0;
            }
            */



            //------------------------------
            _otimeD = otimeD;
            _otimeN = otimeN;
            //------------------------------
            //if (_ztime > 0)
            //{
            //    object tIn = wtdr["TimeIn"];
            //    before6 = GetBefore6(tIn, _ztime);
            //}
            //else before6 = 0;

            //czasIn = wtdr["CzasIn"];

            //------------------------------
            return isWTime;  
        }


        
        
        
        
        
        /*
            switch (algRCP)
            {
                case "0":   // nie liczę pracownika, null -> 0 ale później
                    break;
                case "1":   //ALG 10	We-Wy	                    1	NULL
                case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    break;
                case "2":   //ALG 30	Suma w strefie	            2	NULL
                case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                    break;
                default:    //3 i inne: ALG 50	Obecność 8h          3	8 
                    break;
            }
        */

        
        
        //------------------------------------------------------------------------------------
        /* nowa wersja, w zamysle bez wartosci skorygowanych ktore bylyby dokładane osobną funkcją
        public static bool SolveWorktime(DataRow wtdr,     // na dany dzień wtdr["Data"], zwaraca zaokrąglone warości 
                                        string algRCP, string algPar,
                                        int breakTimeMin,   // czas przerwy
                                        int breakTimeMin2,  // czas przerwy [min] - przemnożyć * 60 !!! do sekund
                                        int timeMarginMin,  // margines poniżej którego generuje alert ze nie osiągnięto czasu nominalnego                                         
                                        int zaokr, int zaokrType,
                                        out int wtime,      // czas łączny - z rcp, bez zaokrąglania !, to nie to samo co różnica Wy-We dla zmian 2 i 12
                                        out int ztime,      // czas zmiany - wg zmiany jesli przepracowany, moze byc = -1 !!!
                                        out int otimeD,     // overtime w dzień
                                        out int otimeN,     // overtime w nocy 
                                        out int ntime,      // nighttime

                                        bool alertCheck,    // czy sprawdzać alerty data < today
                                        ref int wtAlert)    // true jesli jest zarejestrowany        
        {
            const int maxWorktimeHH = 16;                               // poki co w kodzie
            const int maxWorktime = maxWorktimeHH * 3600;
            
            //----- parametry zmiany, zaplanowana lub skorygowana zawsze niezaleznie od useKier -----
            int czas3 = -1;   // do alg obecność 8h
            int ztMinusBreak = 0;
            int ztMinusMargin = 0;

            int zt = zmCzas(wtdr);
            bool isZmiana = zt != -1;
            bool hasZmNadgodziny = zmNadgodziny(wtdr);
            //----- wyznaczam łączny czas pracy i nocny -----
            object oCzas = null;
            object oNocne = null;
            object _oPoNocy = null;
            int NadgodzinyAll = 0;
            //----- podział na czas zmiany i nadgodziny -----
            switch (algRCP)
            {
                case "0":   //ALG nie liczę pracownika
                    break;
                case "1":   //ALG 10	We-Wy	                    1	NULL
                    oCzas = wtdr["Czas1sec"];

                    if (!Base.isNull(oCzas))                                    // o.. -> object - jest czas pracy z rcp
                    {
                    
                    
                    
                    oNocne = wtdr["Nocne1sec"];
                    oPoNocy = wtdr["ponocy"];
                    if (isZmiana && zt > 0)                                     // zt > 0 zmiana tylko z nadgodzinami
                    {
                        ztMinusBreak = zt;                                      // przerwa jest wliczona w czas pracy, pracownik moze ją wykorzystać ale nie musi, nadgodziny po czasie zmiany, uzgodnione z p.prawnik 2012-03-01
                        ztMinusMargin = zt - timeMarginMin * 60;                // tak jest bardzie jednoznacznie, muszą sami uwzglednić przerwę
                        //ztMinusMargin = ztMinusBreak - timeMarginMin * 60;    // czas krótszy o margin od czasu nominalnego bez przerwy - pracownik moze wyjść 7:45 jak nie robił sobie przerwy i jest ok
                    }

                    if (isZmiana)
                    {
                        if (wtime < ztMinusMargin) wtAlert |= alNoNominal;
                        if (wtime > ztMinusBreak) oCzas = zt;       // czas wynikający ze zmiany z uwzględnieniem przerwy               
                        if (!Base.isNull(oNocne))
                            if ((int)oNocne > zt) oNocne = oCzas;   // ograniczam czas nocnych do długości pracy <<< zweryfikować w HR !!! - czas pracy bez nadgodzin tu więc nocnych też nie powinno być więcej
                    }
                    if (wtime > maxWorktime) wtAlert |= alOver16;
                    NadgodzinyAll = 0;
                    break;
                case "2":   //ALG 30	Suma w strefie	            2	NULL
                    if (isZmiana)
                    {
                        int czasZm = (int)wtdr["czasZm2"];
                        //int przerwaZm = (int)wtdr["przerwaZm2"];
                        int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                        //int nadgDzien = (int)wtdr["nadgDzien2"];
                        //int nadgNoc= (int)wtdr["nadgNoc2"];
                        //int przerwaNadg = (int)wtdr["przerwaN2"];
                        //int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                        int nocne = (int)wtdr["nocne2"];

                        if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                        oCzas = czasZm + przerwaZmNom;
                        oNocne = nocne;
                    }
                    if (wtime > maxWorktime) wtAlert |= alOver16;
                    NadgodzinyAll = 0;
                    break;
                case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    oCzas = wtdr["Czas1sec"];
                    oNocne = wtdr["Nocne1sec"];
                    oPoNocy = wtdr["ponocy"];
                    if (isZmiana && zt > 0)                                     // zt > 0 zmiana tylko z nadgodzinami
                    {
                        ztMinusBreak = zt;                                      // przerwa jest wliczona w czas pracy, pracownik moze ją wykorzystać ale nie musi, nadgodziny po czasie zmiany, uzgodnione z p.prawnik 2012-03-01
                        ztMinusMargin = zt - timeMarginMin * 60;                // tak jest bardzie jednoznacznie, muszą sami uwzglednić przerwę
                        //ztMinusMargin = ztMinusBreak - timeMarginMin * 60;    // czas krótszy o margin od czasu nominalnego bez przerwy - pracownik moze wyjść 7:45 jak nie robił sobie przerwy i jest ok
                    }
                    
                    if (isZmiana)
                    {
                        if (wtime < ztMinusMargin) wtAlert |= alNoNominal;
                        if (wtime < ztMinusBreak)
                        {
                            NadgodzinyAll = 0;
                        }
                        else
                        {
                            oCzas = zt;
                            if (hasZmNadgodziny)
                                NadgodzinyAll = wtime - ztMinusBreak + breakTimeMin2 * 60;
                            else
                                NadgodzinyAll = 0;
                        }
                    }
                    else
                    {
                        NadgodzinyAll = 0;
                    }
                    if (wtime > maxWorktime) wtAlert |= alOver16;
                    break;
                case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                    oCzas = wtdr["Czas2sec"];

                    
                    
                    NadgodzinyAll = 0;
                    otimeD = 0;
                    otimeN = 0;
                    if (isZmiana)
                    {
                        int czasZm = (int)wtdr["czasZm2"];
                        //int przerwaZm = (int)wtdr["przerwaZm2"];
                        int przerwaZmNom = (int)wtdr["przerwaZm2nom"];
                        int nadgDzien = (int)wtdr["nadgDzien2"];
                        int nadgNoc = (int)wtdr["nadgNoc2"];
                        //int przerwaNadg = (int)wtdr["przerwaN2"];
                        int przerwaNadgNom = (int)wtdr["przerwaN2nom"];
                        int nocne = (int)wtdr["nocne2"];

                        if (czasZm + przerwaZmNom < zt) wtAlert |= alNoNominal;
                        oCzas = czasZm + przerwaZmNom;
                        oNocne = nocne;
                        if (hasZmNadgodziny)
                        {
                            NadgodzinyAll = nadgDzien + nadgNoc + przerwaNadgNom;
                            otimeD = RoundSec(nadgDzien, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                            otimeN = RoundSec(nadgNoc, zaokr, zaokrType);
                        }
                    }
                    if (wtime > maxWorktime) wtAlert |= alOver16;
                    break;
                default:    //3 i inne: ALG 50	Obecność 8h          3	8 
                    oCzas = wtdr["Czas1sec"];                           // ten wybieram do pokazania
                    oNocne = wtdr["Nocne1sec"];
                    oPoNocy = null;
                    int c;
                    if (Int32.TryParse(algPar, out c))
                    {
                        if (Base.isNull(oCzas))                         // jak kier nie ustawił oCzas to ustawiam
                        {
                            czas3 = c * 3600;
                            oCzas = czas3;                              // czas jest w sek, a algPar w h
                        }
                    }
                    else wtAlert |= alAlgError;                         // czas rzeczywisty biorę
                    if (isZmiana)
                    {
                        ztMinusBreak = czas3;                           // przerwa nie jest liczona (jest wliczona w czas pracy)
                        ztMinusMargin = -1;                             // nie kontroluję
                    }

                    NadgodzinyAll = 0;
                    if (czas3 != -1)                                // jest poprawny
                        if ((int)oNocne > czas3)
                            oNocne = czas3;                         // ograniczam czas nocnych do długości pracy <<< zweryfikować w HR !!!
                    break;
            }




                //----- czas łączny -----
                wtime = Base.isNull(oCzas) ? 0 : (int)oCzas;                // łączny czas zawsze z RCP!



                //----- nadgodziny zwykłe i nocne z RCP ------
                if (algRCP != "2" && algRCP != "12")
                {
                    int ztt = (int)oCzas;
                    int ntt = Base.isNull(oNocne) ? 0 : (int)oNocne;
                    int pott = Base.isNull(oPoNocy) || (int)oPoNocy < 0 ? 0 : (int)oPoNocy;

                    if (ntt > 0)        // był czas pracy w nocy
                    {
                        if (pott > 0)   // przekroczony czas nocny - nadgodziny mogą być znowu w stawce 150%
                        {
                            if (NadgodzinyAll > pott) otimeN = NadgodzinyAll - pott;    // nadgodziny w nocy
                            else otimeN = 0;                                            // nie ma
                            //else otimeN = -1;                                         // nie ma 20120503, późniejsze zaokrąglenie i tak zerwało
                        }
                        else            // nie było przekroczenia czasu nocnego
                        {
                            if (NadgodzinyAll <= ntt) otimeN = NadgodzinyAll;           // wszystkie nadgodziny są w nocy
                            else otimeN = ntt;                                          // tylko te nadgodziny które są w nocy
                        }
                        otimeN = RoundSec(otimeN, zaokr, zaokrType);                    // nocna zaokrąglam na korzyść pracownika bo są droższe !!! >>> spr w HR
                        otimeD = RoundSec(NadgodzinyAll, zaokr, zaokrType) - otimeN;
                        if (otimeD < 0)                                                 // zabezpieczenie, nie powinno miec miejsca
                        {
                            otimeD = 0;
                            Log.Info(Log.t2APP, "SolveWorktime2", "otimeD < 0, data=" +
                                                Base.getDateTime(wtdr, "Data", DateTime.MinValue) + ", zmiana=" +
                                                Base.getValue(wtdr, "ZmianaId"), Log.OK);
                        }
                    }
                    else
                    {
                        otimeN = 0;
                        otimeD = RoundSec(NadgodzinyAll, zaokr, zaokrType);
                    }
                    /*
                    jest:
                    Nall = czas łączny - (czas zmiany - przerwa*) + przerwaN; *jeżeli alg = suma w strefie
                    jeśli nie ma czasu po nocy i Nall < nocny -> wszystko jest NN
                    jeśli 
                  
                    zmienić na:  
                    liczenie czasu nadgodzin - od końca czasu na zmianie policzyć sumę czasu pracy i przerw w dzień i w nocy 
                    czasem przerwy podczas nadgodzin wypełnić pierwszy czas przerwy (nocny albo dzienny) i jeśli starczy go to pozostałą przerwę
                    jeżeli jeszcze zostanie czasu przerwy to dodać do czasu pracy i przeliczyć czas pracy w nocy !

                    * /
                }
            }
            else    // brak czasu pracy z RCP
            {
                otimeN = 0;
                otimeD = 0;
            }

            //----- alerty sprawdzam dla danych przed korektą kierownika -----
            bool isWTime = wtime > 0;   // jest czas z rcp
            if (alertCheck)
            {
#if WT2
                int? a = Base.getInt(wtdr, "Alert");
                if (a != null)
                    wtAlert |= (int)a;
#endif
                object oTimeIn = wtdr["TimeIn"];
                object oTimeOut = wtdr["TimeOut"];
                if (Base.isNull(oTimeIn) && !Base.isNull(oTimeOut)) wtAlert |= alNoIn;
                if (!Base.isNull(oTimeIn) && Base.isNull(oTimeOut)) wtAlert |= alNoOut;

                bool isAbsencja = !Base.isNull(wtdr, "AbsencjaKod") || !Base.isNull(wtdr, "AbsencjaKodKier"); // zeby zgasic alert ...
                //xxx bool isAbsencja = !Base.isNull(wtdr, "AbsencjaKod");  // tylko absencja z KP się tu liczy, po zamknięciu okresu absencja K znika

                if (isZmiana && !isWTime && !isAbsencja) wtAlert |= alNoWork;
                if (isWTime && !isZmiana) wtAlert |= alNoShift;
                if (isWTime && isAbsencja) wtAlert |= alWorkAbsence;
                if (!hasZmNadgodziny && (otimeD > 0 || otimeN > 0)) wtAlert |= alZmNoNadg;
            }
            else wtAlert = 0;
            //----- nadpisanie wartosci ustawionych przez kier lub zamkniecie okresu lub zaokrąglenia -----            
            if (!Base.isNull(oKierCzas)) ztime = (int)oKierCzas;
            else
                if (Base.isNull(oCzas))
                    ztime = -1;                                         // zeby nie pokazywał na acc pp, jak 0 to pokaze
                else
                    ztime = RoundSec((int)oCzas, zaokr, zaokrType);

            if (!Base.isNull(oKierNadgDzien)) otimeD = (int)oKierNadgDzien;
            if (!Base.isNull(oKierNadgNoc)) otimeN = (int)oKierNadgNoc;

            if (!Base.isNull(oKierNocne)) _ntime = (int)oKierNocne;
            else _ntime = Base.isNull(oNocne) ? 0 : RoundSec((int)oNocne, zaokr, zaokrType);
            //else ntime = Base.isNull(oNocne) ? -1 : RoundSec((int)oNocne, zaokr, zaokrType);  20120503 zmieniam bo WT2 wstawia null jak nie znajdzie czasu pracy do dnia 
            //------------------------------
            return isWTime;
        }
        */
        //------------------------------------------------------------------------------------

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        public static List<string> GetAlertMsg(int acode)
        {
            List<string> msg = new List<string>();
            //if ((acode & alTester) != 0)       msg.Add("----- TESTER -----");
            
            if ((acode & alNoWork) != 0)            msg.Add("zmiana jest określona, nie ma czasu pracy lub absencji");
            if ((acode & alNoShift) != 0)           msg.Add("nie określona zmiana");
            if ((acode & alWorkAbsence) != 0)       msg.Add("praca podczas absencji");
            if ((acode & _alNoNominal) != 0)        msg.Add("czas pracy krótszy niż czas zmiany");
            if ((acode & alShortShift) != 0)        msg.Add("czas pracy dłuższy niż czas zmiany");

            if ((acode & alLate) != 0)              msg.Add("spóźnienie");
            if ((acode & alPrzWliczNoNadg) != 0)    msg.Add("przerwa wliczona w trakcie nadgodzin - brak zgody lub algorytm bez nadgodzin");

            if ((acode & alOver16) != 0)            msg.Add("czas pracy dużo większy niż czas zmiany (> 16h)");
            
            if ((acode & alNoIn) != 0)              msg.Add("brak godziny wejścia");
            if ((acode & alNoOut) != 0)             msg.Add("brak godziny wyjścia");

            if ((acode & alCheckInOut) != 0)        msg.Add("brak godziny wejścia lub wyjścia w czasie pracy");
            if ((acode & alBreakPass) != 0)         msg.Add("wcześniejsze wyjście pracownika");  // doliczenie nominalnego czasu przerwy zaliczyło 8h

            if ((acode & alAlgError) != 0)          msg.Add("błąd w definicji algorytmu naliczania czasu");
            if ((acode & alZmNoNadg) != 0)          msg.Add("zmiana nie pozwala na nadgodziny");

            if ((acode & alLockError) != 0)         msg.Add("modyfikacja konfiguracji stref - zmiana danych czasu RCP<br />(proszę ponownie zweryfikować i zaakceptować czas pracy)");
            if ((acode & alMPKError) != 0)          msg.Add("niezgodność czasu pracy i podziału na centra kosztowe");

            if ((acode & alKNadgNoc) != 0)          msg.Add("ilość nadgodzin w nocy większa niż czas pracy w nocy");
            if ((acode & alKCzasZm) != 0)           msg.Add("czas pracy na zmianie większy niż długość zmiany");
            if ((acode & alKCzasNoc) != 0)          msg.Add("czas pracy w nocy nie może być większy niż czas pracy / nadgodzin nocnych");
            if ((acode & alKCzasNoc8) != 0)         msg.Add("czas pracy w nocy nie może być większy niż 8h"); //czas pracy na zmianie i nadgodzin w nocy");

            msg.AddRange(GetAlertMsg2(acode));

            return msg;
        }

        public const int alMask2 = alPrzerwa11 | alPrzerwa35;

        public static List<string> GetAlertMsg2(int acode)
        {
            List<string> msg = new List<string>();
            if ((acode & alPrzerwa11) != 0)         msg.Add("nie została zachowana wymagana przerwa 11h");
            if ((acode & alPrzerwa35) != 0)         msg.Add("nie została zachowana wymagana przerwa 35h");

            return msg;
        }
        /*
        public static bool SolveWorktime(DataRow wtdr,      // na dany dzień wtdr["Data"]
                                        string algRCP, string algPar,
                                        
                                        int breakTimeMin,   // czas przerwy
                                        int breakTimeMin2,  // czas przerwy [min] - przemnożyć * 60 !!! do sekund
                                        int timeMarginMin,  // margines poniżej którego generuje alert ze nie osiągnięto czasu nominalnego 
                                        
                                        int zaokr, int zaokrType,
                                        bool useKier,
                                        out int wtime,      // czas łączny - z rcp
                                        out int ztime,      // czas zmiany - wg zmiany jesli przepracowany
                                        out int otime,      // overtime (w sumie)
                                        out int ontime,     // overtime w nocy (w tym w nocy)
                                        out int ntime,      // nighttime
                                        ref int wtAlert)    // true jesli jest zarejestrowany        
        {
            object oCzas = null;
            object oNocne = null;
            object oPoNocy = null;
            object oNadgodziny = null;

            object oTimeIn, oTimeOut, oKierCzas, oKierNocne, oKierNadgodziny;
            if (useKier)
            {
                oTimeIn = wtdr["CzasIn"];
                oTimeOut = wtdr["CzasOut"];
                oKierCzas = wtdr["CzasZm"];
                oKierNocne = wtdr["Nocne"];
                oKierNadgodziny = wtdr["Nadgodziny"];
            }
            else
            {
                oTimeIn = null;
                oTimeOut = null;
                oKierCzas = null;
                oKierNadgodziny = null;
                oKierNocne = null;
            }

            if (Base.isNull(oTimeIn)) oTimeIn = wtdr["TimeIn"];
            if (Base.isNull(oTimeOut)) oTimeOut = wtdr["TimeOut"];
            
            int czas3 = -1;   // do alg obecność 8h
            //----- parametry zmiany -----
            int zt = 0;
            int ztMinusBreak = 0;
            int ztMinusMargin = 0;
            object zmOd = wtdr["ZmianaOd"];
            object zmDo = wtdr["ZmianaDo"];
            bool isZmiana = !Base.isNull(zmOd) && !Base.isNull(zmDo);   // jest zmiana !
            if (isZmiana)
                try
                {
                    zt = CountTime((DateTime)zmOd, (DateTime)zmDo);
                }
                catch
                {
                    isZmiana = false;
                }
            //----- wyznaczam łączny czas pracy i nocny -----
            switch (algRCP)
            {
                case "1":   //ALG 10	We-Wy	                    1	NULL
                case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    oCzas = wtdr["Czas1sec"];
                    oNocne = wtdr["Nocne1sec"];
                    oPoNocy = wtdr["ponocy"];
                    if (isZmiana)
                    {
                        ztMinusBreak = zt;                                      // przerwa jest wliczona w czas pracy, pracownik moze ją wykorzystać ale nie musi, nadgodziny po czasie zmiany, uzgodnione z p.prawnik 2012-03-01
                        ztMinusMargin = zt - timeMarginMin * 60;                // tak jest bardzie jednoznacznie, muszą sami uwzglednić przerwę
                        //ztMinusMargin = ztMinusBreak - timeMarginMin * 60;  // czas krótszy o margin od czasu nominalnego bez przerwy - pracownik moze wyjść 7:45 jak nie robił sobie przerwy i jest ok
                    }
                    break;
                case "2":   //ALG 30	Suma w strefie	            2	NULL
                case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                    oCzas = wtdr["Czas2sec"];
                    oNocne = wtdr["Nocne2sec"];
                    oPoNocy = wtdr["ponocy2"];
                    if (isZmiana)
                    {
                        ztMinusBreak = zt - breakTimeMin * 60;                  // przerwa odejmuje się od czasu pracy bo wychodzą na nią odbijając się <- jak się nie odbiją i śniadanie zjedzą w trakcie jest to nie do wykrycia przez system
                        ztMinusMargin = ztMinusBreak - timeMarginMin * 60;      // czas krótszy o margin od czasu nominalnego bez przerwy - pracownik moze wyjść 7:45 jak nie robił sobie przerwy i jest ok, uzgodnione z kierownikami 2012-03-01
                    }
                    break;
                default:    //3 i inne: ALG 50	Obecność 8h          3	8 
                    oCzas = wtdr["Czas1sec"];                           // ten wybieram do pokazania
                    oNocne = wtdr["Nocne1sec"];
                    oPoNocy = null;
                    int c;
                    if (Int32.TryParse(algPar, out c))
                    {
                        if (Base.isNull(oCzas))                         // jak kier nie ustawił oCzas to ustawiam
                        {
                            czas3 = c * 3600;
                            oCzas = czas3;                              // czas jest w sek, a algPar w h
                        }
                    }
                    else wtAlert |= alAlgError;                         // czas rzeczywisty biorę
                    if (isZmiana)
                    {
                        ztMinusBreak = czas3;                           // przerwa nie jest liczona (jest wliczona w czas pracy)
                        ztMinusMargin = -1;                             // nie kontroluję
                    }
                    break;
            }
            
            //----- czas łączny -----
            const int maxWorktimeHH = 16;                               // poki co w kodzie
            int maxWorktime = maxWorktimeHH * 3600;
            wtime = Base.isNull(oCzas) ? 0 : (int)oCzas;                // łączny czas zawsze z RCP!

            //----- podział na czas zmiany i nadgodziny -----
            //bool isKierCzas = !Base.isNull(oKierCzas);
            //bool isKierNadgodziny = !Base.isNull(oNadgodziny);

            if (!Base.isNull(oCzas))                                    // o.. -> object - jest czas pracy z rcp
            {
                switch (algRCP)
                {
                    case "1":   //ALG 10	We-Wy	                    1	NULL
                    case "2":   //ALG 30	Suma w strefie	            2	NULL
                        if (isZmiana)
                        {
                            if (wtime < ztMinusMargin) wtAlert |= alNoNominal;
                            if (wtime > ztMinusBreak) oCzas = zt;       // czas wynikający ze zmiany z uwzględnieniem przerwy               
                            if (!Base.isNull(oNocne))
                                if ((int)oNocne > zt) oNocne = oCzas;   // ograniczam czas nocnych do długości pracy <<< zweryfikować w HR !!!
                        }
                        if (wtime > maxWorktime) wtAlert |= alOver16;
                        oNadgodziny = 0;
                        break;
                    case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                        if (isZmiana)
                        {
                            if (wtime < ztMinusMargin) wtAlert |= alNoNominal;
                            if (wtime < ztMinusBreak) 
                            {
                                oNadgodziny = 0;
                            }
                            else
                            {
                                oCzas = zt;
                                oNadgodziny = wtime - ztMinusBreak + breakTimeMin2 * 60;
                            }
                        }
                        else
                        {
                            oNadgodziny = 0;
                        }
                        if (wtime > maxWorktime) wtAlert |= alOver16;
                        break;
                    default:    //3 i inne: ALG 50	Obecność 8h          3	8 
                        oNadgodziny = 0;
                        if (czas3 != -1)                                // jest poprawny
                            if ((int)oNocne > czas3)
                                oNocne = czas3;                         // ograniczam czas nocnych do długości pracy <<< zweryfikować w HR !!!
                        break;    
                }
            }
            
            //----- nadpisanie wartosci ustawionych przez kier lub zamkniecie okresu -----
            if (!Base.isNull(oKierCzas)) oCzas = oKierCzas;
            if (!Base.isNull(oKierNadgodziny)) oNadgodziny = oKierNadgodziny;
            if (!Base.isNull(oKierNocne)) oNocne = oKierNocne;

            // tu by sie przydała mozliwosc nadpisania oPoNocy !!!, na razie nie daję 
            //----- wyliczenie czasu nadgodzin w nocy ------
            int ztt = Base.isNull(oCzas) ? 0 : (int)oCzas;
            int ott = Base.isNull(oNadgodziny) ? 0 : (int)oNadgodziny;
            int ntt = Base.isNull(oNocne) ? 0 : (int)oNocne;
            int pott = Base.isNull(oPoNocy) || (int)oPoNocy < 0 ? 0 : (int)oPoNocy;

            if (ntt > 0)
            {
                if (pott > 0)   // przekroczony czas nocny - nadgodziny mogą być znowu w stawce 150%
                {
                    if (ott > pott) ontime = ott - pott;
                    else ontime = -1;   // nie ma
                }
                else            // nie było przekroczenia czasu nocnego
                {
                    if (ott <= ntt) ontime = ott;
                    else ontime = ntt;
                }
                ontime = RoundSec(ontime, zaokr, zaokrType);
            }
            else ontime = -1;
            //----- zaokrąglenia -----
            //wtime = RoundSec(wtime, zaokr, zaokrType);
            ztime = Base.isNull(oCzas) ? -1 : RoundSec((int)oCzas, zaokr, zaokrType);
            otime = Base.isNull(oNadgodziny) ? -1 : RoundSec((int)oNadgodziny, zaokr, zaokrType);   // -1 - brak
            ntime = Base.isNull(oNocne) ? -1 : RoundSec((int)oNocne, zaokr, zaokrType);

            return wtime > 0;  // >= ?????
        }
         */


        public static string SecToTime(int sec, int prec)      // jako prec przekazac round; zaokrągla w dół !!!
        {
            return SecToTimePP(sec, prec, null, false);
        }

        public static string SecToTimePP(int sec, int round, int? rtype, bool cut00)      // jak rtype=null nie zaokrągla, tylko odcina string pokonwersji
        {
            bool minus = sec < 0;
            if (minus) sec = -sec;

            if (rtype != null)
                sec = RoundSec(sec, round, (int)rtype);
            
            TimeSpan ts = TimeSpan.FromSeconds(sec);
            string t;
            string ms;
            if (round >= 60)        // h
            {
                t = (ts.Days * 24 + ts.Hours).ToString();
            }
            else if (round == 0)    // h:mm:ss
            {
                if (cut00 && ts.Seconds == 0)
                    ms = ts.Minutes != 0 ? ":" + ts.Minutes.ToString("00") : null;
                else
                    ms = ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
                t = (ts.Days * 24 + ts.Hours).ToString() + ms;
            }
            else                    // h:mm
            {
                if (cut00 && ts.Minutes == 0)
                    ms = null;
                else
                    ms = ":" + ts.Minutes.ToString("00");
                t = (ts.Days * 24 + ts.Hours).ToString() + ms;
            }
            return minus ? "-" + t : t;
        }
        
        /*
        public static string SecToTimePP(int sec, int round, int? rtype, bool cut00)      // jak rtype=null nie zaokrągla, tylko odcina string pokonwersji
        {
            if (rtype != null)
                sec = RoundSec(sec, round, (int)rtype);

            
            string t = TimeSpan.FromSeconds(sec).ToString();    // hh:mm:ss 00:00:00
            if (round >= 60)        // h
            {
                if (t.StartsWith("0")) t = t.Substring(1, 1);
                else t = t.Substring(0, 2);
            }
            else if (round == 0)    // h:mm:ss
            {
                if (t.StartsWith("0")) t = t.Substring(1);
                if (cut00)
                    if (t.EndsWith(":00:00")) t = t.Substring(0, t.Length - 6);
                    else if (t.EndsWith(":00")) t = t.Substring(0, t.Length - 3);
            }
            else                    // h:mm
            {
                if (t.StartsWith("0")) t = t.Substring(1, 4);
                else t = t.Substring(0, 5);
                if (cut00)
                    if (t.EndsWith(":00")) t = t.Substring(0, t.Length - 3);
            }
            return t;
        }
         */

        public static string WorktimeToPP(int round, int rtype,        // czasy powinny byc juz pozaokrąglane ... 
                                 int wtime, int ztime, int otimeD, int otimeN, int ntime)
        {
            //ꜛꜜ ˙ꜝ - strzałki w górę/w dół , ? wykrzyknik w indeksie górnym
            string c1 = null;
            string c2 = null;
            string c3 = null;
            string c4 = null;
            int otime = otimeD + otimeN;

            if (ztime != -1)
                if (wtime > 0)                                  // moze byc 0 jak czas wprowadzoy przez kierownika
                    c1 = String.Format("<{0} title=\"{2}\">{1}</{0}>",
                        //"wt1",
                        "h1",
                        SecToTimePP(ztime, round, null, true),
                        SecToTimePP(wtime, 0, null, false));   // łączny czas pracy zaokrąglam wg rtype do minut
                else 
                    c1 = String.Format("<{0}>{1}</{0}>",
                        //"wt1",
                        "h1",
                        SecToTimePP(ztime, round, null, true));
            if (otimeN > 0)
            {
                //c2 = String.Format("<{0} title=\"{1}\">={2}=</{0}>", //"<{0} title=\"{1}\">{2}</{0}>", 
                c2 = String.Format("<{0} title=\"w nocy: {1}\">●{2}</{0}>", 
                    //"wt2",
                    "h2",
                    SecToTimePP(otimeN, round, null, true),
                    SecToTimePP(otime, round, null, true));
            }
            else
                if (otime > 0)
                    c2 = String.Format("<{0}>+{1}</{0}>",
                        //"wt2",
                        "h2",
                        SecToTimePP(otime, round, null, true));
            if (ntime > 0)
                        c3 = String.Format("<{0}>●{1}</{0}>",
                        //"wt3",
                        "h3",
                        SecToTimePP(ntime, round, null, true));            
            /*
            if (wtime > 0) 
                    c4 = br + String.Format("<{0} title=\"{1}\">{2}</{0}>", 
                        "wt4",
                        SecToTimePP(wtime, 0, null, false), 
                        SecToTimePP(wtime, 1, rtype, false));  // łączny czas pracy zaokrąglam wg rtype do minut
            */
            if (c1 != null || c2 != null || c3 != null || c4 != null)
                return c1 + "<br />" + c2 + "<br />" + c3 + c4;
            else
                return null;
        }


        /*
        public static string WorktimeToPP(int round,
                                 int wtime, int ztime, int otime, int ntime)
        {
            string czas;
            if (ztime != -1) czas = SecToTime(ztime, round);
            else czas = null;
            if (otime > 0) czas += "+" + SecToTime(otime, round);
            if (ntime > 0) czas += "<br />●" + SecToTime(ntime, round);   //●•°
            else czas += "<br />";
            if (wtime > 0) czas += "<br />" + SecToTime(wtime, 1);
            return czas;
        }
         */

        public static void WorktimeToPP(int round, int rtype,
                                int ztime, int otimeD, int otimeN, int ntime,
                                out int? ztimeR, out int? otimeDR, out int? otimeNR, out int? ntimeR)
        {                                                                               // wtime nie ma sensu zaokrąglać
            if (ztime != -1) ztimeR = RoundSec(ztime, round, rtype);
            else ztimeR = null;

            if (otimeD > 0) otimeDR = RoundSec(otimeD, round, rtype);
            else otimeDR = null;
            
            if (otimeN > 0) otimeNR = RoundSec(otimeN, round, rtype);
            else otimeNR = null;

            if (ntime > 0) ntimeR = RoundSec(ntime, round, rtype); 
            else ntimeR = null;
        }

        //---------------------------------------------------------------------------
        public static string DateTimeToTimeStr(object dt)
        {
            if (dt.Equals(DBNull.Value))
                return null;
            else
                return ((DateTime)dt).ToString("HH:mm");
        }

        public static string TimeToHourStr(object dt)
        {
            if (dt.Equals(DBNull.Value))
                return null;
            else
                return ((DateTime)dt).TimeOfDay.TotalHours.ToString();
        }

        public static string SecToHourStr(object sec)
        {
            if (sec == null || sec.Equals(DBNull.Value))
                return null;
            else
                return SecToHourStr((int)sec);
        }

        public static string SecToHourStr(int sec)
        {
            return SecToHour(sec).ToString();
        }

        public static int SecToHourInt(int sec)
        {
            return sec / 3600;
        }

        public static double SecToHour(int sec)
        {
            return (double)sec / 3600;
        }
        
        
        /*
        // to nie tak ma być ... - umieścić w SolveWorkTime ...
        public static bool OverrideWorktime(object kTimeIn, object kTimeOut, object kCzasZm, object kNadgodziny,                                        
                                            ref string wtime, ref string ntime, ref string otime)
        {
            if (!kCzasZm.Equals(DBNull.Value))
            {
                ntime = TimeToHourStr(kCzasZm);
            }
            if (!kNadgodziny.Equals(DBNull.Value))
            {
                otime = TimeToHourStr(kNadgodziny);
            }
            if (!kCzasZm.Equals(DBNull.Value) || !kNadgodziny.Equals(DBNull.Value))
            {
                wtime = ntime + (String.IsNullOrEmpty(otime) || otime == "-" ? null : "+" + otime);
                return true;
            }
            else return false;
        }

        */
        //-----------------------------
        public static int TimeInterSec(DateTime? A1, DateTime? A2, DateTime B1, DateTime B2)
        {
            if (A1 != null || A2 != null)
                if (A1 < A2)
                {
                    DateTime t1, t2;
                    if (A1 < B1) t1 = (DateTime)B1;
                    else t1 = (DateTime)A1;
                    if (A2 < B2) t2 = (DateTime)A2;
                    else t2 = (DateTime)B2;
                    if (t2 > t1)
                        return CountDateTimeSec(t1, t2);
                }
            return 0;
        }


        /*
ALTER FUNCTION [dbo].[TimeInterSec](@A1 datetime, @A2 datetime, @B1 datetime, @B2 datetime)          
        declare @t1 datetime
	declare @t2 datetime
	declare @dt int	
	
	if @A1 is null or @A2 is null or @A1 > @A2
		set @dt = 0
	else begin
		if @A1 < @B1 set @t1 = @B1
		else set @t1 = @A1	
		if @A2 < @B2 set @t2 = @A2
		else set @t2 = @B2	
		if @t2 > @t1 set @dt = datediff(second,'1900-01-01',@t2 - @t1);
		else set @dt = 0
	end
	return @dt
         */
        //-----------------------------
        public static DataRow GetStrefaRCP2(string pracId, string nadzien, out string strefaId, out string algId, out string algPar)
        {
            //if (nadzien == null) nadzien = DateTime.Today;
            //string data = ((DateTime)nadzien).ToStringDb();      
            if (String.IsNullOrEmpty(nadzien))
                nadzien = Tools.DateToStrDb(DateTime.Today);
            DataRow dr = db.getDataRow(String.Format(@"
declare @pracId int, @data datetime
set @pracId = {0}
set @data = '{1}'

select 
	R.IdPracownika, R.RcpStrefaId as StrefaId, PP.RcpAlgorytm as Algorytm, K.Parametr as AlgPar,
	S.Nazwa as StrefaNazwa, K.Nazwa as AlgNazwa
from Przypisania R 
left outer join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @data between PP.Od and ISNULL(PP.Do, '20990909')
left outer join Kody K on K.Typ = 'ALG' and K.Kod = PP.RcpAlgorytm
left outer join Strefy S on S.Id = R.RcpStrefaId
where R.IdPracownika = @pracId and @data between R.Od and ISNULL(R.Do, '20990909')
                ", pracId, nadzien));
            if (dr != null)
            {
                strefaId = Base.getValue(dr, "StrefaId");
                if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                algId = Base.getValue(dr, "Algorytm");
                if (String.IsNullOrEmpty(algId)) algId = "0";
                algPar = Base.getValue(dr, "AlgPar");
            }
            else
            {
                strefaId = "0";     // czy null ???????????
                algId = "0";
                algPar = null;
            }
            return dr;
        }








        /* 20131111
        public static DataRow _GetStrefaRCP(SqlConnection con, string pracId, out string strefaId, out string algId, out string algPar)
        {               // troche  moze przekombinowana ...
            //DataRow dr = Base.getDataRow(con,
            //    "select P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
            //        "D.Nazwa as Dzial, " +
            //        "S.Nazwa as Stanowisko, " +
            //        "ISNULL(RcpStrefaId," +
            //            "case when P.Kierownik=1 then D.KierStrefaId " +
            //            "else D.PracStrefaId end) as StrefaId, " +
            //        "'' as StrefaNazwa, " +
            //        "ISNULL(RcpAlgorytm," +
            //            "case when P.Kierownik=1 then D.KierAlgorytm " +
            //            "else D.PracAlgorytm end) as Algorytm, " +
            //        "'' as AlgPar, " +
            //        "'' as AlgNazwa " +
            //    "from Pracownicy P " +
            //    "left outer join Dzialy D ON D.Id = P.IdDzialu " +
            //    "left outer join Stanowiska S ON S.Id = P.IdStanowiska " +
            //    "where P.Id = " + pracId);
            
            DataRow dr = Base.getRow(Worker.GetPracInfo1(con, 1, null,
                ",'' as StrefaNazwa,'' as AlgPar,'' as AlgNazwa ",
                pracId, true));
            if (dr != null)
            {
                strefaId = Base.getValue(dr, "StrefaId");
                if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                algId = Base.getValue(dr, "Algorytm");
                if (String.IsNullOrEmpty(algId)) algId = "0";
                DataRow adr = Base.getDataRow(con, "select * from Kody where Typ='ALG' and Kod=" + algId);
                if (adr != null)
                {
                    algPar = Base.getValue(adr, "Parametr");
                    dr["AlgPar"] = algPar;
                    dr["AlgNazwa"] = Base.getValue(adr, "Nazwa");
                }
                else algPar = null;
                DataRow sdr = Base.getDataRow(con, "select * from Strefy where Id = " + strefaId);
                if (sdr != null)
                    dr["StrefaNazwa"] = Base.getValue(sdr, "Nazwa");
            }
            else
            {
                strefaId = "0";     // czy null ???????????
                algId = "0";
                algPar = null;
            }
            return dr;
        }
         */
        //-----------------------------
        public static int GetIloscDniPracPrac(string pracId, string dataOd, string dataDo)
        {
            int d = Tools.StrToInt(db.getScalar(String.Format(@"
select COUNT(*) 
from dbo.GetDates2('{1}','{2}') D
inner join Przypisania R on R.IdPracownika = {0} and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data
where K.Data is null 
                ", pracId, dataOd, dataDo)), -1);
            if (d == -1)
                return _GetIloscDniPrac(null, dataOd, dataDo);
            else
                return d;
        }

        public static int GetIloscDniPracPracWymiar(string pracId, string dataOd, string dataDo)
        {
            int d = Tools.StrToInt(db.getScalar(String.Format(@"
select 
    sum(ISNULL(PP.WymiarCzasu, 0)) CzasNom 
from dbo.GetDates2('{1}','{2}') D
inner join Przypisania R on R.IdPracownika = {0} and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and D.Data between PP.Od and ISNULL(PP.Do, '20990909')
left join Kalendarz K on K.Data = D.Data
where K.Data is null 
                ", pracId, dataOd, dataDo)), -1);
            if (d == -1)
                return _GetIloscDniPrac(null, dataOd, dataDo) * 28800;  //lub etat ?
            else
                return d;
        }
        //-----
        public static int GetIloscDniPracMiesPrac(string pracId, string data)
        {
            int d = Tools.StrToInt(db.getScalar(String.Format(@"
select COUNT(*) 
from dbo.GetDates2(dbo.bom('{1}'),dbo.eom('{1}')) D
inner join Przypisania R on R.IdPracownika = {0} and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join Kalendarz K on K.Data = D.Data
where K.Data is null 
                ", pracId, data)), -1);
            if (d == -1)
                return _GetIloscDniPracMies(null, data);
            else
                return d;
        }

        public static int GetIloscDniPracMiesPracWymiar(string pracId, string data)
        {
            int d = Tools.StrToInt(db.getScalar(String.Format(@"
select 
    sum(ISNULL(PP.WymiarCzasu, 0)) CzasNom 
from dbo.GetDates2(dbo.bom('{1}'),dbo.eom('{1}')) D
inner join Przypisania R on R.IdPracownika = {0} and D.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and D.Data between PP.Od and ISNULL(PP.Do, '20990909')
left join Kalendarz K on K.Data = D.Data
where K.Data is null 
                ", pracId, data)), -1);
            if (d == -1)
                return _GetIloscDniPracMies(null, data) * 28800;
            else
                return d;
        }

        public static int _GetIloscDniPrac(SqlConnection con, string dataOd, string dataDo)
        {
            string ret = Base.getScalar(con, String.Format(
                "select DATEDIFF(d, '{0}', '{1}') + 1 - COUNT(*) from Kalendarz where Data between '{0}' and '{1}'",
                dataOd, dataDo));
            return Tools.StrToInt(ret, 0);
        }

        public static int _GetIloscDniPracMies(SqlConnection con, string dataDo)
        {
            string ret = Base.getScalar(con, String.Format(
                "select DniPrac from CzasNom where Data='{0}'",
                dataDo.Substring(0,7) + "-01"));   // data zawsze jest w formacie yyyy-mm-dd, odcinam dd i dokładam 1
            return Tools.StrToInt(ret, 0);
        }

        public static int GetRodzajDnia(DateTime data)   // 0 - sobota, 1 - niedziela, 2- święto, -1 dzień pracujący
        {
            return Tools.StrToInt(db.getScalar(String.Format("select ISNULL(Rodzaj, {1}) from Kalendarz where Data = '{0}'", Tools.DateToStrDb(data), dayPracujacy)), dayPracujacy);
        }
        //--------
        public static int GetIloscDniPrac(SqlConnection con, DateTime dataOd, DateTime dataDo)
        {
            string ret = Base.getScalar(con, String.Format(
                "select DATEDIFF(d, '{0}', '{1}') + 1 - COUNT(*) from Kalendarz where Data between '{0}' and '{1}'",
                Tools.DateToStr(dataOd), 
                Tools.DateToStr(dataDo)));
            return Tools.StrToInt(ret, 0);
        }

        public static DateTime GetIloscDniPracDataDo(SqlConnection con, DateTime dataOd, int dni)
        {
            DataSet ds = db.getDataSet(con, String.Format(@"
declare @dataOd datetime
set @dataOd = '{0}'
select top {1} * 
from dbo.GetDates2(@dataOd, DATEADD(DAY, {1} * 2 + 10, @dataOd)) D
left outer join Kalendarz K on K.Data = D.Data
where K.Data is null
order by D.Data", Tools.DateToStr(dataOd), dni));
            int cnt = db.getCount(ds);
            if (cnt > 0)
                return (DateTime)db.getDateTime(db.getRows(ds)[cnt - 1], "Data");
            else
                return dataOd;
        }

        public static DateTime GetIloscDniPracDataOd(SqlConnection con, DateTime dataOd, int dni)
        {
            DataSet ds = db.getDataSet(con, String.Format(@"
declare @dataOd datetime
set @dataOd = '{0}'
select top {1} * 
from dbo.GetDates2(@dataOd, DATEADD(DAY, {1} * 2 + 10, @dataOd)) D
left outer join Kalendarz K on K.Data = D.Data
where K.Data is null
order by D.Data desc", Tools.DateToStr(dataOd), dni));
            int cnt = db.getCount(ds);
            if (cnt > 0)
                return (DateTime)db.getDateTime(db.getRows(ds)[0], "Data");
            else
                return dataOd;
        }
    }
}






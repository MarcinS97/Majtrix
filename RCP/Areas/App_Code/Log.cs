using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

/*
POLA:
 * Id
 * DataCzas
 * ParentId
 * Typ
 * Kod
 * Kod2
 * Info
 * Info2
 
INFORMACJE TRAFIAJĄCE DO LOGU:
- ERROR (aplikacji, wysyłania maila, srwisu)
 * kod błędu
 * info
 * info2
  
- INFO (uruchomienie/zakończenie serwisu, rozpoczęcie/zakończenie programu)
 * info
 * info2
  
- JOB (wysłanie maila)
 * typ 
    SENDMAIL
 * id_pracownika/grupa: pracownicy -1, kierownicy -2, administratorzy -3 itd.
 * id maila - wzór: 1,2,3, ...
 * info
 * info2
  
Do logu trafiają tylko niezbędne informacje, jesli wysłanie maila po akceptacji ankiety ok, to nie trafia!  
*/

namespace HRRcp.App_Code
{
    public class Log
    {
        //----- LOG - Typ -----
        public const int tINFO = 0;             // tylko informacja
        public const int tERROR = 1;            // błąd
        public const int tJOB = 2;              // praca do wykonania

        //----- LOG - Typ2 -----
        public const int t2APP_INFO             = 0;    // informacja ogólna/inna
        public const int t2APP                  = 1;    // błąd aplikacji
        public const int NOACCESS               = 2;    // logowanie użytkownika
        public const int LOGIN                  = 3;    // logowanie użytkownika

        public const int IMPORT                 = 9;    // import danych
        public const int t2APP_IMPORTSTRUCT     = 10;   // import pliku struktury
        public const int t2APP_IMPORTABSENCJE   = 11;   // import z KP: absencje i kody
        public const int t2APP_IMPORTRCP        = 12;   // import danych RCP z plików CSV

        public const int RIGHTS                 = 15;   // nadanie lub usunięcie uprawnień
        
        public const int APP_IMPORTAD           = 311;  // import z AD; było 3
        public const int t2APP_IMPORTKP         = 4;    // import z KP
        
        public const int t2SENDMAIL             = 101;  // błąd wysłania maila
        public const int t2FILLMAIL             = 102;  // błąd wysłania maila
        public const int t2MAILTOSEND           = 103;  // przygotowany mail do wysłania
        public const int SERVICE                = 201;  // błąd lub informacja serwisu
        public const int EXECAUTO               = 220;  // scheduler...  

        public const int t2SQL                  = 301;  // błąd bazy danych - zalogowany sql z błędem
        public const int PARAMS                 = 302;  // błąd przekazywania parametrów
        //----- dla db -----
        public const int SQL                    = t2SQL;    
        public const int SLOWNIK                = 400;      // edycja danych słownikowych
        public const int PRACOWNIK              = 401;      // edycja danych pracownika

        public const int HAKER                  = 997;      // próba zhakowania ...
        //----- RCP --------------------------
        public const int t2APP_OKRESCLOSE       = 1000;
        public const int t2APP_OKRESREOPEN      = 1001;
        public const int t2APP_ADMREPORTS       = 1002;
        public const int RAPORTY                = 1002;

        public const int t2APP_MONITOKRESENDING = 1003;    // zbliza sie koniec, prosze poakceptować na bieżąco
        public const int t2APP_MONITOKRESACC    = 1004;    // koniec okresu - poakceptować trzeba
        public const int t2APP_MONITREMINDER    = 1005;    // przypominacz

        public const int t2APP_OKRESPARAMS      = 1006;    // zmiana ustawień okresu rozliczeniowego
        public const int APP_SETTINGS           = 1007;    // zmiana ustawień konfiguracyjnych

        public const int t2APP_OKRESCLOSEWEEK   = 1008;
        public const int t2APP_OKRESREOPENWEEK  = 1009;

        public const int OKRES_REOPENDAY        = 1010;    // awaryjne odblokowanie dania
        public const int t2APP_MONITWEEKENDING  = 1011;    // przypominacz o zamknięciu tygodnia
        public const int t2APP_MONITWEEKACC     = 1012;    // ponaglenie o zamknięcie tygodnia

        public const int OKRES_DAYTOREOPEN      = 1013;    // dzień do odblokowania i ponownej akceptacji 
        public const int ZERUJNADM              = 1014;     // zerowanie nadminut

        public const int MONITOKRESNOPP         = 1016;    // przypominacz - brak planu pracy
        public const int WTERRORACC             = 1020;    // akceptacja mimo nieprawidłowości w czasie pracy

        public const int ZASTEPSTWO             = 2000;
        public const int PRZESUNIECIA           = 3000;
        public const int SPLITY                 = 3001;
        public const int ROZLICZENIENG          = 2005;     // rozliczenie nadgodzin
    
        //----- ASSECO ---------------------
        public const int ASSECO                 = 3000;
        public const int IMPORT_ASSECO          = 3001;
        public const int EXPORT_ASSECO          = 3002;
        public const int IMPORT_ASSECO_BHP      = 3010;
        public const int EXPORT_ASSECO_BHP      = 3011;

        //----- KWITKI ---------------------
        public const int PRACLOGIN              = 4000;
        public const int PASSRESET              = 4001;
        public const int KWITEK                 = 4002;
        public const int CHANGEPASS             = 4003;     // user zmienił sobie hasło

        //----- WNIOSKI O NADGODZINY -------
        public const int WNIOSKINADG            = 4020;     

        //----- PODZIAŁ LUDZI ---------------------
        public const int PL_IMPORT              = 5000;
        public const int PL_BACKUP              = 5001;
        public const int PL_PRZELICZ            = 5002;

        //----- BADANIA WST ----------------
        public const int BADWST_IMPORT          = 5100;     // import pracowników

        //----- SZKOLENIA BHP --------------

        //----- PORTAL ---------------------
        public const int _PORTAL                = 6000;
        public const int WNIOSEKURLOPOWY        = 6001;
        public const int WNIOSEKURLOPOWYDEL     = 6002;     // usunięcie przez administratora
        public const int SEARCH                 = 6003;     // wyszukiwana fraza       
        public const int SEARCHSELECT           = 6003;     // wyszukiwana fraza wybór
        public const int PORTAL_ADMIN           = 6004;     // zdarzenia administracyjne

        //----- KIOSK ----------------------
        public const int PRACOWNICY             = 7001;
        public const int KIOSKLOGIN             = 7001;
        public const int PRZYPISZRCP            = 7002;
        public const int PDFTOPNG               = 7003;

        //----- KIOSK ----------------------
        public const int IPO                    = 8001;
        public const int IPO_WALUTY             = 8002;
        
        //----- SCORECARDS -----------------
        public const int SCARDS                 = 1337;
        
        //----- HARMONOGRAM ----------------
        public const int HARMONOGRAM            = 6100;
        public const int EXPORT                 = EXPORT_ASSECO;
        
        public const int DEBUG                  = 9999;

        //----- OBIEG DOKUMENTÓW -----------
        public const int OBIEG                  = 6500;





        /*
        public const int t2APP_STARTWER = 6;    // start weryfikacji danych dla pobranej grupy nowych pracowników
        public const int t2APP_ANKIETAMONIT = 11;//przypomnienie o konieczności uzupełnienia ankiety

        /*
        public const int t2APP_TREEREMOVE = 2;  // usunięcie pracownika lub kierownika ze strukturą pracowników z drzewka
        public const int t2APP_IMPORTAD = 3;    // import z ActiveDirectory
        public const int t2APP_CHECKSTRUCT = 4; // sprawdzenie struktury
        public const int t2APP_REPAIRSTRUCT = 5;// sprawdzenie struktury
        public const int t2APP_PROGSTART = 6;   // start programu
        public const int t2APP_PROGSTOP = 7;    // koniec programu
        public const int t2APP_PROGMONIT = 8;   // przypomnienie o zakończeniu programu
        public const int t2APP_ZASTKONIEC = 9;  // koniec zastępstwa
        public const int t2APP_ZASTMONIT = 10;  // przypomnienie o zakończeniu zastępstwa
        public const int t2APP_ANKIETAMONIT = 11;//przypomnienie o konieczności uzupełnienia ankiety
        */
        /*
        //nowe
        public const int t2APP_WORKERCHANGE = 12;//zmiana danych pracownika
        public const int t2APP_WORKERCREATE = 13;//dodanie pracownika
        public const int t2APP_WORKERDELETE = 14;//usunięcie pracownika

        public const int t2APP_ANKIETAKIERCHANGE = 15; // nastąpiła zmiana kierownika na ankiecie
        public const int t2APP_DELZALACZNIK = 16;// usunięcie załącznika z ankiety
        */

        //----- LOG - Status -----
        public const int OK = 0;        // ok, wykonane

        public const int EXECUTE = 1;   // wykonaj
        public const int PENDING = 2;   // w trakcie wykonywania

        public const int ERROR = -1;
        public const int ERROR_BLOCKED = -2;    // wywołanie zablokowane - funkcja w trakcie realizacji np. service

        //------------------------
        public const int NOPARRENT = 0;

        //-----------------------------------------------------------------
#if PORTAL && SPXxx
        public static int Add(int parentId, int Typ, int? Typ2, string PracId, string Login, string Par, int? Kod, int? Kod2, int? Kod3, int? Kod4, string Info, string Info2, int Status)
        {
            return Base.insertSQL("insert into Log values(GETDATE()," +
                                  parentId.ToString() + "," +
                                  Typ.ToString() + "," +
                                  Base.nullParam(Typ2) + "," +
                                  Base.nullParam(PracId) + "," +
                                  Base.nullStrParam(Login) + "," +
                                  Base.nullStrParam(Par) + "," +
                                  Base.nullParam(Kod) + "," +
                                  Base.nullParam(Kod2) + "," +
                                  Base.nullParam(Kod3) + "," +
                                  Base.nullParam(Kod4) + "," +
                                  Base.nullStrParam(Base.sqlPut(Info, 1800)) + "," +
                                  Base.nullStrParam(Base.sqlPut(Info2, 2000)) + "," +
                                  Status.ToString() + ")", false);
        }

        public static int Add2(SqlConnection con, int parentId, int Typ, int? Typ2, string PracId, string Login, string Par, int? Kod, int? Kod2, int? Kod3, int? Kod4, string Info, string Info2, int Status)
        {
            return db.insert(con, db.insertCmd("Log", 0, null,       //get identity, nie loguj 
                      "GETDATE()",
                      parentId.ToString(),
                      Typ.ToString(),
                      db.nullParam(Typ2),
                      db.nullParam(PracId),
                      db.nullStrParam(Login),
                      db.nullStrParam(Par),
                      db.nullParam(Kod),
                      db.nullParam(Kod2),
                      db.nullParam(Kod3),
                      db.nullParam(Kod4),
                      db.nullStrParam(db.sqlPut(Info, 1800)),
                      db.nullStrParam(db.sqlPut(Info2, 2000)),
                      Status.ToString()), true, false);
        }
#else
        public static int Add(int parentId, int Typ, int? Typ2, string PracId, string Login, string Par, int? Kod, int? Kod2, int? Kod3, int? Kod4, string Info, string Info2, int Status)
        {
            return db.insert((SqlConnection)null, db.insertCmd("Log", 0, null,       //get identity, nie loguj 
                      "GETDATE()",
                      parentId,
                      Typ,
                      db.nullParam(Typ2),
                      db.nullParam(PracId),
                      db.nullStrParam(Login),
                      db.nullStrParam(Par),
                      db.nullParam(Kod),
                      db.nullParam(Kod2),
                      db.nullParam(Kod3),
                      db.nullParam(Kod4),
                      db.nullStrParam(db.sqlPut(Info, 1800)),
                      db.nullStrParam(db.sqlPut(Info2, 2000)),
                      Status,
                      db.nullStrParam(App.ID1), 
                      db.nullStrParam(AppUser.UserIP), 
                      db.nullStrParam(AppUser.SesID)), true, false);
        }

        public static int Add2(SqlConnection con, int parentId, int Typ, int? Typ2, string PracId, string Login, string Par, int? Kod, int? Kod2, int? Kod3, int? Kod4, string Info, string Info2, int Status)
        {
            return db.insert(con, db.insertCmd("Log", 0, null,       //get identity, nie loguj 
                      "GETDATE()",
                      parentId,
                      Typ,
                      db.nullParam(Typ2),
                      db.nullParam(PracId),
                      db.nullStrParam(Login),
                      db.nullStrParam(Par),
                      db.nullParam(Kod),
                      db.nullParam(Kod2),
                      db.nullParam(Kod3),
                      db.nullParam(Kod4),
                      db.nullStrParam(db.sqlPut(Info, 1800)),
                      db.nullStrParam(db.sqlPut(Info2, 2000)),
                      Status,
                      db.nullStrParam(App.ID1),
                      db.nullStrParam(AppUser.UserIP),
                      db.nullStrParam(AppUser.SesID)), true, false);
        }
#endif
        //-----------------------------------------------------------------
        public static string CheckValues(string name, string val1, string val2, string sep)
        {
            if (val1 != val2)
                return String.Format("{0}: '{1}' -> '{2}'", name, val1, val2) + sep;
            else
                return null;
        }

        public static string Value(IOrderedDictionary values, string name)    // insert
        {
            object o = values[name];
            string v = o != null ? o.ToString() : "";
            return String.Format("{0}: {1}", name, v) + "\n";
        }

        public static string Value(IOrderedDictionary oldvalues, IOrderedDictionary values, string name)  // modyfikacja
        {
            if (oldvalues == null)
                return Value(values, name);
            else
            {
                object o1 = oldvalues[name];
                object o2 = values[name];
                string v1 = o1 != null ? o1.ToString() : "";
                string v2 = o2 != null ? o2.ToString() : "";
                return Log.CheckValues(name, v1, v2, "\n");
            }
        }

        public static string GetValues(IOrderedDictionary oldvalues, IOrderedDictionary values)  // modyfikacja
        {
            string v = null;
            if (values != null)
                foreach (string key in values.Keys)
                    v += Value(oldvalues, values, key);
            return v;
        }

        public static void LogChanges(int typ2, string id, EventArgs ea)  // modyfikacja
        {
            IOrderedDictionary oldvalues = null;
            IOrderedDictionary values = null;
            string v = null;
            string oper;
            if (!String.IsNullOrEmpty(id)) id += " - ";
            if (ea is ListViewInsertEventArgs)          // insert
            {
                values = ((ListViewInsertEventArgs)ea).Values;
                oper = "Insert";
            }
            else if (ea is ListViewUpdateEventArgs)     // update
            {
                if (((ListViewUpdateEventArgs)ea).Cancel)
                {
                    Info(typ2, id + "Canceled", null, Log.OK);
                    return;    
                }
                v = GetValues(null, ((ListViewUpdateEventArgs)ea).Keys);
                oldvalues = ((ListViewUpdateEventArgs)ea).OldValues;
                values = ((ListViewUpdateEventArgs)ea).NewValues;
                oper = "Update";
            }
            else if (ea is ListViewDeleteEventArgs)
            {
                v = GetValues(null, ((ListViewDeleteEventArgs)ea).Keys);
                oper = "Delete";
            }
            else
            {
                oper = "Unknown";
            }
            v += GetValues(oldvalues, values);
            Info(typ2, id + oper, v, Log.OK);
        }

        public static void LogChanges(object ctrl, EventArgs ea)  
        {
            LogChanges(SLOWNIK, ((Control)ctrl).ClientID, ea);
        }
        //-----------------------------------------------------------------
        public static void Update(int LogId, int NewStatus)
        {
            Base.execSQL("update Log set Status = " + NewStatus.ToString() +
                        " where Id = " + LogId
                        );
        }

        public static void Update(int LogId, int Kod, int NewStatus)
        {
            Base.execSQL("update Log set Status = " + NewStatus.ToString() +
                        ", Kod = " + Kod.ToString() +
                        " where Id = " + LogId
                        );
        }

        public static void Update(int LogId, string Info, int NewStatus)
        {
            Base.execSQL("update Log set Status = " + NewStatus.ToString() +
                        ", Info = " + Base.nullStrParam(Base.sqlPut(Info,1800)) + 
                        " where Id = " + LogId
                        );
        }

        public static void Update2(int LogId, string Info2, int NewStatus)
        {
            Base.execSQL("update Log set Status = " + NewStatus.ToString() +
                        ", Info2 = " + Base.nullStrParam(Base.sqlPut(Info2,2000)) +
                        " where Id = " + LogId
                        );
        }
        //---------------------------------------------------------------
        private class AppUserLog
        {
            public string Id;
            public string Login;
            
            public AppUserLog() 
            {
                Login = AppUser.GetLogin();
                Id = Base.getScalar("select Id from Pracownicy P where UPPER(Login) = " + Base.strParam(Login.ToUpper()), false);
                //Id = db.getScalar("select Id from Pracownicy P where UPPER(Login) = " + Base.strParam(Login.ToUpper()), false);  Log jest tez wywoływany tam gdzie nie ma MasterPage z con
            }

            /*
            public AppUserLog(SqlConnection con)
            {
                Login = AppUser.GetLogin();
                Id = db.getScalar(con, "select Id from Pracownicy where Login = " + db.strParam(Login), false);  // stringi sa porównywane bez uppercase przy std ustawieniach mssql
            }
            */

            public AppUserLog(SqlConnection con)
            {
                Login = AppUser.GetLogin();
                DataRow dr = db.getDataRow(con, "select Id from Pracownicy where Login = " + db.strParam(Login));
                if (dr != null)
                    Id = db.getValue(dr, "Id");
                else
                    Id = null;
            }

            public AppUserLog(bool tr)
            {
                Login = AppUser.GetLogin();
                string select = "select Id from Pracownicy where Login = " + db.strParam(Login);
                if (tr)
                    Id = Base.getScalar(null, select, false);  // stringi sa porównywane bez uppercase przy std ustawieniach mssql
                else
                    Id = db.getScalar(select, false);  // stringi sa porównywane bez uppercase przy std ustawieniach mssql, zakładam, że transakcje sa z masterpage i jest con
            }
        }
        //---------------------------------------------------------------
        public static int Info(AppUser user, int Typ2, string Info, string Info2, int Status)
        {
            return Add(0, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, Status);
        }

        public static int Info(int Typ2, string Info, string Info2, int Status)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, Status);
        }

        public static int Info(int Typ2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }

        public static int Info(int Typ2, string par, int kod, string Info, string Info2, int Status)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tINFO, Typ2, user.Id, user.Login, par, kod, null, null, null, Info, Info2, Status);
        }

        public static int Info(int Typ2, int ParentId, string Info, string Info2, int Status)
        {
            AppUserLog user = new AppUserLog();
            return Add(ParentId, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, Status);
        }

        public static int Info(int Typ2, int ParentId, string par, int? kod, string Info, string Info2, int Status)
        {
            AppUserLog user = new AppUserLog();
            return Add(ParentId, tINFO, Typ2, user.Id, user.Login, par, kod, null, null, null, Info, Info2, Status);
        }
        //----------------
        public static int Info2(int Typ2, string Info, string Info2)
        {
            bool fcon = App.Master == null;
            SqlConnection con2 = null;

            if (fcon) db.DoConnect(ref con2);
            else con2 = db.con;
            try
            {
                AppUserLog user = fcon ? new AppUserLog(con2) : new AppUserLog();
                return Add2(con2, NOPARRENT, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
            }
            finally
            {
                if (fcon) db.DoDisconnect(ref con2);
            }
        }
        //----------------
        public static int InfoTr(int Typ2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog(true);
            //return Add(null, NOPARRENT, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
            return Add(NOPARRENT, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }

        public static int InfoTr(int Typ2, int ParentId, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog(true);
            //return Add(null, ParentId, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
            return Add(ParentId, tINFO, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }
        //-------------------------------------------------
        public static int AddJob(int Typ2, string Par, int Kod, int Kod2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tJOB, Typ2, user.Id, user.Login, Par, Kod, Kod2, null, null, Info, Info2, EXECUTE);
        }
        
        //-----------
        public static int Error(AppUser user, int Typ2, string Info)
        {
            return Add(0, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, null, OK);
        }

        public static int Error(int Typ2, string Info)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, null, OK);
        }

        public static int Error(AppUser user, int Typ2, string Info, string Info2)
        {
            return Add(0, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }

        public static int Error(int Typ2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }

        public static int Error(int Typ2, string Info, string Info2, string par)
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tERROR, Typ2, user.Id, user.Login, par, null, null, null, null, Info, Info2, OK);
        }

        public static int Error(int Typ2, string Info, string Info2, int kod)  // kod do lokalizacji miejsca w źródle
        {
            AppUserLog user = new AppUserLog();
            return Add(0, tERROR, Typ2, user.Id, user.Login, null, kod, null, null, null, Info, Info2, OK);
        }

        public static int Error(int Typ2, int ParentId, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog();
            return Add(ParentId, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }
        //------------------
        public static int ErrorTr(int Typ2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog(true);
            return Add(NOPARRENT, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }

        //---------------------------------------------------------------
        /*
        public static int ErrorTr(int Typ2, string Info, string Info2)
        {
            AppUserLog user = new AppUserLog();
            return Add(NOPARRENT, tERROR, Typ2, user.Id, user.Login, null, null, null, null, null, Info, Info2, OK);
        }
         */ 
    }
}

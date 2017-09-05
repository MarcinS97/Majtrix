using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace HRRcp.App_Code
{
    public class Ustawienia
    {
        public const string sesIdUstawienia = "settings";  // identyfikator do sesji

        public const int DefZaokr = 60;     // 0 - bez, min
        public const int DefZaokrType = 2;  // w górę
        public const int DefPrzerwa = 15;   // min
        public const int DefPrzerwa2 = 0;   // min - przerwa dla nadgodzin
        public const int DefMargines = 15;  // margines ostrzegania
        public const int DefStartDoby = 0;  // -4 h przesunięcie względem 00:00:00

        public const int zaokrDown = 0;
        public const int zaokr05 = 1;
        public const int zaokrUp = 2;
        public const int zaokrUp1 = 3;
        public const int zaokrUp5 = 4;
        public const int zaokrUp15 = 5;
        public const int zaokrUp30 = 6;
        
        DataRow data = null;
        
        public Ustawienia()
        {
        }

        public static Ustawienia CreateOrGetSession(string sesName)
        {
            Ustawienia u; 
            if (HttpContext.Current.Session[sesName] == null)
            {
                u = new Ustawienia();
                HttpContext.Current.Session[sesName] = u;
                return u;
            }
            else
            {
                u = (Ustawienia)HttpContext.Current.Session[sesName];
                if (u.data == null)     // jak sesja wygasnie to jaka ma wartosc ???
                {                       // zakładam, że data było wypełnione 
                    u = new Ustawienia();
                    HttpContext.Current.Session[sesName] = u;
                }
                return u;
            }
        }

        /*
        public static Ustawienia CreateOrGetSession(string sesName)
        {
            if (HttpContext.Current.Session[sesName] == null)
            {
                Ustawienia p = new Ustawienia();
                HttpContext.Current.Session[sesName] = p;
                return p;
            }
            else
                return (Ustawienia)HttpContext.Current.Session[sesName];
        }
         */

        public static Ustawienia CreateOrGetSession()
        {
            return CreateOrGetSession(sesIdUstawienia);
        }

        //----------------
        public void Reread()
        {
            data = null;
        }

        public int Update(string tbADController, string tbADPath)
        {
            int ret = 0;
            // tu validate i zmiana ret
            if (ret == 0)
            {
                bool ok = Base.execSQL("update Ustawienia set " +
                                    Base.updateStrParam("ADKontroler", tbADController) +
                                    Base.updateStrParamLast("ADPath", tbADPath) +
                                "where Id = " + Id);
                Reread();
                if (!ok)
                    ret = -1;
            }
            return ret;
        }

        public int Update(string smtp, string email, string applink,string monitDniPrzed)
        {
            int dni;
            int ret = 0;
            //----- walidacja -----
            if (!Int32.TryParse(monitDniPrzed, out dni))
                ret = -2;
            else
            //----- zapis -----
            {
                string fields = "SMTPSerwer,Email,AppAddr,MonitDniPrzed";
                bool ok = db.update("Ustawienia", 0, fields,"Id=" + Id,
                                    db.strParam(smtp), 
                                    db.strParam(email), 
                                    db.strParam(applink), 
                                    monitDniPrzed);
                Log.Info(Log.APP_SETTINGS, "Ustawienia - Modyfikacja", String.Format(db.PrepareUpdateParams(0, fields),
                                    db.strParam(smtp),
                                    db.strParam(email),
                                    db.strParam(applink),
                                    monitDniPrzed));
                Reread();
                if (!ok) ret = -1;
            }
            return ret;
        }


        /*
        public int Update(string tbSMTP, string tbEmail, string tbMonitDniPrzed)
        {
            int ret = 0;
            // tu validate i zmiana ret
            if (ret == 0)
            {
                bool ok = Base.execSQL("update Ustawienia set " +
                                    Base.updateStrParam("SMTPSerwer", tbSMTP) +
                                    Base.updateStrParam("Email", tbEmail) +
                                    Base.updateStrParamLast("MonitDniPrzed", tbMonitDniPrzed) +
                                "where Id = " + Id);
                Reread();
                if (!ok)
                    ret = -1;
            }
            return ret;
        }
         */
        //-----------------
        private DataRow getData()
        {
            const string sql = "select top 1 * from Ustawienia";
            if (data == null)
            {
                SqlConnection con = Base.Connect();
                data = Base.getDataRow(con, sql);
                if (data == null)
                {                           // brak rekordu - dopisuję pusty
                    Base.execSQL(con, "insert into Ustawienia (Konserwacja) values (0)");  // insertSQL nie moze tu byc bo nie ma autoinc pola !!!
                    data = Base.getDataRow(con ,sql);
                }
                Base.Disconnect(con);
            }
            return data;
        }

        public static bool IsKonserwacja()
        {
            DataRow dr = Base.getDataRow("select top 1 Konserwacja from Ustawienia");
            return Base.getBool(dr, 0, false);
        }

        public string GetSupervisorEMail(SqlConnection con)  // mozna tylko tak spr, bez spr IdSupervisor is null
        {
            string sid = IdSupervisor;
            if (!String.IsNullOrEmpty(sid))   // tylko administratorowi !!! na wszelki wypadek
            {
                return Base.getScalar(con, "select Email from Pracownicy where Administrator = 1 and Id = " + sid);
            }
            else
                return null;
        }
        //----------------
        public bool UpdateKwitekDo(string dDo)  // listy widoczne po dacie wypłąty do ...
        {
            string fields = "KwitekDo";
            string prevDo = db.getValue(Data, "KwitekDo");  // bo moze być null
            bool ok = db.update("Ustawienia", 0, fields, "Id=" + Id,
                                db.strParam(dDo));
            Log.Info(Log.APP_SETTINGS, "Ustawienia - Modyfikacja",
                String.Format(db.PrepareUpdateParams(0, fields), db.strParam(prevDo)) + "\n" +
                String.Format(db.PrepareUpdateParams(0, fields), db.strParam(dDo)));
            Reread();
            return ok;
        }

        //----------------
        public DataRow Data
        {
            get { return getData(); }
        }

        public string Id
        {
            get { return Base.getValue(Data, "Id"); }
        }

        public bool Konserwacja
        {
            get { return IsKonserwacja(); }
        }

        public string ADKontroler
        {
            get { return Base.getValue(Data, "ADKontroler"); }
        }

        public string ADPath
        {
            get { return Base.getValue(Data, "ADPath"); }
        }
        //---------------------------------

        public string SMTPData
        {
            get { return db.getValue(Data, "SMTPSerwer"); }
        }

        public string _SMTPSerwer       // server:port;user;pass
        {
            get
            {
                string smtp = SMTPData;
                if (!String.IsNullOrEmpty(smtp))
                {
                    string[] s = smtp.Split(';', ',');
                    return s[0].Split(':')[0];
                }
                return null;
            }
        }

        public int SMTPPort
        {
            get
            {
                string smtp = SMTPData;
                if (!String.IsNullOrEmpty(smtp))
                {
                    string[] s = smtp.Split(';', ',');
                    string[] p = s[0].Split(':');
                    if (p.Count() > 1)
                        return Tools.StrToInt(p[1], 0);
                }
                return 0;
            }
        }

        public string SMTPUser
        {
            get
            {
                string smtp = SMTPData;
                if (!String.IsNullOrEmpty(smtp))
                {
                    string[] s = smtp.Split(';', ',');
                    if (s.Count() > 1)
                        return s[1];
                }
                return null;
            }
        }

        public string SMTPPass
        {
            get
            {
                string smtp = SMTPData;
                if (!String.IsNullOrEmpty(smtp))
                {
                    string[] s = smtp.Split(';', ',');
                    if (s.Count() > 2)
                        return s[2];
                }
                return null;
            }
        }

        public string x_SMTPSerwer
        {
            get { return Base.getValue(Data, "SMTPSerwer"); }
        }
        //---------------------------------
        public string Email
        {
            get { return Base.getValue(Data, "Email"); }
        }

        public string AppAddr
        {
            get { return Base.getValue(Data, "AppAddr"); }
        }

        public int MonitDniPrzed     // 0-wysle 20, 1-wysle 19, itd jezeli koniec okresu na 20
        {
            get { return Base.getInt(Data, "MonitDniPrzed", -1); }   
        }

        public string IdSupervisor
        {
            get { return Base.getValue(Data, "IdSupervisor"); }
        }
        //---------------------
        public int OkresOd
        {
            get { return Base.getInt(Data, "OkresOd", 1); }
        }

        public int OkresDo
        {
            get { return Base.getInt(Data, "OkresDo", 31); }
        }

        public int NocneOdSec
        {
            get { return Base.getInt(Data, "NocneOd", 0); }
        }

        public int NocneDoSec
        {
            get { return Base.getInt(Data, "NocneDo", 0); }
        }
        //---------
        public int Zaokr
        {
            get { return Base.getInt(Data, "Zaokr", DefZaokr); }
        }

        public int ZaokrType
        {
            get { return Base.getInt(Data, "ZaokrType", DefZaokrType); }
        }

        public int ZaokrSum
        {
            get { return Base.getInt(Data, "ZaokrSum", DefZaokr); }
        }

        public int ZaokrSumType
        {
            get { return Base.getInt(Data, "ZaokrSumType", DefZaokrType); }
        }
        //---------
        public int Przerwa
        {
            get { return Base.getInt(Data, "PrzerwaMM", DefPrzerwa); }
        }

        public int Przerwa2
        {
            get { return Base.getInt(Data, "Przerwa2MM", DefPrzerwa2); }
        }

        public int Margines
        {
            get { return Base.getInt(Data, "MarginesMM", DefMargines); }
        }

        /*
        public int StartDoby
        {
            get { return Base.getInt(Data, "StartDoby", DefStartDoby); }
        }
        */ 
        //-----------
        public DateTime SystemStartDate
        {
            get
            {
                DateTime dt;
                if (Base.getDateTime(Data, "StartSystemu", out dt))
                    return dt;
                else
                    return new DateTime(2011, 12, 1); // na sztywno :)
            }
        }

        public DateTime KwitekDo    // kwitek widoczny od...
        {
            get
            {
                DataRow dr = db.getDataRow("select top 1 KwitekDo from Ustawienia");
                if (dr != null)
                {
                    DateTime dt;
                    if (Base.getDateTime(dr, "KwitekDo", out dt))
                        return dt;
                }
                return DateTime.Today;
            }
        }
    }
}

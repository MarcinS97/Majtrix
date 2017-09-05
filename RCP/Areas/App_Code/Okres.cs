using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using HRRcp.App_Code;
using HRRcp.Controls;

namespace HRRcp.App_Code
{
    public class Okres
    {
        //public const int stNoData = -2;      // wcześniejszy niż data startu systemu
        
        public const int stNotExists = -1;
        public const int stOpen = 0;
        public const int stClosed = 1;



        int FId;                // -1 jak nie ma
        DateTime FDateFrom;
        DateTime FDateTo;
        DateTime FLockedTo;
        DateTime FDataNaliczenia;
        DateTime? FDataNaliczeniaPrev = null;
        double FStawkaNocna;
        int _FStatus;
        int FStatusPL;
        string FZamknalId;      // IdPRacownika, który zamknął lub null 
        SqlConnection Fcon = null;

        public Okres(DateTime dt)
        {
            Fcon = null;
            Prepare(dt);
        }

        public Okres(SqlConnection con, DateTime dt)
        {
            Fcon = con;
            Prepare(dt);
        }

        public Okres(SqlConnection con, string oid)
        {
            Fcon = con;
            Prepare(oid);
        }

        public Okres(DataRow dr, DateTime dt)
        {
            Fcon = null;
            FillData(dr, dt);
        }

        //----------------------------
        public static string GetStatus(int st)
        {
            switch (st)
            {
                case stNotExists: return "Brak danych";
                case stOpen: return "Otwarty";
                case stClosed: return "Zamknięty";
                default: return String.Format("Inny ({0})", st);
            }
        }

        public static void FillStatus(DropDownList ddl, string select)
        {
            if (ddl != null)
            {
                ddl.Items.Clear();
                ddl.Items.Add(new ListItem(GetStatus(stOpen), stOpen.ToString()));
                ddl.Items.Add(new ListItem(GetStatus(stClosed), stClosed.ToString()));
                Tools.SelectItem(ddl, select);
            }
        }

        public static int? GetId(SqlConnection con, DateTime dt)
        {
            Okres ok = new Okres(con, dt); 
            return ok.IsArch() ? (int?)ok.Id : null;
        }

        public string FriendlyName(int typ)  // to samo co w SelectOkres
        {
            /*
            switch (typ)
            {
                default:
                case 1:
                    return Tools.MonthName[FDateTo.Month] + " '" + FDateTo.Year.ToString();
            }
            */
            return Tools.DateFriendlyName(typ, FDateTo);
        }

        public static bool IsArch(string okresId)
        {
            string id = Base.getScalar("select top 1 Id from PracownicyOkresy where IdOkresu = " + okresId);
            return !String.IsNullOrEmpty(id);                    
        }
        //----------------------------
        public static Okres First(SqlConnection con)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DataRow dr = Base.getDataRow(con, "select top 1 * from OkresyRozl order by DataOd");
            if (dr == null)
                return new Okres(con, settings.SystemStartDate);
            else
                return new Okres(dr, settings.SystemStartDate);  // jakby nie poszło - wezmie start systemu
        }

        public static Okres Last(SqlConnection con)
        {
            DataRow dr = Base.getDataRow(con, "select top 1 * from OkresyRozl order by DataOd desc");
            if (dr == null)
                return new Okres(con, DateTime.Today);
            else
                return new Okres(dr, DateTime.Today);  // jakby nie poszło - wezmie dzisiejszy
        }

        public static Okres Current(SqlConnection con)  // zawsze otwarty !!! - do pokazywania danych
        {
            return new Okres(con, DateTime.Today);
        }

        public static Okres FirstToClose(SqlConnection con)     // null jak się nie kwalifikuje ...
        {
            Okres ok = LastClosed(con);
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            if (ok == null)
                ok = new Okres(con, settings.SystemStartDate);      // najpierw zamykam pierwsze okresy
            else
                ok.Next();                                          // biorę następny
            Okres cok = Current(con);
            if (ok.FDateTo < cok.FDateFrom) // ok, nie jest bieżącym, który musi być otwarty !!!
                return ok;
            else
                return null;
        }

        public static Okres LastClosed(SqlConnection con)
        {
            DataRow dr = Base.getDataRow(con, "select top 1 * from OkresyRozl where Status=1 order by DataOd desc");
            if (dr == null)
                return null;
            else
                return new Okres(dr, DateTime.Today);  // jakby nie poszło - wezmie dzisiejszy
        }

        public static Okres LastAccessible(SqlConnection con)
        {
            Okres ok = Current(con);
            ok.Prev();
            if (ok.Status == stClosed)
                return Current(con);
            else
                return ok;
        }
        //----------------------------
        public void Prepare(DateTime dt)
        {
            dt = dt.Date;  // !!! ważne - odcinam time !!!
            DataRow dr = Base.getDataRow(Fcon, String.Format(
                "select * from OkresyRozl where '{0}' between DataOd and DataDo",
                    Base.DateToStr(dt)));
            FillData(dr, dt);
        }

        public void Prepare(string oid)
        {
            if (String.IsNullOrEmpty(oid))
                Prepare(DateTime.Today);
            else
            {
                DataRow dr = Base.getDataRow(Fcon, String.Format(
                    "select * from OkresyRozl where Id = " + oid));
                FillData(dr, DateTime.Today);
            }
        }

        public void FillData(DataRow dr, DateTime dt)  // na dzień, dt bez czasu
        {
            if (dr != null)
            {
                FId = Base.getInt(dr, "Id", -1);
                _FStatus = Base.getInt(dr, "Status", stNotExists);
                FZamknalId = Base.getValue(dr, "Zamknal");
                FStawkaNocna = Base.getDouble(dr, "StawkaNocna", -1);
                FStatusPL = db.getInt(dr, "StatusPL", 0);
            }
            DateTime dOd, dDo;
            if (dr == null ||
                !Base.getDateTime(dr, "DataOd", out dOd) ||
                !Base.getDateTime(dr, "DataDo", out dDo))   // brak lub problem z datami, to zwraca wg dt
            {
                Ustawienia ust = Ustawienia.CreateOrGetSession();
                int defDo = ust.OkresDo;
                int day = dt.Day;
                if (defDo >= 28)  // a nie 31 bo inaczej sie nie pozbieram ...
                {
                    dOd = Tools.bom(dt);
                    dDo = Tools.eom(dt);
                }

                else if (day > defDo)
                {
                    dOd = dt.AddDays(-day + defDo + 1); // następny dzień
                    dDo = dOd.AddMonths(1).AddDays(-1);  // + miesiac - dzień
                }
                else
                {
                    dDo = dt.AddDays(-day + defDo);     // ostatni dzień
                    dOd = dDo.AddMonths(-1).AddDays(1); // minus miesiąc + dzień
                }
                FId = -1;
                _FStatus = stNotExists;
                FZamknalId = null;
                FStawkaNocna = -1;
                FStatusPL = 0;
            }

            if (dr == null || !Base.getDateTime(dr, "DataBlokady", out FLockedTo))
                if (_FStatus == stClosed)
                    FLockedTo = dDo;
                else
                    FLockedTo = dOd.AddDays(-1);

            FDateFrom = dOd;
            FDateTo = dDo;

            FDataNaliczenia = db.getDateTime(dr, "DataNaliczenia", FDateTo); /* jak dr null to default */
            FDataNaliczeniaPrev = null;
        }
        //------------------------
        public bool Jump(int month)
        {
            DateTime prevdt = FDateFrom;
            DateTime dt = FDateFrom.AddMonths(month);
            Prepare(dt);
            return prevdt != FDateFrom;
        }

        public bool _First()
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            DateTime prevdt = FDateFrom;
            DataRow dr = Base.getDataRow(Fcon, "select top 1 * from OkresyRozl order by DataOd");
            if (dr == null) 
                Prepare(settings.SystemStartDate);
            else
                FillData(dr, settings.SystemStartDate);  // jakby nie poszło - wezmie start systemu
            return prevdt != FDateFrom;
        }

        public bool _Last()
        {
            DateTime prevdt = FDateFrom;
            DataRow dr = Base.getDataRow(Fcon, "select top 1 * from OkresyRozl order by DataOd desc");
            if (dr == null)
                Prepare(DateTime.Today);
            else
                FillData(dr, DateTime.Today);  // jakby nie poszło - wezmie dzisiejszy
            return prevdt != FDateFrom;
        }

        public bool _Current()
        {
            DateTime prevdt = FDateFrom;
            Prepare(DateTime.Today);
            return prevdt != FDateFrom;
        }

        public bool Next()
        {
            DateTime prevdt = FDateFrom;
            DataRow dr = Base.getDataRow(Fcon,
                "select top 1 * from OkresyRozl where DataOd > " + Base.strParam(Base.DateToStr(FDateFrom)) +  // poczatek od poczatku - jezeli zachodzilyby na siebie to załapię - chociaż to nie powinno miec miejsca
                " order by DataOd");
            if (dr == null)     // nie ma, biorę + miesiąc
                return Jump(1);
            else                // znalazł
                FillData(dr, DateTime.Today);  // jakby nie poszło - wezmie dzisiejszy
            return prevdt != FDateFrom;
        }

        public bool Prev()
        {
            DateTime prevdt = FDateFrom;
            DataRow dr = Base.getDataRow(Fcon,
                "select top 1 * from OkresyRozl where DataOd < " + Base.strParam(Base.DateToStr(FDateFrom)) +  // poczatek od poczatku - jezeli zachodzilyby na siebie to załapię - chociaż to nie powinno miec miejsca
                " order by DataOd desc");
            if (dr == null)     // nie ma, biorę - miesiąc
                return Jump(-1);
            else                // znalazł
                FillData(dr, FDateFrom);  // jakby nie poszło - wezmie obecny i nie zaznaczy ze zmiana nastąpiła - zwroci false
            return prevdt != FDateFrom;
        }

        //----- akceptacja czasu pracy ---------------------------------
        public static void _UpdatePPAcc(string pracId, DateTime date, DataRow dr,
                             DateTime? czasIn, DateTime? czasOut, int? wt, int? zt, int? otD, int? otN, int? nt,
                             int alerty,
                             string accKierId,
                             DateTime? czasIn2)      // wpisuje czasy jeżeli nie są ustawione przez kier, czasIn2 czas wpisany przez kierownika jesli nie to rcp jesli null to będzie brany ze zmiany - do liczenia nadgodziny przed 6:00 w poniedziałek
        {
            bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
            bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
            bool k_CzasZm = Base.getBool(dr, "k_CzasZm", false);
            bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
            bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
            bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
            bool acc = Base.getBool(dr, "Akceptacja", false);
            bool exists = !Base.isNull(dr, "PPId");

            //----- 50 i 100 -----
            int n50 = 0;
            int n100 = 0;
            PlanPracy.TimeSumator ts = null;
            string zmianaId = db.getValue(dr, "ZmianaId");
            
            
            
            //int before6 = Worktime.GetBefore6(czasIn, zt ?? 0);   // zt moze być -1 ale wtedy nie weźmie go GetBefore6 pod uwagę, jak null to 

            int d50, d100;
            bool zeroZm;
            PlanPracy.SolveNadgodziny(ref ts, date, zmianaId,
                db.ISNULL(zt, 0),
                db.ISNULL(otD, 0),
                db.ISNULL(otN, 0),
                db.ISNULL(nt, 0), 
                out n50, out n100,
                false,
                czasIn2,
                out d50, out d100, out zeroZm); 


            if (!exists)
            {
                //----- insert -----
                string fields = null;
                string values = null;
                if (!k_CzasIn)
                {
                    fields += "CzasIn,";
                    values += (czasIn == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn))) + ",";
                }
                if (!k_CzasOut)
                {
                    fields += "CzasOut,";
                    values += (czasOut == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut))) + ",";
                }
                if (!k_CzasZm)    // czas nie jest wprowadzony przez kierownika 
                {
                    fields += "CzasZm,";
                    values += (zt == null ? Base.NULL : zt.ToString()) + ",";
                }
                if (!k_NadgDzien)
                {
                    fields += "NadgodzinyDzien,";
                    values += (otD == null ? Base.NULL : otD.ToString()) + ",";
                }
                if (!k_NadgNoc)
                {
                    fields += "NadgodzinyNoc,";
                    values += (otN == null ? Base.NULL : otN.ToString()) + ",";
                }
                if (!k_Nocne)
                {
                    fields += "Nocne,";
                    values += (nt == null ? Base.NULL : nt.ToString()) + ",";
                }


                db.execSQL("insert into PlanPracy (" +
                              fields + "IdPracownika,Data,Czas,IdKierownikaAcc,DataAcc,Akceptacja,n50,n100,d50,d100,Alerty) values (" + 
                              values + 
                                Base.insertParam(pracId) +
                                Base.insertStrParam(Base.DateToStr(date)) +
                                Base.insertParam(Base.nullParam(wt)) +
                                Base.insertParam(accKierId) +
                                Base.insertParam("GETDATE()") + 
                                Base.insertParam("1") +
                                Base.insertParam(n50.ToString()) +
                                Base.insertParam(n100.ToString()) +
                                Base.insertParam(d50.ToString()) +
                                Base.insertParam(d100.ToString()) +
                                Base.insertParamLast(alerty.ToString()) +
                                ")");
            }
            else   
                if (!acc)  // akceptuję tylko jeżeli nie są jeszcze zaakaceptowane
                {
                    //----- update -----
                    string set = null;
                    if (!k_CzasIn)      set += "CzasIn="            + (czasIn   == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasIn)))    + ",";
                    if (!k_CzasOut)     set += "CzasOut="           + (czasOut  == null ? Base.NULL : Base.strParam(Base.DateTimeToStr(czasOut)))   + ",";
                    if (!k_CzasZm)      set += "CzasZm="            + (zt       == null ? Base.NULL : zt.ToString())    + ",";
                    if (!k_NadgDzien)   set += "NadgodzinyDzien="   + (otD      == null ? Base.NULL : otD.ToString())   + ",";
                    if (!k_NadgNoc)     set += "NadgodzinyNoc="     + (otN      == null ? Base.NULL : otN.ToString())   + ",";
                    if (!k_Nocne)       set += "Nocne="             + (nt       == null ? Base.NULL : nt.ToString())    + ",";

                    db.execSQL("update PlanPracy set " + set + 
                                        Base.updateParam("Czas", Base.nullParam(wt)) +
                                        Base.updateParam("IdKierownikaAcc", accKierId) +
                                        Base.updateParam("DataAcc", "GETDATE()") +
                                        Base.updateParam("Akceptacja", "1") +
                                        Base.updateParam("n50", n50) +
                                        Base.updateParam("n100", n100) +
                                        Base.updateParam("d50", d50) +
                                        Base.updateParam("d100", d100) +
                                        Base.updateParamLast("Alerty", alerty.ToString()) +
                        "where IdPracownika = " + pracId + " and Data = " + Base.strParam(Base.DateToStr(date)));

                    /* wnioski o nadgodziny - solution of the ancients */
//                    try
//                    {
//                        db.execSQL(String.Format(@"
//declare @pracId int = {0}
//declare @date datetime = '{1}'
//declare @accKierId int = {2}
//
//insert into PodzialNadgodzin (IdPracownika, Data, RodzajId, Uwagi, DataWpisu, AutorId, CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne, n50, n100, IdWnioskuNadg)
//select nw.IdPRacownika, nw.Data, nw.RodzajId, nw.Uwagi, GETDATE(), @accKierId, pp.CzasZm, pp.NadgodzinyDzien, pp.NadgodzinyNoc, pp.Nocne
//  , case when nw.Nadg50 = pp.n50 then pp.n50
//		 when nw.Nadg50 < pp.n50 then nw.Nadg50
//		 when nw.Nadg50 > pp.n50 then pp.n50
//		else 0 end
//  , case when nw.Nadg100 = pp.n100 then pp.n100
//		 when nw.Nadg100 < pp.n100 then nw.Nadg100
//		 when nw.Nadg100 > pp.n100 then pp.n100
//		else 0 end
//  , nw.Id
//from rcpNadgodzinyWnioski nw
//inner join PlanPracy pp on pp.Data = nw.Data and pp.IdPracownika = nw.IdPracownika
//where nw.Data = @date and nw.IdPracownika = @pracId and nw.Status = 2 and nw.RodzajId = 1
//", pracId, date, accKierId));
//                    }
//                    catch { }
                }
        }
        //--------------------------
        public static void UpdatePPNN(string pracId, DateTime date, DataRow dr,
                             DateTime? czasIn, DateTime? czasOut, int? wt, int? zt, int? otD, int? otN, int? nt,
                             int alerty,
                             string accKierId,
                             DateTime? czasIn2)      //tylko dla zaakceptowanych
        {
            bool exists = !Base.isNull(dr, "PPId");
            if (exists)
            {
                bool acc = Base.getBool(dr, "Akceptacja", false);
                //----- 50 i 100 -----
                int n50 = 0;
                int n100 = 0;
                PlanPracy.TimeSumator ts = null;
                string zmianaId = db.getValue(dr, "ZmianaId");


                //int before6 = Worktime.GetBefore6(czasIn, zt ?? 0);
                
                int d50, d100;
                bool zeroZm;
                PlanPracy.SolveNadgodziny(ref ts, date, zmianaId,
                    db.ISNULL(zt, 0),
                    db.ISNULL(otD, 0),
                    db.ISNULL(otN, 0),
                    db.ISNULL(nt, 0),
                    out n50, out n100,
                    false,
                    czasIn2,
                    out d50, out d100, out zeroZm
                    );

                db.execSQL("update PlanPracy set " +
                                    Base.updateParam("n50", n50) +
                                    Base.updateParam("n100", n100) +
                                    Base.updateParam("d50", d50) +
                                    Base.updateParam("d100", d100) +
                                    Base.updateParamLast("Alerty", alerty.ToString()) +
                    "where IdPracownika = " + pracId + " and Data = " + Base.strParam(Base.DateToStr(date)));
            }
        }
        //--------------------------
        public static int AkceptujOkres(string kierId, string accKierId, KierParams kp,
                                    string DataOd, string DataDo,
                                    DateTime accTo,         // do kiedy akceptować
                                    bool errAcc)            // errAcc - akceptuj mimo ostrzeżeń, jak false to niezaakceptuje, chyba ze nie ma błędów
        {
            return _Akceptuj(kierId, null, accKierId, kp, DataOd, DataDo, null, accTo, false, false, null, errAcc);
        }

        public static int AkceptujOneDay(string pracId, string DataOd, string DataDo, string onDay, string accKierId, bool errAcc)   // DataOd, do - okres, errAcc - akceptuj mimo ostrzeżeń, jak false to niezaakceptuje
        {
            return _Akceptuj(null, pracId, accKierId, null, DataOd, DataDo, onDay, null, false, false, null, errAcc);
        }

        public static bool _CofnijAcc(SqlConnection con, string ppId, int alerty, string kierAccId, bool update, bool log, string logInfo)
        {
            string u = update ? ",IdKierownikaAcc,DataAcc" : null;
            bool ok = Base.execSQL(con, Base.updateSql("PlanPracy", 1,
                "Akceptacja,Alerty,DataEksportu" + u,
                "Id={0}",
                ppId, "0", alerty, db.NULL, kierAccId, "GETDATE()"
                ));     // true jesli wykonał
            if (log && ok)
            {
                DataRow dr = Base.getDataRow(con, @"
select PP.Data, P.Nazwisko + ' ' + P.Imie as Pracownik, P.KadryId from PlanPracy PP 
left outer join Pracownicy P on P.Id = PP.IdPracownika
where PP.Id = " + ppId); 
                if (String.IsNullOrEmpty(logInfo)) logInfo = "Odblokowanie dnia";
                Log.Info(Log.OKRES_REOPENDAY, logInfo, 
                    String.Format("Data: {0} Pracownik: {1} {2} Alerty: {3:X8}", 
                        Tools.DateToStr(db.getDateTime(dr, 0, DateTime.MinValue)),
                        db.getValue(dr, 2),
                        db.getValue(dr, 1),
                        alerty
                    ));
            }
            return ok;
        }


        public static bool x_CheckAkceptuj(DataRow dr, StringWriter sw)
        {



                    /*
                    "select D.Lp, D.Data, null as TimeIn, null as TimeOut, " +
                        "null as Czas1sec, " +
                        "null as Czas1, " +
                        //"null as Czas1Rsec, " +
                            "null as Czas1R, " +
                        
                        "null as Czas2sec, " +
                        "null as Czas2, " +
                        //"null as Czas2Rsec, " +
                            "null as Czas2R, " +

                        "null as Nocne1sec, " +
                        //"null as Nocne1, " +
                        //"null as Nocne1Rsec, " +
                        //"null as Nocne1R, " +
                        
                        "null as Nocne2sec, " +
                        //"null as Nocne2, " +
                        //"null as Nocne2Rsec, " +
                        //"null as Nocne2R, " +

                        "null as ponocy, null as ponocy2, " +

                        "P.Id as PPId, " +
                        "P.IdZmiany, " +
                        "P.IdZmianyKorekta, " +
                        "P.CzasIn, P.CzasOut, P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne, " +
                        "P.k_CzasIn, P.k_CzasOut, P.k_CzasZm, P.k_NadgodzinyDzien, P.k_NadgodzinyNoc, P.k_Nocne, " +
                        "P.Uwagi, " +
                        "P.DataAcc, P.IdKierownikaAcc, P.Akceptacja, " +

                        "Z.Id as ZmianaId, " +
                        "Z.Symbol, " +
                        "Z.Kolor, " +
                        "Z.Od as ZmianaOd, " +
                        "Z.Do as ZmianaDo, " +
                        "Z.TypZmiany, Z.Nadgodziny, " +
                        
                        "A.Kod as AbsencjaKod, " +
                        "P.Absencja as AbsencjaKodKier, " +
                        "AK.Symbol as AbsencjaSymbol, " +
                        "AK.Nazwa as AbsencjaNazwa, " + 
                        "AK.GodzinPracy " +
                    "from GetDates2('{1}','{2}') D " +
                        "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
                        "left outer join Zmiany Z on Z.Id = ISNULL(P.IdZmianyKorekta, P.IdZmiany) " +
                        "left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                        "left outer join AbsencjaKody AK on AK.Kod = ISNULL(A.Kod, P.Absencja) " +
                    "order by D.Data",

                        */



            sw.WriteLine(Base.getValue(dr, "Data"));
            return true;
        }
        







        //----- weryfikacja przy akceptacji -----
        private static DateTime? GetCzasIn(DataRow dr)
        {
            DateTime? dt = db.getDateTime(dr, "CzasIn");    // TimeIn jest z RCP
            if (dt == null)
            {
                dt = db.getDateTime(dr, "ZmianaOd");
                if (dt != null)
                {
                    DateTime data = (DateTime)db.getDateTime(dr, "Data");
                    dt = data.AddSeconds(Worktime.TimeToSec((DateTime)dt));
                }
            }
            return dt;
        }

        private static DateTime? GetCzasOut(DataRow dr)
        {
            DateTime? dt = db.getDateTime(dr, "CzasOut");    // TimeIn jest z RCP
            if (dt == null)
            {
                dt = db.getDateTime(dr, "ZmianaDo");
                if (dt != null)
                {
                    DateTime data = (DateTime)db.getDateTime(dr, "Data");
                    dt = data.AddSeconds(Worktime.TimeToSec((DateTime)dt));
                }
            }
            return dt;
        }

        private static bool GetPPDane(DataRow dr, out bool isAbsencja, out bool isZmiana, out DateTime czasIn, out DateTime czasOut, ref int err) // true jak kontynuuje, czyli jest dzień prawidłowy - mam wszystkie dane in out zmniany czas i acc
        {
            isZmiana = false;
            isAbsencja = false;
            czasIn = DateTime.MinValue;
            czasOut = DateTime.MinValue;
            bool acc = db.getBool(dr, "Akceptacja", false);
            if (acc)
            {
                isAbsencja = !db.isNull(dr, "AbsencjaKod");
                isZmiana = !Base.isNull(dr, "ZmianaId");
                if (isZmiana)
                {
                    DateTime data = (DateTime)db.getDateTime(dr, "Data");
                    //----- we -----
                    DateTime? dtIn = db.getDateTime(dr, "CzasIn");    // TimeIn jest z RCP, jak dzien zaakceptowany to powinno być to samo
                    if (dtIn == null)
                    {
                        dtIn = db.getDateTime(dr, "ZmianaOd");
                        if (dtIn != null) dtIn = data.AddSeconds(Worktime.TimeToSec((DateTime)dtIn));
                    }
                    if (dtIn != null)
                    {
                        czasIn = (DateTime)dtIn;
                        //----- wy -----
                        DateTime? dtOut = db.getDateTime(dr, "CzasOut");    // TimeIn jest z RCP, jak dzien zaakceptowany to powinno być to samo
                        if (dtOut == null)
                        {
                            dtOut = db.getDateTime(dr, "ZmianaDo");
                            if (dtOut != null) dtOut = data.AddSeconds(Worktime.TimeToSec((DateTime)dtOut));
                        }
                        if (dtOut != null)
                        {
                            czasOut = (DateTime)dtOut;
                            if (czasIn > czasOut) czasOut = czasOut.AddDays(1);   // zmiana nocna
                            int czasZm = db.getInt(dr, "CzasZm", 0);
                            int nadgD = db.getInt(dr, "NadgodzinyDzien", 0);
                            int nadgN = db.getInt(dr, "NadgodzinyNoc", 0);
                            DateTime czasOut2 = czasIn.AddSeconds(czasZm + nadgD + nadgN);
                            if (czasOut2 > czasOut)
                                czasOut = czasOut2;     // gdyby czas out był niepoprawnie podany lub wziety wg zmiany a były nadgodziny
                            return true;                // ok !!!
                        }
                    }
                }
                else return true;   // brak zmiany
            }
            return false;
        }


        public static int CheckPPAkceptacja(DataSet ds, int index, DataRow drDay, object czasIn, object czasOut)
        {
            const int h11 = 11 * 3600;
            const int h35 = 35 * 3600;

            int err = 0;
            DateTime _tIn, tIn1, tOut1;
            bool isZmiana;
            bool isAbsencja;
            //----- sprawdzany dzień -----
            isAbsencja = !db.isNull(drDay, "AbsencjaKod");
            if (isAbsencja)
                return 0;   // jest absencja - nie sprawdzam

            if (db.isNull(czasIn))
            {
                DateTime? zmOd = db.getDateTime(drDay, "ZmianaOd");
                if (zmOd != null)
                {
                    DateTime data = (DateTime)db.getDateTime(drDay, "Data");
                    _tIn = data.AddSeconds(Worktime.TimeToSec((DateTime)zmOd));
                }
                else
                    return 0;           // nie ma co sprawdzać !!! - brak czasu wejścia i nie ma zmiany
            }
            else _tIn = (DateTime) czasIn;
            //----- przerwa 11h - dzień poprzedni -----  // PlanPracy.cs line: 2682
            if (index > 0)
            {
                DataRow dr = db.getRow(ds, index - 1);
                bool ok = GetPPDane(dr, out isAbsencja, out isZmiana, out tIn1, out tOut1, ref err);
                if (ok && isZmiana && !isAbsencja)
                {
                    if (Worktime.CountDateTimeSec(tOut1, _tIn) < h11)
                        err |= Worktime.alPrzerwa11;
                }
                //----- przerwa 35h -----
                if (ok && index > 1)        // dzień zaakceptowany i nie było zmiany = wolny, co z chorobowym ??? - dopytać ???
                {
                    int idx = index - 2;
                    if (!isZmiana)          // nie ma zmiany więc spr dzień poprzedni
                    {
                        dr = db.getRow(ds, idx);      // dzień poprzedni, w domyśle skoro wolny to tu powinna być przerwa, dla porządku wypadałoby sprawdzić 5 wczesniejszych dni czy dzień po dniu praca
                        ok = GetPPDane(dr, out isAbsencja, out isZmiana, out tIn1, out tOut1, ref err);


                        
                        /*
                         sprawdzić 4 dni wprzód czy jest zmiana i jezeli tak to dopiero to 
                         */




                        if (ok && isZmiana && !isAbsencja)
                        {
                            if (Worktime.CountDateTimeSec(tOut1, _tIn) < h35)
                                err = Worktime.alPrzerwa35;
                        }
                    }
                    else    // była zmiana - trzeba sprawdzić czy nie ma wcześniej przerwy - powinno wystarczyć sprawdzenie 1 dnia pomiędzy
                    {       // to sprawdzimy przy akceptacji całego miesiąca - inną funkcją
                        if (idx > 0)
                        {
                            ok = false;
                            for (int i = 0; i < 5; i++)   //      X X X _ X X X X B A - tylko 4 + 1 sprawdzam bo A i B mają zmiane, a musi wyjść błąd na _
                            {
                                dr = db.getRow(ds, idx);      // dzień poprzedni, w domyśle skoro wolny to tu powinna być przerwa, dla porządku wypadałoby sprawdzić 5 wczesniejszych dni czy dzień po dniu praca
                                if (GetPPDane(dr, out isAbsencja, out isZmiana, out tIn1, out tOut1, ref err))
                                {
                                    if (!isZmiana || isAbsencja)
                                    {
                                        ok = true;
                                        break;      // jest dobowa przerwa - zakładam że wystarczy pozniej dac sprawdzenie konkretnej ilosci godzin
                                    }
                                }
                                else
                                {
                                    ok = true;
                                    break;  // nie ma co dalej sprawdzać - nie wszystko zaakceptowane 
                                }
                                if (idx > 0) idx--;
                                else
                                {
                                    ok = true;
                                    break;  // dojechałem do początku danych więc jest ok
                                }
                            }
                            if (!ok)
                                err = Worktime.alPrzerwa35;

                            /*
                             spr ilość !!! 35h
                             */


                        }
                    }
                }
            }
            return err;
        }















        // tylko te dni w których jest kierownikiem pracownika
        public static int _Akceptuj(string kierId, string pracId, string accKierId, KierParams kp,
                                    string _DataOd, string _DataDo, string _onDay,
                                    DateTime? accTo,            // do kiedy akceptować lub null jak nie sprawdzać, jak null to nie aktualizauje kierParams
                                    bool checkOnly,
                                    bool updateNN,              // aktualizacja nadgodzin i alertów <<< 20140629 wydaję sie ze zawsze false jest przekazywane ...
                                    StringWriter sw,
                                    bool errAcc)                // wymuś akceptację mimo błędów nieblokujących (po potwierdzeniu)
        {
            //----- czy przypadkiem akceptacja nie dotyczy okresu archiwalnego (w razie awarii np) -----
            Okres okres = new Okres(Base.StrToDateTime(_DataDo));
            int? okresId = okres.IsArch() ? (int?)okres.Id : null;

            //----- pracownicy kierownika ------
            const string sqlPracKier = @"
declare 
    @Od datetime,
    @Do datetime,
    @IdKierownika int
set @Od = '{0}'
set @Do = '{1}'
set @IdKierownika = {2}

SELECT distinct P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    PR.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 
    ISNULL(PK.RcpId, -1) as RcpId, 
    ISNULL(PO.Status, P.Status) as Status 
FROM Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left outer join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left outer join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')
left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')
left outer join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')
WHERE R.IdKierownika = @IdKierownika and R.Status = 1 and ISNULL(R.Do, '20990909') >= @Od and R.Od <= @Do 
and ISNULL(PO.Status, P.Status) >= 0  
ORDER BY Kierownik desc, NazwiskoImie";

            const string sqlPrac = @"
declare 
    @Do datetime,
    @IdPracownika int
set @Do = '{0}'     -- na dzień
set @IdPracownika = {1}

SELECT distinct P.Id, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, 
    R.IdKierownika,
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    R.RcpStrefaId as StrefaId,      -- na koniec okresu
    PP.RcpAlgorytm as Algorytm, 
    ISNULL(PK.RcpId, -1) as RcpId, 
    ISNULL(PO.Status, P.Status) as Status 
FROM Przypisania R 
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join OkresyRozl O on @Do between O.DataOd and O.DataDo 
left outer join PracownicyOkresy PO on PO.IdOkresu = O.Id and PO.Id = R.IdPracownika
left outer join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @Do between PP.Od and ISNULL(PP.Do, '20990909')
left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @Do between PK.Od and ISNULL(PK.Do, '20990909')
--left outer join Przypisania PR on PR.IdPracownika = R.IdPracownika and PR.Status = 1 and @Do between PR.Od and ISNULL(PR.Do, '20990909')
WHERE R.IdPracownika = @IdPracownika and R.Status = 1 and @Do between R.Od and ISNULL(R.Do, '20990909')";


            string sOd, sDo;
            int startIdx;
            DataSet dsPrac;
            if (String.IsNullOrEmpty(pracId))    // bez podania pracownika jest to akceptacja za okres wszystkich u kierownika
            {
                dsPrac = db.getDataSet(String.Format(sqlPracKier, _DataOd, _DataDo, kierId));     // wszyscy pracownicy kierownika
            }
            else                                
            {
                dsPrac = db.getDataSet(String.Format(sqlPrac, _onDay, pracId));                  // tylko 1 pracownik za 1 dzień (akceptacja dnia PP)
                kierId = Base.getValue(dsPrac, "IdKierownika");
            }

            bool okresAcc = String.IsNullOrEmpty(_onDay);
            if (okresAcc)
            {
                sOd = _DataOd;
                sDo = _DataDo;
                startIdx = 0;
            }
            else
            {
                DateTime dOd = (DateTime)Tools.StrToDateTime(_DataOd);
                DateTime dDay = (DateTime)Tools.StrToDateTime(_onDay);
                dDay = dDay.AddDays(-7);    // cofam się o tydzień 
                if (dDay < dOd) dDay = dOd;
                sOd = Tools.DateToStr(dDay);
                sDo = _onDay;
                startIdx = -1;   // tylko ostatni
            }


            //----- parametry ------
            if (kp == null)
            {
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                kp = new KierParams(kierId, settings);
            }
            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;

            int err = 0;                                       // zwracany kod błędu, kody blokujące kierownika i kody nieblokujące - wymagające potwierdzenia
            bool first = true;
            foreach (DataRow drv in dsPrac.Tables[0].Rows)     // pracownicy
            {
                if (checkOnly)
                {
                    if (first)
                    {
                        first = false;
                        sw.WriteLine("====================================================");
                    }
                    else sw.WriteLine("----------------------------------------------------");
                    sw.WriteLine(String.Format("{0} {1}", Base.getValue(drv, "KadryId"), Base.getValue(drv, "NazwiskoImie")));
                }

                pracId = drv["Id"].ToString();                          // dla 1 dnia nadpisze id tym samym
                bool kier = (bool)drv["Kierownik"];
                //----- pobieranie danych ------
                string rcpId = drv["RcpId"].ToString();
                string strefaId = drv["StrefaId"].ToString();
                string alg = drv["Algorytm"].ToString();
                string algPar = App.GetAlgPar(alg);  // optymalniej niz dolaczac do query, pobierze tylko jak trzeba
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";
                Worktime2 wt2;








                /*
                DataSet ds = Worktime2._GetWorktime(pracId, alg, algPar, rcpId, true, DataOd, DataDo,
                                onDay,                    //dla acc PP to jest null
                                strefaId,
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, null, true, out wt2);

                DataSet ds1 = Worktime2._GetWorktime(pracId, null, null, rcpId, true, DataOd, DataDo,
                                onDay,                    //dla acc PP to jest null
                                strefaId,
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, null, true, out wt2);

                string cc = db.DataSetCompare(ds, ds1);
                */

                DataSet ds = Worktime2._GetWorktime(pracId, null, null, 
                                rcpId,                  //<<< jeszcze dla testów, później można wyłączyć !!!! 
                                true, sOd, sDo,
                                
                                //_onDay,                 //dla acc PP to jest null
                                null,                   // jak sprawdzam 11h i 35h to od do ustawiam powyzej i onDay musi być null bo w GetWorktime ustawiał zakres od-do 

                                strefaId,
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, null,
                                0, /* wymiar */
                                true, out wt2);




                /*
                                  
                 * 35h - pobrać dane 5 dni wprzód
                 *   
                 
                 */






                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;
                if (startIdx == -1) startIdx = cnt - 1;

                DateTime today = DateTime.Today;

                for (int i = startIdx; i < cnt; i++)       // PlanPracy - Akceptacja
                {
                    DataRow dr = Base.getDataRow(ds, i);
                    string kid = db.getValue(dr, "PrzKierId");
                    DateTime date = Base.getDateTime(dr, "Data", today);    // musi byc ok
                    if (
                        App.User.IsAdmin || 
                        kid == App.User.Id || 
                        App.WPodstrukturze(App.User.Id, pracId, date))      // lub z mojej podstruktury
                    {
                        DateTime? _czasIn = Base.getDateTime(dr, "TimeIn");
                        DateTime? czasOut = Base.getDateTime(dr, "TimeOut");
                        bool isZmiana = !Base.isNull(dr, "ZmianaId");

                        int wtAlert = 0;                                    // alerty z liczenia czasu pracy

                        int wtime, _ztime, otimeD, otimeN, _ntime;  // łączny, zmiana, nadgodziny, nocne
                        int rzt, rnD, rnN, rN;
                        //int before6;

                        bool isWTime = Worktime.SolveWorktime2(dr,
                            alg, algPar,
                            FBreakMM, FBreak2MM, FMarginMM, Fzaokr, FzaokrType,
                            true,                   // wartości skorygowane <<< te muszą być !!!
                            //false,                  // wartości rcp
                            out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                            out rzt, out rnD, out rnN, out rN,
                            //out before6,
                            //false,
                            true,           //n50 n100
                            ref wtAlert);   // alerty RCP mnie tu nie interesują, powinien sprawdzać alerty Kier lub minimum żeby zaakceptować - i tak robie ...

                        int? zt, otD, otN, nt;
                        Worktime.WorktimeToPP(Fzaokr, FzaokrType, _ztime, otimeD, otimeN, _ntime,
                                                      out zt, out otD, out otN, out nt);
                        //----- sprawdzenie poprawek kierownika -----
                        bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                        bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                        DateTime? vCzasIn = Base.getDateTime(dr, "CzasIn");
                        DateTime? vCzasOut = Base.getDateTime(dr, "CzasOut");



                        DateTime? kin  = k_CzasIn  ? (DateTime?)vCzasIn  : _czasIn;       // wprowadzony przez Kierownika lub z RCP, jak jest akceptacja to można by tylko z bazy ...
                        DateTime? kout = k_CzasOut ? (DateTime?)vCzasOut : czasOut;
                        
                        
                        int errK = Worktime.GetKierAlert(pracId, Tools.DateToStr(date), dr, kin, kout, zt, otD, otN, nt);    // błędy kierownika

                        //----- spr zgodności wartości zatrzaśniętych ------
                        string info = null;
                        const string sep = ", ";
                        bool acc = Base.getInt(dr, "Akceptacja", 0) == 1;      // sens sprawdzanie ma tylko dla wartości zaakceptowanych <<< czyli nie ma miejsca przy akceptacji 1 dnia, bo wtedy zawsze startuje z niezaakceptowanym
                        if (acc)
                        {
                            //bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                            //bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                            bool k_CzasZm   = Base.getBool(dr, "k_CzasZm", false);
                            bool k_NadgDzien= Base.getBool(dr, "k_NadgodzinyDzien", false);
                            bool k_NadgNoc  = Base.getBool(dr, "k_NadgodzinyNoc", false);
                            bool k_Nocne    = Base.getBool(dr, "k_Nocne", false);
                            //DateTime? vCzasIn = Base.getDateTime(dr, "CzasIn");
                            //DateTime? vCzasOut = Base.getDateTime(dr, "CzasOut");
                            int vCzas       = Base.getInt(dr, "Czas", 0);                 // dane z RCP
                            int vCzasZm     = Base.getInt(dr, "CzasZm", -1);
                            int vNadgDzien  = Base.getInt(dr, "NadgodzinyDzien", 0);
                            int vNadgNoc    = Base.getInt(dr, "NadgodzinyNoc", 0);
                            int vNocne      = Base.getInt(dr, "Nocne", 0);

                            /* nie zgłaszam błędu po zastanowieniu ... skoro kier juz zaakceptował to nie wymagam ponownej akceptacji, ale trzeba by to jeszcze przemysleć ...
                               - za dużo poprawek, z reguły kier akceptowali jak był brak bez korekty; czas nadgodzin jest dłuższy o dł przerwy bo tak poprzednio liczył
                               - opcję włączyć z początkiem nowego okresu rozliczeniowego !!!
                             */
                            bool lockError =
                                vCzas != wtime ||                                   // wtime - dane zatrzaśnięte
                                !k_CzasIn && vCzasIn != _czasIn ||
                                !k_CzasOut && vCzasOut != czasOut ||
                                !k_CzasZm && vCzasZm != rzt ||
                                !k_NadgDzien && vNadgDzien != rnD ||
                                !k_NadgNoc && vNadgNoc != rnN ||
                                !k_Nocne && vNocne != rN;
                            if (lockError) errK |= Worktime.alLockError;
                            /**/

                            /* old version
                            bool lockError =
                                vCzas != wtime ||                                   // wtime - dane zatrzaśnięte
                                !k_CzasIn && vCzasIn != czasIn ||
                                !k_CzasOut && vCzasOut != czasOut ||
                                !k_CzasZm && vCzasZm != _ztime ||
                                !k_NadgDzien && vNadgDzien != otimeD ||
                                !k_NadgNoc && vNadgNoc != otimeN ||
                                !k_Nocne && vNocne != _ntime;
                            if (lockError) e |= Worktime.alLockError;
                             */

                            //----- info -----
                            if (checkOnly)  // jesli zawsze to wyłaczyć tego if !!!
                            {
                                if (vCzas != wtime)
                                    info += sep + String.Format("Czas {0}/{1}", Worktime.SecToTime(wtime, 0), Worktime.SecToTime(vCzas, 0));                    // łączny czas z rcp - powinno być zawsze to samo chyba ze zmiana konf stref po akceptacji 
                                if (!k_CzasIn && vCzasIn != _czasIn)
                                    info += sep + String.Format("CzasIn {0}/{1}",
                                                                    _czasIn == null ? "null" : Base.TimeToStr((DateTime)_czasIn),
                                                                    vCzasIn == null ? "null" : Base.TimeToStr((DateTime)vCzasIn));                             // czas nie skorygowany i różny od zapisanego    
                                if (!k_CzasOut && vCzasOut != czasOut)
                                    info += sep + String.Format("CzasOut {0}/{1}",
                                                                    czasOut == null ? "null" : Base.TimeToStr((DateTime)czasOut),
                                                                    vCzasOut == null ? "null" : Base.TimeToStr((DateTime)vCzasOut));                             // czas nie skorygowany i różny od zapisanego    
                                if (!k_CzasZm && vCzasZm != rzt)
                                    info += sep + String.Format("CzasZm {0}/{1}",
                                                                rzt == -1 ? "null" : Worktime.SecToTime(rzt, 0),
                                                                vCzasZm == -1 ? "null" : Worktime.SecToTime(vCzasZm, 0));
                                if (!k_NadgDzien && vNadgDzien != rnD)
                                    info += sep + String.Format("NadgDzien {0}/{1}", Worktime.SecToTime(rnD, 0), Worktime.SecToTime(vNadgDzien, 0));
                                if (!k_NadgNoc && vNadgNoc != rnN)
                                    info += sep + String.Format("NadgNoc {0}/{1}", Worktime.SecToTime(rnN, 0), Worktime.SecToTime(vNadgNoc, 0));
                                if (!k_Nocne && vNocne != rN)
                                    info += sep + String.Format("Noc {0}/{1}", Worktime.SecToTime(rN, 0), Worktime.SecToTime(vNocne, 0));
                            }

                            /*
                            if (checkOnly)  // jesli zawsze to wyłaczyć tego if !!!
                            {
                                if (vCzas != wtime)
                                    info += sep + String.Format("Czas {0}/{1}", Worktime.SecToTime(wtime, 0), Worktime.SecToTime(vCzas, 0));                    // łączny czas z rcp - powinno być zawsze to samo chyba ze zmiana konf stref po akceptacji 
                                if (!k_CzasIn && vCzasIn != czasIn)
                                    info += sep + String.Format("CzasIn {0}/{1}",
                                                                    czasIn == null ? "null" : Base.TimeToStr((DateTime)czasIn),
                                                                    vCzasIn == null ? "null" : Base.TimeToStr((DateTime)vCzasIn));                             // czas nie skorygowany i różny od zapisanego    
                                if (!k_CzasOut && vCzasOut != czasOut)
                                    info += sep + String.Format("CzasOut {0}/{1}",
                                                                    czasOut == null ? "null" : Base.TimeToStr((DateTime)czasOut),
                                                                    vCzasOut == null ? "null" : Base.TimeToStr((DateTime)vCzasOut));                             // czas nie skorygowany i różny od zapisanego    
                                if (!k_CzasZm && vCzasZm != _ztime)
                                    info += sep + String.Format("CzasZm {0}/{1}", 
                                                                _ztime == -1 ? "null" : Worktime.SecToTime(_ztime, 0), 
                                                                vCzasZm == -1 ? "null" : Worktime.SecToTime(vCzasZm, 0));
                                if (!k_NadgDzien && vNadgDzien != otimeD)
                                    info += sep + String.Format("NadgDzien {0}/{1}", Worktime.SecToTime(otimeD, 0), Worktime.SecToTime(vNadgDzien, 0));
                                if (!k_NadgNoc && vNadgNoc != otimeN)
                                    info += sep + String.Format("NadgNoc {0}/{1}", Worktime.SecToTime(otimeN, 0), Worktime.SecToTime(vNadgNoc, 0));
                                if (!k_Nocne && vNocne != _ntime)
                                    info += sep + String.Format("Noc {0}/{1}", Worktime.SecToTime(_ntime, 0), Worktime.SecToTime(vNocne, 0));
                            }
                             */

                        }
                        //----- dane MPK -----------------------------


                        //----- weryfikacja danych -------------------
                        wtAlert |= errK;        // do alertu dokładam błędy kierownika
                        
                        if (checkOnly)
                        {
                            bool cofnijAcc = errK != 0;
                            if (acc)
                            {
                                if (cofnijAcc) info = "acc->!acc" + info;       // jest zaakceptowane, a nie może być";
                                else
                                    if (!String.IsNullOrEmpty(info))
                                        info = info.Substring(2);
                                if (!String.IsNullOrEmpty(info)) sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), info));
                            }
                            else
                            {
                                sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), "brak akceptacji"));
                            }
                        }
                        else if (updateNN)
                        {
                            if (acc)
                                UpdatePPNN(pracId, date, dr,
                                     _czasIn, czasOut, wtime, zt, otD, otN, nt,          // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                                     wtAlert, accKierId,
                                     kin);
                        }
                        else
                        //----- zamykanie okresu -------------------
                        {
                            /*//---- debug ----
                            if (pracId == "1228" && Tools.DateToStrDb(date) == "20140710")
                            {
                                int x = 0;
                            }
                            //---------------/**/

                            if (errK != 0)     // są błędy kierownika <<< !!! blokują !!!
                            {
                                if (Base.getBool(dr, "Akceptacja", false))    // dzień jest zaakceptowany 
                                {
                                    /* ---- bez odblokowywania dnia
                                    _CofnijAcc(con, Base.getValue(dr, "PPId"), wtAlert, accKierId, false, true, "Odblokowanie dnia podczas zamykania okresu");    // cofam akceptacje jeśli jest
                                    err |= e;                                                                                                                     // zgaszam blad, jak CofnijAcc zaremowane to to tez, bo inaczej pojawia sie info ze nie wszystkie dni zostay zaakceptowane
                                    */
                                    Log.Info(Log.OKRES_DAYTOREOPEN, "Zmiana danych RCP, wymagane ponowne zatwierdzenie", String.Format(                             // logowanie - tylko jak CofnijAcc zaremowane, bo ma w środku swoje
                                        "Data: {0} Pracownik: {1} {2} Alerty: {3:X8} KierId: {4}",
                                            Base.DateToStr(date),
                                            Base.getValue(drv, "KadryId"), Base.getValue(drv, "NazwiskoImie"),
                                            wtAlert,      // razem z błędami kierownika
                                            accKierId
                                        ));
                                }
                                else
                                    err |= errK;     // zwracać będę błędy blokujące kierownika
                            }
                            else            // nie ma blokujących błędów kierownika
                            {
                                int errP = CheckPPAkceptacja(ds, i, dr, kin, kout);    // sprawdzenie czy są błędy wymagające potwierdzenia 11h i 35h
                                bool update = true;
                                if (okresAcc)  // akceptacja od początku okresu do daty
                                {
                                    if (errP != 0)
                                    {
                                        bool acc1 = db.getBool(dr, "Akceptacja", false);    // dzień jest zaakceptowany 
                                        if (acc1)                                           // 
                                        {
                                            int alert1 = db.getInt(dr, "Alerty", 0);
                                            if ((alert1 & Worktime.alPrzerwa11) == 0 && (errP & Worktime.alPrzerwa11) != 0 ||    // nie było a teraz jest przekroczenie
                                                (alert1 & Worktime.alPrzerwa35) == 0 && (errP & Worktime.alPrzerwa35) != 0)
                                            {
                                                err |= errP;        // wyswietl komunikat ze nie mozna zaktualizować
                                                wtAlert |= errP;
                                                update = false;     // nie aktualizuje
                                                _CofnijAcc(db.con, Base.getValue(dr, "PPId"), wtAlert, accKierId, false, true, "Odblokowanie dnia podczas akceptacji");    // cofam akceptacje jeśli jest
                                            }
                                            else    // było, a teraz nie ma - aktualizacja alertu, nie odblokowuję
                                            {
                                                wtAlert |= errP;
                                            }
                                        }
                                        else   // nie ma akceptacji ale jest przekroczenie - wyswietlam ostrzezenie <<<< dzień z przekroczeniem musi być zaakceptowany pojedynczo !!!
                                        {
                                            err |= errP;        // wyswietl komunikat ze nie mozna zaktualizować
                                            wtAlert |= errP;
                                            update = false;
                                        }
                                    }
                                    else    // nie ma błędów, akceptacja
                                    {
                                    }
                                }
                                else  // akceptacja 1 dnia
                                {
                                    err |= errP;
                                    if (errP == 0 || errAcc)
                                    {
                                        wtAlert |= errP;                                    // zapisze z err potwierdzonymi - dzieki temu przy akceptacji całości nie bedzie trzeba ponownie akceptowac: sa te same errory i akceptacja to ok, jezeli errory sie różnią - trzeba odakceptować i zaktualizować errory 
                                    }
                                }
                                //----- aktualizacja -----
                                if (update)
                                {
                                    _UpdatePPAcc(pracId, date, dr,
                                             _czasIn, czasOut, wtime, zt, otD, otN, nt,          // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                                             wtAlert, accKierId,
                                             kin);
                                }
                            }
                            if (accTo != null && date >= accTo) break;   // dojechałem do końca zadanej akceptacji ; dla 1 day accTo musi być = dacie onDay
                        }
                    }
                }
            }

            if (!checkOnly && !updateNN && accTo != null && (kp.DataAccDo == null || (DateTime)kp.DataAccDo < accTo))
            {
                bool ok = KierParams.Update(kierId, (DateTime)accTo);   // aktualizauje kierownikowi do kiedy zaakceptowane dane ma mimo ze to nie on zaakceptował
            }

            if (checkOnly)
                sw.WriteLine("----------------------------------------------------");

            return err;
        }





























        /* 20131111 obowiązująca, bez przesuniec
        public static int _Akceptuj(string kierId, string pracId, string accKierId, KierParams kp,
                                    string DataOd, string DataDo, string onDay,
                                    DateTime? accTo,             // do kiedy akceptować lub null jak nie sprawdzać, jak null to nie aktualizauje kierParams
                                    bool checkOnly,
                                    StringWriter sw)             
        {
            /*
            const string sqlPrac =   // to samo co w PlanPracy.aspx
                  "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.IdKierownika, " +
                        "P.Kierownik, " +
                        "D.Nazwa as Dzial, " +
                        "ISNULL(RcpStrefaId, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierStrefaId " +
                                "else D.PracStrefaId " +
                            "end) as StrefaId, " +
                        "ISNULL(RcpAlgorytm, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierAlgorytm " +
                                "else D.PracAlgorytm " +
                            "end) as Algorytm " +
                   "FROM Pracownicy P " +
                   "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";
            const string whereK =
                   " WHERE P.IdKierownika = {0}";
            //"ORDER BY P.Kierownik desc, NazwiskoImie";
            const string whereP =
                   " WHERE P.Id = {0}";
            * /
            //----- czy przypadkiem akceptacja nie dotyczy okresu archiwalnego (w razie awarii np) -----
            Okres okres = new Okres(Base.StrToDateTime(DataDo));
            int? okresId = okres.IsArch() ? (int?)okres.Id : null;

            //----- pracownicy kierownika ------
            SqlConnection con = Base.Connect();

            DataSet dsPrac;
            if (String.IsNullOrEmpty(pracId))
                //dsPrac = Base.getDataSet(String.Format(sqlPrac + whereK, kierId));      // wszyscy pracownicy kierownika
                dsPrac = Worker._GetPracInfo1(con, 2, okresId, null, kierId, false);
            else
            {
                //dsPrac = Base.getDataSet(String.Format(sqlPrac + whereP, pracId));      // tylko 1 pracownik za 1 dzień (akceptacja dnia PP)
                dsPrac = Worker._GetPracInfo1(con, 1, okresId, null, pracId, false);
                kierId = Base.getValue(dsPrac, "IdKierownika");
            }
            //----- parametry ------
            if (kp == null)
            {
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                kp = new KierParams(kierId, settings);
            }
            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;

            int err = 0;
            bool first = true;
            foreach (DataRow drv in dsPrac.Tables[0].Rows)
            {
                if (checkOnly)
                {
                    if (first)
                    {
                        first = false;
                        sw.WriteLine("====================================================");
                    }
                    else sw.WriteLine("----------------------------------------------------");
                    sw.WriteLine(String.Format("{0} {1}", Base.getValue(drv, "KadryId"), Base.getValue(drv, "NazwiskoImie")));
                }

                pracId = drv["Id"].ToString();                          // dla 1 dnia nadpisze id tym samym
                bool kier = (bool)drv["Kierownik"];
                //----- pobieranie danych ------
                string rcpId = drv["RcpId"].ToString();
                string strefaId = drv["StrefaId"].ToString();
                string alg = drv["Algorytm"].ToString();
                string algPar = App.GetAlgPar(alg);  // optymalniej niz dolaczac do query, pobierze tylko jak trzeba
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";
                Worktime2 wt2;









                DataSet ds = Worktime2._GetWorktime(pracId, alg, algPar, rcpId, true, DataOd, DataDo,
                                onDay,                    //dla acc PP to jest null
                                strefaId,
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, null, true, out wt2);

                DataSet ds1 = Worktime2._GetWorktime(pracId, null, null, rcpId, true, DataOd, DataDo,
                                onDay,                    //dla acc PP to jest null
                                strefaId,
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, null, true, out wt2);

                string cc = db.DataSetCompare(ds, ds1);








                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;

                DateTime today = DateTime.Today;

                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getDataRow(ds, i);

                    DateTime date = Base.getDateTime(dr, "Data", today);  // musi byc ok
                    DateTime? czasIn = Base.getDateTime(dr, "TimeIn");
                    DateTime? czasOut = Base.getDateTime(dr, "TimeOut");
                    bool isZmiana = !Base.isNull(dr, "ZmianaId");

                    int wtAlert = 0;
                    int wtime, _ztime, otimeD, otimeN, _ntime;  // łączny, zmiana, nadgodziny, nocne
                    int rzt, rnD, rnN, rN;

                    bool isWTime = Worktime._SolveWorktime2(dr, 
                        alg, algPar,
                        FBreakMM, FBreak2MM, FMarginMM, Fzaokr, FzaokrType,
                        true,                   // wartości skorygowane <<< te muszą być !!!
                        //false,                  // wartości rcp
                        out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                        out rzt, out rnD, out rnN, out rN,
                        false, ref wtAlert);    // alerty RCP mnie tu nie interesują, powinien sprawdzać alerty Kier lub minimum żeby zaakceptować - i tak robie ...
                    
                    int? zt, otD, otN, nt;
                    Worktime.WorktimeToPP(Fzaokr, FzaokrType, _ztime, otimeD, otimeN, _ntime,
                                                  out zt, out otD, out otN, out nt);
                    //----- sprawdzenie poprawek kierownika -----
                    bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                    bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                    DateTime? vCzasIn = Base.getDateTime(dr, "CzasIn");
                    DateTime? vCzasOut = Base.getDateTime(dr, "CzasOut");

                    DateTime? kin = k_CzasIn ? (DateTime?)vCzasIn : czasIn;
                    DateTime? kout = k_CzasOut ? (DateTime?)vCzasOut : czasOut;
                    int e = Worktime.GetKierAlert(pracId, Tools.DateToStr(date), dr, kin, kout, zt, otD, otN, nt);

                    //----- spr zgodności wartości zatrzaśniętych ------
                    string info = null;
                    const string sep = ", ";
                    bool acc = Base.getInt(dr, "Akceptacja", 0) == 1;      // sens sprawdzanie ma tylko dla wartości zaakceptowanych
                    if (acc)
                    {
                        //bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                        //bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                        bool k_CzasZm = Base.getBool(dr, "k_CzasZm", false);
                        bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
                        bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
                        bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
                        //DateTime? vCzasIn = Base.getDateTime(dr, "CzasIn");
                        //DateTime? vCzasOut = Base.getDateTime(dr, "CzasOut");
                        int vCzas = Base.getInt(dr, "Czas", 0);                 // dane z RCP
                        int vCzasZm = Base.getInt(dr, "CzasZm", -1);
                        int vNadgDzien = Base.getInt(dr, "NadgodzinyDzien", 0);
                        int vNadgNoc = Base.getInt(dr, "NadgodzinyNoc", 0);
                        int vNocne = Base.getInt(dr, "Nocne", 0);

                        /* nie zgłaszam błędu po zastanowieniu ... skoro kier juz zaakceptował to nie wymagam ponownej akceptacji, ale trzeba by to jeszcze przemysleć ...
                           - za dużo poprawek, z reguły kier akceptowali jak był brak bez korekty; czas nadgodzin jest dłuższy o dł przerwy bo tak poprzednio liczył
                           - opcję włączyć z początkiem nowego okresu rozliczeniowego !!!
                         * / 
                        bool lockError =
                            vCzas != wtime ||                                   // wtime - dane zatrzaśnięte
                            !k_CzasIn && vCzasIn != czasIn ||
                            !k_CzasOut && vCzasOut != czasOut ||
                            !k_CzasZm && vCzasZm != rzt ||
                            !k_NadgDzien && vNadgDzien != rnD ||
                            !k_NadgNoc && vNadgNoc != rnN ||
                            !k_Nocne && vNocne != rN;
                        if (lockError) e |= Worktime.alLockError;
                        /**/

                        /* old version
                        bool lockError =
                            vCzas != wtime ||                                   // wtime - dane zatrzaśnięte
                            !k_CzasIn && vCzasIn != czasIn ||
                            !k_CzasOut && vCzasOut != czasOut ||
                            !k_CzasZm && vCzasZm != _ztime ||
                            !k_NadgDzien && vNadgDzien != otimeD ||
                            !k_NadgNoc && vNadgNoc != otimeN ||
                            !k_Nocne && vNocne != _ntime;
                        if (lockError) e |= Worktime.alLockError;
                         * /

                        //----- info -----
                        if (checkOnly)  // jesli zawsze to wyłaczyć tego if !!!
                        {
                            if (vCzas != wtime)
                                info += sep + String.Format("Czas {0}/{1}", Worktime.SecToTime(wtime, 0), Worktime.SecToTime(vCzas, 0));                    // łączny czas z rcp - powinno być zawsze to samo chyba ze zmiana konf stref po akceptacji 
                            if (!k_CzasIn && vCzasIn != czasIn)
                                info += sep + String.Format("CzasIn {0}/{1}",
                                                                czasIn == null ? "null" : Base.TimeToStr((DateTime)czasIn),
                                                                vCzasIn == null ? "null" : Base.TimeToStr((DateTime)vCzasIn));                             // czas nie skorygowany i różny od zapisanego    
                            if (!k_CzasOut && vCzasOut != czasOut)
                                info += sep + String.Format("CzasOut {0}/{1}",
                                                                czasOut == null ? "null" : Base.TimeToStr((DateTime)czasOut),
                                                                vCzasOut == null ? "null" : Base.TimeToStr((DateTime)vCzasOut));                             // czas nie skorygowany i różny od zapisanego    
                            if (!k_CzasZm && vCzasZm != rzt)
                                info += sep + String.Format("CzasZm {0}/{1}", 
                                                            rzt == -1 ? "null" : Worktime.SecToTime(rzt, 0), 
                                                            vCzasZm == -1 ? "null" : Worktime.SecToTime(vCzasZm, 0));
                            if (!k_NadgDzien && vNadgDzien != rnD)
                                info += sep + String.Format("NadgDzien {0}/{1}", Worktime.SecToTime(rnD, 0), Worktime.SecToTime(vNadgDzien, 0));
                            if (!k_NadgNoc && vNadgNoc != rnN)
                                info += sep + String.Format("NadgNoc {0}/{1}", Worktime.SecToTime(rnN, 0), Worktime.SecToTime(vNadgNoc, 0));
                            if (!k_Nocne && vNocne != rN)
                                info += sep + String.Format("Noc {0}/{1}", Worktime.SecToTime(rN, 0), Worktime.SecToTime(vNocne, 0));
                        }

                        /*
                        if (checkOnly)  // jesli zawsze to wyłaczyć tego if !!!
                        {
                            if (vCzas != wtime)
                                info += sep + String.Format("Czas {0}/{1}", Worktime.SecToTime(wtime, 0), Worktime.SecToTime(vCzas, 0));                    // łączny czas z rcp - powinno być zawsze to samo chyba ze zmiana konf stref po akceptacji 
                            if (!k_CzasIn && vCzasIn != czasIn)
                                info += sep + String.Format("CzasIn {0}/{1}",
                                                                czasIn == null ? "null" : Base.TimeToStr((DateTime)czasIn),
                                                                vCzasIn == null ? "null" : Base.TimeToStr((DateTime)vCzasIn));                             // czas nie skorygowany i różny od zapisanego    
                            if (!k_CzasOut && vCzasOut != czasOut)
                                info += sep + String.Format("CzasOut {0}/{1}",
                                                                czasOut == null ? "null" : Base.TimeToStr((DateTime)czasOut),
                                                                vCzasOut == null ? "null" : Base.TimeToStr((DateTime)vCzasOut));                             // czas nie skorygowany i różny od zapisanego    
                            if (!k_CzasZm && vCzasZm != _ztime)
                                info += sep + String.Format("CzasZm {0}/{1}", 
                                                            _ztime == -1 ? "null" : Worktime.SecToTime(_ztime, 0), 
                                                            vCzasZm == -1 ? "null" : Worktime.SecToTime(vCzasZm, 0));
                            if (!k_NadgDzien && vNadgDzien != otimeD)
                                info += sep + String.Format("NadgDzien {0}/{1}", Worktime.SecToTime(otimeD, 0), Worktime.SecToTime(vNadgDzien, 0));
                            if (!k_NadgNoc && vNadgNoc != otimeN)
                                info += sep + String.Format("NadgNoc {0}/{1}", Worktime.SecToTime(otimeN, 0), Worktime.SecToTime(vNadgNoc, 0));
                            if (!k_Nocne && vNocne != _ntime)
                                info += sep + String.Format("Noc {0}/{1}", Worktime.SecToTime(_ntime, 0), Worktime.SecToTime(vNocne, 0));
                        }
                         * /
                    
                    }
                    //----- dane MPK -----------------------------
                    

                    //----- weryfikacja danych -------------------
                    wtAlert |= e;
                    if (checkOnly)
                    {
                        bool cofnijAcc = e != 0;
                        if (acc)
                        {
                            if (cofnijAcc) info = "acc->!acc" + info;       // jest zaakceptowane, a nie może być";
                            else
                                if (!String.IsNullOrEmpty(info))
                                    info = info.Substring(2);
                            if (!String.IsNullOrEmpty(info)) sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), info));
                        }
                        else
                        {
                            sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), "brak akceptacji"));
                        }
                    }
                    else
                    //----- zamykanie okresu -------------------
                    {
                        if (e != 0)
                        {
                            if (Base.getBool(dr, "Akceptacja", false))
                            {
                                /* ---- bez odblokowywania dnia
                                _CofnijAcc(con, Base.getValue(dr, "PPId"), wtAlert, accKierId, false, true, "Odblokowanie dnia podczas zamykania okresu");    // cofam akceptacje jeśli jest
                                err |= e;                                                                                                                     // zgaszam blad, jak CofnijAcc zaremowane to to tez, bo inaczej pojawia sie info ze nie wszystkie dni zostay zaakceptowane
                                * / 
                                Log.Info(Log.OKRES_DAYTOREOPEN, "Zmiana danych RCP, wymagane ponowne zatwierdzenie", String.Format(                             // logowanie - tylko jak CofnijAcc zaremowane, bo ma w środku swoje
                                    "Data: {0} Pracownik: {1} {2} Alerty: {3:X8} KierId: {4}", 
                                        Base.DateToStr(date), 
                                        Base.getValue(drv, "KadryId"), Base.getValue(drv, "NazwiskoImie"), 
                                        wtAlert,
                                        accKierId
                                    ));
                            }
                            else
                                err |= e;
                        }
                        else
                        {
                            UpdatePPAcc(con, pracId, date, dr,
                                     czasIn, czasOut, wtime, zt, otD, otN, nt,          // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                                     wtAlert, accKierId);
                        }
                        if (accTo != null && date >= accTo) break;   // dojechałem do końca zadanej akceptacji ; dla 1 day accTo musi być = dacie onDay
                    }
                }
            }

            if (!checkOnly && accTo != null && (kp.DataAccDo == null || (DateTime)kp.DataAccDo < accTo))
            {
                bool ok = KierParams.Update(con, kierId, (DateTime)accTo);   // aktualizauje kierownikowi do kiedy zaakceptowane dane ma mimo ze to nie on zaakceptował
            }

            if (checkOnly)
                sw.WriteLine("----------------------------------------------------");

            Base.Disconnect(con);
            return err;
        }
         
         */




        //----- old Akceptuj ------------------------------------------------------
        /*
        public static int old_Akceptuj(string kierId, string pracId, string accKierId, KierParams kp,
                                    string DataOd, string DataDo, string onDay,
                                    DateTime? accTo,             // do kiedy akceptować lub null jak nie sprawdzać, jak null to nie aktualizauje kierParams
                                    bool checkOnly,
                                    StringWriter sw)
        {
            /*
            const string sqlPrac =   // to samo co w PlanPracy.aspx
                  "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.IdKierownika, " +
                        "P.Kierownik, " +
                        "D.Nazwa as Dzial, " +
                        "ISNULL(RcpStrefaId, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierStrefaId " +
                                "else D.PracStrefaId " +
                            "end) as StrefaId, " +
                        "ISNULL(RcpAlgorytm, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierAlgorytm " +
                                "else D.PracAlgorytm " +
                            "end) as Algorytm " +
                   "FROM Pracownicy P " +
                   "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";
            const string whereK =
                   " WHERE P.IdKierownika = {0}";
            //"ORDER BY P.Kierownik desc, NazwiskoImie";
            const string whereP =
                   " WHERE P.Id = {0}";
            * /
            //----- czy przypadkiem akceptacja nie dotyczy okresu archiwalnego (w razie awarii np) -----
            Okres okres = new Okres(Base.StrToDateTime(DataDo));
            int? okresId = okres.IsArch() ? (int?)okres.Id : null;

            //----- pracownicy kierownika ------
            SqlConnection con = Base.Connect();

            DataSet dsPrac;
            if (String.IsNullOrEmpty(pracId))
                //dsPrac = Base.getDataSet(String.Format(sqlPrac + whereK, kierId));      // wszyscy pracownicy kierownika
                dsPrac = Worker.GetPracInfo1(con, 2, okresId, null, kierId, false);
            else
            {
                //dsPrac = Base.getDataSet(String.Format(sqlPrac + whereP, pracId));      // tylko 1 pracownik za 1 dzień (akceptacja dnia PP)
                dsPrac = Worker.GetPracInfo1(con, 1, okresId, null, pracId, false);
                kierId = Base.getValue(dsPrac, "IdKierownika");
            }
            //----- parametry ------
            if (kp == null)
            {
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                kp = new KierParams(kierId, settings);
            }
            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;

            int err = 0;
            bool first = true;
            foreach (DataRow drv in dsPrac.Tables[0].Rows)
            {
                if (checkOnly)
                {
                    if (first)
                    {
                        first = false;
                        sw.WriteLine("====================================================");
                    }
                    else sw.WriteLine("----------------------------------------------------");
                    sw.WriteLine(String.Format("{0} {1}", Base.getValue(drv, "KadryId"), Base.getValue(drv, "NazwiskoImie")));
                }

                pracId = drv["Id"].ToString();                          // dla 1 dnia nadpisze id tym samym
                bool kier = (bool)drv["Kierownik"];
                //----- pobieranie danych ------
                string rcpId = drv["RcpId"].ToString();
                string strefaId = drv["StrefaId"].ToString();
                string alg = drv["Algorytm"].ToString();
                string algPar = App.GetAlgPar(con, alg);  // optymalniej niz dolaczac do query, pobierze tylko jak trzeba
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";

                DataSet ds = Worktime.GetWorktime(con, pracId, rcpId, DataOd, DataDo,
                                onDay,                    //dla acc PP to jest null
                                strefaId,
                                Fzaokr,
                                FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec);

                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;

                DateTime today = DateTime.Today;

                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getDataRow(ds, i);

                    DateTime date = Base.getDateTime(dr, "Data", today);  // musi byc ok
                    DateTime? czasIn = Base.getDateTime(dr, "TimeIn");
                    DateTime? czasOut = Base.getDateTime(dr, "TimeOut");
                    bool isZmiana = !Base.isNull(dr, "ZmianaId");

                    int _wtAlert = 0;
                    int wtime, _ztime, otimeD, otimeN, _ntime;  // łączny, zmiana, nadgodziny, nocne
                    bool isWTime = Worktime.SolveWorktime2(dr, alg, algPar,
                        FBreakMM, FBreak2MM, FMarginMM, Fzaokr, FzaokrType,
                        true,                   // wartości skorygowane
                        out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                        false, ref _wtAlert);   // alerty RCP mnie tu nie interesują, powinien sprawdzać alerty Kier lub minimum żeby zaakceptować - i tak robie ...
                    int? zt, otD, otN, nt;
                    Worktime.WorktimeToPP(Fzaokr, FzaokrType, _ztime, otimeD, otimeN, _ntime,
                                                  out zt, out otD, out otN, out nt);
                    //----- sprawdzenie poprawek kierownika -----
                    int e = Worktime.GetKierAlert(dr, zt, otD, otN, nt);
                    /*
                    int zt0  = zt  == null ? 0 : (int)zt;
                    int otD0 = otD == null ? 0 : (int)otD;
                    int otN0 = otN == null ? 0 : (int)otN;
                    int nt0  = nt  == null ? 0 : (int)nt;
                    isWTime = zt0 > 0;             // jest czas po acc lub korekcie K SolveWorktime2 zwraca czy jest czas rcp !!!

                    int e = 0;
                    if (isWTime && !isZmiana) e |= Worktime.alNoShift;                      // brak zmiany
                    if (isZmiana && !Worktime.zmNadgodziny(dr) && (otD0 > 0 || otN0 > 0))   // zmiana nie ma nadgodzin a są
                        e |= Worktime.alZmNoNadg;
                    if (otN > nt) e |= Worktime.alKNadgNoc;
                    // i inne sprawdzenia ...                    
                    * /

                    //----- weryfikacja danych -------------------
                    _wtAlert |= e;
                    if (checkOnly)
                    {
                        string info = null;
                        bool cofnijAcc = e != 0;
                        const string sep = ", ";

                        bool acc = Base.getInt(dr, "Akceptacja", 0) == 1;      // sens sprawdzanie ma tylko dla wartości zaakceptowanych
                        if (acc)
                        {
                            bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                            bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                            bool k_CzasZm = Base.getBool(dr, "k_CzasZm", false);
                            bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
                            bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
                            bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
                            DateTime? vCzasIn = Base.getDateTime(dr, "CzasIn");
                            DateTime? vCzasOut = Base.getDateTime(dr, "CzasOut");
                            int vCzas = Base.getInt(dr, "Czas", 0);
                            int vCzasZm = Base.getInt(dr, "CzasZm", -1);
                            int vNadgDzien = Base.getInt(dr, "NadgodzinyDzien", 0);
                            int vNadgNoc = Base.getInt(dr, "NadgodzinyNoc", 0);
                            int vNocne = Base.getInt(dr, "Nocne", 0);

                            if (cofnijAcc) info += sep + "acc->!acc";       // jest zaakceptowane, a nie może być";
                            if (vCzas != wtime)
                                info += sep + "Czas";                       // łączny czas z rcp - powinno być zawsze to samo chyba ze zmiana konf stref po akceptacji 
                            if (!k_CzasIn && vCzasIn != czasIn)
                                info += sep + "CzasIn";     // czas nie skorygowany i różny od zapisanego    
                            if (!k_CzasOut && vCzasOut != czasOut) info += sep + "CzasOut";
                            if (!k_CzasZm && vCzasZm != _ztime) info += sep + "CzasZm";
                            if (!k_NadgDzien && vNadgDzien != otimeD) info += sep + "NadgDzien";
                            if (!k_NadgNoc && vNadgNoc != otimeN) info += sep + "NadgNoc";
                            if (!k_Nocne && vNocne != _ntime) info += sep + "Noc";

                            if (!String.IsNullOrEmpty(info)) sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), info.Substring(2)));
                        }
                        else
                        {
                            sw.WriteLine(String.Format("{0}: {1}\n", Base.DateToStr(date), "brak akceptacji"));
                        }
                    }
                    else
                    //----- zamykanie okresu -------------------
                    {
                        if (e != 0)
                        {
                            err |= e;
                            if (Base.getBool(dr, "Akceptacja", false))
                                CofnijAcc(con, Base.getValue(dr, "PPId"), _wtAlert, accKierId);   // cofam akceptacje jeśli jest
                        }
                        else
                        {
                            UpdatePPAcc(con, pracId, date, dr,
                                     czasIn, czasOut, wtime, zt, otD, otN, nt,          // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                                     _wtAlert, accKierId);
                        }
                        if (accTo != null && date >= accTo) break;   // dojechałem do końca zadanej akceptacji ; dla 1 day accTo musi być = dacie onDay
                    }
                }
            }

            if (!checkOnly && accTo != null && (kp.DataAccDo == null || (DateTime)kp.DataAccDo < accTo))
            {
                bool ok = KierParams.Update(con, kierId, (DateTime)accTo);   // aktualizauje kierownikowi do kiedy zaakceptowane dane ma mimo ze to nie on zaakceptował
            }

            if (checkOnly)
                sw.WriteLine("----------------------------------------------------");

            Base.Disconnect(con);
            return err;
        }
        //-------------------------------------------------------------------------
        */

        /*
        public static bool Akceptuj(string kierId, string accKierId, KierParams kp,
                                    string DataOd, string DataDo,
                                    DateTime accTo)  // do kiedy akceptować
        {
            const string sqlPrac =   // to samo co w PlanPracy.aspx
                  "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.Kierownik, " +
                        "D.Nazwa as Dzial, " +
                        "ISNULL(RcpStrefaId, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierStrefaId " +
                                "else D.PracStrefaId " +
                            "end) as StrefaId, " +
                        "ISNULL(RcpAlgorytm, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierAlgorytm " +
                                "else D.PracAlgorytm " +
                            "end) as Algorytm " +
                   "FROM Pracownicy P " +
                   "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu " +
                   "WHERE P.IdKierownika = {0} " +
                   "ORDER BY P.Kierownik desc, NazwiskoImie";

            //----- parametry ------
            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;
            //----- pracownicy kierownika ------
            SqlConnection con = Base.Connect();
            DataSet dsPrac = Base.getDataSet(String.Format(sqlPrac, kierId));
            foreach (DataRow drv in dsPrac.Tables[0].Rows)
            {
                string pracId = drv["Id"].ToString();
                bool kier = (bool)drv["Kierownik"];
                string rcpId = null;
                string strefaId = null;
                string alg = null;
                string algPar = null;

                //----- pobieranie danych ------
                DataSet ds;

                rcpId = drv["RcpId"].ToString();
                strefaId = drv["StrefaId"].ToString();
                alg = drv["Algorytm"].ToString();
                if (!String.IsNullOrEmpty(alg) && alg != "0" && alg != "1" && alg != "2" && alg != "11" && alg != "12")
                    algPar = Base.getScalar(con, "select Parametr from Kody where Typ='ALG' and Kod=" + alg);  //tak chyba będzie optymalniej niż dołączać do query
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";

                ds = Worktime.GetWorktime(con, pracId, rcpId, DataOd, DataDo, null,
                        strefaId,
                        Fzaokr,
                        FzaokrType,
                        kp.Settings.NocneOdSec, kp.Settings.NocneDoSec);

                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;
                
                DateTime today = DateTime.Today;
                    
                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getDataRow(ds, i);
                    
                    DateTime date = Base.getDateTime(dr, "Data", today);  // musi byc ok
                    DateTime? czasIn = Base.getDateTime(dr, "TimeIn");
                    DateTime? czasOut = Base.getDateTime(dr, "TimeOut");

                    int wtAlert = 0;
                    int wtime, ztime, otimeD, otimeN, ntime;  // łączny, zmiana, nadgodziny, nocne
                    bool isWTime = Worktime.SolveWorktime2(dr, alg, algPar,
                        FBreakMM,
                        FBreak2MM,
                        FMarginMM,
                        Fzaokr,
                        FzaokrType,
                        true,
                        out wtime, out ztime, out otimeD, out otimeN, out ntime, ref wtAlert);

                    int? zt, otD, otN, nt;
                    Worktime.WorktimeToPP(Fzaokr, FzaokrType, ztime, otimeD, otimeN, ntime, 
                                                  out zt, out otD, out otN, out nt);

                    UpdatePP(con, pracId, date, dr, 
                             czasIn, czasOut, wtime, zt, otD, otN, nt,    // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                             accKierId);

                    if (date >= accTo) break;   // dojechałem do końca zadanej akceptacji 
                }
            }

            bool ok = true; // na razie
            if (kp.DataAccDo == null || (DateTime)kp.DataAccDo < accTo)
                ok = KierParams.Update(con, kierId, accTo);   // aktualizauje kierownikowi do kiedy zaakceptowane dane ma mimo ze to nie on zaakceptował
            
            Base.Disconnect(con);
            return ok;
        }


        public static bool AkceptujPrac(string pracId, string DataOd, string DataDo, string onDay, string accKierId)   // DataOd, do - okres
        {
            const string sqlPrac =   // to samo co w PlanPracy.aspx
                  "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.IdKierownika, " + //<<<<<
                        "P.Kierownik, " +
                        "D.Nazwa as Dzial, " +
                        "ISNULL(RcpStrefaId, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierStrefaId " +
                                "else D.PracStrefaId " +
                            "end) as StrefaId, " +
                        "ISNULL(RcpAlgorytm, " +
                            "case when P.Kierownik=1 " +
                                "then D.KierAlgorytm " +
                                "else D.PracAlgorytm " +
                            "end) as Algorytm " +
                   "FROM Pracownicy P " +
                   "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";
            const string whereK = 
                   " WHERE P.IdKierownika = {0}";
                   //"ORDER BY P.Kierownik desc, NazwiskoImie";
            const string whereP = 
                   " WHERE P.Id = {0}";

            //----- pracownicy kierownika ------
            SqlConnection con = Base.Connect();
            
            //DataSet dsPrac = Base.getDataSet(String.Format(sqlPrac, kierId));
            DataSet dsPrac = Base.getDataSet(String.Format(sqlPrac + whereP, pracId));

            string kierId = Base.getValue(dsPrac, "IdKierownika");
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            KierParams kp = new KierParams(kierId, settings);
            //DateTime accTo = DateTime.MinValue;
            
            
            //----- parametry ------
            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;
            
            foreach (DataRow drv in dsPrac.Tables[0].Rows)
            {
                //string pracId = drv["Id"].ToString(); <<<<<<<
                bool kier = (bool)drv["Kierownik"];
                string rcpId = null;
                string strefaId = null;
                string alg = null;
                string algPar = null;

                //----- pobieranie danych ------
                DataSet ds;

                rcpId = drv["RcpId"].ToString();
                strefaId = drv["StrefaId"].ToString();
                alg = drv["Algorytm"].ToString();
                if (!String.IsNullOrEmpty(alg) && alg != "0" && alg != "1" && alg != "2" && alg != "11" && alg != "12")
                    algPar = Base.getScalar(con, "select Parametr from Kody where Typ='ALG' and Kod=" + alg);  //tak chyba będzie optymalniej niż dołączać do query
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";

                ds = Worktime.GetWorktime(con, pracId, rcpId, DataOd, DataDo, 
                        onDay,  //<<<<<<<
                        strefaId,
                        Fzaokr,
                        FzaokrType,
                        kp.Settings.NocneOdSec, kp.Settings.NocneDoSec);

                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = ds.Tables[0].Rows.Count;

                DateTime today = DateTime.Today;

                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getDataRow(ds, i);

                    DateTime date = Base.getDateTime(dr, "Data", today);  // musi byc ok
                    DateTime? czasIn = Base.getDateTime(dr, "TimeIn");
                    DateTime? czasOut = Base.getDateTime(dr, "TimeOut");

                    int wtAlert = 0;
                    int wtime, ztime, otimeD, otimeN, ntime;  // łączny, zmiana, nadgodziny, nocne
                    bool isWTime = Worktime.SolveWorktime2(dr, alg, algPar,
                        FBreakMM,
                        FBreak2MM,
                        FMarginMM,
                        Fzaokr,
                        FzaokrType,
                        true,
                        out wtime, out ztime, out otimeD, out otimeN, out ntime, ref wtAlert);

                    int? zt, otD, otN, nt;
                    Worktime.WorktimeToPP(Fzaokr, FzaokrType, ztime, otimeD, otimeN, ntime,
                                                  out zt, out otD, out otN, out nt);

                    UpdatePP(con, pracId, date, dr,
                             czasIn, czasOut, wtime, zt, otD, otN, nt,    // wtime = 0 jak nie ma !!! SolveWorkTime przerobic zeby zwracal null
                             accKierId);

                    //  <<<<<<<<<<<<<
                    /if (date >= accTo) break;   // dojechałem do końca zadanej akceptacji 
                      
                }
            }

            bool ok = true; // na razie
            //      <<<<<<<<<<
            //if (kp.DataAccDo == null || (DateTime)kp.DataAccDo < accTo)
            //    ok = KierParams.Update(con, kierId, accTo);   // aktualizauje kierownikowi do kiedy zaakceptowane dane ma mimo ze to nie on zaakceptował
            Base.Disconnect(con);
            return ok;
        }

         */
        //----- sprawdzenie braku planu pracy --------------------------
        public DataSet GetKierBrakPlanuPracy(SqlConnection con)   // prak pp ze zmiana i w dni pracujace nie ma absencji = jak zwroci date to sie lapie, jak nie to absencja dlugotrwala
        {
            return Base.getDataSet(String.Format(@"
declare @od datetime
declare @do datetime
set @od = '{0}'
set @do = '{1}'

select distinct
	--P.Id as IdPracownika, P.Nazwisko + ' ' + P.Imie as Pracownik,
	K.Id, K.Nazwisko + ' ' + K.Imie as KierownikNI, K.Mailing, K.Email, K.Status
from Pracownicy P
inner join Przypisania R on R.IdPracownika = P.Id and R.Od <= @do and ISNULL(R.Do, '20990909') >= @od and R.Status = 1
left join PracownicyParametry PA on PA.IdPracownika = P.Id and @od between PA.Od and ISNULL(PA.Do, '20990909')
left join PlanPracy PP on PP.IdPracownika = P.Id and PP.Data between @od and @do and PP.IdZmiany is not null
inner join Pracownicy K on K.Id = R.IdKierownika

outer apply (
	select top 1 D.Data from dbo.GetDates2(@od, @do) D
	left join Kalendarz K on K.Data = D.Data
	left join Absencja A on D.Data between A.DataOd and A.DataDo and A.IdPracownika = P.Id
	where K.Data is null and A.Kod is null 
	and D.Data between R.Od and ISNULL(R.Do, '20990909') -- tylko jezeli u przelozonego byli
) as AD

where PP.Id is null and P.Status >= 0 and ISNULL(PA.RcpAlgorytm, 1) <> 0 and AD.Data is not null
--order by Kierownik
            ", Tools.DateToStrDb(FDateFrom), Tools.DateToStrDb(FDateTo)));
        }
        
        //----- zamykanie okresu rozliczeniowego -----------------------
        public DataSet _GetKierNotClosed(SqlConnection con, DateTime dateTo)   // pobierz listę kierowników z niezaakceptowanymi pracownikami
        {
            //Ponieważ nie mam listy pracowników biorących udział w kontroli czasu więc
            //jeżeli kierownik nie zaakceptował żadnego pracownika to go nie zgłaszam.
            //Może przydałaby się taka lista ...

            //jeżeli nie ma w ogóle PP to pomijam - furtka
            //wszyscy przypisani w okresie, ze statusem >= 0 - podówjne zabezpieczenie
            //ostatni kierownik w okresie 
            return Base.getDataSet(con, String.Format(@"   
declare @od datetime
declare @do datetime
set @od = '{0}'
set @do = '{1}'

select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
from Przypisania R
left join Pracownicy K on K.Id = R.IdKierownika
left join PlanPracy PP on PP.IdPracownika = R.IdPracownika and PP.Data between dbo.MaxDate2(@od, R.Od) and dbo.MinDate2(@do, ISNULL(R.Do, '20990909'))
where R.Od <= @do and @od <= ISNULL(R.Do,'20990909') and R.Status = 1 and R.IdPracownika in (
	select distinct IdPracownika from PlanPracy where Data between @od and @do
)
and ISNULL(PP.Akceptacja, 0) = 0
order by KierownikNI        
                ",
                Tools.DateToStrDb(FDateFrom),
                Tools.DateToStrDb(dateTo)
                ));
        }

        /*
         -- najbardziej przejrzysta, ale nie znajdzie jak nie było PP w sprawdzanym okresie mimo ze jakieś PP poza było ustawione
        select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
        from PlanPracy PP 
        inner join Przypisania R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do,'20990909') and R.Status = 1
        left join Pracownicy P on P.Id = PP.IdPracownika
        left join Pracownicy K on K.Id = R.IdKierownika
        where PP.Data between @od and @do and PP.Akceptacja = 0
        order by KierownikNI        

        -- poprzednia 20150201 zwracała za dużo jak zwolniony/zatrudniony - wtedy nie musiało byc PP bo plan od daty zatrudnienia
        select distinct R.IdKierownika as Id, ISNULL(K.Nazwisko + ' ' + K.Imie, ' Poziom główny struktury') as KierownikNI, K.Mailing, K.Email, K.Status  
        --,SUM(convert(int,PP.Akceptacja))
        --,P.Nazwisko + ' ' + P.Imie as Pracownik
        from PlanPracy PP  
        left outer join Pracownicy P on P.Id = PP.IdPracownika  
        outer apply (select top 1 * from Przypisania where IdPracownika = PP.IdPracownika and Od <= @do and Status = 1 order by Od desc) as R
        left join Pracownicy K on K.Id = R.IdKierownika  
        where PP.Data between @od and @do  

        and P.Status >= 0

        and PP.IdPracownika in 
        (
        select IdPracownika from Przypisania where Od <= @do and ISNULL(Do, '20990909') >= @od and Status = 1
        )

        group by  
            R.IdKierownika, K.Nazwisko, K.Imie, K.Mailing, K.Email, K.Status,  
            PP.IdPracownika, P.Nazwisko, P.Imie  
        having SUM(convert(int,PP.Akceptacja)) < DATEDIFF(DAY, @od, @do) + 1 
        order by KierownikNI        
                 */

        public DataSet x_GetKierNotClosed(SqlConnection con, DateTime dateTo)   // pobierz listę kierowników z niezaakceptowanymi pracownikami
        {
            //Ponieważ nie mam listy pracowników biorących udział w kontroli czasu więc
            //jeżeli kierownik nie zaakceptował żadnego pracownika to go nie zgłaszam.
            //Może przydałaby się taka lista ...
            return Base.getDataSet(con, String.Format(   // uwaga Id potrzebne do maila !!!
                "select distinct P.IdKierownika as Id, K.Nazwisko + ' ' + K.Imie as KierownikNI, K.Mailing, K.Email, K.Status " +
                "from PlanPracy PP " +
                "left outer join Pracownicy P on P.Id = PP.IdPracownika " +
                "left outer join Pracownicy K on K.Id = P.IdKierownika " +
                "where PP.Data between {0} and {1} " +

                "and P.Status >= 0 " +
                //"and K.Status >= 0 " +  <<< kierownik nie moze zostać pominięty !!! tylko jego ludzie bo będzie sytuacja kiedy 

                "group by " +
                    "P.IdKierownika, K.Nazwisko + ' ' + K.Imie, K.Mailing, K.Email, K.Status, " +
                    "PP.IdPracownika, P.Nazwisko + ' ' + P.Imie " +
                "having SUM(convert(int,PP.Akceptacja)) < {2} " +
                "order by KierownikNI",
                Base.dateParam(FDateFrom),
                Base.dateParam(dateTo),
                Tools.GetDaysCount(FDateFrom, dateTo)
                ));
        }

        public bool _CheckClose(DateTime dateTo)
        {
            DataSet ds = _GetKierNotClosed(null, dateTo);
            
            
            
            //test !!!
            //return true;




            if (Base.getRows(ds).Count == 0) return true;
            else
            {
                string msg = null;
                string mailing = null;
                foreach (DataRow dr in Base.getRows(ds))
                {
                    if (Base.getValue(dr, "Id") == "0")
                        msg += "\\n- Poziom główny struktury";
                    else
                    {
                        string kier = Base.getValue(dr, "KierownikNI");
                        string mail = Base.getValue(dr, "Email");
                        if (String.IsNullOrEmpty(mail))
                            mail = String.Format("--- {0} - brak maila ---", kier);
                        msg += "\\n- " + kier;
                        mailing += mail + ","; 
                    }
                }
                const string MsgStr = "Zamknięcie niemożliwe dopóki następujący kierownicy nie zaakceptują czasu pracy: ";
                Log.Info(Log.t2APP_OKRESCLOSEWEEK, MsgStr + msg, mailing);
                Tools.ShowMessages(MsgStr + msg, mailing);
                return false;
            }
            /*
            declare @dFrom DateTime 
            declare @dTo DateTime 
            declare @days int
            set @dFrom = '2012-01-21'
            set @dTo = '2012-02-20'
            set @days = DATEDIFF(DAY, @dFrom, @dTo) + 1             
            ------ kierownik - pracownicy ----------------------------------
            -- zbieram wszystkich którzy mają jakiekolwiek korekty i zaakceptowanych mają < il.dni
            select 
            --distinct
            P.IdKierownika, K.Nazwisko + ' ' + K.Imie as Kierownik, 
            PP.IdPracownika, P.Nazwisko + ' ' + P.Imie as Pracownik
            ,COUNT(*), SUM(convert(int,PP.Akceptacja)) as acc
            from PlanPracy PP 
            left outer join Pracownicy P on P.Id = PP.IdPracownika
            left outer join Pracownicy K on K.Id = P.IdKierownika
            where PP.Data between @dFrom and @dTo 
            group by 
            P.IdKierownika, K.Nazwisko + ' ' + K.Imie,
            PP.IdPracownika, P.Nazwisko + ' ' + P.Imie 
            --having COUNT(*) < @days
            having SUM(convert(int,PP.Akceptacja)) < @days
            order by Kierownik, Pracownik
            ------ tylko kierownicy -----------------------------------------
            select 
            distinct
            P.IdKierownika, K.Nazwisko + ' ' + K.Imie as Kierownik 
            --,PP.IdPracownika, P.Nazwisko + ' ' + P.Imie as Pracownik
            --,COUNT(*), SUM(convert(int,PP.Akceptacja)) as acc
            from PlanPracy PP 
            left outer join Pracownicy P on P.Id = PP.IdPracownika
            left outer join Pracownicy K on K.Id = P.IdKierownika
            where PP.Data between @dFrom and @dTo 
            group by 
            P.IdKierownika, K.Nazwisko + ' ' + K.Imie,
            PP.IdPracownika, P.Nazwisko + ' ' + P.Imie 
            --having COUNT(*) < @days
            having SUM(convert(int,PP.Akceptacja)) < @days
            order by Kierownik
            */
        }










        public bool CheckClose(DateTime dateTo, string sql)
        {
            //DataSet ds = GetKierNotClosed(null, dateTo);

            DataSet ds = Base.getDataSet(String.Format(sql, Tools.DateToStrDb(dateTo)));   
            
            if (Base.getRows(ds).Count == 0) return true;
            else
            {
                string msg = null;
                string mailing = null;
                foreach (DataRow dr in Base.getRows(ds))
                {
                    if (Base.getValue(dr, "Id") == "0")
                        msg += "\\n- Poziom główny struktury";
                    else
                    {
                        string kier = Base.getValue(dr, "KierownikNI");
                        string mail = Base.getValue(dr, "Email");
                        if (String.IsNullOrEmpty(mail))
                            mail = String.Format("--- {0} - brak maila ---", kier);
                        msg += "\\n- " + kier;
                        mailing += mail + ",";
                    }
                }
                const string MsgStr = "Zamknięcie niemożliwe dopóki następujący kierownicy nie zaakceptują czasu pracy: ";
                Log.Info(Log.t2APP_OKRESCLOSEWEEK, MsgStr + msg, mailing);
                Tools.ShowMessages(MsgStr + msg, mailing);
                return false;
            }
        }










        //-----------------------------------------------
        public bool IsArch()
        {
            switch (Status)
            {
                default:
                case stNotExists: 
                    return false;
                case stClosed: 
                    return true;
                case stOpen:
                    string id = Base.getScalar("select top 1 Id from PracownicyOkresy where IdOkresu = " + FId);
                    return !String.IsNullOrEmpty(id);                    
            }
        }
        
        //private void CopyStructure(SqlConnection con)
        private void CopyStructure(SqlTransaction tr)
        {
            if (!IsArch())                  // nie ma to kopiuję, jak są już wpisy to nie nadpisuję !!! - póki co potem zobaczymy; po imporcie struktury zamknięcie miesiąca przeniosłoby nową strukturę ...
                Base.execSQL(tr,            // aktualizauję poprawne rcpstrefa i algoryrm, bez wnikania w działy
                    "insert into PracownicyOkresy " +
                               "(IdOkresu,Id,Imie,Nazwisko,IdDzialu,IdStanowiska,IdKierownika,IdProjektu,Kierownik,KadryId,Stawka,RcpId,Status,RcpStrefaId,RcpAlgorytm,EtatL,EtatM,CCInfo,Rights,GrSplitu) " +
                    "select " + FId + ",P.Id,P.Imie,P.Nazwisko,P.IdDzialu,P.IdStanowiska,P.IdKierownika,P.IdProjektu,P.Kierownik,P.KadryId,P.Stawka,P.RcpId,P.Status," +
                    "ISNULL(RcpStrefaId, case when P.Kierownik=1 then D.KierStrefaId else D.PracStrefaId end)," +
                    "ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end)," + 
                    "P.EtatL,P.EtatM," +
                    "ISNULL(CCInfo, case when P.Kierownik=1 then D.KierCCInfo else D.PracCCInfo end)," +
                    "Rights,GrSplitu " +
                    "from Pracownicy P " + 
                    "left outer join Dzialy D ON D.Id = P.IdDzialu");
        }

        public bool _Close()
        {
            SqlTransaction tr = Base.Connect().BeginTransaction();
            //----- kopiowanie brakujących wartości ????

            //----- aktualizacja statusu okresu miesięcznego -----
            AppUser user = AppUser.CreateOrGetSession();
            int pid = Log.Info(Log.t2APP_OKRESCLOSE, "Zamknięcie okresu rozliczeniowego", String.Format("{0} {1} - {2}", FId, Base.DateToStr(FDateFrom), Base.DateToStr(FDateTo)), Log.OK);
            bool b;
            if (FId == -1) // nie istnieje
            {
                FId = Base.insertSQL(tr, Base.insertSql("OkresyRozl", 0,
                    "DataOd, DataDo, DataBlokady, Status, Zamknal, DataZamkniecia, Archiwum",
                    Base.dateParam(FDateFrom),
                    Base.dateParam(FDateTo),
                    Base.dateParam(FDateTo),
                    1,
                    user.OriginalId, 
                    "GETDATE()",
                    1));
                b = FId != -1;
            }
            else
                b = Base.execSQL(tr, Base.updateSql("OkresyRozl", 0,
                    "DataOd, DataDo, DataBlokady, Status, Zamknal, DataZamkniecia, Archiwum",
                    "Id = " + FId,
                    Base.dateParam(FDateFrom),
                    Base.dateParam(FDateTo),
                    Base.dateParam(FDateTo),
                    1,
                    user.OriginalId,
                    "GETDATE()",
                    1));
            if (b)
            {
                CopyStructure(tr);
                //----- okres rozliczeniowy -----
                Base.execSQL(tr, Base.updateSql("OkresyRozliczeniowe", 1, 
                    "Status,Zamknal,DataZamkniecia", 
                    "DataDo = {0}", 
                    Base.dateParam(FDateTo),
                    1, 
                    user.OriginalId,
                    "GETDATE()"));
            }
            else
                Log.Error(Log.t2APP_OKRESCLOSE, pid, "Błąd podczas zamykania okresu rozliczeniowego", FId + " " + OdDoStr);  // update nie poszedł wiec nie ma co w transakcji odwijać
            Base.Disconnect(tr);
            return b;
        }

        public bool SetLockedTo(DateTime dt)
        {
            bool close = dt > FLockedTo;
            FLockedTo = dt;
            int pid = Log.Info(Log.t2APP_OKRESCLOSEWEEK, 
                close ? "Zamknięcie tygodnia" : "Odblokowanie tygodnia", 
                String.Format("{0} {1} - {2}, blokada do: {3}", FId, Base.DateToStr(FDateFrom), Base.DateToStr(FDateTo), Base.DateToStr(dt)), Log.OK);
            bool b;
            if (FId == -1) // nie istnieje
            {
                FId = db.insert("OkresyRozl", true, true, 0,
                    "DataOd, DataDo, DataBlokady, Status",//, Zamknal",
                    Base.dateParam(FDateFrom),
                    Base.dateParam(FDateTo),
                    Base.dateParam(FLockedTo),
                    0);//,App.User.OriginalId);
                b = FId != -1;
            }
            else
                b = db.update("OkresyRozl", 0,
                    "DataBlokady",//, Zamknal",
                    "Id = " + FId,
                    Base.dateParam(FLockedTo));//,App.User.OriginalId);
            if (!b)
                Log.Error(Log.t2APP_OKRESCLOSEWEEK, pid, "Błąd podczas zamykania/odblokowywania tygodnia", FId + " " + OdDoStr);  
            return b;
        }

        public DateTime GetProperLockedTo(DateTime day)    // zwraca niedzielę przed day
        {
            return Tools.bow(day, DayOfWeek.Monday).AddDays(-1);
        }

        public bool _Reopen()
        {
            int pid = Log.Info(Log.t2APP_OKRESREOPEN, "Odblokowanie okresu rozliczeniowego", String.Format("{0} {1} - {2}", FId, Base.DateToStr(FDateFrom), Base.DateToStr(FDateTo)), Log.OK);
            SqlTransaction tr = Base.Connect().BeginTransaction();

            bool b = Base.execSQL(tr, "update OkresyRozl set Status=0, DataBlokady=null where Id=" + FId);   //DATEADD(DAY,-1,DataOd)
            if (b)
            {
                //Base.execSQL(tr, "delete from PracownicyOkresy where IdOkresu = " + FId);  nie kasuję !!!
                Base.execSQL(tr, "update OkresyRozliczeniowe set Status=0 where DataDo=" + Base.dateParam(FDateTo));   
            }
            else
                Log.Error(Log.t2APP_OKRESREOPEN, pid, "Błąd podczas odblokowywania okresu rozliczeniowego", FId + " " + OdDoStr);

            Base.Disconnect(tr);
            return b;
        }

        /*
        public bool Close()
        {
            SqlConnection con;
            if (Fcon != null) con = Fcon;  // insert sie wywala jak bylo null
            else con = Base.Connect();
            //----- kopiowanie brakujących wartości ????

            //----- aktualizacja statusu -----
            AppUser user = AppUser.CreateOrGetSession();
            int pid = Log.Info(Log.t2APP_OKRESCLOSE, "Zamknięcie okresu rozliczeniowego", String.Format("{0} {1} - {2}", FId, Base.DateToStr(FDateFrom), Base.DateToStr(FDateTo)), Log.OK);
            bool b;
            if (FId == -1) // nie istnieje
            {
                FId = Base.insertSQL(con, Base.insertSql("OkresyRozl", 0,
                    "DataOd, DataDo, Status, Zamknal",
                    Base.dateParam(FDateFrom),
                    Base.dateParam(FDateTo),
                    1,
                    user.Id));
                b = FId != -1;
            }
            else
                b = Base.execSQL(con, Base.updateSql("OkresyRozl", 0,
                    "DataOd, DataDo, Status, Zamknal",
                    "Id = " + FId,
                    Base.dateParam(FDateFrom),
                    Base.dateParam(FDateTo),
                    1,
                    user.Id
                    ));
            if (b)
                CopyStructure(con, FId);
            else
                Log.Error(Log.t2APP_OKRESCLOSE, pid, null, null);
            if (Fcon == null)
                Base.Disconnect(con);
            return b;
        }

        public bool Reopen()
        {
            int pid = Log.Info(Log.t2APP_OKRESREOPEN, "Odblokowanie okresu rozliczeniowego", String.Format("{0} {1} - {2}", FId, Base.DateToStr(FDateFrom), Base.DateToStr(FDateTo)), Log.OK);
            bool b = Base.execSQL(Fcon, "update OkresyRozl set Status=0 where Id=" + FId);
            if (!b)
                Log.Error(Log.t2APP_OKRESREOPEN, pid, null, null);
            return b;
        }
         */
        //--------------------------------------------------------------
        public static DataRow GetRozliczeniowy(DateTime data)
        {
            return db.getDataRow(String.Format("select * from OkresyRozliczeniowe where '{0}' between DataOd and DataDo", Tools.DateToStrDb(data)));
        }

        public static DataRow GetRozliczeniowy(string data)
        {
            return db.getDataRow(String.Format("select * from OkresyRozliczeniowe where '{0}' between DataOd and DataDo", data));
        }

        public static DataRow GetRozliczeniowy(string data, out DateTime dataOd, out DateTime dataDo, out int status)
        {
            DataRow dr = GetRozliczeniowy(data);
            if (dr != null)
            {
                dataOd = db.getDateTime(dr, "DataOd", DateTime.MinValue);
                dataDo = db.getDateTime(dr, "DataDo", DateTime.MaxValue);
                status = db.getInt(dr, "Status", 0);
                return dr;
            }
            else
            {
                dataOd = DateTime.MinValue;
                dataDo = DateTime.MaxValue;
                status = 0;
                return null;
            }
        }
        
            
        //--------------------------------------------------------------
        public int Id
        {
            get { return FId; }
        }

        public DateTime DateFrom
        {
            get { return FDateFrom; }
        }

        public DateTime DateTo
        {
            get { return FDateTo; }
        }

        public DateTime LockedTo
        {
            get { return FLockedTo; }
        }
        //--------------
        public DateTime NextLockTo    // zablokuj do 
        {
            get  // -----sn--1--sn-----sn-----sn-----sn-----sn
            {
                DateTime dt = Tools.bow(LockedTo.AddDays(1), DayOfWeek.Monday).AddDays(6);
                if (dt > DateTo) dt = DateTo;
                return dt;
            }
        }

        public DateTime UnlockTo    // odblokuj do - pierwszy dzień odblokowany do SetLockedTo przekazac UnlockTo - 1
        {
            get  
            {
                DateTime dt = Tools.bow(LockedTo, DayOfWeek.Monday);
                if (dt < DateFrom) dt = DateFrom;
                return dt;
            }
        }
        //--------------
        public int Status
        {
            get { return _FStatus; }
        }

        public int StatusPL
        {
            get { return FStatusPL; }
        }

        public string ZamknalId
        {
            get { return FZamknalId; }
        }

        public double StawkaNocna
        {
            get { return FStawkaNocna; }
        }

        public string OdDoStr
        {
            get { return Base.DateToStr(FDateFrom) + " - " + Base.DateToStr(FDateTo); }
        }

        public string OdDoNaliczeniaStr
        {
            get { return Base.DateToStr(DataNaliczeniaPrev) + " - " + Base.DateToStr(DataNaliczenia); }
        }

        public string OdNalDoStr
        {
            get { return Base.DateToStr(DataNaliczeniaPrev) + " - " + Base.DateToStr(FDateTo); }
        }

        public DateTime DataNaliczenia
        {
            get { return FDataNaliczenia; }
        }

        public DateTime DataNaliczeniaPrev
        {
            get
            {
                if (FDataNaliczeniaPrev == null)
                {
                    FDataNaliczeniaPrev = db.getDateTime(db.Select.Row("select DataNaliczenia from OkresyRozl oro where oro.DataDo = DATEADD(DAY, -1, '{0}')", Tools.DateToStrDb(FDateFrom)), 0, FDateFrom.AddDays(-1));
                }
                return (DateTime)FDataNaliczeniaPrev;
            }
        }
    }
}

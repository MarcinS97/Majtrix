using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.App_Code
{
    public class Worktime1
    {
        const int margin = 5; // margines dni pobierania danych; pomyslec nad znalezieniem 1 przed i 1 po bo to wystarczy

        SqlConnection con;
        bool fDoDisconnect;
        
        string FPracId;
        string FRcpId;
        string FStrefaId;
        string FDateFrom;
        string FDateTo;
        string FNocFromSec;
        string FNocToSec;   // zawsze > FNocFromSec, dodaję 86400
        string FSesId;
        string FSqlSesId;   // w '' 

        public void Prepare(SqlConnection acon, // jak null to łączy i rozłącza, jak !null to korzysta z przekazanego
                        string pracId,      // musi być
                        string rcpId, 
                        string strefaId,    // jak null to bierze z historii zmian stref pracownika, a jak nie ma to działu, na razie nie ma obsługi
                        string dateFrom, string dateTo, 
                        int nocFromSec, int nocToSec,
                        string id)          // dodatkowy id dokładany do sesji, sprawdzic czy jednak nie pojdzie na tablicach tymczasowych
        {
            if (acon == null)
            {
                con = Base.Connect();
                fDoDisconnect = true;
            }
            else
            {
                con = acon;
                fDoDisconnect = false;
            }

            FSesId = id + Base.UniqueId(50 - id.Length);
            FSqlSesId = Base.strParam(FSesId);
            //DeleteTmpData(); 

            FPracId = pracId;
            FRcpId = rcpId;
            FStrefaId = strefaId;
            FDateFrom = dateFrom;
            FDateTo = dateTo;

            if (nocFromSec == 0 && nocToSec == 0)
            {
                FNocFromSec = null;
                FNocToSec = null;
            }
            else
            {
                int d = nocFromSec > nocToSec ? 86400 : 0;      // zakładam 1 dobę !!!
                FNocFromSec = nocFromSec.ToString();
                FNocToSec = (nocToSec + d).ToString();
            }

            if (!String.IsNullOrEmpty(rcpId))
            {
                GetReadersData();  // -> tmpRCP1 zbieram zakres danych
                PrepareWorkime3(); // -> tmpRCP2 dbo.GetRcpData do tablicy bo jest kilkukrotnie wykorzystywana później
                GetWorkime3();     // -> tmpRCP3
            }
        }

        public void DeleteTmpData()
        {
            Base.execSQL(con, "delete from tmpRCP1 where sesId = " + FSqlSesId);
            Base.execSQL(con, "delete from tmpRCP2 where sesId = " + FSqlSesId);
            Base.execSQL(con, "delete from tmpRCP3 where sesId = " + FSqlSesId);
            /**/
        }

        public void Unload()
        {
            DeleteTmpData();
            if (fDoDisconnect) Base.Disconnect(con);
        }

        //----------------------------------------------------------------------------
        private void GetReadersData() // -> tmpRCP1
        {
            DateTime dtFrom = DateTime.Parse(FDateFrom).AddDays(-margin);    // zeby poprawnie złapac początek, mozna dać więcej, dtFrom >=, dtTo <
            DateTime dtTo = DateTime.Parse(FDateTo).AddDays(1 + margin);     // zeby poprawnie złapać koniec

            // !!! dołożyć kręcenie się po zmianach strefy i algorytmu i RcpId !!!

            if (!String.IsNullOrEmpty(FStrefaId))   // nie mam strefy - nie mam po czym zebrać danych
            {
                DataSet ds = Base.getDataSet(con,
                    "select * from StrefyReaders where IdStrefy = " + FStrefaId + " order by DataOd");  //and Readers <> ''  wyrzucam zeby mozna bylo wylaczyc cała strefę

                string s = null;
                int max = ds.Tables[0].Rows.Count;
                for (int i = 0; i < max; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    DataRow dr1 = null;
                    DateTime dtOd = (DateTime)dr["DataOd"];
                    DateTime dtOd1 = DateTime.MaxValue;
                    string czas = null;

                    if (dtOd >= dtTo) break; // koncze, poza zakresem, do dtTo dodany jest 1 dzień i nierownosci mają byc ostre !!!, a tu warunek na następny okres więc >=
                    else                    // 
                    {
                        if (i < max - 1)
                        {
                            dr1 = ds.Tables[0].Rows[i + 1];
                            dtOd1 = (DateTime)dr1["DataOd"];
                            if (dtOd1 <= dtFrom) continue;          // koniec poza zakresem, pomijam, początek złapię następnym
                            if (dtOd < dtFrom) dtOd = dtFrom;       // ograniczam zakres z dołu
                            if (dtOd1 >= dtTo) dtOd1 = dtTo;        // ograniczam zakres z góry 
                        }
                        else
                        {
                            if (dtOd < dtFrom) dtOd = dtFrom;       // ograniczam zakres z dołu
                            dtOd1 = dtTo;                           // ograniczam zakres z góry 
                        }
                        czas += "Czas >= " + Base.strParam(Base.DateTimeToStr(dtOd)) +
                           " and Czas < " + Base.strParam(Base.DateTimeToStr(dtOd1));

                        string rio = Base.getValue(dr, "Readers");
                        string r = rio.Replace("-", "");

                        if (!String.IsNullOrEmpty(s)) s += " union ";

                        s += "select " + FSqlSesId + " as sesId," +
                                "*," +
                                "case " +
                                    "when ECReaderId in (" + rio + ") then convert(bit, 0) " +
                                    "else convert(bit, 1) " +
                                "end as InOut2 " +
                            "from RCP where " + czas +
                                " and ECReaderId in (" + r + ")" +
                                " and ECUserId = " + FRcpId;        // też może byc in () !!! bo nie bedzie w tym samym czasie 2 używać - więc w konfiguracji pracownika wystarczy zmienić pole na string i zapisywać z , !!!
                    }
                }
                if (!String.IsNullOrEmpty(s))
                    Base.execSQL(con, "insert into tmpRCP1 " + s + " order by Czas");   // order nie moze byc w union
            }
        }

        // zakładam istnienie wejscia w danym dniu
        private void PrepareWorkime3() // -> tmpRCP2
        {
            Base.execSQL("insert into tmpRCP2 " +
                "select " + FSqlSesId + " as sesId," +
                    "ECUniqueId, 0, 0," +
                    "TimeBefore,TimeIn,TimeOut,TimeAfter," +
                    "worktime,beforetime,aftertime " +
                "from dbo.GetRcpData(" + FSqlSesId + ")");
        }

        private void GetWorkime3() // -> tmpRCP3
        {
            const string minBefore = "21600";  // 6h
            string dOd = Base.strParam(FDateFrom);
            string dDo = Base.strParam(FDateTo);

            string interSecFields = String.IsNullOrEmpty(FNocFromSec) ? "null,null" :
                    "dbo.TimeInterSec(A.TimeIn,B.Czas,DATEADD(SECOND,"     + FNocFromSec + ",D.Data),DATEADD(SECOND," + FNocToSec + ",D.Data)) as nighttime," +
                    "dbo.TimeInterSumSec(A.TimeIn, B.Czas,DATEADD(SECOND," + FNocFromSec + ",D.Data),DATEADD(SECOND," + FNocToSec + ",D.Data)," + FSqlSesId + ") as nighttime2";

            string interSecFieldsPoNocy = String.IsNullOrEmpty(FNocFromSec) ? "null,null" :
                    "datediff(second,'1900-01-01',B.Czas - DATEADD(SECOND," + FNocToSec + ",D.Data)) as ponocy, " +
                    "dbo.TimeInterSumSec(DATEADD(SECOND," + FNocToSec + ",D.Data), B.Czas, A.TimeIn, B.Czas," + FSqlSesId + ") as ponocy2";
            
            Base.execSQL(con, "insert into tmpRCP3 " +
                "select " + FSqlSesId + " as sesId," +
                    "D.Data, A.TimeIn, B.Czas," +
                    "datediff(second,'1900-01-01',B.Czas - A.TimeIn) as worktime, " +
                    "(select sum(Z.worktime) from tmpRCP2 Z where sesId = " + FSqlSesId + " and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.Czas) as worktime2, " +
                    "A.beforetime, null as aftertime," +
                    interSecFields + "," +
                    interSecFieldsPoNocy +
                " from GetDates2(" + dOd + "," + dDo + ") D " +
                    //----- najdłuższa przerwa przed tego dnia -----
                    "left outer join tmpRCP2 A on A.sesId = " + FSqlSesId + " and A.ECUniqueId = " +
                        "(select top 1 X1.ECUniqueId from tmpRCP2 X1 " +
                        "where X1.sesId = " + FSqlSesId +
                            " and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data)" +

                            " and X1.beforetime > " + minBefore +  

                        " order by X1.beforetime desc) " +
                    //----- najdłuższa przerwa przed następnego dnia -----
                    "left outer join tmpRCP2 C on C.sesId = " + FSqlSesId + " and C.ECUniqueId = " +
                        "(select top 1 X3.ECUniqueId from tmpRCP2 X3 " +
                        "where X3.sesId = " + FSqlSesId +
                            " and X3.TimeIn >= DATEADD(DAY, 1, D.Data)" +

                            " and X3.beforetime > " + minBefore +  
                            
                        " order by LEFT(CONVERT(varchar,X3.TimeIn,20),10), X3.beforetime desc) " +
                    //----- ostatni odczyt przed X3, który się łapie -----
                    "left outer join tmpRCP1 B on B.sesId = " + FSqlSesId + " and B.ECUniqueId = " +
                        "(select top 1 X2.ECUniqueId from tmpRCP1 X2 " +
                        "where X2.sesId = " + FSqlSesId +
                            "and X2.Czas > A.TimeIn and X2.Czas < " +
                                                        //"C.TimeIn " +
                                                        "ISNULL(C.TimeIn,DATEADD(DAY, 1, A.TimeIn)) " +  // C.TimeIn jest null jesli w okresie nie ma już danych - więc wtedy na pewno mam do czynienia z ostatnim wyjściem - poki co ten !
                                                        //"case when D.Data < cast(floor(cast(GETDATE() as float)) as datetime) then ISNULL(C.TimeIn,DATEADD(DAY, 1, A.TimeIn)) else NULL end " +  // z uwzględnieniem żeby nie pokazywać danych z dnia dizsiejszego - moze dodam ...
                            "and X2.Czas < DATEADD(DAY, 1, A.TimeIn) " +
                        "order by X2.Czas desc) " +
                        "and B.InOut2 = 1");
        }


        /* 2012-04-02 poprawka ostatni dzień 
            Base.execSQL(con, "insert into tmpRCP3 " +
                "select " + FSqlSesId + " as sesId," +
                    "D.Data, A.TimeIn, B.Czas," +
                    "datediff(second,'1900-01-01',B.Czas - A.TimeIn) as worktime, " +
                    "(select sum(Z.worktime) from tmpRCP2 Z where sesId = " + FSqlSesId + " and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.Czas) as worktime2, " +
                    "A.beforetime, null as aftertime," +
                    interSecFields + "," +
                    interSecFieldsPoNocy +
                " from GetDates2(" + dOd + "," + dDo + ") D " +
                    "left outer join tmpRCP2 A on A.sesId = " + FSqlSesId +
                        " and A.ECUniqueId = (select top 1 X1.ECUniqueId from tmpRCP2 X1 " +
                        "where X1.sesId = " + FSqlSesId +
                            " and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data) " +
                        "order by X1.beforetime desc) " +
                    "left outer join tmpRCP2 C on C.sesId = " + FSqlSesId +
                        " and C.ECUniqueId = " +
                        "(select top 1 X3.ECUniqueId from tmpRCP2 X3 " +
                        "where X3.sesId = " + FSqlSesId +
                            "and X3.TimeIn >= DATEADD(DAY, 1, D.Data) " +
                        "order by LEFT(CONVERT(varchar,X3.TimeIn,20),10), X3.beforetime desc) " +
                    "left outer join tmpRCP1 B on B.sesId = " + FSqlSesId +
                        " and B.ECUniqueId = " +
                        "(select top 1 X2.ECUniqueId from tmpRCP1 X2 " +
                        "where X2.sesId = " + FSqlSesId +
                            "and X2.Czas > A.TimeIn and X2.Czas < C.TimeIn and X2.Czas < DATEADD(DAY, 1, A.TimeIn) " +
                        "order by X2.Czas desc) " +
                        "and B.InOut2 = 1");
        }

         */

        /*
        private void GetWorkime3() // -> tmpRCP3
        {
            string dOd = Base.strParam(FDateFrom);
            string dDo = Base.strParam(FDateTo);
            Base.execSQL(con, "insert into tmpRCP3 " +
                "select " + FSqlSesId + " as sesId," +
                    "D.Data, A.TimeIn, B.Czas," +
                    "datediff(second,'1900-01-01',B.Czas - A.TimeIn) as worktime, " +
                    "(select sum(Z.worktime) from tmpRCP2 Z where sesId = " + FSqlSesId + " and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.Czas) as worktime2, " +
                    "A.beforetime, null as aftertime " +
                "from GetDates2(" + dOd + "," + dDo + ") D " +
                    "left outer join tmpRCP2 A on A.sesId = " + FSqlSesId +
                        " and A.ECUniqueId = (select top 1 X1.ECUniqueId from tmpRCP2 X1 " +
                        "where X1.sesId = " + FSqlSesId +
                            " and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data) " +
                        "order by X1.beforetime desc) " +
                    "left outer join tmpRCP2 C on C.sesId = " + FSqlSesId +
                        " and C.ECUniqueId = " +
                        "(select top 1 X3.ECUniqueId from tmpRCP2 X3 " +
                        "where X3.sesId = " + FSqlSesId +
                            "and X3.TimeIn >= DATEADD(DAY, 1, D.Data) " +
                        "order by LEFT(CONVERT(varchar,X3.TimeIn,20),10), X3.beforetime desc) " +
                    "left outer join tmpRCP1 B on B.sesId = " + FSqlSesId +
                        " and B.ECUniqueId = " +
                        "(select top 1 X2.ECUniqueId from tmpRCP1 X2 " +
                        "where X2.sesId = " + FSqlSesId +
                            "and X2.Czas > A.TimeIn and X2.Czas < C.TimeIn and X2.Czas < DATEADD(DAY, 1, A.TimeIn) " +
                        "order by X2.Czas desc) " +
                        "and B.InOut2 = 1");
        }
         */

        //----------------------------------------------------
        private string WhereCzas(string nTimeIn, string nTimeOut, string dateFrom, string dateTo)
        {
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (fromIsNull && toIsNull)
                return null;
            else if (!fromIsNull && !toIsNull)
                return "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas <= " + Base.strParam(dateTo);
            else if (toIsNull)
                return "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas <= DATEADD(DAY, 1, " + Base.strParam(dateFrom) + ")";
            else  // fromIsNull
                return "A.Czas >= DATEADD(DAY, -1, " + Base.strParam(dateTo) + ") and A.Czas <= " + Base.strParam(dateTo);
        }

        public DataSet GetDetails(string dateFrom, string dateTo, int round, int rtype) // podgląd RCP: czasy składowe in-out
        {
            string czas;
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (!fromIsNull && !toIsNull)
                czas = "TimeIn >= " + Base.strParam(dateFrom) + " and TimeOut <= " + Base.strParam(dateTo);
            else if (toIsNull)
                czas = "TimeIn >= " + Base.strParam(dateFrom) + " and TimeOut <= DATEADD(DAY, 1, " + Base.strParam(dateFrom) + ")";
            else if (fromIsNull)
                czas = "TimeIn >= DATEADD(DAY, -1, " + Base.strParam(dateTo) + ") and TimeOut <= " + Base.strParam(dateTo);
            else
                return null;
            return Base.getDataSet(con,
                "select TimeIn, TimeOut," +
                    "dbo.ToTime(worktime) as Czas," +
                    "dbo.RoundTime(worktime, " + round.ToString() + "," + rtype.ToString() + ") as CzasR," +
                    "worktime " +
                "from tmpRCP2 " +
                "where sesId = " + FSqlSesId + " and " + czas +
                " order by TimeIn");
        }

        public DataSet GetDetails2(string dateFrom, string dateTo)   // podgląd RCP: dane z rejestratorów w strefie
        {
            string czas;
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (!fromIsNull && !toIsNull)
                czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas <= " + Base.strParam(dateTo);
            else if (toIsNull)
                czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas <= DATEADD(DAY, 1, " + Base.strParam(dateFrom) + ")";
            else if (fromIsNull)
                czas = "A.Czas >= DATEADD(DAY, -1, " + Base.strParam(dateTo) + ") and A.Czas <= " + Base.strParam(dateTo);
            else
                return null;
            return Base.getDataSet(con,
                "select A.Czas, A.ECReaderId, A.InOut2, R.Name " + 
                "from tmpRCP1 A " +
                "left outer join Readers R on R.Id = A.ECReaderId " +
                "where sesId = " + FSqlSesId + " and " + czas +
                " order by A.Czas");
        }

        public DataSet GetDetails3(string dateFrom, string dateTo)   // podgląd RCP: dane ze wszystkich rejestratorów 
        {
            string czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas < " + Base.strParam(dateTo);
            return Base.getDataSet(con,
                "select A.Czas, A.ECReaderId, B.InOut2, R.Name, A.InOut " +
                "from RCP A " +
                "left outer join tmpRCP1 B on sesId = " + FSqlSesId + " and B.ECUniqueId = A.ECUniqueId " +
                "left outer join Readers R on R.Id = A.ECReaderId " +
                "where A.ECUserId = " + FRcpId + " and " + czas +
                " order by A.Czas");
        }

        public void SumTime(out string sum, out string sumR, out string sum2, out string sum2R, int dayRound, int drType, int sumRound, int srType)
        {
            string dayr = dayRound.ToString();
            string dayrt = drType.ToString();
            string sr = sumRound.ToString();
            string srt = srType.ToString();
            DataRow dr = Base.getDataRow(con, 
                "select dbo.ToTime(sum(dbo.RoundSec(worktime," + dayr + "," + dayrt + "))) as Suma," +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime," + dayr + "," + dayrt + "))," + sr + "," + srt + ") as SumaR," +
                       "dbo.ToTime(sum(dbo.RoundSec(worktime2," + dayr + "," + dayrt + "))) as Suma2, " +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime2," + dayr + "," + dayrt + "))," + sr + "," + srt + ") as Suma2R " +
                "from tmpRCP3 where sesId = " + FSqlSesId);
            if (dr == null)
            {
                sum = "00:00:00";
                sumR = "0";
                sum2 = "00:00:00";
                sum2R = "0";
            }
            else
            {
                sum = Base.getValue(dr, 0); 
                sumR = Base.getValue(dr, 1);
                sum2 = Base.getValue(dr, 2);
                sum2R = Base.getValue(dr, 3);
                if (String.IsNullOrEmpty(sum)) sum = "00:00:00";
                if (String.IsNullOrEmpty(sumR)) sum = "0";
                if (String.IsNullOrEmpty(sum2)) sum = "00:00:00";
                if (String.IsNullOrEmpty(sum2R)) sum = "0";
            }
        }

        //-------------------------------------------------------------
        public static DataSet _GetWorktime(SqlConnection con, string pracId, string rcpId,   // rcpId == null => bierze tylko PP, bez rcp
                    string fromTime, string toTime, string onDay,
                    string strefaId, int zaokr, int zaokrType, int nocneOdSec, int nocneDoSec)
        {
            DataSet ds;
            string dFrom = String.IsNullOrEmpty(onDay) ? fromTime : onDay;
            string dTo = String.IsNullOrEmpty(onDay) ? toTime : onDay;
            if (String.IsNullOrEmpty(rcpId))  // brak RCP - jak moZmiany, bo nie jest rejestrowany
            {
                ds = Base.getDataSet(con, String.Format(
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
                        "P.Czas," +
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
                        "AK.GodzinPracy, AK.DniWolne " +
                    "from GetDates2('{1}','{2}') D " +
                        "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
                        "left outer join Zmiany Z on Z.Id = ISNULL(P.IdZmianyKorekta, P.IdZmiany) " +
                        "left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                        "left outer join AbsencjaKody AK on AK.Kod = ISNULL(A.Kod, P.Absencja) " +
                    "order by D.Data",
                        pracId, dFrom, dTo));
            }
            else                            // jest RCP
            {

                Worktime1 wt = new Worktime1();
                wt.Prepare(con, pracId, rcpId, strefaId, fromTime, toTime, nocneOdSec, nocneDoSec, "");
                ds = Base.getDataSet(con, String.Format(
                    "select D.Data, D.TimeIn, D.TimeOut, " +
                        "D.worktime as Czas1sec, " +                        // jako sec
                        "dbo.ToTime(D.worktime) as Czas1, " +               // jako HH:MM:SS
                    //"dbo.RoundSec(D.worktime,{4},{5}) as Czas1Rsec, " +    // zaokr sec
                            "dbo.RoundTime(D.worktime,{4},{5}) as Czas1R, " +      // zaokr HH:MM

                        "D.worktime2 as Czas2sec, " +
                        "dbo.ToTime(D.worktime2) as Czas2, " +
                    //"dbo.RoundSec(D.worktime2,{4},{5}) as Czas2Rsec, " +
                            "dbo.RoundTime(D.worktime2,{4},{5}) as Czas2R, " +

                        "D.nighttime as Nocne1sec, " +
                    //"dbo.ToTime(D.nighttime) as Nocne1, " +
                    //"dbo.RoundSec(D.nighttime, {4},{5}) as Nocne1Rsec, " +
                    //"dbo.RoundTime(D.nighttime, {4},{5}) as Nocne1R, " +

                        "D.nighttime2 as Nocne2sec, " +
                    //"dbo.ToTime(D.nighttime2) as Nocne2, " +
                    //"dbo.RoundSec(D.nighttime2, {4},{5}) as Nocne2Rsec, " +
                    //"dbo.RoundTime(D.nighttime2, {4},{5}) as Nocne2R, " +

                        "D.ponocy, D.ponocy2, " +

                        "P.Id as PPId, " +
                        "P.IdZmiany, " +
                        "P.IdZmianyKorekta, " +
                        "P.Czas," +
                        "P.CzasIn, P.CzasOut, P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne, " +
                        "P.k_CzasIn, P.k_CzasOut, P.k_CzasZm, P.k_NadgodzinyDzien, P.k_NadgodzinyNoc, P.k_Nocne, " +  // 1 jak kierownik ustawił, 0 jak wpisane po zamknięciu miesiąca
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
                        "AK.GodzinPracy, AK.DniWolne " +
                    "from tmpRCP3 D " +
                        "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
                        "left outer join Zmiany Z on Z.Id = ISNULL(P.IdZmianyKorekta, P.IdZmiany) " +
                        "left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                        "left outer join AbsencjaKody AK on AK.Kod = ISNULL(A.Kod, P.Absencja) " +
                    "where D.sesId = '{3}' and D.Data between '{1}' and '{2}' " +
                    "order by D.Data",
                        pracId, dFrom, dTo,
                        wt.FSesId,
                        zaokr, zaokrType));
                wt.Unload();
            }
            return ds;
        }

        //------------------------------------------------------
        public string SesId
        {
            get { return FSesId; }
        }
    }
}






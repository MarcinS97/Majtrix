using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp.App_Code
{
    public class Worktime_1
    {
        const int margin = 5; // margines dni pobierania danych; pomyslec nad znalezieniem 1 przed i 1 po bo to wystarczy

        SqlConnection con;
        bool fDoDisconnect;
        string FPracId;
        string FRcpId;
        string FStrefaId;
        string FDateFrom;
        string FDateTo;
        string FSesId;
        string FSqlSesId;  // w '' 

        public Worktime_1(SqlConnection acon, // jak null to łączy i rozłącza, jak !null to korzysta z przekazanego
                        string pracId,      // musi być
                        string rcpId, 
                        string strefaId,    // jak null to bierze z historii zmian stref pracownika, a jak nie ma to działu, na razie nie ma obsługi
                        string dateFrom, string dateTo, 
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

            GetReadersData();  // -> tmpRCP1
            PrepareWorkime3(); // -> tmpRCP2
            GetWorkime3();     // -> tmpRCP3
        }
        //-------------------------------------------------------------
        private void GetReadersData() // -> tmpRCP1
        {
            DateTime dtFrom = DateTime.Parse(FDateFrom).AddDays(-margin);    // zeby poprawnie złapac początek, mozna dać więcej, dtFrom >=, dtTo <
            DateTime dtTo = DateTime.Parse(FDateTo).AddDays(1 + margin);     // zeby poprawnie złapać koniec

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
                    if (i > 0) s += " union ";
                    s += "select " + FSqlSesId + " as sesId," +
                            "*," +
                            "case " +
                                "when ECReaderId in (" + rio + ") then convert(bit, 0) " +
                                "else convert(bit, 1) " +
                            "end as InOut2 " +
                        "from RCP where " + czas +
                            " and ECReaderId in (" + r + ")" +
                            " and ECUserId = " + FRcpId +
                        //" order by ECUniqueId";
                        " order by Czas";
                }
            }
            if (!String.IsNullOrEmpty(s))
                Base.execSQL(con, "insert into tmpRCP1 " + s);
        }























        /*
        public Worktime(string pracId, string rcpId, string strefaId, string dateFrom, string dateTo, string alg, string id)
        {
            con = Base.Connect();

            FSesId = id + Base.UniqueId(50 - id.Length);
            //FSesId = id + "zzz";
            FSqlSesId = Base.strParam(FSesId);
            //DeleteTmpData(); 

            FPracId = pracId;
            if (String.IsNullOrEmpty(rcpId))
                FRcpId = Base.getScalar(con, "select RcpId from Pracownicy where Id = " + pracId);
            else FRcpId = rcpId;

            if (rcpId != "-1")
            {
                if (String.IsNullOrEmpty(strefaId))
                    FStrefaId = "1";    //domyślna ?
                else FStrefaId = strefaId;
                FDateFrom = dateFrom;
                FDateTo = dateTo;

                GetReadersData(); // -> tmpRCP1
                switch (alg)
                {
                    case "1":
                        PrepareWorkime1(); // -> tmpRCP2
                        GetWorkime1();     // -> tmpRCP3
                        break;
                    case "2":
                        PrepareWorkime2(); // -> tmpRCP2
                        GetWorkime2();     // -> tmpRCP3
                        break;
                    default:
                    case "3":
                        PrepareWorkime3(); // -> tmpRCP2
                        GetWorkime3();     // -> tmpRCP3
                        break;
                }
            }
        }

         */


        public void DeleteTmpData()
        {
            Base.execSQL(con, "delete from tmpRCP1 where sesId = " + FSqlSesId);
            Base.execSQL(con, "delete from tmpRCP2 where sesId = " + FSqlSesId);
            Base.execSQL(con, "delete from tmpRCP3 where sesId = " + FSqlSesId);
        }

        public void Unload()
        {
            DeleteTmpData();
            if (fDoDisconnect) Base.Disconnect(con);
        }

        //---------------
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

        public DataSet GetDetails(string dateFrom, string dateTo, string round, string rType) // czasy składowe in-out
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
                    "dbo.RoundTime(worktime, " + round + ") as CzasR," +
                    "worktime " +
                "from tmpRCP2 " +
                "where sesId = " + FSqlSesId + " and " + czas);
        }

        public DataSet GetDetails2(string dateFrom, string dateTo)   // dane z rejestratorów w strefie
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
                "where sesId = " + FSqlSesId + " and " + czas);
        }

        public DataSet GetDetails3(string dateFrom, string dateTo)   // dane ze wszystkich rejestratorów 
        {
            string czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas < " + Base.strParam(dateTo);
            /*
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (!fromIsNull && !toIsNull)
                czas = "A.Czas >= DATEADD(DAY, -1, " + Base.strParam(dateFrom) + ") and A.Czas <= DATEADD(DAY, 1, " + Base.strParam(dateTo) + ")";
            else if (toIsNull)
                czas = "A.Czas >= DATEADD(DAY, -1, " + Base.strParam(dateFrom) + ") and A.Czas <= DATEADD(DAY, 2, " + Base.strParam(dateFrom) + ")";
            else if (fromIsNull)
                czas = "A.Czas >= DATEADD(DAY, -2, " + Base.strParam(dateTo) + ") and A.Czas <= DATEADD(DAY, -1, " + Base.strParam(dateTo) + ")";
            else
                return null;
             */ 
            return Base.getDataSet(con,
                "select A.Czas, A.ECReaderId, B.InOut2, R.Name, A.InOut " +
                "from RCP A " +
                "left outer join tmpRCP1 B on sesId = " + FSqlSesId + " and B.ECUniqueId = A.ECUniqueId " +
                "left outer join Readers R on R.Id = A.ECReaderId " +
                "where A.ECUserId = " + FRcpId + " and " + czas);
        }

        public void SumTime(out string sum, out string sumR, out string sum2, out string sum2R, string sourceRound, string resultRound, string srType)
        {
            DataRow dr = Base.getDataRow(con, 
                "select dbo.ToTime(sum(dbo.RoundSec(worktime," + sourceRound + "))) as Suma," +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime," + sourceRound + "))," + resultRound+ ") as SumaR," +
                       "dbo.ToTime(sum(dbo.RoundSec(worktime2," + sourceRound + "))) as Suma2, " +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime2," + sourceRound + "))," + resultRound + ") as Suma2R " +
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
        //-------------------
        //---------------------------------------------------
        // zakładam istnienie wejscia w danym dniu
        // jezeli brak wyjsc dla kolejnych to wszystkie dostają to samo wyjscie pierwsze znalezione, tylko dla ostatniego in jest to prawda
        private void PrepareWorkime1() // -> tmpRCP2
        {
            Base.execSQL(con,
                "insert into tmpRCP2 " +
                "select " + FSqlSesId + " as sesId," +
                    "A.ECUniqueId, A.ECUserID, A.ECReaderID," +
                    "X.Czas as TimeBefore, A.Czas as TimeIn, B.Czas as TimeOut, C.Czas as TimeAfter," +
                    "datediff(second,'1900-01-01',B.Czas - A.Czas) as worktime," +
                    "datediff(second,'1900-01-01',A.Czas - X.Czas) as beforetime," +
                    "datediff(second,'1900-01-01',C.Czas - B.Czas) as aftertime " +
                "from tmpRCP1 A " +
                    "left outer join tmpRCP1 X on X.sesId = " + FSqlSesId + " and X.ECUniqueId in (select top 1 X1.ECUniqueId from tmpRCP1 X1 where X1.sesId = " + FSqlSesId + " and X1.Czas < A.Czas and X1.InOut2 = 1 order by X1.Czas desc) " +//X1.ECUniqueId desc) " +
                    "left outer join tmpRCP1 B on B.sesId = " + FSqlSesId + " and B.ECUniqueId in (select top 1 X2.ECUniqueId from tmpRCP1 X2 where X2.sesId = " + FSqlSesId + " and X2.Czas > A.Czas and X2.InOut2 = 1 order by X2.Czas asc) " + //X2.ECUniqueId asc) " +
                    "left outer join tmpRCP1 C on C.sesId = " + FSqlSesId + " and C.ECUniqueId in (select top 1 X3.ECUniqueId from tmpRCP1 X3 where X3.sesId = " + FSqlSesId + " and X3.Czas > B.Czas and X3.InOut2 = 0 order by X3.Czas asc) " + //X3.ECUniqueId asc) " +
                "where A.sesId = " + FSqlSesId + " and A.InOut2 = 0 and A.ECUserID = " + FRcpId);
        }

        private void GetWorkime1() // -> tmpRCP3
        {
            string dOd = Base.strParam(FDateFrom);
            string dDo = Base.strParam(FDateTo);
            Base.execSQL(con,
                "insert into tmpRCP3 " +
                "select " + FSqlSesId + " as sesId," +
                    "D.Data,A.TimeIn,B.TimeOut," +
                    "datediff(second,'1900-01-01',B.TimeOut - A.TimeIn) as worktime," +
                    "(select sum(Z.worktime) from tmpRCP2 Z where Z.sesId = " + FSqlSesId + " and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.TimeOut) as worktime2," +
                    "A.beforetime, B.aftertime " +
                "from GetDates2(" + dOd + "," + dDo + ") D " +
                    "left outer join tmpRCP2 A on A.sesId = " + FSqlSesId + " and A.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X1 where X1.sesId = " + FSqlSesId + " and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data) order by timebefore desc) " +
                    "left outer join tmpRCP2 B on B.sesId = " + FSqlSesId + " and B.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X2 where X2.sesId = " + FSqlSesId + " and X2.TimeOut > A.TimeIn order by X2.TimeOut asc,timeafter desc)");
        }
        //---------------------------------------------------
        // zakładam istnienie wejscia w danym dniu
        private void PrepareWorkime2() // -> tmpRCP2
        {
            Base.execSQL(con, "insert into tmpRCP2 " +
                "select " + FSqlSesId + " as sesId," +
                    "A.ECUniqueId, A.ECUserID, A.ECReaderID," +
                    "case X.InOut2 when 1 then X.Czas else null end as TimeBefore," +
                    "A.Czas as TimeIn," +
                    "case B.InOut2 when 1 then B.Czas else null end as TimeOut," +
                    "C.Czas as TimeAfter," +

                    "case B.InOut2 " +
                        "when 1 then datediff(second,'1900-01-01',B.Czas - A.Czas) " +
                        "else null " +
                    "end as worktime," +
                    "case X.InOut2 " +
                        "when 1 then datediff(second,'1900-01-01',A.Czas - X.Czas) " +
                        "else 86400 " +
                    "end as beforetime, " +
                    "datediff(second,'1900-01-01',C.Czas - B.Czas) as aftertime " +

                "from tmpRCP1 A " +
                    "left outer join tmpRCP1 X on X.sesId = " + FSqlSesId + " and X.ECUniqueId in (select top 1 X1.ECUniqueId from tmpRCP1 X1 where X1.sesId = " + FSqlSesId + " and X1.Czas < A.Czas order by X1.ECUniqueId desc) " +
                    "left outer join tmpRCP1 B on B.sesId = " + FSqlSesId + " and B.ECUniqueId in (select top 1 X2.ECUniqueId from tmpRCP1 X2 where X2.sesId = " + FSqlSesId + " and X2.Czas > A.Czas order by X2.ECUniqueId asc) " +
                    "left outer join tmpRCP1 C on C.sesId = " + FSqlSesId + " and C.ECUniqueId in (select top 1 X3.ECUniqueId from tmpRCP1 X3 where X3.sesId = " + FSqlSesId + " and X3.Czas > A.Czas and X3.InOut2 = 0 order by X3.ECUniqueId asc) " +
                "where A.sesId = " + FSqlSesId + " and A.InOut2 = 0 and " +
                    "A.ECUserID = " + FRcpId);
        }

        private void GetWorkime2() // -> tmpRCP3
        {
            string dOd = Base.strParam(FDateFrom);
            string dDo = Base.strParam(FDateTo);
            Base.execSQL(con, "insert into tmpRCP3 " +
                "select " + FSqlSesId + " as sesId," +
                    "D.Data, A.TimeIn, B.TimeOut," +
                    "datediff(second,'1900-01-01',B.TimeOut - A.TimeIn) as worktime," +
                    "(select sum(Z.worktime) from tmpRCP2 Z where sesId = " + FSqlSesId + " and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.TimeOut) as worktime2," +
                    "A.beforetime, B.aftertime " +
                "from GetDates2(" + dOd + "," + dDo + ") D " +
                    "left outer join tmpRCP2 A on A.sesId = " + FSqlSesId + " and A.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X1 where X1.sesId = " + FSqlSesId + " and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data) order by timebefore desc) " +
                    "left outer join tmpRCP2 C on C.sesId = " + FSqlSesId + " and C.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X3 where X3.sesId = " + FSqlSesId + " and X3.TimeIn >= DATEADD(DAY, 1, D.Data) and X3.TimeIn < DATEADD(DAY, 2, D.Data) order by timebefore desc) " +
                    "left outer join tmpRCP2 B on B.sesId = " + FSqlSesId + " and B.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X2 where X2.sesId = " + FSqlSesId + " and X2.TimeOut > A.TimeIn and X2.TimeOut < C.TimeIn order by X2.TimeOut asc,timeafter desc)");
        }

        //---------------------------------------------------
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
                        //"order by X2.ECUniqueId desc) " +
                        "order by X2.Czas desc) " +
                        "and B.InOut2 = 1");
        
                /*    
                    pierwszy in w dniu z max beforetime A
                    pierwszy w kolejnym dniu z max beforetime (moze być po przerwie !!!) C
                    ostatni out przed C
                 
                  
                select 
                    D.Data, A.TimeIn, 
                    --C.TimeIn as TimeInC,
                    B.Czas,
                    datediff(second,'1900-01-01',B.Czas - A.TimeIn) as worktime,
                    (select sum(Z.worktime) from tmpRCP2 Z where sesId = '5zzz' and Z.TimeIn >= A.TimeIn and Z.TimeOut <= B.Czas) as worktime2,
                    A.beforetime, null as aftertime 
                from GetDates2('2011-12-01','2012-01-10') D 
                    left outer join tmpRCP2 A on A.sesId = '5zzz' and A.ECUniqueId = 
	                   (select top 1 X1.ECUniqueId from tmpRCP2 X1 
		                where X1.sesId = '5zzz' 
			                and X1.TimeIn >= D.Data and X1.TimeIn < DATEADD(DAY, 1, D.Data) 
		                order by X1.beforetime desc) 
                    left outer join tmpRCP2 C on C.sesId = '5zzz' and C.ECUniqueId = 
	                   (select top 1 X3.ECUniqueId from tmpRCP2 X3 
		                where X3.sesId = '5zzz' 
			                and X3.TimeIn >= DATEADD(DAY, 1, D.Data) 
		                order by LEFT(CONVERT(varchar,X3.TimeIn,20),10), X3.beforetime desc) 
                    left outer join tmpRCP1 B on B.sesId = '5zzz' and B.ECUniqueId = 
                       (select top 1 X2.ECUniqueId from tmpRCP1 X2 
                        where X2.sesId = '5zzz' and X2.Czas > A.TimeIn and X2.Czas < C.TimeIn and X2.Czas < DATEADD(DAY, 1, A.TimeIn) 
                        order by X2.ECUniqueId desc)
		                and B.InOut2 = 1
                */
        
        
        }
        //---------------------------------------------------
        /*
        private void SelectReaders(SqlConnection con, string dateFrom, string dateTo, string strefaId, string rcpId)
        {
            DateTime dtFrom = DateTime.Parse(dateFrom);
            DateTime dtTo = DateTime.Parse(dateTo);
            DataSet ds = Base.getDataSet(con,
                "select * from StrefyReaders where IdStrefy = " + strefaId + " and Readers <> '' order by DataOd");

            string s = null;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                string czas = null;
                string rr = Base.getValue(dr, "Readers");
                string sr = rr.Replace("-", "");
                s = "select *," +
                   " case " +
                   "    when ECReaderId in (" + rr + ") then convert(bit, 0)" +
                   "    else convert(bit, 1)" +
                   " end as InOut2" +
                   " into tmpRCP" +
                   " from RCP where " + czas +
                   "    ECReaderId in (" + sr + ")" +
                   " and ECUserId = " + rcpId +
                   " order by ECUniqueId";
                break;  // poki co ...
            }
            Base.execSQL2(con, "drop table tmpRCP");
            Base.execSQL(con, s);
        }

        private void PrepareWorkTime(SqlConnection con, string rcpId)
        {
            Base.execSQL2(con, "drop table tmpRCP2");
            Base.execSQL(con,
                "select A.ECUniqueId, A.ECUserID, A.ECReaderID, A.Czas," +
                    "datediff(second,'1900-01-01',B.Czas - A.Czas) as cp," +
                    "datediff(second,'1900-01-01',A.Czas - X.Czas) as pprzed," +
                    "datediff(second,'1900-01-01',C.Czas - B.Czas) as ppo," +

                    "convert(varchar,DATEDIFF(SECOND, A.Czas, B.Czas)/86400) + ' ' + convert(varchar, B.Czas - A.Czas, 8) as CzasPracy," +
                    "convert(varchar,DATEDIFF(SECOND, X.Czas, A.Czas)/86400) + ' ' + convert(varchar, A.Czas - X.Czas, 8) as PrzerwaPrzed," +
                    "convert(varchar,DATEDIFF(SECOND, B.Czas, C.czas)/86400) + ' ' + convert(varchar, C.Czas - B.Czas, 8) as PrzerwaPo," +
                    "X.Czas as CzasPrzed, B.Czas as CzasKoniec, C.Czas as CzasPo " +
                "into tmpRCP2 " +
                "from tmpRCP A " +
                "left outer join tmpRCP X on X.ECUniqueId in (select top 1 ECUniqueId from tmpRCP X1 where X1.Czas < A.Czas and X1.InOut2 = 1 order by ECUniqueId desc) " +
                "left outer join tmpRCP B on B.ECUniqueId in (select top 1 ECUniqueId from tmpRCP X2 where X2.Czas > A.Czas and X2.InOut2 = 1 order by ECUniqueId asc) " +
                "left outer join tmpRCP C on C.ECUniqueId in (select top 1 ECUniqueId from tmpRCP X3 where X3.Czas > B.Czas and X3.InOut2 = 0 order by ECUniqueId asc) " +
                "where A.InOut2 = 0 and A.ECUserID = " + rcpId);
        }

        public void Prepare()
        {
            if (!String.IsNullOrEmpty(RcpId))
            {
                SqlConnection con = Base.Connect();
                lbStrefa.Text = Base.getScalar(con, "select Nazwa from Strefy where Id = " + StrefaId);
                Clean(con, RcpId);
                SelectReaders(con, "2011-10-01", "2012-01-10", FStrefaId, RcpId);
                PrepareWorkTime(con, RcpId);
                Base.Disconnect(con);
            }
        }

        */



        /*
        "select D.Data, 
            convert(varchar,DATEDIFF(SECOND, A.Czas, B.CzasKoniec)/86400) + ' ' + convert(varchar, B.CzasKoniec - A.Czas, 8) as CzasPracy
            null as CzasPracy2,
            A.Czas as TimeIn, 
            B.CzasKoniec as TimeOut
        from GetDates('@hidDateFrom','@hidDateTo') D
        left outer join tmpRCP2 A on A.sesId = @sesId and A.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X1 where X1.sesId = @sesId and X1.Czas >= D.Data and X1.Czas < DATEADD(DAY, 1, D.Data) order by pprzed desc)
        left outer join tmpRCP2 B on B.sesId = @sesId and B.ECUniqueId = (select top 1 ECUniqueId from tmpRCP2 X2 where X2.sesId = @sesId and X2.CzasKoniec > A.Czas order by X2.CzasKoniec asc,ppo desc)"
        */

        public string SesId
        {
            get { return FSesId; }
        }

        //-------------------------------------------------------------
        public const int aNoAlg         = 0x00001000;   // nie okreslony algorytm naliczania czasu
        public const int aNoNominal     = 0x00000100;   // nie osiągnięto czasu nominalnego
        public const int aNoShiftWT     = 0x00000200;   // nie okreslony parametr ilosci czasu przy zmianie "na obecność"
        
        public static TimeSpan CountTime2(DateTime czasOd, DateTime czasDo)
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

        public static int CountTime(DateTime czasOd, DateTime czasDo)
        {
            int dt1 = Convert.ToInt32(czasOd.TimeOfDay.TotalSeconds);  // róznica nie moze przekroczyć doby !!!
            int dt2 = Convert.ToInt32(czasDo.TimeOfDay.TotalSeconds);
            if (dt2 > dt1)
                return dt2 - dt1;
            else
                return dt2 + 86400 - dt1;
        }

        public static DataSet GetWorktime(SqlConnection con, string pracId, string rcpId, 
                    string fromTime, string toTime, string onDay, 
                    string strefaId, string alg, string zaokr, string zaokrType)  // alg to nie jest alg rcp tylko metoda wyznaczania !!!
        {
            DataSet ds;
            string dFrom = String.IsNullOrEmpty(onDay) ? fromTime : onDay;
            string dTo = String.IsNullOrEmpty(onDay) ? toTime : onDay;
            if (String.IsNullOrEmpty(rcpId))  // jak moZmiany, bo nie jest rejestrowany
                ds = Base.getDataSet(con, String.Format(
                    "select D.Lp, D.Data, null as TimeIn, null as TimeOut, " +
                        "null as Czas1sec, " +
                        "null as Czas1, " +
                        "null as Czas1Rsec, " +
                        "null as Czas1R, " +
                        "null as Czas2sec, " +
                        "null as Czas2, " +
                        "null as Czas2Rsec, " +
                        "null as Czas2R, " +

                        "P.IdZmiany, " +
                        "Z.Id as ZmianaId, " +
                        "Z.Symbol, " +
                        //"Z.Nazwa as Zmiana, " +
                        "Z.Kolor, " +
                        "Z.Od, " +
                        "Z.Do, " +
                        "A.Id as Absencja, " +
                        "C.Id as Akceptacja " +
                    "from GetDates2('{1}','{2}') D " +
                        "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
                        "left outer join Zmiany Z on Z.Id = P.IdZmiany " +
                        "left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                        "left outer join Akceptacja C on C.IdPRacownika = {0} and C.Data = D.Data " +
                    "order by D.Data",
                        pracId, dFrom, dTo));
            else
            {
                Worktime_1 wt = new Worktime_1(con, null, rcpId, strefaId, fromTime, toTime, "");
                ds = Base.getDataSet(con, String.Format(
                    "select D.Data, D.TimeIn, D.TimeOut, " +
                        "D.worktime as Czas1sec, " +
                        "dbo.ToTime(D.worktime) as Czas1, " +
                        "dbo.RoundSec(D.worktime, {4}) as Czas1Rsec, " +
                        "dbo.RoundTime(D.worktime, {4}) as Czas1R, " +
                        "D.worktime2 as Czas2sec, " +
                        "dbo.ToTime(D.worktime2) as Czas2, " +
                        "dbo.RoundSec(D.worktime2, {4}) as Czas2Rsec, " +
                        "dbo.RoundTime(D.worktime2, {4}) as Czas2R, " +

                        "P.IdZmiany, " +
                        "Z.Id as ZmianaId, " +
                        "Z.Symbol, " +
                        //"Z.Nazwa as Zmiana, " +
                        "Z.Kolor, " +
                        "Z.Od, " +
                        "Z.Do, " +
                        "A.Id as Absencja, " +
                        "C.Id as Akceptacja " +
                    "from tmpRCP3 D " +
                        "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
                        "left outer join Zmiany Z on Z.Id = P.IdZmiany " +
                        "left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                        "left outer join Akceptacja C on C.IdPRacownika = {0} and C.Data = D.Data " +

                    "where D.sesId = '{3}' and D.Data between '{1}' and '{2}' " +
                    "order by D.Data",
                        pracId, dFrom, dTo,
                        wt.SesId,
                        zaokr));
                wt.Unload();
            }
            return ds;
        }

        public static bool SolveWorktime(SqlConnection con,
                                        string algRCP, string algPar, 
                                        object zmOd, object zmDo, 
                                        int breakTimeMin,    // czas przerwy
                                        int timeMarginMin,   // margines poniżej którego generuje alert ze nie osiągnięto czasu nominalnego 
                                        object timeIn, object timeOut, object czas1, object czas2, 
                                        string zaokr, string zaokrType,
                                        out string wtime, 
                                        out string ntime, // nominal time - wg zmiany jesli przepracowany
                                        out string otime, // overtime
                                        ref int wtAlert)  // true jesli jest zarejestrowany        
        {
            object czas;
            switch (algRCP)
            {
                case "1":   //ALG 10	We-Wy	                    1	NULL
                    czas = czas1;
                    break;
                case "2":   //ALG 30	Suma w strefie	            2	NULL
                    czas = czas2;
                    break;
                case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                    czas = czas1;
                    break;
                case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                    czas = czas2;
                    break;
                case "3":   //ALG 50	Obecność 8h	                3	8
                    if (!timeIn.Equals(DBNull.Value) || !timeOut.Equals(DBNull.Value))  // wystarczy ze system zarejestrował wejście albo wyjście - nie wiem jak to sie bedzie miec do pracy na 3 zmiane, zakladam ze nie ma cos takiego miejsca ...
                    {
                        int c;
                        if (Int32.TryParse(algPar, out c))
                            czas = c * 3600;  // czas jest w sek, a algPar w h
                        else
                        {
                            wtAlert |= aNoShiftWT;
                            czas = 0; //DBNull.Value
                        }
                    }
                    else czas = DBNull.Value;  // nie było w pracy
                    break;
                default:
                    czas = czas1;
                    wtAlert |= aNoAlg; // nie ustalony algorytm naliczania czasu 
                    break;
            }
            if (!czas.Equals(DBNull.Value))
            {
                int wt = (int)czas;  //sek
                int wt0 = wt;
                if (!zmOd.Equals(DBNull.Value) && !zmDo.Equals(DBNull.Value))  // jest zmiana !
                {
                    int zt = CountTime((DateTime)zmOd, (DateTime)zmDo);  
                    int ztBreak = zt - breakTimeMin * 60;          // czas nominalny z uwzględnieniem przerwy
                    int ztMargin = ztBreak - timeMarginMin * 60;
                    switch (algRCP)
                    {
                        case "1":   //ALG 10	We-Wy	                    1	NULL
                        case "2":   //ALG 30	Suma w strefie	            2	NULL
                            if (wt < ztMargin) wtAlert |= aNoNominal;
                            if (wt > ztBreak) wt = zt;      // czas wynikający ze zmiany                
                            wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{1})", wt, zaokr));
                            ntime = wtime;
                            otime = null;
                            break;
                        case "11":  //ALG 20	We-Wy + nadgodziny	        11	NULL
                        case "12":  //ALG 40	Suma w strefia + nadgodziny	12	NULL
                            if (wt < ztMargin) wtAlert |= aNoNominal;
                            if (wt < ztBreak)
                            {
                                wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{1})", wt, zaokr));
                                ntime = wtime;
                                otime = null;
                            }
                            else
                            {
                                DataRow dr = Base.getDataRow(con, String.Format("select dbo.RoundTime({0},{2}), dbo.RoundSec({1},{2}), dbo.RoundTime({1},{2})", zt, wt - ztBreak, zaokr));
                                if (dr[1].ToString() == "0")
                                {
                                    wtime = dr[0].ToString();
                                    ntime = wtime;
                                    otime = null;
                                }
                                else
                                {
                                    wtime = dr[0].ToString() + "+" + dr[2].ToString();
                                    //wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{2}) + '+' + dbo.RoundTime({1},{2})", zt, wt - ztBreak, zaokr));
                                    ntime = dr[0].ToString();
                                    otime = dr[2].ToString();
                                }
                            }
                            break;
                        case "3":   //ALG 50	Obecność 8h	                3	8
                            if (!czas1.Equals(DBNull.Value)) wt0 = (int)czas1;
                            wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{1})", wt, zaokr)); // wt to algPar
                            ntime = wtime;
                            otime = null;
                            break;
                        default:    // nie sposób określic to czas łączny, błąd juz ustawiony
                            wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{1})", wt.ToString(), zaokr));
                            ntime = wtime;
                            otime = null;
                            break;
                    }

                    string s = TimeSpan.FromSeconds(wt0).ToString().Substring(0,5);
                    if (s.StartsWith("0")) s = s.Substring(1);
                    wtime += "<br />" + s;  // timespan format dziala od .net 4
                }
                else  // nie ma zmiany lub błędne czasy to czas łączny, o błędzie nie informuje, malo prawdopodobne
                {
                    wtime = Base.getScalar(con, String.Format("select dbo.RoundTime({0},{1})", wt.ToString(), zaokr));
                    ntime = wtime;
                    otime = null;
                }
                return true;
            }
            else
            {
                wtime = null;
                ntime = null;
                otime = null;
                return false;
            }

            /*


            string alg = drv["Algorytm"].ToString();
            try
            {
                DateTime zmOd = (DateTime)dr["Od"];
                DateTime zmDo = (DateTime)dr["Do"];
                TimeSpan zmCzas = App.CzasZmiany(zmOd, zmDo);
            }
            catch (Exception ex)
            {
                Log.Error(Log.t2APP, "Błąd odczytu dat zmiany", dr["IdZmiany"].ToString());
                wtime = "ERR";
            }


                        
            */


        }
    }
}






/* w AppPropertiesach 
#define WT2   // nowy algorytm liczenia czasu - stary alg
#define WT2a  // nowy algorytm liczenia czasu zm i nadg i nocnych - nowy alg (z sumatorem parowym)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;
using HRRcp.Controls;

/*
 * poprawki do zrobienia powyżej
1. line 685 - Worktime.alCheckInOut - alert ignorować dla dni, w których jest algorytm we/wy - brak we/wy w czasie pracy - tylko dla sumy w strefie








*/

namespace HRRcp.App_Code
{
    public class Worktime2
    {
        const int margin = 1; // margines dni pobierania danych

        //SqlConnection con;
        //bool fDoDisconnect;

        public const string algWeWy = "1";
        public const string algWeWyNadg = "11";
        public const string algSuma = "2";      // w strefie	2
        public const string algSumaNadg = "12";     // w strefie + nadgodziny	
        public const string algObecnosc8h = "3";
        public const string algBezLiczenia = "0";

        string FPracId;
        string FDateFrom;
        string FDateTo;

        int FNocFromSec;
        int FNocToSec;   // zawsze > FNocFromSec, dodaję 86400

        string _FRcpId;
        string _FStrefaId;
        string _FalgRCP;
        string _FalgPar;

        public DataSet Data;
        //Ustawienia settings;

        public void Prepare(
                        DataSet dsDays,         // wynik działania GetWorktime - kolejne dni z info o zmianie, moze być null wtedy tylko wyliczam czas z rcp dla dni które są!
                        string pracId,          // musi być

                        string x_algRCP, string x_algPar,   // mogą być null np w RcpControl - nie jest istotne tam liczenie czasu
                        string x_rcpId,
                        string x_strefaId,        // jak null to bierze z historii zmian stref pracownika, a jak nie ma to działu, na razie nie ma obsługi

                        string dateFrom, string dateTo,
                        int nocFromSec, int nocToSec,       // jak 0 to nie liczy
                        int breakTimeZm, int breakTimeN,    // jak dsDays = null to moze tu być dowolna wartość; [min], tak jak w konfiguracji; później moze dać do danych jak będzie pod innym kierownikiem, ale to nie stanowi na tym etapie
                        int zaokr, int zaokrType)           // j.w.
        {
#if NOMWYM
#if !SIEMENS
        //error
#endif
#endif
            FPracId = pracId;

            _FalgRCP = x_algRCP;
            _FalgPar = x_algPar;
            _FRcpId = x_rcpId;
            _FStrefaId = x_strefaId;

            FDateFrom = dateFrom;
            FDateTo = dateTo;

            FNocFromSec = nocFromSec;
            FNocToSec = nocToSec;
            if (FNocFromSec > FNocToSec)
                FNocToSec += 86400;         // zakładam 1 dobę !!!



            /*
            if (pracId == "138")
            {
                int x = 0;
            }
            */


            Data = _GetReadersData();    // zbieram zakres danych -> Tables[0]
            if (Data != null)
            {
                PrepareWorkime5a();     // klasyfikuje do kolejnych dni i zbiera pary IN/OUT -> Tables[1]
                PrepareWorkime5b();     // grupuje pary -> Tables[2]
                if (dsDays != null)
                    _CountWT2AndMerge(dsDays, breakTimeZm, breakTimeN, zaokr, zaokrType);
            }
        }

        //----------------------------------------------------------------------------
        string LastSql_v1 = null;
        string _LastSql_v2 = null;

        private DataSet GetReadersData_v1()
        {
            DateTime dtFrom = DateTime.Parse(FDateFrom).AddDays(-margin);   // zeby poprawnie złapac początek, mozna dać więcej, dtFrom >=, dtTo <
            DateTime dtTo = DateTime.Parse(FDateTo).AddDays(margin + 1);    // zeby poprawnie złapać koniec, data jest 00 a czas > 00 dlatego jeszcze + 1

            // !!! dołożyć kręcenie się po zmianach strefy i algorytmu i RcpId !!!

            if (!String.IsNullOrEmpty(_FStrefaId))   // nie mam strefy - nie mam po czym zebrać danych
            {
                DataSet ds = db.getDataSet("select * from StrefyReaders where IdStrefy = " + _FStrefaId + " order by DataOd");  //and Readers <> ''  wyrzucam zeby mozna bylo wylaczyc cała strefę

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
                    else                     // 
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

                        s += "select ECuniqueId, Czas, ECUserId, ECReaderId, " +
                                "case " +
                                    "when ECReaderId in (" + rio + ") then convert(bit, 0) " +
                                    "else convert(bit, 1) " +
                                "end as InOut2 " +
                            "from RCP where " + czas +
                                " and ECReaderId in (" + r + ")" +
                                " and ECUserId = " + _FRcpId;        // też może byc in () !!! bo nie bedzie w tym samym czasie 2 używać - więc w konfiguracji pracownika wystarczy zmienić pole na string i zapisywać z , !!! <<< ale problem będzie jak różne osoby w tym samym okresie dostaną ten sam nr rcpid (np. tymczasowe)
                    }
                }



                LastSql_v1 = s;



                if (!String.IsNullOrEmpty(s))
                    return db.getDataSet(s + " order by Czas");   // order nie moze byc w union
            }
            return null;
        }


        private DataSet GetReadersData_v2()
        {
            DateTime dtFrom = DateTime.Parse(FDateFrom).AddDays(-margin);   // zeby poprawnie złapac początek, mozna dać więcej, dtFrom >=, dtTo <
            DateTime dtTo = DateTime.Parse(FDateTo).AddDays(margin);        // zeby poprawnie złapać koniec, data jest 00 a czas > 00 dlatego jeszcze + 1 <<<< nie jest to potrzebne ..., stary algorytm łapie 1 dzień więcej


            string sql = String.Format(@"
declare @dataOd datetime 
declare @dataDo datetime 
declare @rcpid int
declare @pracid int
set @dataOd = '{0}'
set @dataDo = '{1}'
set @rcpid = {2}
set @pracid = {3}

select 
	--dbo.MaxDate3(@dataOd, R.Od, S.DataOd) as Od,
	--dbo.MinDate3(@dataDo, R.Do, S.DataDo) as Do,

	RCP.ECuniqueId, RCP.Czas, RCP.ECUserId, RCP.ECReaderId, 
	case when RCP.ECReaderId in (select items from dbo.SplitInt(S.Readers, ',') where items > 0)
	then convert(bit, 0) 
	else convert(bit, 1) 
	end as InOut2

from Przypisania R 
inner join VStrefyReaders S on S.IdStrefy = R.RcpStrefaId 
	and S.DataOd < DATEADD(DAY, 1, ISNULL(R.Do, '20990909')) 
	and ISNULL(S.DataDo, '20990909') >= R.Od

inner join RCP on RCP.ECUserId=@rcpid 
	and RCP.Czas >= dbo.MaxDate3(@dataOd, R.Od, S.DataOd)
	and RCP.Czas < dbo.MinDate3(DATEADD(DAY, 1, @dataDo), DATEADD(DAY, 1, R.Do), S.DataDo) 
	and RCP.ECReaderId in (select abs(items) from dbo.SplitInt(S.Readers, ','))

where R.IdPracownika = @pracid and R.Od <= @dataDo and ISNULL(R.Do, '20990909') >= @dataOd and R.Status = 1
order by RCP.Czas
                ", Tools.DateToStr(dtFrom), Tools.DateToStr(dtTo), _FRcpId, FPracId);


            //_LastSql_v2 = sql; //testy


            return db.getDataSet(sql);
        }

























        private DataSet _GetReadersData_v3()
        {
            DateTime dtFrom = DateTime.Parse(FDateFrom).AddDays(-margin);   // zeby poprawnie złapac początek, mozna dać więcej, dtFrom >=, dtTo <
            DateTime dtTo = DateTime.Parse(FDateTo).AddDays(margin);        // zeby poprawnie złapać koniec, data jest 00 a czas > 00 dlatego jeszcze + 1 <<<< nie jest to potrzebne ..., stary algorytm łapie 1 dzień więcej

#if SIEMENS
            /* !!! UWAGA !!!
 * Alternatywną metodą do uprawnień (N,S), jest wykonanie tylu stref, ile jest testów ESD (N, S, NS) i przypisanie ich pracownikom.
 * Zalety:
 * - w datach od - do
 * - można cofnąć się ze zmianą parametrów
 * - zawsze będzie pokazywać poprawnie, bo na dzień
 * - nie wymaga zmian w poniższej funkcji
 * Wady:
 * - bardziej skomplikowana konfiguracja
 * - zarządzanie tylko z poziomu admina RCP (nie można zrobić prosych klików zmiany uprawnień na raporcie)
 * 
 * Ze względu na dotychczasowe ustalenia - zostajemy przy uprawnienieach 
 * 
 * 
 * OBSŁUGA PRIORYTETÓW: <<<<< do zrobienia, ale metoda jak poniżej
 * - ESD P=0, pozostałe P=1
 * - kontrola czy istnieje wcześniejsze odbicie w dobie pracowniczej o niższym priorytecie - w c#, bo sql zwraca ciurek
 * 
 * 
 */
            const string ESD_N  = "1001";
            const string ESD_S  = "1002";
            const string ESD_NS = "1003";

            string[] ESD_READERS = { ESD_N, ESD_S, ESD_NS };
/*
 * wersja z kilkoma testerami ESD
outer apply (select case when P1.N = 1 and P1.S = 1 then '1003,1006' 
					when P1.N = 1 then '1001,1004'
					when P1.S = 1 then '1002,1005'
					else null end ESDReaderIds) P2  

and (RCP.ECReaderId not in (1001,1002,1003,1004,1005,1006) or RCP.ECReaderId in (select items from dbo.SplitInt(P2.ESDReaderIds, ','))) -- jak nie ma praw 88,87 to nie bierze odbić ESD
*/

            string sql = String.Format(@"
declare @dataOd datetime 
declare @dataDo datetime 
declare @dataDo1 datetime 
declare @pracid int
set @dataOd = '{0}'
set @dataDo = '{1}'
set @pracid = {2}
set @dataDo1 = DATEADD(DAY, 1, @dataDo)

select 
	RCP.ECuniqueId, RCP.Czas, RCP.ECUserId, RCP.ECReaderId, 
	case when RCP.ECReaderId in (select items from dbo.SplitInt(S.Readers, ',') where items > 0)
	then convert(bit, 0) 
	else convert(bit, 1) 
	end as InOut2
--
  , RCP1.Priorytet
--
from Przypisania R 
--
left join Pracownicy P on P.Id = R.IdPracownika
outer apply (select dbo.GetRightId(P.Rights, {3}) N, dbo.GetRightId(P.Rights, {4}) S) P1
outer apply (select case when P1.N = 1 and P1.S = 1 then {7} 
					when P1.N = 1 then {5}
					when P1.S = 1 then {6}
					else null end ESDReaderId) P2  
--
left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika 
	and ISNULL(PK.Do, '20990909') >= R.Od
	and PK.Od <= ISNULL(R.Do, '20990909')
		
left outer join VStrefyReaders S on S.IdStrefy = R.RcpStrefaId 
	and ISNULL(S.DataDo, '20990909') >= dbo.MaxDate3(@dataOd, R.Od, PK.Od) 
	and S.DataOd < dbo.MinDate3(@dataDo1, DATEADD(DAY, 1, R.Do), ISNULL(PK.Do, '20990909')) 

left outer join RCP on RCP.ECUserId = PK.RcpId 
	and RCP.Czas >= dbo.MaxDate4(@dataOd, R.Od, S.DataOd, PK.Od)
	and RCP.Czas < dbo.MinDate4(@dataDo1, DATEADD(DAY, 1, R.Do), DATEADD(DAY, 1, PK.Do), S.DataDo) 
	and RCP.ECReaderId in (select abs(items) from dbo.SplitInt(S.Readers, ','))
--
    and (RCP.ECReaderId not in ({8}) or RCP.ECReaderId = P2.ESDReaderId) 
--
--
outer apply (select ISNULL(ISNULL(dbo.GetSyncValueInt(RCP.ECReaderId, S.Readers, S.Priorytet, ','),
						          dbo.GetSyncValueInt(-RCP.ECReaderId, S.Readers, S.Priorytet, ',')), 0) Priorytet) RCP1
--
where 
R.IdPracownika = @pracid and 
R.Od <= @dataDo and ISNULL(R.Do, '20990909') >= @dataOd and R.Status = 1
order by RCP.Czas
                ", Tools.DateToStr(dtFrom), Tools.DateToStr(dtTo), db.nullParam(FPracId)    // 0,1,2                 
                 , AppUser.rESD_Nadgarstek, AppUser.rESD_Stopa                              // 3,4
                 , ESD_N, ESD_S, ESD_NS                                                     // 5,6,7
                 , String.Join(",", ESD_READERS));                                          // 8

#else
            string sql = String.Format(@"
declare @dataOd datetime 
declare @dataDo datetime 
declare @dataDo1 datetime 
declare @pracid int
set @dataOd = '{0}'
set @dataDo = '{1}'
set @pracid = {2}
set @dataDo1 = DATEADD(DAY, 1, @dataDo)

select 
	RCP.ECuniqueId, RCP.Czas, RCP.ECUserId, RCP.ECReaderId, 
	case when RCP.ECReaderId in (select items from dbo.SplitInt(S.Readers, ',') where items > 0)
	then convert(bit, 0) 
	else convert(bit, 1) 
	end as InOut2
from Przypisania R 
left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika 
	and ISNULL(PK.Do, '20990909') >= R.Od
	and PK.Od <= ISNULL(R.Do, '20990909')
		
left outer join VStrefyReaders S on S.IdStrefy = R.RcpStrefaId 
	and ISNULL(S.DataDo, '20990909') >= dbo.MaxDate3(@dataOd, R.Od, PK.Od) 
	and S.DataOd < dbo.MinDate3(@dataDo1, DATEADD(DAY, 1, R.Do), ISNULL(PK.Do, '20990909')) 

left outer join RCP on RCP.ECUserId = PK.RcpId 
	and RCP.Czas >= dbo.MaxDate4(@dataOd, R.Od, S.DataOd, PK.Od)
	and RCP.Czas < dbo.MinDate4(@dataDo1, DATEADD(DAY, 1, R.Do), DATEADD(DAY, 1, PK.Do), S.DataDo) 
	and RCP.ECReaderId in (select abs(items) from dbo.SplitInt(S.Readers, ','))
where 
R.IdPracownika = @pracid and 
R.Od <= @dataDo and ISNULL(R.Do, '20990909') >= @dataOd and R.Status = 1
order by RCP.Czas
                ", Tools.DateToStr(dtFrom), Tools.DateToStr(dtTo), db.nullParam(FPracId));
#endif


            /* wolniejsze niż SplitInt	
	--and (CHARINDEX('-' + CONVERT(varchar, RCP.ECReaderId) + ',', ',' + S.Readers + ',') > 0 or
	--	 CHARINDEX(',' + CONVERT(varchar, RCP.ECReaderId) + ',', ',' + S.Readers + ',') > 0)
*/

            //_LastSql_v2 = sql;   // testy

            DataSet ds = db.getDataSet(sql);     // inner join lub where RCP.ECUniqueId is not null jest wolniejsze !!! 3sek <<<<<<<<< do sprawdzenia czy sql with ... select nie załatwiłoby sprawy równie szybko
            foreach (DataRow dr in db.getRows(ds))
                if (db.isNull(dr, 0))
                    dr.Delete();
            ds.AcceptChanges();
            return ds;
        }























        //------------------------------

        private string dsCompare(DataSet ds1, DataSet ds2)
        {
            string s = null;
            int c1 = ds1 != null ? db.getCount(ds1) : -1;
            int c2 = ds2 != null ? db.getCount(ds2) : -1;
            if (c1 != c2)
            {
                s = String.Format("ds1: {0} ds2: {1}", c1, c2) + "\n";
                s += "----------------------------------\n";
                s += (c1 <= 0 ? "ds1: brak danych" : db.toCSV(ds1)) + "\n";
                s += "----------------------------------\n";
                s += (c1 <= 0 ? "ds2: brak danych" : db.toCSV(ds2)) + "\n";
            }
            return s;
        }

        /*
        public static string dsCompare(DataRow[] ddr1, DataRow[] ddr2)
        {
            string csv1 = null;
            string csv2 = null;
            int c1 = 0;
            int c2 = 0;
            if (ds1 != null)
            {
                c1 = getCount(ds1);
                csv1 = toCSV(ds1);
            }
            if (ds2 != null)
            {
                c2 = getCount(ds2);
                csv2 = toCSV(ds2);
            }
            bool match = csv1 == csv2;
            string s1 = String.Format("match: {2} ds1_count: {0} ds2_count: {1}", c1, c2, match ? "1" : "0") + "\n";
            s1 += "----------------------------------\n";
            string diff = null;
            string s2 = null;
            s2 += "----------------------------------\n";
            if (c1 > 0) s2 += csv1 + "\n";
            s2 += "----------------------------------\n";
            if (c2 > 0) s2 += csv2 + "\n";

            if (!match)
            {
                int cnt1 = db.getCount(ds1);
                int cnt2 = db.getCount(ds2);
                int cols = ds1.Tables[0].Columns.Count;
                if (cnt2 < cnt1) cnt1 = cnt2;
                for (int i = 0; i < cnt1; i++)
                {
                    DataRow dr1 = db.getRow(ds1, i);
                    DataRow dr2 = db.getRow(ds2, i);
                    bool m = true;
                    for (int c = 0; c < cols; c++)
                        if (db.getValue(dr1, c) != db.getValue(dr2, c))
                        {
                            m = false;
                            break;
                        }
                    if (!m)
                    {
                        string l1 = null;
                        string l2 = null;
                        for (int c = 0; c < cols; c++)
                        {
                            string d1 = Tools.CtrlToText(dr1[c].ToString());
                            l1 += c == 0 ? d1 : (Tools.TAB + d1);          // mało optymalne !!! ale to do testów ...
                            string d2 = Tools.CtrlToText(dr2[c].ToString());
                            l2 += c == 0 ? d2 : (Tools.TAB + d2);          // mało optymalne !!! ale to do testów ...
                        }
                        diff += l1 + Tools.TAB + " --- " + Tools.TAB + l2 + "\n";
                    }
                }

            }
            string sss = s1 + diff + s2;
            if (!match)
            {
                int x = 0;  //<<<< tu załóż brakepoint'a
            }
            return sss;
        }
        */
        private DataSet _GetReadersData()
        {
            return _GetReadersData_v3();
        }

        private DataSet xGetReadersData()  // testy 
        {
            DataSet ds1, ds2;
            ds1 = GetReadersData_v1();
            //ds2 = GetReadersData_v2();  
            ds2 = _GetReadersData_v3();

            if (ds1 != null && ds2 != null)
            {
                //string s = dsCompare(ds1, ds2);
                //string sss = db.DataSetCompare(ds1, ds2);
                DataRow[] rows1 = ds1.Tables[0].Select(String.Format("Czas >= '{0}' and Czas <= '{1}'", FDateFrom, FDateTo));
                DataRow[] rows2 = ds2.Tables[0].Select(String.Format("Czas >= '{0}' and Czas <= '{1}'", FDateFrom, FDateTo));
                string sss = db.DataRowsCompare(rows1, rows2);

                sss = LastSql_v1;
                sss = _LastSql_v2;
            }
            else
            {
                int x = 0;
            }
            //return ds1;
            return ds2;
        }

        //-----------------------------
        private void PrepareWorkime5a()             // klasyfikuje odczyty do poszczególnych dni i zbija w pary
        {
            //----- klasyfikacja -----
            const int minOutIn = 7 * 3600;          // zakładana minimalna przerwa pomiędzy kolejnymi okresami pracy
            const int maxInOut = 19 * 3600;         // zakładany maksymalny czas pracy

            DataTable dtReaders = ReadersData;
            dtReaders.Columns.Add("Data", typeof(DateTime));

            DateTime prevCzas = DateTime.MinValue;
            int prevReaderId = -1;
            bool prevIsIn = false;
            DateTime? firstIn = null;
            DateTime? lastIn = null;
            DateTime? lastOut = null;
            DateTime _data = DateTime.MinValue;     // na jaki dzeń będzie odczyt

            bool isIn = true;
            DateTime czas = DateTime.MinValue;
            //----- pary in-out -----
            DataTable dtPairs = Data.Tables.Add();  // [1] Pairs
            dtPairs.Columns.Add("Data", typeof(DateTime));
            dtPairs.Columns.Add("TimeIn", typeof(DateTime));
            dtPairs.Columns.Add("TimeOut", typeof(DateTime));
            dtPairs.Columns.Add("before", typeof(int));     //przerwa przed sec.
            dtPairs.Columns.Add("czas", typeof(int));       //worktime
            dtPairs.Columns.Add("after", typeof(int));      //to samo co before następnego
            DataRow drPair = null;
            //----- funkcje -----
            Func<int> NowaPara = () =>
            {
                if (isIn)
                {
                    int? before = !prevIsIn && prevCzas != DateTime.MinValue ? (int?)Worktime.CountDateTimeSec(prevCzas, czas) : null;
                    if (drPair != null)             // poprzedna para - aktualizauję czas przerwy after, tylko tak mogę jak null
                        if (before == null)
                            drPair["after"] = DBNull.Value;
                        else
                            drPair["after"] = before;
                    drPair = dtPairs.Rows.Add(_data, czas, null, before, null, null);    // before jest na potrzeby posumowania przerw i nie musi uwzgledniać min czasu jak null itd. jak w storedproc
                    if (firstIn == null)
                        firstIn = czas;
                }
                else
                    drPair = dtPairs.Rows.Add(_data, null, czas, null, null, null);
                return 0;
            };

            Func<int> UpdateOrNowaPara = () =>
            {
                if (prevIsIn && !isIn)  // uaktualniam tylko prawidłowe
                {
                    drPair["TimeOut"] = czas;
                    drPair["czas"] = Worktime.CountDateTimeSec(prevCzas, czas);
                }
                else
                    NowaPara();
                return 0;
            };
            //-----
            Func<DataRow, int> _NowaData = (dr) =>
            {
                _data = czas.Date;
                dr["Data"] = _data;     // aktualizacja tatusia - odczytu Rogera
                lastIn = null;
                lastOut = null;
                firstIn = null;

                NowaPara();

                return 0;
            };

            Func<DataRow, int> StaraData = (dr) =>
            {
                dr["Data"] = _data;

                UpdateOrNowaPara();

                return 0;
            };

            Func<DataRow, DateTime, int> AnalizeOutIn = (dr, tOut) =>
            {
                int dt1 = Worktime.CountDateTimeSec(tOut, czas);          // odnoszę do out
                int dt2 = firstIn == null ? 0 : Worktime.CountDateTimeSec((DateTime)firstIn, czas); // i do firstIn - zeby nie było problemu z in-out-in przy długim pierwszym in-out //Kazimierz Bednarek 2012-03-22 - 34h 
                //if (tOut.Date != czas.Date && (dt1 > minOutIn || dt2 > maxInOut))    // zmiana daty i czas przerwy większy lub pracy -> nowy; licze do in jakby to byl out i spr czy czas pracy >
                //if (data != czas.Date && (dt1 > minOutIn || dt2 > maxInOut))    // zmiana daty i czas przerwy większy lub pracy -> nowy; licze do in jakby to byl out i spr czy czas pracy >
                if ((_data != czas.Date && dt1 > minOutIn) || dt2 > maxInOut)    // zmiana daty i czas przerwy większy lub pracy -> nowy dzień; licze do in jakby to byl out i spr czy czas pracy >
                    _NowaData(dr);
                else
                    StaraData(dr);
                return 0;
            };

            Func<DataRow, DateTime, int> AnalizeInOut = (dr, tIn) =>
            {
                int dt1 = Worktime.CountDateTimeSec(tIn, czas);                                     // odnoszę do in
                int dt2 = firstIn == null ? 0 : Worktime.CountDateTimeSec((DateTime)firstIn, czas); // i do firstIn - zeby nie było problemu z in-out-out Pietruszczak Michał 2012-03-12 32h
                //if (tIn.Date != czas.Date && (dt1 > maxInOut || dt2 > maxInOut))     // zmiana daty i czas pracy większy -> nowy
                //if (data != czas.Date && (dt1 > maxInOut || dt2 > maxInOut))     // zmiana daty i czas pracy większy -> nowy
                if ((_data != czas.Date && dt1 > maxInOut) || dt2 > maxInOut)     // zmiana daty i czas pracy większy -> nowy dzień
                    _NowaData(dr);
                else
                    StaraData(dr);
                return 0;
            };
            //-----
            Func<DataRow, int> AnalizeInIn = (dr) =>
            {
                int dt1 = Worktime.CountDateTimeSec((DateTime)lastIn, czas);          // odnoszę do in
                int dt2 = firstIn == null ? 0 : Worktime.CountDateTimeSec((DateTime)firstIn, czas); // i do firstIn - zeby nie było problemu z in-out-in przy długim pierwszym in-out //Kazimierz Bednarek 2012-03-22 - 34h 
                if ((_data != czas.Date && dt1 > maxInOut) || dt2 > maxInOut)    // zmiana daty i czas przerwy większy lub pracy -> nowy dzień; licze do in jakby to byl out i spr czy czas pracy >
                    _NowaData(dr);
                else
                {
                    dr["Data"] = _data;
                    NowaPara();
                }
                return 0;
            };

            Func<DataRow, int> AnalizeOutOut = (dr) =>
            {
                int dt1 = Worktime.CountDateTimeSec((DateTime)lastOut, czas);          // odnoszę do in
                int dt2 = firstIn == null ? 0 : Worktime.CountDateTimeSec((DateTime)firstIn, czas); // i do firstIn - zeby nie było problemu z in-out-in przy długim pierwszym in-out //Kazimierz Bednarek 2012-03-22 - 34h 
                if ((_data != czas.Date && dt1 > minOutIn) || dt2 > maxInOut)    // zmiana daty i czas przerwy większy lub pracy -> nowy dzień; licze do in jakby to byl out i spr czy czas pracy >
                    _NowaData(dr);
                else
                {
                    dr["Data"] = _data;
                    NowaPara();
                }
                return 0;
            };
            //----- pętelka klasyfikująca -----
            foreach (DataRow dr in dtReaders.Rows)
            {
                isIn = !Base.getBool(dr, "InOut2", false);                  // zawsze jest; 0=IN 1=OUT
                czas = (DateTime)Base.getDateTime(dr, "Czas");              // musi być
                int readerId = (int)Base.getInt(dr, "ECReaderId");          // musi być

                //if (czas != prevCzas || prevReaderId != readerId)         // pomijam powtórzone odczyty, nie sprawdzam juz in/out
                //if (czas != prevCzas || prevReaderId != readerId || prevIsIn != isIn) 
                // - ten byl - if (czas != prevCzas || prevIsIn != isIn)                   // pomijam powtórzone odczyty, sprawdzam in/out czy się różni - jeżeli odczyty o tym samym czasie z różnych readerów ale oba na in lub na out to przyjmuje jeden np. Dudkiewicz u Kruszki 10.04, 
                if ((czas != prevCzas || prevIsIn != isIn) &&               // pomijam powtórzone odczyty, sprawdzam in/out czy się różni - jeżeli odczyty o tym samym czasie z różnych readerów ale oba na in lub na out to przyjmuje jeden np. Dudkiewicz u Kruszki 10.04, 
                    !(prevReaderId == readerId && prevIsIn == isIn && (czas.Subtract(prevCzas).TotalSeconds < 60)))     // ten sam rejestrator, in-in lub out-out, różnica czasu < 60 sek. - zakładam że powtórne odbicie, 20120619
                {
                    //---- klasyfikacja -----
                    if (prevReaderId == -1)                                 // pierwszy odczyt zawsze w swoim dniu
                        _NowaData(dr);
                    else
                        if (isIn)                       //in
                            if (prevIsIn)               //in-in
                                if (lastOut == null)    //null-in
                                    AnalizeInIn(dr);
                                //NowaData(dr);
                                else                    //lastOut-in
                                    AnalizeOutIn(dr, (DateTime)lastOut); // odnoszę do ostatniego dobrego out
                            else                        //out-in - OK
                                AnalizeOutIn(dr, prevCzas);
                        else                            //out
                            if (prevIsIn)               //in-out - OK
                                AnalizeInOut(dr, prevCzas);
                            else                        //out-out
                                if (lastIn == null)     //null-out
                                    AnalizeOutOut(dr);
                                //NowaData(dr);
                                else                    //lastIn-out
                                    AnalizeInOut(dr, (DateTime)lastIn);
                    prevIsIn = isIn;
                    prevCzas = czas;
                    prevReaderId = readerId;
                    if (isIn) lastIn = czas;
                    else lastOut = czas;
                }
            }
        }

        private void PrepareWorkime5b()    // kumuluje pary na dzień, liczy czas i dodaje alerty
        {
            DataTable dtDays = Data.Tables.Add();       // [2]  -> DayData
            dtDays.Columns.Add("Data", typeof(DateTime));
            dtDays.Columns.Add("TimeIn", typeof(DateTime));
            dtDays.Columns.Add("TimeOut", typeof(DateTime));
            dtDays.Columns.Add("Czas1", typeof(int));   // łączny czas we/wy w sekundach - worktime1
            dtDays.Columns.Add("Czas2", typeof(int));   // czas jako suma w sekundach - worktime2 
            dtDays.Columns.Add("Noc1", typeof(int));
            dtDays.Columns.Add("Noc2", typeof(int));
            dtDays.Columns.Add("ponocy", typeof(int));
            dtDays.Columns.Add("ponocy2", typeof(int));
            dtDays.Columns.Add("Alert", typeof(int));
            dtDays.Columns.Add("przerwy", typeof(int)); // łaczny czas przerw w danym dniu pomiędzy we-wy, posumowane wszystko before co nie jest null i pierwszą parą
            dtDays.Columns.Add("idx1", typeof(int));    // pierwsza para w InOutData - pary rcp
            dtDays.Columns.Add("idx2", typeof(int));    // ostatnia para w InOutData
            dtDays.Columns.Add("idx1r", typeof(int));   // pierwszy wpis w ReadersData - odczyty rcp
            dtDays.Columns.Add("idx2r", typeof(int));   // ostatni wpis w ReadersData
            dtDays.Columns.Add("idx1rcp", typeof(int)); // pierwszy wpis w AnalizeData 
            dtDays.Columns.Add("idx2rcp", typeof(int)); // ostatni wpis w AnalizeData

            DateTime startTimeIn = DateTime.MinValue;
            DateTime data = DateTime.MinValue;
            DateTime dtNocOd = DateTime.MinValue;
            DateTime dtNocDo = DateTime.MinValue;

            DateTime? firstTimeIn = null;
            DateTime? lastTimeOut = null;
            int? czas2 = null;
            //int? noc2 = null;
            //int? ponocy2 = null;
            //int czas2 = 0;
            int noc2 = 0;
            int ponocy2 = 0;
            int alert = 0;
            int przerwy = 0;
            int idx1 = 0;
            int idx = 0;

            bool first = true;              // pierwsza para z dnia
            bool _lastIsCheckInOut = false;  // ostatnio przetwarzana para niekompletna, na potrzeby InsertDay
            bool _isCheckInOut = false;      // czy w ogóle jest

            Func<int> InsertDay = () =>
            {
                if (firstTimeIn == null) alert |= Worktime.alNoIn;
                if (lastTimeOut == null)
                {
                    alert |= Worktime.alNoOut;
                    if (_isCheckInOut && !_lastIsCheckInOut)
                        alert |= Worktime.alCheckInOut;     // nie dawac jak problem dotyczy pierwszego lub ostatniego, i jest noIn/noOut
                }
                int? czas1 = null;
                //int? noc1 = null;
                //int? ponocy1 = null;
                //int czas1 = 0;
                int noc1 = 0;
                int ponocy1 = 0;
                if (firstTimeIn != null && lastTimeOut != null)
                {
                    DateTime tIn = (DateTime)firstTimeIn;
                    DateTime tOut = (DateTime)lastTimeOut;
                    if (tIn < tOut)
                    {
                        int c1 = Worktime.CountDateTimeSec(tIn, tOut);
                        if (c1 > 0) czas1 = c1;
                        //int n1 = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);
                        //if (n1 > 0) noc1 = n1;
                        //int p1 = Worktime.TimeInterSec(dtNocDo, tOut, tIn, tOut);  //jak tOut < dtNocDo to nie załapie i ok
                        //if (p1 > 0) ponocy1 = p1;
                        //czas1 = Worktime.CountDateTimeSec(tIn, tOut);
                        noc1 = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);
                        ponocy1 = Worktime.TimeInterSec(dtNocDo, tOut, tIn, tOut);  //jak tOut < dtNocDo to nie załapie i ok
                    }
                    else
                        lastTimeOut = null; // tylko in daję jak niepoprawna kolejność, moge ustawić na null bo i tak po dodaniu zerwane
                }
                dtDays.Rows.Add(data, firstTimeIn, lastTimeOut, czas1, czas2, noc1, noc2, ponocy1, ponocy2, alert, przerwy, idx1, idx - 1);
                return 0;
            };

            foreach (DataRow dr in InOutData.Rows)
            {
                DateTime nextData = (DateTime)Base.getDateTime(dr, "Data");
                //----- zmiana daty - zapis i zerowanie -----
                if (data != DateTime.MinValue && data != nextData)
                {
                    InsertDay();
                    firstTimeIn = null;
                    lastTimeOut = null;
                    czas2 = null;
                    //noc2 = null;
                    //ponocy2 = null;
                    //czas2 = 0;
                    noc2 = 0;
                    ponocy2 = 0;
                    alert = 0;
                    przerwy = 0;
                    idx1 = idx;
                    first = true;
                    _isCheckInOut = false;
                }
                //----- -----
                _lastIsCheckInOut = false;
                //----- wartości -----
                if (nextData != data)
                {
                    data = nextData;        // nie trzeba sprawdzać warunku, nie szkodzi ze podstawi za kazdym razem
                    dtNocOd = data.AddSeconds(FNocFromSec);
                    dtNocDo = data.AddSeconds(FNocToSec);
                }
                DateTime? timeIn = Base.getDateTime(dr, "TimeIn");
                DateTime? timeOut = Base.getDateTime(dr, "TimeOut");
                //----- przerwy -----
                if (idx > idx1)
                {
                    int? p = Base.getInt(dr, "before");
                    if (!Base.isNull(p)) przerwy += (int)p;
                }
                //----- obliczenia -----
                if (firstTimeIn == null && timeIn != null)
                    firstTimeIn = timeIn;
                if (timeOut != null)
                    lastTimeOut = timeOut;
                if (timeIn != null && timeOut != null)
                {
                    int c2 = (int)Base.getInt(dr, "czas");        // jest zawsze, nawet jak timein/out nie ma to PrepareWt5a ustawia 0
                    //czas2 += c2;
                    if (c2 > 0)
                        if (czas2 == null) czas2 = c2;
                        else czas2 += c2;
                    int n2 = Worktime.TimeInterSec(timeIn, timeOut, dtNocOd, dtNocDo);
                    noc2 += n2;
                    //if (n2 > 0)
                    //    if (noc2 == null) noc2 = n2;
                    //    else noc2 += n2;
                    int p2 = Worktime.TimeInterSec(dtNocDo, timeOut, (DateTime)timeIn, (DateTime)timeOut);  //jak tOut < dtNocDo to nie załapie i ok
                    ponocy2 += p2;
                    //if (p2 > 0)
                    //    if (ponocy2 == null) ponocy2 = p2;
                    //    else ponocy2 += p2;
                }
                else     // nie ma we lub wy
                    if (!first)
                    // 20131111 alg jest co do dnia, a tu go jeszcze nie mam - wszystko niech będzie jak "12" później ten błąd się wytnie przy algorytmach we-wy
                    //if (FalgRCP == algSuma || FalgRCP == algSumaNadg         // "2", "12" tylko w tych algorytmach jest to istotne
                    //    || String.IsNullOrEmpty(FalgRCP)        // 20131026 i jak pusto przekazane bo domyslnie wtedy "12" bierze
                    //    )
                    {
                        //alert |= Worktime.alCheckInOut;       // nie dawac jak problem dotyczy pierwszego lub ostatniego, i jest noIn/noOut
                        _isCheckInOut = true;                    // brak we lub wy
                        _lastIsCheckInOut = true;                // ...dla ostatniego
                    }



                /*
                if (FalgRCP == algSuma || FalgRCP == algSumaNadg         // "2", "12" tylko w tych algorytmach jest to istotne
                    || String.IsNullOrEmpty(FalgRCP)        // 20131026 i jak pusto przekazane bo domyslnie wtedy "12" bierze
                    )
                {
                    //alert |= Worktime.alCheckInOut;       // nie dawac jak problem dotyczy pierwszego lub ostatniego, i jest noIn/noOut
                    isCheckInOut = true;                    // brak we lub wy
                    lastIsCheckInOut = true;                // ...dla ostatniego
                }
                */
                //----- index i flagi -----
                first = false;
                idx++;
            }
            //----- ostatne dane -----
            if (data != DateTime.MinValue)
                InsertDay();

            //----- tabelka analizy rcp - debug ?, wypełniana w CountWT2AndMerge.CountWT2, może być empty jak merge sie nie wykonuje -----
            DataTable dtAnalize = Data.Tables.Add();            //[3]-> AnalizeData
            dtAnalize.Columns.Add("Data", typeof(DateTime));
            dtAnalize.Columns.Add("Typ", typeof(int));          //0-praca, 1-przerwa zmianowa wliczona, 2-przerwa nie wliczona na zmianie, 3-nadgodziny dzień, 4-nadgodziny noc, 5-przerwa nadgodzin wliczona, 6-przerwa nadgodzin nie wliczona
            dtAnalize.Columns.Add("TimeOd", typeof(DateTime));
            dtAnalize.Columns.Add("TimeDo", typeof(DateTime));
            dtAnalize.Columns.Add("Czas", typeof(int));         //czas w sekundach
            dtAnalize.Columns.Add("Nocne", typeof(int));        //czas w sekundach
        }

        //----- WT2a --------------------------------------------
        public const int aeWork = 0;
        public const int aeBreak = 1;
        public const int _aeBreakNoCount = 2;
        public const int aeOver = 3;
        public const int aeBreakOver = 4;
        public const int aeBreakOverNoCount = 5;
        public const int aeWorkNoCount = 6;    // nadgodziny jak zmiana lub algorytm ich nie ma
        public const int aeOut = 7;    // wyjście wliczone - alg 1, 11, 3
        public const int aeOutNoCount = 8;    // wyjście niewliczone - alg 1, 11, 3
        public const int aeLateNoCount = 9;    // spóźnienie niewliczone  
        public const int aeOutOver = 10;   // wyjście wliczone nadgodziny - alg 1, 11, 3

        public const int aePrzerwaWliczona = 11; // przerwa wliczona w czas pracy
        public const int aePrzerwaWliczonaNadg = 12; // przerwa wliczona w czas pracy - nadgodziny
        public const int aePrzerwaNiewliczona = 13; // w tym przerwa niewliczona

        public const int aeDodatek50 = 14;   // dodatek 50 %
        public const int aeDodatek100 = 15;   // dodatek 100 %

        public const int aeNoCount = 16;

        public const int aeLast = 16;
        /*
        public const int aeOverDay = 3;
        public const int aeOverNight = 4;
        public const int aeBreakOverDay = 5;
        public const int aeBreakOverDayNoCount = 6;
        public const int aeBreakOverNight = 7;
        public const int aeBreakOverNightNoCount = 8;
        */
        public static string AnalizeEntryStr(int atyp)
        {
            string[] ae = {
                "praca na zmianie",
                "przerwa wliczona",
                "przerwa niewliczona",
                "nadgodziny",
                "przerwa wliczona",
                "przerwa niewliczona",
                "czas niewliczony",      
                "wyjście wliczone",
                "wyjście niewliczone",
                "spóźnienie niewliczone",
                "wyjście wliczone",          // wyjście wliczone nadgodziny - alg 1, 11, 3, musi być rozróżnione żeby można było dać kolor jak od nadgodzin
                "przerwa wliczona",
                "przerwa wliczona - nadgodziny",
                "w tym przerwa niewliczona",
                //"w tym dodatek 50%",
                //"w tym dodatek 100%"
                "praca na zmianie +50%",
                "praca na zmianie +100%",
                "praca na zmianie - niewliczone"

                /*
                "praca na zmianie",
                "przerwa zmianowa wliczona",
                "przerwa zmianowa niewliczona",
                "nadgodziny",
                "przerwa w czasie nadgodzin wliczona",
                "przerwa w czasie nadgodzin niewliczona"
                 */
                /*
                "praca na zmianie",
                "przerwa zmianowa wliczona",
                "przerwa zmianowa niewliczona",
                "nadgodziny zwykłe",
                "przerwa w czasie nadgodzin zwykłych wliczona",
                "przerwa w czasie nadgodzin zwykłych niewliczona",
                "nadgodziny nocne",
                "przerwa w czasie nadgodzin nocnych wliczona",
                "przerwa w czasie nadgodzin nocnych niewliczona"
                */
                };
            if (atyp >= 0 && atyp <= aeLast)
                return ae[atyp];
            else
                return null;
        }





















        // do zrobienia
        //----- liczenie - wywoływana tylko dla dni za które sa odczyty ------
        /*
        private void dtAnalizeAdd(DateTime dayData, int aeTyp, DateTime tIn, DateTime tOut, int? czas, int? nocne)
        {
            if (tIn != tOut)
                AnalizeData.Rows.Add(dayData, aeTyp, tIn, tOut, czas, nocne);
        }
        */




        public void _CountWT2(DataRow drDay,        // dzień z wszystkiech dni w okresie, z info o zmianie i absencjach, z GetWorktime
                            DataRow drRcpDay,       // dzień z rcp zebrany w Prepare - DayData, tylko dni, za które sa odczyty
                            int btZm, int btN,      //czasy przerw, tutaj już sekundy dla przerw!!!, nadpisuję później jak ma inaczej liczyć
                            ref int alert)
        {
            int _czasZm = 0;            // czas pracy na zmianie - sumowany
            int czasMax = 86400;        // do liczenia alg obecność 8h
            int przerwaZm = 0;          // przerwa w czasie zmiany
            int przerwaZmNom = 0;       // przerwa nominalna w czasie zmiany
            int nadgDzien = 0;
            int nadgNoc = 0;
            int przerwaNadg = 0;
            int przerwaNadgNom = 0;
            int nocne = 0;

            bool fMarginBefore = false;
            bool fZmPrzerwa = true;
            bool fZmCzas = true;
            bool fNadgPrzerwa = false;
            bool fNadgCzas = false;
            //bool fNoCount = false;

            // mam algorytm i parametr - uzależnić w przyszłości
            bool _fOver = true;                  // licz nadgodziny
            int aePrz = aeOut;                   // jak się ma nazywać przerwa: wyjście/przerwa - w zależności od algorytmu 
            int aePrzNoCount = aeOutNoCount;
            int aePrzOver = aeOut;
            int aePrzOverNoCount = aeOutNoCount;

            //----- dzień ------
            string algRCP = db.getValue(drDay, "RcpAlgorytm");
            string algPar = null;
            bool bWeWy = algRCP == algWeWy || algRCP == algWeWyNadg;
            bool bObecnosc = algRCP == algObecnosc8h;
            bool bSuma = algRCP == algSuma || algRCP == algSumaNadg || (!bWeWy && !bObecnosc);  // "2" "12" i inne przypadki niż We-Wy i obecnosc    

            bool zgodaNadg = db.getBool(drDay, "ZgodaNadg", false);     // czy w dniu jest zgoda na nadgodziny 
            int typ = db.getInt(drDay, "Typ", PlanPracy.zmNormalna);    // 0

            bool przWliczNocne = false;                                 // czy liczyć czas nocny dla przerwy wliczonej 
            int przWlicz = db.getInt(drDay, "PrzerwaWliczona", 0);

#if NOMWYM
            btZm += przWlicz;
            przWlicz = 0;
#endif

            int przNiewlicz = db.getInt(drDay, "PrzerwaNiewliczona", 0);
            int _wymiar = db.getInt(drDay, "WymiarCzasu", 28800);

            int par1 = db.getInt(drDay, "Par1", 0);
            int par2 = db.getInt(drDay, "Par2", 0);

            if (typ == PlanPracy.zmPiatek7)
            {
                /*przWlicz = par1;*/
                przNiewlicz = 0;
            }


            /*
            we-wy:
             + wlicz    : do czasu pracy +przerwa jeżeli czas < 8h, do 8h
             + niewlicz : od czasu pracy -przerwa zawsze
            suma:
             + wlicz    : do czasu pracy +przerwa
             + niewlicz : nic
            obecność 8h:
             + wlicz    : nic
             + niewlicz : nic
            */

            switch (algRCP)             // w danym dniu
            {
                case algBezLiczenia:    // "0":           // pomiń
                    return;
                case algWeWy:           // "1":           // we-wy
                    btZm = 86400;       // wszystko ma być wliczone
                    btN = 86400;
                    _fOver = false;
                    break;
                case algWeWyNadg:       // "11":          // we-wy z nadgodzinami
                    btZm = 86400;       // wszystko ma być wliczone                    
                    _fOver = Worktime.zmNadgodziny(drDay) && zgodaNadg;
                    if (_fOver) btN = 86400;
                    else btN = 0;
                    aePrzOver = aeOutOver;
                    break;
                case algSuma:           // "2":           // suma
                    btN = 0;            // wszysko ma być niewliczone
                    _fOver = false;
                    aePrz = aeBreak;
                    aePrzNoCount = _aeBreakNoCount;
                    aePrzOver = aeBreakOver;
                    aePrzOverNoCount = aeBreakOverNoCount;

                    //btZm += przWlicz;
                    break;
                default:
                case algSumaNadg:       // "12":          // suma z nadgodzinami
                    _fOver = Worktime.zmNadgodziny(drDay) && zgodaNadg;
                    if (!_fOver) btN = 0;
                    aePrz = aeBreak;
                    aePrzNoCount = _aeBreakNoCount;
                    aePrzOver = aeBreakOver;
                    aePrzOverNoCount = aeBreakOverNoCount;

                    //btZm += przWlicz;
                    break;
                case algObecnosc8h:     // "3":           // tak samo liczyć bedę jak 11, tylko skoryguję czas na we do 8h
                    algPar = "8";  // na sztywno !!! to i tak nie będzie inaczej

                    czasMax = Base.StrToIntDef(algPar, 8) * 3600;    // błąd powinien być wcześniej wychwycony
                    btZm = 86400;   // wszystko ma być wliczone                    
                    _fOver = Worktime.zmNadgodziny(drDay) && zgodaNadg;
                    if (_fOver) btN = 86400;
                    else btN = 0;
                    break;
            }
            //----- dzień ------
            DateTime dayData = (DateTime)Base.getDateTime(drDay, "Data");
            //----- czas nocny -----
            DateTime dtNocOd = dayData.AddSeconds(FNocFromSec);
            DateTime dtNocDo = dayData.AddSeconds(FNocToSec);
            //----- czas zmiany -----
            int _zmCzas = Worktime.zmCzas(drDay);        // -1 jak nie ma zmiany w danym dniu 
            int zmMargin = -1;
            DateTime zmTimeIn = DateTime.MinValue;
            DateTime zmTimeOut = DateTime.MinValue;
            DateTime zmTimeOutPrzN = DateTime.MinValue;
            DateTime zmTimeOutShort = DateTime.MinValue;
            DateTime zmTimeInMargin1 = DateTime.MinValue;
            DateTime zmTimeInMargin2 = DateTime.MinValue;
            DateTime zmTimeOutMargin1 = DateTime.MinValue;
            DateTime zmTimeOutMargin2 = DateTime.MinValue;


            //----- analiza rcp -----
            DataTable dtAnalize = AnalizeData;
            int idxA1 = dtAnalize.Rows.Count;

            //----- we-wy, suma -----
            if (_zmCzas >= 0)     // jest zmiana, czas na zmianie, -1 brak
            {
                //----- parametry zmiany -----
                zmTimeIn = dayData.Add(((DateTime)Base.getDateTime(drDay, "ZmianaOd")).TimeOfDay);
#if NOMWYM
                string zmSymbol = db.getValue(drDay, "Symbol");
                if (zmSymbol == "N1")  //T:20161025 iQor
                    _zmCzas = 0;
                else
                    _zmCzas = 28800;
                zmTimeOut = dayData.Add(((DateTime)Base.getDateTime(drDay, "ZmianaDo")).TimeOfDay);
                zmTimeOutPrzN = zmTimeOut.AddSeconds(przNiewlicz);
                zmTimeOutShort = zmTimeOut.AddSeconds(par1);
                if (zmTimeOutShort < zmTimeIn) zmTimeOutShort = zmTimeIn;
#else
                if (_wymiar > 0 && _wymiar != 28800)
                {
                    _zmCzas = _wymiar;
                    zmTimeOut = zmTimeIn.AddSeconds(_zmCzas);
                }
                else
                {
                    zmTimeOut = dayData.Add(((DateTime)Base.getDateTime(drDay, "ZmianaDo")).TimeOfDay);
                }
#endif
                if (zmTimeIn > zmTimeOut) zmTimeOut = zmTimeOut.AddDays(1);

                Boolean obetnijOdGory = db.getBool(drDay, "ObetnijOdGory", false);
                int marginesNadgodzin = db.getInt(drDay, "MarginesNadgodzin", 0) * 60;


                zmMargin = db.getInt(drDay, "Margines", -1);
                if (zmMargin != -1)
                {
                    zmMargin *= 60;  // wyrażony jest w minutach - zamieniam na sekundy lub -1 jak nie ma brać pod uwagę
                    zmTimeInMargin1 = zmTimeIn.AddSeconds(-zmMargin);
                    zmTimeInMargin2 = zmTimeIn.AddSeconds(zmMargin);
                    zmTimeOutMargin1 = zmTimeOut.AddSeconds(-zmMargin);
                    zmTimeOutMargin2 = zmTimeOut.AddSeconds(zmMargin);
                    fMarginBefore = true;
                }

                //----- -----
                //bool bWeWy = algRCP == algWeWy || algRCP == algWeWyNadg;
                //bool bObecnosc = algRCP == algObecnosc8h;
                //bool bSuma = algRCP == algSuma || algRCP == algSumaNadg || (!bWeWy && !bObecnosc);  // "2" "12" i inne przypadki niż We-Wy i obecnosc    

                int idx1 = Base.getInt(drRcpDay, "idx1", -1);
                int idx2 = Base.getInt(drRcpDay, "idx2", -1);
                if (idx1 >= 0 && idx2 >= 0 && idx2 >= idx1)  // wszystko ok, moze być 1 para dlatego >=
                {
                    //int przerwy = Base.getInt(drRcpDay, "przerwy", 0);  // łączne przerwy w dniu
                    DateTime? prevTimeOut = null;
                    DateTime? przerwaEnd = null;

                    //----- marines - korekta indeksów i czasów -----
                    /*
                    zmiana    |===|===|
                    ----------a---+---b-----
                    1
                    11-------t==============  timeIn: a 
                    12----------t===========  timeIn: t
                    13-------------------t==  timeIn: t, spóźnienie: t-b jako niewliczone, po t ma prawo do przerwy
                    2
                    21-c==d--t==============  timeIn: a, idx1=i
                    22-c==d-----t===========  timeIn: a, przerwa/niewliczone: a-t
                    23-c==d--------------t==  timeIn: a, przerwa/niewliczone: a-t  
                    3
                    -c==?--t==============  ustawiam: c=d i jak 2 licze
                    -c==?-----t===========  
                    -c==?--------------t==  
                    4
                    -?==d--t==============  ustawiam: c=d i jak 2 licze
                    -?==d-----t===========  
                    -?==d--------------t==  
                    */

                    DateTime? timeInKorekta = null;
                    DateTime? prevTimeOutKorekta = null;   // do korekty 22
                    int przerwaKorekta = -1;

                    bool bylTimeIn = false;
                    if (zmMargin != -1 && bSuma)
                    {
                        bool marginesBreak = false;
                        for (int i = idx1; i <= idx2; i++)                  // kręcę się po parach z info o przerwie przed - before
                        {
                            DataRow drD = InOutData.Rows[i];
                            int czas = Base.getInt(drD, "czas", 0);         // jest zawsze, najwyżej 0
                            int? przerwa = Base.getInt(drD, "before");      // moze być null, jak inny alg niż Sumy to null zeby nie pokazywał przerw
                            DateTime? timeIn = Base.getDateTime(drD, "TimeIn");
                            DateTime? timeOut = Base.getDateTime(drD, "TimeOut");
                            DateTime tIn = timeIn != null ? (DateTime)timeIn : (DateTime)timeOut;      // uzupełniam brakujący istniejącym !!!
                            DateTime tOut = timeOut != null ? (DateTime)timeOut : (DateTime)timeIn;
                            if (tOut > zmTimeInMargin1)         // czy odczyt się łapie - wyjście jest po początku marginesu, we określi co zrobić
                            {
                                bylTimeIn = i > idx1;
                                if (tIn < zmTimeInMargin1)      // wcześniejsze wejście 
                                {
                                    timeInKorekta = zmTimeInMargin1;
                                    if (przerwa != null && i > idx1) dtAnalize.Rows.Add(dayData, _aeBreakNoCount, prevTimeOut, tIn, (int)przerwa, null);   // za pierwszym obrotem nie ma pokazywać
                                    czas = Worktime.CountDateTimeSec(tIn, (DateTime)timeInKorekta);
                                    dtAnalize.Rows.Add(dayData, aeWorkNoCount, tIn, (DateTime)timeInKorekta, czas, null);
                                    if (i == idx1)              // 11 - odcinam
                                    {
                                    }
                                    else                        // 21 - czas niewliczony, przerwa niewliczona i odcinam
                                    {
                                        przerwaKorekta = 0;     // przerwa jest tu zdjęta
                                    }
                                }
                                else if (tIn <= zmTimeInMargin2)// zaliczone ok
                                {
                                    if (i == idx1)              // 12 - ok
                                    {
                                    }
                                    else                        // 22 - czas niewliczony, przerwa niewliczona, przerwa wliczona
                                    {
                                        if (przerwa != null)
                                        {
                                            int p = Worktime.CountDateTimeSec((DateTime)prevTimeOut, zmTimeInMargin1);
                                            przerwaKorekta = Worktime.CountDateTimeSec((DateTime)prevTimeOut, tIn) - p;
                                            //przerwaKorekta = (int)przerwa - p;
                                            if (przerwaKorekta < 0)
                                                przerwaKorekta = 0;  // zabezpieczenie
                                            dtAnalize.Rows.Add(dayData, _aeBreakNoCount, prevTimeOut, zmTimeInMargin1, p, null);
                                            prevTimeOut = zmTimeInMargin1;
                                        }
                                    }
                                }
                                else // tIn > zmTimeInMargin2   // późniejsze wejście
                                {
                                    if (i == idx1)              // 13 - spóźnienie
                                    {
                                        alert |= Worktime.alLate;


                                        //prevTimeOut = zmTimeInMargin2;      // potrzebne zeby policzyć przerwę, a jest null
                                        //przerwaKorekta = Worktime.CountDateTimeSec(zmTimeInMargin2, tIn);


                                        int p = Worktime.CountDateTimeSec(zmTimeInMargin2, tIn);
                                        dtAnalize.Rows.Add(dayData, aeLateNoCount, zmTimeInMargin2, tIn, p, null);


                                        //timeInKorekta = zmTimeInMargin2; ????


                                    }
                                    else                        // 23 - czas niewliczony, przerwa niewliczona, przerwa wliczona
                                    {
                                        if (przerwa != null)
                                        {
                                            int p = Worktime.CountDateTimeSec((DateTime)prevTimeOut, zmTimeInMargin1);
                                            przerwaKorekta = Worktime.CountDateTimeSec((DateTime)prevTimeOut, tIn) - p;
                                            //przerwaKorekta = (int)przerwa - p;
                                            dtAnalize.Rows.Add(dayData, _aeBreakNoCount, prevTimeOut, zmTimeInMargin1, p, null);
                                            prevTimeOut = zmTimeInMargin1;
                                        }
                                    }
                                }
                                idx1 = i;
                                marginesBreak = true;
                                break;
                            }
                            else
                            {
                                if (przerwa != null && i > idx1) dtAnalize.Rows.Add(dayData, _aeBreakNoCount, prevTimeOut, tIn, (int)przerwa, null);
                                dtAnalize.Rows.Add(dayData, aeWorkNoCount, tIn, tOut, czas, null);
                            }
                            prevTimeOut = tOut;
                        }
                        if (!marginesBreak)
                        {
                            idx1 = idx2 + 1; // albo wyłaczyć wykonanie poniższej pętli
                        }
                    }









                    //----- sumator parowy -----
                    if (bSuma)
                    {
                        //prevTimeOut = null; <<< zostaje ustawiony wg marginesu
                        for (int i = idx1; i <= idx2; i++)                  // kręcę się po parach z info o przerwie przed - before
                        {
                            DataRow drD = InOutData.Rows[i];
                            //----- wartości z pary ----- 
                            //DateTime data = (DateTime)Base.getDateTime(drD, "Data");
                            DateTime? timeIn = Base.getDateTime(drD, "TimeIn");
                            DateTime? timeOut = Base.getDateTime(drD, "TimeOut");

                            DateTime tIn = timeIn != null ? (DateTime)timeIn : (DateTime)timeOut;      // uzupełniam brakujący istniejącym !!!
                            DateTime tOut = timeOut != null ? (DateTime)timeOut : (DateTime)timeIn;

                            int czas = Base.getInt(drD, "czas", 0);         // jest zawsze, najwyżej 0
                            int? przerwa = Base.getInt(drD, "before");      // moze być null ale to tu nie robi


                            //----- korekta wejścia przy marginesie ------
                            if (timeInKorekta != null)
                            {
                                int k = Worktime.CountDateTimeSec((DateTime)timeIn, (DateTime)timeInKorekta);
                                timeIn = timeInKorekta;
                                timeInKorekta = null;       // żeby się następnym razem już nie wykonało
                                czas -= k;
                                if (czas < 0)
                                    czas = 0;               // zabezpieczenie
                                if (bylTimeIn)
                                {
                                    przerwa = przerwa == null ? k : (int)przerwa - k;
                                    if ((int)przerwa < 0)
                                        przerwa = 0;        // zabezpieczenie
                                }
                            }



                            //bool ok = !Base.isNull(timeIn) && !Base.isNull(timeOut);
                            //----- -----
                            if (fZmPrzerwa)                                                 // na starcie = true
                            {
                                if (i > idx1 || i == idx1 && bylTimeIn) // || przerwaKorekta != -1)                     // dla pierwszego nie liczę, chyba ze był przed marginesem lub kiedy jest korekta - praktycznie tylko kiedy spóźnienie <<< jak licze jako przerwe niewliczoną to nie ma korekty                            
                                {
                                    if (!Base.isNull(przerwa) || przerwaKorekta != -1)                              // jest podany czas przerwy
                                    {
                                        if (przerwaKorekta != -1)
                                        {
                                            przerwa = przerwaKorekta;
                                            przerwaKorekta = -1;
                                        }
                                        int p = (int)przerwa;

                                        if (przerwaZmNom < btZm)                            // przerwa < '15, jeśli jest większa to i tak nie wliczam już do czasu nocnego; < a nie <= bo = będzie tylko jak najpierw przejdzie przez warunek <
                                        {
                                            if (przerwaZmNom + p > btZm)                    // właśnie przerwa przekracza '15, dopełniam do '15
                                                p = btZm - przerwaZmNom;                    // tyle przerwy brakuje do czasu '15

                                            if (_czasZm + przerwaZmNom + p > _zmCzas)         // spr przekroczenie czasu trwania zmiany
                                            {
                                                p = _zmCzas - _czasZm - przerwaZmNom;         // tyle sekund przerwy wliczanej w czas dorówna do czasu zmiany
                                                przerwaZmNom += p;                          // dokładam
                                                przerwaZm += p;
                                                przerwa -= p;                               // konieczne do policzenia startowej przerwy nadgodzin
                                                przerwaEnd = ((DateTime)prevTimeOut).AddSeconds(p);     // to jest jednocześnie czas początku nadgodzin
                                                int n = Worktime.TimeInterSec(prevTimeOut, przerwaEnd, dtNocOd, dtNocDo);
                                                nocne += n;
                                                //---- zapis analize -----
                                                if (p > 0) dtAnalize.Rows.Add(dayData, aePrz, prevTimeOut, przerwaEnd, p, n);

                                                fZmPrzerwa = false;                         // koniec liczenia zmiany
                                                fZmCzas = false;
                                                fNadgPrzerwa = true;
                                                fNadgCzas = true;
                                                prevTimeOut = przerwaEnd;                   // od tego momentu liczę nadgodziny, startuję od przerwy
                                                przerwaEnd = timeIn;
                                            }
                                            else                                            // nie przekroczono czasu zmiany
                                            {
                                                przerwaZmNom += p;                          // dodaję, tu może osiągnąć '15 ale nie więcej ze względu na wcześniejsze ustawienia
                                                przerwaEnd = ((DateTime)prevTimeOut).AddSeconds(p);
                                                int n = Worktime.TimeInterSec(prevTimeOut, przerwaEnd, dtNocOd, dtNocDo);
                                                nocne += n;
                                                przerwaZm += (int)przerwa;                  // całość dodaję
                                                //---- zapis analize -----
                                                int pn = (int)przerwa - p;
                                                if (p > 0)      // jest przerwa nominalna i moze być niewliczona
                                                {
                                                    dtAnalize.Rows.Add(dayData, aePrz, prevTimeOut, przerwaEnd, p, n);
                                                    if (pn > 0) dtAnalize.Rows.Add(dayData, aePrzNoCount, przerwaEnd, timeIn, pn, 0);       // niewliczona przerwa nie wlicza się w nocne
                                                }
                                                else            // nie ma nominalnej całość jako niewliczona, nie powinien tu wejść
                                                    if (pn > 0) dtAnalize.Rows.Add(dayData, aePrzNoCount, prevTimeOut, przerwaEnd, pn, 0);
                                            }
                                        }
                                        else
                                        {
                                            przerwaZm += p;                                 // łączna przerwa na zmianie, przerwa nominalna = '15 i juz nie dodaję do niej i nie liczę do czasu nocnego
                                            //---- zapis analize -----
                                            if (p > 0) dtAnalize.Rows.Add(dayData, aePrzNoCount, prevTimeOut, timeIn, p, 0);
                                        }
                                    }
                                }
                            }

                            if (fZmCzas)                                                    // czas na zmianie zawsze jest wliczony !!!
                            {
                                int c = czas;
                                if (_czasZm + przerwaZmNom + c > _zmCzas || _zmCzas == 0)     // przekroczenie czasu zmiany lub tylko nadgodziny - jezeli niepoprawny pierwszy odczyt to c = 0 i wskakiwął w else
                                {
                                    c = _zmCzas - _czasZm - przerwaZmNom;                    // tyle sekund dorówna do czasu zmiany
                                    if (c > 0)                                              // jak zmiana nie ma czasu to c == 0 i nie ma sensu pokazywać czas = 0:00:00
                                    {
                                        /*
                                        if (_czasZm > czasMax)                              // spr alg3 - obecność 8h
                                        {
                                            int c3 = _czasZm - czasMax;                     // 


                                        }
                                        else
                                        {
                                        }
                                         */
                                        _czasZm += c;                                       // czas pracy na zmianie czasZm = zmCzas ?
                                        czas -= c;                                          // do policzenia czasu nadgodzin
                                        DateTime dt = ((DateTime)timeIn).AddSeconds(c);     // wkontekscie analizowanej pary
                                        int n = Worktime.TimeInterSec(timeIn, dt, dtNocOd, dtNocDo);
                                        nocne += n;
                                        //if (timeIn != dt)
                                        dtAnalize.Rows.Add(dayData, aeWork, timeIn, dt, c, n);  // >>>>>
                                        timeIn = dt;
                                    }
                                    fZmPrzerwa = false;                                     // koniec liczenia zmiany
                                    fZmCzas = false;
                                    fNadgPrzerwa = false;                                   // startuję od czasu pracy
                                    fNadgCzas = true;
                                }
                                else
                                {
                                    _czasZm += c;
                                    int n = Worktime.TimeInterSec(timeIn, timeOut, dtNocOd, dtNocDo);  // całość liczę
                                    nocne += n;
                                    //---- zapis analize -----
                                    //if (timeIn != timeOut)
                                    dtAnalize.Rows.Add(dayData, aeWork, timeIn, timeOut, c, n);
                                }
                            }
                            if (fNadgPrzerwa)
                            {
                                if (i > idx1)                                                   // dla pierwszego nie liczę, np dla zmiany tylko nadgodziny
                                {
                                    if (!Base.isNull(przerwa))                                  // jest podany czas przerwy
                                    {
                                        int p = (int)przerwa;
                                        if (przerwaNadgNom < btN)                               // przerwa < '10, jeśli jest większa to i tak nie wliczam już do czasu nocnego; < a nie <= bo = będzie tylko jak najpierw przejdzie przez warunek <
                                        {
                                            if (przerwaNadgNom + p > btN)                       // właśnie przerwa przekracza '10, dopełniam do '10
                                                p = btN - przerwaNadgNom;                       // tyle przerwy brakuje do czasu '10

                                            przerwaNadgNom += p;
                                            przerwaEnd = ((DateTime)prevTimeOut).AddSeconds(p); // 
                                            int n = Worktime.TimeInterSec(prevTimeOut, przerwaEnd, dtNocOd, dtNocDo);
                                            nocne += n;
                                            nadgDzien += p - n;         // jak nie ma w nocy to n = 0
                                            nadgNoc += n;
                                            przerwaNadg += (int)przerwa;
                                            //---- zapis analize -----
                                            int pn = (int)przerwa - p;
                                            if (p > 0)      // jest przerwa nominalna i moze być niewliczona
                                            {
                                                dtAnalize.Rows.Add(dayData, aePrzOver, prevTimeOut, przerwaEnd, p, n);
                                                if (pn > 0) dtAnalize.Rows.Add(dayData, _fOver ? aePrzOverNoCount : aePrzNoCount, przerwaEnd, timeIn, pn, 0);       // niewliczona przerwa nie wlicza się w nocne
                                            }
                                            else            // nie ma nominalnej całość jako niewliczona, nie powinien tu wejść
                                                if (pn > 0) dtAnalize.Rows.Add(dayData, _fOver ? aePrzOverNoCount : aePrzNoCount, prevTimeOut, przerwaEnd, pn, 0);
                                        }
                                        else
                                        {
                                            przerwaNadg += p;
                                            //---- zapis analize -----
                                            if (p > 0) dtAnalize.Rows.Add(dayData, _fOver ? aePrzOverNoCount : aePrzNoCount, prevTimeOut, timeIn, p, 0);
                                        }
                                    }
                                }
                            }
                            if (fNadgCzas)
                            {
                                int c = czas;
                                int n = Worktime.TimeInterSec(timeIn, timeOut, dtNocOd, dtNocDo);
                                fNadgPrzerwa = true;                                        // jeżeli startuję od czasu pracy, to wypada włączyć 
                                if (_fOver)
                                {
                                    /*
                                    if (_czasZm + c > czasMax)                              // przekroczony czas 
                                    {
                                        c = czasMax - _czasZm;                              // tyle sekund dorówna do czasu max
                                        if (c > 0)                                          // jak zmiana nie ma czasu to c == 0 i nie ma sensu pokazywać czas = 0:00:00
                                        {
                                            DateTime dt = ((DateTime)timeIn).AddSeconds(c);
                                            n = Worktime.TimeInterSec(timeIn, dt, dtNocOd, dtNocDo);
                                            nocne += n;
                                            nadgDzien += c - n;                             // jak nie ma w nocy to n = 0
                                            nadgNoc += n;
                                            //---- zapis analize -----
                                            dtAnalize.Rows.Add(dayData, aeOver, timeIn, timeOut, c, n);
                                            timeIn = dt;
                                        }
                                        fNadgCzas = false;
                                        fNadgPrzerwa = false;
                                        fNoCount = true;
                                    }
                                    else
                                    {
                                        nocne += n;
                                        nadgDzien += c - n;                                         // jak nie ma w nocy to n = 0
                                        nadgNoc += n;
                                        //---- zapis analize -----
                                        dtAnalize.Rows.Add(dayData, aeOver, timeIn, timeOut, c, n);
                                    }
                                    */
                                    nocne += n;
                                    nadgDzien += c - n;                                         // jak nie ma w nocy to n = 0
                                    nadgNoc += n;
                                    dtAnalize.Rows.Add(dayData, aeOver, timeIn, timeOut, c, n);
                                }
                                else
                                {
                                    //dtAnalize.Rows.Add(dayData, aeWorkNoCount, timeIn, timeOut, c, n);
                                    dtAnalize.Rows.Add(dayData, aeWorkNoCount, timeIn, timeOut, c, n);
                                }
                            }


                            /*
                            if (fNoCount)
                            {
                                int c = czas;
                                int n = Worktime.TimeInterSec(timeIn, timeOut, dtNocOd, dtNocDo);
                                {
                                    //---- zapis analize -----
                                    dtAnalize.Rows.Add(dayData, aeWorkNoCount, timeIn, timeOut, c, n);
                                }
                            }
                            */


                            prevTimeOut = timeOut;
                        }
                        //----- przerwa wliczona, przerwy niewliczonej nie ma sensu liczyć ------
                        if (przWlicz > 0 && prevTimeOut != null)
                        {
                            int p = _zmCzas - _czasZm;   // tyle zostało do dopełnienia zmiany
                            DateTime dt1, dt2;
                            //----- przerwa wliczona - czas pracy na zmianie
                            if (p > 0)
                            {
                                if (p > przWlicz) p = przWlicz;
                                _czasZm += p;
                                dt1 = (DateTime)prevTimeOut;
                                dt2 = dt1.AddSeconds(p);
                                int? nn = null;
                                if (przWliczNocne)
                                {
                                    nn = Worktime.TimeInterSec(dt1, dt2, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                    nocne += (int)nn;
                                }
                                dtAnalize.Rows.Add(dayData, aePrzerwaWliczona, dt1, dt2, p, nn);    // bez czasu nocnego bo matki karmiace rozliczamy jak urlop !!!
                                p = przWlicz - p;  // tyle zostanie
                            }
                            else
                            {
                                p = przWlicz;
                                dt2 = (DateTime)prevTimeOut;
                            }
                            //----- przerwa wliczona - nadgodziny / czas niewliczony - ERROR
                            if (p > 0)   // p - ile przerwy ma wejsc w nadgodziny
                            {
                                DateTime dt3 = dt2.AddSeconds(p);
                                int n = Worktime.TimeInterSec(dt2, dt3, dtNocOd, dtNocDo);
                                if (_fOver)
                                {
                                    nadgDzien += p - n;
                                    nadgNoc += n;
                                    int? nn = null;
                                    if (przWliczNocne)
                                    {
                                        nn = n;
                                        nocne += n;
                                    }
                                    dtAnalize.Rows.Add(dayData, aePrzerwaWliczonaNadg, dt2, dt3, p, nn);
                                }
                                else
                                {
                                    int? nn = null;
                                    if (przWliczNocne)
                                    {
                                        nn = n;
                                    }
                                    dtAnalize.Rows.Add(dayData, aeBreakOverNoCount, dt2, dt3, p, nn);
                                    alert |= Worktime.alPrzWliczNoNadg;
                                }
                            }
                        }
                        //----- wcześniejsze wyjście zamiast przerwy - zaliczona zmiana, brak nadgodzin ------ -> powinien kierownik decydować na razie jako "cicha umowa"
                        if (algRCP == algSuma || algRCP == algSumaNadg    // "2" "12"
                            || String.IsNullOrEmpty(algRCP))               // 20131026
                        {
                            if (_czasZm + przerwaZmNom < _zmCzas &&           // brak czasu nominalnego 
                                przerwaZmNom < btZm &&                      // przerwaNom < '15
                                nadgDzien == 0 && nadgNoc == 0 &&           // na wszelki wypadek nadgodziny = 0
                                _czasZm + btZm >= _zmCzas)                    // czy starczy przerwy zeby zaliczyć
                            {
                                int p = _zmCzas - _czasZm - przerwaZmNom;     // ile brakuje
                                int pn = btZm - przerwaZmNom;               // ile przerwy zostało do wykorzystania
                                if (p <= pn)                                // zmieszczę się w pozostałym czasie przerwy, else - nie da rady 
                                {
                                    DateTime dayTimeOut = (DateTime)Base.getDateTime(drDay, "TimeOut");  // ostatni odczyt, zakładam ze jest bo jak in-null to nie wiem jak policzyć czas nocny 
                                    DateTime dt = dayTimeOut.AddSeconds(p);
                                    przerwaZmNom += p;
                                    int n = Worktime.TimeInterSec(dayTimeOut, dt, dtNocOd, dtNocDo);
                                    nocne += n;
                                    alert |= Worktime.alBreakPass;
                                    //---- zapis analize -----
                                    if (p > 0) dtAnalize.Rows.Add(dayData, aePrz, dayTimeOut, dt, p, n);
                                }
                            }
                        }
                    }
                    //----- algorytm we-wy (i z nadgodzinami) -----
                    else if (bWeWy)
                    {
                        //_czasZm = 0;
                        //przerwaZm = 0;
                        //przerwaZmNom = 0;
                        //nadgDzien = 0;
                        //nadgNoc = 0;
                        //przerwaNadg = 0;
                        //przerwaNadgNom = 0;

                        if (idx1 <= idx2)
                        {
                            DateTime tIn;
                            DateTime tOut;
                            DataRow drD;
                            //----- we - wy ------
                            drD = InOutData.Rows[idx1];
                            DateTime? timeIn = db.getDateTime(drD, "TimeIn");
                            tIn = timeIn != null ? (DateTime)timeIn : (DateTime)db.getDateTime(drD, "TimeOut");                 // jak nie ma przyjmuję wyjście jako wejście
                            drD = InOutData.Rows[idx2];
                            DateTime? timeOut = db.getDateTime(drD, "TimeOut");
                            tOut = timeOut != null ? (DateTime)timeOut : tOut = (DateTime)db.getDateTime(drD, "TimeIn");        // jak nie ma przyjmuję wejście jako wyjście

                            int czas = Worktime.CountDateTimeSec(tIn, tOut);
                            //----- margin -----
                            if (zmMargin != -1)
                                if (tIn < zmTimeInMargin1)          // obcięcie
                                {
                                    int c = Worktime.CountDateTimeSec(tIn, zmTimeInMargin1);
                                    if (c > czas) c = czas;
                                    if (c == 0)         // 1 odbicie tylko tIn = tOut i czas == 0
                                        dtAnalize.Rows.Add(dayData, aeWorkNoCount, tIn, null, c, null);
                                    else
                                    {
                                        DateTime t = tIn.AddSeconds(c);
                                        dtAnalize.Rows.Add(dayData, aeWorkNoCount, tIn, t, c, null);
                                        tIn = zmTimeInMargin1;      // jak za dużo to tOut < tIn i nie pójdzie
                                        czas = Worktime.CountDateTimeSec(tIn, tOut);
                                    }
                                }
                                else if (tIn > zmTimeInMargin2 && timeIn != null)     // spóźnienie
                                {
                                    alert |= Worktime.alLate;
                                    int c = Worktime.CountDateTimeSec(zmTimeInMargin2, tIn);
                                    dtAnalize.Rows.Add(dayData, aeLateNoCount, zmTimeInMargin2, tIn, c, null);
                                }
                            //----- czas pracy i nadgodziny -----
                            if (czas > 0 && tIn <= tOut)
                            {
                                if (czas + przWlicz - przNiewlicz <= _zmCzas)                 // brak nadgodzin
                                {
                                    _czasZm = czas + przWlicz - przNiewlicz;
                                    if (_czasZm < 0) _czasZm = 0;

                                    nocne = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                    if (tIn != tOut)
                                    {
#if SIEMENS
                                        DateTime _tIn = tIn;
                                        DateTime _tOut = tOut;
                                        const int t6 = 6 * 3600;
                                        int sIn = Worktime.TimeToSec(_tIn);
                                        if (typ == PlanPracy.zmPoniedzialek5 && dayData.DayOfWeek == DayOfWeek.Monday && sIn < t6)
                                        {
                                            DateTime t1 = _tIn.AddSeconds(t6 - sIn);
                                            int n1, c1;
                                            if (_tIn != t1)
                                            {
                                                c1 = Worktime.CountDateTimeSec(_tIn, t1);
                                                n1 = Worktime.TimeInterSec(_tIn, t1, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                //dtAnalize.Rows.Add(dayData, aeOver, _tIn, t1, c1, n1);
                                                dtAnalize.Rows.Add(dayData, aeDodatek100, _tIn, t1, c1, n1);
                                            }
                                            if (t1 != _tOut)
                                            {
                                                c1 = Worktime.CountDateTimeSec(t1, _tOut);
                                                n1 = Worktime.TimeInterSec(t1, _tOut, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                dtAnalize.Rows.Add(dayData, aeWork, t1, _tOut, c1, n1);
                                            }
                                        }
                                        else
                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, tOut, czas, nocne);
#elif DBW || VICIM
                                        DateTime _tIn = tIn;
                                        DateTime _tOut = tOut;
                                        const int t6 = 6 * 3600;
                                        int sIn = Worktime.TimeToSec(_tIn);
                                        /**/
                                        int p = czas;
                                        int p0 = _zmCzas + przNiewlicz;      // 8h+0:30
                                        if (p > p0) p = p0;
                                        DateTime dt1 = tIn.AddSeconds(p);   // moze byc = tOut jak p <= p0 !!! - nie ma nadgodzin
                                        /**/
                                        //if (typ == PlanPracy.zmPoniedzialek5 && dayData.DayOfWeek == DayOfWeek.Monday && sIn < t6)
                                        if (typ == PlanPracy.zmPoniedzialek5 && sIn < t6)
                                        {
                                            DateTime t1 = _tIn.AddSeconds(t6 - sIn);
                                            int n1, c1;
                                            if (_tIn != t1)
                                            {
                                                c1 = Worktime.CountDateTimeSec(_tIn, t1);
                                                n1 = Worktime.TimeInterSec(_tIn, t1, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                //dtAnalize.Rows.Add(dayData, aeOver, _tIn, t1, c1, n1);
                                                dtAnalize.Rows.Add(dayData, aeDodatek100, _tIn, t1, c1, n1);
                                                if (dayData.DayOfWeek == DayOfWeek.Monday)
                                                    dtAnalize.Rows.Add(dayData, aeDodatek100, _tIn, t1, c1, n1);
                                                else
                                                    dtAnalize.Rows.Add(dayData, aeDodatek50, _tIn, t1, c1, n1);
                                            }
                                            if (t1 != _tOut)
                                            {
                                                c1 = Worktime.CountDateTimeSec(t1, _tOut);
                                                n1 = Worktime.TimeInterSec(t1, _tOut, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                dtAnalize.Rows.Add(dayData, aeWork, t1, _tOut, c1, n1);
                                            }
                                        }
                                        else if (obetnijOdGory && tIn < zmTimeOutPrzN && zmTimeOutPrzN < dt1)
                                        {
                                            int obcinamy = Worktime.CountDateTimeSec(zmTimeOutPrzN, dt1);
                                            p -= obcinamy;

                                            _czasZm -= obcinamy;

                                            if (p < 0) p = 0;
                                            if (_czasZm < 0) _czasZm = 0;

                                            nocne = Worktime.TimeInterSec(tIn, zmTimeOutPrzN, dtNocOd, dtNocDo);
                                            int n2 = Worktime.TimeInterSec(zmTimeOutPrzN, dt1, dtNocOd, dtNocDo);

                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, zmTimeOutPrzN, p, nocne);
                                            dtAnalize.Rows.Add(dayData, aeNoCount, zmTimeOutPrzN, dt1, obcinamy, n2);
                                        }
                                        else if (typ == PlanPracy.zmPiatek7 && zmTimeOutShort <= tOut && tOut < zmTimeOut)
                                        {
                                            DateTime zmTimeOutS = zmTimeOut; /*= (obetnijOdGory) ? ((zmMargin == -1) ? zmTimeOut.AddSeconds(-Worktime.CountDateTimeSec(tIn, zmTimeIn)) : zmTimeOut) : zmTimeOut.AddSeconds(Worktime.CountDateTimeSec(zmTimeIn, tIn));*/

                                            /* Margines i przyciecie */

                                            if (zmMargin != -1 &&  obetnijOdGory) zmTimeOutS = zmTimeOut.AddSeconds(Worktime.CountDateTimeSec(zmTimeIn, tIn)).AddSeconds(Worktime.CountDateTimeSec(tIn, zmTimeIn));

                                            /* Margines i brak przyciecia */

                                            if (zmMargin != -1 && !obetnijOdGory) zmTimeOutS = zmTimeOut.AddSeconds(Worktime.CountDateTimeSec(zmTimeIn, tIn));

                                            /* Brak marginesu i przyciecie */

                                            if (zmMargin == -1 &&  obetnijOdGory) zmTimeOutS = zmTimeOut.AddSeconds((tIn < zmTimeIn) ? -Worktime.CountDateTimeSec(tIn, zmTimeIn) : 0);

                                            /* Brak marginesu i brak przyciecia */

                                            if (zmMargin == -1 && !obetnijOdGory) zmTimeOutS = zmTimeOut.AddSeconds(-Worktime.CountDateTimeSec(tIn, zmTimeIn));

                                            int c1 = Worktime.CountDateTimeSec(tIn, tOut);
                                            int c2 = Worktime.CountDateTimeSec(tOut, zmTimeOutS);

                                            int n1 = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);
                                            int n2 = Worktime.TimeInterSec(tOut, zmTimeOut, dtNocOd, dtNocDo);

                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, tOut, c1, n1);
                                            dtAnalize.Rows.Add(dayData, aePrzerwaWliczona, tOut, zmTimeOutS, c2, n2);
                                            _czasZm += c2;
                                        }
                                        else
                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, tOut, czas, nocne);
#else
                                        dtAnalize.Rows.Add(dayData, aeWork, tIn, tOut, czas, nocne);
#endif
                                    }



                                    if (przNiewlicz > 0)            // na papierosa 
                                    {
                                        int? nn = null;
                                        if (nocne > czas / 2)       // jezeli wiecej w nocy to odejmuje
                                        {
                                            nn = -przNiewlicz;
                                            nocne -= przNiewlicz;
                                            if (nocne < 0) nocne = 0;
                                        }
                                        dtAnalize.Rows.Add(dayData, aePrzerwaNiewliczona, null, null, -przNiewlicz, nn);   // -0:30 -0:30/null, nawet jak nie ma tyle czasu to pełna wartosc przerwy bo 
                                    }

                                    if (przWlicz > 0)               // karmiące, przyjmuje ze zaczyna się po czasie przerwy niewliczonej, zazwyczaj jest to wczesniejsze wyjscie pracownika        
                                    {
                                        DateTime tOut2 = tOut.AddSeconds(przWlicz);
                                        int? nn = null;
                                        if (przWliczNocne)
                                        {
                                            nn = Worktime.TimeInterSec(tOut, tOut2, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                            nocne += (int)nn;
                                        }
                                        dtAnalize.Rows.Add(dayData, aePrzerwaWliczona, tOut, tOut2, przWlicz, nn);    // bez czasu nocnego bo matki karmiace rozliczamy jak urlop !!!
                                    }
                                }
                                else    // nadgodziny, przerwa wliczona nie jest pokazywana, bo nie liczę przerw przy we-wy
                                {
                                    _czasZm = _zmCzas;
                                    //----- praca na zmianie 
                                    int p = czas;
                                    int p0 = _zmCzas + przNiewlicz;      // 8h+0:30
                                    if (p > p0) p = p0;
                                    DateTime dt1 = tIn.AddSeconds(p);   // moze byc = tOut jak p <= p0 !!! - nie ma nadgodzin
                                    nocne = Worktime.TimeInterSec(tIn, dt1, dtNocOd, dtNocDo);


                                    if (tIn != dt1)                   // zmiana N1 ma we=wy
                                    {
#if SIEMENS
                                        DateTime _tIn = tIn;
                                        DateTime _tOut = dt1;
                                        const int t6 = 6 * 3600;
                                        int sIn = Worktime.TimeToSec(_tIn);
                                        if (typ == PlanPracy.zmPoniedzialek5 && dayData.DayOfWeek == DayOfWeek.Monday && sIn < t6)
                                        {
                                            DateTime t1 = _tIn.AddSeconds(t6 - sIn);
                                            int n1, c1;
                                            if (_tIn != t1)
                                            {
                                                c1 = Worktime.CountDateTimeSec(_tIn, t1);
                                                n1 = Worktime.TimeInterSec(_tIn, t1, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                //dtAnalize.Rows.Add(dayData, aeOver, _tIn, t1, c1, n1);
                                                dtAnalize.Rows.Add(dayData, aeDodatek100, _tIn, t1, c1, n1);
                                            }
                                            if (t1 != _tOut)
                                            {
                                                c1 = Worktime.CountDateTimeSec(t1, _tOut);
                                                n1 = Worktime.TimeInterSec(t1, _tOut, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                dtAnalize.Rows.Add(dayData, aeWork, t1, _tOut, c1, n1);
                                            }
                                        }
                                        else
                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, dt1, p, nocne);
#elif DBW || VICIM
                                        DateTime _tIn = tIn;
                                        DateTime _tOut = dt1;
                                        const int t6 = 6 * 3600;
                                        int sIn = Worktime.TimeToSec(_tIn);
                                        //if (typ == PlanPracy.zmPoniedzialek5 && dayData.DayOfWeek == DayOfWeek.Monday && sIn < t6)
                                        if (typ == PlanPracy.zmPoniedzialek5 && sIn < t6)
                                        {
                                            DateTime t1 = _tIn.AddSeconds(t6 - sIn);
                                            int n1, c1;
                                            if (_tIn != t1)
                                            {
                                                c1 = Worktime.CountDateTimeSec(_tIn, t1);
                                                n1 = Worktime.TimeInterSec(_tIn, t1, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                //dtAnalize.Rows.Add(dayData, aeOver, _tIn, t1, c1, n1);
                                                if (dayData.DayOfWeek == DayOfWeek.Monday)
                                                    dtAnalize.Rows.Add(dayData, aeDodatek100, _tIn, t1, c1, n1);
                                                else
                                                    dtAnalize.Rows.Add(dayData, aeDodatek50, _tIn, t1, c1, n1);
                                            }
                                            if (t1 != _tOut)
                                            {
                                                c1 = Worktime.CountDateTimeSec(t1, _tOut);
                                                n1 = Worktime.TimeInterSec(t1, _tOut, dtNocOd, dtNocDo);      // in-out = czas z przerwa wliczona i bez przerwy niewliczonej
                                                dtAnalize.Rows.Add(dayData, aeWork, t1, _tOut, c1, n1);
                                            }
                                        }
                                        else if (obetnijOdGory && tIn < zmTimeOut && zmTimeOut < dt1)
                                        {
                                            int obcinamy = Worktime.CountDateTimeSec(zmTimeOut, dt1);
                                            p -= obcinamy;

                                            _czasZm -= obcinamy;

                                            if (p < 0) p = 0;
                                            if (_czasZm < 0) _czasZm = 0;

                                            nocne = Worktime.TimeInterSec(tIn, zmTimeOut, dtNocOd, dtNocDo);
                                            int n2 = Worktime.TimeInterSec(zmTimeOut, dt1, dtNocOd, dtNocDo);

                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, zmTimeOut, p, nocne);
                                            dtAnalize.Rows.Add(dayData, aeNoCount, zmTimeOut, dt1, obcinamy, n2);
                                        }
                                        else
                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, dt1, p, nocne);
#else
                                        if (obetnijOdGory && tIn < zmTimeOut && zmTimeOut < dt1)
                                        {
                                            int obcinamy = Worktime.CountDateTimeSec(zmTimeOut, dt1);
                                            p -= obcinamy;

                                            _czasZm -= obcinamy;

                                            if (p < 0) p = 0;
                                            if (_czasZm < 0) _czasZm = 0;

                                            nocne = Worktime.TimeInterSec(tIn, zmTimeOut, dtNocOd, dtNocDo);
                                            int n2 = Worktime.TimeInterSec(zmTimeOut, dt1, dtNocOd, dtNocDo);

                                            dtAnalize.Rows.Add(dayData, aeWork, tIn, zmTimeOut, p, nocne);
                                            dtAnalize.Rows.Add(dayData, aeNoCount, zmTimeOut, dt1, obcinamy, n2);
                                        }
                                        else dtAnalize.Rows.Add(dayData, aeWork, tIn, dt1, p, nocne);
#endif
                                    }

                                    //----- przerwa niewliczona - na papierosa
                                    if (przNiewlicz > 0)
                                    {
                                        int? nn = null;
                                        if (nocne > czas / 2)            // jezeli wiecej w nocy to odejmuje, czas zawiera przerwe niewliczona
                                        {
                                            nn = -przNiewlicz;
                                            nocne -= przNiewlicz;
                                            if (nocne < 0) nocne = 0;
                                        }
                                        dtAnalize.Rows.Add(dayData, aePrzerwaNiewliczona, null, null, -przNiewlicz, nn);   // -0:30 -0:30/null, nawet jak nie ma tyle czasu to pełna wartosc przerwy bo 
                                    }
                                    //----- nadgodziny / czas niewliczony
                                    p = czas - przNiewlicz - _zmCzas;        // czas zawiera przNiewlicz, p = nadgodziny - tyle czasu zostało do przeliczenia
                                    if (p > 0)                              // czy sa nadgodziny mimo przerwy niewliczonej, else => dt1 == tOut
                                    {
                                        int n = Worktime.TimeInterSec(dt1, tOut, dtNocOd, dtNocDo);
                                        if (_fOver && p >= marginesNadgodzin)
                                        {
                                            nocne += n;
                                            nadgDzien = p - n;
                                            nadgNoc = n;
                                            dtAnalize.Rows.Add(dayData, aeOver, dt1, tOut, p, n);
                                        }
                                        else
                                        {
                                            dtAnalize.Rows.Add(dayData, aeWorkNoCount, dt1, tOut, p, n);
                                        }
                                    }
                                    //----- przerwa wliczona / czas niewliczony - ERROR
                                    if (przWlicz > 0)
                                    {
                                        DateTime dt2;
                                        if (p < 0)    // dt1 == tOut, nie ma nadgodzin, p to czas nadgodzin lub uzupełnienie do zmCzas (jak < 0)
                                        {
                                            p = -p;
                                            if (p > przWlicz) p = przWlicz;
                                            dt2 = dt1.AddSeconds(p);
                                            int? nn = null;
                                            if (przWliczNocne)
                                            {
                                                nn = Worktime.TimeInterSec(dt1, dt2, dtNocOd, dtNocDo);
                                                nocne += (int)nn;
                                            }
                                            dtAnalize.Rows.Add(dayData, aePrzerwaWliczona, dt1, dt2, p, nn);  // nie powinna miec miejsca w czasie nocnym zdaje sie dla matek karmiacych - ale nie daję błędu
                                            p = przWlicz - p;  // tyle zostanie
                                        }
                                        else
                                        {
                                            p = przWlicz;   // w p był czas nadgodzin wiec ustawiam zeby cala przerwa niewliczona mogla wejsc w nadgodziny
                                            dt2 = tOut;     // nie było przerwy wliczonej w czas pracy - wszystko jako przerwa wliczona w nadgodziny, czyli tOut (tOut = dt1 tu)
                                        }
                                        //----- przerwa wliczona - nadgodziny / czas niewliczony - ERROR
                                        if (p > 0)   // p - ile przerwy ma wejsc w nadgodziny
                                        {
                                            DateTime dt3 = dt2.AddSeconds(p);
                                            int n = Worktime.TimeInterSec(dt2, dt3, dtNocOd, dtNocDo);
                                            if (_fOver)
                                            {
                                                nadgDzien += p - n;
                                                nadgNoc += n;
                                                int? nn = null;
                                                if (przWliczNocne)
                                                {
                                                    nn = n;
                                                    nocne += n;
                                                }
                                                dtAnalize.Rows.Add(dayData, aePrzerwaWliczonaNadg, dt2, dt3, p, nn);
                                            }
                                            else
                                            {
                                                int? nn = null;
                                                if (przWliczNocne)
                                                {
                                                    nn = n;
                                                }
                                                dtAnalize.Rows.Add(dayData, aeBreakOverNoCount, dt2, dt3, p, nn);
                                                alert |= Worktime.alPrzWliczNoNadg;
                                            }
                                        }
                                    }


                                    //--------------------------------------
                                    /*
                                    _czasZm = zmCzas;
                                    DateTime tNadg = tIn.AddSeconds(zmCzas + przNiewlicz);                              // po tym czasie startują nadgodziny, nocne liczę 

                                    nocne = Worktime.TimeInterSec(tIn, tNadg, dtNocOd, dtNocDo);
                                    if (tIn != tNadg)   // zmiana N1 ma we=wy
                                        dtAnalize.Rows.Add(dayData, aeWork, tIn, tNadg, czas - przNiewlicz, nocne);

                                    if (przNiewlicz > 0)
                                    {
                                        int? nn = null;
                                        if (nocne > czas / 2)            // jezeli wiecej w nocy to odejmuje, czas zawiera przerwe niewliczona
                                        {
                                            nn = -przNiewlicz;
                                            nocne -= przNiewlicz;
                                            if (nocne < 0) nocne = 0;
                                        }
                                        dtAnalize.Rows.Add(dayData, aePrzerwaNiewliczona, null, null, -przNiewlicz, nn);   // -0:30 -0:30/null, nawet jak nie ma tyle czasu to pełna wartosc przerwy bo 
                                    }

                                    int p = czas - zmCzas - przNiewlicz;        // nadgodziny - tyle czasu zostało do przeliczenia
                                    if (p > 0)
                                    {
                                        int n = Worktime.TimeInterSec(tNadg, tOut, dtNocOd, dtNocDo);
                                        if (_fOver)
                                        {
                                            nocne += n;
                                            nadgDzien = p - n;
                                            nadgNoc = n;
                                            dtAnalize.Rows.Add(dayData, aeOver, tNadg, tOut, p, n);
                                        }
                                        else
                                        {
                                            dtAnalize.Rows.Add(dayData, aeWorkNoCount, tNadg, tOut, p, n);
                                        }
                                        p = 0;      // reszta 
                                    }

                                    if (przWlicz > 0)
                                    {
                                        DateTime tOut2 = tOut.AddSeconds(przWlicz);
                                        int n = Worktime.TimeInterSec(tOut, tOut2, dtNocOd, dtNocDo);
                                        if (_fOver)
                                        {
                                        }
                                        else
                                        {
                                        }
                                    }
                                    */
                                }
                            }
                        }
                    }
                }
            }
            else // brak zmiany
            {
                if (bWeWy || bSuma)   // na razie to samo ...
                {
                    int idx1 = Base.getInt(drRcpDay, "idx1", -1);
                    int idx2 = Base.getInt(drRcpDay, "idx2", -1);
                    if (idx1 >= 0 && idx2 >= 0 && idx2 >= idx1)  // wszystko ok, moze być 1 para dlatego >=
                    {
                        DataRow drD = InOutData.Rows[idx1];
                        DateTime? timeIn = Base.getDateTime(drD, "TimeIn");
                        DateTime? timeOut = Base.getDateTime(drD, "TimeOut");
                        DateTime tIn = timeIn != null ? (DateTime)timeIn : (DateTime)timeOut;      // uzupełniam brakujący istniejącym !!!

                        drD = InOutData.Rows[idx2];
                        timeIn = Base.getDateTime(drD, "TimeIn");
                        timeOut = Base.getDateTime(drD, "TimeOut");
                        DateTime tOut = timeOut != null ? (DateTime)timeOut : (DateTime)timeIn;

                        int n = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);
                        int c = Worktime.CountDateTimeSec(tIn, tOut);
                        dtAnalize.Rows.Add(dayData, aeWorkNoCount, tIn, tOut, c, n);
                    }
                }
                else if (bSuma)
                {

                }
                else if (bObecnosc)
                {

                }
            }


            //----- algorytm obecność 8h -----            
            if (algRCP == algObecnosc8h)   // "3"
            {
                int czas3 = Base.StrToIntDef(algPar, 8) * 3600;
                _czasZm = czas3;        // później zastanowić się nad tym jak liczyć czas3 > zmCzas --> nadgodziny ?
                przerwaZm = 0;
                przerwaZmNom = 0;
                nadgDzien = 0;
                nadgNoc = 0;
                przerwaNadg = 0;
                przerwaNadgNom = 0;
                if (_zmCzas >= 0)        // jest zmiana: czas zmiany, -1 brak; jak jest zmiana - nocne biorę wg zmiany
                {
                    //DateTime tIn = dayData.Add(((DateTime)Base.getDateTime(drDay, "ZmianaOd")).TimeOfDay);
                    //DateTime tOut = dayData.Add(((DateTime)Base.getDateTime(drDay, "ZmianaDo")).TimeOfDay);
                    //if (tIn > tOut) tOut = tOut.AddDays(1);
                    if (czas3 > _zmCzas)                                 // parametr był > 8h (czasu zmiany)
                    {
                        _czasZm = _zmCzas;
                        DateTime tOut3 = zmTimeIn.AddSeconds(czas3);    // tIn -czas_na_zmianie- tOut -nadgodziny- tOut3

                        int noc = Worktime.TimeInterSec(zmTimeIn, zmTimeOut, dtNocOd, dtNocDo);
                        dtAnalize.Rows.Add(dayData, aeWork, zmTimeIn, zmTimeOut, _zmCzas, noc);

                        if (Worktime.zmNadgodziny(drDay))               // jak zmiana nie ma nadgodzin to nie liczę
                        {
                            int n = czas3 - _zmCzas;                     // łaczna ilość nadgodzin
                            nadgNoc = Worktime.TimeInterSec(zmTimeOut, tOut3, dtNocOd, dtNocDo);         // nie trzeba zaokrąglać bo tu wyjdą wartości co do minut
                            nadgDzien = n - nadgNoc;

                            dtAnalize.Rows.Add(dayData, aeOver, zmTimeOut, tOut3, n, nadgNoc);
                        }
                        nocne = Worktime.TimeInterSec(zmTimeIn, tOut3, dtNocOd, dtNocDo);
                    }
                    else if (czas3 < _zmCzas)                            // czas alg < czas zmiany; jak = to nic nie trzeba robić
                    {
                        zmTimeOut = zmTimeIn.AddSeconds(czas3);         // równam czas
                        //alert |= Worktime.alNoNominal;                  // to zdaje się jest też wyżej sprawdzane, ale jak juz jest niech będzie; operacje są na wartościach "dużych" - godzinowych, nie ma sensu zaokrąglać <<< 20131111 nie ma sensu zgłaszać alertu
                        nocne = Worktime.TimeInterSec(zmTimeIn, zmTimeOut, dtNocOd, dtNocDo);
                        dtAnalize.Rows.Add(dayData, aeWork, zmTimeIn, zmTimeOut, _zmCzas, nocne);
                    }
                    else
                    {
                        nocne = Worktime.TimeInterSec(zmTimeIn, zmTimeOut, dtNocOd, dtNocDo);
                        dtAnalize.Rows.Add(dayData, aeWork, zmTimeIn, zmTimeOut, _zmCzas, nocne);
                    }
                }
                else                    // brak zmiany <<< nie trzeba dodawać do dtAnalize
                {
                    DateTime? tIn = Base.getDateTime(drDay, "TimeIn");
                    DateTime? tOut = Base.getDateTime(drDay, "TimeOut");
                    if (tIn == null)
                        tIn = ((DateTime)tOut).AddHours(-czas3);
                    else
                        tOut = ((DateTime)tIn).AddHours(czas3);
                    nocne = Worktime.TimeInterSec(tIn, tOut, dtNocOd, dtNocDo);     // od we(+) albo od wy(-) 8h 
                }
            }

            //----- analiza rcp -----
            int idxA2 = dtAnalize.Rows.Count - 1;
            if (idxA1 <= idxA2)
            {
                drRcpDay["idx1rcp"] = idxA1;
                drRcpDay["idx2rcp"] = idxA2;
            }
            //----- podstawienie wartości ------
            drDay["czasZm2"] = _czasZm;
            drDay["przerwaZm2"] = przerwaZm;
            drDay["przerwaZm2nom"] = przerwaZmNom;
            drDay["nadgDzien2"] = nadgDzien;
            drDay["nadgNoc2"] = nadgNoc;
            drDay["przerwaN2"] = przerwaNadg;
            drDay["przerwaN2nom"] = przerwaNadgNom;
            drDay["nocne2"] = nocne;

            //alert |= Worktime.alTester;
        }






















        public void _CountWT2AndMerge(DataSet dsDays,                        // wszystkie dni w okresie, z info o zmianie i absencjach, z GetWorktime
                                        int breakTimeZm, int breakTimeN,    // [min], tak jak w konfiguracji
                                        int zaokr, int zaokrType)
        {
            DataRowCollection drcDays = Base.getRows(dsDays);
            if (drcDays.Count > 0)
            {
                DateTime firstDay = (DateTime)Base.getDateTime(drcDays[0], "Data");                 // musi być
                DateTime lastDay = (DateTime)Base.getDateTime(drcDays[drcDays.Count - 1], "Data");  // musi być
                int btZm = breakTimeZm * 60;
                int btN = breakTimeN * 60;


                /*
                 20150113 pojawił się taki problem ze jak danych jest za duzo bo sie np pojawily zduplikowane absencje to nie podpian poprawnie dni !!!
                 * trzeba to przerobić tak zeby sprawdzał daty !!!
                 
                 
                 */




                foreach (DataRow drRcpDay in DayData.Rows)                 // z rcp/Data
                {
                    DateTime data = (DateTime)Base.getDateTime(drRcpDay, "Data");                      // musi być
                    int idx = Convert.ToInt32(data.Subtract(firstDay).TotalDays);                   // index jest po to zeby zsynchronizować dane ???
                    if (0 <= idx && idx < drcDays.Count)
                    {
                        DataRow drD = drcDays[idx];                     // z dsDays
                        drD["TimeIn"] = drRcpDay["TimeIn"];
                        drD["TimeOut"] = drRcpDay["TimeOut"];

                        object c1 = drRcpDay["Czas1"];
                        drD["Czas1sec"] = c1;
                        if (!Base.isNull(c1))
                        {
                            drD["Czas1"] = Worktime.SecToTime((int)c1, 0);
                            drD["Czas1R"] = Worktime.SecToTimePP((int)c1, zaokr, zaokrType, false);
                        }

                        object c2 = drRcpDay["Czas2"];
                        drD["Czas2sec"] = c2;
                        if (!Base.isNull(c2))
                        {
                            drD["Czas2"] = Worktime.SecToTime((int)c2, 0);
                            drD["Czas2R"] = Worktime.SecToTimePP((int)c2, zaokr, zaokrType, false);
                        }

                        drD["Nocne1sec"] = drRcpDay["Noc1"];
                        drD["Nocne2sec"] = drRcpDay["Noc2"];    // przy liczeniu WT2a noc licznona będzie w CountWT2 
                        drD["ponocy"] = drRcpDay["ponocy"];
                        drD["ponocy2"] = drRcpDay["ponocy2"];   // przy liczeniu WT2a ponocy są zbędne 

                        int alert = Base.getInt(drRcpDay, "Alert", 0);
                        _CountWT2(drD, drRcpDay, btZm, btN, ref alert);

                        drD["Alert"] = alert;
                    }
                    else if (idx > 0) break;                // na jedno wyjdzie czy >= drc.Count
                }
            }
        }

        //----- WT2 --------------------------------------------
        public void MergeWorktime(DataSet dsDays, int zaokr, int zaokrType)    // scala z danymi dni - zmiany itd
        {
            if (Data != null)
            {
                DataRowCollection drcDays = Base.getRows(dsDays);
                if (drcDays.Count > 0)
                {
                    DateTime firstDay = (DateTime)Base.getDateTime(drcDays[0], "Data");                 // musi być
                    DateTime lastDay = (DateTime)Base.getDateTime(drcDays[drcDays.Count - 1], "Data");  // musi być
                    foreach (DataRow drDay in DayData.Rows)
                    {
                        DateTime data = (DateTime)Base.getDateTime(drDay, "Data");                      // musi być
                        int idx = Convert.ToInt32(data.Subtract(firstDay).TotalDays);
                        if (0 <= idx && idx < drcDays.Count)
                        {
                            DataRow drD = drcDays[idx];
                            drD["TimeIn"] = drDay["TimeIn"];
                            drD["TimeOut"] = drDay["TimeOut"];

                            object c1 = drDay["Czas1"];
                            drD["Czas1sec"] = c1;
                            if (!Base.isNull(c1))
                            {
                                drD["Czas1"] = Worktime.SecToTime((int)c1, 0);
                                drD["Czas1R"] = Worktime.SecToTimePP((int)c1, zaokr, zaokrType, false);
                            }

                            object c2 = drDay["Czas2"];
                            drD["Czas2sec"] = c2;
                            if (!Base.isNull(c2))
                            {
                                drD["Czas2"] = Worktime.SecToTime((int)c2, 0);
                                drD["Czas2R"] = Worktime.SecToTimePP((int)c2, zaokr, zaokrType, false);
                            }

                            drD["Nocne1sec"] = drDay["Noc1"];
                            drD["Nocne2sec"] = drDay["Noc2"];
                            drD["ponocy"] = drDay["ponocy"];
                            drD["ponocy2"] = drDay["ponocy2"];

                            drD["Alert"] = drDay["Alert"];
                        }
                        else if (idx > 0) break;                // na jedno wyjdzie czy >= drc.Count
                    }
                }
            }
        }


























        /*
        private void PrepareWorkime4(DataSet dsRCP)    // zbija w pary in-out, dodaje do dsRCP.Tables[1]
        {
            DataTable dtPairs = dsRCP.Tables.Add();
            dtPairs.Columns.Add("TimeIn", typeof(DateTime));
            dtPairs.Columns.Add("TimeOut", typeof(DateTime));
            dtPairs.Columns.Add("before", typeof(int));
            dtPairs.Columns.Add("czas", typeof(int));
            DataRow drPair = null;
            
            int before = 86400;
            
            DateTime prevCzas = DateTime.MinValue;
            int prevReaderId = -1;
            bool prevIsOut = false;
            
            int step = 0;
            foreach (DataRow dr in dsRCP.Tables[0].Rows)
            {
                bool isIn = Base.getBool(dr, "InOut2", false);              // zawsze jest
                DateTime czas = (DateTime)Base.getDateTime(dr, "Czas");     // musi być
                int readerId = (int)Base.getInt(dr, "ECReaderId");          // musi być
            
                if (czas != prevCzas && prevReaderId != readerId)           // pomijam powtórzone odczyty, nie sprawdzam juz in/out
                {
                    if (prevReaderId == -1)
                    {
                        prevCzas = czas.AddDays(-1);                        // jakaś wartośc początkowa zeby zalapal 1 poprawny odczyt
                        prevIsOut = !isIn;
                    }

                    switch (step)
                    {
                        default:
                        case 0:     // start, zobaczę co przyjdzie
                            if (isIn)
                            {
                                before = prevIsOut ? CountDateTimeSec(prevCzas, czas) : 0;
                                drPair = dtPairs.Rows.Add(czas, null, before, 0);
                                step = 1;   // czekam na out
                            }
                            else
                            {
                                drPair = dtPairs.Rows.Add(null, czas, 0, 0);    // dalej czekam co przyjdzie
                            }
                            break;
                        case 1:     // czekam na out 
                            if (isIn)
                            {
                                drPair = dtPairs.Rows.Add(czas, null, 0, 0);  // ale przyszło in więc następny dodaję
                            }
                            else
                            {   // spr jeszcze różnicę TimeOut - TimeIn!!!
                                drPair["TimeOut"] = czas;               // ok, uzupełniam
                                drPair["czas"] = CountDateTimeSec(prevCzas, czas);
                                step = 0;   // zobaczę co przyjdzie
                            }
                            break;
                    }

                    prevIsOut = !isIn;
                    prevCzas = czas;
                }
            }
        }

        private void PrepareWorkime4days(DataSet dsRCP)      // zbiera dni -> Tables[2], dodaje alerty                
        {
            DataTable dtDays = dsRCP.Tables.Add();
            dtDays.Columns.Add("TimeIn", typeof(DateTime));
            dtDays.Columns.Add("TimeOut", typeof(DateTime));
            dtDays.Columns.Add("Czas1sec", typeof(int));
            dtDays.Columns.Add("Czas2sec", typeof(int));
            dtDays.Columns.Add("Alert", typeof(int));
            DataRow drDay = null;
            int step = 0;
            foreach (DataRow dr in dsRCP.Tables[0].Rows)
            {
                bool isIn = Base.getBool(dr, "InOut2", false);              // zawsze jest
                DateTime czas = (DateTime)Base.getDateTime(dr, "Czas");     // musi być
            }
        }
        */


        //-------------------------------------        
        //public DataSet GetRcpAnalize(int? idx1, int? idx2) // podgląd analizy RCP
        public DataSet GetRcpAnalize() // podgląd analizy RCP, przepakowuję do nowej tabeli -> docelowo dać pola w tab źródłowej
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("Typ", typeof(int));
            dt.Columns.Add("Nazwa", typeof(string));
            dt.Columns.Add("TimeOd", typeof(DateTime));
            dt.Columns.Add("TimeDo", typeof(DateTime));
            dt.Columns.Add("CzasSec", typeof(int));
            dt.Columns.Add("NocneSec", typeof(int));
            dt.Columns.Add("Czas", typeof(string));
            dt.Columns.Add("Nocne", typeof(string));

            /*
            if (!Base.isNull(idx1) && !Base.isNull(idx2))
            {
                int i1 = (int)idx1;
                int i2 = (int)idx2;
            */
            if (AnalizeData != null)
            {
                int i1 = 0;
                int i2 = AnalizeData.Rows.Count - 1;

                for (int i = i1; i <= i2; i++)
                {
                    DataRow dr = AnalizeData.Rows[i];
                    int typ = Base.getInt(dr, "Typ", -1);
                    string nazwa = AnalizeEntryStr(typ);
                    DateTime? dtOd = Base.getDateTime(dr, "TimeOd");
                    DateTime? dtDo = Base.getDateTime(dr, "TimeDo");
                    //bool ok = !Base.isNull(dtOd) && !Base.isNull(dtDo);
                    bool ok1 = !Base.isNull(dtOd);
                    bool ok2 = !Base.isNull(dtDo);
                    int czas = Base.getInt(dr, "Czas", 0);
                    int nocne = Base.getInt(dr, "Nocne", 0);
                    string tt, nn;
                    if (ok1 && ok2 || !ok1 && !ok2)
                    {
                        tt = Worktime.SecToTime(czas, 0);
                        nn = nocne != 0 ? Worktime.SecToTime(nocne, 0) : null;
                        switch (typ)
                        {
                            case aePrzerwaWliczona:
                            case aePrzerwaWliczonaNadg:
                                //tt = "+" + tt;
                                //if (nocne != 0) nn = "+" + nn;
                                break;
                            case aePrzerwaNiewliczona:
                                if (czas > 0) tt = "-" + tt;
                                if (nocne > 0) nn = "-" + nn;
                                break;
                        }
                    }
                    else
                    {
                        tt = "0";
                        nn = null;
                    }

                    dt.Rows.Add(typ, nazwa, dtOd, dtDo,
                        czas, nocne,
                        tt, nn
                        //ok1 && ok2 || !ok1 && !ok2 ? Worktime.SecToTime(czas, 0) : "0",   //null,      // bez zaokrąglania
                        //ok ? nocne > 0 ? Worktime.SecToTime(nocne, 0) : null : "0");      // bez zaokrąglania, warunek zagnieżdżony !
                        //nocne > 0 ? Worktime.SecToTime(nocne, 0) : null
                        );                   // bez zaokrąglania tak lepiej 
                }
                return ds;
            }
            else return null;
        }

        //-------------------------------------
        public DataSet GetDetails(int? idx1, int? idx2, int round, int rtype, out DateTime firstDate, out DateTime lastDate) // podgląd RCP: czasy składowe in-out
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("TimeIn", typeof(DateTime));
            dt.Columns.Add("TimeOut", typeof(DateTime));
            dt.Columns.Add("Czas", typeof(string));

            firstDate = DateTime.MinValue;
            lastDate = DateTime.MaxValue;

            if (!Base.isNull(idx1) && !Base.isNull(idx2))
            {
                int i1 = (int)idx1;
                int i2 = (int)idx2;

                for (int i = i1; i <= i2; i++)
                {
                    DataRow dr = InOutData.Rows[i];
                    int? c = Base.getInt(dr, "czas");
                    //----- first last -----
                    DateTime? dIn = Base.getDateTime(dr, "TimeIn");
                    DateTime? dOut = Base.getDateTime(dr, "TimeOut");
                    bool isIn = !Base.isNull(dIn);
                    bool isOut = !Base.isNull(dOut);
                    if (firstDate == DateTime.MinValue)
                        if (isIn) firstDate = (DateTime)dIn;
                        else if (isOut) firstDate = (DateTime)dOut; // nadmiarowy warunek ale na wszelki wypadek; pierwszy in albo out
                    if (isOut) lastDate = (DateTime)dOut;
                    else if (isIn) lastDate = (DateTime)dIn;

                    dt.Rows.Add(dIn, dOut, Base.isNull(c) ? null : Worktime.SecToTime((int)c, 0));  // bez zaokrąglania
                    //dt.Rows.Add(dr["TimeIn"], dr["TimeOut"], c == null ? null : Worktime.SecToTime((int)c, 0));  // bez zaokrąglania
                }
                return ds;
            }
            else return null;

            /*            
            return Base.getDataSet(con,
                "select TimeIn, TimeOut," +
                    "dbo.ToTime(worktime) as Czas," +
                    "dbo.RoundTime(worktime, " + round + "," + rtype + ") as CzasR," +
                    "worktime " +
                "from tmpRCP2 " +
                "where sesId = " + FSqlSesId + " and " + czas +
                " order by TimeIn");
             */
        }

        public DataSet x_GetDetails(string dateFrom, string dateTo, int round, int rtype) // podgląd RCP: czasy składowe in-out
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("TimeIn", typeof(DateTime));
            dt.Columns.Add("TimeOut", typeof(DateTime));
            dt.Columns.Add("Czas", typeof(string));

            string czas;
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (!fromIsNull && !toIsNull)
                czas = "TimeIn >= " + Base.strParam(dateFrom) + " and TimeOut <= " + Base.strParam(dateTo);
            else if (!fromIsNull)
            {
                DateTime dtTo = DateTime.Parse(dateFrom).AddDays(1);
                czas = "TimeIn >= " + Base.strParam(dateFrom) + " and TimeOut <= " + Base.strParam(Base.DateTimeToStr(dtTo));
            }
            else if (!toIsNull)
            {
                DateTime dtFrom = DateTime.Parse(dateTo).AddDays(-1);
                czas = "TimeIn >= " + Base.strParam(Base.DateTimeToStr(dtFrom)) + " and TimeOut <= " + Base.strParam(dateTo);
            }
            else
                return null;

            foreach (DataRow dr in InOutData.Select(czas))
            {
                int? c = Base.getInt(dr, "czas");
                dt.Rows.Add(dr["TimeIn"], dr["TimeOut"], c == null ? null : Worktime.SecToTime((int)c, 0));  // bez zaokrąglania
            }
            return ds;
            /*            
            return Base.getDataSet(con,
                "select TimeIn, TimeOut," +
                    "dbo.ToTime(worktime) as Czas," +
                    "dbo.RoundTime(worktime, " + round + "," + rtype + ") as CzasR," +
                    "worktime " +
                "from tmpRCP2 " +
                "where sesId = " + FSqlSesId + " and " + czas +
                " order by TimeIn");
             */
        }


        public DataSet GetDetails2(DateTime? data)   // podgląd RCP: dane z rejestratorów w strefie
        {
            if (data != null)
            {
                DataSet ds = new DataSet();
                DataTable dt = ds.Tables.Add();
                dt.Columns.Add("Czas", typeof(string));
                dt.Columns.Add("ECReaderId", typeof(int));
                dt.Columns.Add("InOut2", typeof(bool));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Day", typeof(bool));

                string czas = "Data = '" + Base.DateToStr((DateTime)data) + "'";
                //----- zebranie danych z rejestratorów RCP i unikalne Id w podanym okresie -----
                string rIds = ",";
                foreach (DataRow dr in ReadersData.Select(czas))
                {
                    int rid = (int)Base.getInt(dr, "ECReaderId");  // musi być
                    string srid = rid.ToString() + ",";
                    if (!rIds.Contains("," + srid))
                        rIds += srid;
                    dt.Rows.Add(dr["Czas"], rid, dr["InOut2"], null);
                }
                //----- pobranie nazw rejestratorów ------
                string where = rIds.Length > 2 ? " where Id in (" + rIds.Substring(1, rIds.Length - 2) + ")" : null;
                DataSet dsNazwy = db.getDataSet("select Id, Name from Readers" + where + " order by Id");  // order w sumie niepotrzebny przy wyszukiwaniu kolejnym, ale nie zaszkodzi 
                //----- powiąznie nazw rejestratorów z odczytami ------
                foreach (DataRow dr in dt.Rows)
                {
                    int rid = (int)Base.getInt(dr, "ECReaderId");  // musi być
                    foreach (DataRow drN in Base.getRows(dsNazwy))
                    {
                        int rid2 = (int)Base.getInt(drN, "Id");  // musi być 
                        if (rid == rid2)
                        {
                            dr["Name"] = drN["Name"];
                            break;
                        }
                    }
                    dr["Day"] = true;
                }
                return ds;
            }
            else return null;
        }

        /*
        public DataSet x_GetDetails2(string dateFrom, string dateTo)   // podgląd RCP: dane z rejestratorów w strefie
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            dt.Columns.Add("Czas", typeof(string));
            dt.Columns.Add("ECReaderId", typeof(int));
            dt.Columns.Add("InOut2", typeof(bool));
            dt.Columns.Add("Name", typeof(string));

            string czas;
            bool fromIsNull = String.IsNullOrEmpty(dateFrom);
            bool toIsNull = String.IsNullOrEmpty(dateTo);
            if (!fromIsNull && !toIsNull)
                czas = "Czas >= " + Base.strParam(dateFrom) + " and Czas <= " + Base.strParam(dateTo);
            else if (!fromIsNull)
            {
                DateTime dtTo = DateTime.Parse(dateFrom).AddDays(1);
                czas = "Czas >= " + Base.strParam(dateFrom) + " and Czas <= " + Base.strParam(Base.DateTimeToStr(dtTo));
            }
            else if (!toIsNull)
            {
                DateTime dtFrom = DateTime.Parse(dateTo).AddDays(-1);
                czas = "Czas >= " + Base.strParam(Base.DateTimeToStr(dtFrom)) + " and Czas <= " + Base.strParam(dateTo);
            }
            else
                return null;
            //----- zebranie danych z rejestratorów RCP i unikalne Id w podanym okresie -----
            string rIds = ",";
            foreach (DataRow dr in ReadersData.Select(czas))
            {
                int rid = (int)Base.getInt(dr, "ECReaderId");  // musi być
                string srid = rid.ToString() + ",";
                if (!rIds.Contains("," + srid))
                    rIds += srid;
                dt.Rows.Add(dr["Czas"], rid, dr["InOut2"], null);
            }
            //----- pobranie nazw rejestratorów ------
            string where = rIds.Length > 2 ? " where Id in (" + rIds.Substring(1, rIds.Length - 2) + ")" : null;
            DataSet dsNazwy = Base.getDataSet(con, "select Id, Name from Readers" + where + " order by Id");  // order w sumie niepotrzebny przy wyszukiwaniu kolejnym, ale nie zaszkodzi 
            //----- powiąznie nazw rejestratorów z odczytami ------
            foreach (DataRow dr in dt.Rows)
            {
                int rid = (int)Base.getInt(dr, "ECReaderId");  // musi być
                foreach (DataRow drN in Base.getRows(dsNazwy))
                {
                    int rid2 = (int)Base.getInt(drN, "Id");  // musi być 
                    if (rid == rid2)
                    {
                        dr["Name"] = drN["Name"];
                        break;
                    }
                }
            }
            return ds;
            /*
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
             * /
        }
        */

        public DataSet GetDetails3(string dateFrom, string dateTo, DateTime dayFrom, DateTime dayTo)   // podgląd RCP: dane ze wszystkich rejestratorów 
        {
            /* 20131111
            string czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas < " + Base.strParam(dateTo);
            DataSet ds = db.getDataSet(String.Format(
                "select A.Czas, A.ECReaderId, R.Name, A.InOut, A.ECUniqueId " +
                "from RCP A " +
                "left outer join Readers R on R.Id = A.ECReaderId " +
                "where A.ECUserId = " + FRcpId + " and " + czas +
                " order by A.Czas");
            */

            DataSet ds = db.getDataSet(String.Format(@"
declare 
	@czasOd datetime, 
	@czasDo datetime, 
	@pracId int
set @czasOd = '{1}'
set @czasDo = '{2}'
set @pracId = {0}

select A.Czas, A.ECReaderId, R.Name, A.InOut, A.ECUniqueId 
from PracownicyKarty PK 
left outer join RCP A on A.ECUserId = PK.RcpId
	and A.Czas >= dbo.MaxDate2(@czasOd, PK.Od)
	and A.Czas <= dbo.MinDate2(@czasDo, DATEADD(DAY, 1, PK.Do)) 
left outer join Readers R on R.Id = A.ECReaderId 
where PK.IdPracownika = @pracId 
	and ISNULL(PK.Do, '20990909') >= @czasOd
	and PK.Od <= @czasDo
	and A.ECUniqueId is not null    
order by A.Czas
                ", FPracId, dateFrom, dateTo));
            //and A.ECUniqueId is not null - mogło nie byc odbić


            DataTable dt = ds.Tables[0];
            dt.Columns.Add("InOut2", typeof(bool));
            dt.Columns.Add("Day", typeof(bool));

            foreach (DataRow dr in Base.getRows(ds))
            {
                int uid = (int)Base.getInt(dr, "ECUniqueId");           // musi być
                DateTime c = (DateTime)Base.getDateTime(dr, "Czas");    // musi być 
                dr["Day"] = dayFrom <= c && c <= dayTo;                 // wszystko pomiedzy do zaznaczenia
                foreach (DataRow drR in ReadersData.Rows)
                {
                    int uidR = (int)Base.getInt(drR, "ECUniqueId");     // musi być
                    if (uid == uidR)
                    {
                        dr["InOut2"] = drR["InOut2"];
                        break;
                    }
                }
            }

            return ds;
            /*
            string czas = "A.Czas >= " + Base.strParam(dateFrom) + " and A.Czas < " + Base.strParam(dateTo);
            return Base.getDataSet(con,
                "select A.Czas, A.ECReaderId, B.InOut2, R.Name, A.InOut " +
                "from RCP A " +
                "left outer join tmpRCP1 B on sesId = " + FSqlSesId + " and B.ECUniqueId = A.ECUniqueId " +
                "left outer join Readers R on R.Id = A.ECReaderId " +
                "where A.ECUserId = " + FRcpId + " and " + czas +
                " order by A.Czas");
             */
        }






















        public void SumTime(out string sum, out string sumR, out string sum2, out string sum2R, int dayRound, int drType, int sumRound, int srType)
        {
            sum = "00:00:00";
            sumR = "0";
            sum2 = "00:00:00";
            sum2R = "0";
            DateTime dtFrom;
            DateTime dtTo;
            if (DateTime.TryParse(FDateFrom, out dtFrom) && DateTime.TryParse(FDateTo, out dtTo))
            {
                int s1 = 0;
                int s2 = 0;
                if (DayData != null)
                {
                    foreach (DataRow dr in DayData.Rows)
                    {
                        DateTime data = (DateTime)Base.getDateTime(dr, "Data");  // musi być
                        if (dtFrom <= data && data <= dtTo)
                        {
                            int? c1 = Base.getInt(dr, "Czas1");
                            int? c2 = Base.getInt(dr, "Czas2");
                            if (c1 != null) s1 += Worktime.RoundSec((int)c1, dayRound, drType);
                            if (c2 != null) s2 += Worktime.RoundSec((int)c2, dayRound, drType);
                        }
                    }
                    if (s1 != 0)
                    {
                        sum = Worktime.SecToTime(s1, 0);
                        sumR = Worktime.SecToTimePP(s1, sumRound, srType, false);
                    }
                    if (s2 != 0)
                    {
                        sum2 = Worktime.SecToTime(s2, 0);
                        sum2R = Worktime.SecToTimePP(s2, sumRound, srType, false);
                    }
                }
            }
            /*
            DataRow dr = Base.getDataRow(con,
                "select dbo.ToTime(sum(dbo.RoundSec(worktime," + dayRound + "," + drType + "))) as Suma," +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime," + dayRound + "," + drType + "))," + sumRound + "," + srType + ") as SumaR," +
                       "dbo.ToTime(sum(dbo.RoundSec(worktime2," + dayRound + "," + drType + "))) as Suma2, " +
                       "dbo.RoundTime(sum(dbo.RoundSec(worktime2," + dayRound + "," + drType + "))," + sumRound + "," + srType + ") as Suma2R " +
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
             */
        }


























        //--------------------------------------------
        // karta roczna
        public static DataSet GetWorktime1(SqlConnection con, string pracId, string rcpId,   // rcpId == null => bierze tylko PP, bez rcp
                    string fromTime, string toTime, string onDay,
                    string strefaId,
                    int zaokr, int zaokrType,
                    int nocneOdSec, int nocneDoSec,
                    int breakTimeZm, int breakTimeN,    // [min], tak jak w konfiguracji; później moze dać do danych jak będzie pod innym kierownikiem, ale to nie stanowi na tym etapie
                    bool kierAbsencja)                  // jak nie ma absencji z KP to weź wpisaną przez kierownika
        {
            Worktime2 wt;
            return _GetWorktime(pracId, null, null, rcpId, true,
                    fromTime, toTime, onDay,
                    strefaId,
                    zaokr, zaokrType,
                    nocneOdSec, nocneDoSec,
                    breakTimeZm, breakTimeN,
                    null,
                    0, /* wymiar */
                    kierAbsencja,
                    out wt);
        }

        public static DataSet _GetWorktime(
                    string pracId,

                    string _algRCP, string _algPar,
                    string _rcpId,   // rcpId == null => bierze tylko PP, bez rcp
                    bool withRCP,

                    string fromTime, string toTime, string onDay,

                    string _strefaId,

                    int zaokr, int zaokrType,
                    int nocneOdSec, int nocneDoSec,
                    int breakTimeZm, int breakTimeN,    // [min], tak jak w konfiguracji; później moze dać do danych jak będzie pod innym kierownikiem, ale to nie stanowi na tym etapie
                    string zmKorektaId,                 // korekta zmiany w panelu akceptacji 
                    int zmKorektaWymiar,
                    bool kierAbsencja,                  // jak nie ma absencji z KP to weź wpisaną przez kierownika
                    out Worktime2 wt)
        {
            //return GetWorktime(con, pracId, null, null, rcpId,
            return GetWorktime(pracId,

                    _algRCP, _algPar, _rcpId,  //20131021
                    withRCP,

                    fromTime, toTime, onDay,

                    _strefaId,

                    zaokr, zaokrType,
                    nocneOdSec, nocneDoSec,
                    breakTimeZm, breakTimeN,
                    zmKorektaId,
                    zmKorektaWymiar,
                    kierAbsencja,
                    null, null, false,
                    out wt);
        }









        public const String zmRapRCP = "RR";









        // spr wywołania pod kątem kierAbsencja - bo powinno być zawsze true - pokazuj z korektą, - gdzie jest z false?
        // spr zmKorektaDlaPrac - do czego to 
        public static DataSet GetWorktime(//SqlConnection con, 
                    string pracId,

                    string x_algRCP, string x_algPar,
                    string x_rcpId,   // rcpId == null => bierze tylko PP, bez rcp

                    bool withRCP,

                    string fromTime, string toTime, string _onDay,

                    string x_strefaId,

                    int zaokr, int zaokrType,
                    int nocneOdSec, int nocneDoSec,
                    int breakTimeZm, int breakTimeN,    // [min], tak jak w konfiguracji; później moze dać do danych jak będzie pod innym kierownikiem, ale to nie stanowi na tym etapie

                    string _zmKorektaId,                 // korekta zmiany w panelu akceptacji - podczas edycji, null jeżeli z PP
                    int zmKorektaWymiar,

                    bool kierAbsencja,                  // jak nie ma absencji z KP to weź wpisaną przez kierownika

                    string addSelect, string addFrom,
                    bool zmKorektaDlaPrac,              // korekta tylko dla dni pracujących (wolne są liczone wg zmian tam wstawionych), w addSelect musi być powiązanie do Kalendarza >>> później to wyczyścić ...
                    out Worktime2 wt)
        {
            string dFrom = String.IsNullOrEmpty(_onDay) ? fromTime : _onDay;
            string dTo = String.IsNullOrEmpty(_onDay) ? toTime : _onDay;

            /*
            if (pracId == "249")
            {
                int x = 0;
            }
            */

            String zmKorektaWymiarStr = zmKorektaWymiar.ToString();

            if (_zmKorektaId != zmRapRCP)
            {
                if (_zmKorektaId == "P") zmKorektaWymiarStr = "P.Wymiar";
                else if (String.IsNullOrEmpty(_zmKorektaId)) zmKorektaWymiarStr = "ISNULL(P.WymiarKorekta, P.Wymiar)";
                else if (zmKorektaDlaPrac) zmKorektaWymiarStr = String.Format("case when K.Rodzaj is null then {0} else ISNULL(P.WymiarKorekta, P.Wymiar) end", zmKorektaWymiar);

                if (_zmKorektaId == "P") _zmKorektaId = "P.IdZmiany";    //20131111
                else if (String.IsNullOrEmpty(_zmKorektaId)) _zmKorektaId = "ISNULL(P.IdZmianyKorekta, P.IdZmiany)";
                else if (zmKorektaDlaPrac) _zmKorektaId = String.Format("case when K.Rodzaj is null then {0} else ISNULL(P.IdZmianyKorekta, P.IdZmiany) end", _zmKorektaId);
            }

            DataSet ds = Base.getDataSet(String.Format(
                //"select D.Lp, D.Data, " +
                "select D.Data, " +
                    "P.Id as PPId, " +
                    "P.IdZmiany, " +
                    "P.IdZmianyKorekta, " +
                    "P.Czas," +
                    "P.CzasIn, P.CzasOut, P.CzasZm, P.NadgodzinyDzien, P.NadgodzinyNoc, P.Nocne, " +
                    "P.k_CzasIn, P.k_CzasOut, P.k_CzasZm, P.k_NadgodzinyDzien, P.k_NadgodzinyNoc, P.k_Nocne, " +
                    "P.Uwagi, " +
                    "P.DataAcc, P.IdKierownikaAcc, P.Akceptacja, P.Alerty, " +
                    "P.n50, P.n100, " +

                    "Z.Id as ZmianaId, " +
                    "Z.Symbol, " +
                    "Z.Kolor, " +
                    "Z.Od as ZmianaOd, " +
                    "DATEADD(SECOND, {7}, z.Od) ZmianaDo," +
                /*
                "Z.Do as ZmianaDo, " +
                */

                    "P.Wymiar, P.WymiarKorekta, " +

                    "{7} WymiarWlasciwy, " +

                    "Z.TypZmiany, Z.Nadgodziny, Z.ZgodaNadg, Z.HideZgoda, Z.Margines, Z.Typ, Z.ObetnijOdGory, Z.MarginesNadgodzin, Z.Par1, Z.Par2, " +

                    "PP.RcpAlgorytm, PP.WymiarCzasu, PP.PrzerwaWliczona, PP.PrzerwaNiewliczona, R.RcpStrefaId, " +

                    "A.Kod as AbsencjaKod, " +

                    "A.Id AbsencjaId, A.IleDni AbsencjaDni, A.Godzin AbsencjaGodzin, A.DataOd AbsencjaOd, A.DataDo AbsencjaDo, " +

                    "P.Absencja as AbsencjaKodKier, " +
                //"ISNULL(WT.IdKodyAbs, P.Absencja) as AbsencjaKodKier, " +
                    "WT.IdKodyAbs as AbsencjaKodWniosek, " +


                    "AK.Symbol as AbsencjaSymbol, " +
                    "AK.Nazwa as AbsencjaNazwa, " +
                    "AK.GodzinPracy, AK.DniWolne, " +

                    "R.IdKierownika as PrzKierId, R.Do as PrzDo, R.DoMonit, R.Id as PrzId " +

                    "{5} " +        // addSelect
                "from GetDates2('{1}','{2}') D " +
                    "{6} " +        // addFrom
                    "left outer join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +

                    /*20161116*/
                    ((_zmKorektaId != zmRapRCP) ? "left outer join Zmiany Z on Z.Id = {3} "
                    : @"outer apply (select
  0 Id, 'aoe' Symbol, 'aoe' Nazwa, CONVERT(datetime, '20010101 08:00') Od, CONVERT(datetime, '20010101 16:00') Do
, 100 Stawka, 1 Visible, NULL Ikona, NULL Kolor, 1 TypZmiany, NULL Nadgodziny, 150 NadgodzinyDzien, 200 NadgodzinyNoc, -1  Margines, 1 ZgodaNadg, 0 Kolejnosc
, 0 NowaLinia, 1 Widoczna, 1 HideZgoda, 0 Typ, 0 ObetnijOdGory, 0 MarginesNadgodzin, NULL Par1, NULL Par2) Z ") +

                //"left outer join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo " +
                    "outer apply (select top 1 * from Absencja where IdPracownika = {0} and D.Data between DataOd and DataDo) A " +

                    //"left join poWnioskiUrlopowe WU on WU.IdPracownika = {0} and D.Data between WU.Od and WU.do and WU.StatusId in (3,4) " +  // czeka, zaakceptowany, wprowadzony
                    "outer apply (select top 1 * from poWnioskiUrlopowe where IdPracownika = {0} and D.Data between Od and Do and StatusId in (3,4)) WU " +
                    "left join poWnioskiUrlopoweTypy WT on WT.Id = WU.TypId " +

                    "left outer join AbsencjaKody AK on AK.Kod = {4} " +

                    //"left outer join PracownicyParametry PP on PP.IdPracownika = {0} and D.Data between PP.Od and ISNULL(PP.Do, '20990909') " +
                    "outer apply (select top 1 * from PracownicyParametry where IdPracownika = {0} and D.Data between Od and ISNULL(Do, '20990909')) PP " +

                    "left outer join Przypisania R on R.IdPracownika = {0} and R.Status = 1 and D.Data between R.Od and ISNULL(R.Do, '20990909') "
                //+ " left outer join PracownicyKarty PK on PK.IdPracownika = {0} and D.Data between PK.Od and ISNULL(PK.Do, '20990909')"
                //+ " order by D.Data"  optymalizacja i tak jest po dacie posortowane, a zajmowało 37% planu
                    , pracId, dFrom, dTo,
                    _zmKorektaId,



                    //kierAbsencja ? "ISNULL(A.Kod, P.Absencja)" : "A.Kod",    // po zamknieciu okresu powinien brac absencja z KP
                    //kierAbsencja ? "ISNULL(A.Kod, ISNULL(WT.IdKodyAbs, P.Absencja))" : "A.Kod",    // po zamknieciu okresu powinien brac absencja z KP
                    kierAbsencja ? "ISNULL(A.Kod, ISNULL(WT.IdKodyAbs, P.Absencja))" : "case when WT.Rodzaj = 1 then ISNULL(A.Kod, WT.IdKodyAbs) else A.Kod end",    // po zamknieciu okresu powinien brac absencja z KP, oraz wnioski typu 1 (np. Praca Zdalna)



                    addSelect, addFrom
                    , zmKorektaWymiarStr
                    ));

            DataTable dtDays = ds.Tables[0];
            dtDays.Columns.Add("TimeIn", typeof(DateTime));
            dtDays.Columns.Add("TimeOut", typeof(DateTime));

            dtDays.Columns.Add("Czas1sec", typeof(int));
            dtDays.Columns.Add("Czas1", typeof(string));
            dtDays.Columns.Add("Czas1R", typeof(string));

            dtDays.Columns.Add("Czas2sec", typeof(int));
            dtDays.Columns.Add("Czas2", typeof(string));
            dtDays.Columns.Add("Czas2R", typeof(string));

            dtDays.Columns.Add("Nocne1sec", typeof(int));
            dtDays.Columns.Add("Nocne2sec", typeof(int));
            dtDays.Columns.Add("ponocy", typeof(int));
            dtDays.Columns.Add("ponocy2", typeof(int));
#if WT2a
            dtDays.Columns.Add("czasZm2", typeof(int));         // parametry poki co dla algorytmów 2 i 12 !!!
            dtDays.Columns.Add("przerwaZm2", typeof(int));      // łaczny czas przerwy na zmianie
            dtDays.Columns.Add("przerwaZm2nom", typeof(int));   // przerwa nominalna czyli nie większa niż '15 ustawowe lub wartosc kierownika
            dtDays.Columns.Add("nadgDzien2", typeof(int));
            dtDays.Columns.Add("nadgNoc2", typeof(int));
            dtDays.Columns.Add("przerwaN2", typeof(int));       // łączny czas przerwy w nadgodzinach
            dtDays.Columns.Add("przerwaN2nom", typeof(int));    // przerwa w nadgodzinach nie przekraczająca ustawionej przez kier
            dtDays.Columns.Add("nocne2", typeof(int));          // czas nocny z czasu i przerw wliczonych

            //dtDays.Columns.Add("idx1", typeof(int));
            //dtDays.Columns.Add("idx2", typeof(int));
           
#endif
            dtDays.Columns.Add("Alert", typeof(int));


            dtDays.DefaultView.Sort = "Data ASC";
            dtDays = dtDays.DefaultView.ToTable();
            ds.Tables.RemoveAt(0);
            ds.Tables.Add(dtDays);         // dsDays = dsDays.DefaultView nie zadziałało

            if (withRCP)  // brak RCP - jak moZmiany, bo nie jest rejestrowany - nie wchodzę 
            {
                wt = new Worktime2();
                wt.Prepare(ds, pracId,
                            x_algRCP, x_algPar,
                            x_rcpId, x_strefaId,
                            fromTime, toTime,
                            nocneOdSec, nocneDoSec,
                            breakTimeZm, breakTimeN,
                            zaokr, zaokrType);

                /* 201311111
    #if !WT2a   //not
                    wt.MergeWorktime(ds, zaokr, zaokrType);
    #endif
                    //wt.Unload();
                */
            }
            else wt = null;
            return ds;
        }

        public DataTable GetRcpData(int zaokr, int rtype)   // -> dane rcp przy pracowniku
        {
            if (Data != null)
            {
                if (Data.Tables.Count <= 4)
                {
                    DataTable dtRcpData = Data.Tables.Add();   // [4]
                    dtRcpData.Columns.Add("Data", typeof(DateTime));
                    dtRcpData.Columns.Add("TimeIn", typeof(DateTime));
                    dtRcpData.Columns.Add("TimeOut", typeof(DateTime));
                    dtRcpData.Columns.Add("Czas1", typeof(string));
                    dtRcpData.Columns.Add("Czas1R", typeof(string));
                    dtRcpData.Columns.Add("Czas2", typeof(string));
                    dtRcpData.Columns.Add("Czas2R", typeof(string));
                    dtRcpData.Columns.Add("idx1", typeof(int));         // do GetDetails - pary rcp in-out z InOutData
                    dtRcpData.Columns.Add("idx2", typeof(int));
                    dtRcpData.Columns.Add("idx1r", typeof(int));        // do GetDetails2 - odczyty rcp z ReadersData
                    dtRcpData.Columns.Add("idx2r", typeof(int));

                    DateTime dtFrom = DateTime.Parse(FDateFrom);
                    DateTime dtTo = DateTime.Parse(FDateTo);

                    DataRowCollection rcpRows = DayData.Rows;           // na podstawie kolejnych Dni <- DayData
                    int idx = 0;
                    DataRow drRcp = null;
                    DateTime data = DateTime.MinValue;
                    //----- przygotowanie DayData->drRcp startowego ------
                    for (idx = 0; idx < rcpRows.Count; idx++)                       // szukam pierwszej daty która się łapie w okresie
                    {
                        drRcp = rcpRows[idx];
                        DateTime dt = (DateTime)Base.getDateTime(drRcp, "Data");    // musi być
                        if (dt >= dtFrom)
                        {
                            data = dt;
                            break;
                        }
                    }
                    //----- zbieranie danych ------
                    for (DateTime dt = dtFrom; dt <= dtTo; dt = dt.AddDays(1))      // wszystkie dni z zakresu, podpina dane jak znajdzie
                    {
                        if (dt == data)
                        {
                            string czas1 = null, czas1R = null, czas2 = null, czas2R = null;
                            int? c1 = Base.getInt(drRcp, "Czas1");                  // drRcp z DayData
                            int? c2 = Base.getInt(drRcp, "Czas2");
                            int? idx1 = Base.getInt(drRcp, "idx1");
                            int? idx2 = Base.getInt(drRcp, "idx2");
                            int? idx1r = Base.getInt(drRcp, "idx1r");
                            int? idx2r = Base.getInt(drRcp, "idx2r");
                            if (c1 != null && c1 != 0)
                            {
                                czas1 = Worktime.SecToTime((int)c1, 0);
                                czas1R = Worktime.SecToTimePP((int)c1, zaokr, rtype, false);
                            }
                            if (c2 != null && c2 != 0)
                            {
                                czas2 = Worktime.SecToTime((int)c2, 0);
                                czas2R = Worktime.SecToTimePP((int)c2, zaokr, rtype, false);
                            }
                            dtRcpData.Rows.Add(dt, drRcp["TimeIn"], drRcp["TimeOut"], czas1, czas1R, czas2, czas2R, idx1, idx2, idx1r, idx2r);
                            //----- następny dzień rcp -----
                            idx++;
                            if (idx < rcpRows.Count)
                            {
                                drRcp = rcpRows[idx];
                                data = (DateTime)Base.getDateTime(drRcp, "Data");   // musi być
                            }
                            else data = DateTime.MinValue;                          // koniec
                        }
                        else dtRcpData.Rows.Add(dt, null, null, null, null, null, null, null, null);
                    }
                }
                return Data.Tables[4];
            }
            else return null;
        }



        //---------------------------------------------------------------

        public DataTable AppendRcpData(string pracId, string prac, string nrew, int zaokr, int rtype, ref DataTable dtRcpData)   // -> dane rcp przy pracowniku
        {
            if (dtRcpData == null)
            {
                dtRcpData = new DataTable();

                dtRcpData.Columns.Add("Data", typeof(DateTime));
                dtRcpData.Columns.Add("PracId", typeof(int));
                dtRcpData.Columns.Add("Pracownik", typeof(string));
                dtRcpData.Columns.Add("KadryId", typeof(string));

                dtRcpData.Columns.Add("TimeIn", typeof(DateTime));
                dtRcpData.Columns.Add("TimeOut", typeof(DateTime));
                dtRcpData.Columns.Add("Czas1", typeof(string));
                dtRcpData.Columns.Add("Czas1R", typeof(string));
                dtRcpData.Columns.Add("Czas2", typeof(string));
                dtRcpData.Columns.Add("Czas2R", typeof(string));
                dtRcpData.Columns.Add("idx1", typeof(int));         // do GetDetails - pary rcp in-out z InOutData
                dtRcpData.Columns.Add("idx2", typeof(int));
                dtRcpData.Columns.Add("idx1r", typeof(int));        // do GetDetails2 - odczyty rcp z ReadersData
                dtRcpData.Columns.Add("idx2r", typeof(int));
            }

            DateTime dtFrom = DateTime.Parse(FDateFrom);
            DateTime dtTo = DateTime.Parse(FDateTo);

            DataRowCollection rcpRows = DayData.Rows;           // na podstawie kolejnych Dni <- DayData
            int idx = 0;
            DataRow drRcp = null;
            DateTime data = DateTime.MinValue;
            //----- przygotowanie DayData->drRcp startowego ------
            for (idx = 0; idx < rcpRows.Count; idx++)                       // szukam pierwszej daty która się łapie w okresie
            {
                drRcp = rcpRows[idx];
                DateTime dt = (DateTime)Base.getDateTime(drRcp, "Data");    // musi być
                if (dt >= dtFrom)
                {
                    data = dt;
                    break;
                }
            }
            //----- zbieranie danych ------
            for (DateTime dt = dtFrom; dt <= dtTo; dt = dt.AddDays(1))      // wszystkie dni z zakresu, podpina dane jak znajdzie
            {
                if (dt == data)
                {
                    string czas1 = null, czas1R = null, czas2 = null, czas2R = null;
                    int? c1 = Base.getInt(drRcp, "Czas1");                  // drRcp z DayData
                    int? c2 = Base.getInt(drRcp, "Czas2");
                    int? idx1 = Base.getInt(drRcp, "idx1");
                    int? idx2 = Base.getInt(drRcp, "idx2");
                    int? idx1r = Base.getInt(drRcp, "idx1r");
                    int? idx2r = Base.getInt(drRcp, "idx2r");
                    if (c1 != null && c1 != 0)
                    {
                        czas1 = Worktime.SecToTime((int)c1, 0);
                        czas1R = Worktime.SecToTimePP((int)c1, zaokr, rtype, false);
                    }
                    if (c2 != null && c2 != 0)
                    {
                        czas2 = Worktime.SecToTime((int)c2, 0);
                        czas2R = Worktime.SecToTimePP((int)c2, zaokr, rtype, false);
                    }
                    dtRcpData.Rows.Add(dt, pracId, prac, nrew, drRcp["TimeIn"], drRcp["TimeOut"], czas1, czas1R, czas2, czas2R, idx1, idx2, idx1r, idx2r);
                    //----- następny dzień rcp -----
                    idx++;
                    if (idx < rcpRows.Count)
                    {
                        drRcp = rcpRows[idx];
                        data = (DateTime)Base.getDateTime(drRcp, "Data");   // musi być
                    }
                    else data = DateTime.MinValue;                          // koniec
                }
                else dtRcpData.Rows.Add(dt, pracId, prac, nrew, null, null, null, null, null);
            }
            return dtRcpData;
        }

        //-----------------------
        public static void GetRcpData(string dod, string ddo, string table, string kierId)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            int zaokr = settings.Zaokr;
            int zaokrType = settings.ZaokrType;
            DataTable dtRcpData = null;
            if (String.IsNullOrEmpty(kierId)) kierId = "0";

#if WT2
            DataSet ds = db.getDataSet(String.Format(@"
declare 
	@od datetime,
	@do datetime,
	@kierId int
set @od = '{0}'
set @do = '{1}'
set @kierId = {2}

--select * from Przypisania where Od <= @do and ISNULL(Do, '20990909') >= @od and Status = 1 and IdKierownika = @kierId
select distinct IdPracownika, Nazwisko + ' ' + Imie as Pracownik, KadryId from dbo.fn_GetTreeOkres(@kierId, @od, @do, @do) order by Pracownik, KadryId
                ", dod, ddo, db.nullParam(kierId)));

            Worktime wt = new Worktime();
            foreach (DataRow dr in db.getRows(ds))
            {
                string pracId = db.getValue(dr, 0);
                string prac = db.getValue(dr, 1);
                string nrew = db.getValue(dr, 2);

                wt.Prepare(null, pracId, null, null, null, null, dod, ddo, 0, 0, 0, 0, 0, 0);  // czas nocny - jak 0 to nie pobiera bo tu mi to niepotrzebne, dsDays=null to przerwy i zaokr nie są brane pod uwagę
                wt.AppendRcpData(pracId, prac, nrew, zaokr, zaokrType, ref dtRcpData);
            }


            db.execSQL("delete from tmpRcpInOut");
            foreach (DataRow dr in dtRcpData.Rows)
            {
                string pracId = db.getValue(dr, "pracId");
                DateTime data = (DateTime)db.getDateTime(dr, "data");
                DateTime? tin = db.getDateTime(dr, "TimeIn");
                DateTime? tout = db.getDateTime(dr, "TimeOut");

                db.insert("tmpRcpInOut", 0, "IdPracownika,Data,TimeIn,TimeOut",
                    pracId,
                    db.strParam(Tools.DateToStrDb(data)),
                    db.isNull(tin) ? db.NULL : db.strParam(Tools.DateTimeToStr((DateTime)tin)),
                    db.isNull(tout) ? db.NULL : db.strParam(Tools.DateTimeToStr((DateTime)tout))
                    );
            }
#else
#endif
        }


        public static void GetRcpAnalize(string dod, string ddo, string selPrac, string table, string kierId)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            int zaokr = settings.Zaokr;
            int zaokrType = settings.ZaokrType;

            if (String.IsNullOrEmpty(kierId)) kierId = "0";
            if (String.IsNullOrEmpty(table)) table = "tmpRcpAnalize";

            DataSet ds = db.getDataSet(String.Format(selPrac, dod, ddo, db.nullParam(kierId)));

            int cnt = db.getCount(ds);
            Log.Info(Log.t2APP_INFO, String.Format("GetRcpAnalize('{0}','{1}','{2}','{3}')", dod, ddo, table, kierId), String.Format("Prac count: {0}", cnt));

            db.Execute("delete from {0} where Data between '{1}' and '{2}'", table, dod, ddo);
            string kid = null;
            int breakMM = 0;
            int breakMM2 = 0;
            int marginMM = 0;
            foreach (DataRow dr in db.getRows(ds))
            {
                string pracId = db.getValue(dr, 0);
                string prac = db.getValue(dr, 1);
                string nrew = db.getValue(dr, 2);

                DateTime d1 = (DateTime)Tools.StrToDateTime(dod);
                DateTime d2 = (DateTime)Tools.StrToDateTime(ddo);

                for (DateTime day = d1; day < d2; day = day.AddDays(1))
                {
                    string d = Tools.DateToStrDb(day);
                    string k = db.selectScalar("select IdKierownika from Przypisania where '{0}' between Od and ISNULL(Do,'20990909') and Status = 1 and IdPracownika = {1}", d, pracId);

                    if (!String.IsNullOrEmpty(k))   // ograniczenie - tylko dla dni kiedy jest aktywne przypisnie
                    {
                        if (k != kid)
                        {
                            kid = k;
                            KierParams kp = new KierParams(kid, settings);
                            breakMM = kp.Przerwa;    // uwzględnia jak null
                            breakMM2 = kp.Przerwa2;
                            marginMM = kp.Margines;
                        }

                        Worktime2 wt2;
                        DataSet dswt = _GetWorktime(
                                        pracId,
                                        null, null, null, true, //_algRCP, algPar, rcpId, true,
                                        dod, ddo, Tools.DateToStrDb(day),
                                        null, //_strefaId,
                                        zaokr, zaokrType,
                                        settings.NocneOdSec, settings.NocneDoSec,

                                        breakMM, breakMM2,

                                        null, // zmiana - jak null to korekta/plan
                                        0, /* wymiar */

                                        true,
                                        out wt2);   // -1 tak samo jak brak zmiany z pp
                        DataSet dsA = wt2.GetRcpAnalize();

                        if (String.IsNullOrEmpty(table)) table = "tmpRcpAnalize";
                        foreach (DataRow drA in Base.getRows(dsA))
                        {
                            int typ = (int)db.getInt(drA, "Typ");
                            string nazwa = db.getValue(drA, "Nazwa");

                            DateTime? tod = db.getDateTime(drA, "TimeOd");
                            DateTime? tdo = db.getDateTime(drA, "TimeDo");
                            int? czas = db.getInt(drA, "CzasSec");
                            int? noc = db.getInt(drA, "NocneSec");

                            db.insert(table, 0, "IdPracownika,Data,Typ,Nazwa,CzasOd,CzasDo,CzasSec,NocneSec,vCzas,vNocne",
                                pracId,
                                db.strParam(d),
                                typ,
                                db.strParam(nazwa),
                                db.isNull(tod) ? db.NULL : db.strParam(Tools.DateTimeToStr((DateTime)tod)),
                                db.isNull(tdo) ? db.NULL : db.strParam(Tools.DateTimeToStr((DateTime)tdo)),
                                db.isNull(czas) ? db.NULL : czas.ToString(),
                                db.isNull(noc) ? db.NULL : noc.ToString(),
                                db.isNull(czas) ? db.NULL : (((float)czas) / 3600).ToString().Replace(',', '.'),
                                db.isNull(noc) ? db.NULL : (((float)noc) / 3600).ToString().Replace(',', '.')
                                );
                        }
                    }
                }
            }
            Log.Info(Log.t2APP_INFO, "GetRcpAnalize() - END", null);
        }


        //-----------------------
        public DataTable ReadersData        // +/- 1 dzień -> GetDetails2; GetDetails3 bazuje na odczytach źródłowych
        {
            get
            {
                if (Data != null)
                    return Data.Tables[0];
                else
                    return null;
            }
        }

        public DataTable InOutData          // pary, kilka na dzień, +/- 1 dzień -> GetDetails
        {
            get
            {
                if (Data != null)
                    return Data.Tables[1];
                else
                    return null;
            }
        }

        public DataTable DayData            // 1 dzień + indeksy od-do w InOutData, +/- 1 dzień
        {
            get
            {
                if (Data != null)
                    return Data.Tables[2];
                else
                    return null;
            }
        }

        public DataTable AnalizeData        // do pokazania przy pracowniku, okres od-do, 1 dzień
        {
            get
            {
                if (Data != null)
                    return Data.Tables[3];
                else
                    return null;
            }
        }

        public DataTable RcpData            // do pokazania przy pracowniku, okres od-do, 1 dzień
        {
            get
            {
                if (Data != null)
                    return Data.Tables[4];
                else
                    return null;
            }
        }
    }
}

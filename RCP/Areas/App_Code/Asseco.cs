using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Text;
using System.Collections.Generic;

using System.Threading;
using System.Globalization;

using HRRcp.App_Code;
using HRRcp.Controls;
using HRRcp.Controls.Portal;
using HRRcp.Controls.Adm;




/*
Procedura:
1. Na koniec okresu eksport danych bazuje na danych z PP:
 * zaakceptowane - ok
 * zapisane - nie uwzględnia czasu RCP
 * brak PP - liczy tylko absencje, nie uwzględnia czasu RCP
 * MPK może być wprowadzony chociaż nie ma czasu - bo przed akceptacją nie są wartości przeniesione
W zwiazku z tym będą rozbieżności pomiędzy raportem nadgodzin, a sumą MPK
 
2. Po zamknieciu okresu 
 * zaakceptowane - ok, wszystkie są!
 * brak PP - liczy absencje, nie uwzglednia czasu RCP (bo nie ma go gdzie zatrzasnąć)
 * MPK zgodny z sumą czasu

Zmieniona procedura w raporcie nadgodzin i eksporcie do Asseco liczy czas nie tylko zaakceptowany przed zamknieciem miesiaca powiększa ilości.
Jeżeli tylko zaakceptowany, to sumy są mniejsze ok 10-20%
 
Nie ma sensu robić liczenia wg RCP przed zamknięciem, bo to i tak jest to szacunek.
 
 

----------------------------
 * Add a new Simple HTTPHandler to your project and fill it with code something like this.

using System.Web;
using System.Threading;

public class WorkHandler : IHttpHandler
{
public bool IsReusable
{
    get {return true;}
}
	
public void ProcessRequest(HttpContext context)
{
    Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
    thread.Start();
    context.Response.Write("Thread started");
}

public void WorkThreadFunction()
{
    try
    {
        // Call methods to do background work
    }
    catch (Exception ex)
    {
        // log errors
    }
}
}
 * --------------------------
 





*/


namespace HRRcp.App_Code
{
    public static class Asseco
    {
        public static string ASSECO_RCP = ConfigurationManager.ConnectionStrings["ASSECO_RCP"].ConnectionString;
        public static string ASSECO = ConfigurationManager.ConnectionStrings["ASSECO"].ConnectionString;

        public const int _ctCzasRCP = 1;
        public const int ctCzasRCPmac = 2;
        public const int ctCzasRCPprz = 4;
        public const int ctDopelnienia = 3;

        public const int ctAbsencja = 7;
        public const int ctGrSplituOfs = 10;  // lub Typ + 10, było 6

        //----- Import --------------------------------
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        public static void ImportAll()
        {
            ImportAll(true, true);
        }

        public static int ImportAll(bool showOk, bool showError)  // 0 - ok, -1 - error, -2 - pending
        {
            const string info = "Import danych z systemu Asseco HR";
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    SqlConnection conAsseco = db.Connect(Asseco.ASSECO);

                    int c1 = Asseco.ImportABSENCJA(conAsseco);
                    int c2 = Asseco.ImportKODYABS(conAsseco);
                    int c3 = Asseco.ImportKALENDAR(conAsseco);
                    int c4 = Asseco.ImportCZASNOM(conAsseco);
                    int c5 = Asseco.ImportSTAWKI(conAsseco);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    int c6 = Asseco.ImportZBIOR(conAsseco, DateTime.Today);
                    int c7 = Asseco.ImportUMOWY(conAsseco);
                    int c8 = Asseco.ImportLIMITY(conAsseco);
                    int c9 = Asseco.ImportSTANOWISKA(conAsseco);

                    db.Disconnect(conAsseco);

                    bool ok1 = Controls.Portal.cntWnioskiUrlopowe.UpdateEntered();  // jak 0 to też jest błąd

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0 && c7 >= 0 && c8 >= 0 && c9 >= 0)// && ok1)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        if (showOk) Tools.ShowMessage("Import zakończony.");
                        return 0;
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystąpił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", c1, c2, c3, c4, c5, c6, c7, c8, c9, ok1 ? "1" : "0"), Log.OK);
                        //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                        if (showError) Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                        return -1;
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
            {
                Log.Info(Log.IMPORT_ASSECO, info, "Trwa już import danych.", Log.OK);
                if (showError) Tools.ShowMessage("Trwa już import danych.\\n\\n" + App.User.Imie + ", poczekaj na jego zakończenie.");  // bez show
                return -2;
            }
        }

        public static void ImportAbsencje()
        {
            if (!importInProgress)
                try
                {
                    importInProgress = true;

                    const string info = "Import absencji z systemu Asseco HR";
                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    SqlConnection conAsseco = db.Connect(Asseco.ASSECO);

                    int c1 = Asseco.ImportABSENCJA(conAsseco);
                    int c2 = Asseco.ImportKODYABS(conAsseco);

                    db.Disconnect(conAsseco);

                    bool ok1 = Controls.Portal.cntWnioskiUrlopowe.UpdateEntered();  // jak 0 to też jest błąd

                    if (c1 >= 0 && c2 >= 0)// && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0 && c7 >= 0 && c8 >= 0)// && ok1)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import absencji zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu absencji, kody: {0} {1} {2}", c1, c2, ok1 ? "1" : "0"), Log.OK);
                        //Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1}", c1, c2), Log.OK);
                        Tools.ShowMessage("Wystąpił błąd podczas importu. Szczegóły błędu znajdują się w logu systemowym.");
                    }
                }
                finally
                {
                    importInProgress = false;
                }
            else
                Tools.ShowMessage("Trwa już import danych.\\n\\n" + App.User.Imie + ", poczekaj na jego zakończenie.");
        }
        //----- Export ------------------------------------







        public static bool ExportRCP(Okres ok, /*string pracId, */bool weekClose, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            //----- comma -----
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            //-----------------            
            Log.Info(Log.ASSECO, "Eksport danych RCP", String.Format("Okres od: {0} do: {1}", Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo)));
            int cnt = 0;
            SqlConnection conAsseco = db.Connect(ASSECO_RCP);
            //---------------
            //cnt = ExportRCP(conAsseco, ok.DateFrom, ok.DateTo, true, true, true, true);
            cnt = ExportRCP(conAsseco, ok, /*pracId, */weekClose, daneRCP, sumyRCP, daneMPK, daneStr);
            //---------------
            db.Disconnect(conAsseco);
            if (cnt >= 0)
                Log.Info(Log.ASSECO, "Eksport danych RCP - zakończony ok", String.Format("Rekordów: {0}", cnt));
            else
                Log.Error(Log.ASSECO, "Eksport danych RCP - BŁĄD!", String.Format("Rekordów: {0}", -cnt));
            return cnt > 0;
        }

        //----- awaryjne funkcje - odciąc po _ i zaremować oryginalną -----  // uwaga - dodany IdPracownika, trzeba go podlinkowac z Pracownicy
        // nie używaać!!!DaneMPK wAsseco niemają rekordów 019
        public static bool ExportRCP_restoreDaneMPK(Okres ok, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            Log.Info(Log.ASSECO, "Przywrócenie danych z DaneMPK", String.Format("Okres od: {0} do: {1}", Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo)));
            int cnt = 0;
            SqlConnection conAsseco = db.Connect(ASSECO_RCP);
            //---------------
            string d1 = Tools.DateToStr(ok.DateFrom);
            string d2 = Tools.DateToStr(ok.DateTo);
            Base.execSQL(String.Format("spr podlinkowanie IdPracownika >>> delete from DaneMPK where Data between '{0}' and '{1}'", d1, d2));

            cnt = db.ImportTable("DaneMPK", true, conAsseco, "select IdDaneRCP,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,Data,NR_EW,Typ,vCzasZm,vNadg50,vNadg100,vNocne from DaneMPK");
            //---------------
            db.Disconnect(conAsseco);
            if (cnt >= 0)
                Log.Info(Log.ASSECO, "Przywrócenie - zakończony ok", String.Format("Rekordów: {0}", cnt));
            else
                Log.Error(Log.ASSECO, "Przywrócenie - BŁĄD!", String.Format("Rekordów: {0}", -cnt));
            return cnt > 0;
        }

        public static int ExportRCP_copyMPK(SqlConnection asseco, DateTime dtFrom, DateTime dtTo, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            string d1 = Tools.DateToStr(dtFrom);
            string d2 = Tools.DateToStr(dtTo);
            return _CopyDaneMPK(asseco, d1, d2);
        }

        public static int ExportRCP_splity(SqlConnection asseco, Okres ok, bool weekClose, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        //public static int ExportRCP(SqlConnection asseco, Okres ok, bool weekClose, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            string d1 = Tools.DateToStr(ok.DateFrom);
            string d2 = Tools.DateToStr(ok.DateTo);
            return ExportSplityMacierzyste(asseco, d1, d2, ok);
        }
        //--------------------------
        public static int ExportRCP(SqlConnection asseco, Okres ok, /*string pracId, */bool weekClose, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            int cnt = 0;
            string d1 = Tools.DateToStr(ok.DateFrom);
            string d2 = Tools.DateToStr(ok.DateTo);
            string d2w = Tools.DateToStr(weekClose ? ok.LockedTo : ok.DateTo);
            //----- struktura organizacyjna -----
            if (daneStr)
            {
                cnt = db.ExportTable("DaneStr", true, asseco, @"
select P.KadryId, K.KadryId, O.DataOd, O.Datado
from OkresyRozl O
inner join PracownicyOkresy P on P.IdOkresu = O.Id and P.Status >= 0 and P.KadryId is not null and P.KadryId > 0
left outer join Pracownicy K on K.Id = P.IdKierownika
where O.Archiwum = 1
	union 
select P.KadryId, K.KadryId, O.DataOd, null
from OkresyRozl O
inner join Pracownicy P on P.Status >= 0 and P.KadryId is not null and P.KadryId > 0
left outer join Pracownicy K on K.Id = P.IdKierownika
where O.Id = (select top 1 Id from OkresyRozl where Archiwum = 0 order by DataOd)
order by P.KadryId,DataOd
                ");

                /*
                //                                    0          1          
                db.execSQL(asseco, String.Format("update DaneStr set DoDnia = '{0}' where DoDnia is null and OdDnia < '{0}'", Tools.DateToStr(ok.DateFrom.AddDays(-1))));  // nie mozna aktualizować z dziurami !!!
                DataSet dsStr = db.getDataSet("select P.KadryId, K.KadryId from Pracownicy P left outer join Pracownicy K on K.Id = P.IdKierownika where P.Status >= 0");
                foreach (DataRow dr in db.getRows(dsStr))
                {
                    cnt++;
                    if (insertStr(asseco, db.getValue(dr, 0), db.getValue(dr, 1), d1, null) == -1)
                    {
                        Log.Error(Log.ASSECO, "Eksport danych struktury - BŁĄD!", String.Format("Rekordów: {0}", cnt));
                        return -cnt;
                    }
                }
                */
            }

            //----- dane RCP i MPK -----
            if (daneRCP || sumyRCP || daneMPK)
            {
                cnt = 0;
                bool b1 = true;
                bool b2 = true;
                bool b3 = true;
                if (daneRCP) b1 = db.execSQL(asseco, String.Format("delete from DaneRCP where Data between '{0}' and '{1}'", d1, d2));
                if (sumyRCP) b2 = db.execSQL(asseco, String.Format("delete from SumyRCP where OkresRozl = '{0}'", d1));
                if (daneMPK)
                {
                    //db.execSQL(asseco, String.Format("delete from DaneMPK where Data between '{0}' and '{1}'", d1, d2));  później przy kopiowaniu jest usuwane
                    b3 = db.execSQL(String.Format("delete from DaneMPK where Data between '{0}' and '{1}'", d1, d2));
                }
                /* jak nic nie usunie bo nie ma to tez false
                if (!b1 || !b2 || !b3)
                {
                    Log.Error(Log.ASSECO, "Eksport danych RCP - BŁĄD!", String.Format("ExportRCP.delete 1: {0} 2:{1} 3:{2}", b1, b2, b3));
                    return -1;
                }
                 */
                //----- parametry ------
                Ustawienia settings = Ustawienia.CreateOrGetSession();
                int _ilDniPrac = Worktime._GetIloscDniPrac(db.con, d1, d2);         // w okresie - z Kalendarz
                int _ilDniPracMies = Worktime._GetIloscDniPracMies(db.con, d2);     // w miesiącu wynikajcym z końca okresu - z CzasNom
                //----- wszyscy pracownicy -----




                string select_prac = RepNadgodziny3._GetSqlDaneMPK(ok.IsArch(), ok.Id.ToString(), Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo), null);




                //string select_prac = RepNadgodziny3._GetSqlDaneMPK(ok.IsArch(), ok.Id.ToString(), Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo), "00026");
                //string select_prac = RepNadgodziny3._GetSqlDaneMPK(ok.IsArch(), ok.Id.ToString(), Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo), "11701");
                //Tools.ShowMessage("Uwaga - export tylko 1 pracownika");





                //string select_prac = RepNadgodziny3._GetSqlDaneMPK(ok.IsArch(), ok.Id.ToString(), Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo), "00018");
                //string select_prac = RepNadgodziny3.GetSqlDaneMPK(ok.IsArch(), ok.Id.ToString(), Tools.DateToStr(ok.DateFrom), Tools.DateToStr(ok.DateTo), "01112");
                //xstring select_prac = RepNadgodziny.GetSql(ok.IsArch(), ok.Id.ToString(), RepNadgodziny.nmoKier, "21 and P.KadryId='01186' ", null);  // -100 - wszyscy pracownicy





                DataSet dsP = db.getDataSet(select_prac);
                Log.Error(Log.ASSECO, "Eksport danych RCP", String.Format("Ilość pracowników: {0}", db.getCount(dsP)));

                foreach (DataRow dr in db.getRows(dsP))
                {
                    cnt++;
                    if (!ExportPracownik(asseco, dr, ok, d1, d2w, settings, _ilDniPrac, _ilDniPracMies, daneRCP, sumyRCP, daneMPK))
                        return -cnt;
                }
                //----- dane MPK - w całym okresie -----
                if (daneMPK)
                {
                    //----- dane MPK - podział 019 -----
                    //if (!weekClose)                 // dla week close nie dzielimy !!! >>> 20130420 dzielimy
                    if (!UpdateSplityGrupy(d1, d2)) return -1;
                    //----- przeniesienie danych MPK -----
                    cnt = _CopyDaneMPK(asseco, d1, d2);
                    if (cnt < 0) return -1;
                    //----- przeniesienie splitów -----
                    int cnt1 = ExportSplityMacierzyste(asseco, d1, d2, ok);
                    if (cnt1 < 0) return -1;
                }
            }


            //20150122 update OkresyRozl - jak błędy to wyjdzie wcześniej i nie zapisze <<< zerowac tą date ??? !!!
            DateTime di;
            if (weekClose)
                di = ok.LockedTo;
            else
            {
                di = DateTime.Today;
                if (di > ok.DateTo) di = ok.DateTo;
            }
            db.execSQL(String.Format("update OkresyRozl set DataImportu = '{1}' where DataOd = '{0}'", d1, Tools.DateToStrDb(di)));

            //----- dodatkowe procedury -----
            string sql;
            if (AssecoSQL.ExportAfterRCP(out sql, 0, d1, d2))
            {
                Log.Error(Log.ASSECO, "ExportAfterRCP", null);
                int ret = db.execSQLEx(sql);
                if (ret < 0)   // bo może być 0 rekordów i to jest ok
                {
                    Log.Error(Log.ASSECO, "ExportAfterRCP", String.Format("Error: {0}", ret));
                    return -1;
                }
            }
            if (AssecoSQL.ExportAfterAsseco(out sql, 0, d1, d2))
            {
                Log.Error(Log.ASSECO, "ExportAfterAsseco", null);
                int ret = db.execSQLEx(asseco, sql);
                if (ret < 0)    // bo może być 0 rekordów i to jest ok
                {
                    Log.Error(Log.ASSECO, "ExportAfterAsseco", String.Format("Error: {0}", ret));
                    return -1;
                }
            }

            return cnt;
        }












        public static int test_ExportRCP(SqlConnection asseco, Okres ok, /*string pracId, */bool weekClose, bool daneRCP, bool sumyRCP, bool daneMPK, bool daneStr)
        {
            int cnt = 0;
            string d1 = Tools.DateToStr(ok.DateFrom);
            string d2 = Tools.DateToStr(ok.DateTo);
            string d2w = Tools.DateToStr(weekClose ? ok.LockedTo : ok.DateTo);

            //----- dodatkowe procedury -----
            string sql;
            if (AssecoSQL.ExportAfterRCP(out sql, 0, d1, d2))
            {
                Log.Error(Log.ASSECO, "ExportAfterRCP", null);
                int ret = db.execSQLEx(sql);
                if (ret < 0)
                {
                    Log.Error(Log.ASSECO, "ExportAfterRCP", String.Format("Error: {0}", ret));
                    return -1;
                }
            }
            if (AssecoSQL.ExportAfterAsseco(out sql, 0, d1, d2))
            {
                Log.Error(Log.ASSECO, "ExportAfterAsseco", null);
                int ret = db.execSQLEx(asseco, sql);
                if (ret < 0)
                {
                    Log.Error(Log.ASSECO, "ExportAfterAsseco", String.Format("Error: {0}", ret));
                    return -1;
                }
            }
            return cnt;
        }











        public static int _CopyDaneMPK(SqlConnection asseco, string DateFrom, string DateTo)
        {   //                     0         1    2     3   4  5     6      7               8             9     10    11     12      13      14      15       16      
            //const string select = "IdDaneRCP,Data,NR_EW,Typ,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,vCzasZm,vNadg50,vNadg100,vNocne";
            const string select = "IdDaneRCP,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,Data,NR_EW,Typ,vCzasZm,vNadg50,vNadg100,vNocne";

            db.execSQL(asseco, String.Format("delete from DaneMPK where Data between '{0}' and '{1}'", DateFrom, DateTo));
            /*
            if (!Base.execSQL(asseco, String.Format("delete from DaneMPK where Data between '{0}' and '{1}'", DateFrom, DateTo)))
            {
                Log.Error(Log.ASSECO, "Eksport danych RCP - BŁĄD!", "CopyDaneMPK.delete");
                return -1;
            }
             */

            int cnt = db.CopyTable(db.con, String.Format(   // uwaga !!! istotna jest kolejność pól !!!
                "select {0} from DaneMPK where Data between '{1}' and '{2}' and cc not in (select cc from CC where GrSplitu is not null)", select, DateFrom, DateTo),
                asseco, "DaneMPK", false);

            /*
            //DataSet dsP = db.getDataSet("select " + select + " from DaneMPK " + String.Format("where Data between '{0}' and '{1}'", DateFrom, DateTo));
            DataSet dsP = db.getDataSet(String.Format("select {0} from DaneMPK where Data between '{1}' and '{2}'", select, DateFrom, DateTo));
            
            int cnt = 0;
            foreach (DataRow dr in db.getRows(dsP))
            {
                if (db.insert(asseco, db.insertCmd("DaneMPK", 0,
                    select,
                    db.getValue(dr, 0),
                    db.strParam(db.getValue(dr, 1)),
                    db.strParam(db.getValue(dr, 2)),
                    db.nullParam(db.getValue(dr, 3)),
                    db.strParam(db.getValue(dr, 4)),
                    db.strParam(db.getValue(dr, 5)),
                    db.strParam(db.getValue(dr, 6)),
                    db.strParam(db.getValue(dr, 7)),
                    db.strParam(db.getValue(dr, 8)),
                    db.strParam(db.getValue(dr, 9)),
                    db.strParam(db.getValue(dr, 10)),
                    db.strParam(db.getValue(dr, 11)),
                    db.strParam(db.getValue(dr, 12)),
                    db.nullParam(db.getDouble(dr, 13)),
                    db.nullParam(db.getDouble(dr, 14)),
                    db.nullParam(db.getDouble(dr, 15)),
                    db.nullParam(db.getDouble(dr, 16))
                    ), false, true) == -1)
                {
                    return -cnt;
                }
                else
                    cnt++;
            }
             */
            return cnt;
        }









        //-----------------------------------------------------------
        public class TimeSumator2
        {
            int[] sumy;
            int FSize = 0;

            public TimeSumator2(int size)
            {
                FSize = size;
                sumy = new int[FSize];
            }

            public void Reset()
            {
                for (int i = 0; i < FSize; i++)
                    sumy[i] = 0;
            }

            public void SumTimes(params int[] values)
            {
                for (int i = 0; i < values.Count(); i++)
                    if (i < FSize)
                        sumy[i] += values[i];
            }
        }

        public class TimeSumator3
        {
            object[] sumy;
            int FSize = 0;

            public TimeSumator3(int size)
            {
                FSize = size;
                sumy = new object[FSize];
            }

            public void Reset()
            {
                for (int i = 0; i < FSize; i++)
                    sumy[i] = 0;
            }

            public void SumTimes(params object[] values)
            {
                for (int i = 0; i < values.Count(); i++)
                    if (i < FSize)
                        if (values[i] is int)
                            sumy[i] = (sumy[i] == null ? 0 : (int)sumy[i]) + (int)values[i];
                        else if (values[i] is float)
                            sumy[i] = (sumy[i] == null ? 0 : (float)sumy[i]) + (float)values[i];
                        else if (values[i] is double)
                            sumy[i] = (sumy[i] == null ? 0 : (double)sumy[i]) + (double)values[i];
                        else
                            sumy[i] = 0;
            }

            public int GetInt(int idx)
            {
                if (sumy[idx] == null) return 0;
                else return (int)sumy[idx];
            }

            public float GetFloat(int idx)
            {
                if (sumy[idx] == null) return 0;
                else return (float)sumy[idx];
            }

            public double GetDouble(int idx)
            {
                if (sumy[idx] == null) return 0;
                else return (double)sumy[idx];
            }

            public object[] Sum
            {
                get { return sumy; }
            }
        }

        //-----------------------------------------------------------
        // zmiana S 
        public static void _GetSumy(PlanPracy.TimeSumator sumator, int zaokrSum, int zaokrSumType, out int pptime, out int wtime, out int ztime, out int otime50, out int otime100, out int orest, out int ntime, out int htime)
        {
            wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // całkowity 
            otime50 = sumator.GetSum(sumator.sumZTime, "150", true);                                            // nadgodziny 50 ze zmiany
            otime100 = sumator.GetSum(sumator.sumZTime, "200", true);                                           // nadgodziny 100 ze zmiany
            ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);                // pozostały czas ze zmiany (tu są absencje !!!)
            pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);              // zaplanowany
            otime50 += sumator.GetSum(sumator.sumOTime, "150", true);                                           // nadgodziny 50
            otime100 += sumator.GetSum(sumator.sumOTime, "200", true);                                          // nadgodziny 100
            otime50 = Worktime.RoundSec(otime50, zaokrSum, zaokrSumType);                                       // zaokr. nadgodziny 50
            otime100 = Worktime.RoundSec(otime100, zaokrSum, zaokrSumType);                                     // zaokr. nadgodziny 100
            orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadgodzin
            ntime = Worktime.RoundSec(sumator._sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // czas w niedziele i święta
        }
        //-----------------------------------------------------------
        public static int insertDaneRCP(SqlConnection asseco, DataRow drPP, int nadg50, int nadg100, bool zeroZm)   // czas zm moze byc z absencji !!!
        {
            int? zm = zeroZm ? 0 : db.getInt(drPP, "CzasZm");
            int? nd = db.getInt(drPP, "NadgodzinyDzien");
            int? nn = db.getInt(drPP, "NadgodzinyNoc");
            int? noc = db.getInt(drPP, "Nocne");
            int id = db.getInt(drPP, "Id", 0);
            if (db.insert(asseco, db.insertCmd("DaneRCP", 0,
                "Id,Data,NR_EW,IdZmianyPlan,SymbolZmianyPlan,NazwaZmianyPlan,ZmianaOdPlan,ZmianaDoPlan,ZmianaNadgDzienPlan,ZmianaNadgNocPlan,IdZmiany,SymbolZmiany,NazwaZmiany,ZmianaOd,ZmianaDo,ZmianaNadgDzien,ZmianaNadgNoc,CzasIn,CzasOut,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Akceptacja,Nadg50,Nadg100",
                id,
                db.strParam(db.getValue(drPP, "Data")),
                db.strParam(db.getValue(drPP, "KadryId")),

                db.nullParam(db.getValue(drPP, "IdZmianyPlan")),
                db.nullStrParam(db.getValue(drPP, "SymbolZmianyPlan")),
                db.nullStrParam(db.getValue(drPP, "NazwaZmianyPlan")),
                db.nullStrParam(db.getValue(drPP, "ZmianaOdPlan")),
                db.nullStrParam(db.getValue(drPP, "ZmianaDoPlan")),
                db.nullParam(db.getValue(drPP, "ZmianaNadgDzienPlan")),
                db.nullParam(db.getValue(drPP, "ZmianaNadgNocPlan")),

                db.nullParam(db.getValue(drPP, "IdZmiany")),
                db.nullStrParam(db.getValue(drPP, "SymbolZmiany")),
                db.nullStrParam(db.getValue(drPP, "NazwaZmiany")),
                db.nullStrParam(db.getValue(drPP, "ZmianaOd")),
                db.nullStrParam(db.getValue(drPP, "ZmianaDo")),
                db.nullParam(db.getValue(drPP, "ZmianaNadgDzien")),
                db.nullParam(db.getValue(drPP, "ZmianaNadgNoc")),

                db.nullStrParam(db.getValue(drPP, "CzasIn")),
                db.nullStrParam(db.getValue(drPP, "CzasOut")),

                db.nullStrParam(zm == null ? null : Worktime.SecToTime((int)zm, 0)),
                //db.strParam(Worktime.SecToTime(czasZm, 0)),

                db.nullStrParam(nd == null ? null : Worktime.SecToTime((int)nd, 0)),
                db.nullStrParam(nn == null ? null : Worktime.SecToTime((int)nn, 0)),
                db.nullStrParam(noc == null ? null : Worktime.SecToTime((int)noc, 0)),
                //db.strParam(Worktime.SecToTime(db.getInt(drPP, "NadgodzinyDzien", 0), 0)),

                db.strParam(db.sqlPut(db.getValue(drPP, "Uwagi"))),
                db.getBool(drPP, "Akceptacja", false) ? 1 : 0,

                db.strParam(Worktime.SecToTime(nadg50, 0)),
                db.strParam(Worktime.SecToTime(nadg100, 0))
                ), false, true) != -1)
                return id;
            else
                return -1;
        }

        public static bool insertSumyRCP(SqlConnection asseco, string okresOd, DataRow drP, int Nominalny, int Zaplanowany, int Przepracowany, int Nieprzepracowany, int Nadg50, int Nadg100, int Wnocy, int NiedzieleSwieta, string Uwagi)
        {
            return db.insert(asseco, db.insertCmd("SumyRCP", 0,
                "OkresRozl,Pracownik,NR_EW,Dział,Nominalny,Zaplanowany,Przepracowany,Nieprzepracowany,Nadg50,Nadg100,Wnocy,NiedzieleSwieta,Uwagi",
                db.strParam(okresOd),
                db.strParam(db.getValue(drP, "NazwiskoImie")),
                db.strParam(db.getValue(drP, "KadryId")),
                db.strParam(db.getValue(drP, "Dzial")),
#if NOMWYM
 Worktime.SecToHourStr(Nominalny).Replace(',', '.'),   //<<<<<<<<<<<<<<<<<<<  powinno być na float
#else
                Nominalny, 
#endif
                Zaplanowany, Przepracowany, Nieprzepracowany, Nadg50, Nadg100, Wnocy, NiedzieleSwieta,
                db.strParam(db.sqlPut(Uwagi))
                ), false, true) != -1;
        }

        /*
        public static int insertStr(SqlConnection asseco, string pracLogo, string kierLogo, string odDnia, string doDnia)
        {
            string _kierLogo = String.IsNullOrEmpty(kierLogo) ? "KierLogo is null" : String.Format("KierLogo = '{0}'", kierLogo);
            string _doDnia = String.IsNullOrEmpty(doDnia) ? "DoDnia is null" : String.Format("DoDnia = '{0}'", doDnia);
            string id = Base.getScalar(asseco, String.Format("select Id from DaneStr where PracLogo = '{0}' and {1} and OdDnia = '{2}' and {3}", pracLogo, _kierLogo, odDnia, _doDnia));
            if (String.IsNullOrEmpty(id))
                return db.insert(asseco, db.insertCmd("DaneStr", 0,
                    "PracLogo,KierLogo,OdDnia,DoDnia",
                    db.strParam(pracLogo),
                    db.nullStrParam(kierLogo),
                    db.strParam(odDnia),
                    db.nullStrParam(doDnia)), false, true);
            else
                return 0;
        }
        */

        public static bool insertCC(SqlConnection asseco, int idDaneRCP,
                                    string nrew, string pracId, string day, string cc, int typ,
                                    int zm, int nd, int nn, int noc, int t50, int t100,
                                    double vzm, double vt50, double vt100, double vnoc)
        {
            if (zm > 0 || nd > 0 || nn > 0 || noc > 0 || t50 > 0 || t100 > 0)
                return db.insert(db.con, db.insertCmd("DaneMPK", 0,             // wstawiam do HR_DB, później całosc będzie przekopiowana
                    "IdDaneRCP,Data,NR_EW,IdPracownika,Typ,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,vCzasZm,vNadg50,vNadg100,vNocne",
                    idDaneRCP,
                    db.strParam(day),
                    db.strParam(nrew),
                    pracId,
                    typ,
                    db.strParam(cc),
                    db.NULL,
                    db.strParam(Worktime.SecToTime(zm, 0)),
                    db.strParam(Worktime.SecToTime(nd, 0)),
                    db.strParam(Worktime.SecToTime(nn, 0)),
                    db.strParam(Worktime.SecToTime(noc, 0)),
                    db.NULL,
                    db.strParam(Worktime.SecToTime(t50, 0)),
                    db.strParam(Worktime.SecToTime(t100, 0)),
                    vzm, vt50, vt100, vnoc
                    ), false, true) != -1;
            else
                return true;
        }

        public static bool _insertCCsplit(SqlConnection con,
                                        string nrew, string pracId, string day, string idSplitu, int idPrzypisania, int idDaneRCP, int typ,
                                        int zm, int nd, int nn, int n50, int n100, int nocM, int nocP)
        {
            if (!String.IsNullOrEmpty(idSplitu))
            {
                if (zm < 0) zm = 0;
                if (nd < 0) nd = 0;
                if (nn < 0) nn = 0;
                if (nocM < 0) nocM = 0;
                if (nocP < 0) nocP = 0;
                if (n50 < 0) n50 = 0;
                if (n100 < 0) n100 = 0;
                if (zm > 0 || nocM > 0)
                {
                    //                                  0      1
                    DataSet dsS = db.getDataSet("select CC.cc, W.Wsp from SplityWsp W left outer join CC on CC.Id = W.IdCC where W.IdSplitu = " + idSplitu);
                    foreach (DataRow drS in db.getRows(dsS))
                    {
                        string cc = db.getValue(drS, 0);
                        double wsp = db.getDouble(drS, 1, 0);
                        if (!insertCC(con, idDaneRCP, nrew, pracId, day, cc, typ,
                            (int)Worktime.Round05(zm * wsp, 0),
                            0,//(int)Worktime.Round05(nd * wsp, 0),
                            0,//(int)Worktime.Round05(nn * wsp, 0),
                            (int)Worktime.Round05(nocM * wsp, 0),
                            0,//(int)Worktime.Round05(n50 * wsp, 0),
                            0,//(int)Worktime.Round05(n100 * wsp, 0),
                            (double)zm * wsp / 3600,                    // UWAGA !!! Jakby tu dać: Worktime.Round05(zm * wsp, 0) to powiino być to samo w czasach i floatach - PRZETESTOWAć !!!
                            0,//(double)n50 * wsp / 3600,
                            0,//(double)n100 * wsp / 3600,
                            (double)nocM * wsp / 3600))
                        {
                            Log.Error(Log.ASSECO, "analizeCC->insertCC(SPLIT)", String.Format("pracId:{0} data:{1} cc:{2}", nrew, day, cc));
                            return false;
                        }
                    }
                }

                if (nd > 0 || nn > 0 || nocP > 0 || n50 > 0 || n100 > 0)
                {
                    DataSet dsP = db.getDataSet(String.Format("select CC.cc, W.Wsp from SplityWspP W left outer join CC on CC.Id = W.IdCC where W.IdPrzypisania = {0}", idPrzypisania));  //20160215
                    foreach (DataRow drP in db.getRows(dsP))
                    {
                        string cc = db.getValue(drP, 0);
                        double wsp = db.getDouble(drP, 1, 0);
                        if (!insertCC(con, idDaneRCP, nrew, pracId, day, cc, ctCzasRCPprz,  //typ,
                            0,//(int)Worktime.Round05(zm * wsp, 0),
                            (int)Worktime.Round05(nd * wsp, 0),
                            (int)Worktime.Round05(nn * wsp, 0),
                            (int)Worktime.Round05(nocP * wsp, 0),
                            (int)Worktime.Round05(n50 * wsp, 0),
                            (int)Worktime.Round05(n100 * wsp, 0),
                            0,//(double)zm * wsp / 3600,                    // UWAGA !!! Jakby tu dać: Worktime.Round05(zm * wsp, 0) to powiino być to samo w czasach i floatach - PRZETESTOWAć !!!
                            (double)n50 * wsp / 3600,
                            (double)n100 * wsp / 3600,
                            (double)nocP * wsp / 3600))
                        {
                            Log.Error(Log.ASSECO, "analizeCC->insertCC(PRZYPISANIE)", String.Format("pracId:{0} data:{1} cc:{2}", nrew, day, cc));
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                //Log.Info(Log.ASSECO, "analizeCC->insertCC(SPLIT).NoSplit", String.Format("pracId:{0} data:{1}", nrew, day)); info jest wyżej
                return true;        // tylko info 
            }
        }

        public static bool insertCCsplit_mac(SqlConnection con,
                                string nrew, string pracId, string day, string idSplitu, int idDaneRCP, int typ,
                                int zm, int nd, int nn, int n50, int n100, int noc)
        {
            if (!String.IsNullOrEmpty(idSplitu))
            {
                if (zm < 0) zm = 0;
                if (nd < 0) nd = 0;
                if (nn < 0) nn = 0;
                if (noc < 0) noc = 0;
                if (n50 < 0) n50 = 0;
                if (n100 < 0) n100 = 0;
                if (zm > 0 || nd > 0 || nn > 0 || noc > 0 || n50 > 0 || n100 > 0)
                {
                    /*
                    string cc = db.getValue(drS, 0);
                    double wsp = db.getDouble(drS, 1, 0);
                    double vZm = (double)zm * wsp / 3600;
                    double vN50 = (double)n50 * wsp / 3600;
                    double vN100 = (double)n100 * wsp / 3600;
                    double vNoc = (double)noc * wsp / 3600;
                    */
                    //                                  0      1
                    DataSet dsS = db.getDataSet("select CC.cc, W.Wsp from SplityWsp W left outer join CC on CC.Id = W.IdCC where W.IdSplitu = " + idSplitu);
                    foreach (DataRow drS in db.getRows(dsS))
                    {
                        string cc = db.getValue(drS, 0);
                        double wsp = db.getDouble(drS, 1, 0);
                        if (!insertCC(con, idDaneRCP, nrew, pracId, day, cc, typ,
                            (int)Worktime.Round05(zm * wsp, 0),
                            (int)Worktime.Round05(nd * wsp, 0),
                            (int)Worktime.Round05(nn * wsp, 0),
                            (int)Worktime.Round05(noc * wsp, 0),
                            (int)Worktime.Round05(n50 * wsp, 0),
                            (int)Worktime.Round05(n100 * wsp, 0),
                            (double)zm * wsp / 3600,                    // UWAGA !!! Jakby tu dać: Worktime.Round05(zm * wsp, 0) to powiino być to samo w czasach i floatach - PRZETESTOWAć !!!
                            (double)n50 * wsp / 3600,
                            (double)n100 * wsp / 3600,
                            (double)noc * wsp / 3600))
                        {
                            Log.Error(Log.ASSECO, "analizeCC->insertCC(SPLIT)", String.Format("pracId:{0} data:{1} cc:{2}", nrew, day, cc));
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                //Log.Info(Log.ASSECO, "analizeCC->insertCC(SPLIT).NoSplit", String.Format("pracId:{0} data:{1}", nrew, day)); info jest wyżej
                return true;        // tylko info 
            }
        }

        public static bool x_insertCCsplitMac(SqlConnection con,
                                        string nrew, string pracId, string day, string idSplitu, int idDaneRCP, int typ)
        {
            //                                  0      1
            DataSet dsS = db.getDataSet("select CC.cc, W.Wsp from SplityWsp W left outer join CC on CC.Id = W.IdCC where W.IdSplitu = " + idSplitu);
            foreach (DataRow drS in db.getRows(dsS))
            {
                string cc = db.getValue(drS, 0);
                double wsp = db.getDouble(drS, 1, 0);
                if (!insertCC(con, idDaneRCP, nrew, pracId, day, cc, typ,
                    0, 0, 0, 0, 0, 0,
                    wsp, 0, 0, 0))
                {
                    Log.Error(Log.ASSECO, "analizeCC->insertCC(SPLIT-MAC)", String.Format("pracId:{0} data:{1} cc:{2}", nrew, day, cc));
                    return false;
                }
            }
            return true;
        }

        //-------------------------------------------        
        public static bool analizeCC(SqlConnection asseco, DataRow drPP, PlanPracy.TimeSumator sumator,                 // pobiera podział w danym dniu na cc, przelicza na nadg.50 i 100 wg zmiany; drPP - PlanPracy w dniu
                              int idDaneRCP, string pracId, string nrew, DateTime date,
                              string idSplitu, int idPrzypisania,
                              string zmiana, // id, dzień, macierzyste cc, zmiana
                              bool isAbsencja, string kodAbs,                                                           // ztime wtedy ma ilosc godzin z kodówAbs
                              int _ztime, int otimeD, int otimeN, int ntime, int t50, int t100, bool zeroZm)
        {
            //----- centra wskazane przez kierowników ------
            string day = Tools.DateToStr(date);
            //     0       1                2              3      4      5         6
            DataSet dsPK = db.getDataSet(String.Format(@"
select CzasZm, NadgodzinyDzien, NadgodzinyNoc, Nocne, CC.cc, CC.Nazwa, CC.GrSplitu
from PodzialKosztow PK 
left outer join CC on CC.Id = PK.IdMPK
where Data='{0}' and IdPracownika={1}
order by PK.Id", day, pracId));    //20141205 w kolejności wpisywania
            int sZm = 0;
            int sND = 0;
            int sNN = 0;
            int sNoc = 0;
            int s50 = 0;
            int s100 = 0;
            bool acc = db.getBool(drPP, "Akceptacja", false);
            bool ret = true;

            /*    
            if (day == "2013-01-30" && nrew == "01186")
            {
                int x = 0;
            }  /**/

            if (!isAbsencja)                          // jeżeli jest absencja to nie dzielę !!!, absencja zawsze w macierzyste
                foreach (DataRow dr in db.getRows(dsPK))
                {
                    int zm = db.getInt(dr, 0, 0);
                    int nd = db.getInt(dr, 1, 0);
                    int nn = db.getInt(dr, 2, 0);
                    int noc = db.getInt(dr, 3, 0);
                    string cc = db.getValue(dr, 4);
                    int cc0, cc50, cc100;
                    sumator.solveNadgodziny2(date, zm, ref sND, nd, nn, out cc0, out cc50, out cc100);                // drZmiana jest ustawiane wczesniej przy FindZmiana

                    if (zeroZm) zm = 0;  //20151018 zeby nie wchodziło do sumy też

                    if (!insertCC(asseco, idDaneRCP, nrew, pracId, day, cc, _ctCzasRCP, zm, nd, nn, noc, cc50, cc100,
                        (double)zm / 3600, (double)cc50 / 3600, (double)cc100 / 3600, (double)noc / 3600))  // cc0 powinno = zm
                    {
                        Log.Error(Log.ASSECO, "analizeCC->insertCC", String.Format("pracId:{0} data:{1} cc:{2}", nrew, day, cc));
                        ret = false;
                        break;      // nie przetwarzam dalej
                    }

                    sZm += zm;

                    //sND += nd;    //Puchowicz Adrian 933 w 20141115 ma rozpisane 10+1, dlatego ref w _solveNadgoddziny2

                    sNN += nn;
                    sNoc += noc;
                    s50 += cc50;
                    s100 += cc100;
                }
            //----- centrum(a) kosztowe macierzyste (czasy nie wskazane na cc -> na macierzyste)-----
            if (ret)
            {
                int typ = isAbsencja ? ctAbsencja : ctCzasRCPmac;           // <<<< typ 7 - absencja - 2 - macierzyste
                if (MPK3.wgPrzEnabled(date) && !isAbsencja)                 // absencja - macierzystym
                {
                    int nlimit = ntime > sNoc ? ntime - sNoc : 0;           // limit - ile nadg pozostało nierozpisanych
                    int nn = otimeN > sNN ? otimeN - sNN : 0;               // tyle jest nadgodzin w nocy
                    int nocP = nn > nlimit ? nlimit : nn;                   // ile się da dzielimy Prz, ale może coś zostać 
                    int nocM = nlimit - nocP;                               // to dzielimy Mac

                    ret = _insertCCsplit(db.con, nrew, pracId, day, idSplitu, idPrzypisania, idDaneRCP, typ,  // typ 2 - reszta nie podzielona (czyli macierzyste cc)

                                        zeroZm ? 0 : _ztime - sZm,  //20151018

                                        otimeD - sND,
                                        otimeN - sNN,
                                        t50 - s50,
                                        t100 - s100,
                        //ntime - sNoc
                                        nocM,
                                        nocP
                                        );
                }
                else
                {
                    ret = insertCCsplit_mac(db.con, nrew, pracId, day, idSplitu, idDaneRCP, typ,    // typ 2 - reszta nie podzielona (czyli macierzyste cc)

                                        zeroZm ? 0 : _ztime - sZm,  //20151018

                                        otimeD - sND,
                                        otimeN - sNN,
                                        t50 - s50,
                                        t100 - s100,
                                        ntime - sNoc
                                        );
                }
            }
            return ret;
        }

        //-------------------------------------------                                                                               // lub koniec DateTo po zamknięciu
        public static bool ExportPracownik(SqlConnection asseco, DataRow drP, Okres ok, string DateFrom, string _DateTo, Ustawienia settings, int _ilDniPrac, int _ilDniPracMies,
                                   bool daneRCP, bool sumyRCP, bool daneMPK)
        {
            PlanPracy.TimeSumator sumator = new PlanPracy.TimeSumator();
            TimeSumator3 total = new TimeSumator3(12);
            int _ilGodzPrac;
            int _ilGodzPracMies;

            bool incompleteData = false;    // niekompletne dane (nie wszystko zatwierdzone !!!) - do wyświetlenia warninga

            string pracId = db.getValue(drP, "Id");


            /*
            if (pracId == "1105")
            {
                int x = 0;
            }
            */




            string nrew = db.getValue(drP, "KadryId");

            //----- nie licze pracownika -----
            //int alg = Base.getInt(drP, "Algorytm", -1);
            //bool pracOff = alg == 0;
            bool pracOff = false;   //20141111 wszyscy mają być 


            int etatL = Base.getInt(drP, "EtatL", -1);
            int etatM = Base.getInt(drP, "EtatM", -1);
            int etat = etatL > 0 && etatM > 0 ? 8 * etatL / etatM : -1;   // il godzin pracy w dniu lub -1
            //----- kierownik / pracownik ------
            //----- czas pracy w kolejne dni z PP -----
            bool _allAccepted = true;        // wszystkie dni są poakceptowane >>> 
            int pptime, wtime, _ztime2, otime50, otime100, orest, ntime2, htime;  // htime - praca w niedziele i święta, sumator sam to liczy!!!
            if (!pracOff)
            {
                //GrSplitu -> jesli null to przez Linie bierz z kierownika , teraz rozwiązanie TYMCZASOWE !!!

                const string select = @"
select 
    D.Data,
    PP.CzasZm,PP.NadgodzinyDzien,PP.NadgodzinyNoc,PP.Nocne,A.Kod,AK.GodzinPracy,Z.Id as TypZmiany,K.Rodzaj,PP.Akceptacja,
    PP.Id, P.KadryId, P.Id as PracId, 
    P.GrSplitu, S.Id as IdSplitu, 
    PP.IdZmiany as IdZmianyPlan, ZP.Symbol as SymbolZmianyPlan, ZP.Nazwa as NazwaZmianyPlan, ZP.Od as ZmianaOdPlan, ZP.Do as ZmianaDoPlan, ZP.NadgodzinyDzien as ZmianaNadgDzienPlan, ZP.NadgodzinyNoc as ZmianaNadgNocPlan,
    Z.Id as IdZmiany, Z.Symbol as SymbolZmiany, Z.Nazwa as NazwaZmiany, Z.Od as ZmianaOd, Z.Do as ZmianaDo, Z.NadgodzinyDzien as ZmianaNadgDzien, Z.NadgodzinyNoc as ZmianaNadgNoc,
    PP.CzasIn, PP.CzasOut,
    PP.Uwagi,
    ISNULL(PP.IdZmianyKorekta, PP.IdZmiany) as IdZmianyKor,
    R.Id as IdPrzypisania
from dbo.GetDates2('{0}','{1}') D 
    left outer join PlanPracy PP on PP.Data = D.Data and PP.IdPracownika={2}
    outer apply (select top 1 * from Przypisania where IdPracownika={2} and Status = 1 and Od <= D.Data order by Od desc) R
    left outer join Zmiany ZP on ZP.Id = PP.IdZmiany
    left outer join Zmiany Z on Z.Id = ISNULL(PP.IdZmianyKorekta, PP.IdZmiany)
    left outer join Kalendarz K on K.Data = D.Data
    --left outer join Absencja A on A.IdPracownika = {2} and D.Data between A.DataOd and A.DataDo
    outer apply (select top 1 * from Absencja where IdPracownika = {2} and D.Data between DataOd and DataDo) A 
    left outer join AbsencjaKody AK on AK.Kod = A.Kod
    {3}
    left outer join Splity S on S.GrSplitu = P.GrSplitu and D.Data between S.DataOd and ISNULL(S.DataDo, D.Data)";

                /*
//     0       1         2                  3                4        5     6              7                 8        9 
                DataSet ds = Base.getDataSet(db.con, String.Format(@"
select PP.Data,PP.CzasZm,PP.NadgodzinyDzien,PP.NadgodzinyNoc,PP.Nocne,A.Kod,AK.GodzinPracy,Z.Id as TypZmiany,K.Rodzaj,PP.Akceptacja,
    PP.Id, P.KadryId, P.Id as PracId, 
    P.GrSplitu, S.Id as IdSplitu, 
    PP.IdZmiany as IdZmianyPlan, ZP.Symbol as SymbolZmianyPlan, ZP.Nazwa as NazwaZmianyPlan, ZP.Od as ZmianaOdPlan, ZP.Do as ZmianaDoPlan, ZP.NadgodzinyDzien as ZmianaNadgDzienPlan, ZP.NadgodzinyNoc as ZmianaNadgNocPlan,
    Z.Id as IdZmiany, Z.Symbol as SymbolZmiany, Z.Nazwa as NazwaZmiany, Z.Od as ZmianaOd, Z.Do as ZmianaDo, Z.NadgodzinyDzien as ZmianaNadgDzien, Z.NadgodzinyNoc as ZmianaNadgNoc,
    PP.CzasIn, PP.CzasOut,
    PP.Uwagi
from PlanPracy PP
    left outer join Zmiany ZP on ZP.Id = PP.IdZmiany
    left outer join Zmiany Z on Z.Id = ISNULL(PP.IdZmianyKorekta, PP.IdZmiany)
    left outer join Kalendarz K on K.Data = PP.Data
    left outer join Absencja A on A.IdPracownika = PP.IdPracownika and PP.Data between A.DataOd and A.DataDo
    left outer join AbsencjaKody AK on AK.Kod = A.Kod
    {3}
    left outer join Splity S on S.GrSplitu = P.GrSplitu and PP.Data between S.DataOd and ISNULL(S.DataDo, PP.Data)
where PP.Data between '{0}' and '{1}' and PP.IdPracownika={2}
",
                    DateFrom, DateTo, pracId,
                    joinPracownicy,
                    ok.Id,

                    ));
                */

                DataSet ds;
                if (ok.IsArch())
                    ds = db.getDataSet(String.Format(select, DateFrom, _DateTo, pracId,
                        "left outer join PracownicyOkresy P on P.Id = " + pracId + " and P.IdOkresu = " + ok.Id));
                else
                    ds = db.getDataSet(String.Format(select, DateFrom, _DateTo, pracId,
                        "left outer join Pracownicy P on P.Id = " + pracId));

                int cnt = ds.Tables[0].Rows.Count;
                bool absPraca = false;

                string idSplitu = null;
                int idPrzypisania = 0;

                if (cnt == 0)               //tu po zmienie sql w sumie nie wejdzie ...
                {
                    _allAccepted = false;  // brak danych, split trzeba pobrać na koniec okresu // <<<<<<<<<<, tu mozna by dac ostatni ?????
                    idSplitu = db.getScalar(String.Format(@"
select S.Id from Pracownicy P
left outer join Splity S on S.GrSplitu = P.GrSplitu and '{0}' between S.DataOd and ISNULL(S.DataDo, '20990909')
where P.Id = {1}", _DateTo, pracId));
                }
                else
                {
                    //----- spr czy jest plan na cały miesiąc ----- //20150104 - dla algorytmów bez liczenia i "furtki" z brakiem pp
                    bool isPPmies = false;
                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow dr = Base.getDataRow(ds, i);
                        if (!db.isNull(dr, "IdZmianyKor"))
                        {
                            isPPmies = true;
                            break;
                        }
                    }
                    //----- analiza kolejnych dni ----- 
                    for (int i = 0; i < cnt; i++)
                    {
                        DataRow dr = Base.getDataRow(ds, i);
                        DateTime date = Base.getDateTime(dr, 0, DateTime.MinValue);
                        bool acc = Base.getBool(dr, 9, false);
                        idSplitu = db.getValue(dr, "IdSplitu");                 // split w danym dniu 
                        idPrzypisania = db.getInt(dr, "IdPrzypisania", 0);      // split z przypisań w danym dniu 

                        int ztime, otimeD, otimeN, ntime;

                        ztime = Base.getInt(dr, 1, 0);      // zawsze nawet jak niepoakceptowane
                        otimeD = Base.getInt(dr, 2, 0);
                        otimeN = Base.getInt(dr, 3, 0);
                        ntime = Base.getInt(dr, 4, 0);

                        _allAccepted = false;

                        string abs = Base.getValue(dr, 5);
                        bool isAbsencja = !String.IsNullOrEmpty(abs);
                        int absGodzPracy = Base.getInt(dr, 6, 0);
                        string zmiana = Base.getValue(dr, 7);
                        int _wolne = Base.getInt(dr, 8, -1);     // 0 sobota, 1 niedziela, 2 święto

                        //----- absencje jako czas pracy -----
                        bool isZmiana = !String.IsNullOrEmpty(zmiana);
                        int ztime_abs = ztime;
                        if (isAbsencja)
                        {
                            if (ztime + otimeD + otimeN > 0) absPraca = true;               // także wtedy kiedy tylko nadgodziny !
                            if (absGodzPracy > 0 && _wolne == -1)                           // liczę jako czas pracy jeśli nie było pracy i dzień nie jest wolny (bez sobot, niedziel i świąt)
                                //if (absGodzPracy > 0 && isZmiana)                         // liczę jako czas pracy jeśli nie było pracy i zaplanowana/skorygowana zmiana <<< po staremu bo KP nie pozwala na wprowadzanie i sie rozjedzie
                                if (ztime == 0)                                             // praca ma priorytet
                                    ztime_abs = ((etat > 0 ? etat : 8) * 3600 * absGodzPracy) / 8;
                        }
                        int t50, t100;
                        int d50, d100;
                        bool zeroZm;

                        /*//--- debug 
                        if (Tools.DateToStr(date) == "2016-02-11")
                        {
                            int x = 0;
                        }
                        /**/


                        //--->>>>>
                        //int before6 = Worktime.GetBefore6(dr["CzasIn"], ztime);

                        sumator._SumTimes2(date, zmiana, _wolne >= 1, ztime_abs, otimeD, otimeN, ntime, out t50, out t100, isAbsencja, dr["CzasIn"], out d50, out d100, out zeroZm);   //0-sobota 1-niedziela 2-swieto

                        //----- DaneRCP -----                           
                        bool isPP = !db.isNull(dr, "Id");
                        int idDaneRCP;
                        if (daneRCP && isPP && isPPmies)        // zapisuje dane RCP   //20150104 sprawdzam cały miesiąc czy jest zmiana, bo co innego było jak był pp cały na null i jak nie było pp, tym też załatwię algorytm "pomiń" więc ok
                            idDaneRCP = insertDaneRCP(asseco, dr, t50, t100, zeroZm);
                        else
                            idDaneRCP = 0;  // tu mmozna by poszukać chyba ze !isPP

                        //----- DaneMPK - Podział kosztów wprowadzony przez kierownika i to co zostało na centrum(a) macierzyste; dopełnienia wynikające z zaokrągleń i podział 019 później ... -----

                        if (idDaneRCP != -1)// dodał rekord lub nie dopisuje Danych RCP
                        {
                            if (daneMPK)    // zapisuje Dane MPK
                            {
                                if (!analizeCC(asseco, dr, sumator,                             // PlanPracy w dniu
                                          idDaneRCP, pracId, nrew, date, idSplitu, idPrzypisania, zmiana,      // dzień, zmiana, id Splitu w danym dniu
                                          isAbsencja, abs,
                                          ztime_abs, otimeD, otimeN, ntime, t50, t100, zeroZm))         // 20130131 zmieniam na ztime->ztime_abs zeby tez sie pojawily w cc
                                {
                                    Log.Error(Log.ASSECO, "GetData->analizeCC", String.Format("pracId:{0} data:{1}", db.getValue(dr, "PracId"), Tools.DateToStr(date)));
                                    return false;
                                }
                            }
                        }
                        else                // błąd - nie dodał rekodru !!!
                        {
                            Log.Error(Log.ASSECO, "GetData->insertDaneRCP", String.Format("pracId:{0} data:{1}", db.getValue(dr, "PracId"), Tools.DateToStr(date)));
                            return false;
                        }
                    }
                }
                //----- sumy -----
                _GetSumy(sumator, settings.ZaokrSum, settings.ZaokrSumType,
                        out pptime, out wtime, out _ztime2, out otime50, out otime100, out orest, out ntime2, out htime);
                if (sumyRCP)
                {

                    //------- RepNadgodziny3 -------
                    bool etatErr = false;
                    int nptime = 0;
#if NOMWYM
#if !SIEMENS
                    //error póki co
#endif
                    int ilDniPrac2Sec;
                    int ilDniPracMies2Sec;
                    RepNadgodziny3.GetPracDniPracSec(pracId, DateFrom, _DateTo, _ilDniPracMies, out ilDniPrac2Sec, out ilDniPracMies2Sec);
                    _ilGodzPrac = ilDniPrac2Sec;
                    _ilGodzPracMies = ilDniPracMies2Sec;
                    if (etat <= 0)
                        etatErr = true;
#else
                    int ilDniPrac2;
                    int ilDniPracMies2;
                    RepNadgodziny3._GetPracDniPrac(pracId, DateFrom, _DateTo, _ilDniPracMies, out ilDniPrac2, out ilDniPracMies2);
                    //----- nominalny i nieprzepracowany -----
                    if (etat > 0)
                    {
                        //ilGodzPrac = ilDniPrac * etat;
                        //ilGodzPracMies = ilDniPracMies * etat;
                        _ilGodzPrac = ilDniPrac2 * etat;
                        _ilGodzPracMies = ilDniPracMies2 * etat;
                    }
                    else
                    {
                        //ilGodzPrac = ilDniPrac * 8;
                        //ilGodzPracMies = ilDniPracMies * 8;
                        _ilGodzPrac = ilDniPrac2 * 8;
                        _ilGodzPracMies = ilDniPracMies2 * 8;
                        etatErr = true;
                    }
                    nptime = _ilGodzPrac * 3600 - _ztime2;
#endif
                    //------------------
                    if (nptime < 0) nptime = 0;
                    //----- -----
                    string uwagi = null;
                    if (absPraca) uwagi += ", praca i absencje";
                    if (etatErr) uwagi += ", błąd etatu";
                    if (!insertSumyRCP(asseco, DateFrom, drP, _ilGodzPrac, //nominalny  
                                    Worktime.SecToHourInt(pptime),      //Zaplanowany, 
                                    Worktime.SecToHourInt(wtime),       //Przepracowany,
                                    Worktime.SecToHourInt(nptime),      //Nieprzepracowany, 
                                    Worktime.SecToHourInt(otime50),     //Nadg50, 
                                    Worktime.SecToHourInt(otime100),    //Nadg100, 
                                    Worktime.SecToHourInt(ntime2),      //Wnocy,
                                    Worktime.SecToHourInt(htime),       //NiedzieleSwieta,
                                    String.IsNullOrEmpty(uwagi) ? db.NULL : uwagi.Substring(2)))
                    {
                        Log.Error(Log.ASSECO, "GetData->insertSumyRCP");
                        return false;
                    }
                }
                //----- dane MPK - dopełnienia -----
                if (daneMPK)
                {                                                                           // data końca tygodnia    
                    if (!_AddDopelnienia(_ztime2, 0, 0, otime50, otime100, ntime2, DateFrom, _DateTo, nrew, pracId, idSplitu)) return false;   // ostatni split, jak null to błąd, nadgDzien i noc nie liczę !!! 
                }
            }
            return true;  // ok 
        }

        //----- TESTY z Worktime -----------

        // na później jeżeli w ogóle ...






        //----------------------------------
        private static bool insertCCdop(SqlConnection con,
                                        string nrew, string pracId, string day, int typ,
                                        string cc, int zm, int n50, int n100, int noc)
        {
            if (!String.IsNullOrEmpty(cc))
                if (insertCC(con, 0, nrew, pracId, day, cc, typ,
                    zm, 0, 0, n50, n100, noc,
                    (double)zm / 3600,   // UWAGA !!! Jakby tu dać: Worktime.Round05(zm * wsp, 0) to powiino być to samo w czasach i floatach - PRZETESTOWAć !!!
                    (double)n50 / 3600,
                    (double)n100 / 3600,
                    (double)noc / 3600))
                    return true;
            Log.Error(Log.ASSECO, "insertCCdop()", String.Format("pracId:{0} data:{1} cc:{2} zm:{3} n50:{4} n100:{5} noc:{6}", nrew, day, cc, zm, n50, n100, noc));
            return false;
        }

        //moze odpalić 2 razy - dla splitów <> 019 i = 019, teraz tylko dla <> 019
        public static bool _AddDopelnienia(int czasZm, int nadgD, int nadgN, int nadg50, int nadg100, int nocne,   // sumy
                                          string dataOd, string dataDo, string lpLogo, string pracId, string idSplitu)
        {
            if (!String.IsNullOrEmpty(idSplitu))
            {
                // dopełnienia są dodawane przed podziałem 019 więc jednak nie ograniczam zakresu do innych niż 019
                string ccGrupy = null;
                //string ccGrupy = db.Join(db.getDataSet("select cc from CC where GrSplitu is not null"), 0, ",");
                //if (!String.IsNullOrEmpty(ccGrupy)) ccGrupy = String.Format("not cc in ({0})", ccGrupy);
                // 0 1 2 3     4 5 6 7      8 9 10 11
                DataRow dr = db.getDataRow(String.Format(@"select 
sum(dbo.TimeToSec(ISNULL(CzasZm,'0:00'))),
sum(dbo.TimeToSec(ISNULL(NadgodzinyDzien,'0:00'))),
sum(dbo.TimeToSec(ISNULL(NadgodzinyNoc,'0:00'))),
sum(dbo.TimeToSec(ISNULL(Nocne,'0:00'))),
sum(vCzasZm), 
sum(vNadg50),
sum(vNadg100),
sum(vNocne),
(select top 1 cc from DaneMPK where {3} IdPracownika = {2} and Data between '{0}' and '{1}' group by cc order by sum(vCzasZm) desc, cc) as ccCzasZmMax,
(select top 1 cc from DaneMPK where {3} IdPracownika = {2} and Data between '{0}' and '{1}' group by cc order by sum(vNadg50) desc, cc) as ccNadg50Max,
(select top 1 cc from DaneMPK where {3} IdPracownika = {2} and Data between '{0}' and '{1}' group by cc order by sum(vNadg100) desc, cc) as ccNadg100Max,
(select top 1 cc from DaneMPK where {3} IdPracownika = {2} and Data between '{0}' and '{1}' group by cc order by sum(vNocne) desc, cc) as ccNocneMax
from DaneMPK 
where {3} Data between '{0}' and '{1}' and IdPracownika = {2}", dataOd, dataDo, pracId, ccGrupy));
                double vZm = db.getDouble(dr, 4, 0);
                double vN50 = db.getDouble(dr, 5, 0);
                double vN100 = db.getDouble(dr, 6, 0);
                double vNoc = db.getDouble(dr, 7, 0);

                int zm = czasZm - (int)Worktime.Round05(vZm * 3600, 0);
                int nd = 0; // nadgD - db.getInt(dr, 1, 0); nie liczę !!!
                int nn = 0; // nadgN - db.getInt(dr, 2, 0);
                int n50 = nadg50 - (int)Worktime.Round05(vN50 * 3600, 0);
                int n100 = nadg100 - (int)Worktime.Round05(vN100 * 3600, 0);
                int noc = nocne - (int)Worktime.Round05(vNoc * 3600, 0);

                string ccCzasZmMax = db.getValue(dr, 8);
                string ccNadg50Max = db.getValue(dr, 9);
                string ccNadg100Max = db.getValue(dr, 10);
                string ccNocneMax = db.getValue(dr, 11);

                //----- dopisuję na końcu okresu rozliczeniowego -----
                //--- dopełnienia -> do największego cc
                if (zm > 0) insertCCdop(db.con, lpLogo, pracId, dataDo, ctDopelnienia, ccCzasZmMax, zm, 0, 0, 0);   // 3 - dopełnienia 
                if (n50 > 0) insertCCdop(db.con, lpLogo, pracId, dataDo, ctDopelnienia, ccNadg50Max, 0, n50, 0, 0);
                if (n100 > 0) insertCCdop(db.con, lpLogo, pracId, dataDo, ctDopelnienia, ccNadg100Max, 0, 0, n100, 0);
                if (noc > 0) insertCCdop(db.con, lpLogo, pracId, dataDo, ctDopelnienia, ccNocneMax, 0, 0, 0, noc);
                /*--- dopełnienia -> wg splitu macierzystego
                if (!insertCCsplit(db.con, lpLogo, pracId, dataDo, idSplitu, 0, ctDopelnienia, zm, nd, nn, n50, n100, noc))   // 3 - dopełnienia 
                {
                    Log.Error(Log.ASSECO, "AddDopelnienia", String.Format("pracId:{0}", lpLogo));
                    return false;
                } /**/



                /*----- tylko współczynnik <<< na razie nie daje - do ustalenia !!! -------------------
                if (czasZm == 0 && nadg50 == 0 && nadg100 == 0 && nocne == 0 &&   // nie ma wartosci - nic nie dodał i nie bedzie dopełnien ... <<< spr to jeszcze
                    zm <= 0 && n50 <= 0 && n100 <= 0 && noc <= 0)
                {
                    if (!insertCCsplitMac(db.con, lpLogo, dataDo, idSplitu, 0, 8)) 
                    {
                        Log.Error(Log.ASSECO, "AddSplitMacierzysty", String.Format("pracId:{0}", lpLogo));
                        return false;
                    }
                }
                /**/
                //------------------------
            }
            else
            {
                Log.Error(Log.ASSECO, "AddDopelnienia.NoGrSplitu", String.Format("pracId:{0}", lpLogo));
                //return false;    // nie wywalam !!! split byc powinien - a błąd był dla testowego pracownika ...
            }
            return true;
        }

        //20141111 uwzglednia 019 i coś   <<<< BŁĄD W ZAŁOŻENIACH: w DaneMPK są już wartości podizelone splitem macierzystym, tu tylko dzielimy 019 wg 019 !!! - przywracam poprzednią wersję !!! 20141231
        public static bool xxx_UpdateSplityGrupy(string dataOd, string dataDo)  // będzie to robić dzień po dniu, splity bierze co do daty <<<< przydało by się dodać CC uzależnione od daty !!! (ale w okresie od-do a nie na dzien)
        {   // typ = 6 -> +10
            db.execSQL(String.Format(@"
insert into DaneMPK (IdDaneRCP,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,Data,NR_EW,IdPracownika,Typ,vCzasZm,vNadg50,vNadg100,vNocne)
select M.IdDaneRCP, WC.cc, null, 
dbo.ToTime(dbo.TimeToSec(M.CzasZm)          * W.Wsp * W0.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyDzien) * W.Wsp * W0.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyNoc)   * W.Wsp * W0.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nocne)           * W.Wsp * W0.Wsp),
null,
dbo.ToTime(dbo.TimeToSec(M.Nadg50)  * W.Wsp * W0.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nadg100) * W.Wsp * W0.Wsp),
M.Data, M.NR_EW, M.IdPracownika,
--6, 
M.Typ + 10,
vCzasZm  * W.Wsp * W0.Wsp,	
vNadg50  * W.Wsp * W0.Wsp,
vNadg100 * W.Wsp * W0.Wsp,
vNocne   * W.Wsp * W0.Wsp
from CC 
inner join DaneMPK M on M.cc = CC.cc

left join Pracownicy P on P.Id = M.IdPracownika
left join Splity S0 on S0.GrSplitu = P.GrSplitu and M.Data between S0.DataOd and ISNULL(S0.DataDo, M.Data)
left join SplityWsp W0 on W0.IdSplitu = S0.Id and W0.IdCC = CC.Id

inner join Splity S on S.GrSplitu = CC.GrSplitu and M.Data between S.DataOd and ISNULL(S.DataDo, M.Data)
inner join SplityWsp W on W.IdSplitu = S.Id 
left join CC WC on WC.Id = W.IdCC
where CC.GrSplitu is not null and M.Data between '{0}' and '{1}'", dataOd, dataDo));

            /*----- 20130420 wyłaczam usuwanie splitu 019 (grup splitów) i sprawdzanie, w raportach będzie uaktualnienie i przy kopiowaniu do asseco tez
            //string cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Typ=6 and Data between '{0}' and '{1}'", dataOd, dataDo));
            string cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Typ >= 10 and Data between '{0}' and '{1}'", dataOd, dataDo));  // podzielone grupy mają typ+10
            if (cnt != "0")             // coś przekopiował !!!, jak nie ma rekordów to lepiej nie usuwać !!!
            {
                //db.execSQL(String.Format("delete from DaneMPK where Data between '{0}' and '{1}' and cc in (select cc from CC where GrSplitu is not null)", dataOd, dataDo));  // usunięcie rozbitych rekordów
            }
            else                        // nic nie przekopiował tzn. ze nic nie było na 019 ?
            {
                cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Data between '{0}' and '{1}' and cc in (select cc from CC where GrSplitu is not null)", dataOd, dataDo));
                if (cnt != "0")         // spr czy coś było na 019 ?
                {                       // jeśli tak to błąd !
                    Log.Error(Log.ASSECO, "UpdateSplityGrupy - brak podziału", String.Format("cnt:{0}", cnt));
                    return false;
                }
            }
             */
            return true;
        }


        public static bool UpdateSplityGrupy(string dataOd, string dataDo)  // będzie to robić dzień po dniu, splity bierze co do daty <<<< przydało by się dodać CC uzależnione od daty !!! (ale w okresie od-do a nie na dzien)
        {   // typ = 6 -> +10
            db.execSQL(String.Format(@"
insert into DaneMPK (IdDaneRCP,cc,Nazwa,CzasZm,NadgodzinyDzien,NadgodzinyNoc,Nocne,Uwagi,Nadg50,Nadg100,Data,NR_EW,IdPracownika,Typ,vCzasZm,vNadg50,vNadg100,vNocne)
select M.IdDaneRCP, WC.cc, null, 
dbo.ToTime(dbo.TimeToSec(M.CzasZm) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyDzien) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyNoc) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nocne) * W.Wsp),
null,
dbo.ToTime(dbo.TimeToSec(M.Nadg50) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nadg100) * W.Wsp),
M.Data, M.NR_EW,M.IdPracownika,
--6, 
M.Typ + 10,
vCzasZm  * W.Wsp,	
vNadg50  * W.Wsp,
vNadg100 * W.Wsp,
vNocne   * W.Wsp
from CC 
inner join DaneMPK M on M.cc = CC.cc
inner join Splity S on S.GrSplitu = CC.GrSplitu and M.Data between S.DataOd and ISNULL(S.DataDo, M.Data)
inner join SplityWsp W on W.IdSplitu = S.Id 
left outer join CC WC on WC.Id = W.IdCC
where CC.Grupa = 1 and CC.GrSplitu is not null and M.Data between '{0}' and '{1}'", dataOd, dataDo));

            /*----- 20130420 wyłaczam usuwanie splitu 019 (grup splitów) i sprawdzanie, w raportach będzie uaktualnienie i przy kopiowaniu do asseco tez
            //string cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Typ=6 and Data between '{0}' and '{1}'", dataOd, dataDo));
            string cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Typ >= 10 and Data between '{0}' and '{1}'", dataOd, dataDo));  // podzielone grupy mają typ+10
            if (cnt != "0")             // coś przekopiował !!!, jak nie ma rekordów to lepiej nie usuwać !!!
            {
                //db.execSQL(String.Format("delete from DaneMPK where Data between '{0}' and '{1}' and cc in (select cc from CC where GrSplitu is not null)", dataOd, dataDo));  // usunięcie rozbitych rekordów
            }
            else                        // nic nie przekopiował tzn. ze nic nie było na 019 ?
            {
                cnt = db.getScalar(String.Format("select count(*) from DaneMPK where Data between '{0}' and '{1}' and cc in (select cc from CC where GrSplitu is not null)", dataOd, dataDo));
                if (cnt != "0")         // spr czy coś było na 019 ?
                {                       // jeśli tak to błąd !
                    Log.Error(Log.ASSECO, "UpdateSplityGrupy - brak podziału", String.Format("cnt:{0}", cnt));
                    return false;
                }
            }
             */
            return true;
        }




        public static int x_analizeDataCC(SqlConnection asseco)
        {
            //DataSet ds = db.getDataSet(asseco, "select * from DaneMPK where"
            return 1;
            /*
             * ODPALIC Z RĘKI !!! POKI CO
             * 
insert into DaneMPK
select M.IdDaneRCP, WC.cc, null, 
dbo.ToTime(dbo.TimeToSec(M.CzasZm) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyDzien) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.NadgodzinyNoc) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nocne) * W.Wsp),
null,
dbo.ToTime(dbo.TimeToSec(M.Nadg50) * W.Wsp),
dbo.ToTime(dbo.TimeToSec(M.Nadg100) * W.Wsp),
M.Data, M.NR_EW, 2, 
vCzasZm * W.Wsp,	
vNadg50 * W.Wsp,
vNadg100 * W.Wsp,
vNocne * W.Wsp
from CC 
inner join DaneMPK M on M.cc = CC.cc
inner join Splity S on S.GrSplitu = CC.GrSplitu
inner join SplityWsp W on W.IdSplitu = S.Id
left outer join CC WC on WC.Id = W.IdCC
where CC.GrSplitu is not null

----- usunięcie rozbitych rekordów - póki co na sztywno 019
--delete from DaneMPK where Data between '2012-11-21' and '2012-12-31' and cc = '019'


             * 
             * I DOPISANIE RÓZNIC Z ZAOKRAGLENIA NADGODZIN DO PEŁNYCH H, DO MACIERZYSTYCH CC 
            */
        }



        public static int ExportSplityMacierzyste(SqlConnection asseco, string dataOd, string dataDo, Okres ok)
        {       // typ = 0 , bez splitów przypiętych do CC np 019, tylko splity do ludzi 

            string cnt1 = db.getScalar(asseco, String.Format("select count(*) from DaneMPK2 where Data between '{0}' and '{1}'", dataOd, dataDo));

            db.execSQL(asseco, String.Format("delete from DaneMPK2 where Data between '{0}' and '{1}'", dataOd, dataDo));
            //----- spr -----
            string cnt2 = db.getScalar(asseco, String.Format("select count(*) from DaneMPK2 where Data between '{0}' and '{1}'", dataOd, dataDo));
            Log.Error(Log.ASSECO, "ExportSplityMacierzyste", String.Format("{0} - {1} Ilość: {2} -> {3}", dataOd, dataDo, cnt1, cnt2));

            string prac;
            if (!ok.IsArch())
                prac = "Pracownicy P on";
            else
                prac = String.Format("PracownicyOkresy P on P.IdOkresu = {0} and", ok.Id);

            //20141111 dodane * W.Wsp - będzie można dać 019 i coś
            return db.ExportTable("DaneMPK2", false, asseco, String.Format(@"    
select ISNULL(C1.cc, CC.cc), null, S.DataOd, P.KadryId, 0, ISNULL(W1.Wsp * W.Wsp, W.wsp)
--select CC.cc,null,S.DataOd,P.KadryId,0,W.wsp
--select distinct S.DataOd,P.KadryId
from Splity S
left outer join SplityWsp W on W.IdSplitu = S.Id
left outer join CC on CC.Id = W.IdCC

left outer join Splity S1 on S1.GrSplitu = CC.GrSplitu and '{0}' between S1.DataOd and ISNULL(S1.DataDo, '20990909')
left outer join SplityWsp W1 on W1.IdSplitu = S1.Id
left outer join CC C1 on C1.Id = W1.IdCC

left outer join {1} P.GrSplitu = S.GrSplitu 
    --and P.Status >= 0

left outer join PodzialLudziImport K on K.KadryId = P.KadryId and K.Miesiac between S.DataOd and ISNULL(S.DataDo, '20990909')
where '{0}' between S.DataOd and ISNULL(S.DataDo, '20990909')
and W.Id is not null and P.Id is not null
and K.KadryId is not null
            ", dataDo, prac));

            /*
            select CC.cc,null,S.DataOd,P.KadryId,0,W.wsp
            from Splity S
            left outer join SplityWsp W on W.IdSplitu = S.Id
            left outer join CC on CC.Id = W.IdCC
            left outer join {1} P.GrSplitu = S.GrSplitu 
                --and P.Status >= 0
            left outer join VKlasyfikacja K on K.Logo = P.KadryId
            where '{0}' between S.DataOd and ISNULL(S.DataDo, '{0}')
            and W.Id is not null and P.Id is not null
            and K.Logo is not null", dataOd, prac));
            */
            /*
            select CC.cc,null,S.DataOd,P.KadryId,0,W.wsp
            from Splity S
            left outer join SplityWsp W on W.IdSplitu = S.Id
            left outer join CC on CC.Id = W.IdCC
            left outer join {1} P.GrSplitu = S.GrSplitu and P.Status >= 0
            where '{0}' between S.DataOd and ISNULL(S.DataDo, '{0}')
            and W.Id is not null and P.Id is not null", dataOd, prac));
             */
            /*
            select CC.cc, null, S.DataOd, P.KadryId, 0, W.wsp	
            from Splity S
            left outer join CC G on G.GrSplitu = S.GrSplitu
            left outer join SplityWsp W on W.IdSplitu = S.Id
            left outer join CC on CC.Id = W.IdCC
            left outer join Pracownicy P on P.GrSplitu = S.GrSplitu
            where S.DataOd >= '20130101' and S.DataDo is null 
            and W.Id is not null and G.Id is null
            */
            //<<<< mozna by jeszcze sprawdzić czy sumu są = 1 !!! - wiec moze view z tego zrobic ...
        }
















        //-----------------------------------------
        //  IMPORT Z ASSECO
        //-----------------------------------------
        public static int ImportABSENCJA(SqlConnection asseco)
        {
            string startData = db.strParam("2011-11-01");
            //Typ	Id	LpLogo	DataOd	DataDo	Kod	IleDni	Godzin	Zalegly	Planowany	NaZadanie	Rok	Miesiac	Korekta	IdKorygowane	Aktywny
            //----- urlopy -----
            int cntU = db.ImportTable("bufAbsencja", true, asseco, @"
select 'U',lp_UrlopyId,LpLogo,DataOd,DataDo,T.lp_UrlopyTypId,Dni,Godziny,Zalegly,Planowany,NaZadanie,Rok,Miesiac,Korekta,lp_UrlopyIdK,U.Aktywny
from lp_urlopy U
left outer join lp_urlopytyp T on T.UrlopTyp = U.UrlopTyp
where DataOd > " + startData);
            //----- zasiłki -----
            int cntZ = db.ImportTable("bufAbsencja", false, asseco, String.Format(@"
select 'Z',lp_ZasilkiId,LpLogo,DataOd,DataDo,T.lp_ZasilkiTypId,LDni,LDni*8,0,0,0,Rok,Miesiac,Korekta,lp_ZasilkiIdK,Z.Aktywny 
from lp_zasilki Z
left outer join lp_zasilkityp T on T.ZasilekTyp = Z.ZasilekTyp
where DataOd > {0}",
                 startData));         // - problem z pół etatu Kristina - moga sie zduplikowane pojawic - filtrowane/sumowane pózniej przy zapisie do Absencja
            //----- aktualizacja:1:pozostawienie ostatniego rekordu z korekty -----
            db.execSQL(@"
delete from bufAbsencja where Id in 
(select A.Id from bufAbsencja A 
left outer join bufAbsencja K on K.Typ = A.Typ and K.Id= A.IdKorygowane
where (A.Korekta = 1 or A.Id in (select distinct IdKorygowane from bufAbsencja where Korekta = 1))
and (A.Korekta = 0 or A.Id <> (select MAX(Id) from bufAbsencja where Typ = A.Typ and IdKorygowane = A.IdKorygowane)))

update bufAbsencja set Kod = 1000 where NaZadanie = 1

delete from Absencja

insert into Absencja 
select P.Id, A.LpLogo, A.DataOd, A.DataDo, A.Kod, 
case when A.IleDni = 0 and A.Kod = 1000090083 then 1 else A.IleDni end, 
sum(A.Godzin)
from bufAbsencja A left outer join Pracownicy P on P.KadryId = A.LpLogo
where A.IleDni > 0 
or (A.IleDni = 0 and A.Kod = 1000090083)  -- odbior dni wolnych za święto 
group by P.Id, A.LpLogo, A.DataOd, A.DataDo, A.Kod, A.IleDni
            ");
            /*
insert into Absencja 
select P.Id, A.LpLogo, A.DataOd, A.DataDo, A.Kod, A.IleDni, A.Godzin 
from bufAbsencja A left outer join Pracownicy P on P.KadryId = A.LpLogo
where A.IleDni > 0
group by P.Id, A.LpLogo, A.DataOd, A.DataDo, A.Kod, A.IleDni
             */

            int d = cntU < 0 || cntZ < 0 ? -1 : 1;
            return (Math.Abs(cntU) + Math.Abs(cntZ)) * d;
        }

        public static int ImportKODYABS(SqlConnection asseco)
        {
            int cnt = 0;
            int d = 1;
            db.execSQL("update AbsencjaKody set Status = -1 where Status >= 0");   // wszyscy jako starzy
            //     0          1                     2                    3                                                 4
            // 5      6      7     8         9      10         11
            DataSet ds = db.getDataSet(asseco, @"
select 'U' as typ, lp_UrlopyTypId as Id, NazwaObca as Symbol, ISNULL(OpisObcy, ISNULL(Opis, Nazwa)) as OpisRCP, Aktywny, 
UrlopTyp, Nazwa, Opis, OpisObcy, Kolor, KolorPlan, UrlopTyp_Oznaczenie
from lp_urlopytyp
union
select 'Z', lp_ZasilkiTypId, NazwaObca, ISNULL(OpisObcy, ISNULL(Opis, Nazwa)), Aktywny, 
ZasilekTyp, Nazwa, Opis, OpisObcy, Kolor, null, ZasilekTyp_Oznaczenie  
from lp_zasilkityp
            ");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string typ = Base.getValue(dr, 0);
                string id = Base.getValue(dr, 1);
                string symbol = Base.getValue(dr, 2);
                string nazwa = Base.getValue(dr, 3);
                int aktywny = Base.getInt(dr, 4, 0);

                DataRow drAK = db.getDataRow("select * from AbsencjaKody where Kod = " + id);
                if (drAK == null)       // brak kodu - nowy kod
                {
                    bool ok = db.insert("AbsencjaKody", 0, "Kod, Nazwa, Symbol, Widoczny, Status",
                                   id, db.strParam(nazwa), db.strParam(symbol), 1, aktywny);
                    if (!ok)
                    {
                        Log.Error(Log.IMPORT_ASSECO, "Import ASSECO.KODY_ABSENCJI", null);
                        d = -1;
                    }
                }
                else
                {
                    int status = db.getInt(drAK, "Status", -99);
                    if (status != AdmKodyAbs.stDodatkowy)       // jak nie ma lub różny od dodatkowy to aktualizuję/dodaję
                    {
                        bool ok = db.update("AbsencjaKody", 0, "Nazwa,Symbol,Status",
                            "Kod=" + id,
                            db.strParam(nazwa),
                            db.strParam(symbol), 0);
                        if (!ok)
                        {
                            Log.Error(Log.IMPORT_ASSECO, "Import ASSECO.KODY_ABSENCJI.Update", String.Format("id: {0} symbol: {1} nazwa: {2}", id, symbol, nazwa));
                            d = -1;
                        }
                    }
                }

                /*
                                 DataRow drAK = db.getDataRow("select * from AbsencjaKody where Kod = " + id);
                int status = drAK != null ? db.getInt(drAK, "Status", -99) : -99;
                if (status != AdmKodyAbs.stDodatkowy)       // jak nie ma lub różny od dodatkowy to aktualizuję/dodaję
                {
                    bool ok = db.update("AbsencjaKody", 0, "Nazwa,Symbol,Status",
                        "Kod=" + id,
                        db.strParam(nazwa),
                        db.strParam(symbol), 0);
                    if (!ok)
                    {
                        ok = db.insert("AbsencjaKody", 0, "Kod, Nazwa, Symbol, Widoczny, Status",
                                       id, db.strParam(nazwa), db.strParam(symbol), 1, aktywny);
                        if (!ok)
                        {
                            Log.Error(Log.IMPORT_ASSECO, "Import ASSECO.KODY_ABSENCJI", null);
                            d = -1;
                        }
                    }
                }
                */
                cnt++;
            }
            return cnt * d;
        }
        //-------------------------------------------------------------------------

        public static int ImportKALENDAR(SqlConnection asseco)
        {
            db.execSQL("delete from Kalendarz where Data >= '2013-01-01'");
            return db.ImportTable("Kalendarz", false, asseco, @"
declare @df int 
set @df = @@DATEFIRST
set datefirst 6

select Data, 
case when DATEPART(WEEKDAY,Data) > 2 and DzienTyp = '$' then 2 
else DATEPART(WEEKDAY,Data) - 1 end, 
Nazwa
from lp_szablonykalendarzydni
where Szablon = 'GLOWNY' and DzienTyp <> 'P' and Data >= '2013-01-01'

set datefirst @df");
        }



        public static int ImportCZASNOM(SqlConnection asseco)
        {
            db.execSQL("delete from CzasNom where Data >= '2013-01-01'");
            return db.ImportTable("CzasNom", false, asseco, @"
select CONVERT(varchar(7), Data, 20) + '-01', COUNT(*) 
from lp_szablonykalendarzydni
where Szablon = 'GLOWNY' and DzienTyp = 'P' and Data >= '2013-01-01'
group by CONVERT(varchar(7), Data, 20)");
        }







        /*
         ------- wyliczenie wymiaru urlopu do końca roku
A-proporcjnonalnie 20 do daty zmiany, 26 proporcjonalnie do końca roku
B-proporcjnonalnie 26 do końca roku
C-proporcjnonalnie 20 do końca roku +6 
         
         
declare @DataZatrudnienia datetime
declare @dataZmiany datetime
declare @boy datetime
declare @eoy datetime
declare @WymiarRok int
set @boy = '20140101'
set @DataZatrudnienia = '20140203'
set @dataZmiany = '20140101'
set @eoy = dbo.eoy(@boy)
set @WymiarRok = 26

select
case when @DataZatrudnienia > @boy then              ------- wyliczenie wymiaru urlopu do końca roku 
    case when @dataZmiany between @DataZatrudnienia and @eoy then 
		ceiling(cast(20 as float) * (MONTH(@dataZmiany) - MONTH(@DataZatrudnienia)) / 12) +
		ceiling(cast(26 as float) * (13 - MONTH(@dataZmiany)) / 12) 
	else ceiling(cast(@WymiarRok as float) * (13 - MONTH(@DataZatrudnienia)) / 12)
    end
else @WymiarRok
end as UrlopNomRok_A

select
case when @DataZatrudnienia > @boy then              ------- wyliczenie wymiaru urlopu do końca roku
	ceiling(cast(@WymiarRok as float) * (13 - MONTH(@DataZatrudnienia)) / 12)
else @WymiarRok
end as UrlopNomRok_B

select
case when @DataZatrudnienia > @boy then              ------- wyliczenie wymiaru urlopu do końca roku
    case when @dataZmiany between @DataZatrudnienia and @eoy then 
		ceiling(cast(20 as float) * (13 - MONTH(@DataZatrudnienia)) / 12) + 6
	else ceiling(cast(@WymiarRok as float) * (13 - MONTH(@DataZatrudnienia)) / 12)
    end
else @WymiarRok
end as UrlopNomRok_C
         
         
         
         */

        //20151022 uwzględnia pierwszą pracę
        public static int ImportZBIOR_code(SqlConnection asseco, DateTime naDzien)   // 
        {
            const string sql = @"
declare @data datetime = '{0}'
declare @dzis datetime = '{1}'
declare @boy datetime
set @boy = DATEADD(dd, 0, DATEDIFF(dd, 0, @dzis))
set @boy = DATEADD(d, -datepart(dy, @boy) + 1, @boy)

--drop table RCP..tmpUrlopZbior2013 

select Rok, NREWID, 
--urlopnom2, urlopzaleg2, WykorzystanyDoDnia, WymiarRok

ISNULL(UrlopNomDni, 0) * 8 * EtatNum,               -- UrlopNom [h]
ISNULL(UrlopZalegDni, 0) * 8 * EtatNum,             -- UrlopZaleg [h]
ISNULL(
case when NREWID = '02159' then WykorzystanyDoDnia  -- Kristina - z dni głupoty zwraca
else UrlopWykDni * 8 * EtatNum
end, 0),                                            -- UrlopWyk [h]

WymiarRok,                                          -- 20/26
UrlopNomDni, UrlopZalegDni, UrlopWykDni,
urlopnom2, urlopzaleg2, WykorzystanyDoDnia, DataZwiekszenia,

case when DataZatrudnienia > @boy then              ------- wyliczenie wymiaru urlopu do końca roku
--    ceiling(cast(WymiarRok as float) * (13 - MONTH(DataZatrudnienia)) / 12)
    ceiling(cast(WymiarRok as float) * (case when PierwszaPracaUrlop = 1 then 12 else 13 end - MONTH(DataZatrudnienia)) / 12) -- odjąć by trzeba jeszcze urlop wykorzystany w innej pracy !!! 20151230
else WymiarRok
end as UrlopNomRok


--,LimitDniZ, LimitDniB, LimitDniNom, WykDniB, WykDniZ, LimitGodzinZ, LimitGodzinB, LimitGodzinNom, WykGodzinyB, WykGodzinyZ
--into RCP..tmpUrlopZbior2014

from -----------
(
SELECT 
YEAR(l.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(l.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(l.LimitGodzinZ + case when l.WykDniZ > 0 then l.WykGodzinyZ else 0 end) urlopzaleg2,     -- fix godz są (błąd) jak dni nie ma (poprawne)
sum(l.WykGodzinyB + case when l.WykDniZ > 0 then l.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(l.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(l2.LimitDniZ) as UrlopZalegDni,
sum(l.WykDniB + l.WykDniZ) as UrlopWykDni,

max(m.wymiar) as WymiarRok,
max(DZ.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia <= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku
outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd <= @data order by DataOd desc) m
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia <= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd <= @dzis order by DataOd desc) m
outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia > @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2

outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(l.Data) and a.Zatrudniony = 1 and a.Wlasny = 1
group by a.LpLogo, YEAR(l.Data)

union 

SELECT 
YEAR(l.Data) as Rok, a.LpLogo NREWID, 
ISNULL(sum(a.EtatNum), 1) as EtatNum,
sum(ISNULL(l.LimitGodzinNom, 0)) urlopnom2,

--sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,
--sum(l.WykGodzinyB + l.WykGodzinyZ) WykorzystanyDoDnia,
sum(l.LimitGodzinZ + case when l.WykDniZ > 0 then l.WykGodzinyZ else 0 end) urlopzaleg2,
sum(l.WykGodzinyB +case when l.WykDniZ > 0 then l.WykGodzinyZ else 0 end) WykorzystanyDoDnia,

sum(l.LimitDniNom) as UrlopNomDni,
--sum(l.LimitDniZ + l.WykDniZ) as UrlopZalegDni,
sum(l2.LimitDniZ) as UrlopZalegDni,
sum(l.WykDniB + l.WykDniZ) as UrlopWykDni,

max(m.wymiar) as WymiarRok,
max(DZ.DataZwiekszenia) as DataZwiekszenia,
min(a.DataZatrudnienia) as DataZatrudnienia,

max(a.PierwszaPracaUrlop) as PierwszaPracaUrlop  --plomba

--,sum(LimitDniZ)LimitDniZ, sum(LimitDniB)LimitDniB, sum(LimitDniNom)LimitDniNom, sum(WykDniB)WykDniB, sum(WykDniZ)WykDniZ	,sum(LimitGodzinZ)LimitGodzinZ, sum(LimitGodzinB)LimitGodzinB, sum(LimitGodzinNom)LimitGodzinNom, sum(WykGodzinyB)WykGodzinyB, sum(WykGodzinyZ)WykGodzinyZ
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia <= @data then a.DataZwolnienia else @data end, 'w') l	-- koniec roku
outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd <= @data order by DataOd desc) m
--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(a.LpLogo, a.UmowaNumer, case when a.DataZwolnienia is not null and a.DataZwolnienia <= @dzis then a.DataZwolnienia else @dzis end, 'w') l	-- na dzień
--outer apply (select top 1 * from dbo.lp_fn_UrlopyLimityEx(a.LpLogo, a.UmowaNumer) where UrlopTyp = 'w' and DataOd <= @dzis order by DataOd desc) m
outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia > @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'w') l2

outer apply (select dbo.lp_fn_UrlopyLimitData26(a.LpLogo, a.UmowaNumer, @dzis, 'Urlopowy', 2) as DataZwiekszenia) DZ

WHERE year(@data) = year(l.Data) and a.Zatrudniony = 0 and a.Wlasny = 1
and a.LpLogo not in (
	select aa.LpLogo 
	from dbo.lp_fn_BasePracExLow(@dzis) aa 
	cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia <= @data then aa.DataZwolnienia else @data end, 'w') ll	-- koniec roku
	--cross APPLY dbo.lp_fn_LimitUrlopuNaDzien(aa.LpLogo, aa.UmowaNumer, case when aa.DataZwolnienia is not null and aa.DataZwolnienia <= @dzis then aa.DataZwolnienia else @dzis end, 'w') ll	-- na dzień
	WHERE year(@data) = year(ll.Data) and aa.Zatrudniony = 1 and aa.Wlasny = 1)
group by a.LpLogo, YEAR(l.Data)

) D
where (urlopnom2 <> 0 or urlopzaleg2 <> 0 or WykorzystanyDoDnia <> 0)
";

            //DateTime dt = DateTime.Today;
            DateTime dt = naDzien;
            string year = dt.Year.ToString();
            int cnt = 0;
            /*
            string year1 = (dt.Year- 1).ToString();
            db.execSQL("delete from UrlopZbior where Rok = " + year1);
            cnt += db.ImportTable("UrlopZbior", false, asseco,
                    String.Format(sql,
                        year1 + "-12-31",
                        year1 + "-12-31"));      // poprzedni rok - na koniec roku
            */
            /*
            db.execSQL("delete from UrlopZbior where Rok = " + year);
            cnt += db.ImportTable("UrlopZbior", false, asseco,
                    String.Format(sql,
                        year + "-12-31",
                        Tools.DateToStrDb(dt)));  // bieżący rok - na dziś
            */
            Log.Info(Log.IMPORT, "Import begin - UrlopZbior", null);
            cnt += db.ImportTable("UrlopZbior", "delete from UrlopZbior where Rok = " + year, asseco,
                    String.Format(sql,
                        year + "-12-31",
                        Tools.DateToStrDb(dt)));  // bieżący rok - na dziś
            Log.Info(Log.IMPORT, "Import end - UrlopZbior", null);
            return cnt;
        }

        // na długi czas wykonywania pomogło dodanie wpisu do Web.config:
        // <system.web>
        //        <httpRuntime maxRequestLength="2097151" executionTimeout="600"/>   // 180 -> 600




        public static int ImportZBIOR_sql(SqlConnection asseco, DateTime naDzien)   // 
        {
            string sql = HRRcp.Controls.Adm.AssecoSQL.ImportUrlopZbior(0, Tools.DateTimeToStr(naDzien));
            if (String.IsNullOrEmpty(sql))
                return -5;
            else
            {
                //DateTime dt = DateTime.Today;
                DateTime dt = naDzien;
                string year = dt.Year.ToString();
                int cnt = 0;

                Log.Info(Log.IMPORT, "Import begin - UrlopZbior SQL", null);
                cnt += db.ImportTable("UrlopZbior", "delete from UrlopZbior where Rok = " + year, asseco,
                        String.Format(sql,
                            year + "-12-31",
                            Tools.DateToStrDb(dt)));  // bieżący rok - na dziś
                Log.Info(Log.IMPORT, "Import end - UrlopZbior SQL", null);
                return cnt;
            }
        }

        public static int ImportZBIOR(SqlConnection asseco, DateTime naDzien)   //20160418 
        {
            int ret = ImportZBIOR_sql(asseco, naDzien);
            if (ret == -5)
                ret = ImportZBIOR_code(asseco, naDzien);
            return ret;
        }

        //------------
        public static int xxx_ImportZBIOR_xxx(SqlConnection asseco)
        {
            //const string sql = "select Rok,NREWID,urlopbie,urlopzal,WykorzystanyDoDnia from w_lp_fn_limityurlopowe('{0}')";
            const string sql = @"
declare @data datetime = '{0}'
declare @dzis datetime = '{1}'

SELECT 
YEAR(@data) Rok,
a.LpLogo NREWID,
sum(ISNULL(l.LimitGodzinNom, 0)) urlopnom2,
sum(l.LimitGodzinZ + l.WykGodzinyZ) urlopzaleg2,

--l.LimitGodzinNom urlopnom,
--l.LimitGodzinB urlopbie, --(godziny urlopu bieżącego do wykorzystania w dniu '20121231')
--l.LimitGodzinZ urlopzal, --(godziny urlopu zaległego do wykorzystania w dniu '20121231')

sum(l.WykGodzinyB+l.WykGodzinyZ) WykorzystanyDoDnia

--,a.Nazwisko,
--a.Imie,
--a.DataZatrudnienia DATAZATR,
--a.DataZwolnienia DATAZWOL,
--st.Wartosc STAWKA,
--kz.KategoriaZasz,
--a.DzialKadryNazwa
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
JOIN dbo.lp_vv_UrlopyLimitowane ul WITH(NOLOCK) ON ul.UrlopTyp = 'w'
--CROSS APPLY dbo.lp_fn_LimitUrlopuNaDzien( a.LpLogo, a.UmowaNumer, @data, ul.UrlopTyp) l
CROSS APPLY dbo.lp_fn_LimitUrlopuNaDzien( a.LpLogo, a.UmowaNumer, ISNULL(a.DataZwolnienia, @data), ul.UrlopTyp) l
/*
LEFT JOIN dbo.lp_PracKategorieZaszeregowan kz WITH (NOLOCK) ON kz.LpLogo = a.LpLogo AND kz.UmowaNumer = a.UmowaNumer
                                              AND @data BETWEEN kz.DataOd AND ISNULL(kz.DataDo,'20990909')             
LEFT JOIN dbo.lp_SkladnikiPracownika st ON st.LpLogo = a.LpLogo AND st.UmowaNumer = a.UmowaNumer 
                                   AND @data BETWEEN st.DataOd AND ISNULL(st.DataDo,'20990909')
                                   AND st.SkladnikRodzaj = 'ANGAZ'
*/
WHERE 
a.Zatrudniony = 1 
--AND (a.Zatrudniony = 1 or YEAR(ISNULL(a.DataZwolnienia, '20990909')) = YEAR(@data))  --2157 dupkey

/*
and 
(ISNULL(l.LimitGodzinNom, 0) > 0 or
l.LimitGodzinZ + l.WykGodzinyZ > 0 or
l.WykGodzinyB+l.WykGodzinyZ > 0
)
*/

AND a.Wlasny =1

group by a.LpLogo
";

            /*
            SELECT 
            YEAR('{0}') Rok,
            a.LpLogo NREWID,
            l.LimitGodzinNom urlopnom2,
            l.LimitGodzinZ + l.WykGodzinyZ as urlopzaleg2,
            --l.LimitGodzinNom urlopnom,
            --l.LimitGodzinB urlopbie, --(godziny urlopu bieżącego do wykorzystania w dniu '20121231')
            --l.LimitGodzinZ urlopzal, --(godziny urlopu zaległego do wykorzystania w dniu '20121231')
            l.WykGodzinyB+l.WykGodzinyZ WykorzystanyDoDnia
            --,a.Nazwisko,
            --a.Imie,
            --a.DataZatrudnienia DATAZATR,
            --a.DataZwolnienia DATAZWOL,
            --st.Wartosc STAWKA,
            --kz.KategoriaZasz,
            --a.DzialKadryNazwa
            FROM dbo.lp_fn_BasePracExLow('{0}') a 
            JOIN dbo.lp_vv_UrlopyLimitowane ul WITH(NOLOCK) ON ul.UrlopTyp = 'w'
            CROSS APPLY dbo.lp_fn_LimitUrlopuNaDzien( a.LpLogo, a.UmowaNumer, a.NaDzien, ul.UrlopTyp) l
            LEFT JOIN dbo.lp_PracKategorieZaszeregowan kz WITH (NOLOCK) ON kz.LpLogo = a.LpLogo AND kz.UmowaNumer = a.UmowaNumer
                                                          AND '{0}' BETWEEN kz.DataOd AND ISNULL(kz.DataDo,'20990909')             
            LEFT JOIN dbo.lp_SkladnikiPracownika st ON st.LpLogo = a.LpLogo AND st.UmowaNumer = a.UmowaNumer 
                                               AND '{0}' BETWEEN st.DataOd AND ISNULL(st.DataDo,'20990909')
                                               AND st.SkladnikRodzaj = 'ANGAZ'
            WHERE a.Zatrudniony = 1 AND a.Wlasny =1";
            */
            //const string sql = "select Rok,NREWID,urlopbie,urlopzal,WykorzystanyDoDnia from w_lp_fn_limityurlopowe";          <<<<<<<<<<<<<<<

            int cnt0 = 0;
            DateTime dt = Tools.bom(DateTime.Today);
            //DateTime dt = Tools.eom(DateTime.Today);
            int year = dt.Year;

            db.execSQL("delete from UrlopZbior where Rok >= " + year.ToString());


            //db.execSQL("delete from UrlopZbior where Rok >= 2012");  // TESTY !!!!! wyłączyć !!!!

            /*
            if (dt.Year > 2013)
                cnt0 = db.ImportTable("UrlopZbior", false, asseco, String.Format(sql, (dt.Year - 1).ToString() + "-12-31"));  // na koniec roku
            int cnt1 = db.ImportTable("UrlopZbior", false, asseco, String.Format(sql, Tools.DateToStr(dt)));
            
            int d = cnt0 < 0 || cnt1 < 0 ? -1 : 1;
            return (Math.Abs(cnt0) + Math.Abs(cnt1)) * d;
             */

            //ewentualnie dołożyć przez pierwszy miesiac import tez roku poprzedniego ... albo zawsze 2 lat, 2012 zwraca 0 w asseco ...!!!
            //20130822 zmiana na koniec roku wymiar urlopu, lista pracowników na dzień importu - załapuje w ten sposób pracowników którym się kończy umowa
            int cnt = db.ImportTable("UrlopZbior", false, asseco,
                    String.Format(sql,
                        dt.Year.ToString() + "-12-31",
                        Tools.DateToStrDb(DateTime.Today)));  // na koniec roku
            return cnt;
        }


        //20140112 
        public static int ImportUMOWY(SqlConnection asseco)
        {
            string sql = HRRcp.Controls.Adm.AssecoSQL.ImportUmowy(0);
            if (String.IsNullOrEmpty(sql))
                sql = @"
select lp_UmowyId, LpLogo, UmowaNumer, DataOd, DataDo, UmowaOd, UmowaDo, DataZatrudnienia, DataZwolnienia, UmowaTyp, DataRozpoczeciaPracy, DataRozwUmowy,
-1, -1
from lp_Umowy";
            //db.execSQL("update PracownicyUmowy set Status = -1");
            int cnt = db.ImportTable("PracownicyUmowy", true, asseco, sql);
            db.execSQL(@"
update PracownicyUmowy set IdPracownika = P.Id, Status = 0
from PracownicyUmowy PU 
inner join Pracownicy P on P.KadryId = PU.LpLogo");
            return cnt;
        }

        public static int ImportLIMITY(SqlConnection asseco)
        {
            int cnt = db.ImportTable("UrlopLimity", true, asseco, @"
select -1, LpLogo, UmowaNumer, DataOd, DataDo, UrlopTyp, Limit, LimitGodzin, Wymiar
from lp_vv_UrlopyLimityExOdDnia");
            db.execSQL(@"
update UrlopLimity set IdPracownika = P.Id
from UrlopLimity UL 
inner join Pracownicy P on P.KadryId = UL.KadryId");



            //----- aktualizacja zaległego urlopu dodatkowego -----
            string boy = Tools.DateToStrDb(Tools.boy(DateTime.Today));
            string rok = boy.Substring(0, 4);

            string sql1 = HRRcp.Controls.Adm.AssecoSQL.ImportUpdateZalegDod(0, boy);
            if (String.IsNullOrEmpty(sql1))
                sql1 = String.Format(@"
declare @boy datetime
set @boy = '{0}'

select 
--P.Nazwisko + ' ' + P.Imie as Pracownik, 
A.*, Z.* 
from 
(
select distinct LpLogo
from lp_vv_UrlopyLimityExOdDnia
where UrlopTyp  = 'd'
) D
outer apply (select top 1 * from lp_vv_UrlopyLimityExOdDnia where LpLogo = D.LpLogo and UrlopTyp = 'd' order by DataOd desc) A
outer apply (select top 1 * from lp_Umowy where LpLogo = D.LpLogo and UmowaNumer = A.UmowaNumer order by DataZatrudnienia desc) U
--outer apply (select * from lp_fn_LimitUrlopuNaDzien(A.LpLogo, A.UmowaNumer, @boy, 'd')) Z
--outer apply (select * from lp_fn_LimitUrlopuNaDzien(A.LpLogo, A.UmowaNumer, case when @boy < A.DataOd then A.DataOd else @boy end, 'd')) Z

outer apply (select * from lp_fn_LimitUrlopuNaDzien(A.LpLogo, A.UmowaNumer, case when @boy < U.DataZatrudnienia then U.DataZatrudnienia else @boy end, 'd')) Z  
--outer apply (select * from dbo.lp_fn_LimitUrlopowLimitowanychNaDzienEx(a.LpLogo, a.UmowaNumer, case when a.DataZatrudnienia > @boy then a.DataZatrudnienia else @boy end) where UrlopTyp = 'd') Z  --20160425 jeszcze po staremu

--left join lp_Pracownicy P on P.LpLogo = D.LpLogo
            ", boy);

            DataSet dsZ = db.getDataSet(asseco, sql1);

            foreach (DataRow dr in db.getRows(dsZ))
            {
                string logo = db.getValue(dr, "LpLogo");
                DataRow drP = db.getDataRow("select * from Pracownicy where KadryId = " + logo);
                if (drP != null)  // == null nie może być
                {
                    int etatL = Base.getInt(drP, "EtatL", -1);
                    int etatM = Base.getInt(drP, "EtatM", -1);
                    if (etatL == -1 || etatM == -1)
                    {
                        etatL = 1;
                        etatM = 1;
                    }
                    int zalDni = db.getInt(dr, "LimitDniZ", 0);
                    int zalGodz = db.getInt(dr, "LimitGodzinZ", 0);
                    if (zalDni > 0)
                    {
                        string sql = String.Format("update UrlopZbior set UrlopZaleg = UrlopZaleg + {0}, UrlopZalegDni = UrlopZalegDni + {1} where KadryId = '{2}' and Rok = {3}",
                            (float)zalDni * 8 * etatL / etatM,
                            zalDni,
                            logo,
                            rok
                            );
                        bool ok = db.execSQL(sql);
                        if (ok)
                            Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.UrlopyLimity.UpdateDodZal", sql);
                        else
                            Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.UrlopyLimity.UpdateDodZal - ERROR", sql);
                    }
                }
            }
            return cnt;
        }





        /*

zzzzzzzzzzzzzzzzzzzzzzzzzz         
--------------------------|------------------------------|--------------------------
                         1     ^             ^      ^   2  ^  ^                 
                               d             d      d      2  2
         *

        */
        // Kristina !!! - po imporcie całości zaciągnąć sumę stawek i uaktualnić tylko ... <<< najprościej





        public static int ImportSTAWKI(SqlConnection asseco) // i nowi pracownicy, aktulizacja danych starym 
        {
            //Okres _o = Okres.LastClosed(db.con);
            //_o.Next();  // bieżący do zamknięcia, nie moze byc Today !!! bo przed zamknieciem miesiaca a w nastepnym bralby zle

            int cnt = 0;
            int _d = 1;
            DateTime dt = DateTime.Today;
            Okres o = Okres.Current(db.con);
            o.Prev();

            string data = Tools.DateToStrDb(dt);
            string dane;
            if (o.Status == Okres.stClosed)         // jeżeli poprzedni miesiąc jest zamknięty to data dzisiejsza
                dane = data;
            else
                dane = Tools.DateToStrDb(o.DateTo); // jak okres nie jest zamknięty, a import już w nowym to dane wg daty końca okresu

            Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI", String.Format("{0}, {1}", data, dane));


            ////DataSet ds = Base.getDataSet(asseco, String.Format(" select * from RCP.dbo.w_lp_fn_pracownicyRCP('{0}')",  // dołożony pesel
            ////                             Tools.DateToStr(o.DateTo)));
            //DataSet ds = Base.getDataSet(asseco, String.Format(" select * from RCP.dbo.w_lp_fn_pracownicyRCP('{0}')",   // dołożony pesel, zatrudniony i własny
            //                             data));                                               // na dziś
            ////DataSet ds = Base.getDataSet(asseco, String.Format("select * from w_lp_fn_pracownicyRCP",  <<<<<<<<<<<<
            ////                             Tools.DateToStr(o.DateTo)));
            //DataSet ds = Base.getDataSet(asseco, String.Format("select * from RCP.dbo.w_lp_fn_pracownicyRCP2('{0}','{1}')",   // dołożony pesel, zatrudniony i własny
            DataSet ds = db.getDataSet(asseco, String.Format("select * from RCP.dbo.w_lp_fn_pracownicyRCP3('{0}','{1}')",     // +zwolnieni
                                         data, dane));                                               // na dziś, dane na koniec otwartego jeszcze okresu lub na dziś

            string findpracsql = AssecoSQL.ImportFindPracSql;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string nr_ew = dr["LpLogo"].ToString();         // spr czy jest
                string pesel = db.getValue(dr, "Pesel");
                string nrdok = db.getValue(dr, "NrDokumentu");
                string typdok = db.getValue(dr, "TypNrDokumentu");
                string nazwpan = db.getValue(dr, "NazwiskoPan");
                string plec = db.getValue(dr, "Plec");

                //                                  0  1        
                ////DataRow drP = db.getDataRow("select Id,Nick,Status from Pracownicy where KadryId = " + Base.strParam(nr_ew));
                //DataRow drP = db.getDataRow(String.Format("select Id,Nick,Status from Pracownicy where KadryId = '{0}' or Status = -6 and Nick = '{1}'", nr_ew, pesel));  // pracownicy z badań wstępnych
                DataRow drP = db.getDataRow(String.Format(findpracsql, nr_ew, pesel, nrdok, typdok));  //+ pracownicy z badań wstępnych, status -6

                //string pid = drP == null ? null : db.getValue(dr, 0);

                double? etat = db.getDouble(dr, "EtatNumeric");
                if (etat == null) etat = 1.0;
                int sign, etatL, etatM;
                Tools.DecimalToFraction((double)etat, out sign, out etatL, out etatM);
                if (String.IsNullOrEmpty(pesel)) pesel = (nr_ew + "00000000000").Substring(0, 11); //20160411 jak nie ma Pesel (pracownicy z Ukrainy) to wchodzi po numerze ewidencyjnym

                string nazwisko = Tools.PrepareName(Base.getValue(dr, "Nazwisko"));
                string imie = Tools.PrepareName(Base.getValue(dr, "Imie"));
                string miejsce = db.getValue(dr, "MiejsceWykPracy");
                string stanid = db.getValue(dr, "StanowiskoId");
                string stan = db.getValue(dr, "Stanowisko");
                string klas = db.getValue(dr, "RodzajPracownika");

                if (drP == null)                  // nowy pracownik
                {
                    string nr_ew_kier = dr["LogoPrzelozony"].ToString();    // kierownik
                    string kierId = String.IsNullOrEmpty(nr_ew_kier) ? null : db.getScalar("select Id from Pracownicy where KadryId = " + db.strParam(nr_ew_kier));

                    bool ok;
                    if (nr_ew.Length > 5)
                        ok = false;
                    else
                    {
                        string login = "login_" + nr_ew;
                        string email = "email_" + nr_ew;
                        string rights = null;
#if IQOR
                        //----- wartości domyślne -----
                        string i = db.RemovePL(imie);
                        string n = db.RemovePL(nazwisko);
                        string m = String.Format("{0}.{1}@iqor.com", i, n);
                        string idM = db.getScalar("select Id from Pracownicy where EMail = " + db.strParam(m));
                        if (String.IsNullOrEmpty(idM))
                            email = m;

                        //m = Tools.Substring(n, 0, 7) + Tools.Substring(i, 0, 1);  //20160228 wg nowych zasad - pełne nazwisko
                        m = n + Tools.Substring(i, 0, 1);

                        idM = db.getScalar("select Id from Pracownicy where Login = " + db.strParam(m));
                        if (String.IsNullOrEmpty(idM))
                            login = m;
                        //rights = new String('0', 50);
                        //rights[AppUser.rPortalTmp] = '1';
                        //rights[AppUser.rWnioskiUrlopowe] = '1';
                        rights = "00000000000000000000000000001001";
                        if (rights[AppUser.rWnioskiUrlopowe] != '1' ||      // kontrola
                            rights[AppUser.rPortalTmp] != '1')
                            rights = null;
#endif
                        ok = db.insert("Pracownicy", 0,
                            "Nazwisko,Imie,Opis,Login,Email,Admin,Kierownik,KadryId,Status,Nick,Pass,IdKierownika,Stawka,EtatL,EtatM,DataZatr,DataZwol,GrSplitu,Rights",

                            Base.strParam(nazwisko),
                            Base.strParam(imie),
                            Base.nullStrParam(miejsce),

                            db.strParam(login),
                            db.strParam(email),
                            //db.strParam("login_" + nr_ew),
                            //db.strParam("email_" + nr_ew),

                            0, 0,
                            db.strParam(nr_ew),
                            App.stNew,                              // nowy pracownik
                            db.strParam(pesel),
                            //db.strParam(FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod)),     // nowe hasło
                            db.NULL,    //20150213 zmieniam - hasło będzie ustawiane przez kliknięcie resetuj hasło nowym pracownikom po ustawieniu maila i loginu

                            db.nullParam(kierId),

                            db.getFloatAsString(dr, "Stawka", Base.NULL),
                            etatL, etatM,
                            Base.nullStrParam(Base.DateToStr(dr["DataZatrudnienia"])),
                            Base.nullStrParam(Base.DateToStr(dr["DataZwolnienia"])),
                            nr_ew,
                            db.strParam(rights));
                    }
                    if (!ok)
                    {
                        Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.NEWPRAC", nr_ew);
                        _d = -1;
                    }
                }
                else                                        // istniejący pracownik
                {
                    string pid = db.getValue(drP, 0);
                    string nick = db.getValue(drP, 1);      // w RCP jest null lub inny wpis - to przenosze 
                    //string pass_reset = String.IsNullOrEmpty(nick) || (nick != pesel) ? ",Nick,Pass" : "";
                    string pass_reset = null;
                    if (String.IsNullOrEmpty(nick))         // brak w RCP
                        pass_reset = ",Nick,Pass";
                    else if (nick != pesel)                 // różny nick - zmiana na assecowy, bez zmiany hasła !!!
                    {
                        pass_reset = ",Nick";
                        Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.NICK - zmiana identyfikatora", nr_ew);
                    }

                    int status = db.getInt(drP, "Status", -1);
                    string dz = Base.DateToStr(dr["DataZwolnienia"]);

                    /* 200160130 dostaje wszystkich - trzeba zmienić !!!
                    if (!String.IsNullOrEmpty(dz) && (status == App.stCurrent || status == App.stNew))   //zwolnieni
                    {
                        DateTime ddz = (DateTime)Tools.StrToDateTime(dz);
                        if (ddz <= dt) status = App.stOld;
                    }
                    else if (status == App.stOld)   //powtónie zatrudnieni 
                    {
                        //DateTime? dZatrRCP = db.getDateTime(drP, "DataZatr");
                        //DateTime? dZatrAsseco = db.getDateTime(dr, "DataZatrudnienia");
                        status = App.stNew;  // nie ma potrzeby kontrolowania dat, bo z Asseco dostaję tylko aktualnych pracowników
                    }
                    */
                    if (!String.IsNullOrEmpty(dz))  // jest data zwolnienia
                    {
                        if (status == App.stCurrent || status == App.stNew)   //zwolnieni
                        {
                            Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI - DataZwolnienia", String.Format("logo:{0} zatr:{1} zwol:{2} status:{3}", nr_ew, Base.DateToStr(dr["DataZatrudnienia"]), dz, status));
                            DateTime ddz = (DateTime)Tools.StrToDateTime(dz);
                            if (ddz <= dt) status = App.stOld;      // dopiero jak wcześniejsza od dzisiaj
                        }
                    }
                    else if (status == App.stOld)   //nie ma daty zwolnienia - powtónie zatrudnieni ?
                    {
                        Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI - DataZwolnienia - Ponowne zatrudnienie", String.Format("logo:{0} zatr:{1} zwol:{2} status:{3}", nr_ew, Base.DateToStr(dr["DataZatrudnienia"]), dz, status));
                        status = App.stNew;
                    }
                    else if (status == App.stBadaniaWst)   // -6
                    {
                        Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI - Badania Wstępne (-6)", String.Format("logo:{0} status:{1} pesel:{2} nrdok:{3} typdok:{4} ", nr_ew, status, pesel, nrdok, typdok));
                        status = App.stNew;
                    }

                    //----- -----
                    //string dz = Base.DateToStr(dr["DataZwolnienia"]);
                    bool ok = db.update("Pracownicy", 0,
                        "KadryId,Nazwisko,Imie,Opis,Stawka,EtatL,EtatM,DataZatr,DataZwol,Status" + pass_reset,         // ostatnie parametry !!!
                        //"KadryId=" + nr_ew,
                        "Id=" + pid,
                        Base.strParam(nr_ew),
                        Base.strParam(nazwisko),
                        Base.strParam(imie),
                        Base.nullStrParam(miejsce),
                        Base.getFloatAsString(dr, "Stawka", Base.NULL),
                        etatL, etatM,
                        Base.nullStrParam(Base.DateToStr(dr["DataZatrudnienia"])),
                        Base.nullStrParam(dz),
                        status,
                        db.strParam(pesel),                                 // ostatnie parametry to muszą być !!!
                        //db.strParam(FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod))     // nowe hasło
                        db.NULL // wyłączam chociaz to nie robi, bo tylko kiedy pesel był null sie ustawiało (w Asseco zawsze jest Pesel)
                        );





                    if (ok) //20141201 - aktualizacja Do w Przypisaniach
                    {
                        if (!String.IsNullOrEmpty(dz))
                        {
                            bool ok2 = db.execSQL(String.Format(@"
declare @dz datetime
declare @pid int 
set @dz = '{1}'
set @pid = {0}

update Przypisania set Do = @dz 
--select * from Przypisania 
where Id = (select case when D.Od < @dz then D.Id else null end 
from (select top 1 * from Przypisania R where IdPracownika = @pid and Status = 1 order by Od desc) D
)
                            ", pid, dz));
                            if (ok2)    // ok2 jezeli affected rows > 0
                                Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI.UpdateDataZwolnienia", String.Format("{0} {1}", nr_ew, dz));
                            else   //20160411
                                Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI.UpdateDataZwolnienia - Brak przypisania do aktualizacji", String.Format("{0} {1}", nr_ew, dz));



                            /*
                            //ok = db.update("Przypisania", 0, "Do", String.Format("IdPracownika={0} and Do is null", pid), db.strParam(dz));
                            else
                            {
                                Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI.UpdateDataZwolnienia - ERROR", nr_ew);
                                _d = -1;
                            }
                            */
                        }
                    }

                    if (!ok)
                    {
                        Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI", nr_ew);
                        _d = -1;
                    }
                }
                cnt++;
            }
            //----- aktualizacja Kristina -----
            ds = db.getDataSet(asseco, String.Format(@"
declare @data datetime
set @data = '{0}'

select LpLogo, sum(Stawka) as Stawka, sum(EtatNumeric) as EtatNumeric
from RCP.dbo.w_lp_fn_pracownicyRCP(@data)
where LpLogo in (
    select LpLogo from RCP.dbo.w_lp_fn_pracownicyRCP(@data)
    group by LpLogo 
    having count(*) > 1
)
group by LpLogo
                ", data));
            foreach (DataRow dr in db.getRows(ds))
            {
                string nr_ew = db.getValue(dr, "LpLogo");
                double? etat = db.getDouble(dr, "EtatNumeric");
                if (etat == null) etat = 1.0;
                int sign, etatL, etatM;
                Tools.DecimalToFraction((double)etat, out sign, out etatL, out etatM);

                bool ok = db.update("Pracownicy", 0,
                    "Stawka,EtatL,EtatM",
                    "KadryId=" + nr_ew,
                    Base.getFloatAsString(dr, "Stawka", Base.NULL),
                    etatL, etatM);
                if (ok)
                    Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI Kristina", String.Format("{0} Etat:{1}/{2}", nr_ew, etatL, etatM));
                else
                    Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI Kristina Error", String.Format("{0} Etat:{1}/{2}", nr_ew, etatL, etatM));
            }

            return cnt * _d;
        }


        /*
    select IdDzialu,Nazwa,Aktywne,StanId,JobCat,Class,Opis,Status,ksId,Grade from Stanowiska
    select IdPracownika,Od,Do,IdDzialu,IdStanowiska,Grupa,Klasyfikacja,Grade from PracownicyStanowiska
         */



        public static int ImportSTANOWISKA(SqlConnection asseco) // i klasyfikacje
        {
            string s1 = @"
----- stanowiska -----
update Stanowiska set Status = -1

update Stanowiska set StanId = RS.Stanowisko, Nazwa = LTRIM(RS.Nazwa), JobCat = RS.KatStan, Class = LEFT(RS.Class, 20), Opis = LEFT(RS.Typ, 20), Status = 0
from Stanowiska S 
inner join JGBHR01.RCP.dbo.VStanowiska RS on RS.lp_StanowiskaId = S.Id

insert into Stanowiska (Id,IdDzialu,Nazwa,Aktywne,StanId,JobCat,Class,Opis,Status,ksId,Grade)
select lp_StanowiskaId, 0, LTRIM(Nazwa), 1, Stanowisko, KatStan, LEFT(Class, 20), LEFT(Typ, 20), 1, null, null 
from JGBHR01.RCP.dbo.VStanowiska 
where lp_StanowiskaId not in (select Id from Stanowiska)

----- przypisania -----
delete from PracownicyStanowiska

--insert into PracownicyStanowiska (IdPracownika, Od, IdDzialu, IdStanowiska, Grupa, Klasyfikacja)
select distinct PS.LpLogo, P.Id as IdPracownika, PS.DataOd as Od, 0 as IdDzialu, PS.lp_StanowiskaId as IdStanowiska, K.Parametr as Grupa, PS.DzialKadryNazwa as Klasyfikacja
into #ppp
from JGBHR01.RCP.dbo.VPracownicyStanowiska PS
inner join Pracownicy P on P.KadryId = PS.LpLogo
left join Kody K on K.Typ = 'PRACKLAS' and K.Nazwa = PS.DzialKadryNazwa
order by PS.LpLogo, PS.DataOd

-- uwzględnia Kristina
insert into PracownicyStanowiska (IdPracownika,Od,Do,IdDzialu,IdStanowiska,Grupa,Klasyfikacja,Grade)
select D.IdPracownika, D.Od, DATEADD(D, -1, B.Od) as Do, A.IdDzialu, A.IdStanowiska, A.Grupa, A.Klasyfikacja, null 
from 
(
select distinct IdPracownika, Od from #ppp
) D
outer apply (select top 1 * from #ppp where IdPracownika = D.IdPracownika and Od = D.Od) A
outer apply (select top 1 * from #ppp where IdPracownika = D.IdPracownika and Od > D.Od order by Od) B
order by A.LpLogo, A.Od
";
            bool ok = db.execSQL(s1);
            if (ok)
            {
                Log.Info(Log.t2APP_IMPORTKP, "Import ASSECO.STANOWISKA", null);
                return 1;
            }
            else
            {
                Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.STANOWISKA Error", null);
                return -1;
            }
        }

























        //--------- WNIOSKI URLOPOWE ------------

        /*
exec dbo.lp_DodajUrlop 
    '01470'
  , 1			--@UmowaNumer TLPUmowaNumer
  , 'w'			--@UrlopTyp TSLLongIdent
  , '0001'		--@UrlopPodTyp varchar(10)
  , '20140611'	--@DataOd TSLDate
  , '20140612'	--@DataDo TSLDate
  , 0			--@Planowany TSLBoolean
  , 0			--@NaZadanie TSLBoolean
  , 'BezPrawa'	--@UrlopWychTyp varchar(10)
  , 0			--@Licz_Zdro_Wych TSLBoolean
  , 0			--@PierwszaPraca TSLBoolean
  , null		--varchar(10)
  , 'import wniosku'	--@Uwagi TSLDescription
  , null		--@RodzajKorekty TSLInteger = NULL -- null/0 bezkorekty, 1 - utworzenie zerujących wpisów, 2 - zerujące plus poprawne
  , null		--@DataOdOkrKorekty TSLDate = NULL
  , null		--@DataDoOkrKorekty TSLDate = NULL
  , null		--@OdbiorData TSLDate = NULL
  , null		--@lp_RodzinaId TSLBigIdent = NULL -- powiązanie z członkiem rodziny
  , null		--@lp_RodzinaIdOpis TSLLongDescription = NULL -- opisik dla rodzinki
  , null		--@IloscGodzinCzescUrlopu TLPCzasNum = NULL
  , null		--@ZasilekPodTyp TSLLongIdent = NULL
  , null		--@__SloMode__ int = NULL
  , null		--@NumerAbsencji TSLDescription = NULL
  , null		--@Licz_Grupa_1 TSLBoolean = NULL
  , null		--@Licz_Grupa_2 TSLBoolean = NULL
  , null		--@Licz_Grupa_3 TSLBoolean = NULL
         */
        public static bool ExportWniosekUrlopowy(SqlConnection con, string logo, string typ, string podtyp, string dOd, string dDo, double? godz, bool naZadanie, string odbiorData, out string msg)
        {   // jak nie zwroci zadnego rekordu to false
            DataRow dr = db.getDataRow(con, String.Format(@"
declare @logo varchar(20) 
declare @od datetime
declare @do datetime
declare @typ varchar(20)
set @logo = '{0}'
set @od = '{1}'
set @od = '{2}'
set @typ = '{3}'

select * from lp_Urlopy where LpLogo = @logo and DataOd = @od and DataDo = @do and UrlopTyp = @typ
                ", logo, dOd, dDo, typ));
            if (dr != null)
            {
                msg = "Absencja o podanych parametrach już istnieje.";
                return false;
            }
            else
            {
                bool ok = db.execSQL(con, String.Format(@"
declare 
    @logo varchar(20),        
    @umowa int,
    @typ varchar(20),
    @podtyp varchar(20),
    @od datetime,
    @do datetime,
    @odbiorData datetime,
    @nazadanie bit,
    @nagodziny bit,
    @pierwsza bit,
    @g1 bit, 
    @g2 bit, 
    @g3 bit, 
    @liczZW bit,
    @godz TLPCzasNum

set @logo = '{0}'
set @typ = '{1}'
set @podtyp = {2}
set @od = '{3}'
set @do = '{4}'
set @nazadanie = {5}
set @odbiorData = {6}
set @godz = {7}

select @umowa = UmowaNumer, @nagodziny = UrlopNaGodziny, @pierwsza = PierwszaPracaUrlop from dbo.lp_fn_BasePracExLow(@od) where LpLogo = @logo
and @od between UmowaOd and ISNULL(UmowaDo,'20990909')  -- moga byc 2 w odwrotnej kolejnosci
--select @umowa, @nagodziny, @pierwsza

select @g1 = Licz_Grupa_1, @g2 = Licz_Grupa_2, @g3 = Licz_Grupa_3, @liczZW = Licz_Zdro_Wych 
from LP_VV_URLOPYTYPYGRUPYEX where Grupa = 'Podst' and UrlopTyp = @typ

exec dbo.lp_DodajUrlop 
    @logo
  , @umowa
  , @typ
  , @podtyp
  , @od
  , @do
  , 0			--@Planowany TSLBoolean
  , @nazadanie
  , null	    --@UrlopWychTyp varchar(10)
  , @liczZW		--@Licz_Zdro_Wych TSLBoolean
  , @pierwsza   --@PierwszaPraca TSLBoolean
  , null		--@KodZus varchar(10)
  , 'import wniosku'	--@Uwagi TSLDescription
  , null		--@RodzajKorekty TSLInteger = NULL -- null/0 bezkorekty, 1 - utworzenie zerujących wpisów, 2 - zerujące plus poprawne
  , null		--@DataOdOkrKorekty TSLDate = NULL
  , null		--@DataDoOkrKorekty TSLDate = NULL
  , @odbiorData	--@OdbiorData TSLDate = NULL
  , null		--@lp_RodzinaId TSLBigIdent = NULL -- powiązanie z członkiem rodziny
  , null		--@lp_RodzinaIdOpis TSLLongDescription = NULL -- opisik dla rodzinki
  , @godz		--@IloscGodzinCzescUrlopu TLPCzasNum = NULL
  , null		--@ZasilekPodTyp TSLLongIdent = NULL
  , null		--@__SloMode__ int = NULL
  , null		--@NumerAbsencji TSLDescription = NULL
  , @g1		    --@Licz_Grupa_1 TSLBoolean = NULL
  , @g2		    --@Licz_Grupa_2 TSLBoolean = NULL
  , @g3		    --@Licz_Grupa_3 TSLBoolean = NULL
                ", logo, typ, db.nullStrParam(podtyp), dOd, dDo,
                    naZadanie ? 1 : 0,
                    db.nullStrParam(odbiorData),
                    db.isNull(godz) ? db.NULL : ((double)godz).ToString()));
                msg = null;
                return true;
            }
        }

        public static bool ExportWniosekUrlopowy(string logo, string typ, string podtyp, string dOd, string dDo, double? godz, bool naZadanie, string odbiorData, bool showError)
        {
            const string info = "Eksport wniosku urlopowego do Asseco HR";
            int lid = Log.Info(Log.EXPORT_ASSECO, info, String.Format("logo: {0} typ: {1} podtyp: {2} od: {3} do: {4} godz: {6} nazadanie: {5}", logo, typ, podtyp, dOd, dDo, naZadanie ? 1 : 0, db.isNull(godz) ? db.NULL : ((double)godz).ToString()), Log.PENDING);

            bool ok = true;
            string msg = null;
            try
            {
                SqlConnection conAsseco = db.Connect(Asseco.ASSECO);
                try
                {
                    ok = ExportWniosekUrlopowy(conAsseco, logo, typ, podtyp, dOd, dDo, godz, naZadanie, odbiorData, out msg);
                }
                finally
                {
                    db.Disconnect(conAsseco);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ok = false;
                //throw;      // nie wyswietla sie showmessage !! wiec na razie zeby pokazac
            }

            if (ok)
            {
                Log.Update(lid, Log.OK);
                return true;
            }
            else
            {
                Log.Update(lid, Log.ERROR);
                Log.Error(Log.EXPORT_ASSECO, info, String.Format("Wystapił błąd podczas eksportu {0}", msg), Log.OK);
                if (showError)
                    Tools.ShowMessage("Wystąpił błąd podczas eksportu wniosku.\\n" + Tools.ToScript(msg));
                return false;
            }
        }

        public static bool ExportWniosekUrlopowy(string wniosekId, bool showError)
        {
            DataRow dr = db.getDataRow(String.Format(@"
select W.*, T.IdKodyAbs, T.Typ2, T.PodTyp2, K.Nazwa, K.Parametr, K.Nazwa2, P.KadryId 
from poWnioskiUrlopowe W 
left join poWnioskiUrlopoweTypy T on T.Id = W.TypId 
left join Kody K on K.Typ = 'URLOP_OKOL' and K.Kod = W.PodTyp
left join Pracownicy P on P.Id = W.IdPracownika
where W.Id = {0} and W.StatusId = {1}
                ", wniosekId, cntWniosekUrlopowy.stAccepted));
            if (dr != null)
            {
                int typid = db.getInt(dr, "TypId", -1);
                string logo = db.getValue(dr, "KadryId");
                string typ = db.getValue(dr, "Typ2");
                string podtyp = db.getValue(dr, "PodTyp2");
                string pt = db.getValue(dr, "PodTyp");
                bool nazadanie = db.getInt(dr, "IdKodyAbs", 0) == 1000;
                DateTime dOd = (DateTime)db.getDateTime(dr, "Od");
                DateTime dDo = (DateTime)db.getDateTime(dr, "Do");
                double? godz = null;   // będzie sam liczyć
                string odbiorData = null;
                switch (typid)
                {
                    case cntWniosekUrlopowy.wtSW:
                        odbiorData = db.getValue(dr, "Info");
                        break;
                    case cntWniosekUrlopowy.wtO2h:
                        int dni = db.getInt(dr, "Dni", -1);
                        if (dni == 1)           // jak 2 dni, to tak jak inne = NULL
                        {
                            double g = db.getDouble(dr, "Godzin", 8);
                            if (g < 8) godz = g;    // i tylko jak inna niż 8h -> uwaga na niepełnoetatowców, wypadałoby sprawdzić do etatu a nie do 8h
                        }
                        break;
                }

                //----- korekty ----- <<< z bazy !!!
                string pt2 = db.getValue(dr, "Nazwa2");     // typ|podtyp - jak nie jest null to stąd brać !!! 
                if (!String.IsNullOrEmpty(pt2))
                    Tools.GetLineParams(pt2, out typ, out podtyp);

                //----- korekty ----- <<< powinno też być z bazy !!!
                switch (typ)
                {
                    case "w":
                        break;
                    case "k":
                        break;
                }
                return ExportWniosekUrlopowy(logo, typ, podtyp, Tools.DateToStrDb(dOd), Tools.DateToStrDb(dDo), godz, nazadanie, odbiorData, showError);
            }
            return false;
        }

        //----- Szkolenia BHP -----
        public static bool Exec(
            int logTyp2,
            string logInfo1,    // 
            string logInfo2,    // String.Format(logInfo2, par)         
            string errMsg,      // String.Format(errMsg, msg) - powinien zawiwerać {0}, np: "Wystapił błąd podczas eksportu {0}"         
            bool showError,
            out DataSet retDS,  // out
            string sql,         // String.Format(sql, par)         
            params object[] par)
        {
            retDS = null;
            int lid = Log.Info(logTyp2, logInfo1, String.Format(logInfo2, par), Log.PENDING);

            bool ok = true;
            string msg = null;
            try
            {
                SqlConnection conAsseco = db.Connect(Asseco.ASSECO);
                try
                {
                    //xok = db.execSQL(conAsseco, String.Format(sql, par));
                    retDS = db.getDataSet(conAsseco, String.Format(sql, par));
                    //retDS = db.getDataSet("select 10001 as lp_KursyId");   // testy
                }
                finally
                {
                    db.Disconnect(conAsseco);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ok = false;
                //throw;      // nie wyswietla sie showmessage !! wiec na razie zeby pokazac
            }

            if (ok)
            {
                Log.Update(lid, Log.OK);
            }
            else
            {
                Log.Update(lid, Log.ERROR);
                Log.Error(logTyp2, logInfo1, String.Format(errMsg, msg), Log.OK);
                if (showError)
                    Tools.ShowMessage(Tools.ToScript(String.Format(errMsg, msg)));
            }
            return ok;
        }


        /*        
        public static bool KursInsert(, bool showError)
        {
        }

        public static bool KursDelete()
        {
        }

        public static bool KursUpdate()
        {
        }
        */
    }
}







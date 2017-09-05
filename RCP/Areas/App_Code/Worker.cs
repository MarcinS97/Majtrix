using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace HRRcp.App_Code
{
    public class Worker
    {
        public Worker(int mode)   // pobieranie danych po create, zwracanie z propertiesach, na razie za duzo roboty i fajnie by było zrobić pobieranie zakresu danych i tylko next, next ... moze jest na to jakis automat
        {
            //GetData(mode);
        }

        /*
        public static DataRow GetPracInfo1(SqlConnection con, string pracId)
        {
            return Base.getDataRow(con,
                "select P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                    "P.Kierownik, P.IdKierownika, " +
                    "D.Nazwa as Dzial, " +
                    "S.Nazwa as Stanowisko, " +
                    "ISNULL(RcpStrefaId," +
                        "case when P.Kierownik=1 then D.KierStrefaId " +
                        "else D.PracStrefaId end) as StrefaId, " +
                    "ISNULL(RcpAlgorytm," +
                        "case when P.Kierownik=1 then D.KierAlgorytm " +
                        "else D.PracAlgorytm end) as Algorytm " +
                "from Pracownicy P " +
                "left outer join Dzialy D ON D.Id = P.IdDzialu " +
                "left outer join Stanowiska S ON S.Id = P.IdStanowiska " +
                "where P.Id = " + pracId);
        }
        */

        
        
        
        
        
        
        public static DataSet GetPracInfo2(int mode, string naDzien, int? okresId, string addSelect, string param1, bool addStanowisko, bool addKierStrefa, bool addComm)  // kierId lub pracId w zaleznosci od mode
        {
            string where;
            if (okresId != null)    // podany okresId i na dzień == null -> koniec okresu
            {
                if (String.IsNullOrEmpty(naDzien))
                {
                    Okres ok = new Okres(db.con, okresId.ToString());
                    naDzien = Tools.DateToStrDb(ok.DateTo);
                }
            }
            else                    // okresId == null, podane na dzień -> okres id wg na dzień
            {
                DateTime? dt = Tools.StrToDateTime(naDzien);
                if (dt != null)
                {
                    Okres ok = new Okres(db.con, (DateTime)dt);
                    okresId = ok.Id;
                }
                if (okresId == null) okresId = -1;
            }
            switch (mode)
            {
                default:
                case 0:         // wszyscy
                    where = "R.Id is not null";
                    break;
                case 1:         // pracownik
                    where = "P.Id = " + param1;
                    break;
                case 2:         // pracownicy kierownika
                    where = "R.IdKierownika = " + param1;
                    break;
                case 3:         // wszyscy z podstruktury kierownika 
                    where = String.Format("P.Id in (select IdPracownika from dbo.fn_GetSubPrzypisania({0}, '{1}'))", param1, naDzien);
                    break;
            }
            string addJoin = null;
            if (addStanowisko)
            {
            }
            if (addKierStrefa)
            {
                addSelect += ",K.Nazwisko + ' ' + K.Imie as KierownikNI, K.KadryId as KierKadryId, ST.Nazwa as RcpStrefa ";
                addJoin += "left outer join Pracownicy K on K.Id = R.IdKierownika " +
                           "left outer join Strefy ST on ST.Id = R.RcpStrefaId ";
            }
            if (addComm)
            {
                addSelect += ",Com.Commodity, Area.Area, Pos.Position ";
                addJoin += "left outer join Commodity Com on Com.Id = R.IdCommodity " +
                           "left outer join Area on Area.Id = R.IdArea " +
                           "left outer join Position Pos on Pos.Id = R.IdPosition "; 
            }

            return db.getDataSet(String.Format(@"
declare 
    @naDzien datetime,
    @okresId int
set @naDzien = '{0}'
set @okresId = {1}
 
select 
	P.Id, P.KadryId, PK.RcpId, PK.NrKarty, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, R.IdKierownika KierId,
    
    ISNULL(PO.EtatL, P.EtatL) as EtatL, 
    ISNULL(PO.EtatM, P.EtatM) as EtatM, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    
    R.Id as PrzId,
    R.IdKierownika, 
    D.Nazwa as Dzial, S.Nazwa as Stanowisko,
    PS.Grupa, PS.Klasyfikacja, PS.Grade, PS.Rodzaj,

    R.RcpStrefaId, PP.RcpAlgorytm, PP.WymiarCzasu, PP.PrzerwaWliczona, PP.PrzerwaNiewliczona,

    P.Rights 
    {2}
from Pracownicy P
left outer join Przypisania R on R.IdPracownika = P.Id and @naDzien between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
left outer join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = @okresId
left outer join PracownicyKarty PK on PK.IdPracownika = P.Id and @naDzien between PK.Od and ISNULL(PK.Do, '20990909')
left outer join PracownicyParametry PP on PP.IdPracownika = P.Id and @naDzien between PP.Od and ISNULL(PP.Do, '20990909')
left outer join PracownicyStanowiska PS on PS.IdPracownika = P.Id and @naDzien between PS.Od and ISNULL(PS.Do, '20990909')
left outer join Dzialy D ON D.Id = PS.IdDzialu
left outer join Stanowiska S ON S.Id = PS.IdStanowiska
{3}
where {4}", 
                naDzien, okresId, addSelect, addJoin, where));                     
                //"ORDER BY P.Kierownik desc, NazwiskoImie";
        }







        //20131124
        public static DataSet x_GetPracInfo2(int mode, string naDzien, int? okresId, string addSelect, string param1, bool addStanowisko, bool addKierStrefa, bool addComm)  // kierId lub pracId w zaleznosci od mode
        {
            string where;
            if (okresId != null)    // podany okresId i na dzień == null -> koniec okresu
            {
                if (String.IsNullOrEmpty(naDzien))
                {
                    Okres ok = new Okres(db.con, okresId.ToString());
                    naDzien = Tools.DateToStrDb(ok.DateTo);
                }
            }
            else                    // okresId == null, podane na dzień -> okres id wg na dzień
            {
                DateTime? dt = Tools.StrToDateTime(naDzien);
                if (dt != null)
                {
                    Okres ok = new Okres(db.con, (DateTime)dt);
                    okresId = ok.Id;
                }
                if (okresId == null) okresId = -1;
            }
            switch (mode)
            {
                default:
                case 0:         // wszyscy
                    where = null;
                    break;
                case 1:         // pracownik
                    where = "and R.IdPracownika = " + param1;
                    break;
                case 2:         // pracownicy kierownika
                    where = "and R.IdKierownika = " + param1;
                    break;
                case 3:         // wszyscy z podstruktury kierownika
                    where = String.Format("and R.IdPracownika in (select IdPracownika from dbo.fn_GetSubPrzypisania({0}, '{1}'))", param1, naDzien);
                    break;
            }
            string addJoin = null;
            if (addStanowisko)
            {
            }
            if (addKierStrefa)
            {
                addSelect += ",K.Nazwisko + ' ' + K.Imie as KierownikNI, K.KadryId as KierKadryId, ST.Nazwa as RcpStrefa ";
                addJoin += "left outer join Pracownicy K on K.Id = R.IdKierownika " +
                           "left outer join Strefy ST on ST.Id = R.RcpStrefaId ";
            }
            if (addComm)
            {
                addSelect += ",Com.Commodity, Area.Area, Pos.Position ";
                addJoin += "left outer join Commodity Com on Com.Id = R.IdCommodity " +
                           "left outer join Area on Area.Id = R.IdArea " +
                           "left outer join Position Pos on Pos.Id = R.IdArea ";
            }

            return db.getDataSet(String.Format(@"
declare 
    @naDzien datetime,
    @okresId int
set @naDzien = '{0}'
set @okresId = {1}
 
select 
	P.Id, P.KadryId, PK.RcpId, PK.NrKarty, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, 
    
    ISNULL(PO.EtatL, P.EtatL) as EtatL, 
    ISNULL(PO.EtatM, P.EtatM) as EtatM, 
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 
    
    R.Id as PrzId,
    R.IdKierownika, 
    D.Nazwa as Dzial, S.Nazwa as Stanowisko,
    
    R.RcpStrefaId, PP.RcpAlgorytm,

    P.Rights 
    {2}
from Przypisania R
left outer join Pracownicy P on P.Id = R.IdPracownika
left outer join PracownicyOkresy PO on PO.Id = R.IdPracownika and PO.IdOkresu = @okresId
left outer join PracownicyKarty PK on PK.IdPracownika = R.IdPracownika and @naDzien between PK.Od and ISNULL(PK.Do, '20990909')
left outer join PracownicyParametry PP on PP.IdPracownika = R.IdPracownika and @naDzien between PP.Od and ISNULL(PP.Do, '20990909')
left outer join PracownicyStanowiska PS on PS.IdPracownika = R.IdPracownika and @naDzien between PS.Od and ISNULL(PS.Do, '20990909')
left outer join Dzialy D ON D.Id = PS.IdDzialu
left outer join Stanowiska S ON S.Id = PS.IdStanowiska
{3}
where @naDzien between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
{4}",
                    naDzien, okresId, addSelect, addJoin, where));

            //"ORDER BY P.Kierownik desc, NazwiskoImie";
        }



        //* 20131103
        public static DataSet _GetPracInfo1(SqlConnection con, int mode, int? okresId, string addSelect, string param1, bool addStanowisko)
        {
            string where;
            string s1 = addStanowisko ? "S.Nazwa as Stanowisko, " : null;
            string s2 = addStanowisko ? "left outer join Stanowiska S ON S.Id = P.IdStanowiska " : null;

            if (okresId != null)
            {
                switch (mode)
                {
                    default:
                    case 0:         // wszyscy
                        where = "where P.IdOkresu = " + okresId.ToString();
                        break;
                    case 1:         // pracownik
                        where = "where P.IdOkresu = " + okresId.ToString() + " and P.Id = " + param1;
                        break;
                    case 2:         // pracownicy kierownika
                        where = "where P.IdOkresu = " + okresId.ToString() + " and P.Status >= 0 and P.IdKierownika = " + param1;
                        break;
                }
                return Base.getDataSet(con,
                    "select P.Id, P.KadryId, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.EtatL, P.EtatM, " +
                        "P.Kierownik, P.IdKierownika, " +
                        "D.Nazwa as Dzial, " +
                        s1 +
                        "P.RcpStrefaId as StrefaId, " +         // w arch sa zapamietane wartosci obowiazujace
                        "P.RcpAlgorytm as Algorytm, " +
                        "P.CCInfo, D.KierCCInfo, D.PracCCInfo, P.CCInfo as CCInfoMix," +
                        "P.Rights " +
                        addSelect +
                    " from PracownicyOkresy P " +
                    "left outer join Dzialy D ON D.Id = P.IdDzialu " +
                    s2 +
                    where);
            }
            else
            {
                switch (mode)
                {
                    default:
                    case 0:         // wszyscy
                        where = null;
                        break;
                    case 1:         // pracownik
                        where = "where P.Id = " + param1;
                        break;
                    case 2:         // pracownicy kierownika
                        where = "where P.Status >= 0 and P.IdKierownika = " + param1;
                        break;
                }
                return Base.getDataSet(con,
                    "select P.Id, P.KadryId, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                        "P.EtatL, P.EtatM, " +
                        "P.Kierownik, P.IdKierownika, " +
                        "D.Nazwa as Dzial, " +
                        s1 +
                        "ISNULL(P.RcpStrefaId," +
                            "case when P.Kierownik=1 then D.KierStrefaId " +
                            "else D.PracStrefaId end) as StrefaId, " +
                        "ISNULL(P.RcpAlgorytm," +
                            "case when P.Kierownik=1 then D.KierAlgorytm " +
                            "else D.PracAlgorytm end) as Algorytm, " +
                        "P.CCInfo, D.KierCCInfo, D.PracCCInfo, " +
                        "ISNULL(P.CCInfo," +
                            "case when P.Kierownik=1 then D.KierCCInfo " +
                            "else D.PracCCInfo end) as CCInfoMix," +
                        "P.Rights " +
                        addSelect +
                    " from Pracownicy P " +
                    "left outer join Dzialy D ON D.Id = P.IdDzialu " +
                    s2 +
                    where);
                //"ORDER BY P.Kierownik desc, NazwiskoImie";
            }
        }
        
        //--------------------
        public static string GetNazwiskoImie(string pracId)
        {
            return Base.getScalar("select Nazwisko + ' ' + Imie from Pracownicy where Id=" + pracId);
        }

        public static DataRow GetData(string pracId, bool arch)
        {
            return Base.getDataRow("select *, Nazwisko + ' ' + Imie as NazwiskoImie from " +
                                   (arch ? "PracownicyOkresy" : "Pracownicy") +
                                   " where Id=" + pracId);
        }

        public static int GetEtat(DataRow dr)      // il. godzin: 8
        {
            int? etatL = Base.getInt(dr, "EtatL");
            int? etatM = Base.getInt(dr, "EtatM");
            if (etatL != null && etatM != null)
                if ((int)etatL > 0 && (int)etatM > 0)
                    return 8 * (int)etatL / (int)etatM;
            return 0;  // błąd
        }

        public static DataRow GetData(SqlConnection con, int? okresId, string pracId)
        {
            if (okresId != null)
                return Base.getDataRow(con,
                    "select * " +
                    "from PracownicyOkresy " +
                    "where IdOkresu = " + okresId.ToString() + " and Id = " + pracId);
            else
                return Base.getDataRow(con,
                    "select * " +
                    "from Pracownicy " +
                    "where Id = " + pracId);
        }

    }
}

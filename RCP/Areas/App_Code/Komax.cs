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


namespace HRRcp.App_Code
{
    public static class Komax
    {
        public static string KOMAX = ConfigurationManager.ConnectionStrings["KOMAX"].ConnectionString;

        //----- Import --------------------------------
        public static bool importInProgress = false;  // to z opisu będzie instancja zmiennej widziana we wszystkich sesjach/procesach PRP

        public static void ImportAll()
        {
            if (!importInProgress)
                try
                {
                    int c1 = 0;
                    int c2 = 0;
                    int c3 = 0;
                    int c4 = 0;
                    int c5 = 0;
                    int c6 = 0;
                    importInProgress = true;

                    const string info = "Import danych z systemu Komax";
                    int lid = Log.Info(Log.IMPORT_ASSECO, info, null, Log.PENDING);

                    SqlConnection conAsseco = db.Connect(KOMAX);

                    c1 = ImportABSENCJA(conAsseco);
                    c2 = ImportKODYABS(conAsseco);
                    
                    /*
                    c3 = ImportKALENDAR(conAsseco);
                    c4 = ImportCZASNOM(conAsseco);
                    c5 = ImportSTAWKI(conAsseco);    // moze dac pozniej przed zamknieciem miesiaca zeby sie wykonalo ?
                    c6 = ImportZBIOR(conAsseco);
                    */

                    db.Disconnect(conAsseco);

                    if (c1 >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0 && c6 >= 0)// && ok1)
                    {
                        Log.Update(lid, Log.OK);
                        Log.Info(Log.IMPORT_ASSECO, info, "zakończony OK", Log.OK);
                        Tools.ShowMessage("Import zakończony.");
                    }
                    else
                    {
                        Log.Update(lid, Log.ERROR);
                        Log.Error(Log.IMPORT_ASSECO, info, String.Format("Wystapił błąd podczas importu danych, kody: {0} {1} {2} {3} {4} {5}", c1, c2, c3, c4, c5, c6), Log.OK);
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
        //-------------------------------------------------



        //-----------------------------------------
        //  IMPORT Z KOMAX'a
        //-----------------------------------------
        public static int ImportABSENCJA(SqlConnection komax)
        {
            Ustawienia settngs = Ustawienia.CreateOrGetSession();
            string startData = db.strParam(Tools.DateToStrDb(settngs.SystemStartDate));             // UWAGA !!! Daty są na stringach w formacje YYYYMMDD
//Typ	Id	LpLogo	DataOd	DataDo	Kod	IleDni	Godzin	Zalegly	Planowany	NaZadanie	Rok	Miesiac	Korekta	IdKorygowane	Aktywny
            //----- absencje -----
            int cntU = db.ImportTable("bufAbsencja", true, komax, @"
select 'K',ID,NREWID,DATAOD,DATADO,KODNIE,DNIZA,GODZZA,0,0,0,LEFT(OKRES,4),RIGHT(OKRES,2),0,null,1
from KOMAX..K_NIEOBEC
where DATAOD >= " + startData);
            //----- aktualizacja -----
            db.execSQL(@"
delete from Absencja
insert into Absencja 
select P.Id, A.LpLogo, A.DataOd, A.DataDo, A.Kod, A.IleDni, A.Godzin 
from bufAbsencja A 
left outer join Pracownicy P on P.KadryId2 = A.LpLogo
where A.IleDni > 0
            ");
            return (cntU);
        }
                
        public static int ImportKODYABS(SqlConnection komax)
        {
            int cnt = 0;
            int d = 1;
            Base.execSQL("update AbsencjaKody set Status = -1 where Status >= 0");   // wszyscy jako starzy
//     0        1        2                        3 
            DataSet ds = Base.getDataSet(komax, @"
select KODSYMB, KODSYMB, TRESC, case when ARCHIW = 0 then 1 else 0 end --,*
from KOMAX..K_BKODY 
where KODNUM = 51 and KODSYMB is not null and RTRIM(KODSYMB) <> ''
and ARCHIW = 0
--and KODSYMB In (select distinct KODNIE from KOMAX..K_NIEOBEC)
            ");  //20140213 wszystkie kody niearchiwalne
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string id = Base.getValue(dr, 0);
                string symbol = Base.getValue(dr, 1);
                string nazwa = Base.getValue(dr, 2);
                int aktywny = Base.getInt(dr, 3, 0);

                DataRow drAK = db.getDataRow("select * from AbsencjaKody where Kod = " + id);
                if (drAK == null)       // brak kodu - nowy kod
                {
                    bool ok = db.insert("AbsencjaKody", 0, "Kod, Nazwa, Symbol, Widoczny, Status",
                                   id, db.strParam(nazwa), db.strParam(symbol), 1, aktywny);
                    if (!ok)
                    {
                        Log.Error(Log.IMPORT_ASSECO, "Import KOMAX.KODY_ABSENCJI", null);
                        d = -1;
                    }
                }
                else
                {
                    int status = db.getInt(drAK, "Status", -99);
                    if (status != AdmKodyAbs.stDodatkowy)       // jak nie ma lub różny od dodatkowy to aktualizuję/dodaję
                    {
                        int akt = aktywny == 1 ? 0 : -1;
                        bool ok = db.update("AbsencjaKody", 0, "Nazwa,Status",
                            "Kod=" + id,
                            db.strParam(nazwa), 0);
                        if (!ok)
                        {
                            Log.Error(Log.IMPORT_ASSECO, "Import KOMAX.KODY_ABSENCJI.Update", String.Format("id: {0} symbol: {1} nazwa: {2}", id, symbol, nazwa));
                            d = -1;
                        }
                    }
                }
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



        public static int ImportZBIOR(SqlConnection asseco)
        {
            //const string sql = "select Rok,NREWID,urlopbie,urlopzal,WykorzystanyDoDnia from w_lp_fn_limityurlopowe('{0}')";
            const string sql = @"
declare @data datetime = '{0}'
declare @dzis datetime = '{1}'

SELECT 
YEAR(@data) Rok,
a.LpLogo NREWID,
ISNULL(l.LimitGodzinNom, 0) urlopnom2,
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
FROM dbo.lp_fn_BasePracExLow(@dzis) a 
JOIN dbo.lp_vv_UrlopyLimitowane ul WITH(NOLOCK) ON ul.UrlopTyp = 'w'
--CROSS APPLY dbo.lp_fn_LimitUrlopuNaDzien( a.LpLogo, a.UmowaNumer, @data, ul.UrlopTyp) l
CROSS APPLY dbo.lp_fn_LimitUrlopuNaDzien( a.LpLogo, a.UmowaNumer, ISNULL(a.DataZwolnienia, @data), ul.UrlopTyp) l
LEFT JOIN dbo.lp_PracKategorieZaszeregowan kz WITH (NOLOCK) ON kz.LpLogo = a.LpLogo AND kz.UmowaNumer = a.UmowaNumer
                                              AND @data BETWEEN kz.DataOd AND ISNULL(kz.DataDo,'20990909')             
LEFT JOIN dbo.lp_SkladnikiPracownika st ON st.LpLogo = a.LpLogo AND st.UmowaNumer = a.UmowaNumer 
                                   AND @data BETWEEN st.DataOd AND ISNULL(st.DataDo,'20990909')
                                   AND st.SkladnikRodzaj = 'ANGAZ'
WHERE 
a.Zatrudniony = 1 
--AND (a.Zatrudniony = 1 or YEAR(ISNULL(a.DataZwolnienia, '20990909')) = YEAR(@data))  --2157 dupkey
AND a.Wlasny = 1";

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




















        public static int ImportSTAWKI(SqlConnection asseco) // i nowi pracownicy, aktulizacja danych starym 
        {
            Okres o = Okres.LastClosed(db.con);
            o.Next();  // bieżący do zamknięcia, nie moze byc Today !!! bo przed zamknieciem miesiaca a w nastepnym bralby zle

            int cnt = 0;
            int d = 1;
            //DataSet ds = Base.getDataSet(asseco, String.Format(" select * from RCP.dbo.w_lp_fn_pracownicyRCP('{0}')",  // dołożony pesel
            //                             Tools.DateToStr(o.DateTo)));
            DataSet ds = Base.getDataSet(asseco, String.Format(" select * from RCP.dbo.w_lp_fn_pracownicyRCP('{0}')",   // dołożony pesel, zatrudniony i własny
                                         Tools.DateToStr(DateTime.Now)));                                               // na dziś
            //DataSet ds = Base.getDataSet(asseco, String.Format("select * from w_lp_fn_pracownicyRCP",  <<<<<<<<<<<<
            //                             Tools.DateToStr(o.DateTo)));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string nr_ew = dr["LpLogo"].ToString();         // spr czy jest
                //                                  0  1        
                DataRow drP = db.getDataRow("select Id,Nick,Status from Pracownicy where KadryId = " + Base.strParam(nr_ew));
                //string pid = drP == null ? null : db.getValue(dr, 0);
                
                double? etat = db.getDouble(dr, "EtatNumeric");
                if (etat == null) etat = 1.0;
                int sign, etatL, etatM;
                Tools.DecimalToFraction((double)etat, out sign, out etatL, out etatM);
                string pesel = db.getValue(dr, "Pesel");

                if (drP == null)                  // nowy pracownik
                {
                    string nazwisko = Tools.PrepareName(Base.getValue(dr, "Nazwisko"));
                    string imie = Tools.PrepareName(Base.getValue(dr, "Imie"));
                    string nr_ew_kier = dr["LogoPrzelozony"].ToString();    // kierownik
                    string kierId = String.IsNullOrEmpty(nr_ew_kier) ? null : db.getScalar("select Id from Pracownicy where KadryId = " + db.strParam(nr_ew_kier));

                    bool ok;
                    if (nr_ew.Length > 5) 
                        ok = false;
                    else 
                        ok = db.insert("Pracownicy", 0, 
                            "Nazwisko,Imie,Login,Email,Admin,Kierownik,KadryId,Status,Nick,Pass,IdKierownika,Stawka,EtatL,EtatM,DataZatr",

                            Base.strParam(nazwisko), 
                            Base.strParam(imie),
                            db.strParam("login_" + nr_ew),
                            db.strParam("email_" + nr_ew),
                            0, 0, 
                            db.strParam(nr_ew), 
                            App.stNew,                              // nowy pracownik
                            db.strParam(pesel),
                            db.strParam(FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod)),     // nowe hasło
                            db.nullParam(kierId),

                            db.getFloatAsString(dr, "Stawka", Base.NULL),
                            etatL,etatM,
                            Base.nullStrParam(Base.DateToStr(dr["DataZatrudnienia"])));
                    if (!ok)
                    {
                        Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.NEWPRAC", nr_ew);
                        d = -1;
                    }
                }
                else                                        // istniejący pracownik
                {
                    string nazwisko = Tools.PrepareName(Base.getValue(dr, "Nazwisko"));
                    string imie = Tools.PrepareName(Base.getValue(dr, "Imie"));
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
                    //----- powtónie zatrudnieni ------
                    int status = db.getInt(drP, "Status", -1);
                    if (status == App.stOld)
                    {
                        //DateTime? dZatrRCP = db.getDateTime(drP, "DataZatr");
                        //DateTime? dZatrAsseco = db.getDateTime(dr, "DataZatrudnienia");
                        status = App.stNew;  // nie ma potrzeby kontrolowania dat, bo z Asseco dostaję tylko aktualnych pracowników
                    }
                    //----- -----
                    bool ok = db.update("Pracownicy", 0,
                        "Nazwisko,Imie,Stawka,EtatL,EtatM,DataZatr,Status" + pass_reset,         // ostatnie parametry !!!
                        "KadryId=" + nr_ew,
                        Base.strParam(nazwisko),
                        Base.strParam(imie),
                        Base.getFloatAsString(dr, "Stawka", Base.NULL),
                        etatL,etatM,
                        Base.nullStrParam(Base.DateToStr(dr["DataZatrudnienia"])),
                        status,
                        db.strParam(pesel),                                 // ostatnie parametry to muszą być !!!
                        db.strParam(FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod))     // nowe hasło
                        );
                    if (!ok)
                    {
                        Log.Error(Log.t2APP_IMPORTKP, "Import ASSECO.STAWKI", nr_ew);
                        d = -1;
                    }
                }
                cnt++;
            }
            return cnt * d;
        }
    }
}

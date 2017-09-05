using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;


//ilość przepracowanych = niminalny(np.168)-niedomiar+nadgodziny-nieobecnosci

namespace HRRcp.Controls
{
    public partial class RepNadgodziny3 : System.Web.UI.UserControl
    {
        SqlConnection con = null;
        PlanPracy.TimeSumator _sumator;
        TimeSumator3 total;
        int ilDniPrac;      // w okresie z Kalendarza
        int ilGodzPrac;
        int ilDniPracMies;  // w miesiącu wynikajcym z końca okresu z CzasNom
        int ilGodzPracMies;

        double p50 = 1.5;
        double p100 = 2.0;
        double _stawkaNoc = 0;
        bool noData = false;            // brak danych '-' w zestawieniu - okres.From > DateTime.Today; ustawiane w DataBinding
        bool incompleteData = false;    // niekompletne dane (nie wszystko zatwierdzone !!!) - do wyświetlenia warninga

#if SIEMENS || DBW || VICIM
        bool showStawki = false;
        const bool showUrlop = true;        // chorobowe i wolne za nadg, czas przepracowany bez absencji
        //const bool nieprzAbsencja = false;  // absencja wchodzi w nieprzepracowany
        const bool showPrzepracLaczny = false;  // przepracowany + absencja jak 8h zaznaczone
        const bool showPrzeprac = true;     // przepracowany bez absencji, nieprzepracowany tylko wg zmiany, nie zmniejszają go nadgodziny
        const bool pracOkres = true;        // wszyscy pracownicy zatrudnieni w okresie, z przypisaniami na koniec okresu
        const bool showDodatek = false;     // dodatek 50% i 100% jak zmiana poniedziałek zaczyna sie o 5:00  -> false od 20150628
#else
        //const bool showStawki = true;
        bool showStawki = false;
        const bool showUrlop = false;       // chorobowe i wolne za nadg
        //const bool nieprzAbsencja = true;   // absencja wchodzi w nieprzepracowany - Jabil
        const bool showPrzepracLaczny = true;   // absencja wchodzi w nieprzepracowany
        const bool showPrzeprac = false;    // absencja wchodzi w nieprzepracowany
        const bool pracOkres = true;        // wszyscy pracownicy zatrudnieni w okresie, z przypisaniami na koniec okresu
        const bool showDodatek = false;
#endif



#if SIEMENS || DBW || VICIM
        const bool etatF = true;        // testy, etat na float
#else
        const bool etatF = false;       // testy, etat na int
#endif



        //1. okres zamkniety - dane <<< okres.Status = stClosed lub okres.From <= Today; !allData ? warning <<< bo może nie brać udziału !!!
        //2. okres - otwarty - dane <<< okres.From <= Today; !allData ? warning <<< muszą być jakieś dane!
        //3. okres otwarty - pusto <<< okres.From > Today; brak danych

        public const int nmoKierStawki  = 0;    // nadgodziny wg kierownika - stawki
        public const int nmoProjStawki  = 1;    // nadgodziny wg projektów - stawki
        public const int nmoKier        = 3;    // nadgodziny wg kierownika - bez stawek
        public const int nmoRcp         = 4;    // porównanie z czasem Rcp
        public const int nmoDaneMPK     = 5;    // wszyscy pracownicy ze Statusem >= 0 lub aktywnymi Splitami, tylko do GetSql
        //public const int nmoRcpCC       = 6;    // porównanie z czasem Rcp po cc wg uprawnień przełożonego <<< albo zostawic 4 i dodac CCMode 
        
        //int FMode = 0;

        Ustawienia settings;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

#if R2   // musi się zawsze wykonywać bo showStawki to zmienna
            bool kwoty = App.User.HasRight(AppUser.rAdminKwoty);
            showStawki = kwoty;
            //showDodatek = kwoty;   <<<< jeżelil ma być właczony
#endif
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }
        //-----------------------------------------------------------
        /*
        private void GetOkresParams()
        {
            if (OkresId != "-1")
            {
                Okres ok = new Okres(null, OkresId);
                StawkaNocna = ok.StawkaNocna;
                IsArch = ok.IsArch();
            }
            else
            {
                StawkaNocna = -1;
                IsArch = false;
            }
        }
        */

        /*
        private void InitPath(string kierId)
        {
            string kn;
            if (String.IsNullOrEmpty(kierId))  // na starcie jest null
                if (Mode == nmoKier || Mode == nmoKierStawki) kn = null;
                else kn = "Wszyscy pracownicy";
            else if (kierId == "0") kn = "Poziom główny";
            else kn = Worker.GetNazwiskoImie(kierId);
            cntPath.Prepare(kn, kierId, "0");
        }
        */
        
        private void InitPath(string kierId)
        {
            string kn;
            if (String.IsNullOrEmpty(kierId) || kierId == "-100")  // na starcie jest null
                kn = "Wszyscy pracownicy";
            else if (kierId == "0") 
                kn = "Poziom główny";
            else kn = Worker.GetNazwiskoImie(kierId);
            cntPath.Prepare(kn, kierId, "0");
        }

        public void _Prepare(string projId, string kierId, string dFrom, string dTo, string okresId, int okresStatus, bool okresArch, double okresStNoc, int mode)
        {
            DateFrom = dFrom;
            DateTo = dTo;
            ProjId = projId;
            KierId = kierId;
            OkresId = okresId;
            OkresStatus = okresStatus;
            Mode = mode;
            IsArch = okresArch;
            StawkaNocna = okresStNoc;

            if (Mode == nmoKier || Mode == nmoKierStawki || Mode == nmoRcp)
                InitPath(kierId);

            SqlDataSource1.SelectCommand = _GetSql(IsArch);
            /*
            if (param != null)
            {
                //lvNadgodziny.DataBind();
            }
            */ 
        }

        public void _Prepare(string dFrom, string dTo, string okresId, int okresStatus, bool okresArch, double okresStNoc)
        {
            DateFrom = dFrom;
            DateTo = dTo;
            OkresId = okresId;
            OkresStatus = okresStatus;
            IsArch = okresArch;
            StawkaNocna = okresStNoc;
            SqlDataSource1.SelectCommand = _GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void _DataBindKier(string kierId)   // daty musza byc ustawione !!!
        {
            KierId = kierId;
            ProjId = null;
            
            if (Mode == nmoKier || Mode == nmoKierStawki || Mode == nmoRcp)
                InitPath(kierId);

            SqlDataSource1.SelectCommand = _GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void _DataBindProj(string projId)   // daty musza byc ustawione !!!
        {
            ProjId = projId;
            KierId = null;

            InitPath(null);  // poziom główny
            
            SqlDataSource1.SelectCommand = _GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void _DataBindZoom(string kierId)   // daty musza byc ustawione !!!
        {
            KierId = kierId;
            SqlDataSource1.SelectCommand = _GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public string _GetSql(bool arch)
        {
            if (pracOkres)
                return GetSql2(arch, OkresId, Mode, KierId, ProjId, DateFrom, DateTo);
            else
                return GetSql(arch, OkresId, Mode, KierId, ProjId, DateFrom, DateTo);
        }













        public string GetSql(bool arch, string okresId, int mode, string kierId, string projId, string dataOd, string dataDo)
        {
            const string select = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, " +
                                    "P.IdKierownika, K.KadryId as KierKadryId, K.Nazwisko + ' ' + K.Imie as KierownikNI, I.Class, " +
                                    "P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    //"ISNULL(P.RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm, " +
                                    "R.RcpAlgorytm as Algorytm, " +
#if NOMWYM 
                                    "R.WymiarCzasu, " +
#endif
#if SIEMENS || DBW || VICIM
                                    "dbo.fn_GetPracLastCC(P.Id, '{2}', 2, ',') as CC, " +
#else
                                    //"null as CC, " +    
                                    "dbo.fn_GetPracLastCCPodzialLudzi(P.KadryId, '{2}', 4, ', ') as CC, " +
#endif
                                    "D.Nazwa as Dzial ";

            const string select_okres = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.KadryId, " +
                                    "P.IdKierownika, K.KadryId as KierKadryId, K.Nazwisko + ' ' + K.Imie as KierownikNI, I.Class, " +
                                    "P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    //"P.RcpAlgorytm as Algorytm, " +
                                    "R.RcpAlgorytm as Algorytm, " +
#if NOMWYM
                                    "R.WymiarCzasu, " +
#endif
#if SIEMENS || DBW || VICIM
                                    "dbo.fn_GetPracLastCC(P.Id, '{2}', 2, ',') as CC, " +
#else
                                    //"null as CC, " +
                                    "dbo.fn_GetPracLastCCPodzialLudzi(P.KadryId, '{2}', 4, ', ') as CC, " +
#endif
                                    "D.Nazwa as Dzial ";

#if SIEMENS || DBW || VICIM
            const string join = "left outer join Pracownicy K on K.Id = P.IdKierownika " +
                                "left outer join PodzialLudziImport I on I.KadryId = P.KadryId and I.Miesiac = '{0}' " +
                                "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu " +
                                "outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and Od <= '{2}' order by Od desc) R";
#else
            const string join = "left outer join Pracownicy K on K.Id = P.IdKierownika " +
                                "left outer join PodzialLudziImport I on I.KadryId = P.KadryId and I.Miesiac = '{0}' " +
                                "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu " +
                                "outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and Od <= '{2}' order by Od desc) R";
#endif
            const string sql0 = select +
                                ",P.Stawka " +
                                "FROM Pracownicy P " +
                                join;

            const string sql0_okres = select_okres +
                                ",P.Stawka " +
                                "FROM PracownicyOkresy P " +
                                join;

            const string sql2 = select +
                                "FROM Pracownicy P " +
                                join;

            const string sql2_okres = select_okres +
                                "FROM PracownicyOkresy P " +
                                join;
            /*
            const string sql2 = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM Pracownicy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";

            const string sql2_okres = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "P.RcpAlgorytm as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM PracownicyOkresy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";
            */
            const string order0 = " ORDER BY P.Kierownik desc, NazwiskoImie";
            const string order1 = " ORDER BY NazwiskoImie";

            string mc = ImportMiesiac;

            switch (mode)
            {
                default:
                case nmoKierStawki:
                    if (String.IsNullOrEmpty(kierId)) return null;
                    else if (kierId == "-100")//"ALL")                                                                                                  //return sql0 + order0;
                        if (arch) return String.Format(sql0_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0" + order0;
                        else      return String.Format(sql0, mc, dataOd, dataDo) + " where P.Status >= 0" + order0;
                    else                                                                                                                                //return sql0 + " WHERE P.IdKierownika = " + KierId + order0;  // kierID = 0 -> poziom roota
                        if (arch) return String.Format(sql0_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdKierownika = " + kierId + order0;  // kierID = 0 -> poziom roota
                        else      return String.Format(sql0, mc, dataOd, dataDo) + " where P.Status >= 0 and P.IdKierownika = " + kierId + order0;                                              // kierID = 0 -> poziom roota
                case nmoProjStawki:
                    if (String.IsNullOrEmpty(projId)) return null;                                                                      // brak danych ??? 
                    else                                                                                                                //return sql0 + " WHERE P.IdDzialu = " + ProjId + order0;
                    {
                        string whereKier = String.IsNullOrEmpty(kierId) ? null : " and P.IdKierownika = " + kierId;
                        if (arch) return String.Format(sql0_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdDzialu = " + projId + whereKier + order0;
                        else      return String.Format(sql0, mc, dataOd, dataDo) + " where P.Status >= 0 and P.IdDzialu = " + projId + whereKier + order0;
                    }
                case nmoRcp:
                case nmoKier:
                    if (String.IsNullOrEmpty(kierId)) return null;
                    else if (kierId == "-100")//"ALL")                                                                                           //return sql2 + order0;
                        if (arch) return String.Format(sql2_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0" + order0;
                        else return String.Format(sql2, mc, dataOd, dataDo) + " where P.Status >= 0" + order0;
                    else                                                                                                                //return sql2 + " WHERE P.IdKierownika = " + KierId + order0;
                        if (arch) return String.Format(sql2_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdKierownika = " + kierId + order0;
                        else return String.Format(sql2, mc, dataOd, dataDo) + " where P.Status >= 0 and P.IdKierownika = " + kierId + order0;
                    /*
                case nmoRcpCC:
                    if (String.IsNullOrEmpty(kierId)) return null;
                    else if (kierId == "-100")//"ALL")                                                                                           //return sql2 + order0;
                        if (arch) return String.Format(sql2_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0" + order0;
                        else return String.Format(sql2, mc, dataOd, dataDo) + " where P.Status >= 0" + order0;
                    else                                                                                                                //return sql2 + " WHERE P.IdKierownika = " + KierId + order0;
                        if (arch) return String.Format(sql2_okres, mc, dataOd, dataDo) + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdKierownika = " + kierId + order0;
                        else return String.Format(sql2, mc, dataOd, dataDo) + " where P.Status >= 0 and P.IdKierownika = " + kierId + order0;
                    */
            }
        }







        /*
         UWAGA !!!
         Etat jest brany na datę końca okresu - powinien być dzień po dniu
         * 
         * 
         * 
         */



        public string GetSql2(bool arch, string okresId, int mode, string kierId, string projId, string dataOd, string dataDo)
        {
            const string select = @"
declare @od datetime
declare @do datetime
declare @kier int
declare @dzial int
declare @okres int
set @od = '{0}'
set @do = '{1}'
set @kier = {2}
set @dzial = {3}
set @okres = {6}

select 
	P.Id, 
    P.Nazwisko + ' ' + P.Imie + case when P.Status = -1 then ' (zwolniony)' else '' end as NazwiskoImie, P.KadryId, 
    P.Status,
    P.IdKierownika, K.KadryId as KierKadryId, K.Nazwisko + ' ' + K.Imie as KierownikNI, I.Class, 
    
    ISNULL(PO.EtatL, P.EtatL) as EtatL, 
    ISNULL(PO.EtatM, P.EtatM) as EtatM, 

    P.RcpId, 
    
    ISNULL(PO.Kierownik, P.Kierownik) as Kierownik, 

    PR.RcpAlgorytm as Algorytm, 
    D.Nazwa as Dzial," +
    "{4} " +   // P.Stawka
#if NOMWYM
    "PR.WymiarCzasu, " +
#endif
#if SIEMENS || DBW || VICIM
    "dbo.fn_GetPracLastCC(P.Id, @do, 2, ',') as CC " +
#else
    //"null as CC " +
    "dbo.fn_GetPracLastCCPodzialLudzi(P.KadryId, @do, 4, ', ') as CC " +
#endif
 @"from Pracownicy P 
left join PracownicyOkresy PO on PO.Id = P.Id and PO.IdOkresu = @okres
outer apply (select top 1 * from Przypisania where IdPracownika = P.Id and Status = 1 and Od <= @do and ISNULL(Do, '20990909') >= @od and (@kier = -100 or IdKierownika = @kier) order by Od desc) R
left join Pracownicy K on K.Id = R.IdKierownika

left outer join PodzialLudziImport I on I.KadryId = P.KadryId and I.Miesiac = dbo.bom(@do)
outer apply (select top 1 * from PracownicyStanowiska where IdPracownika = P.Id and Od <= @do order by Od desc) PS
LEFT OUTER JOIN Dzialy D ON D.Id = PS.IdDzialu 
outer apply (select top 1 * from PracownicyParametry where IdPracownika = P.Id and Od <= @do order by Od desc) PR

where P.Status >= -1 and R.Id is not null
and (@dzial = -100 or PS.IdDzialu = @dzial) 
{5}";
            //order by Kierownik desc, KierownikNI, NazwiskoImie, KadryId 
            const string stawka = "ISNULL(PO.Stawka, P.Stawka) as Stawka,";
            const string order0 = " ORDER BY P.Kierownik desc, NazwiskoImie";
            const string order1 = " ORDER BY NazwiskoImie";

            //string mc = ImportMiesiac;

            switch (mode)
            {
                default:
                case nmoKierStawki:
                    if (String.IsNullOrEmpty(kierId)) 
                        return null;
                    else
                        return String.Format(select, dataOd, dataDo, kierId, -100, stawka, order0, arch ? okresId : "-1");   
                case nmoProjStawki:
                    if (String.IsNullOrEmpty(projId)) 
                        return null;                                                                      
                    else
                        return String.Format(select, dataOd, dataDo, -100, projId, stawka, order0, arch ? okresId : "-1");   
                case nmoRcp:
                case nmoKier:
                    if (String.IsNullOrEmpty(kierId))
                        return null;
                    else
                        return String.Format(select, dataOd, dataDo, kierId, -100, "", order0, arch ? okresId : "-1");
            }
        }
























        //do eksportu do assceo - pracownicy ze statusem >= 0 lub posiadający otwarty split !!! <<< Podział Ludzi decyduje kto 
        public static string _GetSqlDaneMPK(bool arch, string okresId, string okresOd, string okresDo, string nrew)
        {
            const string sql2 = @"
SELECT P.Id, 
    P.Nazwisko + ' ' + P.Imie as NazwiskoImie, 
    P.KadryId, 
    P.EtatL, P.EtatM, 
    P.RcpId, 
    P.Kierownik, 
    ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm, 
    D.Nazwa as Dzial 
FROM Pracownicy P 
LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu 
left outer join Splity S on S.GrSplitu = P.GrSplitu and '{0}' between S.DataOd and ISNULL(S.DataDo, '{0}') 
where (P.Status >= 0 or S.Id is not null) {1}";

            const string sql2_okres = @"
SELECT P.Id,
    P.Nazwisko + ' ' + P.Imie as NazwiskoImie, 
    P.KadryId, 
    P.EtatL, P.EtatM, 
    P.RcpId, 
    P.Kierownik, 
    P.RcpAlgorytm as Algorytm, 
    D.Nazwa as Dzial 
FROM PracownicyOkresy P 
LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu
left outer join Splity S on S.GrSplitu = P.GrSplitu and '{0}' between S.DataOd and ISNULL(S.DataDo, '{0}') 
where P.IdOkresu = {1} and (P.Status >= 0 or S.Id is not null) {2}";

            string k = String.IsNullOrEmpty(nrew) ? null : String.Format("and P.KadryId = '{0}'", nrew);
            if (arch) 
                return String.Format(sql2_okres, okresOd, okresId, k);
            else 
                return String.Format(sql2, okresOd, k);
        }
        //----------------------------------------------
        private void ShowStawki(Control cnt, string td, bool visible)
        {
            if (!visible)
            {
                Tools.SetControlVisible(cnt, td + "7", false);
                Tools.SetControlVisible(cnt, td + "8", false);
                Tools.SetControlVisible(cnt, td + "9", false);
                Tools.SetControlVisible(cnt, td + "10", false);
                Tools.SetControlVisible(cnt, td + "11", false);
                Tools.SetControlVisible(cnt, td + "12", false);
            }
        }

        private void ShowUrlop(Control cnt, string td, bool visible)
        {
            Tools.SetControlVisible(cnt, td + "51", visible);
            Tools.SetControlVisible(cnt, td + "52", visible);
            Tools.SetControlVisible(cnt, td + "71", visible);

            Tools.SetControlVisible(cnt, td + "53", visible);
            Tools.SetControlVisible(cnt, td + "54", visible);
            Tools.SetControlVisible(cnt, td + "55", visible);
            Tools.SetControlVisible(cnt, td + "56", visible);
            Tools.SetControlVisible(cnt, td + "57", visible);
            Tools.SetControlVisible(cnt, td + "58", visible);
            Tools.SetControlVisible(cnt, td + "59", visible);

            Tools.SetControlVisible(cnt, td + "4", visible && showDodatek);        // dodatek 50
            Tools.SetControlVisible(cnt, td + "14", visible && showDodatek);       // dodatek 100

            Tools.SetControlVisible(cnt, td + "61", showPrzepracLaczny);
            Tools.SetControlVisible(cnt, td + "62", showPrzeprac);
        }

        private void PrepareViewHeader(Control cnt)
        {
#if SIEMENS || DBW || VICIM
            const int allSpan = 4;
            const int kierSpan = 3;
#else
            const int allSpan = 5;
            const int kierSpan = 4;
#endif

            bool all = KierId == "-100";
            int cspan = all ? allSpan : kierSpan;

            switch (Mode)
            {
                default:
                case nmoKierStawki:
                    HtmlTableCell t = (HtmlTableCell)cnt.FindControl("th12");
                    if (t != null) Tools.AddClass(t, "lastcol");
                    t = (HtmlTableCell)cnt.FindControl("td12");  //suma
                    if (t != null) Tools.AddClass(t, "lastcol");
                    Tools.SetControlVisible(cnt, "thKier", all);
                    Tools.SetColSpan(cnt, "tdSumy", cspan + 1);
                    ShowStawki(cnt, "th", showStawki);
                    ShowUrlop(cnt, "th", showUrlop);
                    break;
                case nmoProjStawki:
                    Tools.SetControlVisible(cnt, "thDzial", false);
                    //HtmlTableCell td = (HtmlTableCell)cnt.FindControl("tdSumy");
                    //if (td != null) td.ColSpan = 2;
                    Tools.SetColSpan(cnt, "tdSumy", cspan);
                    
                    t = (HtmlTableCell)cnt.FindControl("th12");
                    if (t != null) Tools.AddClass(t, "lastcol");
                    t = (HtmlTableCell)cnt.FindControl("td12");  //suma
                    if (t != null) Tools.AddClass(t, "lastcol");
                    Tools.SetControlVisible(cnt, "thKier", all);
                    ShowStawki(cnt, "th", showStawki);
                    ShowUrlop(cnt, "th", showUrlop);
                    break;
                case nmoKier:
                    //td = (HtmlTableCell)cnt.FindControl("tdSumy");
                    //if (td != null) td.ColSpan = 2;
                    Tools.SetColSpan(cnt, "tdSumy", cspan);

                    Tools.SetControlVisible(cnt, "thDzial", false);

                    ShowStawki(cnt, "th", false);
                    ShowUrlop(cnt, "th", showUrlop);
                    
                    t = (HtmlTableCell)cnt.FindControl("th3");  //suma
                    if (t != null) Tools.AddClass(t, "lastcol");
                    t = (HtmlTableCell)cnt.FindControl("td3");
                    if (t != null) Tools.AddClass(t, "lastcol");
                    Tools.SetControlVisible(cnt, "thKier", all);
                    break;
                case nmoRcp:
                    //td = (HtmlTableCell)cnt.FindControl("tdSumy");
                    //if (td != null) td.ColSpan = 2;
                    Tools.SetColSpan(cnt, "tdSumy", cspan);

                    Tools.SetControlVisible(cnt, "thDzial", false);
                    ShowStawki(cnt, "th", false);
                    ShowUrlop(cnt, "th", false);

                    Tools.SetControlVisible(cnt, "th41", true);
                    Tools.SetControlVisible(cnt, "th42", true);
                    Tools.SetControlVisible(cnt, "th43", true);
                    t = (HtmlTableCell)Tools.SetControlVisible(cnt, "th44", true);
                    if (t != null) Tools.AddClass(t, "lastcol");
                    Tools.SetControlVisible(cnt, "thKier", all);
                    break;
            }
            Tools.SetControlVisible(cnt, "thCC", true);
#if SIEMENS || DBW || VICIM
            Tools.SetControlVisible(cnt, "thClass", false);
            Tools.SetText(cnt, "lbThCC", "Centrum kosztowe");
            Tools.SetControlVisible(cnt, "ltCzasNieprz", false);
            Tools.SetControlVisible(cnt, "ltCzasNieprzS", true);
#endif
        }

        private void PrepareView(Control cnt)
        {
            bool all = KierId == "-100";
            switch (Mode)
            {
                default:
                case nmoKierStawki:
                    HtmlTableCell td = (HtmlTableCell)cnt.FindControl("td12");
                    if (td != null) Tools.AddClass(td, "lastcol");
                    Tools.SetControlVisible(cnt, "tdKier", all);
                    ShowStawki(cnt, "td", showStawki);
                    ShowUrlop(cnt, "td", showUrlop);
                    break;
                case nmoProjStawki:
                    Tools.SetControlVisible(cnt, "tdDzial", false);
                    td = (HtmlTableCell)cnt.FindControl("td12");
                    if (td != null) Tools.AddClass(td, "lastcol");
                    Tools.SetControlVisible(cnt, "tdKier", all);
                    ShowStawki(cnt, "td", showStawki);
                    ShowUrlop(cnt, "td", showUrlop);
                    break;
                case nmoKier:
                    Tools.SetControlVisible(cnt, "tdDzial", false);
                    ShowStawki(cnt, "td", false);
                    ShowUrlop(cnt, "td", showUrlop);
                    td = (HtmlTableCell)cnt.FindControl("td3");
                    if (td != null) Tools.AddClass(td, "lastcol");
                    Tools.SetControlVisible(cnt, "tdKier", all);
                    break;
                case nmoRcp:
                    Tools.SetControlVisible(cnt, "tdDzial", false);
                    ShowStawki(cnt, "td", false);
                    ShowUrlop(cnt, "td", false);

                    Tools.SetControlVisible(cnt, "td41", true);
                    Tools.SetControlVisible(cnt, "td42", true);
                    Tools.SetControlVisible(cnt, "td43", true);
                    td = (HtmlTableCell)Tools.SetControlVisible(cnt, "td44", true);
                    if (td != null) Tools.AddClass(td, "lastcol");
                    Tools.SetControlVisible(cnt, "tdKier", all);
                    break;
            }
            Tools.SetControlVisible(cnt, "tdCC", true);
#if SIEMENS || DBW || VICIM
            Tools.SetControlVisible(cnt, "tdClass", false);
            //Tools.SetControlVisible(cnt, "lbClass", false);
            //Tools.SetControlVisible(cnt, "lbCC", true);
#endif
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
        public static void _GetSumy(PlanPracy.TimeSumator sumator, int zaokrSum, int zaokrSumType, out int pptime, out int wtime, out int ztime, out int otime50, out int otime100, out int orest, out int ntime, out int htime
            //,out int urlop, out int chorobowe, out int wolnezanadg
            ,out int dod50, out int dod100
            )
        {
            wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // całkowity 
            otime50  = sumator.GetSum(sumator.sumZTime, "150", true);                                            // nadgodziny 50 ze zmiany
            otime100 = sumator.GetSum(sumator.sumZTime, "200", true);                                           // nadgodziny 100 ze zmiany
            ztime  = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);                // pozostały czas ze zmiany (tu są absencje !!!)
            pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);              // zaplanowany
            otime50  += sumator.GetSum(sumator.sumOTime, "150", true);                                           // nadgodziny 50
            otime100 += sumator.GetSum(sumator.sumOTime, "200", true);                                          // nadgodziny 100
            otime50  = Worktime.RoundSec(otime50, zaokrSum, zaokrSumType);                                       // zaokr. nadgodziny 50
            otime100 = Worktime.RoundSec(otime100, zaokrSum, zaokrSumType);                                     // zaokr. nadgodziny 100
            orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadgodzin
            ntime = Worktime.RoundSec(sumator._sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // czas w niedziele i święta

            dod50  = Worktime.RoundSec(sumator.sumDod50, zaokrSum, zaokrSumType);
            dod100 = Worktime.RoundSec(sumator.sumDod100, zaokrSum, zaokrSumType);
            //dod50 = sumator.GetSum(sumator.sumZTime, "50", true);                                              // dodatek 50
            //dod100 = sumator.GetSum(sumator.sumZTime, "100", true);                                            // dodatek 100

            //urlop = 
        }

        /*
        public static void _GetSumy(PlanPracy.TimeSumator sumator, int zaokrSum, int zaokrSumType, out int pptime, out int wtime, out int ztime, out int otime50, out int otime100, out int orest, out int ntime, out int htime)
        {
            wtime = Worktime.RoundSec(sumator.sumWTime, zaokrSum, zaokrSumType);                                // 
            ztime = Worktime.RoundSec(sumator.GetSum(sumator.sumZTime), zaokrSum, zaokrSumType);                // czas na zmianach
            pptime = Worktime.RoundSec(sumator.GetSum(sumator.sumZmiany), zaokrSum, zaokrSumType);              // 
            otime50 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "150", true), zaokrSum, zaokrSumType); // nadgodziny 50
            otime100 = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime, "200", true), zaokrSum, zaokrSumType);// nadgodziny 100
            orest = Worktime.RoundSec(sumator.GetSum(sumator.sumOTime), zaokrSum, zaokrSumType);                // reszta nadgodzin
            ntime = Worktime.RoundSec(sumator.sumNTime, zaokrSum, zaokrSumType);                                // czas nocny
            htime = Worktime.RoundSec(sumator.sumHTime, zaokrSum, zaokrSumType);                                // czas w niedziele i święta
        }
        */ 
        //-----------------------------------------------------------
        protected void lvPlanPracy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SubItems":
                    string par = e.CommandArgument.ToString();  // kierId

                    //LinkButton lbt = (LinkButton)e.Item.FindControl("lbtPracownik");
                    LinkButton lbt = (LinkButton)e.CommandSource;
                    if (lbt != null && !String.IsNullOrEmpty(par))
                    {
                        string name = lbt.Text;
                        cntPath.AddPath(name, par, null);
                        _DataBindZoom(par);
                    }
                    break;
                    /*
                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    break;
                     */
            }
        }
        //------------
        protected void OnSelectPath(object sender, EventArgs e)
        {
            _DataBindZoom(cntPath.SelParam);
        }
        //-------------
        protected void lvPlanPracy_LayoutCreated(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case nmoKierStawki:
                case nmoProjStawki:
                    if (showStawki)
                    {
                        HtmlTable tb = (HtmlTable)lvNadgodziny.FindControl("lvOuterTable");
                        if (tb != null)
                            Tools.AddClass(tb, "narrow");
                    }
                    break;
                default:
                case nmoKier:
                case nmoRcp:
                    break;
            }
        }

        string zmDaneRcp = null;
        int zmWymiar = 28800;

        protected void lvPlanPracy_DataBinding(object sender, EventArgs e)
        {   
            /*
            Okres ok = new Okres(null, OkresId);
            stawkaNoc = ok.StawkaNocna;
            noData = ok.DateFrom > DateTime.Today;  // na pewno nie ma danych, nie ma sensu dołączać tu statusu
            incompleteData = false;                 // wartość początkowa
            lbNomMies.Text = ok.FriendlyName(1) + "(" + ok.DateFrom + ")";
            */
            DateTime dtFrom;
            DateTime dtTo;
            if (DateTime.TryParse(DateTo, out dtTo))
                lbNomMies.Text = Tools.DateFriendlyName(1, dtTo);
            if (DateTime.TryParse(DateFrom, out dtFrom))
                noData = dtFrom > DateTime.Today;   // na pewno nie ma danych, nie ma sensu dołączać tu statusu
            _stawkaNoc = StawkaNocna;                
            incompleteData = false;                 // wartość początkowa

            /*
            con = Base.Connect();
            _sumator = new PlanPracy.TimeSumator(con);
            //if (mode == nmoRcp) rcp_sumator = new PlanPracy.TimeSumator(con);
            total = new TimeSumator3(12 + 4);
            settings = Ustawienia.CreateOrGetSession();
            ilDniPrac = Worktime.GetIloscDniPrac(con, DateFrom, DateTo);
            ilDniPracMies = Worktime.GetIloscDniPracMies(con, DateTo);
            */

            switch (Mode)
            {
                case nmoRcp:
                    zmDaneRcp = Worktime2.zmRapRCP; /*db.getScalar("select top 1 * from Zmiany where TypZmiany = 1 and Margines = -1 and ZgodaNadg = 1 and Typ = 0");
                    if (String.IsNullOrEmpty(zmDaneRcp)) zmDaneRcp = "1";*/
                    break;
            }

            total = new TimeSumator3(lastSumIndex + 1); // 12 + 4 + 5 + 1);
            settings = Ustawienia.CreateOrGetSession();
        }

        const int lastSumIndex = 28;   // nadawac od nastepnego !!!

        protected void lvPlanPracy_DataBound(object sender, EventArgs e)
        {
            if (lvNadgodziny.Items.Count == 0)
            {
                tbStawkaNocna.Visible = false;
            }

            PrepareViewHeader(lvNadgodziny);
            PrepareView(lvNadgodziny);   // w podsumowaniu kolumny się tak samo nazywają !!!
            if (total != null)
            {
                Tools.SetText(lvNadgodziny, "lbSumNominalny", total.GetInt(0).ToString());     // (dni w miesiącu - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!
                Tools.SetText(lvNadgodziny, "lbSumZmianyPlan", SecToHourStr(total.Sum[1]));    // z PP ale z korektami
                Tools.SetText(lvNadgodziny, "lbSumSumaryczny", SecToHourStr(total.Sum[2]));    // zmiany + nadgodziny
                Tools.SetText(lvNadgodziny, "lbSPrzepracowany", SecToHourStr(total.Sum[21]));    // 
                //Tools.SetText(lvNadgodziny, "lbSumZmiany", SecToHourStr(total.Sum[3]));        // przepracowany na zmianach
                Tools.SetText(lvNadgodziny, "lbSumNieprzepracowany", SecToHourStr(total.Sum[3]));        // przepracowany na zmianach

                if (showUrlop)
                {
                    Tools.SetText(lvNadgodziny, "lbSUrlop", SecToHourStr(total.Sum[16]));
                    Tools.SetText(lvNadgodziny, "lbSChorobowe", SecToHourStr(total.Sum[17]));

                    Tools.SetText(lvNadgodziny, "lbSAbsencjeInne", SecToHourStr(total.Sum[25]));

                    Tools.SetText(lvNadgodziny, "lbSWolneZaNadg", SecToHourStr(total.Sum[18]));
                    Tools.SetText(lvNadgodziny, "lbSOdpracowane", SecToHourStr(total.Sum[26]));
                    Tools.SetText(lvNadgodziny, "lbSWyplata50", SecToHourStr(total.Sum[19]));
                    Tools.SetText(lvNadgodziny, "lbSWyplata100", SecToHourStr(total.Sum[20]));

                    Tools.SetText(lvNadgodziny, "lbSDodatek50", SecToHourStr(total.Sum[27]));
                    Tools.SetText(lvNadgodziny, "lbSDodatek100", SecToHourStr(total.Sum[28]));

                    Tools.SetText(lvNadgodziny, "lbSNierNiedomiar", SecToHourStr(total.Sum[22]));
                    Tools.SetText(lvNadgodziny, "lbSNierNadg50", SecToHourStr(total.Sum[23]));
                    Tools.SetText(lvNadgodziny, "lbSNierNadg100", SecToHourStr(total.Sum[24]));
                }

                                /*
                                nptime,
                                otime50, otime100, orest, ntime2, htime, w50, w100, wNoc,
                                rcp_wtime, rcp_nadg50, rcp_nadg100, rcp_noc,
                                urlop, chorobowe, wolnezanadg, wyplata50, wyplata100,
                                przepr,
                                nierNied, nier50, nier100, absinne
                                */

                object s6 = total.Sum[6];
                int orest = s6 == null ? 0 : (int)s6;                                                          // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane 
                Tools.SetText(lvNadgodziny, "lbSumNadg50", SecToHourStr(total.Sum[4]) + (orest > 0 ? "<br />? " + SecToHourStr(orest) : null));
                Tools.SetText(lvNadgodziny, "lbSumNadg100", SecToHourStr(total.Sum[5]));
                Tools.SetText(lvNadgodziny, "lbSumNocny", SecToHourStr(total.Sum[7]));
                Tools.SetText(lvNadgodziny, "lbSumNiedzieleSwieta", SecToHourStr(total.Sum[8]));

                switch (Mode)
                {
                    default:
                    case nmoKier:
                        tbStawkaNocna.Visible = false;
                        break;
                    case nmoRcp:
                        Tools.SetText(lvNadgodziny, "lbSumRcpSumaryczny", SecToHourStr(total.Sum[12]));
                        Tools.SetText(lvNadgodziny, "lbSumRcpNadg50", SecToHourStr(total.Sum[13]));
                        Tools.SetText(lvNadgodziny, "lbSumRcpNadg100", SecToHourStr(total.Sum[14]));
                        Tools.SetText(lvNadgodziny, "lbSumRcpNocny", SecToHourStr(total.Sum[15]));
                        tbStawkaNocna.Visible = false;
                        break;
                    case nmoKierStawki:
                    case nmoProjStawki:
                        Tools.SetText(lvNadgodziny, "lbSumStawkaGodz", "");
                        double w50 = total.GetDouble(9);
                        double w100 = total.GetDouble(10);
                        double wNoc = total.GetDouble(11);
                        Tools.SetText(lvNadgodziny, "lbSumWynagr50", w50.ToString("0.00"));
                        Tools.SetText(lvNadgodziny, "lbSumWynagr100", w100.ToString("0.00"));
                        Tools.SetText(lvNadgodziny, "lbSumWynagrNadgSuma", (w50 + w100).ToString("0.00"));
                        if (_stawkaNoc > 0)
                            Tools.SetText(lvNadgodziny, "lbSumWynagrNoc", wNoc.ToString("0.00"));
                        else
                            Tools.SetText(lvNadgodziny, "lbSumWynagrNoc", "-");

                        if (ilGodzPracMies <= 0)
                        {
                            lbNomCzasMies.Text = "brak danych";
                            lbNomCzasMies.ToolTip = "Proszę pobrać dane z systemu KP";
                        }
                        else
                        {
                            lbNomCzasMies.Text = ilGodzPracMies.ToString();
                            lbNomCzasMies.ToolTip = null;
                        }

                        if (_stawkaNoc <= 0)
                        {
                            lbStawkaNocne.Text = "brak danych";
                            lbStawkaNocne.ToolTip = "Proszę wprowadzić stawkę w parametrach okresów rozliczeniowych";
                        }
                        else
                        {
                            lbStawkaNocne.Text = _stawkaNoc.ToString("0.0000");
                            lbStawkaNocne.ToolTip = null;
                        }
                        tbStawkaNocna.Visible = showStawki;
                        break;
                }
            }
            tbInfo.Visible = !noData && incompleteData;  // pokazuję tylko jak mogą byc dane
        }

        protected void lvPlanPracy_PreRender(object sender, EventArgs e)    // dzięki temu nie znikają zaznaczenia krzyżowe
        {
        }
        //-------------------------------------------
        protected void lvPlanPracy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
        }
        //-------------------------------------------
        // na podstawie Okres.Accept
        
        
        // pracownicy w okresie od - do - lista 
        private void GetRcpData(string kierId, string pracId, string dateFrom, string dateTo, 
            //int zaokrSum, int zaokrSumTyp,
            string _alg, 
            string zmianaId,
            int zmWymiar,
            out int rcp_wtime, out int rcp_nadg50, out int rcp_nadg100, out int rcp_noc)
        {
            rcp_wtime = 0;
            rcp_nadg50 = 0;
            rcp_nadg100 = 0;
            rcp_noc = 0;
            PlanPracy.TimeSumator rcp_sumator = new PlanPracy.TimeSumator();

            Okres okres = new Okres(Base.StrToDateTime(dateTo));
            int? okresId = okres.IsArch() ? (int?)okres.Id : null;

            DataSet dsPrac;
            
            
            
            
            
            dsPrac = Worker.GetPracInfo2(1, null, okresId, null, pracId, false, false, false);



            //----- parametry ------           
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            KierParams kp = new KierParams(kierId, settings);

            int Fzaokr = kp.Settings.Zaokr;
            int FzaokrType = kp.Settings.ZaokrType;
            int FBreakMM = kp.Przerwa;
            int FBreak2MM = kp.Przerwa2;
            int FMarginMM = kp.Margines;

            foreach (DataRow drv in dsPrac.Tables[0].Rows)              // będzie tylko 1
            {
                pracId = drv["Id"].ToString();                          // dla 1 dnia nadpisze id tym samym
                bool kier = (bool)drv["Kierownik"];
                //----- pobieranie danych ------
                //string _rcpId = drv["RcpId"].ToString();
                //string _strefaId = drv["StrefaId"].ToString();

                //if (String.IsNullOrEmpty(_alg)) _alg = drv["Algorytm"].ToString();   //<<<<<<< symulacja ,chyba ze pusto                
                if (String.IsNullOrEmpty(_alg))
                    _alg = db.getScalar(String.Format("select RcpAlgorytm from PracownicyParametry where IdPracownika = {0} and '{1}' between Od and ISNULL(Od, '20990909')", pracId, dateTo));
                
                switch (_alg)
                {
                    default:
                    case "1":  // we-wy
                    case "3":  // obecnośc 8h
                        _alg = "11";
                        break;
                    case "2":  // suma w strefie
                        _alg = "12";
                        break;
                }
                //string _algPar = App.GetAlgPar(_alg);  // optymalniej niz dolaczac do query, pobierze tylko jak trzeba
                //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
                //if (String.IsNullOrEmpty(alg)) alg = "0";
                Worktime2 wt2;
                DataSet ds = Worktime2.GetWorktime(pracId, 
                                null,//_alg, 
                                null,//_algPar, 
                                
                                null,//_rcpId, 
                                
                                true, dateFrom, dateTo,
                                null,  //onDay,                    //dla acc PP to jest null
                                
                                null,//_strefaId,
                                
                                Fzaokr, FzaokrType,
                                kp.Settings.NocneOdSec, kp.Settings.NocneDoSec,
                                kp.Przerwa, kp.Przerwa2, 
                                zmianaId,   //<<<<<<< symulacja 
                                zmWymiar, /* wymiar */
                                true, 
                                ",K.Rodzaj as Kalendarz", "left outer join Kalendarz K on K.Data = D.Data", true,  // true korekta dla dni pracujących, false jak dla wszystkic dni - w wolne ustawiona zmiana
                                out wt2);

                //----- wypełnianie informacji w wierszu kolejne dni -----
                int cnt = db.getCount(ds);

                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getDataRow(ds, i);

                    DateTime date = (DateTime)db.getDateTime(dr, "Data");  // musi byc ok
                    DateTime? czasIn = db.getDateTime(dr, "TimeIn");
                    DateTime? czasOut = db.getDateTime(dr, "TimeOut");
                    bool isZmiana = !Base.isNull(dr, "ZmianaId");

                    int wtAlert = 0;
                    int wtime, _ztime, otimeD, otimeN, _ntime;  // łączny, zmiana, nadgodziny, nocne
                    int rzt, rnD, rnN, rN;
                    //int before6;

                    bool isWTime = Worktime.SolveWorktime2(dr, 
                        
                        
                        _alg, 
                        null,//_algPar,
                        


                        FBreakMM, FBreak2MM, FMarginMM, Fzaokr, FzaokrType,
                        true,                   // wartości skorygowane <<< te muszą być !!!
                        //false,                  // wartości rcp
                        out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                        out rzt, out rnD, out rnN, out rN,
                        //out before6,
                        false, ref wtAlert);    // alerty RCP mnie tu nie interesują, powinien sprawdzać alerty Kier lub minimum żeby zaakceptować - i tak robie ...
                    
                    if (rzt == -1) rzt = 0;
                    int? zt, otD, otN, nt;
                    int wolne = Base.getInt(dr, "Kalendarz", -1);     // 0 sobota, 1 niedziela, 2 święto
                    if (wolne >= 0)  // dni wolne - zmiana wpisana i czas skorygowany
                        Worktime.WorktimeToPP(Fzaokr, FzaokrType, _ztime, otimeD, otimeN, _ntime, 
                                                      out zt, out otD, out otN, out nt);
                    else             // dni pracujące - z RCP
                        Worktime.WorktimeToPP(Fzaokr, FzaokrType, rzt, rnD, rnN, rN, //_ztime, otimeD, otimeN, _ntime,
                                                      out zt, out otD, out otN, out nt);
                    //-----------
                    string zmiana = db.getValue(dr, "ZmianaId");
                    if (db.isNull(zt)) zt = 0;
                    if (db.isNull(otD)) otD = 0;
                    if (db.isNull(otN)) otN = 0;
                    if (db.isNull(nt)) nt = 0;
                    int t50, t100;  // w danym dniu 
                    int d50, d100;
                    bool zeroZm;
                    bool isAbs = !String.IsNullOrEmpty(db.getValue(dr, "AbsencjaKod"));
                    rcp_sumator._SumTimes2(date, zmiana, wolne >= 1, (int)zt, (int)otD, (int)otN, (int)nt, out t50, out t100, isAbs, czasIn, out d50, out d100, out zeroZm);   //0-sobota 1-niedziela 2-swieto
                }

                int pptime, ztime, orest, htime;
                int dod50, dod100;

                _GetSumy(rcp_sumator, settings.ZaokrSum, settings.ZaokrSumType,
                        out pptime, out rcp_wtime, out ztime, out rcp_nadg50, out rcp_nadg100, out orest, out rcp_noc, out htime, out dod50, out dod100);
            }
        }


        //-----------------
        private void FillCzas(ListViewItemEventArgs e, string nominalny, string zmianyPlan, string sumaryczny, string przepr, string nieprzepr, string nadg50, string nadg100, string nocny, string niedziświeta)
        {
            Tools.SetText(e.Item, "lbNominalny", nominalny);
            Tools.SetText(e.Item, "lbZmianyPlan", zmianyPlan);
            Tools.SetText(e.Item, "lbSumaryczny", sumaryczny);
            Tools.SetText(e.Item, "lbPrzepracowany", przepr);
            //Tools.SetText(e.Item, "lbZmiany", "-");
            Tools.SetText(e.Item, "lbNieprzepracowany", nieprzepr);
            Tools.SetText(e.Item, "lbNadg50", nadg50);
            Tools.SetText(e.Item, "lbNadg100", nadg100);
            Tools.SetText(e.Item, "lbNocny", nocny);
            Tools.SetText(e.Item, "lbNiedzieleSwieta", niedziświeta);
        }

        private void FillUrlop(ListViewItemEventArgs e, string urlop, string chorobowe, string absinne, string wolnezanadg, string odpracowane, 
                                string wyplata50, string wyplata100, string dodatek50, string dodatek100, string niernied, string nier50, string nier100)
        {
            Tools.SetText(e.Item, "lbUrlop", urlop);
            Tools.SetText(e.Item, "lbChorobowe", chorobowe);
            Tools.SetText(e.Item, "lbAbsencjeInne", absinne);

            Tools.SetText(e.Item, "lbWolneZaNadg", wolnezanadg);
            Tools.SetText(e.Item, "lbOdpracowane", odpracowane);
            Tools.SetText(e.Item, "lbWyplata50", wyplata50);
            Tools.SetText(e.Item, "lbWyplata100", wyplata100);

            Tools.SetText(e.Item, "lbDodatek50", dodatek50);
            Tools.SetText(e.Item, "lbDodatek100", dodatek100);

            Tools.SetText(e.Item, "lbNierNiedomiar", niernied);
            Tools.SetText(e.Item, "lbNierNadg50", nier50);
            Tools.SetText(e.Item, "lbNierNadg100", nier100);
        }

        private void FillRcpCzas(ListViewItemEventArgs e, string sumaryczny, string nadg50, string nadg100, string nocny)
        {
            Tools.SetText(e.Item, "lbRcpSumaryczny", sumaryczny);
            Tools.SetText(e.Item, "lbRcpNadg50", nadg50);
            Tools.SetText(e.Item, "lbRcpNadg100", nadg100);
            Tools.SetText(e.Item, "lbRcpNocny", nocny);
        }

        private void FillStawki(ListViewItemEventArgs e, string stawka, string stawkaGodz, string wynagr50, string wynagr100, string wynagrNadgSuma, string wynagrNocne)
        {
            Tools.SetText(e.Item, "lbStawka", stawka);
            Tools.SetText(e.Item, "lbStawkaGodz", stawkaGodz);
            Tools.SetText(e.Item, "lbWynagr50", wynagr50);
            Tools.SetText(e.Item, "lbWynagr100", wynagr100);
            Tools.SetText(e.Item, "lbWynagrNadgSuma", wynagrNadgSuma);
            Tools.SetText(e.Item, "lbWynagrNoc", wynagrNocne);
        }





        private string SecToHourStr(object sec)
        {
            if (sec == null || sec.Equals(DBNull.Value))
                return null;
            else
            {
                int ss = (int)sec;
                double hh = Worktime.Round05((double)ss / 3600, 4);
                return hh.ToString();
            }
        }




        private void FillNoData(ListViewItemEventArgs e)
        {
            FillCzas(e, "-", "-", "-", "-", "-", "-", "-", "-", "-");
            FillUrlop(e, "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-");
            switch (Mode)
            {
                case nmoKierStawki:
                case nmoProjStawki:
                    if (showStawki)
                        FillStawki(e, "-", "-", "-", "-", "-", "-");
                    break;
                case nmoRcp:
                    FillRcpCzas(e, "-", "-", "-", "-");
                    break;
            }
        }


        
        
        
        
        //---- czas pracownika zwolnionego zatrudnionego w okresie z uwzględnieniem wolnego za święto przypadające w weekend -----
#if NOMWYM
        // wolne za świeo jest uwzględniane w kalendarzu !!! wiec korekta chyba niepotrzebna ...
        public static bool GetPracDniPracSec(string pracId, string dFrom, string dTo, int ilDniPracMies, out int ilDniPrac2, out int ilDniPracMies2)
        {
            ilDniPrac2 = Worktime.GetIloscDniPracPracWymiar(pracId, dFrom, dTo);    // wg kalendarza uwzględnia nowy/zwolniony
            ilDniPracMies2 = Worktime.GetIloscDniPracMiesPracWymiar(pracId, dTo);      // wg kalendarza uwzględnia nowy/zwolniony
            return true;  
        }
#endif
        public static bool _GetPracDniPrac(string pracId, string dFrom, string dTo, int ilDniPracMies, out int ilDniPrac2, out int ilDniPracMies2)
        {
            ilDniPrac2 = Worktime.GetIloscDniPracPrac(pracId, dFrom, dTo);    // wg kalendarza uwzględnia nowy/zwolniony
            ilDniPracMies2 = Worktime.GetIloscDniPracMiesPrac(pracId, dTo);      // wg kalendarza uwzględnia nowy/zwolniony

            if (ilDniPracMies2 > ilDniPracMies)
            {
                ilDniPracMies2 = ilDniPracMies;         // jabil - jeżeli wg kalendarza jest więcej to pracownik pownien wybrać dzień wolny - moze dowolny <<< tu jest niejednoznaczeni jezeli chodzi o zatrudnionych / zwolnionych bo wypadąło by sprawdzać czy dzień do wybrania za święto w dzień wolny się łapie czy nie ... - trzeba by dodać do kalendarza ... a to będzie mieć wpływ na wszystko ... 
                DateTime dtFrom;
                DateTime dtTo;
                if (DateTime.TryParse(dTo, out dtTo) &&
                    DateTime.TryParse(dFrom, out dtFrom))
                {
                    if (dtFrom.AddMonths(1).AddDays(-1) == dtTo)    // pełen miesiąc          
                    {
                        ilDniPrac2 = ilDniPracMies2;                // czas wg Kalendarza = czas Nominalny
                        return true;
                    }
                }
            }
            return false;
        }





        protected void lvPlanPracy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PrepareView(e.Item);
                int mode = Mode;

                /**/
                if (con == null)
                {
                    con = Base.Connect();
                    _sumator = new PlanPracy.TimeSumator();
                    //total = new TimeSumator3(12 + 4);             // przeniesione do DataBinding, bo się DataBound moze 2 razy wykonać np. CzasPracy i wtedy nie resetował sum !!!; ja jakims z raportów DataDo nie była w DataBinding jeszcze ustawiona i sie wywalał ...
                    //settings = Ustawienia.CreateOrGetSession();
                    ilDniPrac = Worktime._GetIloscDniPrac(con, DateFrom, DateTo);    // wg kalendarza 
                    ilDniPracMies = Worktime._GetIloscDniPracMies(con, DateTo);      // nominalne w miesiącu CzasNom


                }
                else
                {
                    _sumator.Reset();
                }
                /**/
                

                //_sumator.Reset();

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                string pracId = drv["Id"].ToString();



                //---- czas pracownika zwolnionego zatrudnionego w okresie z uwzględnieniem wolnego za święto przypadające w weekend -----
#if NOMWYM
                int ilDniPrac2Sec;         // w okresie rozliczeniowym
                int ilDniPracMies2Sec;     // w miesiącu
                GetPracDniPracSec(pracId, DateFrom, DateTo, ilDniPracMies, out ilDniPrac2Sec, out ilDniPracMies2Sec);    // jabil - jeżeli wg kalendarza jest więcej to pracownik pownien wybrać dzień wolny - moze dowolny <<< tu jest niejednoznaczeni jezeli chodzi o zatrudnionych / zwolnionych bo wypadąło by sprawdzać czy dzień do wybrania za święto w dzień wolny się łapie czy nie ... - trzeba by dodać do kalendarza ... a to będzie mieć wpływ na wszystko ... 
#endif
                int _ilDniPrac2;         // w okresie rozliczeniowym
                int _ilDniPracMies2;     // w miesiącu
                _GetPracDniPrac(pracId, DateFrom, DateTo, ilDniPracMies, out _ilDniPrac2, out _ilDniPracMies2);    // jabil - jeżeli wg kalendarza jest więcej to pracownik pownien wybrać dzień wolny - moze dowolny <<< tu jest niejednoznaczeni jezeli chodzi o zatrudnionych / zwolnionych bo wypadąło by sprawdzać czy dzień do wybrania za święto w dzień wolny się łapie czy nie ... - trzeba by dodać do kalendarza ... a to będzie mieć wpływ na wszystko ... 




                /*
                if (pracId == "528")
                {
                    int x = 0;
                }
                */

                //----- nie licze pracownika -----
                int _alg = Base.getInt(drv["Algorytm"], -1);
                bool pracOff = _alg == 0;
                
                int etatL = Base.getInt(drv["EtatL"], -1);
                int etatM = Base.getInt(drv["EtatM"], -1);
                
                
                int etat = etatL > 0 && etatM > 0 ? 8 * etatL / etatM : -1;   // il godzin pracy w dniu lub -1
                float _etat = etatL > 0 && etatM > 0 ? (float)8 * etatL / etatM : -1;   // il godzin pracy w dniu lub -1
                
                
                //----- kierownik / pracownik ------
                bool kier = Base.getBool(drv["Kierownik"], false);
                Tools.SetControlVisible(e.Item, "PracownikLabel", !kier);
                Tools.SetControlVisible(e.Item, "lbtPracownik", kier);
                //----- czas pracy -----
                bool allAccepted = true;           // wszystkie dni są poakceptowane >>> 
                bool showRcp;
                int pptime, wtime, _ztime2, otime50, otime100, orest, ntime2, htime;    // htime - praca w niedziele i święta, sumator sam to liczy!!!
                int urlop, chorobowe, absinne;
                int absencjeH;                      // absencjeH - czas wszystkich absencji zaznaczonych ze 8h 
                int wolnezanadg, odpracowane, wyplata50, wyplata100;
                int dodatek50, dodatek100;
                int nierNied, nier50, nier100;
                int przepr;

                DataSet ds;
                if (!pracOff)
                {
                    /*
                    ds = Base.getDataSet(con, String.Format(
                             // 0      1        2                 3               4       5      6               7                  8         9 
                        "select P.Data,P.CzasZm,P.NadgodzinyDzien,P.NadgodzinyNoc,P.Nocne,A.Kod, AK.GodzinPracy, Z.Id as TypZmiany, K.Rodzaj, P.Akceptacja " +
                        "from PlanPracy P " +
                            "left outer join Zmiany Z on Z.Id = ISNULL(P.IdZmianyKorekta, P.IdZmiany) " +
                            "left outer join Kalendarz K on K.Data = P.Data " +

                            "left outer join Absencja A on A.IdPracownika = P.IdPracownika and P.Data between A.DataOd and A.DataDo " +
                            "left outer join AbsencjaKody AK on AK.Kod = A.Kod " +

                        "where P.Data between '{0}' and '{1}' and P.IdPracownika={2}"
                        //+ " order by P.Data"
                        , DateFrom, DateTo, pracId
                        ));
                    */

//     0       1         2                  3                4        5     6              7                 8        9              10         11      12       13      14       15  -- od 11 nie byly wykorzystywane ...
                    const string select = @"
select D.Data, PP.CzasZm,PP.NadgodzinyDzien,PP.NadgodzinyNoc,PP.Nocne,A.Kod,AK.GodzinPracy,Z.Id as TypZmiany,K.Rodzaj,PP.Akceptacja, PP.CzasIn, PP.n50, PP.n100, PP.d50, PP.d100, R.Id as IdPrzypisania
from dbo.GetDates2('{0}','{1}') D 
    left join PlanPracy PP on PP.Data = D.Data and PP.IdPracownika={2}
    left join Przypisania R on R.IdPracownika = PP.IdPracownika and PP.Data between R.Od and ISNULL(R.Do, '20990909') and R.Status = 1
    left join Zmiany Z on Z.Id = ISNULL(PP.IdZmianyKorekta, PP.IdZmiany)
    left join Kalendarz K on K.Data = D.Data
    left join Absencja A on A.IdPracownika = {2} and D.Data between A.DataOd and A.DataDo
    left join AbsencjaKody AK on AK.Kod = A.Kod";

                    ds = db.getDataSet(String.Format(select, DateFrom, DateTo, pracId));

                    //---------------
                    if (showUrlop)
                    {
                        const string sqlUrlop = @"
declare @pracId int
declare @od datetime
declare @do datetime
set @pracId = {0}
set @od = '{1}'
set @do = '{2}'

select COUNT(AU.Kod), COUNT(AC.Kod), 
--SUM(AAK.GodzinPracy) 
COUNT(AA.Kod)
from dbo.GetDates2(@od, @do) D
left join Kalendarz K on K.Data = D.Data
left join Absencja AU on D.Data between AU.DataOd and AU.DataDo and AU.IdPracownika = @pracId and AU.Kod in ({3})
left join Absencja AC on D.Data between AC.DataOd and AC.DataDo and AC.IdPracownika = @pracId and AC.Kod in ({4})
left join Absencja AA on D.Data between AA.DataOd and AA.DataDo and AA.IdPracownika = @pracId 
--left join AbsencjaKody AAK on AAK.Kod = AA.Kod
where K.Rodzaj is null";

                        const string sqlWolneZaNadg = @"
declare @IdPracownika int
declare @od datetime
declare @do datetime
set @IdPracownika = {0}
set @od = '{1}'
set @do = '{2}'

select
	(select SUM(ISNULL(P.CzasZm, 0) + ISNULL(P.NadgodzinyDzien, 0) + ISNULL(P.NadgodzinyNoc, 0)) 
	from PlanPracy P
	where P.IdPracownika = @IdPracownika and P.Data between @od and @do and P.Akceptacja = 1) as Przepracowany,

    --sum(Niedomiar) as Niedomiar,	-- policzony, do rozliczenia
    --sum(N50) as N50, 
    --sum(N100) as N100,
    
    sum(Niedomiar + WND + OND) as drNiedomiar,		-- nierozliczony
    sum(N50 - (P50 + W50 + O50)) as dr50,
    sum(N100 - (P100 + W100 + O100)) as dr100,
    
    --sum(PND) as PND,	-- do wypłaty
    sum(P50) as DoWyplaty50, 
    sum(P100) as DoWyplaty100,
    
    sum(WND) as WolneZaNadg,	-- wolne za nadgodziny
    --sum(W50) as W50, 
    --sum(W100) as W100,
    
    sum(OND) as Odpracowane--,	-- odpracowany
    --sum(O50) as O50, 
    --sum(O100) as O100
FROM  VRozliczenieNadgodzin 
where IdPracownika = @IdPracownika and Data between @od and @do

and (ZaDzien is null or ZaDzien between @od and @do)

";

//20140324 


                        /*
declare @pracId int
declare @od datetime
declare @do datetime
set @pracId = {0}
set @od = '{1}'
set @do = '{2}'

select
(select ISNULL(SUM(P.CzasZm), 0) 
from PodzialNadgodzin P
where P.IdPracownika = @pracId and P.Data between @od and @do and P.RodzajId in (2,4,5,12,14,15)) as WolneZaNadg,   --Kody.Par2 = 2
(select ISNULL(SUM(P.n50), 0) 
from PodzialNadgodzin P
where P.IdPracownika = @pracId and P.Data between @od and @do and P.RodzajId in (1)) as DoWylaty50,
(select ISNULL(SUM(P.n100), 0) 
from PodzialNadgodzin P
where P.IdPracownika = @pracId and P.Data between @od and @do and P.RodzajId in (1)) as DoWylaty100,
(select SUM(ISNULL(P.CzasZm, 0) + ISNULL(P.NadgodzinyDzien, 0) + ISNULL(P.NadgodzinyNoc, 0)) 
from PlanPracy P
where P.IdPracownika = @pracId and P.Data between @od and @do and P.Akceptacja = 1) as Przepracowany
                         */

                        DataRow drUrlop = db.getDataRow(String.Format(sqlUrlop, pracId, DateFrom, DateTo, UrlopyKody, ChoroboweKody)); //"510,511", "500,525,526"));
                        DataRow drWolne = db.getDataRow(String.Format(sqlWolneZaNadg, pracId, DateFrom, DateTo));

                        urlop       = db.getInt(drUrlop, 0, 0) * 8 * 3600 * etatL / etatM;      // div !!!
                        chorobowe   = db.getInt(drUrlop, 1, 0) * 8 * 3600 * etatL / etatM;
                        //absencjeH   = db.getInt(drUrlop, 2, 0) * 3600 * etatL / etatM;        // bo juz tam jest ilosc  <<< licze nawet jak nie są płatne
                        absencjeH   = db.getInt(drUrlop, 2, 0) * 8 * 3600 * etatL / etatM;     
                        absinne = absencjeH - urlop - chorobowe;

                        wolnezanadg = db.getInt(drWolne, "WolneZaNadg", 0);
                        odpracowane = db.getInt(drWolne, "Odpracowane", 0);
                        wyplata50   = db.getInt(drWolne, "DoWyplaty50", 0);
                        wyplata100  = db.getInt(drWolne, "DoWyplaty100", 0);

                        nierNied    = db.getInt(drWolne, "drNiedomiar", 0);
                        nier50      = db.getInt(drWolne, "dr50", 0);
                        nier100     = db.getInt(drWolne, "dr100", 0);
                    }
                    else
                    {
                        urlop = 0;
                        chorobowe = 0;
                        absinne = 0;
                        absencjeH = 0;
                        wolnezanadg = 0;
                        odpracowane = 0;
                        wyplata50 = 0;
                        wyplata100 = 0;
                        nierNied = 0;
                        nier50 = 0;
                        nier100 = 0;
                    }
                    //---------------
                    if (showPrzeprac)  // SIEMENS
                    {
                        const string sqlPrzeprac = @"
declare @pracId int
declare @od datetime
declare @do datetime
set @pracId = {0}
set @od = '{1}'
set @do = '{2}'

select dbo.RoundSec(SUM(ISNULL(P.CzasZm, 0) + ISNULL(P.NadgodzinyDzien, 0) + ISNULL(P.NadgodzinyNoc, 0)), {3}, {4}) 
from PlanPracy P
where P.IdPracownika = @pracId and P.Data between @od and @do and P.Akceptacja = 1
";
                        DataRow drPrzepr = db.getDataRow(String.Format(sqlPrzeprac, pracId, DateFrom, DateTo, settings.ZaokrSum, settings.ZaokrSumType));
                        przepr = db.getInt(drPrzepr, 0, 0);
                    }
                    else
                    {
                        przepr = 0;
                    }
                    //---------------
                    int cnt = ds.Tables[0].Rows.Count;   // select from GetDates wiec zawsze cos jest
                    bool absPraca = false;
                    if (cnt == 0) allAccepted = false;  // brak danych - nie ma planu pracy , ale to chyba niemozliwe bo select from GetDates
                    else
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            DataRow dr = Base.getDataRow(ds, i);
                            DateTime date = Base.getDateTime(dr, 0, DateTime.MinValue);
                            bool acc = Base.getBool(dr, 9, false);
                            bool isPrzypisanie = !db.isNull(dr, 15);

                            int ztime, otimeD, otimeN, ntime;

                            if (acc)
                            {
                                ztime = Base.getInt(dr, 1, 0);
                                otimeD = Base.getInt(dr, 2, 0);
                                otimeN = Base.getInt(dr, 3, 0);
                                ntime = Base.getInt(dr, 4, 0);
                            }
                            else    // jak nie poakceptowane to 0
                            {
                                /* 20130128 niech pokaże nawet jak nie sa poakceptowane                                
                                ztime = 0;
                                otimeD = 0;
                                otimeN = 0;
                                ntime = 0;
                                /**/
                                //*
                                ztime = Base.getInt(dr, 1, 0);
                                otimeD = Base.getInt(dr, 2, 0);
                                otimeN = Base.getInt(dr, 3, 0);
                                ntime = Base.getInt(dr, 4, 0);
                                /**/

                                if (isPrzypisanie)      // jak nie ma przypisania to nie powinien zgłaszać błędu
                                    allAccepted = false;
                            }

                            string abs = Base.getValue(dr, 5);
                            int absGodzPracy = Base.getInt(dr, 6, 0);
                            string zmiana = Base.getValue(dr, 7);
                            int _wolne = Base.getInt(dr, 8, -1);     // 0 sobota, 1 niedziela, 2 święto
                            //----- absencje jako czas pracy -----
                            bool isZmiana = !String.IsNullOrEmpty(zmiana);
                            bool isAbs = !String.IsNullOrEmpty(abs);
                            if (isAbs)
                            {
                                if (ztime + otimeD + otimeN > 0) absPraca = true;   // także wtedy kiedy tylko nadgodziny !
                                if (absGodzPracy > 0 && _wolne == -1)   // liczę jako czas pracy jeśli nie było pracy i dzień nie jest wolny (bez sobot, niedziel i świąt)
                                    //if (absGodzPracy > 0 && isZmiana)     // liczę jako czas pracy jeśli nie było pracy i zaplanowana/skorygowana zmiana <<< po staremu bo KP nie pozwala na wprowadzanie i sie rozjedzie
                                    if (ztime == 0)                     // praca ma priorytet
                                        if (etatF)
                                            ztime = (int)(((_etat > 0 ? _etat : 8) * 3600 * absGodzPracy) / 8);
                                        else
                                            ztime = ((etat > 0 ? etat : 8) * 3600 * absGodzPracy) / 8;
                            }
                            //----- sumy -----
                            int t50, t100;  // w danym dniu 
                            int d50, d100;  // dodatek 50 i 100     
                            bool zeroZm;
                        
                            //int before6 = Worktime.GetBefore6(dr["CzasIn"], ztime);

                            _sumator._SumTimes2(date, zmiana, _wolne >= 1, ztime, otimeD, otimeN, ntime, out t50, out t100, isAbs, dr["CzasIn"], out d50, out d100, out zeroZm);   //0-sobota 1-niedziela 2-swieto
                        }
                        if (cnt < _ilDniPrac2)           // ilDniPrac2 powinna być <= niż ilość dni w okresie, jeżeli jest odwrotnie to brak planu pracy
                            allAccepted = false;    // brak planu, a np. tylko 1 dzień zaakceptowany
                    }
                    //----- sumy -----
                    //if (allAccepted || !noData)     // jak wszystko poakceptowane lub kiedy są dane <<< wystarczyłby sam warunek że są (mogą być) dane 
                    if (!noData)   // są dane
                    {
                        _GetSumy(_sumator, settings.ZaokrSum, settings.ZaokrSumType,
                                out pptime, out wtime, out _ztime2, out otime50, out otime100, out orest, out ntime2, out htime
                                //out urlop, out chorobowe, out wolnezanadg
                                ,out dodatek50, out dodatek100
                                );
                    }
                    else                    // wszystkie na 0
                    {
                        pptime = 0;
                        wtime = 0;
                        _ztime2 = 0;
                        otime50 = 0;
                        otime100 = 0;
                        orest = 0;
                        ntime2 = 0;
                        htime = 0;

                        urlop = 0;
                        chorobowe = 0;
                        wolnezanadg = 0;
                        odpracowane = 0;
                        wyplata50 = 0;
                        wyplata100 = 0;

                        dodatek50 = 0;
                        dodatek100 = 0;

                        przepr = 0;
                    }
                    //----- RCP -----
                    int rcp_wtime = 0;
                    int rcp_nadg50 = 0;
                    int rcp_nadg100 = 0;
                    int rcp_noc = 0;
                    if (mode == nmoRcp)
                    {
                        GetRcpData(App.User.Id, pracId, DateFrom, DateTo,
                            //settings.ZaokrSum, settings.ZaokrSumType,        
                            null, //"11",       // we-wy + nadg  <<< niech sam wybierze dodając nadgodziny
                            zmDaneRcp,   //"1",                // Z1
                            zmWymiar, /* wymiar */
                            out rcp_wtime, out rcp_nadg50, out rcp_nadg100, out rcp_noc);
                    }
                    //-----
                    //----- nominalny i nieprzepracowany -----
                    int ilGodzPrac2, ilGodzPracMies2;
                    string nominalny2;
                    string _nominalny;
                    if (etatF)
                    {
                        if (_etat > 0)
                        {
                            ilGodzPrac = (int)(ilDniPrac * _etat);
                            ilGodzPracMies = (int)(ilDniPracMies * _etat);

                            ilGodzPrac2 = (int)(_ilDniPrac2 * _etat);
                            ilGodzPracMies2 = (int)(_ilDniPracMies2 * _etat);
#if NOMWYM
                            nominalny2 = Worktime.SecToHourStr(ilDniPrac2Sec);
#else
                            nominalny2 = ilGodzPrac2.ToString();         // (dni w okresie! - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!, ale tam jest co do miesiąca więc źle ... 
#endif
                        }
                        else
                        {
                            ilGodzPrac = ilDniPrac * 8;
                            ilGodzPracMies = ilDniPracMies * 8;

                            ilGodzPrac2 = _ilDniPrac2 * 8;
                            ilGodzPracMies2 = _ilDniPracMies2 * 8;         // wynika z możliwości ustawienia okresu rozliczeniowego nie od 1 dnia miesiąca                        
#if NOMWYM
                            nominalny2 = Worktime.SecToHourStr(ilDniPrac2Sec) + "<br />błąd etatu";
#else
                            nominalny2 = ilGodzPrac2.ToString() + "<br />błąd etatu";
#endif
                        }
                    }
                    else
                    {
                        if (etat > 0)
                        {
                            ilGodzPrac = ilDniPrac * etat;
                            ilGodzPracMies = ilDniPracMies * etat;

                            ilGodzPrac2 = _ilDniPrac2 * etat;
                            ilGodzPracMies2 = _ilDniPracMies2 * etat;
#if NOMWYM
                            nominalny2 = Worktime.SecToHourStr(ilDniPrac2Sec);
#else
                            nominalny2 = ilGodzPrac2.ToString();         // (dni w okresie! - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!, ale tam jest co do miesiąca więc źle ... 
#endif
                        }
                        else
                        {
                            ilGodzPrac = ilDniPrac * 8;
                            ilGodzPracMies = ilDniPracMies * 8;

                            ilGodzPrac2 = _ilDniPrac2 * 8;
                            ilGodzPracMies2 = _ilDniPracMies2 * 8;         // wynika z możliwości ustawienia okresu rozliczeniowego nie od 1 dnia miesiąca                        
#if NOMWYM
                            nominalny2 = Worktime.SecToHourStr(ilDniPrac2Sec) + "<br />błąd etatu";
#else
                            nominalny2 = ilGodzPrac2.ToString() + "<br />błąd etatu";
#endif
                        }
                    }
                    int nptime = 0;
                    //----- -----

                    //ilość przepracowanych = niminalny(np.168)-niedomiar+nadgodziny-nieobecnosci ???

                    //if (allAccepted || !noData)
                    if (!noData)
                    {
                        if (showPrzeprac)   // SIEMENS
                            nptime = ilGodzPrac2 * 3600 - (przepr - otime50 - otime100) - absencjeH;
                        else                // Jabil
                            nptime = ilGodzPrac2 * 3600 - _ztime2;
                        if (nptime < 0) nptime = 0;
                        FillCzas(e, nominalny2, 
                            SecToHourStr(pptime),              // zaplanowany z PP ale z korektami
                            SecToHourStr(wtime) + (absPraca ? "<br />praca i<br />absencje!" : null),  // sumaryczny; zmiany + nadgodziny
                            //SecToHourStr(ztime2),            // przepracowany na zmianach

                            SecToHourStr(przepr) + (absPraca ? "<br />praca i<br />absencje!" : null), // przepracowany  <<< w Siemesie nie ma wtime
         
                            SecToHourStr(nptime),              // nieprzepracowany 
                            SecToHourStr(otime50) + (orest > 0 ? "<br />? " + SecToHourStr(orest) : null),  // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane
                            SecToHourStr(otime100), 
                            SecToHourStr(ntime2),              // przepracowany w nocy
                            SecToHourStr(htime));              // przepracowany w niedziele i święta
                        if (showUrlop) FillUrlop(e, 
                            SecToHourStr(urlop),               // urlop
                            SecToHourStr(chorobowe),           // chorobowe
                            SecToHourStr(absinne),             // inne absencje

                            SecToHourStr(wolnezanadg),         // wolne za nadgodziny
                            SecToHourStr(odpracowane),         // godziny odpracowane
                            SecToHourStr(wyplata50),           // do wypłaty 50
                            SecToHourStr(wyplata100),          // do wypłaty 100
                            
                            SecToHourStr(dodatek50),
                            SecToHourStr(dodatek100),

                            SecToHourStr(nierNied),            // niedomiar nierozliczony
                            SecToHourStr(nier50),              // nierozliczone nadgodziny 50
                            SecToHourStr(nier100)              // nierozliczone nadgodziny 100
                            );
                        if (mode == nmoRcp) FillRcpCzas(e, 
                            SecToHourStr(rcp_wtime),
                            SecToHourStr(rcp_nadg50),
                            SecToHourStr(rcp_nadg100),
                            SecToHourStr(rcp_noc));
                    }
                    else
                    {
                        FillCzas(e, nominalny2, "-", "-", "-", "-", "-", "-", "-", "-");
                        if (showUrlop) FillUrlop(e, "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-");
                        if (mode == nmoRcp) FillRcpCzas(e, "-", "-", "-", "-");
                    }
                    //----- sumy total -----
                    double w50 = 0;
                    double w100 = 0;
                    double wNoc = 0;
                    switch (mode)
                    {
                        case nmoKierStawki:
                        case nmoProjStawki:
                            object ostawka = drv["Stawka"];
                            double _stawka = Base.isNull(ostawka) ? 0 : Convert.ToDouble(ostawka);
                            double stawkah;
                            //int godzPrac = _ilGodzPrac;

                            int godzPrac = ilGodzPracMies;  // stawka wg ilosci dni prac w miesiącu!
                            
                            if (!Base.isNull(ostawka) && godzPrac > 0)
                            {
                                stawkah = Worktime.Round05(_stawka / godzPrac, 4);
                                w50 = Worktime.Round05(Worktime.SecToHour(otime50) * stawkah * p50, 2);
                                w100 = Worktime.Round05(Worktime.SecToHour(otime100) * stawkah * p100, 2);
                                if (_stawkaNoc > 0)
                                    wNoc = Worktime.Round05(Worktime.SecToHour(ntime2) * _stawkaNoc, 2);
                                else
                                    wNoc = 0;
                            }
                            else
                            {
                                stawkah = 0;
                                w50 = 0;
                                w100 = 0;
                                wNoc = 0;
                            }
                            if (Base.isNull(ostawka)) 
                                FillStawki(e, "brak danych", "-", "-", "-", "-", "-");
                            else if (!noData)   //if (allAccepted || !noData)
                                FillStawki(e,          
                                    _stawka.ToString("0.00"), 
                                    stawkah.ToString("0.0000"),
                                    w50.ToString("0.00"),
                                    w100.ToString("0.00"),
                                    (w50 + w100).ToString("0.00"),
                                    _stawkaNoc > 0 ? wNoc.ToString("0.00") : "-");
                            else 
                                FillStawki(e, _stawka.ToString("0.00"), "-", "-", "-", "-", "-");
                            break;
                    }
                    total.SumTimes(ilGodzPrac2, pptime, wtime,
                        //ztime2, 
                                nptime,
                                otime50, otime100, orest, ntime2, htime, w50, w100, wNoc,
                                rcp_wtime, rcp_nadg50, rcp_nadg100, rcp_noc,
                                urlop, chorobowe, wolnezanadg, wyplata50, wyplata100,
                                przepr,
                                nierNied, nier50, nier100, absinne,
                                odpracowane,
                                dodatek50, dodatek100
                                );
                }
                else FillNoData(e);  //pracOff - alg=bezLiczenia; Pracownik.Status = stPomin <<< w select jest ograniczenie                    

                if (!allAccepted) incompleteData = true;                                // dane nie sa kompletne!!!
                Tools.SetControlVisible(e.Item, "lbWarning", !noData && !allAccepted);  // tylko pokazuję jak mogą byc dane
            }
        }

        //----------
        protected void lvPlanPracy_Unload(object sender, EventArgs e)
        {
            if (con != null) Base.Disconnect(con);
        }


        //-----------------------------------------------------------------
        public int Mode
        {
            get { return Tools.GetViewStateInt(ViewState[ID + "_mode"], nmoKier); }
            set { ViewState[ID + "_mode"] = value; }
            /*
            get { return FMode; }
            set { FMode = value; }
            */ 
        }

        public ListView List
        {
            get { return lvNadgodziny; }
        }

        public string KierId
        {
            get { return hidKierId.Value; }
            set { hidKierId.Value = value; }
        }

        public string ProjId
        {
            get { return hidProjId.Value; }
            set { hidProjId.Value = value; }
        }

        private string OkresId
        {
            get { return hidOkresId.Value; }
            set { hidOkresId.Value = value; }
        }

        public int OkresStatus
        {
            get { return Tools.GetViewStateInt(ViewState[ID + "_okstat"], -1); }
            set { ViewState[ID + "_okstat"] = value; }
        }

        public double StawkaNocna
        {
            get { return Tools.GetViewStateDouble(ViewState[ID + "_okstawkanocne"], -1); }
            set { ViewState[ID + "_okstawkanocne"] = value; }
        }

        public bool IsArch
        {
            get { return Tools.GetViewStateBool(ViewState[ID + "_okarch"], false); }
            set { ViewState[ID + "_okarch"] = value; }
        }

        public string ImportMiesiac
        {
            get { return Tools.GetViewStateStr(ViewState[ID + "_impmies"]); }
            set { ViewState[ID + "_impmies"] = value; }
        }

        public string DateFrom
        {
            get { return hidFrom.Value; }
            set { hidFrom.Value = value; }
        }

        public string DateTo
        {
            get { return hidTo.Value; }
            set 
            { 
                hidTo.Value = value; //yyyy-mm-dd
                DataRow dr = null;
                string mc = null;
                if (!String.IsNullOrEmpty(value))
                {
                    mc = value.Substring(0, 8) + "01";  // początek miesiąca, zadziała bo format daty zawsze yyyy-mm-dd                
                    dr = db.getDataRow(String.Format("select top 1 Id from PodzialLudziImport where Miesiac = '{0}'", mc));
                }
                if (dr == null)
                {
                    dr = db.getDataRow("select max(Miesiac) from PodzialLudziImport");
                    if (dr != null)
                        mc = Tools.DateToStr(dr[0]);
                    else
                        mc = Tools.DateToStr(Tools.bom(DateTime.Today));  // nie ma importów
                }
                ImportMiesiac = mc;
            }
        }
        //------------
        public string UrlopyKody    // pobieram przy pierwszym użyciu
        {
            set { ViewState["ukody"] = value; }
            get 
            {
                string kody = Tools.GetStr(ViewState["ukody"]);
                if (String.IsNullOrEmpty(kody))
                {
                    kody = db.getScalar("select Parametr from Kody where Typ = 'RURLOPY'");
                    if (String.IsNullOrEmpty(kody)) kody = "510,511";
                    UrlopyKody = kody;
                }
                return kody; 
            }
        }

        public string ChoroboweKody
        {
            set { ViewState["chkody"] = value; }
            get 
            { 
                string kody = Tools.GetStr(ViewState["chkody"]);
                if (String.IsNullOrEmpty(kody))
                {
                    kody = db.getScalar("select Parametr from Kody where Typ = 'RCHOROBOWE'");
                    if (String.IsNullOrEmpty(kody)) kody = "500,525,526";
                    ChoroboweKody = kody;
                }
                return kody;
            }
        }
    }
}
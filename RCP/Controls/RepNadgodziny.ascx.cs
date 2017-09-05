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

namespace HRRcp.Controls
{
    public partial class RepNadgodziny : System.Web.UI.UserControl
    {
        SqlConnection con = null;
        PlanPracy.TimeSumator sumator;
        TimeSumator3 total;
        int ilDniPrac;      // w okresie
        int ilGodzPrac;
        int ilDniPracMies;  // w miesiącu wynikajcym z końca okresu
        int ilGodzPracMies;

        double p50 = 1.5;
        double p100 = 2.0;
        double _stawkaNoc = 0;
        bool noData = false;            // brak danych '-' w zestawieniu - okres.From > DateTime.Today; ustawiane w DataBinding
        bool incompleteData = false;    // niekompletne dane (nie wszystko zatwierdzone !!!) - do wyświetlenia warninga

        //1. okres zamkniety - dane <<< okres.Status = stClosed lub okres.From <= Today; !allData ? warning <<< bo może nie brać udziału !!!
        //2. okres - otwarty - dane <<< okres.From <= Today; !allData ? warning <<< muszą być jakieś dane!
        //3. okres otwarty - pusto <<< okres.From > Today; brak danych

        public const int nmoKierStawki = 0;    // nadgodziny wg kierownika - stawki
        public const int nmoProjStawki = 1;    // nadgodziny wg projektów - stawki
        public const int nmoKier = 3;          // nadgodziny wg kierownika - bez stawek
        //int FMode = 0;

        Ustawienia settings;

        protected void Page_Load(object sender, EventArgs e)
        {
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

        public void Prepare(string projId, string kierId, string dFrom, string dTo, string okresId, int okresStatus, bool okresArch, double okresStNoc, int mode)
        {
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            ProjId = projId;
            KierId = kierId;
            OkresId = okresId;
            OkresStatus = okresStatus;
            Mode = mode;
            IsArch = okresArch;
            StawkaNocna = okresStNoc;

            if (Mode == nmoKier || Mode == nmoKierStawki)
                InitPath(kierId);

            SqlDataSource1.SelectCommand = GetSql(IsArch);
            /*
            if (param != null)
            {
                //lvNadgodziny.DataBind();
            }
            */ 
        }

        public void Prepare(string dFrom, string dTo, string okresId, int okresStatus, bool okresArch, double okresStNoc)
        {
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            OkresId = okresId;
            OkresStatus = okresStatus;
            IsArch = okresArch;
            StawkaNocna = okresStNoc;
            SqlDataSource1.SelectCommand = GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void DataBindKier(string kierId)   // daty musza byc ustawione !!!
        {
            KierId = kierId;
            ProjId = null;
            
            if (Mode == nmoKier || Mode == nmoKierStawki)
                InitPath(kierId);

            SqlDataSource1.SelectCommand = GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void DataBindProj(string projId)   // daty musza byc ustawione !!!
        {
            ProjId = projId;
            KierId = null;

            InitPath(null);  // poziom główny
            
            SqlDataSource1.SelectCommand = GetSql(IsArch);
            lvNadgodziny.DataBind();
        }

        public void DataBindZoom(string kierId)   // daty musza byc ustawione !!!
        {
            KierId = kierId;
            SqlDataSource1.SelectCommand = GetSql(IsArch);
            lvNadgodziny.DataBind();
        }
        /*
        public string GetSql(bool arch)
        {
            const string sql0 = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.Stawka, P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM Pracownicy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";

            const string sql0_okres = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.Stawka, P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "P.RcpAlgorytm as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM PracownicyOkresy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";

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

            const string order0 = " ORDER BY P.Kierownik desc, NazwiskoImie";
            const string order1 = " ORDER BY NazwiskoImie";

            switch (Mode)
            {
                default:
                case nmoKierStawki:
                    if (String.IsNullOrEmpty(KierId)) return null;
                    else if (KierId == "-100")//"ALL")                                                                                           //return sql0 + order0;
                        if (arch) return sql0_okres + " where P.IdOkresu = " + OkresId + " and P.Status >= 0" + order0;
                        else      return sql0 + " where P.Status >= 0" + order0;
                    else                                                                                                                //return sql0 + " WHERE P.IdKierownika = " + KierId + order0;  // kierID = 0 -> poziom roota
                        if (arch) return sql0_okres + " where P.IdOkresu = " + OkresId + " and P.Status >= 0 and P.IdKierownika = " + KierId + order0;  // kierID = 0 -> poziom roota
                        else      return sql0 + " where P.Status >= 0 and P.IdKierownika = " + KierId + order0;              // kierID = 0 -> poziom roota
                case nmoProjStawki:
                    if (String.IsNullOrEmpty(ProjId)) return null;                                                                      // brak danych ??? 
                    else                                                                                                                //return sql0 + " WHERE P.IdDzialu = " + ProjId + order0;
                    {
                        string whereKier = String.IsNullOrEmpty(KierId) ? null : " and P.IdKierownika = " + KierId;
                        if (arch) return sql0_okres + " where P.IdOkresu = " + OkresId + " and P.Status >= 0 and P.IdDzialu = " + ProjId + whereKier + order0;
                        else return sql0 + " where P.Status >= 0 and P.IdDzialu = " + ProjId + whereKier + order0;
                    }
                case nmoKier:
                    if (String.IsNullOrEmpty(KierId)) return null;
                    else if (KierId == "-100")//"ALL")                                                                                           //return sql2 + order0;
                        if (arch) return sql2_okres + " where P.IdOkresu = " + OkresId + " and P.Status >= 0" + order0;
                        else      return sql2 + " where P.Status >= 0" + order0;
                    else                                                                                                                //return sql2 + " WHERE P.IdKierownika = " + KierId + order0;
                        if (arch) return sql2_okres + " where P.IdOkresu = " + OkresId + " and P.Status >= 0 and P.IdKierownika = " + KierId + order0;
                        else      return sql2 + " where P.Status >= 0 and P.IdKierownika = " + KierId + order0;
            }
        }
        */

        public string GetSql(bool arch)
        {
            return GetSql(arch, OkresId, Mode, KierId, ProjId);
        }

        public static string GetSql(bool arch, string okresId, int mode, string kierId, string projId)
        {
            const string sql0 = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.Stawka, P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM Pracownicy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";

            const string sql0_okres = "SELECT P.Id, " +
                                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie, " +
                                    "P.KadryId, " +
                                    "P.Stawka, P.EtatL, P.EtatM, " +
                                    "P.RcpId, " +
                                    "P.Kierownik, " +
                                    "P.RcpAlgorytm as Algorytm, " +
                                    "D.Nazwa as Dzial " +
                               "FROM PracownicyOkresy P " +
                               "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu";

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

            const string order0 = " ORDER BY P.Kierownik desc, NazwiskoImie";
            const string order1 = " ORDER BY NazwiskoImie";

            switch (mode)
            {
                default:
                case nmoKierStawki:
                    if (String.IsNullOrEmpty(kierId)) return null;
                    else if (kierId == "-100")//"ALL")                                                                                                  //return sql0 + order0;
                        if (arch) return sql0_okres + " where P.IdOkresu = " + okresId + " and P.Status >= 0" + order0;
                        else return sql0 + " where P.Status >= 0" + order0;
                    else                                                                                                                                //return sql0 + " WHERE P.IdKierownika = " + KierId + order0;  // kierID = 0 -> poziom roota
                        if (arch) return sql0_okres + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdKierownika = " + kierId + order0;  // kierID = 0 -> poziom roota
                        else return sql0 + " where P.Status >= 0 and P.IdKierownika = " + kierId + order0;                                              // kierID = 0 -> poziom roota
                case nmoProjStawki:
                    if (String.IsNullOrEmpty(projId)) return null;                                                                      // brak danych ??? 
                    else                                                                                                                //return sql0 + " WHERE P.IdDzialu = " + ProjId + order0;
                    {
                        string whereKier = String.IsNullOrEmpty(kierId) ? null : " and P.IdKierownika = " + kierId;
                        if (arch) return sql0_okres + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdDzialu = " + projId + whereKier + order0;
                        else return sql0 + " where P.Status >= 0 and P.IdDzialu = " + projId + whereKier + order0;
                    }
                case nmoKier:
                    if (String.IsNullOrEmpty(kierId)) return null;
                    else if (kierId == "-100")//"ALL")                                                                                           //return sql2 + order0;
                        if (arch) return sql2_okres + " where P.IdOkresu = " + okresId + " and P.Status >= 0" + order0;
                        else return sql2 + " where P.Status >= 0" + order0;
                    else                                                                                                                //return sql2 + " WHERE P.IdKierownika = " + KierId + order0;
                        if (arch) return sql2_okres + " where P.IdOkresu = " + okresId + " and P.Status >= 0 and P.IdKierownika = " + kierId + order0;
                        else return sql2 + " where P.Status >= 0 and P.IdKierownika = " + kierId + order0;
            }
        }
        //----------------------------------------------
        private void PrepareViewHeader(Control cnt)
        {
            switch (Mode)
            {
                default:
                case nmoKierStawki:
                    break;
                case nmoProjStawki:
                    Tools.SetControlVisible(cnt, "thDzial", false);
                    HtmlTableCell td = (HtmlTableCell)cnt.FindControl("tdSumy");
                    if (td != null) td.ColSpan = 2;
                    break;
                case nmoKier:
                    td = (HtmlTableCell)cnt.FindControl("tdSumy");
                    if (td != null) td.ColSpan = 2;
                    Tools.SetControlVisible(cnt, "thDzial", false);
                    Tools.SetControlVisible(cnt, "th7", false);
                    Tools.SetControlVisible(cnt, "th8", false);
                    Tools.SetControlVisible(cnt, "th9", false);
                    Tools.SetControlVisible(cnt, "th10", false);
                    Tools.SetControlVisible(cnt, "th11", false);
                    Tools.SetControlVisible(cnt, "th12", false);
                    break;
            }
        }

        private void PrepareView(Control cnt)
        {
            switch (Mode)
            {
                default:
                case nmoKierStawki:
                    break;
                case nmoProjStawki:
                    Tools.SetControlVisible(cnt, "tdDzial", false);
                    break;
                case nmoKier:
                    Tools.SetControlVisible(cnt, "tdDzial", false);
                    Tools.SetControlVisible(cnt, "td7", false);
                    Tools.SetControlVisible(cnt, "td8", false);
                    Tools.SetControlVisible(cnt, "td9", false);
                    Tools.SetControlVisible(cnt, "td10", false);
                    Tools.SetControlVisible(cnt, "td11", false);
                    Tools.SetControlVisible(cnt, "td12", false);
                    break;
            }
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

                    LinkButton lbt = (LinkButton)e.Item.FindControl("lbtPracownik");
                    if (lbt != null && !String.IsNullOrEmpty(par))
                    {
                        string name = lbt.Text;
                        cntPath.AddPath(name, par, null);
                        DataBindZoom(par);
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
            DataBindZoom(cntPath.SelParam);
        }
        //-------------
        protected void lvPlanPracy_LayoutCreated(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case nmoKierStawki:
                case nmoProjStawki:
                    HtmlTable tb = (HtmlTable)lvNadgodziny.FindControl("lvOuterTable");
                    if (tb != null)
                        Tools.AddClass(tb, "narrow");
                    break;
                default:
                case nmoKier:
                    break;
            }
        }

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
        }

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
                Tools.SetText(lvNadgodziny, "lbSumZmianyPlan", Worktime.SecToHourStr(total.Sum[1]));    // z PP ale z korektami
                Tools.SetText(lvNadgodziny, "lbSumSumaryczny", Worktime.SecToHourStr(total.Sum[2]));    // zmiany + nadgodziny
                //Tools.SetText(lvNadgodziny, "lbSumZmiany", Worktime.SecToHourStr(total.Sum[3]));        // przepracowany na zmianach
                Tools.SetText(lvNadgodziny, "lbSumNieprzepracowany", Worktime.SecToHourStr(total.Sum[3]));        // przepracowany na zmianach
                object s6 = total.Sum[6];
                int orest = s6 == null ? 0 : (int)s6;                                                          // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane 
                Tools.SetText(lvNadgodziny, "lbSumNadg50", Worktime.SecToHourStr(total.Sum[4]) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null));
                Tools.SetText(lvNadgodziny, "lbSumNadg100", Worktime.SecToHourStr(total.Sum[5]));
                Tools.SetText(lvNadgodziny, "lbSumNocny", Worktime.SecToHourStr(total.Sum[7]));
                Tools.SetText(lvNadgodziny, "lbSumNiedzieleSwieta", Worktime.SecToHourStr(total.Sum[8]));

                if (Mode == nmoKier)
                {
                    tbStawkaNocna.Visible = false;
                }
                else
                {
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
                    tbStawkaNocna.Visible = true;
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

        protected void lvPlanPracy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PrepareView(e.Item);

                if (con == null)
                {
                    con = Base.Connect();
                    sumator = new PlanPracy.TimeSumator();
                    total = new TimeSumator3(12);
                    settings = Ustawienia.CreateOrGetSession();
                    ilDniPrac = Worktime._GetIloscDniPrac(con, DateFrom, DateTo);  // stary raport
                    ilDniPracMies = Worktime._GetIloscDniPracMies(con, DateTo);
                }
                else sumator.Reset();

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                string pracId = drv["Id"].ToString();
                //----- nie licze pracownika -----
                int alg = Base.getInt(drv["Algorytm"], -1);
                bool pracOff = alg == 0;
                int etatL = Base.getInt(drv["EtatL"], -1);
                int etatM = Base.getInt(drv["EtatM"], -1);
                int etat = etatL > 0 && etatM > 0 ? 8 * etatL / etatM : -1;   // il godzin pracy w dniu lub -1
                //----- kierownik / pracownik ------
                bool kier = Base.getBool(drv["Kierownik"], false);
                Tools.SetControlVisible(e.Item, "PracownikLabel", !kier);
                Tools.SetControlVisible(e.Item, "lbtPracownik", kier);
                //----- czas pracy -----
                bool _allAccepted = true;        // wszystkie dni są poakceptowane >>> 
                int pptime, wtime, _ztime2, otime50, otime100, orest, ntime2, htime;  // htime - praca w niedziele i święta, sumator sam to liczy!!!
                if (!pracOff)
                {
                    DataSet ds = Base.getDataSet(con, String.Format(
                             // 0      1        2                 3               4       5      6               7                  8         9 
                        "select P.Data,P.CzasZm,P.NadgodzinyDzien,P.NadgodzinyNoc,P.Nocne,A.Kod, AK.GodzinPracy, Z.Id as TypZmiany, K.Rodzaj, P.Akceptacja " + 
                        "from PlanPracy P " +
                            "left outer join Zmiany Z on Z.Id = ISNULL(P.IdZmianyKorekta, P.IdZmiany) " +
                            "left outer join Kalendarz K on K.Data = P.Data " +
                            
                            "left outer join Absencja A on A.IdPracownika = P.IdPracownika and P.Data between A.DataOd and A.DataDo " +
                            "left outer join AbsencjaKody AK on AK.Kod = A.Kod " +

                        "where P.Data between '{0}' and '{1}' and P.IdPracownika={2}"
                        //+ " order by P.Data"
                        
                        ,
                        DateFrom, DateTo, pracId
                        ));


                    int cnt = ds.Tables[0].Rows.Count;
                    bool absPraca = false;
                    if (cnt == 0) _allAccepted = false;  // brak danych 
                    else
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            DataRow dr = Base.getDataRow(ds, i);
                            DateTime date = Base.getDateTime(dr, 0, DateTime.MinValue);
                            bool acc = Base.getBool(dr, 9, false);

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
                                ztime = 0;
                                otimeD = 0;
                                otimeN = 0;
                                ntime = 0;
                                _allAccepted = false;
                            }

                            string abs = Base.getValue(dr, 5);
                            int absGodzPracy = Base.getInt(dr, 6, 0);
                            string zmiana = Base.getValue(dr, 7);
                            int _wolne = Base.getInt(dr, 8, -1);     // 0 sobota, 1 niedziela, 2 święto
                            //----- absencje jako czas pracy -----
                            bool isZmiana = !String.IsNullOrEmpty(zmiana);
                            if (!String.IsNullOrEmpty(abs))
                            {
                                if (ztime + otimeD + otimeN > 0) absPraca = true;   // także wtedy kiedy tylko nadgodziny !
                                if (absGodzPracy > 0 && _wolne == -1)   // liczę jako czas pracy jeśli nie było pracy i dzień nie jest wolny (bez sobot, niedziel i świąt)
                                    //if (absGodzPracy > 0 && isZmiana)     // liczę jako czas pracy jeśli nie było pracy i zaplanowana/skorygowana zmiana <<< po staremu bo KP nie pozwala na wprowadzanie i sie rozjedzie
                                    if (ztime == 0)                     // praca ma priorytet
                                        ztime = ((etat > 0 ? etat : 8) * 3600 * absGodzPracy) / 8;
                            }
                            //----- sumy -----
                            int t50, t100;  // w danym dniu 
                            int d50, d100;
                            bool zeroZm;
                            sumator._SumTimes2(date, zmiana, _wolne >= 1, ztime, otimeD, otimeN, ntime, out t50, out t100,             false, 0, out d50, out d100, out zeroZm);   //0-sobota 1-niedziela 2-swieto
                        }
                        if (cnt < ilDniPrac)
                            _allAccepted = false;    // brak planu, a np. tylko 1 dzień zaakceptowany
                    }
                    //----- sumy -----
                    //if (allAccepted || !noData)     // jak wszystko poakceptowane lub kiedy są dane <<< wystarczyłby sam warunek że są (mogą być) dane 
                    if (!noData)     
                    {
                        _GetSumy(sumator, settings.ZaokrSum, settings.ZaokrSumType,
                                out pptime, out wtime, out _ztime2, out otime50, out otime100, out orest, out ntime2, out htime);
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
                    }
                    //----- nominalny i nieprzepracowany -----
                    if (etat > 0)
                    {
                        ilGodzPrac = ilDniPrac * etat;
                        ilGodzPracMies = ilDniPracMies * etat;
                        Tools.SetText(e.Item, "lbNominalny", ilGodzPrac.ToString());         // (dni w okresie! - wolne) * 8 ~ z PP bez korekt ? moze być różne bo za święto w niedziele trzeba dac wolne... - najlepiej było by wziąć z KP!, ale tam jest co do miesiąca więc źle ... 
                    }
                    else
                    {
                        ilGodzPrac = ilDniPrac * 8;
                        ilGodzPracMies = ilDniPracMies * 8;
                        Tools.SetText(e.Item, "lbNominalny", ilGodzPrac.ToString() + "<br />błąd etatu");
                    }
                    int nptime = 0;
                    //----- -----
                    //if (allAccepted || !noData)
                    if (!noData)
                    {
                        Tools.SetText(e.Item, "lbZmianyPlan", Worktime.SecToHourStr(pptime));           // z PP ale z korektami
                        Tools.SetText(e.Item, "lbSumaryczny", Worktime.SecToHourStr(wtime) + (absPraca ? "<br />praca i<br />absencje!" : null));    // zmiany + nadgodziny
                        //Tools.SetText(e.Item, "lbZmiany", Worktime.SecToHourStr(ztime2));               // przepracowany na zmianach
                        nptime = ilGodzPrac * 3600 - _ztime2;
                        if (nptime < 0) nptime = 0;
                        Tools.SetText(e.Item, "lbNieprzepracowany", Worktime.SecToHourStr(nptime));       // nieprzepracowany 
                        Tools.SetText(e.Item, "lbNadg50", Worktime.SecToHourStr(otime50) + (orest > 0 ? "<br />? " + Worktime.SecToHourStr(orest) : null));  // powinno być 0 ! jezeli zmiany sa poprawnie skonfigurowane
                        Tools.SetText(e.Item, "lbNadg100", Worktime.SecToHourStr(otime100));
                        Tools.SetText(e.Item, "lbNocny", Worktime.SecToHourStr(ntime2));                // przepracowany w nocy
                        Tools.SetText(e.Item, "lbNiedzieleSwieta", Worktime.SecToHourStr(htime));       // przepracowany w niedziele i święta
                    }
                    else
                    {
                        Tools.SetText(e.Item, "lbZmianyPlan", "-");
                        Tools.SetText(e.Item, "lbSumaryczny", "-");
                        //Tools.SetText(e.Item, "lbZmiany", "-");
                        Tools.SetText(e.Item, "lbNieprzepracowany", "-");
                        Tools.SetText(e.Item, "lbNadg50", "-");
                        Tools.SetText(e.Item, "lbNadg100", "-");
                        Tools.SetText(e.Item, "lbNocny", "-");
                        Tools.SetText(e.Item, "lbNiedzieleSwieta", "-");
                    }
                    //----- sumy total -----
                    double w50 = 0;
                    double w100 = 0;
                    double wNoc = 0;
                    if (Mode != nmoKier)
                    {
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
                        {
                            Tools.SetText(e.Item, "lbStawka", "brak danych");
                            Tools.SetText(e.Item, "lbStawkaGodz", "-");
                            Tools.SetText(e.Item, "lbWynagr50", "-");
                            Tools.SetText(e.Item, "lbWynagr100", "-");
                            Tools.SetText(e.Item, "lbWynagrNadgSuma", "-");
                            Tools.SetText(e.Item, "lbWynagrNoc", "-");
                        }
                        else
                        {
                            Tools.SetText(e.Item, "lbStawka", _stawka.ToString("0.00"));
                            //if (allAccepted || !noData)
                            if (!noData)
                            {
                                Tools.SetText(e.Item, "lbStawkaGodz", stawkah.ToString("0.0000"));
                                Tools.SetText(e.Item, "lbWynagr50", w50.ToString("0.00"));
                                Tools.SetText(e.Item, "lbWynagr100", w100.ToString("0.00"));
                                Tools.SetText(e.Item, "lbWynagrNadgSuma", (w50 + w100).ToString("0.00"));
                                if (_stawkaNoc > 0)
                                    Tools.SetText(e.Item, "lbWynagrNoc", wNoc.ToString("0.00"));
                                else
                                    Tools.SetText(e.Item, "lbWynagrNoc", "-");
                            }
                            else
                            {
                                Tools.SetText(e.Item, "lbStawkaGodz", "-");
                                Tools.SetText(e.Item, "lbWynagr50", "-");
                                Tools.SetText(e.Item, "lbWynagr100", "-");
                                Tools.SetText(e.Item, "lbWynagrNadgSuma", "-");
                                Tools.SetText(e.Item, "lbWynagrNoc", "-");
                            }
                        }
                    }
                    total.SumTimes(ilGodzPrac, pptime, wtime,
                                    //ztime2, 
                                    nptime,
                                    otime50, otime100, orest, ntime2, htime, w50, w100, wNoc);
                }
                else //Pracownik.Status = stPomin
                {
                    Tools.SetText(e.Item, "lbNominalny", "-"); 
                    Tools.SetText(e.Item, "lbZmianyPlan", "-"); 
                    Tools.SetText(e.Item, "lbSumaryczny", "-");
                    //Tools.SetText(e.Item, "lbZmiany", "-");
                    Tools.SetText(e.Item, "lbNieprzepracowany", "-");
                    Tools.SetText(e.Item, "lbNadg50", "-");
                    Tools.SetText(e.Item, "lbNadg100", "-");
                    Tools.SetText(e.Item, "lbNocny", "-");
                    Tools.SetText(e.Item, "lbNiedzieleSwieta", "-");
                    if (Mode != nmoKier)
                    {
                        Tools.SetText(e.Item, "lbStawka", "-");
                        Tools.SetText(e.Item, "lbStawkaGodz", "-");
                        Tools.SetText(e.Item, "lbWynagr50", "-");
                        Tools.SetText(e.Item, "lbWynagr100", "-");
                        Tools.SetText(e.Item, "lbWynagrNadgSuma", "-");
                        Tools.SetText(e.Item, "lbWynagrNoc", "-");
                    }
                }

                if (!_allAccepted) incompleteData = true;                               // dane nie sa kompletne!!!
                Tools.SetControlVisible(e.Item, "lbWarning", !noData && !_allAccepted);  // tylko pokazuję jak mogą byc dane
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
        
        public string DateFrom
        {
            get { return hidFrom.Value; }
            set { hidFrom.Value = value; }
        }

        public string DateTo
        {
            get { return hidTo.Value; }
            set { hidTo.Value = value; }
        }
        //------------

    }
}
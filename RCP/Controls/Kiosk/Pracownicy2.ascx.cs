using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using HRRcp.App_Code;
using AjaxControlToolkit;

/*
            switch (FMode)
            {
                default:
                case moKier:
                    break;
                case moSearch:c
                    break;
                case moNoLine:
                    break;
                case moAdmin:
                    break;
            }
*/

namespace HRRcp.Controls.Kiosk
{
    public partial class Pracownicy2 : System.Web.UI.UserControl
    {
        public event EventHandler Oddeleguj;
        public event EventHandler SelectedChanged;
        public string delPracId;
        public bool delToMe;

        public int FMode = 0;

        const int moKier        = 0;    // dla kierownika, wszyscy z macierzystej linii (na miejscu i oddelegowani) + przydelegowani, możliwość oddelegowania stąd bez potwierdzenia przez kierownika
        const int moSearch      = 1;    // wyszukiwanie wg czynności 
        const int moNoLine      = 2;    // pracownicy nie przypisani do linii
        const int moOcena       = 3;    // ocena pracowników, którzy tego wymagają: skończyło się zastępstwo >= 1mc, nie ocenieni po 1mc
        const int moAdmin       = 4;    // wszyscy pracownicy
        const int moScal        = 5;    // lista pracownikow do scalenia
        const int moPrzypiszRCP = 6;    // przypisywanie numerów RCP

        const string fnOddeleguj    = "1";
        const string fnPrzypisz     = "2";

        const string sesSortId = "_sort";
        const int maxSortCol = 20;
        private int FDefSortColumn = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                KierId = App.Master.user.Id;
                Data = Tools.DateToStr(DateTime.Today);  // inicjacja na dziś
                App.SelectedPracId = null;
            }
            if (FMode == moPrzypiszRCP)
            {
                if (!IsPostBack)
                {
                    //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
                    btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID);
                    tbSearch.Focus();

                    PrepareSearch();
                }
                //__doPostBack('<%= tbSearch.ClientID %>', '');
            }
        }

        private void PrepareSearch()
        {
            /* nie działa na IE8
            Tools.ExecOnStart2("searchtrigger", String.Format(@"
                        $('#{0}').on('input', function(){{
                            alert(0);
                            doClick('{1}');
                        }});",
                tbSearch.ClientID, btSearch.ClientID));
             */
            /*
            string ev = String.Format("onChanging(this, '{0}');", btSearch.ClientID);
            //tbSearch.Attributes.Add("onkeypress", ev);
            tbSearch.Attributes.Add("onpaste", ev);
            tbSearch.Attributes.Add("onkeyup", ev);
            tbSearch.Attributes.Add("onblur", ev);
             */

            Tools.ExecOnStart2("searchtrigger", String.Format("startSearch2('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
        }
        
        public int Prepare()
        {
            Data = Tools.DateToStr(DateTime.Today);
            SetSelectSql();
            lvPracownicy.DataBind();
            return lvPracownicy.Items.Count;
        }


        /*zzz
        public int Prepare(string liniaId, string linie, bool oneLine, string kierId, string zakres)  // ustawiam aktywną linię
        {
            LiniaId = liniaId == LiniaSelect.ALL ? null : liniaId;
            Linie = linie;
            if (String.IsNullOrEmpty(linie) && String.IsNullOrEmpty(LiniaId))
                Linie = "-1";  // <<<, plomba !!!
            KierId = kierId == LiniaSelect.ALL ? null : kierId;
            Zakres = zakres;
            Data = Tools.DateToStr(DateTime.Today);
            SetSelectSql();
            lvPracownicy.DataBind();
            return lvPracownicy.Items.Count;
        }
        */
          
        /*
        public int Prepare(string liniaId, string kierId)
        {
            if (liniaId.StartsWith("|"))
            {
                LiniaId = null;
                Linie = liniaId.Substring(1);
            }
            else
            {
                LiniaId = liniaId; //App.KierLiniaId; 
                Linie = null;
            }
            KierId = kierId;
            Data = Tools.DateToStr(DateTime.Today);
            SetSelectSql();
            lvPracownicy.DataBind();
            return lvPracownicy.Items.Count;
        }
        */

        public int PrepareSearch(string czynnosci)  //comma separated
        {
            FMode = moSearch;                       // i tak powinno być ustawione na formie
            Params = czynnosci;
            Data = Tools.DateToStr(DateTime.Today);
            lvPracownicy.DataBind();
            return lvPracownicy.Items.Count;
        }

        /*
            if (!String.IsNullOrEmpty(czynnosci))
                sql = String.Format(select1 + 
                    "left outer join Oceny N on N.ID_Pracownicy = P.Id_Pracownicy " +
                    "where N.Id_Zadania in ({1}) " + order1, 
                    data, where);
            else
                sql = String.Format(select1 + " where P.Id_Pracownicy = -1", data);
            SqlDataSource1.SelectCommand = sql;
            SqlDataSource1.DataBind();
            lvPracownicy.DataBind();
             */

        //----------------------------------------------------------------
        /*
            string select = "select " +
                    "P.Id_Pracownicy," +
                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie," +
                    "P.Nazwisko," +
                    "P.Imie," +
                    "P.Imie2," +
                    "P.Nr_Ewid," +

                    "P.Id_Gr_Zatr," +
                    "G.Rodzaj_Umowy," +

                    "P.Id_Stanowiska," +
                    "S.Nazwa_Stan," +

                    "M.Id_Parent as IdStrumieniaM," +
                    "MP.Symb_Jedn as SymbolStrumieniaM," +
                    "MP.Nazwa_Jedn as NazwaStrumieniaM," +
                    "MP.ID_Upr_Przel as IdUprPrzelStrumieniaM," +
                    "MP.Id_Parent as IdParentaStrumieniaM," +

                    "P.Id_Str_OrgM as IdLiniiM," +
                    "M.Symb_Jedn as SymbolLiniiM," +
                    "M.Nazwa_Jedn as NazwaLiniiM," +
                    "M.ID_Upr_Przel as IdUprPrzelLiniiM," +
                    "MK.Nazwisko + ' ' + MK.Imie as KierownikM, " +

                    "case when O.IdStruktury is not null then A.Id_Parent else M.Id_Parent end as IdStrumieniaA," +
                    
                    


                    "ISNULL(A.Id_Parent, M.Id_Parent) as IdStrumieniaA," +
                    "ISNULL(AP.Symb_Jedn, MP.Symb_Jedn) as SymbolStrumieniaA," +
                    "ISNULL(AP.Nazwa_Jedn, MP.Nazwa_Jedn) as NazwaStrumieniaA," +
                    "ISNULL(AP.ID_Upr_Przel, MP.ID_Upr_Przel) as IdUprPrzelStrumieniaA," +
                    "ISNULL(AP.Id_Parent, MP.Id_Parent) as IdParentaStrumieniaA," +
                    
                    "AK.Nazwisko + ' ' + MK.Imie as KierownikM, " +

                    "ISNULL(O.IdStruktury, P.Id_Str_OrgM) as IdLiniiA," +
                    "ISNULL(A.Symb_Jedn, M.Symb_Jedn) as SymbolLiniiA," +
                    "ISNULL(A.Nazwa_Jedn, MP.Nazwa_Jedn) as NazwaLiniiA," +
                    "ISNULL(A.ID_Upr_Przel, M.ID_Upr_Przel) as IdUprPrzelLiniiA," +
                    "O.Od, O.Do," +
                    
                    "0 as Ocena, " +

                    "P.Id_Status," +
                    "T.Status " +
                "from Pracownicy P " +
                    "left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr " +
                    "left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska " +
                    "left outer join StrOrg M on M.Id_Str_Org = P.Id_Str_OrgM " +             // Linia
                    "left outer join StrOrg MP on MP.Id_Str_Org = M.Id_Parent " +             // Strumień
                    "left outer join Przelozeni MK on MK.Id_Przelozeni = P.IdKierownika " +
                    "left outer join Oddelegowania O on O.IdPracownika = P.Id_Pracownicy and @Data between O.Od and O.Do and O.Status = " + Wnioski.stZaakceptowany.ToString() + " " +
                    "left outer join StrOrg A on A.Id_Str_Org = O.IdStruktury " +
                    "left outer join StrOrg AP on AP.Id_Str_Org = A.Id_Parent " +
                    "left outer join Przelozeni AK on AK.Id_Przelozeni = O.IdKierownika " +
                    "left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status " +
                "{0} " +
                "order by NazwiskoImie";
         */



        public const string prac_select0 =
                "select " +
                    "P.Id as Id_Pracownicy," +
                    "P.Nazwisko + ' ' + P.Imie as NazwiskoImie," +
                    "P.Nazwisko," +
                    "P.Imie," +
                    "null as Imie2," +
                    "P.KadryId as Nr_Ewid," +

                    "replace(P.KadryId, '-', ' ') as nrew," +   // do sortowań
                    "P.NrKarty,P.Login,P.marker," +

                    "null as Id_Gr_Zatr," +
                    "null as Rodzaj_Umowy," +
                    "P.DataZatr, P.DataUmDo, P.DataZwol," +

                    "P.Id_Stanowiska," +
                    "S.Nazwa_Stan," +

                    "P.Id_Str_OrgM, P.IdKierownika," +
                    "P.Id_Status," +
                    "P.APT, P.Status,";


        public const string prac_from =
                "from Pracownicy P " +
                    //"left outer join KDR_RPP..wnPracownikAttributes RPP on RPP.Pracownik_ID = P.Id_Pracownicy " +
                    "left outer join GrZatr G on G.Id_Gr_Zatr = P.Id_Gr_Zatr " +
                    "left outer join Stanowiska S on S.Id_Stanowiska = P.Id_Stanowiska ";

        public const string _prac_select = @"
select 
    P.Id as Id_Pracownicy,
    P.Nazwisko + ' ' + P.Imie as NazwiskoImie,
    P.Nazwisko,
    P.Imie,
    null as Imie2,
    P.KadryId as Nr_Ewid,

    replace(P.KadryId, '-', ' ') as nrew,
    P.NrKarty1 as NrKarty, P.Login, 1 as marker,

    null as Id_Gr_Zatr,
    null as Rodzaj_Umowy,
    P.DataZatr, null as DataUmDo, P.DataZwol,

    P.IdStanowiska as Id_Stanowiska,
    null as Nazwa_Stan,

    null as Id_Str_OrgM, 
    P.IdKierownika,
    
    P.Status as Id_Status,
    convert(bit,0) as APT, 
    P.Status,


    null as SymbolStrumieniaM,
    null as NazwaStrumieniaM,
    null as SymbolLiniiM, null as NazwaLiniiM,
    null as KierownikM,null as KierownikA,
    null as IdLiniiM, null as IdLiniiA,
    null as SymbolStrumieniaA, null as NazwaStrumieniaA,
    null as SymbolLiniiA, null as NazwaLiniiA,
    null as Od, null as Do, null as Ocena, null as Absencja 
from Pracownicy P {0} {1}
            ";

//select *, 0 as APT from Pracownicy {0} {1}
        

        public const string join_scal = "join Pracownicy PS on PS.Nazwisko = P.Nazwisko and PS.Imie = P.Imie and PS.Id_Pracownicy <> P.Id_Pracownicy";

        public const string prac_select_noline = prac_select0 +
                "MP.Symb_Jedn as SymbolStrumieniaM," +
                "MP.Nazwa_Jedn as NazwaStrumieniaM," +
                "null as SymbolLiniiM, null as NazwaLiniiM," +
                "null as KierownikM,null as KierownikA," +
                "null as IdLiniiM, null as IdLiniiA," +
                "null as SymbolStrumieniaA, null as NazwaStrumieniaA," +
                "null as SymbolLiniiA, null as NazwaLiniiA," +
                "null as Od, null as Do, null as Ocena, null as Absencja " +
            prac_from +
                "left outer join StrOrg MP on MP.Id_Str_Org = P.IdStrumienia " +             // Strumień
                "left outer join StatusPrac T on T.Id_Status_Prac = P.Id_Status " +
            "{0}";            

        //public const string prac_where1 = "(P.Id_Str_OrgM = @IdLinii or D.IdStruktury = @IdLinii) and (P.IdKierownika = @IdKierownika or P.IdKierownika is null)";
        //public const string prac_where1 = "(P.Id_Str_OrgM = @IdLinii or D.IdStruktury = @IdLinii) ";
        public const string prac_where1 = "(P.Id_Str_OrgM = @IdLinii or D.IdStruktury = @IdLinii) and (P.IdKierownika = @IdKierownika or P.IdKierownika is null or " +
                                                    "(select count(*) from StrukturaPrzelozeni " +
                                                    "where IdPrzelozonego = @IdKierownika and IdStruktury <> @IdLinii) > 0)"; //istnieje choć jeden element spoza listy - domniemanie ze wyzej - wiec przelozony wszystkich - ale co jesli jest liderem na linii w innym strumieniu ??????? <<< uprawnienie ???

        public const string prac_order = " order by NazwiskoImie";


        
        
        //-------------------------------------------
        
        private void SetSelectSql()
        {
            switch (FMode)
            {
                    /*
                default:
                case moKier:
                    string where;
                    //if (!String.IsNullOrEmpty(Linie))

                    //if (String.IsNullOrEmpty(LiniaId))
                    //    //where = String.Format("(P.Id_Str_OrgM in ({0}) or D.IdStruktury in ({0})) and (P.IdKierownika = @IdKierownika or P.IdKierownika is null)", 
                    //    //where = String.Format("(P.Id_Str_OrgM in ({0}) or D.IdStruktury in ({0})) ",
                    //    where = String.Format("(P.Id_Str_OrgM in ({0}) or D.IdStruktury in ({0})) and (P.IdKierownika = @IdKierownika or P.IdKierownika is null or " + 
                    //                                "(select count(*) from StrukturaPrzelozeni " +
                    //                                "where IdPrzelozonego = @IdKierownika and IdStruktury not in ({0})) > 0)", //istnieje choć jeden element spoza listy - domniemanie ze wyzej - wiec przelozony wszystkich - ale co jesli jest liderem na linii w innym strumieniu ??????? <<< uprawnienie ???
                    //                          Linie);           // zmienić to później na parametr przekazywany z SelectLinie !!!!
                    //else
                    //    where = prac_where1;

                    where = App.Matryca_GetPracWhere(KierId, LiniaId, Linie, Zakres, false);

                    SqlDataSource1.SelectCommand = String.Format(_prac_select, "", "where " + where + prac_order);
                    
                    LetterDataPager ldp = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
                    if (ldp != null)
                    {
                        ldp.Letter = "LEFT(P.Nazwisko,1)";
                        ldp.TbName = "Pracownicy P";
                        ldp.Join = "left outer join Oddelegowania D on D.IdPracownika = P.Id_Pracownicy and @Data between D.Od and D.Do and D.Status = " + App.Wnioski_stAcceptedStr;
                        //ldp.Where = "(P.Id_Str_OrgM = @IdLinii or D.IdStruktury = @IdLinii) and (P.IdKierownika = @IdKierownika or P.IdKierownika is null)";
                        ldp.Where = where;
                        ldp.ParName1 = "Data";
                        ldp.ParField1 = "hidData";
                        ldp.ParName2 = "IdLinii";
                        ldp.ParField2 = "hidLiniaId";
                        ldp.ParName3 = "IdKierownika";
                        ldp.ParField3 = "hidKierId";
                    }
                    break;
                case moSearch:
                        // sql = String.Format(select1 +
                        //"left outer join Oceny N on N.ID_Pracownicy = P.Id_Pracownicy " +
                        //"where N.Id_Zadania in ({1}) " + order1,
                        //data, where);
                    break;
                case moNoLine:
                    if (App.User.IsAdmin)
                        where = "P.Id_Str_OrgM is null";
                    else 
                        //if (!String.IsNullOrEmpty(Linie))
                        if (String.IsNullOrEmpty(LiniaId))
                            where = String.Format("P.Id_Str_OrgM is null and (P.IdStrumienia is null or P.IdStrumienia in (select Id_Parent from StrOrg where Id_Str_Org in ({0})))", Linie); 
                        else
                            where = "P.Id_Str_OrgM is null and (P.IdStrumienia is null or P.IdStrumienia = (select Id_Parent from StrOrg where Id_Str_Org = @IdLinii))";
                    SqlDataSource1.SelectCommand = String.Format(prac_select_noline, "where " + where + prac_order);
                    //SqlDataSource1.SelectCommand = String.Format(select, "where P.Id_Str_OrgM is null");
                    //SqlDataSource1.SelectParameters.RemoveAt(1);
                    //SqlDataSource1.SelectParameters.RemoveAt(0);
                    ldp = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
                    if (ldp != null)
                    {
                        ldp.Letter = "LEFT(P.Nazwisko,1)";
                        ldp.TbName = "Pracownicy P";
                        //ldp.Where = "P.Id_Str_OrgM is null";
                        ldp.Where = where;
                        ldp.ParName1 = "Data";
                        ldp.ParField1 = "hidData";
                    }
                    break;
                     */
                case moPrzypiszRCP:
                case moAdmin:  // RPP - ograniczam APT=0
                    //SqlDataSource1.SelectCommand = String.Format(_prac_select, "", "where APT=0 " + prac_order);
                    SqlDataSource1.SelectCommand = String.Format(_prac_select, "", prac_order);
                    LetterDataPager ldp = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
                    if (ldp != null)
                    {
                        ldp.Letter = "LEFT(P.Nazwisko,1)";
                        ldp.TbName = "Pracownicy P";
                        //ldp.Where = "APT=0";
                        ldp.ParName1 = "Data";
                        ldp.ParField1 = "hidData";
                    }
                    break;
                case moScal:
                    SqlDataSource1.SelectCommand = String.Format(_prac_select, join_scal, prac_order);
                    ldp = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
                    if (ldp != null)
                    {
                        ldp.Letter = "LEFT(P.Nazwisko,1)";
                        ldp.TbName = "Pracownicy P";
                        ldp.Where = "";
                        ldp.ParName1 = "Data";
                        ldp.ParField1 = "hidData";
                    }
                    break;
            }
        }

        public static DataSet GetKierPrzypisania()  // lista kierowników, którym się wyświetlą pracownicy do przypisania; trzeba jeszcze spr prawa do przypisywania !!!; jeżeli IdStrumienia = null to wszystkich kier przypiętych do struktury weźmie - nie wiem czy nie powinien wszystkich, ale kier bez struktury nie ma wglądu...
        {
            DataSet ds = db.getDataSet(@"
select distinct S.IdPrzelozonego as Id_Przelozeni, K.Nazwisko, K.Imie, K.Email, K.Rights
from Pracownicy P 
left outer join VPrzelozeniStr S on S.StrId = ISNULL(P.IdStrumienia, S.StrId)
left outer join Przelozeni K on K.Id_Przelozeni = S.IdPrzelozonego 
where P.Status >= 0 and K.Blokada = 0 and K.Status >= 0
and P.Id_Str_OrgM is null");
            for (int i = db.getCount(ds) - 1; i >= 0; i--)           // spr uprawnień 
                //if (!AppUser.HasRight(db.getValue(db.getRows(ds)[i], "Rights"), AppUser.rPrzypisanie))
                    //--db.getRows(ds)[i].Delete();
                    db.getRows(ds).RemoveAt(i);
            return ds;
        }

        //----------------------------------------------------
        public string PrepNazwisko(object ni)
        {
            return Tools.PrepareName(ni.ToString());
        }

        public string PrepareStatus(object st)
        {
            int s;
            try 
            {
                s = Convert.ToInt32(st);
            }
            catch
            {
                s = -1;
            }
            return App.GetStatus(s);
        }

        public bool IsEnabled(object marker)
        {
            return db.getInt(marker, 0) >= 1;  // 1,2
        }

        public bool IsEnabled(object marker, int m)
        {
            return db.getInt(marker, 0) == m;  // 1,2
        }

        public bool IsEnabled(object marker, int m1, int m2)
        {
            int m = db.getInt(marker, 0);
            return m == m1 || m == m2;  // 1,2
        }

        public bool IsReadOnly(object marker)
        {
            return !IsEnabled(marker);
        }
        //----------------------------------------------------
        DataSet dsLinie = null;
        bool adm = false;
        bool ocena = false;
        bool deleg = false;
        bool przyp = false;
        bool addPrac = false;
        bool ro = false;

        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            SetSelectSql();
            switch (FMode)
            {
                case moKier:
                    adm = App.User.IsAdmin;
                    ro = App.User.HasRight(AppUser.rReadOnly);
                    ocena = false;//!ro && (App.User.HasRight(AppUser.rOcenaOwn) || App.User.HasRight(AppUser.rOcenaStr) || App.User.HasRight(AppUser.rOcenaAllLines));
                    deleg = false;//!ro && App.User.HasRight(AppUser.rOddeleg);
                    przyp = false;//!ro && App.User.HasRight(AppUser.rPrzypisanie);
                    //addAPT = false;//!ro && App.User.HasRight(AppUser.rAddAPT);
                    addPrac = !ro && App.User.IsAdmin;
                    Tools.SetControlVisible(lvPracownicy, "btNewRecord", addPrac);
                    if (!ocena && !deleg && !przyp && !addPrac || ro)
                        Tools.SetControlVisible(lvPracownicy, "thControl", false);
                    break;
                case moSearch:
                    Tools.SetControlVisible(lvPracownicy, "thControl", false);
                    break;
                case moNoLine:
                    /*
                    HtmlTableCell th = (HtmlTableCell)lvPracownicy.FindControl("thControl");
                    th.RowSpan = 1;
                    th = (HtmlTableCell)lvPracownicy.FindControl("thPracownik");
                    th.RowSpan = 1;
                    th = (HtmlTableCell)lvPracownicy.FindControl("thStanowisko");
                    th.RowSpan = 1;                    
                    //Tools.SetControlVisible(lvPracownicy, "thJednM", false);
                    //Tools.SetRowSpan(lvPracownicy, "thAbsencja", 1);
                    Tools.SetControlVisible(lvPracownicy, "trThLine2", false);
                    */
                    Tools.SetControlVisible(lvPracownicy, "thJednA", false);
                    Tools.SetControlVisible(lvPracownicy, "thJednA2", false);
                    Tools.SetControlVisible(lvPracownicy, "thOd2", false);
                    Tools.SetControlVisible(lvPracownicy, "thDo2", false);
                    Tools.SetControlVisible(lvPracownicy, "thRating", false);
                    Tools.SetControlVisible(lvPracownicy, "thOcena", false);
                    Tools.SetControlVisible(lvPracownicy, "thStatus", false);                    
                    break;
                case moAdmin:
                    adm = App.User.IsAdmin;
                    ro = App.User.HasRight(AppUser.rReadOnly);
                    //addAPT = false; // !ro && App.User.HasRight(AppUser.rAddAPT);
                    addPrac = !ro && App.User.IsAdmin;
                    Tools.SetControlVisible(lvPracownicy, "btNewRecord", addPrac);
                    break;
                case moPrzypiszRCP:
                    Tools.SetControlVisible(lvPracownicy, "thSelect", true);
                    Tools.SetControlVisible(lvPracownicy, "thControl", false);
                    paFilter.Visible = true;
                    break;
                case moScal:
                    Tools.SetControlVisible(lvPracownicy, "btNewRecord", false);
                    break;
            }
            int sort = Sort;
            Report.ShowSort(lvPracownicy, sort, sort > 0);
        }

        protected void lvPracownicy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.InsertItem)
            {
                switch (FMode)
                {
                    case moKier:
                        Tools.SetControlVisible(e.Item, "btOcen", ocena);
                        Tools.SetControlVisible(e.Item, "btOddeleguj", deleg);
                        Tools.SetControlVisible(e.Item, "btEdit2", przyp);  
                        if (!ocena && !deleg && !przyp && !addPrac)
                            Tools.SetControlVisible(e.Item, "tdControl", false);

                        //Tools.SetControlVisible(e.Item, "lbStatusLabel", false);
                        //Tools.SetControlVisible(e.Item, "ddlStatus", false);
                        
                        break;
                    case moSearch:
                        Tools.SetControlVisible(e.Item, "tdControl", false);
                        break;
                    case moNoLine:
                        Button bt = (Button)Tools.SetControlVisible(e.Item, "btPrzypisz", true);
                        if (bt != null) Tools.MakeConfirmButton(bt, "Potwierdź przypisanie pracownika.");
                        //Tools.SetControlVisible(e.Item, "tdJednM", false);
                        Tools.SetControlVisible(e.Item, "tdJednA", false);
                        Tools.SetControlVisible(e.Item, "tdJednAod", false);
                        Tools.SetControlVisible(e.Item, "tdJednAdo", false);
                        Tools.SetControlVisible(e.Item, "tdRating", false);
                        Tools.SetControlVisible(e.Item, "tdOcena", false);
                        Tools.SetControlVisible(e.Item, "tdStatus", false);

                        //Tools.SetControlVisible(e.Item, "lbStrumienM", false); 
                        //Tools.SetControlVisible(e.Item, "lbLiniaM", false);
                        //Tools.SetControlVisible(e.Item, "lbKierownikM", false);                       
                        Tools.SetControlVisible(e.Item, "paJednM", false);
                        Tools.SetControlVisible(e.Item, "paPrzJednM", true);

                        break;
                    case moOcena:
                        Tools.SetControlVisible(e.Item, "btOcen", true);
                        break;
                    case moAdmin:
                        bool editItem = e.Item.ItemType == ListViewItemType.DataItem &&
                                        ((ListViewDataItem)e.Item).DisplayIndex == lvPracownicy.EditIndex &&
                                        lvPracownicy.EditItem != null;
                        if (e.Item.ItemType == ListViewItemType.InsertItem || editItem)     // insert || edit
                        {

                        }
                        else        // select
                        {
                            Tools.SetControlVisible(e.Item, "EditButton", true);
                            Tools.SetControlVisible(e.Item, "DeleteButton", true);
                        }
                        break;
                    case moPrzypiszRCP:
                        Tools.SetControlVisible(e.Item, "tdSelect", true);
                        Tools.SetControlVisible(e.Item, "tdControl", false);
                        break;
                    case moScal:
                        break;
                }
            }

            InitItem2(e);
        }

        private int InitItem2(ListViewItemEventArgs e)  // 0 - select, 1 - edit, 2 - insert, -1 - other
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, lvPracownicy, out select, out edit, out insert);

            if (edit || insert)
            {
                switch (FMode)
                {
                    case moKier:
                    case moAdmin:   // podwojne sprawdzanie warunku, ale dla porządku ...
                        if (FMode == moAdmin || addPrac)
                        {
                            string idGrZatr = null;
                            string idStan = null;
                            string idJM = null;
                            string idStatus = App.stNew.ToString();
                            if (edit)
                            {
                                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                                DataRowView drv = (DataRowView)dataItem.DataItem;
                                idGrZatr = drv["Id_Gr_Zatr"].ToString();
                                idStan = drv["Id_Stanowiska"].ToString();
                                idJM = String.Format("{0}|{1}", drv["Id_Str_OrgM"], drv["IdKierownika"]);
                                idStatus = drv["Status"].ToString();
                            }
                            if (insert)
                                Tools.SetChecked(e.Item, "cbAPT", true);
                            if (FMode == moKier)
                                Tools.SetControlEnabled(e.Item, "cbAPT", false);

                            DataSet ds = db.getDataSet("select * from GrZatr order by Rodzaj_Umowy");
                            Tools.BindData(e.Item, "ddlGrupaZatr", ds, "Rodzaj_Umowy", "Id_Gr_Zatr", true, idGrZatr);

                            ds = db.getDataSet("select * from Stanowiska order by Nazwa_Stan");
                            Tools.BindData(e.Item, "ddlStanowisko", ds, "Nazwa_Stan", "Id_Stanowiska", true, idStan);

                            /*zzz
                            ds = App.GetLinieKierList(null, true);
                            Tools.BindData(e.Item, "ddlJednM", ds, "Linia", "Id", true, idJM);
                            */

                            DropDownList ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                            if (ddl != null)
                            {
                                App.FillStatus(ddl, idStatus, FMode == moAdmin);
                                //ddl.Enabled = adm || addPrac;
                                //ddl.Enabled = false; // RPP - nie edytujemy !!!
                            }
                        }
                        break;
                }
            }
            else if (select)
            {
                switch (FMode)
                {
                    case moNoLine:
                        Tools.BindData(e.Item, "ddlJednM", dsLinie, "Linia", "Id", true, null);
                        break;
                }
            }
            return lim;
        }

        private bool InitItem(ListViewItemEventArgs e)  // czy dany item jest edit item
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            DataRowView drv = (DataRowView)dataItem.DataItem;
            bool edit = dataItem.DisplayIndex == lvPracownicy.EditIndex && lvPracownicy.EditItem != null;
            //----- select -----
            string liniaM = drv["IdLiniiM"].ToString();
            string liniaA = drv["IdLiniiA"].ToString();
            bool apt = db.getBool(drv["APT"], false);

            //bool admini = db.getInt(drv["Nr_Ewid"].ToString(), 0) < 0;
            bool admini = Tools.StrToInt(drv["Nr_Ewid"].ToString(), 0) < 0;

            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trLine");
            if (tr != null)
            {
                if (liniaM == liniaA)
                    Tools.AddClass(tr, "actual");       //mój, u mnie
                else
                    if (liniaM != LiniaId)
                    {
                        Tools.AddClass(tr, "obcy");     //obcy, u mnie
                        Tools.SetValue(e.Item, "hidObcy", "1");
                    }
                    else
                        Tools.AddClass(tr, "deleg");    //mój, oddelegowany
                if (apt)
                    Tools.AddClass(tr, "apt");
            }
            switch (FMode)
            {
                case moNoLine:
                    string str = drv["SymbolStrumieniaM"].ToString();
                    if (String.IsNullOrEmpty(str))
                        Tools.SetText(e.Item, "lbStrumien", "brak przypisania do strumienia");
                    break;
                case moKier:
                    if (addPrac)
                        //if (apt)
                        if (admini)
                        {
                            Tools.SetControlVisible(e.Item, "EditButton", true);
                            Tools.SetControlVisible(e.Item, "DeleteButton", true);
                            Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                            Tools.SetControlVisible(e.Item, "btEdit2", false);
                        }
                    break;
                case moAdmin:
                    Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                    break;
            }
            //----- edycja -----
            if (edit)
            {
                switch (FMode)
                {
                        /*zzz
                    case moKier:
                        Tools.SetControlVisible(e.Item, "tdOddeleguj", true);
                        DropDownList ddl = (DropDownList)e.Item.FindControl("ddlLinia");
                        if (ddl != null)
                        {
                            DataSet dsL = App.GetLinieKierList(liniaM, true);
                            Tools.BindData(ddl, dsL, "Linia", "Id", true, null);
                        }
                        break;
                         */ 
                    case moNoLine:
                        /*
                        Tools.SetControlVisible(lvPracownicy, "thJednM", true);
                        Tools.SetControlVisible(e.Item, "tdJednM", true);
                        string sL, nL, sS, nS;
                        App.GetLiniaData(App.KierLiniaId, out sL, out nL, out sS, out nS);
                        Tools.SetText(e.Item, "lbStrumienM", sS).ToolTip = nS;
                        Tools.SetText(e.Item, "lbLiniaM", sL).ToolTip = nL;
                        ddl = (DropDownList)e.Item.FindControl("ddlKierownik");
                        if (ddl != null)
                        {
                            DataSet ds = App.GetKierList(App.KierLiniaId, null); 
                            Tools.BindData(ddl, ds, "Kierownik", "Id", "wszyscy kierownicy", null);
                        }
                         */
                        break;
                    case moAdmin:
                        break;
                }
                return true;
            }
            else
                return false;
        }

        //-----------------------
        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            InitItem2(e);

            if (e.Item.ItemType == ListViewItemType.DataItem)
                InitItem(e);
        }

        protected void lvPracownicy_DataBinding(object sender, EventArgs e)
        {
            switch (FMode)
            {
                case moKier:
                    break;
                case moSearch:
                    break;
                    /*zzz
                case moNoLine:
                    if (!App.User.IsAdmin || App.UserIsKier(KierId)) 
                        dsLinie = App.GetLinieKierList(KierId, null, true);  
                    else
                        dsLinie = App.GetLinieKierList(null, null, true);  
                    break;
                     */ 
                case moAdmin:
                    break;
            }
        }

        private void UpdateCount()
        {
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            if (dp != null)
                Tools.SetText(lvPracownicy, "lbPracCount", dp.TotalRowCount.ToString());
        }

        protected void lvPracownicy_DataBound(object sender, EventArgs e)
        {
            switch (FMode)
            {
                case moKier:
                    break;
                case moSearch:
                    break;
                case moNoLine:
                    break;
                case moAdmin:
                    break;
            }
            UpdateCount();
        }
        //----------------
        private void EnableControls(ListView lv, bool edit, bool insert)
        {
            if (edit)
            {
            }
            else
            {
                lv.EditIndex = -1;
            }
            if (insert)
            {
                Tools.SetControlEnabled(lv, "btNewRecord", false);
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            else
            {
                lv.InsertItemPosition = InsertItemPosition.None;
                Tools.SetControlEnabled(lv, "btNewRecord", true);
                Tools.SetControlVisible(lv, "InsertButton", true);  // na empty data template ?
            }
        }

        //--------------------------------------
        private bool UpdateOddeleguj(ListViewItem item)
        {
            /*
            HiddenField pracId = (HiddenField)item.FindControl("hidPracId");
            DropDownList ddlL = (DropDownList)item.FindControl("ddlLinia");
            DropDownList ddlK = (DropDownList)item.FindControl("ddlKierownik");
            DateEdit deOd = (DateEdit)item.FindControl("deOd");
            DateEdit deDo = (DateEdit)item.FindControl("deDo");
            if (DateEdit.ValidateRange(deOd, deDo, Log.PRACOWNICY))
            {
                bool ok = db.insert("Oddelegowania", 0,
                    "IdPracownika,Od,Do,IdStruktury,IdKierownika,IdKierownikaRq,DataRq,UwagiRq,IdKierownikaAcc,DataAcc,UwagiAcc,Status",
                    pracId.Value,
                    db.strParam(deOd.DateStr),
                    db.strParam(deDo.DateStr),
                    Tools.GetLineParam(ddlL.SelectedValue, 0),
                    db.nullParam(Tools.GetLineParam(ddlL.SelectedValue, 1)),
                    //db.nullParam(ddlK.SelectedValue),
                    db.NULL, db.NULL, db.NULL,
                    App.LoggedUserId,
                    "GETDATE()",
                    db.NULL,
                    Wnioski.stAccepted);
                return true;
            }
            */ 
            return false;
        }

        protected void lvPracownicy_PagePropertiesChanged(object sender, EventArgs e)
        {
            EnableControls(lvPracownicy, false, false);
            Select(-1);
        }

        protected void lvPracownicy_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            EnableControls(lvPracownicy, false, false); // zeby w inserting mozna bylo e.Cancel ustawic i wyswietlic ShowMessage z błędem, a nie schowało InsertTemplate
        }

        //---------------------

        private void PrepareEdit2(ListViewCommandEventArgs e, bool edit, bool update)
        {
            /*zzz
            bool cancel = false;
            switch (FMode)
            {
                case moKier:
                    HtmlTableRow trLine = (HtmlTableRow)e.Item.FindControl("trLine");
                    if (edit)
                    {
                        Tools.SetControlVisible(e.Item, "paJednM", false);
                        Tools.SetControlVisible(e.Item, "paPrzJednM", true);
                        
                        if (!App.User.IsAdmin || App.UserIsKier(KierId))
                            dsLinie = App.GetLinieKierList(KierId, null, true);
                        else
                            dsLinie = App.GetLinieKierList(null, null, true);
                        
                        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                        string pid = lvPracownicy.DataKeys[dataItem.DisplayIndex].Value.ToString();

                        DataRow dr = db.getDataRow(@"
                            select convert(varchar,P.Id_Str_OrgM) + '|' + ISNULL(convert(varchar, P.IdKierownika),'') as Id,
                                S.Symb_Jedn + ' / ' + L.Symb_Jedn + ISNULL(' (' + L.Nazwa_Jedn + ')','') + ' - ' + ISNULL(K.Nazwisko + ' ' + K.Imie, 'wszyscy przełożeni') as Linia
                            from Pracownicy P
                            left outer join StrOrg L on L.Id_Str_Org = P.Id_Str_OrgM
                            left outer join StrOrg S on S.Id_Str_Org = L.Id_Parent
                            left outer join Przelozeni K on K.Id_Przelozeni = P.IdKierownika
                            where P.Id_Pracownicy = " + pid);
 
                        string linia = db.getValue(dr, "Id");
                        string nazwa = db.getValue(dr, "Linia");
                        //Tools.SetText(e.Item, "lbStrumien", nazwa);
                        Tools.SetControlVisible(e.Item, "lbStrumien", false);
                        Tools.BindData(e.Item, "ddlJednM", dsLinie, "Linia", "Id", true, linia);

                        if (trLine != null)
                            Tools.AddClass(trLine, "eit");
                    }
                    else
                    {
                        if (update)
                            cancel = !PrzypiszUpdate(e);
                        if (!cancel)
                        {
                            Tools.SetControlVisible(e.Item, "paJednM", true);
                            Tools.SetControlVisible(e.Item, "paPrzJednM", false);
                        }
                        if (trLine != null)
                            Tools.RemoveClass(trLine, "eit");
                    }
                    if (!cancel)
                    {
                        Tools.SetControlVisible(e.Item, "btOcen", !edit);
                        Tools.SetControlVisible(e.Item, "btOddeleguj", !edit);
                        Tools.SetControlVisible(e.Item, "btEdit2", !edit);
                        Tools.SetControlVisible(e.Item, "btUpdate2", edit);
                        Tools.SetControlVisible(e.Item, "btCancel2", edit);
                    }
                    break;
            }
             */ 
        }

        private bool PrzypiszUpdate(ListViewCommandEventArgs e)
        {
            string sid = Tools.GetDdlSelectedValue(e.Item, "ddlJednM");
            string lid = Tools.GetLineParam(sid, 0);
            if (!String.IsNullOrEmpty(lid))
            {
                int idx = ((ListViewDataItem)e.Item).DisplayIndex;
                string pid = lvPracownicy.DataKeys[idx].Value.ToString();
                bool ok = db.update("Pracownicy", 0, "Id_Str_OrgM,IdKierownika,IdStrumienia", "Id_Pracownicy=" + pid,
                    lid,
                    db.nullParam(Tools.GetLineParam(sid, 1)),
                    db.NULL);
                if (ok)
                {
                    lvPracownicy.DataBind();
                    return true;
                }
                else Tools.ShowMessage("Wystąpił błąd podczas przypisywania pracownika do linii.");
            }
            else Tools.ShowMessage("Proszę wybrać linię.");
            return false;
        }

        private bool GotoOddeleguj(ListViewCommandEventArgs e)
        {
            if (Oddeleguj != null)
            {
                int idx = ((ListViewDataItem)e.Item).DisplayIndex;
                delPracId = lvPracownicy.DataKeys[idx].Value.ToString();  //e.CommandArgument.ToString()
                HiddenField hid = (HiddenField)e.Item.FindControl("hidObcy");
                delToMe = hid != null && hid.Value == "1";
                Oddeleguj(this, EventArgs.Empty);
            }
            return true;
        }

        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = lvPracownicy;
            switch (e.CommandName)
            {
                case "NewRecord":
                    EnableControls(lv, false, true);
                    break;
                case "Edit":
                    EnableControls(lv, true, false);
                    hidFunc.Value = e.CommandArgument.ToString();
                    break;
                //case "Insert":
                case "CancelInsert":
                    EnableControls(lv, false, false);
                    break;
                //----- letter data pager -----
                case "JUMP":
                    EnableControls(lv, false, false);
                    Select(-1);
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("NazwiskoImie", SortDirection.Ascending);
                    
                    tbSearch.Text = null;

                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    /*
                    lvPracownicy.SelectedIndex = -1;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                    */
                    break;
                //----- custom -----
                case "Update":
                    switch (hidFunc.Value)
                    {
                        case fnOddeleguj:
                            if (UpdateOddeleguj(e.Item))
                                EnableControls(lv, false, false);
                            break;
                        case fnPrzypisz: //<<<<<<< spr
                            EnableControls(lv, false, false);
                            break;
                    }
                    break;
                case "Przypisz":
                    PrzypiszUpdate(e);
                    break;
                /*
                case "Ocena":
                    HttpContext.Current.Response.Redirect(App.MatrycaForm + "?pid=" + e.CommandArgument.ToString());
                    break;
                */
                case "Deleg":
                    GotoOddeleguj(e);
                    break;
                case "Edit2":
                    PrepareEdit2(e, true, true);
                    break;
                case "Update2":
                    PrepareEdit2(e, false, true);
                    break;
                case "Cancel2":
                    PrepareEdit2(e, false, false);
                    break;
                //----- RPP -----
                case "SetPass":
                    /*zzz
                    TextBox tbPass = e.Item.FindControl("tbPass") as TextBox;
                    if (tbPass != null)
                    {
                        if (!String.IsNullOrEmpty(tbPass.Text.Trim()))
                        {
                            idx = ((ListViewDataItem)e.Item).DisplayIndex;
                            string pid = lvPracownicy.DataKeys[idx].Value.ToString();
                            string nrew = null;
                            TextBox tbNrEw = e.Item.FindControl("Nr_EwidTextBox") as TextBox;
                            if (tbNrEw != null)
                                nrew = tbNrEw.Text;
                            if (Pracownik.SetPass(pid, nrew, tbPass.Text))
                            {
                                tbPass.Text = null;
                                Tools.ShowMessage("Hasło zostało ustawione.");
                            }
                            else Tools.ShowMessage("Wystąpił błąd podczas ustawiania hasła.");
                        }
                        else Tools.ShowError("Hasło nie może być puste.");
                    }
                     */
                    break;
            }
        }

        protected void lvPracownicy_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Session[ClientID + sesSortId] = sort;  // unikalne co do kontrolki
        }

        //----------
        /*
        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = lvPracownicy;
            switch (e.CommandName)
            {
                case "NewRecord":
                    lv.EditIndex = -1;
                    //Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                    Tools.SetControlEnabled(lv, "btNewRecord", false);
                    lv.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "Edit":
                    lv.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlEnabled(lv, "btNewRecord", true);
                    Tools.SetControlVisible(lv, "InsertButton", true);
                    break;
                case "Insert":
                case "CancelInsert":
                    //Tools.SetControlVisible(lvZastepstwa, "btNewRecord", true);
                    Tools.SetControlEnabled(lv, "btNewRecord", true);
                    lv.InsertItemPosition = InsertItemPosition.None;
                    break;
                //----- letter data pager -----
                case "JUMP":

                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("NazwiskoImie", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    /*
                    lvPracownicy.SelectedIndex = -1;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                    * /
                    break;
                //----- custom -----
            }
        }
         */
        
        
        //----- modełko -------------------------------------------------------
        /*
        protected void _LayoutCreated(object sender, EventArgs e)
        {
        }

        protected void _Sorting(object sender, ListViewSortEventArgs e)
        {
        }

        protected void _ItemCreated(object sender, ListViewItemEventArgs e)
        {
        }

        protected void _ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }
        
        protected void _DataBound(object sender, EventArgs e)
        {
        }
    
        protected void _ItemCommand(object sender, ListViewCommandEventArgs e)
        {
        }

        protected void _PagePropertiesChanged(object sender, EventArgs e)
        {
        }
        */
        //----------------------

        //----------------------
        private bool UpdateItem(EventArgs ea, ListViewItem item, IOrderedDictionary values, bool insert)
        {
            switch (FMode)
            {
                default:
                    return true;
                case moSearch:
                    break;
                case moNoLine:
                    break;
                case moOcena:
                    break;
                case moKier:
                case moAdmin:   // eit nie może leżeć w tr runat=server bo wtedy value.count = 0 !!!!!!!!
                    if (FMode == moAdmin || addPrac)
                    {
                        string jm = Tools.GetDdlSelectedValue(item, "ddlJednM");
                        string linia = Tools.GetLineParam(jm, 0);
                        values["Id_Gr_Zatr"] = Tools.GetDdlSelectedValueInt(item, "ddlGrupaZatr");
                        values["Id_Stanowiska"] = Tools.GetDdlSelectedValue(item, "ddlStanowisko");
                        values["Id_Str_OrgM"] = linia;
                        values["IdKierownika"] = Tools.GetLineParam(jm, 1);        
                        values["IdStrumienia"] = null;
                        values["Status"] = Tools.GetDdlSelectedValue(item, "ddlStatus");
                        //if (insert) values["Status"] = App.stNew;

                        Log.LogChanges(Log.PRACOWNICY, "Pracownicy", ea);
                        if (String.IsNullOrEmpty(linia) && FMode == moKier)
                            Tools.ShowMessage(String.Format(
                                "Uwaga!\\n\\nPracownikowi: {0} {1} nie została przypisana jednostka macierzysta - nie będzie widoczny na liście pracowników.\\n" + 
                                "Dostęp do pracownika można uzyskać z opcji:\\n" +
                                "Start | Pracownicy bez przypisania do linii.", values["Nazwisko"], values["Imie"]));
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }

        private bool CanDelete(ListViewDeleteEventArgs e)
        {
            string id = e.Keys[0].ToString();
            DataRow dr = db.getDataRow("select top 1 Id_Oceny from Oceny where Id_Pracownicy = " + id);
            if (dr != null)
            {
                Tools.ShowMessage("Usunięcie niemożliwe.\\nPracownik posiada wystawione oceny.");
                return false;
            }
            else
            {
                Log.LogChanges(Log.PRACOWNICY, "Pracownicy", e);
                return true;
            }
        }

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(e, lvPracownicy.EditItem, e.NewValues, false);
        }

        protected void lvPracownicy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e, e.Item, e.Values, true);
        }

        protected void lvPracownicy_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            e.Cancel = !CanDelete(e);
        }
        //-------------------
        protected void lvPracownicy_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {

        }

        protected void lvPracownicy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {

        }
        //-------------------
        private void SetPageSize(DataPager dp)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            DropDownList ddl = (DropDownList)lvPracownicy.FindControl("ddlLines");
            if (ddl != null)
            {
                if (ddl.SelectedValue == "all")
                    dp.SetPageProperties(0, dp.TotalRowCount, true);
                else
                {
                    int size = Tools.StrToInt(ddl.SelectedValue, 10);
                    if (size == 0) size = 10;
                    dp.SetPageProperties((dp.StartRowIndex / size) * size, size, true);
                }
            }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["pagesize"] = ((DropDownList)sender).SelectedValue;  // jak Brak danych to nie ustawia i trzeba samemu
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            SetPageSize(dp);
        }

        /*
        protected void ddlJednM_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (String.IsNullOrEmpty(ddl.SelectedValue))
                ddl.ToolTip = null;
            else
                ddl.ToolTip = ddl.SelectedItem.Text;
        }
         */
        //---------------------------------------------------
        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
        //   Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
            string ss = tbSearch.Text.Trim();
            if (ss.StartsWith("<") && ss.EndsWith(">"))
                ss = ss.Substring(1, ss.Length - 2);

            string[] words = ss.Split(' ');
            if (words.Length == 1)
            {
                SqlDataSource1.FilterExpression = "(Nazwisko like '{0}%' or Imie like '{0}%' or Nr_Ewid like '{0}%')";
                SqlDataSource1.FilterParameters.Clear();
                SqlDataSource1.FilterParameters.Add("par0", words[0]);
            }
            else
            {
                string[] exp = new string[words.Length];
                SqlDataSource1.FilterParameters.Clear();
                for (int i = 0; i < words.Length; i++)
                {
                    exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or Nr_Ewid like '{{{0}}}%')", i);
                    SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                }

                SqlDataSource1.FilterExpression = String.Join(" and ", exp);
            }
            lvPracownicy.DataBind();
            if      (lvPracownicy.Items.Count == 1)    Select(0);
            else if (lvPracownicy.SelectedIndex != -1) Select(-1); 
        }

        public void Select(int index)
        {
            lvPracownicy.SelectedIndex = index;
            CheckSelectedChanged();
        }

        private bool CheckSelectedChanged()
        {
            string oldId = App.SelectedPracId;
            string newId = lvPracownicy.SelectedDataKey == null ? null : lvPracownicy.SelectedDataKey.Value.ToString();
            if (oldId != newId)
            {
                App.SelectedPracId = newId;
                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);
                return true;
            }
            else
                return false;
        }

        protected void lvPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedChanged();
        }

        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {
        }
        //---------------------------------------------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public string Data    // jak null to dzisiejszą
        {
            get
            {
                if (String.IsNullOrEmpty(hidData.Value))
                    hidData.Value = Tools.DateToStr(DateTime.Today);
                return hidData.Value;
            }
            set { hidData.Value = value; }
        }

        public string KierId 
        {
            get { return hidKierId.Value; }
            set { hidKierId.Value = value; }
        }

        public string LiniaId
        {
            get { return hidLiniaId.Value; }
            set { hidLiniaId.Value = value; }
        }

        public string Linie // musi być jako ViewState a nie hidLinie bo sortowanie nie chodzi 
        {
            get { return Tools.GetViewStateStr(ViewState["linie"]); }
            set { ViewState["linie"] = value; }
        }

        public string Zakres
        {
            get { return Tools.GetViewStateStr(ViewState["zakres"]); }
            set { ViewState["zakres"] = value; }
        }

        public string Params
        {
            get { return hidParams.Value; }
            set { hidParams.Value = value; }
        }
        
        public string sql
        {
            get
            {
                object o = ViewState["sql"];
                if (o != null)
                    return o.ToString();
                else
                    return null;
            }
            set { ViewState["sql"] = value; }
        }

        public int Sort
        {
            get
            {
                object s = Session[ClientID + sesSortId];
                if (s != null)
                {
                    int sort = (int)s;
                    if (sort >= 1 && sort <= maxSortCol)
                        return sort;
                }
                return FDefSortColumn;
            }
            set { FDefSortColumn = value; }
        }

        public ListView List
        {
            get { return lvPracownicy; }
        }

    }
}
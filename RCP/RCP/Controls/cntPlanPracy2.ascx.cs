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
using System.ComponentModel;

namespace HRRcp.RCP.Controls
{
    public partial class cntPlanPracy2 : System.Web.UI.UserControl
    {
        public event EventHandler SelectDay;
        public event EventHandler ShowRozliczenie;
        public event EventHandler ShowPlanUrlopow;

        const bool lockZast = true;         // blokada akceptacji dla zastępujących

        //SqlConnection con = null;

        public const int asKier     = 0;    // kierownik ogląda swoich ludzi
        public const int asParent   = 1;    // kierownik ogląda swoich ludzi swoich kierowników
        public const int asDostepni = 2;    // kierownik ogląda udostępnionych pracowników / kierowników
        int FViewAs = asKier;

        public const int moZmiany = 0;
        public const int moAccept = 1;
        public const int moKartaRoczna = 2;
        public const int moRozliczenie = 3;
        public const int moPlanUrlopow = 4;
        public const int moPlanUrlopowRok = 5;       // tylko dla nagłówka cntPlanPracyLineHeader

        public const int smDefault = 0;
        public const int smPlanAcc = 1;


        int FMode = moZmiany;

        public const int moKier     = 0;
        public const int moAdmin    = 1;
        int FAdm = moKier;


        //----- KODY:ZMIANA.TYP -----
        public const int zmNormalna     = 0;
        public const int zmSobota       = 1;    //200%,150%
        public const int zmNiedziela    = 2;    //200%,200%
        public const int zmTylkoNadgN1  = 3;
        public const int zmPoniedzialek5= 4;    // 5-6 - 200%


        
        
        public const string maHoliday       = "h";   // dzień wolny
        public const string maInfo          = "i";   // są zmiany kierownika
        public const string maAbsencja      = "u";   // jest absencja
        public const string _maAbsencjaKier  = "U";   // jest absencja wpisana przez kierownika
        public const string maAlert         = "A";   // alert mrugający
        public const string maAlert2        = "B";   // bez migania
        public const string maAccepted      = "a";   // dzień zaakceptowany, na razie nie jest jakos specjalnie oznaczany
        public const string maNoAcc         = "n";   // dzień nie zaakceptowany w zaakceptowanym przez kier. okresie
        public const string maToday         = "t";   // dziś
        public const string maNoDay         = "x";   // do raportów - brak dnia np 31.02 przy zestawieniu z 31 kolumnami
        public const string maOff           = "0";   // pracownik wyłączony - brak algorytmu lub nie sprawdzamy czasu - alg0
        public const string _maSelected     = "s";   // kliknięty dzień
        public const string maBlockEdit     = "p";   // dla Przypisań
        public const string maPrzesun       = "P";   // dla Przypisań, skonczył sie okres przesuniecie DoMonit, ale nadal jest u mnie, powinienem przesunąć, albo powinien do mnie wrócić - nie mam prawa do edycji, musi wystąpić z 'p'

        public const string maUrlopKorekta  = "k";   // dla PlanuUrlopów - korekta
        public const string maPoUmowie      = "P";   //z  dla PlanuUrlopów - okres po umowie, żeby inaczej zaznaczyć <<<<< na razie jak przesun - z podkresleniem na czerwono

        //private Worktime wt = null;
        //private Worktime2 wt = null;

        private string cntPrefix;
        public cntPlanPracyLineHeader LineHeader;
        private CheckBox cbDniAll;
        private CheckBox cbPracAll;

        Ustawienia settings;
        TimeSumator sumator;
        int FBreakMM, FBreak2MM, FMarginMM, Fzaokr, FzaokrType;  // na potrzeby databind, jak nie ma datbind to moze to być 0!!!
        DateTime FDataAccOd, FDataAccDo, FDataBlockedTo;
        
        int kierCount;

        //--------------------
        private bool WithPrzypisania()
        {
            return true;



            AppUser user = AppUser.CreateOrGetSession();
            return user.HasRight(AppUser.rTester) && 
                   !String.IsNullOrEmpty(DateFrom) &&
                   dtFrom >= DateTime.Parse("2013-05-01");  // umowna data, zastsanowic sie nad dodaniem przypisań na podstawie PracownicyOkresy ...
        }

        private void PrepareDataSource()
        {
            switch (FMode)
            {
                case moRozliczenie:
                    lvPlanPracy.DataSourceID = "SqlDataSourceRozl";
                    break;
                case moPlanUrlopow:
                    //showMoj = false;
                    //showMoj = true;
                    //AppUser user = AppUser.CreateOrGetSession();                    // App.User zwraca null nie wiem dlaczego ... bo RcpMaster.OnInit sie wykonuje przed ...
                    //showMoj = (FAdm == moAdmin || App.User.HasRight(AppUser.rPlanUrlopowSwoj)) && IsRoot();  // edycja własnego planu urlopów
                    lvPlanPracy.DataSourceID = "SqlDataSourcePlanUrlopow";
                    break;
                default:
                    switch(SubMode)
                    {
                        case smPlanAcc:
                            lvPlanPracy.DataSourceID = "dsAccList";
                            break;
                        case smDefault:
                        default:
                            lvPlanPracy.DataSourceID = "SqlDataSource2";
                            break;
                    }
                    break;
            }
            /* po wygenerowaniu na podstawie PracownicyOkresy !!!
            if (WithPrzypisania())
                lvPlanPracy.DataSourceID = "SqlDataSource2";
            else
                lvPlanPracy.DataSourceID = "SqlDataSource1";
             */ 
        }

        /*
            return App.User.HasRight(AppUser.rTester) && 
                   !String.IsNullOrEmpty(DateFrom) &&
                   dtFrom >= DateTime.Parse("2013-05-01");  // umowna data, zastsanowic sie nad dodaniem przypisań na podstawie PracownicyOkresy ...

        private void PrepareDataSource()
        {
            AppUser user = AppUser.CreateOrGetSession();
            if (user.HasRight(AppUser.rTester))
            {
                if (dtFrom < DateTime.Parse("2013-05-01"))
                    lvPlanPracy.DataSourceID = "SqlDataSource1";
                else
                    lvPlanPracy.DataSourceID = "SqlDataSource2";
            }
        }
         */
        //----------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvBottomSums);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;   // do filtrowania ds
                hidUserZastId.Value = App.User.OriginalId;
                switch (FMode)
                {
                    case moPlanUrlopow:
                        //AppUser user = AppUser.CreateOrGetSession();                    // App.User zwraca null nie wiem dlaczego ... bo RcpMaster.OnInit sie wykonuje przed ...
                        //showMoj = (FAdm == moAdmin || App.User.HasRight(AppUser.rPlanUrlopowSwoj)) && IsRoot();  // edycja własnego planu urlopów
                        break;
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            //if (wt != null) wt.Unload();
        }
        //-------------
        protected void OnSelectPath(object sender, EventArgs e)
        {
            IdKierownika = cntPath.SelParam;
            PrepareDataSource();
            DataBind();
        }

        protected void ibtSubItemsBack_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(IdKierownika) && !IsRoot())
                cntPath.Back(-1);
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["pagesize"] = ((DropDownList)sender).SelectedValue;  // jak Brak danych to nie ustawia i trzeba samemu
            DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
            SetPageSize(dp);
        }

        public void SelectCell(string pracId, int dayIndex, bool select)
        {
            if (dayIndex != -1)
                foreach (ListViewItem item in lvPlanPracy.Items)
                {
                    cntPlanPracyLine2 ppl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                    if (ppl != null)
                        if (ppl.PracId == pracId)
                        {
                            ppl.SelectCell(dayIndex, select);
                            break;
                        }
                }
        }

        protected void btSelectCell_Click(object sender, EventArgs e)
        {
            switch (FMode)
            {
                default:
                case moZmiany:
                    UpdateDays();
                    break;
                case moAccept:
                    SelectCell(PrevClickPracId, PrevClickDayIndex, false);
                    SelectCell(ClickPracId, ClickDayIndex, true);
                    PrevClickPracId = ClickPracId;
                    PrevClickDayIndex = ClickDayIndex;
                    if (SelectDay != null) SelectDay(this, EventArgs.Empty);
                    break;
            }
        }
        //--------------------------------------------------------------------------------
        private void InitPath(string kierId)
        {
            string kn;
            if (String.IsNullOrEmpty(kierId)) kn = null;
            else if (kierId == "0") kn = "Poziom główny";
            else kn = Worker.GetNazwiskoImie(kierId);
            cntPath.Prepare(kn, kierId, "0");

            switch (FMode)
            {
                case moAccept:
                    RootId = !String.IsNullOrEmpty(kierId) && (FAdm == moAdmin || (App.User.HasRight(AppUser.rPlanPracySwoj) && App.User.Id == kierId)) ? kierId : "-1";  // adm zawsze, kier tylko jak on jest root, bo moga byc kier udostępnieni
                    ZastId = lockZast ? App.User.OriginalId : "0";
                    Zakres = "0";       // póki co, nie jest wykorzytywany
                    break;
                case moPlanUrlopow:
                    RootId = !String.IsNullOrEmpty(kierId) && (FAdm == moAdmin || App.User.HasRight(AppUser.rPlanUrlopowSwoj)) ? kierId : "-1";
                    ZastId = lockZast ? App.User.OriginalId : "0";
                    Zakres = "0";
                    break;
                default:
                    RootId = "-1";
                    ZastId = lockZast ? App.User.OriginalId : "0";
                    Zakres = "0";
                    break;
            }
        }





        public void _Prepare(string dFrom, string dTo, string okresId, int okresStatus, bool okresArch)   // panel administratora.selectokres
        {
            HideSelection();

            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            IsArch = okresArch;
            OkresId = okresId;    // tu ustawia sql wiec IsArch musi byc ustawione wczesniej !!!
            OkresStatus = okresStatus;
            PrepareDataSource();
            DataBind();
        }

        //public void Prepare(string kierId, string dFrom, string dTo, string okresId, int okresStatus, bool okresArch)   // panel kierownika
        //{
        //    HideSelection();

        //    IdKierownika = kierId;  // tu też sql ale pozniej OkresId powtórnie
        //    InitPath(kierId);
        //    hidFrom.Value = dFrom;
        //    hidTo.Value = dTo;
        //    IsArch = okresArch;
        //    OkresId = okresId;  // tu ustawia sql wiec IsArch musi byc ustawione wczesniej !!!
        //    OkresStatus = okresStatus;
        //    PrepareDataSource();
        //    DataBind();
        //}



        public void Prepare(string kierId, string dFrom, string dTo, string okresId, int okresStatus, bool okresArch, string pracId)   // panel kierownika
        {
            HideSelection();

            IdKierownika = kierId;  // tu też sql ale pozniej OkresId powtórnie
            InitPath(kierId);
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            IsArch = okresArch;
            OkresId = okresId;  // tu ustawia sql wiec IsArch musi byc ustawione wczesniej !!!
            OkresStatus = okresStatus;

            hidPracId.Value = pracId;

            PrepareDataSource();
            DataBind();
            gvBottomSums.DataBind();
        }


        public void Prepare(string kierId, string dFrom, string dTo, string okresId, int okresStatus, bool okresArch)   // panel kierownika
        {
            Prepare(kierId, dFrom, dTo, okresId, okresStatus, okresArch, null);
        }


        public void Prepare(string dFrom, string dTo)   // rozliczenie nadgodzin
        {
            _Prepare(dFrom, dTo, null, -1, false);
        }







        public void x_Prepare(string dFrom, string dTo)
        {
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
        }

        /*
        public override void DataBind()
        {
            PrepareSql();
            base.DataBind();
        }
        */

        public void Prepare(string kierId)
        {
            HideSelection();

            if (String.IsNullOrEmpty(kierId))
                IdKierownika = "-1";
            else
            {
                IdKierownika = kierId;
            }
            InitPath(kierId);
        }

        public void x_DataBind(string dFrom, string dTo)
        {
            hidFrom.Value = dFrom;
            hidTo.Value = dTo;
            PrepareDataSource();
            DataBind();
        }

        public void DataBind(string kierId)
        {
            IdKierownika = kierId;
            InitPath(kierId);
            PrepareDataSource();
            DataBind();
        }

        public bool IsRoot()
        {
            return cntPath.Path.Count == 0 || IdKierownika == cntPath.Path[0].Param;
        }

        public void HideSelection()
        {
            hidClickPracId.Value = null;
            hidClickDayIndex.Value = null;
            PrevClickPracId = null;
            PrevClickDayIndex = -1;
        }
        //--------
        private void InitDataPager()  // ustala czy pokazywac, ustawia parametry
        {
            HtmlTableRow tr = (HtmlTableRow)lvPlanPracy.FindControl("trPager");
            HtmlTableRow tr2 = (HtmlTableRow)lvPlanPracy.FindControl("trPager2");
            DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
            HRRcp.Controls.LetterDataPager ldp = (HRRcp.Controls.LetterDataPager)lvPlanPracy.FindControl("LetterDataPager1");
            switch (FMode)
            {
                default:
                case moZmiany:
                case moRozliczenie:
                case moPlanUrlopow:    
                    if (tr != null) tr.Visible = false;
                    if (dp != null)
                    {
                        dp.Visible = false;
                        dp.SetPageProperties(0, Int32.MaxValue, false);  // ustawienie tylko PageSize wywoła BindData a tego nie chcę
                    }
                    ldp.Visible = false;
                    if (tr2 != null) tr2.Visible = true;
                    break;
                case moAccept:
                    DropDownList ddl = (DropDownList)lvPlanPracy.FindControl("ddlLines");
                    if (ddl != null && dp != null)
                    {
                        object o = ViewState["pagesize"];
                        if (o != null)
                        {
                            string s = o.ToString();
                            if (s != ddl.SelectedValue)
                            {
                                Tools.SelectItem(ddl, s);
                                if (s == "all")
                                    dp.SetPageProperties(0, Int32.MaxValue, false);
                                else
                                {
                                    int size = Tools.StrToInt(s, 10);
                                    if (size == 0) size = 10;
                                    dp.SetPageProperties(0, size, false);
                                }
                            }
                        }
                    }
                    /*
                    if (tr != null && dp != null)
                        if (dp.TotalRowCount <= 10)
                        {
                            tr.Visible = false;
                            dp.PageSize = dp.TotalRowCount; //Int32.MaxValue;
                        }
                        else
                        {
                            tr.Visible = true;
                            SetPageSize(dp);
                        }
                     */ 
                    break;
            }
        }

        private void SetPageSize(DataPager dp)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            switch (FMode)
            {
                default:
                case moZmiany:
                case moRozliczenie:
                    break;
                case moAccept:
                    DropDownList ddl = (DropDownList)lvPlanPracy.FindControl("ddlLines");
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
                    break;
            }
        }

        private void RefreshLetterPager()
        {
            HRRcp.Controls.LetterDataPager ldp = (HRRcp.Controls.LetterDataPager)lvPlanPracy.FindControl("LetterDataPager1");
            if (ldp != null)
            {
                DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
                if (dp != null && dp.TotalRowCount > dp.PageSize)
                {
                    ldp.Visible = true;
                    //if (OkresStatus == Okres.stClosed)
                    if (IsArch)
                    {
                        ldp.TbName = "PracownicyOkresy";
                        ldp.Where = "IdKierownika = " + IdKierownika + " and Kierownik = 0 and IdOkresu = " + OkresId;
                        ldp.Offset = "select count(*) from PracownicyOkresy where IdKierownika = " + IdKierownika + " and Kierownik = 1 and IdOkresu = " + OkresId;
                    }
                    else
                    {
                        ldp.TbName = "Pracownicy";
                        ldp.Where = "IdKierownika = " + IdKierownika + " and Kierownik = 0";
                        ldp.Offset = "select count(*) from Pracownicy where IdKierownika = " + IdKierownika + " and Kierownik = 1";
                        /* wpisy na formatce
                        Where="IdKierownika = @IdKierownika and Kierownik = 0"
                        Offset="select count(*) from Pracownicy where IdKierownika = @IdKierownika and Kierownik = 1"
                        ParName1="IdKierownika"
                        ParField1="hidKierId"
                         */
                    }
                    ldp.Reload();
                }
                else ldp.Visible = false;
            }
        }

        private void MakeDayClickable()
        {
            //HtmlTable tb = (HtmlTable)lvPlanPracy.TemplateControl.FindControl("itemPlaceholderContainer"); // sprobowac dac na ondatabind
            HtmlTable tb = (HtmlTable)lvPlanPracy.FindControl("lvOuterTable"); // sprobowac dac na ondatabind
            if (tb != null)
                switch (FMode)
                {
                    default:
                        break;
                    case moZmiany:
                    case moPlanUrlopow:
                        if (EditMode) Tools.AddClass(tb, "clickable");
                        else Tools.RemoveClass(tb, "clickable");
                        break;
                    case moAccept:
                        Tools.AddClass(tb, "clickable");
                        break;
                }
        }

        //----- zaznaczanie krzyżowe dni -------
        private void UpdateZmiana(ref string zmiana, string newZmiana, bool overr, bool withholiday)
        {
            string p1, p2, p3, p4, p5, p6;
            string n1, n2, n3, n4, n5, n6;  // id, symbol , kolor, info, wtime, absencja
            Tools.GetLineParams(zmiana, out p1, out p2, out p3, out p4, out p5, out p6);
            Tools.GetLineParams(newZmiana, out n1, out n2, out n3, out n4, out n5, out n6);
            bool clear = String.IsNullOrEmpty(n1);
            bool h = withholiday || p4.IndexOf('h') == -1;              // nie holiday
            bool prz = p4.IndexOf('p') == -1;                           // przypisany, nie maBlockEdit  
            if (clear || ((String.IsNullOrEmpty(p1) || overr) && h))    // nie nadpisuję chyba że na pusto 
            {
                Tools.GetLineParams(newZmiana, out n1, out n2, out n3, out n4, out n5, out n6);
                if (prz)
                    zmiana = Tools.SetLineParams(5, n1, n2, n3, p4, p5, p6);
                else
                    zmiana = Tools.SetLineParams(5, null, null, null, p4, null, p6);
            }

            /* inna metoda 
            Tools.GetLineParams(newZmiana, out n1, out n2, out n3);
            Tools.SetLineParam(ref zmiana, 0, n1);
            Tools.SetLineParam(ref zmiana, 1, n2);
            Tools.SetLineParam(ref zmiana, 2, n3);
            */
        }

        private void SetLineSelected(ListViewItem item, cntPlanPracyLine2 ppl, bool select)
        {
            ppl.Selected = select;
            HtmlTableCell tdP = (HtmlTableCell)item.FindControl("tdPracName");
            HtmlTableCell tdS = (HtmlTableCell)item.FindControl("tdColSelect");
            if (tdP != null && tdS != null)
            {
                if (select)
                {
                    tdP.Attributes["class"] += " selPrac";
                    tdS.Attributes["class"] += " selPrac";
                }
                else
                {
                    Tools.RemoveClass(tdP, "selPrac");
                    Tools.RemoveClass(tdS, "selPrac");
                }
            }
        }

        public bool IsAnyPracSelected()
        {
            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                CheckBox cbP = (CheckBox)item.FindControl("cbPrac");
                if (cbP != null && cbP.Checked)   // zaznaczeni
                    return true;
            }
            return false;
        }

        private void UpdateDays()
        {
            bool daySelected = LineHeader.IsAnySelected();
            bool pracSelected = IsAnyPracSelected();

            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                CheckBox cbP = (CheckBox)item.FindControl("cbPrac");
                if (cbP != null && cbP.Checked || !pracSelected)   // zaznaczeni lub wszyscy
                {
                    cntPlanPracyLine2 ppl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                    if (ppl != null)
                    {
                        string[] zmiany = ppl.Zmiany.Split(',');
                        for (int i = 0; i < ppl.Repeater.Items.Count; i++)
                        {
                            if (daySelected && pracSelected)
                            {
                                //RepeaterItem cell = ppl.Repeater.Items[i];
                                RepeaterItem head = LineHeader.Repeater.Items[i];
                                CheckBox cbD = (CheckBox)head.FindControl("cbDay");
                                if (cbD != null && cbD.Checked)
                                    UpdateZmiana(ref zmiany[i], Zmiana, false, true);  // wszystkie
                            }
                            else if (pracSelected)
                            {
                                UpdateZmiana(ref zmiany[i], Zmiana, false, false);  // wszystkie z wyłaczeniem świąt
                            }
                            else   // daySelected
                            {
                                RepeaterItem head = LineHeader.Repeater.Items[i];
                                CheckBox cbD = (CheckBox)head.FindControl("cbDay");
                                if (cbD != null && cbD.Checked)
                                    UpdateZmiana(ref zmiany[i], Zmiana, false, true);  // wszystkie
                            }
                        }
                        cbP.Checked = false;
                        SetLineSelected(item, ppl, false);
                        ppl.Zmiany = String.Join(",", zmiany);
                        ppl.DataBind();
                    }
                }
            }
            if (daySelected) LineHeader.SelectAll(false);
            cbDniAll.Checked = false;
            cbPracAll.Checked = false;
        }
        //-----------------------------------------------------------------------------
        public class TimeSumator
        {
            public OrderedDictionary sumZTime;      // czas na zmianie z podziałem na stawki
            public OrderedDictionary sumOTime;      // czas nadgodzin z podziałem na stawki
            public OrderedDictionary sumZmiany;     // ilości czasu nominalnego z podziałem na zmiany
            public int _sumNTime;                    // czas nocny ?
            public int sumWTime;                    // czas łączny ztime + otimeD + otimeN (czas przepracowany)
            public int sumHTime;                    // czas w niedziele i święta
            public int _sumZaNadgTime;               // wolne za nadgodziny
            
            public int sumDod50;                    // dodatek 50%
            public int sumDod100;                   // dodatek 100%

            public DataSet dsZmiany;                // zmiany
            public DataRow drZmiana = null;         // znaleziona zmiana

            public TimeSumator()
            {
                sumOTime = new OrderedDictionary();
                sumZTime = new OrderedDictionary();
                sumZmiany = new OrderedDictionary();
                //ds = Base.getDataSet(con, "select Id, Symbol, dbo.TimeToSec(Do) - dbo.TimeToSec(Od) as CzasZm, Stawka, Nadgodziny, TypZmiany, NadgodzinyDzien, NadgodzinyNoc, Visible from Zmiany");
                //                               0   1       2      3                                                4                         5                        6       7           8          9                10             11       12        13         14
                dsZmiany = db.getDataSet("select Id, Symbol, Nazwa, dbo.TimeToSec(Do) - dbo.TimeToSec(Od) as CzasZm, dbo.TimeToSec(Od) as Od , dbo.TimeToSec(Do) as Do, Stawka, Nadgodziny, TypZmiany, NadgodzinyDzien, NadgodzinyNoc, Visible, Margines, ZgodaNadg, Typ from Zmiany");
            }

            public void Reset()
            {
                sumZTime.Clear();
                sumOTime.Clear();
                sumZmiany.Clear();
                _sumNTime = 0;
                sumWTime = 0;
                sumHTime = 0;
                _sumZaNadgTime = 0;

                sumDod50 = 0;
                sumDod100 = 0;

                drZmiana = null;
            }

            public void x_Prepare()
            {
                //DataSet ds = Base.getDataSet("select distinct Stawka, Nadgodziny from Zmiany");
                //foreach (DataRow dr in ds.
                /*
                sumZTime.Add(100, null);
                sumZTime.Add(150, null);
                sumOTime.Add(100, null);
                sumOTime.Add(150, null);
                sumOTime.Add(200, null);
                 */
            }

            public DataRow FindZmiana(int zmiana)
            {
                drZmiana = null;
                foreach (DataRow dr in dsZmiany.Tables[0].Rows)
                {
                    int zm = Base.getInt(dr, "Id", -2);
                    if (zm == zmiana)
                    {
                        drZmiana = dr;
                        break;
                    }
                }
                return drZmiana;
            }

            public DataRow FindZmiana(string zmiana)
            {
                drZmiana = null;
                foreach (DataRow dr in dsZmiany.Tables[0].Rows)
                {
                    string zm = Base.getValue(dr, "Id");
                    if (zm == zmiana)
                    {
                        drZmiana = dr;
                        break;
                    }
                }
                return drZmiana;
            }

            public bool GetZmiana(DataRow drZm, out string symbol, out int zmCzas, out string stawka, out int ztyp, out string nadgodziny, out string stawkaD, out string stawkaN, out int typ)
            {
                if (drZm!= null)
                {
                    stawka = Base.getValue(drZm, "Stawka");
                    zmCzas = Base.getInt(drZm, "CzasZm", 0);
                    symbol = Base.getValue(drZm, "Symbol");
                    if (zmCzas < 0) zmCzas += 86400;
                    ztyp = Base.getInt(drZm, "TypZmiany", 0);
                    typ = Base.getInt(drZm, "Typ", 0);
                    switch (ztyp)
                    {
                        default:
                        case 0:
                            nadgodziny = Base.getValue(drZm, "Nadgodziny");
                            stawkaD = null;
                            stawkaN = null;
                            break;
                        case 1:
                            nadgodziny = null;
                            int? s = Base.getInt(drZm, "NadgodzinyDzien");
                            stawkaD = s != null ? s.ToString() : null;
                            s = Base.getInt(drZm, "NadgodzinyNoc");
                            stawkaN = s != null ? s.ToString() : null;
                            break;
                    }
                    return true;
                }
                else
                {
                    ztyp = 0;
                    typ = 0;
                    zmCzas = 0;
                    symbol = null;
                    stawka = null;
                    stawkaD = null;
                    stawkaN = null;
                    nadgodziny = null;
                    return false;
                }
            }

            public bool FindZmiana(string zmiana, out string symbol, out int czasZm, out string stawka, out int ztyp, out string nadgodziny, out string stawkaD, out string stawkaN, out int typ)
            {
                if (FindZmiana(zmiana) != null)
                    return GetZmiana(drZmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN, out typ);
                else
                {
                    czasZm = 0;
                    symbol = null;
                    stawka = null;
                    stawkaD = null;
                    stawkaN = null;
                    nadgodziny = null;
                    ztyp = 0;
                    typ = 0;
                    return false;
                }
            }

            public bool FindZmiana(string zmiana, out string symbol, out string nazwa, out string ztyp, out int czasZm, out int czasOd, out int czasDo)
            {
                if (FindZmiana(zmiana) != null)
                {
                    string zm = Base.getValue(drZmiana, "Id");
                    if (zm == zmiana)
                    {
                        czasZm = Base.getInt(drZmiana, "CzasZm", 0);
                        symbol = Base.getValue(drZmiana, "Symbol");
                        nazwa = Base.getValue(drZmiana, "Nazwa");
                        if (czasZm < 0) czasZm += 86400;
                        czasOd = Base.getInt(drZmiana, "Od", 0);
                        czasDo = Base.getInt(drZmiana, "Do", 0);
                        ztyp = Base.getValue(drZmiana, "TypZmiany");
                        return true;
                    }
                }
                czasZm = 0;
                symbol = null;
                nazwa = null;
                czasOd = 0;
                czasDo = 0;
                ztyp = null;
                return false;
            }

            private void _Add(ref OrderedDictionary sum, string stawka, int value)
            {
                int s = sum.Contains(stawka) ? (int)sum[stawka] : 0;
                sum[stawka] = s + value;
            }

            private void Add(ref OrderedDictionary sum, string stawka, int value, ref int t50, ref int t100)
            {
                int s = sum.Contains(stawka) ? (int)sum[stawka] : 0;
                sum[stawka] = s + value;
                switch (stawka)
                {
                    case "150":
                        t50 += value;
                        break;
                    case "200":
                        t100 += value;
                        break;
                    default:
                        break;
                }
            }

            // na potrzeby eksportu do asseco póki co
            /*
            public void SumTimes2(DateTime data, string zmiana, bool wolny, int _ztime, int otimeD, int otimeN, int ntime, out int t50, out int t100)
            {
                string stawka, stawkaD, stawkaN;
                string nadgodziny;
                int ztyp;
                string symbol;
                int czasZm;
                int otime = otimeD + otimeN;

                int zt0 = _ztime > 0 ? _ztime : 0;


                t50 = 0;
                t100 = 0;


                sumWTime += zt0 + otime;  // ztime moze byc =-1 co znaczy nie pokazuj
                //_sumWTime += _ztime + otime;  
                sumNTime += ntime;
                if (wolny) 
                    //sumHTime += _ztime + otime;
                    sumHTime += zt0 + otime;

                if (_ztime > 0 || otime > 0)
                    if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))
                    {
                        _Add(ref sumZmiany, symbol, czasZm);
                        if (_ztime > 0)                                  // suma czasu zmiany
                            Add(ref sumZTime, stawka, _ztime, ref t50, ref t100);
                        if (otime > 0)                                 // suma nadgodzin
                        {
                            switch (ztyp)
                            {
                                default:
                                case 0:
                                    string[] stawki = nadgodziny.Split(',');
                                    int cnt = stawki.Count();
                                    if (String.IsNullOrEmpty(nadgodziny) || cnt == 0)       // nie można okreslić nadgodzin - wszystko do jednego worka; cnt = 0 dodatkowe zabezpieczenie ale nie powinno miec miejsca bo split z '' zwraca 1 element!
                                    {
                                        //stawka = "?";                           // jeszcze raz wykorzystuje zmienną 
                                        stawka = Base.DateToStr(data);
                                        if (otime > 0)
                                            Add(ref sumOTime, stawka, otime, ref t50, ref t100);
                                    }
                                    else                                        // mam stawki
                                    {
                                        stawka = stawki[0];                     // ot moze wyjsc = 0 i wtedy musi wpasc do pierwszej stawki
                                        int ot = otime / 3600;
                                        for (int n = 0; n < ot; n++)            // !!! pozniej zoptymalizować !!!
                                        {
                                            if (n < cnt) stawka = stawki[n];
                                            Add(ref sumOTime, stawka, 3600, ref t50, ref t100);    // sumuje sekundy - czas wg zmian jest w godzinach;  // ostatnia stawka, w sumie powinien ją mieć ...
                                        }
                                        if (ot * 3600 < otime)                 // korekta na wypadek zaokrągleń nie do pełnej godziny, nie powinno mieć miejsca ...
                                        {
                                            Add(ref sumOTime, stawka, otime - ot * 3600, ref t50, ref t100);  // ostatnia stawka lub ?
                                        }
                                    }
                                    break;
                                case 1:
                                    //----- N1 testy --
                                    if (symbol == "N1" && data.DayOfWeek == DayOfWeek.Saturday && otimeD > 28800)
                                    {
                                        Add(ref sumOTime, stawkaD, 28800, ref t50, ref t100);
                                        Add(ref sumOTime, "150", otimeD - 28800, ref t50, ref t100);
                                    }
                                    else
                                    //---------------------
                                    if (otimeD > 0) Add(ref sumOTime, stawkaD, otimeD, ref t50, ref t100);                                    
                                    if (otimeN > 0) Add(ref sumOTime, stawkaN, otimeN, ref t50, ref t100);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //stawka = "?";
                        stawka = Base.DateToStr(data);
                        if (_ztime > 0)                              // suma czasu zmiany
                            _Add(ref sumZTime, stawka, _ztime);
                        if (otime > 0)                              // suma nadgodzin
                            _Add(ref sumOTime, stawka, otime);
                    }
                else                                                // nie ma czasów ale może być zmiana
                    if (!String.IsNullOrEmpty(zmiana))
                        if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))
                            _Add(ref sumZmiany, symbol, czasZm);
            }
                
             */














            public void _SumTimes2(DateTime data, string zmiana, bool _wolny, int _ztime, int otimeD, int otimeN, int _ntime,   // wolny tylko zeby do sumy za dni wolne weszło
                                  out int t50, out int t100,
                                  bool isAbs,       // jak absencja to nie liczę dodatku 100 before6  
                                  object czasIn,  
                                  //int before6
                                  out int d50, out int d100,  
                                  out bool zeroZm       // czy zerować ilość czasu pracy na zmianie - dla zmian typ = 1,2,3 S,N,N1 
                                  )                     // ilość czasu przepracowanego przed godziną 6 rano, -1 brak czasu wejścia - trzba wziąć ze zmiany
            {
                string stawka, stawkaD, stawkaN;
                string nadgodziny;
                int ztyp, typ;
                string symbol;
                int czasZm;
                int otime = otimeD + otimeN;

                int zt0 = _ztime > 0 ? _ztime : 0;  // ztime moze byc =-1 co znaczy nie pokazuj

                t50 = 0;
                t100 = 0;

                d50 = 0;
                d100 = 0;
                zeroZm = false;   

                sumWTime += zt0 + otime;        // łączny czas pracy
                _sumNTime += _ntime;              // czas pracy w nocy
                if (_wolny)
                    sumHTime += zt0 + otime;    // czas pracy w dni wolne

                if (_ztime > 0 || otime > 0)
                    if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN, out typ))
                    {
                        //zeroZm = typ == zmSobota || typ == zmNiedziela || typ == zmTylkoNadgN1;   // stawka != 100 ???

                        zeroZm = stawka != "100";

                        //----- zmiana S -----
                        if (stawka == "100")
                        //--------------------
                            _Add(ref sumZmiany, symbol, czasZm);        // ilości czasu nominalnego z podziałem na zmiany

                        if (_ztime > 0)                                 // suma czasu przepracowanego na zmianie, jeżeli inna stawka to dodaje do t50 i t100
                            Add(ref sumZTime, stawka, _ztime, ref t50, ref t100);

                        if (otime > 0)                                  // suma nadgodzin
                        {
                            switch (ztyp)
                            {
                                default:
                                case 0:
                                    string[] stawki = nadgodziny.Split(',');
                                    int cnt = stawki.Count();
                                    if (String.IsNullOrEmpty(nadgodziny) || cnt == 0)       // nie można okreslić nadgodzin - wszystko do jednego worka; cnt = 0 dodatkowe zabezpieczenie ale nie powinno miec miejsca bo split z '' zwraca 1 element!
                                    {
                                        //stawka = "?";                         // jeszcze raz wykorzystuje zmienną 
                                        stawka = Base.DateToStr(data);
                                        if (otime > 0)
                                            Add(ref sumOTime, stawka, otime, ref t50, ref t100);
                                    }
                                    else                                        // mam stawki
                                    {
                                        stawka = stawki[0];                     // ot moze wyjsc = 0 i wtedy musi wpasc do pierwszej stawki
                                        int ot = otime / 3600;
                                        for (int n = 0; n < ot; n++)            // !!! pozniej zoptymalizować !!!
                                        {
                                            if (n < cnt) stawka = stawki[n];
                                            Add(ref sumOTime, stawka, 3600, ref t50, ref t100);    // sumuje sekundy - czas wg zmian jest w godzinach;  // ostatnia stawka, w sumie powinien ją mieć ...
                                        }
                                        if (ot * 3600 < otime)                  // korekta na wypadek zaokrągleń nie do pełnej godziny, nie powinno mieć miejsca ... <<< a nie "reszta" z otime div 3600 ???
                                        {
                                            Add(ref sumOTime, stawka, otime - ot * 3600, ref t50, ref t100);  // ostatnia stawka lub ?
                                        }
                                    }
                                    break;
                                case 1:
                                    //----- N1 i testy -----
                                    DateTime n1data = DateTime.Parse("2012-10-21");
                                    //if (symbol == "N1" && data.DayOfWeek == DayOfWeek.Saturday && otimeD > 28800
                                    if (symbol == "N1" && data.DayOfWeek != DayOfWeek.Sunday && otimeD > 28800     // 20130131 zmieniam ze tylko niedziela jest jako 200%, pozostałe dni z sobotą włącznie jako 150 po 8h
                                        && data >= n1data   // testy jak by było w poprzednich mies., normalnie odremowane !!!
                                        )
                                    {
                                        Add(ref sumOTime, stawkaD, 28800, ref t50, ref t100);
                                        Add(ref sumOTime, "150", otimeD - 28800, ref t50, ref t100);
                                    }
                                    else /**/
                                        //---------------------
                                        if (otimeD > 0) Add(ref sumOTime, stawkaD, otimeD, ref t50, ref t100);

                                    if (otimeN > 0) Add(ref sumOTime, stawkaN, otimeN, ref t50, ref t100);
                                    break;
                            }
                        }

                        



                        //if (ztime > 0)
                        //{
                        //    //----- niedziela 5:00 ------
                        //    if (typ == zmPoniedzialek5 && data.DayOfWeek == DayOfWeek.Monday && before6 != 0)   // && czasIn < 6:00   S1N
                        //    {
                        //        if (before6 == -1)   // nie można wyznaczyć bo nie ma czasu wejścia - trzeba wziąć wg czsu zmiany  <<< już jest zrobione w GetBefore6 ale niech tu zostanie ...
                        //        {
                        //            const int t6 = 6 * 3600;
                        //            int od = db.getInt(drZmiana, "Od", t6);
                        //            before6 = t6 - od;   // różnica czasu wg zmiany, jak < 0 to następny warunek nie doda
                        //        }
                        //        if (before6 > 0)
                        //        {
                        //            if (before6 > ztime) before6 = ztime;
                        //            Add(ref sumOTime, stawkaN, before6, ref t50, ref t100);
                        //        }
                        //    }
                        //}



                        if (_ztime > 0 && !isAbs)    // w SolveWorktime może być przekazany isAbs = false, bo wykorzystywana jest przy akceptacji czasu pracy a tam jeszcze nie wiem czy jest absencja - ale wtedy ztime = 0 i nie zapisze sie do bazy d100, jak bedzie absencja a ztime bedzie jednak > 0 to sygnalizuje praca i absencja - wiec bedzie do wyjasnienia
                        {
                            //----- niedziela 5:00 ------
                            if (typ == zmPoniedzialek5 && data.DayOfWeek == DayOfWeek.Monday)   // && czasIn < 6:00   S1N
                            {
                                const int t6 = 6 * 3600;
                                int before6 = Worktime.GetBefore6(czasIn, _ztime, db.getInt(drZmiana, "Od", t6), db.getInt(drZmiana, "Margines", -1));
                                if (before6 > 0)
                                {
                                    //Add(ref sumOTime, stawkaN, before6, ref t50, ref t100);   nie zwiększam ilości nadgodzin tylko dodatek 100%
                                    d100 = before6;
                                    sumDod100 += d100;



                                    //if (before6 > 0) Add(ref sumZTime, "100", before6, ref t50, ref t100);  testy
                                



                                }
                            }
                        }


                    }
                    else                                            // nie znaleziono zmiany
                    {
                        //stawka = "?";
                        stawka = Base.DateToStr(data);
                        if (_ztime > 0)                             // suma czasu zmiany
                            _Add(ref sumZTime, stawka, _ztime);
                        if (otime > 0)                              // suma nadgodzin
                            _Add(ref sumOTime, stawka, otime);
                    }
                else                                                // nie ma czasów pracy ale może być zmiana
                    if (!String.IsNullOrEmpty(zmiana))
                        if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN, out typ))
                        {
                            zeroZm = stawka != "100";
                            //----- zmiana S -----
                            if (stawka == "100")
                            //--------------------
                                _Add(ref sumZmiany, symbol, czasZm);// ilości czasu nominalnego z podziałem na zmiany
                        }
            }
            //-------------
            private bool Add(string stawka, int czas, ref int t0, ref int t50, ref int t100)
            {
                switch (stawka)
                {
                    case "100":
                        t0 += czas;
                        break;
                    case "150":
                        t50 += czas;
                        break;
                    case "200":
                        t100 += czas;
                        break;
                    default:
                        return false;
                }
                return true;
            }


            /*
            public bool solveNadgodziny(DateTime data, int zm, int otimeD, int otimeN, out int t0, out int t50, out int t100)
            {
                int ztyp;
                string symbol;
                int zmCzas;
                string stawka;
                string nadgodziny, stawkaD, stawkaN;
                t0 = 0;
                t50 = 0;
                t100 = 0;
                int otimeDN = otimeD + otimeN;
                if (GetZmiana(drZmiana, out symbol, out zmCzas, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))    // jest zmiana
                {
                    Add(stawka, zm, ref t0, ref t50, ref t100);
                    if (otimeDN > 0)        // suma nadgodzin
                    {
                        switch (ztyp)
                        {
                            default:
                            case 0:
                                string[] stawki = nadgodziny.Split(',');
                                int cnt = stawki.Count();
                                if (String.IsNullOrEmpty(nadgodziny) || cnt == 0)       // nie można okreslić nadgodzin - wszystko do jednego worka; cnt = 0 dodatkowe zabezpieczenie ale nie powinno miec miejsca bo split z '' zwraca 1 element!
                                {
                                    //stawka = "?";                         // jeszcze raz wykorzystuje zmienną 
                                    stawka = Base.DateToStr(data);
                                    if (otimeDN > 0)
                                        Add(stawka, otimeDN, ref t0, ref t50, ref t100);
                                }
                                else                                        // mam stawki
                                {
                                    stawka = stawki[0];                     // ot moze wyjsc = 0 i wtedy musi wpasc do pierwszej stawki
                                    int ot = otimeDN / 3600;
                                    for (int n = 0; n < ot; n++)            // !!! pozniej zoptymalizować !!!
                                    {
                                        if (n < cnt) stawka = stawki[n];
                                        Add(stawka, 3600, ref t0, ref t50, ref t100);    // sumuje sekundy - czas wg zmian jest w godzinach;  // ostatnia stawka, w sumie powinien ją mieć ...
                                    }
                                    if (ot * 3600 < otimeDN)                // korekta na wypadek zaokrągleń nie do pełnej godziny, nie powinno mieć miejsca ... <<< a nie "reszta" z otime div 3600 ???
                                        Add(stawka, otimeDN - ot * 3600, ref t0, ref t50, ref t100);  // ostatnia stawka lub ?
                                }
                                break;
                            case 1:
                                //----- N1 i testy -----
                                DateTime n1data = DateTime.Parse("2012-10-21");
                                //if (symbol == "N1" && data.DayOfWeek == DayOfWeek.Saturday && otimeD > 28800       20130129 Sabina - N1 ma byc tylko w niedziele liczona jako 200+200, w pozostałe dni 200+150
                                if (symbol == "N1" && data.DayOfWeek != DayOfWeek.Sunday && otimeD > 28800        // <<< zastanowić sie nad zmiana na Id = 11 !!!
                                    && data >= n1data   // testy jak by było w poprzednich mies., normalnie odremowane !!!
                                    )
                                {
                                    Add(stawkaD, 28800, ref t0, ref t50, ref t100);
                                    Add("150", otimeD - 28800, ref t0, ref t50, ref t100);
                                }
                                else 
                                //---------------------
                                    if (otimeD > 0) Add(stawkaD, otimeD, ref t0, ref t50, ref t100);

                                if (otimeN > 0) Add(stawkaN, otimeN, ref t0, ref t50, ref t100);
                                break;
                        }
                    }
                    return true;
                }
                else                // brak zmiany 
                {
                    t0 = zm + otimeDN;
                    return false;
                }
            }
            */


            // na potrzeby Asseco 
            public bool solveNadgodziny2(DateTime data, int zm, ref int otimeDbase, int _otimeD, int _otimeN, out int t0, out int t50, out int t100)
            {
                int ztyp, typ;
                string symbol;
                int zmCzas;
                string stawka;
                string nadgodziny, stawkaD, stawkaN;
                t0 = 0;
                t50 = 0;
                t100 = 0;
                int _otimeDN = _otimeD + _otimeN;
                if (GetZmiana(drZmiana, out symbol, out zmCzas, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN, out typ))    // jest zmiana
                {
                    //zeroZm = stawka != "100";  // typ == 1,2,3  fukcja wykorzystywana tylko w 1 miejscu gdzie mam juz tą informację
                    Add(stawka, zm, ref t0, ref t50, ref t100);
                    if (_otimeDN > 0)        // suma nadgodzin
                    {
                        switch (ztyp)
                        {
                            default:
                            case 0:         // do weryfikacji na później nie ma takiego typu zmian teraz !!! otimeDbase 
                                string[] stawki = nadgodziny.Split(',');
                                int cnt = stawki.Count();
                                if (String.IsNullOrEmpty(nadgodziny) || cnt == 0)       // nie można okreslić nadgodzin - wszystko do jednego worka; cnt = 0 dodatkowe zabezpieczenie ale nie powinno miec miejsca bo split z '' zwraca 1 element!
                                {
                                    //stawka = "?";                         // jeszcze raz wykorzystuje zmienną 
                                    stawka = Base.DateToStr(data);
                                    if (_otimeDN > 0)
                                        Add(stawka, _otimeDN, ref t0, ref t50, ref t100);
                                }
                                else                                        // mam stawki
                                {
                                    stawka = stawki[0];                     // ot moze wyjsc = 0 i wtedy musi wpasc do pierwszej stawki
                                    int ot = _otimeDN / 3600;
                                    for (int n = 0; n < ot; n++)            // !!! pozniej zoptymalizować !!!
                                    {
                                        if (n < cnt) stawka = stawki[n];
                                        Add(stawka, 3600, ref t0, ref t50, ref t100);    // sumuje sekundy - czas wg zmian jest w godzinach;  // ostatnia stawka, w sumie powinien ją mieć ...
                                    }
                                    if (ot * 3600 < _otimeDN)                // korekta na wypadek zaokrągleń nie do pełnej godziny, nie powinno mieć miejsca ... <<< a nie "reszta" z otime div 3600 ???
                                        Add(stawka, _otimeDN - ot * 3600, ref t0, ref t50, ref t100);  // ostatnia stawka lub ?
                                }
                                break;
                            case 1:
                                //----- N1 i testy -----
                                DateTime n1data = DateTime.Parse("2012-10-21");
                                //if (symbol == "N1" && data.DayOfWeek == DayOfWeek.Saturday && otimeD > 28800       20130129 Sabina - N1 ma byc tylko w niedziele liczona jako 200+200, w pozostałe dni 200+150
                                
                                /*
                                if (symbol == "N1" && data.DayOfWeek != DayOfWeek.Sunday && _otimeD > 28800        // <<< zastanowić sie nad zmiana na Id = 11 !!!
                                    && data >= n1data   // testy jak by było w poprzednich mies., normalnie odremowane !!!
                                    )
                                {
                                    Add(stawkaD, 28800, ref t0, ref t50, ref t100);
                                    Add("150", _otimeD - 28800, ref t0, ref t50, ref t100);
                                }
                                else /**/
                                
                                //---------------------
                                if (symbol == "N1" && data.DayOfWeek != DayOfWeek.Sunday && otimeDbase + _otimeD > 28800        // <<< zastanowić sie nad zmiana na Id = 11 !!!
                                    && data >= n1data   // testy jak by było w poprzednich mies., normalnie odremowane !!!
                                    )
                                {
                                    int x100 = 28800 - otimeDbase;              // np 11 = 7 + 4
                                    int x50 = otimeDbase + _otimeD - 28800;
                                    if (x100 > 0) Add(stawkaD, x100, ref t0, ref t50, ref t100);
                                    if (x50 > 0) Add("150", x50, ref t0, ref t50, ref t100);
                                    otimeDbase += x100;   //20141205 zwiekszam tylko o ilość 100
                                }
                                else
                                    if (_otimeD > 0)
                                    {
                                        Add(stawkaD, _otimeD, ref t0, ref t50, ref t100);
                                        otimeDbase += _otimeD;
                                    }
                                    
                                if (_otimeN > 0) Add(stawkaN, _otimeN, ref t0, ref t50, ref t100);
                                break;
                        }
                    }
                    return true;
                }
                else                // brak zmiany 
                {
                    t0 = zm + _otimeDN;
                    return false;
                }
            }
            
            
            //------ ------
            /*
            public void x_SumTimes(DateTime data, string zmiana, bool wolny, int _ztime, int otimeD, int otimeN, int ntime)
            {
                string stawka, stawkaD, stawkaN;
                string nadgodziny;
                int ztyp;
                string symbol;
                int czasZm;
                int otime = otimeD + otimeN;

                int zt0 = _ztime > 0 ? _ztime : 0;

                sumWTime += zt0 + otime;  // ztime moze byc =-1 co znaczy nie pokazuj
                //_sumWTime += _ztime + otime;  
                sumNTime += ntime;
                if (wolny) 
                    //sumHTime += _ztime + otime;
                    sumHTime += zt0 + otime;


                if (_ztime > 0 || otime > 0)
                    if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))
                    {
                        //----- zmiana S -----
                        if (stawka == "100")
                        //-----------------
                            _Add(ref sumZmiany, symbol, czasZm);

                        if (_ztime > 0)                                // suma czasu zmiany
                            _Add(ref sumZTime, stawka, _ztime);

                        if (otime > 0)                                 // suma nadgodzin
                        {
                            switch (ztyp)
                            {
                                default:
                                case 0:
                                    string[] stawki = nadgodziny.Split(',');
                                    int cnt = stawki.Count();
                                    if (String.IsNullOrEmpty(nadgodziny) || cnt == 0)       // nie można okreslić nadgodzin - wszystko do jednego worka; cnt = 0 dodatkowe zabezpieczenie ale nie powinno miec miejsca bo split z '' zwraca 1 element!
                                    {
                                        //stawka = "?";                           // jeszcze raz wykorzystuje zmienną 
                                        stawka = Base.DateToStr(data);
                                        if (otime > 0)
                                            _Add(ref sumOTime, stawka, otime);
                                    }
                                    else                                        // mam stawki
                                    {
                                        stawka = stawki[0];                     // ot moze wyjsc = 0 i wtedy musi wpasc do pierwszej stawki
                                        int ot = otime / 3600;
                                        for (int n = 0; n < ot; n++)            // !!! pozniej zoptymalizować !!!
                                        {
                                            if (n < cnt) stawka = stawki[n];
                                            _Add(ref sumOTime, stawka, 3600);    // sumuje sekundy - czas wg zmian jest w godzinach;  // ostatnia stawka, w sumie powinien ją mieć ...
                                        }
                                        if (ot * 3600 < otime)                 // korekta na wypadek zaokrągleń nie do pełnej godziny, nie powinno mieć miejsca ...
                                        {
                                            _Add(ref sumOTime, stawka, otime - ot * 3600);  // ostatnia stawka lub ?
                                        }
                                    }
                                    break;
                                case 1:
                                    //----- N1 testy --
                                    DateTime n1data = DateTime.Parse("2012-10-21");
                                    //if (symbol == "N1" && data.DayOfWeek == DayOfWeek.Saturday && otimeD > 28800 
                                    if (symbol == "N1" && data.DayOfWeek != DayOfWeek.Sunday && otimeD > 28800        // <<< zastanowić sie nad zmiana na Id = 11 !!!
                                        && data >= n1data   // testy jak w poprzednich mies.
                                        )
                                    {
                                        _Add(ref sumOTime, stawkaD, 28800);
                                        _Add(ref sumOTime, "150", otimeD - 28800);
                                    }
                                    else 
                                    //---------------------
                                        if (otimeD > 0) _Add(ref sumOTime, stawkaD, otimeD);

                                    if (otimeN > 0) _Add(ref sumOTime, stawkaN, otimeN);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //stawka = "?";
                        stawka = Base.DateToStr(data);
                        if (_ztime > 0)                             // suma czasu zmiany
                            _Add(ref sumZTime, stawka, _ztime);
                        if (otime > 0)                              // suma nadgodzin
                            _Add(ref sumOTime, stawka, otime);
                    }
                else                                                // nie ma czasów ale może być zmiana
                    if (!String.IsNullOrEmpty(zmiana))
                        if (FindZmiana(zmiana, out symbol, out czasZm, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))
                            //----- zmiana S -----
                            if (stawka == "100")
                            //-----------------
                                _Add(ref sumZmiany, symbol, czasZm);
            }
            */ 



            /*
                public void SumTimes(int zmiana, int ztime, int otime, int ntime)
                {
                    sumNTime += ntime / 3600;
                    ztime = ztime / 3600;
                    otime = otime / 3600;
                    int sum;
                    string stawka;
                    string nadgodziny;
                    int ztyp, stawkaD, stawkaN;

                    if (ztime > 0 || otime > 0)
                        if (FindZmiana(zmiana, out stawka, out ztyp, out nadgodziny, out stawkaD, out stawkaN))
                        {
                            if (ztime > 0)                              // suma czasu zmiany
                            {
                                sum = sumZTime.Contains(stawka) ? (int)sumZTime[stawka] : 0;
                                sumZTime[stawka] = sum + ztime;
                            }
                            if (otime > 0)                              // suma nadgodzin
                            {
                                switch (ztyp)
                                {
                                    default:
                                    case 0:
                                        string[] stawki = nadgodziny.Split(',');
                                        int cnt = stawki.Count();
                                        stawka = "?";
                                        if (cnt == 0)                           // nie można okreslić nadgodzin - wszystko do jednego worka
                                        {
                                            sum = sumOTime.Contains(stawka) ? (int)sumOTime[stawka] : 0;
                                            sumOTime[stawka] = sum + otime;
                                        }
                                        else
                                            for (int n = 0; n < otime; n++)     // !!! pozniej zoptymalizować !!!
                                            {
                                                if (n < cnt) stawka = stawki[n];
                                                sum = sumOTime.Contains(stawka) ? (int)sumOTime[stawka] : 0;   // ostatnia stawka, w sumie powinien ją mieć ...
                                                sumOTime[stawka] = sum + 1;
                                            }
                                        break;
                                    case 1:
                                    
                                        break;
                                }
                            }
                        }
                        else
                        {
                            stawka = "?";
                            if (ztime > 0)                              // suma czasu zmiany
                            {
                                sum = sumZTime.Contains(stawka) ? (int)sumZTime[stawka] : 0;
                                sumZTime[stawka] = sum + ztime;
                            }
                            if (otime > 0)                              // suma nadgodzin
                            {
                                sum = sumOTime.Contains(stawka) ? (int)sumOTime[stawka] : 0;
                                sumOTime[stawka] = sum + otime;
                            }
                        }
                }
             */

            public int GetSum(OrderedDictionary sum)    // sumuje wszystko
            {
                int s = 0;
                foreach (DictionaryEntry de in sum)
                    s += (int)de.Value;
                return s;
            }

            public int GetSum(OrderedDictionary sum, string key, bool remove)    // sumuje wg klucza
            {
                int ret;
                if (sum.Contains(key))
                {
                    ret = (int)sum[key];
                    if (remove) 
                        sum.Remove(key);
                }
                else ret = 0;
                return ret;
            }
        }
        //----- pomocnicze do sumatora -------------------------
        const string br = "<br />";

        public string GetSumyHeader()
        {
            string hZ, hO, hN;
            //hZ = "Zm.<br />100%<br />150%";
            //hO = "100%<br />150%<br />200%";

            hZ = "Zm.<br />Czas<br />[h]";
            hO = "Nad.<br />50%<br />100%";
            hN = "Noc<br /><br />[h]";
            
            return hZ + "," + hO + "," + hN;
            /*
            DataSet ds = Base.getDataSet("select distinct Stawka from Zmiany order by Stawka");
            int cnt = Base.getCount(ds);
            
            DataSet ds = Base.getDataSet("select Stawka from Stawki where Aktywna=1 order by Stawka");
            int cnt = Base.getCount(ds);
            if (cnt > 0)
            {
                string[] ss = new string[cnt + 1];
                for (int i = 0; i < cnt; i++)
                {
                    DataRow dr = Base.getRows(ds)[i];
                    ss[i] = dr[0].ToString() + Tools.LineParamSeparator + "% [h]";
                }
                ss[cnt] = "Noc" + Tools.LineParamSeparator + "[h]";
                return String.Join(",", ss);    //"100|%,150|%,200|%"
            }
            else return null;
            */
        }

        public string _GetSuma(string tag, OrderedDictionary od, string key, int zaokrSum, int zaokrSumType)
        {
            string sum;
            if (od.Contains(key))
            {
                sum = String.Format("<{0} title=\"{1}\">{2}</{0}>",
                                     tag,
                                     Worktime.SecToTimePP((int)od[key], 1, zaokrSumType, false),
                                     Worktime.SecToTimePP((int)od[key], zaokrSum, zaokrSumType, true));
                od.Remove(key);
            }
            else sum = String.Format("<{0}>{1}</{0}>", tag, "-");
            return sum;
        }

        public string _GetSuma(string tag, OrderedDictionary od1, OrderedDictionary od2, string key, int zaokrSum, int zaokrSumType)
        {
            string sum;
            int s = 0;
            bool f1 = od1.Contains(key);
            bool f2 = od2.Contains(key);
            if (f1 || f2)
            {
                if (f1) s = (int)od1[key];
                if (f2) s += (int)od2[key];
                sum = String.Format("<{0} title=\"{1}\">{2}</{0}>",
                                     tag,
                                     Worktime.SecToTimePP(s, 1, zaokrSumType, false),
                                     Worktime.SecToTimePP(s, zaokrSum, zaokrSumType, true));
                if (f1) od1.Remove(key);
                if (f2) od2.Remove(key);
            }
            else sum = String.Format("<{0}>{1}</{0}>", tag, "-");
            return sum;
        }

        public string _GetSuma(string tag, int sum, int zaokrSum, int zaokrSumType)
        {
            if (sum == 0)
                return String.Format("<{0}>{1}</{0}>", tag, "-");
            else
                return String.Format("<{0} title=\"{1}\">{2}</{0}>",
                                     tag,
                                     Worktime.SecToTimePP(sum, 1, zaokrSumType, false),
                                     Worktime.SecToTimePP(sum, zaokrSum, zaokrSumType, true));
        }

        public string _GetSumaZmian(string tag, TimeSumator ts)        // nominalny z podziałem na zmiany
        {
            int sum = ts.GetSum(ts.sumZmiany);
            if (sum == 0)
                return String.Format("<{0}>{1}</{0}>", tag, "-");
            else
            {
                sum = sum / 3600;
                string hint = null;
                foreach (DictionaryEntry de in ts.sumZmiany)
                    if (String.IsNullOrEmpty(hint))
                        hint = String.Format("{0}: {1}",de.Key.ToString(), ((int)de.Value / 3600).ToString());
                    else
                        hint += 
                            //"&#10;" + 
                            "\n" +
                            String.Format("{0}: {1}", de.Key.ToString(), ((int)de.Value / 3600).ToString());
                return String.Format("<{0} title=\"{1}\">{2}</{0}>",
                                     tag,
                                     hint,
                                     sum.ToString());
            }
        }

        public string CoZostalo(int brCount, string tag, OrderedDictionary od, int zaokrSum, int zaokrSumType)
        {
            string sumy = null;
            string hint = null;
            int suma = 0;

            object[] keys = new object[od.Keys.Count]; 
            od.Keys.CopyTo(keys, 0); 

            for (int i = 0; i < od.Count; i++)
            {
                suma += (int)od[i];
                hint += "\n" + keys[i].ToString() + " ? " + Worktime.SecToTimePP((int)od[i], 1, zaokrSumType, true);  
            }
            if (suma > 0)
                sumy = Tools.StrRepeat(br, brCount) +
                       String.Format("<{0} title=\"{1}\">? {2}</{0}>",
                                     tag, 
                                     hint.Substring(1),
                                     Worktime.SecToTimePP(suma, zaokrSum, zaokrSumType, true));
            return sumy;
        }

        /*
        public string CoZostalo(int brCount, string tag, OrderedDictionary od, int zaokrSum, int zaokrSumType)
        {
            string sumy = null;
            string hint = null;
            int suma = 0;

            object[] keys = new object[od.Keys.Count]; 
            od.Keys.CopyTo(keys, 0); 

            for (int i = 0; i < od.Count; i++)
            {
                sumy += "," + Worktime.SecToTimePP((int)od[i], zaokrSum, zaokrSumType, true);  
                hint += "," + keys[i].ToString() + " ? " +
                        Worktime.SecToTimePP((int)od[i], 1, zaokrSumType, false);  
            }
            if (!String.IsNullOrEmpty(sumy))
            {
                sumy = Tools.StrRepeat(br, brCount) +
                       String.Format("<{0} title=\"{1}\">? {2}</{0}>",
                                     tag, hint.Substring(1), sumy.Substring(1));
            }
            return sumy;
        }
         */


        public string GetSumy(TimeSumator ts, int zaokrSum, int zaokrSumType)  // wypadłoby sie zakrecic po key i wziac rzeczywiste wartosci stawek
        {
            const string st0 = "h1"; //"st0";
            const string st1 = "h1"; //"st1";
            const string st2 = "h2"; //"st2";
            const string st3 = "h3"; //"st3";
            const string str = "h5"; //"str";

            const int n = 0;  //1;
            string sZm = _GetSumaZmian(st0, ts);                                             // zmiany są co do godziny !!!
            string sO = Tools.StrRepeat(br, n) +                                            //GetSuma(ts.sumOTime, "100", zaokrSum, zaokrSumType) + "<br />" + 
                        _GetSuma(st2, ts.sumOTime, ts.sumZTime, "150", zaokrSum, zaokrSumType) + br +
                        _GetSuma(st2, ts.sumOTime, ts.sumZTime, "200", zaokrSum, zaokrSumType) + 
                        CoZostalo(1 + n, str, ts.sumOTime, zaokrSum, zaokrSumType);
            string sZ = _GetSuma(st1, ts.sumZTime, "100", zaokrSum, zaokrSumType) +          //GetSuma(ts.sumZTime, "150", zaokrSum, zaokrSumType) + 
                        CoZostalo(2 + n, str, ts.sumZTime, zaokrSum, zaokrSumType);
            string sN = br + 
                        br +
                        _GetSuma(st3, ts._sumNTime, zaokrSum, zaokrSumType);
            return sZm + ";" + sZ + "|" + sO + "|" + sN;
        }


        /*
        public string GetSumy(TimeSumator ts, int zaokrSum, int zaokrSumType)  // wypadłoby sie zakrecic po key i wziac rzeczywiste wartosci stawek
        {
            const int n = 0;  //1;
            string sZm = GetSumaZmian("st0", ts);               // zmiany są co do godziny !!!
            string sZ = GetSuma("st1", ts.sumZTime, "100", zaokrSum, zaokrSumType) +        //GetSuma(ts.sumZTime, "150", zaokrSum, zaokrSumType) + 
                        CoZostalo(2 + n, "str", ts.sumZTime, zaokrSum, zaokrSumType);
            string sO = Tools.StrRepeat(br, n) +                                            //GetSuma(ts.sumOTime, "100", zaokrSum, zaokrSumType) + "<br />" + 
                        GetSuma("st2", ts.sumOTime, "150", zaokrSum, zaokrSumType) + br +
                        GetSuma("st2", ts.sumOTime, "200", zaokrSum, zaokrSumType) + 
                        CoZostalo(1 + n, "str", ts.sumOTime, zaokrSum, zaokrSumType);
            string sN = br + 
                        br +
                        GetSuma("st3", ts.sumNTime, zaokrSum, zaokrSumType);
            return sZm + ";" + sZ + "|" + sO + "|" + sN;
        }


          
        public string x_GetSumy1(TimeSumator ts)
        {
            string sumy = null;
            string sep = null;
            for (int i = 0; i < ts.sumZTime.Count; i++)
            {
                //head += ts.sumZTime.Keys.GetEnumerator[i];
                sumy += sep + ts.sumZTime[i];
                sep = ",";
            }

            for (int i = 0; i < ts.sumOTime.Count; i++)
            {
                //head += ts.sumZTime.Keys.GetEnumerator[i];
                sumy += sep + ts.sumOTime[i];
                sep = ",";
            }

            sumy += sep + ts.sumNTime.ToString();



            String[] headZ = new String[ts.sumZTime.Count];
            String[] headO = new String[ts.sumOTime.Count];
            ts.sumZTime.Keys.CopyTo(headZ, 0);
            ts.sumOTime.Keys.CopyTo(headO, 0);
            string hZ = String.Join(",", headZ);
            string hO = String.Join(",", headO);
            string head = hZ + "," + hO + ",Noc";

            return sumy;
        }


        public string x_GetSumy(TimeSumator ts)
        {
            String[] headZ = new String[ts.sumZTime.Count];
            String[] sumyZ = new String[ts.sumZTime.Count];
            String[] headO = new String[ts.sumOTime.Count];
            String[] sumyO = new String[ts.sumOTime.Count];
            ts.sumZTime.Keys.CopyTo(headZ, 0);
            ts.sumZTime.Values.CopyTo(sumyZ, 0);
            ts.sumOTime.Keys.CopyTo(headO, 0);
            ts.sumOTime.Values.CopyTo(sumyO, 0);

            string hZ = String.Join(",", headZ);
            string hO = String.Join(",", headO);
            string h = hZ + "," + hO + ",Noc";

            string sZ = String.Join(",", sumyZ);
            string sO = String.Join(",", sumyO);
            string s = sZ + "," + sO + "," + ts.sumNTime.ToString();

            return s;
        }
        */
        //-----------------------------------------------------------
        public static void SolveNadgodziny(ref TimeSumator ts, DateTime data, string zmianaId, 
            int ztime, int otimeD, int otimeN, int ntime, 
            out int n50, out int n100, 
            //int before6
            bool isAbs,         
            object czasIn,
            out int d50, out int d100,
            out bool zeroZm
            )
        {
            if (ts == null) ts = new cntPlanPracy2.TimeSumator();
            ts.Reset();
            ts._SumTimes2(data, zmianaId, false, ztime, otimeD, otimeN, ntime, out n50, out n100, isAbs, czasIn, out d50, out d100, out zeroZm);   //wolne nie musi byc przekazane
        }
        //-----------------------------------------------------------
        public string GetSumyHeaderPlanUrlopow()
        {
            string s1 = "mies<br />narast";
            return s1;
            //string s1 = "Suma<br />mies<br />narast";
            //string s2 = "Suma<br />od<br />01.01";
            //return s1 + "," + s2;
        }
        //-----------------------------------------------------------
        private void SetRozliczenieVisible(Control item, bool header, bool visible, bool edit)
        {
            if (header)
            {
                for (int i = 1; i <= 5; i++)
                    Tools.SetControlVisible(item, "thRT" + i.ToString(), visible);
                Tools.SetRowSpan(item, "thPracownik", visible ? 2 : 1);
                Tools.SetControlVisible(item, "thRB", visible);
                Tools.SetControlVisible(item, "trHeaderRozl2", visible);
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                    Tools.SetControlVisible(item, "tdR" + i.ToString(), visible);
                Tools.SetControlVisible(item, "tdRB", visible);
                if (visible)
                {
                    Tools.SetControlVisible(item, "btRozlEdit", edit);
                    Tools.SetControlVisible(item, "btRozlShow", !edit);
                }
            }
        }

        private void SetPlanUrlopowVisible(Control item, bool header, bool visible, bool edit, bool show)
        {
            if (header)
            {
                Tools.SetControlVisible(item, "thStanUrlopow", visible);
                Tools.SetRowSpan(item, "thPracownik", visible ? 2 : 1);
                Tools.SetRowSpan(item, "thColSelect", visible ? 2 : 1);
                Tools.SetRowSpan(item, "thRB", visible ? 2 : 1);
                Tools.SetControlVisible(item, "thRB", visible && (edit || show));
                Tools.SetControlVisible(item, "trHeaderStanUrlopow2", visible);
            }
            else
            {
                for (int i = 1; i <= 4; i++)
                    Tools.SetControlVisible(item, "tdR" + i.ToString(), visible);
                Tools.SetControlVisible(item, "tdRB", visible && (edit || show));
                if (visible)
                {
                    //Tools.SetControlVisible(item, "btUrlopEdit", edit);      // Szczegóły
                    //Tools.SetControlVisible(item, "btUrlopShow", show);

                    Tools.SetControlVisible(item, "btUrlopShow", show || edit);
                }
            }
        }

        private Label SetText(Control item, string name, string text, string hint)
        {
            Label lb = Tools.SetText(item, name, text);
            if (lb != null)
            {
                lb.ToolTip = hint;
                if (text == "0")
                    Tools.AddClass(lb, "gray");
            }
            return lb;
        }

        private string DoubleToStr(double value)
        {
            string dd = Worktime.Round05(value, 2).ToString().Replace(".", ",");
            if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);
            return dd;
        }

        private Label SetText(Control item, string name, double value, string hint)
        {
            string dd = DoubleToStr(value);
            return SetText(item, name, dd, hint);
        }

        private void SetRozliczenieValues(Control item, DataRowView drv)
        {
            SetText(item, "lbR1", drv["Niedomiar"].ToString(), drv["tNiedomiar"].ToString());
            SetText(item, "lbR2", drv["N50"].ToString(), drv["tN50"].ToString());
            SetText(item, "lbR3", drv["N100"].ToString(), drv["tN100"].ToString());

            SetText(item, "lbR4", drv["drNiedomiar"].ToString(), drv["tdrNiedomiar"].ToString());
            SetText(item, "lbR5", drv["dr50"].ToString(), drv["tdr50"].ToString());
            SetText(item, "lbR6", drv["dr100"].ToString(), drv["tdr100"].ToString());
                     
            SetText(item, "lbR7", drv["P50"].ToString(),    drv["tP50"].ToString());
            SetText(item, "lbR8", drv["P100"].ToString(),   drv["tP100"].ToString());
            SetText(item, "lbR9", drv["W50"].ToString(),    drv["tW50"].ToString());
            SetText(item, "lbR10", drv["W100"].ToString(),   drv["tW100"].ToString());
            SetText(item, "lbR11", drv["O50"].ToString(),    drv["tO50"].ToString());
            SetText(item, "lbR12", drv["O100"].ToString(),   drv["tO100"].ToString());
        }

        private void SetPlanUrlopowValues(Control item, cntPlanPracyLine2 pl, DataRowView drv)
        {
            //double wym = Tools.ToDouble(drv["UrlopNom"], 0);
            double wym = Tools.ToDouble(drv["UrlopNomRok"], 0);
            double zal = Tools.ToDouble(drv["UrlopZaleg"], 0);
            int dod = (int)drv["UrlopDod"];
            double plan = Tools.ToDouble(drv["Zaplanowany"], 0);
            double mies = Tools.ToDouble(drv["mies"], 0);
            double narast = Tools.ToDouble(drv["Narastajaco"], 0);
            double zost = wym + dod + zal - plan;

            string dd = Worktime.Round05(wym + dod + zal - plan, 2).ToString().Replace(".", ",");
            if (dd.EndsWith(".00")) dd = dd.Remove(dd.Length - 3);

            //SetText(item, "lbR1", wym, null;)
            SetText(item, "lbR1", wym.ToString() + (dod == 0 ? null : "<br />+" + dod.ToString()) , null);
            SetText(item, "lbR2", zal, null);
            SetText(item, "lbR3", plan, null);
            SetText(item, "lbR4", zost, null);

            pl._Sumy = String.Format("{0};{1}", 
                DoubleToStr(mies),
                DoubleToStr(narast)
                );
        }

        //-----------------------------------------------------------
        public string SelPracId = null;
        
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
                        IdKierownika = par;   //okres sie nie zmienie
                        PrepareDataSource();
                        DataBind();
                    }
                    break;
                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    break;
                case "ZERUJ":
                    string pracId = e.CommandArgument.ToString();
                    //string dates;
                    //if (Zeruj(DateFrom, DateTo, pracId, 0))  // zerowanie całych nadgodzin
                    if (_Zeruj(DateFrom, DateTo, pracId, 1))     // zerowanie minut w nadgodzinach
                    {
                        PrepareDataSource();
                        DataBind();
                        //Tools.ShowMessage("Zerowanie przeprowadzono dla następujących dni:");
                    }
                    else Tools.ShowMessage("Nie znaleziono dni, w których byłoby możliwe wyzerowanie nadgodzin.");
                    break;
                case "RozlEdit":
                    SelPracId = e.CommandArgument.ToString();
                    if (ShowRozliczenie != null)
                        ShowRozliczenie(this, EventArgs.Empty);
                    break;
                case "RozlShow":
                    SelPracId = e.CommandArgument.ToString();
                    if (ShowRozliczenie != null)
                        ShowRozliczenie(this, EventArgs.Empty);
                    break;
                case "UrlopEdit":
                    SelPracId = e.CommandArgument.ToString();
                    if (ShowPlanUrlopow != null)
                        ShowPlanUrlopow(this, EventArgs.Empty);
                    break;
                case "UrlopShow":
                    SelPracId = e.CommandArgument.ToString();
                    if (ShowPlanUrlopow != null)
                        ShowPlanUrlopow(this, EventArgs.Empty);
                    break;
            }
        }
        //-------------
        /*
        lvPlanPracy_LayoutCreated  // moze sie wykonać 3 razy jak wychodzę z Path w górę jak brak danych
        lvPlanPracy_DataBinding
            
        ,-> lvPlanPracy_ItemCreated 
        |   lvPlanPracy_ItemDataBound -,
        '------------------------------'

        lvPlanPracy_DataBound          
        lvPlanPracy_PreRender         
        lvPlanPracy_Unload          
         */
        //----- debug --------
        int layoutCreatedCnt = 0;
        //int bindCnt = 0;

        bool fPrzypisania = false;

        protected void lvPlanPracy_Load(object sender, EventArgs e)
        {
            //PrepareSql();  // tu ustawia przed załadowaniem przy np wyświetleniu strony, zmiana okresy z selectora wywołuje najpierw to ze starą datą, a potem selector woła BindData - dwa razy odwołuje sie zatem do okresu ale raz renderuje i odswieżam kontrolkę
        }

        protected void lvPlanPracy_Init(object sender, EventArgs e)
        {
            PrepareDataSource();
        }

        protected void lvPlanPracy_LayoutCreated(object sender, EventArgs e)
        {
            LineHeader = (cntPlanPracyLineHeader)lvPlanPracy.FindControl("PlanPracyLineHeader");
            if (LineHeader != null) 
            {
                LineHeader.Mode = FMode;
                if (true || layoutCreatedCnt == 0) //2)  //3 raz, tak się zachowuje jak wyświetlam brak danych i przechodzę do pokazania - nie wiem dlaczego ... ale jak tego nie ma to się gubi; wywołuje: create create binding bound create; i ten ostatni create musi poprawnie ustawić
                {
                    switch (FMode)  // mozna sprawdzic czy headerdata jest wypełnione...
                    {
                        default:
                        case moZmiany:
                            LineHeader.DataBind(hidFrom.Value, hidTo.Value);  // dla cnt=0 hidFrom.Value = null
                            LineHeader.SumHeader = null;
                            break;
                        case moAccept:
                            LineHeader.DataBind(hidFrom.Value, hidTo.Value, FDataAccOd, FDataAccDo, FDataBlockedTo);
                            LineHeader.SumHeader = GetSumyHeader();
                            break;
                        case moRozliczenie:
                            break;
                        case moPlanUrlopow:
                            //LineHeader.DataBind(hidFrom.Value, hidTo.Value);  // dla cnt=0 hidFrom.Value = null
                            //LineHeader.SumHeader = null;
                            break;
                    }
                    InitDataPager();
                }
            }
            if (layoutCreatedCnt == 0)
            {
                cntPrefix = btSelectCell.ClientID.Substring(0, btSelectCell.ClientID.Length - btSelectCell.ID.Length);
                cbDniAll = (CheckBox)lvPlanPracy.FindControl("cbDniAll");
                cbPracAll = (CheckBox)lvPlanPracy.FindControl("cbPracAll");
                InitDataPager();
            }
            else
            {
                int x = 0;
            }
            layoutCreatedCnt++;
        }


        bool planUrlopowEdit = false;
        bool planUrlopowShow = false;

        protected void lvPlanPracy_DataBinding(object sender, EventArgs e)  // nie wolac tu nic co spowoduje ponowne bindowanie !!!
        {
            kierCount = 0;
            fPrzypisania = WithPrzypisania();

            LineHeader = (cntPlanPracyLineHeader)lvPlanPracy.FindControl("PlanPracyLineHeader");
            if (LineHeader != null) LineHeader.Mode = FMode;

            if (LineHeader != null)
            {
                switch (FMode)
                {
                    default:
                    case moZmiany:
                        LineHeader.DataBind(hidFrom.Value, hidTo.Value);
                        LineHeader.SumHeader = null;
                        break;
                    case moAccept:
                        //----- parametry ------
                        //if (con == null) con = Base.Connect();
                        settings = Ustawienia.CreateOrGetSession();
                        KierParams kp = new KierParams(IdKierownika, settings);
                        FBreakMM = kp.Przerwa;
                        FBreak2MM = kp.Przerwa2;
                        FMarginMM = kp.Margines;
                        Fzaokr = settings.Zaokr;
                        FzaokrType = settings.ZaokrType;
                        kp.GetAccDates(out FDataAccOd, out FDataAccDo);
                        //----- data i sumator -----
                        Okres ok = new Okres(db.con, OkresId);
                        FDataBlockedTo = ok.LockedTo;
                        LineHeader.DataBind(hidFrom.Value, hidTo.Value, FDataAccOd, FDataAccDo, FDataBlockedTo);
                        LineHeader.SumHeader = GetSumyHeader();
                        sumator = new TimeSumator();
                        break;
                    case moRozliczenie:
                        break;
                    case moPlanUrlopow:
                        LineHeader.DataBind(hidFrom.Value, hidTo.Value);
                        LineHeader.SumHeader = GetSumyHeaderPlanUrlopow();

                        int status = PlanUrlopowStatus;
                        planUrlopowEdit = App.User.HasRight(AppUser.rPlanUrlopowEditPo) || status == HRRcp.Controls.cntPlanRoczny.stPlanowanie || status == HRRcp.Controls.cntPlanRoczny.stKorekta;
                        planUrlopowShow = !planUrlopowEdit && status != HRRcp.Controls.cntPlanRoczny.stNiedostepny;
                        break;
                }
            }
        }

        protected void lvPlanPracy_DataBound(object sender, EventArgs e)  // nie wolac tu nic co spowoduje ponowne bindowanie !!!
        {
            //bindCnt++;
            /*
            ImageButton ibt = (ImageButton)lvPlanPracy.FindControl("ibtSubItemsBack");
            if (ibt != null)
                ibt.Visible = !IsRoot() && !EditMode;    // wchodzi 2 razy nie znajdując kontrolki, za 3 znajduje
            */
            bool ibt = !IsRoot() && !EditMode;
            Tools.SetControlVisible(lvPlanPracy, "ibtSubItemsBack", ibt);

            cntPath.Enabled = !EditMode;
            HtmlTableCell td = (HtmlTableCell)lvPlanPracy.FindControl("thColSelect");
           
            switch (FMode)
            {
                default:
                case moZmiany:
                    if (td != null) td.Visible = EditMode || PracCheckbox;//EditMode; // juan
                   
                    CheckBox cb = lvPlanPracy.FindControl("cbDniAll") as CheckBox;
                    if (cb != null) cb.Visible = EditMode;

                    
                    //cbBlink.Visible = false;

                    Tools.SetText(lvPlanPracy, "lbCount", lvPlanPracy.Items.Count.ToString());  // nie ma pager'a
                    break;
                case moAccept:
                    if (td != null) td.Visible = false;
                    RefreshLetterPager();
                    break;
                case moRozliczenie:
                    if (td != null) td.Visible = false;
                    SetRozliczenieVisible(lvPlanPracy, true, true, !OkresClosed);
                    break;
                case moPlanUrlopow:
                    if (td != null) td.Visible = EditMode;

                    SetPlanUrlopowVisible(lvPlanPracy, true, true, planUrlopowEdit, planUrlopowShow);
                    break;
            }
            MakeDayClickable();
            //aaa SetDataPager();
        }

        protected void lvPlanPracy_PreRender(object sender, EventArgs e)    // dzięki temu nie znikają zaznaczenia krzyżowe
        {
            //lbDebug.Text = String.Format("{0} {1}",bindCnt,layoutCreatedCnt);

            switch (FMode)
            {
                default:
                case moZmiany:
                    //SetStatusVisible();
                        //SetPracCVisible(SubMode == smPlanAcc || PracCheckbox);
                    break;
                case moPlanUrlopow:
                    if (EditMode)
                    {
                        foreach (ListViewItem item in lvPlanPracy.Items)
                        {
                            cntPlanPracyLine2 ppl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                            CheckBox cb = (CheckBox)item.FindControl("cbPrac");
                            if (cb != null && ppl != null)
                                SetLineSelected(item, ppl, cb.Checked);
                        }
                    }
                    break;
                case moAccept:
                    break;
                case moRozliczenie:
                    break;
            }
        }


        //-------------------------------------------
        protected void lvPlanPracy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            cntPlanPracyLine2 ppl = (cntPlanPracyLine2)e.Item.FindControl("PlanPracyLine");
            if (ppl != null)
            {
                ppl.Mode = FMode;
                ppl.cntPrefix = cntPrefix;
                ppl.LineHeader = LineHeader;
            }
        }

        public string GetLineClass(object ja)
        {
            return "it" + (ja.ToString() == "1" ? " ja" : null);
        }

        protected void lvPlanPracy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                cntPlanPracyLine2 pl = (cntPlanPracyLine2)e.Item.FindControl("PlanPracyLine");
                if (pl != null)
                {
                    ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                    DataRowView drv = (DataRowView)dataItem.DataItem;
                    string pracId = drv["Id"].ToString();

                    bool _kier = (bool)drv["Kierownik"];
                    bool ja = Tools.GetInt(drv["Ja"], 0) == 1;


                    DateTime dataZatr = db.getDateTime(drv["DataZatr"], DateTime.MinValue);
                    DateTime dataZwol = db.getDateTime(drv["DataZwol"], DateTime.MaxValue);


                    //bool kier = !Base.isNull(drv["Kierownik2"]);
                    if (_kier) kierCount++;


                    string rcpId = null;
                    string strefaId = null;
                    //----- algorytm
                    string algPar = null;
                    string alg = drv["Algorytm"].ToString();
                    //if (String.IsNullOrEmpty(alg)) alg = "0";  //20130330 lepiej warunek poniżej
                    //bool pracOff = alg == "0" || String.IsNullOrEmpty(alg);
                    bool pracOff = alg == "0";

                    /* debug 
                    if (pracOff)
                    {
                        int x = 0;
                    }
                    /**/

                    //----- wygląd -----
                    /*
                    HtmlTableRow tr = e.Item.FindControl("trLine") as HtmlTableRow;
                    if (tr != null)
                    {
                        if (ja) Tools.AddClass(tr, "ja");
                    }
                    */

                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdPracName");
                    //if (td != null) td.Attributes["class"] += " mode" + FMode.ToString() + (ja ? " ja" : null);
                    if (td != null) td.Attributes["class"] += " mode" + FMode.ToString();
                    Label lb = (Label)e.Item.FindControl("PracownikLabel");
                    LinkButton lbt = (LinkButton)e.Item.FindControl("lbtPracownik");
                    bool zoom = _kier && !ja;
                    if (lb != null && lbt != null)
                    {
                        lb.Visible = !zoom;
                        lbt.Visible = zoom;
                        if (zoom && EditMode)
                        {
                            lbt.CommandName = null;
                            lbt.Attributes["class"] += " nolink";
                        }
                    }
                    //----- select prac -----
                    CheckBox cb = (CheckBox)e.Item.FindControl("cbPrac");
                    if (cb != null)
                        if (pracOff)
                            cb.Visible = false;
                        else
                            cb.Attributes["onclick"] = String.Format("javascript:cbPracClickPP(this,{0});", pracId);

                    //----- pobieranie danych ------
                    //if (con == null) con = Base.Connect();
                    DataSet ds;
                    switch (FMode)
                    {
                        default:
                        case moZmiany: 
                            HtmlGenericControl tdx = e.Item.FindControl("tdStatus") as HtmlGenericControl;
                            if (tdx != null)
                                tdx.Visible = StatusVisible;
                            if (SubMode == smPlanAcc)
                            {
                                string naglId = Tools.GetText(e.Item, "hidIdNaglowka");
                                ds = db.getDataSet(String.Format(dsSelectAcc.SelectCommand, pracId, hidFrom.Value, hidTo.Value, db.strParam(naglId)));
                            }
                            else
                            {
                                ds = db.getDataSet(String.Format(
                                /*"select D.Lp, D.Data, P.IdZmiany, P.Id as ppId, Z.*, R.Od as PrzOd, R.Do as PrzDo, R.IdKierownika as PrzKierId, R.DoMonit, R.Id as PrzId " +
                                "from GetDates2('{1}','{2}') D " +
    #if DBW
                                "left join rcpHarmonogram P on D.Data = P.Data and P.IdPracownika = {0} and P._do is null " +
    #else
                                "left join PlanPracy P on D.Data = P.Data and P.IdPracownika = {0} " +
    #endif
                                "left join Zmiany Z on Z.Id = P.IdZmiany " +
                                "left join Przypisania R on R.IdPracownika = {0} and R.Status = 1 and D.Data between R.Od and ISNULL(R.Do, '20990909')" +  //20140513, niezależnie od przełożonego !!!
                                "order by Data",*/ /* czemu tego nie bylo? albo to, albo niech to jest przypisywane jakos lepiej, a nie po kolei jak leci... */
                                                dsSelect.SelectCommand,
                                                pracId, hidFrom.Value, hidTo.Value, String.IsNullOrEmpty(Version) ? "GETDATE()" : db.strParam(Version)));

                            }
                            if (!EditMode || PracCheckbox)
                            {
                                td = (HtmlTableCell)e.Item.FindControl("tdColSelect");
                                if (td != null) td.Visible = PracCheckbox; //false; //juan
                            }
                            Tools.SetControlVisible(e.Item, "btZeruj", false);



                            //----- powielony nagłówek
                            //DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
                            int lineno = ((ListViewDataItem)e.Item).DisplayIndex;

                            const int repLinesZ = 30;
                            if (false && lineno >= repLinesZ) //wylaczamy 20160707
                            {
                                if ((lineno) % repLinesZ == 0)
                                {
                                    HtmlTableRow trH = e.Item.FindControl("trInHeader") as HtmlTableRow;
                                    cntPlanPracyInHeader cntH = e.Item.FindControl("cntInHeader") as cntPlanPracyInHeader;
                                    if (trH != null && cntH != null)
                                    {
                                        trH.Visible = true;
                                        cntH.DataBind(LineHeader, moZmiany, EditMode);
                                        if (EditMode) 
                                            Tools.SetColSpan(e.Item, "thPracownik", 2);
                                        bool ibt = !IsRoot() && !EditMode;
                                        Tools.SetControlVisible(e.Item, "ibtSubItemsBack", ibt);
                                    }
                                }
                            }
                            break;
                        case moAccept:
                            //----- zerowanie nadgodzin -----
                            bool oc = OkresClosed;
                            bool zb = !oc && !pracOff && !ja;
                            Button bt = (Button)Tools.SetControlVisible(e.Item, "btZeruj", zb);
                            if (zb && bt != null)
                                /*
                                Tools.MakeConfirmButton(bt, String.Format(
                                    "Potwierdź wyzerowanie nadgodzin dla pracownika:\\n{0}.\\n\\n" + 
                                    "Wyzerowane zostaną nadgodziny tylko w niezaakceptowanych dniach, o ile nie zostały wcześniej zmienione.\\n" +
                                    "Zerowanie zostanie przeprowadzone w całym okresie rozliczeniowym lub do dnia wczorajszego, jeżeli mieści się on w wybranym okresie.",
                                    drv["NazwiskoImie"].ToString()));
                                 */
                                Tools.MakeConfirmButton(bt, String.Format(
                                    "Potwierdź wyzerowanie minut nadgodzin dla pracownika:\\n{0}.\\n\\n" +
                                    "Wyzerowane zostaną minuty tylko w niezaakceptowanych dniach, o ile czasy nie zostały wcześniej wprowadzone.\\n" +
                                    "Zerowanie zostanie przeprowadzone w całym okresie rozliczeniowym lub do dnia wczorajszego, jeżeli mieści się on w wybranym okresie. Czas nocny zostanie automatycznie pomniejszony.",
                                    drv["NazwiskoImie"].ToString()));
                            //--------------------------------
                            rcpId = drv["RcpId"].ToString();
                            strefaId = drv["StrefaId"].ToString();

                            if (alg != "0" && alg != "1" && alg != "2" && alg != "11" && alg != "12" && !String.IsNullOrEmpty(alg))
                                //algPar = db.getScalar("select Parametr from Kody where Typ='ALG' and Kod=" + alg);  //tak chyba będzie optymalniej niż dołączać do query
                                algPar = App.GetAlgPar(alg);

                            //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";   jak null to nie zbiorę czasu i ok!
                            //if (String.IsNullOrEmpty(alg)) alg = "0";

                            //Ustawienia settings = Ustawienia.CreateOrGetSession();
                            
                            /* debug
                            if (pracId == "2213")
                            {
                                int x = 0;
                            }
                            /**/

                            Worktime2 wt2;
                            bool oc2 = Lic.kierAbs ? false : oc;
                            if (fPrzypisania)
                                ds = Worktime.GetWorktime(pracId, alg, algPar, rcpId, true, hidFrom.Value, hidTo.Value, null,
                                    strefaId,
                                    Fzaokr, FzaokrType,
                                    settings.NocneOdSec, settings.NocneDoSec,
                                    FBreakMM, FBreak2MM, null,
                                    0, /* wymiar */
                                    !oc2,
                                    null, null,
                                    false,
                                    out wt2);
                            else
                                ds = Worktime._GetWorktime(pracId, alg, algPar, rcpId, true, hidFrom.Value, hidTo.Value, null,
                                    strefaId,
                                    Fzaokr, FzaokrType,
                                    settings.NocneOdSec, settings.NocneDoSec,
                                    FBreakMM, FBreak2MM, null,
                                    0, /* wymiar */
                                    !oc2, out wt2);

                            td = (HtmlTableCell)e.Item.FindControl("tdColSelect");
                            if (td != null) td.Visible = PracCheckbox;//false; // juan
                            sumator.Reset();



                            //----- powielony nagłówek
                            //DataPager dp = (DataPager)lvPlanPracy.FindControl("DataPager1");
                            lineno = ((ListViewDataItem)e.Item).DisplayIndex;

                            const int repLinesA = 20;
                            if (lineno >= repLinesA)
                            {
                                if ((lineno) % repLinesA == 0)
                                {
                                    HtmlTableRow trH = e.Item.FindControl("trInHeader") as HtmlTableRow;

                                    cntPlanPracyInHeader cntH = e.Item.FindControl("cntInHeader") as cntPlanPracyInHeader;
                                    if (trH != null && cntH != null)
                                    {
                                        trH.Visible = true;
                                        cntH.DataBind(LineHeader, moAccept, false);

                                        bool ibt = !IsRoot() && !EditMode;
                                        Tools.SetControlVisible(e.Item, "ibtSubItemsBack", ibt);
                                    }
                                }
                            }
                            break;
                        case moRozliczenie:
                            ds = null;
                            Tools.SetControlVisible(e.Item, "tdColSelect", false);
                            Tools.SetControlVisible(e.Item, "btZeruj", false);
                            SetRozliczenieVisible(e.Item, false, true, !OkresClosed);
                            SetRozliczenieValues(e.Item, drv);
                            pl.Visible = false;
                            break;
                        case moPlanUrlopow:
                            ds = db.getDataSet(String.Format(@"
select 
    D.Data, P.KodUrlopu, P.Korekta, 
    AK.Symbol, AK.Nazwa, AK.KolorPU, AK.PokazSymbolPU, 
    --A.Kod as AbsKod, AK2.Symbol as AbsSymbol, AK2.Nazwa as AbsNazwa, AK2.Kolor as AbsKolor, AK2.WyborPU as AbsWyborPU
    
    --ISNULL(A.Kod, WT.IdKodyAbs) as AbsKod, 
    --ISNULL(AK2.Symbol, WT.Symbol) as AbsSymbol, 
    --ISNULL(AK2.Nazwa, WT.Typ) as AbsNazwa, 

    ISNULL(A.Kod, case when K.Rodzaj is null then WT.IdKodyAbs else null end) as AbsKod, 
    case when A.Kod is null and K.Rodzaj is null then WT.Symbol else AK2.Symbol end as AbsSymbol,
    case when A.Kod is null and K.Rodzaj is null then WT.Typ else AK2.Nazwa end as AbsNazwa,

    AK2.Kolor as AbsKolor, 
    AK2.WyborPU as AbsWyborPU,
    case when A.Kod is null and W.Id is not null then 1 else 0 end as Wniosek,
    W.Id as IdWniosku

    --,W.StatusId as StatusWniosku, WT.Typ as TypWniosku, WT.Symbol as SymbolWniosku, WT.IdKodyAbs
from GetDates2('{1}','{2}') D
left join Kalendarz K on K.Data = D.Data
--left join PlanUrlopow P on D.Data = P.Data and {3} >= P.Od and {3} < ISNULL(P.Do, '20990909') and P.IdPracownika = {0}
left join PlanUrlopow P on D.Data = P.Data and P.Do is null and P.KodUrlopu is not null and P.IdPracownika = {0}
left join poWnioskiUrlopowe W on D.Data between W.Od and W.Do and W.IdPracownika = {0} and W.StatusId in (3,4)  --bez odrzuconych i czekających na akceptację
left join poWnioskiUrlopoweTypy WT on WT.Id = W.TypId
left join AbsencjaKody AK on AK.Kod = P.KodUrlopu
left join Absencja A on A.IdPracownika = {0} and D.Data between A.DataOd and A.DataDo 
left join AbsencjaKody AK2 on AK2.Kod = A.Kod and (K.Rodzaj is null or AK2.DniWolne = 1)
                                ", pracId, hidFrom.Value, hidTo.Value, "GETDATE()"));
                            Tools.SetControlVisible(e.Item, "tdColSelect", EditMode);
                            Tools.SetControlVisible(e.Item, "btZeruj", false);
                            SetPlanUrlopowVisible(e.Item, false, true, planUrlopowEdit, planUrlopowShow);//!OkresClosed);
                            SetPlanUrlopowValues(e.Item, pl, drv);
                            pl.Visible = true;
                            break;
                    }



                    //----- PRZETWARZANIE DNI DLA PRACOWNIKA -----
                    if (FMode == moZmiany || FMode == moAccept || FMode == moPlanUrlopow)  // dla Rozliczenie nie
                    {
                        //----- wypełnianie informacji w wierszu kolejne dni -----
                        int cnt = ds.Tables[0].Rows.Count;
                        int hcnt = LineHeader.HeaderData.Count();
                        string[] zmiany = new string[hcnt];
                        DateTime today = DateTime.Today;

                        bool selLine = pracId == ClickPracId;
                        int selDay = ClickDayIndex;

                        DateTime prevDate = DateTime.MinValue;
                        int ii = 0;
                        for (int _idx = 0; _idx < cnt; _idx++)
                        {
                            if (ii >= hcnt)
                                break;                      // zabezpieczenie 1 przed przewinięciem zakresu

                            DataRow dr = Base.getDataRow(ds, _idx);
                            DateTime date = (DateTime)Base.getDateTime(dr, "Data");


                            /* debug 
                            if (pracId == "1105" && Tools.DateToStr(date) == "2013-11-28")
                            {
                                int x = 0;
                            }
                            /**/


                            if (prevDate == date)
                            {
                                //continue;             // zabezpieczenie 2 przed pojawieniem sie w GetWorktime zduplikowanych dat wynikajacych z nakładajacych sie absencji
                            }
                            else prevDate = date;

                            string info = LineHeader.HeaderData[ii];     // holiday, today

                            bool isHoliday = info.Contains(maHoliday);

                            string absencja = null;
                            switch (FMode)
                            {
                                default:
                                case moZmiany:
                                    string zid = Base.getValue(dr, "IdZmiany");
                                    DateTime? dtOd = Base.getDateTime(dr, "Od");
                                    DateTime? dtDo = Base.getDateTime(dr, "Do");
                                    string czasy;
                                    if (dtOd != null && dtDo != null)
                                    {
                                        DateTime tOd = (DateTime)dtOd;
                                        DateTime tDo = (DateTime)dtDo;
                                        if (tDo.Minute == 0)
                                            czasy = String.Format("{0}-{1}", tOd.Hour, tDo.Hour);
                                        else
                                            czasy = String.Format("{0}-{1}", tOd.ToString("H:mm"), tDo.ToString("H:mm"));
                                    }
                                    else czasy = null;


                                    //string czasy = dtOd != null && dtDo != null ? String.Format("{0}-{1}", ((DateTime)dtOd).Hour, ((DateTime)dtDo).Hour) : null;
                                    //----- pracownik nie liczony -----
                                    if (pracOff) info += maOff;
                                    //----- Przypisania ------
                                    string przid = db.getValue(dr, "PrzId");
                                    if (String.IsNullOrEmpty(przid))
                                        info += maBlockEdit;
                                    zmiany[ii] = Tools.SetLineParams(5, zid,
                                                                       Base.getValue(dr, "Symbol"),
                                                                       Base.getValue(dr, "Kolor"),
                                                                       info, czasy, null);  // nie zachowuje ppid
                                    break;
                                case moPlanUrlopow:
                                    string puid = db.getValue(dr, "KodUrlopu");
                                    bool korekta = db.getBool(dr, "Korekta", false);
                                    bool wniosek = db.getBool(dr, "Wniosek", false);

                                    //string czasy = dtOd != null && dtDo != null ? String.Format("{0}-{1}", ((DateTime)dtOd).Hour, ((DateTime)dtDo).Hour) : null;
                                    //----- pracownik nie liczony -----
                                    if (korekta) info += maUrlopKorekta;
                                    if (wniosek) info += _maAbsencjaKier;  // tak samo wyświetlana

                                    string symbol = db.getBool(dr, "PokazSymbolPU", false) ? db.getValue(dr, "Symbol") : null;
                                    //string kolor = db.getBool(dr, "AbsWyborPU", false) ? null : db.getValue(dr, "AbsKolor");      // kolor - jeżeli nie jest wybierana w PU  
                                    zmiany[ii] = Tools.SetLineParams(puid,                          // 0 id - kod
                                                                     symbol,                        // 1 s  - symbol 
                                                                     db.getValue(dr, "KolorPU"),    // 2 c  - kolor
                                                                     info,                          // 3 info
                                                                     null,                          // 4 hint 
                                                                     null,                          // 5 numer dnia ~ absencja w pp
                                                                     puid,                          // 6 dla znacznika modyfikacji
                                                                     db.getValue(dr, "AbsSymbol"),  // 7 absencja: symbol
                                                                     db.getValue(dr, "AbsKolor")    // 8 absencja: kolor
                                                                     //kolor                          // 8 absencja: kolor - jeżeli nie jest wybierana w PU 
                                                                     );  
                                    break;
                                case moAccept:
                                    int wtAlert = 0;
                                    int wtime, _ztime, otimeD, otimeN, _ntime;  // łączny, zmiana, nadgodziny, w tym nadgodziny w nocy, nocne
                                    int rzt, rnD, rnN, rN;
                                    //int before6;



                                    //----- SolveWorktime2 ------
                                    bool isWTime = Worktime.SolveWorktime2(dr, alg, algPar,     // tu mam wartości skorygowane przez kier już
                                        FBreakMM,
                                        FBreak2MM,
                                        FMarginMM,
                                        Fzaokr, FzaokrType,
                                        true,
                                        out wtime, out _ztime, out otimeD, out otimeN, out _ntime,
                                        out rzt, out rnD, out rnN, out rN,
                                        //out before6,
                                        date < today,
                                        ref wtAlert);
                                    
                                
                                
                                    //----- akceptacja -----
                                    bool isAcc = Base.getBool(dr, "Akceptacja", false);     //poprawnie zamknięty okres ma wszystko poakceptowane !!! więc nie trzeba sprawdzać - miga bo akceptacja z ręki wpisana ...
                                    //----- zmiana -----
                                    zid = Base.getValue(dr, "ZmianaId");             // biore podlinkowaną zmianę 
                                    bool isZmiana = !String.IsNullOrEmpty(zid);
                                    //----- absencja ------
                                    string absSymbol = Base.getValue(dr, "AbsencjaSymbol");
                                    bool isAbsencja = !String.IsNullOrEmpty(absSymbol);
                                    bool showAbsencja = false;
                                    if (isAbsencja)
                                    {
                                        string kpAbsencja = Base.getValue(dr, "AbsencjaKod");   // z KP
                                        if (String.IsNullOrEmpty(kpAbsencja))                   // absencja jest wpisana przez kier lub z wniosku
                                            info += _maAbsencjaKier;
                                        else
                                            info += maAbsencja;         // "u";
                                        string absNazwa = Tools.FirstUpper(Base.getValue(dr, "AbsencjaNazwa").ToLower());
                                        absencja = absSymbol + " - " + absNazwa;
                                        absencja = absencja.Replace(Tools.LineParamSeparator, ' '); // na wszelki wypadek
                                        showAbsencja = !isHoliday || Base.getBool(dr, "DniWolne", false) || isZmiana;
                                    }
                                    //----- poprawki - akceptacja -----
                                    object kTimeIn = dr["CzasIn"];
                                    object kTimeOut = dr["CzasOut"];
                                    object kCzasZm = dr["CzasZm"];          //TimeToIntSec(dr["CzasZm"]);
                                    object kNadgDzien = dr["NadgodzinyDzien"];  //TimeToIntSec(dr["Nadgodziny"]);
                                    object kNadgNoc = dr["NadgodzinyNoc"];  //TimeToIntSec(dr["Nadgodziny"]);
                                    object kNocne = dr["Nocne"];
                                    object kUwagi = dr["Uwagi"];
                                    object kZmiana = dr["IdZmianyKorekta"];
                                    object _kAbsencja = dr["AbsencjaKodKier"];
                                    object wAbsencja = dr["AbsencjaKodWniosek"];
                                    bool k_CzasIn = Base.getBool(dr, "k_CzasIn", false);
                                    bool k_CzasOut = Base.getBool(dr, "k_CzasOut", false);
                                    bool k_CzasZm = Base.getBool(dr, "k_CzasZm", false);
                                    bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
                                    bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
                                    bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
                                    bool isKZmiana = !Base.isNull(kZmiana);
                                    bool isInfo = !Base.isNull(kTimeIn) && k_CzasIn ||              // zmienione przez kierownika
                                                  !Base.isNull(kTimeOut) && k_CzasOut ||
                                                  !Base.isNull(kCzasZm) && k_CzasZm ||
                                                  !Base.isNull(kNadgDzien) && k_NadgDzien ||
                                                  !Base.isNull(kNadgNoc) && k_NadgNoc ||
                                                  !Base.isNull(kNocne) & k_Nocne ||
                                                  !Base.isNull(_kAbsencja) ||
                                                  //!Base.isNull(wAbsencja) ||   jak z wniosku to nie pokazuję info-dziubka
                                                  !Base.isNull(kUwagi)
                                        //|| !Base.isNull(kZmiana)                        // ze zmianą k też moze mrugac, dalej w warunkach to jest if(alert) ...
                                                  ;
                                    
                                
                                
                                    //----- przypisania -----
                                    string przKierId = fPrzypisania ? db.getValue(dr, "PrzKierId") : null;
                                    string przId = fPrzypisania ? db.getValue(dr, "PrzId") : null;


                                    //----- akceptacja, korekta(info) -----
                                    if (isInfo || isKZmiana) info += maInfo;        // "i";
                                    if (isAcc) info += maAccepted;                  // "a";




                                    if (FDataAccOd <= date && date <= FDataAccDo && !isAcc &&
                                       (!fPrzypisania ||
                                       //!String.IsNullOrEmpty(przId) && dataZatr <= date && date <= dataZwol) 
                                       !String.IsNullOrEmpty(przId) ||              // jest przypisanie
                                       dataZatr <= date && date <= dataZwol)        // jest zatrudniony 
                                       )                                            //20141128 jak nie ma przypisania i jest zwolniony to nie pokazuję obwoluty - pracownik zwolniony by wypadało jeszcze sprawdzić po datach
                                        info += maNoAcc;





                                    //----- alerty -----
                                    if (date < today) // tylko wczesniejsze i && miesiac nie zamknięty - spr. do 6 rano ...
                                    {
                                        /*
                                        bool alert = wtAlert != 0 ||
                                                isZmiana && !isWTime && !isAbsencja ||
                                                isWTime && !isZmiana ||
                                                isWTime && isAbsencja;
                                         */
                                        bool alert = wtAlert != 0;
                                        if (alert)
                                        {
                                            //info += isInfo || isAcc ? maAlert2 : maAlert;   // "A" lub "B" - jak info lub acc to nie mruga; jak tylko korekta zmiany to ma nadal mrugać !!!
                                            if (isInfo || isAcc || 
                                                fPrzypisania && przKierId != IdKierownika)    // 20140727 jak nie jest u mnie to nie mrugam!!!
                                                info += maAlert2;
                                            else
                                                info += maAlert;   // "A" lub "B" - jak info lub acc to nie mruga; jak tylko korekta zmiany to ma nadal mrugać !!!
                                        }
                                    }
                                    //----- zawartość komórki -----
                                    string wt;
                                    if (pracOff)        //pracownik nie liczony, nie sumuję jak nie liczony
                                    {
                                        info += maOff;
                                        wt = null;
                                    }
                                    else
                                    {
                                        wt = Worktime.WorktimeToPP(Fzaokr, FzaokrType, wtime, _ztime, otimeD, otimeN, _ntime);

                                        // póki co tu nie doliczam absencji do sum w PPACC
                                        //if (!String.IsNullOrEmpty(absKod) && ztime == 0) ztime = 8 * 3600;  // etat powinien !!!, liczę tylko jak absencja z KP

                                        int t50, t100;  // w danym dniu 
                                        int d50, d100;
                                        bool zeroZm;
                                        sumator._SumTimes2(date, zid, false, _ztime, otimeD, otimeN, _ntime, out t50, out t100, isAbsencja, kTimeIn, out d50, out d100, out zeroZm);  // wolne nie są mi tu potrzebne
                                    }
                                    //----- select day -----
                                    if (selLine && selDay == ii) info += _maSelected;

                                    //----- Przypisania ------
                                    if (fPrzypisania)
                                    {
                                        //string kid = db.getValue(dr, "PrzKierId");
                                        string kid = przKierId;
                                        DateTime dDo = db.getDateTime(dr, "PrzDo", DateTime.MaxValue);
                                        DateTime dMonit = db.getDateTime(dr, "DoMonit", DateTime.MaxValue);
                                        if (kid != IdKierownika)
                                        {
                                            info += maBlockEdit;
                                            if (dDo == DateTime.MaxValue && date > dMonit)
                                                info += maPrzesun;
                                        }
                                        else
                                            if (dDo == DateTime.MaxValue && date > dMonit)
                                                info += maPrzesun;
                                    }
                                    zmiany[ii] = Tools.SetLineParams(6, zid,          // podlinkowana (lub null jak nie ma)
                                                                       Base.getValue(dr, "Symbol"),
                                                                       Base.getValue(dr, "Kolor"),
                                                                       info, wt,
                                                                       showAbsencja ? absencja : null);
                                    break;
                                case moRozliczenie:
                                    break;
                            }
                            ii++;
                        }
                        pl.PracId = pracId;
                        pl.Zmiany = String.Join(",", zmiany);
                        switch (FMode)
                        {
                            case moAccept:
                                if (pracOff) pl._Sumy = "||";
                                else pl._Sumy = GetSumy(sumator, settings.ZaokrSum, settings.ZaokrSumType);
                                break;
                            case moPlanUrlopow: // ustawiane wczesniej
                                break;
                            default:
                                pl._Sumy = null;
                                break;
                        }
                    }
                }
            }
        }

        //----------
        protected void lvPlanPracy_Unload(object sender, EventArgs e)
        {
            //if (con != null) Base.Disconnect(con);
        }


        //-----------------------------------------------------------------
        private object TimeToIntSec(object t)
        {
            if (t == null || t.Equals(DBNull.Value)) return null;
            else
            {
                DateTime dt = (DateTime)t;
                int sec = (dt.Hour * 60 * 60) + (dt.Minute * 60) + dt.Second;
                return sec;
            }
        }

        public DateTime DateFromIdx(int idx)
        {
            //return DateTime.Parse(DateFrom).AddDays(idx);
            return dtFrom.AddDays(idx);
        }

        //----- update zmiany -------------------------------------------------
        public void Update()  // Plan Pracy - zmiany
        {
            string NowDate = DateTime.Now.ToString();
            if (FMode == moZmiany)  // na wszelki wypadek, mozna by sprawdzic jeszcze czy okres nie jest aby zamkniety, bo jezeli jestem w trakcie edycji, a hr zamknie okres to i tak nadpiszę !!!
            {
                AppUser user = AppUser.CreateOrGetSession();
                SqlTransaction tr = Base.Connect().BeginTransaction();
                foreach (ListViewItem item in lvPlanPracy.Items)
                {
                    string alg;
                    Tools.GetControlValue(item, "hidAlgorytm", out alg);    // moze być null
                    if (alg != "0")                                         // nie sprawdzam jak nie mam 
                    {
                        string pracId;
                        cntPlanPracyLine2 pl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                        if (pl != null)
                        {
                            if (Tools.GetControlValue(item, "hidPracId", out pracId))
                            {
                                List<string> zm = new List<string>();   // tu będą tylko istniejące zmiany
                                string[] zmiany = pl.Zmiany.Split(',');
                                for (int i = 0; i < zmiany.Count(); i++)
                                {
                                    string data = Base.DateToStr(DateFromIdx(i));
                                    //string p2, p3, p4, ppid, p6;
                                    //Tools.GetLineParams(zmiany[i], out zid, out p2, out p3, out p4, out ppid, out p6);
                                    string zid = Tools.GetLineParam(zmiany[i], 0);
                                    bool modified = true; // potem dać tak: klikniecie zmiany ustawia flagę w js (np p5) tu odczytuje
                                    if (modified)
                                    {
                                        string zmm = String.IsNullOrEmpty(zid) ? "-1" : zid;
                                        if (!Base.execSQL2tr(tr, String.Format(      // zmieniam odpowiedzialnego kierownika i jeśli nastąpiła zmiana, zeruję akceptację; jak okres zamknięty to nie mogę zmieniać planu pracy 
                                                dsUpdate.UpdateCommand,
                                                pracId, data, Base.nullParam(zid), user.OriginalId, "ISNULL(IdZmiany,-1)", zmm)))
                                        {
                                            if (!String.IsNullOrEmpty(zid))  // nie udało się zaktualizować, zakładam, ze nie ma wiec jak zid jest null to nie dodaje
                                                Base.insertSQL(tr, String.Format(dsUpdate.InsertCommand,
                                                        pracId, data, Base.nullParam(zid), user.OriginalId));
                                        }
                                        /* UWAGA TERA JA */
                                        /*db.Execute(dsUpdate, pracId, data, db.nullParam(zid), user.OriginalId);*/
                                        //Base.execSQL2tr(tr, String.Format(dsUpdate.SelectCommand, pracId, data, db.nullParam(zid), user.OriginalId, db.strParam(NowDate)));
                                    }
                                }
                                Base.execSQL2tr(tr, String.Format(dsUpdateKorekta.SelectCommand, pracId, db.strParam(hidFrom.Value), db.strParam(hidTo.Value), App.User.OriginalId));
                            }
                        }
                    }
                }
                Base.Disconnect(tr);
            }
        }

        /*
        public void Update()  // zmiany
        {
            SqlTransaction tr = Base.Connect().BeginTransaction();
            foreach (ListViewItem item in lvPlanPracy.Items)
            {
                string pracId;
                cntPlanPracyLine2 pl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                if (pl != null)
                {
                    if (Tools.GetControlValue(item, "hidPracId", out pracId))
                    {
                        List<string> zm = new List<string>();   // tu będą tylko istniejące zmiany
                        string[] zmiany = pl.Zmiany.Split(',');
                        for (int i = 0; i < zmiany.Count(); i++)
                        {
                            string zid = Tools.GetLineParam(zmiany[i], 0);
                            if (!String.IsNullOrEmpty(zid))
                                zm.Add(String.Format("select {0},DATEADD(DAY,{1},'{2}'),{3}", pracId, i, hidFrom.Value, zid)); 
                        }
                        Base.execSQL(tr, String.Format(
                            "delete from PlanPracy where IdPracownika = {0} and Data between '{1}' and '{2}'",
                                pracId, hidFrom.Value, hidTo.Value));
                        if (zm.Count > 0)
                            Base.execSQL(tr, "insert into PlanPracy (IdPracownika, Data, IdZmiany) " + 
                                String.Join(" union all ", zm.ToArray()));      // dodaję ciurkiem

                    }
                }
            }
            Base.Disconnect(tr);
        }
        */
        //----- weryfikacja -----
        public bool Check()
        {
#if SIEMENS
            const bool checkDoba = false;
#else
            const bool checkDoba = true;
#endif
            const string zmfmt = "Dzień: {0}, zmiana: {1} ({2}-{3})\\n";
            const string brfmt = "Przerwa: {0}h\\n";
            const int defPrzerwa11 = 11;
            const int defPrzerwa24 = 24;
            const int defPrzerwa35 = 35;
            const int s86400 = 86400;  // doba
            const int s3600 = 3600;    // godzina

            if (FMode == moZmiany)  // na wszelki wypadek, mozna by sprawdzic jeszcze czy okres nie jest aby zamkniety, bo jezeli jestem w trakcie edycji, a hr zamknie okres to i tak nadpiszę !!!
            {
                TimeSumator zz = new TimeSumator(); // do wyszukiwania zmian
                string msg = null;
                //int przerwaD = -1;
                //int przerwaW = -1;
                int _czasNomOkres = Worktime._GetIloscDniPrac(null, DateFrom, DateTo) * 8; // il dni w okresie a nie w mies <<< wg Kalendarza, tu ok

                //----- pracownicy -----
                foreach (ListViewItem item in lvPlanPracy.Items)    // bez paginatora !!!
                {
                    string alg;
                    Tools.GetControlValue(item, "hidAlgorytm", out alg);    // moze być null
                    if (alg != "0")                                         // nie sprawdzam jak nie mam 
                    {
                        string pracId;
                        cntPlanPracyLine2 pl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                        if (pl != null)
                        {
                            if (Tools.GetControlValue(item, "hidPracId", out pracId))
                            {
                                int czasNomPrac = Worktime.GetIloscDniPracPrac(pracId, DateFrom, DateTo) * 8; // il dni w okresie a nie w mies <<< wg Kalendarza, tu ok jak święto w weekend

                                List<string> zm = new List<string>();   // tu będą tylko istniejące zmiany
                                string[] zmiany = pl.Zmiany.Split(',');

                                int _daysCount = 0;
                                int przerwa7 = 0;
                                bool zmianaZmian = false;
                                int _zmCnt = 0;  //ilosc zaplanowanych zmian - dni pracujących - to nie to samo co czas !!! (bo moze byc zmiana "tylko nadgodziny" zaplanowana)
                                int _czas = 0;   //godz.
                                DateTime _prevData7 = DateFromIdx(0);
                                DateTime prevData = DateTime.MinValue;
                                string prevZm = null;
                                bool isPrevZmiana = false;
                                DateTime prevZmOd = DateTime.MinValue;
                                DateTime prevZmDo = DateTime.MinValue;
                                bool wasAbsencja = false;
                                //----- fix jak pierwszy dzień okresu jest wolny -----
                                int fix = 0;
                                bool isHoliday = Tools.GetLineParam(zmiany[0], 3).Contains(maHoliday);
                                if (isHoliday)
                                {
                                    bool nextHoliday = Tools.GetLineParam(zmiany[1], 3).Contains(maHoliday);
                                    if (!nextHoliday)
                                    {
                                        string symbol, nazwa, ztyp;
                                        int czasZm, czasOd, czasDo;
                                        string zid = Tools.GetLineParam(zmiany[0], 0);
                                        bool isZmiana = zz.FindZmiana(zid, out symbol, out nazwa, out ztyp, out czasZm, out czasOd, out czasDo);
                                        if (isZmiana)   // potrzebne do poprawnej kontroli czasu nominalnego
                                        {
                                            DateTime data = DateFromIdx(0);
                                            _zmCnt++;
                                            DateTime zmOd = data.AddSeconds(czasOd);
                                            DateTime zmDo = data.AddSeconds(czasDo + (czasDo < czasOd ? s86400 : 0));
                                            int zt = Worktime.CountTime((DateTime)zmOd, (DateTime)zmDo);  //sek
                                            _czas += zt / 3600;  //godziny, zmiany zawsze są w godzinach                                        
                                            _daysCount++;
                                        }
                                        fix = 1;
                                    }
                                }
                                //----- kolejne dni -----
                                //for (int _i = 0; _i < zmiany.Count(); _i++)
                                for (int _i = fix; _i < zmiany.Count(); _i++)
                                    {
                                    DateTime data = DateFromIdx(_i);
                                    //----- reset parametrów przed pierwszym dniem tygodnia rozliczeniowego -----
                                    if ((_i - fix) % 7 == 0)
                                    {
                                        _daysCount = 0;
                                        _prevData7 = data;
                                        przerwa7 = 0;
                                        zmianaZmian = false;
                                    }
                                    //----- ustawienie patrametrów -----
                                    string zid = Tools.GetLineParam(zmiany[_i], 0);
                                    string symbol, nazwa, ztyp;
                                    int czasZm, czasOd, czasDo;
                                    DateTime zmOd = DateTime.MinValue;
                                    DateTime zmDo = DateTime.MinValue;
                                    bool isZmiana = zz.FindZmiana(zid, out symbol, out nazwa, out ztyp, out czasZm, out czasOd, out czasDo);
                                    bool isAbsencja = ztyp == App.zmAbsencja;
                                    if (isAbsencja) wasAbsencja = true;

                                    if (isZmiana && !isAbsencja)
                                    {
                                        _zmCnt++;
                                        zmOd = data.AddSeconds(czasOd);
                                        zmDo = data.AddSeconds(czasDo + (czasDo < czasOd ? s86400 : 0));
                                        int zt = Worktime.CountTime((DateTime)zmOd, (DateTime)zmDo);  //sek
                                        _czas += zt / 3600;  //godziny, zmiany zawsze są w godzinach                                        
                                        _daysCount++;
                                    }
                                    //----- 11h -----
                                    if (isZmiana && !isAbsencja)
                                    {
                                        if (prevZmDo != DateTime.MinValue)
                                        {
                                            int sec = Worktime.CountDateTimeSec(prevZmDo, zmOd);
                                            if (sec < defPrzerwa11 * s3600)
                                            {
                                                string prev = String.Format(zmfmt, Base.DateToStr(prevData), prevZm, prevZmOd.Hour, prevZmDo.Hour);
                                                string prz = String.Format(brfmt, Worktime.SecToHour(sec));
                                                string curr = String.Format(zmfmt, Base.DateToStr(data), symbol, Worktime.SecToHour(czasOd), Worktime.SecToHour(czasDo));
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      prev +
                                                      curr +
                                                      prz + "\\n" +
                                                      String.Format("Przerwa dobowa krótsza niż {0}h.", defPrzerwa11);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                        }
                                    }
                                    //----- 35h lub 24h -----
                                    int p;
                                    if (isZmiana && !isAbsencja)
                                    {
                                        if (isPrevZmiana && symbol != prevZm) zmianaZmian = true;
                                        if (przerwa7 == 0 || !isPrevZmiana) // String.IsNullOrEmpty(prevZm))  // reset co 7 dni ???
                                            p = Worktime.CountDateTimeSec(_prevData7, zmOd);
                                        else
                                            p = Worktime.CountDateTimeSec(prevZmDo, zmOd);
                                        if (p > przerwa7) przerwa7 = p;
                                    }
                                    if (((_i - fix) + 1) % 7 == 0 && !isAbsencja)   // ostatni dzień z 7
                                    {
                                        if (!isZmiana)      // nie ma zmiany 
                                        {
                                            if (przerwa7 == 0 || !isPrevZmiana) // String.IsNullOrEmpty(prevZm))  // do końca dnia
                                                p = Worktime.CountDateTimeSec(_prevData7, data.AddDays(1));  // -1 sek ???
                                            else
                                                p = Worktime.CountDateTimeSec(prevZmDo, data.AddDays(1));
                                            if (p > przerwa7) przerwa7 = p;
                                        }
                                        if (zmianaZmian)
                                        {
                                            if (przerwa7 < defPrzerwa24 * s3600)
                                            {
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      String.Format("Okres: {0} - {1} (7 dni)\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data)) +
                                                      String.Format("Przerwa tygodniowa krótsza niż {0}h (jest zmiana zmiany).", defPrzerwa24);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                        }
                                        else
                                            if (przerwa7 < defPrzerwa35 * s3600)
                                            {
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      String.Format("Okres: {0} - {1} (7 dni)\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data)) +
                                                      String.Format("Przerwa tygodniowa krótsza niż {0}h (zmiana zmiany nie występuje).", defPrzerwa35);
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                    }
                                    //----- 5 dni na 7-----
                                    if (((_i - fix) + 1) % 7 == 0 && !isAbsencja)   // ostatni dzień z 7   !!!! tu sprawdzić z fix czy nie psuje
                                    {
                                        //if (daysCount > 5)
                                        if (_daysCount > 6)
                                        {
                                            msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                  String.Format("Okres: {0} - {1} (7 dni)\\nZaplanowana ilość dni pracy: {2}\\n\\n", Base.DateToStr(_prevData7), Base.DateToStr(data), _daysCount) +
                                                  //"Przekroczono dopuszczalny czas pracy (5 dni).";
                                                  "Przekroczono dopuszczalny maksymalny czas pracy (6 dni).";
                                            Tools.ShowMessage(msg);
                                            return false;       // >>>>>>>>>>>>>>>
                                        }
                                    }
                                    //----- wolne za 6 dni ciurkiem -----

                                    //----- naruszenie doby pracowniczej (next day nie mozna zacząć wcześniej) ----- 
                                    if (checkDoba)
                                        if (isZmiana && prevZmDo != DateTime.MinValue && !isAbsencja)      // jest zmiana i zmiana wczesniejsza
                                        {
                                            int sec1 = Worktime.CountDateTimeSec(prevZmOd, zmOd);
                                            if (sec1 < s86400)   // nie może zacząć wcześniej niż doba upłynie
                                            {
                                                string prev = String.Format(zmfmt, Base.DateToStr(prevData), prevZm, prevZmOd.Hour, prevZmDo.Hour);
                                                string curr = String.Format(zmfmt, Base.DateToStr(data), symbol, Worktime.SecToHour(czasOd), Worktime.SecToHour(czasDo));
                                                msg = "Pracownik: " + Worker.GetNazwiskoImie(pracId) + "\\n" +
                                                      prev +
                                                      curr + "\\n" +
                                                      "Naruszenie doby pracowniczej.";
                                                Tools.ShowMessage(msg);
                                                return false;       // >>>>>>>>>>>>>>>
                                            }
                                        }
                                    //----- dzień wolny za niedzielę -----

                                    //----- ustawienia parametrów -----
                                    if (isZmiana && !isAbsencja)
                                    {
                                        isPrevZmiana = true;
                                        prevData = data;
                                        prevZm = symbol;
                                        prevZmOd = zmOd;
                                        prevZmDo = zmDo;
                                    }
                                    //----- error info -----
                                    if (!String.IsNullOrEmpty(msg))
                                    {
                                        Tools.ShowMessage(msg);
                                        return false;           //>>>>>>>>>>>>>>>>
                                    }
                                }
                                //----- czas nominalny -----    // UWAGA - póki co nie sprawdzam tych nie wklikanych 
                                if (_zmCnt > 0 && _czas != czasNomPrac && !wasAbsencja)    // tu wyłapię też takich, dla których K nie ustawiał zmian
                                {
                                    Tools.ShowMessage(
                                          "Pracownik: {0}\\n" +
                                          "Czas zaplanowany ({1}h) nie zgadza się z czasem nominalnym ({2}h).",
                                          Worker.GetNazwiskoImie(pracId), _czas, czasNomPrac);
                                    return false;       // >>>>>>>>>>>>>>> 
                                }
                            }
                        }
                    }
                }
            }
            return true;  // wszystko ok
        }

        //--------------------------------------------------------------------
        /*
        public void x_ZerujNadgodziny(string dateFrom, string dateTo, string pracId)  // beardzo uproszczona wersja, moze sie do czegos przyda
        {
            AppUser user = AppUser.CreateOrGetSession();

            DateTime dt1 = dtFrom;  // DateTime.Parse(dateFrom);
            DateTime dt2 = dtTo;    // DateTime.Parse(dateTo);
            DateTime toDay = DateTime.Today.AddDays(-1);
            if (dt1 <= toDay && toDay < dt2) dt2 = toDay;

            if (con == null) con = Base.Connect();
            DataSet ds = Base.getDataSet(con, String.Format(
                "select D.Data, P.Id as PPId, P.* " + 
                "from dbo.GetDates2(@d1, @d2) D " +                 
                "left outer join PlanPracy P on P.Data = D.Data and P.IdPracownika = {0}",
                pracId));

            int cnt = 0;
            foreach (DataRow dr in Base.getRows(ds))
            {
                bool acc = Base.getBool(dr, "Akceptacja", false);
                bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
                bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
                bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
                bool exists = !Base.isNull(dr, "PPId");
                if (!exists || (!acc && !k_NadgDzien && !k_NadgNoc && !k_Nocne)) // nie zaakceptowane i nie ma wprowadzonego żadnego z parametrów, jak nie ma PP to wszystko false !
                {
                    DateTime day = (DateTime)Base.getDateTime(dr, "Data"); // musi być 
                    int nocne = 0;
                    if (exists)
                    {
                        //nocne = Base.getInt(dr, "", 0);                        
                    }
                    UpdatePPKier(con, pracId, day, dr, null, null, null, 0, 0, nocne, user.Id);
                    cnt++;
                }
            }
            if (cnt == 0)
                Tools.ShowMessage("Nie znaleziono dni, w których byłoby możliwe wyzerowanie nadgodzin.");
        }
        */

        public void _UpdatePPKier(string pracId, DateTime date, DataRow dr,
                             DateTime? czasIn, DateTime? czasOut, int? zt, int? otD, int? otN, int? nt,
                             string accKierId)
        {
            bool exists = !Base.isNull(dr, "PPId");
            if (!exists)
            {
                //----- insert -----
                string fields = null;
                string values = null;
                if (czasIn != null)
                {
                    fields += "CzasIn,k_CzasIn,";
                    values += Base.insertStrParam(Base.DateTimeToStr(czasIn)) +
                              Base.insertParam(Base.bTRUE);
                }
                if (czasOut != null)
                {
                    fields += "CzasOut,k_CzasOut,";
                    values += Base.insertStrParam(Base.DateTimeToStr(czasOut)) +
                              Base.insertParam(Base.bTRUE);
                }
                if (zt != null)
                {
                    fields += "CzasZm,k_CzasZm,";
                    values += Base.insertParam(zt.ToString()) +
                              Base.insertParam(Base.bTRUE);
                }
                if (otD != null)
                {
                    fields += "NadgodzinyDzien,k_NadgodzinyDzien,";
                    values += Base.insertParam(otD.ToString()) +
                              Base.insertParam(Base.bTRUE);
                }
                if (otN != null)
                {
                    fields += "NadgodzinyNoc,k_NadgodzinyNoc,";
                    values += Base.insertParam(otN.ToString()) +
                              Base.insertParam(Base.bTRUE);
                }
                if (nt != null)
                {
                    fields += "Nocne,k_Nocne,";
                    values += Base.insertParam(nt.ToString()) +
                              Base.insertParam(Base.bTRUE);
                }
                db.execSQL("insert into PlanPracy (" +
                              fields + "IdPracownika,Data,IdKierownikaAcc,DataAcc) values (" +
                              values +
                                Base.insertParam(pracId) +
                                Base.insertStrParam(Base.DateToStr(date)) +
                                Base.insertParam(accKierId) +
                                Base.insertParamLast("GETDATE()") +
                                ")");
            }
            else
            {
                //----- update -----                
                string set = null;
                if (czasIn != null) set += 
                    Base.updateDateParam("CzasIn", Base.DateTimeToStr(czasIn)) + 
                    Base.updateParam("k_CzasIn", Base.bTRUE);
                if (czasOut != null) set += 
                    Base.updateDateParam("CzasOut", Base.DateTimeToStr(czasOut)) + 
                    Base.updateParam("k_CzasOut", Base.bTRUE);
                if (zt != null) set += 
                    Base.updateParam("CzasZm", zt) + 
                    Base.updateParam("k_CzasZm", Base.bTRUE);
                if (otD != null) set += 
                    Base.updateParam("NadgodzinyDzien", otD) + 
                    Base.updateParam("k_NadgodzinyDzien", Base.bTRUE);
                if (otN != null) set += 
                    Base.updateParam("NadgodzinyNoc", otN) + 
                    Base.updateParam("k_NadgodzinyNoc", Base.bTRUE);
                if (nt != null) set += 
                    Base.updateParam("Nocne", nt) + 
                    Base.updateParam("k_Nocne", Base.bTRUE);
                db.execSQL("update PlanPracy set " + set +
                                    Base.updateParam("IdKierownikaAcc", accKierId) +
                                    Base.updateParamLast("DataAcc", "GETDATE()") +
                                "where IdPracownika = " + pracId + " and Data = " + Base.strParam(Base.DateToStr(date)));
            }
        }        







        //tylko te dni kiedy jest kierownikiem pracownika
        public bool _Zeruj(string dateFrom, string dateTo, string pracId, int coZmienic)  // coZmienic na razie nie wykorzystywane
        {
            AppUser user = AppUser.CreateOrGetSession();
            DateTime toDay = DateTime.Today.AddDays(-1);
            //----- dane pracownika -----------------
            //if (con == null) con = Base.Connect();
            DataRow pdr = Base.getRow(Worker._GetPracInfo1(db.con, 1, null, null, pracId, false)); // musi być 
            //----- rcp, strefa, algorytm -----------
            string rcpId = Base.getValue(pdr, "RcpId");
            string strefaId = Base.getValue(pdr, "StrefaId");    // !!! po zrobieniu historii zmian stref, dać null; strefy będą przypięte do danych ... albo jakos tak
            //if (String.IsNullOrEmpty(strefaId)) strefaId = "0";
            //if (String.IsNullOrEmpty(alg)) alg = "0";
            string alg = Base.getValue(pdr, "Algorytm");      // !!! j.w.
            string algPar = App.GetAlgPar(alg);  //tak chyba będzie optymalniej niż dołączać do query
            //----- parametry kierownika ------------
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            KierParams kp = new KierParams(Base.getValue(pdr, "IdKierownika"), settings);
            //----- pobieranie danych ---------------
            Worktime2 wt2;
            DataSet ds = Worktime._GetWorktime(pracId, alg, algPar, rcpId, true, hidFrom.Value, hidTo.Value, null, 
                            strefaId,
                            settings.Zaokr, settings.ZaokrType,
                            settings.NocneOdSec, settings.NocneDoSec,
                            kp.Przerwa, kp.Przerwa2, null,
                            0, /* wymiar */
                            true, out wt2);
            int cnt = 0;
            string nadmList = null;
            foreach (DataRow dr in Base.getRows(ds))
            {
                DateTime day = (DateTime)Base.getDateTime(dr, "Data");  // musi być 
                if (day <= toDay)
                {
                    bool exists = !Base.isNull(dr, "PPId");
                    bool isZmiana = !Base.isNull(dr, "ZmianaId");       // w SolveWorktime spr jeszcze czy sa czasy, ale powinno tak wystarczyć
                    bool acc = Base.getBool(dr, "Akceptacja", false);
                    if (exists && isZmiana && !acc)                     // jezeli nie ma PP to nie ma też zmiany więc nie można policzyc nadgodzin 
                    {
                        bool k_NadgDzien = Base.getBool(dr, "k_NadgodzinyDzien", false);
                        bool k_NadgNoc = Base.getBool(dr, "k_NadgodzinyNoc", false);
                        bool k_Nocne = Base.getBool(dr, "k_Nocne", false);
                        if (!k_NadgDzien && !k_NadgNoc && !k_Nocne)     // nie ma wprowadzonego żadnego z parametrów
                        {
                            int wtime, _ztime, otimeD, otimeN, _ntime;
                            int rzt, rnD, rnN, rN;
                            //int before6;
                            int wtAlert = 0;
                            bool isWTime = Worktime.SolveWorktime2(dr, 
                                
                                alg, algPar,
                                
                                kp.Przerwa, kp.Przerwa2, kp.Margines,
                                settings.Zaokr, settings.ZaokrType,
                                false,      // wartości rzeczywiste
                                out wtime, out _ztime, out otimeD, out otimeN, out _ntime,  // dane rzeczywiste (z RCP)
                                out rzt, out rnD, out rnN, out rN,      // dane z RCP
                                //out before6,                            // czas przeg godz 6 rano liczony od wejścia - SIEMENS
                                false, ref wtAlert);                    // w tym momencie alerty mnie nie interesują
                            if (isWTime)                                // jak nie ma czasu pracy to nie ma sensu zerować
                            {
                                switch (coZmienic)
                                {
                                    case 0:     //zerowanie ilości nadgodzin i usuwanie nadgodzin z czasu nocnego !!! uwaga na pracę w soboty i święto!!! bo to chyba nie zrobione...
                                        int nocne = _ntime - otimeN;
                                        if (nocne < 0) nocne = 0;               // zabezpieczenie, nie powinno miec miejsca
                                        _UpdatePPKier(pracId, day, dr, null, null, null, 0, 0, nocne, user.OriginalId);  // taka uwaga - nigdy nie tworzę tu nowego rekordu...
                                        break;
                                    case 1:     //zerowanie ilości minut nadgodzin i usuwanie nadgodzin z czasu nocnego !!!
                                        if (otimeD > 0 || otimeN > 0)  // ustawiam tylko jak są
                                        {
                                            int v1 = otimeD;
                                            int v2 = otimeN;
                                            int v3 = _ntime;
                                            otimeD = Worktime.RoundSec(otimeD, settings.ZaokrSum, Ustawienia.zaokrDown);
                                            int otN = Worktime.RoundSec(otimeN, settings.ZaokrSum, Ustawienia.zaokrDown);
                                            nocne = _ntime - otimeN + otN;          //20120526 zdejmuje o ile wyzerowałem
                                            otimeN = otN;
                                            if (nocne < 0) nocne = 0;               // zabezpieczenie, nie powinno miec miejsca
                                            _UpdatePPKier(pracId, day, dr, null, null, null, otimeD, otimeN, nocne, user.OriginalId);  // taka uwaga - nigdy nie tworzę tu nowego rekordu bo zmieniam tylko w dni zaplanowane kiedy jest zmiana bo tylko tam mogą być nadgodziny...
                                            nadmList += String.Format("{0} ND: {1} -> {2} NN: {3} -> {4} noc: {5} -> {6}\n",
                                                Tools.DateToStr(day),
                                                Worktime.SecToTime(v1, 1), Worktime.SecToTime(otimeD, 1),
                                                Worktime.SecToTime(v2, 1), Worktime.SecToTime(otimeN, 1),
                                                Worktime.SecToTime(v3, 1), Worktime.SecToTime(nocne, 1));
                                        }
                                        else
                                        {
                                            nadmList += String.Format("{0} brak zerowania, WT: {1}\n", 
                                                Tools.DateToStr(day), 
                                                Worktime.SecToTime(wtime, 1));
                                        }
                                        break;
                                }                                
                                cnt++;
                            }
                        }
                    }
                }
            }
            if (cnt > 0)
            {
                string prac = AppUser.GetNazwiskoImieNREW(pracId);
                Log.Info(Log.ZERUJNADM, String.Format("Zerowanie nadminut: {0} {1} {2} {3} {4}", prac, dateFrom, DateTo, pracId, coZmienic), nadmList);
            }
            return cnt > 0;
        }

        //---------------
        private void SetKierIdSql()
        {
            if (WithPrzypisania())
            {
                hidKId.Value = IdKierownika;
                return;
            }

            const string nodata = "-1";

            string kid = IdKierownika;
            if (String.IsNullOrEmpty(kid))
            {
                hidKId.Value = null;
                hidKIdOkresy.Value = null;
            }
            else           
                if (IsArch)    // dane z archiwum
                {
                    hidKId.Value = nodata;
                    hidKIdOkresy.Value = kid;
                }
                else               
                {
                    hidKId.Value = kid;
                    hidKIdOkresy.Value = nodata;
                }
        }

        //--------------------------------------------------------------------
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public int Adm
        {
            get { return FAdm; }
            set 
            { 
                FAdm = value;
                hidAll.Value = value.ToString();
            }
        }

        public bool EditMode
        {
            get { return hidEditMode.Value == "E"; }
            set
            {
                hidEditMode.Value = value ? "E" : "Q";
                //lvPlanPracy.DataBind();
                PrepareDataSource();
                DataBind();
            }
        }

        public ListView List
        {
            get { return lvPlanPracy; }
        }
         
        public string Zmiana
        {
            set { hidSelZmiana.Value = value; }
            get { return hidSelZmiana.Value; }
        }
        //-----
        public string IdKierownika
        {
            get { return hidKierId.Value; }
            set 
            { 
                hidKierId.Value = value;
                SetKierIdSql();
            }
        }


        //-----
        public string OkresId
        {
            get { return hidOkresId.Value; }
            set 
            { 
                hidOkresId.Value = value;
                SetKierIdSql();
            }
        }

        public int OkresStatus
        {
            get { return Tools.GetViewStateInt(ViewState[ID + "_okstat"], -1); }
            set { ViewState[ID + "_okstat"] = value; }
        }

        public int PlanUrlopowStatus
        {
            get { return Tools.GetInt(ViewState[ID + "_pustat"], HRRcp.Controls.cntPlanRoczny.stNiedostepny); }
            set { ViewState[ID + "_pustat"] = value; }
        }

        public bool IsArch
        {
            get { return Tools.GetViewStateBool(ViewState[ID + "_okarch"], false); }
            set { ViewState[ID + "_okarch"] = value; }
        }

        public bool OkresClosed
        {
            get { return Tools.GetViewStateBool(ViewState[ID + "_closed"], false); }
            set { ViewState[ID + "_closed"] = value; }
        }
        //-----
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

        public DateTime dtFrom
        {
            get { return DateTime.Parse(DateFrom); }
            set { DateFrom = Base.DateToStr(value); }
        }

        public DateTime dtTo
        {
            get { return DateTime.Parse(DateTo); }
            set { DateTo = Base.DateToStr(value); }
        }
        //------------
        /*
        public string[] HeaderData          // zeby PPline wiedział co to za dzień
        {
            get { return hidHeaderData.Value.Split(','); }
        }

        public string HeaderDataValue
        {
            set { hidHeaderData.Value = value; }
            get { return hidHeaderData.Value; }
        }
        */ 
        //------------
        public string ClickPracId
        {
            get { return hidClickPracId.Value; }
            //set { hidSelPracId.Value = value; }
        }

        public int ClickDayIndex
        {
            get { return Tools.StrToInt(hidClickDayIndex.Value, -1); }
        }

        public string PrevClickPracId
        {
            get { return Tools.GetViewStateStr(ViewState["PrevClickPracId"]); }
            set { ViewState["PrevClickPracId"] = value; }
        }

        public int PrevClickDayIndex
        {
            get { return Tools.GetViewStateInt(ViewState["PrevClickDayIndex"], -1); }
            set { ViewState["PrevClickDayIndex"] = value; }
        }

        public DateTime ClickDate
        {
            get 
            {
                int idx = ClickDayIndex;
                if (idx >= 0)
                    return DateFromIdx(idx);
                else
                    return dtFrom;
            }
        }

        public string RootId
        {
            set { hidRootId.Value = value; }
            get { return hidRootId.Value; }
        }

        public string ZastId
        {
            set { hidZastId.Value = value; }
            get { return hidZastId.Value; }
        }

        public string Zakres    // 0 - moi pracownicy/kierownika udostępnionego, 1 - mój czas pracy, 2 - pracownicy udostępnieni
        {
            set { hidZakres.Value = value; }
            get { return hidZakres.Value; }
        }

        public string PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        public string Search
        {
            get { return hidSearch.Value; }
            set { hidSearch.Value = value; }
        }

        [DefaultValue(0)]
        public int SubMode
        {
            get;
            set;
        }

        #region 20160220

        public bool GetEmpDays(ref string emp, ref string days)
        {
            bool daySelected = LineHeader.IsAnySelected();
            bool pracSelected = IsAnyPracSelected();

            if (!pracSelected)
                return false;

            bool first = true;

            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                string pracId = Tools.GetText(item, "hidPracId");
                CheckBox cbP = (CheckBox)item.FindControl("cbPrac");
                if (cbP != null && cbP.Checked || !pracSelected)   // zaznaczeni lub wszyscy
                {
                    emp += "," + pracId;
                    cntPlanPracyLine2 ppl = (cntPlanPracyLine2)item.FindControl("PlanPracyLine");
                    if (ppl != null)
                    {
                        string[] zmiany = ppl.Zmiany.Split(',');
                        for (int i = 0; i < ppl.Repeater.Items.Count; i++)
                        {
                            if (first)
                            {
                                RepeaterItem head = LineHeader.Repeater.Items[i];
                                CheckBox cbD = (CheckBox)head.FindControl("cbDay");
                                if (cbD != null && cbD.Checked)
                                {
                                    string date = Base.DateToStr(DateFromIdx(i));
                                    days += "," + date;
                                }
                            }

                            //if (daySelected && pracSelected)
                            //{
                            //    //RepeaterItem cell = ppl.Repeater.Items[i];
                            //    RepeaterItem head = LineHeader.Repeater.Items[i];
                            //    CheckBox cbD = (CheckBox)head.FindControl("cbDay");
                            //    if (cbD != null && cbD.Checked)
                            //        UpdateZmiana(ref zmiany[i], Zmiana, false, true);  // wszystkie
                            //}
                            //else if (pracSelected)
                            //{
                            //    UpdateZmiana(ref zmiany[i], Zmiana, false, false);  // wszystkie z wyłaczeniem świąt
                            //}
                            //else   // daySelected
                            //{
                            //    RepeaterItem head = LineHeader.Repeater.Items[i];
                            //    CheckBox cbD = (CheckBox)head.FindControl("cbDay");
                            //    if (cbD != null && cbD.Checked)
                            //        UpdateZmiana(ref zmiany[i], Zmiana, false, true);  // wszystkie
                            //}
                        }
                        //cbP.Checked = false;
                        //SetLineSelected(item, ppl, false);
                        ppl.Zmiany = String.Join(",", zmiany);
                        ppl.DataBind();
                        first = false;
                    }
                }
            }
            //if (daySelected) LineHeader.SelectAll(false);
            //cbDniAll.Checked = false;
            //cbPracAll.Checked = false;
            return true;
        }

        public string[] GetList(string hidId)
        {
            List<String> list = new List<String>();
            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                string id = Tools.GetText(item, hidId);
                list.Add(id);
            }
            return list.ToArray();
        }

        public string[] GetEmployees()
        {
            return GetList("hidPracId");
        }

        public string[] GetHeaders()
        {
            return GetList("hidIdNaglowka");
        }

        public string[] GetCheckedHeaders()
        {
            List<String> list = new List<String>();
            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                string id = Tools.GetText(item, "hidIdNaglowka");
                CheckBox cb = (CheckBox)item.FindControl("cbPrac");
                if (cb != null)
                    if (cb.Checked)
                        list.Add(id);
                    
            }
            return list.ToArray();
        }

        public string[] GetCheckedEmployees(bool allIfNone)
        {
            List<string> list = new List<string>();
            List<string> listAll = new List<string>();

            foreach (ListViewItem item in lvPlanPracy.Items)  //pracownicy
            {
                string id = Tools.GetText(item, "hidPracId");
                listAll.Add(id);
                CheckBox cb = (CheckBox)item.FindControl("cbPrac");
                if (cb != null)
                    if (cb.Checked)
                        list.Add(id);

            }
            return (list.Count > 0 || !allIfNone) ? list.ToArray() : listAll.ToArray();
        }

        public bool IsBothSelected()
        {
            bool daySelected = LineHeader.IsAnySelected();
            bool pracSelected = IsAnyPracSelected();
            return daySelected && pracSelected;
        }

        public String StatusFilter
        {
            get { return hidStatusFilter.Value; }
            set { hidStatusFilter.Value = value; }
        }

        public String Version
        {
            get { return hidVersion.Value; }
            set { hidVersion.Value = value; }
        }

        public Boolean PracCheckbox
        {
            get { return Tools.GetViewStateBool(ViewState["vPracCheckbox"], false); }
            set { ViewState["vPracCheckbox"] = value; }
        }

        public Boolean StatusVisible
        {
            get { return Tools.GetViewStateBool(ViewState["vStatusVisible"], false); }
            set { ViewState["vStatusVisible"] = value; }
        }

        void SetStatusVisible()
        {
            foreach (ListViewItem item in lvPlanPracy.Items)
            {
                HtmlGenericControl td = item.FindControl("tdStatus") as HtmlGenericControl;
                if (td != null)
                    td.Visible = StatusVisible;
            }
            //LineHeader.CheckVisible = b;
        }

#endregion

        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        public String Commodity
        {
            get { return hidCommodity.Value; }
            set { hidCommodity.Value = value; }
        }


    }
}





/*
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.Kierownik, --D.Nazwa as Dzial,
                        ISNULL(RcpStrefaId, case when P.Kierownik=1 then D.KierStrefaId else D.PracStrefaId end) as StrefaId,
                        ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm
                   FROM Pracownicy P 
                   LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu
                   WHERE P.IdKierownika = @IdKierownika and (P.Status >= 0 or P.Kierownik = 1)
                   ORDER BY P.Kierownik desc, Nazwisko, Imie" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidKierId" Name="IdKierownika" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:HRConnectionString %>" 
    SelectCommand="SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie as NazwiskoImie, P.Kierownik, 
                        RcpStrefaId as StrefaId,
                        RcpAlgorytm as Algorytm
                   FROM PracownicyOkresy P 
                   WHERE P.IdOkresu = @IdOkresu and P.IdKierownika = @IdKierownika and (P.Status >= 0 or P.Kierownik = 1)
                   ORDER BY P.Kierownik desc, Nazwisko, Imie" >
    <SelectParameters>
        <asp:ControlParameter ControlID="hidOkresId" Name="IdOkresu" PropertyName="Value" Type="String" />
        <asp:ControlParameter ControlID="hidKierId" Name="IdKierownika" PropertyName="Value" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>

 */




/*
        
private bool PrepareSql()   // true jak zmiana sql
{
    //----- lista pracowników ------
    string kid = IdKierownika;
    if (String.IsNullOrEmpty(kid))
    {
        hidKId.Value = null;
        hidOkresId.Value = null;
        return true;
    }
    else
    {
        okres = new Okres(dtFrom);  // pozniej przekazac z góry ...
        hidOkresId.Value = okres.Id.ToString();
        switch (okres.Status)
        {
            case Okres.stClosed:
                hidKId.Value = IdKierownika;
                return true;
            default:
            case Okres.stNotExists:
            case Okres.stOpen:
                hidKId.Value = "-" + IdKierownika;
                return false;       // nie zmianiam sql
        }
    }
}



private bool PrepareSql2()   // true jak zmiana sql
{
    //----- lista pracowników ------
    okres = new Okres(dtFrom);  // pozniej przekazac z góry ...
    hidOkresId.Value = okres.Id.ToString();
    switch (okres.Status)
    {
        case Okres.stClosed:
            lvPlanPracy.DataSourceID = "SqlDataSource2";
            return true;
        default:
        case Okres.stNotExists:
        case Okres.stOpen:
            lvPlanPracy.DataSourceID = "SqlDataSource1";
            return false;       // nie zmianiam sql
    }
}




private bool PrepareSql1()   // true jak zmiana sql
{
    //----- lista pracowników ------
    string kierId = IdKierownika;
    if (String.IsNullOrEmpty(kierId))
    {
        //SqlDataSource1.SelectCommand = null;
    }
    else
    {
        okres = new Okres(dtFrom);  // pozniej przekazac z góry ...
        switch (okres.Status)
        {
            case Okres.stClosed: SqlDataSource1.SelectCommand =
                "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie + 'zzz' as NazwiskoImie, P.Kierownik, " + // D.Nazwa as Dzial, " +
                    "RcpStrefaId as StrefaId, " +
                    "RcpAlgorytm as Algorytm " +

                            
                    //",case when P.Kierownik=1 " +
                    //    "then IdKierownika " +
                    //    "else null " +
                    //"end as Kierownik2 " +
 
                "FROM PracownicyOkresy P " +
                    //"LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu " +
                "WHERE P.IdOkresu = " + okres.Id + " and P.IdKierownika = " + IdKierownika + " and (P.Status >= 0 or P.Kierownik = 1) " +
                "ORDER BY P.Kierownik desc, NazwiskoImie";
                break;
            default:
            case Okres.stNotExists:
            case Okres.stOpen: 
                /*                        
                SqlDataSource1.SelectCommand =
                "SELECT P.Id, P.RcpId, P.Nazwisko + ' ' + P.Imie + 'xxx' as NazwiskoImie, P.Kierownik," + // --D.Nazwa as Dzial,
                    "ISNULL(RcpStrefaId, case when P.Kierownik=1 then D.KierStrefaId else D.PracStrefaId end) as StrefaId," +
                    "ISNULL(RcpAlgorytm, case when P.Kierownik=1 then D.KierAlgorytm else D.PracAlgorytm end) as Algorytm " +
                    //",case when P.Kierownik=1 " +
                    //    "then (select top 1 P2.Id from Pracownicy P2 where P2.IdKierownika = P.Id and P2.Status >= 0) " +
                    //    "else null " +
                    //"end as Kierownik2 " +
                "FROM Pracownicy P " +
                "LEFT OUTER JOIN Dzialy D ON D.Id = P.IdDzialu " +
                "WHERE P.IdKierownika = " + IdKierownika + " and (P.Status >= 0 or P.Kierownik = 1) " +
                "ORDER BY P.Kierownik desc, NazwiskoImie";
                * / 
                return false;   // nie zmianiam sql
        }
    }
    return true;
}
                 */

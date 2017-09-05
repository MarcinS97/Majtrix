using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntPrzypisania : System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;

        private int FStatus = stWaitingAcc;     // oczekujące
        private int FMode = moKier;             // 0 - kier, 1 - adm
        private int FFilter = fiAll;  

        //----- statusy przeniesienia ------
        public const int stWaitingAcc   = 0;
        public const int stAccepted     = 1;
        public const int stRejected     = 2;

        public const int stMyRequests   = 9;
        public const int stAll          = 99;
        public const int stKierAll      = 88;

        //----- typ przeniesienia (pole Typ) -----
        public const int tyWniosek  = 0;
        public const int tySubstr   = 1;    // w ramach podstruktury, albo nowy pracownik i pierwsze przypisanie
        public const int tyImport   = 2;    // z importu 

        //----- tryb pracy kontrolki ------
        public const int moKier = 0;
        public const int moAdm  = 1;

        //----- filter ------
        public const int fiOff = 0;
        public const int fiAll = 1;
        public const int fiNoImp = 2;
        
        //--------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvPrzypisania, 0);
            Tools.PrepareSorting(lvPrzypisania, 3, 15);   // data od
        }

        bool v = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidSub.Value = App.User.HasRight(AppUser.rPrzesunieciaAccSub) ? "1" : "0";

                if (PrepareFilter())
                    DoSearch(false);  // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;      // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }

        protected override void OnPreRender(EventArgs e) 
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }


        public int _Prepare(string kierId, DateTime data, bool bind)
        {
            hidKierId.Value = kierId;
            hidData.Value = Tools.DateToStr(data);

            bool b = false;
            if (!String.IsNullOrEmpty(kierId))
                if (App.User.Id == kierId)          // bieżący user
                    b = App.User.HasRight(AppUser.rPrzesunieciaAccSub);
                else
                {                                   // wskazany user, ale wydaje się nie mieć miejsca, bo kierId == null albo App.User.Id
                    DataRow dr = AppUser.GetData(kierId);
                    b = AppUser.HasRight(db.getValue(dr, "Rights"), AppUser.rPrzesunieciaAccSub);
                }

            hidSub.Value = b ? "1" : "0";

            if (bind)
            {
                lvPrzypisania.DataBind();
                return lvPrzypisania.Items.Count;
            }
            else
                return -1;
        }

        public bool SelectPrac(string pracId)
        {
            ClearFilter();
            cbShowAll.Checked = true;
            ddlPracownik.DataBind();
            Tools.SelectItem(ddlPracownik, pracId);
            DoSearch(true);
            return true;
        }
        //----------------------

        // !!! zastanowić się nad sensem Monity - zmienić ...

        public static DataSet GetStartujace()  //-> obaj kierownicy i pracownik, w sumie to ma zwrócić tylko Id
        {
            return db.getDataSet(String.Format(@"
declare @typ varchar(100)
declare @data datetime
set @typ = '{1}'
set @data = {0}

select R.Id from Przypisania R
--left outer join Przypisania R1 on R1.Status = 1 and DATEADD(DAY, -1, R.Od) between R1.Od and ISNULL(R1.Do, '20990909') and R1.IdPracownika = R.IdPracownika
left outer join Monity M on M.Typ = @typ and M.EventId = R.Id and M.UserId = 0 --R.IdKierownika
where R.Status = 1 and R.Od = @data and M.Id is null
                ", db.sqlGetDate("GETDATE()"), Mailing.maPRZES_START_K));
        }

        public static DataSet GetWygasajace(int dni)   // z określoną datą do, na dni przed i w dniu, docelowo dni trzymać przy typie maila
        {
            return db.getDataSet(String.Format(@"
declare @typ varchar(100)
declare @data datetime
declare @dni int
set @typ = '{1}'
set @data = {0}
set @dni = {2}

select R.Id from Przypisania R
left outer join Monity M on M.Typ = @typ and M.EventId = R.Id and M.UserId = 0 --R.IdKierownika
where R.Status = 1 and R.Do is null 
    and (R.DoMonit = DATEADD(DAY, @dni, @data) or R.DoMonit = @data)
    and M.Id is null  
                ", db.sqlGetDate("GETDATE()"), Mailing.maPRZES_MONIT, dni));
        }

        public static DataSet GetMonityAcc()   // czekające na akceptację
        {
            return db.getDataSet(String.Format(@"
declare @typ varchar(100)
declare @data datetime
set @typ = '{1}'
set @data = {0}

select R.Id from Przypisania R
left outer join Monity M on M.Typ = @typ and M.EventId = R.Id and M.UserId = 0 --R.IdKierownika
where R.Status = 0 and R.Od >= @data
                ", db.sqlGetDate("GETDATE()"), Mailing.maPRZES_WN));
        }
        //----------------------
        public string BezTerminu(object o)
        {
            if (db.isNull(o))
                return "bez terminu";
            else
                return Tools.DateToStr(o);
        }

        public bool IsControlVisible
        {
            get
            {
                return true; // zawsze są szczegóły do obejrzenia
                /*
                return FStatus == 0 ||
                       FStatus == 1 && FMode == 1||
                       FStatus == 2 ||
                       FStatus == 9 ||
                       FStatus == 99;
                 */ 
            }  // niezaakceptowane albo moje niezaakceptowane lub wszystkie - wtedy buttony 
        }
        //-------------------------------
        /*
        private void SetPageSize(DataPager dp)  // wywoływana na zmianę w ddl wyboru - musi przybindować
        {
            DropDownList ddl = (DropDownList)lvPrzypisania.FindControl("ddlLines");
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
            DataPager dp = (DataPager)lvPrzypisania.FindControl("DataPager1");
            SetPageSize(dp);
        }
         */ 
        //--------------------------------
        Okres ok = null;

        protected void lvPrzypisania_DataBinding(object sender, EventArgs e)
        {
            ok = Okres.LastAccessible(db.con);
        }

        protected void lvPrzypisania_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvPrzypisania, out drv);
            int status = (int)drv["Status"];
            bool adm = FMode == 1 && App.User.IsAdmin;
            switch (lim)
            {
                case Tools.limSelect:
                    bool ed = false;
                    bool del = false;
                    bool ked = false;   // kierownik przesunął w ramach swojej podstruktury
                    switch (FStatus)
                    {
                        default:
                        case stWaitingAcc:  // wnioski do mnie do akceptacji
                            ed = adm || App.User.HasRight(AppUser.rPrzesunieciaAcc);
                            del = adm; 
                            break;
                        case stAccepted:    // zaakceptowane
                            if (!adm && status == stAccepted && (int)drv["Typ"] == tyWniosek)   // zaakceptowane/do akceptacji przeze mnie
                            {
                                DateTime dOd = (DateTime)drv["Od"];
                                ked = dOd >= ok.DateFrom;   // tylko jak okres jest jeszcze otwarty
                            }
                            ed = adm || ked;
                            del = adm;
                            break;
                        case stRejected:    // odrzucone
                            del = adm;
                            break;
                        case stMyRequests:  //9 moje wnioski, tylko panel kier
                            ed = status == stWaitingAcc;
                            del = status == stWaitingAcc;
                            break;
                        case stKierAll:     //88 wszystkie do kierownika (i z podstruktury)
                            if (status == stAccepted && (int)drv["Typ"] == tySubstr)   // kierownik przesunął w ramach swojej podstruktury 
                            {
                                DateTime dOd = (DateTime)drv["Od"];
                                ked = dOd >= ok.DateFrom;               // tylko jak okres jest jeszcze otwarty
                            }

                            bool ked2 = false;
                            if (status == stAccepted && App.User.HasRight(AppUser.rPrzesunieciaAcc))   // ze mam uprawnienie
                            {
                                DateTime dOd = (DateTime)drv["Od"];
                                ked2 = dOd >= ok.DateFrom;               // tylko jak okres jest jeszcze otwarty
                            }
                            bool last = db.isNull(drv["NextId"]);

                            ed = status == stWaitingAcc || ked || ked2;     // edytować można wszystkie w otwartum okresie
                            del = status == stWaitingAcc || ked && last;    // usuwać tylko ostatni i nie wnioski
                            break;
                        case stAll:         //99 wszystkie
                            ed = adm && (status == stWaitingAcc || status == stAccepted) ||
                                 drv["IdKierownikaRq"].ToString() == App.User.Id && (status == stWaitingAcc || status == stAccepted);
                            del = adm ||
                                 drv["IdKierownikaRq"].ToString() == App.User.Id && (status == stWaitingAcc);
                            break;
                    }
                    Tools.SetControlVisible(e.Item, "EditButton", ed);
                    Tools.SetControlVisible(e.Item, "DeleteButton", del);
                    Tools.SetControlVisible(e.Item, "btDetails", !ed);
                    break;
                case Tools.limEdit:
                    bool upd = false;
                    bool acc = false;
                    switch (FStatus)
                    {
                        default:
                        case stWaitingAcc:   // wnioski do akceptacji - w panelu kierownika: do mnie
                            upd = adm;
                            acc = App.User.HasRight(AppUser.rPrzesunieciaAcc);
                            break;
                        case stAccepted:     // zaakceptowane
                            upd = adm;
                            break;
                        case stRejected:     // odrzucone
                            break;
                        case stMyRequests:   // moje wnioski 9
                            upd = true;
                            break;
                        case stKierAll:          // wszystkie kierownika 88 
                            upd = true;
                            break;
                        case stAll:          // wszystkie 99
                            upd = FMode == moAdm && App.User.IsAdmin && (status == stWaitingAcc || status == stAccepted) ||
                                 drv["IdKierownikaRq"].ToString() == App.User.Id && (status == stWaitingAcc || status == stAccepted);
                            acc = FMode == moAdm && App.User.IsAdmin && (status == stWaitingAcc) ||
                                 drv["IdKierownikaRq"].ToString() == App.User.Id && (status == stWaitingAcc);
                            break;
                    }

                    //Tools.SetControlVisible(e.Item, "btUpdate", upd);
                    Tools.SetControlVisible(e.Item, "btUpdate", !acc);
                    //Tools.SetControlVisible(e.Item, "btCancel", upd);
                    Button btAcc = (Button)Tools.SetControlVisible(e.Item, "btAccept", acc);
                    Button btRej = (Button)Tools.SetControlVisible(e.Item, "btReject", acc);
                    if (btAcc != null) Tools.MakeConfirmButton(btAcc, "Potwierdź akceptację przesunięcia pracownika.");
                    if (btRej != null) Tools.MakeConfirmButton(btRej, "Potwierdź odrzucenie przesunięcia pracownika.");
                    //--- edycja dla admina ----
                    ed = adm && status != stRejected;

                    Tools.SetControlVisible(e.Item, "lbOd", !ed);
                    Tools.SetControlVisible(e.Item, "lbDo", !ed);
                    Tools.SetControlVisible(e.Item, "deFrom", ed);
                    Tools.SetControlVisible(e.Item, "deTo", ed);
                    Tools.SetControlVisible(e.Item, "deMonit", ed);
                    Tools.SetControlVisible(e.Item, "lbMonit", ed);

                    bool u = FStatus == stMyRequests && status == stWaitingAcc || 
                             adm && status == stWaitingAcc;
                    Tools.SetControlVisible(e.Item, "tbUwagi", u);
                    Tools.SetControlVisible(e.Item, "lbUwagi", !u);

                    bool v = FStatus != stMyRequests || status != stWaitingAcc;
                    bool a = status == stWaitingAcc;
                    Tools.SetControlVisible(e.Item, "lbUwagiAccLabel", v);
                    Tools.SetControlVisible(e.Item, "tbUwagiAcc", v && a);
                    Tools.SetControlVisible(e.Item, "lbUwagiAcc", v && !a);

                    break;
            }
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
#if SIEMENS
                Tools.SetControlVisible(e.Item, "paCommodity", Lic.ScoreCards);
                Tools.SetControlVisible(e.Item, "paArea", false);
                RequiredFieldValidator rqv = e.Item.FindControl("RequiredFieldValidator2") as RequiredFieldValidator;
                if (rqv != null)
                {
                    rqv.Enabled = Lic.ScoreCards;
                    rqv = e.Item.FindControl("RequiredFieldValidator3") as RequiredFieldValidator;
                    if (rqv != null) rqv.Enabled = false;
                    rqv = e.Item.FindControl("RequiredFieldValidator4") as RequiredFieldValidator;
                    if (rqv != null) rqv.Enabled = false;
                }
                if (Lic.ScoreCards) Tools.SetText(e.Item, "lbComm", "Arkusz:");
#elif VICIM
                Tools.SetControlVisible(e.Item, "paCommodity", false);
                Tools.SetControlVisible(e.Item, "paArea", false);
                RequiredFieldValidator rqv = e.Item.FindControl("RequiredFieldValidator2") as RequiredFieldValidator;
                if (rqv != null)
                {
                    rqv.Enabled = false;
                    rqv = e.Item.FindControl("RequiredFieldValidator3") as RequiredFieldValidator;
                    if (rqv != null) rqv.Enabled = false;
                    rqv = e.Item.FindControl("RequiredFieldValidator4") as RequiredFieldValidator;
                    if (rqv != null) rqv.Enabled = false;
                }
#endif
                Tools.SetControlVisible(e.Item, "paStrefa", IsEditStrefa);            
            }
        }

        protected void lvPrzypisania_ItemCreated(object sender, ListViewItemEventArgs e)
        {

        }

        protected void lvPrzypisania_DataBound(object sender, EventArgs e)
        {
            Tools.SetControlVisible(lvPrzypisania, "thControl", IsControlVisible);
        }

        protected void lvPrzypisania_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            
            //cntPrzypisaniaParametry pp = lvPrzypisania.EditItem.FindControl("edi")
        }

        int zapisz = -1;
        string przesId = null;
        string pracId = null;
        DataSet mail_data_DEL = null;  // dla delete 
        
        protected void lvPrzypisania_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Update":
                    zapisz = Tools.StrToInt(e.CommandArgument.ToString(), -1);
                    break;
                case "Delete":
                    pracId = e.CommandArgument.ToString();
                    break;
                //----- letter data pager -----
                case "JUMP":
                    //EnableControls(lv, false, false);
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPrzypisania.FindControl("DataPager1");
                    lvPrzypisania.Sort("NazwiskoImie", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);
                    /*
                    lvPracownicy.SelectedIndex = -1;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                    */
                    break; 
                //----- -----
                case "Deselect":
                    lvPrzypisania.SelectedIndex = -1;
                    break;
            }
        }

        protected void lvPrzypisania_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            //Log.LogChanges(lvPrzypisania, e);
            //cntPrzypisaniaParametry.UpdatePrevDateToNull(pracId);

            SqlDataSource1.DeleteParameters["IdPracownika"].DefaultValue = pracId;
            
            przesId = Tools.GetDataKey(lvPrzypisania, e);
            mail_data_DEL = Mailing.GetPrzesData(przesId, false);
        }

        /*
         M-DoMonit  D-Do    EditBox     M       D-zapis
         0          0       null        null    null
         1          0       M           
         0          1       D
         1          1       D           
         =          =       D           D       D
         
         */


        /*
        private bool SetError(ListViewItem item, string lbName, bool error)
        {
            Label lb = item.FindControl(lbName) as Label;
            if (lb != null)
                if (error)
                    Tools.AddCss(lb, "error");
                else
                    Tools.RemoveCss(lb, "error");
            return error;
        }

        private void ShowError(ListViewItem item, bool prac, bool kier, bool deOd, bool deDo, bool idStrefaRcp, bool idCC, bool idComm, bool idArea, bool idPos)
        {
            SetError(item, "lb1", prac);
            SetError(item, "lb2", kier);
            SetError(item, "lb3", deOd);
            SetError(item, "lb4", deDo);
            SetError(item, "lb5", idCC);
            SetError(item, "lb6", idComm);
            SetError(item, "lb7", idArea);
            SetError(item, "lb8", idPos);
            SetError(item, "lb9", idStrefaRcp);
        }

        private void ClearError(ListViewItem item)
        {
            ShowError(item, false, false, false, false, false, false, false, false, false);
        }

        //private bool Validate(ListViewItem item, string pracId, string kierId, DateEdit deOd, DateEdit deDo, int? idStrefaRcp, int? idCC, int? idComm, int? idArea, int? idPos)
        private bool Validate(ListViewItem item, string pracId, string kierId, DateEdit deOd, DateEdit deDo, int? idStrefaRcp, cntSplityWsp cntSplit, int? idComm, int? idArea, int? idPos, out string msg)
        {
            bool e1 = false;
            bool e2 = false;
            bool e3 = false;
            bool e4 = false;
            bool e5 = false;
            bool e6 = false;
            bool e7 = false;
            bool e8 = false;
            bool e9 = false;
            msg = null;
            if (String.IsNullOrEmpty(pracId)) e1 = true;
            if (String.IsNullOrEmpty(kierId)) e2 = true;
            if (pracId == kierId)  // i tu zapętlenie struktury sprawdzić
            { 
                e1 = true; 
                e2 = true; 
            }

            if (deOd.IsValid) 
            {
                DateTime dod = (DateTime)deOd.Date;
                if (deDo.IsValid) 
                    if ((DateTime)deDo.Date < dod) 
                    {
                        e3 = true;
                        e4 = true;
                    }
                if (!e3)    // dotyczy zamkniętego okresu
                {
                    Okres ok = Okres.LastAccessible(db.con);
                    if (dod < ok.DateFrom)
                    {
                        msg = "Przesunięcie nie może rozpoczynać się w zamkniętym okresie rozliczeniowym.";
                        e3 = true;
                    }
                }
                if (!e3)  // zachodzi na inne przesunięcie
                {
                    DateTime lastOd;
                    DateTime? lastDo;
                    DataRow drLast = GetLastPrzypisanie(pracId, out lastOd, out lastDo);
                    if (dod <= lastOd)
                    {
                        msg = "Okres przesunięcia koliduje z już istniejącym.";
                        e3 = true;
                    }
                }
            }
            else e3 = true;

            if (cntSplit != null && !cntSplit.Validate()) e5 = true;
            if (idComm == null) e6 = true;
            if (idArea == null) e7 = true;
            if (idPos  == null) e8 = true;
            if (idStrefaRcp == null) e9 = true;

            ShowError(item, e1, e2, e3, e4, e9, e5, e6, e7, e8);
            return !(e1 || e2 || e3 || e4 || e5 || e6 || e7 || e8 || e9);
        }
        */
        private bool Validate(int zapisz, ListViewUpdateEventArgs e, cntSplityWsp splity)
        {
            switch (zapisz)
            {
                case 0:     // zapisz
                case 1:     // zaakceptuj
                    object o;     
                    if (zapisz == 0)
                    {
                        o = e.NewValues["Status"];
                        if (o != null && o.ToString() == stWaitingAcc.ToString())
                            return true;        // wszystko ok                        
                    }

                    if (IsEditStrefa)
                    {
                        o = e.NewValues["RcpStrefaId"];
                        if (o == null)
                        {
                            Tools.ShowMessage("Proszę podać: Strefa RCP");
                            return false;
                        }
                    }
                
                    if (splity == null || !splity._Validate())
                    {
                        Tools.ShowMessage("Niepoprawne CC.");
                        return false;
                    }
#if !SIEMENS && !VICIM  //<<< T:to jest do zmiany
                    o = e.NewValues["IdCommodity"];
                    if (o == null)
                    {
                        Tools.ShowMessage("Proszę podać: Commodity");
                        return false;
                    }
                    o = e.NewValues["IdArea"];
                    if (o == null)
                    {
                        Tools.ShowMessage("Proszę podać: Area");
                        return false;
                    }
                    o = e.NewValues["IdPosition"];
                    if (o == null)
                    {
                        Tools.ShowMessage("Proszę podać: Position");
                        return false;
                    }
#endif
                    break;
                case 2:  // 2 odrzucam wniosek
                    o = e.NewValues["UwagiAcc"];
                    string u = o != null ? o.ToString().Trim() : null;
                    if (String.IsNullOrEmpty(u))
                    {
                        Tools.ShowMessage("Proszę podać uzasadnienie odrzucenia wniosku");
                        return false;
                    }
                    break;
            }
            return true;
        }

        protected void lvPrzypisania_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            przesId = Tools.GetDataKey(lvPrzypisania, e);
            cntSplityWsp splity = lvPrzypisania.EditItem.FindControl("cntSplityWsp2") as cntSplityWsp;
            if (Validate(zapisz, e, splity))
            {
                switch (zapisz)
                {
                    case 0:     // zapisz
                        DateTime dtOd = (DateTime)e.NewValues["Od"];
                        string pracId = Tools.GetText(lvPrzypisania.EditItem, "hidPracId");
                        cntPrzypisaniaParametry.UpdatePrevDateTo(pracId, dtOd.AddDays(-1));  // <<<<<< trnasakcja !!!!!
                        splity.Update();   //<<<<< transakcja !!!!
                        break;
                    case 1:     // zaakceptuj
                        e.NewValues["Status"] = stAccepted;
                        e.NewValues["IdKierownikaAcc"] = App.User.Id;
                        if (!App.User.IsOriginalUser)
                            e.NewValues["IdKierownikaAccZast"] = App.User.OriginalId;
                        e.NewValues["DataAcc"] = DateTime.Now;

                        dtOd = (DateTime)e.NewValues["Od"];
                        pracId = Tools.GetText(lvPrzypisania.EditItem, "hidPracId");
                        cntPrzypisaniaParametry.UpdatePrevDateTo(pracId, dtOd.AddDays(-1));  // <<<<<< trnasakcja !!!!!
                        splity.Update();
                        break;
                    case 2:     // odrzuć
                        e.NewValues["Status"] = stRejected;
                        e.NewValues["IdKierownikaAcc"] = App.User.Id;
                        if (!App.User.IsOriginalUser)
                            e.NewValues["IdKierownikaAccZast"] = App.User.OriginalId;
                        e.NewValues["DataAcc"] = DateTime.Now;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }

            }
            else e.Cancel = true;
            Log.LogChanges(lvPrzypisania, e);
        }

        protected void lvPrzypisania_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            switch (zapisz)
            {
                case 0:     // zapisz
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_UPD, przesId);
                    break;
                case 1:     // zaakceptuj
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_ACC, przesId);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_K, przesId);
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_P, przesId);
                    break;
                case 2:     // odrzuć
                    Mailing.EventPrzesuniecie(Mailing.maPRZES_REJ, przesId);
                    break;
            }

            Tools.SetUpdated(App.updPrzesuniecia, App.updPrzesunieciaList);
        }

        protected void lvPrzypisania_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Mailing.EventPrzesuniecie(Mailing.maPRZES_DEL_K, przesId, mail_data_DEL);
            Mailing.EventPrzesuniecie(Mailing.maPRZES_DEL_P, przesId, mail_data_DEL);
            Tools.SetUpdated(App.updPrzesuniecia, App.updPrzesunieciaList);
        }
        //---------------------------------------------------
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

            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');$('#{2}').val('0');$('#{3}').val('0');$('#{4}').val('0');$('#{5}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID, ddlPracownik.ClientID, ddlKierownikOd.ClientID, ddlKierownikDo.ClientID, deGreaterThan.EditBox.ClientID);
            Tools.ExecOnStart2("searchtrigger", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        private bool PrepareFilter()
        {
            switch (FFilter)
            {
                case fiOff:
                    paFilter.Visible = false;
                    return false;
                default:
                case fiAll:
                    return true;
                case fiNoImp:
                    cbShowAll.Visible = false;
                    return true;
            }
        }

        public void ClearFilter()
        {
            tbSearch.Text = null;
            cbShowAll.Checked = false;
            ddlPracownik.SelectedIndex = -1;
            ddlKierownikOd.SelectedIndex = -1;
            ddlKierownikDo.SelectedIndex = -1;
            deGreaterThan.DateStr = null;
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private string FilterStr
        {
            set { ViewState["filter"] = value; }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        private void DoSearch(bool bind)
        {
            string filter;
            string f1 =
                (cbShowAll.Checked ?                                  null : " and IdKierownikaRq is not null") +
                (String.IsNullOrEmpty(ddlPracownik.SelectedValue) ?   null : (" and IdPracownika = " + ddlPracownik.SelectedValue)) +
                (String.IsNullOrEmpty(ddlKierownikOd.SelectedValue) ? null : (" and IdKierownikaOd = " + ddlKierownikOd.SelectedValue)) +
                (String.IsNullOrEmpty(ddlKierownikDo.SelectedValue) ? null : (" and IdKierownikaDo = " + ddlKierownikDo.SelectedValue)) +
                (!deGreaterThan.IsValid ?                             null : String.Format(" and Od >= '{0}'", deGreaterThan.DateStr));
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = String.IsNullOrEmpty(f1) ? null : f1.Substring(5);
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + f1;
            }
            FilterStr = filter;
            SqlDataSource1.FilterExpression = filter;
            if (bind)
            {
                lvPrzypisania.DataBind();
                if (lvPrzypisania.Items.Count == 1) Select(0);
                else if (lvPrzypisania.SelectedIndex != -1) Select(-1);
            }
        }

        private void DoSearch_2(bool bind)
        {
            string all;
            string filter;
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                all = !cbShowAll.Checked ? "IdKierownikaRq is not null" : null;
                filter = all;
            }
            else
            {
                all = !cbShowAll.Checked ? " and IdKierownikaRq is not null" : null;
                //   Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string[] words = tbSearch.Text.Trim().Split(' ');
                if (words.Length == 1)
                {
                    filter = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')" + all;
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    filter = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%')" + all;   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    filter = String.Join(" and ", exp) + all;
                }
            }
            FilterStr = filter;
            SqlDataSource1.FilterExpression = filter;
            if (bind)
            {
                lvPrzypisania.DataBind();
                if (lvPrzypisania.Items.Count == 1) Select(0);
                else if (lvPrzypisania.SelectedIndex != -1) Select(-1);
            }
        }

        private void DoSearch_1(bool bind)
        {
            string all;
            string filter;
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                all = !cbShowAll.Checked ? "IdKierownikaRq is not null" : null;
                filter = all;
            }
            else
            {
                all = !cbShowAll.Checked ? " and IdKierownikaRq is not null" : null;
                //   Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string[] words = tbSearch.Text.Trim().Split(' ');
                if (words.Length == 1)
                {
                    filter = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%')" + all;
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else
                {
                    if (words.Length == 2 && words[0].ToLower() == words[1].ToLower())
                    {
                        filter = "(Nazwisko like '{0}%' and Imie like '{0}%' or KadryId like '{0}%')" + all;   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                        SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    }
                    else
                    {
                        string[] exp = new string[words.Length];
                        for (int i = 0; i < words.Length; i++)
                        {
                            exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%')", i);
                            SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                        }
                        filter = String.Join(" and ", exp) + all;
                    }
                }
            }
            FilterStr = filter;
            SqlDataSource1.FilterExpression = filter;
            if (bind)
            {
                lvPrzypisania.DataBind();
                if (lvPrzypisania.Items.Count == 1) Select(0);
                else if (lvPrzypisania.SelectedIndex != -1) Select(-1);
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(true);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(true);
        }

        public void Select(int index)
        {
            lvPrzypisania.SelectedIndex = index;
            CheckSelectedChanged();
        }

        private bool CheckSelectedChanged()
        {
            string oldId = SelectedId;
            string newId = lvPrzypisania.SelectedDataKey == null ? null : lvPrzypisania.SelectedDataKey.Value.ToString();
            if (oldId != newId)
            {
                SelectedId = newId;
                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);
                return true;
            }
            else
                return false;
        }

        protected void lvPrzypisania_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSelectedChanged();
        }

        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {
        }

        protected void lvPrzypisania_LayoutCreated(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterStr;
        }

        protected void lvPrzypisania_Sorting(object sender, ListViewSortEventArgs e)
        {
        }

        protected void lvPrzypisania_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void SqlDataSource1_Init(object sender, EventArgs e)
        {
        }

        //--------------------------------
        public int Status
        {
            get { return FStatus; }
            set
            {
                FStatus = value;
                hidStatus.Value = FStatus.ToString();
            }
        }

        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        public string SelectedId
        {
            set { ViewState["selid"] = value; }
            get { return Tools.GetStr(ViewState["selid"]); }
        }

        public int Filter
        {
            set { FFilter = value; }
            get { return FFilter; }
        }

        public bool IsEditStrefa    // to samo co w cntPrzypisaniaParametry
        {
            get { return App.User.IsAdmin; }
        }
    }
}
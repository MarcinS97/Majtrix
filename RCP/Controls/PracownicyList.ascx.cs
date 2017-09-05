using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.Security;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class PracownicyList: System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
 
        const string sesSortId = "_ADMsort";
        const string sesPageIdx = "_ADMpidxt";
        const int maxSortCol = 15;
        private int FDefSortColumn = 1;
        private int FMode = 0;          // pozostałość z HRCheck
        private int FPageSize = 15;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareView();
        }
        //---------------------------
        public string PrepareName(object name)
        {
            return Tools.PrepareName(name.ToString());
        }

        public string GetStatus(object status)
        {
            int st = Base.getInt(status, -999);
            return App.GetStatus(st);
        }

        /*
        public string GetStatus(object status)
        {
            string st = status.ToString();
            switch (st)
            {
                case "0": return "";        // bez zmian
                case "1": return "Stary";
                case "2": return "Nowy";
                default: return st;
            }
        }
        */
        private static string GetSortField(int col)
        {
            string d = col < 0 ? " desc" : "";
            switch (Math.Abs(col))
            {
                default:
                case 0: return "";
                case 1: return "NazwiskoImie" + d;
                case 2: return "P.Kierownik" + d;
                case 3: return "P.KadryId" + d;
                case 4: return "P.RcpId" + d;
                case 5: return "Strefa" + d;
                case 6: return "Algorytm" + d;
                case 7: return "Dzial" + d;
                case 8: return "Stanowisko" + d;
                case 9: return "KierownikNI" + d;
                case 10: return "P.Status" + d;
                case 11: return "P.Admin" + d;
                case 12: return "P.Raporty" + d;
                case 13: return "P.Mailing" + d;
            }
        }

        public static string GetWhere(int mode)
        {
            switch (mode)
            {
                default:
                case 0: return null;
                case 1: return " where P.Status >= 0";
            }
        }

        private string GetDefOrder()
        {
            string s = GetSortField(Sort);       // nie można odwoływać sie do lv.FindControl bo nie ustawia wtedy poprawnie !!!     
            if (!String.IsNullOrEmpty(s))
                return " order by " + s;
            else
                return null;
        }

        //---------------------------------------
        public void PrepareListView(bool loadLetters)
        {
            switch (FMode)
            {
                case 1:
                    Tools.SetControlVisible(lvPracownicy, "Th12", false);
                    //Tools.SetControlVisible(lvPracownicy, "th10", false);
                    //Tools.SetControlVisible(lvPracownicy, "th11", false);
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", false);
                    break;
            }
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            if (dp != null) dp.PageSize = FPageSize;
            dp = (DataPager)lvPracownicy.FindControl("DataPager2");
            if (dp != null) dp.PageSize = FPageSize;
            
            if (loadLetters)
            {
                LetterDataPager pg = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
                if (pg != null)
                    //pg.Prepare();
                    pg.Reload();
            }
        }

        private void PrepareView()
        {
            //int FMode = ModeAsInt;
            string where = GetWhere(FMode);
            string sort = String.IsNullOrEmpty(lvPracownicy.SortExpression) ? GetDefOrder() : null;
            SqlDataSource1.SelectCommand += where + sort;

            LetterDataPager pg = (LetterDataPager)lvPracownicy.FindControl("LetterDataPager1");
            if (pg != null)
            {
                pg.TbName = "Pracownicy" + where;
                pg.PageSize = FPageSize;
            }

            if (!IsPostBack)
            {
                PrepareListView(false);
                lvPracownicy.DataBind();
                /*

                Button bt = (Button)lvPracownicy.FindControl("btK");
                bt.CommandName = "JUMP";
                bt.CommandArgument = bt.Text;

                bt = (Button)lvPracownicy.FindControl("btW");
                bt.CommandName = "JUMP";
                bt.CommandArgument = bt.Text;
                 */
            }
            //else 

            if (!IsPostBack)
                RestorePage();
        }

        private void PrepareView(ListViewItem item)
        {
            switch (FMode)
            {
                default:
                case 0:
                    DataRowView rowView = (DataRowView)((ListViewDataItem)item).DataItem;
                    string kid = rowView["KadryId"].ToString();
                    bool spoza = String.IsNullOrEmpty(kid);
                    
                    Button bt = (Button)item.FindControl("DeleteButton");
                    if (bt != null)
                    {
                        bool v = spoza || kid == "00000";   //zeby mozna było skasowac pusty root-rekord po imporcie
                        bt.Visible = v;                     //okazuje się ze jak jest na niewidocznej w danym momencie zakładce to Visible nadal = false !!! więc trzeba od zmiennej uzaleznic bo sprawdzenie bt.Visible nie jest wiarygodne !!!
                        if (v) Tools.MakeConfirmDeleteRecordButton(bt);
                    }

                    bool kier = db.getBool(rowView["Kierownik"], false);
                    if (kier)
                    {
                        HtmlTableRow tr = (HtmlTableRow)item.FindControl("trLine");
                        if (tr != null)
                            Tools.AddClass(tr, "kier");
                    }


                    /*
                    if (!spoza)   // ankieta tylko dla tych z KP
                    {
                        bt = (Button)item.FindControl("AnkietaButton");
                        if (bt != null)
                        {
                            /*aa
                            bt.Visible = !Base.getBool(rowView["Checked"], true) && // jak błąd to !true -> nie pokazuje [Ankieta]
                                          Base.getInt(rowView["StanAnkiety"], -1) == Ankieta.stPracownik;   // tylko jak na etapie Pracownik
                             * /z 
                            if (bt.Visible)
                            {
                                bt.CommandName = "Ankieta";
                                bt.CommandArgument = rowView["Id"].ToString();
                            }
                        }
                    }
                    */
                    break;
                case 1:
                    Tools.SetControlVisible(item, "tdStart", false);
                    //Tools.SetControlVisible(item, "tdStatus", false);
                    //Tools.SetControlVisible(item, "tdStanAnkiety", false);
                    //Tools.SetControlVisible(item, "DeleteButton", false);  jest visible=flase domyslnie
                    break;
            }
        }
        //--------------------------------------
        private void StorePage()
        {
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            Session[ID + sesPageIdx] = dp.StartRowIndex; 
        }

        private void RestorePage()
        {
            string sid = ID + sesPageIdx;
            if (Session[sid] != null)
            {
                DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                int idx = (int)Session[sid];
                Session[sid] = null;
                dp.SetPageProperties(idx, dp.PageSize, true);
            }
        }
        //---------------------------------------
        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    string p1, p2;
                    Tools.GetLineParams(e.CommandArgument.ToString(), out p1, out p2);
                    SelectedRcpId = p1;
                    SelectedStrefaId = p2;
                    /*
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);
                     */ 
                    break;
                case "Edit":
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    break;
                case "Update":
                    break;

                case "ZOOM:Ankieta":
                    StorePage();
                    string pid;
                    string stan;
                    Tools.GetLineParams(e.CommandArgument.ToString(), out pid, out stan);
                    /*aa
                    int st = Tools.StrToInt(stan, Ankieta.stKontroler1);
                    Ankieta.Open(pid, Ankieta.StanToPanel(st), false);
                     */
                    //Response.Redirect(App.AnkietaForm + "?id=" + (string)e.CommandArgument + "&p=A&m=q");
                    break;
                case "ZOOM:AnkietaK":
                    StorePage();
                    pid = e.CommandArgument.ToString();
                    /*aa
                    Ankieta.Open(pid, Ankieta.paKontroler, false);
                     */
                    break;
                case "NewRecord":
                    lvPracownicy.EditIndex = -1;
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", false);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "Insert":  // <<<< dodać odświeżenie LetterPagera !!!
                case "CancelInsert":
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "Ankieta":
                    StorePage();
                    /*aa
                    Ankieta.Open(e.CommandArgument.ToString(), Ankieta.paPracownik, true);
                     */
                    break;

                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("NazwiskoImie", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);

                    lvPracownicy.SelectedIndex = -1;
                    SelectedRcpId = null;
                    if (SelectedChanged != null)
                        SelectedChanged(this, EventArgs.Empty);

                    break;
                    //---------------
                case "passreset":
                    string id = e.CommandArgument.ToString();
                    //string pesel = db.getScalar("select Nick from Pracownicy where Id = " + id);
                    DataRow dr = db.getDataRow("select Nazwisko + ' ' + Imie as Pracownik, Nick from Pracownicy where Id = " + id);
                    string nazwisko = db.getValue(dr, 0);
                    string pesel = db.getValue(dr, 1);
                    if (!String.IsNullOrEmpty(pesel)) 
                    {
                        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod);
                        if (db.update("Pracownicy", 0, "Pass", "Id=" + id, db.strParam(hash)))
                        {
                            Tools.ShowMessage("Hasło zostało ustawione.");
                            Log.Info(Log.PASSRESET, "Reset hasła pracownika", nazwisko + ' ' + id);
                        }
                    }
                    else
                    {
                        Tools.ShowMessage("Wystąpił błąd podczas ustawiania hasła.");
                        Log.Error(Log.PASSRESET, "Reset hasła pracownika - BŁĄD", nazwisko + ' ' + id);
                    }
                    break;
            }
        }

        protected void lvPracownicy_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Session[ID + sesSortId] = sort;  // unikalne co do kontrolki
        }

        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            int sort = Sort;
            Report.ShowSort(lvPracownicy, sort, sort > 0);
        }

        //tylko tu mogę modyfikować kontrolki na EditItemTemplate, w innych funkcjach nawet jak znajdzie to nie potrafi ustawić visible
        protected void lvPracownicy_PreRender(object sender, EventArgs e)
        {
            if (lvPracownicy.EditIndex != -1)   
            {
                Label tb = (Label)lvPracownicy.EditItem.FindControl("KadryIdLabel");
                if (tb == null || String.IsNullOrEmpty(tb.Text))
                {
                    DropDownList ddl = (DropDownList)lvPracownicy.EditItem.FindControl("ddlStanAnkiety");
                    if (ddl != null)
                        ddl.Visible = false;
                }
            }
        }

        private void SetTB(Control item, string name, int maxlen, int width)
        {
            TextBox tb;
            if  (Tools.FindTextBox(item, name, out tb))
            {
                tb.MaxLength = maxlen;
                if (width != -1)
                    tb.Width = width;
            }
        }

        private void PrepareEditControls(Control item)
        {
            SetTB(item, "KadryIdTextBox", 5, 50);
            SetTB(item, "RcpIdTextBox", 5, 50);
            SetTB(item, "LoginTextBox", 20, 200);
            SetTB(item, "EmailTextBox", 100, -1);
            //SetTB(item, "StatusTextBox", 3, 50);
            DropDownList ddl = (DropDownList)item.FindControl("ddlStatus");
            App.FillStatus(ddl, null, true);
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (((ListViewDataItem)e.Item).DisplayIndex == lvPracownicy.EditIndex)// tu nie wchodzi insertItem - trzeba to zrobić w OnItemCreated
                    PrepareEditControls(e.Item);
                if (((ListViewDataItem)e.Item).DisplayIndex == lvPracownicy.EditIndex && lvPracownicy.EditItem != null) 
                    InitItem(e);
                PrepareView(e.Item);            
            }
                /* to się nie wykonuje 
            else if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                PrepareEditControls(e.Item);
                InitItem(e);
                PrepareView(e.Item);
            }
                 * */
        }

        protected void lvPracownicy_ItemCreated(object sender, ListViewItemEventArgs e) // tylko tu jest dostęp do InserItemTemplate
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                PrepareEditControls(e.Item);
                Tools.SelectItem(e.Item, "ddlStatus", App.stPomin.ToString());
            }
        }

        private void InitItem(ListViewItemEventArgs e)  
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            DataRowView drv = (DataRowView)dataItem.DataItem;
            Tools.SelectItem(e.Item, "ddlDzial", drv["IdDzialu"]);
            Tools.SelectItem(e.Item, "ddlStanowisko", drv["IdStanowiska"]);
            Tools.SelectItem(e.Item, "ddlKierownik", drv["IdKierownika"]);
            Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
            Tools.SelectItem(e.Item, "ddlAlgorytm", drv["RcpAlgorytm"]);
            
            Tools.SelectItem(e.Item, "ddlLinia", drv["IdLinii"]);
            Tools.SelectItem(e.Item, "ddlSplit", drv["GrSplitu"]);

            object st = drv["Status"];
            Tools.SelectItem(e.Item, "ddlStatus", st == null ? App.stPomin.ToString() : st.ToString());

            bool kadm = App.User.HasRight(AppUser.rKwitekAdm);
            Button bt = (Button)Tools.SetControlVisible(e.Item, "btKwitekPassReset", kadm);
            if (bt != null && kadm)
                Tools.MakeConfirmButton(bt, "Zmienić hasło na domyślne ?");
            /*
            Button bt = (Button)e.Item.FindControl("btKwitekPassReset");
            if (bt != null)
                Tools.MakeConfirmButton(bt, "Zmienić hasło na domyślne ?");
            */
        }

        private bool UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values, EventArgs ea)
        {
            values["IdDzialu"] = Tools.GetDdlSelectedValueInt(item, "ddlDzial");
            values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");
            values["IdKierownika"] = Tools.GetDdlSelectedValueInt(item, "ddlKierownik");
            values["RcpStrefaId"] = Tools.GetDdlSelectedValueInt(item, "ddlStrefa");
            values["RcpAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlAlgorytm");
            values["IdLinii"] = Tools.GetDdlSelectedValueInt(item, "ddlLinia");
            values["GrSplitu"] = Tools.GetDdlSelectedValueInt(item, "ddlSplit");
            int? status = Tools.GetDdlSelectedValueInt(item, "ddlStatus");
            values["Status"] = status;
            AppUser user = AppUser.CreateOrGetSession();
            bool r = (bool)values["Raporty"];
            bool or = oldValues != null ? (bool)oldValues["Raporty"] : false;  // jak dopisuję nowego to zawsze odnoszę do false 
            if (r && !or && !user.IsRaporty)    // chcę ustawić i nowy lub nie miał ustawione
            {
                string id = Base.getScalar("select top 1 Id from Pracownicy where Raporty=1");
                if (!String.IsNullOrEmpty(id))  // jezeli juz ktos jest z uprawnieniem to kontroluje dostep
                {
                    values["Raporty"] = false;
                    Log.Info(Log.t2APP_ADMREPORTS, "Próba ustawienia dostępu do raportów dla " + values["Nazwisko"] + " " + values["Imie"] + " bez wymaganych uprawnień", null, Log.OK);
                    Tools.ShowMessage("Uprawnienia dostępu do raportów można nadać jedynie jeżeli samemu się je posiada.");
                    return false;
                }
            }
            if (lvPracownicy.EditIndex >= 0 && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)
            {
                bool a = (bool)values["Admin"];
                if (!a)
                {
                    values["Admin"] = true;
                    Tools.ShowMessage("Nie można usunąć sobie uprawnień administratora.");
                    return false;
                }
            }
            bool k = (bool)values["Kierownik"];
            if (k && (int)status == App.stPomin)
            {
                Tools.ShowMessage("Nie można pominąć kierownika.");
                return false;
            }
            
            string pid = lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString();
            Log.LogChanges(Log.PRACOWNIK, "Pracownik: " + AppUser.GetNazwiskoImieNREW(pid), ea);
            return true;
        }

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvPracownicy.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvPracownicy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        protected void lvPracownicy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();
            if (lvPracownicy.EditIndex >= 0 && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)
                user.Reload(false);
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Tools.DateOk(args.Value);
        }

        protected void lvPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }
        //---------------------------------------
        public ListView List
        {
            get { return lvPracownicy; }
        }

        /* pozostałość z pwd
        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }
        */
        /* stare
        public string Mode
        {
            get { return hidMode.Value; }
            set { hidMode.Value = value; }
        }

        public int ModeAsInt
        {
            get { return Tools.StrToInt(Mode, 0); }
        }
        */
        public int Sort
        {
            get
            {
                object s = Session[ID + sesSortId];
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

        public int PageSize
        {
            set
            {
                FPageSize = value;

                //DataPager1.
                /*
                DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                if (dp != null) dp.PageSize = value;
                dp = (DataPager)lvPracownicy.FindControl("DataPager2");
                if (dp != null) dp.PageSize = value;
                */
            }
        }
        //-------------------------

        /*
        protected void btJump_Click(object sender, EventArgs e)
        {

        }

        protected void btJump_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "JUMP")
            {
                DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                switch (e.CommandArgument.ToString())
                {
                    case "K":
                        dp.SetPageProperties(100, dp.PageSize, true);        
                        break;
                    case "W":
                        dp.SetPageProperties(300, dp.PageSize, true);
                        break;
                }
            }
        }
         */
        //---------------------------
        public string SelectedPracId
        {
            get 
            {
                if (lvPracownicy.SelectedIndex != -1)
                    return lvPracownicy.SelectedDataKey.Value.ToString();
                else
                    return null;
            }
        }

        public string SelectedRcpId
        {
            get { return hidSelectedRcpId.Value; }
            set { hidSelectedRcpId.Value = value; }
        }

        public string SelectedStrefaId
        {
            get { return hidSelectedStrefaId.Value; }
            set { hidSelectedStrefaId.Value = value; }
        }
    }
}
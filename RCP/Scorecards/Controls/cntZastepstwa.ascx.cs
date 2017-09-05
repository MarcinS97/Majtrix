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
using HRRcp.Controls;

namespace HRRcp.Scorecards.Controls
{
    public partial class cntZastepstwa : System.Web.UI.UserControl
    {
        public int FMode;

        const string sesSortId = "_sort";
        const int maxSortCol = 4;
        private int FDefSortColumn = 1; // 

        public const int moKogoJa       = 0;    // formatka Start, od daty
        public const int moKtoMnie      = 1;    // Ustawienia - wskazywanie zastępującego, od daty
        public const int moKtoMnieHist  = 11;   // Ustawienia - wskazywanie zastępującego, historia do daty
        public const int moAsAdmin      = 2;    // Administracja - wszystkie aktualne
        public const int moAsAdminHist  = 21;   // Administracja - historia

        DataSet dsKier = null;
        bool canSetZast = false;
        bool ro = false;
        AppUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            if (!IsPostBack)
            {
                Prepare(user.Id);
            }
        }

        public int Prepare(string kierId)
        {
            KierId = kierId;
            hidData.Value = Base.DateToStr(DateTime.Today);
            if (FMode == moKtoMnie || FMode == moKtoMnieHist)
            {
                string list = App.GetPracBelowList(kierId, true, 1);
                KierIsParent = !String.IsNullOrEmpty(list) && list != kierId;
                KierIds = list;
                /*
                dsKier = App.GetPracBelow(kierId, false, false);
                KierIsParent = dsKier != null && Base.getCount(dsKier) > 1;
                KierIds = Base.Join(dsKier, "Id", ",");
                */ 
            }
            else
            {
                KierIsParent = false;
                KierIds = kierId;
            }
            lvZastepstwa.DataBind();
            return lvZastepstwa.Items.Count;
        }

        //-----------------------
        /*
                select * from Zastepstwa Z
                left outer join Monity M on M.Typ = 'ZAST_A' and M.EventId = Z.Id and M.UserId = Z.IdZastepowany
                where Z.Od = {0} and M.Id is null or CAST(FLOOR(CAST(M.Data AS FLOAT)) AS DATETIME) <> CAST(FLOOR(CAST(getdate() AS FLOAT)) AS DATETIME)
         */

        public static DataSet GetStartujace()
        {
            return db.getDataSet(String.Format(@"
                select * from Zastepstwa Z
                left outer join Monity M on M.Typ = '{1}' and M.EventId = Z.Id and M.UserId = Z.IdZastepowany
                where Z.Od = {0} and M.Id is null
                ", db.sqlGetDate("GETDATE()"), Mailing.maZAST_ADD));
        }

        public static DataSet GetWygasajace()
        {
            return db.getDataSet(String.Format(@"
                select * from Zastepstwa Z
                left outer join Monity M on M.Typ = '{1}' and M.EventId = Z.Id and M.UserId = Z.IdZastepowany
                where Z.Do = {0} and M.Id is null
                ", db.sqlGetDate("GETDATE()"), Mailing.maZAST_END));
        }

        public static DataSet GetMonity()
        {
            const int monitDays = 3;
            return db.getDataSet(String.Format(@"
                select * from Zastepstwa Z
                left outer join Monity M on M.Typ = '{2}' and M.EventId = Z.Id and M.UserId = Z.IdZastepowany
                where DATEADD(DAY, {1}, Z.Do) = {0} and M.Id is null
                ", db.sqlCutTime2("GETDATE()"), -monitDays, Mailing.maZAST_MONIT));
        }

        public static void GetData(DataRow dr, out string zastId, out string idZastepowany, out string idZastepujacy, out string dataOd, out string dataDo)
        {
            zastId = db.getValue(dr, "Id");
            idZastepowany = db.getValue(dr, "IdZastepowany");
            idZastepujacy = db.getValue(dr, "IdZastepujacy");
            dataOd = Base.DateToStr(dr["Od"]);
            dataDo = Base.DateToStr(dr["Do"]);
        }
        //----------------------------------------------------------------
        private void SetSelectSql()
        {
            const string select_KogoJa = "select R.Id, K.Login," +
                    "K.Nazwisko + ' ' + K.Imie as Zastepowany," +
                    "null as Zastepujacy," +
                    "R.Od, R.Do, R.IdZastepowany, R.IdZastepujacy " +
                "from Zastepstwa R " +
                    "left outer join Pracownicy K on K.Id = R.IdZastepowany " +
                "where R.IdZastepujacy = @IdKierownika and R.Do >= @Data " +
                //"order by R.Od";
                "order by Zastepowany";

            const string select_KtoMnie = "select R.Id, null as Login," +
                    "K.Nazwisko + ' ' + K.Imie as Zastepowany," +
                    "Z.Nazwisko + ' ' + Z.Imie as Zastepujacy," +
                    "R.Od, R.Do, R.IdZastepowany, R.IdZastepujacy " +
                "from Zastepstwa R " +
                    "left outer join Pracownicy K on K.Id = R.IdZastepowany " +
                    "left outer join Pracownicy Z on Z.Id = R.IdZastepujacy " +
                //"where R.IdZastepowany = @IdKierownika {0} " +
                "where R.IdZastepowany in ({0}) {1} " +
                //"order by R.Od";
                "order by Zastepowany, Zastepujacy";

            const string select_AsAdmin = "select R.Id, K.Login," +
                    "K.Nazwisko + ' ' + K.Imie as Zastepowany," +
                    "Z.Nazwisko + ' ' + Z.Imie as Zastepujacy," +
                    "R.Od, R.Do, R.IdZastepowany, R.IdZastepujacy " +
                "from Zastepstwa R " +
                    "left outer join Pracownicy K on K.Id = R.IdZastepowany " +
                    "left outer join Pracownicy Z on Z.Id = R.IdZastepujacy " +
                "{0} " +
                "order by Zastepowany, Zastepujacy, R.Od desc";

            switch (FMode)
            {
                default:
                case moKogoJa:
                    SqlDataSource1.SelectCommand = select_KogoJa;
                    break;
                case moKtoMnie: // i moich kierowników
                    SqlDataSource1.SelectCommand = String.Format(select_KtoMnie, KierIds, "and R.Do >= @Data");
                    break;
                case moKtoMnieHist:
                    SqlDataSource1.SelectCommand = String.Format(select_KtoMnie, KierIds, "and R.Do < @Data");
                    break;
                case moAsAdmin:
                    SqlDataSource1.SelectCommand = String.Format(select_AsAdmin, "where R.Do >= @Data");
                    break;
                case moAsAdminHist:
                    SqlDataSource1.SelectCommand = String.Format(select_AsAdmin, "where R.Do < @Data");
                    break;
            }
        }
        //----------------------------------------------------------------
        protected void lvZastepstwa_LayoutCreated(object sender, EventArgs e)
        {
            canSetZast = App.User.IsSetZastepstwa && App.User.IsOriginalUser; 

            SetSelectSql();
            switch (FMode)
            {
                case moKogoJa:
                    Tools.SetControlVisible(lvZastepstwa, "thZastepujacy", false);
                    Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                    break;
                case moKtoMnie:
                    if (!KierIsParent)  // nie jest kier wyższego szczebla
                    {
                        Tools.SetControlVisible(lvZastepstwa, "thZastepowany", false);
                        if (FDefSortColumn == 1) FDefSortColumn = 2;
                    }
                    if (!canSetZast)
                    {
                        Tools.SetControlVisible(lvZastepstwa, "thControl", false);
                        Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                    }
                    break;
                case moKtoMnieHist:
                    Tools.SetControlVisible(lvZastepstwa, "thControl", false);
                    //Tools.SetControlVisible(lvZastepstwa, "thZastepowany", false);
                    if (FDefSortColumn == 1) FDefSortColumn = 2;
                    Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                    break;
                case moAsAdmin:
                    break;
                case moAsAdminHist:
                    Tools.SetControlVisible(lvZastepstwa, "thControl", false);
                    Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                    break;
            }
            int sort = Sort;
            Report.ShowSort(lvZastepstwa, sort, sort > 0);
        }

        protected void lvZastepstwa_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Session[ClientID + sesSortId] = sort;  // unikalne co do kontrolki
        }

        protected void lvZastepstwa_DataBinding(object sender, EventArgs e)
        {
        }

        protected void lvZastepstwa_DataBound(object sender, EventArgs e)
        {
            /*            
            bool f = (lvZastepstwa.Items.Count == 0 && lvZastepstwa.InsertItemPosition != InsertItemPosition.None);
            Tools.SetControlVisible(lvZastepstwa, "btNewRecord", !f && FMode != moKogoJa && FMode != moKtoMnieHist);  
            */
            switch (FMode)
            {
                case moKogoJa:
                    Tools.SetControlVisible(lvZastepstwa.Controls[0], "btNewRecord", false);  // empty data template 
                    HidePager();
                    break;
                case moKtoMnie:
                    if (!canSetZast)
                    {
                        Tools.SetControlVisible(lvZastepstwa, "btNewRecord", false);
                        Tools.SetControlVisible(lvZastepstwa.Controls[0], "btNewRecord", false);  // empty data template 
                    }
                    if (!KierIsParent) 
                        HidePager();
                    break;
                case moKtoMnieHist:
                    Tools.SetControlVisible(lvZastepstwa.Controls[0], "btNewRecord", false);  // empty data template   
                    break;
                case moAsAdmin:
                    break;
                case moAsAdminHist:
                    Tools.SetControlVisible(lvZastepstwa.Controls[0], "btNewRecord", false);  // empty data template   
                    break;
            }
        }

        protected void lvZastepstwa_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
                InitItem(e);
            else if (e.Item.ItemType == ListViewItemType.EmptyItem)
                InitItem(e);
        }

        protected void lvZastepstwa_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
                InitItem(e);
        }

        private void SetZastepujacyList(DropDownList ddl, bool ddlKier)
        {
            DataSet ds = null;
            switch (FMode)
            {
                default:            // w każdym innym przypadku wyjdź!
                    return;         
                case moKtoMnie:     
                    if (ddlKier)
                        if (KierIsParent)   ds = App.GetPracAll(null, 1);  //siebie tez moge w tym przypadku
                        else                ds = App.GetPracAll(KierId, 1);
                    else                    ds = App.GetPracBelow(KierId, false, 0);
                    break;
                case moAsAdmin:
                    ds = App.GetPracAll(null, ddlKier ? 1 : 0);
                    break;
            }
            Tools.BindData(ddl, ds, "Pracownik", "Id", true, null);
            if (ddlKier)
                ddl.Items.Insert(1, new ListItem("pokaż pracowników ...","0"));     // po wybierz ...
            else
                ddl.Items.Insert(1, new ListItem("pokaż kierowników ...", "-1"));
        }

        private void InitDates(ListViewItemEventArgs e)
        {
            DateEdit deOd = (DateEdit)e.Item.FindControl("deOd");
            DateEdit deDo = (DateEdit)e.Item.FindControl("deDo");
            if (deOd != null && deDo != null)
                if (deOd.Date == null)
                {
                    deOd.Date = DateTime.Today;
                    if (deDo.Date == null)
                        deDo.Date = DateTime.Today.AddDays(7);
                }
        }

        private bool InitItem(ListViewItemEventArgs e)  // czy dany item jest edit item
        {
            switch (FMode)
            {
                case moKogoJa:
                    Tools.SetControlVisible(e.Item, "tdZastepujacy", false);
                    Tools.SetControlVisible(e.Item, "btZastap", true);
                    Tools.SetControlVisible(e.Item, "EditButton", false);
                    Tools.SetControlVisible(e.Item, "DeleteButton", false);
                    break;
                case moKtoMnie:     // przy edycji nie można zmienić kierowników !!!, trzeba usunąć i założyc od nowa !!!
                    if (e.Item.ItemType == ListViewItemType.InsertItem)  // po kliknieciu Zapisz z insera wchodzi tu i musi odtworzyć ddl
                    {
                        if (KierIsParent)
                        {
                            if (dsKier == null) dsKier = App.GetPracBelow(KierId, true, 1);
                            Tools.BindData(e.Item, "ddlZastepowany", dsKier, "Pracownik", "Id", false, KierId);   // wszyscy podlegli kierownicy
                        }
                        DropDownList ddl = (DropDownList)e.Item.FindControl("ddlZastepujacy");
                        SetZastepujacyList(ddl, true);
                        Tools.SetControlEnabled(lvZastepstwa, "btNewRecord", false);
                        InitDates(e);
                    }
                    else
                    {
                        if (!canSetZast)
                            Tools.SetControlVisible(e.Item, "tdControl", false);
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                    }
                    if (!KierIsParent)
                        Tools.SetControlVisible(e.Item, "tdZastepowany", false);                        
                    break;
                case moKtoMnieHist:
                    Tools.SetControlVisible(e.Item, "tdControl", false);
                    //Tools.SetControlVisible(e.Item, "tdZastepowany", false);
                    Tools.SetText(e.Item, "lbNoData", "Brak historii zastępstw");
                    break;
                case moAsAdmin:
                    if (e.Item.ItemType == ListViewItemType.InsertItem)
                    {
                        DataSet dsKier = App.GetPracAll(null, 1);
                        Tools.BindData(e.Item, "ddlZastepowany", dsKier, "Pracownik", "Id", true, null);   // wszyscy kierownicy
                        DropDownList ddl = (DropDownList)e.Item.FindControl("ddlZastepujacy");
                        SetZastepujacyList(ddl, true);
                        Tools.SetControlEnabled(lvZastepstwa, "btNewRecord", false);
                        InitDates(e);    
                    }
                    else
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                    break;
                case moAsAdminHist:
                    Tools.SetControlVisible(e.Item, "tdControl", false);
                    Tools.SetText(e.Item, "lbNoData", "Brak historii zastępstw");
                    Tools.SetControlEnabled(e.Item, "btNewRecord", false);
                    break;
            }
            return false;
        }

        /*
            //----- select -----
            //----- edycja -----
            if (((ListViewDataItem)e.Item).DisplayIndex == lvZastepstwa.EditIndex && lvZastepstwa.EditItem != null)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                switch (FMode)
                {
                    case moKtoMnie:
                        Tools.SelectItem(e.Item, "ddlZastepujacy", drv["IdZastepujacy"]);
                        break;
                    case moAsAdmin:
                        Tools.SelectItem(e.Item, "ddlZastepowany", drv["IdZastepowany"]);
                        Tools.SelectItem(e.Item, "ddlZastepujacy", drv["IdZastepujacy"]);
                        break;
                }
                return true;
            }
            else
                return false;

         * int lim = Tools.GetListItemMode(e, lvZastepstwa);
            bool ed = lim == Tools.limEdit || lim == Tools.limInsert;
            
            switch (lim)
            {
                case Tools.limSelect:
                    break;
                case Tools.limInsert:
                    break;
                case Tools.limEdit:
                    break;
            }
        */

        private void HidePager()
        {
            DataPager dp = (DataPager)lvZastepstwa.FindControl("DataPager1");
            if (dp != null)
            {
                dp.SetPageProperties(0, Int32.MaxValue, false);
                dp.Visible = false;
            }
            Tools.SetControlVisible(lvZastepstwa, "trPager", false);
        }
    
        //----------------------
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
                lv.InsertItemPosition = InsertItemPosition.FirstItem;
                Tools.SetControlEnabled(lv, "btNewRecord", false);
            }
            else
            {
                lv.InsertItemPosition = InsertItemPosition.None;
                Tools.SetControlEnabled(lv, "btNewRecord", true);
                Tools.SetControlVisible(lv, "InsertButton", true);  // na empty data template ?
            }
        }

        protected void lvZastepstwa_PagePropertiesChanged(object sender, EventArgs e)
        {
            EnableControls(lvZastepstwa, false, false);
        }

        protected void lvZastepstwa_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = lvZastepstwa;
            switch (e.CommandName)
            {
                case "NewRecord":
                    EnableControls(lv, false, true);
                    break;
                case "Edit":
                    EnableControls(lv, true, false);
                    break;
                //case "Insert":    -> ItemInserted
                case "CancelInsert":
                    EnableControls(lv, false, false);
                    break;
                //----- custom -----
                case "Zastap":
                    HiddenField hid = (HiddenField)e.Item.FindControl("hidLogin");
                    if (hid != null)
                        if (String.IsNullOrEmpty(hid.Value))
                        {
                            Tools.ShowMessage(String.Format(
                                "Zastępstwo niemożliwe.\\nZastępowany użytkownik {0} nie ma skonfigurowanego dostępu do systemu.\\n\\nProszę skontaktować się z administratorem.",
                                Tools.GetText(e.Item, "lbZastepowany")));  // potem moze dodać logowanie po Id
                        }
                        else App.LoginAsUser(hid.Value);
                    break;
            }
        }
        //------------------------------------
        private bool ValidateKier(int? k1, int? k2)
        {
            if (k1 != null && k2 != null)
            {
                if (k1 != k2) return true; // ok!
                else Tools.ShowMessage("Osoba zastępująca nie może być taka sama jak zastępowana.");
            }
            else Tools.ShowMessage("Proszę wybrać osobę do zastępstwa.");
            return false;
        }

        //------
        int? k1 = null;
        int? k2 = null;

        private bool UpdateItem(EventArgs ea, ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            if (oldValues == null)  // tylko jak insert, dla update nie zmieniam kierowników !!! 
            {
                switch (FMode)
                {
                    default:
                        return false;    // w innych przypadkach update nie występuje
                    case moKtoMnie:
                        if (KierIsParent)
                            k1 = Tools.GetDdlSelectedValueInt(item, "ddlZastepowany");
                        else
                            k1 = Tools.StrToInt(user.Id, 0);
                        k2 = Tools.GetDdlSelectedValueInt(item, "ddlZastepujacy");
                        if (ValidateKier(k1, k2))
                        {
                            values["IdZastepowany"] = k1;
                            values["IdZastepujacy"] = k2;
                        }
                        else return false;
                        break;
                    case moAsAdmin:
                        k1 = Tools.GetDdlSelectedValueInt(item, "ddlZastepowany");
                        k2 = Tools.GetDdlSelectedValueInt(item, "ddlZastepujacy");
                        if (ValidateKier(k1, k2))
                        {
                            values["IdZastepowany"] = k1;
                            values["IdZastepujacy"] = k2;
                        }
                        else return false;
                        break;
                }
            }
            HiddenField hid = Tools.FindHidden(item, "hidSaveConfirm");
            if (hid.Value == "0")   //Cancel
            {
                Button bt = (Button)item.FindControl("btSave");
                Tools.MakeConfirmButton(bt, null);
                hid.Value = null;
            }
            else                    //bez pytania lub Ok
            {
                DateEdit deOd = (DateEdit)item.FindControl("deOd");
                DateEdit deDo = (DateEdit)item.FindControl("deDo");
                if (DateEdit.ValidateRange(deOd, deDo, Log.ZASTEPSTWO))
                {
                    bool confirm = (DateTime)deDo.Date >= DateTime.Today;
                    if (!confirm)
                        confirm = hid.Value == "1";
                    if (confirm)
                    {
                        Log.LogChanges(Log.ZASTEPSTWO, "Zastępstwo", ea);
                        return true;
                    }
                    else    // data wcześniejsza
                    {
                        Button bt = (Button)item.FindControl("btSave");
                        Tools.MakeConfirmButton(bt, "Data zakończenia zastępstwa jest wcześniejsza od dzisiejszej.\\nKontynuować ?", hid);
                        Tools.ExecOnStart2("btSaveClick", String.Format("doClick('{0}');", bt.ClientID));
                    }
                }
            }
            return false;
        }

        string IdZastepowany(ListViewItem item)
        {
            if (item != null)
            {
                HiddenField hid = Tools.FindHidden(item, "hidIdZastepowany");
                if (hid != null) return hid.Value;
                //else return Tools.GetDdlSelectedValueInt(item, "ddlZastepowany").ToString(); dla inserta
            }
            return null;
        }

        string IdZastepujacy(ListViewItem item)
        {
            if (item != null)
            {
                HiddenField hid = Tools.FindHidden(item, "hidIdZastepujacy");
                if (hid != null) return hid.Value;
                //else return Tools.GetDdlSelectedValueInt(item, "ddlZastepujacy").ToString(); dla inserta
            }
            return null;
        }

        string DataOd(ListViewItem item)
        {
            if (item != null)
            {
                DateEdit de = (DateEdit)item.FindControl("deOd");
                if (de != null) return de.DateStr;
                else
                {
                    Label lb = Tools.FindLabel(item, "OdLabel");
                    if (lb != null) return lb.Text;
                }
            }
            return null;
        }

        string DataDo(ListViewItem item)
        {
            if (item != null)
            {
                DateEdit de = (DateEdit)item.FindControl("deDo");
                if (de != null) return de.DateStr;
                else
                {
                    Label lb = Tools.FindLabel(item, "DoLabel");
                    if (lb != null) return lb.Text;
                }
            }
            return null;
        }
        //---------------------
        string id1 = null;
        string id2 = null;
        string d1 = null;
        string d2 = null;

        protected void lvZastepstwa_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(e, lvZastepstwa.EditItem, e.OldValues, e.NewValues);
        }

        protected void lvZastepstwa_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e, e.Item, null, e.Values);
        }

        protected void lvZastepstwa_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            id1 = IdZastepowany(lvZastepstwa.Items[e.ItemIndex]);
            id2 = IdZastepujacy(lvZastepstwa.Items[e.ItemIndex]);
            d1 = DataOd(lvZastepstwa.Items[e.ItemIndex]);
            d2 = DataDo(lvZastepstwa.Items[e.ItemIndex]);

            Log.LogChanges(Log.ZASTEPSTWO, "Zastępstwo", e);
        }
        //----------------------------------------------------------------
        protected void lvZastepstwa_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            if (!Tools.Equals(e.OldValues, e.NewValues))
            {
                id1 = IdZastepowany(lvZastepstwa.EditItem);
                id2 = IdZastepujacy(lvZastepstwa.EditItem);
                Mailing.EventZastepstwo(Mailing.maZAST_ADD, null,
                        id1,
                        id2,
                        Tools.DateToStr(e.NewValues["Od"]),
                        Tools.DateToStr(e.NewValues["Do"]),
                        Mailing.zaModify);
            }
        }

        protected void lvZastepstwa_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            EnableControls(lvZastepstwa, false, false);   // inaczej gasi jak e.Cancel w Inserting
            Mailing.EventZastepstwo(Mailing.maZAST_ADD, null,
                e.Values["IdZastepowany"].ToString(),
                e.Values["IdZastepujacy"].ToString(),
                Tools.DateToStr(e.Values["Od"]),
                Tools.DateToStr(e.Values["Do"]),
                Mailing.zaSet);
            if (k2 != null)                      
            {
                DataRow dr = db.getDataRow("select Kierownik, Login, Email, Nazwisko + ' ' + Imie as Pracownik from Pracownicy where Id = " + k2);
                bool k = db.getBool(dr, 0, true);
                string login = db.getValue(dr, 1);
                bool l = String.IsNullOrEmpty(login) || login.StartsWith("login_");
                bool m = db.isNull(dr, 2);
                string n = db.getValue(dr, 3);
                if (l)
                {
                    Tools.ShowMessage(String.Format("Uwaga!!!\\nPracownik: {0} nie ma skonfigurowanego dostępu do systemu.\\nProszę skontaktować się z administratorem.", n));
                    // mail do admina
                }
            }
        }

        protected void lvZastepstwa_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            Mailing.EventZastepstwo(Mailing.maZAST_END, null, id1, id2, d1, d2, Mailing.zaDel);
        }
        //----------------------------------------------------------------
        protected void ddlZastepujacy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            switch (ddl.SelectedValue)
            {
                case "0":
                    SetZastepujacyList(ddl, false);
                    break;
                case "-1":
                    SetZastepujacyList(ddl, true);
                    break;
            }
        }
        //----------------------------------------------------------------
        public int Mode
        { 
            get { return FMode; }
            set { FMode = value; }
        }

        public string KierId
        {
            get { return Tools.GetViewStateStr(ViewState["kierId"]); }
            set 
            {
                hidKierId.Value = value;
                ViewState["kierId"] = value; 
            }
        }

        public string KierIds
        {
            get { return Tools.GetViewStateStr(ViewState["kierIds"]); }
            set { ViewState["kierIds"] = value; }
        }

        public bool KierIsParent
        {
            get { return Tools.GetViewStateBool(ViewState["kparent"], false); }
            set { ViewState["kparent"] = value; }
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

    }
}
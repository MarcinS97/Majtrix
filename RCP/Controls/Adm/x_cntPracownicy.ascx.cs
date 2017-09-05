using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class x_cntPracownicy: System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        public int FMode = 0;
        public int FPageSize = 10;

        const int moEdit = 0;   // do edycji w konfiguracji
        const int moSelect = 1; // do podpinania do struktury

        const int maxRights = 21;   // 1..21 kolumnmy, checkboxy, linkbuttony, 0-mailing w osobnej kolumnie

        bool canSetRights = false;
        bool ro = false;

        int liMode = 0;

        const int rightsCount = 14;     // max=21
        
        public static object [,] rights = new object[rightsCount,2] {                     // nie używać '-' w opisach !!!
             {AppUser._rAdmin,               "A - Administracja"}                                        // rAdmin                 = 0; // administrator
            ,{AppUser.rRights,              "R - Nadawanie uprawnień"}                                  // rRights                = 1; // prawo do nadawania uprawnień        
            ,{AppUser.rTester,              "T - Testowanie nowych funkcjonalności"}                                           // _rTester               = 2; //<<<<<<<< testy cc
            ,{AppUser.rKwitekAdm,           "AK - Administracja kwitkiem płacowym"}                      // rKwitekAdm             = 3; //<<<<<<<< podglad pracownika z listy

            ,{AppUser.rRepCzasPracy,        "CR - Raport czasu pracy RCP"}                               // rRepCzasPracy          = 4; // raport czasu pracy do testów dla Adama Peplinskiego, tymczas poźniej zmienić 
            ,{AppUser.rDostepniKierownicy,  "PO - Dostęp do obcych pracowników"}                         // rDostepniKierownicy    = 5; // mozliwosc wyboru kierownika i jego pracowników
            ,{AppUser.rRepPodzialCC,        "CC - Raporty podziału czasu na cc"}                         // rRepPodzialCC          = 6; // raport poczuału czasu na cc, uprawnienia do cc i class
            ,{AppUser.rRepPodzialCCKwoty,   "CK - Raporty podziału czasu na cc - kwoty"}                 // rRepPodzialCCKwoty     = 7; // raport poczuału czasu na cc z kwotami, uprawnienia do cc i class
            ,{AppUser.rRepPodzialCCAll,     "CA - Raporty podziału czasu na cc - cała klasyfikacja"}     // rRepPodzialCCAll       = 8; // pełen wgląd w dane, bez konieczności przypisywania cc i class - dla adminiów
            
            ,{AppUser.rKierParams,          "KP - Dostep do konfiguracji czasów przerw"}
            ,{AppUser.rPrzesuniecia,        "PP - Przesuwanie pracowników"}
            ,{AppUser.rPrzesunieciaAcc,     "PA - Akceptowanie przesunięć pracowników"}

            ,{AppUser.rWnioskiUrlopoweAcc,  "WU - Akceptowanie wniosków urlopowych"}
            ,{AppUser.rWnioskiUrlopoweAdm,  "WA - Administracja wniosków urlopowych"}

            
            /*
            {AppUser.rMailing,            "Otrzymywanie powiadomień"}
            ,{AppUser.rAdmin,               "A - Administracja"}
            ,{AppUser.rRights,              "U - Nadawanie uprawnień"}
            
            ,{AppUser.rSlowniki,            "S - Edycja słowników"}
             
            ,{AppUser.rRaporty,             "R - Raporty"}
            ,{AppUser.rZastepstwo,          "Z - Ustalenie zastępstwa"}
                         
            ,{AppUser.rWnioski,             "WN - Składanie wniosków"}
            ,{AppUser.rWnioskiAccNew,       "AN - Akceptowanie wniosków o dodanie jorg."}
            ,{AppUser.rWnioskiAccDel,       "AD - Akceptowanie wniosków o usunięcie jorg."}
            ,{AppUser.rWnioskiAccMod,       "AM - Akceptowanie wniosków o modyfikację jorg."}
                         
            ,{AppUser.rReadOnly,            "RO - Tryb podglądu danych"}
            ,{AppUser.rSuperuser,           "K - Konfiguracja aplikacji"}
             */
        };




        //-----------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvPracownicy, 0);
            Tools.PrepareSorting(lvPracownicy, 41, 50);
            Tools.PrepareRights(lvPracownicy, rights, AppUser.maxRight, 2);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*
                DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                if (dp != null)
                    dp.PageSize = FPageSize;
                 */
            }
        }

        public string GetEditColSpan()
        {
            return (rightsCount + 6).ToString();
        }

        //----------------------------------------------------
        private void TriggerSelectedChanged()
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        protected void lvPracownicy_SelectedChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }
        //----------------------------------------------------
        public string PrepNazwisko(object ni)
        {
            return Tools.PrepareName(ni.ToString());
        }

        public string GetStatus(object st)
        {
            int s = st == null ? App.stCurrent : (int)st; 
            return App.GetStatus(s);
        }
        //----------------------------------------------------
        /*
        private void prepareRightTh(int idx)
        {
            string c = idx.ToString();
            Tools.SetControlVisible(lvPracownicy, "thR" + c, true);
            LinkButton lbt = (LinkButton)lvPracownicy.FindControl("LinkButton" + c);
            if (lbt != null)
            {
                lbt.Visible = true;

                int right = (int)rights[idx, 0];
                string name = rights[idx, 1].ToString();
                if (String.IsNullOrEmpty(lbt.Text))
                    lbt.Text = name.Substring(0, 2).Trim();     // nn - nnnnnn

                if (String.IsNullOrEmpty(lbt.ToolTip))
                    lbt.ToolTip = name;
                
                lbt.CommandName = "Sort";
                lbt.CommandArgument = String.Format("[SUBSTRING(Rights,{0},1)]", right);
            }
        }

        private void prepareRightTd(ListViewItem item, bool setText, int idx, char[] dbRights, bool enabled)    // td
        {
            string c = idx.ToString();          // dla select
            Control td = Tools.SetControlVisible(item, "tdR" + c, true);

            bool r = false;                     // dla select i edit/insert
            int right = (int)rights[idx, 0];
            string name = rights[idx, 1].ToString();
            CheckBox cb = (CheckBox)item.FindControl("cbR" + c);
            if (cb != null)
            {
                if (0 <= right && right < dbRights.Count())
                    r = dbRights[right] == AppUser.chHasRight;
                cb.Checked = r;
                if (setText)
                {
                    string[] nn = name.Split('-');
                    if (nn.Length > 1)
                        cb.Text = String.Format("<span>{0}</span> - {1}", nn[0], nn[1]);
                    else
                        cb.Text = name;
                }
                else
                    cb.ToolTip = name;

                //cb.Enabled = liMode != Tools.limSelect && canSetRights;
                cb.Enabled = enabled;
                cb.Visible = true;
            }

            if (td == null)
                Tools.SetControlVisible(item, "br" + c, true);
        }

        private void applyRight(ListViewItem item, int idx, ref char[] dbRights)
        {
            int right = (int)rights[idx, 0];
            CheckBox cb = (CheckBox)item.FindControl("cbR" + idx);
            if (cb != null)
                if (0 <= right && right <= AppUser.maxRights)
                    dbRights[right] = cb.Checked ? '1' : '0';
        }
         */
        //----------------------------------------------------
    
        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            canSetRights = App.User.HasRight(AppUser.rRights);
            ro = App.User.HasRight(AppUser.rReadOnly);
            if (FMode == moSelect)
            {
                Tools.SetControlVisible(lvPracownicy, "thSelect", true);
                Tools.SetControlVisible(lvPracownicy, "thControl", false);

                Tools.SetControlVisible(lvPracownicy, "thBlokada", false);
                Tools.SetControlVisible(lvPracownicy, "thMailing", false);
                Tools.SetControlVisible(lvPracownicy, "thContact", false);
                /*
                Tools.SetControlVisible(lvPracownicy, "thRights", false);
                for (int i = 1; i <= maxRights; i++)
                    Tools.SetControlVisible(lvPracownicy, "thR" + i.ToString(), false);
                 */ 
            }
                /*
            else
            {
                for (int i = 0; i < rightsCount; i++)
                    Tools.prepareRightTh(rights, lvPracownicy, i);
                Tools.SetColSpan(lvPracownicy, "thRights", rightsCount - 1);  // -1 bo mailing
            }
                 */
            SqlDataSource1.SelectCommand = SqlDataSource1.SelectCommand.Replace("select", "select " + Tools.RightsToSelectSql("A.Rights"));
        }

        protected void lvPracownicy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            liMode = Tools.GetListItemMode(e, lvPracownicy);
            if (e.Item.ItemType == ListViewItemType.InsertItem)
                InitItem(e);
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
                InitItem(e);
        }


        /*
        private void UpdateCount()
        {
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            if (dp != null) 
                Tools.SetText(lvPracownicy, "lbKierCount", dp.TotalRowCount.ToString());
        }
        */
        protected void lvPracownicy_DataBound(object sender, EventArgs e)
        {
            //UpdateCount();
        }
        
        //----------------------------------------------------
        private void EnableControls(ListView lv, bool edit, bool insert)
        {
            if (edit)
            {
                Tools.SetControlEnabled(lv, "btNewRecord", false);
            }
            else
            {
                Tools.SetControlEnabled(lv, "btNewRecord", true);
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

        protected void lvPracownicy_PagePropertiesChanged(object sender, EventArgs e)
        {
            EnableControls(lvPracownicy, false, false);
        }

        protected void lvPracownicy_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            EnableControls(lvPracownicy, false, false);
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
                    break;
                case "CancelInsert":
                    EnableControls(lv, false, false);
                    break;
                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("Pracownik", SortDirection.Ascending);
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



            }
        }
        //----------------------------------------------------
        /*
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
        */
        private void HidePager()
        {
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            if (dp != null)
            {
                dp.SetPageProperties(0, Int32.MaxValue, false);
                dp.Visible = false;
            }
            Tools.SetControlVisible(lvPracownicy, "trPager", false);
        }
        /*
        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["pagesize"] = ((DropDownList)sender).SelectedValue;  // jak Brak danych to nie ustawia i trzeba samemu
            DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
            SetPageSize(dp);
        }
        */
        protected void lvPracownicy_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvPracownicy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            string kierId = lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString();
            if (kierId == App.User.Id)
                App.User.Reload(false);
        }

        //----------------------------------------------------------
        private bool InitItem(ListViewItemEventArgs e)  // czy dany item jest edit item
        {
            //----- select -----
            //----- edycja -----
            //string stanowisko = null;
            //string umowa = null;
            string status = App.stNew.ToString();
            string dbRights;
            bool edit = false;
            bool insert = e.Item.ItemType == ListViewItemType.InsertItem;
            if (!insert)
            {
                if (FMode == moSelect)
                {
                    Tools.SetControlVisible(e.Item, "tdSelect", true);
                    Tools.SetControlVisible(e.Item, "tdControl", false);

                    //Tools.SetControlVisible(e.Item, "tdBlokada", false);
                    Tools.SetControlVisible(e.Item, "tdMailing", false);
                    Tools.SetControlVisible(e.Item, "tdContact", false);
                    /*
                    for (int i = 1; i <= AppUser.maxRights; i++)
                        Tools.SetControlVisible(e.Item, "tdR" + i.ToString(), false);
                    */
                }
                else
                {
                    Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                }

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                edit = ((ListViewDataItem)e.Item).DisplayIndex == lvPracownicy.EditIndex && lvPracownicy.EditItem != null;
                if (edit)
                {
                    //stanowisko = drv["Id_Stanowiska"].ToString();
                    //umowa = drv["Id_Gr_Zatr"].ToString();
                    status = drv["Status"].ToString();
                }
                dbRights = drv["Rights"].ToString();
            }
            else
            {
                //dbRights = new String('0', AppUser.maxRights + 1);  // domyślne
                dbRights = new String('0', rights.Length);  // domyślne
            }

            if (FMode == moEdit)
            {
                //----- rights -----
                Tools.SetText(e.Item, "lbRightsTitle", canSetRights ? "Uprawnienia" : "Uprawnienia - brak prawa do nadawania");

                char[] ra = dbRights.ToCharArray();
                bool ei = edit || insert;
                
                bool nselect = liMode != Tools.limSelect;
                bool enabled = nselect && canSetRights && !ro;

                for (int i = 0; i < rightsCount; i++)
                {
                    if ((int)rights[i, 0] == AppUser._rMailing)
                        Tools.prepareRightTd(e.Item, ei, i, ra, nselect);     // mailing niech zawsze będzie można ustawić
                    else
                        Tools.prepareRightTd(e.Item, ei, i, ra, enabled);
                }
            }
            //------------------
            if (edit || insert)
            {
                /*
                Tools.BindData(e.Item, "ddlStanowisko", 
                        db.getDataSet("select Id_Stanowiska, Nazwa_Stan from Stanowiska order by Nazwa_Stan"), 
                        "Nazwa_Stan", "Id_Stanowiska", true, stanowisko);                
                Tools.BindData(e.Item, "ddlRodzajUmowy",
                        db.getDataSet("select Id_Gr_Zatr, Rodzaj_Umowy from GrZatr order by Rodzaj_Umowy"), 
                        "Rodzaj_Umowy", "Id_Gr_Zatr", true, umowa);
                */ 
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                if (ddl != null) App.FillStatus(ddl, status, true);
                return true;
            }
            else
                return false;
        }

        private void LogChanges(IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            /*
            if (oldValues == null)  // insert
            {
                string v = Log.Value(values, "IdZastepowany") +
                           Log.Value(values, "IdZastepujacy") +
                           Log.Value(values, "Od") +
                           Log.Value(values, "Do");
                Log.Info(Log.ZASTEPSTWO, "Ustanowienie zastępstwa", v, Log.OK);
            }
            else    // update
            {
                string v = Log.Value(oldValues, values, "IdZastepowany") +
                           Log.Value(oldValues, values, "IdZastepujacy") +
                           Log.Value(oldValues, values, "Od") +
                           Log.Value(oldValues, values, "Do");
                Log.Info(Log.ZASTEPSTWO, "Modyfikacja zastępstwa", v, Log.OK);
            }
             */ 
        }
        //------------------------------------
        private bool UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            if (oldValues == null)  // tylko jak insert, dla update nie zmieniam kierowników !!! 
            {
                /*
                switch (FMode)
                {
                    case moKogoJa:      // nie ma miejsca póki co !!!
                        values["IdZastepowany"] = Tools.GetDdlSelectedValueInt(item, "ddlZastepowany");
                        values["IdZastepujacy"] = App.Master.user.Id;
                        break;
                    case moKtoMnie:
                        values["IdZastepowany"] = App.Master.user.Id;
                        values["IdZastepujacy"] = Tools.GetDdlSelectedValueInt(item, "ddlZastepujacy");
                        break;
                    case moAsAdmin:
                        values["IdZastepowany"] = Tools.GetDdlSelectedValueInt(item, "ddlZastepowany");
                        values["IdZastepujacy"] = Tools.GetDdlSelectedValueInt(item, "ddlZastepujacy");
                        break;
                }
                */
            }

            /*
            DateEdit deOd = (DateEdit)item.FindControl("deOd");
            DateEdit deDo = (DateEdit)item.FindControl("deDo");
            if (DateEdit.ValidateRange(deOd, deDo, Log.ZASTEPSTWO))
            {
                LogChanges(oldValues, values);
                return true;
            }
            */
            
            //----- struktura -----
            //values["Id_Stanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");
            //values["Id_Gr_zatr"] = Tools.GetDdlSelectedValueInt(item, "ddlRodzajUmowy");
            values["Status"] = Tools.GetDdlSelectedValueInt(item, "ddlStatus");
            //----- uprawnienia -----
            /*
            char[] ra = new char[AppUser.maxRights + 1];
            for (int i = 0; i <= AppUser.maxRights; i++)
                ra[i] = '0';

            for (int i = 0; i < rightsCount; i++)
                applyRight(item, i, ref ra);

            string rights = new string(ra);
            values["Rights"] = rights;
            */
            values["Rights"] = Tools.applyRights(item);
            return true;
        }

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvPracownicy.EditItem, e.OldValues, e.NewValues);
        }

        protected void lvPracownicy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values);
        }
        //------------------------

        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        /*
        public int PageSize
        {
            get { return FPageSize; }
            set { FPageSize = value; }
        }
        */



        public ListView List
        {
            get { return lvPracownicy; }
        }
    }
}
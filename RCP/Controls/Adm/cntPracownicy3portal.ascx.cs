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
using HRRcp.Controls.Przypisania;

namespace HRRcp.Controls
{
    public partial class cntPracownicy3portal: System.Web.UI.UserControl
    {
        public event EventHandler SelectedChanged;
        public event CommandEventHandler Command;

        const string sesPageIdx = "_ADMpidxt";

        public int FMode = 0;
        const int moEdit = 0;   // do edycji w konfiguracji
        const int moSelect = 1; // do podpinania do struktury

        const string lcPrzypisania = "0";
        const string lcUprawnienia = "1";

        const int maxRights = 40;   // 1..21 kolumnmy, checkboxy, linkbuttony, 0-mailing w osobnej kolumnie

        bool canSetRights = false;
        bool ro = false;

        const int rightsCount = 12 + 3;

        public static object[,] rights = new object[rightsCount, 2] {                     
            // {AppUser.rAdmin,               "A - Administracja"},                                      // rAdmin                 = 0; // administrator
             {AppUser.rRights,              "U - Nadawanie uprawnień"}                                   // rRights                = 1; // prawo do nadawania uprawnień        
            ,{AppUser.rTester,              "T - Testowanie nowych funkcjonalności"}                     // _rTester               = 2; //<<<<<<<< testy cc

            ,{AppUser.rPortalAdmin,         "PA - Administracja portalem"}                               // 
            ,{AppUser.rPortalArticles,      "PB - Administracja portalem - artykuły i pliki"}            // artykuły, gazetka, pliki
            
            ,{AppUser.rUbezpieczeniaAdm,    "UA - Administracja ubezpieczeniami"}
            ,{AppUser.rOgloszeniaAdm,       "OA - Administracja ogłoszeniami"}
            ,{AppUser.rOgloszeniaBlokada,   "OB - Blokada wystawiania ogłoszeń"}
            ,{AppUser.rWnioskiUrlopoweAdm,  "WX - Administracja wniosków urlopowych"}

          //,{AppUser.rKartyTmp,            "KT - Wydawanie kart tymczasowych"}
 
            ,{AppUser.rWnioskiUrlopowe,     "WU - Wnioski urlopowe"}
            ,{AppUser.rWnioskiUrlopoweAcc,  "WA - Akceptowanie wniosków urlopowych"}
            ,{AppUser.rWnioskiUrlopoweSub,  "WS - Dostęp do wniosków z podstuktury"}
            ,{AppUser.rWnioskiUrlopoweNoAccMail, 
                                            "WM - Mailing wniosków urlopowych"}                    // przy wyłączonym mailingu głównym 

            ,{AppUser.rPortalTmp,           "PO - Dostęp do portalu"}                                   //

            ,{AppUser.rInfokiosk,           "IK - Użytkownik - Infokiosk"}                              //
            ,{AppUser.rSuperuser,           "SU - Superuser (tryb developerski)"}
        };


        //-----------------------------------------------------
        protected void Page_Init(object sender, EventArgs e)
        {
#if PORTAL
            Tools.PrepareDicListView(lvPracownicy, Tools.ListViewMode.Bootstrap);
#else
            Tools.PrepareDicListView(lvPracownicy, 0);
#endif
            Tools.PrepareSorting(lvPracownicy, 31, 60);
            Tools.PrepareRights(null, rights, AppUser.maxRight, 2);
        }

        bool v = false;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidNaDzien.Value = Tools.DateToStr(DateTime.Today);
                DoSearch(true);     // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }

        public void Prepare(DateTime nadzien)
        {
            hidNaDzien.Value = Tools.DateToStr(nadzien);
            lvPracownicy.DataBind();
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

        public string GetOdDo(object dOd, object dDo)
        {
            return Tools.GetOdDo(dOd, dDo);
        }

        public string GetEditColSpan()   // celka na tabelkę przy edycji 
        {
            switch (ListContent)
            {
                default:
                case lcPrzypisania:
                    return (7).ToString();
                case lcUprawnienia:
                    return (rightsCount + 6).ToString();
            }
        }

        private void TriggerSelectedChanged()
        {
            bool s = lvPracownicy.SelectedIndex != -1;
            btPrzesuniecia.Enabled = s;
            btPlanPracy.Enabled = s;
            btAkceptacja.Enabled = s;

            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }

        private void TriggerCommand(string cmd, string arg)
        {
            if (Command != null)
            {
                CommandEventArgs e = new CommandEventArgs(cmd, arg);
                Command(this, e);
            }
        }

        private void Deselect()
        {
            SelectedRcpId = null;
            SelectedStrefaId = null;
            if (lvPracownicy.SelectedIndex != -1)
            {
                lvPracownicy.SelectedIndex = -1;
                TriggerSelectedChanged();
            }
        }











        //----- FILTER ----------------------------------
        private void PrepareSearch()
        {
            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
            Tools.ExecOnStart2("searchtrigprac", String.Format("startSearch('{0}','{1}');",
                tbSearch.ClientID, btSearch.ClientID));
            tbSearch.Focus();
        }

        public void FocusSearch()
        {
            tbSearch.Focus();
        }

        protected void tbSearch_TextChanged(object sender, EventArgs e)
        {
        }

        /*
        private string GetSearch(string field, string search)
        {
            bool s1 = search.StartsWith(" ");
            bool s2 = search.EndsWith(" ");
            int len = search.Length;

            if (s1 && s2 && len > 2) return String.Format("{0}='{1}'", field, search.Substring(1, len-2));
            else
                if (s1 && len > 1) 
                    return String.Format("{0} like '{1}%'", field, search.Substring(1, len-1));
                else if (s2 && len > 1) 
                    return String.Format("{0} like '%{1}'", field, search.Substring(0, len-1));
                else
                    return String.Format("{0} like '%{1}%'", field, search.Trim());
        }
        */



        private string SetFilterExpr(bool resetPager)
        {
            string filter;
            string f1 = tabFilter.SelectedValue.Trim();
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = f1;
            }
            else
            {
                //Tools.ExecOnStart2("searchfocus", String.Format("$('#{0}').focus();", tbSearch.ClientID));
                string f2;
                string[] words = Tools.RemoveDblSpaces(tbSearch.Text.Trim()).Split(' ');   // nie trzeba sprawdzać czy words[i] != ''
                if (words.Length == 1)
                {
                    /*
                    string search = tbSearch.Text;
                    bool s1 = search.StartsWith(" ");
                    bool s2 = search.EndsWith(" ");
                    int len = search.Length;
                    */
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or NrKarty like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#else
                    f2 = "(Nazwisko like '{0}%' or Imie like '{0}%' or KadryId like '{0}%' or RcpIdTxt like '{0}%' or KierNazwisko like '{0}%' or KierImie like '{0}%')";
#endif
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
#if SIEMENS
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or NrKarty like '{0}%' or NrKarty like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#else
                    f2 = "(Nazwisko like '{0}%' and Imie like '{1}%' or Nazwisko like '{1}%' and Imie like '{0}%' or KadryId like '{0}%' or KadryId like '{1}%' or RcpIdTxt like '{0}%' or RcpIdTxt like '{1}%' or KierNazwisko like '{0}%' and KierImie like '{1}%' or KierNazwisko like '{1}%' and KierImie like '{0}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
#endif
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
#if SIEMENS
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or NrKarty like '{{{0}}}%')", i);
#else
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or KadryId like '{{{0}}}%' or KierNazwisko like '{{{0}}}%' or KierImie like '{{{0}}}%' or RcpIdTxt like '{{{0}}}%')", i);
#endif
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvPracownicy);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
        }

        private string SetFilterExpr_1(bool resetPager)
        {
            string filter;
            string f1 = tabFilter.SelectedValue.Trim();
            SqlDataSource1.FilterParameters.Clear();
            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                filter = f1;
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
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            if (resetPager) Tools.ResetLetterPager(lvPracownicy);       //resetPager nie robić kiedy !IsPostback - w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
            return filter;
        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            SetFilterExpr(!init); 
            if (!init)
            {
                //lvPracownicy.DataBind();
                Deselect();
                /*
                if (lvPracownicy.Items.Count == 1) Select(0);
                else if (lvPracownicy.SelectedIndex != -1) Select(-1);
                 */
            }
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        protected void cnt_ChangeFilter(object sender, EventArgs e)
        {
            DoSearch(false);
        }


        
        /*      
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
        */





        //---------------------------------------
        private void ShowContentHeader()
        {
            switch (ListContent)
            {
                case lcPrzypisania:
                    break;
                case lcUprawnienia:
                    break;
            }
            bool cp = ListContent == lcPrzypisania;
            Tools.SetControlVisible(lvPracownicy, "thStrefa", cp);
            Tools.SetControlVisible(lvPracownicy, "thKierownik", cp);
            Tools.SetControlVisible(lvPracownicy, "thDzial", cp);

            bool cu = ListContent == lcUprawnienia;
            if (cu)
            {
                Tools.PrepareRightsTh(lvPracownicy);
            }
            else
            {
                Tools.SetControlVisible(lvPracownicy, "thRights", false);
                for (int i = 0; i < rightsCount; i++)
                {
                    string c = (i + 1).ToString();
                    Tools.SetControlVisible(lvPracownicy, "thR" + c, false);
                }
            }
            Tools.SetControlVisible(lvPracownicy, "th111", cu);  // adm
            Tools.SetControlVisible(lvPracownicy, "th112", cu);  // raporty
        }

        private void ShowContentLine(ListViewItemEventArgs e)
        {
            bool cp = ListContent == lcPrzypisania;
            Tools.SetControlVisible(e.Item, "tdStrefa", cp);
            Tools.SetControlVisible(e.Item, "tdKierownik", cp);
            Tools.SetControlVisible(e.Item, "tdDzial", cp);

            bool cu = ListContent == lcUprawnienia;     
            if (!cu)
                for (int i = 0; i < rightsCount; i++)
                {
                    string c = (i + 1).ToString();
                    Tools.SetControlVisible(e.Item, "tdR" + c, false);
                }
            Tools.SetControlVisible(e.Item, "td111", cu);  // adm
            Tools.SetControlVisible(e.Item, "td112", cu);  // raporty
        }

        protected void tabContent_MenuItemClick(object sender, MenuEventArgs e)
        {
            lvPracownicy.DataBind();
        }

        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {
            //FilterExpression = tabFilter.SelectedValue;
            SetFilterExpr(true);
        }



















        //---------------------------------------
        //bool cancelSelect = false;
        
        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    //if (!cancelSelect)
                    {
                        string p1, p2;
                        Tools.GetLineParams(e.CommandArgument.ToString(), out p1, out p2);
                        SelectedRcpId = p1;
                        SelectedStrefaId = p2;
                        //TriggerSelectedChanged();
                    }
                    break;
                case "DeSelect":
                    Deselect();
                    break;
                case "Edit":
                    //cancelSelect = true;
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    break;
                case "Update":
                    break;
                case "NewRecord":
                    lvPracownicy.EditIndex = -1;
                    Deselect();
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", false);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "Insert":  // <<<< dodać odświeżenie LetterPagera !!!
                    break;
                case "CancelInsert":
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "JUMP":
                    int idx = Tools.StrToInt(e.CommandArgument.ToString(), 0);
                    DataPager dp = (DataPager)lvPracownicy.FindControl("DataPager1");
                    lvPracownicy.Sort("NazwiskoImie", SortDirection.Ascending);
                    //lvPracownicy.SelectedIndex = idx;
                    idx = (idx / dp.PageSize) * dp.PageSize;  // bez tego wyswietli dana literke od gory a zwykły paginator ma inny topindex
                    dp.SetPageProperties(idx, dp.PageSize, true);

                    Deselect();
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
#if SPX
                        if (AppUser.UpdatePass(id, pesel, true))
                        //if (true)
#else
                        string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(pesel, AppUser.hashMethod);
                        if (db.update("Pracownicy", 0, "Pass", "Id=" + id, db.strParam(hash)))
#endif
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
                case "editrcpid":
                    id = Tools.GetDataKey(lvPracownicy, e);     // wywala się jak z InsertItem, bo nie ma tam DataListItem
                    cntKartyRcpPopup.Show(id);
                    break;
                case "editalg":
                    id = Tools.GetDataKey(lvPracownicy, e);
                    cntAlgorytmyPopup.Show(id);
                    break;
                case "editstan":
                    id = Tools.GetDataKey(lvPracownicy, e);
                    cntStanowiskaPopup.Show(id);
                    break;
            }
        }

        protected void lvPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerSelectedChanged();
        }
        //------------------------------------------
        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            canSetRights = App.User.HasRight(AppUser.rRights);
            ro = App.User.HasRight(AppUser.rReadOnly);
            if (SqlDataSource1.SelectCommand.StartsWith("select\r\n"))
                SqlDataSource1.SelectCommand = "select " + Tools.RightsToSelectSql("P.Rights") + SqlDataSource1.SelectCommand.Substring(6);  //select do sortowania po prawach
            SqlDataSource1.FilterExpression = FilterExpression;
        }
        
        protected void lvPracownicy_DataBinding(object sender, EventArgs e)
        {

        }

        protected void lvPracownicy_DataBound(object sender, EventArgs e)
        {
            ShowContentHeader();
            Tools.UpdateLetterPager(lvPracownicy);    // może być wywoływane automatycznie przy Tools.PrepareDicListView, ale szkoda czasu bo przy każdym dic
#if SIEMENS
            //Tools.SetControlVisible(lvPracownicy, "thKierownikBr", false);
            Tools.SetControlVisible(lvPracownicy, "thKierownikLine2", false);
            LinkButton lbt = lvPracownicy.FindControl("LinkButton34") as LinkButton;
            if (lbt != null)
                lbt.CommandArgument = "NrKarty";
#endif


            if (lvPracownicy.Items.Count == 1)
            {
                lvPracownicy.SelectedIndex = 0;
                TriggerSelectedChanged();
            }
        }

        protected void lvPracownicy_PreRender(object sender, EventArgs e)   //tylko tu mogę modyfikować kontrolki na EditItemTemplate, w innych funkcjach nawet jak znajdzie to nie potrafi ustawić visible
        {
        }
        //--------------------------------------------
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
#if SIEMENS
            SetTB(item, "KadryIdTextBox", 8, 80);
            SetTB(item, "tbKadryId2", 8, 80);
            SetTB(item, "RcpIdTextBox", 20, 80);
#else
            SetTB(item, "KadryIdTextBox", 5, 60);
            SetTB(item, "RcpIdTextBox", 5, 60);
#endif
            SetTB(item, "LoginTextBox", 20, 200);
            SetTB(item, "EmailTextBox", 200, -1);
            //SetTB(item, "StatusTextBox", 3, 50);
            DropDownList ddl = (DropDownList)item.FindControl("ddlStatus");
            App.FillStatus(ddl, null, true);
        }

        private void PrepareRights(ListViewItemEventArgs e, bool insert, bool edit)
        {
            string dbRights;
            if (insert)
                dbRights = new String('0', maxRights);  // domyślne
            else
            {
                DataRowView drv = Tools.GetDataRowView(e);
                dbRights = drv["Rights"].ToString();
            }
            
            Tools.SetText(e.Item, "lbRightsTitle", canSetRights ? "Uprawnienia" : "Uprawnienia - brak prawa do nadawania");

            char[] ra = dbRights.ToCharArray();

            bool ei = edit || insert;

            //bool nselect = liMode != Tools.limSelect;
            bool ie = insert || edit;
            bool enabled = ie && canSetRights && !ro;

            for (int i = 0; i < rightsCount; i++)
            {
                if ((int)rights[i, 0] == AppUser._rMailing)
                    Tools.prepareRightTd(e.Item, ei, i, ra, ie);     // mailing niech zawsze będzie można ustawić
                else
                    Tools.prepareRightTd(e.Item, ei, i, ra, enabled);
            }
        }
        //-------------------------------
        private void ddlSelect(Control item, string ddlName, object selValue, SqlDataSource sqlDS)
        {
            DropDownList ddl = item.FindControl(ddlName) as DropDownList;
            if (ddl != null)
            {
                string sv = selValue == null ? null : selValue.ToString();
                sqlDS.SelectParameters["selId"].DefaultValue = sv;
                ddl.DataSourceID = null;
                ddl.DataSource = sqlDS;
                ddl.DataBind();
            }
            Tools.SelectItem(ddl, selValue);
        }

        //private void PrepareButtons(ListViewItem item, string cntSelectOnClick, string btSelect, string cntEditOnClick, string btEdit)
        //{
        //    //Control cntS = item.FindControl(cntSelectOnClick) as Control;
        //    //Control cntE = item.FindControl(cntEditOnClick) as Control;
        //    //if (cntS != null && btS != null && cntE != null && btE != null)
        //    Control btS = item.FindControl(btSelect);
        //    Control btE = item.FindControl(btEdit);
        //    if (btS != null && btE != null)
        //    {
        //        Tools.OnClick(item, cntSelectOnClick, "javascript:doClickSelectFFfix('{0}');", btS.ClientID);
        //        Tools.OnClick(item, cntEditOnClick,   "javascript:doClickEditFFfix('{0}');return false;", btE.ClientID);    
        //    }
        //}

        //private void PrepareButtons(ListViewItem item, string tr, string btSelect, string btEdit)
        //{
        //    Control btS = item.FindControl(btSelect);
        //    if (btS != null)
        //    {
        //        Tools.OnClick(item, tr, "javascript:doClickSelectFFfix('{0}');", btS.ClientID);
        //        Tools.OnClick(item, btEdit, "javascript:doClickEditFFfix2();", "");
        //    }
        //}
        
        private void PrepareItem(ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, lvPracownicy, out select, out edit, out insert);
            switch (lim)
            {
                case Tools.limSelect:
                    if (!create)
                    {
                        //Tools.OnClick(e.Item, "trLine", "NazwiskoLinkButton"); //"SelectButton");
                        //PrepareButtons(e.Item, "trLine", "NazwiskoLinkButton", "EditButton", "btEdit");
                        
                        //20161025
                        //PrepareButtons(e.Item, "trLine", "NazwiskoLinkButton", "EditButton");

                        ShowContentLine(e);
                        if (ListContent == lcUprawnienia)
                            PrepareRights(e, false, false);
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "tdKierownikLine2", false);
                        Tools.SetControlVisible(e.Item, "RcpLabel", false);
                        Tools.SetControlVisible(e.Item, "RcpLabel2", true);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#endif
                    }
                    break;
                case Tools.limEdit:
                    if (!create)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);

                        //string kid = drv["KadryId"].ToString();
                        //bool spoza = String.IsNullOrEmpty(kid);
                        //bool v = spoza || kid == "00000";   //zeby mozna było skasowac pusty root-rekord po imporcie

                        bool v = true;
                        Button bt = (Button)Tools.SetControlVisible(e.Item, "DeleteButton", v);   // zawsze można usunąć
                        if (bt != null && v) Tools.MakeConfirmDeleteRecordButton(bt);

                        PrepareEditControls(e.Item);

                        /*
                        object kid = drv["IdKierownika"];
                        DropDownList ddl = e.Item.FindControl("ddlKierownik") as DropDownList;
                        if (ddl != null)
                        {
                            SqlDataSourceKier.SelectParameters["IdKierownika"].DefaultValue = Tools.GetInt(kid, -1).ToString();
                            ddl.DataBind();
                        }
                        Tools.SelectItem(e.Item, "ddlKierownik", kid);
                         */

                        ddlSelect(e.Item, "ddlKierownik", drv["IdKierownika"], SqlDataSourceKierEdit);
                        ddlSelect(e.Item, "ddlStrefa", drv["RcpStrefaId"], SqlDataSourceStrefaEdit);
                        ddlSelect(e.Item, "ddlCommodity", drv["IdCommodity"], SqlDataSourceCommEdit);
                        ddlSelect(e.Item, "ddlArea", drv["IdArea"], SqlDataSourceAreaEdit);
                        ddlSelect(e.Item, "ddlPosition", drv["IdPosition"], SqlDataSourcePosEdit);

                        //Tools.SelectItem(e.Item, "ddlKierownik", drv["IdKierownika"]);
                        //Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
                        //Tools.SelectItem(e.Item, "ddlCommodity", drv["IdCommodity"]);
                        //Tools.SelectItem(e.Item, "ddlArea", drv["IdArea"]);
                        //Tools.SelectItem(e.Item, "ddlPosition", drv["IdPosition"]);
                        
                        
                        /*
                        Tools.SelectItem(e.Item, "ddlDzial", drv["IdDzialu"]);
                        Tools.SelectItem(e.Item, "ddlStanowisko", drv["IdStanowiska"]);
                        Tools.SelectItem(e.Item, "ddlKierownik", drv["IdKierownika"]);
                        Tools.SelectItem(e.Item, "ddlStrefa", drv["RcpStrefaId"]);
                        Tools.SelectItem(e.Item, "ddlAlgorytm", drv["RcpAlgorytm"]);
                        
                        Tools.SelectItem(e.Item, "ddlLinia", drv["IdLinii"]);
                        Tools.SelectItem(e.Item, "ddlSplit", drv["GrSplitu"]);
                        */


                        object st = drv["Status"];
                        Tools.SelectItem(e.Item, "ddlStatus", st == null ? App.stPomin.ToString() : st.ToString());

                        bool kadm = App.User.HasRight(AppUser.rKwitekAdm) || App.User.HasRight(AppUser.rPortalAdmin);
                        bt = (Button)Tools.SetControlVisible(e.Item, "btKwitekPassReset", kadm);
                        if (bt != null)
                        {
#if PORTAL  
                            bt.Text = "Resetuj hasło";
#endif
                            if (kadm) Tools.MakeConfirmButton(bt, "Zmienić hasło na domyślne ?");
                        }
                        
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                        Tools.OnClick(e.Item, "RcpIdTextBox2", "ibtEditRcpId");
                        Tools.SetControlVisible(e.Item, "paCommodity", false);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#else
                        Tools.OnClick(e.Item, "RcpIdTextBox", "ibtEditRcpId");
#endif                   
                        PrepareRights(e, false, true);

                        cntPrzypisaniaParametry pp = e.Item.FindControl("cntParametry1") as cntPrzypisaniaParametry;
                        if (pp != null)
                        {
                            string pracId =  Tools.GetDataKeyEdited(lvPracownicy);
                            pp.Prepare(pracId, Tools.DateToStr(DateTime.Today));
                        }

                    }
                    break;
                case Tools.limInsert:
                    if (create)
                    {
                        PrepareEditControls(e.Item);
                        Tools.SelectItem(e.Item, "ddlStatus", App.stNew.ToString()); //App.stPomin.ToString());
#if SIEMENS
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox", false);
                        Tools.SetControlVisible(e.Item, "RcpIdTextBox2", true);
                        Tools.SetControlVisible(e.Item, "paCommodity", false);
                        Tools.SetControlVisible(e.Item, "paKadryId2", true);
#endif                   

                        TimeEdit te = e.Item.FindControl("teWymiar") as TimeEdit;
                        if (te != null) te.Seconds = 8 * 3600;
                        te = e.Item.FindControl("tePrzerwaWliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;
                        te = e.Item.FindControl("tePrzerwaNiewliczona") as TimeEdit;
                        if (te != null) te.Seconds = 0;

                        PrepareRights(e, true, false);
                    }
                    break;
            }
            //-----
            if (!create)
            {
                if (select || edit)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trLine");
                    if (tr != null)
                    {
                        DataRowView drv = Tools.GetDataRowView(e);
                        bool kier = db.getBool(drv["Kierownik"], false);
                        string status = drv["Status"].ToString();
                        Tools.AddClass(tr, (kier ? "kier " : "") + "status" + status);
                    }
                }
            }
        }

        protected void lvPracownicy_ItemCreated(object sender, ListViewItemEventArgs e) // tylko tu jest dostęp do InserItemTemplate
        {
            PrepareItem(e, true);
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            PrepareItem(e, false);
        }






        //--------------------------------------------
        private bool UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values, EventArgs ea)
        {
            values["IdDzialu"] = Tools.GetDdlSelectedValueInt(item, "ddlDzial");
            values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");
            values["RcpAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlAlgorytm");
            values["IdLinii"] = Tools.GetDdlSelectedValueInt(item, "ddlLinia");
            values["GrSplitu"] = Tools.GetDdlSelectedValueInt(item, "ddlSplit");
            




            string p1 = Tools.GetText(item, "hidIdPrzypisania");
            int? p2 = Tools.GetDdlSelectedValueInt(item, "ddlKierownik");
            int? p3 = Tools.GetDdlSelectedValueInt(item, "ddlStrefa");
            int? p4 = Tools.GetDdlSelectedValueInt(item, "ddlCommodity");
            int? p5 = Tools.GetDdlSelectedValueInt(item, "ddlArea");
            int? p6 = Tools.GetDdlSelectedValueInt(item, "ddlPosition");

            /* trzeba by jeszcze wymusić zeby validator się wyświetlił, na razie włączam na sztywno
            bool ppv = p1 == null && (p3 != null || p4 != null || p5 != null || p6 != null);   // coś z przypisania wypełnione oprócz kierownika
            Tools.SetControlEnabled(item, "rfvKierownik", ppv);
            if (ppv) 
                return false;
            */
            values["IdPrzypisania"] = p1;
            values["IdKierownika"]  = p2;
            values["RcpStrefaId"]   = p3;
            values["IdCommodity"]   = p4;
            values["IdArea"]        = p5;
            values["IdPosition"]    = p6;




            DateEdit de1 = item.FindControl("deZatr") as DateEdit;
            DateEdit de2 = item.FindControl("deZwol") as DateEdit;
            if (de1 != null && de2 != null)
            {
                if (de1.Date != null && de2.Date != null)
                {
                    DateTime d1 = (DateTime)de1.Date;
                    DateTime d2 = (DateTime)de2.Date;
                    if (d2 < d1)
                    {
                        Tools.ShowError("Data zwolnienia wcześniejsza od daty zatrudnienia.");
                        return false;
                    }
                }
            }


            //values["IdStanowiska"] = Tools.GetDdlSelectedValueInt(item, "ddlStanowisko");    // Tools.UpdateDdlValues przy prepare Dic to zrobi
            //values["Grupa"] = Tools.GetDdlSelectedValueInt(item, "ddlGrupa");
            //values["Klasyfikacja"] = Tools.GetDdlSelectedValueInt(item, "ddlKlasyfikacja");
            
            int? status = Tools.GetDdlSelectedValueInt(item, "ddlStatus");
            values["Status"] = status;
            values["Rights"] = Tools.applyRights(item);

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
            
            //string pid = lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString();
            //Log.LogChanges(Log.PRACOWNIK, "Pracownik: " + AppUser.GetNazwiskoImieNREW(pid), ea);
            return true;
        }

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            SqlDataSource1.UpdateParameters["AutorId"].DefaultValue = App.User.OriginalId;
            e.Cancel = !UpdateItem(lvPracownicy.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvPracownicy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            SqlDataSource1.InsertParameters["AutorId"].DefaultValue = App.User.OriginalId;
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        protected void lvPracownicy_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string pracId = Tools.GetDataKey(lvPracownicy, e);
            DataRow dr = db.getDataRow("select top 1 Data from PlanPracy where IdPracownika = " + pracId);
            if (dr != null)
            {
                Tools.ShowError("Dla pracownika istnieje plan pracy: {0}. Usunięcie niemożliwe.", (DateTime)db.getDateTime(dr, "Data"));
                e.Cancel = true;
            }
        }
        //-----
        protected void lvPracownicy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            AppUser user = AppUser.CreateOrGetSession();

            cntSplityWsp splity = lvPracownicy.EditItem.FindControl("cntSplityWsp2") as cntSplityWsp;
            if (splity != null)
                splity.Update();            
            
            if (lvPracownicy.EditIndex >= 0 && lvPracownicy.DataKeys[lvPracownicy.EditIndex].Value.ToString() == user.Id)
                user.Reload(false);
        }

        protected void lvPracownicy_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
            lvPracownicy.InsertItemPosition = InsertItemPosition.None;
        }
        //-----
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                cntSplityWsp splity = lvPracownicy.InsertItem.FindControl("cntSplityWsp2") as cntSplityWsp;
                if (splity != null)
                {
                    System.Data.Common.DbCommand command = e.Command;
                    //string pracId = command.Parameters["@IdPracownika"].Value.ToString();
                    string przId = command.Parameters["@IdPrzypisania"].Value.ToString();
                    splity.IdPrzypisania = przId;
                    splity.Update();
                }
            }
        }




        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Tools.DateOk(args.Value);
        }





        protected void lvPracownicy_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvPracownicy.SelectedIndex = e.NewEditIndex;
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
        }
        //---------------------------------------
        protected void cntKartyRcpPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
#if SIEMENS
                TextBox tb = lvPracownicy.EditItem.FindControl("RcpIdTextBox2") as TextBox;
#else
                TextBox tb = lvPracownicy.EditItem.FindControl("RcpIdTextBox") as TextBox;
#endif
                if (tb != null)
                {
                    //DataRow dr = db.getDataRow("select * from PracownicyKarty where "
                    tb.Text = cntKartyRcpPopup.CurrentRcpId;
                    tb.ToolTip = null;   // później zmienić ...
                }
            }
        }

        protected void cntAlgorytmyPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
                TextBox tb = Tools.SetTextBox(lvPracownicy.EditItem, "AlgorytmTextBox", cntAlgorytmyPopup.CurrentAlgorytm);
                if (tb != null) tb.ToolTip = cntAlgorytmyPopup.CurrentOdDo;
                /*
                tb = Tools.SetTextBox(lvPracownicy.EditItem, "StrefaTextBox", cntAlgorytmyPopup.CurrentStrefa);
                if (tb != null) tb.ToolTip = cntAlgorytmyPopup.CurrentOdDo;
                */

                TimeEdit te = lvPracownicy.EditItem.FindControl("teWymiar") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.WymiarCzasuSec;
                te = lvPracownicy.EditItem.FindControl("tePrzerwaWliczona") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.PrzerwaWliczonaSec;
                te = lvPracownicy.EditItem.FindControl("tePrzerwaNiewliczona") as TimeEdit;
                if (te != null) te.Seconds = cntAlgorytmyPopup.PrzerwaNiewliczonaSec;
            }
        }

        protected void cntStanowiskaPopup_Changed(object sender, EventArgs e)
        {
            if (lvPracownicy.EditItem != null)
            {
                DataRow dr = cntStanowiskaPopup.Current;
                Tools.SetTextBox(lvPracownicy.EditItem, "DzialTextBox", db.getValue(dr, "Dzial"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "StanowiskoTextBox", db.getValue(dr, "Stanowisko"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "GrupaTextBox", db.getValue(dr, "Grupa"), null);
                Tools.SetTextBox(lvPracownicy.EditItem, "KlasyfikacjaTextBox", db.getValue(dr, "Klasyfikacja"), null);
            }
        }

        //---------------------------------------
        protected void btPrzesuniecia_Click(object sender, EventArgs e)
        {
            TriggerCommand("przes", SelectedPracId);
        }

        protected void btPlanPracy_Click(object sender, EventArgs e)
        {
            TriggerCommand("plan", SelectedPracId);
        }

        protected void btAkceptacja_Click(object sender, EventArgs e)
        {
            TriggerCommand("ppacc", SelectedPracId);
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
        //----------------------------
        public string ListContent
        {
            set 
            {
                if (value != ListContent)
                {
                    Tools.SelectMenu(tabContent, value);
                    lvPracownicy.DataBind();
                }
            }
            get { return tabContent.SelectedValue; }
        }

        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                Deselect();
                lvPracownicy.EditIndex = -1;
                lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }


        
        
        
        
        protected void SqlDataSourceKierEdit_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }

        protected void SqlDataSourceKierEdit_DataBinding(object sender, EventArgs e)
        {

        }

        protected void lvPracownicy_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            //if (cancelSelect)
            //    e.Cancel = true;
        }

    }
}
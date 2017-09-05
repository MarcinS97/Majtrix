using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class x_AdmPracownicyControl : System.Web.UI.UserControl
    {
        const string sesSortId = "_ADMsort";
        const string sesPageIdx = "_ADMpidxt";
        const int maxSortCol = 12;
        private int FDefSortColumn = 1;
        private int FMode = 0;
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

        private static string GetSortField(int col)
        {
            string d = col < 0 ? " desc" : "";
            switch (Math.Abs(col))
            {
                default:
                case 0: return "";
                case 1: return "NazwiskoImie" + d;
                case 2: return "NR_EW" + d;
                case 3: return "Login" + d;
                case 4: return "Email" + d;
                case 5: return "Checked" + d;
                case 6: return "Kontroler1" + d;
                case 7: return "Kontroler2" + d;
                case 8: return "Operator" + d;
                case 9: return "Admin" + d;
                case 10: return "Status" + d;
                case 11: return "StanAnkiety" + d;
                case 12: return "P_Start" + d;
            }
        }

        public static string GetWhere(int mode)
        {
            switch (mode)
            {
                default:
                case 0: return null;
                case 1: return " where Status >= 0";
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
                    bool spoza = false;
                    Button bt = (Button)item.FindControl("DeleteButton");
                    if (bt != null)
                    {
                        spoza = String.IsNullOrEmpty(rowView["NR_EW"].ToString());
                        bt.Visible = spoza;
                        if (spoza)
                        {
                            Tools.MakeConfirmDeleteRecordButton(bt);
                            bt.CommandName = "Delete";
                            LinkButton lb = (LinkButton)item.FindControl("lbtCol10"); // stan ankiety
                            if (lb != null)
                                lb.Visible = false;
                        }
                    }
                    if (!spoza)   // ankieta tylko dla tych z KP
                    {
                        bt = (Button)item.FindControl("AnkietaButton");
                        if (bt != null)
                        {
                            /*aa
                            bt.Visible = !Base.getBool(rowView["Checked"], true) && // jak błąd to !true -> nie pokazuje [Ankieta]
                                          Base.getInt(rowView["StanAnkiety"], -1) == Ankieta.stPracownik;   // tylko jak na etapie Pracownik
                             */ 
                            if (bt.Visible)
                            {
                                bt.CommandName = "Ankieta";
                                bt.CommandArgument = rowView["Id"].ToString();
                            }
                        }
                    }
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
        protected void lvPracownicy_Sorting(object sender, ListViewSortEventArgs e)
        {
            //Report.ShowSort(sender, e, 9, 1);
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Session[ID + sesSortId] = sort;  // unikalne co do kontrolki
        }

        protected void lvPracownicy_LayoutCreated(object sender, EventArgs e)
        {
            /*
            if (ViewState["lvPSort"] == null)
            {
                ViewState["lvPSort"] = "1";
                Report.ShowSort(lvPracownicy, 1, true);
            }
            */
            int sort = Sort;
            Report.ShowSort(lvPracownicy, sort, sort > 0);
        }

        protected void lvPracownicy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)     
            {
                PrepareView(e.Item);
            }
        }

        protected void lvPracownicy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    lvPracownicy.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlVisible(lvPracownicy, "InsertButton", true);
                    break;
                case "Update":
                    // zmiana StanuAnkiety z reki - zoom i powrót - wyswietla stan poprzedni
                    /*
                    lvPracownicy.DataBind();  nic nie daje ..., wyglada jakby strona byla ładowana z cache mimo ustawienia Tools.NoCache na razie brak pomyslu 
                    SqlDataSource1.DataBind();
                     */
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
                    break;
            }
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
        //---------------------------------------
        public ListView List
        {
            get { return lvPracownicy; }
        }

        public int Mode
        {
            get { return FMode; }
            set { FMode = value; }
        }

        /*
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

        protected void lvPracownicy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            TextBox tb = (TextBox)lvPracownicy.EditItem.FindControl("StartTextBox");
            if (tb != null)
                if (!String.IsNullOrEmpty(tb.Text))
                    if (!Tools.DateOk(tb.Text))
                    {
                        e.Cancel = true;
                        tb.BackColor = Tools.warnColor;
                        Tools.ShowMessage("Niepoprawna data.");
                    }
                    else
                        tb.BackColor = Color.White;
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Tools.DateOk(args.Value);
        }

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
    }
}
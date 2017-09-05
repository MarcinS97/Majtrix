using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Collections;
using HRRcp.BadaniaWstepne.Templates;
using System.Collections.Specialized;
using HRRcp.Controls;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntBadaniaWst : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate LayoutTemplate { get; set; }
#if DEBUG
        void __DEBUGTEST()
        {
            var SelectCmd = this.SelectCmd;
            var FilterExpression = this.FilterExpression;
        }
#endif

        public int UC_DataColumnRightId { get; set; }
        public int UC_ZdjecieRightId { get; set; }

        //cntMenuZakres MenuZakres1
        //{
        //    get
        //    {
        //        return (cntMenuZakres)ListView1.FindControl("MenuZakres1");
        //    }
        //}
        bool canEdit;
        bool canAddDel
        {
            get
            {
                return App.User.IsAdmin || App.User.HasRight(AppUser.rBadaniaWstepneAddDel);
            }
        }
        private KeyValuePair<string, SortDirection> SortCol
        {
            set
            {
                ViewState["SelectCmd"] = value;
            }
            get
            {
                object obj = ViewState["SelectCmd"];
                if (obj == null)
                    return new KeyValuePair<string, SortDirection>(null, SortDirection.Ascending);
                return (KeyValuePair<string, SortDirection>)obj;
            }
        }
        private int PgSizeIndex
        {
            set
            {
                ViewState["PgSizeIndex"] = value;
            }
            get { return Tools.GetInt(ViewState["PgSizeIndex"], 0); }
        }
        private string PgSizeValue
        {
            set
            {
                ViewState["PgSizeValue"] = value;
            }
            get { return Tools.GetStr(ViewState["PgSizeValue"]); }
        }
        private string SelectCmd
        {
            set
            {
                ViewState["SelectCmd"] = value;
            }
            get { return Tools.GetStr(ViewState["SelectCmd"]); }
        }

        //private string ExcelCmd
        //{
        //    set { ViewState["ExcelCmd"] = value; }
        //    get { return Tools.GetStr(ViewState["ExcelCmd"]); }
        //}

        private string SelectCmdBase
        {
            set
            {
                ViewState["SelectCmdBase"] = value;
            }
            get { return Tools.GetStr(ViewState["SelectCmdBase"]); }
        }
        private string AdvFilter
        {
            set
            {
                ViewState["advFilter"] = value;
                //UpdateSelectCmd();
            }
            get { return Tools.GetStr(ViewState["advFilter"]); }
        }
        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                UpdateSelectCmd();
                //SqlDataSource1.FilterExpression = value;
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }
        private int LastOrderId
        {
            set
            {
                ViewState["LastOrderId"] = value;
            }
            get { return Tools.GetInt(ViewState["LastOrderId"], -1); }
        }
        private string LastOrderExp
        {
            set
            {
                ViewState["LastOrderExp"] = value;
            }
            get { return Tools.GetStr(ViewState["LastOrderExp"]); }
        }
        private SortDirection LastOrderDir
        {
            set
            {
                ViewState["LastOrderDir"] = value;
            }
            get { return (SortDirection)(ViewState["LastOrderDir"] ?? SortDirection.Ascending); }
        }
        private bool IsEmptyLastLoad
        {
            set
            {
                ViewState["IsEmptyLastLoad"] = value;
            }
            get { return Tools.GetBool(ViewState["IsEmptyLastLoad"], false); }
        }
        bool? _isOkresColumnVisible = null;
        public bool isOkresColumnVisible
        {
            get
            {
                if (!_isOkresColumnVisible.HasValue)
                    isOkresColumnVisible = string.IsNullOrEmpty(MenuZakres1._SelectedValue);
                return _isOkresColumnVisible.Value;
            }
            set
            {
                var cnt = ListView1.FindControl("ZakrThLT");
                //HtmlTableCellTh cnt = ListView1.FindControl("ZakrThLT") as HtmlTableCellTh;

                if (cnt != null)
                    cnt.Visible = value;
                _isOkresColumnVisible = value;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (ImNazFil.Visible)
                PrepareSearch();
            //ListView1.FindControl("InsertButton").Visible = canAddDel;
            //Tools.SetControlVisible(ListView1, "InsertButton", canAddDel);
            btAdd.Visible = canAddDel;
            base.OnPreRender(e);
        }

        CrColumnsCollection Columns;
        CrColumnsCollection ColumnsR;
        Dictionary<CrTemplateGroupCreator.TemplateTypes, IEnumerable<Func<Control, CrTemplateCreator.UpdInsArgsCnt>>> PreLoadedColumns;

        void PrepareColumns()
        {
            Func<int, CrTemplateGroupCreator.AccessTypes> f1;
            string Upr = db.getScalar("SELECT Uprawnienia FROM BadaniaWstKolUpr A RIGHT JOIN Pracownicy B on A.IdPrac = B.Id where B.Id = " + App.User.Id);
            /* uprawnienia admina jak innych
            if (App.User.IsAdmin)
                f1 = a => CrTemplateGroupCreator.AccessTypes.Write;
            else 
            */
            if (string.IsNullOrEmpty(Upr))
                f1 = a => CrTemplateGroupCreator.AccessTypes.Hide;
            else
                f1 = a => (a >= Upr.Length) ? CrTemplateGroupCreator.AccessTypes.Hide : (CrTemplateGroupCreator.AccessTypes)(Upr.ElementAt(a) - '0');

            Columns = new CrColumnsCollection(new string[] {
                //"LEFT(LTRIM(Main.Nazwisko), 1) as NazwiskoLetter"
            });

            


            ColumnsR = new CrColumnsCollection(new string[] {}); //T


            var tmp = PreColumns.Select(a => a.Compile(f1(a.RightId)));
            Columns.AddRange(tmp);

            tmp = Columns.Where(a => a.AccessType != CrTemplateGroupCreator.AccessTypes.Hide);
            ColumnsR.AddRange(tmp);            
            
            PreLoadedColumns = new Dictionary<CrTemplateGroupCreator.TemplateTypes, IEnumerable<Func<Control, CrTemplateCreator.UpdInsArgsCnt>>>();

            foreach (var Item in new CrTemplateGroupCreator.TemplateTypes[] { 
                CrTemplateGroupCreator.TemplateTypes.Item,
                CrTemplateGroupCreator.TemplateTypes.Edit,
                CrTemplateGroupCreator.TemplateTypes.Insert,
                CrTemplateGroupCreator.TemplateTypes.Filter,
            })
            {
                PreLoadedColumns.Add(Item,
                Columns.getColumns(Item, LoadControl).Select(a => (Func<Control, CrTemplateCreator.UpdInsArgsCnt>)(b => new CrTemplateCreator.UpdInsArgsCnt(a.Column, a.Template, b)))
                );
            }
        }

        void PrepateLv1Templates()
        {
            //ListView1.LayoutTemplate = LayoutTemplate;
            var Data = new Dictionary<BasicTemplate.DCreateFunc, Action<BasicTemplate>> {
                //{ LayoutTemplate_Create, a => ListView1.LayoutTemplate = a },
                { ItemTemplate_Create, a => ListView1.ItemTemplate = a },
                { EditTemplate_Create, a => ListView1.EditItemTemplate = a },
                { InsertTemplate_Create, a => ListView1.InsertItemTemplate = a }
            };

            foreach (var Item in Data)
            {
                var El = new BasicTemplate(LoadControl);
                Item.Value(El);
                El.Create += Item.Key;
            }
        }



        // nie wykonuje sie bo script w ascx
        protected void Page_Init(object sender, EventArgs e)
        {


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PrepareColumns();
            PrepateLv1Templates();
            PrepareFilterRepeater();
            if (!IsPostBack)
            {
                CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Filter;
                if (!Columns.Any(a => a.AccessType != CrTemplateGroupCreator.AccessTypes.Hide))
                {
                    MenuZakres1.Visible = ListView1.Visible = FilterBasic.Visible = false; // ukrywam tabele, zakladki i opcje wyszukiwania
                    LNoCols.Visible = true; // komunikat o braku uprawnien
                }
            }
            //canEdit = Columns.Any(a => a.AccessType == CrTemplateGroupCreator.AccessTypes.FullAccess);
            canEdit = Columns.Any(a => a.AccessType >= CrTemplateGroupCreator.AccessTypes.Write);

            Tools.PrepareDicListView(ListView1, 0);
            ListView1.LayoutCreated += ListView1_LayoutCreated;
            Tools.PrepareSorting2(ListView1, 2, Columns.Count + 1);
            Tools.SetSortColumn(ListView1, Tools.sortCol, LastOrderId);
        }


        void CreateSelectCmd()
        {
            List<string> Cols = new List<string>();
            List<string> Joins = new List<string>();
            foreach (var Item in Columns)
            {
                Item.TGroup.Creator.Creating(Cols, Joins);
            }
            string baseStr = string.Format(SqlDataSource1.SelectCommand,
                string.Join(",", Cols.Concat(Columns.SqlAddColumns).ToArray()),
                string.Join(" ", Joins.ToArray()),
                "{0}",
                "{1}"
                ); 
            SelectCmdBase = string.Format(baseStr, "{0}", "*");

            /*
            Cols.Clear();
            foreach (var Item in Columns.Where(a => a.TGroup.ContainsKey(CrTemplateGroupCreator.TemplateTypes.Item)).Where(a => !string.IsNullOrEmpty(a.SqlName)).OrderBy(a => a.Sort))
            {
                Cols.Add(Item.SqlName + " [" + Item.Header + "]");
            }
            ExcelCmd = string.Format(baseStr, "",
                string.Join(",", Cols.Concat(Columns.SqlAddColumns).ToArray()));
            */


            /*
            Cols.Clear();
            Joins.Clear();
            int idx1 = -1;
            foreach (var Item in Columns.Where(a => a.TGroup.ContainsKey(CrTemplateGroupCreator.TemplateTypes.Item)).Where(a => !string.IsNullOrEmpty(a.SqlName)).OrderBy(a => a.Sort))
            {
                Item.TGroup.Creator.Creating(Cols, Joins);
                int idx = Cols.Count - 1;
                //T plomba !!!
                if (idx1 != idx)
                {
                    idx1 = idx;
                    int p;
                    string c = Cols[idx].TrimEnd();
                    if (c.EndsWith("]"))
                    {
                        p = c.LastIndexOf("[");
                    }
                    else
                    {
                        p = c.LastIndexOf(" ") + 1;  // zakładam, ze nazwy nie są w []
                    }

                    //int p = c.ToLower().IndexOf(" as ");
                    c = String.Format("{0}[{1}]", c.Remove(p), Item.Header);
                    Cols[idx] = c;
                }
            }
            ExcelCmd = string.Format(SqlDataSource1.SelectCommand,
                string.Join(",", Cols.Concat(Columns.SqlAddColumns).ToArray()),
                string.Join(" ", Joins.ToArray()),
                "",
                string.Join(",", Cols.Concat(Columns.SqlAddColumns).ToArray())
                ); 
            */

            /*
            //3 wersja
            Cols.Clear();
            Joins.Clear();
            foreach (var Item in Columns)
            {
                Cols.Add(Item.SqlName + " [" + Item.Header + "]");
            }
            ExcelCmd = string.Format(baseStr, "",
                string.Join(",", Cols.Concat(Columns.SqlAddColumns).ToArray()));
            */


            UpdateSelectCmd();
        }

        public string GetExcelCmd()
        {
            string _data = MenuZakres1._SelectedZakr;
            string sql = null;

            List<string> Cols = new List<string>();
            List<string> Joins = new List<string>();
            
            
            foreach (var Item in ColumnsR)
            //foreach (var Item in PreLoadedColumns[CrTemplateGroupCreator.TemplateTypes.Item].Select))

            {
                if (Item.TGroup.Creator is CrTG7_DDLTextBox)
                {
                    Cols.Add(String.Format("Main.{0} as [{1}]", Item.SqlName, Item.Header));
                }
                else if (Item.TGroup.Creator is CrTemplateGroupCreatorLookup)
                {
                    Item.TGroup.Creator.Creating2(Cols, Joins, Item.Header);
                }
                else if (Item.TGroup.Creator is CrTG5_CC)
                {
                    Item.TGroup.Creator.Creating2(Cols, Joins, Item.Header);
                }
                else if (Item.TGroup.Creator is CrTG9_NazwImie)
                {
                }
                else
                {
                    Cols.Add(String.Format("Main.{0} as [{1}]", Item.SqlName, Item.Header));
                }
            }

            sql = string.Format(@"select {0} from BadaniaWst Main {1}",
                string.Join(",", Cols.ToArray()),
                string.Join(" ", Joins.ToArray())
                );

            if (String.IsNullOrEmpty(_data) || _data == cntMenuZakres3.ALL)
                return sql;
            else
            {
                //sql += " where Main.Zakr between '{0}' and DATEADD(D,6,'{0}')";// order by {1}";
                sql += " where Main.Zakr = '{0}'";// order by {1}";
                return String.Format(sql, _data);
            }
        }

        void UpdateSelectCmd()
        {
            string fe = (!string.IsNullOrEmpty(FilterExpression)) ? "WHERE " + FilterExpression : "";
            SelectCmd = string.Format(SelectCmdBase, fe);
            SqlDataSource1.SelectCommand = SelectCmd;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateSelectCmd();
                ListView1.Sort("CrTG9NazwImie", SortDirection.Ascending);

                bool adm = App.User.IsAdmin;
                btAdmin.Visible = adm;
                btImport.Visible = adm;
                Tools.MakeConfirmButton(btImportAsk, "Potwierdź import danych do RCP.");


                Tools.SetMainServiceUrl();
                Tools.ExecOnStart2("iniLUid", "initLastUpdateId();");
            }
            SqlDataSource1.SelectCommand = SelectCmd;
            //            Tools.ExecOnStart2("tabItemsHover", @"$('.tabItem').hover(
            //                                                    function () { $(this).addClass('tabHover'); },
            //                                                    function () { $(this).removeClass('tabHover'); });");
#if DEBUG
            __DEBUGTEST();
#endif
            /*
            if (ListView1.Items.Count == 0 && ListView1.EmptyDataTemplate != null)
            {
                ListView1.DataBound += (a, b) =>
                {
                    if(ListView1.Items.Count != 0)
                    {
                        LinkButton lb = (LinkButton)ListView1.FindControl("LinkButton" + Math.Abs(LastOrderId));
                        if(lb != null)
                        {
                            Report.ShowSort(ListView1, 
                                new ListViewSortEventArgs(lb.CommandArgument, LastOrderId > 0 ? SortDirection.Ascending : SortDirection.Descending), 
                                Columns.Count + 1, LastOrderId);
                        }
                    }
                };
            }
            DropDownList ddl = ListView1.FindControl("ddlLines") as DropDownList;
            if(ddl != null)
            {
                if (PgSizeIndex < 0)
                {
                    PgSizeIndex *= -1;
                    ddl.SelectedIndex = PgSizeIndex;
                }
                else
                {
                    PgSizeIndex = ddl.SelectedIndex;
                }
            }
            else
            {
                if (PgSizeIndex > 0)
                    PgSizeIndex *= -1;
            }
             */ 



			DropDownList ddl = ListView1.FindControl("ddlLines") as DropDownList;
            //if (ddl != null)
            //{
            //    if (IsEmptyLastLoad)
            //    {
            //        ddl.SelectedValue = PgSizeValue;
            //    }
            //    else
            //    {
            //        PgSizeValue = ddl.SelectedValue;
            //    }
            //}

        }
        void PrepareListView()
        {

        }
        private void DoSearch(bool init)
        {
            ListView1.EditIndex = -1;
            SetFilterExpr(!init);
        }
        private string SetFilterExpr(bool init)
        {
            string filter;
            string f1 = string.Format(MenuZakres1._SelectedValue, "Zakr");
            if (!string.IsNullOrEmpty(f1))
                f1 = "(" + f1 + ")";
            SqlDataSource1.FilterParameters.Clear();

            if (!string.IsNullOrEmpty(AdvFilter))
            {
                filter = AdvFilter + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            else if (String.IsNullOrEmpty(ImNazFil.Text))
            {
                filter = f1;
            }
            else
            {
                string f2;
                string[] words = Tools.RemoveDblSpaces(ImNazFil.Text.Trim()).Split(' ');
                if (words.Length == 1)
                {
                    f2 = string.Format("(Nazwisko like '{0}%' or Imie like '{0}%' or (ISNUMERIC('{0}') = 1 and NrEw = '{0}'))", (object[])words);
                }
                else if (words.Length == 2)
                {
                    f2 = string.Format(
                        @"( Imie like '{0}%' and Nazwisko like '{1}%' or 
                            Imie like '{0}%' and (ISNUMERIC('{1}') = 1 and NrEw = '{1}') or
                            Nazwisko like '{0}%' and Imie like '{1}%' or
                            Nazwisko like '{0}%' and (ISNUMERIC('{1}') = 1 and NrEw = '{1}') or 
                            (ISNUMERIC('{0}') = 1 and NrEw = '{0}') and Imie like '{1}%' or 
                            (ISNUMERIC('{0}') = 1 and NrEw = '{0}') and Nazwisko like '{1}%'
                            )", (object[])words);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("(Nazwisko like '{{{0}}}%' or Imie like '{{{0}}}%' or (ISNUMERIC('{{{0}}}') = 1 and NrEw = '{{{0}}}'))", i);
                    }
                    f2 = string.Format(String.Join(" and ", exp), (object[])words);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;
            //if (init) Tools.ResetLetterPager(ListView1);
            return filter;
        }
        private void PrepareSearch()
        {
            btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", ImNazFil.ClientID, btSearch.ClientID);
            Tools.ExecOnStart2("searchBadWst", String.Format(@"$(function() {{ startSearch('{0}','{1}'); }});",
                ImNazFil.ClientID, btSearch.ClientID));
        }
        protected void btSearch_Click(object sender, EventArgs e)
        {
            if(MenuZakres1.IsFilterSelected)
                MenuZakres1.SelectAll();
            AdvFilter = "";
            ListView1.Sort("Zakr", SortDirection.Descending);
            DoSearch(false);
        }
        public bool Prepare()
        {

            return true;
        }

        //protected void btExcel_Click(object sender, EventArgs e)
        //{
        //    string d = "";
        //    Menu m = MenuZakres1.FindControl("Menu1") as Menu;  // to powinno byc jako property w MenuZakres1
        //    if (m != null && MenuZakres1.SelectedIndex != -1)
        //    {
        //        long l;
        //        if (long.TryParse(m.Items[MenuZakres1.SelectedIndex].Text, out l))
        //            if (l != 0)
        //                d = " - " + Tools.DateToStr(DateTime.FromBinary(l));
        //    }
        //    string filename = String.Format("Badania wstępne{0}", d);
        //    Report.ExportCSV(filename, SqlDataSource1, null, null, true, false);
        //}
        protected void btExcel_Click(object sender, EventArgs e)
        {
            string d = MenuZakres1._SelectedZakr;
            if (String.IsNullOrEmpty(d) || d == cntMenuZakres3.ALL) d = "Wszyscy";
            string filename = String.Format("Badania wstępne - {0}", d);
            Report.ExportCSV(filename, GetExcelCmd(), null, null, true);
        }

        private int CheckImport(out string info)
        {
            DataSet ds = db.getDataSet(SqlDataSourceImport.SelectCommand);
            int cnt = db.getCount(ds);
            if (cnt > 0)
            {
                info = db.Join(ds, 0, "\n");
                return cnt;
            }
            info = null;
            return 0;
        }

        private bool Import()
        {
            DataSet ds = db.getDataSet(SqlDataSourceImport.UpdateCommand);
            DataRow dr = db.getRow(ds);
            int err    = db.getInt(dr, 0, -1);
            string msg = db.getValue(dr, 1);
            int step   = db.getInt(dr, 2, 0);
            int cnt    = db.getInt(dr, 3, 0);
            int lastid = db.getInt(dr, 4, 0);
            if (err == 0)
            {
                Log.Info(Log.BADWST_IMPORT, "BadaniaWst.Import", String.Format("lastid:{0} count:{1}", lastid, cnt));
                Tools.ShowMessage("Dane pracowników zostały zaimportowane.\nIlość: {0}.", cnt);
                return true;
            }
            else
            {
                Log.Error(Log.BADWST_IMPORT, "BadaniaWst.Import", String.Format("lastid:{0} count:{1} err:{2} step: {3} msg: {4}", lastid, cnt, err, step, msg));
                Tools.ShowError("Wystąpił błąd podczas importu:\n" + msg);
                return false;
            }
        }

        protected void btImport_Click(object sender, EventArgs e)
        {
            if (sender == btImportAsk)
            {
                string info;
                int cnt = CheckImport(out info);
                Tools.ShowConfirm(String.Format("Ilość pracowników do importu: {0}.\nCzy wykonać najpierw import danych z Asseco?\n\nUwaga!\nPo imporcie z Asseco ilość pracowników może ulec zmianie.", cnt), btImportBig, btImport);
            }
            else if (sender == btImportBig)
            {
                switch (Asseco.ImportAll(false, true))
                {
                    case 0:
                        string info;
                        int cnt = CheckImport(out info);
                        if (cnt > 0)
                            Tools.ShowConfirm(String.Format("Ilość pracowników po imporcie z Asseco: {0}.\nPotwierdź import do RCP.", cnt), btImport, null);
                        else
                            Tools.ShowMessage("Brak pracowników do importu.");
                        break;
                    case -1:
                        break;
                    case -2:  // pending - w ImportAll jest komunikat
                        break;
                }
            }
            else if (sender == btImport)
                Import();
        }


        /*
        protected void btImport_Click(object sender, EventArgs e)
        {
            if (sender == btImportAsk)
            {
                string info;
                int cnt = CheckImport(out info);
                if (cnt > 0)
                    Tools.ShowConfirm(String.Format("Potwierdź import danych. Ilość pracowników: {0}.", cnt), btImportAsk2, null);
                else
                    Tools.ShowMessage("Brak pracowników do importu.");
            }
            else if (sender == btImportAsk2)
            {
                Tools.ShowConfirm("Czy wykonać import danych z Asseco?", btImportBig, btImport);
            }
            else if (sender == btImportBig)
            {
                switch (Asseco.ImportAll(false))
                {
                    case 0: 
                        Import();
                        break;
                    case -1:
                        break;
                    case -2:  // pending - w ImportAll jest komunikat
                        break;
                }
            }
            else if (sender == btImport)
                Import();
        }
         */

        protected void btAdmin_Click(object sender, EventArgs e)
        {
            App.Redirect("BadaniaWstepne/RejestrAdmin.aspx");
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            ListView1.EditIndex = -1; // zapytac?
            ListView1.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        List<Action<ListViewUpdatedEventArgs>> UpdatedAct;
        List<Action<DataRow>> InsertedAct;
        List<Action> DeletedAct;

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Edit;
            string sql = "UPDATE BadaniaWst SET {0} WHERE id = {1}";
            string sql2 = "SELECT * from BadaniaWst WHERE id = {0}";
            UpdatedAct = new List<Action<ListViewUpdatedEventArgs>>();

            Control container = (sender as ListView).Items[e.ItemIndex];

            bool isError = false;
            foreach (var Item in Columns.getColumnsArgs(TT, LoadControl, container).Where(a => a.Column.AccessType >= CrTemplateGroupCreator.AccessTypes.Write))
            {
                Item.Creator.Updating(Item, e.NewValues);
                var Test = Item;
                UpdatedAct.Add(a => Test.Creator.Updated(Test, a));
                string v = Item.Template.ValidatorFunc(container);
                if ((BadaniaWstColumny.Instance.ReqCols.Contains(Item.Column.RightId) || Item.Column.AccessType == CrTemplateGroupCreator.AccessTypes.WriteReq) && Item.Template.IsEmptyFunc(container))
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if (lb != null)
                    {
                        lb.Visible = true;
                        lb.Text = "Błąd";
                    }
                    isError = true;
                }
                else if (!string.IsNullOrEmpty(v))
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if (lb != null)
                    {
                        lb.Visible = true;
                        lb.Text = v;
                    }
                    isError = true;
                }
                else
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if (lb != null)
                    {
                        lb.Visible = false;
                    }
                }
            }
            if (isError)
            {
                e.Cancel = true;
                return;
            }
            List<string> sqlSetters = new List<string>();
            foreach (var Item in e.NewValues.Cast<DictionaryEntry>())
            {
                sqlSetters.Add(string.Format("{0} = {1}", Item.Key, Item.Value));
            }
            string sqlN = string.Format(sql, string.Join(",", sqlSetters.ToArray()), e.Keys["id"]);
            SqlDataSource1.UpdateCommand = sqlN;

            ViewState[string.Format("{0}_UpdCmd", this.UniqueID)] = sqlN;

            var sqlOldValues = db.getDataRow(string.Format(sql2, e.Keys["id"]));
            Dictionary<string, object> oldValues = ViewState[string.Format("{0}_LastEditItem", (sender as Control).UniqueID)] as Dictionary<string, object>;
            List<string> sqlColumnsErr = new List<string>();
            foreach (var Item in e.NewValues.Keys.Cast<string>())
            {
                if (!sqlOldValues[Item].Equals(oldValues[Item]))
                {
                    e.Cancel = true;
                    Tools.ShowError("Dane zostały zmienione, nie możesz ich nadpisać.");
                }
            }
        }
        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            string sql = "INSERT INTO BadaniaWst({0},DataWpr) VALUES({1},GETDATE()); SET @LID = SCOPE_IDENTITY();";
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Insert;
            InsertedAct = new List<Action<DataRow>>();

            Control container = e.Item;
            List<string> nValidStrs = new List<string>();
            bool isError = false;
            foreach (var Item in Columns.getColumnsArgs(TT, LoadControl, container))
            {
                Item.Template.Creator.Inserting(Item, e.Values);
                var Test = Item;
                InsertedAct.Add(a => Test.Creator.Inserted(Test, a));
                string v = Item.Template.ValidatorFunc(container);
                if ((BadaniaWstColumny.Instance.ReqCols.Contains(Item.Column.RightId) || Item.Column.AccessType == CrTemplateGroupCreator.AccessTypes.WriteReq) && Item.Template.IsEmptyFunc(container))
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if(lb != null)
                    {
                        lb.Visible = true;
                        lb.Text = "Błąd";
                    }
                    isError = true;
                }
                else if (!string.IsNullOrEmpty(v))
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if (lb != null)
                    {
                        lb.Visible = true;
                        lb.Text = v;
                    }
                    isError = true;
                }
                else
                {
                    var lb = (container.FindControl("errLabel" + Item.Column.Sort) as Label);
                    if (lb != null)
                    {
                        lb.Visible = false;
                    }
                }
            }
            if (isError)
            {
                e.Cancel = true;
                return;
            }
            List<string> sqlColumns = new List<string>();
            List<string> sqlValues = new List<string>();
            foreach (var Item in e.Values.Cast<DictionaryEntry>())
            {
                sqlColumns.Add((string)Item.Key);
                sqlValues.Add((string)Item.Value);
            }

            //if (!MenuZakres1.IsFilterSelected)
            //{
            //    e.Cancel = true;
            //    Tools.ShowError("Zaznacz okres aby dodać rekord.");
            //}

            string strCols = string.Join(",", sqlColumns.ToArray());
            string strVals = string.Join(",", sqlValues.ToArray());
            SqlDataSource1.InsertCommand = string.Format(sql, strCols, strVals);
        }
        
        #region Templates
        void PrepareFilterRepeater()
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Filter;
            HtmlTable tab = new HtmlTable();
            tab.Attributes["class"] = "DynCols";
            HtmlTableRow tr;

            bool isFirst = true;

            //HtmlTableRow tr = new HtmlTableRow();
            //tab.Controls.Add(tr);
            //HtmlTableCellTh th = new HtmlTableCellTh();
            //th.InnerText = "Kolumna";
            //tr.Controls.Add(th);
            //th = new HtmlTableCellTh();
            //th.InnerText = "Wartość";
            //tr.Controls.Add(th);
            //th = new HtmlTableCellTh();
            //th.InnerText = "Pokaż puste";
            //tr.Controls.Add(th);
            foreach (var Item in Columns.getColumns(TT, LoadControl))
            {
                CrTemplate template = Item.Template;
                tr = new HtmlTableRow();
                HtmlTableCell td = new HtmlTableCell();
                td.InnerText = Item.Column.Header + ":";
                td.Attributes["class"] = "label";
                tr.Controls.Add(td);
                td = new HtmlTableCell();
                td.Attributes["class"] = Item.Column.Css[TT];
                template.Template.InstantiateIn(td);
                tr.Controls.Add(td);
                td = new HtmlTableCell();
                CheckBox cb = new CheckBox();
                cb.ID = "NNcb" + Item.Column.Sort;
                cb.Attributes["class"] = "FilterCB";
                td.Controls.Add(cb);
                if(isFirst)
                {
                    Label lb = new Label();
                    lb.Attributes["class"] = "FilterFirstCBLabel";
                    lb.Text = "brak wartości";
                    td.Controls.Add(lb);
                    isFirst = false;
                }
                tr.Controls.Add(td);
                tab.Controls.Add(tr);
            }
            PHFilter.Controls.Add(tab);
            PHFilter.DataBind();
        }
        
        void InsertTemplate_Create(Control container, Func<string, Control> LoadCnt)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Insert;
            HtmlTableRow tr = new HtmlTableRow();
            tr.Attributes["class"] = "iit";
            HtmlTableCell td = new HtmlTableCell();
            td.Attributes["class"] = "ItemContainer";
            //td.ColSpan = PreLoadedColumns[CrTemplateGroupCreator.TemplateTypes.Item].Count() + (isOkresColumnVisible ? 1 : 0) - 1;
            td.ColSpan = PreLoadedColumns[CrTemplateGroupCreator.TemplateTypes.Item].Count() + (isOkresColumnVisible ? 1 : 0);
            tr.Controls.Add(td);

            HtmlTable tab2 = new HtmlTable();
            tab2.Attributes["class"] = "DynColTab";

            bool showimg = false;
            CrColumn c = Columns.GetByRight(UC_ZdjecieRightId);
            if (c != null)
                showimg = c.AccessType >= CrTemplateGroupCreator.AccessTypes.Read;
            
            foreach (var Item in Columns.getColumns(TT, LoadControl))
            {
                HtmlTableRow tr2 = new HtmlTableRow();
                HtmlTableCell td2 = new HtmlTableCell();
                td2.InnerText = Item.Column.Header + ":";
                td2.Attributes["class"] = "label";                
                tr2.Controls.Add(td2);
                td2 = new HtmlTableCell();
                //td2.Attributes["class"] = Item.Column.Css[TT];
                td2.Attributes["class"] = string.Format("{0} {1}", Item.Column.Css[TT], "col" + Item.Column.Sort);
                Item.Template.Template.InstantiateIn(td2);
                var Test = Item;
                
                string zakr = MenuZakres1._SelectedZakr;
                if (Item.Column.RightId == UC_DataColumnRightId && !string.IsNullOrEmpty(zakr) && zakr != cntMenuZakres3.ALL)
                {
                    ((DateEdit)td2.Controls[0]).Date = DateTime.Parse(zakr);
                }

                Label errLabel = new Label();
                errLabel.ID = "errLabel" + Item.Column.Sort;
                errLabel.Visible = false;
                errLabel.CssClass = "error";
                td2.Controls.Add(errLabel);
                tr2.Controls.Add(td2);
                //td2 = new HtmlTableCell();
                //tr2.Controls.Add(td2);
                if (BadaniaWstColumny.Instance.ReqCols.Contains(Item.Column.RightId) || Item.Column.AccessType == CrTemplateGroupCreator.AccessTypes.WriteReq)
                    Item.ValidatorTemplate.InstantiateIn(td2);
                tab2.Controls.Add(tr2);
            }
            td.Controls.Add(tab2);


            if (showimg)
            {
                //td = new HtmlTableCell();
                //td.Attributes["class"] = "BWImgCell";
                Literal l1 = new Literal();
                l1.Text = "<div class=\"img1\"><div class=\"img2\">";
                Literal l2 = new Literal();
                l2.Text = "</div></div>";
                Image img = new Image();
                img.ID = "__imgtest";
                td.Controls.Add(l1);
                td.Controls.Add(img);
                td.Controls.Add(l2);
            }

            //tr.Controls.Add(td);
            td = new HtmlTableCell();

            Button button = new Button();
            td.Attributes["class"] = "control";
            button.ID = "InsertButton";
            button.Text = "Zapisz";
            button.ValidationGroup = "vg1";
            button.CommandName = "Insert";
            td.Controls.Add(button);

            Label ll1 = new Label();
            ll1.Text = "<br />";

            td.Controls.Add(ll1);
            button = new Button();
            button.ID = "btCancelInsert";
            button.Text = "Anuluj";
            button.CommandName = "CancelInsert";
            td.Controls.Add(button);

            tr.Controls.Add(td);
            container.Controls.Add(tr);
        }

        void EditTemplate_Create(Control container, Func<string, Control> LoadCnt)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Edit;
            HtmlTableRow tr = new HtmlTableRow();
            tr.Attributes["class"] = "eit";
            HtmlTableCell td = new HtmlTableCell();
            td.Attributes["class"] = "ItemContainer";
            //td.ColSpan = PreLoadedColumns[CrTemplateGroupCreator.TemplateTypes.Item].Count() + (isOkresColumnVisible ? 1 : 0) - 1;
            td.ColSpan = PreLoadedColumns[CrTemplateGroupCreator.TemplateTypes.Item].Count() + (isOkresColumnVisible ? 1 : 0);
            tr.Controls.Add(td);

            bool showimg = false;
            HtmlTable tab2 = new HtmlTable();
            tab2.Attributes["class"] = "DynColTab";
            foreach (var Item in Columns.getColumns(TT, LoadControl))
            {
                //if (Item.Column.RightId == zdjecieRightId && Item.Column.canedit)
                //    doimg = true;

                if (Item.Column.RightId == UC_ZdjecieRightId)
                {
                    showimg = Item.Column.AccessType >= CrTemplateGroupCreator.AccessTypes.Read;
                    if (Item.Column.AccessType <= CrTemplateGroupCreator.AccessTypes.Read)
                        continue;   //dla zdjęcia nie robimy, bo poniżej !!!
                }

                HtmlTableRow tr2 = new HtmlTableRow();
                HtmlTableCell td2 = new HtmlTableCell();
                td2.InnerText = Item.Column.Header + ":";
                td2.Attributes["class"] = "label";
                tr2.Controls.Add(td2);
                td2 = new HtmlTableCell();
                //td2.Attributes["class"] = Item.Column.Css[TT];
                td2.Attributes["class"] = string.Format("{0} {1}", Item.Column.Css[TT], "col" + Item.Column.Sort);
                var Test = Item;
                td2.DataBinding += (a, b) =>
                {
                    var UItem = (((a as Control).NamingContainer as ListViewDataItem).DataItem as DataRowView);
                    CrTemplateCreator.UpdInsArgsCnt Args = new CrTemplateCreator.UpdInsArgsCnt(Test.Column, Test.Template, (Control)a);
                    Args.Creator.SetControlValue(Args, UItem);
                };
                Item.Template.Template.InstantiateIn(td2);
                Label errLabel = new Label();
                errLabel.ID = "errLabel" + Item.Column.Sort;
                errLabel.Visible = false;
                errLabel.CssClass = "error";
                td2.Controls.Add(errLabel);
                tr2.Controls.Add(td2);
                //td2 = new HtmlTableCell();
                //tr2.Controls.Add(td2);
                if (BadaniaWstColumny.Instance.ReqCols.Contains(Item.Column.RightId) || Item.Column.AccessType == CrTemplateGroupCreator.AccessTypes.WriteReq)
                    Item.ValidatorTemplate.InstantiateIn(td2);
                tab2.Controls.Add(tr2);
            }
            td.Controls.Add(tab2);

            if (showimg)
            {
                //td = new HtmlTableCell();
                //td.Attributes["class"] = "BWImgCell";
                Literal l1 = new Literal();
                l1.Text = "<div class=\"img1\"><div class=\"img2\">";
                Literal l2 = new Literal();
                l2.Text = "</div></div>";
                Image img = new Image();
                img.ID = "__imgtest";
                td.Controls.Add(l1);
                td.Controls.Add(img);
                td.Controls.Add(l2);
            }

            //tr.Controls.Add(td);
            td = new HtmlTableCell();
            td.Attributes["class"] = "control";

            Label ll1 = new Label();
            ll1.Text = "<br />";
            Label ll2 = new Label();
            ll2.Text = "<br /><br />";

            Button button = new Button();
            button.ID = "UpdateButton";
            button.Text = "Zapisz";
            button.ValidationGroup = "vg1";
            button.CommandName = "Update";
            td.Controls.Add(button);
            td.Controls.Add(ll1);

            button = new Button();
            button.ID = "CancelButton";
            button.Text = "Anuluj";
            button.CommandName = "Cancel";
            td.Controls.Add(button);
            td.Controls.Add(ll2);

            button = new Button();
            button.ID = "DeleteButton";
            button.Text = "Usuń";
            button.CommandName = "Delete";
            button.Visible = canAddDel;
            td.Controls.Add(button);
            tr.Controls.Add(td);
            container.Controls.Add(tr);
        }
        
        void ItemTemplate_Create(Control container, Func<string, Control> LoadCnt)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Item;
            HtmlTableRow tr = new HtmlTableRow();
            tr.Attributes["class"] = "it";
            HtmlTableCell td;

            {
                td = new HtmlTableCell();
                td.Visible = isOkresColumnVisible;
                td.Attributes["class"] = "OkresFC";
                td.DataBinding += (a, b) =>
                {
                    var UItem = (((a as Control).NamingContainer as ListViewDataItem).DataItem as DataRowView);
                    (a as HtmlTableCell).InnerText = Tools.DateToStr(((DateTime)UItem["Zakr"]));
                };
                tr.Controls.Add(td);
            }
            foreach (var Item in Columns.getColumns(TT, LoadControl))
            {
                td = new HtmlTableCell();
                td.Attributes["class"] = Item.Column.Css[TT];
                var Test = Item;
                td.DataBinding += (a, b) =>
                {
                    var UItem = (((a as Control).NamingContainer as ListViewDataItem).DataItem as DataRowView);
                    CrTemplateCreator.UpdInsArgsCnt Args = new CrTemplateCreator.UpdInsArgsCnt(Test.Column, Test.Template, (Control)a);
                    Args.Creator.SetControlValue(Args, UItem);
                };
                Item.Template.Template.InstantiateIn(td);
                tr.Controls.Add(td);
            }

            if (canEdit)
            {
                td = new HtmlTableCell();
                td.Attributes["class"] = "control";

                Button button = new Button();
                button.ID = "EditButton";
                button.Text = "Edytuj";
                button.CommandName = "Edit";
                td.Controls.Add(button);

                tr.Controls.Add(td);
            }
            container.Controls.Add(tr);
        }

        void LayoutTemplate_Create(Control container, Func<string, Control> LoadCnt)
        {
     
        }
        #endregion
        protected int? EditingIndex { get; set; }
        protected void ListView1_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            EditingIndex = e.NewEditIndex;
        }
        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var obj = (ListViewDataItem)e.Item;
            if (!EditingIndex.HasValue || EditingIndex.Value != obj.DisplayIndex)
                return;

            Dictionary<string, object> Data = new Dictionary<string, object>();
            DataRowView Values = (obj.DataItem as DataRowView);

            foreach (var Item in Values.DataView.Table.Columns.Cast<DataColumn>())
            {
                Data.Add(Item.ColumnName, Values[Item.ColumnName]);
            }
            ViewState[string.Format("{0}_LastEditItem", (sender as Control).UniqueID)] = Data;
        }











        private void ShowTopFilter(bool visible)
        {
            if (visible)
            {
                Tools.ExecOnStart2("showTF", String.Format("$('#{0}').show();", ImNazFil.ClientID));
            }
            else
            {
                Tools.ExecOnStart2("showTF", String.Format("$('#{0}').val('').hide();", ImNazFil.ClientID));
                /*
                if (ImNazFil.Text.Trim() != null)
                {
                    ImNazFil.Text = null;
                    DoSearch(false);  // wyłaczam filtrowanie
                }
                */
            }
        }
        void showAdvFilter(bool show)
        {
            if (show)
            {
                FilterBasic.Visible = false;
                FilterAdv.Visible = true;
            }
            else
            {
                FilterBasic.Visible = true;
                FilterAdv.Visible = false;
            }
            ShowTopFilter(!show);
        }













        protected void buttSAdv_Click(object sender, EventArgs e)
        {
            showAdvFilter(true);
        }

        protected void buttSBasic_Click(object sender, EventArgs e)
        {
            ClearFilter();
            showAdvFilter(false);
        }
        
        protected void btNewData_Click(object sender, EventArgs e)
        {
            if (ListView1.EditIndex == -1)
            {
                UpdatePanel1.DataBind();
            }
        }

        private void ClearFilter()
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Filter;
            ImNazFil.Text = "";

            Control container = PHFilter;
            foreach (var Item in PreLoadedColumns[TT].Select(a => a(container)))
            {
                Item.Template.ValueSetter(container, null);
                ((CheckBox)container.FindControl("NNcb" + Item.Column.Sort)).Checked = false;
            }

            AdvFilter = "";
            DoSearch(false);
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }
        protected void btSearchAdv_Click(object sender, EventArgs e)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Filter;

            List<string> Values = new List<string>();
            Control container = PHFilter;
            foreach (var Item in PreLoadedColumns[TT].Select(a => a(container)))
            {
                Item.Column.TGroup.Creator.Filtering(Item, Values);
                bool nn = ((CheckBox)container.FindControl("NNcb" + Item.Column.Sort)).Checked;
                if (nn)
                    Item.Column.TGroup.Creator.IsEmpty(Item, Values);
            }
            List<string> sqlCmps = new List<string>();
            foreach (var Item in Values)
            {
                sqlCmps.Add(Item);
            }

            AdvFilter = string.Join(" AND ", sqlCmps.ToArray());
            if (!string.IsNullOrEmpty(AdvFilter))
                AdvFilter = "(" + AdvFilter + ")";
            if (MenuZakres1.IsFilterSelected)
                MenuZakres1.SelectAll();

            ListView1.Sort("Zakr", SortDirection.Descending);
            DoSearch(false);
            //showAdvFilter(false);
        }

        //--------------------------------------
        void IncrUpdateId()
        {
            Tools.ExecOnStart2("BadWstIncr", "IncrLUI();");
        }

        DateTime dataZakr = DateTime.MinValue;

        protected void ListView1_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            IncrUpdateId();
            foreach (var Item in UpdatedAct)
            {
                Item(e);
            }
            UpdatedAct.Clear();

            object d = e.NewValues["Zakr"];   //T jak nie będzie daty to znaczy ze user nie ma prawa do jej edycji wiec nie trzeba zmieniać zakładki :)
            if (d != null)
                MenuZakres1.CheckReload(d);
        }
        
        protected void ListView1_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            IncrUpdateId();

            MenuZakres1.CheckReload(e.Values["Zakr"]);   //T musi byc ...
        }
        
        protected void ListView1_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            // __LOG:
            // string logdelstr
            foreach (var Item in DeletedAct)
            {
                Item();
            }
            DeletedAct.Clear();
            IncrUpdateId();

            //MenuZakres1.CheckReload(dataZakr);
        }
        //------------------------------------

        protected void btNewData_Load(object sender, EventArgs e)
        {
            Tools.ExecOnStart2("btNewDataInit", string.Format("InitNDButton(\"{0}\");", (sender as Button).ClientID));
        }
        string logdelstr;
        protected void ListView1_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            var data = db.getDataRow("SELECT * FROM BadaniaWst where id = " + e.Keys["id"]);
            logdelstr = string.Join(",", data.Table.Columns.OfType<DataColumn>().Select(a => new { Column = a, Value = data[a.ColumnName] })
                .Where(a => a.Value != null && !(a.Value is DBNull))
                .Select(a => a.Column + "=" + a.Value).ToArray());
            //ld.Table.Columns
            SqlDataSource1.DeleteCommand = string.Format("DELETE BadaniaWst where id = {0}", e.Keys["id"]);

            string sql = @"SELECT * FROM BadaniaWst WHERE id = {0}";
            var dr = db.getDataRow(string.Format(sql, e.Keys["id"]));
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Edit;
            DeletedAct = new List<Action>();

            Control container = (sender as ListView).Items[e.ItemIndex];
            foreach (var Item in PreLoadedColumns[TT].Select(a => a(container)))
            {
                Item.Template.Creator.Deleting(Item, dr);
                DeletedAct.Add(() => Item.Creator.Deleted(Item, dr));
            }
        }
        CrColumnCreator[] PreColumns
        {
            get
            {
                //return TMPClass1.PreColumns;
                return BadaniaWstColumny.Instance.PreColumns;
            }
        }

        //-----------------------------------------
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            string sql = @"SELECT * FROM BadaniaWst WHERE id = {0}";
            var lid = e.Command.Parameters["@LID"].Value;
            var dr = db.getDataRow(string.Format(sql, lid));
            foreach (var Item in InsertedAct)
            {
                Item(dr);
            }
            InsertedAct.Clear();
        }

        //-----------------------------------------
        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {

        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

		/*
        protected void ListView1_Sorting(object sender, ListViewSortEventArgs e)
        {
            Control cnt = sender as LinkButton;
            if(cnt != null && cnt.ID.Substring(0, "LinkButton".Length) == "LinkButton")
            {
                LastOrderId = int.Parse((sender as Control).ID.Remove(0, "LinkButton".Length));
                if (e.SortDirection == SortDirection.Descending)
                    LastOrderId *= -1;
            }
        }
		*/
		protected void ListView1_Sorting(object sender, ListViewSortEventArgs e)
        {
            LastOrderExp = e.SortExpression;
            LastOrderDir = e.SortDirection;

            Control dc = ListView1.FindControl("DynCols");
            if (dc != null)
            {
                LinkButton lb = dc.Controls
                    .OfType<HtmlTableCellTh>().Select(a => a.Controls[0] as LinkButton)
                    .FirstOrDefault(a => a.CommandArgument == e.SortExpression);

                if (lb != null && lb.ID.Substring(0, "LinkButton".Length) == "LinkButton")
                {
                    LastOrderId = int.Parse(lb.ID.Remove(0, "LinkButton".Length));
                }
            }
        }

        protected void ListView1_Sorted(object sender, EventArgs e)
        {
            
        }

        protected void ListView1_DataBound(object sender, EventArgs e)
        {
            if (IsEmptyLastLoad && ListView1.Items.Count != 0)
            {
                Report.ShowSort(ListView1,
                    new ListViewSortEventArgs(LastOrderExp, LastOrderDir), Columns.Count + 1, LastOrderId);
                IsEmptyLastLoad = false;
            }
            else if(ListView1.Items.Count == 0 && ListView1.EmptyDataTemplate != null)
            {
                IsEmptyLastLoad = true;
            }
        }












        protected void MenuZakres1_SelectedItemChanged(object sender, EventArgs e)
        {
            //var tmp = (sender as cntMenuZakres).SelectedValue;
            var tmp = MenuZakres1._SelectedValue;

            isOkresColumnVisible = string.IsNullOrEmpty(MenuZakres1._SelectedValue);
            //if(isOkresColumnVisible)
            //    ListView1.Sort("Zakr", SortDirection.Ascending);
            ListView1.InsertItemPosition = InsertItemPosition.None;
            DoSearch(false);
        }









        protected void ListView1_LayoutCreated(object sender, EventArgs e)
        {
            const int defSize = 20;

			//20150905
            string s = PgSizeValue;
            if (!String.IsNullOrEmpty(s))
            {
                DataPager dp = (DataPager)(sender as ListView).FindControl("DataPager1");
                DropDownList ddl = (DropDownList)(sender as ListView).FindControl("ddlLines");
                if (s == "all")
                {
                    if (dp.PageSize != int.MaxValue)
                        dp.SetPageProperties(0, int.MaxValue, false);
                }
                else
                {
                    int size = Tools.StrToInt(s, defSize);
                    if (size == 0) size = defSize;
                    if (dp.PageSize != size)
                        dp.SetPageProperties(0, size, false);
                }
                //ddl.SelectedValue = PgSizeValue;
            }
        }

        protected void DynCols_Init(object sender, EventArgs e)
        {
            CrTemplateGroupCreator.TemplateTypes TT = CrTemplateGroupCreator.TemplateTypes.Item;
            HtmlTableRow tr = (HtmlTableRow)sender;
            if (tr != null)
            {
                HtmlTableCellTh th = new HtmlTableCellTh();
                {
                    th = new HtmlTableCellTh();
                    th.Visible = isOkresColumnVisible;
                    th.ID = "ZakrThLT";
                    LinkButton lb = new LinkButton();
                    lb.ID = "LinkButton1";
                    //lb.Text = "Tydzień";
                    lb.Text = "Data";
                    lb.CommandName = "Sort";
                    lb.CommandArgument = "Zakr";
                    th.Controls.Add(lb);
                    tr.Controls.Add(th);
                }
                int i = 2;
                foreach (var Item in Columns.getColumns(TT, LoadControl))
                {
                    th = new HtmlTableCellTh();
                    LinkButton lb = new LinkButton();
                    lb.ID = "LinkButton" + i++;
                    lb.Text = Item.Column.Header;
                    lb.CommandName = "Sort";
                    lb.CommandArgument = Item.Column.TGroup.Creator.OrderBy(Item.Column, Item.Template);
                    th.Controls.Add(lb);
                    tr.Controls.Add(th);
                }
                if (canEdit)
                {
                    th = new HtmlTableCellTh();
                    th.Attributes["class"] = "control";

                    th.ID = "thControl";

                    Button button = new Button();
                    button.ID = "InsertButton";
                    button.CommandName = "NewRecord";
                    button.Text = "Dodaj";
                    th.Controls.Add(button);
                    tr.Controls.Add(th);
                }
            }
        }
        public BadaniaWstColumny BWCHandle { get { return BadaniaWstColumny.Instance; } }

        protected void DataPager1_Init(object sender, EventArgs e)
        {
            
        }

        protected void DataPager1_Load(object sender, EventArgs e)
        {
			/*
            DataPager dp = (sender as DataPager);
            if (PgSizeValue == "all")
                dp.SetPageProperties(0, dp.TotalRowCount, false);
            else
            {
                int size = Tools.StrToInt(PgSizeValue, 10);
                if (size == 0) size = 10;
                dp.SetPageProperties((dp.StartRowIndex / size) * size, size, false);
            }
            //(sender as DataPager).SetPageProperties(0, 2, false);
			*/
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (sender as DropDownList);
            //20150905
			//PgSizeIndex = ddl.SelectedIndex;
            PgSizeValue = ddl.SelectedValue;
        }
    }


    public class BadaniaWstColumny
    {
        static readonly BadaniaWstColumny _instance = new BadaniaWstColumny();
        public static BadaniaWstColumny Instance
        {
            get
            {
                return _instance;
            }
        }
        private CrColumnCreator[] _PreColumns = null;
        private Dictionary<int, int> _PreColsH = null;
        private int[] _ReqCols = null;

        public BadaniaWstColumny()
        {
            var tmp = typeof(cntBadaniaWst).GetMethods();
            var tmp2 = tmp.Where(a => a.IsStatic).ToArray();
        }

        public void CreateColumns(CrColumnCreator[] PreColumns, Dictionary<int, int> PreColsH, int[] _ReqCols)
        {
            this._PreColumns = PreColumns;
            this._PreColsH = PreColsH;
            this._ReqCols = _ReqCols;
        }

        public CrColumnCreator[] PreColumns
        {
            get
            {
                return _PreColumns;
            }
        }
        public Dictionary<int, int> PreColsH
        {
            get
            {
                return _PreColsH;
            }
        }
        public int[] ReqCols
        {
            get
            {
                return _ReqCols;
            }
        }
    }

    /*

    public class TMPClass1
    {
        public static CrColumnCreator[] PreColumns { get; private set; }
        public static Dictionary<int, int> PreColsH { get; private set; }
        static TMPClass1()
        {
            PreColsH = new Dictionary<int, int>();
            var Tb50 = new CrTG0_TextBoxAll(50);
            var Tb50E = new CrTG0_TextBoxAll(50, true);
            var TbN5 = new CrTG1_TextBoxNum(5, false, false);
            var TbN11 = new CrTG1_TextBoxNum(11, false, false);
            var TbU = new CrTG2_MultiLineTB(200, 3);
            var Cb = new CrTG3_CheckBox();
            var cl = new CrTG4_Class();
            var cc = new CrTG5_CC();
            var de = new CrTG6_DateEdit();
            var de2 = new CrTG11_DateEditZakr();
            var st = new CrTG7_Stanowiska();
            var pr = new CrTG8_Przel();
            var ni = new CrTG9_NazwImie();
            //var fu = new CrTG10_FileUpload("~/BadaniaWstepne/images/");
            var fu = new CrTG10_FileUpload("~/Images/photos/");
            PreColumns = new CrColumnCreator[] {
                    new CrColumnCreator(0, 17, "Data", "Zakr", de2, "data"),
                    new CrColumnCreator(2, 1, "Imię", "Imie", Tb50E, "c312"),
                    new CrColumnCreator(10, 1, "Nazwisko", "Nazwisko", Tb50E, "c312"),
                    new CrColumnCreator(20, 2, "Nr Ewid.", "NrEw", TbN5, "c75"),
                    new CrColumnCreator(30, 3, "Uwagi dotyczace badań", "UwagiBadania", TbU, "c312"),
                    new CrColumnCreator(40, 4, "Numer identyfikatora", "IdOliwia", TbN5, "c75"),
                    new CrColumnCreator(50, 5, "Wniosek o zatrudnienie", "Zatrudnienie", Cb, "c75"),
                    new CrColumnCreator(60, 6, "Rekruter", "Rekruter", Tb50, "c312"),
                    new CrColumnCreator(70, 7, "Class", "Class", cl, "c312"),
                    new CrColumnCreator(80, 8, "CC", "", cc, "tnw"),
                    new CrColumnCreator(90, 9, "Data zatrudnienia", "DataZatr", de, "data"),
                    new CrColumnCreator(100, 10, "Stanowisko", "Stanowisko", st, "c312"),
                    new CrColumnCreator(110, 11, "Przełożony", "PrzelId", pr, "c312"),
                    new CrColumnCreator(120, 12, "Data badań", "DataBadan", de, "data"),
                    new CrColumnCreator(130, 13, "PESEL", "PESEL", TbN11, "c312"),
                    new CrColumnCreator(140, 14, "Numer RCP ID IT", "NrIT", TbN5, "c75"),
                    new CrColumnCreator(150, 15, "Status", "Status", Tb50, "c312"),
                    new CrColumnCreator(160, 16, "Zdjęcie", "Zdjecie", fu, "img"),
                    new CrColumnCreator(1, 1, "Pracownik", "CrTG9NazwImie", ni, "c312"),
                };
            PreColsH.Add(1, 1);
        }
    }
     */ 
}
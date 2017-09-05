using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Reports;
using System.Data;
using System.Text;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class cntPodzialLudzi : System.Web.UI.UserControl
    {
        const string ses_tab1 = "ses_PLt1";
        const string ses_tab2 = "ses_PLt2";
        const string ses_tab3 = "ses_PLt3";

        public const int stNone = 0;            // brak
        public const int stOpen = 1;            // edycja
        public const int stOpenNoImport = 2;    // edycja    
        public const int stClosed = 3;          // zablokowany

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPodzial, "GridView1", true, 25, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoSearch(true);     // ustawienie filtra domyślnego
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!IsPostBack)
                PrepareSearch();
            ShowDane();
            base.OnPreRender(e);
        }

        public bool Prepare(string dOd, string dDo, bool editable, bool import, DateEdit deImport)
        {
            //hidOd.Value = dOd;
            //hidDo.Value = dDo;
            DataOd = dOd;
            DataDo = dDo;

            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            if (App.User.HasRight(AppUser.rPodzialLudzi) || adm)
            {
                paKier.Visible = adm;
                if (adm)
                {
                    ddlPM.DataBind();
                    Tools.SelectItem(ddlPM, App.User.Id);
                }
                FillTabs(adm ? ddlPM.SelectedValue : App.User.Id);

                Tools.SelectMenuFromSession(tabClass.Tabs, ses_tab1);
                Tools.SelectMenuFromSession(tabDane, ses_tab2);
                Tools.SelectMenuFromSession(tabWartosci, ses_tab3);
                
                //FillTabs(ALL);  

                hidNoEdit.Value = editable ? "0" : "1";

                bool imp = adm && editable; // && import; <<< zawsze
                btImport.Visible = imp;
                deNaDzien.Visible = imp;
                deNaDzien.Date = deImport.Date;
                deID = deImport.ID;    // do zainicjowania wartości przed pokazaniem splitów

                PrepareReport();
                return true;
            }
            else
                return false;  // brak dostępu
        }
        //---------------------------
        public static string GetStatus(int st)
        {
            switch (st)
            {
                default:
                case stNone:            // 0 - brak
                    return "Brak podziału";
                case stOpen:            // 1 - edycja
                    return "Otwarty";
                case stOpenNoImport:    // 2 - edycja    
                    return "Korekta";
                case stClosed:          // 3 - zablokowany
                    return "Zamknięty";
            }
        }


        //---------------------------
        const string ALL = "-99";

        private void FillTabs(string kid)
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            string noClassSql = @"
union all 
select 'all|$$$' as Value, 'Wszystko' as Text, 2 as Sort, 1 as Lp
union all 
select 'noclass|$$$' as Value, 'Brak klasyfikacji' as Text, 3 as Sort, 1 as Lp
";

            string sel = Tools.GetLineParam(tabClass.Tabs.SelectedValue, 0);
            tabClass.Prepare(String.Format(@"
declare @kid int
set @kid = {0}
select K.Nazwa + '|' + case when W.Id is not null or @kid = -99 then '$$$' else '' end as Value, K.Nazwa as Text, 1 as Sort, K.Lp 
from Kody K
left join ccPrawa R on R.IdCC = 0 and R.CC = K.Nazwa and R.UserId = @kid
left join ccPrawa W on W.IdCC = 0 and W.CC = K.Nazwa + '$$$' and W.UserId = @kid
where K.Typ = 'PRACGRUPA'
and (@kid = -99 or R.Id is not null)
{1}
order by Sort, Lp, Text
                ",
                 kid, kid == "-99" && adm ? noClassSql : null), false);
            Tools.SelectMenu(tabClass.Tabs, sel);   // to ustawia wg LineParam(0)

            /*
            if (tabClass.Tabs.Items.Count > 1)
            {
                tabClass.Tabs.Items.Add(new MenuItem("Wszystkie klasyfikacje", ALL));
            }
            */ 
        }

        private void PrepareReport()
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
            //bool all = App.User.HasRight(AppUser.rRepPodzialCCAll);
            //bool kwoty = App.User.HasRight(AppUser.rRepPodzialCCKwoty);
            /*
            switch (FMode)
            {
                default:
                case moKier:
                    cntReport1.SQL1 = App.User.Id;
                    break;
                case moAdm:
                    cntReport1.SQL1 = ddlPM.SelectedValue;
                    break;
            }
            */
            string kid = adm ? ddlPM.SelectedValue : App.User.Id;
            KierId = kid;

            string p1, p2;
            Tools.GetLineParams(tabClass.SelectedValue, out p1, out p2);
            hidClass.Value = p1;
            hidStawki.Value = p2 == "$$$" ? "1" : "0";
            hidStawkiH.Value = App.User.HasRight(AppUser.rShowHiddenSalary) ? "1" : "0";
        }

        private void ShowDane()
        {
            Tools.ExecOnStart2("hiddane", tabDane.SelectedValue == "HIDE" ? "$('.dane1').hide();" : "$('.dane1').show();");
        }

        private void SortReset()
        {
            //gvPodzial.Sort("Pracownik", SortDirection.Ascending);
            gvPodzial.Sort(null, SortDirection.Ascending);
        }

        protected void ddlPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string kid = ddlPM.SelectedValue;
            FillTabs(kid);
            SortReset();
            PrepareReport();
        }

        //------------
        protected void tabClass_SelectTab(object sender, EventArgs e)
        {
            Session[ses_tab1] = tabClass.Tabs.SelectedValue;
            PrepareReport();
        }

        //protected void tabClass_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    Session[ses_tab1] = tabClass.Tabs.SelectedValue;
        //}

        protected void tabDane_MenuItemClick(object sender, MenuEventArgs e)
        {
            Session[ses_tab2] = tabDane.SelectedValue;
        }

        protected void tabWartosci_MenuItemClick(object sender, MenuEventArgs e)
        {
            string p1 = Tools.GetStr(Session[ses_tab3]);
            if (p1 == "CC")
                SortReset();
            Session[ses_tab3] = tabWartosci.SelectedValue;
        }

        //-------------
        private bool Multiselect
        {
            get { return !String.IsNullOrEmpty(gvPodzialSelected.Value); }
        }

        private void ShowSelected(bool show, string sel, string napodstawie)   // sel zaczyna się i kończy , !!!
        {
            if (show && !String.IsNullOrEmpty(sel) && sel.Length > 2)
            {
                const int max = 5;
                DataSet ds = db.getDataSet(String.Format(@"
select
    --KadryId + ' ' + Nazwisko + ' ' + Imie 
    Nazwisko + ' ' + Imie + ISNULL(' (' + KadryId + ')','')
as Pracownik from Pracownicy where Id in ({0}) order by Pracownik"
                    , sel.Substring(1, sel.Length - 2)));     // ,1,2,3,
                int cnt = db.getCount(ds);
                bool m = cnt > max;
                lbSelected.Text = db.Join(ds, 0, "<br />", 0, max, true) + (m ? "<br />..." : "");  
                if (m)
                    lbSelected.ToolTip = db.Join(ds, 0, "\n", 0, 100, true);
                else
                    lbSelected.ToolTip = null;
                lbNaPodstawie.Text = napodstawie;
                tbInfo.Visible = true;
            }
            else
            {
                lbSelected.Text = null;
                lbSelected.ToolTip = null;
                lbNaPodstawie.Text = null;
                tbInfo.Visible = false;
            }
        }

        protected void gvPodzialCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvPodzialCmdPar.Value);
            
            //string[] par = Grid.DecryptParams(gvPodzialCmdPar.Value);
            
            switch (par[0])
            {
                case "edit":
                    if (par.Length >= 4)
                    {
                        string pracId = par[1];
                        string splitId = par[2];
                        string dOd = par[3];

                        //cntSplityWsp1.Visible = true;
                        //cntSplityWsp1.IdPrzypisania = splitId;

                        SplitPracId = pracId;
                        cntSplityWsp1.Prepare(splitId, KierId, hidOd.Value, hidDo.Value, pracId);

                        string id = deID;
                        if (!String.IsNullOrEmpty(id))
                        {
                            DateEdit deP = Parent.FindControl(deID) as DateEdit;
                            if (deP != null)
                                deNaDzien.Date = deP.Date;                
                        }

                        string title;
                        string sel = gvPodzialSelected.Value;
                        if (Multiselect && String.Format(",{0},", pracId) != sel)
                        {
                            string[] s = sel.Split(',');
                            title = String.Format("{0} - Ustaw split zaznaczonym pracownikom ({1})", dOd.Substring(0, 7), s.Length - 2);
                            ShowSelected(true, sel, AppUser.GetNazwiskoImieNREW(pracId));   // zaczyna się i kończy , !!!
                        }
                        else
                        {
                            title = String.Format("{0} - Split pracownika: {1}", dOd.Substring(0, 7), AppUser.GetNazwiskoImieNREW(pracId));
                            ShowSelected(false, null, null);
                        }
                        //Tools.ShowDialog(this, "divZoom", 600, btClose, title);
                        const string divZoom = "divZoom";
                        Tools.ShowDialog(paContainer, divZoom, 600, btClose, title);
                        Tools.ExecOnStart2("zoomh", String.Format("$('#{0}').css('height','auto');", divZoom));
                    }
                    break;
            }
        }









        //------------------------------------------
        //http://stackoverflow.com/questions/954198/what-is-the-best-or-most-interesting-use-of-extension-methods-youve-seen/954225#954225


        protected void btImport_Click(object sender, EventArgs e)
        {
            if (deNaDzien.Date != null)
            {
                DataSet ds = HRRcp.PodzialLudzi.GetSplity(SplitPracId, KierId, (DateTime)deNaDzien.Date);
                cntSplityWsp1.Prepare(ds);
            }
            else
                Tools.ShowError("Proszę podać poprawną datę.");
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            //string filename = "PodzialLudzi.csv";
            //App_Code.Report.ExportExcel(hidReport.Value, filename, null);   // >>>>>> musi jak redirect do nowej strony bo się nie ściąga
        }

        private void Save(bool check)
        {
            int err = 0;
            if (check)
            {
                err = cntSplityWsp1.Validate();
                switch (err)
                {
                    case -1:
                        //Tools.ShowError("Suma współczynników splitu jest różna od 1.");
                        Tools.ShowConfirm("Uwaga!!!\\n\\nSuma współczynników splitu jest różna od 1.\\nPotwierdź zapis.", btSave2, null);
                        break;
                    case -2:
                        Tools.ShowError("Wprowadzony współczynnik nie jest liczbą.");
                        break;
                    case -3:
                        Tools.ShowError("Wartość współczynnika musi być mniejsza lub równa 1.");
                        break;
                }
            }
            if (err == 0)
            {
                DataSet dsSplity = null;
                if (Multiselect)
                {
                    dsSplity = db.getDataSet(String.Format(@"
declare @kid int
declare @dataDo datetime
set @kid = {0}
set @dataDo = '{1}'

select S.Id from dbo.SplitInt('{2}', ',') D
inner join Pracownicy P on P.Id = D.items
inner join Splity S on S.GrSplitu = P.GrSplitu and @dataDo between S.DataOd and ISNULL(S.DataDo, '20990909')
where @kid = -99 or items in 
(
select P.Id from Pracownicy P 
inner join Splity S on S.GrSplitu = P.GrSplitu and @dataDo between S.DataOd and ISNULL(S.DataDo, '20990909')
inner join SplityWsp W on W.IdSplitu = S.Id
inner join ccPrawa R on R.UserId = @kid and R.IdCC = W.IdCC
)
                        ", KierId, DataDo, gvPodzialSelected.Value));

                }
                if (cntSplityWsp1.Update(dsSplity))
                {
                    Tools.CloseDialog("divZoom");
                    gvPodzial.DataBind();
                    cntSplityWsp1.StopExecSumuj();
                }
                else
                    Tools.ShowError("Wystąpił błąd podczas zapisu współczynników.");
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        protected void btSave2_Click(object sender, EventArgs e)
        {
            Save(false);
        }

        /*
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (cntSplityWsp1.Validate())
            {
                if (cntSplityWsp1.Update())
                {
                    Tools.CloseDialog("divZoom");
                    gvPodzial.DataBind();
                }
                else
                    Tools.ShowError("Wystąpił błąd podczas zapisu współczynników.");
            }
            else
                Tools.ShowError("Suma współczynników splitu jest różna od 1.");
        }
        */

        protected void btClose_Click(object sender, EventArgs e)
        {
            cntSplityWsp1.StopExecSumuj();
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
            string f1 = null;// tabFilter.SelectedValue.Trim();
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
                    f2 = "([nn:-] like '{0}%' or [ii:-] like '{0}%' or [nrew:-] like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = "([nn:-] like '{0}%' and [ii:-] like '{1}%' or [nn:-] like '{1}%' and [ii:-] like '{0}%' or [nrew:-] like '{0}%' or [nrew:-] like '{1}%')";   // przypadek kiedy szukam po inicjałach wpisując to samo np s s
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("([nn:-] like '{{{0}}}%' or [ii:-] like '{{{0}}}%' or [nrew:-] like '{{{0}}}%')", i);
                        SqlDataSource1.FilterParameters.Add(String.Format("par{0}", i), words[i]);
                    }
                    f2 = String.Join(" and ", exp);
                }
                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            SqlDataSource1.FilterExpression = filter;
            //gvPodzial.DataBind();

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

        private void Deselect()
        {
            /*
            SelectedRcpId = null;
            SelectedStrefaId = null;
            if (lvPracownicy.SelectedIndex != -1)
            {
                lvPracownicy.SelectedIndex = -1;
                TriggerSelectedChanged();
            }
             */ 
        }
        
        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                Deselect();
                /*
                lvPracownicy.EditIndex = -1;
                lvPracownicy.InsertItemPosition = InsertItemPosition.None;
                */
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        protected void gvPodzial_Load(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
        }
        //-----------------------------------       
        public int RowsCount
        {
            set { ViewState["gvrows"] = value; }
            get { return Tools.GetInt(ViewState["gvrows"], 0); }
        }

        protected void ddlLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ln = ddlLines.SelectedValue;
            if (ln == "all")
            {
                //gvPodzial.AllowPaging = false;
                gvPodzial.PageSize = RowsCount;
            }
            else
            {
                //gvPodzial.AllowPaging = true;
                gvPodzial.PageSize = Tools.StrToInt(ln, 25);
            }
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            lbCount.Text = e.AffectedRows.ToString();
        }

        public SqlDataSource DataSource
        {
            get { return SqlDataSource1; }
        }

        public string SelectedPodzial
        {
            get { return tabWartosci.SelectedItem.Text.Replace(" * ", " x "); }
        }

        public string DataOd
        {
            set { hidOd.Value = value; }
            get { return hidOd.Value; }
        }

        public string DataDo
        {
            set { hidDo.Value = value; }
            get { return hidDo.Value; }
        }

        public string KierId
        {
            set { hidKierId.Value = value; }
            get { return hidKierId.Value; }
        }

        public string deID
        {
            set { ViewState["deicid"] = value; }
            get { return Tools.GetStr(ViewState["deicid"]); }
        }
 
        public string SplitPracId
        {
            set { ViewState["sppracid"] = value; }
            get { return Tools.GetStr(ViewState["sppracid"]); }
        }
    }
}
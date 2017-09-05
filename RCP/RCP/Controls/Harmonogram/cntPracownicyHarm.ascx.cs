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
using HRRcp.Controls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class cntPracownicyHarm : System.Web.UI.UserControl
    {
        const string ses_tab1 = "tab_PH1";

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvPracownicy, "GridView1", false, 25, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                deNaDzien.Date = GetDateFromSession();//DateTime.Today;
                DoSearch(true);     // ustawienie filtra domyślnego
            }
        }

        protected String GetDateFromSession()
        {
            String date = Session["ses_pracownicy_harm_date"] as String;
            if (String.IsNullOrEmpty(date))
                return DateTime.Today.ToString();
            return date;
        }

        protected void SetSessionDate()
        {
            if (deNaDzien.Date != null)
                Session["ses_pracownicy_harm_date"] = deNaDzien.Date.ToString();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!IsPostBack)
                PrepareSearch();
            base.OnPreRender(e);
        }

        public bool Prepare()
        {
            return true;
        }

        /*
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
         */ 
        //---------------------------
        const string ALL = "-99";

        private void FillTabs(string kid)
        {
        }

        private void PrepareReport()
        {
            bool adm = App.User.HasRight(AppUser.rPodzialLudziAdm);
        }

        private void SortReset()
        {
            //gvPracownicy.Sort("Pracownik", SortDirection.Ascending);
            gvPracownicy.Sort(null, SortDirection.Ascending);
        }

        //------------
        protected void tabStatus_MenuItemClick(object sender, MenuEventArgs e)
        {
            Session[ses_tab1] = tabStatus.SelectedValue;
        }


        //-------------
        private bool Multiselect
        {
            get { return !String.IsNullOrEmpty(gvPracownicySelected.Value); }
        }

        private void ShowSelected(bool show, string sel, string napodstawie)   // sel zaczyna się i kończy , !!!
        {
            /*
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
                lbSelected.Text = db.Join(ds, 0, "<br />", 0, max) + (m ? "<br />..." : "");  
                if (m)
                    lbSelected.ToolTip = db.Join(ds, 0, "\n", 0, 100);
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
             */ 
        }

        protected void gvPracownicyCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvPracownicyCmdPar.Value);

            //string[] par = Grid.DecryptParams(gvPracownicyCmdPar.Value);
            
            switch (par[0])
            {
                case "ajax":
                    if (par.Length >= 5)  // cmd,typ,pid,par,ret
                    {
                        switch (Tools.StrToInt(par[1], -1))
                        {
                            case 1:
                                gvPracownicy.DataBind();
                                break;
                        }
                    }
                    break;
                case "status":
                    if (par.Length >= 2)
                    {
                        string pid = par[1];
                        string act = par[2];
                        string data = Tools.DateToStrDb(DateTime.Today);
                        bool set = act == "0";  // przekazywany jest stan obecny
                        bool ok = db.Execute(dsSetPracAktywny, pid, set ? 1 : 0, data);  
                        if (ok)
                        {
                            gvPracownicy.DataBind();
                            Log.Info(Log.HARMONOGRAM,
                                set ? "Aktywowanie pracownika" : "Deaktywacja pracownika",
                                String.Format("Pracownik: {0} {1}", pid, data));
                        }
                        else
                        {
                            Tools.ShowErrorLog(Log.HARMONOGRAM,
                            //Log.Error(Log.HARMONOGRAM,
                                String.Format("Zmiana statusu pracownika: {0}, {1}, {2}", pid, act, data),
                                "Błąd podczas aktualizacji statusu pracownika.");
                        }
                    }
                    break;
                case "edit":
                    string pracId = par[1];
                    cntPracownicyEditModal.ShowEdit(pracId);
                    /*
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
                        string sel = gvPracownicySelected.Value;
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
                     */
                    break;
            }
        }









        //------------------------------------------
        //http://stackoverflow.com/questions/954198/what-is-the-best-or-most-interesting-use-of-extension-methods-youve-seen/954225#954225



        //----- FILTER ----------------------------------
        private void PrepareSearch()
        {
            ////btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('');doClick('{1}');return false;", tbSearch.ClientID, btSearch.ClientID);
            //btClear.Attributes["onclick"] = String.Format(@"$('#{0}').val('').focus();return false;", tbSearch.ClientID, btSearch.ClientID);
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
            //gvPracownicy.DataBind();

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

        protected void gvPracownicy_Load(object sender, EventArgs e)
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
                //gvPracownicy.AllowPaging = false;
                gvPracownicy.PageSize = RowsCount;
            }
            else
            {
                //gvPracownicy.AllowPaging = true;
                gvPracownicy.PageSize = Tools.StrToInt(ln, 25);
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

        protected void msFirma_DataBound(object sender, EventArgs e)
        {

        }

        protected void msFirma_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void msDzial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = null;
            msDzial.ClearSelection();
            msFirma.ClearSelection();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnAssignOkresType_Click(object sender, EventArgs e)
        {
            if (deNaDzien.IsValid)
            {
                string pracList = gvPracownicySelected.Value;
                if (!String.IsNullOrEmpty(pracList))
                {
                    cntModal.Show();
                    lblNaDzienOkres.Text = Tools.DateToStr(Tools.bom((DateTime)deNaDzien.Date));
                }
                else
                    Tools.ShowMessage("Proszę wybrać pracowników.");
            }
            else
                Tools.ShowMessage("Niepoprawna data!");
        }

        protected void btnAssignOkresConfirm_Click(object sender, EventArgs e)
        {
            string pracList = gvPracownicySelected.Value;
            if (!String.IsNullOrEmpty(pracList))
            {
                Tools.ShowConfirm("Czy na pewno chcesz przypisać typ okresu do pracowników?", btnAssignOkres);
            }
            else
                Tools.ShowMessage("Proszę wybrać pracowników.");
        }

        protected void btnAssignOkres_Click(object sender, EventArgs e)
        {
            string pracList = gvPracownicySelected.Value;
            if (!String.IsNullOrEmpty(pracList) && pracList != ",")
            {
                string trimmedPracList = pracList.Substring(1, pracList.Length - 2);
                string okresTypeId = ddlTypOkresu.SelectedValue;

                db.Execute(dsAssignOkres, trimmedPracList, okresTypeId, Tools.bom((DateTime)deNaDzien.Date));
                Tools.ShowMessage("Dane zostały zapisane.");
                gvPracownicy.DataBind();
                cntModal.Close();
            }
            else
                Tools.ShowMessage("Proszę wybrać pracowników.");
        }

        protected void deNaDzien_DateChanged(object sender, EventArgs e)
        {
            SetSessionDate();
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            cntPracownicyEditModal.ShowInsert();
        }

        protected void cntPracownicyEditModal_Saved(object sender, EventArgs e)
        {
            gvPracownicy.DataBind();
        }


    }
}
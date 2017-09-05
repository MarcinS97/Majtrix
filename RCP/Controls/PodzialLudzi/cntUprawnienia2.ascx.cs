using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.Controls.PodzialLudzi
{
    public partial class cntUprawnienia2 : System.Web.UI.UserControl
    {
        const string ses_tab = "ses_PLUPTyp";

        protected void Page_Init(object sender, EventArgs e)
        {
            //Grid.Prepare(gvUprawnienia, null, false, 0, true);
            Grid.Prepare(gvUprawnienia, null, true, 25, true);
        }

        bool v = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;
                hidUserAdm.Value = App.User.HasRight(AppUser.rPodzialLudziAdm) ? "1" : "0";
                DoSearch(true);     // ustawienie filtra domyślnego
                Tools.SelectMenuFromSession(tabTyp, ses_tab);
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
            Tools.SetMainServiceUrl();

            SetGridDataSource();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)   // nastąpiła zmiana, kontrolka jak nie jest widoczna to zwarac Visible = false niezaleznie czy jest ustawiane czy nie
                PrepareSearch();
            base.OnPreRender(e);
        }


        //----- FILTER ----------------------------------
        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                Deselect();
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        private void Deselect()
        {
        }
        //-----
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
        //----------------------
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
                gvUprawnienia.PageSize = RowsCount;
            }
            else
            {
                //gvPodzial.AllowPaging = true;
                gvUprawnienia.PageSize = Tools.StrToInt(ln, 25);
            }
        }

        private string GetRowsCount(int filter, int total)
        {
            return filter == total ? total.ToString() : String.Format("{0}/{1}", filter, total);
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)  // tez dla 2
        {
            RowsCount = e.AffectedRows;
            //lbCount.Text = e.AffectedRows.ToString();
            lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
        }

        protected void SqlDataSource1_Load(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
        }

        protected void SqlDataSource2_Selected(object sender, SqlDataSourceStatusEventArgs e)  // tez dla 2
        {
            RowsCount = e.AffectedRows;
            //lbCount.Text = e.AffectedRows.ToString();
            lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
        }

        protected void SqlDataSource2_Load(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
        }

        private int GetFilteredRowsCount()
        {
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            return dv.Count;
        }

        private int GetFilteredRowsCount(SqlDataSource ds)
        {
            if (ds != null)
            {
                DataView dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
                return dv.Count;
            }
            else
                return 0;
        }

        protected void gvUprawnienia_DataBound(object sender, EventArgs e)
        {
            //int cnt = ((DataView)gvUprawnienia.DataSource).Count;            
            //int cnt = gvUprawnienia.
            int cnt;
            if (String.IsNullOrEmpty(tbSearch.Text.Trim()))
                cnt = RowsCount;
            else
                cnt = GetFilteredRowsCount((sender as GridView).DataSource as SqlDataSource);
            lbCount.Text = GetRowsCount(cnt, RowsCount);
        }

        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {

        }

        protected void SqlDataSource1_PreRender(object sender, EventArgs e)
        {

        }

        //-----------------
        protected void tabTyp_MenuItemClick(object sender, MenuEventArgs e)
        {
            Session[ses_tab] = tabTyp.SelectedValue;
            gvUprawnienia.Sort(null, SortDirection.Ascending);
            SetGridDataSource();
        }

        private void SetGridDataSource()
        {
            string typ = tabTyp.SelectedValue;
            switch (typ)
            {
                case "1":
                    SqlDataSource1.SelectCommand = SqlDataSourceA.SelectCommand;
                    break;
                case "2":
                    SqlDataSource1.SelectCommand = SqlDataSourceB.SelectCommand;
                    break;
            }
            //gvUprawnienia.DataBind();
        }

        //-----------------------------------------------
        int GetSortColumnIndex()
        {

            // Iterate through the Columns collection to determine the index
            // of the column being sorted.
            foreach (DataControlField field in gvUprawnienia.Columns)
            {
                if (field.SortExpression == gvUprawnienia.SortExpression)
                {
                    return gvUprawnienia.Columns.IndexOf(field);
                }
            }

            return -1;
        }

        // This is a helper method used to add a sort direction
        // image to the header of the column being sorted.
        void AddSortImage(int columnIndex, GridViewRow headerRow)
        {

            // Create the sorting image based on the sort direction.
            Image sortImage = new Image();
            if (gvUprawnienia.SortDirection == SortDirection.Ascending)
            {
                sortImage.ImageUrl = "~/Images/Ascending.jpg";
                sortImage.AlternateText = "Ascending Order";
            }
            else
            {
                sortImage.ImageUrl = "~/Images/Descending.jpg";
                sortImage.AlternateText = "Descending Order";
            }

            // Add the image to the appropriate header cell.
            headerRow.Cells[columnIndex].Controls.Add(sortImage);

        }


        protected void gvUprawnienia_RowCreated(object sender, GridViewRowEventArgs e)
        {

            // Use the RowType property to determine whether the 
            // row being created is the header row. 
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = GetSortColumnIndex();

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    AddSortImage(sortColumnIndex, e.Row);
                }
            }

        }

    }
}
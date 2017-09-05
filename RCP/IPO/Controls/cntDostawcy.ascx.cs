using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.IPO.Controls
{
    public partial class cntDostawcy : System.Web.UI.UserControl
    {

        bool v = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvKody, 0);
            Tools.PrepareSorting(lvKody, 1, 10);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoSearch(true);     // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (Visible && Visible != v)
            {
                PrepareSearch();
            }
            lvKody.DataBind();

            base.OnPreRender(e);
        }


        #region Search Functionality

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


        protected void btSearch_Click(object sender, EventArgs e)
        {
            DoSearch(false);
        }

        private void DoSearch(bool init)  //init = !IsPostback, w SteFilter był ResetLetterPager który w !IsPostback szukał pagera czym powodował bind lvPracownicy, ustawiał się LetterPager i zaraz był ustawiany do zerowania - się nie wyświetlał 
        {
            SetFilterExpr(!init);
        }
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

                f2 = "(Nazwa like '{0}%')";
                SqlDataSource1.FilterParameters.Add("par0", words[0]);

                filter = f2 + (String.IsNullOrEmpty(f1) ? null : " and " + f1);
            }
            FilterExpression = filter;

            return filter;
        }

        private string FilterExpression
        {
            set
            {
                ViewState["filter"] = value;
                SqlDataSource1.FilterExpression = value;    // fiter jest ustawiany w lv_OnLayoutCreate więc przy zmianie trzeba ustawić
            }
            get { return Tools.GetStr(ViewState["filter"]); }
        }

        protected void SqlDataSource1_Load(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
        }
        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            /*
            RowsCount = e.AffectedRows;
            lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
             * */
        }
        protected void SqlDataSource1_Filtering(object sender, SqlDataSourceFilteringEventArgs e)
        {

        }

        protected void SqlDataSource1_PreRender(object sender, EventArgs e)
        {

        }

        protected void ch_CheckedChanged(object sender, EventArgs e)
        {
            if (Visible && Visible != v)
            {
                PrepareSearch();
            }
            lvKody.DataBind();
        }

        protected void lvKody_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            /*
            int cnt;
            if (String.IsNullOrEmpty(tbSearch.Text.Trim()))
                cnt = RowsCount;
            else
                cnt = GetFilteredRowsCount();
            lbCount.Text = GetRowsCount(cnt, RowsCount);
             * */
        }

        #endregion


        bool? ed = null;
        protected bool GetEditable
        {
            get
            {
                if (ed == null)
                {
                    //ed = App.User.HasRight(AppUser.rKierParams);   // przykład !!!!
                    ed = true;
                }
                return (bool)ed;
            }
        }

        protected void lvKody_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Usun":
                    string idDostawcy = lvKody.DataKeys[((ListViewDataItem)e.Item).DisplayIndex].Value.ToString();
                    bool dostawcaIsUsedInZamowienia = Boolean.Parse(db.getScalar(@"SELECT 
	                                                                        CASE 
		                                                                        WHEN count('x') > 0 THEN 'True'
		                                                                        ELSE 'False'
	                                                                        END AS rodzajProduktuIsUsed
                                                                         FROM IPO_PozycjeZamowien
                                                                         WHERE IPO_PozycjeZamowien.idDostawcy = " + idDostawcy));
                    bool dostawcaIsUsedInProdukty = Boolean.Parse(db.getScalar(@"SELECT 
	                                                                        CASE 
		                                                                        WHEN count('x') > 0 THEN 'True'
		                                                                        ELSE 'False'
	                                                                        END AS rodzajProduktuIsUsed
                                                                         FROM IPO_Produkty
                                                                         WHERE IPO_Produkty.idDostawcy = " + idDostawcy));
                    if (dostawcaIsUsedInZamowienia)
                    {
                        Tools.ShowError("Ten dosatwca nie może zostać usunięty, ponieważ został użyty w conajmniej jednym zamówieniu.");
                    }
                    else if (dostawcaIsUsedInProdukty)
                    {
                        Tools.ShowError("Ten dosatwca nie może zostać usunięty, ponieważ został do conajmniej jednego produktu w bazie produktów.");
                    }
                    else
                    {
                        db.execSQL("DELETE FROM [IPO_Dostawcy] WHERE [Id] = " + idDostawcy);
                        lvKody.DataBind();
                    }
                    break;
                case "Insert":
                    lvKody.DataBind();
                    break;
            }
           
        }
    }
}
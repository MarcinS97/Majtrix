using HRRcp.App_Code;
using HRRcp.BadaniaWstepne.Templates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntBadaniaWstKolUpr : System.Web.UI.UserControl
    {
        public string KolumnySql;

        protected void Page_Init(object sender, EventArgs e)
        {
            string preSql = string.Format(@"case dbo.getRightId(Uprawnienia, {{1}}) when 3 then '{3}' when 2 then '{2}' when 1 then '{1}' else '{0}' end
                                            [{{0}}|js:setPrawaBadKol(this @pid {{1}})|{{0}}]", (object[])CrTemplateGroupCreator.BadWstRightStrs);
            //var data = TMPClass1.PreColumns
            var data = BadaniaWstColumny.Instance.PreColumns
                .GroupBy(a => a.RightId, a => a)
                .Select(a =>
                {
                    //CrColumnCreator Item = (a.Count() > 1) ? Item = a.First(b => b.Sort == TMPClass1.PreColsH[b.RightId]) : Item = a.First();
                    CrColumnCreator Item = (a.Count() > 1) ? Item = a.First(b => b.Sort == BadaniaWstColumny.Instance.PreColsH[b.RightId]) : Item = a.First();
                    return new { Name = Item.Header, id = Item.RightId };
                }).ToList();
            // data.add( Kolumna )

            //App.User.HasRight(AppUser.rBadaniaWstepneAddDel);

            KolumnySql = string.Join(",", data.Select(a => string.Format(preSql, a.Name, a.id)).ToArray());
            Grid.Prepare(gvUprawnienia, null, true, 25, true);
        }

        bool v = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;
                hidUserAdm.Value = App.User.HasRight(AppUser.rPodzialLudziAdm) ? "1" : "0";
				gvUprawnienia.Sort("Pracownik", SortDirection.Ascending);
                DoSearch(true);     // ustawienie filtra domyślnego
            }
            else
            {
                v = Visible;        // jak !IsPostBack i Visible tzn ze startuje od formatki z kontrolką
            }
            SqlDataSource1.SelectCommand = string.Format(SqlDataSource1.SelectCommand, KolumnySql);
            Tools.SetMainServiceUrl();
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
                {//kk:-
                    f2 = "([nn:-] like '{0}%' or [ii:-] like '{0}%' or [kk:-] like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                }
                else if (words.Length == 2)
                {
                    f2 = @"([nn:-] like '{0}%' and [ii:-] like '{1}%' or 
                            [nn:-] like '{1}%' and [ii:-] like '{0}%' or
                            [kk:-] like '{1}%' and [ii:-] like '{0}%' or
                            [ii:-] like '{1}%' and [kk:-] like '{0}%' or
                            [kk:-] like '{1}%' and [nn:-] like '{0}%' or
                            [nn:-] like '{1}%' and [kk:-] like '{0}%')";
                    SqlDataSource1.FilterParameters.Add("par0", words[0]);
                    SqlDataSource1.FilterParameters.Add("par1", words[1]);
                }
                else
                {
                    string[] exp = new string[words.Length];
                    for (int i = 0; i < words.Length; i++)
                    {
                        exp[i] = String.Format("([nn:-] like '{{{0}}}%' or [ii:-] like '{{{0}}}%' or [kk:-] like '{{{0}}}%')", i);
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

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RowsCount = e.AffectedRows;
            //lbCount.Text = e.AffectedRows.ToString();
            lbCount.Text = GetRowsCount(e.AffectedRows, e.AffectedRows);
        }

        protected void SqlDataSource1_Load(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = FilterExpression;
        }

        private int GetFilteredRowsCount()
        {
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            return dv.Count;
        }

        protected void gvUprawnienia_DataBound(object sender, EventArgs e)
        {
            //int cnt = ((DataView)gvUprawnienia.DataSource).Count;            
            //int cnt = gvUprawnienia.
            int cnt;
            if (String.IsNullOrEmpty(tbSearch.Text.Trim()))
                cnt = RowsCount;
            else
                cnt = GetFilteredRowsCount();
            lbCount.Text = GetRowsCount(cnt, RowsCount);
        }
        //--------------------------------------
        private bool ChangeAll(string pracId)    // przeniesiona do cmd w cntBadaniaWstKolUpr
        {
            const string BadaniaWstKolUpr = "BadaniaWstKolUpr";
            const int maxCol = 100;

            DataRow dr = db.getDataRow(String.Format("select Uprawnienia from {0} where IdPrac = {1}", BadaniaWstKolUpr, pracId));
            string r = dr != null ? db.getValue(dr, 0) : null;

            //int len = HRRcp.BadaniaWstepne.Controls.cntBadaniaWst.lastColumnRight;
            if (String.IsNullOrEmpty(r)) r = new String('1', maxCol);
            else
                switch (r[0])
                {
                    case '0':
                        r = new String('1', maxCol);
                        break;
                    case '1':
                        r = new String('2', maxCol);
                        break;
                    case '2':
                        r = new String('3', maxCol);
                        break;
                    default:
                    case '3':
                        r = new String('0', maxCol);
                        break;
                }
            bool ok;
            if (dr == null)
                ok = db.insert(BadaniaWstKolUpr, 0, "IdPrac,Uprawnienia", pracId, db.strParam(r));
            else
                ok = db.update(BadaniaWstKolUpr, 1, "Uprawnienia", "IdPrac={0}", pracId, db.strParam(r));
            return ok;
        }

        protected void gvUprawnieniaCmd_Click(object sender, EventArgs e)
        {
            string[] par = Tools.GetLineParams(gvUprawnieniaCmdPar.Value);            
            switch (par[0])
            {
                case "set":
                    string pid = par[1];
                    ChangeAll(pid);
                    gvUprawnienia.DataBind();  // nie zmienia paginatora i sortowania !!!
                    break;
            }
        }


    }
}
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
    public partial class AdmKodyAbs : System.Web.UI.UserControl
    {
        public const int stDodatkowy = -2;
        public const int stStary = -1;
        public const int stOk = 0;
        public const int stNowy = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS
                tabFilter.Items[0].Text = "Aktywne";
                tabFilter.Items[1].Text = "Stare";
#else
#endif
                Tools.SelectMenuFromSession(tabFilter, tabFilter.ClientID);
                SelectTab();
            }
            else
            {
                SqlDataSource2.FilterExpression = FilterWhere;
            }        
        }

        public string GetStatus(object status)
        {
            string st = status.ToString();
            switch (st)
            {
                case "-2": return "Dodatkowy";  // z ręki ... urlop na żądanie
                case "-1": return "Stary";
                case "0": return "Ok";
                case "1": return "Nowy";
                default: return st;
            }
        }

        private void SelectTab() //ApplyFilter
        {
            ApplyFilter();
            string filter = tabFilter.SelectedValue;
            Session[tabFilter.ClientID] = filter;
        }

        public void AddStatus(DropDownList ddl)
        {
            ddl.Items.Add(new ListItem(GetStatus(stDodatkowy), stDodatkowy.ToString()));
            ddl.Items.Add(new ListItem(GetStatus(stStary), stStary.ToString()));
            ddl.Items.Add(new ListItem(GetStatus(stOk), stOk.ToString()));
            ddl.Items.Add(new ListItem(GetStatus(stNowy), stNowy.ToString()));
        }

        protected void lvAbsencjaKody_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int lim = Tools.GetListItemMode(e, lvAbsencjaKody);
            switch (lim)
            {
                case Tools.limSelect:
                    break;
                case Tools.limEdit:
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                    if (ddl != null) AddStatus(ddl);
                    Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                    break;
                case Tools.limInsert:
                    ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                    if (ddl != null) AddStatus(ddl);
                    break;
                case Tools.limOther:
                    break;
            }
        }

        protected void lvAbsencjaKody_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvAbsencjaKody, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    break;
                case Tools.limEdit:

                    int status = db.getInt(drv["Status"], -99);
                    string typ2 = db.getStr(drv["Typ2"]);
                    bool ed = status == stDodatkowy;
                    Tools.SetControlVisible(e.Item, "KodTextBox", ed);
                    Tools.SetControlVisible(e.Item, "KodLabel", !ed);
                    Tools.SetControlVisible(e.Item, "NazwaTextBox", ed);
                    Tools.SetControlVisible(e.Item, "NazwaLabel", !ed);                    
                    Tools.SelectItem(e.Item, "ddlStatus", status);
                    Tools.SelectItem(e.Item, "ddlTyp2", typ2);
                    break;
                case Tools.limInsert:
                    break;
                case Tools.limOther:
                    break;
            }
        }
        //------------------------
        private bool UpdateItem(ListViewItem item, EventArgs ea, IOrderedDictionary oldValues, IOrderedDictionary values, bool create)
        {
            values["Status"] = Tools.GetDdlSelectedValueInt(item, "ddlStatus");
            values["Typ2"] = Tools.GetDdlSelectedValue(item, "ddlTyp2");
            string kod2 = db.getStr(values["Kod2"]);

            if (!String.IsNullOrEmpty(kod2))
            {
                DataRow dr;
                if (create)
                    dr = db.getDataRow(String.Format("select * from AbsencjaKody where Kod2 = '{0}'", kod2));
                else
                {
                    string kod = Tools.GetDataKey(lvAbsencjaKody, (ListViewUpdateEventArgs)ea);
                    dr = db.getDataRow(String.Format("select * from AbsencjaKody where Kod2 = '{0}' and Kod != {1}", kod2, kod));
                }
                if (dr != null)
                {
                    Tools.ShowError("Kod2 już istnieje.");
                    return false;
                }
            }
            return true;
        }

        protected void lvAbsencjaKody_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView lv = (ListView)sender;
            bool ok = UpdateItem(lv.InsertItem, e, null, e.Values, true);
            e.Cancel = !ok;
            if (ok) Log.LogChanges(sender, e);
        }

        protected void lvAbsencjaKody_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            bool ok = UpdateItem(lv.EditItem, e, e.OldValues, e.NewValues, false);
            e.Cancel = !ok;
            if (ok) Log.LogChanges(sender, e);
        }

        protected void lvAbsencjaKody_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            Log.LogChanges(sender, e);
        }

        //------------------------
        protected void tabFilter_MenuItemClick(object sender, MenuEventArgs e)
        {
            object o = Session[tabFilter.ClientID];
            bool check = o != null && o.ToString() != tabFilter.SelectedValue; 

            if (check)
                if (lvAbsencjaKody.EditIndex != -1)
                {
                    Tools.SelectMenuFromSession(tabFilter, tabFilter.ClientID);
                    Tools.ShowMessage("Trwa edycja danych.\\n{0}, najpierw zapisz lub anuluj zmiany.", App.User.OriginalImie);
                }
                else
                    SelectTab();
        }

        private void ApplyFilter()
        {
            string f = GetFilterWhere();
            SqlDataSource2.FilterExpression = f;
            FilterWhere = f;
            /*
            //lvHistoria.EditIndex = -1;
            lvHistoria.InsertItemPosition = InsertItemPosition.None;

            DataPager dp = (DataPager)lvHistoria.FindControl("DataPager1");
            if (dp != null) dp.SetPageProperties(0, dp.PageSize, true);
            //lvHistoria.DataBind();
             */
        }

        private string GetFilterWhere()
        {
            string filter = tabFilter.SelectedValue;
#if SIEMENS
            switch (filter)
            {
                default:
                case "0":   // aktywne
                    return "Status <> -1";
                case "1":   // stare
                    return "Status = -1";
                case "2":   //All
                    return null;
            }
#else
            switch (filter)
            {
                default:
                case "0":   //Asseco
                    return "kod >= 1000";
                case "1":   //KP
                    return "kod < 1000";
                case "2":   //All
                    return null;
            }
#endif
        }

        public string FilterWhere
        {
            get { return Tools.GetViewStateStr(ViewState["filter"]); }
            set { ViewState["filter"] = value; }
        }        
        //----- sortowanie -----
        const string sesSortId = "_sort";
        const int maxSortCol = 8;
        private int FDefSortColumn = 2;

        protected void lvAbsencjaKody_LayoutCreated(object sender, EventArgs e)
        {
            int sort = Sort;
            Report.ShowSort(lvAbsencjaKody, sort, sort > 0);
        }

        protected void lvAbsencjaKody_Sorting(object sender, ListViewSortEventArgs e)
        {
            int sort;
            Report.ShowSort(sender, e, maxSortCol, FDefSortColumn, out sort);
            Sort = sort;  // unikalne co do kontrolki
        }

        public int Sort
        {
            get
            {
                object s = Session[lvAbsencjaKody.ClientID + sesSortId + sesSortId];
                //object s = ViewState[lvDzialy.ClientID + sesSortId];
                if (s != null)
                {
                    int sort = (int)s;
                    if (1 <= sort && sort <= maxSortCol)
                        return sort;
                }
                return FDefSortColumn;
            }
            set
            {
                Session[lvAbsencjaKody.ClientID + sesSortId + sesSortId] = value;
                //ViewState[lvDzialy.ClientID + sesSortId + sesSortId] = value;
            }
        }

        public int DefSort
        {
            get { return FDefSortColumn; }
            set { FDefSortColumn = value; }
        }


        //----------------------
    }
}
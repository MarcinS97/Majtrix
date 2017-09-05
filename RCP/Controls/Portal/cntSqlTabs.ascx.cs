using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using System.ComponentModel;

namespace HRRcp.Controls.Portal
{
    public partial class cntSqlTabs : System.Web.UI.UserControl
    {
        public event EventHandler SelectTab;
        public event EventHandler DataBound;

        string FSQL;
        string FDataTextField  = "Text";
        string FDataValueField = "Value";
        string FAddCssClass    = null;
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int cnt = 0;
                if (!String.IsNullOrEmpty(FSQL))     // jeżeli SQL jest podany w aspx 
                {
                    cnt = Prepare(FSQL);
                }
                else
                {
                    if (!String.IsNullOrEmpty(Grupa))
                        cnt = Prepare(String.Format(dsGrupa.SelectCommand, Grupa));
                    if (cnt == 0 && !String.IsNullOrEmpty(Items))   // wartości domyślne
                        PrepareItems(Items, false, null);
                }
            }

            //if (!IsPostBack)
            //{
            //    if (Visible && !String.IsNullOrEmpty(FSQL))     // jeżeli SQL jest podany w aspx - Visible nie działa !!!
            //        Prepare(FSQL);
            //}

            //if (!String.IsNullOrEmpty(FSQL) && !Initialized && Visible)    // mnTabs.Visible też jest false
            //{
            //    Prepare(FSQL);
            //    Initialized = true;
            //}
        }
        //---------------------------
        const string defItemHtml = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{0}</div></div></div>";

        public int PrepareItems(string items, bool trigger, string selectValue)
        {
            string[] sep = { "|", "\r\n" };
            string[] a = items.Split(sep, StringSplitOptions.None);
            int len = a.Length;
            string html = String.IsNullOrEmpty(ItemHtml) ? defItemHtml : ItemHtml;
            mnTabs.Items.Clear();
            bool go = false;
            for (int i = 0; i < len; i++)
            {
                string text = a[i].Trim();
                if (!go)
                {
                    go = !String.IsNullOrEmpty(text);
                    if (!go) continue;
                }
                text = L.p(text);
                i++;
                string value = null;
                if (i < len) value = a[i].Trim();
                if (i < len || !String.IsNullOrEmpty(text))   // po ostatnim value jest | to nie dodaję już nic
                    mnTabs.Items.Add(new MenuItem(String.Format(html, text), value));
            }
            return PrepareAfter(trigger, selectValue);
        }

        private int PrepareAfter(bool triggerSelect, string selectValue)
        {
            int cnt = mnTabs.Items.Count;
            if (cnt > 0)
            {
                if (String.IsNullOrEmpty(selectValue))
                    mnTabs.Items[0].Selected = true;
                else
                    Tools.SelectMenu(mnTabs, selectValue);
                if (triggerSelect)
                    TriggerSelectTab();
            }
            mnTabs.StaticMenuStyle.CssClass = "tabsStrip" + (!String.IsNullOrEmpty(FAddCssClass) ? " " + FAddCssClass : null);
            TriggerDataBound();
            return cnt;
        }
        //---------------------------
        public int Prepare(string sql, bool trigger)
        {
            return Prepare(sql, trigger, null);
        }

        public int Prepare(string sql, bool trigger, string selectedTab)
        {
            DataSet ds = db.getDataSet(String.Format(@"declare @lang varchar(10) set @lang = '{0}' ", L.Lang) + sql);
            //DataSet ds = db.getDataSet(sql);
            mnTabs.Items.Clear();
            foreach (DataRow dr in db.getRows(ds))
                mnTabs.Items.Add(new MenuItem(String.Format(defItemHtml, db.getValue(dr, FDataTextField)), db.getValue(dr, FDataValueField)));
            return PrepareAfter(trigger, selectedTab);
            /*
            if (mnTabs.Items.Count > 0)
            {
                if(string.IsNullOrEmpty(selectedTab))
                    mnTabs.Items[0].Selected = true;
                else
                    Tools.SelectMenu(mnTabs, selectedTab);
                if (trigger)
                    TriggerSelectTab();
            }
            mnTabs.StaticMenuStyle.CssClass = "tabsStrip" + (!String.IsNullOrEmpty(FAddCssClass) ? " " + FAddCssClass : null);
            TriggerDataBound();
            return db.getCount(ds);
            */ 
        }

        public int Prepare(string sql)
        {
            return Prepare(sql, true);
        }

        public int Prepare(bool trigger)
        {
            return Prepare(SQL, trigger);
        }

        public int Prepare()
        {
            return Prepare(SQL, true);
        }
        //-----
        public int Reload(bool trigger)
        {
            string selected = mnTabs.SelectedValue;
            return Prepare(SQL, trigger, selected);
        }

        public int Reload()
        {
            return Reload(true);
        }

        private void TriggerSelectTab()
        {
            if (SelectTab != null)
                SelectTab(this, EventArgs.Empty);
        }

        private void TriggerDataBound()
        {
            if (DataBound != null)
                DataBound(this, EventArgs.Empty);
        }

        protected void mnTabs_MenuItemClick(object sender, MenuEventArgs e)
        {
            TriggerSelectTab();
        }

        //------------------------------------------
        public string SQL
        {
            set { FSQL = value; }
            get { return FSQL; }
        }

        [Bindable(true)]
        [Browsable(true)]
        public string SelectedValue
        {
            get { return mnTabs.SelectedValue; }
            set { Tools.SelectMenu(mnTabs, value); }
        }

        public Menu Tabs
        {
            get { return mnTabs; }
        }

        public string DataTextField
        {
            set { FDataTextField = value; }
            get { return FDataTextField; }
        }

        public string DataValueField
        {
            set { FDataValueField = value; }
            get { return FDataValueField; }
        }

        public string AddCssClass
        {
            set { FAddCssClass = value; }
            get { return FAddCssClass; }
        }

        private bool Initialized
        {
            set { ViewState["ized"] = value; }
            get { return Tools.GetBool(ViewState["ized"], false); }
        }
        //-----
        //public string DataSourceID   musi być hierarchical
        //{
        //    set { mnTabs.DataSourceID = value; }
        //    get { return mnTabs.DataSourceID; }
        //}
 
        public string CssClass
        {
            set { mnTabs.CssClass = value; }
            get { return mnTabs.CssClass; }
        }

        public string Items   // text,value,text,value...
        {
            set;
            get;
        }

        public string ItemHtml   // <div>{0}</div>
        {
            set;
            get;
        }

        public string Grupa   // SqlMenu
        {
            set;
            get;
        }
    }
}
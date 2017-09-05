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
    public partial class cntSqlTabsLeft : System.Web.UI.UserControl
    {
        public event EventHandler SelectTab;
        public event EventHandler DataBound;

        string FSQL;
        string FDataTextField = "Text";
        string FDataValueField = "Value";
        string FAddCssClass = null;
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(FSQL))     // jeżeli SQL jest podany w aspx 
                {
                    Prepare(FSQL);
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

        public int Prepare(string sql, bool trigger)
        {
            return Prepare(sql, trigger, null);
        }

        public int Prepare(string sql, bool trigger, string selectedTab)
        {
            const string item = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{0}</div></div></div>";

            DataSet ds = db.getDataSet(String.Format(@"declare @lang varchar(10) set @lang = '{0}' ", L.Lang) + sql);
            //DataSet ds = db.getDataSet(sql);
            mnTabs.Items.Clear();
            foreach (DataRow dr in db.getRows(ds))
                mnTabs.Items.Add(new MenuItem(String.Format(item, db.getValue(dr, FDataTextField)), db.getValue(dr, FDataValueField)));
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
        
        private void TriggerDataBound()
        {
            if (DataBound != null)
                DataBound(this, EventArgs.Empty);
        }
    }
}
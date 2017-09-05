using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntSqlMenu : System.Web.UI.UserControl
    {
        public delegate void ESelectedChanged(string id, string value, string par1);
        public event ESelectedChanged SelectedChanged;

        string FGrupa = null;
        string FSelectCommand = null;   // lowercase

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidRights.Value = App.User.Rights;
            }
            else
            {
                if (!acordionMenu.RequireOpenedPane)
                {
                    int idx = ClickedIndex;
                    if (idx != -1)
                    {
                        acordionMenu.RequireOpenedPane = true;
                        if (acordionMenu.SelectedIndex == -1)   // klikam jeszcze raz w to samo - tylko wtedy trzeba zmienić
                            acordionMenu.SelectedIndex = idx;
                    }
                }
            }
        }

        public void Reload()
        {
            acordionMenu.DataBind();
        }

        private void TriggerSelectedChanged(string id, string cmd, string par1)
        {
            SelectedCmd = cmd;
            if (SelectedChanged != null)
                SelectedChanged(id, cmd, par1);
        }
        //---------------------------------
        public string SelectedCmd   // ubezpieczenia
        {
            set { Session["lmenusel"] = value; }
            get { return Tools.GetStr(Session["lmenusel"], null); }
        }

        //------------------------------
        public bool Select(string id)   // unikalny id z bazy lub null jak nic
        {
            //----- deselect -----
            acordionMenu.SelectedIndex = -1;
            for (int i = 0; i < acordionMenu.Panes.Count; i++)
            {
                ListView lv = acordionMenu.Panes[i].FindControl("lvPodKategorie") as ListView;
                if (lv != null)
                    lv.SelectedIndex = -1;
            }
            //----- select -----
            if (!String.IsNullOrEmpty(id))
            {
                for (int i = 0; i < acordionMenu.Panes.Count; i++)
                {
                    HiddenField hid = acordionMenu.Panes[i].FindControl("hidId") as HiddenField;
                    if (hid != null && hid.Value == id)
                    {
                        acordionMenu.SelectedIndex = i;
                        return true;
                    }
                    ListView lv = acordionMenu.Panes[i].FindControl("lvPodKategorie") as ListView;
                    if (lv != null)
                    {
                        for (int k = 0; k < lv.DataKeys.Count; k++)
                            if (lv.DataKeys[k].Value.ToString() == id)
                            {
                                lv.SelectedIndex = k;
                                return true;
                            }
                    }
                }
                return false;
            }
            return true;
        }
        //--------
        private bool unselect = false;

        public void Unselect()
        {
            unselect = true;
            acordionMenu.RequireOpenedPane = false;  // tylko programowo wyłączam
        }
        //------------------------------
        int acordionItemIndex = 0;

        protected void acordionMenu_DataBinding(object sender, EventArgs e)
        {
            acordionItemIndex = 0;
        }

        private bool urlCompare(string url1, string url2)
        {
            //cmd: >>>url>>>~/Portal/Pliki.aspx?p=3
            //RawUrl: /portal2/portal/pliki.aspx?p=2        

            int len = url1.Length;
            if (len > 1)
                return url2.EndsWith(url1.Substring(1, len - 1));
            else
                return false;
        }

        private bool IsSelected(string cmd)
        {
            //Log.Info(Log.PORTAL, cmd, FSelectCommand);
            //20160901 T wyłączam bo się nie zaznacza
            //FSelectCommand = SelectedCmd;

            if (!String.IsNullOrEmpty(cmd) && !String.IsNullOrEmpty(FSelectCommand))
            {
                string url;
                if (Tools.IsUrl(cmd, out url))
                {
                    //Log.Info(Log._PORTAL, ">>>url>>>" + url, FSelectCommand);
                    //return url.ToLower() == FSelectCommand;
                    return urlCompare(url.ToLower(), FSelectCommand);
                }
                else
                    return cmd.ToLower() == FSelectCommand;
            }
            return false;
        }

        Label lb = null;
        LinkButton lbt = null;
        bool isSubSelected = false; 

        protected void acordionMenu_ItemDataBound(object sender, AjaxControlToolkit.AccordionItemEventArgs e)
        {
            if (e.ItemType == AjaxControlToolkit.AccordionItemType.Content)
            {
                ListView lv = e.AccordionItem.FindControl("lvPodKategorie") as ListView;
                if (lv != null)
                {
                    HiddenField hidId = e.AccordionItem.FindControl("hidId") as HiddenField;
                    HiddenField hidCmd = e.AccordionItem.FindControl("hidCmd") as HiddenField;

                    DataRowView drv = (DataRowView)e.AccordionItem.DataItem;
                    string cmd = drv["Command"].ToString();
                    string id = drv["Id"].ToString();

                    if (IsSelected(cmd))
                        acordionMenu.SelectedIndex = acordionItemIndex;     // poziom główny

                    isSubSelected = false;
                    SqlDataSource ds = e.AccordionItem.FindControl("SqlDataSource1") as SqlDataSource;
                    ds.SelectParameters["ParentId"].DefaultValue = hidId.Value;
                    lv.DataSourceID = "SqlDataSource1";
                    lv.DataBind();
                    if (isSubSelected)
                        acordionMenu.SelectedIndex = acordionItemIndex;     // submenu

                    if (lb != null)// && lbt != null)
                        if (lv.Items.Count > 0)
                        {
                            //lb.Attributes["onclick"] = String.Format("javascript:window.setTimeout(function(){{doClick('{0}');}}, 400);return true;", lbt.ClientID);   // wyszło ze return nie ma żadnego znaczenia...  <<< powodowało zwinięcie przy acordionMenu.RequireOpenedPane = false, daltego button poza acordeonem
                            lb.Attributes["onclick"] = String.Format("javascript:window.setTimeout(function(){{acordeonSelect('{0}','{1}','{2}');}}, 400);return true;", hidKatCommand.ClientID, btKatSelect.ClientID, hidCmd.Value);
                        }
                        else
                        {
                            lb.Attributes["onclick"] = String.Format("javascript:doClick('{0}');return true;", lbt.ClientID);
                        }

                }
                acordionItemIndex++;
            }
            else    // Header
            {
                lb = Tools.FindLabel(e.AccordionItem, "lbCaption");
                lbt = Tools.FindLinkButton(e.AccordionItem, "lbtKategoria");
            }
        }

        protected void acordionMenu_ItemCommand(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    int idx = acordionMenu.SelectedIndex;
                    
                    if (idx == -1)
                    {
                        acordionMenu.SelectedIndex = 0;

                        /*
                        for (int i = 0; i < acordionMenu.Panes.Count; i++)
                        {
                            AjaxControlToolkit.AccordionPane pp = acordionMenu.Panes[i];

                        }
                        */
                    }


                    ClickedIndex = idx;

                    if (idx != -1)
                    {
                        HiddenField hid = acordionMenu.Panes[idx].FindControl("hidId") as HiddenField;
                        string id = hid != null ? hid.Value : null;
                        hid = acordionMenu.Panes[idx].FindControl("hidPar1") as HiddenField;
                        string par1 = hid != null ? hid.Value : null;
                        TriggerSelectedChanged(id, e.CommandArgument.ToString(), par1);
                    }
                    break;
            }
        }

        protected void acordionMenu_PreRender(object sender, EventArgs e)
        {
            if (unselect)
            {
                unselect = false;
                ClickedIndex = -1;
                acordionMenu.RequireOpenedPane = false;
                Select(null);
            }
        }

        //--------------------------------
        protected void lvPodKategorie_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView lv = (ListView)sender;
            switch (e.CommandName)
            {
                case "Select":
                    string id = Tools.GetDataKey(lv, e);
                    string par1 = Tools.GetText(e.Item, "hidPar1");
                    TriggerSelectedChanged(id, e.CommandArgument.ToString(), par1);
                    break;
                case "Deselect":
                    TriggerSelectedChanged(null, null, null);
                    break;
            }
        }

        protected void lvPodKategorie_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DataRowView drv = Tools.GetDataRowView(e);
                string cmd = drv["Command"].ToString();
                ListView lv = (ListView)sender;
                if (lv.SelectedIndex == -1 && IsSelected(cmd))
                {
                    acordionMenu.SelectedIndex = acordionItemIndex;
                    lv.SelectedIndex = ((ListViewDataItem)e.Item).DisplayIndex;
                    isSubSelected = true;  //acordion
                }
            }
        }

        protected void lvPodKategorie_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //--------------------------------
        protected void SqlDataSource3_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }
        //--------------------------------
        protected void btKatSelect_Click(object sender, EventArgs e)
        {
            ClickedIndex = acordionMenu.SelectedIndex;
            //acordionMenu.RequireOpenedPane = true; trzeba w PageLoad zrobić zeby się do ViewState zapisało
        }
        //--------------------------------
        public string Grupa
        {
            set
            {
                FGrupa = value;
                hidGrupa.Value = value;
            }
            get { return FGrupa; }
        }

        public int ClickedIndex   // z acordeon'a
        {
            set { ViewState["clidx"] = value; }
            get { return Tools.GetInt(ViewState["clidx"], -1); }
        }

        public string SelectCommand   // komenda do zaznaczenia, ustawiana przed DataBind - powoduje rozwinięcie acordeonu i zaznaczenie zgodniej opcji 
        {
            set { FSelectCommand = Tools.GetRedirectUrl(value).ToLower(); }
        }
    }
}
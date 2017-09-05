using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;
using System.Text;

//http://forums.asp.net/t/1120892.aspx?Accordian+Selected+Index+problem+after+refresh+in+firefox



namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntSqlReports : System.Web.UI.UserControl
    {
        public event EventHandler Rendered;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.IsOriginalUser)
                    hidRights.Value = App.User.Rights;
                else
                    /*if (App.User.HasRight(AppUser._rScorecardsAdmin) && !AppUser.HasRight(App.User.OriginalRights, AppUser._rScorecardsAdmin))    // zastępuje admina, a nie jestem adminiem 
                    {
                        //hidRights.Value = App.User.OriginalRights;                                  // troche uproszczenie - wchodzę ze swoimi uprawnieniami
                        hidRights.Value = AppUser.SetRights(App.User.Rights, App.User.OriginalRights, // zeruję uprawnienia admina jeżeli sam ich nie mam
                            //AppUser._rScorecardsAdmin,
                            //AppUser._rScorecardsHR,
                            //AppUser.rScorecardsControlling,
                            //AppUser.rScorecardsKwoty, 
                            //AppUser.rScorecardsZoom
                            AppUser.rScorecardsRestricted
                            );
                    }
                    else */
                    hidRights.Value = App.User.Rights;
                hidMode.Value = FMode.ToString();
                RenderMenu();
            }


        }

        string GetUrl(string id)
        {
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            return ResolveUrl(String.Format("~/Redirect.aspx?p={0}&f={1}", e, "RaportF.aspx"));
        }

        public bool IsEmpty()
        {
            return String.IsNullOrEmpty(litMenu.Text);
        }

        void RenderMenu()
        {
            DataTable dt = db.Select.TableCon(dsData, db.strParam(Grupa), Mode, db.nullParamStr(hidRights.Value));
            StringBuilder sb = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                sb.Append("<ul class='sql-menu'>");
                sb.Append(RenderNode(dt, null));
                sb.Append("</ul>");
            }
            litMenu.Text = sb.ToString();
            if (Rendered != null)
                Rendered(null, EventArgs.Empty);
        }

        string RenderNode(DataTable dt, int? id)
        {
            DataRow[] rows = dt.Select(String.Format(id == null ? "ParentId is NULL" : String.Format("ParentId = {0}", id)));
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in rows)
            {
                int? pid = (int?)dr["Id"];
                string node = HasChildren(dt, pid) ? "<ul>{0}</ul>" : "{0}";
                string cmd = db.getValue(dr, "Command");
                string link = "";
                string pm1 = db.getValue(dr, "Par1");
                if (!Tools.IsUrl(cmd, out link))
                {
                    if (cmd != "TOGGLE")
                        link = GetUrl(pid.ToString());
                    else
                        link = "";
                }
                link = !String.IsNullOrEmpty(link) ? link : "javascript://";
                if (!String.IsNullOrEmpty(pm1)) link += "?p1=" + pm1;

                sb.Append("<li>");
                sb.AppendFormat("<a class='toggler' href='{1}'>{0}</a>", dr["MenuText"], ResolveUrl(link));
                sb.AppendFormat("<span class='description'>{0}</span>", dr["ToolTip"]);
                sb.AppendFormat(node, RenderNode(dt, pid));
                sb.Append("</li>");
            }
            return sb.ToString();//String.Format("<li><a href=''>{0}</a></li>", dr["MenuText"]);
        }

        bool HasChildren(DataTable dt, int? id)
        {
            DataRow[] rows = dt.Select(String.Format(id == null ? "ParentId is NULL" : String.Format("ParentId = {0}", id)));
            return rows.Length > 0;
        }



        public delegate void ESelectedChanged(string id, string value, string par1);
        public event ESelectedChanged SelectedChanged;

        string FGrupa = null;
        string FSelectCommand = null;   // lowercase

        int FMode = moKier;
        const int moKier = 1;
        const int moAll = 2;   // -> na Par1 jest filtr co ma się gdzie pokazywać 1,2,3

        const string PAR = "a=";
        const string PAR_ALL = "a=1";

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        if (App.User.IsOriginalUser)
        //            hidRights.Value = App.User.Rights;
        //        else
        //            /*if (App.User.HasRight(AppUser._rScorecardsAdmin) && !AppUser.HasRight(App.User.OriginalRights, AppUser._rScorecardsAdmin))    // zastępuje admina, a nie jestem adminiem 
        //            {
        //                //hidRights.Value = App.User.OriginalRights;                                  // troche uproszczenie - wchodzę ze swoimi uprawnieniami
        //                hidRights.Value = AppUser.SetRights(App.User.Rights, App.User.OriginalRights, // zeruję uprawnienia admina jeżeli sam ich nie mam
        //                    //AppUser._rScorecardsAdmin,
        //                    //AppUser._rScorecardsHR,
        //                    //AppUser.rScorecardsControlling,
        //                    //AppUser.rScorecardsKwoty, 
        //                    //AppUser.rScorecardsZoom
        //                    AppUser.rScorecardsRestricted
        //                    );
        //            }
        //            else */
        //                hidRights.Value = App.User.Rights;
        //        hidMode.Value = FMode.ToString();
        //    }
        //    else
        //    {
        //        if (!acordionMenu.RequireOpenedPane)
        //        {
        //            int idx = ClickedIndex;
        //            if (idx != -1)
        //            {
        //                acordionMenu.RequireOpenedPane = true;
        //                if (acordionMenu.SelectedIndex == -1)   // klikam jeszcze raz w to samo - tylko wtedy trzeba zmienić
        //                    acordionMenu.SelectedIndex = idx;
        //            }
        //        }
        //    }
        //}

        public void Reload()
        {
            acordionMenu.DataBind();
        }

        //private void TriggerSelectedChanged(string id, string cmd)
        //{
        //    if (SelectedChanged != null)
        //        SelectedChanged(id, cmd);
        //}

        private void TriggerSelectedChanged(string id, string cmd, string par1)
        {
            if (SelectedChanged != null)
                SelectedChanged(id, cmd, par1);
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

        private bool IsSelected(string cmd)
        {
            //Log.Info(Log.PORTAL, cmd, FSelectCommand);


            if (!String.IsNullOrEmpty(cmd))
            {
                string url;
                if (Tools.IsUrl(cmd, out url))
                {
                    //Log.Info(Log.PORTAL, ">>>url>>>" + url, FSelectCommand);

                    return url.ToLower() == FSelectCommand;
                }
                else
                    return cmd == FSelectCommand;
            }
            return false;
        }

        //Label lb = null;
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

                    /*
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
                    */
                    if (lbt != null)// && lbt != null)
                        if (lv.Items.Count > 0)
                        {
                            //lb.Attributes["onclick"] = String.Format("javascript:window.setTimeout(function(){{doClick('{0}');}}, 400);return true;", lbt.ClientID);   // wyszło ze return nie ma żadnego znaczenia...  <<< powodowało zwinięcie przy acordionMenu.RequireOpenedPane = false, daltego button poza acordeonem
                            lbt.Attributes["onclick"] = String.Format("javascript:window.setTimeout(function(){{acordeonSelect('{0}','{1}','{2}');}}, 400);return true;", hidKatCommand.ClientID, btKatSelect.ClientID, hidCmd.Value);
                        }
                        else
                        {
                            lbt.Attributes["onclick"] = null;
                        }

                }
                acordionItemIndex++;
            }
            else    // Header
            {
                //lb = Tools.FindLabel(e.AccordionItem, "lbCaption");
                lbt = Tools.FindLinkButton(e.AccordionItem, "lbtKategoria");
            }
        }

        private string GetCmd(string cmd)
        {
            if (FMode == moAll && cmd.ToLower().StartsWith("url:") && !cmd.Contains(PAR))
                cmd += (cmd.Contains("?") ? "&" : "?") + PAR_ALL;
            return cmd;
        }

        protected void acordionMenu_ItemCommand(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    int idx = acordionMenu.SelectedIndex;
                    ClickedIndex = idx;

                    if (idx != -1)
                    {
                        HiddenField hid = acordionMenu.Panes[idx].FindControl("hidId") as HiddenField;
                        string id = hid != null ? hid.Value : null;
                        hid = acordionMenu.Panes[idx].FindControl("hidPar1") as HiddenField;
                        string par1 = hid != null ? hid.Value : null;
                        TriggerSelectedChanged(id, GetCmd(e.CommandArgument.ToString()), par1);
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
                    TriggerSelectedChanged(id, GetCmd(e.CommandArgument.ToString()), par1);
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
        ////--------------------------------
        public string Grupa
        {
            set
            {
                FGrupa = value;
                hidGrupa.Value = value;
            }
            get { return FGrupa; }
        }

        public int Mode
        {
            set
            {
                hidMode.Value = value.ToString();
                FMode = value;
            }
            get { return FMode; }
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
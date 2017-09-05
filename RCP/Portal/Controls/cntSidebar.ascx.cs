using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntSidebar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbUserName.Text = App.User.NazwiskoImie;
                lnkSidebarEditMode.Visible = App.User.IsSuperuser && false;
                litTitle.Text = GetSidebarTitle();

                if (Lic.Social)
                {
                    cntAvatar.PostBackUrl = "~/Portal/Social/Profil.aspx";
                    divSpolecznosc.Visible = true;
                    paTitlePortal.Visible = true;
                }
            }
            //Tools.ExecuteJavascript("handleSidebar();");
        }

        protected string GetMenuIdsAfterRightsCheck(DataTable dt)
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(App.User);
            string ids = "";
            foreach (DataRow dr in dt.Rows)
            {
                bool hasRights = db.CheckRightsExpr(userRights, db.getValue(dr, "Rights"));
                if (hasRights)
                    ids += db.getValue(dr, "Id") + ",";
            }
            return ids;
        }

        protected void dsLeftMenu_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            DataTable dt = db.Select.Table(dsLeftMenuItems, String.IsNullOrEmpty(hidGroup.Value) ? "PRAC" : hidGroup.Value);
            e.Command.Parameters["@ids"].Value = GetMenuIdsAfterRightsCheck(dt);
        }

        protected void dsLeftMenuSocial_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            DataTable dt = db.Select.Table(dsLeftMenuSocialItems);
            e.Command.Parameters["@ids"].Value = GetMenuIdsAfterRightsCheck(dt);
        }

        protected void lnkLeftMenuRedirect_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            string split = (lb.CommandArgument);
            string cmd = split.Split(';')[0];
            string id = split.Split(';')[1];


            if (EditMode)
            {
                cntSidebarEditModal.Show(id, Group);
            }
            else
            {                
                if (!String.IsNullOrEmpty(cmd))
                {
                    string url = "";
                    if (Tools.IsUrl(cmd, out url))
                    {
                        //App.Redirect(url);
                    }
                    else if (cmd.Contains("REPORT"))
                    {
                        string repId = cmd.Split(':')[1];
                        url = GetUrl(repId);
                    }
                    bool append = url.Contains("?");
                    url += String.Format("{0}lm={1}", (append ? "&" : "?"), id);
                    SelectedItem = lb.Text;
                    App.Redirect(url);
                }
            }
        }

        string GetUrl(string id)
        {
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            return ResolveUrl(String.Format("/Redirect.aspx?p={0}&f={1}", e, "RaportF.aspx"));
        }

        protected void lbUserName_Click(object sender, EventArgs e)
        {
            App.User.Reload(false);
            App.Redirect(App.DefaultForm);
        }

        public void Set(string grupa)
        {
            this.Group = grupa;
            rpLeftMenu.DataBind();
        }

        public String GetLeftMenuSelectedClass(String id)
        {
            return GetSelectedClass("lm", id);
        }

        public String GetSelectedClass(String queryParam, String id)
        {
            String selected = Request.QueryString[queryParam];
            if (id == selected)
                return "selected ";
            return "";
        }

        public String GetSidebarTitle()
        {
            return hidGroup.Value == "KIER" ? "Panel kierownika" : "Panel pracownika";
        }

        void PrepareEditMode(bool editMode)
        {
            EditMode = editMode;
            lblMode.Visible = editMode;
            lnkAddNewItem.Visible = editMode;

            lnkSidebarEditMode.Visible = !editMode;
            lnkSidebarEditModeCancel.Visible = editMode;

            rpLeftMenu.DataBind();
        }

        protected void lnkSidebarEditMode_Click(object sender, EventArgs e)
        {
            divSidebar.Attributes["class"] = "sidebar sidebar-left edit";
            Tools.ExecuteJavascript("sidebarEdit();");
            PrepareEditMode(true);
        }

        protected void lnkSidebarEditModeCancel_Click(object sender, EventArgs e)
        {
            divSidebar.Attributes["class"] = "sidebar sidebar-left";
            Tools.ExecuteJavascript("sidebarPreview();");
            PrepareEditMode(false);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {

        }

        protected void lnkAddNewItem_Click(object sender, EventArgs e)
        {
            cntSidebarEditModal.Show(Group);
        }

        protected void cntSidebarEditModal_Saved(object sender, EventArgs e)
        {
            upMain.Update();
            rpLeftMenu.DataBind();
        }

        public static String SelectedItem
        {
            set { HttpContext.Current.Session["sesSelectedLeftMenu"] = value; }
            get { return (String)HttpContext.Current.Session["sesSelectedLeftMenu"]; }
        }

        public bool EditMode
        {
            get { return Tools.GetViewStateBool(ViewState["vEditMode"], false); }
            set { ViewState["vEditMode"] = value; }
        }

        public String Group
        {
            get { return hidGroup.Value; }
            set { hidGroup.Value = value; }
        }
    }
}
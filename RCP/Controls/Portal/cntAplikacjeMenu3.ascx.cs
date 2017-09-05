using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.IO;

namespace HRRcp.Controls.Portal
{
    public partial class cntAplikacjeMenu3 : System.Web.UI.UserControl
    {
        string FCId = "appMenu1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (EditMode)
                {

                }
            }
        }

        public void Prepare()
        {
            btnAddApp.Visible = EditMode;
            Repeater1.DataBind();
        }

        protected string GetPath(object ico)
        {
            return App.ImagesPathPortal + ico.ToString();
        }

        public string GetCId()
        {
            return FCId;
        }

        protected string GetUrl(object cmd)
        {
            string c = cmd.ToString();
            string url;
            if (Tools.IsUrl(c, out url))
                return url;
            else
                return c;
        }

        //------------------------
        public string CId
        {
            set { FCId = value; }
            get { return FCId; }
        }

        public string Menu
        {
            set { hidMenu.Value = value; }
            get { return hidMenu.Value; }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "click":
                    string c = e.CommandArgument.ToString();
                    string url;


                    PortalMasterPage.CheckLogout2(c);


                    if (Tools.IsUrl(c, out url))
                        App.Redirect(url);
                    else
                    {
                        url = Tools.GetRedirectUrl(c);
                        App.Redirect(url);
                        switch (c)
                        {
                            case "":
                                break;
                        }
                    }
                    break;
            }
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbt = e.Item.FindControl("LinkButton1") as LinkButton;
                if (lbt != null)
                {
                    //   lbt.CommandArgument = ((DataRowView)e.Item.DataItem)["Command"].ToString();
                }
                
                /* juan */

                //Tools.MakeConfirmDeleteRecordButton(e.Item, "lnkRemoveApp");

                LinkButton lnkRemoveApp = e.Item.FindControl("lnkRemoveApp") as LinkButton;
                if (lnkRemoveApp != null)
                {
                    Button btnRemoveApp = e.Item.FindControl("btnRemoveApp") as Button;
                    if (btnRemoveApp != null)
                    {
                        //Tools.MakeConfirmButton(lnkRemoveApp, "Czy na pewno chcesz usunąć aplikacje?", btnRemoveApp, null);
                      
                        //   lbt.CommandArgument = ((DataRowView)e.Item.DataItem)["Command"].ToString();
                    }
                }

            }
        }

        /* ======================================================================= */

        protected void AddApp(object sender, EventArgs e)
        {
            string appName = tbAppName.Text;
            string appLink = tbAppLink.Text;
            string appHint = tbAppHint.Text;
            string appImgPath = string.Empty;
            string filename = string.Empty;
            string order = tbOrder.Text;

            if (fuAppImage.HasFile)
            {
                try
                {
                    filename = Path.GetFileName(fuAppImage.FileName);
                    //appImgPath = "/3/" + filename;
                    fuAppImage.SaveAs(Server.MapPath("~/images/Portal/3/" + filename));
                }
                catch (Exception ex)
                {
                }
            }

            db.Execute(dsInsertApp, db.strParam(Menu), db.strParam(appName), db.strParam(appHint), db.strParam("url:" + appLink), db.strParam("3/" + filename), db.nullParam(order));
            Repeater1.DataBind();
        }

        //protected void RemoveAppConfirm(object sender, EventArgs e)
        //{
        //    string id = (sender as LinkButton).CommandArgument;
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        //btnRemoveApp.CommandArgument = id;
        //        //Tools.ShowConfirm("Czy na pewno chcesz usunąć aplikację?", btnRemoveApp);
        //    }
        //}

        protected void RemoveAppConfirm(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                btnRemoveApp.CommandArgument = id;
                Tools.ShowConfirm("Czy na pewno chcesz usunąć aplikację?", btnRemoveApp);
            }
        }

        protected void RemoveApp(object sender, EventArgs e)
        {
            string id = (sender as Button).CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                db.Execute(dsRemoveApp, id);
                Repeater1.DataBind();
            }
            else
            {
                Tools.ShowMessage("Błąd!");
            }
        }

        protected void ShowNewApp(object sender, EventArgs e)
        {
            paAddApp.Title = "Dodaj aplikację";
            btnInsertApp.Visible = true;
            btnSaveApp.Visible = false;
            paAddApp.Show();
        }

        protected void ShowEditApp(object sender, EventArgs e)
        {
            string id = (sender as LinkButton).CommandArgument;
            if(!String.IsNullOrEmpty(id))
            {
                DataRow dr = db.Select.RowCon(dsAppData, id);
                paAddApp.Title = "Edytuj aplikację";
                btnInsertApp.Visible = false;
                btnSaveApp.Visible = true;
                btnSaveApp.CommandArgument = id;
                paAddApp.Show();
                tbAppName.Text = db.getValue(dr, "AppName");
                tbAppHint.Text = db.getValue(dr, "AppHint");
                tbAppLink.Text = db.getValue(dr, "AppLink").Replace("url:", "");
                tbOrder.Text = db.getValue(dr, "AppOrder");
                hidLastImage.Value = db.getValue(dr, "AppImage");
            }
        }

        protected void EditApp(object sender, EventArgs e)
        {
            string id = btnSaveApp.CommandArgument;
            if (!String.IsNullOrEmpty(id))
            {
                string appName = tbAppName.Text;
                string appLink = tbAppLink.Text;
                string appHint = tbAppHint.Text;
                string appImgPath = string.Empty;
                string filename = string.Empty;
                string order = tbOrder.Text;

                if (fuAppImage.HasFile)
                {
                    try
                    {
                        filename = Path.GetFileName(fuAppImage.FileName);
                        //appImgPath = "/3/" + filename;
                        fuAppImage.SaveAs(Server.MapPath("~/images/Portal/3/" + filename));
                    }
                    catch (Exception ex)
                    {
                    }
                    filename = "3/" + filename;
                }
                else
                    filename = hidLastImage.Value;

                db.Execute(dsUpdateApp, db.strParam(Menu), db.strParam(appName), db.strParam(appHint), db.strParam("url:" + appLink), db.strParam(filename), db.nullParam(order), id);
                Repeater1.DataBind();
            }
        }

        public Boolean IsInEditMode()
        {   
            return EditMode;
        }

        public Boolean EditMode
        {
            get { return Tools.GetViewStateBool(ViewState["vEditMode"], false); }
            set { ViewState["vEditMode"] = value; }
        }
    }
}
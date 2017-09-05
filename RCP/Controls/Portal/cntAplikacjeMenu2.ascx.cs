using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntAplikacjeMenu2 : System.Web.UI.UserControl
    {
        string FCId = "appMenu1";

        protected void Page_Load(object sender, EventArgs e)
        {

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
        protected void lvMenu_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                LinkButton lbt = e.Item.FindControl("LinkButton1") as LinkButton;
                if (lbt != null)
                {
                    //   lbt.CommandArgument = ((DataRowView)e.Item.DataItem)["Command"].ToString();
                }
            }

        }

        protected void lvMenu_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "click":
                    string c = e.CommandArgument.ToString();
                    string url;
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

        //----------------------
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
    }
}
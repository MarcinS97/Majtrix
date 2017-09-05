using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntLoginAs : System.Web.UI.UserControl
    {
        const bool fromKiosk = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.IsPortalAdmin;
                if (adm)
                {
                    if (fromKiosk)
                        ddlLogin.Attributes["onchange"] = String.Format("javascript:return testLoginUser(this, '{0}');", ResolveUrl("~/Kiosk/Login.aspx"));
                }
                jsLoginTest.Visible = adm;
                paKioskLogin.Visible = adm;
            }
        }

        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(ddlLogin.SelectedValue);
            App.User._LoginAsUserId(p[0]);
            App.User.CheckPassLoginTest(App.User.NR_EW);
            App.KwitekKadryId = App.User.NR_EW;
            App.KwitekPracId = App.User.Id;
            AppUser.x_IdKarty = App.User.NR_EW;
            App.Redirect(App.DefaultForm);
        }

        protected void ddlLogin_DataBound(object sender, EventArgs e)
        {
            Tools.SelectItem(ddlLogin, App.User.Id);
        }
    }
}
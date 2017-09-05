using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Social
{
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Tools.ExecuteJavascript("prepareFullChat(" + App.User.Id + ", '" + ResolveUrl("~/Portal/PortalMethods.asmx") + "');");
                hidUser.Value = App.User.Id;
            }
        }

        protected void lnkSearchFriends_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Portal/Social/Znajomi.aspx");
        }
    }
}
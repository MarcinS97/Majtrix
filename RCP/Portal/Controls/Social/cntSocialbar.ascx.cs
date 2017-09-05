using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntSocialBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUser.Value = App.User.Id;
                Tools.ExecuteJavascript("prepareChat(" + App.User.Id +  ", '" + ResolveUrl("~/Portal/PortalMethods.asmx")  + "');");

                rpFriends.DataBind();
                if (rpFriends.Items.Count == 0)
                {
                    divNoFriends.Visible = true;
                }
            }
        }

        public String GetAvatar(String KadryId)
        {
            String avatarImg = Tools.GetAvatarImage(KadryId + ".jpg", true);
            return String.Format(@"background-image: url(""{0}"");", avatarImg);
        }
    }
}
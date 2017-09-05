using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Social
{
    public partial class cntMyFriends : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUser.Value = App.User.Id;
            }
        }

        public String GetAvatar(String kadryId)
        {
            return Tools.GetUserAvatar(kadryId);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
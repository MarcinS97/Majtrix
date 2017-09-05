using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp
{
    public partial class Start : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (App.User.HasAccess)
            {
                Tools.SetNoCache();
            }
            else
                App.ShowNoAccess("Start", null);
        }
        //------------------------------
        public static bool HasFormAccess
        {
            get { return true; }
        }
    }
}

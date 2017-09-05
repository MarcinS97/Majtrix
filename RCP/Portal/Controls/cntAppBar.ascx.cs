using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls
{
    public partial class cntAppBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Set(string grupa)
        {
            this.Group = grupa;
            rpApps.DataBind();
        }
        
        protected void lnkApp_Click(object sender, EventArgs e)
        {
            string cmd = (sender as LinkButton).CommandArgument;
            string url = string.Empty;
            PortalMasterPage3.CheckLogout2(cmd);

            Tools.IsUrl(cmd, out url);
            App.Redirect(url);
        }

        public String Group
        {
            get { return hidGroup.Value; }
            set { hidGroup.Value = value; }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class Scorecards : System.Web.UI.Page
    {
        private const string FormName = "Scorecardy - wybór";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.IsScAdmin;
                bool wn = App.User.IsScTL
                       || App.User.IsScKier
                       || App.User.IsScZarz;
                if (adm || wn)
                {
                    Tools.SetNoCache();

                }
                else
                    App.ShowNoAccess(FormName, null);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }
    }
}

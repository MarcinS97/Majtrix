using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class Parametry : System.Web.UI.Page
    {
        private const string FormName = "Scorecards - Parametry";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.IsScAdmin;
                if (adm)
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

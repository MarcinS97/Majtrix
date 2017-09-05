using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.BadaniaWstepne
{
    public partial class RejestrAdmin : System.Web.UI.Page
    {
        const string form_name = "Rejestr Badań Wstępnych - Administracja";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                AppUser user = AppUser.CreateOrGetSession();
                if (user.IsAdmin && user.HasRight(AppUser.rBadaniaWstepne))
                {
                    Info.SetHelp("HBADWSTADM");
                }
                else
                    App.ShowNoAccess(form_name, user);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(form_name);
        }

    }
}

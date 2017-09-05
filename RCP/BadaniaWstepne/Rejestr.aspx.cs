using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.BadaniaWstepne
{
    public partial class Rejestr : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (false)
                //    cntBadaniaWst1.uwagaZmianyBylyTW = 1;  // jak nie znajdzie zmiennej to wersja jest nadpisana i trzeba przywrócić !!!

                Tools.SetNoCache();
                AppUser user = AppUser.CreateOrGetSession();
                if (user.HasRight(AppUser.rBadaniaWstepne))
                {
                    Info.SetHelp("HBADWST");
                }
                else
                    App.ShowNoAccess("Badania wstępne - rejestr", user);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Rejestr Badań");
        }

        public static bool HasFormAccess
        {
            get { return Lic.BadaniaWstepne && App.User.HasRight(AppUser.rBadaniaWstepne); }
        }
    }
}

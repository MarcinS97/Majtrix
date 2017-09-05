using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class PodzialLudziAdm : System.Web.UI.Page
    {
        const string title = "Podział Ludzi - Administracja";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasRight(AppUser.rPodzialLudziAdm))
                {
                    Tools.SetNoCache();
                }
                else
                    App.ShowNoAccess(title, App.User);
            }
        }   

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(title);
        }

        public static bool HasFormAccess
        {
            get { return Lic.podzialLudzi && App.User.HasRight(AppUser.rPodzialLudziAdm); }
        }
    }
}


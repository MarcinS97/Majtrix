using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.MatrycaSzkolen
{
    public partial class Szkolenia : System.Web.UI.Page
    {
        const string FormName = "Pracownicy administracji";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess)
                {
                    if(HasFormAccess)
                    {
                        Tools.SetNoCache();
                        Info.SetHelp("HMSPRACADM");
                    }
                    else
                        App.ShowNoAccess(FormName, App.User);
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        public static bool HasFormAccess
        {
            get { return App.User.IsMSAdmin || App.User.HasRight(AppUser.rMSPracownicyAdm) || App.User.HasRight(AppUser.rMSBHP); }
        }
    }
}

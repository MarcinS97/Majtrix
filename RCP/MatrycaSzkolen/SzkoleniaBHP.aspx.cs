using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen
{
    public partial class SzkoleniaBHP : System.Web.UI.Page
    {
        const string FormName = "SzkoleniaBHP";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess)
                {
                    if (HasFormAccess)
                    {
                        Tools.SetNoCache();
                        Info.SetHelp(FormName);
                    }
                    else
                        App.ShowNoAccess(FormName, App.User);
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        public static bool HasFormAccess
        {
            get { return App.User.IsMSAdmin || App.User.HasRight(AppUser.rMSBHP); }
        }
    }
}
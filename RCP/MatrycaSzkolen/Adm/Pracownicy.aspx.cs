using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Adm
{
    public partial class Pracownicy : System.Web.UI.Page
    {
        const string FormName = "Pracownicy";

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
                    {
                        string p = Request.QueryString["p"];
                        if (p == "start")
                            App.Redirect(App.DefaultForm);  // albo na raport pokazujący wszystkich pracowników
                        else
                            App.ShowNoAccess(FormName, App.User);
                    }
                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        public static bool HasFormAccess
        {
            get { return App.User.IsMSAdmin; }
        }
    }
}
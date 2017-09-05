using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Adm
{
    public partial class Oddelegowania : System.Web.UI.Page
    {
        const string FormName = "Oddelegowania";

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
                        Przesuniecia.Prepare();

                        string pracId = null;//SelectPracIdPrzes;
                        if (!String.IsNullOrEmpty(pracId))
                        {
                            //SelectPracIdPrzes = null;
                            Przesuniecia.SelectPrac(pracId);
                        }

                    }
                    else
                        App.ShowNoAccess(FormName, App.User);
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
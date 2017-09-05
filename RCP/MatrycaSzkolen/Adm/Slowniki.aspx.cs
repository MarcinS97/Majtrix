using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Adm
{
    public partial class Slowniki : System.Web.UI.Page
    {
        const string FormName = "Słowniki";

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
            get { return App.User.IsMSAdmin; }
        }

        protected void Types_Deleting(object sender, EventArgs e)
        {
            SqlDataSourceCommandEventArgs args = (SqlDataSourceCommandEventArgs)e;

            string lineExists = db.Select.Scalar(dsLineExists, args.Command.Parameters["@Id"].Value);

            if(lineExists != "0")
            {
                Tools.ShowMessage("Istnieje już powiązanie z tą linią. Aby usunąć linię najpierw usuń powiązanie.");
                args.Cancel = true;
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class UbezpieczeniaForm : System.Web.UI.Page
    {
        const string FormName = "Portal - Ubezpieczenia";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (App.User.HasAccess)
                {
                    Prepare(Request["p"]);
                }
                else
                    App.ShowNoAccess(FormName, App.User);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        public void Prepare(string cmd)
        {
            cntUbezpieczenia.Prepare(cmd);
        }
    }
}
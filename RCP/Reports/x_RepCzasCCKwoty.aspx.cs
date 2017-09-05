using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Reports
{
    public partial class RepCzasCCKwoty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                if (App.User.HasRight(AppUser.rRepPodzialCCKwoty))
                {
                    if (App.User.HasRight(AppUser.rRepPodzialCCAll))
                        cntReport1.SQL1 = null;
                }
                else
                    App.ShowNoAccess("Raporty", App.User);
        }

        private string Prepare()
        {
            return null; 
        }
    }
}

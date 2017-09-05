using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class WnioskiUrlopoweAkceptacja : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                paWnioskiUrlopowe.Prepare();
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Portal - Wnioski Urlopowe Akceptacja");
        }
    }
}

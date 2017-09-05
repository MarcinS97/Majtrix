using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Reports
{
    public partial class RepCCSplity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                App_Code.App.CheckccPrawaSplity(cntReport1);
                //cntReport1.ExportCSV(null, true);
            }
        }
    }
}

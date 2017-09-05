using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntStrefySelect : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
            Tools.PrepareSorting(ListView1, 0, 10);
        }
    }
}
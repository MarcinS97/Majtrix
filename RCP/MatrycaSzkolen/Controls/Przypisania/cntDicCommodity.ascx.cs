using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntDicCommodity : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            HRRcp.App_Code.Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
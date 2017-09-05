using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls.Mails
{
    public partial class cntMailsGrupy : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            HRRcp.App_Code.Tools.PrepareDicListView(lvMailsGrupy, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
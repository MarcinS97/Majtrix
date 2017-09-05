using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Adm
{
    public partial class cntSciezkiPlikiEdit : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, Tools.ListViewMode.Bootstrap);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Adm
{
    public partial class cntDicMarginesy : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvMarginesy, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvMarginesy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
    }
}
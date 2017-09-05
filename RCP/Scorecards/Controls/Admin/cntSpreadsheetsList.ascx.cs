using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSpreadsheetsList : System.Web.UI.UserControl
    {
        public event EventHandler SheetSelected;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSpreadsheets, 0);
            Tools.PrepareSorting2(lvSpreadsheets, 1, 10);
        }

        protected void lvSpreadsheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SheetSelected != null) SheetSelected(lvSpreadsheets.SelectedDataKey.Value, EventArgs.Empty);

        }
    }
}
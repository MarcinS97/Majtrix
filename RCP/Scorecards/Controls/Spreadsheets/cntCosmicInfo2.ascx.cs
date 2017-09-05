using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntCosmicInfo2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(Boolean InEdit)
        {
            rowRemover.Visible = InEdit;
        }
    }
}
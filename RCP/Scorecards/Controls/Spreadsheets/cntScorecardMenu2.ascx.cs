using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntScorecardMenu2 : System.Web.UI.UserControl
    {
        public event EventHandler Saved;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ToggleItems(Boolean B)
        {
            save.Visible = B;
        }

        protected void Save(object sender, EventArgs e)
        {
            if (Saved != null) Saved(null, EventArgs.Empty);
        }

        protected void Back(object sender, EventArgs e)
        {
            if (Saved != null) Saved(null, EventArgs.Empty);
            Response.Redirect(ResolveUrl("~/Scorecards/Scorecards.aspx"));
        }
    }
}
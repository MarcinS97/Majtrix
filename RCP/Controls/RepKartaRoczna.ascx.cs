using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class RepKartaRoczna: System.Web.UI.UserControl
    {
        public event EventHandler DataSaved;

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        //---------------------------
        public void Prepare(string rok, string pracId)
        {
            cntPlanPracy.Prepare(rok, pracId);
        }
    }
}
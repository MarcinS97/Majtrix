using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Raporty
{
    public partial class cntWniosekWolneZaNadg : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string wnId)
        {
            WniosekId = wnId;
            lvWniosek.DataBind();
        }

        public string WniosekId
        {
            set { hidWniosekId.Value = value; }
            get { return hidWniosekId.Value; }
        }
    }
}
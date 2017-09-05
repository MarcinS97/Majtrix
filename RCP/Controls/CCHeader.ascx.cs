using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class CCHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int Prepare(string data)
        {
            hidData.Value = data;
            Repeater1.DataBind();
            return Repeater1.Items.Count;
        }
    }
}
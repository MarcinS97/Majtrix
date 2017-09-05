using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class AdmLogControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lvEvents.DataBind();
        }

        protected void cbAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)lvEvents.FindControl("cbAutoRefresh");
            Timer1.Enabled = cb.Checked;
        }

        protected void lvEvents_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
    }
}
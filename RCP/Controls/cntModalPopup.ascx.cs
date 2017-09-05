using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntModalPopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show(string title)
        {
            Tools.ShowDialog(this, paModalPopup.ClientID, null, btCancel, title);
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void btCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btOption_Click(object sender, EventArgs e)
        {

        }


    }
}
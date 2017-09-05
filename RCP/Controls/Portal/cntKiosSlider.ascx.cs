using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntKiosSlider : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void tbtEnter_Click(object sender, EventArgs e)
        {
            //Response.Redirect(App.KierStartForm);
        }

        protected void tbtTest_Click(object sender, EventArgs e)
        {
            /*
            const string testpid = "-1000";
            if (Pracownik.Login(testpid))
                Response.Redirect(App.StartForm);
            else
                App.ShowPracNoAccess(true);
            */
        }
    }
}
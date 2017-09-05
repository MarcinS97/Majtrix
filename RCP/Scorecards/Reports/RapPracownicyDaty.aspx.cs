using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Reports
{
    public partial class RapPracownicyDaty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapPracownicyDaty))
            {
                Tools.SetNoCache();
            }
            else
            {
                App.ShowNoAccess("Raport czynności", App.User);
            }

            //if (!App.User.HasRight(AppUser.rScorecardsZoom))
            //    App.ShowNoAccess("Raport czynności", App.User);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
    }
}

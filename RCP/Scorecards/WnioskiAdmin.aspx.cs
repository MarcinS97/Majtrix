using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards
{
    public partial class WnioskiAdmin : System.Web.UI.Page
    {
       private const string FormName = "Scorecards - Wnioski premiowe";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm = App.User.IsScAdmin;
                //bool wn = App.User.IsScTL
                //       || App.User.IsScKier
                //       || App.User.IsScZarz;
                //bool acc = App.User.HasRight(AppUser.rScorecardsWnAcc);
                //bool hr = App.User.HasRight(AppUser._rScorecardsHR);
                //bool contr = App.User.HasRight(AppUser.rScorecardsControlling);
                if (adm)
                {
                    Tools.SetNoCache();


                }
                else
                    App.ShowNoAccess(FormName, null);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }
    
    }
}

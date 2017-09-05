using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.Security;
//using HRRcp.Controls.PlanUrlopow;

namespace HRRcp
{
    public partial class AbsencjeDlugotrwale : System.Web.UI.Page
    {
        const string FormName = "Absencje długotrwałe";

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if PORTAL
            this.MasterPageFile = App.PortalMaster;
#endif
#if SCARDS
            this.MasterPageFile = App.ScMaster;
#endif
#if RCP
            this.MasterPageFile = App.GetMasterPage();
#endif
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.RePostCancel();
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess && 
                    (App.User.IsAdmin || App.User.HasRight(AppUser.rAbsencjeDlugotrwale)
#if SCARDS
                    || App.User.IsScControlling || App.User.HasRight(AppUser.rScorecardsHR)
#endif                    
                    ))
                {
                    Tools.SetNoCache();

                }
                else App.ShowNoAccess(FormName, App.User);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            App.Master.SetWideJs(false);
            base.OnPreRender(e);
        }

        public static void Show()
        {
            HttpContext.Current.Response.Redirect("~/AbsencjeDlugotrwale.aspx");
        }
        //---------------------------------------------
    }
}

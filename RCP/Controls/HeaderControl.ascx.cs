using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class HeaderControl : System.Web.UI.UserControl
    {
        bool FShowInfo = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();
#if KWITEK || PORTAL
                if (user.IsPassLogged)
                {
                    if (!String.IsNullOrEmpty(user.Id))
                        lbUser.Text = user.ImieNazwisko;
                }
                else
                {
                    ltWelcome.Visible = false;
                    lbUser.Visible = false;
                }
#else
                if (!String.IsNullOrEmpty(user.Id))
                    lbUser.Text = user.ImieNazwisko;
#endif
                lbProgram.Text = Tools.GetAppName();

                if (FShowInfo)
                {
                    topMenuContainer.Attributes["class"] = "topMenuContainerP";
                    //ltTopInfo.Text = Info.GetInfoText(Info.INFO_PROGRAM);
                    topInfo.Visible = true;
                }
                else
                {
                    topMenuContainer.Attributes["class"] = "topMenuContainer";
                    topInfo.Visible = false;
                }
            
#if PORTAL || KWITEK
    #if SPX
                imgLogo.ImageUrl = "~/images/spx/spx_logo.png";
    #elif OKT || APATOR
                imgLogo.ImageUrl = App.StylePath + "logo.png";
                paLogo.Visible = true;
    #elif KDR
                imgLogo.ImageUrl = "~/images/kdr/kdr_logo.png";
                paLogo.Visible = true;
#else
                imgLogo.ImageUrl = "~/images/iQor/iQor2a.png";
                paLogo.Visible = true;
    #endif
#endif
            }
        }
        //--------------------------
        public bool ShowInfo
        {
            get { return FShowInfo; }
            set { FShowInfo = value; }
        }

        public HtmlGenericControl Logo
        {
            get { return paLogo; }
        }

        public string LogoImage
        {
            set { imgLogo.ImageUrl = value; }
            get { return imgLogo.ImageUrl; }
        }

        protected void lbtLogo_Click(object sender, EventArgs e)
        {
            App.Redirect(App.DefaultForm);
        }
    }
}
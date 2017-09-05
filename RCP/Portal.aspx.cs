using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class PortalPrac : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.EnableUpload();
#if KDR
                //((PortalMasterPage2)Master).LogoImage.Style.Add("display", "none");
                ((PortalMasterPage2)Master).paLeft.Style.Add("left", "-1000px");
                ((PortalMasterPage2)Master).paRight.Style.Add("left", "1000px");
#elif PORTAL

#else
                ((PortalMasterPage)Master).LogoImage.Style.Add("display", "none");
                ((PortalMasterPage)Master).paLeft.Style.Add("left", "-1000px");
                ((PortalMasterPage)Master).paRight.Style.Add("left", "1000px");
#endif

                /*
                if (App.User.HasRight(AppUser.r
                if (App.User.IsKierownik())
                {
                }
                else
                 */
            }
        }

        public string isSPX
        {
            get
            {
#if SPX
                return "true";
#else
                return "false";
#endif
            }
        }

        public string isZELMER
        {
            get
            {
#if ZELMER
                return "true";
#else
                return "false";
#endif
            }
        }



        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                frame1.Attributes["onload"] = "javascript:iframeLoaded(this);";
            }
        }

        public void Prepare(string url)
        {
            frame1.Attributes.Add("src", url);
        }

        protected void btOpen_Click(object sender, EventArgs e)
        {

        }

        protected void bt1_Click(object sender, EventArgs e)
        {
            pgPortalStart.Visible = true;
            Prepare("https://sites.google.com/a/jabil.com/bydgoszcz-intranet/");
            paCarousel.Visible = false;            
        }

        protected void bt2_Click(object sender, EventArgs e)
        {
            pgPortalStart.Visible = false;
            paCarousel.Visible = true;            
        }

        protected void bt3_Click(object sender, EventArgs e)
        {
            pgPortalStart.Visible = true;
            Prepare("http://www.jabil.com");
            paCarousel.Visible = false;            
        }
         */
    }
}


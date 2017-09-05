using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class PortalKier : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*
                if (App.User.HasRight(AppUser.r
                if (App.User.IsKierownik())
                {
                }
                else
                 */
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


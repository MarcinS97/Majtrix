using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Social
{
    public partial class cntAvatar : System.Web.UI.UserControl
    {
        bool FCustom = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!FCustom)
                {
                    if (String.IsNullOrEmpty(NrEw))
                    {
                        NrEw = App.User.NR_EW;
                    }
                    LoadAvatar(NrEw);
                }
            }
        }

        public void LoadAvatar(string nrew)
        {
            if (!String.IsNullOrEmpty(nrew))
            {
                String avatarImg = Tools.GetAvatarImage(nrew + ".jpg", true);
                imgUser.Style["background-image"] = String.Format(@"url(""{0}"");", avatarImg);
            }
        }

        public String NrEw
        {
            get { return ViewState["vNrEw"] as String; }
            set 
            {
                ViewState["vNrEw"] = value;
                if (FCustom)
                    LoadAvatar(value);
            }
        }

        public bool Custom
        {
            set { FCustom = value; }
            get { return FCustom; }
        }

        public String PostBackUrl
        {
            get { return lnkRedirect.PostBackUrl; }
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    lnkRedirect.Enabled = true;
                    lnkRedirect.PostBackUrl = value;
                }
                else
                {
                    lnkRedirect.Enabled = false;
                }
            }
        }

        public String Width
        {
            set { imgUser.Style["width"] = value; }
            get { return imgUser.Style["width"]; }
        }

        public String Height
        {
            set { imgUser.Style["height"] = value; imgUser.Style["background-size"] = "auto " + value; }
            get { return imgUser.Style["height"]; }
        }
    }
}
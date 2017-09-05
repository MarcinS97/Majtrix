using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class RepHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS
                img3.Src = "../images/RepLogo_Siemens.jpg";
#else
                //img3.Src = "../images/RepLogo_Jabil.jpg";
                //img3.Src = "../images/RepLogo_iqor.png";
                img3.Src = "../images/iqor1a.png";          // z wzornika
                //img3.Src = "../images/iqor3.png";         // nieprzezroczyste
                //img3.Src = "../images/iqor-logo.png";     // ze stoplki maili, nieprzezroczyste
#endif
#if VC
                img3.Src = "../images/VC/logo.png";          // z wzornika
                img3.Attributes["class"] = "replogo_demo";
#endif
            }
        }

        public string Caption
        {
            set { lbCaption.Text = value; }
            get { return lbCaption.Text; }
        }

        public string Caption1
        {
            set { lbCaption1.Text = String.IsNullOrEmpty(value) ? null : "<br />" + value; }
            get 
            {
                string v = lbCaption1.Text;
                return String.IsNullOrEmpty(v) ? null : v.Substring(6);
            }
        }

        public string Caption2
        {
            set { lbCaption2.Text = value; }
            get { return lbCaption2.Text; }
        }

        public int Icon
        {
            set 
            {
                img1.Visible = value == 1;
                img2.Visible = value == 2;
            }
            get 
            {
                if (img1.Visible) return 1;
                else if (img2.Visible) return 2;
                else return 0;
            }
        }
    }
}
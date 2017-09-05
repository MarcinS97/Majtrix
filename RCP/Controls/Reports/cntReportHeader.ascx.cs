using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class cntReportHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if SIEMENS
                img3.Src = "~/images/RepLogo_Siemens.jpg";
#elif VC
                //img3.Src = "~/images/VC/logo.png";
                img3.ImageUrl = "~/images/VC/logo.png";
#else
                //img3.Src = "~/images/RepLogo_Jabil.jpg";
                //img3.Src = "~/images/RepLogo_iqor.png";
                
                //img3.Src = "~/images/iqor1a.png";          // z wzornika
                //img3.ImageUrl = "~/images/iqor1a.png";
                img3.ImageUrl = "~/styles/User/raplogo.png";
#endif

#if SCARDS
                lbCaption0.Text = "Scorecards<br />";
#elif MS
                lbCaption0.Text = "Matryca Szkoleń<br />";
#else
                lbCaption0.Text = "Rejestracja Czasu Pracy<br />";
#endif


            }
        }

        public string PrintTitle
        {
            set { lbCaption0.Text = value; }
            get { return lbCaption0.Text; }
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
            set { lbCaption2.Text = String.IsNullOrEmpty(value) ? null : "<br />" + value; }
            get
            {
                string v = lbCaption2.Text;
                return String.IsNullOrEmpty(v) ? null : v.Substring(6);
            }
        }

        public string Caption3
        {
            set { lbCaption3.Text = String.IsNullOrEmpty(value) ? null : "<br />" + value; }
            get
            {
                string v = lbCaption3.Text;
                return String.IsNullOrEmpty(v) ? null : v.Substring(6);
            }
        }

        /*
        public string Caption1
        {
            set { lbCaption1.Text = value; }
            get { return lbCaption1.Text; }
        }

        public string Caption2
        {
            set { lbCaption2.Text = value; }
            get { return lbCaption2.Text; }
        }

        public string Caption3
        {
            set { lbCaption3.Text = value; }
            get { return lbCaption3.Text; }
        }
        */
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

        public bool IsEmpty
        {
            get 
            {
                return String.IsNullOrEmpty(Caption) &&
                       String.IsNullOrEmpty(Caption1) &&
                       String.IsNullOrEmpty(Caption2) &&
                       String.IsNullOrEmpty(Caption3);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class testPracownikCzasControl2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string RcpId
        {
            get { return hidRcpId.Value; }
            set { hidRcpId.Value = value; }
        }

        public string andRogers
        {
            set 
            {
                if (String.IsNullOrEmpty(value)) 
                {
                    hidRogersA.Value = null;
                    hidRogersC.Value = null;
                }
                else if (value.StartsWith("!"))
                {
                    hidRogersA.Value = "and not A.ECReaderId in (" + value.Substring(1) + ")";
                    hidRogersC.Value = "and not C.ECReaderId in (" + value.Substring(1) + ")";
                }
                else
                {
                    hidRogersA.Value = "and A.ECReaderId in (" + value + ")";
                    hidRogersC.Value = "and C.ECReaderId in (" + value + ")";
                }
            }
        }

        public string andRogersA
        {
            get { return hidRogersA.Value; }
        }

        public string andRogersC
        {
            get { return hidRogersC.Value; }
        }
    }
}
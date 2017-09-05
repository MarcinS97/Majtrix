using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Ubezpieczenia.Majatkowe
{
    public partial class Warunki : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = "../../Pliki/";
            string fileName = "";
            switch(Request.QueryString["t"])
            {
                case "OWU":
                    fileName = "6515_OWU_prev.pdf";
                    break;
                case "SWU":
                    fileName = "6530_SWU_prev.pdf";
                    break;
                default:

                    break;
            }
            litData.Text = String.Format("<embed src=\"{0}\" class=\"viewer\" >", path + fileName);
        }
    }
}
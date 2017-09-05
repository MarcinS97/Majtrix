using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string p = Request.QueryString["p"];
            string f = Request.QueryString["f"];
            
            if(!String.IsNullOrEmpty(f))
            {
                string s = String.Format(String.IsNullOrEmpty(p) ? "{0}" : "{0}?p={1}", f, p);
                App.Redirect(s);
            }
            else
            {
                App.ShowBadRepParameters("Redirect");
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Redirect");
        }
    }
}
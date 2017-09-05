using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class ServiceForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int mode = 0;
                string m = Request.QueryString["mode"];
                string p = Request.QueryString["p"];
                switch (m)
                {
#if SIEMENS || KDR
                    case "ALARMUS":
                    case "AUTOID":
                        mode = 1;
                        break;
#endif
#if VICIM
                    case "SATURN":
                        mode = 3;
                        break;
#endif
                    case "SCHEDULER":
                        mode = 2;
                        break;
                }
                Service.ExecuteStatic(mode, p);
            }
            finally         // zawsze dodajemy na końcu
            {
                Response.Write("<script language=javascript>this.window.opener = null;window.open('','_self'); window.close();</script>");
            }
        }
        
        private void Page_Error(object sender, System.EventArgs e)
        {
            string InfoEx;               
            if (Server.GetLastError() != null)
                 InfoEx = Server.GetLastError().Message;
            else InfoEx = null;
            Log.Error(Log.SERVICE, "Service", InfoEx);  // w opis1 z reguły jest refferer przy tSERVICE
            Server.ClearError();  //<<< odremować później !!! zeby nie pokazywał formatki
        }
    }
}

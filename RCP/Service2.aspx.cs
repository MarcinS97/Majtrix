using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class ServiceForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Service service = new Service();
                service.Execute2();     // TESTER
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

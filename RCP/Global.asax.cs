using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using System.Globalization;
using HRRcp.App_Code;
using System.Security.Principal;
using System.Web.Mvc;

namespace HRRcp
{
    public class Global : System.Web.HttpApplication
    {
        public int Test { get; set; }   // przykład użycia property globalnych -> App.Global
        public int Test2 = 0;

        public static Global Get        // analogiczna w App: App.Global.xxx lub Global.Get.xxx
        {
            get { return (Global)HttpContext.Current.ApplicationInstance; }
        }
        //----------------------

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
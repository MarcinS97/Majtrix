using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class Pliki : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.EnableUpload();
                //Tools.RePostCancel();
            }
#if KDR || PORTAL
            string g = Tools.GetStr(Request["p1"]);



#else
            string g = Tools.GetStr(Session["lmenugrp"]);
#endif
            if (!String.IsNullOrEmpty(g))
                cntPliki1.Grupa = g;
        }
    }
}

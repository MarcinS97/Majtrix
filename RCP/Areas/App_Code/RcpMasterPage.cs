using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AjaxControlToolkit;

namespace HRRcp.App_Code
{
    public class RcpMasterPage : System.Web.UI.MasterPage
    {
        public SqlConnection con = null;
        public SqlConnection conP = null; // PORTAL
        public AppUser user;

        public RcpMasterPage()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            user = AppUser.CreateOrGetSession();
            base.OnInit(e);
        }
        //---------------------------
        public virtual void Redirect(string page)
        {
        }

        public virtual void SetWideJs(bool wide)
        {
        }

        public virtual ToolkitScriptManager GetToolkitScriptManager()
        {
            ToolkitScriptManager m = FindControl("ToolkitScriptManager1") as ToolkitScriptManager;
            return m;
        }

        //---------------------------
        protected override void OnLoad(EventArgs e)     // wykonuje się po onLoad form
        {
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (con != null)
                db.DoDisconnect(ref con);
            if (conP != null)
                db.DoDisconnect(ref conP);
        }
    }
}
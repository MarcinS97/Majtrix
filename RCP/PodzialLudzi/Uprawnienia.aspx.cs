using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class UprawnieniaPL : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasRight(AppUser.rPodzialLudziAdm))
                {
                    Tools.SetNoCache();
                    
                    /* wersja bez ajaxa z odwolaniem do strony 
                    string p1 = Request.QueryString["p1"];
                    if (!String.IsNullOrEmpty(p1))
                        Update(p1, Request.QueryString["p2"], Request.QueryString["p3"]);
                     */
                }
                else
                    App.ShowNoAccess("Podział Ludzi - Uprawnienia", App.User);
            }
        }
        //----------------------------
        protected void Page_Unload(object sender, EventArgs e)
        {
        }

        //----------------------------
        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = lbTitle.Text;
            App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }
    }
}

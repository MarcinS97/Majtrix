using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PreciToolB2BInit;
using PreciToolB2BInit.Helpers;

namespace PreciToolB2BInit.WAdmin
{
    public partial class RaportyF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Raport.F = true;   // na czas testów
                if (!User.IsInRole(Utils.rRaportyWidok))
                {
                    Response.Redirect("/");
                }

                if (User.IsInRole(Utils.rRaportyAdm))
                {
                    paEdit.Visible = true;
                    cntSqlReportEdit.Visible = true;
                    upScheduler.Visible = true;
                    cntReportScheduler.PrepareAdm();
                }
            }
        }

        protected void cntSqlReports_SelectedChanged(string id, string cmd, string par1)
        {
            if (!String.IsNullOrEmpty(id))
            {
                string url;
                if (Tools.IsUrl(cmd, out url))
                    App.Redirect(url);
                else
                    RaportF.Show(id);
            }
        }
        //---------------------
        protected void btNew_Click(object sender, EventArgs e)
        {
            cntSqlReportEdit.Show(null, cntSqlReports.Grupa);
        }

        protected void cntSqlReportEdit_Save(object sender, EventArgs e)
        {
            cntSqlReports.DataBind();
        }
    }
}
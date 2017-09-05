using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class ImportPDF : System.Web.UI.Page
    {
        const string FormName = "Import - PDF";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(FormName);
        }

        public void Prepare(string cmd)
        {
    
        }

        protected void btnEdytuj_Click(object sender, EventArgs e)
        {
            cntModal.Show(false);
        }
    }
}
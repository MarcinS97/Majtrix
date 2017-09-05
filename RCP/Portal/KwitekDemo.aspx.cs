using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal
{
    public partial class KwitekDemo : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            //int ccc = Controls.Count;

            //string filename = null;
            //foreach (Control cnt in ContentPlaceHolderReport.Controls)
            //    if (cnt is cntReport)
            //    {
            //        filename = ((cntReport)cnt).Title;
            //        break;
            //    }

            //if (String.IsNullOrEmpty(filename))
            //{
            //    filename = hidReportTitle.Value;
            //    if (String.IsNullOrEmpty(filename))
            //        filename = "report.csv";
            //}

            //App_Code.Report.ExportExcel(hidReport.Value, filename, null);
        }
    }
}

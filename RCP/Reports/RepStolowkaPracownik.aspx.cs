using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Reports
{
    public partial class RepStolowkaPracownik : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            App.CheckPrawaRapStolowka();
            if (!Report.CheckParametersCount(5, 5) ||
                !Report.CheckParameterData(1) ||
                !Report.CheckParameterData(2) ||
                !Report.CheckParameterToken(3, "F|S|FS|2|*") ||
                !Report.CheckParameterString(4, 0, 5) ||
                !Report.CheckParameterInt(5, 0, 9999999))
                App.ShowBadRepParameters("Raport - Stołówka");

            if (!IsPostBack)
            {
            }
        }
    }
}

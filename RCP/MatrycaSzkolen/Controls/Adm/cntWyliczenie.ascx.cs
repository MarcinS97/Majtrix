using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntWyliczenie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void Calculate(String Od, String Do)
        {
            db.Execute(dsCalculate, db.strParam(Od), db.strParam(Do));
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            Tools.ShowConfirm("Czy na pewno chcesz dokonać przeliczenia ocen?", btnCalculateConfirm);
        }

        protected void btnCalculateConfirm_Click(object sender, EventArgs e)
        {
            Calculate(deLeft.Date.ToString(), Convert.ToString(deRight.Date));
        }
    }
}
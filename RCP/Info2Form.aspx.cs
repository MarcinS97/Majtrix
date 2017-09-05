using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class Info2Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Typ;
                int back;
                if (!Info.GetInfoParamsLine(out Typ, out back))
                    if (!Info.GetInfoParams(out Typ, out back))
                    {
                        Typ = Tools.GetParam(1);
                        string b = Tools.GetParam(2);
                        back = Base.StrToIntDef(b, Info.ibNone);
                    }
                if (!String.IsNullOrEmpty(Typ))
                {
                    ltInfo.Text = Info.GetInfoText(Typ);
                    if (!String.IsNullOrEmpty(ltInfo.Text))
                    {
                        lbNoData.Visible = false;
                        ltInfo.Visible = true;
                    }
                }
                //divButtons.Visible = Info.PrepareBackButton(btBack, back);  lepiej wygląda jak jest odstęp
                btBack.Visible = Info.PrepareBackButton(btBack, back);
            }
        }

        private void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Informacje2 Form");
        }
    }
}

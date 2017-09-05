using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

/*
 
 wywołanie: 
 * Tools.Redirect("InfoForm.aspx", typ, kategoria, show_back, null, null)
 *              show_back=="1" pokaz else ukryj button Powrót
 * wywolanie bez parametrów lub z błędnymi - Brak danych 
 */

namespace HRRcp
{
    public partial class InfoForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Typ;
                int back;
                if (!Info.GetInfoParamsLine(out Typ, out back))  // ?par=xxx&par=xxx...
                    if (!Info.GetInfoParams(out Typ, out back))  // Session[par]...
                    {                                                           // Server.Transfer(form,...
                        Typ = Tools.GetParam(1);
                        string b = Tools.GetParam(2);
                        back = Base.StrToIntDef(b, Info.ibNone);
                    }
                if (!String.IsNullOrEmpty(Typ))
                {
                    ltInfo.Text = Info.GetInfoText(Typ);
                    ViewState["info_text"] = ltInfo.Text;

                    if (!String.IsNullOrEmpty(ltInfo.Text))
                    {
                        lbNoData.Visible = false;
                        ltInfo.Visible = true;
                    }
                }
                else
                {
                    ltInfo.Text = String.Format("<b>{0}</b><br /><br /><br />{1}", Info.Info1, Info.InfoEx);
                    back = Info.BtBack;
                    lbNoData.Visible = false;
                    ltInfo.Visible = true;
                    App.Master.SetWideJs(false);
                }
                //divButtons.Visible = Info.PrepareBackButton(btBack, back);  lepiej wygląda jak jest odstęp
                btBack.Visible = Info.PrepareBackButton(btBack, back);
                Info.SetHelp(Info.HELP_INFO);  // domyślny 
            }
        }

        private void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Informacje Form");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.RCP.Controls;
using HRRcp.App_Code;

/*
Params:
 * pracId
 * stanId
*/

namespace HRRcp.Controls.Raporty
{
    public partial class cntStanM : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Prepare();   // wykonuje się po dodaniu kontrolki do listy po LoadControl, uwaga - teraz nie ma zabezpieczenia przy postback z kontrolki 
        }

        private void Prepare()
        {
            string cmd, cnt, pracId, stanId, skip;

            /*
            Tools.GetLineParams(Params.Value, out pracId, out stanId);
            hidPracId.Value = Tools.StrToInt(pracId, -1).ToString();  // zabezpiczenie sql injection
            if (ddlStanM.Items.Count == 0) ddlStanM.DataBind();
            Tools.SelectItem(ddlStanM, stanId);
            */

            cntModal modal = cntModal;
            if (modal != null)
            {
                Tools.GetLineParams(Params, out cmd, out cnt, out pracId, out stanId, out skip);
                hidPracId.Value = Tools.StrToInt(pracId, -1).ToString();  // zabezpiczenie sql injection
                if (ddlStanM.Items.Count == 0) ddlStanM.DataBind();
                Tools.SelectItem(ddlStanM, stanId);

                modal.Title = AppUser.GetNazwiskoImieNREW(pracId);
                Button bt = Tools.FindControl(modal, "btModalOk") as Button;
                if (bt != null)
                {
                    bt.Visible = true;
                    bt.Click += new EventHandler(btOk_Click);
                }
            }
        }

        protected void btOk_Click(object sender, EventArgs e)
        {
            SqlDataSource1.Update();
            cntModal.Close();
        }

        cntModal cntModal
        {
            get { return Tools.FindParentControl<cntModal>(this); }
        }

        string Params
        {
            get { return Tools.GetText(cntModal, "Params"); }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string id = ddlStanM.SelectedValue;
            SqlDataSource1.Update();
            cntModal.Close();
        }

    }
}
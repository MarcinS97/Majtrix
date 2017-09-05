using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls
{
    public partial class cntHarmonogramAcc : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvList);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void MenuFilter_SelectTab(object sender, EventArgs e)
        {
            cntPlanPracy.Visible = false;
            //SetEditMode(edit);
            cntPlanPracy.StatusFilter = MenuFilter.SelectedValue;
        }

        protected void MenuFilter_DataBound(object sender, EventArgs e)
        {

        }

        void SetEditMode(bool b)
        {
            bool edit = false;
            if (MenuFilter.SelectedValue == "1")
                edit = true;
            b &= edit;
            btnAcceptConfirm.Visible = btnAccept.Visible = b;
            btnRejectConfirm.Visible = btnReject.Visible = b;
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            App.Redirect(String.Format("RCP/HarmonogramAcc.aspx"));
        }

        protected void gvListCmd_Click(object sender, EventArgs e)
        {
            string p = gvListCmdPar.Value;
            string[] par = Tools.GetLineParams(p);

            string cmd = par[0];
            string kierId = par[1];
            string data = par[2];


            cntModal.Show(false);

            Okres okr = new Okres(Convert.ToDateTime(data));
            SetEditMode(true);


            cntPlanPracy.Prepare(kierId, okr.DateFrom.ToString(), okr.DateTo.ToString(), okr.Id.ToString(), okr.Status, okr.IsArch());
            cntPlanPracy.Visible = true;


        }

        protected void btnAcceptConfirm_Click(object sender, EventArgs e)
        {
            string[] ch = cntPlanPracy.GetCheckedHeaders();
            if (ch.Length > 0)
            {
                Tools.ShowConfirm("Czy na pewno chcesz zaakceptować wybranych pracowników?", btnAccept);
            }
            else
            {
                Tools.ShowMessage("Proszę zaznaczyć pracowników!");
            }
        }


        protected void btnAccept_Click(object sender, EventArgs e)
        {
            string[] ch = cntPlanPracy.GetCheckedHeaders();
            if(ch.Length > 0)
            {
                string p = String.Join(",", ch);

                db.Execute(dsAccept, db.nullParamStr(p));
                cntPlanPracy.DataBind();
                gvList.DataBind();
                cntModal.Close();
            }
            else
            {
                Tools.ShowMessage("Proszę zaznaczyć pracowników!");
            }
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            string[] ch = cntPlanPracy.GetCheckedHeaders();
            if (ch.Length > 0)
            {
                Tools.ShowConfirm("Czy na pewno chcesz odrzucić wybranych pracowników?", btnReject);
            }
            else
            {
                Tools.ShowMessage("Proszę zaznaczyć pracowników!");
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string[] ch = cntPlanPracy.GetCheckedHeaders();
            if (ch.Length > 0)
            {
                string p = String.Join(",", ch);
                db.Execute(dsReject, db.nullParamStr(p));
                cntPlanPracy.DataBind();
                gvList.DataBind();
                cntModal.Close();
            }
            else
            {
                Tools.ShowMessage("Proszę zaznaczyć pracowników!");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Reports
{
    public partial class RapCC : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapCC))
                {
                    Tools.SetNoCache();
                }
                else
                {
                    App.ShowNoAccess("Raport czynności", App.User);
                }

                //if (!Raport.HasRight(1))
                //    App.ShowNoAccess("Raport czynności", App.User);


                deDateLeft.Date = Tools.bom(DateTime.Now);
                deDateRight.Date = Tools.eom(DateTime.Now);


                //fltrDate.StartValue = "2015-01-01;2015-06-01";

                //ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser) ? "0" : App.User.Id;
                //ddlSuperiors.DataBind();
                ddlCC.DataBind();
                Rep1.SQL1 = ddlCC.SelectedValue;
                Rep1.SQL2 = deDateLeft.DateStr;
                Rep1.SQL3 = deDateRight.DateStr;
                //if (ddlSuperiors.Items.Count < 1)
                //{
                //    Rep1.SQL1 = "0";
                //    Rep1.Visible = false;
                //}

                PrepareTitle();
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void PrepareTitle()
        {
            Rep1.Title = String.Format("Czynności - MPK{0}", (ddlCC.SelectedValue == "0") ? String.Empty : String.Format(": {0}", ddlCC.SelectedItem.Text));
            Rep1.Title2 = String.Format("Okres: {0} - {1}", deDateLeft.DateStr, deDateRight.DateStr);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public void btnFilter_Click(object sender, EventArgs e)
        {
            Rep1.SQL1 = ddlCC.SelectedValue;
            Rep1.SQL2 = deDateLeft.DateStr;
            Rep1.SQL3 = deDateRight.DateStr;
            PrepareTitle();
            Rep1.GenerateReportHeaders();
            Rep1.ReloadTable();
            Rep1.DataBind();
        }
    }
}

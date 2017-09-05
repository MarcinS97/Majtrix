using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Reports
{
    public partial class RapProdukcjaRok : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapProdukcjaRok))
                {
                    Tools.SetNoCache();
                }
                else
                {
                    App.ShowNoAccess("Raport produkcja roczna", App.User);
                }


                //if (!Raport.HasRight(7))
                //    App.ShowNoAccess("Raport produkcja roczna", App.User);

                //fltrDate.StartValue = "2015-01-01;2015-06-01";

                ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser || App.User.IsScZarz || App.User.IsScControlling) ? "0" : App.User.Id;
                ddlSuperiors.DataBind();
                ddlYears.DataBind();
                ddlCC.DataBind();
                
                Rep1.SQL1 = ddlSuperiors.SelectedValue;
                Rep1.SQL2 = ddlYears.SelectedValue;
                Rep1.SQL3 = ddlCC.SelectedValue;

                PrepareTitle();

            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void PrepareTitle()
        {
            Rep1.Title = String.Format("Raport Produktywność / FPY na rok{0}", String.IsNullOrEmpty(ddlSuperiors.SelectedValue) ? String.Empty : String.Format(" - {0}", ddlSuperiors.SelectedItem.Text));
            Rep1.Title2 = String.Format("Rok: {0}", ddlYears.SelectedItem.Text);
            Rep1.Title3 = (ddlCC.SelectedValue == "0") ? String.Empty : String.Format("MPK M: {0}", ddlCC.SelectedItem.Text);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public void btnFilter_Click(object sender, EventArgs e)
        {
            Rep1.SQL1 = ddlSuperiors.SelectedValue;
            Rep1.SQL2 = ddlYears.SelectedValue;
            Rep1.SQL3 = ddlCC.SelectedValue;
            PrepareTitle();
            Rep1.GenerateReportHeaders();
            Rep1.ReloadTable();
            Rep1.DataBind();
            //Rep1.ReloadTable();
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }
    }
}

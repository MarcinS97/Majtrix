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
    public partial class RapAbs : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapAbs))
                {
                    Tools.SetNoCache();
                }
                else
                {
                    App.ShowNoAccess("Raport wskaźnik absencji", App.User);
                }

                deDateLeft.Date = Tools.bom(DateTime.Now);
                deDateRight.Date = Tools.eom(DateTime.Now);


                //fltrDate.StartValue = "2015-01-01;2015-06-01";

                ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser || App.User._IsScHR || App.User.IsScControlling) ? "0" : App.User.Id;
                ddlSuperiors.DataBind();
                ddlCC.DataBind();
                Rep1.SQL1 = ddlSuperiors.SelectedValue;
                Rep1.SQL2 = deDateLeft.DateStr;
                Rep1.SQL3 = deDateRight.DateStr;
                Rep1.SQL4 = ddlCC.SelectedValue;
                if (ddlSuperiors.Items.Count < 1)
                {
                    Rep1.SQL1 = "0";
                    Rep1.Visible = false;
                }
                PrepareTitle();

            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void PrepareTitle()
        {
            Rep1.Title = String.Format("Raport wskaźnik absencji{0}", String.IsNullOrEmpty(ddlSuperiors.SelectedValue) ? String.Empty : String.Format(" - {0}", ddlSuperiors.SelectedItem.Text));
            Rep1.Title2 = String.Format("Okres: {0} - {1}", deDateLeft.DateStr, deDateRight.DateStr);
            Rep1.Title3 = (ddlCC.SelectedValue == "0") ? String.Empty : String.Format("MPK: {0}", ddlCC.SelectedItem.Text);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public void btnFilter_Click(object sender, EventArgs e)
        {
            Rep1.SQL1 = ddlSuperiors.SelectedValue;
            Rep1.SQL2 = deDateLeft.DateStr;
            Rep1.SQL3 = deDateRight.DateStr;
            Rep1.SQL4 = ddlCC.SelectedValue;
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

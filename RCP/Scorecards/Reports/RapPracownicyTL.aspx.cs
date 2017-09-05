using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Reports
{
    public partial class RapPracownicyTL : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                deDateLeft.Date = Tools.bom(DateTime.Now);
                deDateRight.Date = Tools.eom(DateTime.Now);


                //fltrDate.StartValue = "2015-01-01;2015-06-01";
                    
                ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser) ? "0" : App.User.Id;
                ddlSuperiors.DataBind();
                Rep1.SQL1 = ddlSuperiors.SelectedValue;
                Rep1.SQL2 = deDateLeft.DateStr;
                Rep1.SQL3 = deDateRight.DateStr;
                if (ddlSuperiors.Items.Count < 1)
                {
                    Rep1.SQL1 = "0";
                    Rep1.Visible = false;
                }
                
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            
            base.OnLoad(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

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
    public partial class RapProduktywnosc3 : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                if (App.User.HasScAccessAdm && Raport.HasRight(Dictionaries.Reports.RapProduktywnosc))
                {
                    //Tools.SetNoCache();
                }
                else
                {
                    App.ShowNoAccess("Raport produktywność", App.User);
                }


                //if (!Raport.HasRight(15))
                //    App.ShowNoAccess("Raport produktywność", App.User);

                deDateLeft.Date = Tools.bom(DateTime.Now);
                deDateRight.Date = Tools.eom(DateTime.Now);


                //fltrDate.StartValue = "2015-01-01;2015-06-01";

                ObserverId = (App.User.IsScAdmin || App.User.IsSuperuser) ? "0" : App.User.Id;
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
            Rep1.Title = String.Format("Raport produktywność / FPY{0}", String.IsNullOrEmpty(ddlSuperiors.SelectedValue) ? String.Empty : String.Format(" - {0}", ddlSuperiors.SelectedItem.Text));
            Rep1.Title2 = String.Format("Okres: {0} - {1}", deDateLeft.DateStr, deDateRight.DateStr);
            Rep1.Title3 = (ddlCC.SelectedValue == "0") ? String.Empty : String.Format("MPK M: {0}", ddlCC.SelectedItem.Text);
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
            Rep1.Prepare();
        }

        protected void PrintAll(object sender, EventArgs e)
        {
            DataTable AllEmployees = db.Select.Table(dsAllEmployees, ddlSuperiors.SelectedValue, deDateLeft.DateStr, deDateRight.DateStr, ddlCC.SelectedValue);
            String Output = String.Empty;
            foreach (DataRow Row in AllEmployees.Rows)
            {
                RepPDF.SQL1 = Row["kid"].ToString();
                RepPDF.SQL2 = deDateLeft.DateStr;
                RepPDF.SQL3 = deDateRight.DateStr;
                RepPDF.SQL4 = Row["pid:-"].ToString();

                RepPDF.Prepare();
                RepPDF.Visible = true;
                RepPDF.GridDataBind();
                //upMain.Update();
                
                String Data = HRRcp.Controls.EliteReports.DynamicControl.RenderControl(RepPDF);
                Output += Data;//.Replace("&lt;", "<").Replace("&gt;", ">");
            }
            RepPDF.Visible = false;
            PDF PDF = new PDF();
            if (PDF.Download(PDF.Prepare(Output, Request), Server, Response, Request, "Produktywnosc pracownikow") != 0)
                Tools.ShowMessage("Błąd pobierania");

        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntTasks1 : System.Web.UI.UserControl
    {
        public event EventHandler EmptyTasks;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String EmployeeId, String ScorecardTypeId, String Date, String OutsideJob)
        {
            this.EmployeeId = EmployeeId;
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.OutsideJob = OutsideJob;
            PrepareRepeaters();
        }

        protected void dsTasks_Selected(object sender, EventArgs e)
        {

        }

        protected void PrepareRepeaters()
        {
            DataView dv = (DataView)dsTasks.Select(DataSourceSelectArguments.Empty);

            rpNames.DataSource = dv;
            rpCC.DataSource = dv;
            rpTime.DataSource = dv;
            rpSum.DataSource = dv;

            rpNames.DataBind();
            rpCC.DataBind();
            rpTime.DataBind();
            rpSum.DataBind();

            if (rpNames.Items.Count == 0)
            {
                if (EmptyTasks != null) EmptyTasks(null, EventArgs.Empty);
            }
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }
        public String EmployeeId
        {
            get { return hidEmployeeId.Value; }
            set { hidEmployeeId.Value = value; }
        }
        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }
        public String OutsideJob
        {
            get { return hidOutsideJob.Value; } 
            set { hidOutsideJob.Value = value; }
        }
    }
}
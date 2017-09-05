using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntEmployeesZoom : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Grid.Prepare(gvEmployees, null, false, 25, false);
        }

        public void Prepare(String ScorecardTypeId, String Date, String ObserverId, String TL, String Col)
        {
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.ObserverId = ObserverId;
            this.TeamLeader = TL;
            this.Col = Col;

            //gvEmployees.DataSource = dsEmployees;
            //gvEmployees.DataBind();
        
        }

        public String GetDisplayColumns()
        {
            return String.Empty;
        }

        public String GetWhereColumns()
        {
            return String.Empty;
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }

        public String TeamLeader
        {
            get { return hidTeamLeader.Value; }
            set { hidTeamLeader.Value = value; }
        }

        public String Col
        {
            get { return hidCol.Value; }
            set { hidCol.Value = value; }
        }

    //    public Button CloseButton
    //    {
    //        get { return btClose; }
    //    }
    }
}
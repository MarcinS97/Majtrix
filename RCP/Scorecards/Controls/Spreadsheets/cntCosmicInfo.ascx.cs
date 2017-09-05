using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntCosmicInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String OutsideJob, String Genre, String TeamLeader)
        {
            this.OutsideJob = OutsideJob;
            this.Genre = Genre;
            this.TeamLeader = TeamLeader;
            plusik.Visible = (OutsideJob == "0");
            switch (Genre)
            {
                case Dictionaries.Spreadsheets.Individual:
                    tdPlannedTeamSize.Visible =
                        //tdTeamSizeCorrection.Visible =
                        //tdTeamSizeCorrectionSum.Visible = 
                    tdPlannedTeamSizeSum.Visible = false;
                    tdKodNieob.Visible =
                    tdKodNieobSum.Visible = true;
                    break;
                case Dictionaries.Spreadsheets.Team:
                    tdPlannedTeamSize.Visible =
                        //tdTeamSizeCorrection.Visible =
                        //tdTeamSizeCorrectionSum.Visible = 
                    tdPlannedTeamSizeSum.Visible = true;
                    tdKodNieob.Visible = 
                    tdKodNieobSum.Visible = false;
                    break;
            }
            tdTLPrac.Visible = IsTL();
            tdTLPrac2.Visible = IsTL();
        }

        public Boolean IsOutsideJob()
        {
            return OutsideJob == "1";
        }

        public Boolean IsTL()
        {
            return TeamLeader == "1";
        }


        public String Genre
        {
            get { return hidGenre.Value; }
            set { hidGenre.Value = value; }
        }

        public String OutsideJob
        {
            get { return hidOutsideJob.Value; }
            set { hidOutsideJob.Value = value; }
        }

        public String TeamLeader 
        {
            get { return hidTeamLeader.Value; }
            set { hidTeamLeader.Value = value; }
        }
    }
}
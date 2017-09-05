using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSpreadsheetsParameters : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void Prepare()
        {
            if (!String.IsNullOrEmpty(ScorecardTypeId))
            {
                PrepareControls(ScorecardTypeId, tabType.SelectedValue);
                SetControlsVisible(true);
                ddlScorecardsImport.DataBind();
            }
            else
            {
                SetControlsVisible(false);
            }
        }

        void SetControlsVisible(Boolean B)
        {
            Productivity.Visible =
            QC.Visible =
            Absence.Visible =
            Other.Visible =
            tabType.Visible = 
            divImport.Visible = B;
        }

        void PrepareControls(String ScorecardTypeId, String TL)
        {
            Productivity.Prepare(ScorecardTypeId, TL);
            QC.Prepare(ScorecardTypeId, TL);
            Absence.Prepare(ScorecardTypeId, TL);
            Other.Prepare(ScorecardTypeId, TL);
        }

        protected void tabType_MenuItemClick(object sender, MenuEventArgs e)
        {
            PrepareControls(ScorecardTypeId, tabType.SelectedValue);
        }

        protected void ddlScorecards_SelectedIndexChanged(object sender, EventArgs e)
        {
            Prepare();
        }

        protected void Import(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ddlScorecardsImport.SelectedValue))
            {
                db.Execute(dsImport, ScorecardTypeId, ddlScorecardsImport.SelectedValue);
                Prepare();
                Tools.ShowMessage("Pomyślnie zaimportowano dane.");
            }
        }

        public String ScorecardTypeId
        {
            get { return ddlScorecards.SelectedValue; }
        }
    }
}
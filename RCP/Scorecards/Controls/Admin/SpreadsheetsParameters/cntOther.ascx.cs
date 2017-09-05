using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Admin.SpreadsheetsParameters
{
    public partial class cntOther : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String ScorecardTypeId, String TL)
        {
            this.ScorecardTypeId = ScorecardTypeId;
            OtherListPGIO.Prepare(ScorecardTypeId, TL);
            OtherListPREM.Prepare(ScorecardTypeId, TL);
            OtherListSANDWHICH.Prepare(ScorecardTypeId, TL);
            OtherListPROD.Prepare(ScorecardTypeId, TL);
            OtherListQC.Prepare(ScorecardTypeId, TL);
            OtherListPREMZAD.Prepare(ScorecardTypeId, TL);

            if (TL == "1")
            {
                OtherListWPI.Prepare(ScorecardTypeId, TL);
                OtherListWPZ.Prepare(ScorecardTypeId, TL);
                OtherListQUATRO.Prepare(ScorecardTypeId, TL);
                OtherListTLPRAC.Prepare(ScorecardTypeId, TL);
                OtherListWPI.Visible = true;
                OtherListWPZ.Visible = true;
                OtherListQUATRO.Visible = true;
                OtherListPREMZAD.Visible = true;
                OtherListTLPRAC.Visible = true;
            }
            else
            {
                OtherListWPI.Visible = false;
                OtherListWPZ.Visible = false;
                OtherListQUATRO.Visible = false;
                OtherListPREMZAD.Visible = false;
                OtherListTLPRAC.Visible = false;
            }
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }
    }
}
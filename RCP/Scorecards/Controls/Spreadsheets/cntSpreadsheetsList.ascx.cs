using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntSpreadsheetsList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSpreadsheets, 0);
            Tools.PrepareSorting2(lvSpreadsheets, 1, 10);
        }

        public int Prepare(String ObserverId)
        {
            this.ObserverId = ObserverId;
            lvSpreadsheets.DataBind();
            return lvSpreadsheets.Items.Count;
        }

        protected void ShowSpreadsheet(object sender, EventArgs e)
        {
            Button lnk = (sender as Button);

            if (lnk != null)
            {
                string arg = lnk.CommandArgument;
                if (!String.IsNullOrEmpty(arg))
                {
                    string[] args = arg.Split(';');
                    if (args.Length == 4)
                    {
                        string observerId = args[0];
                        string employeeId = args[1];
                        string scorecardTypeId = args[2];
                        string date = args[3];

                        Session["scObserverId"] = observerId;
                        Session["scEmployeeId"] = employeeId;
                        Session["scScorecardTypeId"] = scorecardTypeId;
                        Session["scDate"] = date;

                        Response.Redirect("~/Scorecards/Scorecards.aspx");
                            

                    }
                }
            }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }
    }
}
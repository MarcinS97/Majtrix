using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSpreadsheetsTasks : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAdd.Enabled = false;
            }
        }

        protected void SheetSelected(object sender, EventArgs e)
        {
            string spreadsheetId = sender.ToString();
            SpreadsheetId = SpreadsheetsTasksList.SpreadsheetId = spreadsheetId;
            btnAdd.Enabled = true;
            SpreadsheetsTasksList.SetEdit();
        }

        protected void AddTask(object sender, EventArgs e)
        {
            SpreadsheetsTasksList.AddTasks(SpreadsheetId, TaskList.SelectedTasks);
            TaskList.ClearSelect();
        }

        protected void RemoveTask(object sender, EventArgs e)
        {
            SpreadsheetsTasksList.RemoveTask();
            TaskList.ClearSelect();
        }

        public string SpreadsheetId
        {
            get { return ViewState["vSpreadsheetId"] as String; }
            set { ViewState["vSpreadsheetId"] = value; }
        }
    }
}
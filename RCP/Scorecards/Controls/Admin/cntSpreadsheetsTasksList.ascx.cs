using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;
using System.Data;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSpreadsheetsTasksList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSpreadsheets, 0);
            Tools.PrepareSorting2(lvSpreadsheets, 1, 10);
        }

        public void SetEdit()
        {
            lblMustSelect.Visible = false;
        }

        public void AddTasks(String spreadsheetId, List<String> tasks)
        {
            foreach (String task in tasks)
            {
                db.execSQL(String.Format(@"insert into scTypyArkuszyCzynnosci (IdTypuArkuszy, IdCzynnosci, Od) values ({0}, {1}, '{2}')",
                                spreadsheetId, task, DateTime.Now.ToString()));
            }
            lvSpreadsheets.DataBind();
        }

        public void RemoveTask()
        {

            String Id = null;
            if (lvSpreadsheets.SelectedDataKey != null) Id = lvSpreadsheets.SelectedDataKey.Value.ToString();
            if (!String.IsNullOrEmpty(Id))
            {
                dsGetAssigned.SelectParameters["tacId"].DefaultValue = Id;
                DataView Assigned = (DataView)dsGetAssigned.Select(DataSourceSelectArguments.Empty);

                if (Assigned.Table.Rows.Count > 0)
                {
                    String Message = "Uwaga! Próbujesz usunąć rekord, do którego przypisane są już oceny:\\n";
                    foreach (DataRow Row in Assigned.Table.Rows)
                    {
                        Message += String.Format("{0} - {1}\\n", Row["Pracownik"], Row["Ilosc"]);
                    }
                    Tools.ShowMessage(Message);
                }
                else
                {
                    dsRemove.SelectParameters["tacId"].DefaultValue = Id;
                    dsRemove.Select(DataSourceSelectArguments.Empty);
                    lvSpreadsheets.DataBind();
                }
            }
        }


        protected void CheckItem(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            bool isChecked = cb.Checked;
            HtmlTableRow row = cb.Parent.Parent as HtmlTableRow;
            HiddenField hidId = cb.Parent.FindControl("hidId") as HiddenField;
            string id = hidId.Value;

            if (isChecked)
            {
                row.Attributes["class"] = "sit";
                if (!SelectedTasks.Contains(id)) SelectedTasks.Add(id);
            }
            else
            {
                row.Attributes["class"] = "it";
                SelectedTasks.Remove(id);
            }
        }

        public List<string> SelectedTasks
        {
            get
            {
                if (ViewState["vSelectedTasks"] != null) return ViewState["vSelectedTasks"] as List<string>;
                else
                {
                    ViewState["vSelectedTasks"] = new List<string>();
                    return new List<string>();
                }
            }
            set { ViewState["vSelectedTasks"] = value; }
        }

        public String SpreadsheetId
        {
            get { return hidSpreadsheetId.Value; }
            set { hidSpreadsheetId.Value = value; }
        }
    }
}
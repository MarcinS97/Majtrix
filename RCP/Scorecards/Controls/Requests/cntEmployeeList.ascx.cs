using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntEmployeeList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String ObserverId)
        {
            this.ObserverId = ObserverId;
            lvEmployees.DataBind();
        }

        protected void Create(object sender, EventArgs e)
        {
            Boolean Ok = db.Execute(dsCreateRequest, ObserverId, db.strParam(DateTime.Now.ToString()), db.strParam(DateTime.Now.ToString()));
            
            if (Ok)
            {
                Tools.ShowMessage("Wniosek został utworzony");
            }
            else
            {
            }
        }

        public void ClearSelect()
        {
            CheckAll(false);
        }

        protected void CheckAll(object sender, EventArgs e)
        {
            CheckBox cbCheckAll = sender as CheckBox;
            CheckAll(cbCheckAll.Checked);
        }

        protected void CheckAll(Boolean C)
        {
            foreach (ListViewDataItem Item in lvEmployees.Items)
            {
                String Id = Tools.GetText(Item, "hidId");
                Tools.SetChecked(Item, "cbSelect", C);
                HtmlTableRow Row = Item.FindControl("trSelect") as HtmlTableRow;
                CheckItem(Row, Id, C);
            }
        }

        protected void CheckItem(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            HtmlTableRow Row = cb.Parent.Parent as HtmlTableRow;
            String Id = Tools.GetText(cb.Parent, "hidId");
            CheckItem(Row, Id, cb.Checked);
        }

        protected void CheckItem(HtmlTableRow Row, String Id, Boolean C)
        {
            if (C)
            {
                Row.Attributes["class"] = "sit";
                if (!SelectedEmployees.Contains(Id)) SelectedEmployees.Add(Id);
            }
            else
            {
                Row.Attributes["class"] = "it";
                SelectedEmployees.Remove(Id);
            }
        }

        public List<String> SelectedEmployees
        {
            get
            {
                if (ViewState["vSelectedEmployees"] != null) return ViewState["vSelectedEmployees"] as List<string>;
                else
                {
                    ViewState["vSelectedEmployees"] = new List<String>();
                    return new List<String>();
                }
            }
            set { ViewState["vSelectedEmployees"] = value; }
        }

        public Button CloseButton
        {
            get { return btnClose; }
        }

        public Button CreateButton
        {
            get { return btnCreate; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie
{
    public partial class cntLinie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
        
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvLinie, 1337);
            Tools.PrepareSorting2(lvLinie, 1, 10);
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
            foreach (ListViewDataItem Item in lvLinie.Items)
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
                if (!SelectedLinie.Contains(Id)) SelectedLinie.Add(Id);
            }
            else
            {
                Row.Attributes["class"] = "it";
                SelectedLinie.Remove(Id);
            }
        }

        public List<String> SelectedLinie
        {
            get
            {
                if (ViewState["vSelectedLinie"] != null) return ViewState["vSelectedLinie"] as List<string>;
                else
                {
                    ViewState["vSelectedLinie"] = new List<String>();
                    return new List<String>();
                }
            }
            set { ViewState["vSelectedLinie"] = value; }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbSearch.Text = "";
        }
    }
}
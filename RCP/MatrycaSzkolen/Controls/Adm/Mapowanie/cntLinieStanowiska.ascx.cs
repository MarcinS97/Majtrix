using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;
using System.Data;

namespace HRRcp.MatrycaSzkolen.Controls.Adm.Mapowanie
{
    public partial class cntLinieStanowiska : System.Web.UI.UserControl
    {

        public event EventHandler Deleted;
        public event EventHandler Selected;
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvLinieStanowiska, 1337);
            Tools.PrepareSorting2(lvLinieStanowiska, 1, 10);
        }

        public void SetEdit()
        {
            lblMustSelect.Visible = false;
        }

        public void AddTasks(String stanowiskoId, List<String> linie)
        {
            foreach (String linia in linie)
            {
                db.Execute(dsInsert, stanowiskoId, linia, db.strParam(DateTime.Now.ToString()));
            }
            lvLinieStanowiska.DataBind();
        }

        public void RemoveTask()
        {

            String Id = null;
            if (lvLinieStanowiska.SelectedDataKey != null) Id = lvLinieStanowiska.SelectedDataKey.Value.ToString();
            if (!String.IsNullOrEmpty(Id))
            {
                db.Execute(dsRemove, Id);
                lvLinieStanowiska.DataBind();
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
                if (!SelectedLinie.Contains(id)) SelectedLinie.Add(id);
            }
            else
            {
                row.Attributes["class"] = "it";
                SelectedLinie.Remove(id);
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


        public int GetCount()
        {
            lvLinieStanowiska.DataBind();
            return lvLinieStanowiska.Items.Count;
        }

        public void ClearSelect()
        {
            lvLinieStanowiska.SelectedIndex = -1;
        }

        public String StanowiskoId
        {
            get { return hidStanowiskoId.Value; }
            set { hidStanowiskoId.Value = value; }
        }

        protected void lvLinieStanowiska_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            if (Deleted != null)
                Deleted(null, EventArgs.Empty);
        }

        protected void lvLinieStanowiska_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Selected != null)
                Selected(null, EventArgs.Empty);
        }
    }
}
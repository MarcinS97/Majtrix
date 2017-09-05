using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;

namespace HRRcp.MatrycaSzkolen.Controls.Adm
{
    public partial class cntUprawnieniaSzkol : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvPracownicy, 1337);
            Tools.PrepareSorting2(lvPracownicy, 1, 10);


            Tools.PrepareDicListView(lvSzkolenia, 1337);
            Tools.PrepareSorting2(lvSzkolenia, 1, 10);

            
            Tools.PrepareDicListView(lvPracSzkolenia, 1337);
            Tools.PrepareSorting2(lvPracSzkolenia, 1, 10);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }

        }

        protected void lvPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (StanowiskoSelected != null) StanowiskoSelected(lvStanowiska.SelectedDataKey.Value, EventArgs.Empty);

            string pracId = lvPracownicy.SelectedDataKey.Value.ToString();
            PracId = pracId;
            btnAdd.Enabled = true;
            SetEdit();
        }

        /* buttons */


        protected void AddTask(object sender, EventArgs e)
        {
            AddTasks(PracId, SelectedSzkolenia);
            ClearSelect();
        }

        protected void RemoveTask(object sender, EventArgs e)
        {
            RemoveTask();
            ClearSelect();
        }

        /* right */

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
            foreach (ListViewDataItem Item in lvSzkolenia.Items)
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
                if (!SelectedSzkolenia.Contains(Id)) SelectedSzkolenia.Add(Id);
            }
            else
            {
                Row.Attributes["class"] = "it";
                SelectedSzkolenia.Remove(Id);
            }
        }

        public List<String> SelectedSzkolenia
        {
            get
            {
                if (ViewState["vSelectedSzkolenia"] != null) return ViewState["vSelectedSzkolenia"] as List<string>;
                else
                {
                    ViewState["vSelectedSzkolenia"] = new List<String>();
                    return new List<String>();
                }
            }
            set { ViewState["vSelectedSzkolenia"] = value; }
        }

        /* middle */

        public void SetEdit()
        {
            lblMustSelect.Visible = false;
        }

        public void AddTasks(String pracId, List<String> szkolenia)
        {
            foreach (String szkol in szkolenia)
            {
                //db.execSQL(String.Format(@"insert into scTypyArkuszyCzynnosci (IdTypuArkuszy, IdCzynnosci, Od) values ({0}, {1}, '{2}')",
                //                spreadsheetId, task, DateTime.Now.ToString()));

                db.Execute(dsInsert, szkol, PracId, db.strParam(DateTime.Now.ToString())); 
            }
            lvPracSzkolenia.DataBind();
        }

        public void RemoveTask()
        {

            String Id = null;
            if (lvPracSzkolenia.SelectedDataKey != null) Id = lvPracSzkolenia.SelectedDataKey.Value.ToString();
            if (!String.IsNullOrEmpty(Id))
            {
                db.Execute(dsRemove, Id);
                lvPracSzkolenia.DataBind();
                //dsGetAssigned.SelectParameters["tacId"].DefaultValue = Id;
                //DataView Assigned = (DataView)dsGetAssigned.Select(DataSourceSelectArguments.Empty);

                //if (Assigned.Table.Rows.Count > 0)
                //{
                //    String Message = "Uwaga! Próbujesz usunąć rekord, do którego przypisane są już oceny:\\n";
                //    foreach (DataRow Row in Assigned.Table.Rows)
                //    {
                //        Message += String.Format("{0} - {1}\\n", Row["Pracownik"], Row["Ilosc"]);
                //    }
                //    Tools.ShowMessage(Message);
                //}
                //else
                //{
                //    dsRemove.SelectParameters["tacId"].DefaultValue = Id;
                //    dsRemove.Select(DataSourceSelectArguments.Empty);
                //    lvSpreadsheets.DataBind();
                //}
            }
        }

        /*
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
                if (!SelectedPracSzkolenia.Contains(id)) SelectedPracSzkolenia.Add(id);
            }
            else
            {
                row.Attributes["class"] = "it";
                SelectedPracSzkolenia.Remove(id);
            }
        }
         * */

        public List<String> SelectedPracSzkolenia
        {
            get
            {
                if (ViewState["vSelectedPracSzkolenia"] != null) return ViewState["vSelectedPracSzkolenia"] as List<string>;
                else
                {
                    ViewState["vSelectedPracSzkolenia"] = new List<String>();
                    return new List<String>();
                }
            }
            set { ViewState["vSelectedPracSzkolenia"] = value; }
        }

        public String PracId
        {
            get { return hidPracId.Value; }
            set { hidPracId.Value = value; }
        }

        protected void btnClearPrac_Click(object sender, EventArgs e)
        {
            tbSearchPrac.Text = "";
        }

        protected void btnClearSzkol_Click(object sender, EventArgs e)
        {

        }
    }
}
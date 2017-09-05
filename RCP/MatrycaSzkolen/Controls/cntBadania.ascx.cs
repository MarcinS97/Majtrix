using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntBadania : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvBadania, 1337);
            Tools.PrepareSorting2(lvBadania, 1, 10);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lvBadania.InsertItemPosition = InsertItemPosition.LastItem;
        }

        List<string> Selected = new List<string>();

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            Repeater rpItems = Item.FindControl("rpItems") as Repeater;
            string badId = Tools.GetText(Item, "hidId");

            if (rpItems != null)
            {
                if (rpItems.Items.Count > 0)
                {
                    foreach (RepeaterItem item in rpItems.Items)
                    {
                        string id = Tools.GetText(item, "hidId");
                        string badTypId = Tools.GetText(item, "hidBadTypId");
                        CheckBox cb = Tools.FindCheckBox(item, "cbSelect");
                        if (cb != null)
                        {
                            bool check = cb.Checked;
                            if(check)
                                Selected.Add(badTypId);
                            if (!String.IsNullOrEmpty(badId))
                            {
                                if (check && String.IsNullOrEmpty(id)) // insert
                                {
                                    db.Execute(dsInsert, badId, badTypId);
                                }
                                else if (!check && !String.IsNullOrEmpty(id)) // delete
                                {
                                    db.Execute(dsDelete, id);
                                }
                                else
                                {
                                    // nothing
                                }
                            }
                        }


                    }
                }
            }


            //Values["Od"] = Double.Parse(Tools.GetText(Item, "tbOd").Replace(',', '.'), CultureInfo.InvariantCulture) / (Percent == "1" ? 100 : 1);
            //Values["Do"] = Double.Parse(Tools.GetText(Item, "tbDo").Replace(',', '.'), CultureInfo.InvariantCulture) / (Percent == "1" ? 100 : 1);
            //Values["IdLinii"] = Tools.GetDdlSelectedValue(Item, "ddlLinie");


            //if (Employee) Values["Parametr2"] = Tools.GetDdlSelectedValue(Item, "ddlEmployees");
            //else if (IsCustomDDL()) Values["Parametr2"] = Tools.GetDdlSelectedValue(Item, "ddlCustom");
            return true;
        }

        protected void lvBadania_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvBadania.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvBadania_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        protected void ddlPracownicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            divEmployee.Visible = !String.IsNullOrEmpty(ddlPracownicy.SelectedValue);
        }

        protected void lvBadania_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {

        }

        protected void dsBadania_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            int id = (int)e.Command.Parameters["@Ind"].Value;
            foreach (string item in Selected)
            {
                db.Execute(dsInsert, id, item);
               
            }
            lvBadania.InsertItemPosition = InsertItemPosition.None;
        }

        protected void btnCancelInsert_Click(object sender, EventArgs e)
        {
            lvBadania.InsertItemPosition = InsertItemPosition.None;
        }
    }
}
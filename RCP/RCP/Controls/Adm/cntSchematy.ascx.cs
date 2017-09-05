using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Adm
{
    public partial class cntSchematy : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSchematy, Tools.ListViewMode.Bootstrap);
        }

        protected void lvSchematy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //int mode = Tools.GetListItemMode(e, lvSchematy);
            //if(mode == Tools.limEdit)
            //{
            //    string zmiany = Tools.GetText(e.Item, "hidZmiany");
            //    Repeater rpZmiany = e.Item.FindControl("rpZmiany") as Repeater;

            //    if(rpZmiany != null)
            //    {
            //        //rpZmiany.DataSource = zmiany.Split(',');
            //        //rpZmiany.DataBind();
            //    }

            //}
        }

        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            HiddenField hidZmiany = null;
            if (lvSchematy.EditIndex >= 0)
                hidZmiany = lvSchematy.EditItem.FindControl("hidZmiany") as HiddenField;
            else if (lvSchematy.InsertItem != null)
                hidZmiany = lvSchematy.InsertItem.FindControl("hidZmiany") as HiddenField;

            if (hidZmiany != null)
            {
                string zmiana = (sender as LinkButton).CommandArgument;
                if (!String.IsNullOrEmpty(zmiana))
                {
                    string id = zmiana.Split(';')[0];
                    string sidx = zmiana.Split(';')[1];
                    int idx = Convert.ToInt32(sidx);

                    List<String> zmianyList = hidZmiany.Value.Split(',').ToList();

                    for (int i = 0; i < zmianyList.Count; i++)
                    {
                        if (zmianyList[i] == id && i == idx)
                        {
                            zmianyList.RemoveAt(i);
                            break;
                        }
                    }
                    hidZmiany.Value = String.Join(",", zmianyList.ToArray());

                    //string old = hidZmiany.Value;
                    //hidZmiany.Value = hidZmiany.Value.Replace(zmiana + ",", "");
                    //if(hidZmiany.Value == old)
                    //    hidZmiany.Value = hidZmiany.Value.Replace(zmiana, "");
                }
            }

        }

        protected void btnAddZmiana_Click(object sender, EventArgs e)
        {
            string val = null;
            if (lvSchematy.EditIndex >= 0)
                val = Tools.GetDdlSelectedValue(lvSchematy.EditItem, "ddlZmiany");
            else if (lvSchematy.InsertItem != null)
                val = Tools.GetDdlSelectedValue(lvSchematy.InsertItem, "ddlZmiany");
            if (!String.IsNullOrEmpty(val))
            {
                HiddenField hidZmiany = null;
                if (lvSchematy.EditIndex >= 0)
                    hidZmiany = lvSchematy.EditItem.FindControl("hidZmiany") as HiddenField;
                else if (lvSchematy.InsertItem != null)
                    hidZmiany = lvSchematy.InsertItem.FindControl("hidZmiany") as HiddenField;

                if (hidZmiany != null)
                {
                    string newv = "";
                    if (hidZmiany.Value.Length > 0)
                        newv = ",";
                    hidZmiany.Value += newv + val;
                }
            }

            DropDownList ddl = (sender as DropDownList);
            if (ddl != null)
            {
                ddl.SelectedValue = null;
            }
        }

        public ListView List
        {
            get { return lvSchematy; }
        }

        public Boolean IsEdit()
        {
            return lvSchematy.EditIndex >= 0 || (lvSchematy.InsertItemPosition == InsertItemPosition.LastItem);
        }

        protected void lvSchematy_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CancelInsert":
                    lvSchematy.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "Update":
                    lvSchematy.InsertItemPosition = InsertItemPosition.None;
                    break;
                case "Insert":
                    lvSchematy.InsertItemPosition = InsertItemPosition.None;
                    break;
            }
        }

        public void TriggerChange()
        {
            if (Changed != null)
                Changed(null, EventArgs.Empty);
        }

        protected void lvSchematy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            TriggerChange();
        }

        protected void lvSchematy_ItemDeleted(object sender, ListViewDeletedEventArgs e)
        {
            TriggerChange();
        }

        protected void lvSchematy_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            TriggerChange();
        }

    }
}
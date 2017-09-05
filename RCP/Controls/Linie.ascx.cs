using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class Linie : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvLinie_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            switch (Tools.GetListItemMode(e, lvLinie, out drv))
            {
                case Tools.limEdit:
                    Tools.SelectItem(e.Item, "ddlSplitP", drv["GrSplituP"]);
                    Tools.SelectItem(e.Item, "ddlSplitK", drv["GrSplituK"]);
                    break;
                case Tools.limSelect:
                    break;
                case Tools.limInsert:
                    break;
                default:
                    break;
            }
        }

        private bool UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            values["GrSplituP"] = Tools.GetDdlSelectedValueInt(item, "ddlSplitP");
            values["GrSplituK"] = Tools.GetDdlSelectedValueInt(item, "ddlSplitK");
            return true;
        }

        protected void lvLinie_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView lv = (ListView)sender;
            e.Cancel = !UpdateItem(lv.InsertItem, null, e.Values);
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
            lv.EditIndex = -1;
        }

        protected void lvLinie_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            e.Cancel = !UpdateItem(lv.EditItem, e.OldValues, e.NewValues);
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
        }

        protected void lvLinie_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView lv = (ListView)sender;
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
        }
    }
}
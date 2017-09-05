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
    public partial class SplitWsp : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string IdSplitu
        {
            get { return hidIdSplitu.Value; }
            set { hidIdSplitu.Value = value; }    
        }

        protected void lvSplityWsp_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            switch (Tools.GetListItemMode(e, lvSplityWsp, out drv))
            {
                case Tools.limEdit:
                    Tools.SelectItem(e.Item, "ddlCC", drv["IdCC"]);
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
            values["IdCC"] = Tools.GetDdlSelectedValueInt(item, "ddlCC");
            return true;
        }

        protected void lvSplityWsp_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListView lv = (ListView)sender;
            e.Cancel = !UpdateItem(lv.EditItem, e.OldValues, e.NewValues);
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
        }

        protected void lvSplityWsp_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListView lv = (ListView)sender;
            e.Cancel = !UpdateItem(lv.InsertItem, null, e.Values);
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
            lv.EditIndex = -1;
        }

        protected void lvSplityWsp_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            ListView lv = (ListView)sender;
            Log.LogChanges(Log.SLOWNIK, lv.ClientID, e);
        }
    }
}
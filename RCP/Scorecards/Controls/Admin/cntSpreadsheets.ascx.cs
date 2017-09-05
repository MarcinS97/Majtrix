using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.Collections.Specialized;

namespace HRRcp.Scorecards.Controls.Admin
{
    public partial class cntSpreadsheets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvSpreadsheets, 0);
            Tools.PrepareSorting2(lvSpreadsheets, 1, 10);
        }

        protected void lvSpreadsheets_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv = null;
            int li = Tools.GetListItemMode(e, lvSpreadsheets, out drv);
            if (li == Tools.limEdit && drv != null)
            {
                Tools.SelectItem(e.Item, "ddlGenre", drv["Rodzaj"]);
                Tools.SelectItem(e.Item, "ddlQC", drv["QC"]);
                //Tools.SelectItem(e.Item, "ddlProd", drv["Produktywnosc"]);
            }
        }

        protected void lvSpreadsheets_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int li = Tools.GetListItemMode(e, lvSpreadsheets);
            if (li == Tools.limInsert)
            {
                Tools.SetChecked(e.Item, "cbActive", true);
                Tools.SetChecked(e.Item, "cbRequest", true);
            }
        }

        protected void lvSpreadsheets_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvSpreadsheets.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvSpreadsheets_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            Values["Rodzaj"] = Tools.GetDdlSelectedValueInt(Item, "ddlGenre");
            Values["QC"] = Tools.GetDdlSelectedValueInt(Item, "ddlQC");
            //Values["Produktywnosc"] = Tools.GetDdlSelectedValueInt(Item, "ddlProd");
            return true;
        }
    }
}
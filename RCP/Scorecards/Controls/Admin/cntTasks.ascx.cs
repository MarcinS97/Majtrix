using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntTasks : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvTasks, 0);
            Tools.PrepareSorting2(lvTasks, 1, 10);
        }
        protected void lvTasks_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int li = Tools.GetListItemMode(e, lvTasks);
            if (li == Tools.limInsert)
            {
                Tools.SetChecked(e.Item, "cbActive", true);
            }
        }
        
        protected void lvTasks_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvTasks.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvTasks_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            Values["IdCC"] = Tools.GetDdlSelectedValueInt(Item, "ddlCC");
            Values["Czas"] = Tools.GetText(Item, "tbCzas").Replace(',', '.');
            return true;
        }

    }
}
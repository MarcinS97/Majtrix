using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal.Controls
{
    public partial class cntOgloszeniaKategorie : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvKategorie, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvKategorie_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                Tools.SetChecked(e.Item, "cbAktywna", true);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AdmKodyWyjasnicControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvKodyWyjasnic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Button b = (Button)e.Item.FindControl("DeleteButton");
                if (b != null) Tools.MakeConfirmDeleteRecordButton(b);
            }
        }
    }
}
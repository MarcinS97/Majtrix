using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class DzialyStrefy : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string dzialId)
        {
            DzialId = dzialId;
        }

        //-------------------------------
        public string DzialId
        {
            get { return hidDzialId.Value; }
            set
            {
                hidDzialId.Value = value;
                lvDzialyStrefy.DataBind();
            }
        }

        private bool InitEditItem(ListViewItemEventArgs e)
        {
            if (lvDzialyStrefy.EditItem != null && ((ListViewDataItem)e.Item).DisplayIndex == lvDzialyStrefy.EditIndex)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                Tools.SelectItem(e.Item, "ddlKierStrefa",   drv["KStrefaId"]);
                Tools.SelectItem(e.Item, "ddlKierAlgorytm", drv["KAlgorytm"]);
                Tools.SelectItem(e.Item, "ddlPracStrefa",   drv["PStrefaId"]);
                Tools.SelectItem(e.Item, "ddlPracAlgorytm", drv["PAlgorytm"]);
                return true;
            }
            else return false;
        }

        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            values["KStrefaId"] = Tools.GetDdlSelectedValueInt(item, "ddlKierStrefa");
            values["KAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlKierAlgorytm");
            values["PStrefaId"] = Tools.GetDdlSelectedValueInt(item, "ddlPracStrefa");
            values["PAlgorytm"] = Tools.GetDdlSelectedValueInt(item, "ddlPracAlgorytm");
        }

        //------------------------------
        protected void lvDzialyStrefy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (InitEditItem(e))
            {
            }
        }

        protected void lvDzialyStrefy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvDzialyStrefy.EditItem, e.NewValues);
        }

        protected void lvDzialyStrefy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateItem(e.Item, e.Values);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntDicCC : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvDic, 0);
            Tools.PrepareSorting(lvDic, 1, 10);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //----------------------
        protected void btFilter_Click(object sender, EventArgs e)
        {
            //SqlDataSource1.FilterExpression = new filter
        }

        protected void lvDic_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, lvDic, out drv);
            switch (lim)
            {
                case Tools.limSelect:
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trLine");
                    if (tr != null)
                    {
                        bool akt = db.getBool(drv["Wybor"], false);
                        bool grp = db.getBool(drv["Grupa"], false);
                        Tools.AddClass(tr, akt ? "actual" : "old");
                        Tools.AddClass(tr, grp ? "grupa" : "cc");
                    }
                    break;
                case Tools.limEdit:
                    Tools.SelectItem(e.Item, "ddlGrupa", drv["IdGrupy"]);
                    break;
            }
        }
        //------------------------------
        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            //values["IdGrupy"] = Tools.GetDdlSelectedValueInt(item, "ddlGrupa");
        }

        //------------------------------
        protected void lvDic_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvDic.EditItem, e.NewValues);
        }

        protected void lvDic_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateItem(e.Item, e.Values);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntDicArea : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            HRRcp.App_Code.Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private DataSet dsCommodity()
        {
            return db.getDataSet("select Id as CommodityId, Commodity from Commodity where Aktywne = 1 order by Commodity");
        }

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int lim = Tools.GetListItemMode(e, ListView1);
            switch (lim)
            {
                case Tools.limInsert:
                    Tools.BindData(e.Item, "ddlCommodity", dsCommodity(), "Commodity", "CommodityId", true, null);
                    Tools.SetChecked(e.Item, "AktywneCheckBox", true);
                    break;
            }
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv;
            int lim = Tools.GetListItemMode(e, ListView1, out drv);
            switch (lim)
            {
                case Tools.limEdit:
                    string id = drv["CommodityId"].ToString();
                    Tools.BindData(e.Item, "ddlCommodity", dsCommodity(), "Commodity", "CommodityId", true, id);
                    break;
            }
        }

        //private bool Update(ListViewItem item, )

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {

        }

        protected void ListView1_ItemInserting(object sender, ListViewInsertEventArgs e)
        {

        }
    }
}
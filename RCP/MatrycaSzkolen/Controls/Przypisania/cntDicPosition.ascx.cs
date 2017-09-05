using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.MatrycaSzkolen.Controls.Przypisania
{
    public partial class cntDicPosition : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(ListView1, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private DataSet dsCommodity()
        {
            return db.getDataSet("select Id as CommodityId, Commodity from Commodity where Aktywne = 1 order by Commodity");
        }

        private DataSet dsArea(string commId)
        {
            return db.getDataSet(String.Format(@"
select A.Id as AreaId, A.Area, 
ISNULL(C.Commodity + '/', '') + A.Area as CommArea,
A.Area + ISNULL(' (' + C.Commodity + ')', '') as CommArea2
from Area A
left outer join Commodity C on C.Id = A.CommodityId
where A.Aktywne = 1 and (A.CommodityId is null{0}) 
order by C.Commodity, A.Area
                ", String.IsNullOrEmpty(commId) ? null : " or CommodityId = " + commId));
        }

        const string areaField = "CommArea2";

        protected void ListView1_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int lim = Tools.GetListItemMode(e, ListView1);
            switch (lim)
            {
                case Tools.limInsert:
                    Tools.BindData(e.Item, "ddlCommodity", dsCommodity(), "Commodity", "CommodityId", true, null);
                    Tools.BindData(e.Item, "ddlArea", dsArea(null), areaField, "AreaId", true, null);
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
                    string id2 = drv["AreaId"].ToString();
                    Tools.BindData(e.Item, "ddlCommodity", dsCommodity(), "Commodity", "CommodityId", true, id);
                    Tools.BindData(e.Item, "ddlArea", dsArea(id), areaField, "AreaId", true, id2);
                    break;
            }
        }

        protected void ddlCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCommodity = (DropDownList)sender;
            DropDownList ddlArea = ddlCommodity.Parent.FindControl("ddlArea") as DropDownList;
            if (ddlArea != null)
            {
                string sel = ddlArea.SelectedValue;
                Tools.BindData(ddlArea, dsArea(ddlCommodity.SelectedValue), areaField, "AreaId", true, sel);
            }
        }
    }
}
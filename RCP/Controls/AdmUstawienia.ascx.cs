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
    public partial class AdmUstawienia : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //-------------------
        private bool InitEditItem(ListViewItemEventArgs e)
        {
            if (lvParametry.EditItem != null && ((ListViewDataItem)e.Item).DisplayIndex == lvParametry.EditIndex)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;

                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlPrzerwaMM");
                App.FillBreak(ddl, drv["PrzerwaMM"].ToString(), null, false);
                ddl = (DropDownList)e.Item.FindControl("ddlPrzerwa2MM");
                App.FillBreak(ddl, drv["Przerwa2MM"].ToString(), null, true);
                ddl = (DropDownList)e.Item.FindControl("ddlMarginesMM");
                App.FillBreak(ddl, drv["MarginesMM"].ToString(), null, false);

                ddl = (DropDownList)e.Item.FindControl("ddlZaokr");
                App.FillTimeRound(ddl, drv["Zaokr"].ToString(), null);
                ddl = (DropDownList)e.Item.FindControl("ddlZaokrSum");
                App.FillTimeRound(ddl, drv["ZaokrSum"].ToString(), null);
                ddl = (DropDownList)e.Item.FindControl("ddlZaokrType");
                App.FillRoundType(ddl, drv["ZaokrType"].ToString(), null);
                ddl = (DropDownList)e.Item.FindControl("ddlZaokrSumType");
                App.FillRoundType(ddl, drv["ZaokrSumType"].ToString(), null);

                ddl = (DropDownList)e.Item.FindControl("ddlOkresDo");
                App.FillDays(ddl, drv["OkresDo"].ToString());
                return true;
            }
            else return false;
        }

        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            values["PrzerwaMM"] = Tools.GetDdlSelectedValueInt(item, "ddlPrzerwaMM");
            values["Przerwa2MM"] = Tools.GetDdlSelectedValueInt(item, "ddlPrzerwa2MM");
            values["MarginesMM"] = Tools.GetDdlSelectedValueInt(item, "ddlMarginesMM");
            
            values["Zaokr"] = Tools.GetDdlSelectedValueInt(item, "ddlZaokr");
            values["ZaokrSum"] = Tools.GetDdlSelectedValueInt(item, "ddlZaokrSum");
            values["ZaokrType"] = Tools.GetDdlSelectedValueInt(item, "ddlZaokrType");
            values["ZaokrSumType"] = Tools.GetDdlSelectedValueInt(item, "ddlZaokrSumType");

            values["OkresDo"] = Tools.GetDdlSelectedValueInt(item, "ddlOkresDo");
        }
        
        protected void lvParametry_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
                if (InitEditItem(e))
                {
                }
        }

        protected void lvParametry_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvParametry.EditItem, e.NewValues);
        }

        protected void lvParametry_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            Ustawienia settings = Ustawienia.CreateOrGetSession();
            settings.Reread();
        }
    }
}
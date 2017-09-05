using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AdmKierParams : System.Web.UI.UserControl
    {
        Ustawienia settings;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ListView1_DataBinding(object sender, EventArgs e)
        {
            settings = Ustawienia.CreateOrGetSession();
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem di = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)di.DataItem;
                string kid = drv["IdKierownika"].ToString();

                KierParams kp = new KierParams(kid, settings);

                DropDownList ddlPrzerwa = (DropDownList)e.Item.FindControl("ddlPrzerwa");
                DropDownList ddlPrzerwa2 = (DropDownList)e.Item.FindControl("ddlPrzerwa2");
                DropDownList ddlMargin = (DropDownList)e.Item.FindControl("ddlMargin");

                App.FillBreak(ddlPrzerwa, kp.KierPrzerwa.ToString(), settings.Przerwa.ToString(), false);
                App.FillBreak(ddlPrzerwa2, kp.KierPrzerwa2.ToString(), settings.Przerwa2.ToString(), true);
                App.FillBreak(ddlMargin, kp.KierMargines.ToString(), settings.Margines.ToString(), false);
            }

        }

        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            string kid = lvKierParams.DataKeys[e.ItemIndex].Value.ToString();
            string p = Tools.GetDdlSelectedValue(lvKierParams.EditItem, "ddlPrzerwa");
            string p2 = Tools.GetDdlSelectedValue(lvKierParams.EditItem, "ddlPrzerwa2");
            string m = Tools.GetDdlSelectedValue(lvKierParams.EditItem, "ddlMargin");
            string d = Base.DateToStr(e.NewValues["DataAccDo"]);
            KierParams.Update(kid, p, p2, m, d);
            e.Cancel = true;
            lvKierParams.EditIndex = -1;
            //lvKierParams.DataBind();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections.Specialized;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class ZmianyControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lvZmiany_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public string StawkaText(string stawka)
        {
            return App.StawkaText(stawka);
        }

        public Color GetColorNull(string color)
        {
            try
            {
                //return ColorTranslator.FromHtml(color.StartsWith("#") ? color : "#" + color);
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        public string GetColorNoHash(string color)
        {
            try
            {
                return color.StartsWith("#") ? color.Substring(1) : color;
            }
            catch
            {
                return "Transparent";
            }
        }

        protected void StawkaDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lvZmiany_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            /*
            DropDownList ddl = (DropDownList)lvZmiany.EditItem.FindControl("StawkaDropDownList");
            if (ddl != null)
                Tools.SelectItem(ddl, 
             */
            //lvZmiany.DataBind();
        }

        private void InitItem(ListViewItem item, string tod, string tdo, string stawka)
        {
            DropDownList ddl = (DropDownList)item.FindControl("StawkaDropDownList");
            if (ddl != null)
                App.FillStawki(ddl, stawka);

            ddl = (DropDownList)item.FindControl("ddlCzasOd");
            if (ddl != null)
            {
                Tools.FillTime(ddl, 0, 60);
                Tools.SelectItem(ddl, tod);
            }

            ddl = (DropDownList)item.FindControl("ddlCzasDo");
            if (ddl != null)
            {
                Tools.FillTime(ddl, 0, 60);
                Tools.SelectItem(ddl, tdo);
            }
        }

        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            DataRowView drv = (DataRowView)dataItem.DataItem;

            if (lvZmiany.EditItem != null && dataItem.DisplayIndex == lvZmiany.EditIndex)
                InitItem(e.Item, drv["CzasOd"].ToString(),
                                 drv["CzasDo"].ToString(),
                                 drv["Stawka"].ToString());
            
            /*
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("StawkaDropDownList");
                if (ddl != null) 
                    App.FillStawki(ddl, drv["Stawka"].ToString());

                ddl = (DropDownList)e.Item.FindControl("ddlCzasOd");
                if (ddl != null)
                {
                    Tools.FillTime(ddl, 0, 60);
                    Tools.SelectItem(ddl, drv["CzasOd"].ToString());
                }

                ddl = (DropDownList)e.Item.FindControl("ddlCzasDo");
                if (ddl != null)
                {
                    Tools.FillTime(ddl, 0, 60);
                    Tools.SelectItem(ddl, drv["CzasDo"].ToString());
                }
            }
             */
        }

        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            DropDownList ddl = (DropDownList)item.FindControl("StawkaDropDownList");
            if (ddl != null) values["Stawka"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasOd");
            if (ddl != null) values["Od"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasDo");
            if (ddl != null) values["Do"] = ddl.SelectedValue;

            ZmianaGodziny zg = (ZmianaGodziny)item.FindControl("cntNadgodziny");
            if (zg != null) values["Nadgodziny"] = zg.Overtimes;  // tu się pobierze z pól

            TextBox tb = (TextBox)item.FindControl("KolorTextBox");
            if (tb != null) values["Kolor"] = String.IsNullOrEmpty(tb.Text) ? null : "#" + tb.Text; 
        }

        protected void lvZmiany_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateItem(e.Item, e.Values);
        }

        protected void lvZmiany_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvZmiany.EditItem, e.NewValues);

            /*
            DropDownList ddl = (DropDownList)lvZmiany.EditItem.FindControl("StawkaDropDownList");
            if (ddl != null) e.NewValues["Stawka"] = ddl.SelectedValue;

            ddl = (DropDownList)lvZmiany.EditItem.FindControl("ddlCzasOd");
            if (ddl != null)
            {
                e.NewValues["Od"] = ddl.SelectedValue;
            }

            ddl = (DropDownList)lvZmiany.EditItem.FindControl("ddlCzasDo");
            if (ddl != null)
            {
                e.NewValues["Do"] = ddl.SelectedValue;
            }

            ZmianaGodziny zg = (ZmianaGodziny)lvZmiany.EditItem.FindControl("cntNadgodziny");
            if (zg != null)
                e.NewValues["Nadgodziny"] = zg.Overtimes;  // tu się pobierze z pól
             */ 
        }

        protected void lvZmiany_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    lvZmiany.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlVisible(lvZmiany, "InsertButton", true);
                    break;
                case "Update":
                    // zmiana StanuAnkiety z reki - zoom i powrót - wyswietla stan poprzedni
                    /*
                    lvPracownicy.DataBind();  nic nie daje ..., wyglada jakby strona byla ładowana z cache mimo ustawienia Tools.NoCache na razie brak pomyslu 
                    SqlDataSource1.DataBind();
                     */
                    break;
                case "NewRecord":
                    lvZmiany.EditIndex = -1;
                    Tools.SetControlVisible(lvZmiany, "InsertButton", false);
                    lvZmiany.InsertItemPosition = InsertItemPosition.FirstItem;
                    break;
                case "Insert": 
                case "CancelInsert":
                    Tools.SetControlVisible(lvZmiany, "InsertButton", true);
                    lvZmiany.InsertItemPosition = InsertItemPosition.None;
                    break;
            }
        }

        protected void lvZmiany_ItemCreated(object sender, ListViewItemEventArgs e)  // tu mam dostęp do InsertItemTemplate controls
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                InitItem(e.Item, "08:00", "16:00", null);
            }
        }
    }
}
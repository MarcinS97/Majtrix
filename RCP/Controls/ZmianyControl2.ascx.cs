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
    public partial class ZmianyControl2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lvZmiany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }

        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                if (dataItem.DisplayIndex == lvZmiany.EditIndex)
                {
                    ImageButton ibt = (ImageButton)e.Item.FindControl("KolorImageButton");
                    if (ibt != null)
                    {
                        //ibt.Attributes.Add("onClick", "javascript:colorClick('" + ibt.ClientID + "');return false;");
                        //ibt.Attributes.Add("onClick", "javascript:colorClick(this);return false;");
                        ibt.Attributes.Add("onClick", "javascript:jscolor.init2(this);return false;");
                    }

                    DataRowView drv = (DataRowView)dataItem.DataItem;
                    if (lvZmiany.EditItem != null && dataItem.DisplayIndex == lvZmiany.EditIndex)
                        InitItem(e.Item, drv["CzasOd"].ToString(),
                                         drv["CzasDo"].ToString(),
                                         drv["Stawka"].ToString());
                }
            }
        }

        protected void lvZmiany_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateItem(e.Item, e.Values);
        }

        protected void lvZmiany_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvZmiany.EditItem, e.NewValues);
        }

        const string msgTrwaEdycja = "Proszę zakończyć bieżącą edycję danych.";

        protected void lvZmiany_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            if (lvZmiany.EditIndex != -1)
            {
                Tools.ShowMessage(msgTrwaEdycja);
                e.Cancel = true;
            }
        }

        protected void lvZmiany_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Edit":
                    lvZmiany.InsertItemPosition = InsertItemPosition.None;  // chowam
                    Tools.SetControlEnabled(lvZmiany, "InsertButton", true);
                    break;
                case "Update":
                    break;
                case "NewRecord":
                    if (lvZmiany.EditIndex != -1) Tools.ShowMessage(msgTrwaEdycja);
                    else
                    {
                        lvZmiany.EditIndex = -1;
                        Tools.SetControlEnabled(lvZmiany, "InsertButton", false);
                        lvZmiany.InsertItemPosition = InsertItemPosition.FirstItem;
                    }
                    break;
                case "Insert":
                case "CancelInsert":
                    Tools.SetControlEnabled(lvZmiany, "InsertButton", true);
                    lvZmiany.InsertItemPosition = InsertItemPosition.None;
                    break;
            }
        }

        protected void lvZmiany_ItemCreated(object sender, ListViewItemEventArgs e)  // tu mam dostęp do InsertItemTemplate controls
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                InitItem(e.Item, "08:00", "16:00", null);
                /*
                ImageButton ibt = (ImageButton)e.Item.FindControl("KolorImageButton");
                if (ibt != null) ibt.Attributes["value"] = "FFFFFF";
                */
                HiddenField hid = (HiddenField)e.Item.FindControl("hidColor");
                if (hid != null) hid.Value = "#FFFFFF";

                CheckBox cb = (CheckBox)e.Item.FindControl("VisibleCheckBox");
                if (cb != null) cb.Checked = true;

                ImageButton ibt = (ImageButton)e.Item.FindControl("KolorImageButton");
                if (ibt != null) ibt.Attributes["onClick"] = "javascript:jscolor.init2(this);return false;";
            }
        }

        protected void lvZmiany_PreRender(object sender, EventArgs e)
        {
            if (lvZmiany.InsertItemPosition != InsertItemPosition.None)
            {
                HiddenField hid = (HiddenField)lvZmiany.InsertItem.FindControl("hidColor");
                Label lb = (Label)lvZmiany.InsertItem.FindControl("SymbolLabel");
                if (hid != null && lb != null) lb.BackColor = GetColorNull(hid.Value);
            }
            else if (lvZmiany.EditIndex != -1)
            {
                HiddenField hid = (HiddenField)lvZmiany.EditItem.FindControl("hidColor");
                Label lb = (Label)lvZmiany.EditItem.FindControl("SymbolLabel");
                if (hid != null && lb != null) lb.BackColor = GetColorNull(hid.Value);
            }
        }

        //----------------------------
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
        //----------------------------
        private void InitItem(ListViewItem item, string tod, string tdo, string stawka)
        {
            DropDownList ddl = (DropDownList)item.FindControl("ddlStawka");
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

        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            DropDownList ddl = (DropDownList)item.FindControl("ddlStawka");
            if (ddl != null) values["Stawka"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasOd");
            if (ddl != null) values["Od"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasDo");
            if (ddl != null) values["Do"] = ddl.SelectedValue;

            ZmianaGodziny2 zg = (ZmianaGodziny2)item.FindControl("cntNadgodziny");
            if (zg != null) values["Nadgodziny"] = zg.Overtimes;  // tu się pobierze z pól

            /*
            ImageButton ibt = (ImageButton)item.FindControl("KolorImageButton");
            if (ibt != null)
            {
                string v = ibt.Attributes["value"];
                values["Kolor"] = String.IsNullOrEmpty(v) ? null : ("#" + v);
                //values["Kolor"] = v;
            }
            /**/
            /*
            Label lb = (Label)item.FindControl("SymbolLabel");
            if (lb != null) values["Kolor"] = lb.BackColor;
            */
                /*
            string c;
            if (Tools.GetControlValue(item, "hidColor", out c))
                values["Kolor"] = c;
                 */
        }

    }
}
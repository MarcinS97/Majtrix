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
    public partial class ZmianyControl3 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /*
        protected string GetTypZmiany(object typ)
        {
            return "zwykłe i nocne";
        }
        */

        protected string GetMargines(object margines)
        {
            if (!db.isNull(margines))
            {
                int m = db.getInt(margines, 0);
                if (m > 0)
                    //return String.Format("±{0} min.", m.ToString());
                    return String.Format("+/- {0} min.", m.ToString());
            }                
            return null;
        }

        protected string GetPanelClass(object newLine)
        {
            bool nl = db.getBool(newLine, false);
            return nl ? " newline" : null;
        }
        
        protected void lvZmiany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }

        private void switchTypZmiany(ListViewItem li, string ztyp, string nadgodziny)
        {
            bool v_1 = ztyp == App.zmNadgBrakZgody || (ztyp == App.zmNadgKolejne && String.IsNullOrEmpty(nadgodziny));
            bool v0  = ztyp == App.zmNadgKolejne && !String.IsNullOrEmpty(nadgodziny);
            bool v1  = ztyp == App.zmNadgZwykleNocne;
            bool v2  = ztyp == App.zmAbsencja;
            Tools.SetControlVisible(li, "trBrakZgody",        !v2 && v_1);
            Tools.SetControlVisible(li, "cntNadgodziny",      !v2 && v0);
            Tools.SetControlVisible(li, "trNadgodzinyZwykle", !v2 && v1);
            Tools.SetControlVisible(li, "trNadgodzinyNocne",  !v2 && v1);

            Tools.SetControlVisible(li, "lbNadgodziny",       !v2 && v_1);
            Tools.SetControlVisible(li, "lbNadgodzinyZgoda",  !v2 && !v_1);

            Tools.SetControlVisible(li, "tr1",  !v2);
            Tools.SetControlVisible(li, "tr1b", !v2);
            Tools.SetControlVisible(li, "tr2",  !v2);
            Tools.SetControlVisible(li, "tr2b", !v2);
        }

        protected void ddlTypZmiany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem li;

            if (lvZmiany.EditIndex != -1) li = lvZmiany.EditItem;
            else if (lvZmiany.InsertItemPosition != InsertItemPosition.None) li = lvZmiany.InsertItem;
            else return;    // generalnie niemożliwe...
            switchTypZmiany(li, ((DropDownList)sender).SelectedValue, "x");  // tak trzeba
        }
       
        protected void lvZmiany_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                DataRowView drv = (DataRowView)dataItem.DataItem;
                string ztyp = drv["TypZmiany"].ToString();
                string nadg = drv["Nadgodziny"].ToString();
                switchTypZmiany(e.Item, ztyp, nadg);
                //----- edit item -----
                if (dataItem.DisplayIndex == lvZmiany.EditIndex)
                {
                    ImageButton ibt = (ImageButton)e.Item.FindControl("KolorImageButton");
                    if (ibt != null)
                    {
                        //ibt.Attributes.Add("onClick", "javascript:colorClick('" + ibt.ClientID + "');return false;");
                        //ibt.Attributes.Add("onClick", "javascript:colorClick(this);return false;");
                        ibt.Attributes.Add("onClick", "javascript:jscolor.init2(this);return false;");
                    }
                    InitItem(e.Item, drv["CzasOd"].ToString(),
                                     drv["CzasDo"].ToString(),
                                     drv["Stawka"].ToString(),
                                     drv["Margines"].ToString(),
                                     ztyp,
                                     drv["NadgodzinyDzien"].ToString(),
                                     drv["NadgodzinyNoc"].ToString(),
                                     nadg,
                                     drv["Typ"].ToString(),
                                     drv["MarginesNadgodzin"].ToString());
                }
#if SIEMENS
                Tools.SetText2(e.Item, "ltBrakZgody", brakzgody1);
#endif
            }
        }

#if SIEMENS
        const string brakzgody1 = "Wymagana zgoda na nadgodziny";
        const string brakzgody2 = "wymagana zgoda";
#endif

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
                InitItem(e.Item, "08:00", "16:00", "100", "0", "1", "150", "200", null, "0", "0");
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
        private void InitItem(ListViewItem item, string tod, string tdo, string stawka, string margines, string typZmiany, string stawkaZwykle, string stawkaNocne, string overtimes, string typ, string marginesNadgodzin)
        {
            App.FillStawki((DropDownList)item.FindControl("ddlStawka"), stawka);
            App.FillStawki((DropDownList)item.FindControl("ddlStawkaZwykle"), stawkaZwykle);
            App.FillStawki((DropDownList)item.FindControl("ddlStawkaNocne"), stawkaNocne);

            Tools.BindData(item, "ddlMargines", 
                db.getDataSet("select Nazwa, Kod as Margines from Kody where Typ = 'MARGINES' order by Lp"), 
                "Nazwa", "Margines", false, margines);

            Tools.SelectItem(item, "ddlMarginesNadgodzin", marginesNadgodzin);

            //if (String.IsNullOrEmpty(typZmiany)) typZmiany = "0";
            if (String.IsNullOrEmpty(typZmiany)) typZmiany = "-1";
            if (typZmiany == App.zmNadgKolejne && String.IsNullOrEmpty(overtimes)) typZmiany = "-1"; 
            Tools.SelectItem(item, "ddlTypZmiany", typZmiany);

            DropDownList ddl = (DropDownList)item.FindControl("ddlCzasOd");
            if (ddl != null)
            {
                Tools.FillTime(ddl, 0, 15);
//#if SIEMENS || DBW || VICIM || VC
//                Tools.FillTime(ddl, 0, 30);
//#else
//                Tools.FillTime(ddl, 0, 60);
//#endif
                Tools.SelectItem(ddl, tod);
            }

            ddl = (DropDownList)item.FindControl("ddlCzasDo");
            if (ddl != null)
            {
                Tools.FillTime(ddl, 0, 15);
//#if SIEMENS || DBW || VICIM || VC
//                Tools.FillTime(ddl, 0, 30);
//#else
//                Tools.FillTime(ddl, 0, 60);
//#endif
                Tools.SelectItem(ddl, tdo);
            }

            Tools.SelectItem(item, "ddlTyp", typ);
        }

        private void UpdateItem(ListViewItem item, IOrderedDictionary values)
        {
            DropDownList ddl = (DropDownList)item.FindControl("ddlStawka");
            if (ddl != null) values["Stawka"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasOd");
            if (ddl != null) values["Od"] = ddl.SelectedValue;

            ddl = (DropDownList)item.FindControl("ddlCzasDo");
            if (ddl != null) values["Do"] = ddl.SelectedValue;

            values["Margines"] = Tools.GetDdlSelectedValueInt(item, "ddlMargines");
            values["MarginesNadgodzin"] = Tools.GetDdlSelectedValueInt(item, "ddlMarginesNadgodzin");

            int? ztyp = Tools.GetDdlSelectedValueInt(item, "ddlTypZmiany");
            if (ztyp == null) ztyp = 0;

            bool zgoda = false;
            switch (ztyp)
            {
                default:
                case -1:
                    ztyp = 0;  // App.zmNadgodzinyKolejne
                    values["Nadgodziny"] = null;
                    values["NadgodzinyDzien"] = null;
                    values["NadgodzinyNoc"] = null;
                    break;
                case 0:
                    ZmianaGodziny3 zg = (ZmianaGodziny3)item.FindControl("cntNadgodziny");
                    if (zg != null) 
                    {
                        string ot = zg.Overtimes;
                        values["Nadgodziny"] = ot;  // tu się pobierze z pól
                        zgoda = !String.IsNullOrEmpty(ot);
                    }
                    values["NadgodzinyDzien"] = null;
                    values["NadgodzinyNoc"] = null;                               
                    break;
                case 1:
                    zgoda = true;
                    values["Nadgodziny"] = null;
                    values["NadgodzinyDzien"] = Tools.GetDdlSelectedValueInt(item, "ddlStawkaZwykle");
                    values["NadgodzinyNoc"] = Tools.GetDdlSelectedValueInt(item, "ddlStawkaNocne");
                    break;
                case 2:     // absencja (długotrwała)
                    values["Nadgodziny"] = null;
                    values["NadgodzinyDzien"] = null;
                    values["NadgodzinyNoc"] = null;
                    break;
            }

            values["TypZmiany"] = ztyp;
            values["ZgodaNadg"] = zgoda;

            values["Typ"] = Tools.GetDdlSelectedValueInt(item, "ddlTyp");
        }


        protected void lvZmiany_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string id = Tools.GetDataKey(lvZmiany, e);
            if (!String.IsNullOrEmpty(id))
            {
                DataRow dr = db.getDataRow(String.Format("select top 1 Id from PlanPracy where IdZmiany = {0} or IdZmianyKorekta = {0}", id));
                bool cancel = dr != null;
                if (cancel) Tools.ShowError("Usunięcie niemożliwe, zmiana była używana w planie pracy.");
                e.Cancel = cancel;
            }
            //else - niemożliwe ...
        }


        protected void lnkRemove_Click(object sender, EventArgs e)
        {
            HiddenField hidInneCzasy = null;
            if (lvZmiany.EditIndex >= 0)
                hidInneCzasy = lvZmiany.EditItem.FindControl("hidInneCzasy") as HiddenField;
            else if (lvZmiany.InsertItem != null)
                hidInneCzasy = lvZmiany.InsertItem.FindControl("hidInneCzasy") as HiddenField;

            if (hidInneCzasy != null)
            {
                string zmiana = (sender as LinkButton).CommandArgument;
                if (!String.IsNullOrEmpty(zmiana))
                {

                    string id = zmiana.Split(';')[0];
                    string sidx = zmiana.Split(';')[1];
                    int idx = Convert.ToInt32(sidx);

                    List<String> zmianyList = hidInneCzasy.Value.Split(';').ToList();

                    for (int i = 0; i < zmianyList.Count; i++)
                    {
                        if (zmianyList[i] == id && i == idx)
                        {
                            zmianyList.RemoveAt(i);
                            break;
                        }
                    }
                    
                    hidInneCzasy.Value = String.Join(";", zmianyList.ToArray());

                }
            }
        }

        protected void lnkAddInneCzasy_Click(object sender, EventArgs e)
        {
            HiddenField hidInneCzasy = null;
            TimeEdit te = null;
            if (lvZmiany.EditIndex >= 0){
                hidInneCzasy = lvZmiany.EditItem.FindControl("hidInneCzasy") as HiddenField;
                te = lvZmiany.EditItem.FindControl("teTimeIn") as TimeEdit;
            }
            else if (lvZmiany.InsertItem != null) {
                hidInneCzasy = lvZmiany.InsertItem.FindControl("hidInneCzasy") as HiddenField;
                te = lvZmiany.InsertItem.FindControl("teTimeIn") as TimeEdit;
            }

            if (hidInneCzasy != null)
            {
                if (!String.IsNullOrEmpty(hidInneCzasy.Value)) { 
                hidInneCzasy.Value = hidInneCzasy.Value + ";" + te.Seconds;
                }
                else
                {
                    hidInneCzasy.Value = te.Seconds.ToString();
                }
            }
        }


        protected void lvZmiany_DataBound(object sender, EventArgs e)
        {
#if SIEMENS
            if (lvZmiany.EditIndex != -1)
            {
                DropDownList ddl = lvZmiany.EditItem.FindControl("ddlTypZmiany") as DropDownList;
                if (ddl != null)
                {
                    ListItem item = ddl.Items.FindByValue("-1");
                    if (item != null) item.Text = brakzgody2;
                }
            }
            if (lvZmiany.InsertItemPosition != InsertItemPosition.None)
            {
                DropDownList ddl = lvZmiany.InsertItem.FindControl("ddlTypZmiany") as DropDownList;
                if (ddl != null)
                {
                    ListItem item = ddl.Items.FindByValue("-1");
                    if (item != null) item.Text = brakzgody2;
                }
            }
#endif
        }
    }
}
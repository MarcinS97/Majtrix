using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Collections.Specialized;
using System.Data;

namespace HRRcp.Controls.Reports
{
    public partial class cntFilterFields : System.Web.UI.UserControl
    {
        public event EventHandler DataBound;
        public event EventHandler SelectedChanged;

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvFields, 2);  // z ImageButtons
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool ApplyTo(SqlDataSource ds)
        {
            bool b = false;
            foreach (ListViewItem item in lvFields.Items)
            {
                cntField f = item.FindControl("cntField") as cntField;
                if (f != null)
                    if (f.ApplyTo(ds))
                        b = true;
            }
            return b;
        }

        //public void ApplyTo(ref string sql)
        //{
        //    foreach (ListViewItem item in lvFields.Items)
        //    {
        //        cntField f = item.FindControl("cntField") as cntField;
        //        if (f != null)
        //            f.ApplyTo(ref sql);
        //    }
        //}

        public void Clear()
        {
            foreach (ListViewItem item in lvFields.Items)
            {
                cntField f = item.FindControl("cntField") as cntField;
                if (f != null) f.Clear();
            }
        }

        //------------------
        private void TriggerDataBound()
        {
            if (lvFields.Items.Count > 0)
            {

            }
            if (DataBound != null)
                DataBound(this, EventArgs.Empty);
        }

        private void TriggerselectedChanged()
        {
            if (SelectedChanged != null)
                SelectedChanged(this, EventArgs.Empty);
        }
        //------------------
        public void Insert()
        {
            lvFields.EditIndex = -1; // pytanie ?
            showModal = true;
            lvFields.InsertItemPosition = InsertItemPosition.LastItem;
//            lvFields.DataBind();
        }

        protected string GetItemCss(object rpDataItem)
        {
            DataRowView drv = (DataRowView)rpDataItem;
            bool akt = db.getBool(drv["Aktywny"], false);
            return akt ? "" : " it-disabled";
        }
        //--------------------------
        public static bool MoveListItem(ListView lv, int dir, string table, string fid, string flp)//, DataSet ds, string _id, string table, string prefix)
        {
            List<int> nodes = new List<int>();
            bool ok = false;
            int sel = lv.SelectedIndex;
            var d = lv.DataKeys;

            if (sel != -1)
            {
                foreach(DataKey k in lv.DataKeys)
                    nodes.Add((int)k.Value);
                int id = (int)lv.DataKeys[sel].Value;
                switch (dir)
                {
                    case -9:
                        nodes.RemoveAt(sel);
                        nodes.Insert(0, id);
                        sel = 0;
                        ok = true;
                        break;
                    case 9:
                        nodes.RemoveAt(sel);
                        nodes.Add(id);
                        ok = true;
                        sel = nodes.Count - 1;
                        break;
                    case -1:
                        if (sel > 0)
                        {
                            nodes.RemoveAt(sel);
                            sel--;
                            nodes.Insert(sel, id);
                            ok = true;
                        }
                        break;
                    case 1:
                        if (sel < nodes.Count - 1)
                        {
                            nodes.RemoveAt(sel);
                            sel++;
                            nodes.Insert(sel, id);
                            ok = true;
                        }
                        break;
                }
                if (ok)
                {
                    int lp = 10;
                    foreach (int id2 in nodes)
                    {
                        db.update(table, 0, flp, String.Format("{0}={1}", fid, id2), lp);
                        lp += 10;
                    }
                    lv.DataBind();
                    lv.SelectedIndex = sel; 
                }
            }
            return ok;
        }

        public bool Move(int dir)
        {
            return MoveListItem(lvFields, dir, "SqlFields", "Id", "Kolejnosc");
        }
        //--------------------------
        protected void SqlDataSource1_DataBinding(object sender, EventArgs e)
        {

        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        int lastId = -1;
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                lastId = (int)e.Command.Parameters["@Id"].Value;
            }
        }

        //----------------------------
        bool showModal = false;
        const string eitZoom = "eit-divZoom";
        const string iitZoom = "iit-divZoom";

        int idx = 0;
        protected object getOnClick(object item)
        {
            if (EditMode)
            {
                Button bt = ((Control)item).FindControl("btSelect") as Button;
                if (bt != null)
                {
                    string onclick = String.Format("javascript:doClick(\"{0}\");", bt.ClientID);   //ContentPlaceHolder1_cntFilter_cntFilterFields1_lvFields_btSelect_0  !!! muszą być \" bo w ascx jest ''
                    idx++;
                    return onclick;
                }

                //string onclick = String.Format("javascript:doClick(\"{0}_btSelect_{1}\");", lvFields.ClientID, idx.ToString());   //ContentPlaceHolder1_cntFilter_cntFilterFields1_lvFields_btSelect_0
                //idx++;
                //return onclick;
            }
            //else
                return null;
        }

        protected void lvFields_DataBinding(object sender, EventArgs e)
        {
            idx = 0;
        }

        protected void lvFields_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                cntFieldParams fp = lvFields.InsertItem.FindControl("cntFieldParamsI") as cntFieldParams;
                if (fp != null) fp.SetData(null);
                if (showModal)
                {
                    showModal = false;
                    Tools.ShowDialogF(this, iitZoom, 800, btCancelEdit, true, "Dodaj pole");
                }
            }
        }

        protected void lvFields_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //TableCell td = e.Item.FindControl("tdControl") as TableCell;
            //if (td != null) td.Visible = EditMode;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (((ListViewDataItem)e.Item).DisplayIndex == lvFields.EditIndex)
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    cntFieldParams fp = e.Item.FindControl("cntFieldParamsE") as cntFieldParams;   // ciekawostka: na lvFields.EditItem nie widać tej kontrolki jeszcze ...
                    if (fp != null) fp.SetData(drv);
                    if (showModal)
                    {
                        showModal = false;
                        Tools.ShowDialogF(this, eitZoom, 800, btCancelEdit, true, "Edycja");   // btCancelEdit musi być poza ListView bo nie chowa EditItem/InsertItem
                    }
                }
                else
                {
                    //Tools.OnClick(e.Item, "trLine", "btSelect");  // to się nie wykonuje, bo nie ma trLine - nie moze być bo to tr zawiera cntFields a nie td
                }
            }
        }

        protected void lvFields_DataBound(object sender, EventArgs e)
        {
            if (lvFields.InsertItemPosition != InsertItemPosition.None)
            {
            }
            TriggerDataBound();
        }
        //-----
        protected void lvFields_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvFields.SelectedIndex = e.NewEditIndex;
            lvFields.InsertItemPosition = InsertItemPosition.None;   // pytanie ?
            showModal = true;
            //Tools.ShowDialog(this, "divZoom", 800, btClose, true, "Edycja");
            //Tools.ShowDialog(this, "eit-divZoom", 800, null, true, "Edycja");
        }

        protected void lvFields_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            Tools.CloseDialog(eitZoom);
        }

        protected void lvFields_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Unselect":
                    lvFields.SelectedIndex = -1;
                    TriggerselectedChanged();
                    break;
                case "NewRecord":    // niewykorzystywane, przez funkcję Insert() się to robi
                    lvFields.EditIndex = -1; // pytanie ?
                    lvFields.InsertItemPosition = InsertItemPosition.LastItem;                    
                    showModal = true;
                    break;
                case "CancelInsert":
                    lvFields.InsertItemPosition = InsertItemPosition.None;
                    Tools.CloseDialog(iitZoom);
                    break;
            }
        }

        private void CloseDialog(string idZoom, int val)
        {
            Tools.CloseDialog(idZoom);
            Tools.CloseDialogOverlay();
            lvFields.DataBind();
            for (int idx = 0; idx < lvFields.Items.Count - 1; idx++)
                if ((int)lvFields.DataKeys[idx].Value == val)
                {
                    lvFields.SelectedIndex = idx;
                    break;
                }
            UpdatePanel up = Tools.FindUpdatePanel(this);
            if (up != null) up.Update();
        }

        protected void lvFields_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            CloseDialog(eitZoom, (int)lvFields.DataKeys[lvFields.SelectedIndex].Value);
        }

        protected void lvFields_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            CloseDialog(iitZoom, lastId);


            lvFields.InsertItemPosition = InsertItemPosition.None;  // plomba !!!
        }

        protected void lvFields_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            cntFieldParams fp = lvFields.EditItem.FindControl("cntFieldParamsE") as cntFieldParams;
            if (fp != null) fp.GetData(e.NewValues);
        }

        protected void lvFields_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            cntFieldParams fp = lvFields.InsertItem.FindControl("cntFieldParamsI") as cntFieldParams;
            if (fp != null) fp.GetData(e.Values);
        }

        protected void btCancelEdit_Click(object sender, EventArgs e)
        {
            lvFields.EditIndex = -1;
            lvFields.InsertItemPosition = InsertItemPosition.None;
        }

        protected void lvFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerselectedChanged();
        }
        //--------------------------------------
        public string ReportId
        {
            set { hidRepId.Value = value; }
            get { return hidRepId.Value; }
        }

        public string Column
        {
            set { hidColumn.Value = value; }
            get { return hidColumn.Value; }
        }

        public bool EditMode
        {
            set
            {
                //ViewState["editm"] = value;
                hidEditMode.Value = value ? "1" : "0";
                if (!value)
                {
                    lvFields.EditIndex = -1;
                    lvFields.InsertItemPosition = InsertItemPosition.None;
                }
                lvFields.DataBind();
            }
            get
            {
                //return Tools.GetBool(ViewState["editm"], false); 
                return hidEditMode.Value == "1";
            }
        }

        public ListView List
        {
            get { return lvFields; }
        }


    }
}
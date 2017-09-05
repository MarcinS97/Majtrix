using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class MPK : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(string planId)
        {
            hidPlanId.Value = planId;
            lvMPK.DataBind();
        }

        //-------------------------------------
        public static void InitItem(ListView lv, ListViewItemEventArgs e, bool create)
        {
            bool select, edit, insert;
            int lim = Tools.GetListItemMode(e, lv, out select, out edit, out insert);
            if (create)
            {
                switch (lim)
                {
                    case Tools.limSelect:
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        Tools.SetButton(e.Item, "EditButton", "Edytuj");
                        Tools.SetButton(e.Item, "DeleteButton", "Usuń");
                        //SetControlVisible(e.Item, "DeleteButton", false);
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case Tools.limEdit:
                        Button bt = (Button)Tools.SetButton(e.Item, "UpdateButton", "Zapisz");
                        if (bt != null)
                            bt.ValidationGroup = "vge";
                        Tools.SetButton(e.Item, "CancelButton", "Anuluj");
                        Tools.SetButton(e.Item, "DeleteButton", "Usuń");
                        Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
                        break;
                    case Tools.limInsert:
                        bt = (Button)Tools.SetButton(e.Item, "InsertButton", "Dodaj");
                        if (bt != null)
                            bt.ValidationGroup = "vgi";
                        Tools.SetButton(e.Item, "CancelButton", "Czyść");
                        Tools.SetControlVisible(e.Item, "CancelButton", false);
                        break;
                }
            }
            else
            {
                switch (lim)
                {
                    case Tools.limEdit:
                        ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                        DataRowView drv = (DataRowView)dataItem.DataItem;
                        Tools.SelectItem(e.Item, "ddlMPK", drv["IdMPK"]);
                        break;
                }
            }
        }

        protected void lvMPK_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvMPK, e, true);
        }

        protected void lvMPK_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            InitItem(lvMPK, e, false);
        }
        //-----------------------
        private bool UpdateItem(EventArgs ea, ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            if (oldValues == null)  // tylko jak insert, dla update nie zmieniam kierowników !!! 
            {
            }
            int? idMPK = Tools.GetDdlSelectedValueInt(item, "ddlMPK");
            if (idMPK != null)
            {
                values["IdMPK"] = idMPK;
                return true;
            }
            else
            {
                Tools.ShowMessage("Proszę podać CC.");
                return false;
            }
        }

        protected void lvMPK_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e, e.Item, null, e.Values);
        }

        protected void lvMPK_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(e, lvMPK.EditItem, e.OldValues, e.NewValues);
        }

        protected void lvMPK_DataBound(object sender, EventArgs e)
        {
            Tools.SetText(lvMPK, "lbnocneOdDo", App.GetNocneOdDo);
        }
    }
}
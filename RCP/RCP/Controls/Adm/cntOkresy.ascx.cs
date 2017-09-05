using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRRcp.App_Code;

namespace HRRcp.RCP.Controls.Adm
{
    public partial class cntOkresy : System.Web.UI.UserControl
    {
        public event EventHandler Changed;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvOkresy, Tools.ListViewMode.Bootstrap);
        }

        public string GetStatus(object status)
        {
            int st = Base.getInt(status, -999);
            return Okres.GetStatus(st);
        }

        private void TriggerChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        //-------------------
        private string Value(IOrderedDictionary values, string name)
        {
            object o = values[name];
            string v = o != null ? o.ToString() : "";
            return String.Format("{0}: {1}", name, v) + "\n";
        }

        private string Value(IOrderedDictionary oldvalues, IOrderedDictionary values, string name)
        {
            object o1 = oldvalues[name];
            object o2 = values[name];
            string v1 = o1 != null ? o1.ToString() : "";
            string v2 = o2 != null ? o2.ToString() : "";
            return Log.CheckValues(name, v1, v2, "\n");
        }
        
        private void UpdateItem(ListViewItem item, IOrderedDictionary oldValues, IOrderedDictionary values)
        {
            int status = db.getInt(Tools.GetDdlSelectedValueInt(item, "ddlStatus"), 0);
            values["Status"] = status;
            //values["Zamknal"] = Tools.GetDdlSelectedValueInt(item, "ddlZamknal");

            string o = String.Format("{0} - {1}", Base.DateToStr(values["DataOd"]), Base.DateToStr(values["DataDo"]));
            if (oldValues == null)    // create
            {
                if (status == 1)
                {
                    values["DataZamkniecia"] = DateTime.Now;
                    values["Zamknal"] = App.User.OriginalId;
                }
                string v = Value(values, "DataOd") +
                           Value(values, "DataDo") +
                           Value(values, "DataBlokady") +
                           Value(values, "StawkaNocna") +
                           Value(values, "Status") +
                           Value(values, "Zamknal") +
                           Value(values, "DataZamkniecia");
                Log.Info(Log.t2APP_OKRESPARAMS, "Dodanie nowego okresu rozliczeniowego: " + o, v, Log.OK);
            }
            else
            {
                //if (status == 1 && db.getInt(oldValues["DataZamkniecia"], 0) == 0) // za pierwszym zamknieciem mozna dac -1
                if (status == 1 && db.isNull(oldValues["DataZamkniecia"])) // za pierwszym zamknieciem mozna dac -1
                {
                    values["DataZamkniecia"] = DateTime.Now;
                    values["Zamknal"] = App.User.OriginalId;
                }
                string v = Value(oldValues, values, "DataOd") +
                           Value(oldValues, values, "DataDo") +
                           Value(oldValues, values, "DataBlokady") +
                           Value(oldValues, values, "StawkaNocna") +
                           Value(oldValues, values, "Status") +
                           Value(oldValues, values, "Zamknal") +
                           Value(oldValues, values, "DataZamkniecia");
                Log.Info(Log.t2APP_OKRESPARAMS, "Modyfikacja okresu rozliczeniowego: " + o, v, Log.OK);
            }
            TriggerChanged();
        }

        protected void lvOkresy_ItemDeleted(object sender, ListViewDeletedEventArgs e) // nie mam info o wartosciach np dat
        {
            Log.Info(Log.t2APP_OKRESPARAMS, "Usunięcie okresu rozliczeniowego", null, Log.OK);
            TriggerChanged();
        }
        //-----
        protected void lvOkresy_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem ||
                e.Item.ItemType == ListViewItemType.DataItem && ((ListViewDataItem)e.Item).DisplayIndex == lvOkresy.EditIndex)
            {
                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                Okres.FillStatus(ddl, Okres.stOpen.ToString());
            }
        }

        protected void lvOkresy_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (((ListViewDataItem)e.Item).DisplayIndex == lvOkresy.EditIndex)
                {
                    ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                    DataRowView drv = (DataRowView)dataItem.DataItem;
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlStatus");
                    Okres.FillStatus(ddl, drv["Status"].ToString());
                    Tools.SelectItem(e.Item, "ddlZamknal", drv["Zamknal"]);
                }
                Tools.MakeConfirmDeleteRecordButton(e.Item, "DeleteButton");
            }
        }

        protected void lvOkresy_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            UpdateItem(e.Item, null, e.Values);
        }

        protected void lvOkresy_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            UpdateItem(lvOkresy.EditItem, e.OldValues, e.NewValues);
        }

        protected void lvOkresy_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            string id = lvOkresy.DataKeys[e.ItemIndex].Value.ToString();
            string oid = Base.getScalar("select Id from PracownicyOkresy where IdOkresu = " + id);
            if (!String.IsNullOrEmpty(oid))
            {
                e.Cancel = true;
                Tools.ShowMessage("Istnieje struktura przypisana do wskazanego okresu.\\nNie można usunąć."); 
            }
        }
        //-----------------------------
        protected void vlDataBlokady_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Tools.DateOk(args.Value);
        }

        protected void btnAddOkresModal_Click(object sender, EventArgs e)
        {
            DataRow dr = db.Select.Row(dsNewOkres);
            string from = db.getValue(dr, "DateFrom");
            string to = db.getValue(dr, "DateTo");

            cntModal.Show();

            deFrom.Date = from;
            deTo.Date = to;

        }

        protected void btnAddOkres_Click(object sender, EventArgs e)
        {
            bool good = AddOkres(deFrom.DateStr, deTo.DateStr, deCalc.DateStr, tbStawkaNoc.Text);

            if(good)
            {
                cntModal.Close();
                lvOkresy.DataBind();
            }
            else
            {
                Tools.ShowMessage("Wystąpił błąd!");
            }

        }

        bool AddOkres(string from, string to, string calc, string stawka)
        {
            return db.Execute(dsInsert, db.nullStrParam(from), db.nullStrParam(to), db.nullStrParam(calc), db.nullParam(stawka.Replace(',', '.')));
        }

    }
}

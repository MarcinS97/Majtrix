using AjaxControlToolkit;
using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
  Scheduler (?mode="SCHEDULER"&p=id_sch) 
 * -> co jest do wykonania w RaportyScheduler (GETDATE() >= NextStart), zwraca dsMails z wypełnionymi danymi raportów
 * -> zapis RaportyWysylki
 * -> raport -> csv 
 * -> wysyłka
 */

namespace HRRcp.Controls.Reports
{
    public partial class cntReportScheduler : System.Web.UI.UserControl
    {
        const int moRaport  = 0;
        const int moAll     = 1;
        int FMode = moRaport;

        public const int typLink    = 0;    // jako link do raportu
        public const int typCSV     = 1;    // jako wygenerowany plik CSV
        public const int typPDF     = 2;    // jako wygenerowany plik PDF  <<< oprogramować !


        public const string userALL = "all";
        public const string repAll  = "-99";

        public const string ivOne = "1";    // jednorazowo
        public const string ivHH = "HH";
        public const string ivDD = "DD";
        public const string ivWW = "WW";
        public const string ivMM = "MM";
        public const string ivLM = "LM";    // ostatni dzień miesiąca

        protected void Page_Init(object sender, EventArgs e)
        {
            //Tools.PrepareDicListView(lvScheduler, Tools.dicBS);
            Tools.PrepareDicListView(lvScheduler, 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool PrepareAdm()
        {
            return Prepare(userALL, repAll);
        }

        public bool Prepare(string userId, string repId)
        {
            UserId = userId;
            ReportId = repId;
            //if (repId != repAll)
            //    ReportName = db.getScalar(String.Format("select MenuText from "));
            
            
            //lvScheduler.DataBind();
            
            
            return lvScheduler.Items.Count > 0;
        }
        //---------------------
        //public static bool IsIntervalVisible(string typ)
        //{
        //    return !(String.IsNullOrEmpty(typ) || typ.IsAny(ivOne, ivLM));
        //}

        public static bool IsIntervalVisible(object typ)
        {
            return !(db.isNull(typ) || typ.ToString().IsAny(ivOne, ivLM));
        }
        //-----
        protected void lvScheduler_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.InsertItem)
            {
                bool r = ReportId == repAll;
                Tools.SetControlVisible(e.Item, "tdRaport", r);
                
                bool u = UserId == userALL;
                Tools.SetControlVisible(e.Item, "tdUser", u);

                if (!r)
                {
                    Tools.SetText2(e.Item, "EMailTextBox", App.User.EMail);
                    Tools.SetText2(e.Item, "lbEmail", App.User.EMail);
                }
                Tools.SetChecked(e.Item, "AktywnyCheckBox", true);

                /*
                Tools.SetControlVisible(e.Item, "ddlReport", r);
                Label lb = Tools.SetControlVisible(e.Item, "lbReport", !r) as Label;
                if (!r && lb != null)
                    lb.Text = 

                Tools.SetControlVisible(e.Item, "ddlUser", u);
                Tools.SetControlVisible(e.Item, "lbUser", !u);
                */
            }
        }

        protected void lvScheduler_DataBound(object sender, EventArgs e)
        {
            bool r = ReportId == repAll;
            Tools.SetControlVisible(lvScheduler, "thRaport", r);
            bool u = UserId == userALL;
            Tools.SetControlVisible(lvScheduler, "thUser", u);
        }

        protected void lvScheduler_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                bool r = ReportId == repAll;
                bool u = UserId == userALL;
                Tools.SetControlVisible(e.Item, "tdRaport", r);
                Tools.SetControlVisible(e.Item, "tdUser", u);
                if (((ListViewDataItem)e.Item).DataItemIndex == lvScheduler.EditIndex)
                {
                    DataRowView drv = Tools.GetDataRowView(e);
                    if (r) Tools.SelectItem(e.Item, "ddlRaport", drv["IdRaportu"]);
                    if (u) Tools.SelectItemByParam(e.Item, "ddlUser", 0, drv["UserId"]);
                    Tools.SelectItem(e.Item, "ddlInterwalTyp", drv["InterwalTyp"]);
                }
            }
        }
        //-----
        private bool UpdateItem(Control item, IOrderedDictionary values, bool insert)
        {
            bool r = ReportId == repAll;
            bool u = UserId == userALL;
            values["IdRaportu"] = r ? Tools.GetDdlSelectedValue(item, "ddlRaport", 0) : ReportId;
            values["UserId"]    = u ? Tools.GetDdlSelectedValue(item, "ddlUser", 0) : UserId;
            values["InterwalTyp"] = Tools.GetDdlSelectedValue(item, "ddlInterwalTyp");
            return true;
        }

        protected void lvScheduler_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, e.Values, true);
        }

        protected void lvScheduler_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvScheduler.EditItem, e.NewValues, false);
        }
        //-----
        private void ddlUser_SelectedIndexChanged(object sender, Control item)
        {
            string[] p = Tools.GetLineParams(((DropDownList)sender).SelectedValue);
            string mail = p.Length > 1 ? p[1] : null;
            Tools.SetText2(item, "EMailTextBox", mail);
            Tools.SetText2(item, "lbEmail", mail);
        }

        protected void ddlInterwalTyp_SelectedIndexChanged(object sender, Control item)
        {
            string sel = ((DropDownList)sender).SelectedValue;
            //bool show = !(String.IsNullOrEmpty(sel) || sel.IsAny(ivOne, ivLM));
            bool show = IsIntervalVisible(sel);
            Tools.SetControlEnabled(item, "ftbInterwal", show);
            Tools.SetControlEnabled(item, "rfvInterwal", show);
            TextBox tb = Tools.SetControlVisible(item, "InterwalTextBox", show) as TextBox;
            if (show) tb.Focus();
        }
        
        protected void ddlUserI_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUser_SelectedIndexChanged(sender, lvScheduler.InsertItem);
        }

        protected void ddlInterwalTypI_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlInterwalTyp_SelectedIndexChanged(sender, lvScheduler.InsertItem);
        }
        //-----
        protected void ddlUserE_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUser_SelectedIndexChanged(sender, lvScheduler.EditItem);
        }

        protected void ddlInterwalTypE_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlInterwalTyp_SelectedIndexChanged(sender, lvScheduler.EditItem);
        }
        //---------------------
        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }

        public string UserId
        {
            set { hidUserId.Value = value; }
            get { return hidUserId.Value; }
        }

        public string ReportId
        {
            set { hidReportId.Value = value; }
            get { return hidReportId.Value; }
        }

        public string ReportName
        {
            set { ViewState["repname"] = value; }
            get { return Tools.GetStr(ViewState["repname"]); }
        }


    }
}
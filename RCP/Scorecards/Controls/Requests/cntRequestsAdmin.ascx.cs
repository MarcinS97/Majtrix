using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.Collections.Specialized;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntRequestsAdmin : System.Web.UI.UserControl
    {
        public event EventHandler Changed;
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvRequests, 0);
            Tools.PrepareSorting2(lvRequests, 1, 15);
        }

        protected void lvRequests_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DataRowView drv = null;
            int li = Tools.GetListItemMode(e, lvRequests, out drv);
            if (li == Tools.limEdit && drv != null)
            {
                Int32 ScorecardTypeId = (Int32)drv["IdTypuArkuszy"];
                String Type = ScorecardTypeId > 0 ? "0" : "1";
                Boolean Custom = Type == "1";

                Tools.SetControlEnabled(e.Item, "tbName", Custom);
                Tools.SetControlEnabled(e.Item, "ddlSpreadsheets", !Custom);

                //RadioButtonList rbl = e.Item.FindControl("rblType") as RadioButtonList;
                //rbl.SelectedValue = ScorecardTypeId > 0 ? "" : "0";

                Tools.SelectItem(e.Item, "rblType", Type);
                Tools.SelectItem(e.Item, "ddlOwner", drv["IdPracownika"]);
                Tools.SelectItem(e.Item, "ddlStatus", drv["Status"]);
                Tools.SelectItem(e.Item, "ddlKacc", drv["Kacc"]);
                Tools.SelectItem(e.Item, "ddlPacc", drv["Pacc"]);
            }
        }

        protected void lvRequests_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            int li = Tools.GetListItemMode(e, lvRequests);
            if (li == Tools.limInsert)
            {
                //Tools.SetChecked(e.Item, "cbActive", true);
            }
        }

        protected void lvRequests_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            e.Cancel = !UpdateItem(lvRequests.EditItem, e.OldValues, e.NewValues, e);
        }

        protected void lvRequests_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Cancel = !UpdateItem(e.Item, null, e.Values, e);
        }

        private bool UpdateItem(ListViewItem Item, IOrderedDictionary OldValues, IOrderedDictionary Values, EventArgs e)
        {
            RadioButtonList rbl = Item.FindControl("rblType") as RadioButtonList;

            switch(rbl.SelectedValue)
            {
                case "0":
                    Values["IdTypuArkuszy"] = Tools.GetDdlSelectedValueInt(Item, "ddlSpreadsheets");
                    break;
                case "1":
                    Values["IdTypuArkuszy"] = -1337;
                    break;
                default:
                    break;
            }

            Values["IdPracownika"] = Tools.GetDdlSelectedValueInt(Item, "ddlOwner");
            Values["Status"] = Tools.GetDdlSelectedValueInt(Item, "ddlStatus");
            Values["Kacc"] = Tools.GetDdlSelectedValueInt(Item, "ddlKacc");
            Values["Pacc"] = Tools.GetDdlSelectedValueInt(Item, "ddlPacc");

            return true;
        }


        protected String GetStatus(Int32 Status, Int32 Kacc, Int32 Pacc)
        {
            return cntRequests.GetStatus(Status, Kacc, Pacc);
        }

        protected String GetAcc(Int32 Acc)
        {
            switch (Acc)
            {
                case -1: return "Do akceptacji";
                case 0: return "Odrzucony";   // czy cofnięty ?
                case 1: return "Zaakceptowany";
                default: return "Nieznany";
            }
        }

        protected void ShowRequest(object sender, EventArgs e)
        {
            Button btnShowRequest = (sender as Button);
            String Arg = btnShowRequest.CommandArgument;
            String RequestId = Arg.Split(';')[0];
            String Status = Arg.Split(';')[1];
            String Type = Arg.Split(';')[2];
            String Genre = Arg.Split(';')[3];

            ShowRequest(RequestId, App.User.Id, Status, true, Type == "1", Genre == "1");
        }

        public void ShowRequest(String RequestId, String ObserverId, String Status, Boolean Editable, Boolean Custom, Boolean Genre)
        {
            Request.Visible = true;
            Request.Prepare(RequestId, ObserverId, "99", "-1", Editable, Custom, Genre && !Custom);
            Tools.MakeDialogCloseButton(Request.SaveButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.SendButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.RejectButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.CloseButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.DestroyButton, "divZoom");
            if (Custom) Tools.MakeDialogCloseButton(Request.DeleteButton, "divZoom");
            int width = (Custom) ? 900 : 1400;
            Tools.ShowDialog("divZoom", upMain.ClientID, "Wniosek Premiowy", width, null);
        }

        protected void Request_SomethingChanged(object sender, EventArgs e)
        {
            upMain.Update();
            lvRequests.DataBind();
            if (Changed != null) Changed(null, EventArgs.Empty);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Web.UI.HtmlControls;

namespace HRRcp.Scorecards.Controls.Requests
{
    public partial class cntRequests : System.Web.UI.UserControl
    {
        public event EventHandler ObserverChanged;
        public event EventHandler Changed;

        const String uprTL = "0";
        const String uprKier = "1";
        const String uprPrezes = "2";

        const String moMine = "0";
        const String moToAcc = "1";
        const String moAccepted = "2";
        const String moDoWyjasnienia = "3";
        const String moRejected = "4";

        const String moAll = "99";

        public Boolean GetRight(String UserId, String RightId)
        {
            return (db.Select.Scalar("select dbo.GetRight({0}, {1})", UserId, RightId) == "1");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ObserverId = App.User.Id;
                Upr = GetUpr();
                btnNewRequest.Visible = (Mode == moMine);
                lvRequests.DataBind();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Tools.PrepareDicListView(lvRequests, 0);
            Tools.PrepareSorting2(lvRequests, 1, 10);
        }

        //public void Prepare(String EmployeeId)
        //{
        //    Upr = GetUpr();   
        //    lvRequests.DataBind();
        //}

        public int Prepare(String ObserverId, String Upr)
        {
            this.ObserverId = ObserverId;
            this.Upr = String.IsNullOrEmpty(Upr) ? GetUpr() : Upr;
            lvRequests.DataBind();
            return lvRequests.Items.Count;
        }

        public void Prepare()
        {
            lvRequests.DataBind();
        }

        public void HideRequest()
        {
            Request.Visible = false;
        }

        public  static String GetStatus(Int32 Status, Int32 Kacc, Int32 Pacc)
        {
            String Output = String.Empty;
            if (Status == 0 && Kacc == -1 && Pacc == -1) Output = "Nowy";
            else if (Status == 1 && Kacc == -1 && Pacc == -1) Output = "Do zaakceptowania przez Kierownika";
            else if (Status == 0 && Kacc == 0 && Pacc == -1) Output = "Cofnięty przez Kierownika";
            else if (Status == 2 && Kacc == 1 && Pacc == -1) Output = "Do zaakceptowania przez Zarząd";
            else if (Status == 1 && Pacc == 0) Output = "Cofnięty przez Zarząd";
            else if (Status == 3 && Kacc == 1 && Pacc == 1) Output = "Zaakceptowany przez Zarząd";
            else if (Status == -1) Output = "Odrzucony"; //"Odrzucony permanentnie";
            else Output = String.Format("Inny: {0}, {1}, {2}", Status, Kacc, Pacc);
            return Output;
        }

        protected void ShowRequest(object sender, EventArgs e)
        {
            Button btnShowRequest = (sender as Button);
            String Arg = btnShowRequest.CommandArgument;
            String RequestId = Arg.Split(';')[0];
            String Status = Arg.Split(';')[1];
            String Type = Arg.Split(';')[2];
            String Genre = Arg.Split(';')[3];

            ShowRequest(RequestId, ObserverId, Status, (Mode == moMine && (Status == "0" || (Status == "1" && Upr == uprKier))) || (Mode == moToAcc), Type == "1", Genre == "1");
        }

        protected void ShowNewRequest(object sender, EventArgs e)
        {
            bool isEmpty = db.Select.Scalar(dsIsEmpty, ObserverId) == "1";
            if (isEmpty)
            {
                Tools.ShowMessage("Istnieje już pusty wniosek do premii uznaniowej!");
            }
            else
            {
                String RequestId = db.insert(String.Format(dsCreateRequest.SelectCommand, ObserverId), true, true).ToString();
                lvRequests.DataBind();
                ShowRequest(RequestId, ObserverId, "0", true, true, false);
            }
        }

        public void ShowRequest(String RequestId, String ObserverId, String Status, Boolean Editable, Boolean Custom, Boolean Genre)
        {
            Request.Visible = true;
            Request.Prepare(RequestId, ObserverId, Mode, Upr, Editable, Custom, Genre && !Custom);
            //Tools.MakeDialogCloseButton(Request.SaveButton, "divZoom");
            //Tools.MakeDialogCloseButton(Request.SendButton, "divZoom");
            //Tools.MakeDialogCloseButton(Request.RejectButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.CloseButton, "divZoom");
            Tools.MakeDialogCloseButton(Request.DestroyButton, "divZoom");
            if (Custom) Tools.MakeDialogCloseButton(Request.DeleteButton, "divZoom");
            int width = (Custom) ? 900 : 1400;
            Tools.ShowDialog("divZoom", upMain.ClientID, "Wniosek Premiowy", width, null);
        }
        
        public String GetUpr()
        {
            /*if(App.User.IsScAdmin) return "-1";
            else*/ if(/*App.User.HasRight(AppUser.rScorecardsPrez))*/GetRight(ObserverId, "59")) return "2";
            else if(/*App.User.HasRight(AppUser.rScorecardsKier))*/GetRight(ObserverId, "58")) return "1";
            else if(/*App.User.IsScTL*/GetRight(ObserverId, "57") || GetRight(ObserverId, "65")) return "0";
            else return "-1337";
        }

        #region DEBUG
        protected void DebugClick(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "TL":
                    Upr = uprTL;
                    btnTL.Enabled = false;
                    btnPrez.Enabled = btnKier.Enabled = true;
                    break;
                case "KIER":
                    Upr = uprKier;
                    btnKier.Enabled = false;
                    btnPrez.Enabled = btnTL.Enabled = true;
                    break;
                case "PREZ":
                    Upr = uprPrezes;
                    btnPrez.Enabled = false;
                    btnKier.Enabled = btnTL.Enabled = true;
                    break;
            }
            upMain.Update();
            if (ObserverChanged != null) ObserverChanged(Upr, EventArgs.Empty);
            lvRequests.DataBind();
        }
        #endregion

        public String GetColorClass(String Default, Int32 Key)
        {
            String Class = Default;
            switch (Key)
            {
                //case -1: return Class + " jail";
                case 0: return Class + " rej";
                case 1: return Class + " acc";
                case 2: return Class + " hr";
            }
            return Class;
        }

        protected void Request_SomethingChanged(object sender, EventArgs e)
        {
            upMain.Update();
            lvRequests.DataBind();
            if (Changed != null) Changed(null, EventArgs.Empty);
        }

        //protected void Request_Closed(object sender, EventArgs e)
        //{
        //    Request.Visible = false;
        //    if (Changed != null) Changed(null, EventArgs.Empty);
        //}

        protected void lvRequests_DataBound(object sender, EventArgs e)
        {
            HtmlTableCell Cell = lvRequests.FindControl("th8") as HtmlTableCell;
            if (Cell != null) Cell.Visible = !StartForm;
        }

        public Boolean IsStartForm()
        {
            return StartForm;
        }

        public Boolean StartForm
        {
            get { return Tools.GetViewStateBool(ViewState["vStartForm"], false); }
            set { ViewState["vStartForm"] = value; }
        }

        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public String Mode
        {
            get { return hidMode.Value; }
            set { hidMode.Value = value; }
        }

        public String Upr
        {
            get { return hidUpr.Value; }
            set { hidUpr.Value = value; }
        }
    }
}
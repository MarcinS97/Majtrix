using HRRcp.App_Code;
using HRRcp.MatrycaSzkolen.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.MatrycaSzkolen.Controls
{
    public partial class cntAkceptacjeSzk : System.Web.UI.UserControl
    {
        public const string cmdYes = "Yes";
        public const string cmdNo = "No";

        public const string tabRejected = "-1";
        //public const string tabInPreparation = "0";
        //public const string tabToTrain = "1";
        public const string tabInProgress = "1";
        public const string tabTrained = "2";
        public const string tabTrainedAcc = "3";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SelectedTab = Tabs.SelectedValue;
            }
        }

        protected void Button_Command(object sender, CommandEventArgs e)
        {
            string id = (sender as Button).CommandArgument;

            if (!String.IsNullOrEmpty(id))
            {
                switch (e.CommandName)
                {
                    case cmdYes:
                        btnYesConfirm.CommandArgument = id;
                        Tools.ShowConfirm("Czy na pewno chcesz zaakceptować?", btnYesConfirm);
                        break;
                    case cmdNo:
                        btnNoConfirm.CommandArgument = id;
                        string text = (SelectedTab == tabTrainedAcc) ? "Czy na pewno chcesz odrzucić? Wiąże się to z usunięciem ankiet ewaluacyjnych." : "Czy na pewno chcesz odrzucić?";
                        Tools.ShowConfirm(text, btnNoConfirm);
                        break;
                    default:

                        break;

                }
            }
        }

        protected void Tabs_SelectTab(object sender, EventArgs e)
        {
            SelectedTab = Tabs.SelectedValue;
        }

        public String SelectedTab
        {
            get { return hidSelectedTab.Value; }
            set { hidSelectedTab.Value = value; }
        }

        protected void btnYesConfirm_Click(object sender, EventArgs e)
        {
            string id = (sender as Button).CommandArgument;
            string status = null;
            if (!String.IsNullOrEmpty(id))
            {
                switch (SelectedTab)
                {
                    //case tabInPreparation:
                    //    status = "1";
                    //    break;
                    case tabRejected:
                        status = "1";
                        UpdateStatus(id, status);
                        break;
                    //case tabToTrain:
                    //    status = "2";
                    //    break;
                    case tabInProgress:
                        status = "2";
                        UpdateStatus(id, status);
                        break;
                    case tabTrained:
                        status = "3";
                        if (UpdateStatus(id, status))
                            AcceptTraining(id);
                        break;
                    //case tabTrainedAcc:
                    //    status = "4";
                    //    break;
                }
            }

        }

        bool UpdateStatus(string id, string status)
        {
            if (!String.IsNullOrEmpty(status))
            {
                db.Execute(dsAccept, status, id);

                Tabs.Reload();
                gvList.DataBind();
                //Tabs.DataBind();
                //gvList.DataBind();
                //UpdatePanel up = Tools.FindUpdatePanel(this);
                //if (up != null && up.UpdateMode == UpdatePanelUpdateMode.Conditional)
                //    up.Update();
                return true;
            }
            return false;
        }

        // ankieta prac - 0, ankieta kier - 1, ankieta kier2 - 2

        void AcceptTraining(string id)
        {
            db.Execute(dsAcceptTraining, id, db.strParam(DateTime.Now.ToString()), App.User.Id, App.User.OriginalId);

            DataTable dt = db.Select.Table(dsSurveys, id);

            foreach (DataRow dr in dt.Rows)
            {
                string typ = db.getValue(dr, "Typ");
                string ankietaId = db.getValue(dr, "Id");

                if (typ == "0") // prac
                {
                    Ewaluacja.cntEwaluacja.AnkietPracMail(ankietaId);
                }

                if (typ == "1") // kier
                {
                    Ewaluacja.cntEwaluacja.AnkietaKierMail(ankietaId);
                }





            }


            Tabs.Reload();
            gvList.DataBind();

        }

        protected void btnNoConfirm_Click(object sender, EventArgs e)
        {
            string id = (sender as Button).CommandArgument;
            string status = null;
            if (!String.IsNullOrEmpty(id))
            {
                //switch (SelectedTab)
                //{
                //    case tabRejected:
                //        //status = "0";
                //        break;
                //    case tabToTrain:
                //        status = "1";
                //        break;
                //    case tabTrained:
                //        status = "2";
                //        break;
                //    case tabTrainedAcc:
                //        status = "3";
                //        break;
                //}
                db.Execute(dsAccept, "-1", id);

                if (SelectedTab == tabTrainedAcc)
                {
                    db.Execute(dsRemoveSurveys, id);
                }

                Tabs.Reload();
                gvList.DataBind();

                //Tabs.Tabs.DataBind();
                //gvList.DataBind();
                //UpdatePanel up = Tools.FindUpdatePanel(this);
                //if (up != null && up.UpdateMode == UpdatePanelUpdateMode.Conditional)
                //    up.Update();
            }

        }

        protected void Tabs_DataBound(object sender, EventArgs e)
        {
            if (Request.QueryString.Count == 0)
            {
                Tabs.Tabs.Items[0].Selected = true;
                SelectedTab = Tabs.Tabs.SelectedValue;
                /*SelectView();*/
            }
            else if (Request.QueryString["p"] == "1")
            {
                Tabs.Tabs.Items[0].Selected = false;
                /*SelectedTab = Request.QueryString["p"];*/
                SelectedTab = "2";
                Tabs.Tabs.Items[1].Selected = true;
                /*SelectView();*/
            }
        }
    }
}
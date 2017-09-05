using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using HRRcp.Scorecards.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntSpreadsheet : System.Web.UI.UserControl
    {
        private const String _OutsideJob = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateChanger.DataBind();
                EmployeeSelect.Date = GetSelectedDate();
                EmployeeSelect.PrepareOnStart();
                PrepareEmployeeChanger();
                PrepareScorecard();
            }
        }

        public void PrepareEmployeeChanger()
        {
            Boolean Left = EmployeeSelect.PreviousEmployeeAvailable();
            Boolean Right = EmployeeSelect.NextEmployeeAvailable();
            btnPreviousEmployee.Enabled = Left;
            btnNextEmployee.Enabled = Right;
            EmployeeChanger.Visible = (Left || Right) && !String.IsNullOrEmpty(EmployeeId);
            if(!String.IsNullOrEmpty(EmployeeSelect.EmployeeId)) lblCurrentEmployee.Text = db.getScalar(String.Format(@"select Nazwisko + ' ' + Imie from Pracownicy where Id = {0}", EmployeeSelect.EmployeeId));
        }

        protected void EmployeeChanged(object sender, EventArgs e)
        {
            PrepareEmployeeChanger();
            PrepareScorecard();
            EmployeeChanger.Visible = true;
        }

        protected void EmployeeChangedLeft(object sender, EventArgs e)
        {
            EmployeeSelect.SelectPreviousEmployee();
            PrepareEmployeeChanger();
            PrepareScorecard();
        }

        protected void EmployeeChangedRight(object sender, EventArgs e)
        {
            EmployeeSelect.SelectNextEmployee();
            PrepareEmployeeChanger();
            PrepareScorecard();
        }

        protected void EmployeeChangedTop(object sender, EventArgs e)
        {
            PrepareEmployeeChanger();
            PrepareScorecard();
        }

        protected void TeamChanged(object sender, EventArgs e)
        {
            PrepareScorecard();
            EmployeeChanger.Visible = false;
        }

        protected void SuperiorChanged(object sender, EventArgs e)
        {
            EmployeeSelect.Prepare(true);
            PrepareEmployeeChanger();
            PrepareScorecard();
        }

        protected void DateChanged(object sender, EventArgs e)
        {
            EmployeeSelect.Date = GetSelectedDate();
            int Selected = EmployeeSelect.PrepareAndCheck();
            if (Selected == 0)
            {
                EmployeeChanger.Visible = true;
                PrepareEmployeeChanger();
                PrepareScorecard();
            }
            else if (Selected == 1)
            {
                PrepareScorecard();
            }
            else if (Selected == -1) // jak nie ma niczego
            {
                PrepareEmployeeChanger();
                lblTitle.Text = String.Empty;
                HideScorecard();
            }
            else
            {
                //EmployeeChanger.Visible = true;
                //PrepareEmployeeChanger();
                PrepareScorecard();
                //EmployeeChanger.Visible = false;
            }
        }

        protected void PrepareScorecard() // TRZEBA TO BYŁO PRZEPISAC DO CHOLERY JASNEJ
        {
            this.ScorecardTypeId = EmployeeSelect.ScorecardTypeId;
            this.EmployeeId = EmployeeSelect.EmployeeId;
            this.Date = GetSelectedDate();

            String OwnerId = EmployeeSelect.OwnerId;
            // jak cofamy
            RestoreSession();

            Boolean Ok = /*!String.IsNullOrEmpty(EmployeeId) &&*/ !String.IsNullOrEmpty(ScorecardTypeId) && !String.IsNullOrEmpty(Date) && !String.IsNullOrEmpty(OwnerId);
            if (Ok)
            {
                Boolean Accepted = IsAccepted(ScorecardTypeId, Date, EmployeeId);
                String ObserverId = EmployeeSelect.ObserverId;

                DataRow HeaderRow = db.getDataRow(String.Format("select s.Id, ta.Nazwa/*, Nazwa2*/, case when ta.Wniosek = 1 then 1 else 0 end as Wniosek from scTypyArkuszy ta left join scSlowniki s on ta.Rodzaj = s.Id where ta.Id = {0} and s.Typ = '{1}' and s.Aktywny = 1", ScorecardTypeId, Dictionaries.Types.Spreadsheets));
                String Genre = db.getValue(HeaderRow, "Id", String.Empty);
                String ScorecardTypeName = db.getValue(HeaderRow, "Nazwa", String.Empty);
                String Request = db.getValue(HeaderRow, "Wniosek", String.Empty);
                String Status = db.Select.Scalar("select Status from OkresyRozl where DataOd = {0}", db.strParam(Date));
                String UnAcc = UnAccepted(ScorecardTypeId, Date, EmployeeId);
                // okres próbny
                //EmployeeSelect.AcceptButton.Visible = Request == "1";

                if (Accepted)
                {
                    Scorecard.InEdit = false;
                    EmployeeSelect.ToggleItems(false);
                    EmployeeSelect.UnAcceptButton.Visible = !(EmployeeId == ObserverId) && (Request == "1") && (GetTL(EmployeeId) == "1") && (UnAcc == "0" || UnAcc == "1") && App.User.IsScWnRej;
                }
                else
                {
                    Scorecard.InEdit = true;
                    EmployeeSelect.ToggleItems(true);
                    EmployeeSelect.AcceptButton.Visible = !(EmployeeId == ObserverId) && (Status == "1") && (Request == "1");
                    EmployeeSelect.AcceptAllButton.Visible = !(EmployeeId == ObserverId) && (Status == "1") && (Request == "1") && Genre != Dictionaries.Spreadsheets.Team;
                    EmployeeSelect.UnAcceptButton.Visible = false;
                }


                // PLOMBA
                Scorecard.Visible = true;
                divTasksEmpty.Visible = false;
                imgTitle.Visible = true;
                Scorecard.Prepare(EmployeeId, ScorecardTypeId, Date, _OutsideJob, Genre, ObserverId, Accepted, OwnerId);
                PrepareTitle(ScorecardTypeName);
            }
            else
            {
                HideScorecard();
            }
        }

        protected void RestoreSession()
        {
            if (Session["scEmployeeId"] != null && Session["scScorecardTypeId"] != null && Session["scDate"] != null)
            {
                EmployeeId = Tools.GetStr(Session["scEmployeeId"], String.Empty);
                ScorecardTypeId = Tools.GetStr(Session["scScorecardTypeId"], String.Empty);
                Date = Tools.GetStr(Session["scDate"], String.Empty);
                
                // POWAŻNA ZMIANA
                //EmployeeSelect.ObserverId = Tools.GetStr(Session["scObserverId"]);
                EmployeeSelect.ObserverId = App.User.Id;
                EmployeeSelect.OwnerId = Tools.GetStr(Session["scObserverId"]);
                
                if (EmployeeId != null)
                    EmployeeSelect.EmployeeId = (EmployeeId.StartsWith("-")) ? null : EmployeeId;
                else
                    EmployeeSelect.EmployeeId = null;
                EmployeeSelect.ScorecardTypeId = ScorecardTypeId;
                EmployeeSelect.Prepare(false);
                PrepareEmployeeChanger();

                DateTime dt = Convert.ToDateTime(Date);
                DateChanger.Rok = dt.Year;
                DateChanger.Miesiac = dt.Month;

                // TUTAJ JAK SESJA NIE ZADZIALA 
                Session["scEmployeeId"] = Session["scScorecardTypeId"] = Session["scDate"] = null;
            }
        }


        Boolean IsAccepted(String ScorecardTypeId, String Date, String EmployeeId)
        {
            String Acc = db.getScalar(String.Format(@"select COUNT(*) as Locked from scPremie p left join scWnioski w on p.IdWniosku = w.Id where w.IdTypuArkuszy = {0} and w.Data = {1} and p.IdPracownika = {2}", ScorecardTypeId, db.strParam(Date), String.IsNullOrEmpty(EmployeeId) ? "-" + ScorecardTypeId : EmployeeId));
            return Acc != "0";
        }

        String UnAccepted(String ScorecardTypeId, String Date, String EmployeeId)
        {
            String UnAccepted = db.getScalar(String.Format(@"select Status from scWnioski where IdTypuArkuszy = {0} and Data = {1} and IdPracownika = {2}", ScorecardTypeId, db.strParam(Date), String.IsNullOrEmpty(EmployeeId) ? "-" + ScorecardTypeId : EmployeeId));
            return UnAccepted;
        }

        protected void UnAccept(object sender, EventArgs e)
        {
            Scorecard.UnAccept();
        }

        protected void PrepareTitle(String ScorecardTypeName)
        {
            lblTitle.Text = (String.IsNullOrEmpty(ScorecardTypeName)) ? "Scorecard" : "Scorecard - " + ScorecardTypeName;
        }

        protected void SaveScorecard(object sender, EventArgs e)
        {
            Scorecard.SaveAndCheckPGIO();
        }

        protected void AcceptScorecard(object sender, EventArgs e)
        {
            Scorecard.Accept();
        }

        protected void AcceptAllScorecards(object sender, EventArgs e)
        {
            Scorecard.AcceptAll();
        }

        protected void Accepted(object sender, EventArgs e)
        {
            PrepareScorecard();
        }

        protected void TasksEmpty(object sender, EventArgs e)
        {
            divTasksEmpty.Visible = true;
            Scorecard.Visible = false;
        }

        protected String GetTL(String Who)
        {
            try
            {
                return db.Select.Scalar(dsTeamLeader, db.strParam(Date), Who/*EmployeeId*//*ObserverId <- actual*/, ScorecardTypeId);
            }
            catch
            {
                return "0";
            }
        }

        void HideScorecard()
        {
            Scorecard.Visible = false;
            imgTitle.Visible = false;
            EmployeeSelect.ToggleItems(false);
        }

        public String GetSelectedDate()
        {
            return String.Format(@"{0}-{1}-01", DateChanger.Rok, DateChanger.Miesiac);
        }

        public String ScorecardTypeId
        {
            get { return ViewState["vScorecardTypeId"] as String; }
            set { ViewState["vScorecardTypeId"] = value; }
        }

        public String EmployeeId
        {
            get { return ViewState["vEmployeeId"] as String; }
            set { ViewState["vEmployeeId"] = value; }
        }

        public String Date
        {
            get { return ViewState["vDate"] as String; }
            set { ViewState["vDate"] = value; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntEmployeeSelect2 : System.Web.UI.UserControl
    {
        public event EventHandler EmployeeChanged;
        public event EventHandler SuperiorChanged;
        public event EventHandler TeamChanged;
        public event EventHandler Saved;
        public event EventHandler Accepted;
        public event EventHandler AcceptAlled;
        public event EventHandler UnAccepted;
        //public event EventHandler ChangedToMe;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public void PrepareOnStart()
        {
            if (String.IsNullOrEmpty(ObserverId)) ObserverId = App.User.Id;
            showSuperiors.Visible = rpSuperiors.Visible = App.User.IsScAdmin;
            Prepare(true);
            ShowHideMine();
        }

        public void Prepare(Boolean Select)
        {
            rpSuperiors.DataBind();
            rpEmployees.DataBind();
            rpTeams.DataBind();

            showList.Visible = rpEmployees.Items.Count != 0;
            showGroups.Visible = rpTeams.Items.Count != 0;
            //save.Visible = accept.Visible = true;

            if (Select)
            {
                /*if (App.User.IsScAdmin && rpSuperiors.Items.Count > 0 && ObserverId == App.User.Id) SelectFirstSuperior();
                else*/
                if (rpEmployees.Items.Count > 0) SelectFirstEmployee();
                else if (rpTeams.Items.Count > 0) SelectFirstTeam();
            }
        }

        public int PrepareAndCheck()
        {
            rpSuperiors.DataBind();
            rpEmployees.DataBind();
            rpTeams.DataBind();

            showList.Visible = rpEmployees.Items.Count != 0;
            showGroups.Visible = rpTeams.Items.Count != 0;
            //save.Visible = accept.Visible = true;

            if (IsSelectNeeded() && !String.IsNullOrEmpty(EmployeeId) && !(EmployeeId == ObserverId))
            {
                /*if (App.User.IsScAdmin && rpSuperiors.Items.Count > 0 && ObserverId == App.User.Id) SelectFirstSuperior();
                else*/
                if (rpEmployees.Items.Count > 0)
                {
                    SelectFirstEmployee();
                    return 0; // Employee
                }
                else if (rpTeams.Items.Count > 0)
                {
                    SelectFirstTeam();
                    return 1;
                }
                else return -1;
                //return 1337;
            }// UWAGA UWAGA
            else
            {
                try
                {
                    this.OwnerId = db.Select.Scalar("select IdKierownika from Przypisania where ((IdPracownika = {0} and IdPracownika != -1337) or (IdCommodity = {2} and {0} = -1337)) and Status = 1 and dbo.eom({1}) between Od and isnull(Do, '20990909') ",
                        (String.IsNullOrEmpty(EmployeeId) ? (-1337).ToString() : EmployeeId), db.strParam(Date), String.IsNullOrEmpty(ScorecardTypeId) ? (-1).ToString() : ScorecardTypeId);
                }
                catch { this.OwnerId = null; }
            }
            return 1337;
        }

        public Boolean IsSelectNeeded()
        {
            for (int i = 0; i < rpEmployees.Items.Count; i++)
            {
                LinkButton lnkEmployee = rpEmployees.Items[i].FindControl("lnkEmployee") as LinkButton;
                String toSplit = lnkEmployee.CommandArgument;
                String scorecardTypeId = toSplit.Split(';')[0];
                String employeeId = toSplit.Split(';')[1];
                if (employeeId == EmployeeId && scorecardTypeId == ScorecardTypeId) return false;
            }
            return true;
        }

        public void ToggleItems(Boolean b)
        {
            save.Visible = accept.Visible = acceptall.Visible = b;
        }

        public void ShowHideMine()
        {
            String WPI = null;
            if (!String.IsNullOrEmpty(ScorecardTypeId) && !String.IsNullOrEmpty(EmployeeId)) WPI = db.getScalar(String.Format(@"
--select Parametr from scParametry where Typ = 'WPI' and IdTypuArkuszy = {0} and TL = 1 and GETDATE() between Od and ISNULL(Do, '20990909')
declare @date datetime = {2}
declare @pracId int = {1}
declare @typark int = {0}

declare @WPI float

select Parametr from /*Przypisania p
left join*/ scParametry par /*on p.IdCommodity = par.IdTypuArkuszy and @date between par.Od and ISNULL(par.Do, '20990909')*/
where /*p.IdKierownika = @pracId and @date between p.Od and ISNULL(p.Do, '20990909') and p.Status = 1 and*/ par.Typ = 'WPI' and par.TL = 1 and par.IdTypuArkuszy = @typark and par.Parametr2 = @pracId and @date between par.Od and ISNULL(par.Do, '20990909')
", ScorecardTypeId, ObserverId, db.strParam(Date)));
            else
            {
                showMine.Visible = false;
                return;
            }
            showMine.Visible = (!String.IsNullOrEmpty(WPI) && WPI != "0");
        }

        protected void SelectFirstSuperior()
        {
            LinkButton lnkEmployee = rpSuperiors.Items[0].FindControl("lnkSuperior") as LinkButton;
            ObserverId = lnkEmployee.CommandArgument;
            Prepare(false);
            // tak musi być żeby nie było stackoverflowa
            if (rpEmployees.Items.Count > 0) SelectFirstEmployee();
            else if (rpTeams.Items.Count > 0) SelectFirstTeam();
        }

        protected void SelectFirstEmployee()
        {
            LinkButton lnkEmployee = rpEmployees.Items[0].FindControl("lnkEmployee") as LinkButton;
            String toSplit = lnkEmployee.CommandArgument;
            ScorecardTypeId = toSplit.Split(';')[0];
            EmployeeId = toSplit.Split(';')[1];
            OwnerId = toSplit.Split(';')[2];
            ShowHideMine();
        }

        protected void SelectFirstTeam()
        {
            LinkButton lnkTeam = rpTeams.Items[0].FindControl("lnkTeam") as LinkButton;
            String ToSplit = lnkTeam.CommandArgument;
            ScorecardTypeId = ToSplit.Split(';')[0];
            OwnerId = ToSplit.Split(';')[1];
        }

        public void SelectNextEmployee()
        {
            String temp = String.Empty;
            for (int i = 0; i < rpEmployees.Items.Count; i++)
            {
                LinkButton lnkEmployee = rpEmployees.Items[i].FindControl("lnkEmployee") as LinkButton;
                String toSplit = lnkEmployee.CommandArgument;
                String scorecardTypeId = toSplit.Split(';')[0];
                String employeeId = toSplit.Split(';')[1];
                String ownerId = toSplit.Split(';')[2];
                if (temp == EmployeeId)
                {
                    ScorecardTypeId = scorecardTypeId;
                    EmployeeId = employeeId;
                    OwnerId = ownerId;
                    break;
                }
                temp = employeeId;
            }
            rpEmployees.DataBind();
        }

        public void SelectPreviousEmployee()
        {
            String temp = String.Empty, scorecardTypeId = String.Empty, ownerId = String.Empty;
            for (int i = 0; i < rpEmployees.Items.Count; i++)
            {
                LinkButton lnkEmployee = rpEmployees.Items[i].FindControl("lnkEmployee") as LinkButton;
                String toSplit = lnkEmployee.CommandArgument;
                String employeeId = toSplit.Split(';')[1];
                if (employeeId == EmployeeId)
                {
                    ScorecardTypeId = scorecardTypeId;
                    EmployeeId = temp;
                    OwnerId = ownerId;
                    break;
                }
                scorecardTypeId = toSplit.Split(';')[0];
                ownerId = toSplit.Split(';')[2];
                temp = employeeId;
            }
            rpEmployees.DataBind();
        }

        public Boolean NextEmployeeAvailable()
        {
            if (rpEmployees.Items.Count == 0) return false;
            for (int i = 0; i < rpEmployees.Items.Count; i++)
            {
                LinkButton lnkEmployee = rpEmployees.Items[i].FindControl("lnkEmployee") as LinkButton;
                String toSplit = lnkEmployee.CommandArgument;
                String employeeId = toSplit.Split(';')[1];
                if (employeeId == EmployeeId && i == (rpEmployees.Items.Count - 1)) return false;
            }
            return true;
        }

        public Boolean PreviousEmployeeAvailable()
        {
            if (rpEmployees.Items.Count == 0) return false;
            for (int i = 0; i < rpEmployees.Items.Count; i++)
            {
                LinkButton lnkEmployee = rpEmployees.Items[i].FindControl("lnkEmployee") as LinkButton;
                String toSplit = lnkEmployee.CommandArgument;
                String employeeId = toSplit.Split(';')[1];
                if (employeeId == EmployeeId && i == 0) return false;
            }
            return true;
        }

        protected void Save(object sender, EventArgs e)
        {
            if (Saved != null) Saved(null, EventArgs.Empty);
        }

        protected void Accept(object sender, EventArgs e)
        {
            if (Accepted != null) Accepted(null, EventArgs.Empty);
        }

        protected void AcceptAll(object sender, EventArgs e)
        {
            if (AcceptAlled != null) AcceptAlled(null, EventArgs.Empty);
        }

        protected void ChangeSuperior(object sender, EventArgs e)
        {
            String observerId = (sender as LinkButton).CommandArgument;
            ObserverId = observerId;
            EmployeeId = null;
            ScorecardTypeId = null;
            OwnerId = null;
            if (SuperiorChanged != null) SuperiorChanged(observerId, EventArgs.Empty);
            rpSuperiors.DataBind();
        }

        protected void ChangeEmployee(object sender, EventArgs e)
        {
            String toSplit = (sender as LinkButton).CommandArgument;
            ScorecardTypeId = toSplit.Split(';')[0];
            EmployeeId = toSplit.Split(';')[1];
            OwnerId = toSplit.Split(';')[2];
            if (EmployeeChanged != null) EmployeeChanged(ScorecardTypeId, EventArgs.Empty);
            rpEmployees.DataBind();
            rpTeams.DataBind();
            ShowHideMine();
        }

        protected void ChangeTeam(object sender, EventArgs e)
        {
            String ToSplit = (sender as LinkButton).CommandArgument;
            ScorecardTypeId = ToSplit.Split(';')[0];
            OwnerId = ToSplit.Split(';')[1];
            EmployeeId = null;
            if (TeamChanged != null) TeamChanged(ScorecardTypeId, EventArgs.Empty);
            rpTeams.DataBind();
            rpEmployees.DataBind();
            ShowHideMine();
        }

        protected void MySpreadsheet(object seneder, EventArgs e)
        {
            String scorecardTypeId = db.getScalar(String.Format("select IdCommodity from Przypisania where IdPracownika = {0} and Do is null", ObserverId));
            ScorecardTypeId = scorecardTypeId;
            EmployeeId = ObserverId;
            //if (ChangedToMe != null) ChangedToMe(scorecardTypeId, EventArgs.Empty);
            if (TeamChanged != null) TeamChanged(scorecardTypeId, EventArgs.Empty);
            accept.Visible = showMine.Visible = false;
            rpEmployees.DataBind();
        }

        protected void UnAccept(object sender, EventArgs e)
        {
            if (UnAccepted != null) UnAccepted(null, EventArgs.Empty);
        }

        public String GetEmployeeClass(String Class, String employeeId, String TL)
        {
            return (employeeId == EmployeeId) ? Class + " sel" + ((TL == "1") ? " tl" : "") : Class + ((TL == "1") ? " tl" : "");
        }

        public String GetTeamClass(String Class, String ScorecardTypeId)
        {
            return String.IsNullOrEmpty(EmployeeId) && this.ScorecardTypeId == ScorecardTypeId ? Class + " sel" : Class;
        }
        public String GetObserverClass(String Class, String observerId)
        {
            return (observerId == ObserverId) ? Class + " sel" : Class;
        }

        public String EmployeeId
        {
            get { return hidEmployeeId.Value; }
            set { hidEmployeeId.Value = value; }
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }

        public String ObserverId
        {
            get { return hidObserverId.Value; }
            set { hidObserverId.Value = value; }
        }

        public String Date
        {
            get { return hidDate.Value; }
            set { hidDate.Value = value; }
        }

        public String OwnerId
        {
            get { return ViewState["vOwnerId"] as String; }
            set { ViewState["vOwnerId"] = value; }
        }

        public Control AcceptButton
        {
            get { return accept; }
        }

        public Control AcceptAllButton
        {
            get { return acceptall; }
        }

        public Control UnAcceptButton
        {
            get { return unaccept; }
        }

        public Control SaveButton
        {
            get { return save; }
        }

        public Control MySpreadsheetButton
        {
            get { return showMine; }
        }
    }
}
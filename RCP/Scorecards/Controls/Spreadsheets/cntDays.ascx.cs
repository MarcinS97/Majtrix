using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Scorecards.App_Code;
using System.Data;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntDays : System.Web.UI.UserControl
    {
        public event EventHandler OutsideClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btExcel.Attributes["onclick"] = String.Format("javascript:exportExcel('{0}');return true;", hidReport.ClientID);
            }
        }



        protected void btExcel_Click(object sender, EventArgs e)
        {
            string filename = "report.csv";
            Report.ExportExcel(hidReport.Value, filename, null);   // >>>>>> musi jak redirect do nowej strony bo się nie ściąga
        }

        protected void btClose_Click(object sender, EventArgs e)
        {
            //((UpdatePanel)Parent.Parent).Update();  // bo jest Conditional
        }




        public void Prepare(String EmployeeId, String ScorecardTypeId, String Date, Boolean InEdit, String OutsideJob, String Genre, String ObserverId, String TeamLeader, DataView Days)
        {
            this.EmployeeId = EmployeeId;
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.OutsideJob = OutsideJob;
            this.Genre = Genre;
            this.InEdit = InEdit;
            this.ObserverId = ObserverId;
            this.TeamLeader = TeamLeader;
            upMain.Update();
            rpDays.DataSource = Days;
            rpDays.DataBind();
        }

        protected void OutsideJobClick(object sender, EventArgs e)
        {
            if (OutsideClick != null) OutsideClick(null, EventArgs.Empty);
            //Save();
            Tools.MakeDialogCloseButton(btClose, "divZoom");
            Tools.ShowDialog("divZoom", upMain.ClientID, "Arkusze", 300, btClose.ClientID);
            SelectedDay = (sender as LinkButton).CommandArgument;
            lvSpreadsheets.DataBind();
        }

        protected void lvSpreadsheets_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Go":
                    string scorecardTypeId = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(scorecardTypeId)) return;
                    db.execSQL(String.Format(@"
if (select COUNT(*) from scDni d where d.Data = '{2}' and d.IdTypuArkuszy = {1} and d.IdPracownika = {0}) = 0
    insert into scDni (IdPracownika, IdTypuArkuszy, Data, CelProd, CelJak, Nominal) values ({0}, {1}, '{2}', 0.5, 0.5, 8)", EmployeeId, scorecardTypeId, SelectedDay));


                    Session["scEmployeeId"] = EmployeeId;
                    Session["scScorecardTypeId"] = ScorecardTypeId;
                    Session["scDate"] = Date;
                    Session["scObserverId"] = ObserverId;

                    Response.Redirect(ResolveUrl(String.Format("~/Scorecards/Scorecard.aspx?p={0}", Report.EncryptQueryString(String.Format("{0}|{1}|{2}|{3}|{4}|", EmployeeId, scorecardTypeId, Date, InEdit, ObserverId), Grid.key, Grid.salt))));
                    //redirect
                    break;
            }
        }

        public Boolean GetState(String State)
        {
            return State == "1";
        }

        public void Save()
        {
            foreach (RepeaterItem item in rpDays.Items)
            {
                HiddenField hidState = item.FindControl("hidState") as HiddenField;
                if (hidState.Value == "0") continue;
                TextBox tbValue = item.FindControl("tbUnprod") as TextBox;
                TextBox tbOutsideJob = item.FindControl("tbOutsideJob") as TextBox;
                HiddenField hidId = item.FindControl("hidId") as HiddenField;
                HiddenField hidDate = item.FindControl("hidDate") as HiddenField;
                HiddenField hidOldValue = item.FindControl("hidOldValue") as HiddenField;
                HiddenField hidOldOutside = item.FindControl("hidOldOutside") as HiddenField;
                String date = hidDate.Value;
                String id = hidId.Value;
                String value = tbValue.Text.Replace(',', '.');
                String outside = tbOutsideJob.Text.Replace(',', '.');
                String oldValue = hidOldValue.Value;
                String oldOutside = hidOldOutside.Value;
                /*String OldCorrection = Tools.GetText(item, "hidOldCorrection");
                String Correction = Tools.GetText(item, "tbTeamCorrection");*/

                if (value == oldValue && outside == oldOutside /*&& Correction == OldCorrection*/) continue;

                id = db.getScalar(String.Format("select Id from scDni where IdTypuArkuszy = {0} and IdPracownika = {1} and Data = {2}", ScorecardTypeId, EmployeeId, db.strParam(date)));


                if (String.IsNullOrEmpty(id))
                {
                    //db.execSQL(String.Format(@"insert into scDni (IdPracownika, IdTypuArkuszy, Data, CelProd, CelJak, Nominal, CzasNieprod, PracaInnyArkusz) values ({0}, {1}, '{2}', {3}, {4}, {5}, {6}, {7})",
                    //EmployeeId, ScorecardTypeId, date, "0.5", "0.5", "8", String.IsNullOrEmpty(value) ? "NULL" : value, outside)); //do poprawki kiedy indziej - przejdziemy przez ten most kiedy do niego dojdziemy

                    int idd = db.insert("scDni", true, true, 0, "IdPracownika, IdTypuArkuszy, Data, CelProd, CelJak, Nominal, CzasNieprod, PracaInnyArkusz",
                        EmployeeId, ScorecardTypeId, db.strParam(date), "0.5", "0.5", "8", String.IsNullOrEmpty(value) ? "NULL" : value, db.nullParam(outside));
                    //if (idd == -1)
                }
                else
                {
                    //db.execSQL(String.Format(@"update scDni set CzasNieprod = {0}, PracaInnyArkusz = {2} where Id = {1}", String.IsNullOrEmpty(value) ? "NULL" : value, id, outside));
                    bool ok = db.update("scDni", 0, "CzasNieprod,PracaInnyArkusz", "Id=" + id, db.nullParam(value), db.nullParam(outside));
                    //if (!ok) ...
                }
            }
        }

        protected void ShowZoom(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            String ToSplit = lnk.CommandArgument;

            String Date, Col;
            Tools.GetLineParams(ToSplit, out Date, out Col);
            ShowZoom(Date, Col);
        }

        protected void ShowZoom(String Date, String Col)
        {
            EmployeeZoom.Prepare(ScorecardTypeId, Date, ObserverId, TeamLeader, Col);
            //Tools.MakeDialogCloseButton(EmployeeZoom.CloseButton, "divZoomInfo");
            //Tools.ShowDialog("divZoomInfo", upMain.ClientID, String.Format("Pracownicy - {0}", Date), 1000, EmployeeZoom.CloseButton.ClientID);
            Tools.MakeDialogCloseButton(btClose2, "divZoomInfo");
            Tools.ShowDialog("divZoomInfo", upMain.ClientID, String.Format("Pracownicy - {0}", Date), 1100, btClose2.ClientID);
            //Tools.ShowDialog("divZoomInfo", UpdatePanel2.ClientID, String.Format("Pracownicy - {0}", Date), 1000, btClose2.ClientID);
        }

        public Boolean IsTeam()
        {
            return Genre == Dictionaries.Spreadsheets.Team;
        }

        public Boolean IsOutsideJob()
        {
            return OutsideJob == "1";
        }

        public String GetClass(String DefaultClass, Boolean Edit)
        {
            return DefaultClass + ((Edit) ? " edit" : String.Empty);
        }

        public Boolean IsInEdit()
        {
            return InEdit;
        }

        public Boolean InEdit
        {
            get { return Tools.GetViewStateBool(ViewState["vInEdit"], false); }
            set { ViewState["vInEdit"] = value; }
        }

        public String SelectedDay
        {
            get { return hidSelectedDay.Value; }
            set { hidSelectedDay.Value = value; }
        }

        public String ScorecardTypeId
        {
            get { return hidScorecardTypeId.Value; }
            set { hidScorecardTypeId.Value = value; }
        }

        public String EmployeeId
        {
            get { return hidEmployeeId.Value; }
            set { hidEmployeeId.Value = value; }
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

        public String OutsideJob
        {
            get { return hidOutsideJob.Value; }
            set { hidOutsideJob.Value = value; }
        }

        public String Genre
        {
            get { return hidGenre.Value; }
            set { hidGenre.Value = value; }
        }

        public String TeamLeader
        {
            get { return hidTeamLeader.Value; }
            set { hidTeamLeader.Value = value; }
        }

        public Boolean IsTL()
        {
            return TeamLeader == "1";
        }

    }
}
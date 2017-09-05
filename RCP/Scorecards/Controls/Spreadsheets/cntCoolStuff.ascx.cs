using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntCoolStuff : System.Web.UI.UserControl
    {
        public event EventHandler RowDeleted;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String EmployeeId, String ScorecardTypeId, String Date, Boolean InEdit, String OutsideJob, DataView Days)
        {
            this.EmployeeId = EmployeeId;
            this.ScorecardTypeId = ScorecardTypeId;
            this.Date = Date;
            this.InEdit = InEdit;
            this.OutsideJob = OutsideJob;
            rpDays.DataSource = Days;
            rpDays.DataBind();
        }

        public void Save()
        {
            foreach (RepeaterItem item in rpDays.Items)
            {
                HiddenField hidState = item.FindControl("hidState") as HiddenField;
                if (hidState.Value == "0") continue;
                TextBox tbNotes = item.FindControl("tbNotes") as TextBox;
                TextBox tbErrors = item.FindControl("tbErrors") as TextBox;
                HiddenField hidId = item.FindControl("hidId") as HiddenField;
                HiddenField hidDate = item.FindControl("hidDate") as HiddenField;
                HiddenField hidOldNotes = item.FindControl("hidOldNotes") as HiddenField;
                HiddenField hidOldErrors = item.FindControl("hidOldErrors") as HiddenField;
                string date = hidDate.Value;
                string notes = tbNotes.Text;
                string errors = tbErrors.Text;
                string id = hidId.Value;
                string oldNotes = hidOldNotes.Value;
                string oldErrors = hidOldErrors.Value;

                id = db.getScalar(String.Format("select Id from scDni where IdTypuArkuszy = {0} and IdPracownika = {1} and Data = {2}", ScorecardTypeId, EmployeeId, db.strParam(date)));

                if (errors == oldErrors && notes == oldNotes) continue;
                if (String.IsNullOrEmpty(id)) db.execSQL(String.Format(@"insert into scDni (IdPracownika, IdTypuArkuszy, Data, CelProd, CelJak, Nominal, Uwagi, IloscBledow) values ({2}, {3}, '{4}', {5}, {6}, {7}, '{0}', {1})",
                    notes, String.IsNullOrEmpty(errors) ? "0" : errors, EmployeeId, ScorecardTypeId, date, "0.5", "0.5", "8")); //do poprawki kiedy indziej - przejdziemy przez ten most kiedy do niego dojdziemy
                else db.execSQL(String.Format(@"update scDni set Uwagi = '{0}', IloscBledow = {1} where Id = {2}", notes, db.nullParam(errors), id));
            }
        }

        protected void rpDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            LinkButton lnkDelete = e.Item.FindControl("lnkRemoveRow") as LinkButton;
            if (lnkDelete != null) Tools.MakeConfirmButton(lnkDelete, "Czy na pewno chcesz usunąć wszystkie dane?"); 
        }

        protected void DeleteRow(object sender, EventArgs e)
        {
            LinkButton btnDelete = (sender as LinkButton);
            String DateToRemove = btnDelete.CommandArgument;

            if ((OutsideJob == "0")) db.execSQL(String.Format(@"
delete from scDni where Data = '{0}' /*and IdTypuArkuszy = {1}*/ and IdPracownika = {2}
delete from scWartosci where Data = '{0}' /*and IdTypuArkuszy = {1}*/ and IdPracownika = {2}"
,DateToRemove
,ScorecardTypeId
,EmployeeId));
            else db.execSQL(String.Format(@"
delete from scWartosci where Data = '{0}' and IdTypuArkuszy = {1} and IdPracownika = {2}"
, DateToRemove
, ScorecardTypeId
, EmployeeId));


            if (RowDeleted != null) RowDeleted(null, EventArgs.Empty);
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
        //public String OutsideJob
        //{
        //    get { return hidOutsideJob.Value; }
        //    set { hidOutsideJob.Value = value; }
        //}
        public String OutsideJob
        {
            get { return hidOutsideJob.Value; }
            set { hidOutsideJob.Value = value; }
        }
    }
}
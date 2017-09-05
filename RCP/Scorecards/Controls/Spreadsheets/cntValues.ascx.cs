using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using System.Data;
using System.Web.UI.HtmlControls;

namespace HRRcp.Scorecards.Controls.Spreadsheets
{
    public partial class cntValues : System.Web.UI.UserControl
    {
        List<IEnumerable<object>> dvList = null;
        int index = 0;
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

            DataView days = Days;
            
            rpDays.DataSource = days;
            
            dvList = new List<IEnumerable<object>>();

            const int sqlColumns = 24;
            int colsCount = days.Table.Columns.Count - sqlColumns;

            foreach (DataRow dr in days.Table.Rows)
            {
                dvList.Add(dr.ItemArray.Skip(sqlColumns).Take(colsCount));
            }
            rpDays.DataBind();

        }

        protected void rpDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpValues = e.Item.FindControl("rpValues") as Repeater;
            if (rpValues != null)
            {
                //HiddenField hidState = rpValues.FindControl("hidState") as HiddenField;
                //if (hidState != null)
                //{
                //    DataRowView drv = (DataRowView)e.Item.DataItem;
                //    hidState.Value = drv["State"].ToString();
                //}

                DataRowView drv = (DataRowView)e.Item.DataItem;
                String State = drv["State"].ToString();

                rpValues.DataSource = dvList[index];
                rpValues.DataBind();

                foreach (RepeaterItem item in rpValues.Items)
                {
                    TextBox tbValue = item.FindControl("tbValue") as TextBox;
                    HtmlTableCell td = item.FindControl("tdValue") as HtmlTableCell;

                    tbValue.Visible = IsInEdit() && State == "1";
                    Tools.AddClass(td, GetClass("hid3 notselected", (IsInEdit() && State == "1")));
                }
                
                index++;
            }
        }

        public String GetValue(object o, int par)
        {
            return Tools.GetLineParam(o.ToString(), par);
        }

        protected void rpDays_DataBinding(object sender, EventArgs e)
        {
            index = 0;
        }

        public Boolean IsInEdit()
        {
            return InEdit;
        }

        public void Save()
        {
            foreach (RepeaterItem item in rpDays.Items)
            {
                HiddenField hidKonkret = item.FindControl("hidKonkret") as HiddenField;
                HiddenField hidDayExists = item.FindControl("hidDayExists") as HiddenField;
                Repeater rpValues = item.FindControl("rpValues") as Repeater;
                if (hidKonkret == null || rpValues == null) continue;
                string konkret = hidKonkret.Value;
                string dayExists = hidDayExists.Value;
                dayExists = db.getScalar(String.Format("select Id from scDni where IdTypuArkuszy = {0} and IdPracownika = {1} and Data = {2}", ScorecardTypeId, EmployeeId, db.strParam(konkret)));
                foreach (RepeaterItem child in rpValues.Items)
                {
                    HiddenField hidTaskId = child.FindControl("hidTaskId") as HiddenField;
                    HiddenField hidOldValue = child.FindControl("hidOldValue") as HiddenField;
                    TextBox tbValue = child.FindControl("tbValue") as TextBox;
                    HiddenField hidState = child.FindControl("hidState") as HiddenField;

                    if (hidTaskId == null || tbValue == null || hidState == null) continue;
                    if (hidState.Value == "0") continue;
                    string taskId = hidTaskId.Value;
                    string value = tbValue.Text;
                    string oldValue = hidOldValue.Value;
                    if (/*String.IsNullOrEmpty(value) || */value == oldValue) continue;
                    if (String.IsNullOrEmpty(dayExists))
                    {
                        db.execSQL(String.Format(@"insert into scDni (IdPracownika, IdTypuArkuszy, Data, CelProd, CelJak, Nominal) values ({0}, {1}, '{2}', {3}, {4}, {5})",
                        EmployeeId, ScorecardTypeId, konkret, "0.5", "0.5", "8")); //do poprawki kiedy indziej //do poprawki? dziala perfekcyjnie //a, dobra, wiem, te 0.5 i 8
                        dayExists = hidDayExists.Value = "1";
                    }
                    db.execSQL(String.Format(@"
if ((select COUNT(Ilosc) from scWartosci w where w.IdTypuArkuszy = {0} and w.IdPracownika = {5} and w.Data = '{1}' and w.IdCzynnosci = {2}) = 0)
	insert into scWartosci (Data, DataModyfikacji, DataUtworzenia, IdTypuArkuszy, IdPracownika, IdCzynnosci, IdModyfikujacego, IdTworzacego, Ilosc)
		values ('{1}', GETDATE(), GETDATE(), {0}, {5}, {2}, {3}, {3}, {4})
	else update scWartosci set Ilosc = {4}, DataModyfikacji = GETDATE(), IdModyfikujacego = {3} where IdTypuArkuszy = {0} and IdPracownika = {5} and Data = '{1}' and IdCzynnosci = {2}
", ScorecardTypeId, konkret, taskId, App.User.Id, String.IsNullOrEmpty(value) ? "NULL" : value, EmployeeId));

                }
            }
        }

        protected void dsValues_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            //string arkusz = hidScorecardId.Value;
            e.Command.Parameters["@typark"].Value = ScorecardTypeId;//arkusz;
            e.Command.Parameters["@pracId"].Value = EmployeeId;
            e.Command.Parameters["@date"].Value = Date;
            e.Command.Parameters["@oj"].Value = OutsideJob;
        }

        public Boolean GetState(Control Item)
        {
            HiddenField hidState = Item.FindControl("hidState") as HiddenField;
            if (hidState != null) return (hidState.Value == "1");
            return false;
        }

        public String GetClass(String DefaultClass, Boolean Edit)
        {
            return DefaultClass + ((Edit) ? " edit" : String.Empty);
        }

        public Boolean InEdit
        {
            get { return Tools.GetViewStateBool(ViewState["vInEdit"], false); }
            set { ViewState["vInEdit"] = value; }
        }

        public Boolean Ok()
        {

            foreach (RepeaterItem item in rpDays.Items)
            {
                HiddenField hidKonkret = item.FindControl("hidKonkret") as HiddenField;
                Repeater rpValues = item.FindControl("rpValues") as Repeater;
                if (hidKonkret == null || rpValues == null) continue;
                string konkret = hidKonkret.Value;
                foreach (RepeaterItem child in rpValues.Items)
                {
                    TextBox tbValue = child.FindControl("tbValue") as TextBox;
                    if (String.IsNullOrEmpty(tbValue.Text)) return false;
                }
            }
            return true;
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
        public String OutsideJob
        {
            get { return hidOutsideJob.Value; } //NIE DZIALA INACZEJ, DO CHOINKI
            set { hidOutsideJob.Value = value; }
        }
    }
}
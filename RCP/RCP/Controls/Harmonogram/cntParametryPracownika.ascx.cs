using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class cntParametryPracownika : System.Web.UI.UserControl
    {
        public event EventHandler Saved;

        public class Employee
        {
            public Employee(DataRow dr)
            {
                this.Id = db.getInt(dr, "Id", -1);
                this.Name = db.getValue(dr, "Name");
                this.Func = db.getValue(dr, "Func");
                this.Code = db.getInt(dr, "Code");
                //this.AllowedDays = db.getValue(dr, "AllowedDays");
                this.PeriodType = db.getInt(dr, "PeriodType");
            }
            public int Id { get; set; }
            public String Name { get; set; }
            public String Func { get; set; }
            public int? Code { get; set; }
            //public String AllowedDays { get; set; }
            public int? PeriodType { get; set; }

            //public List<Day> GetAllowedDays()
            //{
            //    List<Day> days = new List<Day>();
            //    if (AllowedDays.Length > 0)
            //    {
            //        for (int i = 0; i < AllowedDays.Length; i++)
            //        {
            //            Day day = new Day
            //            {
            //                Name = Tools.DayName[i + 1],
            //                Index = i,
            //                Checked = (AllowedDays[i] == '1')
            //            };
            //            days.Add(day);
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < 7; i++)
            //        {
            //            Day day = new Day
            //            {
            //                Name = Tools.DayName[i + 1],
            //                Index = i,
            //                Checked = true
            //            };
            //            days.Add(day);

            //        }
            //    }
            //    return days;
            //}
        }

        public class Day
        {
            public String Name { get; set; }
            public int Index { get; set; }
            public Boolean Checked { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public void Show(String EmployeeId, String DateFrom, String DateTo)
        {
            this.EmployeeId = EmployeeId;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;

            Emp = new Employee(db.Select.Row(dsParametry, EmployeeId, DateFrom, DateTo));

            cntModal.Show(false);
            Prepare();
        }

        public void Prepare()
        {
            cntModal.Title = "Parametry - " + Emp.Name;

            ClearData();
            
            tbFunk.Text = Emp.Func;

            ddlKody.DataBind();
            ddlKody.SelectedValue = (Emp.Code.HasValue ? Emp.Code.Value.ToString() : null);
            PrepareKody();
            //if (ddlKody.Items.Count > 1)
            //    ddlKody.Items.RemoveAt(0);
            ddlTypyOkresow.DataBind();
            ddlTypyOkresow.SelectedValue = Emp.PeriodType.HasValue ? Emp.PeriodType.Value.ToString() : null;


            PrepareEmployeeScheme();
            //rpDays.DataSource = Emp.GetAllowedDays();
            //rpDays.DataBind();
        }

        private void ClearData()
        {
            tbNewCode.Text = tbNewCodeDescription.Text = String.Empty;
        }

        public void Close()
        {
            cntModal.Close();
        }

        public Employee Emp { get; set; }
        public String EmployeeId
        {
            get { return hidEmployeeId.Value; }
            set { hidEmployeeId.Value = value; }
        }

        public String DateFrom
        {
            get { return hidDateFrom.Value; }
            set { hidDateFrom.Value = value; }
        }

        public String DateTo
        {
            get { return hidDateTo.Value; }
            set { hidDateTo.Value = value; }
        }

        //protected string GetNewAllowedDays()
        //{
        //    String output = "";
        //    //foreach(RepeaterItem item in rpDays.Items)
        //    //{
        //    //    bool check = false;
        //    //    CheckBox cb = item.FindControl("cbDay") as CheckBox;
        //    //    if (cb != null)
        //    //        check = cb.Checked;

        //    //    output += (check ? "1" : "0");
        //    //}
        //    return output;
        //}

        protected void bntSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            string kod = ddlKody.SelectedValue;
            if (!String.IsNullOrEmpty(tbNewCode.Text))
            {
                int id = db.insert(String.Format(dsNewCode.SelectCommand, db.strParam(tbNewCode.Text), db.nullStrParam(tbNewCodeDescription.Text)), true, true);
                kod = id.ToString();
                //db.Execute(dsNewCode, db.strParam(tbNewCode.Text), db.nullStrParam(tbNewCodeDescription.Text));

            }

            //String NewAllowedDays = GetNewAllowedDays();
            //db.Execute(dsSaveAllowedDays, EmployeeId, db.nullStrParam(NewAllowedDays));

            SaveEmployeeScheme();

            if (String.IsNullOrEmpty(kod))
            {
                db.Execute(dsDeleteCode, EmployeeId);
            }
            else
            {
                db.Execute(dsSaveCode, EmployeeId, db.nullParam(kod)/*db.nullParam(ddlKody.SelectedValue)*/, DateFrom, DateTo);
            }


            db.Execute(dsSavePeriodType, EmployeeId, db.nullParam(ddlTypyOkresow.SelectedValue), DateFrom, DateTo);
            db.Execute(dsSaveFunc, EmployeeId, db.nullStrParam(tbFunk.Text), DateFrom, DateTo);
            cntModal.Close();

            if (Saved != null)
                Saved(null, EventArgs.Empty);

        }

        protected void ddlKody_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareKody();
        }

        protected void PrepareKody()
        {
            bool newCode = ddlKody.SelectedValue == "-1";
            tbNewCode.Visible = tbNewCodeDescription.Visible = divNewCode.Visible = newCode;
        }

        List<IEnumerable<object>> dvList = null;
        int index = 0;
        DataTable dtTemp = null;

        private void PrepareEmployeeScheme()
        {
            //DataTable dt = db.Select.Table(dsEmployeeScheme);


            DataView dvEmployeeScheme = (DataView)dsEmployeeScheme.Select(DataSourceSelectArguments.Empty);

            rpSchemeHeader.DataSource = dvEmployeeScheme.Table.Columns;
            rpSchemeDataRows.DataSource = dvEmployeeScheme;

            dtTemp = dvEmployeeScheme.Table;

            dvList = new List<IEnumerable<object>>();
            foreach (DataRow dr in dvEmployeeScheme.Table.Rows)
            {
                dvList.Add(dr.ItemArray.Skip(1));
            }


            rpSchemeHeader.DataBind();
            rpSchemeDataRows.DataBind();


        }

        protected void rpSchemeDataRows_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpCells = e.Item.FindControl("rpSchemeDataCells") as Repeater;
            if (rpCells != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                rpCells.DataSource = dvList[index];
                rpCells.DataBind();
                index++;
            }
        }

        protected void rpSchemeDataRows_DataBinding(object sender, EventArgs e)
        {
            index = 0;
        }

        public String GetValue(object o, int par)
        {
            return Tools.GetLineParam(o.ToString(), par);
        }

        public void SaveEmployeeScheme()
        {
            foreach (RepeaterItem rpRow in rpSchemeDataRows.Items)
            {
                String weekDay = Tools.GetText(rpRow, "hidWeekDay");
                Repeater rpCells = rpRow.FindControl("rpSchemeDataCells") as Repeater;
                if (rpCells != null)
                {
                    foreach (RepeaterItem item in rpCells.Items)
                    {
                        String shiftId = Tools.GetText(item, "hidShiftId");
                        CheckBox cb = item.FindControl("cbCheck") as CheckBox;
                        if (cb != null)
                        {
                            bool check = cb.Checked;
                            string scheme = db.Select.Scalar(dsGetEmployeeScheme, EmployeeId, shiftId, weekDay);
                            bool exists = !String.IsNullOrEmpty(scheme);
                            if (check)
                            {
                                if(exists)
                                {
                                    db.Execute(dsRemoveEmployeeScheme, scheme);
                                }
                            }
                            else
                            {
                                if(!exists)
                                {
                                    db.Execute(dsAddEmployeeScheme, EmployeeId, shiftId, weekDay);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
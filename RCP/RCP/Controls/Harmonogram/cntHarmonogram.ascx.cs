using HRRcp.App_Code;
using HRRcp.Controls;
using HRRcp.Controls.RozliczenieNadg;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace HRRcp.RCP.Controls.Harmonogram
{

    /* Name;Class;Style;Checkbox */
    public class Header
    {
        public String Name { get; set; }
        public String Text { get; set; }
        public String Class { get; set; }
        public String Style { get; set; }
        public Boolean Visible { get; set; }
        public String Date { get; set; }
        public String Hint { get; set; }
        public String Type { get; set; }

        public String Raw { get; set; }



        public Header(String Parameters)
        {
            String[] Splitted = Tools.GetLineParams(Parameters);
            this.Name = Tools.GetParam(Splitted, 0);
            this.Text = Tools.GetParam(Splitted, 1);
            this.Class = Tools.GetParam(Splitted, 2);
            this.Style = Tools.GetParam(Splitted, 3);
            this.Visible = Tools.GetBoolean(Tools.GetParam(Splitted, 4), true);
            this.Date = Tools.GetParam(Splitted, 5);
            this.Hint = Tools.GetParam(Splitted, 6);
            this.Type = Tools.GetParam(Splitted, 7);
            this.Raw = Parameters;
        }
    }

    public class Cell
    {
        public String Text { get; set; }
        public String Class { get; set; }
        public String Style { get; set; }
        public String ShiftId { get; set; }
        public Double? Time { get; set; }
        public Boolean State { get; set; }

        public Cell(String Parameters)
        {
            String[] Splitted = Tools.GetLineParams(Parameters);
            this.Text = Tools.GetParam(Splitted, 0);
            this.Class = Tools.GetParam(Splitted, 1);
            this.Style = Tools.GetParam(Splitted, 2);
            this.ShiftId = Tools.GetParam(Splitted, 3);
            this.Time = Convert.ToDouble(Tools.GetParam(Splitted, 4));
            this.State = Tools.GetParam(Splitted, 5) == "1";
        }
    }


    public static class CustomExtensions
    {
        public static void SetControlsVisible(this Repeater rp, bool b, params string[] ctrl)
        {
            if (ctrl != null)
            {
                foreach (RepeaterItem item in rp.Items)
                {
                    foreach (string c in ctrl)
                    {
                        Control toSet = Tools.FindControl(item, c);
                        if (toSet != null)
                            toSet.Visible = b;
                    }
                }
            }
        }

        public static void SetControlsVisible(this Control cnt, bool b, params string[] ctrl)
        {
            foreach (string c in ctrl)
            {
                Control toSet = Tools.FindControl(cnt, c);
                if (toSet != null)
                    toSet.Visible = b;
            }
        }
    }


    public partial class cntHarmonogram : System.Web.UI.UserControl
    {
        List<IEnumerable<Cell>> DaysCells = null;
        List<Header> Headers = null;

        public event EventHandler ParametryPracownikaShow;


        int index = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public bool Prepare(String DataOd, String DataDo, String OkresId)
        {
            this.DataOd = DataOd;
            this.DataDo = DataDo;
            return Prepare();
        }


        public bool Prepare(String DataOd, String DataDo, String OkresId, String IdKierownika)
        {
            this.IdKierownika = IdKierownika;
            this.DataOd = DataOd;
            this.DataDo = DataDo;
            return Prepare();
        }

        public bool Prepare()
        {
            ClearBeforePrepare();
            SetClientVariables();
            DataView dv = (DataView)dsPivot.Select(DataSourceSelectArguments.Empty);
            if (dv != null && dv.Table.Rows.Count > 0)
            {
                PrepareHeader(dv.Table);
                PrepareTable(dv);
                divIfEmpty.Visible = false;
                ctHarmonogram.Visible = true;
#if OKT
                /*DateTime dt = Convert.ToDateTime(DataOd);*/
                /*cntRightSummary.Prepare(dv.Table, dt, DaysIndex + BeforeDays);*/
                //cntRightSummary.Visible = cntErrorsPanel.Visible = Editable;
                cntRightSummary.Prepare(DataOd, DataDo, /*IdCommodity*/Entities);
                cntErrorsPanel.Prepare(hidAllEmployees.Value, DataOd, DataDo);

#endif

#if OKT
                this.SetControlsVisible(false, "thRSum", "thPSum", "thNDSum");
                rpData.SetControlsVisible(false, "tdRSum", "tdPSum", "tdNDSum");
#endif
#if DBW || VICIM || VC
                this.SetControlsVisible(false, "thNDSum", "thKod", "thFunk", "th2");
                rpData.SetControlsVisible(false, "tdNDSum", "tdKod", "tdFunk", "tdNDLSum");
#endif

                lblEmployeeCount.Text = GetEmployeeCountText();
                return true;
            }
            else
            {
                divIfEmpty.Visible = true;
                ctHarmonogram.Visible = false;
                return false;
            }
        }

        int GetEmployeeCount()
        {
            return rpData.Items.Count;
        }

        string GetEmployeeCountText()
        {
            return " (" + GetEmployeeCount() + ")";
        }

        private void SetClientVariables()
        {
#if OKT
            //Tools.MakeConfirmButton(btnTest, "Potw");
            cntRightSummary.Visible = cntErrorsPanel.Visible = Editable;
            hidApp.Value = "keeeper";
#endif

#if DBW
            hidApp.Value = "DBW";
#endif
#if VICIM
            hidApp.Value = "VICIM";
#endif
#if VC
            hidApp.Value = "VC";
#endif
        }

        private void ClearBeforePrepare()
        {
            hidErrors.Value = "";
            hidSchedule.Value = "";
            hidAllEmployees.Value = "";
        }

        private void PrepareHeader(DataTable dt)
        {
            Headers = new List<Header>();
            int colsCount = dt.Columns.Count - DaysIndex - SumSize;
            String Splitted = String.Empty;
            Header NewHeader = null;
            foreach (DataColumn dc in dt.Columns)
            {
                Splitted = dc.ColumnName;
                NewHeader = new Header(dc.ColumnName);
                Headers.Add(NewHeader);
            }
            rpHeader.DataSource = Headers.Skip(DaysIndex).Take(colsCount);
            rpHeader.DataBind();
        }

        private void PrepareTable(DataView dv)
        {
            RPN = db.Select.Scalar(dsGetRPN);
            String Splitted = String.Empty;
            rpData.DataSource = dv;
            int colsCount = dv.Table.Columns.Count - DaysIndex - SumSize;

            DaysCells = new List<IEnumerable<Cell>>();

            List<Cell> Cells = null;
            int i = 0;
            foreach (DataRow dr in dv.Table.Rows)
            {                hidAllEmployees.Value += db.getValue(dr, 0) + ";";

                IEnumerable<object> Objects = dr.ItemArray.Skip(DaysIndex).Take(colsCount);
                Cells = new List<Cell>();
                i = 0;
                foreach (object o in Objects)
                {
                    Splitted = Tools.GetStr(o);
                    Cell cell = new Cell(Splitted);
                    cell.Class = Headers[DaysIndex + i].Class;
                    Cells.Add(cell);
                    i++;
                }
                DaysCells.Add(Cells);
            }
            rpData.DataBind();
        }


        public String GetColumnName(String Name)
        {
            foreach (Header h in Headers)
                if (h.Name == Name)
                    return h.Raw;
            return null;
        }

        public object QEval(String Name)
        {
            return Eval(GetColumnName(Name));
        }


        public class SaveObject
        {
            public String ShiftId { get; set; }
            public String EmployeeId { get; set; }
            public String Date { get; set; }
            public Double? Time { get; set; }
        }

        public static void SaveSchedule(cntHarmonogram.SaveObject[] data)
        {
            AppUser user = AppUser.CreateOrGetSession();
            foreach (SaveObject obj in data)
            {

                db.execSQL(con, String.Format(@"
declare @pracId int = {0}
declare @data datetime = {1}
declare @zmId int = {2}

declare @accId int = {3}
declare @time float = {4} * 3600
declare @nadzien datetime = GETDATE()

select
  @pracId IdPracownika
, @data Data
, @zmId IdZmiany
, @nadzien DataZm
, ISNULL(h.Id, 0) OriginalId
into #ccc
from (select 1 x) x
left join PlanPracy h on h.IdPracownika = @pracId and h.Data = @data
where ISNULL(@zmId, -1) != ISNULL(h.IdZmianyPlan, -1) or ISNULL(@time, -1) != ISNULL(h.WymiarPlan, -1)

update PlanPracy set
  IdZmianyPlan = @zmId
, DataZm = @nadzien
, IdKierownikaZm = @accId
, WymiarPlan = @time
from PlanPracy h
inner join #ccc a on a.OriginalId = h.Id

insert into PlanPracy (IdPracownika, Data, IdZmianyPlan, DataZm, IdKierownikaZm, Akceptacja, DataAcc, IdKierownikaAcc, WymiarPlan)
select a.IdPracownika, a.Data, a.IdZmiany, a.DataZm, @accId, 0, a.DataZm, @accId, @time
from (select 1 x) x
inner join #ccc a on a.OriginalId = 0

drop table #ccc
"
, db.nullParam(obj.EmployeeId)
, db.ToSqlValue(obj.Date)
, db.nullParam(obj.ShiftId)
, user.Id
, db.nullParam(obj.Time)
));


            }

            dbDisconnect();
        }


        class ShiftObject
        {
            public ShiftObject(int id, string name, string color)
            {
                this.id = id;
                this.name = name;
                this.color = color;
            }
            public int id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
        }

        public static String GetShifts()
        {
            DataTable dt = db.getDataSet(con, "select * from Zmiany").Tables[0];

            List<ShiftObject> objects = new List<ShiftObject>();

            foreach (DataRow dr in dt.Rows)
            {
                int id = db.getInt(dr, "Id", 0);
                string name = db.getValue(dr, "Symbol");
                string color = db.getValue(dr, "Kolor");

                ShiftObject obj = new ShiftObject(id, name, color);
                objects.Add(obj);
            }
            dbDisconnect();
            return JsonConvert.SerializeObject(objects);
        }

        public static String GetHistory(String EmpId, String Od, String Do)
        {
            DataTable dt = db.getDataSet(con, String.Format(@"
declare @pracId int = {0}
declare @od datetime = {1}
declare @do datetime = {2}

select
  '{{' +
     '""date"":""' + CONVERT(varchar, q.Wersja, 20) + '""' +
      ',""shifts"":[' + dbo.cat(
        '{{' +
             '""day"":""' + CONVERT(varchar, DAY(q.Data) - 1) + '""' +
            ',""id"":""' + CONVERT(varchar, q.IdZmiany) + '""' +
            ',""name"":""' + ISNULL(q.Symbol, '') + '""' +
            ',""time"":""' + ISNULL(CONVERT(varchar, q.Wymiar / 3600), '') + '""' +
            ',""color"":""' + ISNULL(q.Kolor, '') + '""' +
        '}}'
    , ',', 0) +
      ']' +
  '}}'
from 
(
    select
      v.Wersja
    , d.Data
    , /*case when hh.Id is not null then*/ ISNULL(hh.IdZmiany, 0) /*else -1 end*/ IdZmiany
    , z.Symbol
    , case when hh.Id is not null then hh.Wymiar else null end Wymiar
    , z.Kolor
    from dbo.getdates2(@od, @do) d
    cross join (select distinct Wersja from rcpHarmonogramHistoria where IdPracownika = @pracId and Data between @od and @do) v
    /*left join rcpHarmonogramHistoria hh on hh.Data = d.Data and hh.IdPracownika = @pracId and hh.Wersja = v.Wersja*/
    outer apply (select top 1 * from rcpHarmonogramHistoria where Data = d.Data and IdPracownika = @pracId and Wersja <= v.Wersja order by Wersja desc) hh 
    left join Zmiany z on z.Id = hh.IdZmiany
) q
group by q.Wersja
", EmpId, db.strParam(Od), db.strParam(Do))).Tables[0];


            string outputJson = "[";

            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                string jsonMaster = db.getValue(dt.Rows[i], 0);
                if (!String.IsNullOrEmpty(jsonMaster))
                {
                    outputJson += (i != 0 ? "," : "") + jsonMaster;
                }

            }
            outputJson += "]";
            dbDisconnect();
            return outputJson;
        }



        public static String GetErrors(String emp, String DataOd, String DataDo)
        {
            emp = emp.Replace(';', ',');
            DataTable dt = db.getDataSet(con, "select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') and InstantCheck = 1 order by Kolejnosc").Tables[0];

            foreach(DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");
                DataRow conditionData = db.getDataRow(con, String.Format(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo)));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {
                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");
                    cMsg = cMsg.Replace("\r\n", "<br />");
                    Tools.ShowMessage(cMsg);

                    try
                    {
                        string json = db.getValue(conditionData, "JSON", "");
                        return json;
                        //Tools.ExecuteJavascript("triggerErrorChange();");
                    }
                    catch { }
                    return "";
                }
            }
            return "";
        }

        public bool Check(bool includeNotRequired)
        {
            DataTable dt = db.Select.Table(dsConditions, includeNotRequired ? "0" : "1");
            string emp = GetSelectedEmployeesString(true);//GetSelectedEmployeesArray(true);
            hidErrors.Value = "";
            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");

                DataRow conditionData = db.Select.Row(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {


                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");
                    cMsg = cMsg.Replace("\r\n", "<br />");
                    Tools.ShowMessage(cMsg);

                    try
                    {
                        string json = db.getValue(conditionData, "JSON", "");
                        hidErrors.Value = json;
                        //Tools.ExecuteJavascript("triggerErrorChange();");
                    }
                    catch { }
                    return false;
                }
            }

            return true;
        }

        public string Check(bool includeNotRequired, bool tmp)
        {
            DataTable dt = db.Select.Table(dsConditions, includeNotRequired ? "0" : "1");
            string emp = GetSelectedEmployeesString(true);//GetSelectedEmployeesArray(true);
            hidErrors.Value = "";
            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");

                DataRow conditionData = db.Select.Row(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {


                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");
                    cMsg = cMsg.Replace("\r\n", "<br />");
                    //Tools.ShowMessage(cMsg);

                    try
                    {
                        string json = db.getValue(conditionData, "JSON", "");
                        hidErrors.Value = json;
                        //Tools.ExecuteJavascript("triggerErrorChange();");
                    }
                    catch { }
                    return cMsg;
                }
            }

            return "";
        }


        static SqlConnection fcon = null;
        private static SqlConnection con
        {
            get
            {
                if (fcon == null) db.DoConnect(ref fcon);
                return fcon;
            }
        }
        private static void dbDisconnect()
        {
            if (fcon != null)
                db.DoDisconnect(ref fcon);
        }

        protected void rpData_DataBinding(object sender, EventArgs e)
        {
            index = 0;
        }

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpDays = e.Item.FindControl("rpDays") as Repeater;
            if (rpDays != null)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                rpDays.DataSource = DaysCells[index];
                rpDays.DataBind();
                index++;
            }


#if DBW || VICIM || VC
            LinkButton lbtn = e.Item.FindControl("LinkButton1") as LinkButton;
            if(lbtn != null)
            {
                lbtn.Enabled = false;
                lbtn.ToolTip = "";
            }
#endif

        }




        public string[] GetSelectedEmployeesArray(bool allIfNone)
        {
            List<string> list = new List<string>();
            List<string> listAll = hidAllEmployees.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string selected = hidSelectedEmployees.Value;

            foreach (string id in selected.Split(';'))
            {
                if (!String.IsNullOrEmpty(id))
                    list.Add(id);
            }
            return (list.Count > 0 || !allIfNone) ? list.ToArray() : listAll.ToArray();
        }

        //public IEnumerable<string> GetAllEmployees()
        //{
        //    //List<string> employees = hidAllEmployees.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //    //foreach(string emp in employees)
        //    //{

        //    //}


        //}

        //public RepeaterItem FindRepeaterItemByEmployeeId(string employeeId)
        //{
        //    foreach(RepeaterItem item in rpData.Items)
        //    {
        //        string 

        //    }
        //}

        public string GetSelectedEmployeesString(bool allIfNone)
        {
            return String.Join(",", GetSelectedEmployeesArray(allIfNone));
        }

        public string[] GetNotSelectedEmployeesArray()
        {
            List<string> listAll = GetListFromString(hidAllEmployees.Value);
            List<string> listSelected = GetListFromString(hidSelectedEmployees.Value);
            return listAll.Where(x => !listSelected.Any(e => x.Contains(e))).ToArray();
        }

        public String GetNotSelectedEmployeesString()
        {
            return String.Join(",", GetNotSelectedEmployeesArray());
        }

        public List<String> GetListFromString(string s)
        {
            return s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public String IdCommodity
        {
            get { return hidCommodity.Value; }
            set { hidCommodity.Value = value; }
        }

        public String IdKierownika
        {
            get { return hidIdKierownika.Value; }
            set { hidIdKierownika.Value = value; }
        }

        public String IdStanowiska
        {
            get { return hidIdStanowiska.Value; }
            set { hidIdStanowiska.Value = value; Prepare(); }
        }

        public String IdDzialu
        {
            get { return hidIdDzialu.Value; }
            set { hidIdDzialu.Value = value; Prepare(); }
        }

        public String IdKlasyfikacji
        {
            get { return hidIdKlasyfikacji.Value; }
            set { hidIdKlasyfikacji.Value = value; Prepare(); }
        }

        public String Klasyfikacje
        {
            get { return hidKlasyfikacje.Value; }
            set { hidKlasyfikacje.Value = value; Prepare(); }
        }


        
        public String Entities
        {
            set { hidEntities.Value = value; Prepare(); }
            get { return hidEntities.Value; }
        }

        public String DataOd
        {
            get { return hidDataOd.Value; }
            set { hidDataOd.Value = value; }
        }

        public String DataDo
        {
            get { return hidDataDo.Value; }
            set { hidDataDo.Value = value; }
        }

        public int DaysIndex
        {
            get { return Tools.GetViewStateInt(ViewState["vDaysIndex"], 3); }
            set { ViewState["vDaysIndex"] = value; }
        }

        public int SumSize
        {
            get { return Tools.GetViewStateInt(ViewState["vSumSize"], 0); }
            set { ViewState["vSumSize"] = value; }
        }

        public int BeforeDays
        {
            get { return 7; }
        }

        public Boolean Editable
        {
            get { return Tools.GetViewStateBool(ViewState["vEditable"], false); }
            set
            {
                ViewState["vEditable"] = value;
                ctHarmonogram.Attributes["data-editable"] = value.ToString();
                cntRightSummary.Visible = cntErrorsPanel.Visible = value;
            }
        }

        protected void PracownikLabel_Click(object sender, EventArgs e)
        {
            String EmployeeId = (sender as LinkButton).CommandArgument;
            if (!String.IsNullOrEmpty(EmployeeId))
            {
                if (ParametryPracownikaShow != null)
                    ParametryPracownikaShow(EmployeeId, e);
                //cntParametryPracownika.Show(EmployeeId);
            }
            else
            {
                Tools.ShowError("Bląd");
            }
        }

        public string Search
        {
            get { return hidSearch.Value; }
            set { hidSearch.Value = value; }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            Tools.ShowMessage("HEHE");
        }

        protected void cntParametryPracownika_Saved(object sender, EventArgs e)
        {
            UpdatePanel panel = Tools.FindUpdatePanel(this);
            UpdatePanel panel2 = Tools.FindUpdatePanel(panel);
            panel2.Update();
        }

        public String RPN
        {
            get { return hidRPN.Value; }
            set { hidRPN.Value = value; }
        }

        public String GetAdditionalEmployeeClass(object firmaSort)
        {
            if(firmaSort != null)
            {
                return ((int)firmaSort == -1) ? "additional" : "";
            }
            return "";
        }

        public String GetStateClass(object state)
        {
            return Convert.ToBoolean(state) ? " present" : "";
        }
    }
}
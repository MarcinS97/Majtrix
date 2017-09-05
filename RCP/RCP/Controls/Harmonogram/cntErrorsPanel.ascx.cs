using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json;
using System.IO;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public partial class cntErrorsPanel : System.Web.UI.UserControl
    {
        const String _ExcelFilename = "{0}_Bledy_Harmonogram";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Prepare(String Employees, String DataOd, String DataDo)
        {
            this.Employees = Employees;
            this.DataOd = DataOd;
            this.DataDo = DataDo;
        }

        public static String GetInstantErrors(String emp, String DataOd, String DataDo)
        {
            emp = emp.Replace(';', ',');
            DataTable dt = db.getDataSet(con, "select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') order by Kolejnosc").Tables[0];
            List<string> errors = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");
                DataRow conditionData = db.getDataRow(con, String.Format(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo)));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {
                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");

                    List<String> msgs = cMsg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    string output = "<span class='error-title'>" + msgs[0] + "</span>";

                    for (int i = 1; i < msgs.Count; i++)
                        output += "<span class='error-item'>" + msgs[i] + "</span>";

                        //cMsg = cMsg.Replace("\r\n", "<br />");
                    errors.Add(output);
                }
            }
            return JsonConvert.SerializeObject(errors);
        }

        public static String GetErrorsCount(String emp, String DataOd, String DataDo)
        {
            emp = emp.Replace(';', ',');
            DataTable dt = db.getDataSet(con, "select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') order by Kolejnosc").Tables[0];

            int count = 0;

            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");
                DataRow conditionData = db.getDataRow(con, String.Format(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo)));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {
                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");

                    List<String> msgs = cMsg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    count += msgs.Count - 1;
                }
            }
            return count.ToString();
        }


        public List<String> GetExcelErrors(String emp, String DataOd, String DataDo)
        {
            emp = emp.Replace(';', ',');
            DataTable dt = db.getDataSet("select * from rcpWarunki where getdate() between DataOd and isnull(DataDo, '20990909') order by Kolejnosc").Tables[0];
            List<string> errors = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string conditionSQL = db.getValue(dr, "Sql");
                DataRow conditionData = db.getDataRow(String.Format(conditionSQL, /*IdKierownika*/db.nullStrParam(emp), db.strParam(DataOd), db.strParam(DataDo)));
                int cCode = db.getInt(conditionData, "ErrorLevel", 0);

                if (cCode > 0)
                {
                    string cMsg = db.getValue(conditionData, "ErrorMsg", "");
                    List<String> msgs = cMsg.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (string msg in msgs)
                        errors.Add(msg);
                }
            }
            return errors;
        }

        public void ExportToExcel(List<String> lines)
        {
            DateTime dt = DateTime.Now;
            String Date = dt.ToString("yyyyMMdd_HHmm");
            String Filename = String.Format(_ExcelFilename, Date);
            Report.StartExportCSV(Filename);
            StringWriter sw = new StringWriter();
            foreach(string line in lines)
            {
                Report.AddLineCSV(sw, line);
            }
            Report.EndExportCSV(sw);
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            List<String> errors = GetExcelErrors(Employees, DataOd, DataDo);
            if(errors.Count > 0)
                ExportToExcel(errors);
        }


        public String Employees
        {
            get { return hidEmployees.Value; }
            set { hidEmployees.Value = value; }
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
    }
}
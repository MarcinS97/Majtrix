using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Configuration;
using HRRcp.App_Code;

namespace HRRcp.Controls.Reports
{
    public partial class cntDetails2 : System.Web.UI.UserControl
    {
        const string _sql = "_sql";

        public const string cssSumRow = "sum";
        public const string cssAltRow = "alt";
        public const string cssHidden = "hidden";

        public const string tHide = "-";
        public const string tDate = "D";
        public const string tDateTime = "DT";
        public const string tNum = "N";
        public const string tSum = "S";   // na końcu

        string FTitle = null;
        string FTitle2 = null;
        string FTitle3 = null;
        string FTitle4 = null;
        string FDesc = null;

        string FSql = null;
        string FGenerator = null;
        string FCmd1 = null;            // komendy wykonywane przed, wynik do wstawienia: @CMDn
        string FCmd2 = null;
        string FCmd3 = null;
        string FCmd4 = null;
        string FCmd5 = null;

        string FSql1 = null;            // parametry do modyfikacji w kodzie
        string FSql2 = null;
        string FSql3 = null;
        string FSql4 = null;
        string FSql5 = null;

        string FP1 = null;              // parametry do podstawień w kodzie z ręki, np prawa lepiej tak jak mają być nadane przez dołożenie zapytania - jak cos sie nie wykona, nie zostaną nadane, SQLn będzie z automatu podstawiony
        string FP2 = null;
        string FP3 = null;
        string FP4 = null;
        string FP5 = null;

        bool FAllowQueryString = true;  // parametry przekazywane w linii poleceń - zgodność, powinno być wyłączone BEZPIECZEŃSTWO !!!

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = SQL;
            if (!IsPostBack)
            {
            }
        }

        public override void DataBind()
        {
            base.DataBind();                // tu podepnie wyrażenia <%# Eval() %>

            if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
                Prepare();
        }




        public void GridDataBind()
        {
            //gvReport.DataBind();
        }







        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
                Prepare();                  // raporty, które nie mają sql (np header), ale moga mieć parametry - dla nich sie nie wykonuje DataBind

            base.OnPreRender(e);
        }


        //--------------------------------
        //public string Prepare(int mode)
        public string Prepare()
        {
            
            cntReportHeader1.Caption = PrepareText(FTitle);
            cntReportHeader1.Caption1 = PrepareText(FTitle2);
            cntReportHeader1.Caption2 = PrepareText(FTitle3);
            cntReportHeader1.Caption3 = PrepareText(FTitle4);

            cntReportHeader1.Visible = !cntReportHeader1.IsEmpty;

            string desc = PrepareDesc(FDesc);
            if (!String.IsNullOrEmpty(desc))
            {
                ltDesc.Text = desc;
                ltDesc.Visible = true;
            }

            if (!String.IsNullOrEmpty(FSql))
            {
                string sql = null;
                sql = PrepareParams(FSql);

                if (!String.IsNullOrEmpty(FGenerator))
                {
                    DataSet dsCmd = db.getDataSet(PrepareParams(FGenerator));
                    string cmd = db.Join(dsCmd, 0, ",");
                    sql = sql.Replace("@GEN", cmd);
                }

                PrepareCmd(ref sql, "@CMD1", FCmd1);
                PrepareCmd(ref sql, "@CMD2", FCmd2);
                PrepareCmd(ref sql, "@CMD3", FCmd3);
                PrepareCmd(ref sql, "@CMD4", FCmd4);
                PrepareCmd(ref sql, "@CMD5", FCmd5);

                //SQLtmp = sql;
                ViewState[_sql] = sql;
                SqlDataSource1.SelectCommand = sql;
                //gvReport.Visible = true;
                return sql;
            }
            else
            {
                //gvReport.Visible = false;
                return null;
            }
        }

        private string PrepareText(string value)
        {
            string t = PrepareParams(value);
            if (t != null && t.StartsWith("select")) t = db.getScalar(t);
            return t;
        }

        private string PrepareDesc(string value)
        {
            string d = PrepareText(value);
            if (d != null) d = d.Replace("\r\n", "<br />");
            return d;
        }

        private void PrepareCmd(ref string sql, string cmdId, string cmd)
        {
            string value = null;
            if (!String.IsNullOrEmpty(cmd))
            {
                value = PrepareParams(cmd);
                if (value != null && value.StartsWith("select"))
                    value = db.getScalar(value);
            }
            if (!String.IsNullOrEmpty(sql))
                sql = sql.Replace(cmdId, value);
        }

        private string PrepareParams(string sql)        // podmiana parametrów @p1
        {
            if (!String.IsNullOrEmpty(sql))
            {
                sql = sql.Trim().Replace("@SQL1", SQL1);        // żeby starts with zadzialalo
                sql = sql.Replace("@SQL2", SQL2);
                sql = sql.Replace("@SQL3", SQL3);
                sql = sql.Replace("@SQL4", SQL4);
                sql = sql.Replace("@SQL5", SQL5);
                //----- zmienne wywołania raportu -----         // <<<< !!!!! BEZPIECZEŃSTWO !!!!!
                if (FAllowQueryString)
                    for (int i = 0; i < Request.QueryString.Count; i++)
                    {
                        string pname = Request.QueryString.Keys[i];
                        sql = sql.Replace("@" + pname, Request.QueryString[i]);
                    }
                //----- zmienne predefiniowane -----
                AppUser user = AppUser.CreateOrGetSession();
                sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
                sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));// to najpierw
                sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
                sql = sql.Replace("@Login", db.strParam(user.Login));
            }
            return sql;
        }

        //----------------------------
        protected void dvDetails_DataBound(object sender, EventArgs e)
        {
            App_Code.Tools.ShowMessage("asddsa");
        }

        protected void dvDetails_PreRender(object sender, EventArgs e)
        {

        }
    





        //----------------------------
        public string SQL
        {
            get { return Tools.GetViewStateStr(ViewState[_sql]); }
            set { FSql = value; }
        }
        /*
        public string SQLtmp
        {
            get { return Tools.GetViewStateStr(ViewState[_sql + "tmp"]); }
            set { ViewState[_sql + "tmp"] = value; }
        }
        */
        public string GEN   // wkleja wynik zapytania sklejając kolejne wiersze znakiem ,
        {
            get { return FGenerator; }
            set { FGenerator = value; }
        }
        //----------
        public string CMD1
        {
            get { return FCmd1; }
            set { FCmd1 = value; }
        }

        public string CMD2
        {
            get { return FCmd1; }
            set { FCmd1 = value; }
        }

        public string CMD3
        {
            get { return FCmd1; }
            set { FCmd1 = value; }
        }

        public string CMD4
        {
            get { return FCmd1; }
            set { FCmd1 = value; }
        }

        public string CMD5
        {
            get { return FCmd1; }
            set { FCmd1 = value; }
        }

        //----------
        [Bindable(true, BindingDirection.OneWay)]
        public string SQL1
        {
            get { return FSql1; }
            set { FSql1 = value; }
        }

        public string SQL2
        {
            get { return FSql2; }
            set { FSql2 = value; }
        }

        public string SQL3
        {
            get { return FSql3; }
            set { FSql3 = value; }
        }

        public string SQL4
        {
            get { return FSql4; }
            set { FSql4 = value; }
        }

        public string SQL5
        {
            get { return FSql5; }
            set { FSql5 = value; }
        }
        //----------
        public string P1
        {
            get { return FP1; }
            set { FP1 = value; }
        }

        public string P2
        {
            get { return FP2; }
            set { FP2 = value; }
        }

        public string P3
        {
            get { return FP3; }
            set { FP3 = value; }
        }

        public string P4
        {
            get { return FP4; }
            set { FP4 = value; }
        }

        public string P5
        {
            get { return FP5; }
            set { FP5 = value; }
        }
        //-----------
        /*
        public bool Browser     // ustawić na false i wtedy w kodzie ExportCsv
        {
            get { return divReport.Visible; }
            set { divReport.Visible = value; }
        }

        public string CssClass
        {
            get { return divReport.Attributes["class"]; }
            set { divReport.Attributes["class"] = value; }
        }
        */



        public cntReportHeader Header
        {
            get { return cntReportHeader1; }
        }

        public bool HeaderVisible
        {
            get { return cntReportHeader1.Visible; }
            set { cntReportHeader1.Visible = value; }
        }

        public string ConStr
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                    SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString; ;
            }
            //get { return SqlDataSource1.ConnectionString; }
        }

        public bool AllowQueryString
        {
            set { FAllowQueryString = value; }
            get { return FAllowQueryString; }
        }
    }
}





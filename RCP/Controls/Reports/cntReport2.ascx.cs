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
    public partial class cntReport2 : System.Web.UI.UserControl
    {
        public event EventHandler Command;

        const string _sql  = "_sql";

        //public const bool cryptParams = true;
        //public const string key = "148169751135946";
        //public const string salt = "96654182751586";   // na razie, potem dac losowe i zapisywane w sesji

        string FTitle = null;
        string FTitle2 = null;
        string FTitle3 = null;
        string FTitle4 = null;
        string FDesc   = null;

        string FSql = null;
        string FGenerator = null;
        string FCmd1 = null;            // komendy wykonywane przed, wynik do wstawienia: @CMDn
        string FCmd2 = null;
        string FCmd3 = null;
        string FCmd4 = null;
        string FCmd5 = null;

        string FSql1 = null;            // parametry do modyfikacji w kodzie, np parametry @SQL1, które będą zamienione na wartości
        string FSql2 = null;
        string FSql3 = null;
        string FSql4 = null;
        string FSql5 = null;

        string FP1 = null;              // parametry do podstawień w kodzie z ręki, np prawa lepiej tak jak mają być nadane przez dołożenie zapytania - jak cos sie nie wykona, nie zostaną nadane, SQLn będzie z automatu podstawiony
        string FP2 = null;              // podstawiam je do SQL1..5
        string FP3 = null;
        string FP4 = null;
        string FP5 = null;

        bool FAllowQueryString = true;  // parametry przekazywane w linii poleceń - zgodność, powinno być wyłączone BEZPIECZEŃSTWO !!!

        protected void Page_Init(object sender, EventArgs e)
        {
            //Grid.Prepare(gvReport, "GridView2", false, 0, true);
            //Grid.Prepare(gvReport, null, false, 0, true);
            Grid.Prepare(gvReport, null, AllowPaging, PageSize, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string con = ConStr;
            if (!String.IsNullOrEmpty(con))
                SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings[con].ConnectionString; 

            SqlDataSource1.SelectCommand = SQL;
            SqlDataSource1.UpdateCommand = FooterSql;   // 20160106
            if (!IsPostBack)
            {
                /*
                //if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
                if (String.IsNullOrEmpty(FSql)) // tylko jak pusto to ma zrobić Prepare bo się wtedy nie wykonuje DataBind
                {
                    Prepare();
                    //Tools.ExecOnStart2("start_" + ClientID, "doClick('" + btExecute.ClientID + "');");
                }
                */
            }
        }

        /*
        public void Execute()
        {
            //Tools.AddClass(divReport, ID);
            if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
            {
                string sql = SQLtmp;
                ViewState[_sql] = sql;
                SqlDataSource1.SelectCommand = sql;
            }
        }
        */

        public override void DataBind()
        {
            base.DataBind();                // tu podepnie wyrażenia <%# Eval() %>

            if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
                Prepare();
        }




        public void GridDataBind()
        {
            gvReport.DataBind();
        }





        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (String.IsNullOrEmpty(SQL))  // tylko za pierwszym razem, bo wyżej można poustawiać 
                Prepare();                  // raporty, które nie mają sql (np header), ale moga mieć parametry - dla nich sie nie wykonuje DataBind

            base.OnPreRender(e);
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 0;   //dodać jako property ...
        }

        protected void btExecute_Click(object sender, EventArgs e)
        {
            //Execute();
        }

        protected void gvReportCmd_Click(object sender, EventArgs e)
        {
            //string p = gvReportCmdPar.Value;
            if (Command != null)
                Command(this, EventArgs.Empty);
        }

        public string CommandName
        {
            get { return Tools.GetLineParam(gvReportCmdPar.Value, 0); }
        }

        public string CommandParameter   // razem z name !!!
        {
            get { return gvReportCmdPar.Value; }
            //get { return Grid.DecryptParam(gvReportCmdPar.Value); }
        }

        //--------------------------------
        //public string Prepare(int mode)
        public string x_Prepare(string p1, string p2, string p3, string p4, string p5)
        {
            SQL1 = p1;
            SQL2 = p2;
            SQL3 = p3;
            SQL4 = p4;
            SQL5 = p5;
            return Prepare();
        }

        public string Prepare(params object[] par)
        {
            for (int i = 0; i < par.Length; i++)
                switch (i)
                {
                    case 0:
                        SQL1 = par[i].ToString();
                        break;
                    case 1:
                        SQL2 = par[i].ToString();
                        break;
                    case 2:
                        SQL3 = par[i].ToString();
                        break;
                    case 3:
                        SQL4 = par[i].ToString();
                        break;
                    case 4:
                        SQL5 = par[i].ToString();
                        break;
                }
            return Prepare();
        }

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
                
                gvReport.Sort("", SortDirection.Ascending);
                
                gvReport.Visible = true;
                return sql;
            }
            else
            {
                gvReport.Visible = false;
                return null;
            }
        }

        private string PrepareText(string value)
        {
            string t = PrepareParams(value);
            if (t != null && t.StartsWith("select")) 
                return db.getScalar(t);
            else
                //return L.p(t);
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

        //-------------
        public static string[] GetParams()
        {
            string p = Tools.GetStr(HttpContext.Current.Request.QueryString["p"]);
            if (!String.IsNullOrEmpty(p))
            {
                p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                if (!String.IsNullOrEmpty(p))
                    return Tools.GetLineParams(p);
            }
            return null;
        }
        
        public string GetParam(int idx)  // 1,2... do pobrania parametru na formatce raportu
        {
            idx--;
            if (Grid.cryptParams)
            {
                string p = Tools.GetStr(Request.QueryString["p"]);
                if (!String.IsNullOrEmpty(p))
                {
                    p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                    if (!String.IsNullOrEmpty(p))
                        return Tools.GetLineParam(p, idx);
                }
            }
            else
            {
                if (0 <= idx && idx < Request.QueryString.Count)
                    return Request.QueryString[idx];
            }
            return null;
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
                if (Grid.cryptParams)
                {
                    string p = Tools.GetStr(Request.QueryString["p"]);
                    if (!String.IsNullOrEmpty(p))
                    {
                        p = Report.DecryptQueryString(p, Grid.key, Grid.salt);
                        if (!String.IsNullOrEmpty(p))
                        {
                            string[] par = Tools.GetLineParams(p);
                            for (int i = 0; i < par.Length - 1; i++)   // ostatni parametr to 0x0
                            {
                                string pname = "p" + (i + 1).ToString();   // p1 p2 ...
                                sql = sql.Replace("@" + pname, par[i]);
                            }
                        }
                    }
                }
                else
                {
                    if (FAllowQueryString)
                        for (int i = 0; i < Request.QueryString.Count; i++)
                        {
                            string pname = Request.QueryString.Keys[i];
                            sql = sql.Replace("@" + pname, Request.QueryString[i]);
                        }
                }
                //----- zmienne predefiniowane -----
                /*
                AppUser user = AppUser.CreateOrGetSession();
                sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
                sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));  // to najpierw
                sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
                sql = sql.Replace("@Login", db.strParam(user.Login));
                //sql = sql.Replace("@lang", db.strParam(L.Lang));
                sql = sql.Replace("@lang", db.strParam("PL"));
                 */
                sql = PrepareStdParams(sql);
            }
            return sql;
        }
        //-----
        public static string PrepareStdParams(string sql)
        {
            return PrepareStdParams(sql, null);
        }

        public static string PrepareStdParams(string sql, AppUser user)
        {
            if (user == null) user = AppUser.CreateOrGetSession();
            sql = sql.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
            sql = sql.Replace("@OriginalId", user.OriginalId);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
            sql = sql.Replace("@KadryId2", db.strParam(user.KadryId2));  // to najpierw
            sql = sql.Replace("@KadryId", db.strParam(user.NR_EW));
            sql = sql.Replace("@Login", db.strParam(user.Login));
            sql = sql.Replace("@lang", db.strParam(L.Lang));
            //sql = sql.Replace("@lang", db.strParam("PL"));
            return sql;
        }
        //-----

        /*
        public void Execute()
        {
        }
        */


        public bool ExportCSV(string filename, bool prepare)
        {
            if (prepare)
            {
                SqlDataSource1.SelectCommand = Prepare();
            }
            if (String.IsNullOrEmpty(filename))
            {
                filename = Report.PrepareRepFileName(cntReportHeader1.Caption);
                if (String.IsNullOrEmpty(filename))
                    filename = "Report";
            }
            string header = Tools.AddCRLF(cntReportHeader1.Caption) +
                            Tools.AddCRLF(cntReportHeader1.Caption1) +
                            Tools.AddCRLF(cntReportHeader1.Caption2);
            string footer = Tools.AddCRLF(Description);
            Report.ExportCSV(filename, SQL, header, footer, true);
            return true;
        }

        public string GetNoDataInfo(string defInfo)
        {
            string info = NoDataInfo;
            if (String.IsNullOrEmpty(info))
                //return L.p(defInfo);
                return defInfo;
            else
                //return L.p(info);
                return info;
        }
        //-----------------------------------
        //---------------------------------------    
        public string Title
        {
            get { return cntReportHeader1.Caption; }
            set { FTitle = value; }
        }

        public string Title2
        {
            get { return cntReportHeader1.Caption1; }
            set { FTitle2 = value; }
        }

        public string Title3
        {
            get { return cntReportHeader1.Caption2; }
            set { FTitle3 = value; }
        }

        public string Title4
        {
            get { return cntReportHeader1.Caption3; }
            set { FTitle4 = value; }
        }

        public string Description
        {
            get { return Tools.GetViewStateStr(ViewState["_desc"]); }
            set { FDesc = value; }
        }

        /*
        public string Title
        {
            get { return Tools.GetViewStateStr(ViewState["_title"]); }
            set
            {
                string t = PrepareParams(value);
                ViewState["_title"] = t;
            }
        }

        public string Title2
        {
            get { return Tools.GetViewStateStr(ViewState["_title2"]); }
            set
            {
                string t = PrepareParams(value);
                ViewState["_title2"] = t;
            }
        }

        public string Title3
        {
            get { return Tools.GetViewStateStr(ViewState["_title3"]); }
            set
            {
                string t = PrepareParams(value);
                ViewState["_title3"] = t;
            }
        }
        */
        public string NoDataInfo
        {
            get { return Tools.GetViewStateStr(ViewState["_nodata"]); }
            set { ViewState["_nodata"] = value; }
        }
        //----------
        public string SQL
        {
            get { return Tools.GetViewStateStr(ViewState[_sql]); }
            set { FSql = value; }
        }

        public string FooterSql
        {
            set { ViewState["footersql"] = value; }
            get { return Tools.GetStr(ViewState["footersql"]); }
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

        public string GridCssClass
        {
            get { return gvReport.CssClass; }
            set { gvReport.CssClass = value; }
        }

        public cntReportHeader Header
        {
            get { return cntReportHeader1; }
        }

        public bool HeaderVisible
        {
            get { return cntReportHeader1.Visible; }
            set { cntReportHeader1.Visible = value; }
        }
        public bool GridVisible
        {
            get { return gvReport.Visible; }
            set { gvReport.Visible = value; }
        }
        //------------
        public bool AllowPaging
        {
            get { return gvReport.AllowPaging; }
            set { gvReport.AllowPaging = value; }
        }

        public int PageSize
        {
            get { return gvReport.PageSize; }
            set { gvReport.PageSize = value; }
        }
        //-----------
        public string ConStr
        {
            set
            {
                if (String.IsNullOrEmpty(value))
                    value = "HRConnectionString";
                ViewState["constr"] = value;
                SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
            }
            //get { return SqlDataSource1.ConnectionString; }
            get { return Tools.GetStr(ViewState["constr"]); }
        }

        public SqlDataSource DataSourceSql
        {
            get { return SqlDataSource1; }
        }

        public GridView ReportGrid
        {
            get { return gvReport; }
        }

        public bool AllowQueryString
        {
            set { FAllowQueryString = value; }
            get { return FAllowQueryString; }
        }

        int RecCount = 0;

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            RecCount = e.AffectedRows;
        }

        public int Execute()
        {
            Prepare();
            DataBind();
            return RecCount;
        }
    }
}
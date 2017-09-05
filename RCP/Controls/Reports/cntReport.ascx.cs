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
    public partial class cntReport : System.Web.UI.UserControl
    {
        const string _sql  = "_sql";

        public const string cssSumRow   = "sum";
        public const string cssAltRow   = "alt";
        public const string cssHidden   = "hidden";

        public const string tHide       = "-";
        public const string tDate       = "D";
        public const string tDateTime   = "DT";
        public const string tNum        = "N";
        public const string tSum        = "S";   // na końcu

        string FTitle  = null;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = SQL;
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

        protected void btExecute_Click(object sender, EventArgs e)
        {
            //Execute();
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
                sql = sql.Replace("@KadryId2", user.KadryId2);// to najpierw
                sql = sql.Replace("@KadryId", user.NR_EW);
                sql = sql.Replace("@Login", user.Login);

            }
            return sql;
        }

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
                return defInfo;
            else
                return info;
        }
        //-----------------------------------
        public class CellData
        {
            public const string errValue = "error";
            public string header = null;       // napis
            public string typ = null;          // 
            public string format = null;           
            public bool withSum = false;
            public bool withZoom = false;
            public string zoom = null;
            public string hint = null;
            public double sum = 0;

            public CellData(TableCell cell)
            {
                if (cell.Controls.Count > 0)        // tu jest dynamiczna kontrolka LinkButton jezeli autogenerate columns
                {
                    LinkButton c = cell.Controls[0] as LinkButton;
                    if (c != null)
                    {
                        header = PrepareHeader(c.Text);
                        c.Text = header;
                        cell.CssClass = css;
                    }
                }
            }
            //---------
            // header:typ format S|page param1 param2 ..)
            private string PrepareHeader(string h)
            {
                string[] hh = h.Split('|');
                string[] tt = hh[0].Split(':');
                header = tt[0];
                //----- format -----
                if (tt.Length > 1)
                {
                    int len = tt[1].Length - 1;
                    withSum = tt[1].EndsWith(tSum);
                    if (withSum) len--;
                    
                    switch (Tools.Substring(tt[1],0,2))    // dwuliterowe, jeśli nie to jednoliterowe, string moze być krótszy
                    {
                        case tDateTime:
                            typ = tt[1].Substring(0,2);
                            format = tt[1].Substring(1,len - 1);
                            break;
                        default:
                            typ = tt[1].Substring(0,1);
                            format = tt[1].Substring(1,len);
                            break;
                    }
                }
                //----- zoom -----
                if (hh.Length > 1)
                {
                    withZoom = true;
                    zoom = hh[1];
                }
                //----- hint -----
                if (hh.Length > 2)
                    hint = hh[2];
                return header;
            }

            private string N(string num, out bool zero)
            {
                double d;
                string n;
                if (Double.TryParse(num, out d))
                {
                    if (String.IsNullOrEmpty(format))
                    {
                        n = d.ToString();
                        sum += d;
                    }
                    else
                    {
                        n = d.ToString(format);  //20130829 <<< po co to ??? zeby sumy były z wyświetlonych zaokrągleń ?
                        sum += Double.Parse(n);


                        //n = d.ToString("#'" + format.Replace("0", "#"));

                    }
                    zero = d == 0.0;
                    return n;
                }
                else
                {
                    zero = false;
                    return num;
                }
            }

            private string D(string d)
            {
                if (d.Length > 10)
                    return d.Substring(0, 10);
                else
                    return d;
            }

            private bool FieldIs(string colName, string fieldName)
            {
                if (colName.Length == fieldName.Length)
                    return colName == fieldName;
                else if (colName.Length > fieldName.Length)
                    return colName.StartsWith(fieldName + ":") ||
                           colName.StartsWith(fieldName + "|");
                else
                    return false;
            }

            private string GetColumnName(string colNameFormat)
            {
                int p = colNameFormat.IndexOfAny(":|".ToCharArray());
                if (p >= 0)
                    return colNameFormat.Substring(0, p);
                else
                    return colNameFormat;
            }

            private string Zoom(string value, DataRowView drv)
            {
                if (withZoom)
                {
                    string title = null;
                    if (!String.IsNullOrEmpty(hint))
                        title = String.Format(" title=\"{0}\"", hint); //pozniej dac parsowanie 

                    if (zoom.Trim().ToLower().StartsWith("javascript"))
                    {
                        //----- javascript -----
                        string js = zoom;
                        for (int idx = 0; idx < drv.Row.ItemArray.Length; idx++)
                        {
                            string h = GetColumnName(drv.Row.Table.Columns[idx].Caption);
                            string v = drv[idx].ToString();
                            js = js.Replace("@" + h, v);
                        }
                        return String.Format("<a href=\"{0}\"{2}>{1}</a>", js, value, title);
                    }
                    else
                    {
                        string[] zz = zoom.Split(' ');

                        if (String.IsNullOrEmpty(zz[0]))
                        {
                            //----- hint -----
                            return String.Format("<span {1}>{0}</span>", value, title);
                        }
                        else
                        {
                            //----- link -----
                            string page = zz[0] + ".aspx";
                            string par = null;
                            for (int i = 1; i < zz.Length; i++)
                            {
                                string p = zz[i];
                                if (p.StartsWith("@"))
                                {
                                    string p1 = p.Substring(1);
                                    int idx = Tools.StrToInt(p1, -99);
                                    if (idx == -99)
                                    {
                                        for (int j = 0; j < drv.Row.ItemArray.Length; j++)
                                            if (FieldIs(drv.Row.Table.Columns[j].Caption, p1))    //drv.Row.Table.Columns[j].Caption.StartsWith(p1))
                                            {
                                                idx = j;
                                                break;
                                            }
                                    }
                                    if (idx != -99)
                                        p = drv[idx].ToString();
                                }
                                par += String.Format("&p{0}={1}", i, p.Replace("&", "%26"));   //formatuje do postaci p1=xxx&p2=yyy...
                            }
                            if (!String.IsNullOrEmpty(par))
                                page += "?" + par.Substring(1);

                            return String.Format("<a href=\"{0}\"{2}>{1}</a>", page, value, title);
                        }
                    }
                }
                return value;
            }
            
            //---------
            public void PrepareCell(TableCell cell, DataRowView drv)     // rozbija value na elementy klasy
            {
                cell.CssClass = css;
                string c;
                switch (typ)
                {
                    case tHide:
                        cell.Visible = false;
                        return;
                    case tDate:
                        c = D(cell.Text);
                        break;
                    case tNum:
                        bool zero;
                        c = N(cell.Text, out zero);
                        if (zero)                            
                            cell.CssClass += " zero";
                        break;
                    default:
                        c = cell.Text;
                        break;
                }
                cell.Text = Zoom(c, drv);
                if (!withZoom)
                    cell.ToolTip = hint;
            }

            public void PrepareFooter(TableCell cell)
            {
                cell.CssClass = css;
                switch (typ)
                {
                    case tHide:
                        cell.Visible = false;
                        return;
                    default:
                        if (typ == tNum && withSum)
                        {
                            cell.CssClass = css;
                            cell.Text = sum.ToString(format);
                        }
                        break;
                }
            }

            public string css
            {
                get
                {
                    switch (typ)
                    {
                        case tHide:
                            return cssHidden;
                        default:
                            return typ;
                    }
                }
            }        
        }
        //-----------------------------
        CellData[] cellData = null;
        private int rcnt = 0;

        public void PrepareHeader(GridViewRow row)
        {
            bool footer = false;
            int cnt = row.Cells.Count;
            cellData = new CellData[cnt];           // tabelka
            for (int i = 0; i < cnt; i++)
            {
                CellData cd = new CellData(row.Cells[i]);
                cellData[i] = cd;
                if (cd.withSum)
                    footer = true; 
            }
            gvReport.ShowFooter = footer;
        }

        public void PrepareCells(GridViewRow row)
        {
            int cnt = row.Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                CellData cd = cellData[i];
                cd.PrepareCell(row.Cells[i], (DataRowView)row.DataItem);
            }
            if (rcnt % 2 == 0) row.CssClass = cssAltRow;
            if (row.Visible) rcnt++;
        }

        public void PrepareFooter(GridViewRow row)
        {
            for (int i = 0; i < row.Cells.Count; i++)
            {
                CellData cd = cellData[i];
                cd.PrepareFooter(row.Cells[i]);
            }
            row.CssClass = cssSumRow;
        }
        
        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.EmptyDataRow:
                    break;
                case DataControlRowType.Header:
                    PrepareHeader(e.Row);                
                    break;
                case DataControlRowType.DataRow:
                    PrepareCells(e.Row);
                    break;
                case DataControlRowType.Footer:
                    PrepareFooter(e.Row);
                    break;
                case DataControlRowType.Separator:
                    break;
                case DataControlRowType.Pager:
                    break;
            }
        }

        protected void gvReport_PreRender(object sender, EventArgs e)
        {
            gvReport.Attributes["name"] = "report";

            /*
            int cnt = headerRow.Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                TableCell tc = headerRow.Cells[i];
                //string c1 = gvReport.Columns[i].HeaderText;
                string c1 = gvReport.HeaderRow.Cells[i].Text;

                DataControlFieldCell fieldCell = gvReport.HeaderRow.Cells[i] as DataControlFieldCell;
                DataControlField field = fieldCell.ContainingField;
                string strHdrTxt = field.HeaderText;
                string c2 = field.SortExpression;
            }
             */ 
        }

        protected void gvReport_DataBound(object sender, EventArgs e)
        {

        }
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

        public cntReportHeader Header
        {
            get { return cntReportHeader1; }
        }

        public bool HeaderVisible
        {
            get { return cntReportHeader1.Visible; }
            set { cntReportHeader1.Visible = value; }
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
                if (!String.IsNullOrEmpty(value))
                    SqlDataSource1.ConnectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString; }
            //get { return SqlDataSource1.ConnectionString; }
        }

        public bool AllowQueryString
        {
            set { FAllowQueryString = value; }
            get { return FAllowQueryString; }
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 300; 
        }

        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {

        }
    }
}
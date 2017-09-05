using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using System.Text;
using HRRcp.App_Code;

using System.Web.UI.HtmlControls;
//using KDR.Controls.koshi;

using System.Net;
using System.IO;
using System.Diagnostics;

namespace HRRcp.Controls.EliteReports
{
    public partial class cntReport : System.Web.UI.UserControl
    {
        private const String _SQL = "__sql";

        private const String _VTABLECLASS = "_TABLECLASS";
        private const String _VROWCLASS = "_ROWCLASS";
        private const String _VALTERNATEROWCLASS = "_ALTERNATEROWCLASS";
        private const String _VDIVCLASS = "_DIVCLASS";
        private const String _VSELECTEDCHART = "___SELECTEDCHART";
        private const String _VFORMATTOKENS = "___FORMATTOKENS";
        private const String _VPAGER = "___PAGERBOOLEAN";

        private const String _TDESC = "_desc"; //Tomek
        private const String _COLUMNNAMES = "___COLUMNNAMES";
        private const String _TABLEHEADER = "th";
        private const String _TD_STYLE = "td_style";
        private const String _TD_FORMAT = "td_format";
        private const String _TD_LINK = "td_link";
        private const String _TH_SORT = "th_sort";
        //new 
        private const String _TD_CHECKBOX = "checkbox";
        private const String RowId = "TableRow{0}";
        //
        private const String _ASC = "asc";
        private const String _DESC = "desc";
        private const String _LASTSORT = "_LASTSORT";
        private const String _FOOTER = "footer";
        private const String _SUM = "sum";
        private const String _COUNT = "count";
        private const String _MEAN = "mean";
        private const String ___PAGE = "___PAGE";
        private const String ___COUNTER = "___COUNTEROFTHEELEMENTSINTHETABLE";
        private const String _CURRENTSORTING = "__CURRENTSORTING";
        private const String _DORECREATE = "__DORECREATE";
        private const String _NEWP = "__NEWP";
        private const String _TABLE = "_TABLE";

        private const String __SQL1 = "__SQL1";
        private const String __SQL2 = "__SQL2";
        private const String __SQL3 = "__SQL3";
        private const String __SQL4 = "__SQL4";
        private const String __SQL5 = "__SQL5";

        private const String __P1 = "__P1";
        private const String __P2 = "__P2";
        private const String __P3 = "__P3";
        private const String __P4 = "__P4";
        private const String __P5 = "__P5";

        private const String _TOOLTIP = "tooltip";
        private const Char _Escape = '\\';
        private const Char _Begin = '[';
        private const Char _End = ']';
        private const Char _Divide = '|';
        private const Char _Override = '`';
        private const Char _Also = ':';
        private const Char _Hidden = '-';
        private const Char _Level = '^';
        private const String __Start = "start";
        private const String __Begin = "begin";
        private const String __End = "end";
        private const String ___TABLE = "___TABLE";
        private const String ___ENUMERATOR = "___ENUMERATOR";
        private const Char _WHITESPACE = (Char)0x20;
        private const Char _QUESTIONMARK = (Char)0x3f;
        private const Char _AMPERSAND = (Char)0x26;
        private const Char _ChartsSplitter = ',';
        private volatile int VisiblePages = 5;

        //Dynamic controls Id's
        private const String _ID_HeaderLink = "HeaderLink";
        private const String _ID_PagerControls = "PagerControls";

        // Defaults
        private const String _DEFAULTTABLECLASS = "GridView1 table0";
        private const String _DEFAULTROWCLASS = "";
        private const String _DEFAULTALTERNATEROWCLASS = "alt";
        private const String _DEFAULTDIVCLASS = "border";

        public const String FDate = "D";      // [:D;css] css opcjonalny
        public const String FDateTime = "DT";     // [:DT;css]
        public const String FNum = "N";      // [:N;css]
        public const String FSum = "S";      // [:S;css] pod tabelą - sumuje tylko widoczne !!!
        public const String FCount = "C";      // [:C;css] pod tabelą - ilość 
        public const String FMean = "M";

        public const String __SESSION = "__SESSIONQUERYVALUE";
        public const String __SESSIONPARAMETER = "__SESSIONPARAMETERQUERYVALUE";

        public readonly String[] FormatTypes = { FDate, FDateTime, FNum, FSum, FCount, FMean };

        String FTitle = null;
        String FTitle2 = null;
        String FTitle3 = null;
        String FTitle4 = null;
        String FDesc = null;

        //Dictionary<String, String>[] Columns;
        List<Dictionary<String, String>> Columns;
        List<LinkButton> links = new List<LinkButton>();

        public enum ActionType
        {
            None, Pages, Count, One
        }
        public class FooterElements
        {
            public int Count = -1;
            public int Sum = -1;
            public FooterElements()
            {
                Count = 0;
                Sum = 0;
            }
        }

        protected String ParseSQL(String query, ActionType Action)
        {
            StringBuilder TemporaryColumn = null;
            //int Iteration = -1;
            Boolean remp = false, lremp = false;
            int TokenIndex = -1;
            //Columns = new Dictionary<String, String>[255];
            Columns = new List<Dictionary<String, String>>();
            Dictionary<String, String> Column;
            if (String.IsNullOrEmpty(query)) return String.Empty;
            String ParsedQuery = query;

            //ZASTEPYWANIE TYCH TYCH TAKICH TYCH TAM

            //predefiniowane zmienne Tomka
            AppUser user = AppUser.CreateOrGetSession();
            ParsedQuery = ParsedQuery.Replace("@UserId", user.Id);//App.User.Id);  cos tu sie wywalalo - jakby za wczesnie i jeszcze nie było zmiennej
            ParsedQuery = ParsedQuery.Replace("@KadryId", db.strParam(user.NR_EW));
            ParsedQuery = ParsedQuery.Replace("@Login", db.strParam(user.Login));
            ParsedQuery = ParsedQuery.Replace("@lang", db.strParam(L.Lang));

            List<String> Parameters = GetParameters();
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (ParsedQuery.Contains(String.Format("@p{0}", i + 1)))
                    ParsedQuery = ParsedQuery.Replace(String.Format("@p{0}", i + 1), Parameters[i]);
            }
            ParsedQuery = ParsedQuery.Replace("@SQL1", SQL1);
            ParsedQuery = ParsedQuery.Replace("@SQL2", SQL2);
            ParsedQuery = ParsedQuery.Replace("@SQL3", SQL3);
            ParsedQuery = ParsedQuery.Replace("@SQL4", SQL4);
            ParsedQuery = ParsedQuery.Replace("@SQL5", SQL5);

            //TU JUZ NIE

            //TU MOJE

            //ParsedQuery = ParsedQuery.Replace("@_list", FormatList(hidCheck.Value)); //syf, nie ruszac najlepiej

            try
            {
                ParsedQuery = ParsedQuery.Replace("@_session", Session[__SESSION] as String);
                ParsedQuery = ParsedQuery.Replace("@_value", (Session[__SESSION] as String).Substring(1).Split(new Char[] {';', ';'})[(int)Session[__SESSIONPARAMETER]]);
            }
            catch { }
            //TU JUZ NIE

            for (int i = 0; i < ParsedQuery.Length; i++)
            {
                switch (ParsedQuery[i])
                {
                    case '-':
                        if (ParsedQuery[i - 1] == '-' && !remp) lremp = true; break;
                    case '\n':
                        if (lremp) lremp = false; break;
                    case '*':
                        if (ParsedQuery[i - 1] == '/' && !lremp) remp = true; break;
                    case '/':
                        if (ParsedQuery[i - 1] == '*' && remp) remp = false; break;
                    case _Begin:
                        if ((remp || lremp) && ParsedQuery[i - 1] != _Escape)
                        {
                            i++;
                            //Iteration++;
                            TemporaryColumn = new StringBuilder();
                            for (; !(ParsedQuery[i] == ']' && ParsedQuery[i - 1] != _Escape); i++)
                            {
                                if (!(ParsedQuery[i] == _Escape && ParsedQuery[i - 1] != _Escape)) TemporaryColumn.Append(ParsedQuery[i]);
                            }
                            //Columns[Iteration] = new Dictionary<String, String>();
                            Column = new Dictionary<String, String>();
                            TokenIndex = -1;
                            foreach (String Token in TemporaryColumn.ToString().Split(_Divide))
                            {
                                String[] Values = Token.Split(_Override);
                                if (Values.Length == 1)
                                {
                                    TokenIndex++;
                                    switch (TokenIndex)
                                    {
                                        case 0:
                                            //Columns[Iteration][_TABLEHEADER] = Values[0];
                                            Column[_TABLEHEADER] = Values[0];
                                            break;
                                        case 1:
                                            //tu bedzie ten ten ten do zoomow
                                            //no dobra, to smigamy
                                            if (String.IsNullOrEmpty(Values[0])) break;
                                            String[] ZoomParams = Values[0].Split(_WHITESPACE);
                                            StringBuilder ZoomLink = new StringBuilder();
                                            ZoomLink.AppendFormat("{0}.aspx?{1}", (ZoomParams[0][0] == '~') ? ResolveUrl(ZoomParams[0]) : ZoomParams[0], SetParameters(ZoomParams));
                                            //Columns[Iteration][_TD_LINK] = ZoomLink.ToString();
                                            Column[_TD_LINK] = ZoomLink.ToString();
                                            break;
                                        case 2:
                                            //Columns[Iteration][_TOOLTIP] = Values[0];
                                            Column[_TOOLTIP] = Values[0];
                                            break;
                                    }
                                }
                                //else Columns[Iteration][Values[0]] = Values[1];
                                else Column[Values[0]] = Values[1];
                            }
                            Columns.Add(Column);
                        }
                        break;
                }
            }
            newp = ParsedQuery.Contains(__Start);

            switch (Action)
            {
                case ActionType.Pages:
                    ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __Start),
                        String.Format("ROW_NUMBER() over ({0}) as {1},", String.Format("order by {0}", CurrentSorting), ___ENUMERATOR));
                    if (ElementsCounter != 0)
                    {
                        ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __Begin),
                            String.Format("select top {0} * from (", ElementsCounter));
                        ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __End),
                            String.Format(") _TABLE where {1} > {0}", ElementsCounter * (CurrentPage - 1), ___ENUMERATOR));
                    }
                    break;
                case ActionType.Count:
                    ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __Begin), String.Format("select COUNT(*) from ("));
                    ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __End), String.Format(") _TABLE"));
                    break;
                case ActionType.One:
                    ParsedQuery = ParsedQuery.Replace(String.Format("/*{0}*/", __Start), String.Format("top 1"));
                    break;
            }
            return ParsedQuery;
        }

        // Old methods
        private String SetParameters(String[] ZoomParams)
        {
            StringBuilder Parameters = new StringBuilder();
            int j = 1;
            for (; j < ZoomParams.Length; j++)
            {
                /*Parameters.AppendFormat("&p{1}={0}",
                    ZoomParams[j], j);*/
                Parameters.AppendFormat("|{0}", ZoomParams[j]);
            }
            //Parameters.AppendFormat("&p{0}=", j + 1);
            Parameters.AppendFormat("|");
            /*return (Grid.cryptParams)
                ? String.Format("p={0}", Report.EncryptQueryString(Parameters.ToString().Substring(1), Grid.key, Grid.salt))
                : Parameters.ToString().Substring(1);*/
            return Parameters.ToString().Substring(1);
        }
        protected List<String> GetParameters()
        {
            List<String> Parameters = new List<String>();
            if (Grid.cryptParams)   // BEZPIECZENSTWO !!!
            {
                String p = Tools.GetStr(Request.QueryString["p"]);
                if (!String.IsNullOrEmpty(p))
                    try
                    {
                        //foreach (String Union in Report.DecryptQueryString(p, Grid.key, Grid.salt).Split('&'))
                        //    Parameters.Add(Union.Split('=')[1]);
                        foreach (String Union in Report.DecryptQueryString(p, Grid.key, Grid.salt).Split('|'))
                            Parameters.Add(Union);
                    }
                    catch
                    {
                        Parameters.Add("1");
                    }
            }
            else for (int i = 0; i < Request.QueryString.Count; i++) Parameters.Add(Request.QueryString[i]);
            return Parameters;
        }
        public String GetParam(int v)
        {
            List<String> Params = GetParameters();
            return Params[v];
        }
        private string PrepareText(string value) //Tomek
        {
            string t = ParseSQL(value, ActionType.None);
            if (t != null && t.StartsWith("select"))
                return db.getScalar(t);
            else
                return L.p(t);
        }
        public void GenerateReportHeaders()
        {
            cntReportHeader1.Caption = PrepareText(FTitle);
            cntReportHeader1.Caption1 = PrepareText(FTitle2);
            cntReportHeader1.Caption2 = PrepareText(FTitle3);
            cntReportHeader1.Caption3 = PrepareText(FTitle4);

            cntReportHeader1.Visible = !cntReportHeader1.IsEmpty;
            return;
        }

        // Override of base methods
        protected override void OnLoad(EventArgs e)
        {
            if (Table)
            {
                //skiddy
                DataTable dt;
                try
                {
                    dt = db.getDataSet(ParseSQL(SQL, ActionType.One)).Tables[0];
                    foreach (DataColumn dc in dt.Columns)
                    {
                        ColumnNames.Add(dc.ColumnName);
                    }

                }
                catch (Exception ex)
                {
                    String asd = ex.ToString(); // debug purposes
                    dt = null;
                }

                // Ustawienie sortowania domyślnego
                if (String.IsNullOrEmpty(CurrentSorting) && ColumnNames.Count > 0)
                {
                    CurrentSorting = String.Format("{0}{1}{2}", ColumnNames[0], _WHITESPACE, _ASC);
                }


                int Iteration = 0;
                //Page.Form.Controls.Add(pControls);
                foreach (Dictionary<String, String> column in Columns)
                {
                    if (column != null)
                    {
                        LinkButton lnk = new LinkButton();
                        lnk.CommandArgument = Iteration.ToString();
                        lnk.Click += new EventHandler(lnk_Click);
                        lnk.ID = _ID_HeaderLink + Iteration.ToString();
                        pControls.Controls.Add(lnk);

                    }
                    Iteration++;
                }

                //nie skiddy
                for (int i = 0; i < VisiblePages + 6; i++)
                {
                    LinkButton PagerPage = new LinkButton();
                    PagerPage.ID = _ID_PagerControls + i.ToString();
                    PagerPage.Click += new EventHandler(PagerPage_Click);
                    pControls.Controls.Add(PagerPage);
                }

            }


            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            CreateChart();

            if (DoRecreate && Table)
                ReloadTable();
            base.OnPreRender(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Table)
            {
                GenerateReportHeaders();
                ReloadTable();
            }
            if (!Table) cntReportHeader1.Visible = false;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SelectScript", 
String.Format(
@"function SelectRow(rowId, cb, rowValue) 
{{
    var hidChecked = document.getElementById('{0}');
    var row = document.getElementById(rowId); 
    if(cb.checked) 
    {{
        row.className = 'selected'; 
        hidChecked.value += ';' + rowValue + ';';
    }}
    else 
    {{
        row.className = '';
        hidChecked.value = hidChecked.value.replace(';' + rowValue + ';', ''); 

    }}

    ////////////////////////alert(hidChecked.value);

    
}}", hidCheck.ClientID), true);
        }

        // Chart Section
        protected void CreateChart()
        {
            if (!String.IsNullOrEmpty(SelectedChart))
            {
                MainChart.Type = SelectedChart;
                MainChart.Visible = true;

                DataTable dtCharts = null;

                try
                {
                    dtCharts = db.getDataSet(ParseSQL(SQL, ActionType.One)).Tables[0];
                }
                catch
                {
                    return;
                    //dtCharts = null;
                }
                if (!dtCharts.Equals(null))
                {
                    SetPaging();

                    Dictionary<String, String> column = null;

                    dtCharts = SetFilter(dtCharts);
                    dtCharts = SetSorting(dtCharts);

                    MainChart.Names = String.Empty;
                    MainChart.Values = String.Empty;

                    String CurrentDS = String.Empty;

                    for (int j = (newp) ? 1 : 0; j < dtCharts.Columns.Count; j++)
                    {
                        column = Columns[(newp) ? j - 1 : j];
                        if (GetColumnProperty(column, _CHART).Equals(_CHARTSNAMES))
                        {
                            for (int i = (newp) ? 0 : ((ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0);
                    (newp) ? i < dtCharts.Rows.Count : ((i < dtCharts.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true));
                    i++)
                            {
                                MainChart.Names += "|" + dtCharts.Rows[i][(newp) ? j - 1 : j];
                            }
                            if (MainChart.Names.Length > 0)
                                MainChart.Names = MainChart.Names.Substring(1);
                            MainChart.Names += ";";
                        }
                        if (GetColumnProperty(column, _CHART).Equals(_CHARTSVALUES))
                        {
                            for (int i = (newp) ? 0 : ((ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0);
                    (newp) ? i < dtCharts.Rows.Count : ((i < dtCharts.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true));
                    i++)
                            {
                                CurrentDS += (!String.IsNullOrEmpty(dtCharts.Rows[i][(newp) ? j - 1 : j].ToString())) ? ("," + dtCharts.Rows[i][(newp) ? j - 1 : j]) : ",0";
                            }
                            if (CurrentDS.Length > 0 && CurrentDS.StartsWith(","))
                                CurrentDS = CurrentDS.Substring(1);
                            MainChart.Values += CurrentDS + ";";
                            CurrentDS = String.Empty;
                            if (SelectedChart.Equals(Chart.Pie) ||
                                SelectedChart.Equals(Chart.Polar) ||
                                SelectedChart.Equals(Chart.Doughnut))
                                break;
                        }
                    }
                    if (MainChart.Names.EndsWith(";"))
                        MainChart.Names = MainChart.Names.Remove(MainChart.Names.Length - 1);
                    if (MainChart.Values.EndsWith(";"))
                        MainChart.Values = MainChart.Values.Remove(MainChart.Values.Length - 1);
                }

                MainChart.Colors = ChartsColors;
                MainChart.Options = ChartsOptions;

            }
            else
            {
                MainChart.Visible = false;
            }
        }
        public void CreateChart(cntChart ChartToCreate)
        {
            DataTable dtCharts = null;

            try
            {
                dtCharts = db.getDataSet(ParseSQL(SQL, ActionType.One)).Tables[0];
            }
            catch
            {
                dtCharts = null;
            }

            if (!dtCharts.Equals(null))
            {
                SetPaging();

                Dictionary<String, String> column = null;

                dtCharts = SetFilter(dtCharts);
                dtCharts = SetSorting(dtCharts);

                ChartToCreate.Names = String.Empty;
                ChartToCreate.Values = String.Empty;

                String CurrentDS = String.Empty;

                for (int j = (newp) ? 1 : 0; j < dtCharts.Columns.Count; j++)
                {
                    column = Columns[(newp) ? j - 1 : j];
                    if (GetColumnProperty(column, _CHART).Equals(_CHARTSNAMES))
                    {
                        for (int i = (newp) ? 0 : ((ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0);
                (newp) ? i < dtCharts.Rows.Count : ((i < dtCharts.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true));
                i++)
                        {
                            ChartToCreate.Names += "|" + dtCharts.Rows[i][(newp) ? j - 1 : j];
                        }
                        if (ChartToCreate.Names.Length > 0)
                            ChartToCreate.Names = ChartToCreate.Names.Substring(1);
                        ChartToCreate.Names += ";";
                    }
                    if (GetColumnProperty(column, _CHART).Equals(_CHARTSVALUES))
                    {
                        for (int i = (newp) ? 0 : ((ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0);
                (newp) ? i < dtCharts.Rows.Count : ((i < dtCharts.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true));
                i++)
                        {
                            CurrentDS += (!String.IsNullOrEmpty(dtCharts.Rows[i][(newp) ? j - 1 : j].ToString())) ? ("," + dtCharts.Rows[i][(newp) ? j - 1 : j]) : ",0";
                        }
                        if (CurrentDS.Length > 0 && CurrentDS.StartsWith(","))
                            CurrentDS = CurrentDS.Substring(1);
                        ChartToCreate.Values += CurrentDS + ";";
                        CurrentDS = String.Empty;
                    }
                }
                if (ChartToCreate.Names.EndsWith(";"))
                    ChartToCreate.Names = ChartToCreate.Names.Remove(ChartToCreate.Names.Length - 1);
                if (ChartToCreate.Values.EndsWith(";"))
                    ChartToCreate.Values = ChartToCreate.Values.Remove(ChartToCreate.Values.Length - 1);
            }
        }

        protected void ddlCharts_Selected(object sender, EventArgs e)
        {
            switch (ddlCharts.SelectedValue)
            {
                case "0":
                    SelectedChart = Chart.Line;
                    break;
                case "1":
                    SelectedChart = Chart.Bar;
                    break;
                case "2":
                    SelectedChart = Chart.Pie;
                    break;
                case "3":
                    SelectedChart = Chart.Radar;
                    break;
                case "4":
                    SelectedChart = Chart.Polar;
                    break;
                case "5":
                    SelectedChart = Chart.Doughnut;
                    break;
                default:
                    SelectedChart = String.Empty;
                    break;
            }
        }
        protected void ddlCharts_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Charts))
            {
                ddlCharts.Visible = false;
                return;
            }
            if (Charts.Equals("All"))
            {
                foreach (ListItem Item in ddlCharts.Items)
                {
                    Item.Enabled = true;
                }
                return;
            }
            int ActiveCharts = 0;
            String AcceptableChart = String.Empty;
            for (int i = 0; i < Charts.Split(_ChartsSplitter).Length; i++)
            {
                ++ActiveCharts;
                switch (Charts.Split(_ChartsSplitter)[i].Trim().ToLower())
                {
                    case Chart.Line:
                        ddlCharts.Items[1].Enabled = true;
                        AcceptableChart = Chart.Line;
                        break;
                    case Chart.Bar:
                        ddlCharts.Items[2].Enabled = true;
                        AcceptableChart = Chart.Bar;
                        break;
                    case Chart.Pie:
                        ddlCharts.Items[3].Enabled = true;
                        AcceptableChart = Chart.Pie;
                        break;
                    case Chart.Radar:
                        ddlCharts.Items[4].Enabled = true;
                        AcceptableChart = Chart.Radar;
                        break;
                    case Chart.Polar:
                        ddlCharts.Items[5].Enabled = true;
                        AcceptableChart = Chart.Polar;
                        break;
                    case Chart.Doughnut:
                        ddlCharts.Items[6].Enabled = true;
                        AcceptableChart = Chart.Doughnut;
                        break;
                    case Chart.SpeedoMeter:
                        AcceptableChart = Chart.SpeedoMeter;
                        break;
                    case Chart.Fusion:
                        AcceptableChart = Chart.Fusion;
                        break;
                    default:
                        --ActiveCharts;
                        break;
                }
            }
            if (ActiveCharts > 1)
            {
                ddlCharts.Visible = true;
            }
            else
            {
                ddlCharts.Visible = false;
                SelectedChart = AcceptableChart;
            }

        }
        public String Charts
        {
            get { return ViewState[_CHARTS] as String; }
            set { ViewState[_CHARTS] = value; }
        }
        public String ChartsColors
        {
            get { return ViewState[_CHARTSCOLORS] as String; }
            set { ViewState[_CHARTSCOLORS] = value; }
        }
        public String ChartsOptions
        {
            get { return ViewState[_CHARTSOPTIONS] as String; }
            set { ViewState[_CHARTSOPTIONS] = value; }
        }
        public String ChartsWidth
        {
            get { return MainChart.Width.ToString(); }
            set { MainChart.Width = Convert.ToInt32(value); }
        }
        public String ChartsHeight
        {
            get { return MainChart.Height.ToString(); }
            set { MainChart.Height = Convert.ToInt32(value); }
        }
        public String SelectedChart
        {
            get { return ViewState[_VSELECTEDCHART] as String; }
            set { ViewState[_VSELECTEDCHART] = value; }
        }

        public const String _CHART = "charts";
        public const String _CHARTS = "_CHARTS";
        public const String _CHARTSNAMES = "names";
        public const String _CHARTSVALUES = "values";
        public const String _CHARTSCOLORS = "_CHARTSCOLORS";
        public const String _CHARTSOPTIONS = "_CHARTSOPTIONS";
        //public const String _CHARTSWIDTH = "_CHARTSWIDTH";
        //public const String _CHARTSHEIGHT = "_CHARTSHEIGHT";

        public void SetProperties(Dictionary<String, String> Data)
        {

            //Dictionary<String, String> Properties = Presentable.ParseProperties(Data);

            foreach (KeyValuePair<String, String> Property in Data)
            {
                switch (Property.Key.ToLower())
                {
                    case "sql":
                        SQL = Property.Value;
                        break;
                    case "charts":
                        Charts = Property.Value;
                        break;
                    case "chartscolors":
                        ChartsColors = Property.Value;
                        break;
                    case "title":
                        Title = Property.Value;
                        break;
                    case "rowclass":
                        RowClass = Property.Value;
                        break;
                    case "divclass":
                        DivClass = Property.Value;
                        break;
                    case "alternaterowclass":
                        AlternateRowClass = Property.Value;
                        break;
                    case "pager":
                        Pager = Convert.ToBoolean(Property.Value);
                        break;
                    case "table":
                        Table = Convert.ToBoolean(Property.Value);
                        break;
                    case "chartswidth":
                        ChartsWidth = Property.Value;
                        break;
                    case "chartsheight":
                        ChartsHeight = Property.Value;
                        break;
                    case "chartsoptions":
                        ChartsOptions = Property.Value;
                        break;
                }
            }
        }

        //NIE WIEM CO TO ZA SEKCJA
        String FormatList(String list)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < list.Split(';').Length; i++)
                if (!String.IsNullOrEmpty(list.Split(';')[i]))
                    output.AppendFormat("{1}{0}", list.Split(';')[i], (i != 0) ? " or " : "");
            return output.ToString();
        }

        // Generate Methods
        protected String GenerateHeaders()
        {
            StringBuilder sb = new StringBuilder();
            int HeadersHeight = 0;

            foreach (Dictionary<String, String> Column in Columns)
            {
                if (Column != null)
                {
                    if (Column[_TABLEHEADER] != null)
                    {
                        if (Column[_TABLEHEADER].Split(_Also)[0].Split(_Level).Length > HeadersHeight)
                            HeadersHeight = Column[_TABLEHEADER].Split(_Also)[0].Split(_Level).Length;
                    }
                }
            }
            List<string>[] HeadersList = new List<string>[HeadersHeight];

            foreach (Dictionary<String, String> Column in Columns)
            {
                if (Column != null)
                {
                    string[] ColHeaders = new string[HeadersHeight];
                    for (int i = 0; i < HeadersHeight; i++)
                    {
                        try
                        {

                            //if(Column[_TABLEHEADER].Split(_Also).Length > 1)                 cos trza zrobic zeby nie robil hiddenow
                            //    if (Column[_TABLEHEADER].Split(_Also)[1].Equals(_Hidden))
                            //      throw new Exception();

                            ColHeaders[i] = Column[_TABLEHEADER].Split(_Also)[0].Split(_Level)[i];
                        }
                        catch
                        {
                            ColHeaders[i] = String.Empty;
                        }
                    }

                    //Array.Reverse(ColHeaders);

                    for (int i = 0; i < ColHeaders.Length; i++)
                    {
                        if (HeadersList[i] == null)
                        {
                            HeadersList[i] = new List<string>();
                            HeadersList[i].Add(ColHeaders[i]);
                        }
                        else
                        {
                            HeadersList[i].Add(ColHeaders[i]);
                        }
                    }
                }
            }
            bool lowest = false;
            bool First = true;
            for (int i = 0; i < HeadersList.Length; i++) //wiersze
            {
                lowest = (i == (HeadersList.Length - 1));

                sb.Append(@"
    <tr>");
                int colspan = 1;
                for (int j = 0; j < HeadersList[i].Count; j++)
                {
                    bool create = false;
                    if (j == (HeadersList[i].Count - 1)) // jak ostatnia cellka
                        create = true;
                    else
                    {
                        if (HeadersList[i][j] == HeadersList[i][j + 1])
                        {
                            try
                            {
                                if (HeadersList[i][j + 1][0] != '!') //
                                    colspan++;
                            }
                            catch
                            {
                                colspan++;
                            }
                        }
                        else
                        {
                            create = true;
                            if (HeadersList[i][j + 1].Length > 0)
                                if (HeadersList[i][j + 1][0] == '!')
                                    HeadersList[i][j + 1] = HeadersList[i][j + 1].Remove(0, 1);
                        }
                    }
                    if (create) // jak ma zrobic cellke
                    {

                        if ((Columns[j][_TABLEHEADER].Split(_Also).Length > 1) ? (!Columns[j][_TABLEHEADER].Split(_Also)[1].Equals(_Hidden.ToString()))
                            : true)
                        {
                            sb.AppendFormat(@"
        <th colspan=""{0}"">{1}
        </th>", colspan, GenerateLink(HeadersList[i][j], j, lowest, First));
                            colspan = 1;
                            if (lowest)
                                First = false;
                        }
                    }
                }
                sb.Append(@"
    </tr>");
            }
            return sb.ToString();
        }
        protected String GenerateFooter(int ColumnsCount, Dictionary<int, FooterElements> CFooterElements, DataTable FooterTable)
        {
            StringBuilder Footer = new StringBuilder();
            String Property = null;
            //int Sum = -1;
            //int Count = -1;
            //-------------------
            //foreach (object Column in (ActualTable.Rows[0].ItemArray))
            if (CFooterElements != null)
            {
                Footer.Append(@"
    <tr class=""sum"">");
                for (int i = (newp) ? 1 : 0; i < ((newp) ? ColumnsCount - 1 : ColumnsCount)/*Rows[0].ItemArray.Length*/; i++)
                {

                    if (Columns[i][_TABLEHEADER].Split(_Also).Length > 1)
                        if (Columns[i][_TABLEHEADER].Split(_Also)[1].Equals(_Hidden.ToString()))
                            continue;
                    Footer.Append(@"
        <td>");
                    Property = GetColumnProperty(Columns[i], _FOOTER);
                    if (!String.IsNullOrEmpty(Property))
                    {
                        switch (Property)
                        {
                            case _COUNT:
                                try
                                {
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>", CFooterElements[i].Count, "Ilość");
                                }
                                catch
                                {
                                    CFooterElements[i] = new FooterElements();
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>", CFooterElements[i].Count, "Ilość");

                                }
                                break;
                            case _SUM:
                                try
                                {
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>", CFooterElements[i].Sum, "Suma");
                                }
                                catch
                                {
                                    CFooterElements[i] = new FooterElements();
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>", CFooterElements[i].Sum, "Suma");
                                }
                                break;
                            case _MEAN:
                                try
                                {
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>",
                                        ((double)CFooterElements[i].Sum) / ((double)CFooterElements[i].Count), "Średnia");
                                }
                                catch
                                {
                                    CFooterElements[i] = new FooterElements();
                                    Footer.AppendFormat(@"<span title=""{1}"">{0}</span>",
                                        ((double)CFooterElements[i].Sum) / ((double)CFooterElements[i].Count), "Średnia");
                                }
                                break;
                        }
                    }
                    else Footer.Append("&nbsp;");
                    Footer.Append(@"
        </td>");

                }
                Footer.Append(@"
    </tr>");
            }
            //-------------------
            if (FooterTable != null)
            {
                Footer.Append(@"
    <tr class=""sum"">");
                foreach (object Column in (FooterTable.Rows[0].ItemArray))
                {
                    Footer.AppendFormat(@"
        <td>
            {0}
        </td>", Column.ToString());
                }
                Footer.Append(@"
    </tr>");
            }
            return Footer.ToString();
        }
        protected void GeneratePager(int Rows)
        {
            int Pages = (ElementsCounter != 0) ?
                (Rows % ElementsCounter == 0) ? (Rows / ElementsCounter) : (Rows / ElementsCounter) + 1
                : 1;
            LinkButton CurrentButton = null;
            int k = (((CurrentPage % VisiblePages == 0) ? (CurrentPage / VisiblePages) - 1 : (CurrentPage / VisiblePages)) * VisiblePages + 1);
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, 0) as LinkButton;
            CurrentButton.Text = "|◄";
            CurrentButton.CssClass = "nav";
            if (CurrentPage > 1)
            {
                CurrentButton.CommandArgument = 1.ToString();
                CurrentButton.Enabled = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Enabled = false;
            }
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, 1) as LinkButton;
            CurrentButton.Text = "◄";
            CurrentButton.CssClass = "nav";
            if (CurrentPage > 1)
            {
                CurrentButton.CommandArgument = (CurrentPage - 1).ToString();
                CurrentButton.Enabled = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Enabled = false;
            }
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, 2) as LinkButton;
            CurrentButton.Text = "...";
            CurrentButton.CssClass = null;
            if (k - 1 != 0)
            {
                CurrentButton.CommandArgument = (k - 1).ToString();
                CurrentButton.Visible = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Visible = false;
            }
            int Iteration = 0;
            for (; k < (((CurrentPage % VisiblePages == 0) ? (CurrentPage / VisiblePages) - 1 :
                (CurrentPage / VisiblePages)) * VisiblePages + (VisiblePages + 1)) && k < Pages + 1;
                //tutaj jest syf, bo 5/5 to 1 a nie 0 
                k++)
            {
                Iteration++;
                CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, 2 + Iteration) as LinkButton;
                CurrentButton.Text = k.ToString();
                CurrentButton.CommandArgument = k.ToString();
                CurrentButton.CssClass = null;
                CurrentButton.Visible = true;
                if (k == CurrentPage)
                {
                    CurrentButton.Enabled = false;
                    CurrentButton.Attributes.Add("style", "background-color: #FED731; color: #375C8E; border: 1px solid #DDD;");
                }
                else
                {
                    CurrentButton.Enabled = true;
                    CurrentButton.Attributes.Remove("style");
                }
            }
            Iteration++;
            //TUTAJ JAKIES CZYSZCZENIE JAKBVY COS ZLE POSZLO
            for (; Iteration < VisiblePages + 1; Iteration++)
            {
                CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, 2 + Iteration) as LinkButton;
                CurrentButton.Text = null;
                CurrentButton.CommandArgument = null;
                CurrentButton.Visible = false;
                CurrentButton.CssClass = null;
            }
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, VisiblePages + 3) as LinkButton;
            CurrentButton.Text = "...";
            CurrentButton.CssClass = null;
            if (k < Pages + 1)
            {
                CurrentButton.CommandArgument = k.ToString();
                CurrentButton.Visible = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Visible = false;
            }
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, VisiblePages + 4) as LinkButton;
            CurrentButton.Text = "►";
            CurrentButton.CssClass = "nav";
            if (CurrentPage < Pages)
            {
                CurrentButton.CommandArgument = (CurrentPage + 1).ToString();
                CurrentButton.Enabled = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Enabled = false;
            }
            CurrentButton = DynamicControl.GetControl(pControls, _ID_PagerControls, VisiblePages + 5) as LinkButton;
            CurrentButton.Text = "►|";
            CurrentButton.CssClass = "nav";
            if (CurrentPage < Pages)
            {
                CurrentButton.CommandArgument = Pages.ToString();
                CurrentButton.Enabled = true;
            }
            else
            {
                CurrentButton.CommandArgument = null;
                CurrentButton.Enabled = false;
            }
        }
        public String GenerateLink(String Text, int index, bool lowest, bool First)
        {
            if (!lowest)
                return Text;
            StringBuilder sb = new StringBuilder();

            LinkButton HeaderLink = DynamicControl.GetControl(pControls, _ID_HeaderLink, index) as LinkButton;
            HeaderLink.Text = Text;
            sb.Append(DynamicControl.RenderControl(HeaderLink));
            //TUTAJ STRZALKI 

            if (!String.IsNullOrEmpty(LastSort))
            {
                if (ColumnNames[index] == LastSort)
                    sb.AppendFormat("<span>&nbsp;{0}</span>", (Sorting[LastSort] == _ASC) ? "▼" : "▲");
            }
            else
            {
                if (First)
                {
                    sb.Append("<span>&nbsp;▼</span>");
                    //LastSort = String.Format("{0}{1}{2}", ColumnNames[index], _WHITESPACE, _ASC);//LastSort = ColumnNames[index] + " " + _ASC;
                }
            }

            return sb.ToString();
        }
        protected String GenerateTable()
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = db.getDataSet(ParseSQL(SQL, ActionType.Pages));
            DataSet dsc = db.getDataSet(ParseSQL(SQL, ActionType.Count));
            DataTable dt = ds.Tables[0];
            DataTable dt2 = null;
            if (ds.Tables.Count > 1)
                dt2 = (ds.Tables[1] != null) ? ds.Tables[1] : null;
            Dictionary<int, FooterElements> CFooterElements = null;
            Dictionary<String, String> column = null;
            String Property = null;

            dt = SetFilter(dt);
            dt = SetSorting(dt);

            //to wrzuce w metode
            foreach (Dictionary<String, String> Column in Columns)
            {
                if (Column != null)
                {
                    FormatTokens[GetColumnIndex(Column)] = GetFormatTokens(Column);
                }
            }

            SetFooter();
            SetPaging();

            if (Pager) GeneratePager((newp) ? (int)dsc.Tables[0].Rows[0][0] : dt.Rows.Count);

            sb.AppendFormat(@"
<table name=""report"" class=""{0}"" cellspacing =""0"" cellpadding = ""0"">", TableClass);

            sb.Append(GenerateHeaders());

            /*for (int i = (ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0;
    (i < dt.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true);
    i++)               //FORKA OD WSZYSTKIEGO JA JA BEDE POTEM POTRZEBOWAL*/
            //for (int i = 0; i < dt.Rows.Count; i++)

            // ULTYMATYWNA PETLA DO WYPISYWANIA DANYCH POZDRO DLA KUMATYCH
            //support pod stary i nowy (RCP) system, if newp then nowy else stary
            for (int i = (newp) ? 0 : ((ElementsCounter != 0) ? ((CurrentPage - 1) * ElementsCounter) : 0);
                (newp) ? i < dt.Rows.Count : ((i < dt.Rows.Count) && ((ElementsCounter != 0) ? (i < CurrentPage * ElementsCounter) : true));
                i++)
            {                                                           //スキディはバカです。
                sb.AppendFormat(@"
    <tr class=""{0}"" id=""{1}"" >", (i % 2 == 0) ? AlternateRowClass : RowClass, String.Format(RowId, i));
                for (int j = (newp) ? 1 : 0; j < dt.Columns.Count; j++)
                {
                    column = Columns[(newp) ? j - 1 : j];
                    Boolean Num = false;
                    try
                    {
                        Num = (FormatTokens[GetColumnIndex(column)].Count > 0) ? FormatTokens[GetColumnIndex(column)][0] == "N" : false;
                    }
                    catch { }

                    try
                    {
                        if (column[_TABLEHEADER].Split(_Also)[1].Equals(_Hidden.ToString()))
                        {
                            continue;
                        }
                    }
                    catch { }
                    //if (column != null)
                    {
                        sb.AppendFormat(@"
        <td style=""{0}"" class=""{2}"">{1}</td>", Num ? "text-align: right;" : GetColumnProperty(column, _TD_STYLE),
                                                   GetData(column, dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString(), dt.Rows[i], i),
                                                   (dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString().Equals("0")) ? "zero" : "default"); //ELITA
                        //TUTAJ SYF DO FOOTEROW
                        Property = GetColumnProperty(column, _FOOTER);
                        if (!String.IsNullOrEmpty(Property))
                        {
                            if (CFooterElements == null) CFooterElements = new Dictionary<int, FooterElements>();
                            switch (Property)
                            {
                                case _COUNT:
                                    try
                                    {
                                        if (dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]] != DBNull.Value) CFooterElements[j].Count++;
                                    }
                                    catch //(Exception e)
                                    {
                                        CFooterElements[j] = new FooterElements();
                                        CFooterElements[j].Count++;
                                    }
                                    break;
                                case _SUM:
                                    try
                                    {
                                        int Temp = 0;
                                        int.TryParse(dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString(), out Temp);
                                        CFooterElements[j].Sum += Temp;
                                    }
                                    catch
                                    {
                                        CFooterElements[j] = new FooterElements();
                                        int Temp = 0;
                                        int.TryParse(dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString(), out Temp);
                                        CFooterElements[j].Sum += Temp;
                                    }
                                    break;
                                case _MEAN:
                                    try
                                    {
                                        int Temp = 0;
                                        int.TryParse(dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString(), out Temp);
                                        CFooterElements[j].Sum += Temp;
                                    }
                                    catch
                                    {
                                        CFooterElements[j] = new FooterElements();
                                        int Temp = 0;
                                        int.TryParse(dt.Rows[i][ColumnNames[(newp) ? j - 1 : j]].ToString(), out Temp);
                                        CFooterElements[j].Sum += Temp;
                                    }
                                    CFooterElements[j].Count++;
                                    break;
                            }
                        }
                        //TUTAJ JUZ NIE
                    }
                }
                sb.Append(@"
    </tr>");
            }
            sb.Append(GenerateFooter(dt.Columns.Count, CFooterElements, dt2));
            sb.Append(@"
</table>");
            if (Pager)
            {
                sb.Append(@"
<table class=""ListView1 narrow"">
    <tr class=""pager"">
        <td class=""left"">");
                StringBuilder sb2 = new StringBuilder();
                List<LinkButton> PagerLinks = DynamicControl.GetLinkButtonList(pControls, _ID_PagerControls);
                foreach (LinkButton PagerLink in PagerLinks)
                {
                    sb2.Append(DynamicControl.RenderControl(PagerLink));
                    if (PagerLink.CssClass != "nav" && PagerLink.CommandArgument != String.Empty || PagerLink.CssClass == "nav")
                        sb2.Append("&nbsp;");
                }
                sb.Append(sb2);

                //for (int j = 0; j < PagerLinks.Controls.Count; j++)
                //{
                //    if (PagerLinks.Controls[j] is LinkButton)
                //    {
                //        sb2 = new StringBuilder();
                //        //sw = new StringWriter(sb2);
                //        //writer = new HtmlTextWriter(sw);

                //        //((LinkButton)PagerLinks.Controls[j]).RenderControl(writer); JAK COŚ

                //        sb2.Append(DynamicControl.RenderControl(PagerLinks.Controls[j]));


                //        if ((((LinkButton)PagerLinks.Controls[j]).CssClass != "nav"
                //            &&
                //            ((LinkButton)PagerLinks.Controls[j]).CommandArgument != String.Empty)
                //            ||
                //            ((LinkButton)PagerLinks.Controls[j]).CssClass == "nav") sb2.Append("&nbsp;");
                //        sb.Append(sb2);
                //    }
                //}

                sb.Append(@"
        </td>
        <td class=""right"">");
                sb.AppendFormat(@"
            <span class=""count"">{0}</span>", L.p("Pokaż na stronie:"));
                sb2 = new StringBuilder();
                //sw = new StringWriter(sb2);
                //writer = new HtmlTextWriter(sw); //JAK COŚ
                ddlPager.Visible = true;
                sb2.Append(DynamicControl.RenderControl(ddlPager));
                ddlPager.Visible = false;
                sb.Append(sb2);
                sb.Append(@"
        </td>
    </tr>");
                sb.Append(@"
</table>");
            }
            ddlPager.Visible = false;
            DoRecreate = false;
            return sb.ToString();
        }

        // Get Methods
        public String GetFormattedData(String Data, String Format)
        {
            switch (Format)
            {
                case FDate:
                    try
                    {
                        return String.Format("{0:d}", Convert.ToDateTime(Data));
                    }
                    catch
                    {
                        return Data;
                    }
                case FDateTime:

                    break;
                case FNum:

                    break;
            }
            return Data;
        }
        public int GetColumnIndex(Dictionary<String, String> column)
        {
            for (int i = 0; i < Columns.Count/*Length*/; i++)
            {
                if (Columns[i] == column)
                    return i;
            }
            return -1;
        }
        public String GetColumnProperty(Dictionary<String, String> column, String property)
        {
            String s = String.Empty;
            if (column != null)
                column.TryGetValue(property, out s);
            else
                return String.Empty;
            if (String.IsNullOrEmpty(s))
                return String.Empty;
            return s;
        }
        public String GetColumnProperty(int index, String property)
        {
            String s = "";
            Dictionary<String, String> column = Columns[index];
            column.TryGetValue(property, out s);
            return s;

        }
        public String GetData(Dictionary<String, String> Column, String Data, DataRow Dr, Int32 RowIndex) // tu sprawdzanie czy jest linkiem, czy jest jakis format itp
        {
            String Output = Data;

            if (!String.IsNullOrEmpty(GetColumnProperty(Column, _TD_FORMAT))) //jak jest formatowanie to rob syf STARE
                Output = GetFormattedData(Output, Column[_TD_FORMAT].ToString());
            else
            {
                if (Column != null)
                {
                    foreach (String Token in FormatTokens[GetColumnIndex(Column)])
                    {
                        Output = GetFormattedData(Output, Token);
                    }
                }
            }

            // formatowanie

            if (!String.IsNullOrEmpty(GetColumnProperty(Column, _TD_LINK))) //jak jest linkiem
            {
                List<String> Values = new List<String>();
                StringBuilder TemporaryValue = null;
                String ActualLink = GetColumnProperty(Column, _TD_LINK);
                for (int i = 0; i < ActualLink.Length; i++)
                {
                    if (ActualLink[i].Equals('@'))
                    {
                        TemporaryValue = new StringBuilder();
                        for (; (i < ActualLink.Length) ? (!ActualLink[i].Equals(_WHITESPACE) &&
                            !ActualLink[i].Equals('|')) : false; i++) TemporaryValue.Append(ActualLink[i]); //&
                        Values.Add(TemporaryValue.ToString());
                    }
                }
                foreach (String Value in Values)
                {
                    int index = -1;

                    for (int i = 0; i < Columns.Count/*255*/; i++)
                    {
                        if (Columns[i] != null)
                        {
                            if (Columns[i][_TABLEHEADER].Split(_Also)[0].Equals(Value.Substring(1, Value.Length - 1)))
                                index = i;
                        }
                    }
                    if (!index.Equals(-1))
                        ActualLink = ActualLink.Replace(Value, Dr[(newp) ? index + 1 : index].ToString());
                }
                if (Grid.cryptParams)
                    ActualLink = ActualLink.Replace(ActualLink.Substring(ActualLink.IndexOf('?')),
                        String.Format("?p={0}", (Report.EncryptQueryString(ActualLink.Substring(ActualLink.IndexOf('?') + 1), Grid.key, Grid.salt))));
                Output = String.Format(@"<a href=""{0}"">{1}</a>", ActualLink, Output);
            }

            // checkbox

            if (!String.IsNullOrEmpty(GetColumnProperty(Column, _TD_CHECKBOX)))
            {
                String CBParam = GetColumnProperty(Column, _TD_CHECKBOX);
                int index = -1;
                for (int i = 0; i < Columns.Count/*255*/; i++)
                {
                    if (Columns[i] != null)
                    {
                        //String asd1 = Columns[i][_TABLEHEADER].Split(_Also)[0];
                        //String asd2 = CBParam.Substring(1, CBParam.Length - 1);
                        if (Columns[i][_TABLEHEADER].Split(_Also)[0].Equals(CBParam.Substring(1, CBParam.Length - 1)))
                            index = i;
                    }
                }
                if (!index.Equals(-1))
                    CBParam = CBParam.Replace(CBParam, Dr[(newp) ? index + 1 : index].ToString());
               Output = String.Format(@"<input type=""checkbox"" onclick=""SelectRow('{0}', this, {1});"" >", String.Format(RowId, RowIndex), CBParam);
            }

            return Output;
        }
        protected String GetSortingType(String column)
        {
            if (Sorting == null)
            {
                Sorting = new Dictionary<string, string>();
                Sorting.Add(column, _DESC);
                ViewState[Constants.Sorting] = Sorting;
                return _DESC;
            }
            String output = String.Empty;
            String current = String.Empty;
            Sorting.TryGetValue(column, out current);

            if (String.IsNullOrEmpty(current))
                Sorting[column] = _ASC;
            else if (current == _ASC)
                Sorting[column] = _DESC;
            else if (current == _DESC)
                Sorting[column] = _ASC;

            ViewState[Constants.Sorting] = Sorting;
            return Sorting[column];
        }
        protected String GetFilters()
        {
            StringBuilder sb = new StringBuilder();
            List<String> Filters = new List<String>();

            Dictionary<String, String> dFilters = ViewState[Constants.Filters] as Dictionary<String, String>;
            if (dFilters == null)
                return String.Empty;

            foreach (String Filter in dFilters.Values)
            {
                if (!String.IsNullOrEmpty(Filter))
                    Filters.Add(Filter);
            }

            if (Filters == null || Filters.Count == 0)
                return String.Empty;


            for (int i = 0; i < Filters.Count; i++)
            {
                if (!String.IsNullOrEmpty(Filters[i]))
                {
                    sb.AppendFormat(Filters[i]);
                    if ((i != Filters.Count - 1) && !String.IsNullOrEmpty(Filters[i + 1]))
                        sb.Append(" and ");
                }
            }
            return sb.ToString();
        }
        protected List<String> GetFormatTokens(Dictionary<String, String> Column)
        {
            List<String> Tokens = new List<String>();
            String format = String.Empty;
            if (Column[_TABLEHEADER].Split(_Also).Length > 1)
            {
                format = Column[_TABLEHEADER].Split(_Also)[1];

                Dictionary<int, List<String>> Types = new Dictionary<int, List<String>>();
                int Priority = -1;
                for (int Iterator90 = 0; Iterator90 < FormatTypes.Length; Iterator90++)
                {
                    try
                    {
                        Types[FormatTypes[Iterator90].Length].Add(FormatTypes[Iterator90]);
                    }
                    catch
                    {
                        Types[FormatTypes[Iterator90].Length] = new List<String>();
                        Types[FormatTypes[Iterator90].Length].Add(FormatTypes[Iterator90]);
                    }
                    if (FormatTypes[Iterator90].Length > Priority) Priority = FormatTypes[Iterator90].Length;
                }
                for (; !Priority.Equals(0); Priority--) if (Types[Priority] != null)
                        foreach (String Token in Types[Priority])
                            if (format.Contains(Token))
                            {
                                format = format.Replace(Token, String.Empty);
                                Tokens.Add(Token);
                            }
            }
            return Tokens;
        }


        // Set / Update Methods
        public void UpdateFilters(String ControlId, String Filter, Boolean Recreate)
        {
            if (Recreate == false)
                DoRecreate = true;
            Dictionary<String, String> Filters = ViewState[Constants.Filters] as Dictionary<String, String>;
            if (Filters == null)
                Filters = new Dictionary<string, string>();

            Filters[ControlId] = Filter;
            ViewState[Constants.Filters] = Filters;
            if (Recreate)
                ReloadTable();
        }
        protected DataTable SetSorting(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                try
                {
                    dv.Sort = CurrentSorting;
                }
                catch
                {
                    try
                    {
                        dv.Sort = ColumnNames[0] + _ASC;
                    }
                    catch { }
                }
                dt = dv.ToTable();
            }
            return dt;
        }
        public void ReloadTable()
        {
            MainTable.Text = GenerateTable();
            updMain.Update();
        }
        protected void SetPaging()
        {
            if (Pager)
            {
                if (ElementsCounter == -1)
                {
                    ElementsCounter = 10;
                    CurrentPage = 1;
                }
                try
                {
                    ddlPager.SelectedValue = ElementsCounter.ToString();
                }
                catch
                {
                    ddlPager.SelectedValue = "all";
                }
            }
            else
            {
                ElementsCounter = 0;
                CurrentPage = 1;
                ddlPager.SelectedValue = "all"; //asd
            }
        }
        protected DataTable SetFilter(DataTable dt)
        {
            String Filter = GetFilters();
            if (!String.IsNullOrEmpty(Filter))
            {
                DataRow[] rows;
                try
                {
                    rows = dt.Select(Filter);
                    if (rows.Length > 0)
                        return rows.CopyToDataTable();
                    else
                        return new DataTable();
                }
                catch { }
            }
            return dt;
        }
        protected void SetFooter()
        {
            foreach (Dictionary<String, String> Column in Columns)
            {
                if (Column != null)
                {
                    foreach (String Token in FormatTokens[GetColumnIndex(Column)])
                    {
                        switch (Token)
                        {
                            case FCount:
                                Columns[GetColumnIndex(Column)][_FOOTER] = _COUNT;
                                break;
                            case FSum:
                                Columns[GetColumnIndex(Column)][_FOOTER] = _SUM;
                                break;
                            case FMean:
                                Columns[GetColumnIndex(Column)][_FOOTER] = _MEAN;
                                break;
                        }
                    }
                }
            }
        }


        // Handlers
        protected void lnk_Click(object sender, EventArgs e)
        {
            String column = ColumnNames[int.Parse(((LinkButton)sender).CommandArgument)];
            String sort = column + " " + GetSortingType(column);
            CurrentSorting = sort;
            LastSort = column;
            CurrentPage = 1;
            ReloadTable();
        }
        protected void ddlPager_SelectedIndexChanged(Object sender, EventArgs e)
        {
            int temp = 0;
            if (ddlPager.SelectedValue == "all")
                temp = 0;
            else
                int.TryParse(ddlPager.SelectedValue, out temp);

            ElementsCounter = temp;
            CurrentPage = 1;
            ReloadTable();
        }
        protected void PagerPage_Click(Object sender, EventArgs e)
        {
            int Temp = 1;
            LinkButton Sender = sender as LinkButton;
            Int32.TryParse(Sender.CommandArgument, out Temp);
            CurrentPage = Temp;
            ReloadTable();
        }


        // Get / Sets 
        protected int CurrentPage
        {
            get
            {
                try
                {
                    return (int)ViewState[___PAGE];
                }
                catch
                {
                    ViewState[___PAGE] = 1;
                    return 1;
                }
            }
            set { ViewState[___PAGE] = value; }
        }
        protected int ElementsCounter
        {
            get
            {
                try
                {
                    return (int)ViewState[___COUNTER];
                }
                catch
                {
                    ViewState[___COUNTER] = 10;
                    return -1;
                }
            }
            set { ViewState[___COUNTER] = value; }
        }
        protected String CurrentSorting
        {
            get { return ViewState[_CURRENTSORTING] as String; }
            set { ViewState[_CURRENTSORTING] = value; }
        }
        protected Dictionary<String, String> Sorting
        {
            get { return ViewState[Constants.Sorting] as Dictionary<String, String>; }
            set { ViewState[Constants.Sorting] = value; }
        }
        protected Dictionary<int, List<String>> FormatTokens
        {
            get
            {
                if (ViewState[_VFORMATTOKENS] == null)
                {
                    ViewState[_VFORMATTOKENS] = new Dictionary<int, List<String>>();
                    return ViewState[_VFORMATTOKENS] as Dictionary<int, List<String>>;
                }
                return ViewState[_VFORMATTOKENS] as Dictionary<int, List<String>>;
            }
            set { ViewState[_VFORMATTOKENS] = value; }
        }
        protected String LastSort
        {
            get { return ViewState[_LASTSORT] as String; }
            set { ViewState[_LASTSORT] = value; }
        }
        public int Pages
        {
            get { return VisiblePages; }
            set { VisiblePages = value; }
        }
        protected Boolean DoRecreate
        {
            get
            {
                try
                {
                    return (Boolean)ViewState[_DORECREATE];
                }
                catch
                {
                    return false;
                }
            }
            set { ViewState[_DORECREATE] = value; }
        }
        protected List<String> ColumnNames
        {
            get
            {
                if (ViewState[_COLUMNNAMES] == null)
                {
                    List<String> cNames = new List<string>();
                    ViewState[_COLUMNNAMES] = cNames;
                    return cNames;
                }
                return ViewState[_COLUMNNAMES] as List<String>;
            }
            set { ViewState[_COLUMNNAMES] = value; }
        }
        protected Boolean newp
        {
            get { return (Boolean)ViewState[_NEWP]; }
            set { ViewState[_NEWP] = value; }
        }
        public String SQL1
        {
            get
            {
                try
                {
                    return ViewState[__SQL1] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__SQL1] = value;
            }
        }
        public String SQL2
        {
            get
            {
                try
                {
                    return ViewState[__SQL2] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__SQL2] = value;
            }
        }
        public String SQL3
        {
            get
            {
                try
                {
                    return ViewState[__SQL3] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__SQL3] = value;
            }
        }
        public String SQL4
        {
            get
            {
                try
                {
                    return ViewState[__SQL4] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__SQL4] = value;
            }
        }
        public String SQL5
        {
            get
            {
                try
                {
                    return ViewState[__SQL5] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__SQL5] = value;
            }
        }
        public String P1
        {
            get
            {
                try
                {
                    return ViewState[__P1] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__P1] = value;
            }
        }
        public String P2
        {
            get
            {
                try
                {
                    return ViewState[__P2] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__P2] = value;
            }
        }
        public String P3
        {
            get
            {
                try
                {
                    return ViewState[__P3] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__P3] = value;
            }
        }
        public String P4
        {
            get
            {
                try
                {
                    return ViewState[__P4] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__P4] = value;
            }
        }
        public String P5
        {
            get
            {
                try
                {
                    return ViewState[__P5] as String;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                ViewState[__P5] = value;
            }
        }
        public String Title
        {
            get { return cntReportHeader1.Caption; }
            set { FTitle = value; }
        }
        public String Title2
        {
            get { return cntReportHeader1.Caption1; }
            set { FTitle2 = value; }
        }
        public String Title3
        {
            get { return cntReportHeader1.Caption2; }
            set { FTitle3 = value; }
        }
        public String Title4
        {
            get { return cntReportHeader1.Caption3; }
            set { FTitle4 = value; }
        }
        public String Description
        {
            get { return Tools.GetViewStateStr(ViewState[_TDESC]); }
            set { FDesc = value; }
        }
        public Boolean Pager
        {
            get
            {
                if (Table)
                {
                    try
                    {
                        return (Boolean)ViewState[_VPAGER];
                    }
                    catch
                    {
                        return true;
                    }
                }
                else return false;
            }
            set { ViewState[_VPAGER] = value; }
        }
        public Boolean Table
        {
            get
            {
                try
                {
                    return (Boolean)ViewState[_TABLE];
                }
                catch
                {
                    return true;
                }
            }
            set { ViewState[_TABLE] = value; }
        }

        // Properties
        public String TableClass
        {
            get
            {
                String Current = ViewState[_VTABLECLASS] as String;
                if (String.IsNullOrEmpty(Current))
                    return _DEFAULTTABLECLASS;
                else
                    return Current;
            }
            set { ViewState[_VTABLECLASS] = value; }
        }
        public String RowClass
        {
            get
            {
                String Current = ViewState[_VROWCLASS] as String;
                if (String.IsNullOrEmpty(Current))
                    return _DEFAULTROWCLASS;
                else
                    return Current;
            }
            set { ViewState[_VROWCLASS] = value; }
        }
        public String AlternateRowClass
        {
            get
            {
                String Current = ViewState[_VALTERNATEROWCLASS] as String;
                if (String.IsNullOrEmpty(Current))
                    return _DEFAULTALTERNATEROWCLASS;
                else
                    return Current;
            }
            set { ViewState[_VALTERNATEROWCLASS] = value; }
        }
        public String DivClass
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                    MainDiv.Attributes["class"] = value;
                else
                    MainDiv.Attributes["class"] = _DEFAULTDIVCLASS;
            }
        }
        public String SQL
        {
            get { return hidSQL.Value; }
            set { hidSQL.Value = value; }
        }

        public Boolean PDF
        {
            get { return btnConvertToPdf.Visible; }
            //set { btnConvertToPdf.Visible = value; }

        }

        //PDF

        public void PDFConvert()
        {
            //WebClient myClient = new WebClient();
            //string myPageHTML = null;
            //byte[] requestHTML;
            //string currentPageUrl = Request.Url.ToString();
            UTF8Encoding utf8 = new UTF8Encoding();
            //requestHTML = myClient.DownloadData(currentPageUrl);
            //myPageHTML = utf8.GetString(requestHTML); 

            StreamWriter sw = new StreamWriter(Server.MapPath("~/HTMLTOPDF/asd.html"));
            sw.Write(@"<!DOCTYPE html>

<html>
<head id=""ctl00_ctl00_Head1""><meta http-equiv=""X-UA-Compatible"" content=""IE=9"" /><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><title>
	Matryca Elastyczności
</title>
<link href=""../styles/default.css"" rel=""stylesheet"" type=""text/css"" /><link id=""ctl00_ctl00_cssSPX"" href=""../styles/spx.css"" rel=""stylesheet"" type=""text/css"" /><link id=""ctl00_ctl00_cssONB"" href=""../styles/onb.css"" rel=""stylesheet"" type=""text/css"" /><link href=""../styles/zelmer.css"" rel=""stylesheet"" type=""text/css"" /><link href=""../styles/ui-lightness/jquery-ui-1.8.17.custom.css"" rel=""stylesheet"" type=""text/css"" />
    
    <link href=""/styles/print.css"" rel=""stylesheet"" media=""print"" type=""text/css"" />
    <!--[if IE 7.0]>
        <style type=""text/css"">
            .clearfix {display: inline-block;}
        </style>
    <![endif]-->

    <!--[if lte IE 6]>
        <style type=""text/css"">
            .clearfix {height: 1%;}        
        </style>   
    <![endif]-->

</head>");
            sw.Write("<body>");
            sw.Write(GenerateTable());
            sw.Write("</body>");
            // sw.Write(myPageHTML);
            sw.Close();

            Process ConverterProcess = new Process();
            ConverterProcess.StartInfo.FileName = Server.MapPath("~/HTMLTOPDF/wkhtmltopdf.exe");
            ConverterProcess.StartInfo.WorkingDirectory = Server.MapPath("~/HTMLTOPDF/");
            String FileToSave = Guid.NewGuid().ToString("N");
            ConverterProcess.StartInfo.Arguments = String.Format("asd.html {0}.pdf", FileToSave);
            ConverterProcess.StartInfo.RedirectStandardInput = false;
            ConverterProcess.StartInfo.RedirectStandardOutput = true;
            ConverterProcess.StartInfo.CreateNoWindow = true;
            ConverterProcess.StartInfo.UseShellExecute = false;
            ConverterProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ConverterProcess.EnableRaisingEvents = true;
            ConverterProcess.Start();
            ConverterProcess.WaitForExit();


            String FilePath = Server.MapPath(String.Format("~/HTMLTOPDF/{0}.pdf", FileToSave));
            FileInfo file = new FileInfo(FilePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", file.Name));
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.Flush();
                Response.End();
            }
        }

        protected void ConvertToPDF(object sender, EventArgs e)
        {
            PDFConvert();
        }

        public List<Int32> GetCheckedList()
        {
            List<Int32> CheckedList = new List<Int32>();

            String Value = hidCheck.Value.Substring(0, 1);
            
            //Value = Value.Split(';', 2);

            return CheckedList;
        }

    }
}
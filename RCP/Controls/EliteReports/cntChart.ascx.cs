using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

using HRRcp.App_Code;
//using KDR.Controls.koshi;

namespace HRRcp.Controls.EliteReports
{
    public partial class cntChart : System.Web.UI.UserControl
    {
        protected const String __CANVASID = "_CANVAS1337";
        protected const String ___WIDTH = "___WIDTH";
        protected const String ___HEIGHT = "___HEIGHT";
        protected const String ___NAMES = "___NAMES";
        protected const String ___VALUES = "___VALUES";
        protected const String ___TYPE = "___TYPE";
        protected const String ___COLORS = "___COLORS";
        protected const String ___OPTIONS = "___OPTIONS";
        protected const String ___REPORTID = "__REPORTIDCHART";

        private const int _DEFAULTWIDTH = 400;
        private const int _DEFAULTHEIGHT = 400;

        public Chart MainChart;

        public void SetProperties(Dictionary<String, String> Data)
        {
            //Dictionary<String, String> Properties = Presentable.ParseProperties(Data);

            foreach (KeyValuePair<String, String> Property in Data)
            {
                switch (Property.Key.ToLower())
                {
                    case "width":
                        Width = Convert.ToInt32(Property.Value);
                        break;
                    case "height":
                        Height = Convert.ToInt32(Property.Value);
                        break;
                    case "names":
                        Names = Property.Value;
                        break;
                    case "values":
                        Values = Property.Value;
                        break;
                    case "type":
                        Type = Property.Value;
                        break;
                    case "colors":
                        Colors = Property.Value;
                        break;
                    case "options":
                        Options = Property.Value;
                        break;
                    case "reportid":
                        ReportId = Property.Value;
                        break;
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ReportId))
            {
                cntReport Report = this.Parent.FindControl(ReportId) as cntReport;
                Report.CreateChart(this);
            }
            Boolean TypeFound = true;
            switch (Type.ToLower())
            {
                case Chart.Line:
                    MainChart = new LineChart();
                    break;
                case Chart.Radar:
                    MainChart = new RadarChart();
                    break;
                case Chart.Bar:
                    MainChart = new BarChart();
                    break;
                case Chart.Polar:
                    MainChart = new PolarChart();
                    break;
                case Chart.Pie:
                    MainChart = new PieChart();
                    break;
                case Chart.Doughnut:
                    MainChart = new DoughnutChart();
                    break;
                case Chart.SpeedoMeter:
                    MainChart = new SpeedoMeter();
                    break;
                case Chart.Fusion:
                    MainChart = new Fusion();
                    break;
                default:
                    TypeFound = false;
                    break;
            }

            if (TypeFound)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script1", MainChart.Render(__CANVASID, Names, Values, Colors.Trim(), Options), true);
            else
                ChartCanvas.Visible = false;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            {
                GenerateChart();
            }
        }
        protected String CreateChart()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<canvas id=""{0}"" width=""{1}"" height=""{2}"" ></canvas>", __CANVASID, Width, Height);
            return sb.ToString();
        }
        protected void GenerateChart()
        {
            ChartCanvas.Text = CreateChart();
        }
        public int Width
        {
            get 
            {
                int output = _DEFAULTWIDTH;
                if (ViewState[___WIDTH] == null)
                    return output;
                int.TryParse(ViewState[___WIDTH].ToString(), out output);
                return output; 
            }
            set { ViewState[___WIDTH] = value.ToString(); }
        }
        public int Height
        {
            get
            {
                int output = _DEFAULTHEIGHT;
                if (ViewState[___HEIGHT] == null)
                    return output;
                int.TryParse(ViewState[___HEIGHT].ToString(), out output);
                return output;
            }
            set { ViewState[___HEIGHT] = value.ToString(); }
        }
        public String Names
        {
            get
            {
                if (ViewState[___NAMES] == null)
                    return String.Empty;
                return ViewState[___NAMES] as String;
            }
            set { ViewState[___NAMES] = value; }
        }
        public String Values
        {
            get
            {
                if (ViewState[___VALUES] == null)
                    return String.Empty;
                return ViewState[___VALUES] as String;
            }
            set { ViewState[___VALUES] = value; }
        }
        public String Type
        {
            get
            {
                if (ViewState[___TYPE] == null)
                    return String.Empty;
                return ViewState[___TYPE] as String;
            }
            set { ViewState[___TYPE] = value; }
        }
        public String Colors
        {
            get
            {
                if (ViewState[___COLORS] == null)
                    return String.Empty;
                return ViewState[___COLORS] as String;
            }
            set { ViewState[___COLORS] = value; }
        }
        public String Options
        {
            get
            {
                if (ViewState[___OPTIONS] == null)
                    return String.Empty;
                return ViewState[___OPTIONS] as String;
            }
            set { ViewState[___OPTIONS] = value; }
        }
        public String ReportId
        {
            get { return ViewState[___REPORTID] as String; }
            set { ViewState[___REPORTID] = value; }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            imgCanvas.ImageUrl = hidImg.Value;
            base.RenderControl(writer);
        }


    }
}
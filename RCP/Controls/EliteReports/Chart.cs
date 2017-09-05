using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;

namespace HRRcp.Controls.EliteReports
{
    public abstract class Chart : System.Web.UI.Control
    {
        /// <summary>
        /// 
        /// Global class for chart creation
        /// 
        /// </summary>

        public const String Line = "line";
        public const String Radar = "radar";
        public const String Bar = "bar";
        public const String Polar = "polar";
        public const String Pie = "pie";
        public const String Doughnut = "doughnut";
        public const String SpeedoMeter = "speedometer";
        public const String Fusion = "fusion";

        protected const Char _Splitter = ',';
        protected const Char _NamesSplitter = '|';
        protected const Char _ColorSplitter = '|';
        protected const Char _Separator = ';';
        protected const String _DATASETLABEL = "_DATASETLABEL";

        //KANCIASTE
        protected readonly String[] DefaultFillColor = { "rgba(58,136,228,0.5)", "rgba(33,122,7,0.5)" };
        protected readonly String[] DefaultStrokeColor = { "rgba(220,220,220,1)" };
        protected readonly String[] DefaultPointColor = { "rgba(220,220,220,1)" };
        protected readonly String[] DefaultPointStrokeColor = { "#fff" };
        protected readonly String[] DefaultPointHighlightFill = { "#fff" };
        protected readonly String[] DefaultPointHighlightStroke = { "rgba(220,220,220,1)" };

        //OKRĄGŁE
        protected readonly String[] DefaultColor = { "#CDE4FF", "#79B7FF", "#3A88E4", "#4364E3", "#8395F6" };
        protected readonly String[] DefaultHighlight = { "#8395F6", "#CDE4FF", "#79B7FF", "#3A88E4", "#4364E3" };

        //protected const int LessThan = -1;
        //protected const int GreaterThan = 1;
        //protected const int Equals = 0;


        public virtual String TryGetColor(String Current, String Colors, int i, int j)
        {
            if (String.IsNullOrEmpty(Colors))
                return Current;
            try
            {
                return Colors.Split(_Separator)[i].Split(_ColorSplitter)[j];
            }
            catch
            {
                return Current;
            }
        }
        public virtual String TryGetColor(String Current, String Colors, int i)
        {

            if (String.IsNullOrEmpty(Colors))
                return Current;
            try
            {
                return Colors.Split(_ColorSplitter)[i];
            }
            catch
            {
                return Current;
            }
        }
        public virtual String TryGetValue(String Data, int index)
        {
            try
            {
                return Data.Split(_Splitter)[index];
            }
            catch
            {
                return String.Empty;
            }
        }
        public virtual String TryGetName(String Data, int index)
        {
            try
            {
                return Data.Split(_NamesSplitter)[index];
            }
            catch
            {
                return String.Empty;
            }
        }
        public abstract String Render(String Id, String Names, String Values, String Colors, String Options);
        public virtual String CreateLineData(String Names, String Values, String Colors)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbNames = new StringBuilder();

            String FillColor,
                   StrokeColor,
                   PointColor,
                   PointStrokeColor,
                   PointHighlightFill,
                   PointHighlightStroke;

            for (int i = 0; i < Names.Split(_NamesSplitter).Length; i++)
            {
                if (!i.Equals(0)) sbNames.AppendFormat(",");
                sbNames.AppendFormat(@"""{0}""", Names.Split(_NamesSplitter)[i]);
            }
            sb.AppendFormat(@"
var data = {{
    labels: [{0}],", sbNames.ToString());
            sb.AppendFormat(@"
    datasets: [");
            for (int i = 0; i < Values.Split(_Separator).Length; i++)
            {
                if (!i.Equals(0)) sb.AppendFormat(",");

                try { FillColor = DefaultFillColor[i]; }
                catch { FillColor = DefaultFillColor[0]; }
                try { StrokeColor = DefaultStrokeColor[i]; }
                catch { StrokeColor = DefaultStrokeColor[0]; }
                try { PointColor = DefaultPointColor[i]; }
                catch { PointColor = DefaultPointColor[0]; }
                try { PointStrokeColor = DefaultPointStrokeColor[i]; }
                catch { PointStrokeColor = DefaultPointStrokeColor[0]; }
                try { PointHighlightFill = DefaultPointHighlightFill[i]; }
                catch { PointHighlightFill = DefaultPointHighlightFill[0]; }
                try { PointHighlightStroke = DefaultPointHighlightStroke[i]; }
                catch { PointHighlightStroke = DefaultPointHighlightStroke[0]; }


                FillColor = TryGetColor(FillColor, Colors, i, 0);
                StrokeColor = TryGetColor(StrokeColor, Colors, i, 1);
                PointColor = TryGetColor(PointColor, Colors, i, 2);
                PointStrokeColor = TryGetColor(PointStrokeColor, Colors, i, 3);
                PointHighlightFill = TryGetColor(PointHighlightFill, Colors, i, 4);
                PointHighlightStroke = TryGetColor(PointHighlightStroke, Colors, i, 5);

                sb.AppendFormat(@"
        {{
            label: ""{1}"",
            fillColor: ""{2}"",
            strokeColor: ""{3}"",
            pointColor: ""{4}"",
            pointStrokeColor: ""{5}"",
            pointHighlightFill: ""{6}"",
            pointHighlightStroke: ""{7}"",
            data: [{0}]
        }}", Values.Split(_Separator)[i],
String.Format("{0}{1}", _DATASETLABEL, i),
FillColor,
StrokeColor,
PointColor,
PointStrokeColor,
PointHighlightFill,
PointHighlightStroke);

            }
            sb.AppendFormat(@"

    ]
}};");
            return sb.ToString();
        }
        public virtual String CreateCircleData(String Names, String Values, String Colors)
        {
            StringBuilder sb = new StringBuilder();
            String Color, Highlight, Stroke;
            sb.Append(@"
var data = [ ");

            for (int i = 0; i < Values.Split(_Splitter).Length; i++)
            {
                if (!i.Equals(0)) sb.AppendFormat(",");

                try { Color = DefaultColor[i % DefaultColor.Length]; }
                catch { Color = DefaultColor[0]; }
                try { Highlight = DefaultHighlight[i % DefaultHighlight.Length]; }
                catch { Highlight = DefaultHighlight[0]; }
                try { Stroke = DefaultStrokeColor[i % DefaultStrokeColor.Length]; }
                catch { Stroke = "#000"; }
                Color = TryGetColor(Color, Colors, i, 0);
                Highlight = TryGetColor(Highlight, Colors, i, 1);
                Stroke = TryGetColor(Stroke, Colors, i, 2);

                sb.AppendFormat(@"
        {{
            value: {0},
            color:""{2}"",
            highlight: ""{3}"",
            stroke: ""{4}"",
            label: ""{1}""
               
        }}", TryGetValue(Values, i),
             TryGetName(Names, i),
             Color,
             Highlight,
             Stroke);
            }
            sb.AppendFormat(@"

    ];");
            return sb.ToString();
        }

    }
    public class LineChart : Chart
    {
        public override String Render(String Id, String Names, String Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateLineData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Line(data, {{{1}}});", Id, Options);
            return sb.ToString();
        }
    }
    public class RadarChart : Chart
    {
        public override String Render(String Id, String Names, String Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateLineData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Radar(data, {{{1}}});;", Id, Options);
            return sb.ToString();
        }
    }

    public class BarChart : Chart
    {
        public override String Render(String Id, String Names, String Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateLineData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Bar(data, {{{1}}});
                myLineChart.generateLegend();", Id, Options);
            return sb.ToString();
        }
    }

    public class Fusion : Chart
    {
        public override String Render(String Id, String Names, String Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateLineData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Fusion(data, {{{1}}});
                myLineChart.generateLegend();", Id, Options);
            return sb.ToString();
        }
    }

    public class PolarChart : Chart
    {
        public override String Render(String Id, String Names, String Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateCircleData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).PolarArea(data, {{{1}}});", Id, Options);
            return sb.ToString();
        }
    }

    public class PieChart : Chart
    {
        public override String Render(String Id, String Names, string Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateCircleData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Pie(data, {{{1}}});", Id, Options);
            return sb.ToString();
        }
    }

    public class DoughnutChart : Chart
    {
        public override string Render(string Id, string Names, string Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateCircleData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).Doughnut(data, {{{1}}});", Id, Options);
            return sb.ToString();
        }
    }

    public class SpeedoMeter : Chart
    {
        public override string Render(string Id, string Names, string Values, String Colors, String Options)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(CreateCircleData(Names, Values, Colors));
            sb.AppendFormat(@"
		        var ctx = document.getElementById('{0}').getContext(""2d"");
                var myLineChart = new Chart(ctx).SpeedoMeter(data, {{{1}}});", Id, Options);
            return sb.ToString();
        }
    }

}

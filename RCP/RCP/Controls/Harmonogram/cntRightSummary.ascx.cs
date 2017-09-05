using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.RCP.Controls.Harmonogram
{
    public class Day
    {
        public Day(DateTime day, int R, int R0, int P, int P0, int N, int N0)
        {
            this.DayName = day.Day + " - " + Tools.GetShortDayName(day);
            this.R = R;
            this.R0 = R0;
            this.P = P;
            this.P0 = P0;
            this.N = N;
            this.N0 = N0;
        }
        public String DayName { get; set; }
        public int R { get; set; }
        public int R0 { get; set; }
        public int P { get; set; }
        public int P0 { get; set; }
        public int N { get; set; }
        public int N0 { get; set; }
    }

    public partial class cntRightSummary : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //public void Prepare(String DataOd, String DataDo, String IdDzialu)
        //{
        //    this.DataOd = DataOd;
        //    this.DataDo = DataDo;
        //    this.IdDzialu = IdDzialu;

        //    DateTime dt = Convert.ToDateTime(DataOd);

        //    lblRightSummaryTitle.Text = dt.Year + " - " + Tools.MonthFriendlyName(dt.Month);
        //}

        public void Prepare(String DataOd, String DataDo, String Entities)
        {
            this.DataOd = DataOd;
            this.DataDo = DataDo;
            this.Entities = Entities;

            DateTime dt = Convert.ToDateTime(DataOd);

            lblRightSummaryTitle.Text = dt.Year + " - " + Tools.MonthFriendlyName(dt.Month);
            rpSummary.DataBind();
        }


        [Obsolete]
        public void Prepare(DataTable data, DateTime month, int daysIndex)
        {
            lblRightSummaryTitle.Text = month.Year + " - " + Tools.MonthFriendlyName(month.Month);

            IEnumerable<Day> days = GetData(data, month, daysIndex);
            rpSummary.DataSource = days;
            rpSummary.DataBind();
        }


        public List<Day> GetData(DataTable data, DateTime month, int daysIndex)
        {
            List<Day> days = new List<Day>();
            string shiftRaw = "", shift = "", funkRaw = "", funk = "";
            int i = 0;
            foreach (DateTime day in Tools.AllDatesInMonth(month.Year, month.Month))
            {
                int r = 0, r0 = 0, p = 0, p0 = 0, n = 0, n0 = 0, f = 0;
                foreach (DataRow dr in data.Rows)
                {
                    shiftRaw = db.getValue(dr, i + daysIndex);
                    funk = db.getValue(dr, "Funk", "");


                    if (!String.IsNullOrEmpty(shiftRaw))
                        shift = shiftRaw.Split('|')[0];
                    else
                        shift = "";

                    switch (shift)
                    {
                        case "R":
                            r++;
                            if (String.IsNullOrEmpty(funk))
                                r0++;
                            break;
                        case "P":
                            p++;
                            if (String.IsNullOrEmpty(funk))
                                p0++;
                            break;
                        case "N":
                            n++;
                            if (String.IsNullOrEmpty(funk))
                                n0++;
                            break;
                    }
                }

                days.Add(new Day(day, r, r0, p, p0, n, n0));
                i++;
            }
            return days;
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

        public String IdDzialu
        {
            get { return hidIdDzialu.Value; }
            set { hidIdDzialu.Value = value; }
        }

        public String Entities
        {
            get { return hidEntities.Value; }
            set { hidEntities.Value = value; }
        }
    }
}
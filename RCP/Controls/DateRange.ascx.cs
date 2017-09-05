using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class DateRange : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string dt = (string)Session["enddate1"];
                tbDataDo.Text = String.IsNullOrEmpty(dt) ? Base.DateToStr(DateTime.Now) : dt;
                dt = (string)Session["startdate1"];
                //if (String.IsNullOrEmpty(dt)) dt = "2011-12-01";
                //tbDataOd.Text = String.IsNullOrEmpty(dt) ? Base.DateToStr(DateTime.Now.AddMonths(-1).AddDays(-5)) : dt;
                tbDataOd.Text = String.IsNullOrEmpty(dt) ? Base.DateToStr(DateTime.Now.AddMonths(-1).AddDays(1)) : dt;
            }
        }

        public void StoreDates()
        {
            Session["startdate1"] = tbDataOd.Text;
            Session["enddate1"] = tbDataDo.Text;
        }
        //------------------
        public string DateFrom
        {
            get { return tbDataOd.Text; }
            set { tbDataOd.Text = value; }
        }

        public string DateTo
        {
            get { return tbDataDo.Text; }
            set { tbDataDo.Text = value; }
        }
    }
}
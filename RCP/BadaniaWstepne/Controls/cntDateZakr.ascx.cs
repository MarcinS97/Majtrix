using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.BadaniaWstepne.Controls
{
    public partial class cntDateZakr : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Clear()
        {
            DateFrom = "";
            DateTo = "";
        }

        public KeyValuePair<DateTime?, DateTime?> Value
        {
            get
            {
                DateTime dtf;
                DateTime dtt;
                if (string.IsNullOrEmpty(DateFrom) || !DateTime.TryParse(DateFrom, out dtf)) dtf = DateTime.FromBinary(0);
                if (string.IsNullOrEmpty(DateTo) || !DateTime.TryParse(DateTo, out dtt)) dtt = DateTime.Now.AddYears(100);
                return new KeyValuePair<DateTime?, DateTime?>(dtf, dtt);
            }
            set
            {
                DateFrom = (value.Key.HasValue) ? Tools.DateToStr(value.Key.Value) : "";
                DateTo = (value.Value.HasValue) ? Tools.DateToStr(value.Value.Value) : "";
            }
        }
        public string FilterExp
        {
            get
            {
                DateTime dtf;
                DateTime dtt;
                DateTime? ntf = (string.IsNullOrEmpty(DateFrom) || !DateTime.TryParse(DateFrom, out dtf)) ? null : (DateTime?)dtf;
                DateTime? ntt = (string.IsNullOrEmpty(DateTo) || !DateTime.TryParse(DateTo, out dtt)) ? null : (DateTime?)dtt;
                string dod = (ntf.HasValue) ? "{0} >= '" + Tools.DateToStr(ntf.Value) + "'" : "";
                string ddo = (ntt.HasValue) ? "{0} <= '" + Tools.DateToStr(ntt.Value) + "'" : "";
                string dw = (ntf.HasValue && ntt.HasValue) ? " AND " : "";
                return string.Format("{0}{1}{2}", dod, dw, ddo);
            }
        }

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
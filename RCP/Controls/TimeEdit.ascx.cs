using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using HRRcp.App_Code;


namespace HRRcp.Controls
{
    public partial class TimeEdit : System.Web.UI.UserControl
    {
        string FFormat = "H:mm";        // 24h bez 0 przy h   ---> moze trzeba będzie dac jako hidden field

        bool FAllowNegative = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool Validate()
        {
            bool valid = IsValid;
            lbError.Visible = !valid;
            return valid;
        }

        public void ClearError()
        {
            lbError.Visible = false;
        }

        public void SetError(bool err, string text)
        {
            lbError.Visible = err;
            if (!String.IsNullOrEmpty(text))
                lbError.Text = text;
        }
        //--------------------------
        /*
        private string ToString(object o, string format)
        {
            if (o == null || o.Equals(DBNull.Value))
                return null;
            else
                return ((DateTime)o).ToString(format);
        }
        */

        public static string SecToTimeStr(int? sec, string format)
        {
            if (Base.isNull(sec)) return null;
            else
                if (format.Contains(':'))
                    return DateTime.MinValue.AddSeconds((int)sec).ToString(format);
                else
                {
                    string t = DateTime.MinValue.AddSeconds((int)sec).ToString(format + ":mm");  // zakładam, że format może zaczynać się tylko od H lub HH !!!
                    return t.Substring(0, t.Length - 3);
                }
        }

        public static string TimeToTimeStr(DateTime? dt, string format)
        {
            if (Base.isNull(dt)) return null;
            else
                if (format.Contains(':'))
                    return ((DateTime)dt).ToString(format);
                else
                {
                    string t = ((DateTime)dt).ToString(format + ":mm");  // zakładam, że format może zaczynać się tylko od H lub HH !!!
                    return t.Substring(0, t.Length - 3);
                }
        }

        public static bool ToTimeSpan(string time, out TimeSpan ts)
        {
            if (!String.IsNullOrEmpty(time))
            {
                string t = time.Trim();
                if (!String.IsNullOrEmpty(time))
                {
                    if (!t.Contains(':')) t += ":00";
                    else
                        if (t.StartsWith(":")) t = "0" + t;
                        else if (t.EndsWith(":")) t += "00";
                    return TimeSpan.TryParse(t, out ts);
                    //return TimeSpan.TryParse(time.Contains(':') ? time : time + ":00", out ts);
                }
            }
            ts = TimeSpan.MinValue;
            return false;
        }

        public static int StrToSec(string time, int def)
        {
            TimeSpan ts;
            if (ToTimeSpan(time, out ts))           
                return Convert.ToInt32(ts.TotalSeconds);
            else
                return def;
        }
        //--------------------------
        public bool IsValid
        {
            get 
            {
                if (String.IsNullOrEmpty(tbTime.Text)) 
                    return true;
                else
                {
                    TimeSpan ts;
                    return ToTimeSpan(tbTime.Text, out ts);
                }
            }
        }

        public bool IsEntered
        {
            get { return !String.IsNullOrEmpty(tbTime.Text); }
        }

        public string IsEntered01       // do zapisu do bazy
        {
            get { return IsEntered ? "1" : "0"; }
        }

        public TimeSpan Time
        {
            //get { return Base.StrToTimeSpan(tbTime.Text); }
            get 
            {
                TimeSpan ts;
                if (ToTimeSpan(tbTime.Text, out ts))
                    return ts;
                else
                    return TimeSpan.MinValue;
            }
            set { tbTime.Text = Base.DateToStr(value); }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        public DateTime DateTime
        {
            get { return Tools.StrToDateTime(null, tbTime.Text); }
            set { tbTime.Text = TimeToTimeStr(value, FFormat); }
        }

        public string TimeStr
        {
            get { return tbTime.Text; }
            set 
            { 
                TimeSpan ts;
                if (ToTimeSpan(value, out ts))
                    tbTime.Text = TimeToTimeStr(DateTime.MinValue.Add(ts), FFormat);
                else
                    tbTime.Text = null;
            }
        }

        public string Format
        {
            get { return FFormat; }
            set 
            {
                if (value.Length > 5)           // hh:mm:ss 
                {
                    tbTime.MaxLength = 8;       
                    tbFilter.ValidChars = (FAllowNegative ? "-" : null) + "0123456789:";
                }
                else if (value.Length > 2)      // hh:mm 
                {
                    tbTime.MaxLength = 5;
                    tbFilter.ValidChars = (FAllowNegative ? "-" : null) + "0123456789:";
                }
                else                            // hh
                {
                    tbTime.MaxLength = 2;
                    tbFilter.ValidChars = (FAllowNegative ? "-" : null) + "0123456789";
                }
                tbTime.Width = tbTime.MaxLength * 9
#if BOOTSTRAP
                    + 24
#endif
                    ;
                FFormat = value; 
            }
        }

        public string Opis
        {
            get { return lbOpis.Text; }
            set { lbOpis.Text = value; }
        }

        public bool ReadOnly
        {
            get { return tbTime.ReadOnly; }
            set { Tools.SetReadOnly(tbTime, !value); }
        }

        [Bindable(true, BindingDirection.TwoWay)]
        public object Seconds
        {
            get
            {
                TimeSpan ts;
                if (ToTimeSpan(tbTime.Text, out ts))
                    return Convert.ToInt32(ts.TotalSeconds);
                else
                    return null;
            }
            set 
            {
                if (value.Equals(DBNull.Value))
                    tbTime.Text = null;
                else
                    tbTime.Text = SecToTimeStr((int)value, FFormat); 
            }
        }

        public TextBox EditBox
        {
            get { return tbTime; }
        }

        public bool AllowNegative
        {
            set 
            { 
                FAllowNegative = value;
                Format = FFormat;
            }
            get { return FAllowNegative; }
        }

        public string ValidationGroup
        {
            set
            {
                RequiredFieldValidator1.ValidationGroup = value;
                RequiredFieldValidator1.Enabled = !String.IsNullOrEmpty(value);
            }
            get { return RequiredFieldValidator1.ValidationGroup; }
        }
    }
}
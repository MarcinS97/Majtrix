using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class DateTimeEnter : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CalendarChanged(object sender, EventArgs e)
        {
            int x = 0;
        }
        //-------------------------
        public bool Validate()
        {
            bool valid = IsValid;
            if (!valid)
            {
                // wyswietlam warning
            }
            return valid;
        }

        public void ClearError()
        {
        }
        //--------------------------
        public bool IsValid
        {
            get 
            {
                DateTime dt;
                return Base.StrToDateTime(tbDate.Text, out dt);
            }
        }

        public DateTime DateTime
        {
            get { return Base.StrToDateTime(tbDate.Text + " " + tbTime.Text); }
            set 
            {
                string dt = Base.DateTimeToStr(value);
                tbDate.Text = dt.Substring(0, 10);
                tbTime.Text = dt.Substring(11);
            }
        }

        public string DateTimeStr
        {
            get { return tbDate.Text + " " + tbTime.Text; }
            set 
            { 
                tbDate.Text = value; 
            }
        }

        public string DateStr
        {
            get { return tbDate.Text; }
            set { tbDate.Text = value; }
        }

        public string TimeStr
        {
            get { return tbTime.Text; }
            set { tbTime.Text = value; }
        }
    }
}
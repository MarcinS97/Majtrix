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
    public partial class xxxDateEdit2 : System.Web.UI.UserControl
    {
        public event EventHandler DateChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
#if KDR
            Tools.ExecOnStart(@"prepareCalendar");
            tbDate2.Visible = btDate2.Visible = true;
            tbDate.Visible = btDate.Visible = false;
#else
            tbDate2.Visible = btDate2.Visible = false;
            tbDate.Visible = btDate.Visible = true;
#endif
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

        public static bool ValidateRange(DateEdit deOd, DateEdit deDo, int logId)
        {
            if (deOd != null && deDo != null)
            {
                if (deOd.IsValid && deDo.IsValid)
                {
                    if ((DateTime)deDo.Date >= (DateTime)deOd.Date)
                        return true;
                    else
                        Tools.ShowMessage("Błąd!\\n\\nData końca wcześniejsza od daty początku.");
                }
                else
                {
                    Tools.ShowMessage("Błąd!\\n\\nNiepoprawny format daty.");
                    Log.Error(logId, "Błąd formatu daty", String.Format("Od: '{0}' Do: '{1}'", deOd.DateStr, deDo.DateStr));
                }
            }
            else
            {
                Tools.ShowMessage("Błąd!\\n\\nBłąd konwersji daty.");
                Log.Error(logId, "Nie można znaleźć kontrolki daty.");
            }
            return false;
        }
        //--------------------------
        protected void tbDate_TextChanged(object sender, EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, EventArgs.Empty);
        }

        protected void tbTime_TextChanged(object sender, EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, EventArgs.Empty);
        }
        //--------------------------
        public bool IsValid
        {
            get 
            {
                //DateTime dt;
                //return DateTime.TryParse(tbDate.Text, out dt); 
                return Tools.DateIsValid(tbDate.Text);
            }
        }

        public string Opis
        {
            get { return lbOpis.Text; }
            set { lbOpis.Text = value; }
        }

        public string Error
        {
            get { return lbError.Visible ? lbError.Text : null; }
            set 
            { 
                lbError.Text = value;
                lbError.Visible = !String.IsNullOrEmpty(value);
            }
        }

        public bool ReadOnly
        {
            get { return tbDate.ReadOnly; }
            set 
            { 
                Tools.SetReadOnly(tbDate, !value);
                btDate.Enabled = !value;
            }
        }

        [Bindable(true, BindingDirection.OneWay)]
        public object Date  // DateTime
        {
            get 
            {
                bool t = IsTime;
                if (!String.IsNullOrEmpty(tbDate.Text) && (!t || !String.IsNullOrEmpty(tbTime.Text)))
                {
                    DateTime dt;
                    if (t)
                    {
                        if (DateTime.TryParse(tbDate.Text + " " + tbTime.Text, out dt))
                            return dt;
                    }  
                    else
                        if (DateTime.TryParse(tbDate.Text, out dt))
                            return dt;
                }
                return null;
                //return Base.StrToDateTime(tbDate.Text); 
            }
            set 
            {
                if (value == null || value.Equals(DBNull.Value))
                {
                    tbDate.Text = null;
                    tbTime.Text = null;
                }
                else
                {
                    tbDate.Text = Tools.DateToStr(value);
                    if (IsTime)
                        tbTime.Text = Tools.DateTimeToStr(value, TimeFormat);
                }
            }
        }

        public string DateStr
        {
            get { return tbDate.Text; }
            set { tbDate.Text = value; }
        }

        public string YearMonthStr
        {
            get 
            {
                object dt = Date;
                if (dt != null)
                    return Tools.DateToStr(dt).Substring(0, 7);  //yyyy-mm
                else
                    return null;
            }
        }
        public TextBox EditBox
        {
            get { return tbDate; }
        }

        public ImageButton Button
        {
            get { return btDate; }
        }

        public bool AutoPostBack
        {
            set { tbDate.AutoPostBack = value; }
            get { return tbDate.AutoPostBack; }
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

        public AjaxControlToolkit.CalendarPosition CalendarPosition
        {
            set
            {
                //CalendarExtender.PopupPosition = value;
                //CalendarExtenderButton.PopupPosition = value;
            }
            get { return CalendarExtender.PopupPosition; }
        }

        //-----
        public bool IsTime
        {
            get { return !String.IsNullOrEmpty(TimeFormat); }
        }

        public string TimeFormat
        {
            set
            {
                ViewState["tformat"] = value;
                bool v = !String.IsNullOrEmpty(value);
                tbTime.Visible = v;
                if (v)
                {
                    tbTime.MaxLength = value.Length;
                }
            }
            get { return Tools.GetStr(ViewState["tformat"]); }
        }

        public TextBox TimeEditBox
        {
            get { return tbTime; }
        }

    }
}
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
    public partial class DateEdit : System.Web.UI.UserControl
    {
        public event EventHandler DateChanged;

        const int mo0 = 0;  //T: dodałem do uzupełnienia - za co konkretnie odpowiadają te wartości ?
        const int mo1 = 1;
        const int mo2 = 2;
        const int mo3 = 3;
        const int mo4 = 4;

        protected void Page_Load(object sender, EventArgs e)
        {
#if KDR || MS || DBW || OKT || PORTAL || VICIM || VC
     
    #if PORTAL

            Options opt = new Options();
            opt.minViewMode = MinViewMode;
            opt.maxViewMode = MaxViewMode;

            opt.startDate = StartDate;
            opt.endDate = EndDate;

            Tools.ExecOnStart2(this.ClientID + "prepareCal", String.Format("prepareCalendar2('{0}', {1});", this.ctDateEdit.ClientID, Newtonsoft.Json.JsonConvert.SerializeObject(opt)));
            //Tools.ExecOnStart(@"prepareCalendar2", "'" + this.ctDateEdit.ClientID + "'," + Newtonsoft.Json.JsonConvert.SerializeObject(opt));
    #else
                Tools.ExecOnStart(@"prepareCalendar");
    #endif
            //tbDate2.Visible = btDate2.Visible = true;
            tbDate.Visible = btDate.Visible = false;
            RequiredFieldValidator1.ControlToValidate = "tbDate2";
            tbFilter.TargetControlID = "tbDate2";
#else
            tbDate2.Visible = /*btDate2.Visible = */ false;
            //tbDate.Visible = btDate.Visible = true;
            RequiredFieldValidator1.ControlToValidate = "tbDate";
            tbFilter.TargetControlID = "tbDate";
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
        //--------------------------

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
#if KDR || MS || DBW || OKT || PORTAL || VICIM || VC

#if PORTAL

        class Options
        {
            public Options()
            {
                format = "yyyy-mm-dd";
                clearBtn = true;
                language = "pl";
                todayHighlight = true;
                daysOfWeekHighlighted = new string[] { "0" };
                autoClose = true;
            }

            public String format { get; set; }
            public Boolean clearBtn { get; set; }
            public String language { get; set; }
            public Boolean todayHighlight { get; set; }
            public string[] daysOfWeekHighlighted { get; set; }
            public Boolean autoClose { get; set; }

            public int minViewMode { get; set; }
            public int maxViewMode { get; set; }

            public string startDate { get; set; }
            public string endDate { get; set; }
        }


#endif

        public bool ReadOnly
        {
            get { return tbDate.ReadOnly; }
            set
            {
                tbDate2.Enabled = !value;
                //btDate2.Visible = !value;
            }
        }

        public bool IsValid
        {
            get
            {
                //DateTime dt;
                //return DateTime.TryParse(tbDate.Text, out dt); 
                return Tools.DateIsValid(tbDate2.Text);
            }
        }


        [Bindable(true, BindingDirection.OneWay)]
        public object Date  // DateTime
        {
            get 
            {
                if (!String.IsNullOrEmpty(tbDate2.Text))
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbDate2.Text, out dt))
                        return dt;
                }
                return null;
                //return Base.StrToDateTime(tbDate2.Text); 
            }
            set 
            {
                if (value == null || value.Equals(DBNull.Value))
                    tbDate2.Text = null;
                else
                    tbDate2.Text = Tools.DateToStr(value); 
            }
        }

        public string DateStr
        {
            get { return tbDate2.Text; }
            set { tbDate2.Text = value; }
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
            get { return tbDate2; }
        }

        public WebControl Button
        {

            get { return btDate; }//btDate2; }
        }

        public bool AutoPostBack
        {
            set { tbDate2.AutoPostBack = value; }
            get { return tbDate2.AutoPostBack; }
        }
#else

        public bool ReadOnly
        {
            get { return tbDate.ReadOnly; }
            set 
            { 
                Tools.SetReadOnly(tbDate, !value);
                btDate.Enabled = !value;
            }
        }

        public bool IsValid
        {
            get 
            {
                //DateTime dt;
                //return DateTime.TryParse(tbDate.Text, out dt); 
                return Tools.DateIsValid(tbDate.Text);
            }
        }


        [Bindable(true, BindingDirection.OneWay)]
        public object Date  // DateTime
        {
            get 
            {
                if (!String.IsNullOrEmpty(tbDate.Text))
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbDate.Text, out dt))
                        return dt;
                }
                return null;
                //return Base.StrToDateTime(tbDate.Text); 
            }
            set 
            {
                if (value == null || value.Equals(DBNull.Value))
                    tbDate.Text = null;
                else
                    tbDate.Text = Tools.DateToStr(value); 
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

#endif

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

        /* UWAGA UWAGA */

        public int MinViewMode
        {
            get { return Tools.GetViewStateInt(ViewState["vMinViewMode"], mo0); }
            set { ViewState["vMinViewMode"] = value; }
        }

        public int MaxViewMode
        {
            get { return Tools.GetViewStateInt(ViewState["vMaxViewMode"], mo4); }
            set { ViewState["vMaxViewMode"] = value; }
        }

        public string StartDate
        {
            get { return Tools.GetViewStateStr(ViewState["vStartDate"]); }
            set { ViewState["vStartDate"] = value; }
        }

        public string EndDate
        {
            get { return Tools.GetViewStateStr(ViewState["vEndDate"]); }
            set { ViewState["vEndDate"] = value; }
        }
    }
}
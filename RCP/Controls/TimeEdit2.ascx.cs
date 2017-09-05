using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using HRRcp.App_Code;


namespace HRRcp.Controls
{
    public partial class TimeEdit2 : System.Web.UI.UserControl
    {
        string FFormat = "H:mm";        // 24h bez 0 przy h   ---> moze trzeba będzie dac jako hidden field
        bool FSelectPanel = true;
        bool FRight = false;
        int FInterval = 30;
        int FInLineCount = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareTimeList();
        }
        //--------------------
        /*
        private HtmlGenericControl AddNav1(string text, int opt, string hint)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.InnerText = text;
            div.Attributes["Title"] = hint;
            div.Attributes["opt"] = opt.ToString();
            div.Attributes["OnClick"] = "TimeEdit2_NavClick(this);return false;";
            div.Attributes["class"] = "timenav";
            phTimeListNav.Controls.Add(div);
            return div;
        }
        */

        private void AddNav(PlaceHolder ph, string text, int opt, string hint)
        {
            Button bt = new Button();
            bt.Text = text;
            bt.ToolTip = hint;
            bt.Attributes["opt"] = opt.ToString();
            bt.Attributes["OnClick"] = String.Format("TimeEdit2_NavClick(this,'{0}');return false;", tbTime.ClientID);
            bt.CssClass = "timenav";
            bt.TabIndex = -1;
            ph.Controls.Add(bt);
        }

        private void AddNav(PlaceHolder ph, string text, HtmlGenericControl paShow, HtmlGenericControl paHide, string hint)
        {
            Button bt = new Button();
            bt.Text = text;
            bt.ToolTip = hint;
            bt.Attributes["OnClick"] = String.Format("TimeEdit2_Nav2Click(this,'{0}','{1}');return false;", paShow.ClientID, paHide.ClientID);
            bt.CssClass = "timenav2";
            bt.TabIndex = -1;
            ph.Controls.Add(bt);
        }

        private HtmlGenericControl tab(string text)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.InnerText = text;
            div.Attributes["OnClick"] = String.Format("TimeEdit2_Click(this,'{0}');return false;",tbTime.ClientID);
            div.Attributes["class"] = "timetab";
            return div;
        }

        private void br(PlaceHolder ph)
        {
            Literal lt = new Literal();
            lt.Text = "<br />";
            ph.Controls.Add(lt);
        }

        private void AddTabs(PlaceHolder ph, int startHH, int startMM, int count)
        {
            //const string hhmm = "{0:D2}:{1:D2}";
            const string hhmm = "{0}:{1:D2}";
            TimeSpan ts = new TimeSpan(startHH, startMM, 0);
            TimeSpan its = new TimeSpan(0, FInterval, 0);
            ph.Controls.Add(tab(String.Format(hhmm, ts.Hours, ts.Minutes)));
            for (int i = 1; i < count; i++)
            {
                ts = ts.Add(its);
                ph.Controls.Add(tab(String.Format(hhmm, ts.Hours, ts.Minutes)));
                if (i % 2 == 1) br(ph);
            }
        }

        private void AddTabs(PlaceHolder ph, int startHH, int startMM, int endHH, int endMM)
        {
            //const string hhmm = "{0:D2}:{1:D2}";
            const string hhmm = "{0}:{1:D2}";
            TimeSpan ts = new TimeSpan(startHH, startMM, 0);
            TimeSpan ets = new TimeSpan(endHH, endMM, 0);
            TimeSpan its = new TimeSpan(0, FInterval, 0);
            ph.Controls.Add(tab(String.Format(hhmm, ts.Hours, ts.Minutes)));
            int i = 1;
            while (ts < ets)
            {
                ts = ts.Add(its);
                ph.Controls.Add(tab(String.Format(hhmm, ts.Hours, ts.Minutes)));
                i++;
                if (i % FInLineCount == 0) br(ph);
            }
        }

        private void PrepareTimeList()
        {
            if (FSelectPanel)
            {
                tbTimeList.Visible = true;

                AddNav(phTimeListNav, "+", FInterval, String.Format("Dodaj {0} minut", FInterval));
                AddNav(phTimeListNav, "-", -FInterval, String.Format("Odejmij {0} minut", FInterval));
                AddNav(phTimeListNav, "x", -99, "Usuń czas");

                AddNav(phTimeListNav1, "◄", list3, list1, "Najdłuższe czasy");
                AddNav(phTimeListNav1, "►", list2, list1, "Dłuższe czasy");
                br(phTimeListNav1);

                AddNav(phTimeListNav2, "◄", list1, list2, "Krótsze czasy");
                AddNav(phTimeListNav2, "►", list3, list2, "Dłuższe czasy");
                br(phTimeListNav2);

                AddNav(phTimeListNav3, "◄", list2, list3, "Krótsze czasy");
                AddNav(phTimeListNav3, "►", list1, list3, "Najkrótsze czasy");
                br(phTimeListNav3);

                /*
                AddTabs(phTimeList1, 0, 0, 30, 17);
                AddTabs(phTimeList2, 8, 0, 30, 17);
                AddTabs(phTimeList3, 16, 0, 30, 16);
                */

                AddTabs(phTimeList1, 0, 0, 8, 0);
                AddTabs(phTimeList2, 8, 0, 16, 0);
                AddTabs(phTimeList3, 16, 0, 23, 60 - FInterval);

                tbTime.Attributes["OnKeyPress"] = "return TimeEdit2_KeyPress(this);";
                if (FRight)
                    Tools.AddClass(tbTimeList, "timeeditlistright");
            }
            else
                tbTimeList.Visible = false;
        }
        //--------------------
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
                    tbTime.MaxLength = 8;       
                else if (value.Length > 2)      // hh:mm 
                    tbTime.MaxLength = 5;       
                else                            // hh
                    tbTime.MaxLength = 2;       
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

        //--------------
        public bool SelectPanel
        {
            get { return FSelectPanel; }
            set { FSelectPanel = value; }
        }

        public bool Right
        {
            get { return FRight; }
            set { FRight = value; }
        }

        public int Interval
        {
            get { return FInterval; }
            set { FInterval = value; }
        }

        public int InLineCount
        {
            get { return FInLineCount; }
            set { FInLineCount = value; }
        }
    }
}

/*
<%--
<!--
    <asp:CustomValidator ID="CustomValidator1" runat="server" 
        ControlToValidate="tbTime"
        ErrorMessage="*"
        Enabled="false"
        ClientValidationFunction="validate">
    </asp:CustomValidator>

    var tb = document.getElementById('<%=tbTime.ClientID %>');

    function validate(sender, args) {
        args.IsValid = false;
        var tb = document.getElementById('<%=tbTime.ClientID %>');
        if (args.Value.match(/^\w{6}\d{6}$/)) {
            args.IsValid = true
            tb.style.backgroundColor = "white";
            tb.style.color = "green";
        }
        else {
            tb.style.backgroundColor = "red";
        }
    }

    function OnlyNums(e, cText) {
        var keynum
        var keychar
        var numcheck
     
        if (window.event) // IE
        { keynum = e.keyCode; }
        else if (e.which) // Netscape/Firefox/Opera
        { keynum = e.which; }
     
        keychar = String.fromCharCode(keynum);
        numcheck = /\d/;
        return numcheck.test(keychar); //returns true if is digit 
    }

<div class="timeeditbox">
    <asp:TextBox ID="tbTime" CssClass="textbox timeedit" runat="server" Width="60px" MaxLength="5"></asp:TextBox>
    <div id="tbTimeList" runat="server" class="timeeditlist" >
        <div class="timeeditlistpanel">
            <asp:PlaceHolder ID="phTimeList" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</div>
-->
--%>
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Controls
{
    public partial class cntSelectRokMiesiac : System.Web.UI.UserControl
    {
        static string[]  _Miesiace;
        public int Rok
        {
            get
            {
                if (string.IsNullOrEmpty(HFYVal.Value)) Rok = RokNow;
                return int.Parse(HFYVal.Value);
            }
            set
            {
                HFYVal.Value = value.ToString();
                LabelY.Text = Rok.ToString();
            }
        }
        public int Miesiac
        {
            get
            {
                if (string.IsNullOrEmpty(HFMVal.Value)) Miesiac = MiesiacNow;
                return int.Parse(HFMVal.Value);
            }
            set
            {
                if (value < 1)
                {
                    value += 12;
                    Rok--;
                }
                if(value > 12)
                {
                    value -= 12;
                    Rok++;
                }
                HFMVal.Value = value.ToString();
                LabelM.Text = MiesiacName;
            }
        }
        public string MiesiacName
        {
            get
            {
                return _Miesiace[Miesiac - 1];
            }
        }
        public static int RokNow
        {
            get
            {
                return DateTime.Now.Year;
            }
        }
        public static int MiesiacNow
        {
            get
            {
                return DateTime.Now.Month;
            }
        }
        public event EventHandler ValueChanged;
        public event EventHandler BackAll;
        public event EventHandler NextAll;
        [DefaultValue(false)]
        public bool canBackAll { get; set; }
        [DefaultValue(false)]
        public bool canNextAll { get; set; }
        [DefaultValue(false)]
        public bool Quatro { get; set; }

        public String ToolTipPrev
        {
            get
            {
                return ((Quatro) ? "Poprzednie trzy miesiące" : "Poprzedni rok");
            }
        }

        public String ToolTipNext
        {
            get
            {
                return ((Quatro) ? "Następne trzy miesiące" : "Następny rok");
            }
        }

        protected void FirstLoad()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                FirstLoad();
            }
        }

        protected void btSelectMY_Click(object sender, EventArgs e)
        {
            Button bt = (sender as Button);
            int v = (bt.CommandArgument == "1") ? 1 : -1;
            if (bt.CommandName == "M")
                Miesiac += v;
            else
                if (!Quatro) Rok += v; else Miesiac += 3 * v;
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        static cntSelectRokMiesiac()
        {
            _Miesiace = DateTimeFormatInfo.GetInstance(new CultureInfo("pl-PL")).MonthNames.Take(12).ToArray();
        }

        public void SelectNow()
        {
            Miesiac = MiesiacNow;
            Rok = RokNow;
        }

        protected void BTA_Click(object sender, EventArgs e)
        {
            Button bt = (sender as Button);
            EventHandler ev = (bt.CommandArgument == "1") ? NextAll : BackAll;
            if (ev != null)
                ev(this, new EventArgs());
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }
    }
}
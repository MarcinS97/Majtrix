using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.WnioskiMajatkowe
{
    public partial class cntLista : System.Web.UI.UserControl
    {
        public event EventHandler Show;
        public event EventHandler ListDataBound;

        private int FMode = moAktywne;
        public const int moAktywne      =  0;
        public const int moZakonczone   = -1;
        public const int moAll          = 99;
        public const int moSelect       = 11;
        public const int moSelectZakoncz= 12;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hidUserId.Value = App.User.Id;
            }
        }
        //-------------------------------
        public bool ModeVisible(params int[] modes)
        {
            return FMode.IsAny(modes);
        }

        private void TriggerShow()
        {
            if (Show != null)
            {
                Show(lvWnioski, EventArgs.Empty);
            }
        }

        public ListView List
        {
            get { return lvWnioski; }
        }

        protected void lvWnioski_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "show":
                    lvWnioski.SelectedIndex = ((ListViewDataItem)e.Item).DataItemIndex;
                    TriggerShow();
                    break;
            }
        }

        protected void lvWnioski_DataBound(object sender, EventArgs e)
        {
            HtmlTableRow tr = lvWnioski.FindControl("trHeader") as HtmlTableRow;
            if (tr != null)
                tr.DataBind();
            if (ListDataBound != null)
                ListDataBound(sender, e);
        }

        public String GetButtonIcon()
        {
            return String.IsNullOrEmpty(ButtonIcon) ? "glyphicon glyphicon-search" : ButtonIcon;
        }
        public String ButtonIcon { get; set; }

        public String GetButtonClass()
        {
            return String.IsNullOrEmpty(ButtonClass) ? "btn btn-primary" : ButtonClass;
        }

        public String ButtonTooltip { get; set; }

        public String GetButtonTooltip()
        {
            return String.IsNullOrEmpty(ButtonTooltip) ? "Pokaż szczegóły" : ButtonTooltip;
        }

        public String ButtonClass { get; set; }

        //public String Status
        //{
        //    get { return hidStatus.Value; }
        //    set { hidStatus.Value = value; }
        //}

        public int Mode
        {
            set 
            {
                FMode = value;
                hidMode.Value = value.ToString(); 
            }
            get { return Tools.StrToInt(hidMode.Value, moAktywne); }
        }
    }
}
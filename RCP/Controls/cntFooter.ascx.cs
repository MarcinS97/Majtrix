using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class cntFooter : System.Web.UI.UserControl
    {
        public event EventHandler RegulaminClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbAppName.Text = Tools.GetAppName();
                lbVersion.Text = Tools.GetAppVersion();
#if IQOR && KWITEK
                string cpr = "iQor '" + DateTime.Today.Year.ToString();
                string www = null;
#else
                string cpr = "© KDR Solutions Sp. z o.o. '" + DateTime.Today.Year.ToString();
                string www = "http://www.kdrsolutions.pl";
#endif
                lbCopyright.Text = cpr;
                aCopyright.Text = cpr;
                aCopyright.NavigateUrl = www;
                aCopyright.ToolTip = www;
            }
        }

        protected void btRegulamin_Click(object sender, EventArgs e)
        {
            if (RegulaminClick == null)
                Info.SetHelp(Info.REGULAMIN, true, true);
            else
                RegulaminClick(this, new EventArgs());
        }

        //private void TriggerRegulaminClick()
        //{
        //    if (RegulaminClick != null)
        //        RegulaminClick(this, new EventArgs());
        //}
        //---------------------
        public string AppName
        {
            get { return lbAppName.Text; }
            set { lbAppName.Text = value; }
        }

        public string AppVer
        {
            get { return lbVersion.Text; }
            set { lbVersion.Text = value; }
        }

        public bool CopyrightAsLink
        {
            set
            {
                aCopyright.Visible = value;
                lbCopyright.Visible = !value;
            }
        }

        public LinkButton RegulaminButton
        {
            get { return btRegulamin; }
        }
    }
}
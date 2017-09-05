using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class FooterControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbProgram.Text = Tools.GetAppName();
                lbVersion.Text = Tools.GetAppVersion();
                //lbJabil.Text = " '2013";
#if KWITEK
                lbJabil.Text = "iQor '" + DateTime.Today.Year.ToString();
#else
                lbJabil.Text = "© KDR Solutions Sp. z o.o. '" + DateTime.Today.Year.ToString();
#endif
            }
        }

        protected void btRegulamin_Click(object sender, EventArgs e)
        {
            Info.SetHelp(Info.REGULAMIN, true, true);
        }

        public string AppName
        {
            get { return lbProgram.Text; }
            set { lbProgram.Text = value; }
        }

        public string AppVer
        {
            get { return lbVersion.Text; }
            set { lbVersion.Text = value; }
        }

        public LinkButton RegulaminButton
        {
            get { return btRegulamin; }
        }
    }
}
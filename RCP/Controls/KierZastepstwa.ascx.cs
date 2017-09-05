using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class KierZastepstwa : System.Web.UI.UserControl
    {
        private const string active_menu = "kzastmenu";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(mnLeft, active_menu);
                if (!App.User.IsSetZastepstwa)
                {
                    if (mnLeft.SelectedValue == "1")
                        mnLeft.Items[0].Selected = true;
                    mnLeft.Items[1].Enabled = false;
                }
                SelectMenu(false);
            }
        }

        private void SelectMenu(bool setHelp)
        {
            string sel = mnLeft.SelectedValue;
            Session[active_menu] = sel;            
            switch (sel)
            {
                case "0":
                    mvSettings.SetActiveView(vMoje);
                    //if (setHelp) Info.SetHelp(Info.HELP_);
                    break;
                case "1":
                    mvSettings.SetActiveView(vMnie);
                    //if (setHelp) Info.SetHelp(Info.HELP_);
                    break;
                case "2":
                    mvSettings.SetActiveView(vHist);
                    //if (setHelp) Info.SetHelp(Info.HELP_);
                    break;
            }
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }
    }
}
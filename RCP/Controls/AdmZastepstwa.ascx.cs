using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls
{
    public partial class AdmZastepstwa : System.Web.UI.UserControl
    {
        private const string active_menu = "azastmenu";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(mnLeft, active_menu);
                SelectMenu(false);
            }
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }

        public void SelectMenu(bool setHelp)  
        {
            string sel = mnLeft.SelectedValue;
            Session[active_menu] = sel;            
            switch (sel)
            {
                case "0":
                    mvSettings.SetActiveView(vZastepstwa);
                    //if (setHelp) Info.SetHelp(Info.HELP_);
                    break;
                case "1":
                    mvSettings.SetActiveView(vHistoria);
                    //if (setHelp) Info.SetHelp(Info.HELP_);
                    break;
            }
        }
    }
}
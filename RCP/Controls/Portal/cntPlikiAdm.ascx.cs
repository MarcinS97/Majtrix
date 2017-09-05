using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class cntPlikiAdm : System.Web.UI.UserControl
    {
        private const string active_menu = "aplikimn";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(mnLeft, active_menu);
                SelectMenu();
            }
        }

        private void SelectMenu()
        {
            string sel = mnLeft.SelectedValue;
            Session[active_menu] = sel;
            cntPliki.Grupa = sel;
        }
        
        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu();
        }
    }
}
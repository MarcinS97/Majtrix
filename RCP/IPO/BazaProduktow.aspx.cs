using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.IPO
{
    public partial class BazaProduktow : System.Web.UI.Page
    {
        private const string active_tab = "pgProdukty"; 
        AppUser user;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
   
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("IPO Baza Produków Form");
        }

        protected void IPOtabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            
            SelectTab(e.Item.Text.ToString());
        }



        private void SelectTab(String zakladka)
        {
            Session[active_tab] = IPOBazaProduktow.SelectedValue;
            Tools.SelectTab(IPOBazaProduktow, mvBazaProduktow, active_tab, false);
 

        }
    }
}

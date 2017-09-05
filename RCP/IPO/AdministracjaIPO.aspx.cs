using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.IPO.Controls;
using HRRcp.IPO.App_Code;

namespace HRRcp.IPO
{
    public partial class IPOAdmin : System.Web.UI.Page
    {
        private const string active_tab = "pgAdministratorzy"; 
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
            AppError.Show("IPO Administracja Form");
        }

        protected void IPOtabAdmin_MenuItemClick(object sender, MenuEventArgs e)
        {
            
            SelectTab(e.Item.Text.ToString());
        }



        private void SelectTab(String zakladka)
        {
            if(zakladka.Equals("Konfiguracja") || zakladka.Equals("Produkty") || zakladka.Equals("Dostawcy"))
            {
                Session[active_tab] = Config.SelectedValue;
                Tools.SelectTab(Config, mvAdministracja, active_tab, false);
                if (IPOTabAdmin.SelectedItem != null)
                {
                    IPOTabAdmin.SelectedItem.Selected = false;
                }
            }else
            {             
                Session[active_tab] = IPOTabAdmin.SelectedValue;            
                Tools.SelectTab(IPOTabAdmin, mvAdministracja, active_tab, false);
                if (Config.SelectedItem != null)
                {
                    Config.SelectedItem.Selected = false;
                }
            }

        }

        protected void TabOnActivate(object sender, EventArgs e)
        {
            View v = sender as View;
            try
            {
                cntAkceptujacyAdmin caa = v.FindControl("cntUprawnieniaAkceptujacy") as cntAkceptujacyAdmin;
                caa.refresh();
            }
            catch (Exception ex)
            {
            }
        }
    }
}

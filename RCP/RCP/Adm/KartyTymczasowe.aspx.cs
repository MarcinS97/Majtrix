using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp
{
    public partial class KartyTymczasowe : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();
                if (App.User.HasRight(AppUser.rKartyTmp))
                {
                    /*
                    if (String.IsNullOrEmpty(App.KartaRCP))
                        Tools.SelectMenuFromSession(tabPrzypisz, active_tab);
                    else
                     */
                    //tabPrzypisz.Items[0].Selected = true;   // zawsze na zakładkę przypisz, skrypty sie nie ładują, albo przeniesc do common.js
                    //SelectTab();
                }
                else
                    App.ShowNoAccess("Wydawanie identyfikatorów tymczasowych", App.User);
            }
        }

        protected void tabPrzypisz_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

        protected void vPrzypisz_Activate(object sender, EventArgs e)
        {

        }

        protected void vPrzypisz_Deactivate(object sender, EventArgs e)
        {

        }


    }
}
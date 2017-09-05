using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp
{
    public partial class PrzypiszRCP_x : System.Web.UI.Page
    {
        private const string active_tab = "atabPrzypisz";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SetNoCache();

                if (App.User.IsAdmin)
                {
                    /*
                    if (String.IsNullOrEmpty(App.KartaRCP))
                        Tools.SelectMenuFromSession(tabPrzypisz, active_tab);
                    else
                     */
                        tabPrzypisz.Items[0].Selected = true;   // zawsze na zakładkę przypisz, skrypty sie nie ładują, albo przeniesc do common.js
                    SelectTab();
                }
                else
                    App.ShowNoAccess("Przypisywanie numerów kart RCP", App.User);
            }
            App._PrzypisywanieRCP = true;
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(null, "Przypisywanie numerów kart RCP");
        }
        //-----------------------------------
        protected void tabPrzypisz_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        private void SelectTab()
        {
            Tools.SelectTab(tabPrzypisz, mvPrzypisz, active_tab, true);
        }
        //-----------------------------------

        protected void cntPrzypiszRCPHistoria1_Deleted(object sender, EventArgs e)
        {
            RqRefresh = true;
        }

        public bool RqRefresh
        {
            get { return Tools.GetBool(ViewState["refresh"], false); }
            set { ViewState["refresh"] = value; }
        }

        protected void vPrzypisz_Activate(object sender, EventArgs e)
        {
            if (RqRefresh)
                cntPrzypiszRCP1.DataBind();
        }

        protected void vPrzypisz_Deactivate(object sender, EventArgs e)
        {
            cntPrzypiszRCP1.SetSelected(-1);
        }
    }
}

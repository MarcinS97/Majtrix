using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
//using KDR.Controls;

namespace HRRcp.MatrycaSzkolen
{
    public partial class SzkoleniaAdmin : System.Web.UI.Page
    {
        private const string active_tab = "atabAdminUpr";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string FormTitle = "Panel Administratora - Uprawnienia";

        const string FormName = "Uprawnienia Admin";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (App.User.HasAccess)
                {
                    if (HasFormAccess)
                    {
                        Tools.SetNoCache(); 
                        Tools.EnableUpload();
                        Tools.SelectMenuFromSession(mnTabs, active_tab);
                        SelectTab();
                        Info.SetHelp(FormName);
                        TranslatePage();
                    }
                    else
                        App.ShowNoAccess(FormName, App.User);

                }
                else
                    App.ShowNoAccess(FormName, App.User);
            }
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            switch (mnTabs.SelectedValue)
            {
                case "0":
                    //((MasterPage)Master).SetWideJs(true);
                    break;
                default:
                    //((MasterPage)Master).SetWideJs(false);
                    break;
            }
            base.OnPreRender(e);
        }

        private void TranslatePage()
        {
            L.p(mnTabs);
        }

        //-----------------------------------
        protected void mnTabs_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectTab();
        }

        private void SelectTab()
        {
            Tools.SelectTab(mnTabs, mvAdmin, active_tab, true);
        }

        //-----------------------------------
        protected void vTypy_Activate(object sender, EventArgs e)
        {
        }

        protected void vUprawnienia_Activate(object sender, EventArgs e)
        {
            //cntUprawnieniaCzynnosciEdit1.DataBind();
        }

        protected void vGrupy_Activate(object sender, EventArgs e)
        {
            //cntUprawnieniaGrupy.DataBind();
        }
        //--------------------------------------

        public static bool HasFormAccess
        {
            get { return App.User.IsMSAdmin; }
        }
    
    }
}

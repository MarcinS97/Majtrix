using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
//using KDR.Controls;

namespace HRRcp.SzkoleniaBHP
{
    public partial class AdminUprawnieniaForm : System.Web.UI.Page
    {
        private const string active_tab = "atabAdminUpr";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie
        private const string FormTitle = "Panel Administratora - Uprawnienia";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();
                //if (user.IsAdmin)
                if (App.User.HasRight(AppUser.rSzkoleniaBHPAdm))
                {
                    Tools.SetNoCache();
                    Tools.EnableUpload();  // ponieważ mamy FileUpload1 na formatce to musze to dodać bo za pierwszym razem nie widzi wpisanego pliku
                    
                    Tools.SelectMenuFromSession(mnTabs, active_tab);
                    SelectTab();
                    
                    //Info.SetHelp(Info.HELP_ADM_UPRAWNIENIA);
                    Info.SetHelp("HSZKOLBHPADM");
                    Tools.MakeBackButton(btBack);

                    TranslatePage();
                }
                else
                    App.ShowNoAccess(L.p(FormTitle), user);
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

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(L.p(FormTitle));
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
    
    }
}

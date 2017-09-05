using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntPodzialLudziKier : System.Web.UI.UserControl
    {
        private const string active_menu = "mn_podzL";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool adm    = App.User.HasRight(AppUser.rPodzialLudziAdm);
                bool PL     = App.User.HasRight(AppUser.rPodzialLudzi);
                bool PM     = App.User.HasRight(AppUser.rPodzialLudziPM);
                bool edS    = App.User.HasRight(AppUser.rPodzialLudziEditS);         
                bool edSG   = App.User.HasRight(AppUser.rPodzialLudziEditSGrupy);    // Grupy splitów
                bool edCAP  = App.User.HasRight(AppUser.rPodzialLudziEditCAP);       // Comodity Area Position
                bool lim    = App.User.HasRight(AppUser.rRaportCCLim);
                bool edLim  = App.User.HasRight(AppUser.rEditCCLim);

                if (!(adm || PL))
                {
                    Tools.RemoveMenu(mnLeft, "vPodzial");
                }
                if (!(adm || PM))
                {
                    Tools.RemoveMenu(mnLeft, "vRaportyPM");
                    Tools.RemoveMenu(mnLeft, "vRaportDL");
                }
                if (!(edS || edCAP || edSG))
                {
                    Tools.RemoveMenu(mnLeft, "DIC3");
                }
                if (!(adm || edSG))
                {
                    Tools.RemoveMenu(mnLeft, "vSplity");
                }   
                if (!(adm || edCAP))
                {
                    Tools.RemoveMenu(mnLeft, "vCC");
                    Tools.RemoveMenu(mnLeft, "vCommodity");
                    Tools.RemoveMenu(mnLeft, "vArea");
                    Tools.RemoveMenu(mnLeft, "vPosition");
                }
                if (!adm)
                {
                    Tools.RemoveMenu(mnLeft, "vUprawnienia");
                    Tools.RemoveMenu(mnLeft, "vLimity");                    
                }

                if (!App.User.HasRight(AppUser.rRaportIlDL))                        // !!! TYMCZAS docelowo widoczne jeżeli jestem PM
                    Tools.RemoveMenu(mnLeft, "vRaportDL");

#if SIEMENS
                Tools.RemoveMenu(mnLeft, "vCommodity");
                Tools.RemoveMenu(mnLeft, "vArea");
                Tools.RemoveMenu(mnLeft, "vPosition");
#endif
                if (Lic.limityNCC)
                {
                    if (!(adm || lim))
                    {
                        Tools.RemoveMenu(mnLeft, "vRaportLimity");
                    }
                    if (!(adm || edLim))
                    {
                        Tools.RemoveMenu(mnLeft, "vLimity");
                    }
                }
                else 
                {
                    Tools.RemoveMenu(mnLeft, "vRaportLimity");
                    Tools.RemoveMenu(mnLeft, "vLimity");
                }

                Tools.SelectMenuFromSession(mnLeft, active_menu);
                SelectMenu(false);
            }
        }

        public void Prepare()
        {
            string kid = App.User.Id;
            DateTime dt = DateTime.Today;
            //...
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, active_menu, setHelp);
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            string url, sel, help, par;

            Tools.GetLineParams(mnLeft.SelectedValue, out sel, out help, out par);

            if (Tools.IsUrl(par, out url))         // UWAGA !!! SelectedValue musi być znacznik|opcja/help - po znaczniku usuwam menu <<<< jeszcze do zmiany bo help
            {
                Session[active_menu] = Tools.GetLineParam(mnLeft.Items[0].Value, 0);    // domyślnie zawsze pierwsza opcja zaznaczona - wypadałoby sprawdzić czy jest selectable
                App.Redirect(url);
            }
            else

                switch (mnLeft.SelectedValue)
                {

                    case "vUprawnienia":
                        Session[active_menu] = Tools.GetLineParam(mnLeft.Items[0].Value, 0);
                        Response.Redirect("~/PodzialLudzi/Uprawnienia.aspx");
                        break;
                    case "vRaportyPM":
                        Session[active_menu] = Tools.GetLineParam(mnLeft.Items[0].Value, 0);
                        Response.Redirect("~/PodzialLudzi/RaportPM.aspx");
                        break;
                    case "vRaportLimity":
                        Session[active_menu] = Tools.GetLineParam(mnLeft.Items[0].Value, 0);
                        Response.Redirect("~/PodzialLudzi/RaportLimityNN.aspx");
                        break;
                    case "vRaportDL":
                        Session[active_menu] = Tools.GetLineParam(mnLeft.Items[0].Value, 0);
                        Response.Redirect("~/PodzialLudzi/RaportDL.aspx");
                        break;
                    default:
                        SelectMenu(true);
                        break;
                }
        }
        //--------------------
    }
}
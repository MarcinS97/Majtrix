using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Przypisania
{
    public partial class cntPrzesunieciaAdm : System.Web.UI.UserControl
    {
        private const string active_menu = "mn_przesA";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!App.User.HasRight(AppUser.rPrzesuniecia))
                    Tools.RemoveMenu(mnLeft, "vPrzesun");
                if (!App.User.HasRight(AppUser.rPrzesunieciaAcc))
                    Tools.RemoveMenu(mnLeft, "vDoAkceptacji");

                if (!App.User.HasRight(AppUser.rPrzesunieciaAdm))
                {
                    Tools.RemoveMenu(mnLeft, "DIC3");
                    Tools.RemoveMenu(mnLeft, "vSplity");
                    Tools.RemoveMenu(mnLeft, "vCC");
                    Tools.RemoveMenu(mnLeft, "vCommodity");
                    Tools.RemoveMenu(mnLeft, "vArea");
                    Tools.RemoveMenu(mnLeft, "vPosition");
                }
#if SIEMENS
                Tools.RemoveMenu(mnLeft, "vCommodity");
                Tools.RemoveMenu(mnLeft, "vArea");
                Tools.RemoveMenu(mnLeft, "vPosition");
#endif 
                Tools.SelectMenuFromSession(mnLeft, active_menu);
                SelectMenu(false);
            }
        }

        public void Prepare()
        {
             //cntPrzypisaniaEdit.Prepare(null, DateTime.Today);
            DateTime dt = DateTime.Today;
            cntPrzypisaniaEdit.Prepare(null, dt);

            cntPrzypisania1._Prepare(null, dt, false);
            cntPrzypisania2._Prepare(null, dt, false);
            cntPrzypisania3._Prepare(null, dt, false);
            cntPrzypisania4._Prepare(null, dt, false);
            
            if (mnLeft.SelectedValue == "vPrzesun")
                cntPrzypisaniaEdit.InitScript();        
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, active_menu, setHelp);
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }

        protected void vPrzesun_Activate(object sender, EventArgs e)
        {
            //Prepare();
            cntPrzypisaniaEdit.InitScript();
        }

        public bool SelectPrac(string pracId)
        {
            Tools.SelectMenu(mnLeft, "vZaakceptowane");
            SelectMenu(true);
            return cntPrzypisania3.SelectPrac(pracId);
        }
    }
}
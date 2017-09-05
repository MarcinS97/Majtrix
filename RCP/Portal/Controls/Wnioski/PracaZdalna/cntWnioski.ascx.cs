using HRRcp.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRRcp.Portal.Controls.Wnioski.PracaZdalna
{
    public partial class cntWnioski : System.Web.UI.UserControl
    {
        private const string _active_menu = "mn_wnurK";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        const int moAll = 0;
        const int moAcc = 1;
        const int moWnioski = 2;
        int FMode = moAll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // && !App.User.HasRight(AppUser.rWnioskiUrlopoweSub) && !App.User.HasRight(AppUser.rWnioskiUrlopoweAdm))
                if (!App.User.HasRight(AppUser.rWnioskiZdalnaAcc))
                {
                    Tools.RemoveMenu(mnLeft, "vDoAkceptacji");
                    Tools.RemoveMenu(mnLeft, "vZaakceptowane");
                    Tools.RemoveMenu(mnLeft, "vOdrzucone");
                }
                Tools.SelectMenuFromSession(mnLeft, _active_menu + FMode.ToString());
                SelectMenu(false);

            }
            else
            {
                //if (PopupVisible) extWniosekPopup.Show();   //fix hide on postback 
            }
        }

        public void Prepare()
        {
            string kid = App.User.Id;
            DateTime dt = DateTime.Today;
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, _active_menu + FMode.ToString(), setHelp);
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }

        //--------------------
        protected void cntWnioskiUrlopowe1_Show(object sender, EventArgs e)
        {
            ToUpdate = 1;  // do akceptacji
            int wnid = ((cntWnioskiUrlopowe)sender).ShowWniosekId;
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osKierownik, false);

            UpdatePanel5.Update();
        }

        protected void cntWnioskiUrlopoweMoje_Show(object sender, EventArgs e)
        {
            ToUpdate = 2;   // moje
            int wnid = ((cntWnioskiUrlopowe)sender).ShowWniosekId;
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osPracownik, false);

        }

        public void Update()
        {
            cntWnioskiUrlopowe2.List.DataBind();
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            if (cntWniosekUrlopowy1.Updated)
                switch (ToUpdate)
                {
                    case 1:     // do akceptacji
                        cntWnioskiUrlopowe2.List.DataBind();    // do acc
                        cntWnioskiUrlopowe3.List.DataBind();    // zaakceptowane
                        cntWnioskiUrlopowe4.List.DataBind();    // odrzucone
                        break;
                }
        }

        public int ToUpdate
        {
            set { ViewState["toupdate"] = value; }
            get { return Tools.GetInt(ViewState["toupdate"], -1); }
        }

        public int Mode
        {
            set { FMode = value; }
            get { return FMode; }
        }
    }
}
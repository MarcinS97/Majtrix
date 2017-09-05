using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Portal.Controls
{
    public partial class paWnioskiUrlopoweKier3 : System.Web.UI.UserControl
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

                if (!App.User.HasRight(AppUser.rPrzesuniecia))
                {
                    Tools.RemoveMenu(mnLeft, "vPrzesun");
                }

                // && !App.User.HasRight(AppUser.rWnioskiUrlopoweSub) && !App.User.HasRight(AppUser.rWnioskiUrlopoweAdm))
                if (!App.User.HasRight(AppUser.rWnioskiUrlopoweAcc) || FMode == moWnioski)
                {
                    Tools.RemoveMenu(mnLeft, "vDoAkceptacji");
                    Tools.RemoveMenu(mnLeft, "vZaakceptowane");
                    Tools.RemoveMenu(mnLeft, "vOdrzucone");
                }
                if (!App.User.HasRight(AppUser.rWnioskiUrlopowe) || FMode == moAcc)
                {
                    Tools.RemoveMenu(mnLeft, "vEnter");
                    Tools.RemoveMenu(mnLeft, "vMojeWnioski");
                }
                Tools.SelectMenuFromSession(mnLeft, _active_menu + FMode.ToString());
                SelectMenu(false);

                Urlop1._PrepareKier(Tools.DateToStr(DateTime.Today), App.User.Id);
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
            /*
            cntPrzypisaniaEdit.Prepare(kid, dt);
            cntPrzypisania1.Prepare(kid, dt, false);
            cntPrzypisania2.Prepare(kid, dt, false);
            cntPrzypisania3.Prepare(kid, dt, false);
            cntPrzypisania4.Prepare(kid, dt, false);
            if (mnLeft.SelectedValue == "vPrzesun")
                cntPrzypisaniaEdit.InitScript();
            */
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, _active_menu + FMode.ToString(), setHelp);
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }

        protected void vPrzesun_Activate(object sender, EventArgs e)
        {
            //cntPrzypisaniaEdit.InitScript();
        }

        //--------------------
        protected void cntWnioskiUrlopowe1_Show(object sender, EventArgs e)
        {
            ToUpdate = 1;  // do akceptacji
            int wnid = ((cntWnioskiUrlopowe3)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osKierownik, false);
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osKierownik, false);

            UpdatePanel5.Update();
        }

        protected void cntWnioskiUrlopoweMoje_Show(object sender, EventArgs e)
        {
            ToUpdate = 2;   // moje
            int wnid = ((cntWnioskiUrlopowe3)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osPracownik, false);
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osPracownik, false);

            //UpdatePanel5.Update();
        }

        private void Update()
        {
            //UpdatePanel up = Tools.FindUpdatePanel(this);
            //if (up != null && up.UpdateMode == UpdatePanelUpdateMode.Conditional)
            //    up.Update();
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
                        Update();
                        break;
                    case 2:     // moje
                        cntWnioskiUrlopowe1.List.DataBind();
                        Update();
                        break;
                    case 3:     // dla pracownika
                        //cntWnioskiUrlopowe5.List.DataBind();
                        cntWnioskiUrlopowe5.DataBind();         // dla pracownika      // cnt musi lezec na update panelu nieconditional, bo inaczej nie odświeża mimo bind
                        if (App.User.HasRight(AppUser.rWnioskiUrlopoweAcc))
                        {
                            cntWnioskiUrlopowe2.List.DataBind();    // do acc
                            cntWnioskiUrlopowe3.List.DataBind();    // zaakceptowane
                        }
                        Update();
                        break;
                }
        }
    
        protected void cntWnioskiUrlopoweSelect1_Select(object sender, EventArgs e)
        {
            int wtyp = Tools.StrToInt(cntWnioskiUrlopoweSelect1.SelectedTyp, -1);
            if (wtyp != -1)
            {
                ToUpdate = 2;   // moje
                App.KwitekPracId = App.User.Id;
                App.KwitekKadryId = App.User.NR_EW;
                //cntWniosekUrlopowy1.PrepareNewPopup(extWniosekPopup, wtyp, cntWniosekUrlopowy.osPracownik);

                cntWniosekUrlopowy1.PrepareNew(wtyp, cntWniosekUrlopowy.osPracownik);
               
            }
        }

        protected void cntWnioskiUrlopoweSelect2_Select(object sender, EventArgs e)
        {
            int wtyp = Tools.StrToInt(cntWnioskiUrlopoweSelect2.SelectedTyp, -1);
            if (wtyp != -1)
            {
                ToUpdate = 3;   // dla pracownika
                //cntWniosekUrlopowy1.PrepareNewPopup(extWniosekPopup, wtyp, cntWniosekUrlopowy.osKierownik);

                cntWniosekUrlopowy1.PrepareNew(wtyp, cntWniosekUrlopowy.osKierownik);

                //UpdatePanel5.Update();    // jezeli wywołanie jest z updatepanel to trzeba też zrobić update
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
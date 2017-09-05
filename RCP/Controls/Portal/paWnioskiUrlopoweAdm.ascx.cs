using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;

namespace HRRcp.Controls.Portal
{
    public partial class paWnioskiUrlopoweAdm : System.Web.UI.UserControl
    {
        private const string active_menu = "mn_wnurA";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!App.User.HasRight(AppUser.rPrzesuniecia))
                {
                    Tools.RemoveMenu(mnLeft, "vPrzesun");
                }

                //if (!App.User.HasRight(AppUser.rWnioskiUrlopoweAcc) && !App.User.HasRight(AppUser.rWnioskiUrlopoweSub) && !App.User.HasRight(AppUser.rWnioskiUrlopoweAdm))
                if (!App.User.HasRight(AppUser.rWnioskiUrlopowe))
                {
                    Tools.RemoveMenu(mnLeft, "vMojeWnioski");
                }
                Tools.SelectMenuFromSession(mnLeft, active_menu);
                SelectMenu(false);
            }
        }

        public void _Prepare()
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

        public void Prepare(bool init)
        {
            string kid = App.User.Id;
            DateTime dt = DateTime.Today;

            if (init && !Initialized)
            {
                Initialized = true;
                bool bind = false;
                cntWnioskiUrlopowe1.Prepare(bind);
                cntWnioskiUrlopowe2.Prepare(bind);
                cntWnioskiUrlopowe3.Prepare(bind);
                cntWnioskiUrlopowe4.Prepare(bind);
                cntWnioskiUrlopowe5.Prepare(bind);
                cntWnioskiUrlopowe6.Prepare(bind);
            }
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(mnLeft, mvPrzesuniecia, active_menu, setHelp);

            /*
            View v = mvPrzesuniecia.GetActiveView();
            if (v != null)
            {
                foreach (Control c in v.Controls)
                    if (c is cntWnioskiUrlopowe)
                        ((cntWnioskiUrlopowe)c).Prepare(false);
            }
            */ 
        }

        protected void mnLeft_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }

        protected void vPrzesun_Activate(object sender, EventArgs e)
        {
            //cntPrzypisaniaEdit.InitScript();
        }
        //----- wnioski urlopowe ------
        protected void cntWnioskiUrlopowe1_Show(object sender, EventArgs e)
        {
            ToUpdate = 1;  // do akceptacji
            int wnid = ((HRRcp.Portal.Controls.cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin, false);
            //extWniosekPopup.Show();

            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osAdmin, false);
            //UpdatePanel5.Update();  // zoom na urlopy - popup jquery wymaga Conditional bo sortowanie ukrywa
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin, false);
        }

        protected void cntWnioskiUrlopoweMoje_Show(object sender, EventArgs e)
        {
            ToUpdate = 2;   // moje
            int wnid = ((HRRcp.Portal.Controls.cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osPracownik, false);
            //extWniosekPopup.Show();
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osPracownik, false);
            //UpdatePanel5.Update();  // zoom na urlopy - popup jquery wymaga Conditional bo sortowanie ukrywa
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osPracownik, false);
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            if (cntWniosekUrlopowy1.Updated)
                switch (ToUpdate)
                {
                    case 1:     // do akceptacji
                        cntWnioskiUrlopowe2.DataBind();
                        cntWnioskiUrlopowe4.DataBind();     // odrzucone
                        cntWnioskiUrlopowe5.DataBind();
                        break;
                    case 2:     // moje
                        cntWnioskiUrlopowe1.DataBind();
                        break;
                    case 3:     // dla pracownika
                        cntWnioskiUrlopowe3.DataBind();     // 
                        cntWnioskiUrlopowe2.DataBind();     // do akceptacji
                        cntWnioskiUrlopowe5.DataBind();     // do wprowadzenia
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
                //cntWniosekUrlopowy1.PrepareNewPopup(extWniosekPopup, wtyp, cntWniosekUrlopowy.osAdmin);
                cntWniosekUrlopowy1.PrepareNew(wtyp, cntWniosekUrlopowy.osAdmin);
            }
        }

        protected void btEnter_Click(object sender, EventArgs e)
        {
            //Response.Redirect(App.WnioskiUrlopoweWpiszForm);
            App.Redirect(App.WnioskiUrlopoweWpiszForm);
        }

        public int ToUpdate
        {
            set { ViewState["toupdate"] = value; }
            get { return Tools.GetInt(ViewState["toupdate"], -1); }
        }

        public bool Initialized
        {
            set { ViewState["init"] = value; }
            get { return Tools.GetBool(ViewState["init"], false); }
        }

        protected void mvPrzesuniecia_ActiveViewChanged(object sender, EventArgs e)
        {
            View v = mvPrzesuniecia.GetActiveView();
            if (v != null)
            {
                foreach (Control c in v.Controls)
#if PORTAL
                    if (c is HRRcp.Portal.Controls.cntWnioskiUrlopowe)
                    {
                        ((HRRcp.Portal.Controls.cntWnioskiUrlopowe)c).PrepareOnShow();
                        break;
                    }
#else
                    if (c is cntWnioskiUrlopowe)
                    {
                        ((cntWnioskiUrlopowe)c).PrepareOnShow();
                        break;
                    }
#endif
            }
        }


    }
}
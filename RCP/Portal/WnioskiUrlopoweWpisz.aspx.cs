using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRRcp.App_Code;
using HRRcp.Controls.Portal;

namespace HRRcp.Portal
{
    public partial class WnioskiUrlopoweWpisz : System.Web.UI.Page
    {
        private const string active_menu = "active_mnwnu";  // moze byc to samo dla K1 i K2 bo nigdy na tym samym nie będa pracować jednoczesnie

        protected void Page_PreInit(object sender, EventArgs e)
        {
#if PORTAL
            this.MasterPageFile = App.GetMasterPage();//App.PortalMaster;    //setmasterpage
#endif
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Tools.SelectMenuFromSession(tabWnioski, active_menu);
                SelectMenu(true);
                NeedRefresh(mvWnioski);
            }
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Wprowadzanie Wniosków Urlopowych");
        }
        //-----------------------
        public void Prepare()
        {
            string kid = App.User.Id;
            DateTime dt = DateTime.Today;
        }

        private void SelectMenu(bool setHelp)
        {
            Tools.SelectTab(tabWnioski, mvWnioski, active_menu, setHelp);
        }

        protected void tabWnioski_MenuItemClick(object sender, MenuEventArgs e)
        {
            SelectMenu(true);
        }
        //------------------------------------------------
        //public static void NeedRefresh(object vs)      // ViewState["asdf"]
        //public static void NeedRefresh(MultiView mv)      // ViewState["asdf"]
        public void NeedRefresh(MultiView mv)      // ViewState["asdf"]
        {
            int idx = mv.ActiveViewIndex;
            int bit = 1 << idx;
            ViewState[mv.ClientID + "needref"] = 0xFFFF & ~bit;   // bez inicjatora
            //((Control)mv.Parent.Parent).ViewState["needref"] = 0xFFFF;
        }

        //public static bool NeedRefresh(object vs, int idx, bool clear)   // idx 0,1,2,3,4,5... - index zakładki
        //public static bool NeedRefresh(MultiView mv, bool clear)   // idx 0,1,2,3,4,5... - index zakładki
        public bool NeedRefresh(MultiView mv, bool clear)   // idx 0,1,2,3,4,5... - index zakładki
        {
            int idx = mv.ActiveViewIndex;
            object vs = ViewState[mv.ClientID + "needref"];
            int nr = Tools.GetInt(vs, 0);
            int bit = 1 << idx;
            bool ret = (nr & bit) != 0;
            if (ret && clear)           // jak nie ustawiony to nie ma sensu czyścić
            {
                nr &= ~bit;
                ViewState[mv.ClientID + "needref"] = nr;
            }
            return ret;
        }

        protected void vDoWprowadzenia_Activate(object sender, EventArgs e)
        {
            if (NeedRefresh(mvWnioski, true))
                cntWnioskiUrlopowe1.DataBind();
        }

        protected void vWprowadzone_Activate(object sender, EventArgs e)
        {
            if (NeedRefresh(mvWnioski, true))
                cntWnioskiUrlopowe2.DataBind();
        }

        protected void vDoWyjasnienia_Activate(object sender, EventArgs e)
        {
            if (NeedRefresh(mvWnioski, true))
                cntWnioskiUrlopowe3.DataBind();
        }
        //----- wnioski urlopowe ------
        protected void cntWnioskiUrlopowe1_Show(object sender, EventArgs e)
        {
            int wnid = ((Portal.Controls.cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osAdmin, false);
            //UpdatePanel5.Update();  // zoom na urlopy - popup jquery wymaga Conditional bo sortowanie ukrywa
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin, false);
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
        }

        protected void cntWnioskiUrlopowe1_DataBound(object sender, EventArgs e)
        {
            /*  trochę przeciętnie działa - nie daję, bo za dużo czasu potrzeba zeby zrobić
            int cnt = ((cntWnioskiUrlopowe)sender).RecCount;
            if (sender == cntWnioskiUrlopowe1)
                tabWnioski.Items[0].Text = String.Format("{0} ({1})", "Wnioski do wprowadzenia", cnt);
            if (sender == cntWnioskiUrlopowe2)
                tabWnioski.Items[1].Text = String.Format("{0} ({1})", "Wprowadzone", cnt);
            if (sender == cntWnioskiUrlopowe3)
                tabWnioski.Items[2].Text = String.Format("{0} ({1})", "Do wyjaśnienia", cnt);
            tabWnioski.DataBind();
            UpdatePanel6.Update();
             */ 
        }

        protected void cntWnioskiUrlopowe1_Changed(object sender, EventArgs e)
        {
            NeedRefresh(mvWnioski); 
        }



        //----- wprowadzanie -----
        protected void btNext_Click(object sender, EventArgs e)
        {
            int idx = cntWnioskiUrlopowe1.List.SelectedIndex;
            if (idx != -1) cntWnioskiUrlopowe1.SetWprowadzony(idx, true);
            idx++;
            if (idx < cntWnioskiUrlopowe1.List.Items.Count)
            {
                cntWnioskiUrlopowe1.List.SelectedIndex = idx;
            }
            else
            {
                cntWnioskiUrlopowe1.List.SelectedIndex = -1;
                Tools.ShowMessage("Koniec.");
            }
        }
    }
}
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        public bool Prepare()
        {
            bool z = Zastepstwa.Prepare(App.User.Id) > 0;
            divZastepstwa.Visible = z;

            bool p = App.User.HasRight(AppUser.rPrzesunieciaAcc) && cntPrzypisania.Prepare(App.User.Id, DateTime.Today, true) > 0;
            divPrzesuniecia.Visible = p;

            bool we = App.User.HasRight(AppUser.rWnioskiUrlopoweAdm) && cntWnioskiUrlopoweAdm.Prepare() > 0;
            divWnioskiAdm.Visible = we;

            bool wa = App.User.HasRight(AppUser.rWnioskiUrlopoweAcc) && cntWnioskiUrlopoweKier.Prepare() > 0;
            divWnioskiKier.Visible = wa;

            bool nop = !z && !p && !we && !wa;
            divNothingToDo.Visible = nop;
            return !nop;
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            App.Master.SetWideJs(false);
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(null, "Formatka startowa");
        }

        //----- Wnioski Urlopowe -----
        // cntWniosek musi być na UpdatePanelu bo inaczej nie wyświetla danych !!!
        protected void cntWnioskiUrlopoweAdm_Show(object sender, EventArgs e)
        {
            int wnid = ((cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin);
            //extWniosekPopup.Show();
            cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osAdmin, false);
            UpdatePanel5.Update();  // zoom na urlopy - popup jquery wymaga Conditional bo sortowanie ukrywa
        }

        protected void cntWnioskiUrlopoweKier_Show(object sender, EventArgs e)
        {
            int wnid = ((cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osKierownik);
            //extWniosekPopup.Show();
            cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osKierownik, false);
            UpdatePanel5.Update();
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            if (cntWniosekUrlopowy1.Updated)
            {
                if (cntWniosekUrlopowy1.Osoba == cntWniosekUrlopowy.osKierownik)
                    cntWnioskiUrlopoweKier.DataBind();   // mozna sprawdzić czy dodał nowy wniosek
                if (cntWniosekUrlopowy1.Osoba == cntWniosekUrlopowy.osAdmin)
                    cntWnioskiUrlopoweAdm.DataBind();    // mozna sprawdzić czy dodał nowy wniosek
            }
        }

        protected void btEnter_Click(object sender, EventArgs e)
        {

        }
    }
}
        */
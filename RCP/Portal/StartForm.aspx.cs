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
    public partial class StartForm : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.MasterPageFile = App.GetMasterPage();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            /*----- -----*/
            //int ret = db.execProc("importPLSplity '20150201', '20150208'");
            //int ret = db.execProc("return exec importPLSplity '20150201', '20150208';");
            //string ret = db.getScalar("select dbo.importPLSplity(null, '20150201', '20150208')");
            //int ret = PodzialLudzi.execProc("importPLSplity", null, (DateTime)Tools.StrToDateTime("2015-02-01"), (DateTime)Tools.StrToDateTime("2015-02-08"));



            /*---- testy ---------------------
            string nr_ew = "12345";
            string imie = "Grzegorz";
            //string nazwisko = "Brzęk";
            string nazwisko = "Brzęczyszczykiewicz";

            string login = "login_" + nr_ew;
            string email = "email_" + nr_ew;
            string rights = null;
            //----- wartości domyślne -----
            string i = db.RemovePL(imie);
            string n = db.RemovePL(nazwisko);
            string m = String.Format("{0}.{1}@iqor.com", i, n);
            string idM = db.getScalar("select Id from Pracownicy where EMail = " + db.strParam(m));
            if (String.IsNullOrEmpty(idM))
                email = m;
            m = Tools.Substring(n, 0, 7) + Tools.Substring(i, 0, 1);
            idM = db.getScalar("select Id from Pracownicy where Login = " + db.strParam(m));
            if (String.IsNullOrEmpty(idM))
                login = m;
            //rights = new String('0', 50);
            //rights[AppUser.rPortalTmp] = '1';
            //rights[AppUser.rWnioskiUrlopowe] = '1';
            rights = "00000000000000000000000000001001";
            if (rights[AppUser.rWnioskiUrlopowe] != '1' ||      // kontrola
                rights[AppUser.rPortalTmp] != '1')
                rights = null;

            /*---------------------------------*/



        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool show = Tools.GetStr(Session["start"]) == "1";
                Session["start"] = null;  // tylko za pierwszym razem
                if (!Prepare())
                    if (!show)
                    {
#if PORTAL


                        
                        if (App.User.IsPortalAdmin)
                            App.Redirect(App.PortalPracForm);
                        else if (App.User.IsKierownik)
                            App.Redirect(App.PortalKierForm);
                        else
                            App.Redirect(App.PortalPracForm);
                         


#else
                        //if (Page.PreviousPage != null && Page.PreviousPage.ClientID == App.DefaultForm)
                        if (App.User.IsAdmin)
                            Response.Redirect("~/" + App.AdminForm);
                        else
                            Response.Redirect("~/" + App.KierownikForm);
#endif
                    }
                Info.SetHelp(Info.HELP_START);

                //Tools.ExecOnStart2("xxx", "test('asd/dfg//hjk////lmno')");
            }
        }

        public bool Prepare()
        {
            bool z = Zastepstwa.Prepare(App.User.Id) > 0;
            divZastepstwa.Visible = z;

            bool p = App.User.HasRight(AppUser.rPrzesunieciaAcc) && cntPrzypisania._Prepare(App.User.Id, DateTime.Today, true) > 0;
            divPrzesuniecia.Visible = p;

            bool we = App.User.HasRight(AppUser.rWnioskiUrlopoweAdm) && cntWnioskiUrlopoweAdm.Prepare(true) > 0;
            divWnioskiAdm.Visible = we;

            bool wa = App.User.HasRight(AppUser.rWnioskiUrlopoweAcc) && cntWnioskiUrlopoweKier.Prepare(true) > 0;
            divWnioskiKier.Visible = wa;

            bool bhp = (App.User.HasRight(AppUser.rSzkoleniaBHP) || App.User.HasRight(AppUser.rSzkoleniaBHPAdm)) && cntStartBHP.Prepare() > 0;
            divSzkoleniaBHP.Visible = bhp;

            bool pz = (App.User.HasRight(AppUser.rWnioskiZdalnaAcc) && cntWnioskiPracaZdalna.Prepare(true) > 0);
            divWnioskiZdalna.Visible = pz;

            bool nop = !z && !p && !we && !wa && !bhp;
            divNothingToDo.Visible = nop;
            return !nop;
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            App.Master.SetWideJs(false);
            base.OnPreRender(e);
        }

#if !PORTAL   // dla Portalu w MasterPage3 jest ustawiana Page_Error
        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show(null, "Formatka startowa");
        }
#endif
        //----- Wnioski Urlopowe -----
        // cntWniosek musi być na UpdatePanelu bo inaczej nie wyświetla danych !!!
        protected void cntWnioskiUrlopoweAdm_Show(object sender, EventArgs e)
        {
            int wnid = ((Portal.Controls.cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin);
            //extWniosekPopup.Show();
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osAdmin, false);
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osAdmin, false);
            //UpdatePanel5.Update();  // zoom na urlopy - popup jquery wymaga Conditional bo sortowanie ukrywa
        }

        protected void cntWnioskiUrlopoweKier_Show(object sender, EventArgs e)
        {
            int wnid = ((Portal.Controls.cntWnioskiUrlopowe)sender).ShowWniosekId;
            //cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osKierownik);
            //extWniosekPopup.Show();
            //cntWniosekUrlopowy1.ShowPopup(extWniosekPopup, wnid, cntWniosekUrlopowy.osKierownik, false);
            cntWniosekUrlopowy1.Show(wnid, cntWniosekUrlopowy.osKierownik, false);
            //UpdatePanel5.Update();
        }

        protected void cntWniosekUrlopowy1_Close(object sender, EventArgs e)
        {
            if (cntWniosekUrlopowy1.Updated)
            {
                if (cntWniosekUrlopowy1.Osoba == cntWniosekUrlopowy.osKierownik)
                    cntWnioskiUrlopoweKier.DataBind();   // mozna sprawdzić czy dodał nowy wniosek
                if (cntWniosekUrlopowy1.Osoba == cntWniosekUrlopowy.osAdmin)
                {
                    cntWnioskiUrlopoweAdm.DataBind();    // mozna sprawdzić czy dodał nowy wniosek
                }
                cntWnioskiPracaZdalna.DataBind();
            }
        }

        protected void btEnter_Click(object sender, EventArgs e)
        {
            //Response.Redirect(App.WnioskiUrlopoweWpiszForm);
            App.Redirect(App.WnioskiUrlopoweWpiszForm);
        }

        protected void btStartBHP_Click(object sender, EventArgs e)
        {
            //Response.Redirect(App.WnioskiUrlopoweWpiszForm);
            App.Redirect(App.SzkoleniaBHP);
        }
    }
}

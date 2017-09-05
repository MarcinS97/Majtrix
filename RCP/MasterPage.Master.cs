using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;
using System.IO;

//"<%= Page.ResolveUrl("~/BusinessOrderInfo/page.aspx") %>">...</

namespace HRRcp
{
    //public partial class MasterPage : System.Web.UI.MasterPage
    public partial class MasterPage : RcpMasterPage
    {
        private int mnIndex = 0;

        const string mnSTART        = "START";
        const string mnADMIN        = "ADMIN";
        const string mnUSTAWIENIA   = "USTAWIENIA";
        const string mnPAKIER       = "PAKIER";
        const string mnRAPORTY      = "RAPORTY";

        const string mnSCORECARDS   = "SCARDS";

        //-----
        protected void OnPageInit(object sender, EventArgs e)
        {
            if (IsPostBack)
                Tools.CheckSessionExpired();
        }

        protected void OnPageError(object sender, System.EventArgs e)
        {
            string page = Path.GetFileName(Request.Path);   // powinna być wzięta nazwa formatki ...
            AppError.Show(page);
        }
        //-----
        protected void Page_Init(object sender, EventArgs e)
        {
#if PORTAL
            if (!(Parent is _Default))
                App.Redirect(App.DefaultForm);
#endif
#if SCARDS
            Response.Redirect(App.ScStartForm);
#endif
            //-------------------------------
            if (!(Parent is ErrorForm))
            {
                Page.Init += new EventHandler(OnPageInit);
                Page.Error += new EventHandler(OnPageError);
            }
            //-------------------------------
            if (!IsPostBack)
            {
                Tools.InitSessionExpired();
                App.SetApp(App.Rcp);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();  
            if (!IsPostBack)
            {
                AppUser user = AppUser.CreateOrGetSession();
                if (!String.IsNullOrEmpty(user.Id))
                    lbUser.Text = Tools.PrepareName(user.ImieNazwisko);
                lbProgram.Text = Tools.GetAppName();
                ShowLoggedUser(user);
                //----- kontrola dostępu -----
                bool adm  = user.IsAdmin;
                bool kier = user.IsKierownik;
                bool rap  = user.IsRaporty;
                bool rap2 = user.IsRaporty2;
                bool scards = Lic.ScoreCards && App.User.HasScAccessAdm;  // user lub admin
                //----- ustawienia moje -----
                if (adm && (user.Login.ToLower() == "wojciowt" || user.Login.ToLower() == "tomekw"))
                {
                    lbtTest.Visible = true;
                    //lbtTest2.Visible = true;
                    //lbtKwitek.Visible = true;
                    //lbtRaporty.Visible = true;
                    //lbtRaporty2.Visible = true;


                    //----- logowanie tree -----
                    /**/ 
                    lbtUserCCA.Visible = true;
                    lbtUserCC1.Visible = true;
                    lbtUserCC2.Visible = true;
                    lbtUserCC3.Visible = true;
                    lbtUserCC4.Visible = true;
                    lbtUserCC5.Visible = true;

                    paUsersTree.Visible = true;
                    /**/
                }
                //----- widoczność menu -----                   
                if (!rap) Tools.RemoveMenu(MainMenu, mnRAPORTY); // Wyniki - kierownik ma dostęp do swoich wyników
                if (adm)
                {
                    if (scards)                                 // jak jestem adminem i mam dostęp do scorecardów -> Ustawienia jako podopcja bo się nie mieści
                        Tools.RemoveMenu(MainMenu, mnUSTAWIENIA);
                }
                else
                {
                    Tools.RemoveMenu(MainMenu, mnUSTAWIENIA);   // Ustawienia
                    Tools.RemoveMenu(MainMenu, mnADMIN);        // Administracja
                }
                if (!scards)
                    Tools.RemoveMenu(MainMenu, mnSCORECARDS);

                //----- disable click na submenu -----
                if (rap)
                {
                    if (rap2)
                    {
                        //rTools.SkipMenuItemClick(MainMenu, mnRAPORTY);
                    }
                }
                //----- select menu, help -----
                string s;
                if      (Parent is AdminForm
                      || Parent is UstawieniaForm && scards
                        )                           s = mnADMIN;
                else if (Parent is UstawieniaForm && !scards)  
                                                    s = mnUSTAWIENIA;
                else if (Parent is KierownikForm
                      || Parent is BadaniaWstepne.Rejestr
                      || Parent is BadaniaWstepne.RejestrAdmin
                      || Parent is SzkoleniaBHP.Rejestr
                      || Parent is SzkoleniaBHP.AdminUprawnieniaForm
                      || Parent is Scorecards.Start
                        )                           s = mnPAKIER;
                else if (Parent is WynikiForm)      s = mnRAPORTY;
                else if (Parent is WynikiKierForm)  s = mnRAPORTY;
                else if (Parent is StartForm)       s = mnSTART;
                else s = null;
                Tools._SelectMenu(MainMenu, s, "mainMenu");
                //Info.SetHelp("H" + s);   // albo w każdej z formatek indywidualnie
                //----- widoczne elementy -----
#if SIEMENS
                cssSIE.Visible = true;
                cssSIE1.Visible = true;
                paLogo.Visible = true;
#else
                paLogo.Visible = true;
                //ximgLogo.ImageUrl = "~/images/RepLogo_iqor.png";
                //imgLogo.ImageUrl = "~/images/iqor1a.png";
                imgLogo.ImageUrl = "~/images/iQor/iqor2b.png";
                //ximgLogo.ImageUrl = "~/images/iqor3.jpg";  nieprzezroczyste
                //ximgLogo.ImageUrl = "~/images/iqor-logo.png";

                cssPortal_iQor.Visible = true;
#endif
#if VC
                paLogo.Attributes["class"] = "logo logo_demo";
                imgLogo.ImageUrl = "~/images/VC/logo.png";
#endif
                //----- info baza danych, www, mailing -----
                string info, tooltip;
                if (Tools.IsTestDb(App.ProdDB, App.ProdWWW, out info, out tooltip))
                {
                    lbDbVerInfo.Visible = true;
                    lbDbVerInfo.Text = info;
                    lbDbVerInfo.ToolTip = tooltip;
                }
            }
            if (paUsersTree.Visible)
                PrepareTest();
        }

        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (!IsPostBack)
            {
                /*
                string app = "iPO - System Zamówień Wewnetrznych";
                lbProgram.Text = app;
                FooterControl1.AppName = app;
                FooterControl1.AppVer = Tools.GetAppVersion();
                FooterControl1.RegulaminButton.Visible = false;
                */
            }
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("RCP Master Page");
        }
        //------------------------------
        public string Lp(string msg)
        {
            return L.p(msg);
            //if (!IsPostBack)
            //    return L.p(msg);
            //else
            //    return msg;
        }

        private void Translate()
        {
            /*
            L.p(btHelp);
            //L.p(ltWelcome);
            L.p(lbWelcome);
            L.p(lbProgram);
            L.p(MainMenu);
            L.p(lbZastInfo);
            L.p(lbtEndZastepstwo);

            L.p(updProgress1, "lbWait");
            */ 
        }
        //------------------------------

        //---------------------------------------------
        public override void Redirect(string page)
        {
            if (String.IsNullOrEmpty(page))
                page = MainMenu.Items[0].Value;
            switch (page)
            {
                case mnADMIN: 
                    App.Redirect(App.AdminForm);
                    break;
                case mnUSTAWIENIA:
                    App.Redirect(App.UstawieniaForm);
                    break;
                case mnPAKIER:
                    App.Redirect(App.KierownikForm);
                    break;
                case mnRAPORTY:
                    AppUser user = AppUser.CreateOrGetSession();
                    if (user.IsAdmin && MainMenu.Items.Count > 2)  // do testów potem mozna > 2 wywalic - jak jestem adminem to nie dostaje wyników kier
                        App.Redirect(App.WynikiForm);
                    else if (user.IsKierownik)
                        App.Redirect(App.WynikiKierForm);
                    break;
                case mnSTART:
                    Session["start"] = "1";
                    App.Redirect(App._StartForm);
                    break;
                //-----
                case mnSCORECARDS:
                    App.Redirect(Tools.Slash(App.wwwSCORECARDS) + App.ScStartForm);   // stąd będzie redirect
                    break;
            }
        }

        //----------------------------------
        /*
        protected string GetMenuIndex(object value)
        {
#if SIEMENS
            mnIndex++;
            int ret = 5 - MainMenu.Items.Count + mnIndex;
            return ret.ToString();
#else
            //return value.ToString();

            mnIndex++;
            int ret = 5 - MainMenu.Items.Count + mnIndex;
            return ret.ToString();
#endif
        }
        */
        protected string GetMenuIndex(int inc)
        {
            //mnIndex++;
            mnIndex += inc;
            int ret = 5 - MainMenu.Items.Count + mnIndex;
            return ret.ToString();
        }

        //------------------------------
        bool? mStart = null;
        bool? mAdmin = null;
        bool? mPaKier = null;
        bool? mRaporty = null;
        bool? mScorecards = null;

        bool? oBadania = null;
        bool? oBadaniaAdm = null;
        bool? oSzkolBHP = null;
        bool? oSzkolBHPAdm = null;
        bool? oScorecardsA = null;
        bool? oScorecardsK = null;
        bool? oUstawienia = null;
        bool? oPodzialL = null;
        bool? oRaportyAdm = null;

        protected bool IsVisible(string mn, bool defVisible)  // tu zwracana jest widoczność menu
        {
            switch (mn)
            {
                case mnSTART:
                    return false;
             
                    if (mStart == null)
                        mStart = Lic.BadaniaWstepne && App.User.HasRight(AppUser.rBadaniaWstepne) ||
                                 Lic.ScoreCards && App.User.HasScAccess;
                    return (bool)mStart;

                case mnADMIN:
                    //return false;
                    if (mAdmin == null)
                        mAdmin = Lic.BadaniaWstepne && App.User.HasRight(AppUser.rBadaniaWstepne) && App.User.IsAdmin
                              || Lic.SzkoleniaBHP && App.User.HasRight(AppUser.rSzkoleniaBHPAdm) 
                              || Lic.ScoreCards && App.User.IsScAdmin;
                    return (bool)mAdmin;
                case mnPAKIER:
                    //return false;
                    if (mPaKier == null)
                        mPaKier = App.User.IsKierownik && !App.User.IsAdmin && (
                                 Lic.BadaniaWstepne && App.User.HasRight(AppUser.rBadaniaWstepne)
                              || Lic.SzkoleniaBHP && (App.User.HasRight(AppUser.rSzkoleniaBHP) || App.User.HasRight(AppUser.rSzkoleniaBHPAdm))
                              //|| Lic.scoreCards && App.User.HasScAccess && App.User.IsAdmin
                              );
                    return (bool)mPaKier;
                case mnSCORECARDS:
                    return false;

                    if (mScorecards == null)
                        mScorecards = Lic.ScoreCards && App.User.HasScAccessAdm;
                    return (bool)mScorecards;
                case mnRAPORTY:
                    if (mRaporty == null)
                        mRaporty = App.User.IsRaporty2 || App.User.HasRight(AppUser.rSuperuser);                                // dodatkowe warunki
                    return (bool)mRaporty;
                //-----------------------
                case "TODO":
                    return true;
                case "MNPODZIALL":
                    if (oPodzialL == null)
                        oPodzialL = Lic.podzialLudzi && App.User.HasRight(AppUser.rPodzialLudziAdm);
                    return (bool)oPodzialL;
                case "MNUSTAWIENIA":
                    if (oUstawienia == null)
                        oUstawienia = Lic.ScoreCards && App.User.IsScAdmin;
                    return (bool)oUstawienia;
                case "BADWST":
                case "BADWSTK":
                    if (oBadania == null)
                        oBadania = Lic.BadaniaWstepne && App.User.HasRight(AppUser.rBadaniaWstepne);
                    return (bool)oBadania;
                case "BADWSTADM":
                case "BADWSTADMK":
                    if (oBadaniaAdm == null)
                        oBadaniaAdm = Lic.BadaniaWstepne && App.User.IsAdmin && App.User.HasRight(AppUser.rBadaniaWstepne);
                    return (bool)oBadaniaAdm;
                //-----
                case "SZKOLBHP":
                case "SZKOLBHPK":
                    if (oSzkolBHP == null)
                        oSzkolBHP = Lic.SzkoleniaBHP && (App.User.HasRight(AppUser.rSzkoleniaBHP) || App.User.HasRight(AppUser.rSzkoleniaBHPAdm));
                    return (bool)oSzkolBHP;
                    /*
                case "SZKOLBHPADM":  
                case "SZKOLBHPADMK":
                    if (oSzkolBHPAdm == null)
                        oSzkolBHPAdm = Lic.szkoleniaBHP && App.User.HasRight(AppUser.rSzkoleniaBHPAdm);
                    return (bool)oSzkolBHPAdm;
                     */ 
                //-----
                case "RAPADM":
                    if (oRaportyAdm == null)
                        oRaportyAdm = App.User.HasRight(AppUser.rSuperuser);
                    return (bool)oRaportyAdm;
                //-----
                    /*
                case "SCORECARDS":
                    if (oScorecardsA == null)
                        oScorecardsA = Lic.scoreCards && App.User.IsScAdmin;
                    return (bool)oScorecardsA;
                case "SCORECARDSK":
                    if (oScorecardsK == null)
                        oScorecardsK = Lic.scoreCards && App.User.HasScAccess && App.User.IsAdmin;
                    return (bool)oScorecardsK;
                     */ 
                //-----------------------
                default:
                    return defVisible;      // dla submenu = false, dla opcji = true
            }
        }

        /*
Admin:
Start	Administracja	Ustawienia	Panel Kierownika	Raporty
		RCP			
		Scorecards			
					
Admin+Kier:
Start	Administracja	Ustawienia	Panel Kierownika	Raporty
		RCP		                    RCP	
		Scorecards		            Scorecards	
										
Kier:
Start	Start   Panel Kierownika	Scorecards	Raporty
        */



        public string EndMenu(MenuItemTemplateContainer container)           // do wywołania po wszystkich opcjach menu w celu ukrycia ostatniego hr
        {
            string menuId = ((MenuItem)container.DataItem).Value;
            HtmlGenericControl mnCaption = container.FindControl("mmCaption") as HtmlGenericControl;
            Tools.AddClass(mnCaption, "mmCaption" + GetMenuIndex(1));       // numeracja menu do wyświetlania backgroundów
            //foreach (Control m in container.Controls)
            foreach (Control m in mnCaption.Controls)
            {
                if (m is LiteralControl) continue;
                bool isSubMenu = m is HtmlGenericControl && ((HtmlGenericControl)m).Attributes["class"] == "mmSubMenu";
                if (isSubMenu)
                {
                    int count = 0;
                    if (m.ID == menuId)
                    {
                        if (IsVisible(menuId, false))                       // widoczność całego submenu
                        {
                            Control lastCnt = null;
                            bool lastHr = true;
                            foreach (Control cnt in m.Controls)
                            {
                                if (cnt is LiteralControl) continue;
                                bool hr = cnt is HtmlGenericControl && ((HtmlGenericControl)cnt).TagName.ToLower() == "hr";
                                bool v;
                                if (hr && lastHr) 
                                    v = false;                              // ukrywam hr
                                else 
                                    v = IsVisible(cnt.ID, true);            // widoczność opcji 
                                cnt.Visible = v;
                                if (v)
                                {
                                    count++;
                                    lastCnt = cnt;
                                    lastHr = hr;                            // ustawiam, że ostatni hr       
                                }
                            }
                            if (lastHr && lastCnt != null) lastCnt.Visible = false;
                        }
                    }
                    if (count > 0) 
                        //mnCaption.Attributes["class"] = "mmIsSubMenu1";
                        Tools.AddClass(mnCaption, "mmIsSubMenu1");          // po dodaniu tej klasy submenu jest widoczne na hover
                    else
                        m.Visible = false;
                }
            }
            return null;
        }

        //----------------------------------
        protected void MainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Redirect(MainMenu.SelectedValue);
        }

        protected void mmSubmenu_Click(object sender, EventArgs e)
        {
            string addr = ((Button)sender).CommandArgument;
            if (!String.IsNullOrEmpty(addr))
            {
                if (addr.StartsWith("cmd:"))
                {
                    string par = addr.Remove(0, 4);
                    switch (par)
                    {
                        case "rap1":
                            if      (App.User.IsAdmin)     App.Redirect(App.WynikiForm);
                            else if (App.User.IsKierownik) App.Redirect(App.WynikiKierForm);
                            break;
                        case "rap2":
                            //App.Redirect();
                            break;
                        default: 
                            Redirect(par);
                            break;
                    }
                }
                else
                    App.Redirect(addr);     // jak jest postback url to sie back wywala
            }
        }

        protected void mmSubmenu_Command(object sender, CommandEventArgs e)
        {
            App.Redirect(e.CommandArgument.ToString());
        }

        protected void btHelp_Click(object sender, EventArgs e)
        {
            if (cntHelp.Visible)
                cntHelp.Show(false, true);
            else
            {
                cntHelp.Show(true, false);
                Tools.ExecOnStart2("schelpshow", String.Format("showHelp('{0}');", cntHelp.HelpClientID));   //String.Format("$('#{0}').hide(0).show(1000);", cntHelp.HelpClientID));  
            }
        }

        protected void cntHelp_HideClick(object sender, EventArgs e)
        {
            //bool ms = Parent is MainScreen;
            //btHelpK.Visible = !ms;
            //btHelp.Visible = ms;
        }

        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }

        //---------------------------------
        private void ShowLoggedUser(AppUser user)
        {
            if (!String.IsNullOrEmpty(user.Id))
            {
                if (user.IsOriginalUser)
                {
                    divZastInfo.Visible = false;
                    lbUser.Text = Tools.PrepareName(user.ImieNazwisko);
                }
                else
                {
                    divZastInfo.Visible = true;
                    lbUser.Text = String.Format("{0}<span class=\"text\">, zastępujesz: </span>{1}",
                        Tools.PrepareName(user.OriginalImieNazwisko),
                        Tools.PrepareName(user.ImieNazwisko));
                }
            }
            else
                divZastInfo.Visible = false;
        }
        //---------------------------------
        public string GetTitle()
        {
            return L.p(Tools.GetAppName());
        }

        public string GetTitleV()
        {
            double ver = getIEVer();
            return L.p(Tools.GetAppName()) + " " + ver.ToString();
        }

        private float getIEVer()
        {
            // Returns the version of Internet Explorer or a -1
            // (indicating the use of another browser).
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            if (browser.Browser == "IE")
                return (float)(browser.MajorVersion + browser.MinorVersion);
            return -1;
        }

        public string GetMetaIE8()
        {
            double ver = getIEVer();
            if (ver > 0.0)
                if (ver < 9.0)
                    return "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=8\">";
            return null;
        }

        public string GetClassIE8()
        {
            double ver = getIEVer();
            if (ver > 0.0)
                if (ver < 9.0)
                    return " class=\"ie8\"";
            return null;
        }

        public string GetCaption(object mm)
        {
            string s = mm.ToString();
            int p = s.IndexOf('|');
            return p >= 0 ? s.Substring(0, p) : s;
        }

        public string GetOpis(object mm)
        {
            string s = mm.ToString();
            int p = s.IndexOf('|');
            return p >= 0 ? s.Substring(p + 1) : null;
        }
        //---------------------------------
        public void SetWide(bool wide)
        {
            divContent.Attributes["class"] = wide ? "center_wide" : "center";
        }

        public override void SetWideJs(bool wide)
        {
            Tools.ExecOnStart("setClassById", "'" + divContent.ClientID + "'," + (wide ? "'center_wide'" : "'center'"));
        }

        public void SetWideJs(int wide)
        {
            string css;
            switch (wide)
            {
                default:
                case 0:
                    css = "center";
                    break;
                case 1:
                    css = "center_wide";
                    break;
                case 2:
                    css = "center_wide2";
                    break;
            }
            Tools.ExecOnStart("setClassById", String.Format("'{0}','{1}'", divContent.ClientID, css));
        }

        public void SetBodyScrollBar(bool visible)
        {
            if (visible)
                //Tools.RemoveClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').removeClass('noscroll');");   // uruchamia jak jest, bez -2 dokłada ();
            else
                //Tools.AddClass(htmlbody, "noscroll");
                Tools.ExecOnStart2("bodyscroll", "$('body').addClass('noscroll');");
        }

        protected void lbUser_Click(object sender, EventArgs e)
        {
            App.User.Reload(false);
            //Response.Redirect(App.DefaultForm);
            App.Redirect(App.DefaultForm);
        }

        protected void lbtUserCC_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            string login = db.getScalar("select Login from Pracownicy where Id = " + id);
            
            
            
            Session["start"] = null;



            App.LoginAsUser(login);
        }
        //---------------------------------------------------
        private void lbtPrepare(LinkButton lbt, string css, string text, string cmd, int ev)
        {
            lbt.CssClass = css;
            lbt.Text = text;
            lbt.CommandArgument = cmd;
            lbt.Visible = true;
            switch (ev)
            {
                case 1:
                    lbt.Click += new EventHandler(btLogin1_Click);
                    break;
                case 2:
                    lbt.Click += new EventHandler(btLogin2_Click);
                    break;
            }
        }

        private void PrepareTest()
        {
#if SIEMENS
#else
            lbtPrepare(LinkButton1,  "i0", "Bogdan Kardas",              "kardasb",     1);  
            lbtPrepare(LinkButton2,  "i1", "Tomek Nauman",               "naumant",     1);  
            lbtPrepare(LinkButton3,  "i2", "Michał Kazubski",            "kazubskm1",   1);
            lbtPrepare(LinkButton4,  "i3", "Dzieweczyński Juliusz",      "dziewecj",    1); 
            lbtPrepare(LinkButton5,  "i4", "Bielicki Marcin",            "bielickm1",   1);
            lbtPrepare(LinkButton6,  "i5", "Badziąg Magda",              "02408",       2);    
            lbtPrepare(LinkButton7,  "i5", "Borkowski Grzegorz",         "02684",       2);
            lbtPrepare(LinkButton8,  "i2", "Jarek Marecik",              "marecikj",    1);
            lbtPrepare(LinkButton9,  "i3", "Tomasz Wujciów",             "wojciowt",    1); 
            lbtPrepare(LinkButton10, "i2", "Sławomir Majchrzak",         "majchrzs",    1);                                
            lbtPrepare(LinkButton11, "i3", "Sabina Słowikowska (Admin)", "slowikos",    1);
            
            lbtPrepare(LinkButton12, "i0", "Pracownicy",                 "AbucewiL",    2);
            //lbtPrepare(LinkButton7,  "i0", "", "");
            //lbtPrepare(LinkButton10, "i0", "", "");
#endif
        }


        //----------------------
        protected void btLogin1_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(((LinkButton)sender).CommandArgument.ToString());
        }

        protected void btLogin2_Click(object sender, EventArgs e)
        {
            user.CheckPassLoginTest(((LinkButton)sender).CommandArgument.ToString());            
            Response.Redirect(App.WnioskiUrlopoweForm);


            /*
            App.User.LoginAsUser(((LinkButton)sender).CommandArgument.ToString());
            Response.Redirect(App.DefaultForm);
            */
            }

        protected void lbtLogo_Click(object sender, EventArgs e)
        {
            App.Redirect("~/" + App.DefaultForm);
        }

    
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HRRcp.App_Code;
using System.IO;
using HRRcp.Scorecards.Controls.Requests;
using HRRcp.Scorecards.Controls;

//"<%= Page.ResolveUrl("~/BusinessOrderInfo/page.aspx") %>">...</

namespace HRRcp.Scorecards
{
    //public partial class MasterPage : System.Web.UI.MasterPage
    public partial class ScorecardsMasterPage : RcpMasterPage
    {
        //private const string Version = "1.0.0.0";
        //private const string AppName = "Scorecards";

        private int mnIndex = 0;

        const string mnSTART        = "START";
        const string mnSCORECARDS   = "SCORECARDS";
        const string mnWNIOSKI      = "WNIOSKI";
        const string mnRAPORTY      = "RAPORTY";
        const string mnRCP          = "RCP";
        const string mnUSTAWIENIA   = "USTAWIENIA";
        const string mnADMIN        = "ADMIN";

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
#if !SCARDS
            Info.Show("Scorecards", "Funkcjonalność w przygotowaniu", Info.ibBack); 
            //App.Redirect(App.DefaultForm);
#endif
            //-------------------------------
            if (!(Parent is ErrorForm))
            {
                Page.Init += new EventHandler(OnPageInit);
                Page.Error += new EventHandler(OnPageError);
            }
            //-------------------------------
#if PORTAL
            if (!(Parent is _Default))
                App.Redirect(App.DefaultForm);
#endif
            if (!IsPostBack)
            {
                if (Lic.ScoreCards)
                {
                    if (App.User.HasAccess)
                    {
                        Tools.InitSessionExpired();
                        //App.SetApp(App.Rcp);
                    }
                    else
                        //AppError.Show(L.p("Błąd autoryzacji użytkownika"), L.p("Użytkownik: {0} nie ma uprawnień zezwalających na dostęp do systemu.", user.ImieNazwiskoOrLogin), AppError.btNone);
                        App.ShowNoAccess(L.p("Błąd autoryzacji użytkownika"), user);
                }
                else
                    App.Redirect(App.DefaultForm);
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
                ShowLoggedUser(user);
                
                //lbProgram.Text = Tools.GetAppName();
                //lbProgram.Text = AppName;
                //----- kontrola dostępu -----
                bool _adm  = user.IsScAdmin;
                bool sc   = _adm || user.IsScTL || user.IsScKier || user.IsScZarz;
                bool kier = user.IsKierownik;
                bool rap  = user.IsRaporty;
                bool rap2 = user.IsRaporty2;
                bool _hr   = user._IsScHR && !sc;
                bool contr = user.IsScControlling && !sc;

                //----- ustawienia moje -----
                if (_adm && (user.Login.ToLower() == "wojciowt" || user.Login.ToLower() == "tomekw"))
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
                if (!rap) Tools.RemoveMenu(MainMenu, mnRAPORTY);    // Wyniki - kierownik ma dostęp do swoich wyników
                //if (adm) 
                    Tools.RemoveMenu(MainMenu, mnUSTAWIENIA);       // Ustawienia
                if (_adm)
                {
                    Tools.RemoveMenu(MainMenu, mnWNIOSKI);      // Administracja
                }
                else
                {
                    //Tools.RemoveMenu(MainMenu, mnRCP);
                    Tools.RemoveMenu(MainMenu, mnADMIN);      // Administracja
                }
                if (_hr) // tylko hr: Start, Wnioski, RCP, Raporty
                {
                    Tools.RemoveMenu(MainMenu, mnSCORECARDS);
                }
                if (contr) // tylko Controlling: Start, RCP, Raporty
                {
                    Tools.RemoveMenu(MainMenu, mnSCORECARDS);
                    Tools.RemoveMenu(MainMenu, mnWNIOSKI);
                }
                //----- disable click na submenu -----
                if (rap)
                {
                    if (rap2)
                    {
                        //Tools.SkipMenuItemClick(MainMenu, mnRAPORTY);
                    }
                }
                //----- select menu, help -----
                string s;
                if      (Parent is HRRcp.Scorecards.Start)      s = mnSTART;
                else if (Parent is HRRcp.Scorecards.Scorecards
                      || Parent is HRRcp.Scorecards.Scorecard
                        )                                       s = mnSCORECARDS;
                else if (Parent is HRRcp.Scorecards.Wnioski
                      || Parent is HRRcp.Scorecards.WnioskiAdmin
                      || Parent is HRRcp.Scorecards.WniosekPremiowy
                        )                                       s = mnWNIOSKI;
                else if (Parent is HRRcp.Scorecards.Ustawienia) s = _adm ? mnADMIN : mnUSTAWIENIA;
                else if (Parent is HRRcp.Scorecards.AdminForm
                      || Parent is HRRcp.Scorecards.Parametry
                      || Parent is HRRcp.Scorecards.Arkusze
                        )                                       s = mnADMIN;
                else if (Parent is HRRcp.Scorecards.Raporty
                      || Parent is HRRcp.Scorecards.Raport  
                        )                                       s = mnRAPORTY;
                else s = null;
                Tools._SelectMenu(MainMenu, s, "mainMenu");
                //Info.SetHelp("H" + s);   // albo w każdej z formatek indywidualnie
                //----- widoczne elementy -----
#if SIEMENS
                cssSIE.Visible = true;
                cssSIE1.Visible = true;
                paLogo.Visible = true;
                //ddlLogin.Visible = _adm;
                ddlLogin.Visible = _adm && App.User.IsSuperuser && App.User.IsOriginalUser;
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
                string app = Tools.GetAppName();
                string ver = Tools.GetAppVersion();
                lbProgram.Text = app;   
                FooterControl1.AppName = app;      //tu, bo Footer sam ustawia   
                FooterControl1.AppVer = ver;
                FooterControl1.RegulaminButton.Visible = false;
            }
            base.OnPreRender(e);
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Scorecards Master Page");
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
        //---------------------------------------------
        public override void Redirect(string page)
        {
            if (String.IsNullOrEmpty(page))
                page = MainMenu.Items[0].Value;
            switch (page)
            {
                case mnSTART:
                    Session["scstart"] = "1";
                    App.Redirect(App.ScStartForm);
                    break;
                case mnSCORECARDS:
                    App.Redirect(App.ScScorecardsForm);
                    break;
                case mnWNIOSKI:
                    if (App.User.IsScAdmin)
                        App.Redirect(App.ScWnioskiAdminForm);
                    else
                        App.Redirect(App.ScWnioskiForm);
                    break;
                case mnUSTAWIENIA:
                    App.Redirect(App.ScUstawieniaForm);
                    break;
                case mnADMIN:
                    App.Redirect(App.ScAdminForm);
                    break;
                case mnRAPORTY:
                    App.Redirect(App.ScRaportyForm);
                    break;
                //-----
                case mnRCP:
                    App.Redirect(Tools.Slash(App.wwwRCP) + App._StartForm);   // stąd będzie redirect
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
        bool? mRaporty = null;

        bool? mScorecards = null;
        bool? oWnioski = null;      //moje wnioski jak admin
        
        protected bool IsVisible(string mn, bool defVisible)  // tu zwracana jest widoczność menu
        {
            switch (mn)
            {
                case mnSTART:
                    return false;

                    if (mStart == null) mStart = Lic.ScoreCards; 
                    return (bool)mStart;
                case "TODO":
                    return true;
                case "USTAWIENIA":
                    return true;
                //-----
                case mnSCORECARDS:
                    if (mScorecards == null) mScorecards = Lic.ScoreCards && App.User.IsScAdmin;
                    return (bool)mScorecards;
                case mnADMIN:
                    if (mAdmin == null) mAdmin = App.User.IsScAdmin;
                    return (bool)mAdmin;
                case "MNWNIOSKI":
                    if (oWnioski == null) oWnioski = App.User.IsScTL || App.User.IsScKier || App.User.IsScZarz;  // może składać wnioski
                    return (bool)oWnioski;
                //-----
                case mnRAPORTY:
                    if (mRaporty == null)
                        mRaporty = App.User.IsRaporty2;                                // dodatkowe warunki
                    //return false;
                    return (bool)mRaporty;
                default:
                    return defVisible;      // dla submenu = false, dla opcji = true
            }
        }

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

        protected void mmSubmenu_Click(object sender, EventArgs e)   //cmd:START lub adres
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
                            if (App.User.IsAdmin) App.Redirect(App.WynikiForm);
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
        //--------------
        /*
        protected void btHelp_Click(object sender, EventArgs e)
        {
            if (cntHelp.Visible)
                cntHelp.Show(false, true);
            else
                cntHelp.Show(true, false);
        }
        */

        protected void btHelp_Click(object sender, EventArgs e)
        {
            if (cntHelp.Visible)
            {
                cntHelp.Show(false, true);
            }
            else
            {
                //btHelp.Visible = false;
                //btHelpK.Visible = false;
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
        //--------------
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
            App.Redirect(App.DefaultForm);
        }

        protected void lbtUserCC_Click(object sender, EventArgs e)
        {
            string id = ((LinkButton)sender).CommandArgument;
            //string login = db.getScalar("select Login from Pracownicy where Id = " + id);
            string login = db.getScalar(String.Format("select Login from Pracownicy where KadryId = '{0}'", id));
            
            
            
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

    
        //-------------------------
        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            string sel = ddl.SelectedValue;
            switch (sel)
            {
                case "0":
                case "-1":
                    hidLoginTyp.Value = sel;
                    ((DropDownList)sender).DataBind();
                    break;
                default:
                    cntWnioskiMenu.ResetSelectMenu();
                    cntWnioskiMenuAdmin.ResetSelectMenu();  //albo przy Session odwołać sie przez stałe 

                    string[] p = Tools.GetLineParams(sel);
                    App.User.LoginAsUserId2(p[0]);
                    App.Redirect(App.DefaultForm);
                    break;
            }
            /*                      
            //App.User._LoginAsUserId(ddlLogin.SelectedValue);

            string[] p = Tools.GetLineParams(ddlLogin.SelectedValue);
            App.User.LoginAsUserId2(p[0]);

            //App.User.CheckPassLoginTest(App.User.NR_EW);

            //lbUser.Text = Tools.PrepareName(App.User.ImieNazwisko);

            //App.KwitekKadryId = App.User.NR_EW;
            //App.KwitekPracId = App.User.Id;
            //AppUser.x_IdKarty = App.User.NR_EW;

            //App.Redirect(App.PortalStartForm);
            //App.Redirect("IPO/IPO.aspx");

            App.Redirect(App.DefaultForm);
            */
            // żeby wnioski działały poprawnie
            //ViewState["tabsupd"] = null;

            //Session["mnScWn2"] = null;    //T: musi być wywołane przed Redirect powyżej ...
            //Session["mnScWn"] = null;
        }

        protected void ddlLogin_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem item in ((DropDownList)sender).Items)
            {
                string id, css;
                Tools.GetLineParams(item.Value, out id, out css);
                if (!String.IsNullOrEmpty(css))
                    item.Attributes["class"] = css;
            }
            //Tools.SelectItem(ddlLogin, App.User.Id);  // zostaje zmien usera
        }


    }
}

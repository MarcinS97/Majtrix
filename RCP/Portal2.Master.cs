using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using HRRcp.App_Code;
using HRRcp.Portal;
using HRRcp.Controls.Portal;
using System.Web.UI.HtmlControls;

/*
 
Master.Page_Init -> Page.Page_Init -> Page.Page_Load -> Master.Page_Load
 
*/
/*
 
 do logowania powinny byc 2 skrypty - dla admina z #kartaRCP i z kiosku bez !!!
 
 */

namespace HRRcp
{
    //public partial class MasterPage : System.Web.UI.MasterPage
    public partial class PortalMasterPage2 : RcpMasterPage
    {
        private int mnIndex = 0;

        const string mnSTART    = "START";
        const string mnPAPRAC   = "PAPRAC";
        const string mnPAKIER   = "PAKIER";
        const string mnADMIN    = "ADMIN";
        const string mnLOGOUT   = "LOGOUT";
        string[] menuId = { mnSTART, mnPAPRAC, mnPAKIER, mnADMIN, mnLOGOUT };

        protected void Page_Init(object sender, EventArgs e)
        {
            Uri r = HttpContext.Current.Request.UrlReferrer;
            string host = HttpContext.Current.Request.UserHostName;
            string ip = HttpContext.Current.Request.UserHostAddress;
            //string Referrer = (r != null ? r.ToString() + " " : "") + (char)13 + host + " (" + ip + ")";
            string Referrer = (r != null ? r.ToString() + " " + (char)13 : "") + (host == ip ? host : host + " (" + ip + ")");

            if (!IsPostBack)
            {
                Tools.InitSessionExpired();

                if (!(Parent is Kiosk.Login))         //Logout nie ma mastera, mogłoby być bez !IsPostBack, w opji Login musi przetrwać
                    App._PrzypisywanieRCP = false;

#if SPX
                imgLogo.ImageUrl = "~/images/spx/spx_logo.png";
#elif ZELMER
                imgLogo.ImageUrl = "~/styles/zelmer/logo.png";
#elif KDR
                imgLogo.ImageUrl = "~/images/kdr/kdr_logo.png";
                btnEditMode.Visible = App.User.IsPortalAdmin;
#endif


                if (user.HasAccess && user.HasRight(AppUser.rPortalTmp) || user.IsPortalAdmin)         // wpuszczam tylko z listy użytkowników, którzy mają login
                {
                    //----- zabezpieczenie przed zastępstwem -----
                    if (Parent is KwitekForm)
                        if (!user.IsOriginalUser)
                        {
                            user.DoPassLogout();
                            App.LoginAsUser(null);  // tu przekieruje na Default
                        }
                    
                    
                    //----- DOSTĘP bez logowania ------                    
                    if (Parent is Kiosk.Login ||
                        
                        
                        
                        Parent is PortalPrac || 
                        Parent is PortalKier || 
                        Parent is PracLoginForm || 
                        Parent is Aktualnosci || 
                        Parent is Kontakty || 
                        Parent is Faq || 
                        Parent is ArticleEdit ||
                        Parent is Search ||
                        Parent is PDFViewer ||

                        Parent is IPO.IPO ||
                        Parent is IPO.IPOAdmin

                        || Parent.Parent is UbezpieczeniaForm   // nested MasterPage
                        || Parent.Parent is ContentForm
                        || Parent.Parent is Portal.Ubezpieczenia.Formularz // nested MasterPage

                        || Parent is OgloszeniaForm                        

                        )  // karuzele bez logowania i formatka logowania
                    {
                    }



                        // testy bez logowania
                    else if (Parent is Pliki || Parent is PlikiKier || Parent is Newsletter || Parent is Aktualnosci || Parent is PortalAdmin)
                    {
                    }





                    //----- II poziom zabezpieczeń ------
                    else if (Parent is KwitekForm)
                    {
                        if (NeedPassLogin(user, 2))
                            PracLoginForm.Show();
                    }
                    //----- I poziom zabezpieczeń ------
                    else //if (Parent is ) <<< dla wszystkich pozostałych stron
                    {
                        if (NeedPassLogin(user, 1))
                            PracLoginForm.Show();              
          
                        else if (!(Parent is PracPassChangeForm) && user.NeedPassChange())
                            App.Redirect(App.PracPassChangeForm);   // wymuszenie logowania
                    }

                    //App.SetApp(App._Kwitek);
                    App.SetApp(App.Portal);
                }
                else
                {
                    //--- Log, lub info do admina o konieczności weryfikacji/dodania loginu do usera

                    //>>>> tymczas .... do ogarniecia logowanie !!!
                    string login = AppUser.GetLogin();   // user.Login ma to samo ... :/
                    string userName = user.ImieNazwiskoOrLogin;
                    if (!String.IsNullOrEmpty(login))  //originalid jest zmienione !!! :/
                    {
                        string rights = db.getScalar(String.Format("select Rights from Pracownicy where Login = '{0}'", login));
                        if (AppUser.HasRight(rights, AppUser.rPortalAdmin))
                            LogoutAdm();
                    }

                    /*
                    string login = AppUser.GetLogin();   
                    if (user.Login != login && !String.IsNullOrEmpty(login))  //originalid jest zmienione !!! :/
                    {
                        string rights = db.getScalar(String.Format("select Rights from Pracownicy where Login = '{0}'", login));
                        if (AppUser.HasRight(rights, AppUser.rPortalAdmin))
                            LogoutAdm();
                    }
                     */
                    App.ShowNoAccess2(Tools.GetAppName(), userName);
                }
            }
        }

        private bool NeedPassLogin(AppUser user, int secLevel)   // 1,2
        {
            //return false;
#if AAA
            if (false) { }
#else
            if (user.IsKiosk) return !user.IsKioskLogged;
#endif
            else
            {
                int lt = user.LoginType;
                switch (lt)
                {
                    default:
                    case AppUser.ltPass:
                        return !user.IsPassLogged;
                    case AppUser.ltAuthWinPass:
                        if (secLevel == 2)
                            return !user.IsPassLogged;
                        else
                            return false;
                    case AppUser.ltAuthWin:
                        return false;
                }
            }
        }

        private bool xxx_IsPracAuth()
        {
            return false;
        }












        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            if (!IsPostBack)
            {
                bool adm = App.User.IsPortalAdmin;
#if SPX
                cssPortal_SPX.Visible = true;
#elif ZELMER
                css1.Visible = true;
                cssPortal_Zelmer.Visible = true;
#elif OKT || APATOR
                imgLogo.ImageUrl = App.StylePath + "logo.png";
                cssPortal.Href = App.StylePath + "custom.css";
                cssPortal.Visible = true;
#elif KDR
                cssPortal_KDR.Visible = true;
#else
                cssPortal_iQor.Visible = true;
#endif           
                jsLoginTest.Visible = adm;        
                        
                //-----------------------------
                //----- widoczne elementy -----
                //-----------------------------
                if (App.User.IsKiosk)
                {
#if KDR
                    cssPortal_KDR_Kiosk.Visible = true;
#else
                    cssPortal_Kiosk.Visible = true;
#endif
                    //hidUniqueId.Value = Tools.UniqueId();
                    //tbLogin.ID = "tbLogin" + hidUniqueId.Value;
                    //tbPass.ID = "tbPass" + hidUniqueId.Value; 
                }
                //-----------------------------


                if (Parent is Kiosk.Login ||                    
                    Parent is PracLoginForm ||
                    Parent is PracPassChangeForm)  
                {
                    MainMenu.Visible = false;
                    tdLeft.Visible = false;
                    tdRight.Visible = false;
                    paSearch.Visible = false;

                    bool w = Parent is PracPassChangeForm;
                    if (w)
                        if (!String.IsNullOrEmpty(user.Id)
#if AAA
#else
                            && (!user.IsKiosk || user.IsKioskLogged)
#endif
                            )
                            lbUser.Text = Tools.PrepareName(user.ImieNazwisko);
                    lbWelcome.Visible = w;
                    lbUser.Visible = w;

                    lbtLogin.Visible = false;
                    lbtChangePass.Visible = false;
                    
                    ddlLogin.Visible = false;
                }
                else
                {
                    MainMenu.Visible = true;
                    tdLeft.Visible = true;
                    tdRight.Visible = true;
                    paSearch.Visible = true;

                    
                    lbWelcome.Visible = true;
                    lbUser.Visible = true;

                    //lbtLogin.Visible = (user.LoginType == AppUser.ltPass || user.LoginType == AppUser.ltAuthWinPass) && !user.IsPassLogged;
                    lbtLogin.Visible = false;
#if OKT || APATOR
                    lbtChangePass.Visible = false;
#elif SPX
                    lbtChangePass.Visible = false;
#elif KDR
                    lbtChangePass.Visible = false;
#else
                    lbtChangePass.Visible = (user.IsPassLogged || user.LoginType == AppUser.ltAuthWin || user.LoginType == AppUser.ltAuthWinPass) && !user.IsKiosk;
#endif               
                    ddlLogin.Visible = adm;
                    paKioskLogin.Visible = adm;
                    if (adm)
                    {
#if IPO
                        ddlLogin.DataSourceID = SqlDataSource2.ID;
#endif
                        ddlLogin.Attributes["onchange"] = "javascript:return testLoginUser(this);";
                    }





                    bool lout = user.IsPassLogged;



                    if (!String.IsNullOrEmpty(user.Id) 
#if !AAA
                        && (!user.IsKiosk || user.IsKioskLogged)
#endif
                        )
                        lbUser.Text = Tools.PrepareName(user.ImieNazwisko);

                    //if (user.IsPortalAdmin)
                    if (adm)
                    {
                        Tools.RemoveMenu(MainMenu, mnLOGOUT);
                        lbtLogout.Visible = lout;

                        string login = user.Login.ToLower();
                        lbtTest.Visible = login == "wojciowt" || login == "tomekw";

                        lbtPrzypisz.Visible = true;
                    }
                    else
                    {
                        Tools.RemoveMenu(MainMenu, mnADMIN);
                        if (!lout) Tools.RemoveMenu(MainMenu, mnLOGOUT);
                    }

                    if (!user.IsKierownik 
#if !AAA
                        || user.IsKiosk
#endif
                        )
                    {
                        Tools.RemoveMenu(MainMenu, mnPAKIER);
                    }



                    //----- Zaznaczenie - Top Menu -----
                    string s1 = "x";
                    /*
                    if (Parent is StartForm)
                    {
                        string menu = Tools.GetStr(Request.QueryString["m"], mnPAPRAC);
                        bool kier = menu == mnPAKIER;
                        s1 = SetMenu(menu, !kier, kier);
                    }
                    else 
                    */    
                    if (Parent is StartForm)
                    {
                        s1 = SetMenu("START", true, false);
                    }
                    else if (Parent is PortalPrac ||
                            Parent is Portal.WnioskiUrlopoweForm ||
                            Parent is Portal.UrlopForm ||
                            Parent is Portal.ZwolnieniaForm ||
                            Parent is PlanUrlopow ||
                            Parent is PortalDane ||
                            Parent is Portal.Pliki ||
                            Parent is Portal.Aktualnosci ||
                            Parent is Portal.Newsletter ||
                            Parent is Portal.Kontakty ||
                            Parent is Portal.Faq ||
                            Parent is Portal.ArticleEdit ||
                            Parent is Portal.Search ||

                            Parent is Portal.KwitekDemo
                            || Parent.Parent is UbezpieczeniaForm // nested MasterPage
                            || Parent.Parent is ContentForm // nested MasterPage
                            || Parent.Parent is Portal.Ubezpieczenia.Formularz // nested MasterPage

                            || Parent is OgloszeniaForm
                            )
                    {
                        s1 = SetMenu(mnPAPRAC, true, false);
                    }
                    else if (Parent is PortalKier ||
                            Parent is PortalPracownicy ||
                            Parent is Portal.PlanUrlopowKier ||
                            Parent is Portal.WnioskiUrlopoweKierForm ||
                            Parent is Portal.WnioskiUrlopoweAkceptacja ||
                            Parent is Portal.PlanUrlopowKier ||
                            Parent is Portal.PlikiKier
                            )
                    {
                        if (!App.User.IsKierownik && !adm)
                            App.Redirect(App.PortalPracForm);
                        s1 = SetMenu(mnPAKIER, false, true);
                    }
                    else if (Parent is PortalAdmin ||
                            Parent is WnioskiUrlopoweWpisz ||
                            Parent is Kiosk.PrzypiszRCP)
                    {
                        s1 = SetMenu(mnADMIN, false, false);
                    }
                    Tools._SelectMenu(MainMenu, s1, "mainMenu");


                    /*
                    tdRight.Visible = Parent is StartForm ||
                                      Parent is PortalPrac ||
                                      Parent is PortalKier ||
                                      Parent is Portal.PortalAdmin ||
                                      Parent is Portal.WnioskiUrlopoweForm ||
                                      Parent is Portal.UrlopForm ||
                                      Parent is Portal.ZwolnieniaForm ||
                                      Parent is PlanUrlopow ||
                                      Parent is Portal.PlanUrlopowKier ||
                                      Parent is Portal.WnioskiUrlopoweKierForm ||
                                      Parent is Portal.Pliki ||
                                      Parent is Portal.PlikiKier ||
                                      Parent is Portal.Newsletter ||
                                      Parent is Portal.Aktualnosci
                                      ;
                    */
                    
                    
                    
                    
                    
                    //tdRight.Visible = !(Parent is Portal.PortalAdmin);





                    //----- Zaznaczenie - Lewe Menu -----
                    if (Parent is StartForm ||
                        Parent is PortalPrac ||
                        Parent is PortalKier ||
                        Parent is Search)
                    {}//LeftMenu.Unselect();
                    else
                    {
                        //string page = Request.Url.AbsolutePath;    //string x = Path.GetFileName(Request.Url.AbsolutePath);
                        //string page = Request.Path;
                        //string page = Request.AppRelativeCurrentExecutionFilePath;


                        //string page = "~" + Request.RawUrl;
                        
                        /*
                        string page = Request.RawUrl;
                        LeftMenu.SelectCommand = page;
                        */
                        // na serwerze
                        //cmd: >>>url>>>~/Portal/Pliki.aspx?p=3
                        //RawUrl: /portal2/portal/pliki.aspx?p=2


                        //Log.Info(Log.t2APP, "Request.Url.AbsolutePath", String.Format("a:{0} b:{1} c:{2} d:{3}", page, Request.Url.AbsolutePath, Request.Url.LocalPath, Request.AppRelativeCurrentExecutionFilePath));
                    }
                }
                Info.SetHelp(Info.PANELPRAC);
            }
        }



            /*                
                
                AppUser user = AppUser.CreateOrGetSession();

                //----- logowanie -----
                if (user.IsPassLogged ||






                    user.IsAdmin ||  //testy bez logowania






                    Parent is AdminKwitekForm && App.User.IsAdmin && App.User.HasRight(AppUser.rKwitekAdm))
                {
                    if (!String.IsNullOrEmpty(user.Id))
                        lbUser.Text = Tools.PrepareName(user.ImieNazwisko);




                    if (user.IsAdmin)   // testy bez logowania
                    {
                        App.KwitekKadryId = App.User.NR_EW;
                        App.KwitekPracId = App.User.Id;
                    }
                    


                    











                    string s1 = "x";
                    if (Parent is StartForm)
                    {
                        s1 = SetMenu("START", true, false);
                    }
                    else if (Parent is PortalPrac ||
                             Parent is PortalDane)
                    {
                        s1 = SetMenu("PAPRAC", true, false);
                    }
                    else if (Parent is PortalKier ||
                             Parent is PortalPracownicy)
                    {
                        s1 = SetMenu("PAKIER", false, true);
                    }
                    else if (Parent is PortalAdmin)
                    {
                        s1 = SetMenu("ADMIN", false, false);
                    }
                    Tools._SelectMenu(MainMenu, s1, "mainMenu");



                    if (Parent is StartForm)
                        LeftMenu.Unselect();
                    else
                    {
                        string page = Request.Url.AbsolutePath;    //string x = Path.GetFileName(Request.Url.AbsolutePath);
                        LeftMenu.SelectCommand = page;
                        




                        Log.Info(Log.t2APP, "Request.Url.AbsolutePath", page);
                    }
                }
                else
                {
                    if (!(Parent is PracLoginForm))
                        App.Redirect(App.PracLoginForm);
                    MainMenu.Visible = false;
                    lbChangePass.Visible = false;
                }


                Info.SetHelp(Info.PANELPRAC);
            }
        }
        */



        private string SetMenu(string top, bool prac, bool kier)
        {
            //cntLeftMenuP.Visible = prac;
            //cntLeftMenuK.Visible = kier;

            cntLeftMenu.Visible = prac || kier;
            string gr;
            if(prac) gr = "PRAC";
            else if(kier) gr = "KIER";
            else gr = "";
            cntLeftMenu.Grupa = gr;


            cntAplikacjeMenuP.Visible = prac;
            cntAplikacjeMenuK.Visible = kier;

            paRightFiller.Visible = !prac && !kier;

            //cntAplikacjeMenuP.Visible = false;
            //cntAplikacjeMenuK.Visible = false;





            return top;
        }

        private cntSqlMenu3 LeftMenu
        {
            get 
            {
                return cntLeftMenu;
                //if (cntLeftMenuK.Visible)
                //    return cntLeftMenuK;
                //return cntLeftMenuP;        
            }
        }

        //-------------------------------------------------------
        public void LoadMenu(int typ, Menu menu, string grupa, bool clear, int selIdx)
        {
            const string itemTabs = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{1}</div></div></div>";
            const string itemMain = "<div class=\"mmCaption\" id=\"mmCaption{0}\"><div class=\"mmText\">{1}</div></div>";

            DataSet ds = db.getDataSet(String.Format("select * from {0}..SqlContent where Grupa = '{1}' order by Kolejnosc, MenuText", App.dbPORTAL, grupa));
            if (clear)
                menu.Items.Clear();
            string item;
            switch (typ)
            {
                case 1:
                    item = itemMain;
                    break;
                default:
                case 2:
                    item = itemTabs;
                    break;
            }
            int idx = 0;
            foreach (DataRow dr in db.getRows(ds))
            {
                idx++;
                menu.Items.Add(new MenuItem(String.Format(item, idx, db.getValue(dr, "MenuText")), db.getValue(dr, "Id")));
            }
            if (selIdx != -1 && selIdx < idx)
                menu.Items[selIdx].Selected = true;
        }
        //-------------------------------------------------------
        protected override void OnPreRender(EventArgs e)  // tu widzimy juz ustawione zmienne i wykonuje się to tylko w momencie pokazywania kontrolki !!!
        {
            if (!IsPostBack)
            {
                /*
                if (Parent is PortalStart)
                    cntLeftMenu.Select(null);
                */
                string app = App.User.IsPortalAdmin ? "Portal Pracownika / Kierownika" : App.User.IsKierownik && !App.User.IsKiosk ? "Portal Kierownika" : "Portal Pracownika";
                lbProgram.Text = app;
                FooterControl1.AppName = app;
                FooterControl1.AppVer = Tools.GetAppVersion();
                FooterControl1.RegulaminButton.Visible = false;
            }
            base.OnPreRender(e);
        }
        
        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Portal");
        }        
        //------------------------------
        protected string GetMenuIndex()
        {
            mnIndex++;
            int ret = 5 - MainMenu.Items.Count + mnIndex;
            return ret.ToString();
        }        
        //------------------------------
        private void Logout()
        {
            AppUser.x_IdKarty = null;
            App._PrzypisywanieRCP = false;
            
            Log.Info(Log.PRACLOGIN, App.APP + "Wylogowanie pracownika", String.Format("ID: {0}, {1}", App.User.Id, App.User.NazwiskoImie), Log.OK);
            App.User.DoPassLogout();
            App.User.Reload(true);

            App.Redirect(App.DefaultForm);
        }

        public static bool CheckLogout2(string cmd)
        {
            string c = cmd.ToLower();
            if (c.Contains("http://") || c.Contains("https://"))   // plomba
            {
                AppUser.x_IdKarty = null;
                App._PrzypisywanieRCP = false;

                Log.Info(Log.PRACLOGIN, App.APP + "Wylogowanie pracownika 2", String.Format("ID: {0}, {1}", App.User.Id, App.User.NazwiskoImie), Log.OK);
                App.User.DoPassLogout();
                App.User.Reload(true);
                return true;
            }
            return false;
        }

        private void LogoutAdm()
        {
            App._PrzypisywanieRCP = false;

            App.User.DoPassLogout();
            App.User.Reload(true);
        }

        public override void Redirect(string cmd)
        {
            if (String.IsNullOrEmpty(cmd))
                cmd = MainMenu.Items[0].Value;
            string c1, c2;
            Tools.GetLineParams(cmd, out c1, out c2);
            switch (c1)
            {
                case mnSTART:
                    App.Redirect(App.PortalStartForm);
                    break;
                case mnPAPRAC:
                    App.Redirect(App.PortalPracForm);
                    break;
                case mnPAKIER:
                    App.Redirect(App.PortalKierForm);
                    break;
                case mnADMIN:
                    App.Redirect(App.PortalAdminForm);
                    break;
                case mnLOGOUT:
                    Logout();                    
                    break;
                default:
                    if (!String.IsNullOrEmpty(c2))
                        Response.Redirect(c2);
                    break;

            }
        }

        protected void MainMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Redirect(MainMenu.SelectedValue);
        }

        protected void btHelp_Click(object sender, EventArgs e)
        {
            if (cntHelp.Visible)
                cntHelp.Show(false, true);
            else
                cntHelp.Show(true, false);
        }

        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }

        protected void lbtLogin_Click(object sender, EventArgs e)
        {
            PracLoginForm.Show();
        }

        protected void lbtLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        //---------------------------------
        protected void cntLeftMenu_SelectedChanged(string id, string cmd, string par1)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Session["lmenugrp"] = String.IsNullOrEmpty(par1) ? null : par1;

                string url;
                if (Tools.IsUrl(cmd, out url))
                {
                    CheckLogout2(cmd);
                    App.Redirect(url);
                }
                
                /*
                if (cmd.ToLower().StartsWith("url:"))
                {
                    string url = cmd.Substring(4);
                    App.Redirect(url);
                }
                 */ 
            }
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

        protected void ddlLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] p = Tools.GetLineParams(ddlLogin.SelectedValue);

            //App.User._LoginAsUserId(ddlLogin.SelectedValue);
            App.User._LoginAsUserId(p[0]);
            App.User.CheckPassLoginTest(App.User.NR_EW);

            lbUser.Text = Tools.PrepareName(App.User.ImieNazwisko);

            App.KwitekKadryId = App.User.NR_EW;
            App.KwitekPracId = App.User.Id;

            AppUser.x_IdKarty = App.User.NR_EW;

            //App.Redirect(App.PortalStartForm);
            App.Redirect(App.DefaultForm);
        }

        protected void ddlLogin_DataBound(object sender, EventArgs e)
        {
            Tools.SelectItem(ddlLogin, App.User.Id);
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            string search = Tools.HtmlEncode(Tools.RemoveHtmlTags(Tools.Substring(tbSearch.Text.Trim(), 0, tbSearch.MaxLength)));

            string menu = MainMenu.SelectedValue;
            Response.Redirect(String.Format("~/Portal/Search.aspx?m={0}&s={1}", menu, search));
        }

        public Image LogoImage
        {
            get { return imgLogo; }
        }

        public HtmlGenericControl paLeft
        {
            get { return LeftPanel; }
        }

        public HtmlGenericControl paRight
        {
            get { return RightPanel; }
        }

        public HtmlTableCell ttdLeft
        {
            get { return tdLeft; }
        }

        public HtmlTableCell ttdContent
        {
            get { return tdContent; }
        }

        public HtmlTable ttbPortal
        {
            get { return tbPortal; }
        }

        protected void lbtLogo_Click(object sender, EventArgs e)
        {
            Redirect(mnPAPRAC);
        }

        /* ----------------------------------------------------------- */

        protected void ToggleEditMode(object sender, EventArgs e)
        {
            EditMode = !EditMode;
            cntAplikacjeMenuK.EditMode = EditMode;
            cntAplikacjeMenuP.EditMode = EditMode;
            cntAplikacjeMenuK.Prepare();
            cntAplikacjeMenuP.Prepare();
            string text = EditMode ? "Anuluj edycję" : "Edytuj";
            btnEditMode.Text = text;
        }

        public Boolean EditMode
        {
            get { return Tools.GetViewStateBool(ViewState["vEditMode"], false); }
            set { ViewState["vEditMode"] = value; }
        }

    }
}

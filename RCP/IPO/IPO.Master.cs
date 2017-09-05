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
using HRRcp.IPO.App_Code;

/*
 
Master.Page_Init -> Page.Page_Init -> Page.Page_Load -> Master.Page_Load
 
*/
/*
 
 do logowania powinny byc 2 skrypty - dla admina z #kartaRCP i z kiosku bez !!!
 
 */

namespace HRRcp
{
    //public partial class MasterPage : System.Web.UI.MasterPage
    public partial class IPOMasterPage : RcpMasterPage
    {
        private int mnIndex = 0;

        const string mnIPO       = "IPO";
        const string mnBAZAPROD  = "BAZAPROD";
        const string mnADMINIPO  = "ADMINIPO";
        const string mnLOGOUT    = "LOGOUT";
        string[] menuId = { mnIPO, mnBAZAPROD, mnADMINIPO, mnLOGOUT };

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
                        Parent is Search)  // karuzele bez logowania i formatka logowania
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
                        string rights = IPO_db.getScalar(String.Format("select Rights from Pracownicy where Login = '{0}'", login));
                        if (AppUser.HasRight(rights, AppUser.rPortalAdmin))
                            LogoutAdm();
                    }

                    /*
                    string login = AppUser.GetLogin();   
                    if (user.Login != login && !String.IsNullOrEmpty(login))  //originalid jest zmienione !!! :/
                    {
                        string rights = IPO_db.getScalar(String.Format("select Rights from Pracownicy where Login = '{0}'", login));
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
            if (user.IsKiosk) return !user.IsKioskLogged;
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
                bool adm = false;
                //if("1".Equals(IPO_db.getScalar("select dbo.GetRightId(Rights, 32) from Pracownicy where Id = " + user.Id)))

                if (App.User.HasRight(AppUser.rIPOGlobalAdmin))
                    adm = true;
 
                

                cssPortal_iQor.Visible = true;           
                jsLoginTest.Visible = adm;        
                        
                //-----------------------------
                //----- widoczne elementy -----
                //-----------------------------
                if (App.User.IsKiosk)
                {
                    cssPortal_Kiosk.Visible = true;

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

                    lbWelcome.Visible = false;
                    lbUser.Visible = false;
                    lbtLogin.Visible = false;
   
                    
                    ddlLogin.Visible = false;
                }
                else
                {
                    MainMenu.Visible = true;
                    tdLeft.Visible = false;
                    tdRight.Visible = false;
                    paSearch.Visible = false; // ukryte w iPO

                    
                    lbWelcome.Visible = true;
                    lbUser.Visible = true;

                    //lbtLogin.Visible = (user.LoginType == AppUser.ltPass || user.LoginType == AppUser.ltAuthWinPass) && !user.IsPassLogged;
                    lbtLogin.Visible = false;
//#if OKT || APATOR 20160725 DP
#if APATOR
                    lbtChangePass.Visible = false;
#elif SPX
                    //lbtChangePass.Visible = false;
#else
#endif               
                    ddlLogin.Visible = adm;
                    paKioskLogin.Visible = adm;
                    //if (adm) ddlLogin.Attributes["onchange"] = "javascript:return testLoginUser(this);";






                    bool lout = user.IsPassLogged;



                    if (!String.IsNullOrEmpty(user.Id) && (!user.IsKiosk || user.IsKioskLogged))
                        lbUser.Text = Tools.PrepareName(user.ImieNazwisko);

                    if (user.IsPortalAdmin)
                    {
                        Tools.RemoveMenu(MainMenu, mnLOGOUT);
                        lbtLogout.Visible = lout;

                        string login = user.Login.ToLower();
                        lbtTest.Visible = login == "wojciowt" || login == "tomekw";

                        lbtPrzypisz.Visible = true;
                    }
                    else
                    {
                        if (!lout) Tools.RemoveMenu(MainMenu, mnLOGOUT);
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
      

  

                    //----- Zaznaczenie - Lewe Menu -----
                    if (Parent is StartForm ||
                        Parent is PortalPrac ||
                        Parent is PortalKier ||
                        Parent is Search)
                        LeftMenu.Unselect();
                    else
                    {
                        //string page = Request.Url.AbsolutePath;    //string x = Path.GetFileName(Request.Url.AbsolutePath);
                        //string page = Request.Path;
                        //string page = Request.AppRelativeCurrentExecutionFilePath;


                        //string page = "~" + Request.RawUrl;
                        string page = Request.RawUrl;
                        LeftMenu.SelectCommand = page;

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
            cntLeftMenuP.Visible = prac;
            cntLeftMenuK.Visible = kier;

            cntAplikacjeMenuP.Visible = prac;
            cntAplikacjeMenuK.Visible = kier;

            paRightFiller.Visible = !prac && !kier;

            //cntAplikacjeMenuP.Visible = false;
            //cntAplikacjeMenuK.Visible = false;





            return top;
        }

        private cntSqlMenu LeftMenu
        {
            get 
            {
                if (cntLeftMenuK.Visible)
                    return cntLeftMenuK;
                return cntLeftMenuP;        
            }
        }

        //-------------------------------------------------------
        public void LoadMenu(int typ, Menu menu, string grupa, bool clear, int selIdx)
        {
            const string itemTabs = "<div class=\"tabCaption\"><div class=\"tabLeft\"><div class=\"tabRight\">{1}</div></div></div>";
            const string itemMain = "<div class=\"mmCaption\" id=\"mmCaption{0}\"><div class=\"mmText\">{1}</div></div>";

            DataSet ds = IPO_db.getDataSet(String.Format("select * from {0}..SqlContent where Grupa = '{1}' order by Kolejnosc, MenuText", App.dbPORTAL, grupa));
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
            foreach (DataRow dr in IPO_db.getRows(ds))
            {
                idx++;
                menu.Items.Add(new MenuItem(String.Format(item, idx, IPO_db.getValue(dr, "MenuText")), IPO_db.getValue(dr, "Id")));
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
                string app = "IPO - System Zamówień Wewnetrznych";
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
            App.Redirect("IPO/AdministracjaIPO.aspx");
        }

        private void LogoutAdm()
        {
            App._PrzypisywanieRCP = false;

            App.User.DoPassLogout();
            App.User.Reload(true);
            App.Redirect("IPO/AdministracjaIPO.aspx");
        }

        public override void Redirect(string cmd)
        {
            if (String.IsNullOrEmpty(cmd))
                cmd = MainMenu.Items[0].Value;
            string c1, c2;
            Tools.GetLineParams(cmd, out c1, out c2);
            switch (c1)
            {
                case mnIPO:
                    App.Redirect("IPO/IPO.aspx");
                    break;
                case mnBAZAPROD:
                    App.Redirect("IPO/BazaProduktow.aspx");
                    break;
                case mnADMINIPO:
                    App.Redirect("IPO/AdministracjaIPO.aspx");
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
                    App.Redirect(url);
                
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
            App.Redirect("IPO/IPO.aspx");
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



    }
}

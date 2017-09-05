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
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using HRRcp.Areas.MVC.Controllers;

/*

Master.Page_Init -> Page.Page_Init -> Page.Page_Load -> Master.Page_Load
 
*/
/*
 
do logowania powinny byc 2 skrypty - dla admina z #kartaRCP i z kiosku bez !!!
 
*/

namespace HRRcp
{
    public class WebFormController : Controller { }

    public partial class PortalMasterPage3 : RcpMasterPage
    {
        
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

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!(Parent is ErrorForm))
            {
                Page.Init += new EventHandler(OnPageInit);
                Page.Error += new EventHandler(OnPageError);
            }
            //-----
            //Uri r = HttpContext.Current.Request.UrlReferrer;
            string host = HttpContext.Current.Request.UserHostName;
            string ip = HttpContext.Current.Request.UserHostAddress;
            //string Referrer = (r != null ? r.ToString() + " " : "") + (char)13 + host + " (" + ip + ")";
            //string Referrer = (r != null ? r.ToString() + " " + (char)13 : "") + (host == ip ? host : host + " (" + ip + ")");

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
                        Parent is HRRcp.Portal.PracLogin ||
                        Parent is Aktualnosci ||
                        Parent is Kontakty ||
                        Parent is Faq ||
                        Parent is ArticleEdit ||
                        Parent is Search ||
                        Parent is PDFViewer ||
                        Parent is Portal.PdfContent ||
                        Parent is Portal.PDFViewer ||
                        Parent is Portal.Ubezpieczenia.Majatkowe.Warunki ||
                        Parent is Portal.Ubezpieczenia.Majatkowe.Warunki2 ||
                        Parent is IPO.IPO ||
                        Parent is IPO.IPOAdmin

                        || Parent.Parent is UbezpieczeniaForm   // nested MasterPage
                        || Parent is Portal.UbezpieczeniaForm
                        || Parent is ContentForm
                        || Parent.Parent is Portal.Ubezpieczenia.Formularz // nested MasterPage

                        || Parent is OgloszeniaForm
                        || Parent is Portal.Ubezpieczenia.Majatkowe.UbezpAdministracja
                        || Parent is Portal.Ubezpieczenia.ListaPlacowek  

                        || Parent is KartyTymczasowe
                        || Parent is PL.Kalkulator
                        || Parent is RaportyForm
                        || Parent is Portal3Report  // wszystko co jest na tym masterze (prościej będzie)
                        
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
                            PracLogin.Show();
                    }
                    //----- I poziom zabezpieczeń ------
                    else //if (Parent is ) <<< dla wszystkich pozostałych stron
                    {

                        /* KOSMOS NIE WYBACZA */
                        if (NeedPassLogin(user, 1))
                            PracLogin.Show();

                        else if (!(Parent is PracPassChangeForm) && user.NeedPassChange())
                            App.Redirect(App.PracPassChangeForm);   // wymuszenie logowania
                    }
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

        public static string RenderPartialToString(string partialName, object model)
        {
            StringWriter txWriter = new StringWriter();
            var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);
            var rt = new RouteData();            
            rt.Values.Add("controller", "WebFormController");
            var ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new WebFormController());

            var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;
            var vctx = new ViewContext(ctx, view,
                new ViewDataDictionary { Model = model },
                new TempDataDictionary(), txWriter);

            view.Render(vctx, txWriter);
            return txWriter.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            if (!IsPostBack)
            {                
                LayoutController lc = new LayoutController();                
                AppBar.Text = RenderPartialToString("~/Areas/MVC/Views/Shared/_RightMenu.cshtml", lc.RightMenu2());
                SideBar.Text = RenderPartialToString("~/Areas/MVC/Views/Shared/_LeftMenu.cshtml", lc.LeftMenu2());
                MainMenu.Text = RenderPartialToString("~/Areas/MVC/Views/Shared/_MainMenu.cshtml", lc.TopMenu2());
#if KWITEK
                lbJabil.Text = "iQor '" + DateTime.Today.Year.ToString();
#else
                //lbJabil.Text = "© KDR Solutions Sp. z o.o. '" + DateTime.Today.Year.ToString();
#endif
                bool adm = App.User.IsPortalAdmin;
                cntSqlMenuEditGrupyChld.Visible = adm;
               // var liAdmin = (HtmlGenericControl)Page.FindControl("liAdmin");//dodane
                liAdmin.Visible = adm;
#if CO || PRON
                css_co.Visible = true;
#elif IQOR
                css_iqor.Visible = true;
#elif SPX
                css_spx.Visible = true;
#endif


                //jsLoginTest.Visible = adm;

                //-----------------------------
                //----- widoczne elementy -----
                //-----------------------------
                if (App.User.IsKiosk)
                {
                    Tools.ExecuteJavascript("prepareKiosk();");

#if KDR
                    /*cssPortal_KDR_Kiosk.Visible = true;*/
#elif PORTAL

#else
                    //cssPortal_Kiosk.Visible = true;
#endif
                    //hidUniqueId.Value = Tools.UniqueId();
                    //tbLogin.ID = "tbLogin" + hidUniqueId.Value;
                    //tbPass.ID = "tbPass" + hidUniqueId.Value; 
                }
                //-----------------------------


                if (Parent is Kiosk.Login ||
                    Parent is HRRcp.Portal.PracLogin ||
                    Parent is PracPassChangeForm)
                {
                    bool w = Parent is PracPassChangeForm;
                    //ddlLogin.Visible = false;
                    //cntLoginAs.Visible = false;//zakomentowane
                }
                else
                {

                    ////lbtLogin.Visible = (user.LoginType == AppUser.ltPass || user.LoginType == AppUser.ltAuthWinPass) && !user.IsPassLogged;
                    //lbtLogin.Visible = false;
#if (OKT || APATOR) && PORTAL
                    lbtChangePass.Visible = false;
#elif SPX
                    lbtChangePass.Visible = false;
#elif KDR
                    lbtChangePass.Visible = false;
#else
                  //  var lbtChangePass = (HtmlGenericControl)Page.FindControl("lbtChangePass");//dodane
                  //  lbtChangePass.Visible = (user.IsPassLogged || user.LoginType == AppUser.ltAuthWin || user.LoginType == AppUser.ltAuthWinPass) && !user.IsKiosk;
#endif
                    //ddlLogin.Visible = adm;
                   // cntLoginAs.Visible = adm;//zakomentowane
                    //paKioskLogin.Visible = adm;
                    //if (adm)
                    //{
                    //    ddlLogin.Attributes["onchange"] = "javascript:return testLoginUser(this);";
                    //}

                    bool lout = user.IsPassLogged;
                    /*
                    if (adm)
                        lnkLogout_.Visible = lout;
                    */
                    //var liLogout = (HtmlGenericControl)Page.FindControl("liLogout");//dodane
                    liLogout.Visible = lout;



                    if (!user.IsKierownik
#if !AAA // DO SPRAWDZENIA
 || user.IsKiosk
#endif
)
                    {
                        //Tools.RemoveMenu(MainMenu, mnPAKIER);
                    }



                    if (Parent is StartForm)
                    {
                        SetMenu(true, false);
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
                        SetMenu(true, false);
                    }
                    else if (Parent is PortalKier ||
                            Parent is PortalPracownicy ||
                            Parent is Portal.PlanUrlopowKier ||
                            Parent is Portal.WnioskiUrlopoweKierForm ||
                            Parent is Portal.WnioskiUrlopoweAkceptacja ||
                            Parent is Portal.PlanUrlopowKier ||
                            Parent is Portal.PlikiKier ||
                            Parent is Portal.Wnioski.PracaZdalna.Kier ||
                            Parent is Portal.Wnioski.PracaZdalna.Acc
                            )
                    {
                        if (!App.User.IsKierownik && !adm)
                            App.Redirect(App.PortalPracForm);
                        SetMenu(false, true);
                    }
                    else if (Parent is PortalAdmin ||
                            Parent is WnioskiUrlopoweWpisz ||
                            Parent is Kiosk.PrzypiszRCP)
                    {
                        SetMenu(false, false);
                    }


                    //----- Zaznaczenie - Lewe Menu -----
                    if (Parent is StartForm ||
                        Parent is PortalPrac ||
                        Parent is PortalKier ||
                        Parent is Search)
                    { }
                }
                Info.SetHelp(Info.PANELPRAC);

                if (Request.QueryString["b"] == "1")
                    lbtnBack.Visible = true;

                if (Request.QueryString["wn"] == "1")
                    lbtnFillRequest.Visible = true;

                //lnkChat.Visible = cntSocialBar.Visible = Lic.Chat;
                //var liChat = (HtmlGenericControl)Page.FindControl("liChat");//dodane
                //liChat.Visible = cntSocialBar.Visible = Lic.Chat;
            }
        }

        private void SetMenu(bool prac, bool kier)
        {
            string gr;
            if (prac) gr = "PRAC";
            else if (kier) gr = "KIER";
            else gr = "";

            hidGroup.Value = gr;

            //cntSidebar.Set(gr); zakomentowane przez MVC
            //cntAppBar.Set(gr); zakomentowane przez MVC
        }

        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Portal");
        }

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

        protected void lbtEndZastepstwo_Click(object sender, EventArgs e)
        {
            App.LoginAsUser(null);
        }

        protected void lbtLogin_Click(object sender, EventArgs e)
        {
            HRRcp.Portal.PracLogin.Show();
        }

        protected void lbtLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        //public Image LogoImage
        //{
        //    get { return imgLogo; }
        //}

        protected void lbtLogo_Click(object sender, EventArgs e)
        {
            Redirect(App.DefaultForm);
        }

        protected void lnkMainMenuRedirect_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            string split = (lb.CommandArgument);
            string cmd = split.Split(';')[0];
            string id = split.Split(';')[1];

            Session["start"] = "1";


            if (!String.IsNullOrEmpty(cmd))
            {
                string url = "";
                if (Tools.IsUrl(cmd, out url))
                {
                    //App.Redirect(url);
                }
                else if (cmd.Contains("REPORT"))
                {
                    string repId = cmd.Split(':')[1];
                    url = GetUrl(repId);
                }
                bool append = url.Contains("?");
                url += String.Format("{0}mm={1}", (append ? "&" : "?"), id);
                App.Redirect(url);
            }
        }

        string GetUrl(string id)
        {
            string p = Tools.SetLineParams(3, id, null, null, null, null, null);   // jak za krótki string to dołoży #zera na końcu - dlatego pusty parametr, zeby nie trzeba było ich odcinać
            string e = Report.EncryptQueryString(p, Grid.key, Grid.salt);
            return ResolveUrl(String.Format("/Redirect.aspx?p={0}&f={1}", e, "RaportF.aspx"));
        }

        public String GetMainMenuSelectedClass(String par)
        {
            String group = hidGroup.Value;
            if ((par == "START" && Parent is Portal.StartForm)
                || (par == "PRAC" && group == "PRAC")
                || (par == "KIER" && group == "KIER")
                || (par == "ADM" && (Parent is Portal.PortalAdmin || Parent is Portal.Ubezpieczenia.Majatkowe.UbezpAdministracja))
                )
            {
                return " selected";
            }
            return String.Empty;
        }

        protected void dsMainMenu_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(App.User);
            SqlDataSource dsMMI = new SqlDataSource("connectionString:Portal", "select * from SqlMenu where Grupa = 'MAINMENU'");
            DataTable dt = db.Select.Table(dsMMI);
            //DataTable dt = db.Select.Table(dsMainMenuItems);


            string ids = "";

            foreach (DataRow dr in dt.Rows)
            {
                bool hasRights = db.CheckRightsExpr(userRights, db.getValue(dr, "Rights"));
                if (hasRights)
                    ids += db.getValue(dr, "Id") + ",";
            }
            e.Command.Parameters["@ids"].Value = ids;//db.PrepareRightsForCheckRightsExpression(user);
        }

        //public String GetUserName()
        //{
        //    return App.User.NazwiskoImie;
        //}

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Tools.Back();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoSearch();
            //string search = Tools.HtmlEncode(Tools.RemoveHtmlTags(Tools.Substring(tbSearch.Text.Trim(), 0, tbSearch.MaxLength)));
            //Response.Redirect(String.Format("~/Portal/Search.aspx?lm={0}&s={1}&b=1", Request.QueryString["lm"], search));
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }

        //protected void imgUser_Click(object sender, ImageClickEventArgs e)
        //{
        //    cntImageEditModal.Show();
        //}

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        protected void DoSearch()
        {
            TextBox tbSearch = this.Master.FindControl("tbSearch") as TextBox; //dodane
            string search = Tools.HtmlEncode(Tools.RemoveHtmlTags(Tools.Substring(tbSearch.Text.Trim(), 0, tbSearch.MaxLength)));
            Response.Redirect(String.Format("~/Portal/Search.aspx?lm={0}&s={1}&b=1", Request.QueryString["lm"], search));
        }

        protected void lbtnFillRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Portal/Ubezpieczenia/Majatkowe/Formularz.aspx?force=1&b=1");
        }
        
        public Button SearchButton
        {

            get { return (Button)this.FindControl("btnSearch"); } //dodane bylo return btnSearch (Button)this.FindControl("btnSearch");
        }

        public TextBox SearchTextBox
        {            
            get { return(TextBox)this.FindControl("tbSearch"); } //dodane bylo return tbSearch
        }

        protected void lnkAdmin_Click(object sender, EventArgs e)
        {
            cntSqlMenuEditGrupyChld.Show();
        }
    }
}

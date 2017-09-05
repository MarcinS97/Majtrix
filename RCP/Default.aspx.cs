using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.Globalization;
using System.Threading;

using HRRcp.App_Code;

namespace HRRcp
{
    public partial class _Default : System.Web.UI.Page
    {
        private AppUser user;

        protected void Page_Init(object sender, EventArgs e)
        {
            /*
            object s = Session["ADM.Login"];
            if (s != null)
                Tools.ShowMessage(s.ToString());
            */





#if RCP
            //App.Redirect("RCP/Pracownicy.aspx");

            AppUser usr = AppUser.CreateOrGetSession();
            if (usr.IsKomputer) App.Redirect("Login.aspx");
            else
            {
                App.Redirect("Start.aspx");
                App.Redirect("RCP/Harmonogram.aspx");
            }
#endif     
            //App.Redirect("RaportF.aspx");            
            //Scorecards.RaportF.Show("18");
            //RaportF.Show("1");




            
#if MS
            App.Redirect("~/MatrycaSzkolen/Start.aspx");
#elif SCARDS
            //Response.Redirect(App.ScStartForm);   //<<<<<<< do zmiany później ustawić na Start !!!!

            AppUser user = AppUser.CreateOrGetSession();
            if (user.IsScAdmin)
                App.Redirect(App.ScAdminForm);
            else if (user.IsScTL || user.IsScKier)
                App.Redirect(App.ScScorecardsForm);
            else if (user.IsScZarz)
                App.Redirect(App.ScWnioskiForm);
            else if (user._IsScHR)
                App.Redirect(App.ScRaportyForm);
            else if (user.IsScControlling)
                App.Redirect(App.ScRaportyForm);
            else App.ShowNoAccess(Tools.GetAppName(), user);
#endif
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            user = AppUser.CreateOrGetSession();

            //Response.Redirect("test3.aspx");
            //PlanUrlopow.Show("123", "2013-01-01");
            //PlanUrlopow.Show("873", "2013-01-01");      // Zalewski u Krzysiu Bielicki
            //PlanUrlopow.Show("737", "2013-01-01");      // zwolniony

#if KWITEK
            Response.Redirect(App.KwitekForm);   // odermować w wersji kwitka <<< potestować jeszcze !!!
#endif
#if PORTAL
            if (user.HasAccess)
            {
                //Response.Redirect(App.PortalAktualnosciForm);
    #if APATOR
                Response.Redirect(App.PortalPracForm);
    #else
                if (user.IsAdmin || user.IsKiosk)
                {
                    if (App._PrzypisywanieRCP)
                        App.Redirect(App.PrzypiszRCP);  // parametry ???
                    else
                        App.Redirect(App.PortalPracForm);
                }
                //if (user.IsAdmin) Response.Redirect(App.PortalStartForm);
                else if (user.IsKierownik) App.Redirect(App.PortalKierForm);    // karuzela kierownika
                else App.Redirect(App.PortalPracForm);                          // karuzela pracownika            
    #endif
            }
            App.ShowNoAccess(Tools.GetAppName(), user);
#endif

            /*
            Default nie korzysta z Master        
            if (App.User.IsKierownik)
                App.Redirect(App.PortalKierForm);  
            else
                App.Redirect(App.PortalPracForm);  
             */
            /*
            AppUser user = AppUser.CreateOrGetSession();
            user.CheckPassLogin("", "", true);
            Response.Redirect(App.WnioskiUrlopoweForm);
            */




            //Response.Redirect("test.aspx");
            /*
            int c, l, m;
            Tools.DecimalToFraction(1.0, out c, out l, out m);
            Tools.DecimalToFraction(0.1, out c, out l, out m);      //1/10
            Tools.DecimalToFraction(0.875, out c, out l, out m);    //7/8
            Tools.DecimalToFraction(0.75, out c, out l, out m);     //6/8
            Tools.DecimalToFraction(0.625, out c, out l, out m);    //5/8
            Tools.DecimalToFraction(0.5, out c, out l, out m);      //1/2    
            Tools.DecimalToFraction(0.25, out c, out l, out m);     //1/4
            Tools.DecimalToFraction(0.125, out c, out l, out m);    //1/8
            */

            /*
            CultureInfo ci = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
            */
            /*
            double d = 2.4;
            string s = String.Format("{0}", d);
            */

            //----- TESTY ----- 
            //symbol TEST dodać w app propertiesach: Build|Conditional compilation symbols


            #if TEST
            //*----- parametry -----
            /*
            string u = Request.QueryString["user"];
            string t = Request.QueryString["test"];
            if (String.IsNullOrEmpty(u))
            {
                if (//String.IsNullOrEmpty(user.Login) ||
                    user.Login == "tomekw")
                {
                    user.Login = "WojciowT";
                    //user.Login = "tomekw";
                    //user.Login = "WeckwerA";
                }
            }
            else
                user.Login = u;
            /**/


            
            //if (t == "1") Tools.TestMode(true);

            /*
            if (String.IsNullOrEmpty(user.Login))
            {
                string u = Request.QueryString["user"];
                string t = Request.QueryString["test"];
                if (String.IsNullOrEmpty(u))
                    user.Login = "WojciowT";
                else
                    user.Login = u;
                if (t == "1") Tools.TestMode(true);
            }
            if (String.IsNullOrEmpty(user.Login) || user.Login.ToLower() == "tomekw")
            {
                user.Login = "WojciowT";  // !!! wyłaczyc pozniej mozliwosc podstawienia !!!
                Response.Redirect("test.aspx");
            }
            /**/
            //----- TESTY ----- 
            /* zeby zaczynał zawsze od test *
            bool test = Request.UrlReferrer == null ? true : !Request.UrlReferrer.ToString().Contains("test.aspx");
            if (test && user.IsAdmin)
                Response.Redirect("test.aspx");
            /**/
            #endif
            
            //if (String.IsNullOrEmpty(user.Login)) Response.Redirect(App.LoginForm);

            if (user.IsAdmin) Response.Redirect(App._StartForm);
            else if (user.IsKierownik) Response.Redirect(App._StartForm);
            else if (user.HasZastepstwo) Response.Redirect(App._StartForm);
            else App.ShowNoAccess(Tools.GetAppName(), user);
            /*
            if (user.IsAdmin) Response.Redirect(App.AdminForm);
            else if (user.IsKierownik) Response.Redirect(App.KierownikForm);
            else if (user.HasZastepstwo) Response.Redirect(App.KierownikForm);
            else App.ShowNoAccess(Tools.GetAppName(), user);
             * */
        }
        
        protected void Page_Error(object sender, System.EventArgs e)
        {
            AppError.Show("Formatka startowa default.aspx");
        }
    }
}

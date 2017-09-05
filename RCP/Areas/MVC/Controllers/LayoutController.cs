using HRRcp.App_Code;
using HRRcp.Areas.MVC.Models;
using HRRcp.Areas.MVC.Models.customModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace HRRcp.Areas.MVC.Controllers
{
    public class LayoutController : Controller
    {
        ObiegDokumentow ob = new ObiegDokumentow();
        // GET: MVC/Layout
        public ActionResult TopMenu()
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(AppUser.CreateOrGetSession());

            //List<SqlMenu> menus = ob.Database.SqlQuery<SqlMenu>("select * from SqlMenu where Grupa = 'MAINMENU'").ToList();
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string menuTypeNow=null;
            bool menuType = url.Contains("Portal.aspx");
            if (menuType)
            {
                menuTypeNow = "PRAC";
            }
            List<SqlMenu> mainMenusAll = ob.SqlMenu.Where(a => a.Grupa == "MAINMENU").ToList();

            String ids = "";

            foreach (var item in mainMenusAll)
            {
                bool hasrights = db.CheckRightsExpr(userRights, item.Rights);
                if (hasrights)
                    ids += item.Id + ",";
            }

            List<MagicMenu1> menuSel = ob.Database.SqlQuery<MagicMenu1>(String.Format("select * from fn_GetMenu3('MAINMENU','{0}') order by Sort",ids)).ToList();            
            List<Logowanie> loginList = ob.Database.SqlQuery<Logowanie>("select 'zaloguj jako ...' as Pracownik, null as Id, 1 as Sort union all select Nazwisko + ' ' + Imie + ' | " + "' + ISNULL(KadryId, '-') + ' | ' + ISNULL(Login, '-') + ' | ' + ISNULL(NrKarty1, '-') + ' | ' + case when Kierownik = 1 then ' K' else '' end as Pracownik, convert(varchar, Id) + '|' + ISNuLL(NrKarty1 + '|', ISNULL(KadryId, '') + '|kdr123') as Id , 2 as Sort from Pracownicy where Status >= 0 or Admin = 1 or(Status = -2 and Kierownik = 1) order by Sort, Pracownik").ToList();
            

            var selectList =  loginList.Select(s => new SelectListItem
            { Value = s.Id, Text = s.Pracownik });
            //foreach (var item in loginList)
            //{
            //    selectList.Add(new SelectListItem { Text = item.Pracownik, Value = item.Id });
            //}

            //ViewBag.Login = new SelectList(selectList.ToList(), "Value", "Text", null);
            MagicMenu1 sl = new MagicMenu1();
            sl.MenuText = "LoginList";
            sl.LogList = new SelectList(selectList.ToList(), "Value", "Text", null);
            sl.command = menuTypeNow;
            menuSel.Add(sl);

            return PartialView("_MainMenu",menuSel);
        }

        public List<MagicMenu1> TopMenu2()
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(AppUser.CreateOrGetSession());
            //List<SqlMenu> menus = ob.Database.SqlQuery<SqlMenu>("select * from SqlMenu where Grupa = 'MAINMENU'").ToList();
            List<SqlMenu> mainMenusAll = ob.SqlMenu.Where(a => a.Grupa == "MAINMENU").ToList();           
            String ids = "";
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            string menuTypeNow = null;
            bool menuType = url.Contains("Portal.aspx");
            if (url.Contains("Portal.aspx"))
            {
                menuTypeNow = "PRAC";
            }else if (url.Contains("StartForm.aspx"))
            {
                menuTypeNow = "STARTFORM";
            }
            else if (url.Contains("PortalKier.aspx"))
            {
                menuTypeNow = "KIER";
            }

            foreach (var item in mainMenusAll)
            {
                bool hasrights = db.CheckRightsExpr(userRights, item.Rights);
                if (hasrights)
                    ids += item.Id + ",";
            }

            List<MagicMenu1> menuSel = ob.Database.SqlQuery<MagicMenu1>(String.Format("select * from fn_GetMenu3('MAINMENU','{0}') order by Sort", ids)).ToList();
            List<Logowanie> loginList = ob.Database.SqlQuery<Logowanie>("select 'zaloguj jako ...' as Pracownik, null as Id, 1 as Sort union all select Nazwisko + ' ' + Imie + ' | " + "' + ISNULL(KadryId, '-') + ' | ' + ISNULL(Login, '-') + ' | ' + ISNULL(NrKarty1, '-') + ' | ' + case when Kierownik = 1 then ' K' else '' end as Pracownik, convert(varchar, Id) + '|' + ISNuLL(NrKarty1 + '|', ISNULL(KadryId, '') + '|kdr123') as Id , 2 as Sort from Pracownicy where Status >= 0 or Admin = 1 or(Status = -2 and Kierownik = 1) order by Sort, Pracownik").ToList();


            var selectList = loginList.Select(s => new SelectListItem
            { Value = s.Id, Text = s.Pracownik });
            //foreach (var item in loginList)
            //{
            //    selectList.Add(new SelectListItem { Text = item.Pracownik, Value = item.Id });
            //}

            // ViewBag.Login = new SelectList(selectList.ToList(), "Value", "Text", null);

            MagicMenu1 sl = new MagicMenu1();
            sl.MenuText = "LoginList";
            sl.LogList = new SelectList(selectList.ToList(), "Value", "Text", null);
            sl.command = menuTypeNow;
            menuSel.Add(sl);

            return  menuSel;
        }

        public ActionResult LeftMenu()
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(AppUser.CreateOrGetSession());


            List<SqlMenu> mainMenusAll = ob.SqlMenu.Where(a => a.Grupa == "LEFTMENUPRAC").ToList();

            String ids = "";
            String group = "PRAC";

            foreach (var item in mainMenusAll)
            {
                bool hasrights = db.CheckRightsExpr(userRights, item.Rights);
                if (hasrights)
                    ids += item.Id + ",";
            }

            List<MagicMenu1> menuSel = ob.Database.SqlQuery<MagicMenu1>(String.Format("select * from fn_GetMenu3('LEFTMENU' + '{0}','{1}') order by Sort",group,ids)).ToList();

            return PartialView("_LeftMenu",menuSel);
        }

        public List<MagicMenu1> LeftMenu2()
        {
            String userRights = db.PrepareRightsForCheckRightsExpression(AppUser.CreateOrGetSession());


            List<SqlMenu> mainMenusAll = ob.SqlMenu.Where(a => a.Grupa == "LEFTMENUPRAC").ToList();

            String ids = "";
            String group = "PRAC";

            foreach (var item in mainMenusAll)
            {
                bool hasrights = db.CheckRightsExpr(userRights, item.Rights);
                if (hasrights)
                    ids += item.Id + ",";
            }

            List<MagicMenu1> menuSel = ob.Database.SqlQuery<MagicMenu1>(String.Format("select * from fn_GetMenu3('LEFTMENU' + '{0}','{1}') order by Sort", group, ids)).ToList();

            return  menuSel;
        }

        public ActionResult RightMenu()
        {                   
                       
            String group = "PRAC";
            List<SqlMenu> menuSel = ob.Database.SqlQuery<SqlMenu>(String.Format("select * from SqlMenu where Grupa = 'APP' + '{0}' and Aktywny = 1  order by Kolejnosc", group)).ToList();

            return PartialView("_RightMenu", menuSel);
        }

        public List<SqlMenu> RightMenu2()
        {
            String group = "PRAC";
            List<SqlMenu> menuSel = ob.Database.SqlQuery<SqlMenu>(String.Format("select * from SqlMenu where Grupa = 'APP' + '{0}' and Aktywny = 1  order by Kolejnosc", group)).ToList();

            return  menuSel;
        }


        public ActionResult LoginAs(string group, string actionName, string controllerName)
        {
            string[] p = Tools.GetLineParams(group);
            App.User._LoginAsUserId(p[0]);
            App.User.CheckPassLoginTest(App.User.NR_EW);
            App.KwitekKadryId = App.User.NR_EW;
            App.KwitekPracId = App.User.Id;
            AppUser.x_IdKarty = App.User.NR_EW;
            string a  = VirtualPathUtility.ToAbsolute(Tools.GetRedirectUrl(App.DefaultForm));
            //App.Redirect("https://www.google.pl/");


            //return RedirectToAction("Index", "obdRoleDicts");
            return Redirect(a);
        }

        public ActionResult DefaultRedirect()
        {
            string a = VirtualPathUtility.ToAbsolute(Tools.GetRedirectUrl(App.DefaultForm));
            //App.Redirect("https://www.google.pl/");
            //return RedirectToAction("Index", "obdRoleDicts");
            return Redirect(a);
        }

        public ActionResult SearchAction(string search)
        {
            string searchs = Tools.HtmlEncode(Tools.RemoveHtmlTags(Tools.Substring(search.Trim(), 0, 50)));
            string redirectUrl = (String.Format("~/Portal/Search.aspx?lm={0}&s={1}&b=1", Request.QueryString["lm"], searchs));
            return Redirect(VirtualPathUtility.ToAbsolute(redirectUrl));
        }

        public ActionResult SearchRedirect(string search,string action,string controller)
        {
            if (!String.IsNullOrEmpty(search))
            {
                string searchs = Tools.HtmlEncode(Tools.RemoveHtmlTags(Tools.Substring(search.Trim(), 0, 50)));
                string redirectUrl = (String.Format("~/Portal/Search.aspx?lm={0}&s={1}&b=1", Request.QueryString["lm"], searchs));
                return Redirect(VirtualPathUtility.ToAbsolute(redirectUrl));
            }
            else
            {
                return RedirectToAction("Index", "obdPolaDicts");
            }            
        }

        public ActionResult Logout()
        {
            AppUser.x_IdKarty = null;
            App._PrzypisywanieRCP = false;
            Log.Info(Log.PRACLOGIN, App.APP + "Wylogowanie pracownika", String.Format("ID: {0}, {1}", App.User.Id, App.User.NazwiskoImie), Log.OK);
            App.User.DoPassLogout();
            App.User.Reload(true);
            string a = VirtualPathUtility.ToAbsolute(Tools.GetRedirectUrl(App.DefaultForm));
            return Redirect(a);
        }
    }
}
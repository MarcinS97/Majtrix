using HRRcp.Areas.ME.Models;
using HRRcp.Areas.ME.Models.CustomModels;
using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRRcp.Areas.ME.Controllers
{
    public class MenuController : Controller
    {
        private IMenuSet menuSet;

        public MenuController(IMenuSet menuSetParam)
        {
            menuSet = menuSetParam;
            menuSet.setMenu();

        }
        // GET: ME/Menu
        public ActionResult getMainMenu()
        {

            return PartialView("_TopMenu", (List<ME_SqlMenu>)menuSet.getMenuSet());
        }
        
    }
}
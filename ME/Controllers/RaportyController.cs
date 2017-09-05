using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRRcp.Areas.ME.Models;

namespace HRRcp.Areas.ME.Controllers
{
    public class RaportyController : Controller
    {
        MatrycaMVC bazaMatryca = new MatrycaMVC();
        // GET: ME/Raporty
        public ActionResult RaportyIndex()
        {
            return View();
        }
        public ActionResult raporty1(int p1, int p2, int p3)
        {
            //var raport1 = bazaMatryca.Database.ExecuteSqlCommand()

            return View();
            
            
        }



    }
}
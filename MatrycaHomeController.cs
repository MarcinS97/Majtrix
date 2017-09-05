using HRRcp.Areas.ME.Models;
using HRRcp.Areas.ME.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRRcp.Areas.App_Code;
using HRRcp.App_Code;
using HRRcp.Areas.ME.Models.CustomModels;

namespace HRRcp.Areas.ME.Controllers
{
    public class MatrycaHomeController : Controller
    {
        private ToDoOddelegowania toDoOddelegowania;
        // GET: ME/Matryca
        public ActionResult Index()
        {
            toDoOddelegowania = new ToDoOddelegowania();
            toDoOddelegowania.setToDo();
            return View();
        }
    }
}
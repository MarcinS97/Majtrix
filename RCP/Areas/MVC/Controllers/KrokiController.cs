using HRRcp.App_Code;
using HRRcp.Areas.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.SqlClient;

namespace HRRcp.Areas.MVC.Controllers
{
    public class KrokiController : Controller
    {
        ObiegDokumentow ob = new ObiegDokumentow();

        const string stanowiskaQuery = @"select
  CONVERT(nvarchar(max), rd.Id) Value
, rd.Nazwa Text
, 1 sort
from obdRoleDict rd
union select '-1' Value, 'wybierz...' Text, 0 sort
order by sort, Text";

        //
        // GET: /MVC/Kroki/
        public ActionResult Index(int IdObiegu)
        {
            //ViewBag.krokiDict = ob.obdKrokiDict.ToList().FindAll(it => it.IdObieguDict == IdObiegu);
            ViewBag.krokiDict = ob.Database.SqlQuery<obdKrokiDict>("select * from obdKrokiDict where IdObieguDict = @IdObiegu order by Kolejnosc", new SqlParameter("IdObiegu", IdObiegu));
            ViewBag.ddlItems = ob.Database.SqlQuery<SelectListItem>(stanowiskaQuery).ToList();

            return View();
        }
	}
}
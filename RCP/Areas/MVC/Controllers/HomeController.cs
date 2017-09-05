using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRRcp.Areas.MVC.Models;


namespace HRRcp.Areas.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ObiegDokumentow db = new ObiegDokumentow();


        public ActionResult Szablon(int? idObieguDict)
        {
            ViewBag.zniacznikizadklarowanie = db.obdPolaZadeklarowane.ToList();
            ViewBag.znaczniki = db.obdPola.ToList();
            ViewBag.znacznikirodzaj = new SelectList(db.obdPolaDict, "Id", "Nazwa");
            ViewBag.nazwaPola = "";
            ViewBag.maxdDlugoscPola = "100";
            ViewBag.minDlugoscPola = "0";
            ViewBag.dozwoloneZnakiPola = "";
            ViewBag.idObieguDict = idObieguDict;
            return View(); 
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdSzablon obdSzablon = db.obdSzablon.Find(id);
            if (obdSzablon == null)
            {
                return HttpNotFound();
            }
            return View(obdSzablon);
        }

        // POST: MVC/obdObiegDict/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdSzablon obdSzablon = db.obdSzablon.Find(id);
            db.obdSzablon.Remove(obdSzablon);
            db.SaveChanges();
            return RedirectToAction("ListaSzablonow");
        }


        [HttpPost]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Szablon(int idObieguDict, string nazwa, string opis, string contentHTML, string znacznikiwszystkie)
        {  
            obdSzablon szablon = new obdSzablon();
            szablon.IdObieguDict = idObieguDict;
            szablon.Nazwa = nazwa;
            szablon.Opis = opis;
            szablon.ContentHTML = contentHTML;
            db.obdSzablon.Add(szablon);
            db.SaveChanges();
            //App_Code.AppUser user = App_Code.AppUser.CreateOrGetSession();
            //var polazadeklarowane = db.obdPolaZadeklarowane.ToList();
            //foreach(var item in polazadeklarowane)
            //{
            //    string nazwapolezadeklarowane = item.Nazwa;

            //    //string imie = db.Database.SqlQuery<string>(item.Sql.ToString(), user.Id).FirstOrDefault();

            //    //string imiePracownika = HRRcp.App_Code.db.Select.Scalar(item.Sql.ToString(),user.Id);


            //}
            if (!String.IsNullOrEmpty(znacznikiwszystkie))
            {
                string[] pola = znacznikiwszystkie.Split('|');
                if (pola.Length > 1)
                {
                    for (int i = 0; i < pola.Length - 1; i++) //ostani element jest pusty (-1)
                    {

                        string[] pole = pola[i].Split(';');
                        obdPola opola = new obdPola();
                        opola.IdObieguDict = szablon.IdObieguDict;
                        opola.IdPolaDict = int.Parse(pole[0]);
                        opola.IdSzablonu = szablon.Id;
                        opola.Nazwa = pole[1];
                        opola.MaxDlugosc = int.Parse(pole[2]);
                        opola.MinDlugosc = int.Parse(pole[3]);
                        opola.DozwoloneZnaki = pole[4];
                        db.obdPola.Add(opola);
                    }
                }
            }
            db.SaveChanges();






            return RedirectToAction("ListaSzablonow");
            return View();
        }

        public ActionResult ListaSzablonow()
        {
            IEnumerable<obdSzablon> szablony = db.obdSzablon.ToList();

            return View(szablony);
        }


        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdSzablon obdSzablon = db.obdSzablon.Find(id);
            if (obdSzablon == null)
            {
                return HttpNotFound();
            }
            return View(obdSzablon);
        }

        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdSzablon obdSzablon = db.obdSzablon.Find(id);
            if (obdSzablon == null)
            {
                return HttpNotFound();
            }
            return View(obdSzablon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdObieguDict,Nazwa,Opis,ContentHTML")] obdSzablon obdSzablon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdSzablon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaSzablonow");
            }
            return View(obdSzablon);
        }

        public ActionResult WypelnijDokument(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdSzablon obdSzablon = db.obdSzablon.Find(id);
            var obdPolaZ = db.obdPolaZadeklarowane.ToList();
            HRRcp.App_Code.AppUser user = HRRcp.App_Code.AppUser.CreateOrGetSession();
            var obdPola = db.obdPola;
            foreach (var item in obdPolaZ)
            {

                string nazwapolezadeklarowane = item.Nazwa;

                string wartosc = db.Database.SqlQuery<string>(item.Sql.ToString(), user.Id).FirstOrDefault();
                obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + nazwapolezadeklarowane + "%", wartosc);
                //string imiePracownika = HRRcp.App_Code.db.Select.Scalar(item.Sql.ToString(),user.Id);


            }

            foreach (var item in obdPola)
            {
                if (item.IdPolaDict.Equals(58))
                {
                    string textbox = @" <center>
                                        <div class='row'>
                                        <div style='display:inline-block;' class='input-field col s3 inline' >
                                        <input type='text' class='validate' id='" + item.Id + @"'/>
                                        <label>" + item.Nazwa + "</label></div></div></center>";
                                        obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%"+item.Nazwa+"%",textbox);
                }

                if (item.IdPolaDict.Equals(59))
                {

                    //string date = "<input class='col s3' type='date' id=" + item.Id + "/>";
                    string date = @"<center><div class='row'><div class='input-field col s3 inline'>
                        <input type='text' class='datepicker' id=" + item.Id +@">
                        <label for='"+item.Id+@"'>"+item.Nazwa+@"</label>
                        </div></div></center>";
                    obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + item.Nazwa + "%", date);



                }
//                if ("RadioButton".Equals(item.TypPola))
//                {
//                    string radio = @"<form action='#'>
//                        <input type='radio' class='mui-col-md-3' name='" + item.Id + @"' value='" + item.MinDlugosc + @"'> Male<br>
//                        <input type='radio'class='mui-col-md-3' name='" + item.Id + @"' value='" + item.MaxDlugosc + @"'> Female<br>
//                        </form>";
//                    obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + item.Nazwa + "%", radio);
//                }
                if (item.IdPolaDict.Equals(63))
                {
                    string checkbox = @"<center><form action='#'>
                        
                        <input type='checkbox' id='" + item.Id + "'  name='" + item.Id + "' value='" + item.Nazwa + @"'>"+@"
                        <label for='"+item.Id +"'>" + item.Nazwa + @"</label>
                        </p>
                        </form></center>";
                    obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + item.Nazwa + "%", checkbox);
                }
                //if ("Label".Equals(item.TypPola))
                //{
                //    string Label = "<span id='" + item.Id + "' name='" + item.Nazwa + ">" + item.Nazwa + "</span>";
                //    obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + item.Nazwa + "%", Label);
                //}

                if (item.IdPolaDict.Equals(62))
                {
                    string button = "<button type='button'>" + item.Nazwa + "</button>";
                    obdSzablon.ContentHTML = obdSzablon.ContentHTML.Replace("%" + item.Nazwa + "%", button);
                }


            }


            if (obdSzablon == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentHtml = obdSzablon.ContentHTML.Replace("~",  Server.MapPath("~"));
            return View();
        }


    }
}
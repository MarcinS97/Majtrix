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
    public class dicUprawnieniaController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/dicUprawnienia
        public ActionResult Index()
        {
            return View(db.dicUprawnienia.ToList());
        }

        // GET: MVC/dicUprawnienia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dicUprawnienia dicUprawnienia = db.dicUprawnienia.Find(id);
            if (dicUprawnienia == null)
            {
                return HttpNotFound();
            }
            return View(dicUprawnienia);
        }

        // GET: MVC/dicUprawnienia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/dicUprawnienia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,Opis")] dicUprawnienia dicUprawnienia)
        {
            if (ModelState.IsValid)
            {
                db.dicUprawnienia.Add(dicUprawnienia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dicUprawnienia);
        }

        // GET: MVC/dicUprawnienia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dicUprawnienia dicUprawnienia = db.dicUprawnienia.Find(id);
            if (dicUprawnienia == null)
            {
                return HttpNotFound();
            }
            return View(dicUprawnienia);
        }

        // POST: MVC/dicUprawnienia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,Opis")] dicUprawnienia dicUprawnienia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dicUprawnienia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dicUprawnienia);
        }

        // GET: MVC/dicUprawnienia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dicUprawnienia dicUprawnienia = db.dicUprawnienia.Find(id);
            if (dicUprawnienia == null)
            {
                return HttpNotFound();
            }
            return View(dicUprawnienia);
        }

        // POST: MVC/dicUprawnienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dicUprawnienia dicUprawnienia = db.dicUprawnienia.Find(id);
            db.dicUprawnienia.Remove(dicUprawnienia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Adding(string id)
        {

            if (id == null)
            {
                id = "";
            }
            IEnumerable<Pracownicy> pracownicy = db.Pracownicy;
            var selectList = pracownicy.Select(s => new SelectListItem
            { Value = s.Id.ToString(), Text = s.Imie + " " + s.Nazwisko });
            int id2;
            bool id3 = Int32.TryParse(id,out id2);

            ViewBag.PracownicyList = new SelectList(selectList, "Value", "Text", null);
            PracownicyUprawnienia pu = new PracownicyUprawnienia();
            if (id3)
            {
                pu = db.PracownicyUprawnienia.Where(x => x.IdPracownika == id2).FirstOrDefault();
            }
            IEnumerable<dicUprawnienia> uprawnienia = db.dicUprawnienia.Where(x => x.Id ==pu.IdUprawnienia);


            return View("Adding",uprawnienia.ToList());
        }

       
        public ActionResult ListAdding(string id)
        {

            if (id == null)
            {
                id = "";
            }
            IEnumerable<Pracownicy> pracownicy = db.Pracownicy;
            var selectList = pracownicy.Select(s => new SelectListItem
            { Value = s.Id.ToString(), Text = s.Imie + " " + s.Nazwisko });
            int id2;
            bool id3 = Int32.TryParse(id, out id2);

            ViewBag.PracownicyList = new SelectList(selectList, "Value", "Text", null);
            PracownicyUprawnienia pu = new PracownicyUprawnienia();
            if (id3)
            {
                pu = db.PracownicyUprawnienia.Where(x => x.IdPracownika == id2).FirstOrDefault();
            }
            IEnumerable<dicUprawnienia> uprawnienia = db.dicUprawnienia;
           
                 //uprawnienia = db.dicUprawnienia.Where(x => x.Id == pu.IdUprawnienia);
                 foreach(var item in uprawnienia)
                {

                if (pu != null)
                {
                    if (item.Id == pu.IdUprawnienia)
                    {
                        item.Dotyczy = true;
                    }
                    else
                    {
                        item.Dotyczy = false;
                    }
                }
                else
                {
                    item.Dotyczy = false;
                }
            }
            

            return PartialView("_List", uprawnienia.ToList());
        }

        //[HttpPost]
        //public ActionResult ListAdding(string[] check)
        //{




        //    return View("Adding");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

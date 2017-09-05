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
    public class obdPolaController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdPola
        public ActionResult Index()
        {
            return View(db.obdPola.ToList());
        }

        // GET: MVC/obdPola/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPola obdPola = db.obdPola.Find(id);
            if (obdPola == null)
            {
                return HttpNotFound();
            }
            return View(obdPola);
        }

        // GET: MVC/obdPola/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdPola/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdSzablonu,IdPolaDict,IdObieguDict,Nazwa,Grupa,MaxDlugosc,MinDlugosc,DozwoloneZnaki")] obdPola obdPola)
        {
            if (ModelState.IsValid)
            {
                db.obdPola.Add(obdPola);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obdPola);
        }

        // GET: MVC/obdPola/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPola obdPola = db.obdPola.Find(id);
            if (obdPola == null)
            {
                return HttpNotFound();
            }
            return View(obdPola);
        }

        // POST: MVC/obdPola/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdSzablonu,IdPolaDict,IdObieguDict,Nazwa,Grupa,MaxDlugosc,MinDlugosc,DozwoloneZnaki")] obdPola obdPola)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdPola).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdPola);
        }

        // GET: MVC/obdPola/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPola obdPola = db.obdPola.Find(id);
            if (obdPola == null)
            {
                return HttpNotFound();
            }
            return View(obdPola);
        }

        // POST: MVC/obdPola/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdPola obdPola = db.obdPola.Find(id);
            db.obdPola.Remove(obdPola);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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

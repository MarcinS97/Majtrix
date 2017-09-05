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
    public class obdAkcjeDictsController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdAkcjeDicts
        public ActionResult Index()
        {
            return View(db.obdAkcjeDict.ToList());
        }

        // GET: MVC/obdAkcjeDicts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdAkcjeDict obdAkcjeDict = db.obdAkcjeDict.Find(id);
            if (obdAkcjeDict == null)
            {
                return HttpNotFound();
            }
            return View(obdAkcjeDict);
        }

        // GET: MVC/obdAkcjeDicts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdAkcjeDicts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,Opis,Skrypt,Typ")] obdAkcjeDict obdAkcjeDict)
        {
            if (ModelState.IsValid)
            {
                db.obdAkcjeDict.Add(obdAkcjeDict);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obdAkcjeDict);
        }

        // GET: MVC/obdAkcjeDicts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdAkcjeDict obdAkcjeDict = db.obdAkcjeDict.Find(id);
            if (obdAkcjeDict == null)
            {
                return HttpNotFound();
            }
            return View(obdAkcjeDict);
        }

        // POST: MVC/obdAkcjeDicts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,Opis,Skrypt,Typ")] obdAkcjeDict obdAkcjeDict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdAkcjeDict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdAkcjeDict);
        }

        // GET: MVC/obdAkcjeDicts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdAkcjeDict obdAkcjeDict = db.obdAkcjeDict.Find(id);
            if (obdAkcjeDict == null)
            {
                return HttpNotFound();
            }
            return View(obdAkcjeDict);
        }

        // POST: MVC/obdAkcjeDicts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdAkcjeDict obdAkcjeDict = db.obdAkcjeDict.Find(id);
            db.obdAkcjeDict.Remove(obdAkcjeDict);
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

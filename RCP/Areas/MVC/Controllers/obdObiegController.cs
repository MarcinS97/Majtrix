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
    public class obdObiegController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdObieg
        public ActionResult Index()
        {
            return View(db.obdObieg.ToList());
        }

        // GET: MVC/obdObieg/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObieg obdObieg = db.obdObieg.Find(id);
            if (obdObieg == null)
            {
                return HttpNotFound();
            }
            return View(obdObieg);
        }

        // GET: MVC/obdObieg/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdObieg/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdObieguDict,IdTworzacego,DataZlozenia,DataZakonczenia,CzasTrwania,Status")] obdObieg obdObieg)
        {
            if (ModelState.IsValid)
            {
                db.obdObieg.Add(obdObieg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obdObieg);
        }

        // GET: MVC/obdObieg/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObieg obdObieg = db.obdObieg.Find(id);
            if (obdObieg == null)
            {
                return HttpNotFound();
            }
            return View(obdObieg);
        }

        // POST: MVC/obdObieg/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdObieguDict,IdTworzacego,DataZlozenia,DataZakonczenia,CzasTrwania,Status")] obdObieg obdObieg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdObieg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdObieg);
        }

        // GET: MVC/obdObieg/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObieg obdObieg = db.obdObieg.Find(id);
            if (obdObieg == null)
            {
                return HttpNotFound();
            }
            return View(obdObieg);
        }

        // POST: MVC/obdObieg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdObieg obdObieg = db.obdObieg.Find(id);
            db.obdObieg.Remove(obdObieg);
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

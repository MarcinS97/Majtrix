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
    public class obdObiegDictController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdObiegDict
        public ActionResult Index()
        {
            return View(db.obdObiegDict.ToList());
        }

        // GET: MVC/obdObiegDict/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObiegDict obdObiegDict = db.obdObiegDict.Find(id);
            if (obdObiegDict == null)
            {
                return HttpNotFound();
            }
            return View(obdObiegDict);
        }

        // GET: MVC/obdObiegDict/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdObiegDict/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,Opis,Uwagi,DataOd,DataDo,DataUtworzenia,CzasTrwania,Aktywny")] obdObiegDict obdObiegDict)
        {
            if (ModelState.IsValid)
            {
                db.obdObiegDict.Add(obdObiegDict);
                db.SaveChanges();
                return RedirectToAction("Szablon", "Home", new { @idobiegudict = obdObiegDict.Id });
            }

            return View(obdObiegDict);
        }

        // GET: MVC/obdObiegDict/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObiegDict obdObiegDict = db.obdObiegDict.Find(id);
            if (obdObiegDict == null)
            {
                return HttpNotFound();
            }
            return View(obdObiegDict);
        }

        // POST: MVC/obdObiegDict/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,Opis,Uwagi,DataOd,DataDo,DataUtworzenia,CzasTrwania,Aktywny")] obdObiegDict obdObiegDict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdObiegDict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdObiegDict);
        }

        // GET: MVC/obdObiegDict/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdObiegDict obdObiegDict = db.obdObiegDict.Find(id);
            if (obdObiegDict == null)
            {
                return HttpNotFound();
            }
            return View(obdObiegDict);
        }

        // POST: MVC/obdObiegDict/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdObiegDict obdObiegDict = db.obdObiegDict.Find(id);
            db.obdObiegDict.Remove(obdObiegDict);
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

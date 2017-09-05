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
    public class obdFiltrPoleDictsController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdFiltrPoleDicts
        public ActionResult Index()
        {
            return View(db.obdFiltrPoleDict.ToList());
        }

        // GET: MVC/obdFiltrPoleDicts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdFiltrPoleDict obdFiltrPoleDict = db.obdFiltrPoleDict.Find(id);
            if (obdFiltrPoleDict == null)
            {
                return HttpNotFound();
            }
            return View(obdFiltrPoleDict);
        }

        // GET: MVC/obdFiltrPoleDicts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdFiltrPoleDicts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,WarunekSQL,Typ")] obdFiltrPoleDict obdFiltrPoleDict)
        {
            if (ModelState.IsValid)
            {
                db.obdFiltrPoleDict.Add(obdFiltrPoleDict);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obdFiltrPoleDict);
        }

        // GET: MVC/obdFiltrPoleDicts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdFiltrPoleDict obdFiltrPoleDict = db.obdFiltrPoleDict.Find(id);
            if (obdFiltrPoleDict == null)
            {
                return HttpNotFound();
            }
            return View(obdFiltrPoleDict);
        }

        // POST: MVC/obdFiltrPoleDicts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,WarunekSQL,Typ")] obdFiltrPoleDict obdFiltrPoleDict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdFiltrPoleDict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdFiltrPoleDict);
        }

        // GET: MVC/obdFiltrPoleDicts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdFiltrPoleDict obdFiltrPoleDict = db.obdFiltrPoleDict.Find(id);
            if (obdFiltrPoleDict == null)
            {
                return HttpNotFound();
            }
            return View(obdFiltrPoleDict);
        }

        // POST: MVC/obdFiltrPoleDicts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdFiltrPoleDict obdFiltrPoleDict = db.obdFiltrPoleDict.Find(id);
            db.obdFiltrPoleDict.Remove(obdFiltrPoleDict);
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

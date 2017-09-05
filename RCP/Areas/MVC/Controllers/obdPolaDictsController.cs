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
    public class obdPolaDictsController : Controller
    {
        private ObiegDokumentow db = new ObiegDokumentow();

        // GET: MVC/obdPolaDicts
        public ActionResult Index()
        {

            // List<obdPolaDict> pola = db.obdPolaDict.ToList();
            // obdPolaDict obd = new obdPolaDict();
            //List<obdPolaDict> pola = db.Database.SqlQuery(obd.GetType(), "select * from obdPolaDict")
            return View(db.obdPolaDict.ToList());
           
        }

        // GET: MVC/obdPolaDicts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPolaDict obdPolaDict = db.obdPolaDict.Find(id);
            if (obdPolaDict == null)
            {
                return HttpNotFound();
            }
            return View(obdPolaDict);
        }

        // GET: MVC/obdPolaDicts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MVC/obdPolaDicts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,DozwoloneZnaki,MaxDlugosc,MinDlugosc")] obdPolaDict obdPolaDict)
        {
            if (ModelState.IsValid)
            {
                db.obdPolaDict.Add(obdPolaDict);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obdPolaDict);
        }

        // GET: MVC/obdPolaDicts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPolaDict obdPolaDict = db.obdPolaDict.Find(id);
            if (obdPolaDict == null)
            {
                return HttpNotFound();
            }
            return View(obdPolaDict);
        }

        // POST: MVC/obdPolaDicts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,DozwoloneZnaki,MaxDlugosc,MinDlugosc")] obdPolaDict obdPolaDict)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obdPolaDict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obdPolaDict);
        }

        // GET: MVC/obdPolaDicts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            obdPolaDict obdPolaDict = db.obdPolaDict.Find(id);
            if (obdPolaDict == null)
            {
                return HttpNotFound();
            }
            return View(obdPolaDict);
        }

        // POST: MVC/obdPolaDicts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            obdPolaDict obdPolaDict = db.obdPolaDict.Find(id);
            db.obdPolaDict.Remove(obdPolaDict);
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

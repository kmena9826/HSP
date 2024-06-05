using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class CS_TEST_DETALLEController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

        // GET: CS_TEST_DETALLE
        public ActionResult Index()
        {
            var cS_TEST_DETALLE = db.CS_TEST_DETALLE.Include(c => c.CS_TEST);
            return View(cS_TEST_DETALLE.ToList());
        }

        // GET: CS_TEST_DETALLE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_TEST_DETALLE cS_TEST_DETALLE = db.CS_TEST_DETALLE.Find(id);
            if (cS_TEST_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cS_TEST_DETALLE);
        }

        // GET: CS_TEST_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.TEST = new SelectList(db.CS_TEST, "TEST", "NOMBRE");
            return View();
        }


        // POST: CS_TEST_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TEST_DETALLE,TEST,SEXO,EDAD_MIN,EDAD_MAXIMA,RANGO_MINIMO,RANGO_MAXIMO,LOW_PANIC,HIGH_PANIC")] CS_TEST_DETALLE cS_TEST_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.CS_TEST_DETALLE.Add(cS_TEST_DETALLE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TEST = new SelectList(db.CS_TEST, "TEST", "NOMBRE", cS_TEST_DETALLE.TEST);
            return View(cS_TEST_DETALLE);
        }

        // GET: CS_TEST_DETALLE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_TEST_DETALLE cS_TEST_DETALLE = db.CS_TEST_DETALLE.Find(id);
            if (cS_TEST_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.TEST = new SelectList(db.CS_TEST, "TEST", "NOMBRE", cS_TEST_DETALLE.TEST);
            return View(cS_TEST_DETALLE);
        }

        // POST: CS_TEST_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TEST_DETALLE,TEST,SEXO,EDAD_MIN,EDAD_MAXIMA,RANGO_MINIMO,RANGO_MAXIMO,LOW_PANIC,HIGH_PANIC")] CS_TEST_DETALLE cS_TEST_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_TEST_DETALLE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TEST = new SelectList(db.CS_TEST, "TEST", "NOMBRE", cS_TEST_DETALLE.TEST);
            return View(cS_TEST_DETALLE);
        }

        // GET: CS_TEST_DETALLE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_TEST_DETALLE cS_TEST_DETALLE = db.CS_TEST_DETALLE.Find(id);
            if (cS_TEST_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cS_TEST_DETALLE);
        }

        // POST: CS_TEST_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_TEST_DETALLE cS_TEST_DETALLE = db.CS_TEST_DETALLE.Find(id);
            db.CS_TEST_DETALLE.Remove(cS_TEST_DETALLE);
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

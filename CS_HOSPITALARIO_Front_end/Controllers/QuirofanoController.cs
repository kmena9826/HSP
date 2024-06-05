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
    public class QuirofanoController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

        // GET: Quirofano
        public ActionResult Index()
        {
            return View(db.CS_QUIROFANO.ToList());
        }

        // GET: Quirofano/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_QUIROFANO cS_QUIROFANO = db.CS_QUIROFANO.Find(id);
            if (cS_QUIROFANO == null)
            {
                return HttpNotFound();
            }
            return View(cS_QUIROFANO);
        }

        // GET: Quirofano/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quirofano/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_QUIROFANO,COD_QUIROFANO,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_QUIROFANO cS_QUIROFANO)
        {
            if (ModelState.IsValid)
            {
                cS_QUIROFANO.ACTIVO = true;
                cS_QUIROFANO.USUARIO_CREACION = 1;
                cS_QUIROFANO.FECHA_CREACION = DateTime.Now;
                db.CS_QUIROFANO.Add(cS_QUIROFANO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cS_QUIROFANO);
        }

        // GET: Quirofano/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_QUIROFANO cS_QUIROFANO = db.CS_QUIROFANO.Find(id);
            if (cS_QUIROFANO == null)
            {
                return HttpNotFound();
            }
            return View(cS_QUIROFANO);
        }

        // POST: Quirofano/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_QUIROFANO,COD_QUIROFANO,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_QUIROFANO cS_QUIROFANO)
        {
            if (ModelState.IsValid)
            {
                cS_QUIROFANO.USUARIO_MODIFICACION = 1;
                cS_QUIROFANO.FECHA_MODIFICACION = DateTime.Now;
                db.Entry(cS_QUIROFANO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_QUIROFANO);
        }

        // GET: Quirofano/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_QUIROFANO cS_QUIROFANO = db.CS_QUIROFANO.Find(id);
            if (cS_QUIROFANO == null)
            {
                return HttpNotFound();
            }
            return View(cS_QUIROFANO);
        }

        // POST: Quirofano/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_QUIROFANO cS_QUIROFANO = db.CS_QUIROFANO.Find(id);
            db.CS_QUIROFANO.Remove(cS_QUIROFANO);
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

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
    public class EmergenciasController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

        // GET: Emergencias
        public ActionResult Index()
        {
            return View(db.CS_EMERGENCIA.ToList());
        }














        //// GET: Emergencias/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CS_EMERGENCIA cS_EMERGENCIA = db.CS_EMERGENCIA.Find(id);
        //    if (cS_EMERGENCIA == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cS_EMERGENCIA);
        //}
        
        //// GET: Emergencias/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Emergencias/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EMERGENCIA,PEDIDO,CLIENTE_ID,NIVEL_EMERGENCIA,ADMISION_ID,FECHA_INGRESO,TRIAGE,TRANSFERENCIA,TIPO_ALTA,FECHA_ALTA,FECHA_CREACION,USUARIO_CREACION,FECHA_MODIFICACION,USUARIO_MODIFICACION,ADMISION_EGY,PEDIDO_EGY,CLIENTE_EGY")] CS_EMERGENCIA cS_EMERGENCIA)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CS_EMERGENCIA.Add(cS_EMERGENCIA);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(cS_EMERGENCIA);
        //}

        //// GET: Emergencias/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CS_EMERGENCIA cS_EMERGENCIA = db.CS_EMERGENCIA.Find(id);
        //    if (cS_EMERGENCIA == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cS_EMERGENCIA);
        //}

        //// POST: Emergencias/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EMERGENCIA,PEDIDO,CLIENTE_ID,NIVEL_EMERGENCIA,ADMISION_ID,FECHA_INGRESO,TRIAGE,TRANSFERENCIA,TIPO_ALTA,FECHA_ALTA,FECHA_CREACION,USUARIO_CREACION,FECHA_MODIFICACION,USUARIO_MODIFICACION,ADMISION_EGY,PEDIDO_EGY,CLIENTE_EGY")] CS_EMERGENCIA cS_EMERGENCIA)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cS_EMERGENCIA).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(cS_EMERGENCIA);
        //}

        //// GET: Emergencias/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CS_EMERGENCIA cS_EMERGENCIA = db.CS_EMERGENCIA.Find(id);
        //    if (cS_EMERGENCIA == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cS_EMERGENCIA);
        //}

        //// POST: Emergencias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CS_EMERGENCIA cS_EMERGENCIA = db.CS_EMERGENCIA.Find(id);
        //    db.CS_EMERGENCIA.Remove(cS_EMERGENCIA);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;

namespace CS_HOSPITALARIO.Controllers
{
    public class DefinicionProcedimientoController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

       public ActionResult Definicion_Proc_Listar()
        {
            return View(db.CS_DEFINICION_PROCEDIMIENTO.ToList());
        }


        
        public ActionResult Definicion_Proc_Detalle (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO = db.CS_DEFINICION_PROCEDIMIENTO.Find(id);
            if (cS_DEFINICION_PROCEDIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

      
        public ActionResult Definicion_Proc_Nuevo()
        {
            ViewBag.ARTICULO = new SelectList(db.ARTICULO.Where(y => y.CLASIFICACION_1=="07SE" && y.CLASIFICACION_5== "S06"), "ARTICULO1", "DESCRIPCION");
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION");
            return View();
        }

        [HttpPost]
        
        public ActionResult Definicion_Proc_Nuevo([Bind(Include = "ID_EXAMEN,ARTICULO,DESCRIPCION,AREA_SERVICIO,ACTIVO")] CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.CS_DEFINICION_PROCEDIMIENTO.Add(cS_DEFINICION_PROCEDIMIENTO);
                db.SaveChanges();
                return RedirectToAction("Definicion_Proc_Listar");
            }

            ViewBag.ARTICULO = new SelectList(db.ARTICULO, "ARTICULO1", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }
        [HttpPost]
        public JsonResult BuscarAticulo(string nombre)
        {
            return Json(SearchArticle(nombre),JsonRequestBehavior.AllowGet);
        }

        public List<ARTICULO> SearchArticle(string nombre)
        {
               db.Configuration.LazyLoadingEnabled = false;
               db.Configuration.ProxyCreationEnabled = false;

                var articulo = db.ARTICULO.OrderBy(x => x.DESCRIPCION)
                                        .Where(x => x.DESCRIPCION.Contains(nombre) && x.CLASIFICACION_1 == "07SE" && x.CLASIFICACION_5 == "S06")
                                        .Take(10)
                                        .ToList();

                return articulo;
            
        }

       


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO = db.CS_DEFINICION_PROCEDIMIENTO.Find(id);
            if (cS_DEFINICION_PROCEDIMIENTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ARTICULO = new SelectList(db.ARTICULO, "ARTICULO1", "PLANTILLA_SERIE", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

        // POST: DefinicionProcedimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "ID_EXAMEN,ARTICULO,DESCRIPCION,AREA_SERVICIO,ACTIVO")] CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_DEFINICION_PROCEDIMIENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ARTICULO = new SelectList(db.ARTICULO, "ARTICULO1", "PLANTILLA_SERIE", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

        // GET: DefinicionProcedimiento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO = db.CS_DEFINICION_PROCEDIMIENTO.Find(id);
            if (cS_DEFINICION_PROCEDIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

        // POST: DefinicionProcedimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO = db.CS_DEFINICION_PROCEDIMIENTO.Find(id);
            db.CS_DEFINICION_PROCEDIMIENTO.Remove(cS_DEFINICION_PROCEDIMIENTO);
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

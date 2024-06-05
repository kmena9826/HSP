//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using CS_HOSPITALARIO.Models;

//namespace CS_HOSPITALARIO_Front_end.Controllers
//{
//    public class CuartoController : Controller
//    {
//        private HospitalarioBD db = new HospitalarioBD();

//        // GET: Cuarto
//        public ActionResult Index()
//        {
//            var cS_CUARTO = db.CS_CUARTO.Include(c => c.CS_HABITACION);
//            return View(cS_CUARTO.ToList());
//        }

//        // GET: Cuarto/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_CUARTO cS_CUARTO = db.CS_CUARTO.Find(id);
//            if (cS_CUARTO == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_CUARTO);
//        }

//        // GET: Cuarto/Create
//        public ActionResult Create()
//        {
//            ViewBag.ID_HABITACION = new SelectList(db.CS_HABITACION, "ID_HABITACION", "COD_HABITACION");
//            return View();
//        }

//        // POST: Cuarto/Create
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID_CUARTO,COD_CUARTO,ID_HABITACION,CANTIDAD_CAMA,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CUARTO cS_CUARTO)
//        {
//            if (ModelState.IsValid)
//            {
//                cS_CUARTO.USUARIO_CREACION = 1;
//                cS_CUARTO.FECHA_CREACION = DateTime.Now;
//                cS_CUARTO.ACTIVO = true;
//                db.CS_CUARTO.Add(cS_CUARTO);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ID_HABITACION = new SelectList(db.CS_HABITACION, "ID_HABITACION", "COD_HABITACION", cS_CUARTO.ID_HABITACION);
//            return View(cS_CUARTO);
//        }

//        // GET: Cuarto/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_CUARTO cS_CUARTO = db.CS_CUARTO.Find(id);
//            if (cS_CUARTO == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ID_HABITACION = new SelectList(db.CS_HABITACION, "ID_HABITACION", "COD_HABITACION", cS_CUARTO.ID_HABITACION);
//            return View(cS_CUARTO);
//        }

//        // POST: Cuarto/Edit/5
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID_CUARTO,COD_CUARTO,ID_HABITACION,CANTIDAD_CAMA,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CUARTO cS_CUARTO)
//        {
//            if (ModelState.IsValid)
//            {
//                cS_CUARTO.USUARIO_MODIFICACION = 1;
//                cS_CUARTO.FECHA_MODIFICACION = DateTime.Now;
//                db.Entry(cS_CUARTO).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ID_HABITACION = new SelectList(db.CS_HABITACION, "ID_HABITACION", "COD_HABITACION", cS_CUARTO.ID_HABITACION);
//            return View(cS_CUARTO);
//        }

//        // GET: Cuarto/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_CUARTO cS_CUARTO = db.CS_CUARTO.Find(id);
//            if (cS_CUARTO == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_CUARTO);
//        }

//        // POST: Cuarto/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            CS_CUARTO cS_CUARTO = db.CS_CUARTO.Find(id);
//            db.CS_CUARTO.Remove(cS_CUARTO);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using CS_HOSPITALARIO.Models;

//namespace CS_HOSPITALARIO.Controllers
//{
//    public class HabitacionController : Controller
//    {
//        private HospitalarioBD db = new HospitalarioBD();

//        // GET: Habitacion
//        public ActionResult Index()
//        {
//            return View(db.CS_HABITACION.ToList());
//        }

//        // GET: Habitacion/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_HABITACION cS_HABITACION = db.CS_HABITACION.Find(id);
//            if (cS_HABITACION == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_HABITACION);
//        }

//        // GET: Habitacion/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Habitacion/Create
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID_HABITACION,COD_HABITACION,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_HABITACION cS_HABITACION)
//        {
//            if (ModelState.IsValid)
//            {
//               // db.CS_HABITACION.Add(cS_HABITACION);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(cS_HABITACION);
//        }

//        // GET: Habitacion/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_HABITACION cS_HABITACION = db.CS_HABITACION.Find(id);
//            if (cS_HABITACION == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_HABITACION);
//        }

//        // POST: Habitacion/Edit/5
//        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
//        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID_HABITACION,COD_HABITACION,DESCRIPCION,ESTADO,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_HABITACION cS_HABITACION)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(cS_HABITACION).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(cS_HABITACION);
//        }

//        // GET: Habitacion/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_HABITACION cS_HABITACION = db.CS_HABITACION.Find(id);
//            if (cS_HABITACION == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_HABITACION);
//        }

//        // POST: Habitacion/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            CS_HABITACION cS_HABITACION = db.CS_HABITACION.Find(id);
//            db.CS_HABITACION.Remove(cS_HABITACION);
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

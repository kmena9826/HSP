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
//    public class ImagenologiaController : Controller
//    {
//        private HospitalarioBD db = new HospitalarioBD();

//       public ActionResult Plantillas_Listar()
//        {
//            return View(db.CS_PLANTILLA.ToList());
//        }


//        public ActionResult Plantillas_Nueva ()
//        {
//            ViewBag.ESTUDIO = new SelectList(db.ARTICULO.Where(y => y.CLASIFICACION_1 == "07SE" && y.CLASIFICACION_5 == "S01"), "ARTICULO1", "DESCRIPCION");
//            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES");
//            return View();

//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Plantillas_Nueva([Bind(Include = "ID_PLANTILLA, EXAMEN_IMAG_ID,DOCTOR_ID,COMENTARIO ACTIVO")] CS_PLANTILLA pLANTILLA,  string ESTUDIO, string DOCTOR, string COMENTARIO)
//        {
//            if (ModelState.IsValid)
//            {
//                pLANTILLA.ACTIVO = true;
//                pLANTILLA.EXAMEN_IMAG_ID = ESTUDIO;
//                pLANTILLA.DOCTOR_ID = int.Parse(DOCTOR);
//                pLANTILLA.COMENTARIO = COMENTARIO;
//                db.CS_PLANTILLA.Add(pLANTILLA);
//                db.SaveChanges();
//                return RedirectToAction("Plantillas_Listar");
//            }
//            ViewBag.ESTUDIO = new SelectList(db.ARTICULO.Where(y => y.CLASIFICACION_1 == "07SE" && y.CLASIFICACION_5 == "S01"), "ARTICULO1", "DESCRIPCION");
//            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES");
//            return View(pLANTILLA);

//        }

//        public ActionResult Plantillas_Editar(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_PLANTILLA pLANTILLA = db.CS_PLANTILLA.Find(id);
//            if (pLANTILLA == null)
//            {
//                return HttpNotFound();
//            }

//            ViewBag.ESTUDIO = new SelectList(db.ARTICULO.Where(y => y.CLASIFICACION_1 == "07SE" && y.CLASIFICACION_5 == "S01" && y.ARTICULO1==pLANTILLA.EXAMEN_IMAG_ID), "ARTICULO1", "DESCRIPCION", pLANTILLA.EXAMEN_IMAG_ID);
//            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", pLANTILLA.DOCTOR_ID);
//            ViewBag.COMENTARIO = pLANTILLA.COMENTARIO;
//            return View(pLANTILLA);
//        }

//        [HttpPost]
//        [ValidateInput(false)]
//        public ActionResult Plantillas_Editar([Bind(Include = "ID_PLANTILLA, ESTUDIO,DOCTOR,COMENTARIO, ACTIVO")] CS_PLANTILLA pLANTILLA, string ESTUDIO, string DOCTOR, string COMENTARIO)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(pLANTILLA).State = EntityState.Modified;
//                pLANTILLA.COMENTARIO = COMENTARIO;
//                pLANTILLA.EXAMEN_IMAG_ID = ESTUDIO;
//                pLANTILLA.DOCTOR_ID = int.Parse(DOCTOR);
//                db.SaveChanges();
//                return RedirectToAction("Plantillas_Listar");
//            }
//            ViewBag.ESTUDIO = new SelectList(db.ARTICULO.Where(y => y.CLASIFICACION_1 == "07SE" && y.CLASIFICACION_5 == "S01" && y.ARTICULO1 == pLANTILLA.EXAMEN_IMAG_ID), "ARTICULO1", "DESCRIPCION", pLANTILLA.EXAMEN_IMAG_ID);
//            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", pLANTILLA.DOCTOR_ID);
//            ViewBag.COMENTARIO = pLANTILLA.COMENTARIO;
//            return View(pLANTILLA);
//        }





//            public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_IMAGENOLOGIA cS_IMAGENOLOGIA = db.CS_IMAGENOLOGIA.Find(id);
//            if (cS_IMAGENOLOGIA == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_IMAGENOLOGIA);
//        }

        
       

        
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_IMAGENOLOGIA cS_IMAGENOLOGIA = db.CS_IMAGENOLOGIA.Find(id);
//            if (cS_IMAGENOLOGIA == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ID_ADMISION = new SelectList(db.CS_ADMISION, "ID_ADMISION", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_ADMISION);
//            ViewBag.ID_IMAGENOLOGIA = new SelectList(db.CS_IMAGENOLOGIA, "ID_IMAGENOLOGIA", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_IMAGENOLOGIA);
//            ViewBag.ID_IMAGENOLOGIA = new SelectList(db.CS_IMAGENOLOGIA, "ID_IMAGENOLOGIA", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_IMAGENOLOGIA);
//            ViewBag.RADIOLOGO_ID = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", cS_IMAGENOLOGIA.RADIOLOGO_ID);
//            return View(cS_IMAGENOLOGIA);
//        }

       
//        [HttpPost]
      
//        public ActionResult Edit([Bind(Include = "ID_IMAGENOLOGIA,RADIOLOGO_ID,CLIENTE_ID,PROCEDIMIENTO,ID_ADMISION,PEDIDO,SEXO,EDAD,IMPRESO,ENV_POR_CORREO,USUARIO_REGISTRO,FECHA_REGISTRO,STATUS,STAT")] CS_IMAGENOLOGIA cS_IMAGENOLOGIA)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(cS_IMAGENOLOGIA).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ID_ADMISION = new SelectList(db.CS_ADMISION, "ID_ADMISION", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_ADMISION);
//            ViewBag.ID_IMAGENOLOGIA = new SelectList(db.CS_IMAGENOLOGIA, "ID_IMAGENOLOGIA", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_IMAGENOLOGIA);
//            ViewBag.ID_IMAGENOLOGIA = new SelectList(db.CS_IMAGENOLOGIA, "ID_IMAGENOLOGIA", "CLIENTE_ID", cS_IMAGENOLOGIA.ID_IMAGENOLOGIA);
//            ViewBag.RADIOLOGO_ID = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", cS_IMAGENOLOGIA.RADIOLOGO_ID);
//            return View(cS_IMAGENOLOGIA);
//        }

       
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            CS_IMAGENOLOGIA cS_IMAGENOLOGIA = db.CS_IMAGENOLOGIA.Find(id);
//            if (cS_IMAGENOLOGIA == null)
//            {
//                return HttpNotFound();
//            }
//            return View(cS_IMAGENOLOGIA);
//        }

       
//        [HttpPost, ActionName("Delete")]
      
//        public ActionResult DeleteConfirmed(int id)
//        {
//            CS_IMAGENOLOGIA cS_IMAGENOLOGIA = db.CS_IMAGENOLOGIA.Find(id);
//            db.CS_IMAGENOLOGIA.Remove(cS_IMAGENOLOGIA);
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

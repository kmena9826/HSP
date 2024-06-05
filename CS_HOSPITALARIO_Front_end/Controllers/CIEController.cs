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
    public class CIEController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

        // GET: CIE
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetCapitulos()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var CAPITULOS = db.SP_Listado_CIE_Capitulos().Select(x => new
                    {
                        ID = x.GRP,
                        DESCRIPCION = x.COD_CIE + "-" + x.DEC
                    }).ToList();
                    return Json(CAPITULOS.Select(x => new
                    {
                        ID = x.ID,
                        DESCRIPCION = x.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public JsonResult GetSubCapitulos(int id_capitulo)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var SUBCAPITULOS = db.SP_Listado_CIE_SubCapitulos().Where(X => X.GRP == id_capitulo.ToString()).Select(x => new
                    {
                        ID = x.COD_CIE,
                        DESCRIPCION = x.DEC
                    }).ToList();
                    return Json(SUBCAPITULOS.Select(x => new
                    {
                        ID = x.ID,
                        DESCRIPCION = x.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public JsonResult GetTema(string id_subcapitulo)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var TEMAS = db.SP_Listado_CIE_Tema().Where(X => X.GRP == id_subcapitulo.ToString()).Select(x => new
                    {
                        ID = x.COD_CIE,
                        DESCRIPCION = x.DEC
                    }).ToList();
                    return Json(TEMAS.Select(x => new
                    {
                        ID = x.ID,
                        DESCRIPCION = x.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public JsonResult GetCIE(string id_tema)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var CIES = db.CS_CIE.Where(x => x.COD_CIE.Contains(id_tema) && x.GRP == string.Empty).Select(x => new
                    {
                        ID = x.COD_CIE,
                        DESCRIPCION = x.DEC
                    }).ToList();
                    return Json(CIES.Select(x => new
                    {
                        ID = x.ID,
                        DESCRIPCION = x.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        [HttpPost]
        public JsonResult GetCatalogoCIE()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var datos = db.SP_Listado_CIE_Total().Select(x => new
                    {
                        ID = x.COD_CIE,
                        DESCRIPCION = x.DESCRIPCION,
                        GRP = x.GRP
                    }).ToList();
                    //return  Json (datos,JsonRequestBehavior.AllowGet);
                    return new JsonResult { Data = new { datos } };
                    //return Json(datos.Select(x => new
                    //{
                    //    ID = x.ID,
                    //    DESCRIPCION = x.DESCRIPCION,
                    //    GRP = x.GRP
                    //}), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        // GET: CIE/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CIE cS_CIE = db.CS_CIE.Find(id);
            if (cS_CIE == null)
            {
                return HttpNotFound();
            }
            return View(cS_CIE);
        }

        // GET: CIE/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CIE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_CIE,COD_CIE,DEC,GRP,VERSION,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CIE cS_CIE)
        {
            if (ModelState.IsValid)
            {
                db.CS_CIE.Add(cS_CIE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cS_CIE);
        }

        // GET: CIE/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CIE cS_CIE = db.CS_CIE.Find(id);
            if (cS_CIE == null)
            {
                return HttpNotFound();
            }
            return View(cS_CIE);
        }

        // POST: CIE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_CIE,COD_CIE,DEC,GRP,VERSION,ACTIVO,USUARIO_CREACION,FECHA_CREACION,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CIE cS_CIE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_CIE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_CIE);
        }

        // GET: CIE/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CIE cS_CIE = db.CS_CIE.Find(id);
            if (cS_CIE == null)
            {
                return HttpNotFound();
            }
            return View(cS_CIE);
        }

        // POST: CIE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_CIE cS_CIE = db.CS_CIE.Find(id);
            db.CS_CIE.Remove(cS_CIE);
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

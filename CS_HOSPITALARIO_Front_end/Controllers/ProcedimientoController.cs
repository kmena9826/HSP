using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class ProcedimientoController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();

        // GET: Procedimiento
        public ActionResult Index()
        {
            var cS_DEFINICION_PROCEDIMIENTO = db.CS_DEFINICION_PROCEDIMIENTO.Include(c => c.ARTICULO1).Include(c => c.CS_CATALOGO_DETALLE);
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

        public JsonResult GetTestDetalles(int TEST_DETALLE)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var cS_TEST = db.CS_TEST_DETALLE.Include(p=>p.CS_TEST).Where(x => x.ACTIVO == true && x.TEST == TEST_DETALLE).ToList();
                    return Json(cS_TEST.Select(x => new
                    {
                        x.TEST_DETALLE,
                        x.TEST,
                        x.SEXO,
                        x.EDAD_MIN,
                        x.EDAD_MAXIMA,
                        x.RANGO_MINIMO,
                        x.RANGO_MAXIMO,
                        x.LOW_PANIC,
                        x.HIGH_PANIC,
                        x.CS_TEST.VALOR
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public ActionResult TestDetalles()
        {
            //var cS_TEST_DETALLE = db.CS_TEST.Include(c => c.CS_TEST);
            //return View(cS_TEST_DETALLE.ToList());
            var cS_TEST_DETALLE = db.CS_TEST;
            return View(cS_TEST_DETALLE.ToList());
        }
        [HttpPost]
        public JsonResult InsertarTestDetalle(CS_TEST_DETALLE detalle, int TEST_DETALLE)
        {
            var status = true;
            try
            {
                detalle.TEST = TEST_DETALLE;
                detalle.ACTIVO = true;
                db.CS_TEST_DETALLE.Add(detalle);
                status = db.SaveChanges() > 0;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult ActualizarTestDetalle(CS_TEST_DETALLE detalle)
        {
            var status = true;
            try
            {
                detalle.ACTIVO = true;
                db.Entry(detalle).State = EntityState.Modified;
                status = db.SaveChanges() > 0;
                //db.SP_Actualizar_pedido_monto_paciente(cod_pedido,version,total, cantarticulo);
                //status = true;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult EliminarTestDetalle(int id)
        {
            var status = true;
            try
            {

                CS_TEST_DETALLE cS_TEST_DETALLE = db.CS_TEST_DETALLE.Find(id);
                cS_TEST_DETALLE.ACTIVO = false;
                //db.CS_TEST_DETALLE.Remove(cS_TEST_DETALLE);
                status = db.SaveChanges() > 0;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        // GET: Procedimiento/Details/5
        public ActionResult Details(string id)
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
        public ActionResult NuevoTest()
        {
            //ViewBag.ID_EXAMEN = new SelectList(db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ACTIVO == true).Select(x => new { x.ID_EXAMEN, ARTICULO = x.ID_EXAMEN + "-" + x.ARTICULO }), "ID_EXAMEN", "ARTICULO");
            ViewBag.UNIDAD_MEDIDA = new SelectList(db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 26 && x.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
            return View();
        }
        [HttpPost]
        public ActionResult NuevoTest([Bind(Include = "TEST,NOMBRE,OBSERVACION,ACTIVO,UNIDAD_MEDIDA,VALOR")] CS_TEST cS_TEST)
        {
            if (ModelState.IsValid)
            {
                cS_TEST.ACTIVO = true;
                db.CS_TEST.Add(cS_TEST);
                db.SaveChanges();

                //CS_DEFINICION_PROCEDIMIENTO_DET definidet = new CS_DEFINICION_PROCEDIMIENTO_DET();
                //definidet.PROCEDIMIENTO = ID_EXAMEN;
                //definidet.TEST = cS_TEST.TEST;
                //db.CS_DEFINICION_PROCEDIMIENTO_DET.Add(definidet);
                //db.SaveChanges();

                return RedirectToAction("TestDetalles");
            }
            ViewBag.UNIDAD_MEDIDA = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_TEST.UNIDAD_MEDIDA);
            return View(cS_TEST);
        }
        public ActionResult EditarTest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_TEST cS_TEST = db.CS_TEST.Find(id);
            if (cS_TEST == null)
            {
                return HttpNotFound();
            }
            //var procedimientodet = db.CS_DEFINICION_PROCEDIMIENTO_DET.Where(x => x.TEST == id).FirstOrDefault();
            //var procedimiento = db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ID_EXAMEN == procedimientodet.CS_DEFINICION_PROCEDIMIENTO.ID_EXAMEN).FirstOrDefault();

            List<Valor> valor = new List<Valor>();

            Valor valor1 = new Valor();
            valor1.ID = "Seleccione";
            valor1.DESCRIPCION = "Seleccione";
            valor.Add(valor1);

            Valor valor2 = new Valor();
            valor2.ID = "N";
            valor2.DESCRIPCION = "Numerico";
            valor.Add(valor2);

            Valor valor3 = new Valor();
            valor3.ID = "A";
            valor3.DESCRIPCION = "AlfaNumerico";
            valor.Add(valor3);


            ViewBag.VALOR = new SelectList(valor, "ID", "DESCRIPCION", cS_TEST.VALOR);
            //ViewBag.ID_EXAMEN = new SelectList(db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ACTIVO == true).Select(x => new { x.ID_EXAMEN, ARTICULO = x.ID_EXAMEN + "-" + x.ARTICULO }), "ID_EXAMEN", "ARTICULO", procedimiento.ID_EXAMEN);
            ViewBag.UNIDAD_MEDIDA = new SelectList(db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 26 && x.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION", cS_TEST.UNIDAD_MEDIDA);
            return View(cS_TEST);
        }

        [HttpPost]
        public ActionResult EditarTest([Bind(Include = "TEST,NOMBRE,OBSERVACION,ACTIVO,UNIDAD_MEDIDA,VALOR")] CS_TEST cS_TEST)
        {
            if (ModelState.IsValid)
            {
                var testbd = db.CS_TEST.Where(x => x.TEST == cS_TEST.TEST).FirstOrDefault();
                testbd.NOMBRE = cS_TEST.NOMBRE;
                testbd.UNIDAD_MEDIDA = cS_TEST.UNIDAD_MEDIDA;
                testbd.VALOR = cS_TEST.VALOR;
                testbd.OBSERVACION = cS_TEST.OBSERVACION;
                db.SaveChanges();
                return RedirectToAction("TestDetalles");
            }
            ViewBag.UNIDAD_MEDIDA = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_TEST.UNIDAD_MEDIDA);
            return View(cS_TEST);
        }
        // GET: Procedimiento/Create
        public ActionResult Create()
        {
            var parametros = db.CS_PARAMETROS.FirstOrDefault();
            ViewBag.TEST = new SelectList(db.CS_TEST.Where(x => x.ACTIVO == true), "TEST", "NOMBRE");
            ViewBag.ARTICULO = new SelectList(db.ARTICULO.Where(x => x.CLASIFICACION_1 == parametros.CLA_LAB).Select(x => new
            {
                x.ARTICULO1,
                DESCRIPCION = x.ARTICULO1 + "-" + x.DESCRIPCION
            }), "ARTICULO1", "DESCRIPCION");
            //ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 2 && x.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION",26);
            ViewBag.COLOR_TUBO = new SelectList(db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 27 && x.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
            return View();
        }
        public JsonResult GetUnidadMedida()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 26).Select(x => new
                    {
                        ID = x.ID_CAT_DETALLE,
                        DESCRIPCION = x.DESCRIPCION
                    }).ToList();
                    return Json(AREA.Select(x => new
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
        public JsonResult GetTest(int test)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var TEST = db.CS_TEST.Where(a => a.TEST == test).ToList();
                    return Json(TEST.Select(x => new
                    {
                        x.TEST,
                        x.NOMBRE,
                        UNIDAD_MEDIDA = x.CS_CATALOGO_DETALLE.DESCRIPCION,
                        x.VALOR,
                        x.OBSERVACION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        // POST: Procedimiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_EXAMEN,ARTICULO,DESCRIPCION,AREA_SERVICIO,ACTIVO")] CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.CS_DEFINICION_PROCEDIMIENTO.Add(cS_DEFINICION_PROCEDIMIENTO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ARTICULO = new SelectList(db.ARTICULO, "ARTICULO1", "PLANTILLA_SERIE", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);

            return View(cS_DEFINICION_PROCEDIMIENTO);
        }
        [HttpPost]
        public JsonResult GuardarProcedimientoDetalle(CS_DEFINICION_PROCEDIMIENTO procedimiento, List<CS_TEST> test)
        {
            bool status = true;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //db.CS_DEFINICION_PROCEDIMIENTO.Add(procedimiento);
                    //db.SaveChanges();
                    //procedimiento.ID_EXAMEN = procedimiento.ARTICULO;
                    db.CS_DEFINICION_PROCEDIMIENTO.Add(procedimiento);
                    db.SaveChanges();
                    int id_examen = procedimiento.ID_EXAMEN;
                    foreach (var i in test)
                    {
                        // i.ID_EXAMEN = procedimiento.ARTICULO;
                        //db.CS_TEST.Add(i);
                        //db.SaveChanges();

                        CS_DEFINICION_PROCEDIMIENTO_DET detalle = new CS_DEFINICION_PROCEDIMIENTO_DET();
                        detalle.PROCEDIMIENTO = procedimiento.ID_EXAMEN;
                        detalle.ACTIVO = true;
                        detalle.TEST = i.TEST;
                        db.CS_DEFINICION_PROCEDIMIENTO_DET.Add(detalle);
                        db.SaveChanges();
                    }
                    status = true;
                    transaction.Commit();
                }
                catch (EntityException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    status = false;
                }

            }

            return new JsonResult
            {
                Data = new
                {
                    status = status
                }
            };
        }
        [HttpPost]
        public JsonResult GuardarProcedimiento(CS_DEFINICION_PROCEDIMIENTO procedimiento)
        {
            bool status = true;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    CS_DEFINICION_PROCEDIMIENTO definicionbd = db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ID_EXAMEN == procedimiento.ID_EXAMEN).FirstOrDefault();

                    definicionbd.ARTICULO = procedimiento.ARTICULO;
                    definicionbd.OBSERVACION = procedimiento.OBSERVACION;
                    definicionbd.AREA_SERVICIO = 26;
                    definicionbd.COLOR_TUBO = procedimiento.COLOR_TUBO;
                    definicionbd.IMPRIME_ETIQUETA = procedimiento.IMPRIME_ETIQUETA;
                    definicionbd.ENVIAR_CORREO = procedimiento.ENVIAR_CORREO;
                    db.SaveChanges();

                    status = true;
                    transaction.Commit();
                }
                catch (EntityException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    status = false;
                }

            }

            return new JsonResult
            {
                Data = new
                {
                    status = status
                }
            };
        }
        [HttpPost]
        public JsonResult GuardarProcedimientoDetalleSolo(int test, int procedimeinto)
        {
            bool status = true;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //i.ID_EXAMEN = procedimiento.ARTICULO;
                    // db.CS_TEST.Add(test);
                    //db.SaveChanges();


                    CS_DEFINICION_PROCEDIMIENTO_DET detalle = new CS_DEFINICION_PROCEDIMIENTO_DET();
                    detalle.PROCEDIMIENTO = procedimeinto;
                    detalle.ACTIVO = true;
                    detalle.TEST = test;

                    db.CS_DEFINICION_PROCEDIMIENTO_DET.Add(detalle);
                    db.SaveChanges();

                    status = true;
                    transaction.Commit();
                }
                catch (EntityException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    status = false;
                }

            }

            return new JsonResult
            {
                Data = new
                {
                    status = status
                }
            };
        }
        public partial class Valor
        {
            public string ID { get; set; }
            public string DESCRIPCION { get; set; }
        }
        [HttpPost]
        public JsonResult EliminarProcedimientoDetalleSolo(int procedimeintodet)
        {
            bool status = true;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //i.ID_EXAMEN = procedimiento.ARTICULO;
                    CS_DEFINICION_PROCEDIMIENTO_DET procedmiento = db.CS_DEFINICION_PROCEDIMIENTO_DET.Where(x => x.ID_DEF_DETALLE == procedimeintodet).FirstOrDefault();
                    procedmiento.ACTIVO = false;
                    db.SaveChanges();

                    status = true;
                    transaction.Commit();
                }
                catch (EntityException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    status = false;
                }

            }

            return new JsonResult
            {
                Data = new
                {
                    status = status
                }
            };
        }
        // GET: Procedimiento/Edit/5
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

            var parametros = db.CS_PARAMETROS.FirstOrDefault();
            ViewBag.TEST = new SelectList(db.CS_TEST.Where(x => x.ACTIVO == true), "TEST", "NOMBRE");
            ViewBag.ARTICULO = new SelectList(db.ARTICULO.Where(x => x.CLASIFICACION_1 == parametros.CLA_LAB).Select(x => new
            {
                x.ARTICULO1,
                DESCRIPCION = x.ARTICULO1 + "-" + x.DESCRIPCION
            }), "ARTICULO1", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);
            ViewBag.COLOR_TUBO = new SelectList(db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 27 && x.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.COLOR_TUBO);
            //ViewBag.PROCEDIMIENTO_DET = (from dt in db.CS_DEFINICION_PROCEDIMIENTO_DET
            //                             join t in db.CS_TEST on dt.TEST equals t.TEST
            //                             where dt.PROCEDIMIENTO == id && t.ACTIVO == true
            //                             select t
            //                            ).ToList();
            ViewBag.PROCEDIMIENTO_DET = (from dt in db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(t=>t.CS_TEST)                                        
                                         where dt.PROCEDIMIENTO == id && dt.ACTIVO == true
                                         select dt
                                  ).ToList();
            //var test = from dt in db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(d => d.CS_TEST) where dt.PROCEDIMIENTO == id && dt.ACTIVO == true select dt;
            //var test_det= test.Select(x => new
            //{
            //    TEST=x.TEST,
            //    ID_DEF_DETALLE=x.ID_DEF_DETALLE,
            //    NOMBRE = x.CS_TEST.NOMBRE,
            //    OBSERVACION = x.CS_TEST.OBSERVACION,
            //    UNIDAD_MEDIDA = x.CS_TEST.CS_CATALOGO_DETALLE.DESCRIPCION,
            //    VALOR = x.CS_TEST.VALOR
            //}).ToList();
            //ViewBag.PROCEDIMIENTO_DET = test_det;

            //ViewBag.PROCEDIMIENTO_DET = (from dt in db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(d => d.CS_TEST).Select(x => new {
            //    x.TEST,
            //    x.ID_DEF_DETALLE,
            //    NOMBRE = x.CS_TEST.NOMBRE,
            //    OBSERVACION = x.CS_TEST.OBSERVACION,
            //    UNIDAD_MEDIDA = x.CS_TEST.CS_CATALOGO_DETALLE.DESCRIPCION,
            //    VALOR = x.CS_TEST.VALOR
            //}).ToList();
            return View(cS_DEFINICION_PROCEDIMIENTO);
            //select(x => new
            //{
            //    x.TET,
            //    x.NOMBRE,
            //    x.OBSERVACION,
            //    x.UNIDAD_MEDIDA,
            //    x.CS_CATALOGO_DETALLE,
            //    x.VALOR,
            //    x.ID_DEF_DETALLE
            //})
        }

        // POST: Procedimiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_EXAMEN,ARTICULO,DESCRIPCION,AREA_SERVICIO")] CS_DEFINICION_PROCEDIMIENTO cS_DEFINICION_PROCEDIMIENTO)
        {
            if (ModelState.IsValid)
            {
                cS_DEFINICION_PROCEDIMIENTO.ACTIVO = true;
                db.Entry(cS_DEFINICION_PROCEDIMIENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ARTICULO = new SelectList(db.ARTICULO, "ARTICULO1", "PLANTILLA_SERIE", cS_DEFINICION_PROCEDIMIENTO.ARTICULO);
            ViewBag.AREA_SERVICIO = new SelectList(db.CS_CATALOGO_DETALLE, "ID_CAT_DETALLE", "DESCRIPCION", cS_DEFINICION_PROCEDIMIENTO.AREA_SERVICIO);
            return View(cS_DEFINICION_PROCEDIMIENTO);
        }

        // GET: Procedimiento/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Procedimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
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

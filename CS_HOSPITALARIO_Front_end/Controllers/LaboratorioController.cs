using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using CS_HOSPITALARIO_Front_end.Models;
using CS_HOSPITALARIO_Front_end.Reports.Laboratorio;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class LaboratorioController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        Utilerias util = new Utilerias();
        // GET: Laboratorio
        public ActionResult Index()
        {
            var cS_RESULTADOS_LAB_ENCABEZADO = db.CS_RESULTADOS_LAB_ENCABEZADO.Include(c => c.CS_LABORATORIO);
            return View(cS_RESULTADOS_LAB_ENCABEZADO.ToList());
        }
        //// GET: Laboratorio
        //public ActionResult ListaTrabajo()
        //{
        //    var laboratorioad = db.SpListadoExamenLaboratorioRecepcionado().ToList();
        //    return View(laboratorioad);
        //}

        // GET: Laboratorio
        public ActionResult ListaTrabajo()
        {
            Models.ViewModel.LaboratorioViewModel model = new Models.ViewModel.LaboratorioViewModel();

            model.desde = DateTime.Now.Date;
            model.hasta = DateTime.Now.Date;
            DateTime fechafin = Convert.ToDateTime(model.hasta).AddDays(1);
            var laboratorioad = db.SpListadoExamenLaboratorioRecepcionado().Where(x => x.FECHA_RECEPCION >= model.desde && x.FECHA_RECEPCION <= fechafin).ToList();
            model.ListadoExamenLaboratorioRecepcionado_Result = laboratorioad;
            return View(model);
        }
        [HttpPost]
        public ActionResult ListaTrabajo(DateTime desde, DateTime hasta)
        {

            hasta = hasta.AddDays(1);
            Models.ViewModel.LaboratorioViewModel model = new Models.ViewModel.LaboratorioViewModel();

            var laboratorioad = db.SpListadoExamenLaboratorioRecepcionado().Where(x => x.FECHA_RECEPCION >= desde && x.FECHA_RECEPCION <= hasta).ToList();
            model.ListadoExamenLaboratorioRecepcionado_Result = laboratorioad;
            //  return new JsonResult { Data = model };
            return View(model);
        }
        public ActionResult TomaMuestra()
        {
            var laboratorioad = db.SpListadoExamenLaboratorio().ToList();
            return View(laboratorioad);
        }
        public ActionResult RegistrarTomaMuestra(int id)
        {
            var laboratorioad = db.SpListadoExamenLaboratorio().Where(x => x.NUM_MUESTRA == id).FirstOrDefault();
            return View(laboratorioad);
        }
        public ActionResult RegistrarRecepcionMuestra()
        {
            // var laboratorioad = db.SpListadoExamenLaboratorio().Where(x => x.NUM_MUESTRA == id).FirstOrDefault();
            return View();
        }

        public ActionResult RecepcionMuestra()
        {
            var laboratorioad = db.SpListadoExamenLaboratorioRecepcionado().ToList();
            return View(laboratorioad);
        }
        public ActionResult RegistrarResultadoLaboratorio(int id)
        {

            var laboratorioad = db.SpListadoExamenLaboratorioRecepcionado().Where(x => x.NUM_MUESTRA == id).FirstOrDefault();
            //var T = db.Sp_Listado_Limite_Registrar_Resultado(laboratorioad.ID_EXAMEN, laboratorioad.SEX, laboratorioad.EDAD_AL_TOMAR_MUESTRA).FirstOrDefault();
            List<RegistrarResultadoLaboratorio> registrar = new List<RegistrarResultadoLaboratorio>();
            var regi = db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(t => t.CS_TEST).Where(x => x.PROCEDIMIENTO == laboratorioad.ID_EXAMEN && x.ACTIVO == true).
                                         Select(x => new
                                         {
                                             TEST = x.CS_TEST.TEST,
                                             NOMBRE = x.CS_TEST.NOMBRE,
                                             VALOR = x.CS_TEST.VALOR,
                                             DESCRIPCION = x.CS_TEST.CS_CATALOGO_DETALLE.DESCRIPCION
                                         }).ToList();

            foreach (var item in regi)
            {
                RegistrarResultadoLaboratorio r = new RegistrarResultadoLaboratorio();
                Sp_Listado_Limite_Registrar_Resultado_Result LIMI = new Sp_Listado_Limite_Registrar_Resultado_Result();

                r.TEST = item.TEST;
                r.NOMBRE = item.NOMBRE;
                r.VALOR = item.VALOR;
                r.LIMITE = db.Sp_Listado_Limite_Registrar_Resultado(item.TEST, laboratorioad.SEX, laboratorioad.EDAD_AL_TOMAR_MUESTRA).FirstOrDefault();
                r.DESCRIPCION = item.DESCRIPCION;
                registrar.Add(r);
            }

            ViewBag.PROCEDIMIENTO_DET = registrar;
            return View(laboratorioad);
        }
        public ActionResult EditarResultadoLaboratorio(int id)
        {

            var laboratorioad = db.SpListadoExamenLaboratorioRegistrado().Where(x => x.NUM_MUESTRA == id).FirstOrDefault();
            //var T = db.Sp_Listado_Limite_Registrar_Resultado(laboratorioad.ID_EXAMEN, laboratorioad.SEX, laboratorioad.EDAD_AL_TOMAR_MUESTRA).FirstOrDefault();
            List<RegistrarResultadoLaboratorio> registrar = new List<RegistrarResultadoLaboratorio>();
            var regi = db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(t => t.CS_TEST).Where(x => x.PROCEDIMIENTO == laboratorioad.ID_EXAMEN && x.ACTIVO == true).
                                         Select(x => new
                                         {
                                             TEST = x.CS_TEST.TEST,
                                             NOMBRE = x.CS_TEST.NOMBRE,
                                             VALOR = x.CS_TEST.VALOR,
                                             DESCRIPCION = x.CS_TEST.CS_CATALOGO_DETALLE.DESCRIPCION
                                         }).ToList();

            foreach (var item in regi)
            {
                RegistrarResultadoLaboratorio r = new RegistrarResultadoLaboratorio();
                Sp_Listado_Limite_Registrar_Resultado_Result LIMI = new Sp_Listado_Limite_Registrar_Resultado_Result();

                r.TEST = item.TEST;
                r.NOMBRE = item.NOMBRE;
                r.VALOR = item.VALOR;
                r.LIMITE = db.Sp_Listado_Limite_Registrar_Resultado(item.TEST, laboratorioad.SEX, laboratorioad.EDAD_AL_TOMAR_MUESTRA).FirstOrDefault();
                r.DESCRIPCION = item.DESCRIPCION;
                r.RESULTADO = db.CS_RESULTADOS_LAB_DETALLE.Where(x => x.RESULTADO_LAB_ID == laboratorioad.ID_RESULTADO_LAB).FirstOrDefault().RESULTADO;
                registrar.Add(r);
            }

            ViewBag.PROCEDIMIENTO_DET = registrar;
            return View(laboratorioad);
        }
        public ActionResult FormartoEmprimirEtiqueta(int muestra)
        {
            rptEtiquetaExamenLaboratorio report = new rptEtiquetaExamenLaboratorio();
            report.RequestParameters = false;
            report.Parameters[0].Value = muestra;
            report.Parameters[0].Visible = false;
            return View(report);
        }
        [HttpPost]
        public JsonResult ListadoTestDetalleExamen(int ID_EXAMEN)
        {
            var status = false;
            List<SP_Listado_Examen_Test_Detalle_Result> testbd = new List<SP_Listado_Examen_Test_Detalle_Result>();
            try
            {
                testbd = db.SP_Listado_Examen_Test_Detalle(ID_EXAMEN).ToList();

            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, test = testbd } };
        }
        [HttpPost]
        public JsonResult RegistrarTomaMuestra(int NUM_MUESTRA, int ED)
        {
            var status = false;
            try
            {
                var laboratoriodb = db.CS_LABORATORIO.Find(NUM_MUESTRA);
                laboratoriodb.USUARIO_TOMA_MUESTRA = util.GetUser();
                laboratoriodb.FECHA_TOMA_MUESTRA = DateTime.Now;
                laboratoriodb.EDAD_AL_TOMAR_MUESTRA = ED;
                laboratoriodb.FECHA_TOMA_MUESTRA = DateTime.Now;
                laboratoriodb.ESTADO = "T";
                status = db.SaveChanges() > 0;

            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult RegistrarRecepcionMuestra(int NUM_MUESTRA)
        {
            var status = false;
            try
            {
                var laboratoriodb = db.CS_LABORATORIO.Find(NUM_MUESTRA);
                laboratoriodb.FECHA_RECEPCION = DateTime.Now;
                laboratoriodb.USUARIO_RECEPCION = util.GetUser();
                laboratoriodb.ESTADO = "R";
                status = db.SaveChanges() > 0;

            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult BuscarTomaMuestra(string NUM_MUESTRA)
        {
            var status = false;
            SpListadoExamenLaboratorioRecepcion_Result laboratoriodb = new SpListadoExamenLaboratorioRecepcion_Result();
            string mensaje = string.Empty;
            try
            {
                int cod = Convert.ToInt32(NUM_MUESTRA);
                laboratoriodb = db.SpListadoExamenLaboratorioRecepcion().Where(x => x.NUM_MUESTRA == cod).FirstOrDefault();
                if (laboratoriodb != null)
                {
                    status = true;
                    mensaje = "La muestra se encontro con exito";
                }
                else
                {
                    status = false;
                    mensaje = "No se encontro la muestra";
                }

            }
            catch
            {
                status = false;
                mensaje = "No se encontro la muestra";
            }
            return new JsonResult { Data = new { status = status, laboratoriodb = laboratoriodb, mensaje = mensaje } };
        }
        public JsonResult GuardarExamen(CS_RESULTADOS_LAB_ENCABEZADO examen_encabezado, List<CS_RESULTADOS_LAB_DETALLE> examenes, int id_diagnostico, string estado, string observacion)
        {
            var status = true;
            try
            {
                if (examen_encabezado != null && examenes != null && id_diagnostico != 0 && estado != null && observacion != null)
                {
                    using (HospitalarioBD dc = new HospitalarioBD())
                    {
                        CS_LABORATORIO laboratoriodb = dc.CS_LABORATORIO.Find(examen_encabezado.NUM_MUESTRA);
                        laboratoriodb.DIAGNOSTICO_ID = id_diagnostico;
                        laboratoriodb.FECHA_RESULTADO = DateTime.Now;
                        laboratoriodb.ESTADO = estado.ToString();
                        laboratoriodb.OBSERVACIONES = observacion;

                        CS_RESULTADOS_LAB_ENCABEZADO labencabezadodb = new CS_RESULTADOS_LAB_ENCABEZADO
                        {
                            NUM_MUESTRA = examen_encabezado.NUM_MUESTRA,
                            EXAMEN_ID = examen_encabezado.EXAMEN_ID,
                            CLIENTE_ID = examen_encabezado.CLIENTE_ID,
                            FECHA_RESULTADO = DateTime.Now,
                            USUARIO_REGISTRO = util.GetUser(),
                            FECHA_REGISTRO = DateTime.Now,
                            ESTADO = estado.ToString(),
                            IMPRESO = false,
                            ENV_POR_CORREO = false
                        };
                        foreach (var item in examenes)
                        {
                            var testdetalle = (from t in db.CS_TEST where t.TEST == item.ID_TEST_DETALLE select t).FirstOrDefault();
                            CS_RESULTADOS_LAB_DETALLE detalledb = new CS_RESULTADOS_LAB_DETALLE
                            {
                                ID_TEST_DETALLE = item.ID_TEST_DETALLE,
                                RESULTADO = item.RESULTADO,
                                OBSERVACIONES = testdetalle.OBSERVACION
                            };
                            labencabezadodb.CS_RESULTADOS_LAB_DETALLE.Add(detalledb);
                        }
                        dc.CS_RESULTADOS_LAB_ENCABEZADO.Add(labencabezadodb);
                        status = dc.SaveChanges() > 0;
                    }

                }
                else
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        // GET: Laboratorio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO = db.CS_RESULTADOS_LAB_ENCABEZADO.Find(id);
            if (cS_RESULTADOS_LAB_ENCABEZADO == null)
            {
                return HttpNotFound();
            }
            return View(cS_RESULTADOS_LAB_ENCABEZADO);
        }

        // GET: Laboratorio/Create
        public ActionResult Create()
        {
            ViewBag.NUM_MUESTRA = new SelectList(db.CS_LABORATORIO, "NUM_MUESTRA", "CLIENTE_ID");
            return View();
        }

        // POST: Laboratorio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_RESULTADO_LAB,NUM_MUESTRA,EXAMEN_ID,CLIENTE_ID,FECHA_RESULTADO,USUARIO_REGISTRO,FECHA_REGISTRO,IMPRESO,ENV_POR_CORREO")] CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO)
        {
            if (ModelState.IsValid)
            {
                db.CS_RESULTADOS_LAB_ENCABEZADO.Add(cS_RESULTADOS_LAB_ENCABEZADO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NUM_MUESTRA = new SelectList(db.CS_LABORATORIO, "NUM_MUESTRA", "CLIENTE_ID", cS_RESULTADOS_LAB_ENCABEZADO.NUM_MUESTRA);
            return View(cS_RESULTADOS_LAB_ENCABEZADO);
        }

        // GET: Laboratorio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO = db.CS_RESULTADOS_LAB_ENCABEZADO.Find(id);
            if (cS_RESULTADOS_LAB_ENCABEZADO == null)
            {
                return HttpNotFound();
            }
            ViewBag.NUM_MUESTRA = new SelectList(db.CS_LABORATORIO, "NUM_MUESTRA", "CLIENTE_ID", cS_RESULTADOS_LAB_ENCABEZADO.NUM_MUESTRA);
            return View(cS_RESULTADOS_LAB_ENCABEZADO);
        }

        // POST: Laboratorio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_RESULTADO_LAB,NUM_MUESTRA,EXAMEN_ID,CLIENTE_ID,FECHA_RESULTADO,USUARIO_REGISTRO,FECHA_REGISTRO,IMPRESO,ENV_POR_CORREO")] CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_RESULTADOS_LAB_ENCABEZADO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NUM_MUESTRA = new SelectList(db.CS_LABORATORIO, "NUM_MUESTRA", "CLIENTE_ID", cS_RESULTADOS_LAB_ENCABEZADO.NUM_MUESTRA);
            return View(cS_RESULTADOS_LAB_ENCABEZADO);
        }

        // GET: Laboratorio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO = db.CS_RESULTADOS_LAB_ENCABEZADO.Find(id);
            if (cS_RESULTADOS_LAB_ENCABEZADO == null)
            {
                return HttpNotFound();
            }
            return View(cS_RESULTADOS_LAB_ENCABEZADO);
        }

        // POST: Laboratorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_RESULTADOS_LAB_ENCABEZADO cS_RESULTADOS_LAB_ENCABEZADO = db.CS_RESULTADOS_LAB_ENCABEZADO.Find(id);
            db.CS_RESULTADOS_LAB_ENCABEZADO.Remove(cS_RESULTADOS_LAB_ENCABEZADO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult rptExamenLab(string id)
        {
            rptResultadoLab report = new rptResultadoLab();
            report.Parameters[0].Value = id;
            report.Parameters[0].Visible = false;

            return View(report);
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

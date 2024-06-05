using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO_Front_end.Models;
using CS_HOSPITALARIO_Front_end.Models.ViewModel;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Globalization;
using CS_HOSPITALARIO.Models.CustomMethods;
using CS_HOSPITALARIO_Front_end.Reports.Imagenologia;
using System.Data.Entity.Core;
using System.Threading.Tasks;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class ImagenologiaController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private ImagenologiaBusinessLogic dbb = new ImagenologiaBusinessLogic();
        private Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1, 2, 3, 4, 5 };
        public ActionResult Plantillas_Listar()
        {
            var  list_plan = from t in db.CS_PLANTILLA join a in db.ARTICULO on t.ARTICULO equals a.ARTICULO1
                              select new PlantillaViewModel
                               { ID_PLANTILLA=t.ID_PLANTILLA, ARTICULO=t.ARTICULO, DESCRIPCION_ARTICULO=a.DESCRIPCION, DOCTOR=t.CS_PERSONAL.PERSONAL_ID, NOMBRE_DOCTOR=t.CS_PERSONAL.NOMBRES , DESCRIPCION=t.DESCRIPCION};
            return View(list_plan.ToList());
        }


        public ActionResult Plantillas_Nueva()
        {
            var ESTUDIO_LIST = (from a in db.CS_DEFINICION_PROC_IMAGEN
                          join b in db.ARTICULO on a.ARTICULO equals b.ARTICULO1
                         
                          select new {a.ARTICULO, b.DESCRIPCION }).ToList();

            ViewBag.ESTUDIO = new SelectList(ESTUDIO_LIST, "ARTICULO", "DESCRIPCION");
            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES");
            return View();

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Plantillas_Nueva([Bind(Include = "ID_PLANTILLA, ARTICULO,DOCTOR_ID,COMENTARIO ,ACTIVO, DESCRIPCION")] CS_PLANTILLA pLANTILLA, string ESTUDIO, string DOCTOR, string COMENTARIO, string DESCRIPCION)
        {
            if (ModelState.IsValid)
            {
                pLANTILLA.ACTIVO = true;
                pLANTILLA.ARTICULO = ESTUDIO;
                pLANTILLA.DOCTOR_ID = int.Parse(DOCTOR);
                pLANTILLA.COMENTARIO = COMENTARIO;
                db.CS_PLANTILLA.Add(pLANTILLA);
                db.SaveChanges();
                return RedirectToAction("Plantillas_Listar");
            }
            var ESTUDIO_LIST = (from a in db.CS_DEFINICION_PROC_IMAGEN
                                join b in db.ARTICULO on a.ARTICULO equals b.ARTICULO1

                                select new { a.ARTICULO, b.DESCRIPCION }).ToList();

            ViewBag.ESTUDIO = new SelectList(ESTUDIO_LIST, "ARTICULO", "DESCRIPCION");
            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES");
            return View(pLANTILLA);

        }

        public ActionResult Plantillas_Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_PLANTILLA pLANTILLA = db.CS_PLANTILLA.Find(id);
            if (pLANTILLA == null)
            {
                return HttpNotFound();
            }
            var ESTUDIO_LIST = (from a in db.CS_DEFINICION_PROC_IMAGEN
                                join b in db.ARTICULO on a.ARTICULO equals b.ARTICULO1

                                select new { a.ARTICULO, b.DESCRIPCION }).ToList();



            ViewBag.ESTUDIO = new SelectList(ESTUDIO_LIST, "ARTICULO", "DESCRIPCION", pLANTILLA.ARTICULO);
            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", pLANTILLA.DOCTOR_ID);
            ViewBag.COMENTARIO = pLANTILLA.COMENTARIO;
            return View(pLANTILLA);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Plantillas_Editar([Bind(Include = "ID_PLANTILLA, ESTUDIO,DOCTOR,COMENTARIO, DESCRIPCION, ACTIVO")] CS_PLANTILLA pLANTILLA, String ESTUDIO, string DOCTOR, string COMENTARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pLANTILLA).State = EntityState.Modified;
                pLANTILLA.COMENTARIO = COMENTARIO;
                pLANTILLA.ARTICULO = ESTUDIO;
                pLANTILLA.DOCTOR_ID = int.Parse(DOCTOR);
                db.SaveChanges();
                return RedirectToAction("Plantillas_Listar");
            }

            var ESTUDIO_LIST = (from a in db.CS_DEFINICION_PROC_IMAGEN
                                join b in db.ARTICULO on a.ARTICULO equals b.ARTICULO1

                                select new { a.ARTICULO, b.DESCRIPCION }).ToList();


            ViewBag.ESTUDIO = new SelectList(ESTUDIO_LIST, "ARTICULO", "DESCRIPCION", pLANTILLA.ARTICULO);
            ViewBag.DOCTOR = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES", pLANTILLA.DOCTOR_ID);
            ViewBag.COMENTARIO = pLANTILLA.COMENTARIO;
            return View(pLANTILLA);
        }

        public PartialViewResult Top10ImagenPendientes()
        {
            List<CS_IMAGENOLOGIA> top10Trans = db.CS_IMAGENOLOGIA.Where(x => x.STATUS == 1096 || x.STATUS == 1097).OrderByDescending(item => item.FECHA_REGISTRO).Take(10).ToList();

            return PartialView("_Top10ImagenPendientes", top10Trans);
        }

        public PartialViewResult Top10ImagenListos()
        {
            List<CS_IMAGENOLOGIA> top10TransList = db.CS_IMAGENOLOGIA.Where(x => x.STATUS == 1098).OrderByDescending(item => item.FECHA_REGISTRO).Take(10).ToList();

            return PartialView("_Top10ImagenListos", top10TransList);
        }


        public ActionResult Index()
        {
            ListaImagenViewModel model = new ListaImagenViewModel();
            model.DESDE = DateTime.Now.Date;
            model.HASTA = DateTime.Now.Date;
            return View(model);
        }

        [HttpPost]
        public JsonResult GetListaTrabajo(DateTime? desde, DateTime? hasta)
        {
            DateTime fechafinal =  Convert.ToDateTime(hasta).AddDays(1);

            var Lista = (from p in db.CS_IMAGENOLOGIA.Where(x => x.FECHA_REGISTRO >= desde && x.FECHA_REGISTRO <= fechafinal )
                         join t in db.CS_DEFINICION_PROC_IMAGEN on p.ARTICULO equals t.ARTICULO
                         join x in db.CS_PACIENTES on p.CLIENTE_ID equals x.CLIENTE_ID
                         join s in db.CS_CATALOGO_DETALLE on p.STATUS equals s.ID_CAT_DETALLE
                         where !(from s in db.CS_RESULTADO_IMAGENOLOGIA select s.ID_RESULTADO_IMAG).Contains(p.ID_IMAGENOLOGIA)
                         select new
                         {
                             ID = p.ID_IMAGENOLOGIA,
                             FECHA_REGISTRO = p.FECHA_REGISTRO.Day + "/" + p.FECHA_REGISTRO.Month + "/" + p.FECHA_REGISTRO.Year +" "+p.FECHA_REGISTRO.Hour+":"+p.FECHA_REGISTRO.Minute ,
                             ESTADO = s.DESCRIPCION,
                             PRIORIDAD = p.STAT == true ? "Stat" : "Rutina",
                             PACIENTE = p.CS_ADMISION.CLIENTE_ID,
                             ADMISION = p.ID_ADMISION,
                             NOMBRE = x.NOMBRES,
                             APELLIDO = x.APELLIDOS,
                             PROCEDIMIENTO = t.ARTICULO1.DESCRIPCION,
                             SEXO = p.CS_CATALOGO_DETALLE.DESCRIPCION,
                             EDAD = p.EDAD,
                             PEDIDO=p.PEDIDO

                         }).ToList();



            return new JsonResult { Data = new { Lista } };
        }

        public ActionResult Historial_Imagen()
        {
            ListaImagenViewModel model = new ListaImagenViewModel();
            model.DESDE = DateTime.Now.Date;
            model.HASTA = DateTime.Now.Date;
            return View(model);
        }

        [HttpPost]
        public JsonResult GetListaTrabajo_Hist(DateTime? desde, DateTime? hasta)
        {
            DateTime fechafinal = Convert.ToDateTime(hasta).AddDays(1);

            var Lista = (from p in db.CS_IMAGENOLOGIA.Where(x => x.FECHA_REGISTRO >= desde && x.FECHA_REGISTRO <= fechafinal)
                         join t in db.CS_DEFINICION_PROC_IMAGEN on p.ARTICULO equals t.ARTICULO
                         join x in db.CS_PACIENTES on p.CLIENTE_ID equals x.CLIENTE_ID
                           join s in db.CS_CATALOGO_DETALLE on p.STATUS equals s.ID_CAT_DETALLE
                           //where !(from s in db.CS_RESULTADO_IMAGENOLOGIA select s.ID_RESULTADO_IMAG).Contains(p.ID_IMAGENOLOGIA)
                           select new
                           {
                               ID = p.ID_IMAGENOLOGIA,
                               FECHA_REGISTRO = p.FECHA_REGISTRO.Day + "/" + p.FECHA_REGISTRO.Month + "/" + p.FECHA_REGISTRO.Year,
                               ESTADO = s.DESCRIPCION,
                               PRIORIDAD = p.STAT == true ? "Stat" : "Rutina",
                               PACIENTE = p.CS_ADMISION.CLIENTE_ID,
                               ADMISION = p.ID_ADMISION,
                               NOMBRE = x.NOMBRES,
                               APELLIDO = x.APELLIDOS,
                               PROCEDIMIENTO =t.ARTICULO1.DESCRIPCION,
                               SEXO = p.CS_CATALOGO_DETALLE.DESCRIPCION,
                               EDAD = p.EDAD

                           }).ToList();




            return new JsonResult { Data = new { Lista } };
        }

        public ActionResult Lista_Trabajo_Imagen()
        {
            return View(db.CS_IMAGENOLOGIA.ToList());
        }


        public async Task<ActionResult> Transcripcion_Nueva(int id)
        {
            ResultadoImagenViewModel model = new ResultadoImagenViewModel();
            string usuario = User.Identity.GetUserName();
            var img = (from i in db.CS_IMAGENOLOGIA where i.ID_IMAGENOLOGIA == id select i).FirstOrDefault();
            var admin = (from a in db.CS_ADMISION where a.ID_ADMISION == img.ID_ADMISION select a).FirstOrDefault();
            var medico = (from m in db.CS_MEDICOS where m.ID_MEDICO == admin.DOCTOR_ID select m).FirstOrDefault();
            var articulo_id = (from p in db.CS_DEFINICION_PROC_IMAGEN where p.ARTICULO == img.ARTICULO select p).FirstOrDefault();

            var articulo = (from p in db.ARTICULO where p.ARTICULO1 == articulo_id.ARTICULO select p).FirstOrDefault();
            var paciente = (from p in db.CS_PACIENTES where p.CLIENTE_ID == img.CLIENTE_ID select p).FirstOrDefault();
            var cliente = (from c in db.CLIENTE where c.CLIENTE1 == paciente.CLIENTE_ID select c).FirstOrDefault();
            var estado = (from p in db.CS_CATALOGO_DETALLE where p.ID_CAT_DETALLE == img.STATUS select p).FirstOrDefault();
            var area = (from a in db.CS_CATALOGO_DETALLE where a.ID_CAT_DETALLE == admin.AREA_SERVICIO_ID select a).FirstOrDefault();
           // var sexo = (from p in db.CS_CATALOGO_DETALLE where p.ID_CAT_DETALLE == img.SEXO select p).FirstOrDefault();

            model.ADMISION_ID = img.ID_ADMISION;
            model.FECHA_REGISTRO = ImagenologiaBusinessLogic.validateDatetime(admin.FECHA_REGISTRO.ToString());
            model.FECHA_ALTA = admin.FECHA_ALTA;
            model.FECHA_REGISTRO_PROC = img.FECHA_REGISTRO;
            model.IMAGENOLOGIA_ID = img.ID_IMAGENOLOGIA;
            model.PROCEDIMIENTO = img.ARTICULO;
            model.DESC_PROCEDIMIENTO = articulo.DESCRIPCION;
            model.STATUS = estado.DESCRIPCION;
            model.STAT = img.STAT == true ? "Stat" : "Rutina";
            model.CLIENTE_ID = cliente.CLIENTE1;
            model.NOMBRE_PACIENTE = cliente.NOMBRE;
            model.PACIENTE_ID = img.CLIENTE_ID;
            model.SEXO = img.SEXO;
            model.EDAD = img.EDAD;
            model.FECHA_NACIMIENTO = paciente.FECHA_NACIMIENTO;
            model.DOCTOR_ID = medico.ID_MEDICO;
            model.NOMBRE_DOCTOR = medico.NOMBRES;
            model.AREA_SERVICIO_ID = admin.AREA_SERVICIO_ID;
            model.DESC_AREA_SERVICIO = area.DESCRIPCION;
            model.PEDIDO = admin.PEDIDO;

            var MED = (from p in db.CS_MEDICOS.Where(x => x.USUARIO == usuario) select p).FirstOrDefault();
            //         var plan = (from s in db.CS_PLANTILLA.Where(x => x.DOCTOR_ID == MED.ID_MEDICO) select s).ToList();
            //ViewBag.PLANTILLA_ID = new SelectList(plan, "ID_PLANTILLA", "EXAMEN_IMAG_ID");
            var RADI = await db.CS_PERSONAL.Where(p => p.ACTIVO == true).Select(x => new
            {
                PERSONAL_ID = x.PERSONAL_ID,
                NOMBRES = x.NOMBRES
            }).ToListAsync();
            var RADIOLOGOS = new SelectList(RADI, "PERSONAL_ID", "NOMBRES");
            ViewBag.RADIOLOGO = RADIOLOGOS;

            return View(model);

        }

        public JsonResult GetPlantillas(int medico)
        {
            List<getDetailsPlantilla> plantilla = new List<getDetailsPlantilla>();

            plantilla = (from p in db.CS_PLANTILLA
                         join a in db.CS_DEFINICION_PROC_IMAGEN on p.ARTICULO equals a.ARTICULO
                         join r in db.ARTICULO on a.ARTICULO equals r.ARTICULO1
                         where p.DOCTOR_ID == medico
                         select new getDetailsPlantilla { Text = r.DESCRIPCION, Value = p.ID_PLANTILLA }).ToList();


            return Json(new SelectList(plantilla, "Value", "Text"));
        }

        public JsonResult GetDetallePlantillas(int plantilla)
        {
            string messageerror = "", detalle = "";
            bool status = false;

            var details = (from d in db.CS_PLANTILLA where d.ID_PLANTILLA == plantilla select d).FirstOrDefault();
            detalle = details.COMENTARIO;
            status = true;

            return new JsonResult { Data = new { status = status, messageerror = messageerror, detalle = detalle } };
        }

        [HttpPost]
        public ActionResult Transcripcion_Nueva(ResultadoImagenViewModel result)
        {
            int resultado = 0;
            bool status = false;
            string messageerror = "";

            CS_IMAGENOLOGIA img = db.CS_IMAGENOLOGIA.Find(result.IMAGENOLOGIA_ID);

            CS_RESULTADO_IMAGENOLOGIA re = new CS_RESULTADO_IMAGENOLOGIA { CLIENTE_ID = img.CLIENTE_ID, IMAGENOLOGIA_ID = img.ID_IMAGENOLOGIA, PLANTILLA_ID = result.PLANTILLA_ID, FECHA_RESULTADO = System.DateTime.Now, USUARIO_RESULTADO = util.GetUser(), FECHA_CREACION = System.DateTime.Now, USUARIO_CREACION = util.GetUser(), ESTADO = true, RADIOLOGO = result.RADIOLOGO_ID };
            db.CS_RESULTADO_IMAGENOLOGIA.Add(re);
            db.SaveChanges();
            resultado = re.ID_RESULTADO_IMAG;

            if (resultado > 0)
            {
                CS_RESULT_IMAGENOLOGIA_DETALLE det = new CS_RESULT_IMAGENOLOGIA_DETALLE { RESULTADO_IMAG_ID = resultado, DIAGNOSTICO = result.TRANSCRIPCION, FECHA_CREACION = System.DateTime.Now, USUARIO_CREACION = util.GetUser() };
                db.CS_RESULT_IMAGENOLOGIA_DETALLE.Add(det);
                db.SaveChanges();

                db.Entry(img).State = EntityState.Modified;
                img.STATUS = 1098;
                db.SaveChanges();

                status = true;

            }
            ViewBag.RADIOLOGO = new SelectList(db.CS_PERSONAL, "PERSONAL_ID", "NOMBRES");
            return new JsonResult { Data = new { status = status, messageerror = messageerror } };

        }

        [HttpPost]
        public ActionResult Lectura(int id)
        {
            bool status = false;
            string messageerror = "";

            CS_IMAGENOLOGIA img = db.CS_IMAGENOLOGIA.Find(id);

            if (img.STATUS == 1096)
            {
                img.LECTURA = "S";
                img.FECHA_LECTURA = System.DateTime.Now;
                img.USUARIO_LECTURA = User.Identity.GetUserName();
                img.STATUS = 1097;
                db.Entry(img).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    status = true;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                            messageerror = raise.ToString();
                        }
                    }
                    throw raise;

                }

            }
            else
            {
                status = false;
                messageerror = "Solo los estudios con estado de Solicitado, pueden pasar a Lectura.";
            }

            return new JsonResult { Data = new { status = status, messageerror = messageerror } };
        }

        [HttpPost]
        public ActionResult Transcripcion(int imagen)
        {
            bool status = false;
            string messageerror = "";
            string pantalla = "";

            CS_IMAGENOLOGIA img = db.CS_IMAGENOLOGIA.Find(imagen);
            if (img.STATUS == 1099)
            {
                status = false;
                messageerror = "La orden esta anulada.";
            }
            else if (img.STATUS == 1098)
            {
                //mando a llamar el reporte de resultado
                status = true;
                pantalla = "REPORTE";

            }
            else if (img.STATUS == 1096)
            {
                status = false;
                messageerror = "La orden debe estar en lectura, para ser transcripta";
            }
            else if (img.STATUS == 1097)
            {
                //aca vamos a la nueva transcripcion
                status = true;
                pantalla = "NUEVO";

            }


            return new JsonResult { Data = new { status = status, messageerror = messageerror, pantalla = pantalla } };
        }


        [HttpPost]
        public ActionResult Anulacion(int imagen)
        {
            bool status = false;
            string messageerror = "";

            CS_IMAGENOLOGIA img = db.CS_IMAGENOLOGIA.Find(imagen);
            if (img.STATUS == 1099)
            {
                status = false;
                messageerror = "La orden ya esta anulada.";
            }
            else if (img.STATUS == 1098)
            {
                status = false;
                messageerror = "La orden ya tiene una transcripción asociada";

            }
            else if (img.STATUS == 1096 || img.STATUS == 1097)
            {
                img.ANULADO = "S";
                img.FECHA_ANULACION = System.DateTime.Now;
                img.USUARIO_ANULACION = util.GetUsuario();
                img.STATUS = 1099;
                db.Entry(img).State = EntityState.Modified;

                var pedidolinea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == img.PEDIDO && x.PEDIDO_LINEA1 == img.PEDIDO_LINEA && x.ARTICULO == img.ARTICULO).FirstOrDefault();
                if (pedidolinea != null)
                {
                    db.Entry(pedidolinea).State = EntityState.Deleted;
                }
               

                try
                {
                    db.SaveChanges();
                    status = true;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                            messageerror = raise.ToString();
                        }
                    }
                    throw raise;

                }
                status = true;


            }
            return new JsonResult { Data = new { status = status, messageerror = messageerror } };
        }

        public ActionResult rptEstudioImagen(int? img)
        {
            rptResultadoImagenologia report = new rptResultadoImagenologia();
            report.Parameters[0].Value = img;
            report.Parameters[0].Visible = false;

            return View(report);
        }




        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_IMAGENOLOGIA cS_IMAGENOLOGIA = db.CS_IMAGENOLOGIA.Find(id);
            if (cS_IMAGENOLOGIA == null)
            {
                return HttpNotFound();
            }
            return View(cS_IMAGENOLOGIA);
        }

        public ActionResult Definicion_Imagen()
        {
            var examen = (from e in db.SpListadoProcImagen() select new { ARTICULO = e.ARTICULO, DESCRIPCION = e.PROCEDIMIENTO }).ToList();

            ViewBag.ID_EXAMEN = new SelectList(examen, "ARTICULO", "DESCRIPCION");
            
            return View();
        }

        [HttpPost]
        public ActionResult CrearDefinicion(CS_DEFINICION_PROC_IMAGEN definicion)
        {
            var status = true;
            string sexo = string.Empty;

            definicion.FECHA_REGISTRO = DateTime.Now;
            definicion.USUARIO_REGISTRO = util.GetUser();
            definicion.ACTIVO = true;
            db.CS_DEFINICION_PROC_IMAGEN.Add(definicion);

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    try
                    {
                        db.SaveChanges();
                        status = true;
                       
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        status = false;
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }
                    transaction.Commit();
                   
                }
                catch (EntityException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error occurred.");
                    status = false;
                }
            }
                    
            return new JsonResult { Data = new { status = status} };
        }

        public ActionResult Definicion_Img_Listar()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var imagen = db.CS_DEFINICION_PROC_IMAGEN.ToList().OrderByDescending(a => a.DEF_IMG).ToList();
                return View(imagen);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }


     
        public ActionResult Definicion_ImagenEditar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_DEFINICION_PROC_IMAGEN cS_DEFINICION_PROC_IMAGEN = db.CS_DEFINICION_PROC_IMAGEN.Find(id);
            if (cS_DEFINICION_PROC_IMAGEN == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.ARTICULO = new SelectList(db.ARTICULO.Where(X =>X.ARTICULO1== cS_DEFINICION_PROC_IMAGEN.ARTICULO), "ARTICULO1", "DESCRIPCION", cS_DEFINICION_PROC_IMAGEN.ARTICULO);
            return View(cS_DEFINICION_PROC_IMAGEN);
        }

     
        [HttpPost]
   
        public ActionResult Definicion_ImagenEditar(CS_DEFINICION_PROC_IMAGEN definicion)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                db.Entry(definicion).State = EntityState.Modified;
                definicion.FECHA_MODIFICACION = DateTime.Now;
                db.SaveChanges();
                status = true;
               // return RedirectToAction("Definicion_Img_Listar");
            }


            ViewBag.ARTICULO = new SelectList(db.ARTICULO.Where(X => X.ARTICULO1 == definicion.ARTICULO), "ARTICULO1", "DESCRIPCION", definicion.ARTICULO);
            return new JsonResult { Data = new { status = status } };
            //return View(definicion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult DatosImagenParcial(int id)
        {
            CS_IMAGENOLOGIA img = db.CS_IMAGENOLOGIA.Where(i => i.ID_IMAGENOLOGIA == id).FirstOrDefault();

            return PartialView("_datoImagen", img);
        }

    }
    class getDetailsPlantilla
    {
        public int Value { get; set; }
        public string Text { get; set; }


    }
}

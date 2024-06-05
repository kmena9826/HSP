using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Data.Entity;
using CS_HOSPITALARIO.Models.CustomMethods;
using System.Collections.Generic;
using CS_HOSPITALARIO.Models;

namespace CS_HOSPITALARIO.Controllers
{
    [AutorizeController]
    public class AdmisionController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();

        [HttpGet]
        public ActionResult Admisiones()
        {
            return View(db.CS_ADMISION.Where(x => x.ACTIVO == true).OrderBy(x=>x.ID_ADMISION).ToList());

        }

        [HttpGet]
        public ActionResult NuevaAdmision()
        {
            ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "CLIENTE_ID", "NOMBRE_COMPLETO");

            ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");

            ViewBag.Title = "Nueva Admisión";
            return View("NuevoEditarAdmision");
        }

        [HttpPost]
        public ActionResult NuevaAdmision([Bind(Include = "CLIENTE_ID, PRIORIDAD_ID, CAUSA_ADMISION_ID, TIPO_INGRESO_ID, AREA_SERVICIO_ID, PEDIDO")] CS_ADMISION admision)
        {
            if (ModelState.IsValid)
            {
                admision.FECHA_RECEPCION = DateTime.Now;
                admision.ATENDIDO = false;
                admision.SIGNOS_VITALES = false;
                admision.FECHA_REGISTRO = DateTime.Now;
                admision.USUARIO_REGISTRO = util.GetUser();
                admision.ACTIVO = true;

                db.CS_ADMISION.Add(admision);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

                return RedirectToAction("Admisiones");
            }

            return View(admision);
        }

        [HttpGet]
        public ActionResult EditarAdmision(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Admisiones");
            }
            CS_ADMISION admision = db.CS_ADMISION.Find(id);
            if (admision == null)
            {
                return HttpNotFound();
            }

            //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "CLIENTE_ID", "NOMBRE_COMPLETO", admision.CLIENTE_ID);

            ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10), "ID_CAT_DETALLE", "DESCRIPCION", admision.PRIORIDAD_ID);
            ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.AREA_SERVICIO_ID);
            ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_INGRESO_ID);
            ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.CAUSA_ADMISION_ID);

            ViewBag.Title = "Editar Admisión";
            ViewBag.Accion = "Editar";
            return View("NuevoEditarAdmision", admision);
        }

        [HttpPost]
        public ActionResult EditarAdmision([Bind(Include = "ID_ADMISION, CLIENTE_ID, PRIORIDAD_ID, CAUSA_ADMISION_ID, TIPO_INGRESO_ID, AREA_SERVICIO_ID, PEDIDO")] CS_ADMISION admision)
        {
            if (ModelState.IsValid)
            {
                CS_ADMISION admi = db.CS_ADMISION.Find(admision.ID_ADMISION);

                //admi.CLIENTE_ID = admision.CLIENTE_ID;
                admi.PRIORIDAD_ID = admision.PRIORIDAD_ID;
                admi.CAUSA_ADMISION_ID = admision.CAUSA_ADMISION_ID;
                admi.TIPO_INGRESO_ID = admision.TIPO_INGRESO_ID;
                admi.AREA_SERVICIO_ID = admision.AREA_SERVICIO_ID;
                admi.PEDIDO = admision.PEDIDO;

                db.Entry(admi).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                return RedirectToAction("Admisiones");
            }

            return View(admision);
        }

        [HttpGet]
        public ActionResult BorrarAdmision(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Admisiones");
            }

            CS_ADMISION admi = db.CS_ADMISION.Find(id);

            if (admi == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Borrar Admisión";
            return View("BorrarAdmision", admi);
        }

        [HttpPost, ActionName("BorrarAdmision")]
        public ActionResult BorrarAdmis(int id)
        {
            CS_ADMISION admi = db.CS_ADMISION.Find(id);
            admi.ACTIVO = false;
            db.Entry(admi).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Admisiones");
        }

        [HttpGet]
        public ActionResult SignosVitales(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Admisiones");
            }

            CS_ADMISION admi = db.CS_ADMISION.Find(id);

            if (admi == null)
            {
                return HttpNotFound();
            }

            ViewBag.ADMISION_ID = admi.ID_ADMISION;
            ViewBag.PACIENTE = admi.CLIENTE.NOMBRE;

            ViewBag.Title = "Registro de Signos Vitales";
            return View("RegistrarSignosVitales");
        }

        [HttpPost]
        public ActionResult SignosVitales([Bind(Include = "ADMISION_ID, TEMPERATURAC, ESTATURA, PESOKG, IMC, PULSO, RESPIRACION, SATURACION, GLUCOMETRIA, PSISTOLICA, PDIASTOLICA, PERIMETRO_TORAXICO, PERIMETRO_MUNECA, PERIMETRO_CEFALICO, PERIMETRO_ABDOMINAL, HALLAZGO")] CS_SIGNOS_VITALES signosVitales)
        {
            if (ModelState.IsValid)
            {
                CS_ADMISION admi = db.CS_ADMISION.Find(signosVitales.ADMISION_ID);
                admi.SIGNOS_VITALES = true;
                db.Entry(admi).State = EntityState.Modified;

                signosVitales.FECHA_HORA_TOMA = DateTime.Now;
                signosVitales.USUARIO_REGISTRO = util.GetUser();
                signosVitales.FECHA_REGISTRO = DateTime.Now;
                signosVitales.ACTIVO = true;

                db.CS_SIGNOS_VITALES.Add(signosVitales);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

                return RedirectToAction("Admisiones");
            }

            // return View(signosVitales);
            return RedirectToAction("Admisiones");
        }

        [HttpGet]
        public ActionResult DetalleSignosVitales(int? id)
        {
            CS_ADMISION admi = db.CS_ADMISION.Find(id);

            if (admi == null)
            {
                return HttpNotFound();
            }

            ViewBag.ADMISION_ID = admi.ID_ADMISION;
            ViewBag.PACIENTE = admi.CLIENTE.NOMBRE;

            CS_SIGNOS_VITALES signosVitales = db.CS_SIGNOS_VITALES.Where(i => i.ADMISION_ID == id).FirstOrDefault();

            return PartialView(signosVitales);
        }

        [HttpGet]
        public ActionResult Pacientes()
        {
            return View(db.CS_PACIENTES.ToList().Where(x => x.ACTIVO == true));
        }

        [HttpGet]
        public ActionResult NuevoPaciente()
        {
            CONSECUTIVO consecutivoPaciente = db.CONSECUTIVO.Where(a => a.CONSECUTIVO1 == "PACI-00000").FirstOrDefault();
            if (consecutivoPaciente != null)
            {
                ViewBag.CLIENTE_ID = Utilerias.ProximoCodigo(consecutivoPaciente.ULTIMO_VALOR, Decimal.ToInt32(consecutivoPaciente.LONGITUD));
            }
            else
            {
                ViewBag.CLIENTE_ID = "-";
            }
            
            ViewBag.ESTADO_CIVIL_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 6), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.TIPO_SANGRE_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 15), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.ASEGURADORA_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 13), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.TIPO_ASEGURADO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.ESCOLARIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 11), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.PROFESION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 1), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.RELIGION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 5), "ID_CAT_DETALLE", "DESCRIPCION");
            
            ViewBag.Title = "Nuevo Paciente";
            return View("NuevoEditarPaciente");
        }

        [HttpPost]
        public ActionResult NuevoPaciente([Bind(Include = "CLIENTE_ID, IDENTIFICACION, NOMBRES, APELLIDOS, SEXO, ESTADO_CIVIL_ID, FECHA_NACIMIENTO, TIPO_SANGRE_ID, ASEGURADORA_ID, TIPO_ASEGURADO_ID, NUM_SEGURO_SOCIAL, ESCOLARIDAD_ID, PROFESION_ID, RELIGION_ID, OBSERVACIONES")] CS_PACIENTES paciente, string TEL1, string TEL2, string EMAIL, string DIRECCION)
        {
            if (ModelState.IsValid)
            {
                CONSECUTIVO cons = db.CONSECUTIVO.Where(c => c.CONSECUTIVO1 == "PACI-00000").FirstOrDefault();
                cons.ULTIMO_VALOR = paciente.CLIENTE_ID;
                db.Entry(cons).State = EntityState.Modified;

                CLIENTE cli = new CLIENTE();
                cli.CLIENTE1 = paciente.CLIENTE_ID;
                cli.NOMBRE = paciente.NOMBRES + " " + paciente.APELLIDOS;
                cli.TELEFONO1 = TEL1;
                cli.TELEFONO2 = TEL2;
                cli.E_MAIL = EMAIL;
                cli.DIRECCION = DIRECCION;

                cli.NIVEL_PRECIO = "PACIENTES HM";
                cli.CATEGORIA_CLIENTE = "000";
                cli.MONEDA = "HNL";
                cli.MONEDA_NIVEL = "L";
                cli.PAIS = "504";
                cli.LOCAL = "L";
                cli.ACTIVO = "S";
                cli.DOC_A_GENERAR = "F";
                cli.CLASE_DOCUMENTO = "N";
                cli.ES_CORPORACION = "S";

                cli.SALDO = 0;
                cli.SALDO_CREDITO = 0;
                cli.SALDO_DOLAR = 0;
                cli.TASA_INTERES = 0;
                cli.TASA_INTERES_MORA = 0;
                cli.DESCUENTO = 0;
                cli.EXENCION_IMP1 = 0;
                cli.EXENCION_IMP2 = 0;
                cli.DIAS_ABASTECIMIEN = 0;
                cli.SALDO_TRANS = 0;
                cli.SALDO_TRANS_LOCAL = 0;
                cli.SALDO_TRANS_DOLAR = 0;
                cli.DIAS_PROMED_ATRASO = 0;

                cli.CONTACTO = "ND";
                cli.CARGO = "ND";
                cli.FAX = "ND";
                cli.CONTRIBUYENTE = "ND";
                cli.ZONA = "ND";
                cli.RUTA = "ND";
                cli.COBRADOR = "ND";
                cli.CONDICION_PAGO = "ND";

                cli.ACEPTA_BACKORDER = "N";
                cli.ACEPTA_FRACCIONES = "N";
                cli.EXCEDER_LIMITE = "N";
                cli.EXENTO_IMPUESTOS = "N";
                cli.COBRO_JUDICIAL = "N";
                cli.USA_TARJETA = "N";
                cli.REQUIERE_OC = "N";
                cli.REGISTRARDOCSACORP = "N";
                cli.USAR_DIREMB_CORP = "N";
                cli.APLICAC_ABIERTAS = "N";
                cli.VERIF_LIMCRED_CORP = "N";
                cli.USAR_DESC_CORP = "N";
                cli.TIENE_CONVENIO = "N";
                cli.ASOCOBLIGCONTFACT = "N";
                cli.USAR_PRECIOS_CORP = "N";
                cli.USAR_EXENCIMP_CORP = "N";
                cli.PARTICIPA_FLUJOCAJA = "N";
                cli.AJUSTE_FECHA_COBRO = "N";
                cli.NOTIFICAR_ERROR_EDI = "N";
                cli.MOROSO = "N";
                cli.MODIF_NOMB_EN_FAC = "N";
                cli.PERMITE_DOC_GP = "N";
                cli.MULTIMONEDA = "N";
                cli.ACEPTA_DOC_EDI = "N";
                cli.ACEPTA_DOC_ELECTRONICO = "N";
                cli.CONFIRMA_DOC_ELECTRONICO = "N";
                cli.DETALLAR_KITS = "N";

                cli.FECHA_INGRESO = DateTime.Now;
                cli.FECHA_ULT_MORA = DateTime.Now;
                cli.FECHA_ULT_MOV = DateTime.Now;

                db.CLIENTE.Add(cli);
                
                paciente.ACTIVO = true;
                paciente.USUARIO_ID = util.GetUser();
                paciente.FECHA_REGISTRO = DateTime.Now;

                db.CS_PACIENTES.Add(paciente);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

                return RedirectToAction("Pacientes");
            }

            return View(paciente);
        }

        [HttpGet]
        public ActionResult EditarPaciente(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Pacientes");
            }
            CS_PACIENTES pacliente = db.CS_PACIENTES.Where(i => i.CLIENTE_ID == id).FirstOrDefault();
            if (pacliente == null)
            {
                return HttpNotFound();
            }

            ViewBag.ESTADO_CIVIL_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 6).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.ESTADO_CIVIL_ID);
            ViewBag.TIPO_SANGRE_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 15).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.TIPO_SANGRE_ID);
            //ViewBag.ASEGURADORA_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 13).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.ASEGURADORA_ID);
            //ViewBag.TIPO_ASEGURADO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.TIPO_ASEGURADO_ID);
            ViewBag.ESCOLARIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 11).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.ESCOLARIDAD_ID);
            ViewBag.PROFESION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 1).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.PROFESION_ID);
            ViewBag.RELIGION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 5).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.RELIGION_ID);

            ViewBag.Title = "Editar Paciente";
            ViewBag.Accion = "Editar";
            return View("NuevoEditarPaciente", pacliente);
        }

        [HttpPost]
        public ActionResult EditarPaciente([Bind(Include = "ID_PACIENTE, CLIENTE_ID, IDENTIFICACION, NOMBRES, APELLIDOS, SEXO, ESTADO_CIVIL_ID, FECHA_NACIMIENTO, TIPO_SANGRE_ID, ASEGURADORA_ID, TIPO_ASEGURADO_ID, NUM_SEGURO_SOCIAL, ESCOLARIDAD_ID, PROFESION_ID, RELIGION_ID, OBSERVACIONES, CLIENTE")] CS_PACIENTES pacliente)
        {
            if (ModelState.IsValid)
            {
                CLIENTE cli = db.CLIENTE.Find(pacliente.CLIENTE_ID);
                cli.NOMBRE = pacliente.NOMBRES + " " + pacliente.APELLIDOS;
                db.Entry(cli).State = EntityState.Modified;

                CS_PACIENTES pacli = db.CS_PACIENTES.Find(pacliente.ID_PACIENTE);

                pacli.CLIENTE_ID = pacliente.CLIENTE_ID;
                pacli.IDENTIFICACION = pacliente.IDENTIFICACION;
                pacli.NOMBRES = pacliente.NOMBRES;
                pacli.APELLIDOS = pacliente.APELLIDOS;
                pacli.SEXO = pacliente.SEXO;
                pacli.ESTADO_CIVIL_ID = pacliente.ESTADO_CIVIL_ID;
                pacli.FECHA_NACIMIENTO = pacliente.FECHA_NACIMIENTO;
                pacli.TIPO_SANGRE_ID = pacliente.TIPO_SANGRE_ID;
                //pacli.FINANCIADOR = pacliente.FINANCIADOR;
                //pacli.TIPO_ASEGURADO_ID = pacliente.TIPO_ASEGURADO_ID;
                //pacli.NUM_AFILIADO = pacliente.NUM_AFILIADO;
                pacli.ESCOLARIDAD_ID = pacliente.ESCOLARIDAD_ID;
                pacli.PROFESION_ID = pacliente.PROFESION_ID;
                pacli.RELIGION_ID = pacliente.RELIGION_ID;
                pacli.OBSERVACIONES = pacliente.OBSERVACIONES;

                pacli.CLIENTE.TELEFONO1 = pacliente.CLIENTE.TELEFONO1;
                pacli.CLIENTE.TELEFONO2 = pacliente.CLIENTE.TELEFONO2;
                pacli.CLIENTE.E_MAIL = pacliente.CLIENTE.E_MAIL;
                pacli.CLIENTE.DIRECCION = pacliente.CLIENTE.DIRECCION;

                db.Entry(pacli).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                return RedirectToAction("Pacientes");
            }

            return View(pacliente);
        }

        [HttpGet]
        public ActionResult BorrarPaciente(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Pacientes");
            }

            CS_PACIENTES pacliente = db.CS_PACIENTES.Where(i => i.CLIENTE_ID == id).FirstOrDefault();

            if (pacliente == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Borrar Paciente";
            return View("BorrarPaciente", pacliente);
        }

        [HttpPost, ActionName("BorrarPaciente")]
        public ActionResult BorrarPacliente(string id)
        {
            CS_PACIENTES pacli = db.CS_PACIENTES.Where(i => i.CLIENTE_ID == id).FirstOrDefault();
            pacli.ACTIVO = false;
            db.Entry(pacli).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Pacientes");
        }

        public PartialViewResult Popup()
        {
            List<CLIENTE> clis = db.CLIENTE.Where(c => c.ACTIVO == "S" && c.CATEGORIA_CLIENTE == "000" && c.NIVEL_PRECIO == "PACIENTES HM").ToList();
            return PartialView("_ListadoPacientesSoftland", clis);
        }
    }
}
using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;

using CS_HOSPITALARIO_Front_end.Models.ViewModel;
using System.Text.RegularExpressions;

namespace CS_HOSPITALARIO_Front_end.Controllers
{

    public class AdmisionController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();
        private string consecutivoPacienteSoftland = "";
        private string consecutivoPedidos = "";
        private string Bodega_gral = "";
        List<int> listRolAccess = new List<int>() { 1, 2, 3, 4, 5 };
        public AdmisionController()
        {
            var parametro_consec = (from p in db.CS_PARAMETROS select p).FirstOrDefault();

            consecutivoPacienteSoftland = parametro_consec.CONSECUTIVO_CLIENTE;

            consecutivoPedidos = parametro_consec.CONSECUTIVO_PEDIDO;

            Bodega_gral = parametro_consec.BODEGA_GENERAL;
        }

        [HttpGet]
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true).OrderByDescending(a => a.FECHA_REGISTRO));
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }

        }
        [HttpGet]
        public ActionResult Pacientes()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_PACIENTES.ToList().Where(x => x.ACTIVO == true).OrderByDescending(p => p.FECHA_REGISTRO));
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }

        }
        [HttpGet]
        public ActionResult NuevoPaciente()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    CONSECUTIVO consecutivoPaciente = db.CONSECUTIVO.Where(a => a.CONSECUTIVO1 == this.consecutivoPacienteSoftland).FirstOrDefault();
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
                    ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CLIENTE1.Contains("C00")), "CLIENTE1", "NOMBRE").Take(10);
                    ViewBag.TIPO_ASEGURADO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.ESCOLARIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 11), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.PROFESION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 1), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.RELIGION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 5), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.CATEGORIA_CLIENTE_ID = new SelectList(db.CATEGORIA_CLIENTE, "CATEGORIA_CLIENTE1", "DESCRIPCION");
                    ViewBag.NIVEL_PRECIO_ID = new SelectList(db.NIVEL_PRECIO, "NIVEL_PRECIO1", "NIVEL_PRECIO1");
                    ViewBag.PAIS_ID = new SelectList(db.PAIS, "PAIS1", "NOMBRE");
                    ViewBag.CONDICION_PAGO_ID = new SelectList(db.CONDICION_PAGO, "CONDICION_PAGO1", "DESCRIPCION");
                    ViewBag.DEPARTAMENTO = new SelectList(db.DIVISION_GEOGRAFICA1, "DIVISION_GEOGRAFICA11", "NOMBRE");

                    ViewBag.Title = "Nuevo Paciente";
                    return View("NuevoEditarPaciente");
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }
        }
        [HttpPost]
        public ActionResult NuevoPaciente([Bind(Include = "CLIENTE_ID, IDENTIFICACION, NOMBRES, APELLIDOS, SEXO, ESTADO_CIVIL_ID, FECHA_NACIMIENTO, TIPO_SANGRE_ID, FINANCIADOR, TIPO_ASEGURADO_ID, NUM_AFILIADO, ESCOLARIDAD_ID, PROFESION_ID, RELIGION_ID, OBSERVACIONES, CONTACTO_1, CONTACTO_2, TEL_CONT_1, TEL_CONT_2,LOCAL,DEPARTAMENTO")] CS_PACIENTES paciente, string TEL1, string TEL2, string EMAIL, string DIRECCION, string NIVEL_PRECIO_ID, string CATEGORIA_CLIENTE_ID, string PAIS_ID, string CONDICION_PAGO_ID, string LOCAL, [Bind(Include = "chbxEmpresa")]  bool chbxEmpresa, [Bind(Include = "chbxExtranjero")] bool chbxExtranjero)
        {
            ViewBag.ESTADO_CIVIL_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 6), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.TIPO_SANGRE_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 15), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CLIENTE1.Contains("C00")), "CLIENTE1", "NOMBRE").Take(10);
            ViewBag.TIPO_ASEGURADO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.ESCOLARIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 11), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.PROFESION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 1), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.RELIGION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 5), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.CATEGORIA_CLIENTE_ID = new SelectList(db.CATEGORIA_CLIENTE, "CATEGORIA_CLIENTE1", "DESCRIPCION");
            ViewBag.NIVEL_PRECIO_ID = new SelectList(db.NIVEL_PRECIO, "NIVEL_PRECIO1", "NIVEL_PRECIO1");
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "PAIS1", "NOMBRE");
            ViewBag.CONDICION_PAGO_ID = new SelectList(db.CONDICION_PAGO, "CONDICION_PAGO1", "DESCRIPCION");
            ViewBag.DEPARTAMENTO = new SelectList(db.DIVISION_GEOGRAFICA1, "DIVISION_GEOGRAFICA11", "NOMBRE");

            if (ModelState.IsValid)
            {
                //VALIDANDO QUE NO EXISTA UN PACIENTE CON LA MISMA IDENTIFICACION
                var ident = db.CS_PACIENTES.Where(x => x.IDENTIFICACION == paciente.IDENTIFICACION).FirstOrDefault();
                if (ident != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un paciente con esta identificación");
                    return View("NuevoEditarPaciente", paciente);
                }

                if (chbxEmpresa)
                {
                    if (!Regex.IsMatch(paciente.IDENTIFICACION, @"^\d{4}-\d{4}-\d{6}$"))
                    {
                        ModelState.AddModelError(string.Empty, "Revise los datos de la identificacion, deben estar completos.");
                        return View("NuevoEditarPaciente", paciente);
                    }
                }

                if (!chbxEmpresa && !chbxExtranjero)
                {
                    if (!Regex.IsMatch(paciente.IDENTIFICACION, @"^\d{4}-\d{4}-\d{5}$"))
                    {
                        ModelState.AddModelError(string.Empty, "Revise los datos de la identificacion, deben estar completos.");
                        return View("NuevoEditarPaciente",paciente);
                    }
                }

                paciente.CLIENTE_ID = paciente.IDENTIFICACION;

                ////VALIDMOS QUE EL CONSECUTIVO AUN NO SE HAYA UTILIZADO
                //var existecli = (from p in db.CLIENTE where p.CLIENTE1 == paciente.CLIENTE_ID select p).FirstOrDefault();
                //if (existecli != null)
                //{
                //    var ultimo_consec = (from p in db.CONSECUTIVO where p.CONSECUTIVO1 == this.consecutivoPacienteSoftland select p).FirstOrDefault();
                    
                //    paciente.CLIENTE_ID = Utilerias.ProximoCodigo(ultimo_consec.ULTIMO_VALOR, Decimal.ToInt32(ultimo_consec.LONGITUD)); ;
                //}

                ////actualizamos consecutivo
                //CONSECUTIVO cons = db.CONSECUTIVO.Where(c => c.CONSECUTIVO1 == this.consecutivoPacienteSoftland).FirstOrDefault();
                //cons.ULTIMO_VALOR = paciente.CLIENTE_ID;
                //db.Entry(cons).State = EntityState.Modified;

                var monedanivel = db.NIVEL_PRECIO.Where(x => x.NIVEL_PRECIO1 == NIVEL_PRECIO_ID).FirstOrDefault();
                string moneda = string.Empty;
                if (monedanivel.MONEDA == "L")
                {
                    moneda = db.GLOBALES_AS.FirstOrDefault().MONEDA_LOCAL.ToString();
                }
                else
                {
                    moneda = db.GLOBALES_AS.FirstOrDefault().MONEDA_DOLAR.ToString();
                }

                var detalle_direccion = db.SP_Insertar_detalle_direccion_cliente(DIRECCION, "ERPADMIN").FirstOrDefault();
                int detadir = Convert.ToInt32(detalle_direccion.DETALLE_DIRECCION);


                db.SP_Insertar_cliente_NIT(paciente.IDENTIFICACION, paciente.NOMBRES + " " + paciente.APELLIDOS, PAIS_ID, "sa");

                CLIENTE cli = new CLIENTE();
                cli.CLIENTE1 = paciente.CLIENTE_ID;
                cli.NOMBRE = paciente.NOMBRES + " " + paciente.APELLIDOS;
                cli.ALIAS = paciente.NOMBRES + " " + paciente.APELLIDOS;
                cli.TELEFONO1 = TEL1;
                cli.TELEFONO2 = TEL2;
                cli.E_MAIL = EMAIL;
                cli.FAX = "";
                cli.CONTRIBUYENTE = paciente.IDENTIFICACION;
                cli.DETALLE_DIRECCION = detadir;
                cli.DIRECCION = "DETALLE:" + DIRECCION;
                cli.DIR_EMB_DEFAULT = "ND";
                cli.CONDICION_PAGO = CONDICION_PAGO_ID;
                cli.NIVEL_PRECIO = NIVEL_PRECIO_ID;
                cli.CATEGORIA_CLIENTE = CATEGORIA_CLIENTE_ID;
                cli.MONEDA = moneda;
                cli.MONEDA_NIVEL = monedanivel.MONEDA;
                cli.PAIS = PAIS_ID;
                cli.LOCAL = LOCAL;
                cli.ACTIVO = "S";
                cli.DOC_A_GENERAR = "F";
                cli.CLASE_DOCUMENTO = "N";
                cli.ES_CORPORACION = "S";
                cli.ES_EXTRANJERO = LOCAL == "S" ? "N" : "S";
                cli.UpdatedBy = "AH/ERPADMIN";
                cli.CreatedBy = "AH/ERPADMIN";
                cli.ES_AGENTE_PERCEPCION = "N";
                cli.ES_BUEN_CONTRIBUYENTE = "N";
                cli.SUJETO_PORCE_SUNAT = "N";
                cli.PDB_EXPORTADORES = "N";
                cli.SALDO = 0;
                cli.SALDO_LOCAL = 0;
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
                cli.SALDO_NOCARGOS = 0;

                cli.CONTACTO = "ND";
                cli.CARGO = "ND";
                cli.FAX = "ND";
                cli.CONTRIBUYENTE = paciente.IDENTIFICACION;
                cli.ZONA = "ND";
                cli.RUTA = "ND";
                cli.VENDEDOR = "ND";
                cli.COBRADOR = "ND";
                cli.CLASE_ABC = "A";
                cli.TIPO_CONTRIBUYENTE = "F";
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
                cli.MODIF_NOMB_EN_FAC = "S";
                cli.PERMITE_DOC_GP = "N";
                cli.MULTIMONEDA = "S";
                cli.ACEPTA_DOC_EDI = "N";
                cli.ACEPTA_DOC_ELECTRONICO = "N";
                cli.CONFIRMA_DOC_ELECTRONICO = "N";
                cli.DETALLAR_KITS = "N";

                cli.FECHA_INGRESO = DateTime.Now;
                cli.FECHA_HORA_CREACION = DateTime.Now;
                cli.FCH_HORA_ULT_MOD = DateTime.Now;
                cli.FECHA_ULT_MOV = DateTime.Now;
                cli.FECHA_ULT_MORA = DateTime.Now;
                cli.RecordDate = DateTime.Now;
                cli.CreateDate = DateTime.Now;
                cli.RowPointer = Guid.NewGuid();

                db.CLIENTE.Add(cli);



                paciente.ACTIVO = true;
                paciente.USUARIO_ID = util.GetUser();
                paciente.FECHA_REGISTRO = DateTime.Now;
                paciente.OBSERVACIONES = (string.IsNullOrEmpty(paciente.OBSERVACIONES)) ? paciente.OBSERVACIONES = "Ninguna" : paciente.OBSERVACIONES;
                db.CS_PACIENTES.Add(paciente);
                try
                {
                    db.SaveChanges();
                    db.SP_Insertar_cliente_vendedor(paciente.CLIENTE_ID, "ND", "ERPADMIN");
                    db.SP_Insertar_direcc_embarque(paciente.CLIENTE_ID, "ND", String.Empty, "ERPADMIN");

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

            return View("NuevoEditarPaciente", paciente);
        }
        public JsonResult GuardarContactos(List<CS_CONTACTOS> contactos)
        {
            var status = true;
            try
            {
                foreach (var item in contactos)
                {
                    CS_CONTACTOS contacto = new CS_CONTACTOS();
                    contacto.NOMBRES = item.NOMBRES;
                    contacto.APELLIDOS = item.APELLIDOS;
                    contacto.TEL_CONT_1 = item.TEL_CONT_1;
                    contacto.TEL_CONT_2 = item.TEL_CONT_2;
                    contacto.CORREO = item.CORREO;
                    contacto.CLIENTE_ID = item.CLIENTE_ID;
                    contacto.ID_PARENTESCO = item.ID_PARENTESCO;
                    contacto.ACTIVO = true;
                    contacto.USUARIO_REGISTRA = util.GetUser();
                    contacto.FECHA_REGISTRO = DateTime.Now;
                    db.CS_CONTACTOS.Add(contacto);
                    db.SaveChanges();
                }
                //db.SP_Actualizar_pedido_monto_paciente(cod_pedido,version,total, cantarticulo);
                status = true;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        public JsonResult EditarContactos(List<CS_CONTACTOS> contactos)
        {
            var status = true;
            try
            {
                foreach (var item in contactos)
                {
                    CS_CONTACTOS contacto = db.CS_CONTACTOS.Where(x => x.ID_CONTACTO == item.ID_CONTACTO).FirstOrDefault();
                    contacto.NOMBRES = item.NOMBRES;
                    contacto.APELLIDOS = item.APELLIDOS;
                    contacto.TEL_CONT_1 = item.TEL_CONT_1;
                    contacto.TEL_CONT_2 = item.TEL_CONT_2;
                    contacto.CORREO = item.CORREO;
                    contacto.CLIENTE_ID = item.CLIENTE_ID;
                    contacto.ID_PARENTESCO = item.ID_PARENTESCO;
                    contacto.ACTIVO = true;
                    contacto.USUARIO_MODIFICA = util.GetUser();
                    contacto.FECHA_MODIFICA = DateTime.Now;

                    db.SaveChanges();
                }
                //db.SP_Actualizar_pedido_monto_paciente(cod_pedido,version,total, cantarticulo);
                status = true;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        public JsonResult EliminarContactos(List<CS_CONTACTOS> contactos)
        {
            var status = true;
            try
            {
                foreach (var item in contactos)
                {
                    CS_CONTACTOS contacto = db.CS_CONTACTOS.Where(x => x.ID_CONTACTO == item.ID_CONTACTO).FirstOrDefault();
                    contacto.ACTIVO = false;
                    contacto.USUARIO_MODIFICA = util.GetUser();
                    contacto.FECHA_MODIFICA = DateTime.Now;
                    db.SaveChanges();
                }
                //db.SP_Actualizar_pedido_monto_paciente(cod_pedido,version,total, cantarticulo);
                status = true;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult ValidarIdentificacion(string identificacion)
        {
            var status = true;
            var mensaje = string.Empty;
            try
            {
                var ident = db.CS_PACIENTES.Where(x => x.IDENTIFICACION == identificacion).FirstOrDefault();

                if (ident == null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                    mensaje = "Debe de cambair la identificacion!!.Este se ha registrado al paciente:" + ident.CLIENTE_ID + "-" + ident.NOMBRES + " " + ident.APELLIDOS;
                }
            }
            catch
            {
                status = false;
            }
            return Json(new JsonResult { Data = new { status = status, mensaje = mensaje } }, JsonRequestBehavior.AllowGet);
            //return new JsonResult { Data = new { status = status, mensaje = mensaje } };
        }
        public JsonResult GetParentesco()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var PARENTESCO = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 7 && a.ACTIVO == true).Select(x => new
                    {
                        ID = x.ID_CAT_DETALLE,
                        DESCRIPCION = x.DESCRIPCION
                    }).ToList();
                    return Json(PARENTESCO.Select(x => new
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
        public JsonResult GetContactos(string cliente)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var PARENTESCO = db.CS_CONTACTOS.Where(a => a.CLIENTE_ID == cliente && a.ACTIVO == true).ToList();
                    return Json(PARENTESCO.Select(x => new
                    {
                        x.ID_CONTACTO,
                        x.NOMBRES,
                        x.APELLIDOS,
                        x.TEL_CONT_1,
                        x.TEL_CONT_2,
                        x.CORREO,
                        x.CLIENTE_ID,
                        x.ID_PARENTESCO
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        [HttpGet]
        public ActionResult EditarPaciente(string id)
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                if (id == null)
                {
                    return RedirectToAction("Pacientes");
                }
                SP_listar_paciente_Result pacliente = db.SP_listar_paciente().Where(x => x.CLIENTE_ID == id).FirstOrDefault();
                if (pacliente == null)
                {
                    return HttpNotFound();
                }

                ViewBag.ESTADO_CIVIL_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 6).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.ESTADO_CIVIL_ID);
                ViewBag.TIPO_SANGRE_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 15).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.TIPO_SANGRE_ID);
                //ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CLIENTE1.Contains("C00")), "CLIENTE1", "NOMBRE", pacliente.FINANCIADOR).Take(10);
                //ViewBag.TIPO_ASEGURADO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.TIPO_ASEGURADO_ID);
                ViewBag.ESCOLARIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 11).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.ESCOLARIDAD_ID);
                ViewBag.PROFESION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 1).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.PROFESION_ID);
                ViewBag.RELIGION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 5).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", pacliente.RELIGION_ID);
                ViewBag.CATEGORIA_CLIENTE_ID = new SelectList(db.CATEGORIA_CLIENTE, "CATEGORIA_CLIENTE1", "DESCRIPCION", pacliente.CATEGORIA_CLIENTE_ID);
                ViewBag.NIVEL_PRECIO_ID = new SelectList(db.NIVEL_PRECIO, "NIVEL_PRECIO1", "NIVEL_PRECIO1", pacliente.NIVEL_PRECIO_ID);
                ViewBag.PAIS_ID = new SelectList(db.PAIS, "PAIS1", "NOMBRE", pacliente.PAIS_ID);
                ViewBag.CONDICION_PAGO_ID = new SelectList(db.CONDICION_PAGO, "CONDICION_PAGO1", "DESCRIPCION", pacliente.CONDICION_PAGO_ID);
                ViewBag.DEPARTAMENTO = new SelectList(db.DIVISION_GEOGRAFICA1, "DIVISION_GEOGRAFICA11", "NOMBRE", pacliente.DEPARTAMENTO);
                ViewBag.FechaNac = pacliente.FECHA_NACIMIENTO;
                ViewBag.Title = "Editar Paciente";
                ViewBag.Accion = "Editar";
                return View("EditarPaciente", pacliente);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }


        }
        [HttpPost]
        public ActionResult EditarPaciente([Bind(Include = "ID_PACIENTE, CLIENTE_ID, IDENTIFICACION, NOMBRES, APELLIDOS, SEXO, ESTADO_CIVIL_ID, FECHA_NACIMIENTO, TIPO_SANGRE_ID, FINANCIADOR, TIPO_ASEGURADO_ID, NUM_AFILIADO, ESCOLARIDAD_ID, PROFESION_ID, RELIGION_ID, OBSERVACIONES, CLIENTE, CONTACTO_1, CONTACTO_2, TEL_CONT_1, TEL_CONT_2, DEPARTAMENTO")] CS_PACIENTES pacliente, string TELEFONO1, string TELEFONO2, string E_MAIL, string DIRECCION, string NIVEL_PRECIO_ID, string CATEGORIA_CLIENTE_ID, string PAIS_ID, string CONDICION_PAGO_ID, string LOCAL)
        {
            Debug.WriteLine("ID PACIENTE: " + pacliente.ID_PACIENTE);
            Debug.WriteLine("CLIENTE ID: " + pacliente.CLIENTE_ID);


            if (ModelState.IsValid)
            {
                var monedanivel = db.NIVEL_PRECIO.Where(x => x.NIVEL_PRECIO1 == NIVEL_PRECIO_ID).FirstOrDefault();
                string moneda = string.Empty;
                if (monedanivel.MONEDA == "L")
                {
                    moneda = db.GLOBALES_AS.FirstOrDefault().MONEDA_LOCAL.ToString();
                }
                else
                {
                    moneda = db.GLOBALES_AS.FirstOrDefault().MONEDA_DOLAR.ToString();
                }

                CLIENTE cli = db.CLIENTE.Find(pacliente.CLIENTE_ID);
                cli.NOMBRE = pacliente.NOMBRES + " " + pacliente.APELLIDOS;
                cli.ALIAS = pacliente.NOMBRES + " " + pacliente.APELLIDOS;
                cli.CATEGORIA_CLIENTE = CATEGORIA_CLIENTE_ID;
                cli.LOCAL = LOCAL;
                cli.CONDICION_PAGO = CONDICION_PAGO_ID;
                cli.PAIS = PAIS_ID;
                cli.NIVEL_PRECIO = NIVEL_PRECIO_ID;
                cli.MONEDA = moneda;
                cli.TELEFONO1 = TELEFONO1;
                cli.TELEFONO2 = TELEFONO2;
                cli.DIRECCION = DIRECCION;
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
                pacli.ESCOLARIDAD_ID = pacliente.ESCOLARIDAD_ID;
                pacli.PROFESION_ID = pacliente.PROFESION_ID;
                pacli.RELIGION_ID = pacliente.RELIGION_ID;
                pacli.OBSERVACIONES = pacliente.OBSERVACIONES;
                pacli.DEPARTAMENTO = pacliente.DEPARTAMENTO;

                //pacli.CLIENTE.TELEFONO1 = pacliente.CLIENTE.TELEFONO1;
                //pacli.CLIENTE.TELEFONO2 = pacliente.CLIENTE.TELEFONO2;
                //pacli.CLIENTE.E_MAIL = pacliente.CLIENTE.E_MAIL;
                //pacli.CLIENTE.DIRECCION = pacliente.CLIENTE.DIRECCION;
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
        public JsonResult GetContactoPaciente(string codcliente)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    //CS_PACIENTES pac = db.CS_PACIENTES.Where(x=>x.ID_PACIENTE==codpaciente).FirstOrDefault();
                    var datos = db.CS_CONTACTOS.Where(x => x.ACTIVO == true && x.CLIENTE_ID == codcliente).Include(x => x.CS_CATALOGO_DETALLE).ToList();
                    //return  Json (datos,JsonRequestBehavior.AllowGet);
                    //return new JsonResult { Data = new { datos } };
                    return Json(datos.Select(x => new
                    {
                        x.ID_CONTACTO,
                        x.NOMBRES,
                        x.APELLIDOS,
                        x.TEL_CONT_1,
                        x.TEL_CONT_2,
                        x.CORREO,
                        PARENTESCO = x.CS_CATALOGO_DETALLE.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        [HttpGet]
        public ActionResult BorrarPaciente(string id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
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
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }
        }
        //[HttpPost, ActionName("BorrarPaciente")]
        public ActionResult BorrarPacliente(string id)
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                CS_PACIENTES pacli = db.CS_PACIENTES.Where(i => i.CLIENTE_ID == id).FirstOrDefault();
                pacli.ACTIVO = false;
                db.Entry(pacli).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Pacientes");
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }

        }
        [HttpGet]
        public ActionResult NuevaAdmision()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                    if (consecutivoPedido != null)
                    {
                        ViewBag.NUM_PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                    }
                    else
                    {
                        ViewBag.NUM_PEDIDO = "-";
                    }

                    var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
                    {
                        ID_PACIENTE = x.CLIENTE_ID,
                        CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
                    }).ToList();
                    var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
                    {
                        ID_MEDICO = x.ID_MEDICO,
                        MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
                    }).ToList();
                    //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "ID_PACIENTE", "CLIENTE_ID");
                    //SACAMOS EL PARAMETRO DE LAS ASEGURADORAS
                    var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

              

                    ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID");
                    ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO");
                    ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10 && p.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2 && a.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3 && ti.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(u => u.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE");
                    ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");

                    var financiadorList = new SelectList(db.CLIENTE.Where(u => u.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE");
                    System.Diagnostics.Debug.WriteLine($"Financiador count: {financiadorList.Count()}");
                    
                    
                    ViewBag.Title = "Nueva Admisión";
                    return View("NuevoEditarAdmision");
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }
        }
        [HttpPost]
        public ActionResult NuevaAdmision([Bind(Include = "CLIENTE_ID,DOCTOR_ID, NUM_AFILIADO, FINANCIADOR, TIPO_SEGURO, PRIORIDAD_ID, CAUSA_ADMISION_ID, TIPO_INGRESO_ID, AREA_SERVICIO_ID, PEDIDO, ASEGURADO, REMITIDO")] CS_ADMISION admision, string ID_FINANC)
        {
            if (ModelState.IsValid)
            {
                var parametros = db.CS_PARAMETROS.FirstOrDefault();

                //Validamos si aun no existe el pedido

                PEDIDO ped = db.PEDIDO.Where(x => x.PEDIDO1 == admision.PEDIDO).FirstOrDefault();
                if (ped != null)
                {
                    CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                    if (consecutivoPedido != null)
                    {
                        admision.PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                    }
                }

                admision.FECHA_RECEPCION = DateTime.Now;
                admision.FECHA_REGISTRO = DateTime.Now;
                admision.USUARIO_REGISTRO = util.GetUser();
                admision.ATENDIDO = false;
                admision.SIGNOS_VITALES = false;
                admision.ACTIVO = true;
                admision.DOCTOR_ID = admision.DOCTOR_ID;
                admision.TIPO_ALTA = parametros.ALTA_DEFAULT;
                admision.FECHA_ALTA = DateTime.Now;
                CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();

                if (admision.ASEGURADO)
                {
                    admision.FINANCIADOR = ID_FINANC;
                }
                else
                {
                    admision.FINANCIADOR = paci.CLIENTE_ID;
                    admision.TIPO_SEGURO = null;
                    admision.NUM_AFILIADO = null;
                }
                //AGREGAR CONSULTA
               // CS_PACIENTES paciente = db.CS_PACIENTES.Find(admision.CLIENTE_ID);
                CLIENTE cliente = db.CLIENTE.Find(admision.CLIENTE_ID);
                CS_CONSULTAS consulta = new CS_CONSULTAS();
                consulta.CLIENTE_ID = admision.CLIENTE_ID;
                consulta.DOCTOR_ID = admision.DOCTOR_ID;
                consulta.AREA_SERVICIO_ID = admision.AREA_SERVICIO_ID;
                consulta.ID_ADMISION = admision.ID_ADMISION;
                consulta.FECHA_REGISTRO = DateTime.Now;
                consulta.USUARIO_REGISTRO = 1;

                db.CS_CONSULTAS.Add(consulta);

               



                //buscamos informacion del nivel 
                VERSION_NIVEL dato_version = (db.VERSION_NIVEL.Where(x => x.NIVEL_PRECIO == cliente.NIVEL_PRECIO)).FirstOrDefault();

                //Agregar Pedido
                PEDIDO pedidoAdm = new PEDIDO();

                pedidoAdm.PEDIDO1 = admision.PEDIDO;
                pedidoAdm.ESTADO = "N";
                pedidoAdm.DIREC_EMBARQUE = "ND";
                pedidoAdm.TIPO_PEDIDO = "N";
                pedidoAdm.MONEDA_PEDIDO = dato_version.MONEDA;
                pedidoAdm.AUTORIZADO = "N";
                pedidoAdm.DOC_A_GENERAR = "F";
                pedidoAdm.CLASE_PEDIDO = "N";
                pedidoAdm.MONEDA = dato_version.MONEDA;
                pedidoAdm.COBRADOR = "ND";
                pedidoAdm.RUTA = "ND";
                pedidoAdm.ZONA = "ND";
                pedidoAdm.VENDEDOR = "ND";
                pedidoAdm.TIPO_DOC_CXC = "FAC";
                pedidoAdm.BACKORDER = "N";
                pedidoAdm.DESCUENTO_CASCADA = "N";
                pedidoAdm.FIJAR_TIPO_CAMBIO = "N";
                pedidoAdm.IMPRESO = "N";
                pedidoAdm.ORIGEN_PEDIDO = "F";
                pedidoAdm.TIPO_DESCUENTO1 = "P";
                pedidoAdm.TIPO_DESCUENTO2 = "P";
                pedidoAdm.FECHA_ULT_EMBARQUE = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ULT_CANCELAC = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ORDEN = DateTime.Now;
                pedidoAdm.TARJETA_CREDITO = String.Empty;
                pedidoAdm.DIRECCION_FACTURA = "DETALLE:" + cliente.DIRECCION;
                pedidoAdm.PORC_INTCTE = 0;
                pedidoAdm.BASE_IMPUESTO1 = 0;
                pedidoAdm.BASE_IMPUESTO2 = 0;
                pedidoAdm.FECHA_PROYECTADA = DateTime.Now;
                pedidoAdm.TASA_IMPOSITIVA_PORC = 0;
                pedidoAdm.TASA_CREE1_PORC = 0;
                pedidoAdm.TASA_CREE2_PORC = 0;
                pedidoAdm.TASA_GAN_OCASIONAL_PORC = 0;
                pedidoAdm.CONTRATO_REVENTA = "N";
                pedidoAdm.RUBRO1 = paci.ID_PACIENTE.ToString();

                pedidoAdm.MONTO_ANTICIPO = 0;
                pedidoAdm.MONTO_FLETE = 0;
                pedidoAdm.MONTO_SEGURO = 0;
                pedidoAdm.MONTO_DOCUMENTACIO = 0;
                pedidoAdm.MONTO_DESCUENTO1 = 0;
                pedidoAdm.MONTO_DESCUENTO2 = 0;
                pedidoAdm.TOTAL_MERCADERIA = 0;
                pedidoAdm.TOTAL_IMPUESTO1 = 0;
                pedidoAdm.TOTAL_IMPUESTO2 = 0;
                pedidoAdm.TOTAL_A_FACTURAR = 0;
                pedidoAdm.TOTAL_CANCELADO = 0;
                pedidoAdm.TOTAL_UNIDADES = 0;
                pedidoAdm.PORC_DESCUENTO1 = 0;
                pedidoAdm.PORC_DESCUENTO2 = 0;
                pedidoAdm.PORC_COMI_COBRADOR = 0;
                pedidoAdm.PORC_COMI_VENDEDOR = 0;
                pedidoAdm.OBSERVACIONES = "Pedido generado desde el sistema Hospitalario AH";


                pedidoAdm.FECHA_PEDIDO = DateTime.Now;
                pedidoAdm.FECHA_PROMETIDA = DateTime.Now;
                pedidoAdm.FECHA_PROX_EMBARQU = DateTime.Now;
                pedidoAdm.FECHA_HORA = DateTime.Now;

                pedidoAdm.CLIENTE = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_CORPORAC = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_ORIGEN = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_DIRECCION = admision.FINANCIADOR;
                pedidoAdm.RUBRO1 = admision.CLIENTE_ID;

                pedidoAdm.CONTRATO_REVENTA = "N";
      
                pedidoAdm.EMBARCAR_A = paci.CLIENTE.ALIAS;
                pedidoAdm.DIRECCION_FACTURA = paci.CLIENTE.DIRECCION;

                if (ID_FINANC == string.Empty)
                {
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(cliente.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = paci.CLIENTE.ALIAS;
                    pedidoAdm.PAIS = cliente.PAIS;
                    pedidoAdm.NIVEL_PRECIO = cliente.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = cliente.CONDICION_PAGO;
                }
                else
                {
                    CLIENTE financiador = db.CLIENTE.Find(ID_FINANC);
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(financiador.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = financiador.ALIAS;
                    pedidoAdm.PAIS = financiador.PAIS;
                    pedidoAdm.NIVEL_PRECIO = financiador.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = financiador.CONDICION_PAGO;
                }
                pedidoAdm.USUARIO = "ERPADMIN";
                pedidoAdm.SUBTIPO_DOC_CXC = 0;
                pedidoAdm.BODEGA = Bodega_gral;
                pedidoAdm.RecordDate = DateTime.Now;
                pedidoAdm.CreateDate = DateTime.Now;
                pedidoAdm.RowPointer = Guid.NewGuid();
                pedidoAdm.CreatedBy = "ERPADMIN";
                pedidoAdm.UpdatedBy = "ERPADMIN";
                pedidoAdm.TIPO_DOCUMENTO = "P";
                db.PEDIDO.Add(pedidoAdm);

                //Agregar Admisión
                db.CS_ADMISION.Add(admision);

                //Actualizar Pedido Consecutivo
                CONSECUTIVO_FA cons = db.CONSECUTIVO_FA.Where(c => c.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                cons.VALOR_CONSECUTIVO = admision.PEDIDO;
                db.Entry(cons).State = EntityState.Modified;

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

                return RedirectToAction(nameof(Admisiones));
            }
            return RedirectToAction(nameof(Admisiones));
            //return View(admision);
        }
        [HttpGet]
        public ActionResult AdmisionLaboratorio()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var laboratorioad = db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.AREA_SERVICIO_ID == 26 && x.TIPO_INGRESO_ID == 2163 && x.SIGNOS_VITALES == null).OrderByDescending(a => a.FECHA_REGISTRO).ToList();
                return View(laboratorioad);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }
        [HttpGet]
        public ActionResult NuevaAdmisionLaboratorio()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                if (consecutivoPedido != null)
                {
                    ViewBag.NUM_PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                }
                else
                {
                    ViewBag.NUM_PEDIDO = "-";
                }
                var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_PACIENTE = x.CLIENTE_ID,
                    CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();
                var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_MEDICO = x.ID_MEDICO,
                    MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();


                var examen = (from e in db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ACTIVO == true)
                              join a in db.ARTICULO on e.ARTICULO equals a.ARTICULO1
                              select new { ARTICULO = e.ARTICULO, DESCRIPCION = a.DESCRIPCION }).ToList();

                var regi = db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(t => t.CS_TEST).Where(x =>  x.ACTIVO == true).
                                         Select(x => new
                                         {
                                             TEST = x.CS_TEST.TEST,
                                             NOMBRE = x.CS_TEST.NOMBRE,

                                             VALOR = x.CS_TEST.VALOR,
                                             DESCRIPCION = x.CS_TEST.CS_CATALOGO_DETALLE.DESCRIPCION
                                         }).ToList();

     

                ViewBag.PROCEDIMIENTO_DET = regi;

                var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

                ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID");
                ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO");
                ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10 && p.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE");
                ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.ID_EXAMEN = new SelectList(examen, "ARTICULO", "DESCRIPCION");
                ViewBag.CANTIDAD = 1;
                return View();
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }

        [HttpPost]
        public JsonResult ExamenDetalle(string examen)
        {

            var status = true;
            try
            {
                var detexamen = (from p in db.CS_DEFINICION_PROCEDIMIENTO where p.ARTICULO == examen select new { DEF_IMG = p.ID_EXAMEN, ARTICULO = p.ARTICULO, OBSERVACION = p.OBSERVACION, DESCRIPCION = p.ARTICULO1.DESCRIPCION, FECHA_CREACION = DateTime.Now.ToString() }).FirstOrDefault();
                status = true;
                return new JsonResult { Data = new { status = status, detexamen = detexamen } };

            }
            catch
            {
                return new JsonResult { Data = new { status = status } };
            }

        }
        [HttpPost]
        public JsonResult NuevaAdmisionLaboratorio(CS_ADMISION admision, string ID_FINANC)
        {
            var status = true;
            string sexo = string.Empty;

            try
            {
                //Validamos si aun no existe el pedido

                PEDIDO ped = db.PEDIDO.Where(x => x.PEDIDO1 == admision.PEDIDO).FirstOrDefault();
                if (ped != null)
                {
                    CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                    if (consecutivoPedido != null)
                    {
                        admision.PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                    }
                }

                admision.FECHA_RECEPCION = DateTime.Now;
                admision.FECHA_REGISTRO = DateTime.Now;
                admision.USUARIO_REGISTRO = util.GetUser();
                admision.ATENDIDO = false;
                admision.ACTIVO = true;
                admision.DOCTOR_ID = admision.DOCTOR_ID;
                admision.AREA_SERVICIO_ID = 26;
                admision.TIPO_INGRESO_ID = 2163;
                admision.SIGNOS_VITALES = null;
                //admision.CAUSA_ADMISION_ID

                CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();

                if (admision.ASEGURADO)
                {
                    admision.FINANCIADOR = ID_FINANC;
                }
                else
                {
                    admision.FINANCIADOR = paci.CLIENTE_ID;
                    admision.TIPO_SEGURO = null;
                    admision.NUM_AFILIADO = null;
                }
                ////AGREGAR CONSULTA
               // CS_PACIENTES paciente = db.CS_PACIENTES.Find(admision.CLIENTE_ID);
                CLIENTE cliente = db.CLIENTE.Find(admision.CLIENTE_ID);

                //buscamos informacion del nivel 
                VERSION_NIVEL dato_version = (db.VERSION_NIVEL.Where(x => x.NIVEL_PRECIO == cliente.NIVEL_PRECIO)).FirstOrDefault();

                //Agregar Pedido
                PEDIDO pedidoAdm = new PEDIDO();

                pedidoAdm.PEDIDO1 = admision.PEDIDO;
                pedidoAdm.ESTADO = "N";
                pedidoAdm.DIREC_EMBARQUE = "ND";
                pedidoAdm.TIPO_PEDIDO = "N";
                pedidoAdm.MONEDA_PEDIDO = dato_version.MONEDA;
                pedidoAdm.AUTORIZADO = "N";
                pedidoAdm.DOC_A_GENERAR = "F";
                pedidoAdm.CLASE_PEDIDO = "N";
                pedidoAdm.MONEDA = dato_version.MONEDA;
                pedidoAdm.COBRADOR = "ND";
                pedidoAdm.RUTA = "ND";
                pedidoAdm.ZONA = "ND";
                pedidoAdm.VENDEDOR = "ND";
                pedidoAdm.TIPO_DOC_CXC = "FAC";
                pedidoAdm.BACKORDER = "N";
                pedidoAdm.DESCUENTO_CASCADA = "N";
                pedidoAdm.FIJAR_TIPO_CAMBIO = "N";
                pedidoAdm.IMPRESO = "N";
                pedidoAdm.ORIGEN_PEDIDO = "F";
                pedidoAdm.TIPO_DESCUENTO1 = "P";
                pedidoAdm.TIPO_DESCUENTO2 = "P";
                pedidoAdm.FECHA_ULT_EMBARQUE = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ULT_CANCELAC = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ORDEN = DateTime.Now;
                pedidoAdm.TARJETA_CREDITO = String.Empty;
                pedidoAdm.DIRECCION_FACTURA = "DETALLE:" + cliente.DIRECCION;
                pedidoAdm.PORC_INTCTE = 0;
                pedidoAdm.BASE_IMPUESTO1 = 0;
                pedidoAdm.BASE_IMPUESTO2 = 0;
                pedidoAdm.FECHA_PROYECTADA = DateTime.Now;
                pedidoAdm.TASA_IMPOSITIVA_PORC = 0;
                pedidoAdm.TASA_CREE1_PORC = 0;
                pedidoAdm.TASA_CREE2_PORC = 0;
                pedidoAdm.TASA_GAN_OCASIONAL_PORC = 0;
                pedidoAdm.CONTRATO_REVENTA = "N";
                pedidoAdm.RUBRO1 = paci.ID_PACIENTE.ToString();

                pedidoAdm.MONTO_ANTICIPO = 0;
                pedidoAdm.MONTO_FLETE = 0;
                pedidoAdm.MONTO_SEGURO = 0;
                pedidoAdm.MONTO_DOCUMENTACIO = 0;
                pedidoAdm.MONTO_DESCUENTO1 = 0;
                pedidoAdm.MONTO_DESCUENTO2 = 0;
                pedidoAdm.TOTAL_MERCADERIA = 0;
                pedidoAdm.TOTAL_IMPUESTO1 = 0;
                pedidoAdm.TOTAL_IMPUESTO2 = 0;
                pedidoAdm.TOTAL_A_FACTURAR = 0;
                pedidoAdm.TOTAL_CANCELADO = 0;
                pedidoAdm.TOTAL_UNIDADES = 0;
                pedidoAdm.PORC_DESCUENTO1 = 0;
                pedidoAdm.PORC_DESCUENTO2 = 0;
                pedidoAdm.PORC_COMI_COBRADOR = 0;
                pedidoAdm.PORC_COMI_VENDEDOR = 0;
                pedidoAdm.OBSERVACIONES = "Pedido generado desde el sistema Hospitalario AH";


                pedidoAdm.FECHA_PEDIDO = DateTime.Now;
                pedidoAdm.FECHA_PROMETIDA = DateTime.Now;
                pedidoAdm.FECHA_PROX_EMBARQU = DateTime.Now;
                pedidoAdm.FECHA_HORA = DateTime.Now;

                pedidoAdm.CLIENTE = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_CORPORAC = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_ORIGEN = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_DIRECCION = admision.FINANCIADOR;
                pedidoAdm.RUBRO1 = admision.CLIENTE_ID;

                pedidoAdm.CONTRATO_REVENTA = "N";

                pedidoAdm.EMBARCAR_A = paci.CLIENTE.ALIAS;
                pedidoAdm.DIRECCION_FACTURA = paci.CLIENTE.DIRECCION;

                if (ID_FINANC == string.Empty)
                {
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(cliente.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = paci.CLIENTE.ALIAS;
                    pedidoAdm.PAIS = cliente.PAIS;
                    pedidoAdm.NIVEL_PRECIO = cliente.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = cliente.CONDICION_PAGO;
                }
                else
                {
                    CLIENTE financiador = db.CLIENTE.Find(ID_FINANC);
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(financiador.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = financiador.ALIAS;
                    pedidoAdm.PAIS = financiador.PAIS;
                    pedidoAdm.NIVEL_PRECIO = financiador.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = financiador.CONDICION_PAGO;
                }
                pedidoAdm.USUARIO = "ERPADMIN";
                pedidoAdm.SUBTIPO_DOC_CXC = 0;
                pedidoAdm.BODEGA = Bodega_gral;
                pedidoAdm.RecordDate = DateTime.Now;
                pedidoAdm.CreateDate = DateTime.Now;
                pedidoAdm.RowPointer = Guid.NewGuid();
                pedidoAdm.CreatedBy = "ERPADMIN";
                pedidoAdm.UpdatedBy = "ERPADMIN";
                pedidoAdm.TIPO_DOCUMENTO = "P";
                db.PEDIDO.Add(pedidoAdm);

                //Agregar Admisión
                db.CS_ADMISION.Add(admision);

                //Actualizar Pedido Consecutivo
                CONSECUTIVO_FA cons = db.CONSECUTIVO_FA.Where(c => c.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                cons.VALOR_CONSECUTIVO = admision.PEDIDO;
                db.Entry(cons).State = EntityState.Modified;
                sexo = paci.SEXO;

               

                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            db.SaveChanges();
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
                //try
                //{
                //    db.SaveChanges();


                //}
                //catch (DbEntityValidationException ex)
                //{
                //    // Retrieve the error messages as a list of strings.
                //    var errorMessages = ex.EntityValidationErrors
                //            .SelectMany(x => x.ValidationErrors)
                //            .Select(x => x.ErrorMessage);

                //    // Join the list to a single string.
                //    var fullErrorMessage = string.Join("; ", errorMessages);

                //    // Combine the original exception message with the new one.
                //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //    // Throw a new DbEntityValidationException with the improved exception message.
                //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                //}
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, sexo = sexo, idadmision = admision.ID_ADMISION, pedido = admision.PEDIDO } };
        }
        [HttpPost]
        public JsonResult NuevoLaboratorio(CS_LABORATORIO laboratorio, string sexo, int idadmision, string pedido)
        {

            //MODIFICAR LAS TABLAS PARA IMAGEN
            var status = true;
            int version = 0;
            decimal total = 0;
            decimal precio = 0;
            int count = 0;

            try
            {
                using (var context = new HospitalarioBD())
                {


                    var dato_admision = (from p in db.CS_ADMISION where p.ID_ADMISION == idadmision select p).FirstOrDefault();

                    var dato_paciente = (from p in db.CS_PACIENTES where p.CLIENTE_ID == laboratorio.CLIENTE_ID select p).FirstOrDefault();

                    int eage = DateTime.Today.AddTicks(-Convert.ToDateTime(dato_paciente.FECHA_NACIMIENTO).Ticks).Year - 1;

                    //verificamos la linea del pedido por si fue modifcado desde el pedido
                    var linea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList();

                    if (linea.Count > 0)
                    {
                        var contador_linea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList().Max(z => z.PEDIDO_LINEA1);

                        count = contador_linea;
                    }

                    if (count >= laboratorio.PEDIDO_LINEA)
                    {
                        laboratorio.PEDIDO_LINEA = count + 1;
                    }
                    //else
                    //{
                    //    laboratorio.PEDIDO_LINEA = count + 1;
                    //}

                    if (string.IsNullOrEmpty(sexo))
                    { sexo = dato_paciente.SEXO; }


                    //CS_IMAGENOLOGIA img = new CS_IMAGENOLOGIA();
                    //img.CLIENTE_ID = imagen.CLIENTE_ID;
                    //img.ARTICULO = imagen.ARTICULO;
                    //img.CANTIDAD = imagen.CANTIDAD;
                    //img.ID_ADMISION = idadmision;
                    //img.PEDIDO = pedido;
                    //img.PEDIDO_LINEA = imagen.PEDIDO_LINEA;
                    //img.SEXO = sexo;
                    //img.EDAD = eage;
                    //img.IMPRESO = false;
                    //img.ENV_POR_CORREO = false;
                    //img.USUARIO_REGISTRO = util.GetUser();
                    //img.FECHA_REGISTRO = imagen.FECHA_REGISTRO;
                    //img.STATUS = 1096;
                    //img.STAT = imagen.STAT;
                    //img.LECTURA = imagen.LECTURA;
                    //img.ANULADO = imagen.ANULADO;
                    //context.CS_IMAGENOLOGIA.Add(img);
                    //context.SaveChanges();

                    laboratorio.SEXO = sexo;
                    laboratorio.DIAGNOSTICO_ID = 0;
                    laboratorio.EDAD_AL_TOMAR_MUESTRA = eage;
                    laboratorio.USUARIO_CREACION = util.GetUser();
                    laboratorio.ESTADO = "P";
                    laboratorio.ADMISION = idadmision;
                    laboratorio.PEDIDO = pedido;
                    laboratorio.IMPRESO = false;
                    laboratorio.ENV_POR_CORREO = false;
            

                    //CS_LABORATORIO lab = new CS_LABORATORIO();
                    //lab.EXAMEN_ID = laboratorio.EXAMEN_ID;
                    //lab.STAT = laboratorio.STAT;
                    //lab.CLIENTE_ID = laboratorio.CLIENTE_ID;
                    //lab.ACTIVO = laboratorio.ACTIVO;
                    //lab.SEXO = sexo;
                    //lab.FECHA_CREACION = DateTime.Now;
                    //lab.USUARIO_CREACION = 0;
                    //lab.ESTADO = "P";
                    //lab.ADMISION = idadmision;
                    //lab.USUARIO_CREACION = util.GetUser();
                    context.CS_LABORATORIO.Add(laboratorio);
                    context.SaveChanges();




                    var pedidobd = db.PEDIDO.Where(x => x.PEDIDO1 == pedido).FirstOrDefault();

                    var versionnivel = db.SP_Listado_precio_articulo(laboratorio.ARTICULO, pedidobd.NIVEL_PRECIO, DateTime.Now).FirstOrDefault();

                    context.SP_Insertar_LineaPedido(laboratorio.NUM_MUESTRA, pedido, laboratorio.PEDIDO_LINEA, pedidobd.BODEGA, laboratorio.ARTICULO, laboratorio.FECHA_CREACION, versionnivel.PRECIO, laboratorio.CANTIDAD, laboratorio.CANTIDAD, laboratorio.FECHA_CREACION, "ERPADMIN", "ERPADMIN");
                    version = versionnivel.VERSION;
                    precio = versionnivel.PRECIO * laboratorio.CANTIDAD;
                    total += precio;

               
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, version = version, total = total } };
        
        
        }
        [HttpPost]
        public JsonResult NuevoLaboratorioSolo(CS_LABORATORIO laboratorio, int idadmision, string pedido)
        {
            var status = true;
            int version = 0;
            decimal total = 0;
            decimal precio = 0;
            try
            {
                using (var context = new HospitalarioBD())
                {
                    int IdPaciente = Convert.ToInt32(laboratorio.CLIENTE_ID);
                    var paciente = db.CS_PACIENTES.Where(x => x.ID_PACIENTE == IdPaciente).FirstOrDefault();
                    int codlab = Convert.ToInt32(context.CS_LABORATORIO.OrderByDescending(p => p.NUM_MUESTRA).FirstOrDefault().NUM_MUESTRA);
                    CS_LABORATORIO lab = new CS_LABORATORIO();
                    lab.ARTICULO = laboratorio.ARTICULO;
                    lab.STAT = laboratorio.STAT;
                    lab.CLIENTE_ID = paciente.CLIENTE_ID;
                    lab.ACTIVO = laboratorio.ACTIVO;
                    lab.SEXO = paciente.SEXO;
                    lab.FECHA_RECEPCION = DateTime.Now;
                    lab.USUARIO_RECEPCION = 0;
                    lab.ESTADO = "P";
                    lab.ADMISION = idadmision;
                    lab.USUARIO_CREACION = util.GetUser();
                    context.CS_LABORATORIO.Add(lab);
                    context.SaveChanges();

                    var pedidobd = db.PEDIDO.Where(x => x.PEDIDO1 == pedido).FirstOrDefault();
                    var examen = db.CS_DEFINICION_PROCEDIMIENTO.Where(x => x.ARTICULO == laboratorio.ARTICULO).FirstOrDefault();

                    var versionnivel = db.SP_Listado_precio_articulo(examen.ARTICULO, pedidobd.NIVEL_PRECIO, DateTime.Now).FirstOrDefault();

                  //  context.SP_Insertar_Laboratorio(codlab++, pedido, pedidobd.BODEGA, examen.ARTICULO, DateTime.Now, versionnivel.PRECIO, 1, 1, DateTime.Now, "ERPADMIN", "ERPADMIN");
                    version = versionnivel.VERSION;
                    precio = versionnivel.PRECIO * 1;
                    total += precio;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, version = version, total = total } };
        }
        [HttpPost]
        public JsonResult EliminarLaboratorio(int laboratorio)
        {
            var status = true;
            try
            {
                var laboratoriodb = db.CS_LABORATORIO.Find(laboratorio);
                laboratoriodb.FECHA_ANULACION = DateTime.Now;
                laboratoriodb.USUARIO_ANULACION = util.GetUser();
                laboratoriodb.ACTIVO = false;
                laboratoriodb.FECHA_ANULACION = DateTime.Now;

                var pedidolinea = db.PEDIDO_LINEA.Where(x => x.NUM_MUESTRA == laboratorio).FirstOrDefault();
                db.Entry(pedidolinea).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult MontoPedidoLaboratorio(string pedido)
        {

           var status = true;
            try
            {
                using (var context = new HospitalarioBD())
                {
                    decimal cantart = 0;
                    decimal totalpedido = 0;
                    var pedidobd = context.PEDIDO.Where(x => x.PEDIDO1 == pedido).FirstOrDefault();
                    var lispedido = context.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList();

                    foreach (var item in lispedido)
                    {
                        totalpedido += item.PRECIO_UNITARIO * item.CANTIDAD_PEDIDA;
                        cantart += Convert.ToDecimal(item.CANTIDAD_PEDIDA);
                    }

                    context.SP_Actualizar_pedido_monto_paciente(pedido, pedidobd.VERSION_NP, totalpedido, cantart);
                }
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };

        }
        [HttpGet]
        public ActionResult EditarAdmisionLaboratorio(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AdmisionLaboratorio");
            }
            CS_ADMISION admision = db.CS_ADMISION.Find(id);
            if (admision == null)
            {
                return HttpNotFound();
            }
            var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
            {
                ID_PACIENTE = x.CLIENTE_ID,
                CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
            }).ToList();
            var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
            {
                ID_MEDICO = x.ID_MEDICO,
                MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
            }).ToList();
            //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "ID_PACIENTE", "CLIENTE_ID");
            ViewBag.PACIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID", admision.CLIENTE_ID);
            ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO", admision.DOCTOR_ID);
            //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "ID_PACIENTE", "CLIENTE_ID", admision.PACIENTE_ID);

            ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10), "ID_CAT_DETALLE", "DESCRIPCION", admision.PRIORIDAD_ID);
            ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.AREA_SERVICIO_ID);
            ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_INGRESO_ID);
            ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.CAUSA_ADMISION_ID);
            ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE", admision.FINANCIADOR);
            ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_SEGURO);
            ViewBag.ID_EXAMEN = new SelectList(db.SpListadoExamen(), "ID_EXAMEN", "EXAMEN");
            ViewBag.EXAMENES_LABORATORIO = db.SpListadoExamenAdmisionLaboratorio(id).ToList();
            ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION",admision.REMITIDO);
            ViewBag.Title = "Editar Admisión Laboratorio";
            ViewBag.Accion = "Editar";
            return View(admision);
        }


        [HttpGet]
        public ActionResult EditarAdmision(int? id)
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                if (id == null)
                {
                    return RedirectToAction(nameof(Admisiones));
                }
                CS_ADMISION admision = db.CS_ADMISION.Find(id);
                if (admision == null)
                {
                    return HttpNotFound();
                }
                var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_PACIENTE = x.CLIENTE_ID,
                    CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();
                var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_MEDICO = x.ID_MEDICO,
                    MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();
                //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "ID_PACIENTE", "CLIENTE_ID");
                ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID", admision.CLIENTE_ID);
                ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO", admision.DOCTOR_ID);
                //ViewBag.PACIENTE_ID = new SelectList(db.CS_PACIENTES.ToList().Where(p => p.ACTIVO == true), "ID_PACIENTE", "CLIENTE_ID", admision.PACIENTE_ID);

                var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

                ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10), "ID_CAT_DETALLE", "DESCRIPCION", admision.PRIORIDAD_ID);
                ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.AREA_SERVICIO_ID);
                ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_INGRESO_ID);
                ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.CAUSA_ADMISION_ID);
                ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE", admision.FINANCIADOR);
                ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_SEGURO);
                ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.REMITIDO);
                ViewBag.Title = "Editar Admisión";
                ViewBag.Accion = "Editar";
                return View("NuevoEditarAdmision", admision);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }
        [HttpPost]
        public ActionResult EditarAdmision([Bind(Include = "ID_ADMISION, CLIENTE_ID,DOCTOR_ID, NUM_AFILIADO, FINANCIADOR, TIPO_SEGURO, PRIORIDAD_ID, CAUSA_ADMISION_ID, TIPO_INGRESO_ID, AREA_SERVICIO_ID, PEDIDO, ASEGURADO, REMITIDO")] CS_ADMISION admision, string ID_FINANC)
        {

            Debug.WriteLine(ID_FINANC);
            Debug.WriteLine(admision.TIPO_SEGURO);
            Debug.WriteLine(admision.NUM_AFILIADO);
            Debug.WriteLine(admision.FINANCIADOR);


            if (ModelState.IsValid)
            {
                CS_ADMISION admi = db.CS_ADMISION.Find(admision.ID_ADMISION);

                admi.PRIORIDAD_ID = admision.PRIORIDAD_ID;
                admi.REMITIDO = admision.REMITIDO;
                admi.CLIENTE_ID = admision.CLIENTE_ID;
                admi.CAUSA_ADMISION_ID = admision.CAUSA_ADMISION_ID;
                admi.TIPO_INGRESO_ID = admision.TIPO_INGRESO_ID;
                admi.AREA_SERVICIO_ID = admision.AREA_SERVICIO_ID;
                admi.PEDIDO = admision.PEDIDO;
                admi.DOCTOR_ID = admision.DOCTOR_ID;
                if (admision.ASEGURADO)
                {
                    admi.FINANCIADOR = ID_FINANC;
                    admi.TIPO_SEGURO = admision.TIPO_SEGURO;
                    admi.NUM_AFILIADO = admision.NUM_AFILIADO;
                }
                else
                {
                    CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();
                    admi.FINANCIADOR = paci.CLIENTE_ID;
                    admi.TIPO_SEGURO = null;
                    admi.NUM_AFILIADO = null;
                }

                admi.ASEGURADO = admision.ASEGURADO;
                //AGREGAR CONSULTA
                // CS_PACIENTES paciente = db.CS_PACIENTES.Find(admi.CLIENTE_ID);
                CS_CONSULTAS consulta = db.CS_CONSULTAS.Where(x => x.ID_ADMISION == admi.ID_ADMISION).FirstOrDefault();

                consulta.CLIENTE_ID = admi.CLIENTE_ID;
                consulta.DOCTOR_ID = admi.DOCTOR_ID;
                consulta.AREA_SERVICIO_ID = admi.AREA_SERVICIO_ID;
                consulta.ID_ADMISION = admi.ID_ADMISION;

                db.Entry(consulta).State = EntityState.Modified;



                //MODIFICAMOS EL PEDIDO
                PEDIDO pedidoAdm = db.PEDIDO.Where(x => x.PEDIDO1 == admi.PEDIDO).FirstOrDefault();
                if (!admision.ASEGURADO)
                {
                  CLIENTE  paci = db.CLIENTE.Find(admision.CLIENTE_ID);
                    pedidoAdm.NOMBRE_CLIENTE = paci.ALIAS;
                    pedidoAdm.PAIS = paci.PAIS;
                    pedidoAdm.NIVEL_PRECIO = paci.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = util.GetVersionPrecio(paci.NIVEL_PRECIO);
                    pedidoAdm.CONDICION_PAGO = paci.CONDICION_PAGO;
                    pedidoAdm.CLIENTE = paci.CLIENTE1;
                    pedidoAdm.CLIENTE_DIRECCION = paci.CLIENTE1;
                    pedidoAdm.CLIENTE_CORPORAC = paci.CLIENTE1;
                    pedidoAdm.CLIENTE_ORIGEN = paci.CLIENTE1;
                    pedidoAdm.RUBRO1 = paci.CLIENTE1;
                    pedidoAdm.EMBARCAR_A = paci.ALIAS;
                    pedidoAdm.DIRECCION_FACTURA = paci.DIRECCION;
                    pedidoAdm.DIREC_EMBARQUE = paci.DIR_EMB_DEFAULT;
                }
                else
                {
                    CLIENTE financiador = db.CLIENTE.Find(ID_FINANC);
                    pedidoAdm.NOMBRE_CLIENTE = financiador.ALIAS;
                    pedidoAdm.PAIS = financiador.PAIS;
                    pedidoAdm.NIVEL_PRECIO = financiador.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = util.GetVersionPrecio(financiador.NIVEL_PRECIO); ;
                    pedidoAdm.CONDICION_PAGO = financiador.CONDICION_PAGO;
                    pedidoAdm.CLIENTE = financiador.CLIENTE1;
                    pedidoAdm.CLIENTE_DIRECCION = financiador.CLIENTE1;
                    pedidoAdm.CLIENTE_CORPORAC = financiador.CLIENTE1;
                    pedidoAdm.CLIENTE_ORIGEN = financiador.CLIENTE1;
                    pedidoAdm.RUBRO1 = financiador.CLIENTE1;
                    pedidoAdm.EMBARCAR_A = financiador.ALIAS;
                    pedidoAdm.DIRECCION_FACTURA = financiador.DIRECCION;
                    pedidoAdm.DIREC_EMBARQUE = financiador.DIR_EMB_DEFAULT;

                }

                db.Entry(admi).State = EntityState.Modified;
                db.Entry(pedidoAdm).State = EntityState.Modified;


                //aca reclaculamos los precios del pedido
                var  ped_det = (from p in db.PEDIDO_LINEA.Where(x => x.PEDIDO == admision.PEDIDO) select p).ToList();
                if (ped_det.Count > 0)
                {

                }
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
                return RedirectToAction(nameof(Admisiones));
            }

            return View(admision);
        }
        [HttpGet]
        public ActionResult Admisiones()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.SIGNOS_VITALES != null).OrderByDescending(a => a.FECHA_REGISTRO));
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return Redirect("/Login/Index");
            }
        }
        private int itemcatalogo_odontologia()
        {
            var laboratorioad = db.CS_GLOBAL_ODONTO.First();
            return laboratorioad.ID_ITEM_CATALOGO ?? 0;
        }
        [HttpGet]
        public ActionResult SignosVitales(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Admisiones));
            }

            CS_ADMISION admi = db.CS_ADMISION.Find(id);

            if (admi == null)
            {
                return HttpNotFound();
            }


            ViewBag.ADMISION_ID = admi.ID_ADMISION;
            ViewBag.PACIENTE = admi.CLIENTE.ALIAS;
            ViewBag.DOCTOR_ID = admi.DOCTOR_ID;
            ViewBag.CLIENTE_ID = admi.CLIENTE_ID;
            ViewBag.AREA_SERVICIO_ID = admi.AREA_SERVICIO_ID;
            ViewBag.TIPO = admi.ASEGURADO ? "Asegurado" : "Privado";
            ViewBag.ITEM_ODONTO = itemcatalogo_odontologia();


            ViewBag.Title = "Registro de Signos Vitales";
            return View("RegistrarSignosVitales");
        }
        [HttpPost]
        public ActionResult SignosVitales( CS_SIGNOS_VITALES signosVitales)
        {
            //if (ModelState.IsValid)
            //{
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

            return RedirectToAction(nameof(Admisiones));
            //}
            //return View(signosVitales);
        }
        public PartialViewResult DetalleSignosVitales(int? id)
        {
            CS_SIGNOS_VITALES signos = db.CS_SIGNOS_VITALES.Where(s => s.ADMISION_ID == id).FirstOrDefault();
            ViewBag.Title = "Detalle de Signos Vitales";
            return PartialView("_DetalleSignosVitales", signos);
        }
        public PartialViewResult Top10Admisiones()
        {
            List<CS_ADMISION> top10Admis = db.CS_ADMISION.Where(x => x.ACTIVO == true && x.SIGNOS_VITALES != null).OrderByDescending(item => item.FECHA_RECEPCION).Take(10).ToList();

            return PartialView("_Top10Admisiones", top10Admis);
        }
        public PartialViewResult Top10Pacientes()
        {
            List<CS_PACIENTES> top10Pacis = db.CS_PACIENTES.Where(x => x.ACTIVO == true).OrderByDescending(item => item.FECHA_REGISTRO).Take(10).ToList();
            return PartialView("_Top10Pacientes", top10Pacis);
        }
        public PartialViewResult Popup_BorrarPaciente(int id)
        {
            CS_PACIENTES pacliente = db.CS_PACIENTES.Where(i => i.ID_PACIENTE== id).FirstOrDefault();
            ViewBag.Title = "Borrar Paciente";
            return PartialView("_BorrarPaciente", pacliente);
        }
        public PartialViewResult Popup_DetalleSignosVitales(int id)
        {
            CS_SIGNOS_VITALES signos = db.CS_SIGNOS_VITALES.Where(s => s.ADMISION_ID == id).FirstOrDefault();

            ViewBag.TipoSeguro = signos.CS_ADMISION.ASEGURADO ? "Asegurado" : "Privado";

            ViewBag.Title = "Detalle de Signos Vitales";
            return PartialView("_DetalleSignosVitales", signos);
        }

        #region Admision Imagen

        [HttpGet]
        public ActionResult AdmisionImagen()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var imagenad = db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.AREA_SERVICIO_ID == 27 && x.TIPO_INGRESO_ID == 2163 && x.SIGNOS_VITALES == null).OrderByDescending(a => a.FECHA_REGISTRO).ToList();
                return View(imagenad);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }

        [HttpGet]
        public ActionResult NuevaAdmisionImagen()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                if (consecutivoPedido != null)
                {
                    ViewBag.NUM_PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                }
                else
                {
                    ViewBag.NUM_PEDIDO = "-";
                }
                var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_PACIENTE = x.CLIENTE_ID,
                    CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();
                var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_MEDICO = x.ID_MEDICO,
                    MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();

                var examen = (from e in db.CS_DEFINICION_PROC_IMAGEN.Where(x=>x.ACTIVO==true)
                              join a in db.ARTICULO on e.ARTICULO equals a.ARTICULO1
                              select new {ARTICULO=e.ARTICULO, DESCRIPCION=a.DESCRIPCION }).ToList();

                var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

                ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID");
                ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO");
                ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10 && p.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CLIENTE1.Contains(parameto_seg)), "CLIENTE1", "NOMBRE").Take(10);
                ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.ID_EXAMEN = new SelectList(examen, "ARTICULO", "DESCRIPCION");
                ViewBag.CANTIDAD = 1;
                return View();
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }

        [HttpPost]
        public JsonResult NuevaAdmisionImagen(CS_ADMISION admision, string ID_FINANC)
        {
            var status = true;
            string sexo = string.Empty;

            try
            {
                var parametros = db.CS_PARAMETROS.FirstOrDefault();

                //Validamos si aun no existe el pedido

                PEDIDO ped = db.PEDIDO.Where(x => x.PEDIDO1 == admision.PEDIDO).FirstOrDefault();
                if (ped != null)
                {
                    CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                    if (consecutivoPedido != null)
                    {
                        admision.PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                    }
                }



                admision.FECHA_RECEPCION = DateTime.Now;
                admision.FECHA_REGISTRO = DateTime.Now;
                admision.USUARIO_REGISTRO = util.GetUser();
                admision.ATENDIDO = false;
                admision.ACTIVO = true;
                admision.DOCTOR_ID = admision.DOCTOR_ID;
                admision.AREA_SERVICIO_ID = 27;
                admision.TIPO_INGRESO_ID = 2163;
                admision.SIGNOS_VITALES = null;
                admision.TIPO_ALTA = parametros.ALTA_DEFAULT;
                admision.FECHA_ALTA = DateTime.Now;
                
                //admision.CAUSA_ADMISION_ID

                CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();

                if (admision.ASEGURADO)
                {
                    admision.FINANCIADOR = ID_FINANC;
                }
                else
                {
                    admision.FINANCIADOR = paci.CLIENTE_ID;
                    admision.TIPO_SEGURO = null;
                    admision.NUM_AFILIADO = null;
                }
                ////AGREGAR CONSULTA
              //  CS_PACIENTES paciente = db.CS_PACIENTES.Find(admision.CLIENTE_ID);
                CLIENTE cliente = db.CLIENTE.Find(admision.CLIENTE_ID);

                //buscamos informacion del nivel 
                VERSION_NIVEL dato_version = (db.VERSION_NIVEL.Where(x => x.NIVEL_PRECIO == cliente.NIVEL_PRECIO)).FirstOrDefault();

                //Agregar Pedido
                PEDIDO pedidoAdm = new PEDIDO();
                pedidoAdm.PEDIDO1 = admision.PEDIDO;
                pedidoAdm.ESTADO = "N";
                pedidoAdm.DIREC_EMBARQUE = "ND";
                pedidoAdm.TIPO_PEDIDO = "N";
                pedidoAdm.MONEDA_PEDIDO = dato_version.MONEDA;
                pedidoAdm.AUTORIZADO = "N";
                pedidoAdm.DOC_A_GENERAR = "F";
                pedidoAdm.CLASE_PEDIDO = "N";
                pedidoAdm.MONEDA = dato_version.MONEDA;
                pedidoAdm.COBRADOR = "ND";
                pedidoAdm.RUTA = "ND";
                pedidoAdm.ZONA = "ND";
                pedidoAdm.VENDEDOR = "ND";
                pedidoAdm.TIPO_DOC_CXC = "FAC";
                pedidoAdm.BACKORDER = "N";
                pedidoAdm.DESCUENTO_CASCADA = "N";
                pedidoAdm.FIJAR_TIPO_CAMBIO = "N";
                pedidoAdm.IMPRESO = "N";
                pedidoAdm.ORIGEN_PEDIDO = "F";
                pedidoAdm.TIPO_DESCUENTO1 = "P";
                pedidoAdm.TIPO_DESCUENTO2 = "P";
                pedidoAdm.FECHA_ULT_EMBARQUE = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ULT_CANCELAC = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ORDEN = DateTime.Now;
                pedidoAdm.TARJETA_CREDITO = String.Empty;
                pedidoAdm.DIRECCION_FACTURA = "DETALLE:" + cliente.DIRECCION;
                pedidoAdm.PORC_INTCTE = 0;
                pedidoAdm.BASE_IMPUESTO1 = 0;
                pedidoAdm.BASE_IMPUESTO2 = 0;
                pedidoAdm.FECHA_PROYECTADA = DateTime.Now;
                pedidoAdm.TASA_IMPOSITIVA_PORC = 0;
                pedidoAdm.TASA_CREE1_PORC = 0;
                pedidoAdm.TASA_CREE2_PORC = 0;
                pedidoAdm.TASA_GAN_OCASIONAL_PORC = 0;
                pedidoAdm.CONTRATO_REVENTA = "N";
                pedidoAdm.RUBRO1 = paci.ID_PACIENTE.ToString();

                pedidoAdm.MONTO_ANTICIPO = 0;
                pedidoAdm.MONTO_FLETE = 0;
                pedidoAdm.MONTO_SEGURO = 0;
                pedidoAdm.MONTO_DOCUMENTACIO = 0;
                pedidoAdm.MONTO_DESCUENTO1 = 0;
                pedidoAdm.MONTO_DESCUENTO2 = 0;
                pedidoAdm.TOTAL_MERCADERIA = 0;
                pedidoAdm.TOTAL_IMPUESTO1 = 0;
                pedidoAdm.TOTAL_IMPUESTO2 = 0;
                pedidoAdm.TOTAL_A_FACTURAR = 0;
                pedidoAdm.TOTAL_CANCELADO = 0;
                pedidoAdm.TOTAL_UNIDADES = 0;
                pedidoAdm.PORC_DESCUENTO1 = 0;
                pedidoAdm.PORC_DESCUENTO2 = 0;
                pedidoAdm.PORC_COMI_COBRADOR = 0;
                pedidoAdm.PORC_COMI_VENDEDOR = 0;
                pedidoAdm.OBSERVACIONES = "Pedido generado desde el sistema Hospitalario AH";


                pedidoAdm.FECHA_PEDIDO = DateTime.Now;
                pedidoAdm.FECHA_PROMETIDA = DateTime.Now;
                pedidoAdm.FECHA_PROX_EMBARQU = DateTime.Now;
                pedidoAdm.FECHA_HORA = DateTime.Now;

                pedidoAdm.CLIENTE = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_CORPORAC = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_ORIGEN = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_DIRECCION = admision.FINANCIADOR;
                pedidoAdm.RUBRO1 = admision.CLIENTE_ID;

                pedidoAdm.CONTRATO_REVENTA = "N";

                pedidoAdm.EMBARCAR_A = paci.CLIENTE.ALIAS;
                pedidoAdm.DIRECCION_FACTURA = paci.CLIENTE.DIRECCION;

                if (ID_FINANC == string.Empty)
                {
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(cliente.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = paci.CLIENTE.ALIAS;
                    pedidoAdm.PAIS = cliente.PAIS;
                    pedidoAdm.NIVEL_PRECIO = cliente.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = cliente.CONDICION_PAGO;
                }
                else
                {
                    CLIENTE financiador = db.CLIENTE.Find(ID_FINANC);
                    //buscamos la version
                    int version_nivel = util.GetVersionPrecio(financiador.NIVEL_PRECIO);
                    pedidoAdm.NOMBRE_CLIENTE = financiador.ALIAS;
                    pedidoAdm.PAIS = financiador.PAIS;
                    pedidoAdm.NIVEL_PRECIO = financiador.NIVEL_PRECIO;
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = version_nivel;
                    pedidoAdm.CONDICION_PAGO = financiador.CONDICION_PAGO;
                }
                pedidoAdm.USUARIO = "ERPADMIN";
                pedidoAdm.SUBTIPO_DOC_CXC = 0;
                pedidoAdm.BODEGA = Bodega_gral;
                pedidoAdm.RecordDate = DateTime.Now;
                pedidoAdm.CreateDate = DateTime.Now;
                pedidoAdm.RowPointer = Guid.NewGuid();
                pedidoAdm.CreatedBy = "ERPADMIN";
                pedidoAdm.UpdatedBy = "ERPADMIN";
                pedidoAdm.TIPO_DOCUMENTO = "P";
                db.PEDIDO.Add(pedidoAdm);

                //Agregar Admisión
                db.CS_ADMISION.Add(admision);

                //Actualizar Pedido Consecutivo
                CONSECUTIVO_FA cons = db.CONSECUTIVO_FA.Where(c => c.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                cons.VALOR_CONSECUTIVO = admision.PEDIDO;
                db.Entry(cons).State = EntityState.Modified;
                sexo = paci.SEXO;
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            db.SaveChanges();

                           
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
                //try
                //{
                //    db.SaveChanges();


                //}
                //catch (DbEntityValidationException ex)
                //{
                //    // Retrieve the error messages as a list of strings.
                //    var errorMessages = ex.EntityValidationErrors
                //            .SelectMany(x => x.ValidationErrors)
                //            .Select(x => x.ErrorMessage);

                //    // Join the list to a single string.
                //    var fullErrorMessage = string.Join("; ", errorMessages);

                //    // Combine the original exception message with the new one.
                //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //    // Throw a new DbEntityValidationException with the improved exception message.
                //    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                //}
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, sexo = sexo, idadmision = admision.ID_ADMISION, pedido = admision.PEDIDO } };
        }

        [HttpPost]
        public JsonResult NuevoImagen(CS_IMAGENOLOGIA imagen, string sexo, int idadmision, string pedido)
        {

            //MODIFICAR LAS TABLAS PARA IMAGEN
            var status = true;
            int version = 0;
            decimal total = 0;
            decimal precio = 0;
            int count = 0;
            
            try
            {
                using (var context = new HospitalarioBD())
                {
            
                   
                    var dato_admision = (from p in db.CS_ADMISION where p.ID_ADMISION == idadmision select p).FirstOrDefault();

                    var dato_paciente = (from p in db.CS_PACIENTES where p.CLIENTE_ID== imagen.CLIENTE_ID select p).FirstOrDefault();

                    int eage = DateTime.Today.AddTicks(-Convert.ToDateTime(dato_paciente.FECHA_NACIMIENTO).Ticks).Year - 1;

                    //verificamos la linea del pedido por si fue modifcado desde el pedido
                    var linea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList();

                    if (linea.Count > 0)
                    {
                        var contador_linea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList().Max(z => z.PEDIDO_LINEA1);

                        count = contador_linea;
                    }

                    if (count >= imagen.PEDIDO_LINEA)
                    {
                        imagen.PEDIDO_LINEA = count + 1;
                    }
                    else
                    {
                        imagen.PEDIDO_LINEA = count + 1;
                    }

                    if (string.IsNullOrEmpty(sexo))
                    { sexo = dato_paciente.SEXO; }

                    CS_IMAGENOLOGIA img = new CS_IMAGENOLOGIA();
                    img.CLIENTE_ID = imagen.CLIENTE_ID;
                    img.ARTICULO = imagen.ARTICULO;
                    img.CANTIDAD = imagen.CANTIDAD;
                    img.ID_ADMISION = idadmision;
                    img.PEDIDO = pedido;
                    img.PEDIDO_LINEA = imagen.PEDIDO_LINEA;
                    img.SEXO =sexo;
                    img.EDAD = eage;
                    img.IMPRESO = false;
                    img.ENV_POR_CORREO = false;
                    img.USUARIO_REGISTRO = util.GetUser();
                    img.FECHA_REGISTRO = imagen.FECHA_REGISTRO;
                    img.STATUS = 1096;
                    img.STAT = imagen.STAT;
                    img.LECTURA = imagen.LECTURA;
                    img.ANULADO = imagen.ANULADO;
                    context.CS_IMAGENOLOGIA.Add(img);
                    context.SaveChanges();


                    var pedidobd = db.PEDIDO.Where(x => x.PEDIDO1 == pedido).FirstOrDefault();
         
                    var versionnivel = db.SP_Listado_precio_articulo(img.ARTICULO, pedidobd.NIVEL_PRECIO, DateTime.Now).FirstOrDefault();

                    context.SP_Insertar_LineaPedido(img.ID_IMAGENOLOGIA, pedido,img.PEDIDO_LINEA ,pedidobd.BODEGA, img.ARTICULO,imagen.FECHA_REGISTRO, versionnivel.PRECIO, imagen.CANTIDAD, imagen.CANTIDAD, imagen.FECHA_REGISTRO, "ERPADMIN", "ERPADMIN");
                    version = versionnivel.VERSION;
                    precio = versionnivel.PRECIO * imagen.CANTIDAD;
                    total += precio;


                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, version = version, total = total } };
        }


        [HttpPost]
        public JsonResult EliminarImagen(int imagen)
        {
            var status = true;
            try
            {
                var imagendb = db.CS_IMAGENOLOGIA.Find(imagen);

                var pedidolinea = db.PEDIDO_LINEA.Where(x => x.PEDIDO == imagendb.PEDIDO && x.PEDIDO_LINEA1 == imagendb.PEDIDO_LINEA && x.ARTICULO == imagendb.ARTICULO).FirstOrDefault();

                

                imagendb.FECHA_ANULACION = DateTime.Now;
                imagendb.USUARIO_ANULACION = util.GetUsuario();
                imagendb.ANULADO = "S";
                imagendb.FECHA_ANULACION = DateTime.Now;

                
                db.Entry(pedidolinea).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult MontoPedidoImagen(string pedido)
        {
            var status = true;
            try
            {
                using (var context = new HospitalarioBD())
                {
                    decimal cantart = 0;
                    decimal totalpedido = 0;
                    var pedidobd = context.PEDIDO.Where(x => x.PEDIDO1 == pedido).FirstOrDefault();
                    var lispedido = context.PEDIDO_LINEA.Where(x => x.PEDIDO == pedido).ToList();

                    foreach (var item in lispedido)
                    {
                        totalpedido += item.PRECIO_UNITARIO*item.CANTIDAD_PEDIDA;
                        cantart+=Convert.ToDecimal(item.CANTIDAD_PEDIDA);
                    }

                    context.SP_Actualizar_pedido_monto_paciente(pedido, pedidobd.VERSION_NP, totalpedido, cantart);
                }
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };

        }

        [HttpGet]
        public ActionResult EditarAdmisionImagen(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AdmisionImagen");
            }
            CS_ADMISION admision = db.CS_ADMISION.Find(id);
            if (admision == null)
            {
                return HttpNotFound();
            }
            var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
            {
                ID_PACIENTE = x.CLIENTE_ID,
                CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
            }).ToList();
            var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
            {
                ID_MEDICO = x.ID_MEDICO,
                MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
            }).ToList();

            var examen = (from e in db.CS_DEFINICION_PROC_IMAGEN.Where(x => x.ACTIVO == true)
                          join a in db.ARTICULO on e.ARTICULO equals a.ARTICULO1
                          select new { ARTICULO = e.ARTICULO, DESCRIPCION = a.DESCRIPCION }).ToList();

            var ordenes_hist = from a in db.CS_IMAGENOLOGIA.Where(x => x.ID_ADMISION == id)
                               join b in db.ARTICULO on a.ARTICULO equals b.ARTICULO1
                               join c in db.CS_CATALOGO_DETALLE on a.STATUS equals c.ID_CAT_DETALLE
                               select new OdenesHistViewModel

                               { ORDEN = a.ID_IMAGENOLOGIA, ARTICULO = a.ARTICULO, DESCRIPCION = b.DESCRIPCION,CANTIDAD=a.CANTIDAD, LECTURA = a.LECTURA, ANULADO = a.ANULADO, FECHA_ANULACION = a.FECHA_ANULACION, FECHA_LECTURA = a.FECHA_LECTURA, PEDIDO = a.PEDIDO, PEDIDO_LINEA = a.PEDIDO_LINEA, ESTADO = c.DESCRIPCION, STAT = a.STAT , FECHA_REGISTRO=a.FECHA_REGISTRO};

                              
            ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID", admision.CLIENTE_ID);
            ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO", admision.DOCTOR_ID);

            var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

            ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10), "ID_CAT_DETALLE", "DESCRIPCION", admision.PRIORIDAD_ID);
            ViewBag.AREA_SERVICIO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.AREA_SERVICIO_ID);
            ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_INGRESO_ID);
            ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.CAUSA_ADMISION_ID);
            ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(c => c.CLIENTE1.Contains(parameto_seg)), "CLIENTE1", "NOMBRE", admision.FINANCIADOR).Take(10);
            ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION", admision.TIPO_SEGURO);
            ViewBag.ID_EXAMEN = new SelectList(examen, "ARTICULO", "DESCRIPCION");
            ViewBag.EXAMENES_IMAGEN = db.SpListadoExamenAdmisionImagen(id).ToList();
            ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
            ViewBag.CANTIDAD = 1;
            ViewBag.Title = "Editar Admisión Imagen";
            ViewBag.Accion = "Editar";
            ViewBag.HISTORIAL = ordenes_hist.ToList();
            return View(admision);
        }
        [HttpPost]
        public JsonResult ExamenDetalleImagen(string examen)
        {

            var status = true;
          
            try
            {
               
                var detexamen = (from p in db.CS_DEFINICION_PROC_IMAGEN where p.ARTICULO==examen select new { DEF_IMG =p.DEF_IMG , ARTICULO=p.ARTICULO, OBSERVACION=p.OBSERVACION, DESCRIPCION=p.ARTICULO1.DESCRIPCION, FECHA_REGISTRO=DateTime.Now.ToString()}).FirstOrDefault();
                status = true;
                return new JsonResult { Data = new { status = status, detexamen = detexamen } };

            }
            catch
            {
                return new JsonResult { Data = new { status = status } };
            }

        }

        [HttpPost]
        public JsonResult ExamenDetalleImagenHist(int admision_id, string pedido)
        {

            var status = true;

            try
            {
      
                var detexamen = (db.spListadoArticulosPedios(admision_id)).ToList();
                status = true;
                return new JsonResult { Data = new { status = status, detexamen = detexamen } };

            }
            catch
            {
                return new JsonResult { Data = new { status = status } };
            }

        }

        #endregion

        #region AdmisionGeneral
        [HttpGet]
        public ActionResult AdmisionGeneral()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var imagenad = db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.AREA_SERVICIO_ID == 27 && x.TIPO_INGRESO_ID == 2163 && x.SIGNOS_VITALES == null).OrderByDescending(a => a.FECHA_REGISTRO).ToList();
                return View(imagenad);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }

        [HttpGet]
        public ActionResult NuevaAdmisionGeneral()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                CONSECUTIVO_FA consecutivoPedido = db.CONSECUTIVO_FA.Where(a => a.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                if (consecutivoPedido != null)
                {
                    ViewBag.NUM_PEDIDO = Utilerias.ProximoCodigo(consecutivoPedido.VALOR_CONSECUTIVO, Decimal.ToInt32(consecutivoPedido.LONGITUD));
                }
                else
                {
                    ViewBag.NUM_PEDIDO = "-";
                }
                var PACIENTE = db.CS_PACIENTES.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_PACIENTE = x.CLIENTE_ID,
                    CLIENTE_ID = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();
                var MEDICO = db.CS_MEDICOS.Where(p => p.ACTIVO == true).Select(x => new
                {
                    ID_MEDICO = x.ID_MEDICO,
                    MEDICO = x.ID_MEDICO + "-" + x.NOMBRES + " " + x.APELLIDOS
                }).ToList();

                var examen = (from e in db.CS_DEFINICION_PROC_IMAGEN.Where(x => x.ACTIVO == true)
                              join a in db.ARTICULO on e.ARTICULO equals a.ARTICULO1
                              select new { ARTICULO = e.ARTICULO, DESCRIPCION = a.DESCRIPCION }).ToList();

                var area_servicio = db.CS_CATALOGO_DETALLE.Where(x => x.ID_CATALOGO == 2 && (x.ID_CAT_DETALLE != 29 || x.ID_CAT_DETALLE != 30 || x.ID_CAT_DETALLE != 2180)).ToList();
                var parameto_seg = (from p in db.CS_PARAMETROS select p.CATEGORIA_SEGURO).FirstOrDefault();

                ViewBag.CLIENTE_ID = new SelectList(PACIENTE, "ID_PACIENTE", "CLIENTE_ID");
                ViewBag.DOCTOR_ID = new SelectList(MEDICO, "ID_MEDICO", "MEDICO");
                ViewBag.PRIORIDAD_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 10 && p.ACTIVO == true), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.CAUSA_ADMISION_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 4 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.FINANCIADOR = new SelectList(db.CLIENTE.Where(u => u.CATEGORIA_CLIENTE == "SEGUROS").ToList(), "CLIENTE1", "NOMBRE");
                ViewBag.TIPO_SEGURO = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 8), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.ID_EXAMEN = new SelectList(examen, "ARTICULO", "DESCRIPCION");
                ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.AREA_SERVICIO_ID = new SelectList(area_servicio, "ID_CAT_DETALLE", "DESCRIPCION");
                ViewBag.CANTIDAD = 1;
                return View();
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }

        [HttpPost]
        public JsonResult NuevaAdmisionGeneral(CS_ADMISION admision, string ID_FINANC)
        {
            var status = true;
            string sexo = string.Empty;

            try
            {
                var parametros = db.CS_PARAMETROS.FirstOrDefault();

                admision.FECHA_RECEPCION = DateTime.Now;
                admision.FECHA_REGISTRO = DateTime.Now;
                admision.USUARIO_REGISTRO = util.GetUser();
                admision.ATENDIDO = false;
                admision.ACTIVO = true;
                admision.DOCTOR_ID = admision.DOCTOR_ID;
                admision.AREA_SERVICIO_ID = 27;
                admision.TIPO_INGRESO_ID = 2163;
                admision.SIGNOS_VITALES = null;
                admision.TIPO_ALTA = parametros.ALTA_DEFAULT;
                admision.FECHA_ALTA = DateTime.Now;

                //admision.CAUSA_ADMISION_ID

                CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();

                if (admision.ASEGURADO)
                {
                    admision.FINANCIADOR = ID_FINANC;
                }
                else
                {
                    admision.FINANCIADOR = paci.CLIENTE_ID;
                    admision.TIPO_SEGURO = null;
                    admision.NUM_AFILIADO = null;
                }
                ////AGREGAR CONSULTA
                //  CS_PACIENTES paciente = db.CS_PACIENTES.Find(admision.CLIENTE_ID);
                CLIENTE cliente = db.CLIENTE.Find(admision.CLIENTE_ID);

                //Agregar Pedido
                PEDIDO pedidoAdm = new PEDIDO();
                pedidoAdm.PEDIDO1 = admision.PEDIDO;
                pedidoAdm.ESTADO = "N";
                pedidoAdm.DIREC_EMBARQUE = "ND";
                pedidoAdm.TIPO_PEDIDO = "N";
                pedidoAdm.MONEDA_PEDIDO = "L";
                pedidoAdm.AUTORIZADO = "N";
                pedidoAdm.DOC_A_GENERAR = "F";
                pedidoAdm.CLASE_PEDIDO = "N";
                pedidoAdm.MONEDA = "L";
                pedidoAdm.COBRADOR = "ND";
                pedidoAdm.RUTA = "ND";
                pedidoAdm.ZONA = "ND";
                pedidoAdm.VENDEDOR = "ND";
                pedidoAdm.TIPO_DOC_CXC = "FAC";
                pedidoAdm.BACKORDER = "N";
                pedidoAdm.DESCUENTO_CASCADA = "N";
                pedidoAdm.FIJAR_TIPO_CAMBIO = "N";
                pedidoAdm.IMPRESO = "N";
                pedidoAdm.ORIGEN_PEDIDO = "F";
                pedidoAdm.TIPO_DESCUENTO1 = "P";
                pedidoAdm.TIPO_DESCUENTO2 = "P";
                pedidoAdm.FECHA_ULT_EMBARQUE = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ULT_CANCELAC = Convert.ToDateTime("1980-01-01 00:00:00.000");
                pedidoAdm.FECHA_ORDEN = DateTime.Now;
                pedidoAdm.TARJETA_CREDITO = String.Empty;
                pedidoAdm.DIRECCION_FACTURA = "DETALLE:" + cliente.DIRECCION;
                pedidoAdm.RUBRO1 = paci.ID_PACIENTE.ToString();
                pedidoAdm.PORC_INTCTE = 0;
                pedidoAdm.BASE_IMPUESTO1 = 0;
                pedidoAdm.BASE_IMPUESTO2 = 0;
                pedidoAdm.FECHA_PROYECTADA = DateTime.Now;
                pedidoAdm.TASA_IMPOSITIVA_PORC = 0;
                pedidoAdm.TASA_CREE1_PORC = 0;
                pedidoAdm.TASA_CREE2_PORC = 0;
                pedidoAdm.TASA_GAN_OCASIONAL_PORC = 0;
                pedidoAdm.CONTRATO_REVENTA = "N";


                pedidoAdm.MONTO_ANTICIPO = 0;
                pedidoAdm.MONTO_FLETE = 0;
                pedidoAdm.MONTO_SEGURO = 0;
                pedidoAdm.MONTO_DOCUMENTACIO = 0;
                pedidoAdm.MONTO_DESCUENTO1 = 0;
                pedidoAdm.MONTO_DESCUENTO2 = 0;
                pedidoAdm.TOTAL_MERCADERIA = 0;
                pedidoAdm.TOTAL_IMPUESTO1 = 0;
                pedidoAdm.TOTAL_IMPUESTO2 = 0;
                pedidoAdm.TOTAL_A_FACTURAR = 0;
                pedidoAdm.TOTAL_CANCELADO = 0;
                pedidoAdm.TOTAL_UNIDADES = 0;
                pedidoAdm.PORC_DESCUENTO1 = 0;
                pedidoAdm.PORC_DESCUENTO2 = 0;
                pedidoAdm.PORC_COMI_COBRADOR = 0;
                pedidoAdm.PORC_COMI_VENDEDOR = 0;
                pedidoAdm.OBSERVACIONES = String.Empty;


                pedidoAdm.FECHA_PEDIDO = DateTime.Now;
                pedidoAdm.FECHA_PROMETIDA = DateTime.Now;
                pedidoAdm.FECHA_PROX_EMBARQU = DateTime.Now;
                pedidoAdm.FECHA_HORA = DateTime.Now;

                pedidoAdm.CLIENTE = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_CORPORAC = admision.FINANCIADOR;
                pedidoAdm.CLIENTE_ORIGEN = admision.CLIENTE_ID;
                pedidoAdm.CLIENTE_DIRECCION = admision.FINANCIADOR;

                pedidoAdm.CONTRATO_REVENTA = "N";
                pedidoAdm.NOMBRE_CLIENTE = paci.CLIENTE.ALIAS;
                pedidoAdm.EMBARCAR_A = paci.CLIENTE.ALIAS;
                pedidoAdm.DIRECCION_FACTURA = paci.CLIENTE.DIRECCION;

                if (ID_FINANC == string.Empty)
                {

                    pedidoAdm.PAIS = cliente.PAIS;
                    pedidoAdm.NIVEL_PRECIO = cliente.NIVEL_PRECIO;
                    pedidoAdm.RUBRO3 = "HMEP";
                    pedidoAdm.RUBRO4 = "1";
                    pedidoAdm.RUBRO5 = "03-00-000-000";
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = 1;
                    pedidoAdm.CONDICION_PAGO = cliente.CONDICION_PAGO;
                }
                else
                {
                    CLIENTE financiador = db.CLIENTE.Find(ID_FINANC);
                    pedidoAdm.PAIS = financiador.PAIS;
                    pedidoAdm.NIVEL_PRECIO = financiador.NIVEL_PRECIO;
                    pedidoAdm.RUBRO3 = "HMEP";
                    pedidoAdm.RUBRO4 = "1";
                    pedidoAdm.RUBRO5 = "03-00-000-000";
                    pedidoAdm.DESCUENTO_VOLUMEN = 0;
                    pedidoAdm.VERSION_NP = 1;
                    pedidoAdm.CONDICION_PAGO = financiador.CONDICION_PAGO;
                }
                pedidoAdm.USUARIO = "ERPADMIN";
                pedidoAdm.SUBTIPO_DOC_CXC = 0;
                pedidoAdm.BODEGA = Bodega_gral;
                db.PEDIDO.Add(pedidoAdm);

                //Agregar Admisión
                db.CS_ADMISION.Add(admision);

                //Actualizar Pedido Consecutivo
                CONSECUTIVO_FA cons = db.CONSECUTIVO_FA.Where(c => c.CODIGO_CONSECUTIVO == this.consecutivoPedidos).FirstOrDefault();
                cons.VALOR_CONSECUTIVO = admision.PEDIDO;
                db.Entry(cons).State = EntityState.Modified;
                sexo = paci.SEXO;
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        try
                        {
                            db.SaveChanges();


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
               
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, sexo = sexo, idadmision = admision.ID_ADMISION, pedido = admision.PEDIDO } };
        }

        #endregion

        [HttpGet]
        public ActionResult AdmisionEmergencia()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var imagenad = db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.AREA_SERVICIO_ID == 2180 && x.TIPO_INGRESO_ID == 2181 && x.SIGNOS_VITALES == null).OrderByDescending(a => a.FECHA_REGISTRO).ToList();
                return View(imagenad);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }
    }
}
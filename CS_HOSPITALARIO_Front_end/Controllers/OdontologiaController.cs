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
using System.Data;
using System.Net;



namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class OdontologiaController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();
        private string consecutivoPacienteSoftland = "";
        private string consecutivoPedidos = "";
        private string Bodega_gral = "";
        List<int> listRolAccess = new List<int>() { 1, 2, 3, 4, 5 };
        public OdontologiaController()
        {
            var parametro_consec = (from p in db.CS_PARAMETROS select p).FirstOrDefault();

            consecutivoPacienteSoftland = parametro_consec.CONSECUTIVO_CLIENTE;

            consecutivoPedidos = parametro_consec.CONSECUTIVO_PEDIDO;

            Bodega_gral = parametro_consec.BODEGA_GENERAL;
        }
        private int itemcatalogo()
        {
            var laboratorioad = db.CS_GLOBAL_ODONTO.First();
            return laboratorioad.ID_ITEM_CATALOGO ?? 0;
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
        public ActionResult Historicoconsultas()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View();
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
        public JsonResult GetConsultasPacientes(string paciente)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var consultas = db.SP_Listado_consulta().Where(x => x.CLIENTE_ID == paciente && x.AREA_SERVICIO_ID == itemcatalogo()).ToList();
                    return Json(consultas.Select(x => new
                    {
                        x.NUM_CONSULTA,
                        x.ID_MEDICO,
                        x.MEDICO,
                        x.AREA_SERVICIO,
                        FECHA_REGISTRO = x.FECHA_REGISTRO.ToString()
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }

        public ActionResult Consultas()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.SIGNOS_VITALES == true && x.ATENDIDO == false && x.AREA_SERVICIO_ID == itemcatalogo()).OrderByDescending(a => a.FECHA_REGISTRO));
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
        public ActionResult Editconsultas(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {

                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    SP_Listado_consulta_Result Consulta = db.SP_Listado_consulta().Where(x => x.ACTIVO == true && x.ADMISION_ID == id).FirstOrDefault();
                    if (Consulta == null)
                    {
                        return HttpNotFound();
                    }
                    return View(Consulta);
                }
                else
                {
                    return Redirect("/Home/AccessDeneg");
                }
            }
            else
            {
                return RedirectToAction("", "home");
            }
        }

        
        [HttpPost]
        public JsonResult Editconsultas(int num_consulta, string cliente_id, int id_medico, string razon, string antecendente, string examen, int area, int admision)
        {
            var status = false;
            try
            {
                using (HospitalarioBD dc = new HospitalarioBD())
                {

                    var consultabd = dc.CS_CONSULTAS.Where(a => a.NUM_CONSULTA == num_consulta).FirstOrDefault();
                    if (consultabd != null)
                    {
                        var admisiondb = dc.CS_ADMISION.Where(a => a.ID_ADMISION == consultabd.ID_ADMISION).FirstOrDefault();
                        admisiondb.ATENDIDO = true;

                        consultabd.RAZON_CONSULTA = razon;
                        consultabd.ANTECEDENTE = antecendente;
                        consultabd.EXAMEN_FISICO = examen;
                        consultabd.FECHA_REGISTRO = DateTime.Now;
                        consultabd.USUARIO_REGISTRO = 1;

                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult GuardarDiagnostico(int num_consulta, string observaciones, string tratamiento, string iddientes, string diagnosticoManual )
        {
            var status = false;
            int ID_DIAGNOSTICO = 0;
            try
            {
                using (HospitalarioBD dc = new HospitalarioBD())
                {
                        CS_DIAGNOSTICO diagnostico = new CS_DIAGNOSTICO();
                        diagnostico.ID_ADMISION = num_consulta;
                        diagnostico.OBSERVACIONES = observaciones;
                        diagnostico.TRATAMIENTO = tratamiento;
                        diagnostico.DIAGNOSTICO_MANUAL = diagnosticoManual;
                        diagnostico.ID_DIENTES = iddientes;
                        dc.CS_DIAGNOSTICO.Add(diagnostico);

                        status = dc.SaveChanges() > 0;
                        ID_DIAGNOSTICO = diagnostico.ID_DIAGNOSTICO;

                }
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, ID_DIAGNOSTICO = ID_DIAGNOSTICO } };
        }
        public JsonResult GetDientes()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_DENTADURA.Select(x => new
                    {
                        CODIGO = x.CODIGO,
                        NOMBRE = x.NOMBRE
                    }).ToList();
                    return Json(AREA.Select(x => new
                    {
                        CODIGO = x.CODIGO,
                        NOMBRE = x.NOMBRE
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public JsonResult GuardarDiagnosticoDetalle(int ID_CIE, string COD_CIE, string TIPO, int ID_DIAGNOSTICO)
        {
            //, int id_diagnostico
            var status = true;
            try
            {


                CS_DIAGNOSTICO_DETALLE diagnostico = new CS_DIAGNOSTICO_DETALLE();
                diagnostico.ID_CIE = ID_CIE;
                diagnostico.COD_CIE = COD_CIE;
                diagnostico.TIPO = TIPO;
                diagnostico.ID_DIAGNOSTICO = ID_DIAGNOSTICO;

                db.CS_DIAGNOSTICO_DETALLE.Add(diagnostico);
                status = db.SaveChanges() > 0;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        public class Recetas
        {
            public int NUM_CONSULTA { get; set; }
            public string CODPEDIDO { get; set; }
            public string ARTICULO { get; set; }
            public int DOSIS { get; set; }
            public int VIA { get; set; }
            public int DURACION { get; set; }
            public int CANTIDAD { get; set; }
            public int PRESENTACION { get; set; }
        }
        public JsonResult GuardarReceta(List<Recetas> Receta)
        {
            string cod_pedido = string.Empty;
            int version = 0;
            int cantarticulo = 0;
            decimal total = 0;
            var status = true;
            try
            {
                foreach (var item in Receta)
                {
                    //Receta en pedido
                    //decimal precio = 0;
                    //cod_pedido = item.CODPEDIDO;
                    //var pedidobd = db.PEDIDO.Where(x => x.PEDIDO1 == item.CODPEDIDO).FirstOrDefault();
                    //var versionnivel = db.SP_Listado_precio_articulo(item.ARTICULO, pedidobd.NIVEL_PRECIO, DateTime.Now).FirstOrDefault();

                    //db.SP_Insertar_receta(item.NUM_CONSULTA, item.CODPEDIDO, pedidobd.BODEGA, item.ARTICULO, DateTime.Now, versionnivel.PRECIO, item.CANTIDAD, item.CANTIDAD, DateTime.Now, "ERPADMIN", "ERPADMIN", item.DOSIS, item.VIA, item.DURACION);

                    //version = versionnivel.VERSION;
                    //precio = versionnivel.PRECIO * item.CANTIDAD;
                    //total += precio;


                    //RecetaSinPedido
                    db.SP_Insertar_receta_No_Inv(item.NUM_CONSULTA, item.CODPEDIDO, "", item.ARTICULO, DateTime.Now, decimal.Zero, item.CANTIDAD, item.CANTIDAD, DateTime.Now, "ERPADMIN", "ERPADMIN", item.DOSIS, item.VIA, item.DURACION, item.PRESENTACION);

                    cantarticulo++;
                }
                //db.SP_Actualizar_pedido_monto_paciente(cod_pedido,version,total, cantarticulo);


                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, version = version, total = total, cantidad = cantarticulo } };
        }
        public JsonResult GuardarPedidoMonto(string codpedido, int? version, string total, int? cantidad)
        {
            var status = true;
            try
            {
                db.SP_Actualizar_pedido_monto_paciente(codpedido, version, Convert.ToDecimal(total), cantidad);
                status = true;
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }
        public ActionResult Delete(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    CS_CONSULTAS cS_CONSULTAS = db.CS_CONSULTAS.Find(id);
                    if (cS_CONSULTAS == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cS_CONSULTAS);
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
        // POST: Consultas/Delete/5
 

        [HttpGet]
        public ActionResult AdmisionesOdontologia()
        {
            int userID = util.GetUser();
            int rol = util.GetRol(userID);
            if (listRolAccess.Contains(rol))
            {
                var laboratorioad = db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.AREA_SERVICIO_ID == itemcatalogo()).OrderByDescending(a => a.FECHA_REGISTRO).ToList();
                return View(laboratorioad);
            }
            else
            {
                return Redirect("/Home/AccessDeneg");
            }
        }
        [HttpGet]
        public ActionResult NuevaAdmisionOdontologia()
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

                var regi = db.CS_DEFINICION_PROCEDIMIENTO_DET.Include(t => t.CS_TEST).Where(x => x.ACTIVO == true).
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
                ViewBag.TIPO_INGRESO_ID = new SelectList(db.CS_CATALOGO_DETALLE.Where(ti => ti.ID_CATALOGO == 3 && ti.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
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
        public JsonResult NuevaAdmisionOdontologia(CS_ADMISION admision, string ID_FINANC)
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
                admision.AREA_SERVICIO_ID = itemcatalogo();
                admision.TIPO_INGRESO_ID = admision.TIPO_INGRESO_ID;
                admision.SIGNOS_VITALES = false;
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
        public JsonResult EditarAdmisionOdontologia(CS_ADMISION admision, string ID_FINANC, int ID)
        {
            var status = true;
            string sexo = string.Empty;
            // Validamos si la admisión ya existe
            var existingAdmision = db.CS_ADMISION.Find(ID);
            if (existingAdmision == null)
            {
                return new JsonResult { Data = new { status = false, message = "La admisión no existe" } };
            }

            try
            {
                existingAdmision.PRIORIDAD_ID = admision.PRIORIDAD_ID;
                existingAdmision.CAUSA_ADMISION_ID = admision.CAUSA_ADMISION_ID;
                existingAdmision.DOCTOR_ID = admision.DOCTOR_ID;
                existingAdmision.CLIENTE_ID = admision.CLIENTE_ID;
                existingAdmision.REMITIDO = admision.REMITIDO;
                existingAdmision.TIPO_INGRESO_ID = admision.TIPO_INGRESO_ID;

                // Actualizamos el asegurador si corresponde
                if (admision.ASEGURADO)
                {
                    existingAdmision.FINANCIADOR = ID_FINANC;
                    existingAdmision.TIPO_SEGURO = admision.TIPO_SEGURO;
                    existingAdmision.NUM_AFILIADO = admision.NUM_AFILIADO;
                }
                else
                {
                    CS_PACIENTES paci = db.CS_PACIENTES.Where(p => p.CLIENTE_ID == admision.CLIENTE_ID).FirstOrDefault();
                    existingAdmision.FINANCIADOR = paci.CLIENTE_ID;
                    existingAdmision.TIPO_SEGURO = null;
                    existingAdmision.NUM_AFILIADO = null;
                }

                existingAdmision.ASEGURADO = admision.ASEGURADO;
                //AGREGAR CONSULTA
                // CS_PACIENTES paciente = db.CS_PACIENTES.Find(admi.CLIENTE_ID);
                CS_CONSULTAS consulta = db.CS_CONSULTAS.Where(x => x.ID_ADMISION == existingAdmision.ID_ADMISION).FirstOrDefault();

                consulta.CLIENTE_ID = existingAdmision.CLIENTE_ID;
                consulta.DOCTOR_ID = existingAdmision.DOCTOR_ID;
                consulta.AREA_SERVICIO_ID = existingAdmision.AREA_SERVICIO_ID;
                consulta.ID_ADMISION = existingAdmision.ID_ADMISION;

                db.Entry(consulta).State = EntityState.Modified;

                PEDIDO pedidoAdm = db.PEDIDO.Where(x => x.PEDIDO1 == existingAdmision.PEDIDO).FirstOrDefault();
                if (!admision.ASEGURADO)
                {
                    CLIENTE paci = db.CLIENTE.Find(admision.CLIENTE_ID);
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

                db.Entry(existingAdmision).State = EntityState.Modified;
                db.Entry(pedidoAdm).State = EntityState.Modified;

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
            catch (Exception)
            {
                status = false;
            }

            return new JsonResult { Data = new { status, sexo, idadmision = existingAdmision.ID_ADMISION, pedido = existingAdmision.PEDIDO } };
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


        [HttpGet]
        public ActionResult EditarAdmisionOdontologia(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(AdmisionesOdontologia));
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
            ViewBag.REMITIDO = new SelectList(db.CS_CATALOGO_DETALLE.Where(c => c.ID_CATALOGO == 29 && c.ACTIVO == true).OrderBy(p => p.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", admision.REMITIDO);
            ViewBag.Title = "Editar Admisión Laboratorio";
            ViewBag.Accion = "Editar";
            return View(admision);
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
            CS_PACIENTES pacliente = db.CS_PACIENTES.Where(i => i.ID_PACIENTE == id).FirstOrDefault();
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


        [HttpPost]
        public JsonResult EliminarAdmisionOdontologia(int? id)
        {
            var status = true;
            string message = string.Empty;

            // Validamos si la admisión ya existe
            var existingAdmision = db.CS_ADMISION.Find(id);
            if (existingAdmision == null)
            {
                return new JsonResult { Data = new { status = false, message = "La admisión no existe" } };
            }

            try
            {
                // Buscar el pedido asociado a la admisión
                var pedidoAdm = db.PEDIDO.FirstOrDefault(x => x.PEDIDO1 == existingAdmision.PEDIDO);
                CS_CONSULTAS consulta = db.CS_CONSULTAS.Where(x => x.ID_ADMISION == existingAdmision.ID_ADMISION).FirstOrDefault();

                // Iniciar una transacción
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Eliminar la admisión
                        db.CS_ADMISION.Remove(existingAdmision);

                        // Si existe el pedido, eliminarlo
                        if (pedidoAdm != null)
                        {
                            db.PEDIDO.Remove(pedidoAdm);
                        }


                        // Guardar los cambios
                        db.SaveChanges();

                        // Confirmar la transacción
                        transaction.Commit();

                        message = "Admisión y pedido eliminados correctamente.";
                    }
                    catch (Exception ex)
                    {
                        // Revertir la transacción en caso de error
                        transaction.Rollback();
                        status = false;
                        message = "Error al intentar eliminar la admisión: " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                message = "Error general: " + ex.Message;
            }

            return new JsonResult { Data = new { status = status, message = message } };
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

    }
}
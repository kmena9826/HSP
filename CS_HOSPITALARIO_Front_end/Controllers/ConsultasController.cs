using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;
using DevExpress.XtraReports.UI;
using CS_HOSPITALARIO.Models.CustomMethods;
using System.Threading.Tasks;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class ConsultasController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1, 2 };
        // GET: Consultas
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_ADMISION.ToList().Where(x => x.ACTIVO == true && x.SIGNOS_VITALES == true && x.ATENDIDO == false).OrderByDescending(a => a.FECHA_REGISTRO));
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
        // GET: Consultas/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult FormartoHistorialClinico(int? consulta)
        {

            rptHistorialClinico report = new rptHistorialClinico();
            report.RequestParameters = false;
            report.Parameters[0].Value = consulta;
            report.Parameters[0].Visible = false;

            //tool.ShowPreviewDialog();
            //report.Parameters[1].Value = consulta;
            //report.Parameters[1].Visible = false;

            return View(report);
        }
        public ActionResult FormartoRecetaConsulta(int? consulta)
        {

            rptRecetaConsulta report = new rptRecetaConsulta();
            report.Parameters[0].Value = consulta;
            report.Parameters[0].Visible = false;

            return View(report);
        }
        public JsonResult GetPacientes()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var PACIENTE = db.CS_PACIENTES.Select(x => new
                    {
                        ID = x.CLIENTE_ID,
                        NOMBRE = x.CLIENTE_ID + "," + x.NOMBRES + " " + x.APELLIDOS
                    }).ToList();
                    return Json(PACIENTE.Select(x => new
                    {
                        ID = x.ID,
                        NOMBRE = x.NOMBRE
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        [HttpPost]
        public JsonResult GetConsultasPacientes(string paciente)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var consultas = db.SP_Listado_consulta().Where(x => x.CLIENTE_ID == paciente).ToList();
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
        // GET: Consultas/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Historico()
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
        // POST: Consultas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NUM_CONSULTA,CLIENTE_ID,DOCTOR_ID,RAZON_CONSULTA,AREA_SERVICIO_ID,FECHA_REGISTRO,USUARIO_REGISTRO")] CS_CONSULTAS cS_CONSULTAS)
        {
            if (ModelState.IsValid)
            {
                db.CS_CONSULTAS.Add(cS_CONSULTAS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cS_CONSULTAS);
        }
        [HttpPost]
        public JsonResult GetDiagnosticoCie()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var datos = db.SP_Listado_CIE_Total().Where(x => x.GRP == string.Empty && x.COD_CIE != "|v2").Select(x => new
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
        [HttpPost]
        public JsonResult GetCatalogoCIE()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var datos = db.SP_Listado_CIE_Total().Where(x => x.GRP == string.Empty && x.COD_CIE != "|v2").Select(x => new
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
        public JsonResult GetCIE(string cie)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var datos = db.SP_Listado_CIE_Total().Where(x => x.GRP == string.Empty && x.COD_CIE == cie).Select(x => new
                    {
                        ID = x.COD_CIE,
                        COD = x.ID_CIE,
                        DESCRIPCION = x.DESCRIPCION,
                        GRP = x.GRP
                    }).ToList();
                    //return  Json (datos,JsonRequestBehavior.AllowGet);
                    //return new JsonResult { Data = new { datos } };
                    return Json(datos.Select(x => new
                    {
                        ID = x.ID,
                        COD = x.COD,
                        DESCRIPCION = x.DESCRIPCION,
                        GRP = x.GRP
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        [HttpPost]
        public JsonResult EditarConsulta(int num_consulta, string cliente_id, int id_medico, string razon, string antecendente, string examen, int area, int admision)
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
        public JsonResult GuardarDiagnostico(int num_consulta, string observaciones, string tratamiento, string diagnosticoManual)
        {
            var status = false;
            int ID_DIAGNOSTICO = 0;
            try
            {
                using (HospitalarioBD dc = new HospitalarioBD())
                {
                    var diagnosticobd = db.CS_DIAGNOSTICO.Where(x => x.ID_ADMISION == num_consulta).FirstOrDefault();
                    if (diagnosticobd == null)
                    {
                        CS_DIAGNOSTICO diagnostico = new CS_DIAGNOSTICO();
                        diagnostico.ID_ADMISION = num_consulta;
                        diagnostico.OBSERVACIONES = observaciones;
                        diagnostico.TRATAMIENTO = tratamiento;
                        diagnostico.DIAGNOSTICO_MANUAL = diagnosticoManual;
                        diagnostico.ID_DIENTES = null;
                        dc.CS_DIAGNOSTICO.Add(diagnostico);

                        status = dc.SaveChanges() > 0;
                        ID_DIAGNOSTICO = diagnostico.ID_DIAGNOSTICO;
                    }
                    else
                    {
                        ID_DIAGNOSTICO = diagnosticobd.ID_DIAGNOSTICO;
                        status = true;
                    }

                }
            }
            catch
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, ID_DIAGNOSTICO = ID_DIAGNOSTICO } };
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
        public JsonResult GetMedicamentos()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var parameter = db.CS_PARAMETROS.FirstOrDefault();

                    var datos = db.ARTICULO.Where(x => x.CLASIFICACION_1 == parameter.CLA_FARMACIA).Select(x => new
                    {
                        x.ARTICULO1,
                        x.DESCRIPCION,
                        x.UNIDAD_VENTA
                    }).ToList();
                    //return  Json (datos,JsonRequestBehavior.AllowGet);
                    //return new JsonResult { Data = new { datos } };
                    return Json(datos.Select(x => new
                    {
                        ARTICULO = x.ARTICULO1,
                        DESCRIPCION = x.ARTICULO1 + "-" + x.DESCRIPCION,
                        UNIDAD = x.UNIDAD_VENTA
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }

        [HttpGet]
        public async Task<JsonResult> IsAvailable(string medicamento)
        {
            var available = false;
            try
            {
                using (HospitalarioBD bd = new HospitalarioBD())
                {
                    CS_PARAMETROS parameter = db.CS_PARAMETROS.FirstOrDefault();
                    List<ARTICULO> articulos = await bd.ARTICULO.Where(a => a.DESCRIPCION.ToLower().Contains(medicamento.ToLower()) && a.CLASIFICACION_1 == parameter.CLA_FARMACIA).ToListAsync();

                    if (articulos.Count > 0)
                    {
                        available = true;
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { available = available }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { available = available }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMedicamentoMedida(string articulo)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var art = db.ARTICULO.Where(x => x.ARTICULO1 == articulo).FirstOrDefault();
                    var medida = db.UNIDAD_DE_MEDIDA.Where(u => u.UNIDAD_MEDIDA == art.UNIDAD_VENTA).FirstOrDefault();
                    var cantidad = db.EXISTENCIA_BODEGA.Where(p => p.ARTICULO == articulo && p.BODEGA == "0004").FirstOrDefault();
                    var data = new JsonResult { Data = new { medida.DESCRIPCION, cantidad.CANT_DISPONIBLE } };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
        public JsonResult GetDosis()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 24).Select(x => new
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
        public JsonResult GetVias()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 25).Select(x => new
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
        // GET: Consultas/Edit/5
        public ActionResult Edit(int? id)
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
                return RedirectToAction("","home");
            }
        }
        // POST: Consultas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NUM_CONSULTA,CLIENTE_ID,DOCTOR_ID,RAZON_CONSULTA,AREA_SERVICIO_ID,FECHA_REGISTRO,USUARIO_REGISTRO")] CS_CONSULTAS cS_CONSULTAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_CONSULTAS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_CONSULTAS);
        }
        // GET: Consultas/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    CS_CONSULTAS cS_CONSULTAS = db.CS_CONSULTAS.Find(id);
                    db.CS_CONSULTAS.Remove(cS_CONSULTAS);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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

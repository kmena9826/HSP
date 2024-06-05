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
using CS_HOSPITALARIO.Models.CustomMethods;
using Microsoft.AspNet.Identity;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class CitaController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1, 2, 3, 4, 5 };
        // GET: Cita
        public ActionResult Index()
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

        public ActionResult ListadoCitas()
        {
            var cITAS = db.SpListadoCitasCS2().Where(x => x.ACTIVO == true).ToList();
            return View(cITAS);
        }

        public ActionResult CitaLaboratorio()
        {
            return View();
        }
        public ActionResult CitaOdontologia()
        {
            return View();
        }
        public ActionResult CitaImagen()
        {
            return View();
        }

        

        public JsonResult GetEventsAll()          
{
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var events = dc.SpListadoCitasCS2().Where(c => c.ACTIVO == true).ToList();
                    var result = events.Select(x => new
                    {
                        ID_CITA = x.ID_CITA,
                        PASIENTEID = x.ID_PACIENTE,
                        PASIENTECODCLIENTE = x.CLIENTE,
                        PACIENTES = x.NOMBRES + " " + x.APELLIDOS,
                        AREAID = x.AREA,
                        AREADESCRIPCION = x.AREA_DESCRIPCION,
                        RECURSOSID = x.RECURSO,
                        RECURSODESCRIPCION = x.RECURSO_DESCRIPCION,
                        MEDICOSID = x.MEDICO,
                        MEDICOSDESCRIPCION = x.MEDICO_DESCRIPCION,
                        REMITIDOSID = x.REMITIDO,
                        REMITIDOSDESCRIPCION = x.REMITIDO_DESCRIPCION,
                        ESTADOID = x.ESTADO,
                        ESTADODESCRIPCION = x.ESTADO_DESCRIPCION,
                        ASUNTO = x.ID_CITA + "/" + x.NOMBRES + " " + x.APELLIDOS + "/Observacion: " + x.OBSERVACION + " /Area: " + x.AREA_DESCRIPCION,
                        OBSERVACIONES = x.OBSERVACION,
                        Start = x.FECHA_INICIO_ATENCION,
                        End = x.FECHA_FIN_ATENCION,
                        Arrival = x.FECHA_HORA_LLEGADA_PAC,
                        Enter = x.FECHA_HORA_ENTRADA_PAC,
                        Exit = x.FECHA_HORA_SALIDA_PAC


                    }).ToList();

                    return Json(new { status = true, data = result}, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Logging de la excepción
                    Console.WriteLine("Error: " + ex.Message);
                    return Json(new { status = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public JsonResult GetEvents(int estado, int recurso, int medico)
        {
            using (HospitalarioBD db = new HospitalarioBD())
            {
                List<SpListadoCitasCS2_Result1> events = new List<SpListadoCitasCS2_Result1>();
                try
                {
                    events = db.SpListadoCitasCS2().ToList();
                }
                catch
                {
                    return null;
                }

                if (estado != 0)
                {
                    events = events.Where(e => e.ESTADO == estado).ToList();
                }

                if (recurso != 0)
                {
                    events = events.Where(e => e.RECURSO == recurso).ToList();
                }

                if (medico != 0)
                {
                    events = events.Where(e => e.MEDICO == medico).ToList();
                }

                return Json(events.Select(x => new
                {
                    ID_CITA = x.ID_CITA,
                    PASIENTEID = x.ID_PACIENTE,
                    PASIENTECODCLIENTE = x.CLIENTE,
                    PACIENTES = x.NOMBRES + " " + x.APELLIDOS,
                    AREAID = x.AREA,
                    AREADESCRIPCION = x.AREA_DESCRIPCION,
                    RECURSOSID = x.RECURSO,
                    RECURSODESCRIPCION = x.RECURSO_DESCRIPCION,
                    MEDICOSID = x.MEDICO,
                    MEDICOSDESCRIPCION = x.MEDICO_DESCRIPCION,
                    REMITIDOSID = x.REMITIDO,
                    REMITIDOSDESCRIPCION = x.REMITIDO_DESCRIPCION,
                    ESTADOID = x.ESTADO,
                    ESTADODESCRIPCION = x.ESTADO_DESCRIPCION,
                    ASUNTO = x.ID_CITA + "/" + x.NOMBRES + " " + x.APELLIDOS + "/Observacion: " + x.OBSERVACION + " /Area: " + x.AREA_DESCRIPCION,
                    OBSERVACIONES = x.OBSERVACION,
                    Start = x.FECHA_INICIO_ATENCION,
                    End = x.FECHA_FIN_ATENCION,
                    Arrival = x.FECHA_HORA_LLEGADA_PAC,
                    Enter = x.FECHA_HORA_ENTRADA_PAC,
                    Exit = x.FECHA_HORA_SALIDA_PAC
                }), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetEventsRecursosEstado(int estado, int recurso)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var events = db.SpListadoCitasCS2().Where(c => c.ACTIVO == true && c.ESTADO == estado && c.RECURSO == recurso).ToList();
                    return Json(events.Select(x => new
                    {
                        ID_CITA = x.ID_CITA,
                        PASIENTEID = x.ID_PACIENTE,
                        PASIENTECODCLIENTE = x.CLIENTE,
                        PACIENTES = x.NOMBRES + " " + x.APELLIDOS,
                        AREAID = x.AREA,
                        AREADESCRIPCION = x.AREA_DESCRIPCION,
                        RECURSOSID = x.RECURSO,
                        RECURSODESCRIPCION = x.RECURSO_DESCRIPCION,
                        MEDICOSID = x.MEDICO,
                        MEDICOSDESCRIPCION = x.MEDICO_DESCRIPCION,
                        REMITIDOSID = x.REMITIDO,
                        REMITIDOSDESCRIPCION = x.REMITIDO_DESCRIPCION,
                        ESTADOID = x.ESTADO,
                        ESTADODESCRIPCION = x.ESTADO_DESCRIPCION,
                        ASUNTO = x.ID_CITA + "/" + x.NOMBRES + " " + x.APELLIDOS + "/Observacion: " + x.OBSERVACION + " /Area: " + x.AREA_DESCRIPCION,
                        OBSERVACIONES = x.OBSERVACION,
                        Start = x.FECHA_INICIO_ATENCION,
                        End = x.FECHA_FIN_ATENCION,
                        Arrival = x.FECHA_HORA_LLEGADA_PAC,
                        Enter = x.FECHA_HORA_ENTRADA_PAC,
                        Exit = x.FECHA_HORA_SALIDA_PAC
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }
            }
        }
        [HttpPost]
        public JsonResult GetEventsRecursos(int recurso)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var events = db.SpListadoCitasCS2().Where(c => c.ACTIVO == true && c.RECURSO == recurso).ToList();
                    return Json(events.Select(x => new
                    {
                        ID_CITA = x.ID_CITA,
                        PASIENTEID = x.ID_PACIENTE,
                        PASIENTECODCLIENTE = x.CLIENTE,
                        PACIENTES = x.NOMBRES + " " + x.APELLIDOS,
                        AREAID = x.AREA,
                        AREADESCRIPCION = x.AREA_DESCRIPCION,
                        RECURSOSID = x.RECURSO,
                        RECURSODESCRIPCION = x.RECURSO_DESCRIPCION,
                        MEDICOSID = x.MEDICO,
                        MEDICOSDESCRIPCION = x.MEDICO_DESCRIPCION,
                        REMITIDOSID = x.REMITIDO,
                        REMITIDOSDESCRIPCION = x.REMITIDO_DESCRIPCION,
                        ESTADOID = x.ESTADO,
                        ESTADODESCRIPCION = x.ESTADO_DESCRIPCION,
                        ASUNTO = x.ID_CITA + "/" + x.NOMBRES + " " + x.APELLIDOS + "/Observacion: " + x.OBSERVACION + " /Area: " + x.AREA_DESCRIPCION,
                        OBSERVACIONES = x.OBSERVACION,
                        Start = x.FECHA_INICIO_ATENCION,
                        End = x.FECHA_FIN_ATENCION,
                        Arrival = x.FECHA_HORA_LLEGADA_PAC,
                        Enter = x.FECHA_HORA_ENTRADA_PAC,
                        Exit = x.FECHA_HORA_SALIDA_PAC
                    }), JsonRequestBehavior.AllowGet);
                    //JsonResult eventsJSON = new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //return eventsJSON;
                }
                catch
                {
                    return null;
                }
            }
        }
        [HttpPost]
        public JsonResult GetEventsEstado(int estado)
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var events = db.SpListadoCitasCS2().Where(c => c.ACTIVO == true && c.ESTADO == estado).ToList();
                    return Json(events.Select(x => new
                    {
                        ID_CITA = x.ID_CITA,
                        PASIENTEID = x.ID_PACIENTE,
                        PASIENTECODCLIENTE = x.CLIENTE,
                        PACIENTES = x.NOMBRES + " " + x.APELLIDOS,
                        AREAID = x.AREA,
                        AREADESCRIPCION = x.AREA_DESCRIPCION,
                        RECURSOSID = x.RECURSO,
                        RECURSODESCRIPCION = x.RECURSO_DESCRIPCION,
                        MEDICOSID = x.MEDICO,
                        MEDICOSDESCRIPCION = x.MEDICO_DESCRIPCION,
                        REMITIDOSID = x.REMITIDO,
                        REMITIDOSDESCRIPCION = x.REMITIDO_DESCRIPCION,
                        ESTADOID = x.ESTADO,
                        ESTADODESCRIPCION = x.ESTADO_DESCRIPCION,
                        ASUNTO = x.ID_CITA + "/" + x.NOMBRES + " " + x.APELLIDOS + "/Observacion: " + x.OBSERVACION + " /Area: " + x.AREA_DESCRIPCION,
                        OBSERVACIONES = x.OBSERVACION,
                        Start = x.FECHA_INICIO_ATENCION,
                        End = x.FECHA_FIN_ATENCION,
                        Arrival = x.FECHA_HORA_LLEGADA_PAC,
                        Enter = x.FECHA_HORA_ENTRADA_PAC,
                        Exit = x.FECHA_HORA_SALIDA_PAC
                    }), JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }
            }
        }
     

        [HttpPost]
        public JsonResult SaveEvent(CS_CITA e)
        {
            var status = false;
            var message = string.Empty;
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                e.ACTIVO = true;
                var overlappingAppointments = dc.CS_CITA.Where(a => a.MEDICO_ID == e.MEDICO_ID && a.ID_CITA != e.ID_CITA && (e.FECHA_INICIO_ATENCION < a.FECHA_FIN_ATENCION && e.FECHA_FIN_ATENCION > a.FECHA_INICIO_ATENCION)).ToList();
                if (overlappingAppointments.Any())
                {
                    return new JsonResult { Data = new { status = false, message = "Los horarios de las citas se superponen con una cita existente." } };
                }
                if (e.ID_CITA > 0)
                {
                    //Update the event
                    var v = dc.CS_CITA.Where(a => a.ID_CITA == e.ID_CITA).FirstOrDefault();
                    if (v != null)
                    {
                        v.AREA_SERVICIO_ID = e.AREA_SERVICIO_ID;
                        v.RECURSO_ID = e.RECURSO_ID;
                        v.CLIENTE_ID = e.CLIENTE_ID;
                        v.MEDICO_ID = e.MEDICO_ID;
                        v.REMITIDO_ID = e.REMITIDO_ID;
                        v.ESTATUS = e.ESTATUS;
                        v.FECHA_INICIO_ATENCION = e.FECHA_INICIO_ATENCION;
                        v.FECHA_FIN_ATENCION = e.FECHA_FIN_ATENCION;
                        v.OBSERVACION = e.OBSERVACION;
                        v.USUARIO_MODIFICACION = util.GetUser();
                        v.FECHA_MODIFICACION = DateTime.Now;
                        v.FECHA_HORA_LLEGADA_PAC = e.FECHA_HORA_LLEGADA_PAC.HasValue ? e.FECHA_HORA_LLEGADA_PAC : v.FECHA_HORA_LLEGADA_PAC;
                        v.FECHA_HORA_ENTRADA_PAC = e.FECHA_HORA_ENTRADA_PAC.HasValue ? e.FECHA_HORA_ENTRADA_PAC : v.FECHA_HORA_ENTRADA_PAC;
                        v.FECHA_HORA_SALIDA_PAC = e.FECHA_HORA_SALIDA_PAC.HasValue ? e.FECHA_HORA_SALIDA_PAC : v.FECHA_HORA_SALIDA_PAC;
                    }
                }
                else
                {
                    e.ESTATUS = 2104;
                    e.ASUNTO = e.AREA_SERVICIO_ID + "/" + e.CLIENTE_ID + "/" + e.RECURSO_ID;
                    e.ACTIVO = true;
                    e.USUARIO_CREACION = util.GetUser();
                    e.FECHA_CREACION = DateTime.Now;
                    dc.CS_CITA.Add(e);
                }

                try
                {
                    dc.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {

                    throw;
                }


            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                var v = dc.CS_CITA.Where(a => a.ID_CITA == eventID).FirstOrDefault();
                if (v != null)
                {
                    v.ACTIVO = false;
                    v.USUARIO_MODIFICACION = 1;
                    v.FECHA_MODIFICACION = DateTime.Now;
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
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
                        NOMBRE = x.CLIENTE_ID + "-" + x.NOMBRES + " " + x.APELLIDOS
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
        public JsonResult GetAreaServicios()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 2 && a.ACTIVO == true).Select(x => new
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
        public JsonResult GetRecursos()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 22).Select(x => new
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

        public JsonResult GetMedicos()
        {
            using (HospitalarioBD db = new HospitalarioBD())
            {
                try
                {
                    var medicos = db.CS_MEDICOS.Where(m => m.ACTIVO == true).ToList();
                    return Json(medicos.Select(m => new
                    {
                        ID = m.ID_MEDICO,
                        DESCRIPCION = m.NOMBRES
                    }), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(
                        ex.Message, JsonRequestBehavior.AllowGet
                    );
                }
            }
        }

        public JsonResult GetRemitidos()
        {
            using (HospitalarioBD db = new HospitalarioBD())
            {
                try
                {
                    var remitidos = db.CS_CATALOGO_DETALLE.Where(cd => cd.ID_CATALOGO == 29).ToList();
                    return Json(remitidos.Select(r => new
                    {
                        ID = r.ID_CAT_DETALLE,
                        DESCRIPCION = r.DESCRIPCION
                    }), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(
                        ex.Message, JsonRequestBehavior.AllowGet
                    );
                }
            }
        }

        public JsonResult GetEstados()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var AREA = db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 23).Select(x => new
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
        // GET: Cita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CITA cS_CITA = db.CS_CITA.Find(id);
            if (cS_CITA == null)
            {
                return HttpNotFound();
            }
            return View(cS_CITA);
        }

        // GET: Cita/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cita/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_CITA,CLIENTE_ID,FECHA,HORA,AREA_SERVICIO_ID,TIPO_SERVICIO_ID,OBSERVACION,NUM_QUIROFANO,FECHA_CREACION,USUARIO_CREACION,ESTATUS,ACTIVO,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CITA cS_CITA)
        {
            if (ModelState.IsValid)
            {
                db.CS_CITA.Add(cS_CITA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cS_CITA);
        }

        // GET: Cita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CITA cS_CITA = db.CS_CITA.Find(id);
            if (cS_CITA == null)
            {
                return HttpNotFound();
            }
            return View(cS_CITA);
        }
        // POST: Cita/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_CITA,CLIENTE_ID,FECHA,HORA,AREA_SERVICIO_ID,TIPO_SERVICIO_ID,OBSERVACION,NUM_QUIROFANO,FECHA_CREACION,USUARIO_CREACION,ESTATUS,ACTIVO,USUARIO_MODIFICACION,FECHA_MODIFICACION")] CS_CITA cS_CITA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cS_CITA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_CITA);
        }

        // GET: Cita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CS_CITA cS_CITA = db.CS_CITA.Find(id);
            if (cS_CITA == null)
            {
                return HttpNotFound();
            }
            return View(cS_CITA);
        }
        // POST: Cita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_CITA cS_CITA = db.CS_CITA.Find(id);
            db.CS_CITA.Remove(cS_CITA);
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

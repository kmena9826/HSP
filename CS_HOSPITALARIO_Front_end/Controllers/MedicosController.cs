
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

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class MedicosController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1 };

        // GET: Medicos
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_MEDICOS.ToList());
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
        // GET: Medicos/Details/5
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
                    CS_MEDICOS cS_MEDICOS = db.CS_MEDICOS.Find(id);
                    if (cS_MEDICOS == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cS_MEDICOS);
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
        // GET: Medicos/Create
        public ActionResult Create()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    //ViewBag.ID_ESTADO_CIVIL = new SelectList(db.CS_CATALOGO_DETALLE.Where(a => a.ID_CATALOGO == 6), "ID_CAT_DETALLE", "DESCRIPCION");
                    ViewBag.ID_ESPECIALIDAD = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 16), "ID_CAT_DETALLE", "DESCRIPCION");
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

        // POST: Medicos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_MEDICO,NOMBRES,APELLIDOS,IDENTIFICACION,LICENCIA,ID_ESPECIALIDAD,FECHA_NACIMIENTO,CORREO,TELEFONO,OBSERVACION,ACTIVO,USUARIO_REGISTRO,FECHA_REGISTRO,USUARIO_MODIFICACION,FECHA_MODIFICACION,SEXO")] CS_MEDICOS cS_MEDICOS)
        {
            if (ModelState.IsValid)
            {
                cS_MEDICOS.USUARIO_REGISTRO = util.GetUser();
                cS_MEDICOS.FECHA_REGISTRO = DateTime.Now;
                cS_MEDICOS.ACTIVO = true;
                db.CS_MEDICOS.Add(cS_MEDICOS);
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
          
                return RedirectToAction("Index");
            }
            return View(cS_MEDICOS);
        }
        // GET: Medicos/Edit/5
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
                    CS_MEDICOS cS_MEDICOS = db.CS_MEDICOS.Find(id);
                    if (cS_MEDICOS == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.FECHA_NACIMIENTO = cS_MEDICOS.FECHA_NACIMIENTO.ToString("dd/MM/yyyy");
                    ViewBag.ID_ESPECIALIDAD = new SelectList(db.CS_CATALOGO_DETALLE.Where(p => p.ID_CATALOGO == 16), "ID_CAT_DETALLE", "DESCRIPCION", cS_MEDICOS.ID_ESPECIALIDAD);
                    return View(cS_MEDICOS);
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

        // POST: Medicos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Edit([Bind(Include = "ID_MEDICO,NOMBRES,APELLIDOS,IDENTIFICACION,LICENCIA,ID_ESPECIALIDAD,FECHA_NACIMIENTO,CORREO,TELEFONO,OBSERVACION,ACTIVO,USUARIO_REGISTRO,FECHA_REGISTRO,USUARIO_MODIFICACION,FECHA_MODIFICACION,SEXO")] CS_MEDICOS cS_MEDICOS)
        {
            //if (ModelState.IsValid)
            //{
            //CS_MEDICOS medico = db.CS_MEDICOS.Find(cS_MEDICOS.ID_MEDICO);
            //medico.NOMBRES = cS_MEDICOS.NOMBRES;
            //medico.APELLIDOS = cS_MEDICOS.APELLIDOS;
            //medico.FECHA_NACIMIENTO = cS_MEDICOS.FECHA_NACIMIENTO;
            //medico.CORREO = cS_MEDICOS.CORREO;
            //medico.TELEFONO = cS_MEDICOS.TELEFONO;
            //medico.ACTIVO = cS_MEDICOS.ACTIVO;
            //medico.IDENTIFICACION = cS_MEDICOS.IDENTIFICACION;
            //medico.LICENCIA = cS_MEDICOS.LICENCIA;
            //medico.ID_ESPECIALIDAD = cS_MEDICOS.ID_ESPECIALIDAD;
            //medico.SEXO = cS_MEDICOS.SEXO;

            //medico.USUARIO_MODIFICACION = 1;
            //medico.FECHA_MODIFICACION = DateTime.Now;
            cS_MEDICOS.USUARIO_MODIFICACION = 1;
            cS_MEDICOS.FECHA_MODIFICACION = DateTime.Now;
            db.Entry(cS_MEDICOS).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            //}
            //return View(cS_MEDICOS);
        }

        // GET: Medicos/Delete/5
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
                    CS_MEDICOS cS_MEDICOS = db.CS_MEDICOS.Find(id);
                    if (cS_MEDICOS == null)
                    {
                        return HttpNotFound();
                    }

                    return View(cS_MEDICOS);
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
        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
       
        public ActionResult DeleteConfirmed(int id)
        {
            CS_MEDICOS cS_MEDICOS = db.CS_MEDICOS.Find(id);
            db.CS_MEDICOS.Remove(cS_MEDICOS);
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

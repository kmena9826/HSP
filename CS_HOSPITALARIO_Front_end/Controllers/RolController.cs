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

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class RolController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1 };
        // GET: Rol
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    var cS_ROL = db.CS_ROL.Include(c => c.CS_USER).Include(c => c.CS_USER1);
                    return View(cS_ROL.ToList());
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
        // GET: Rol/Create
        public ActionResult Create()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    ViewBag.USUARIO_REGISTRO = new SelectList(db.CS_USER, "ID_USER", "NOMBRE");
                    ViewBag.USUARIO_MODIFICO = new SelectList(db.CS_USER, "ID_USER", "NOMBRE");
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
        // POST: Rol/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_ROL,DESCRIPCION,FECHA_REGISTRO,USUARIO_REGISTRO,FECHA_MODIFICACION,USUARIO_MODIFICO,ISACTIVE")] CS_ROL cS_ROL)
        {
            if (ModelState.IsValid)
            {
                cS_ROL.FECHA_REGISTRO = DateTime.Now;
                cS_ROL.USUARIO_REGISTRO = util.GetUser();
                cS_ROL.ISACTIVE = true;
                db.CS_ROL.Add(cS_ROL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_ROL);
        }
        // GET: Rol/Edit/5
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
                    CS_ROL cS_ROL = db.CS_ROL.Find(id);
                    if (cS_ROL == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.USUARIO_REGISTRO = new SelectList(db.CS_USER, "ID_USER", "NOMBRE", cS_ROL.USUARIO_REGISTRO);
                    ViewBag.USUARIO_MODIFICO = new SelectList(db.CS_USER, "ID_USER", "NOMBRE", cS_ROL.USUARIO_MODIFICO);
                    return View(cS_ROL);
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
        // POST: Rol/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_ROL,DESCRIPCION,FECHA_REGISTRO,USUARIO_REGISTRO,FECHA_MODIFICACION,USUARIO_MODIFICO,ISACTIVE")] CS_ROL cS_ROL)
        {
            if (ModelState.IsValid)
            {
                cS_ROL.USUARIO_MODIFICO = util.GetUser();
                cS_ROL.FECHA_MODIFICACION = DateTime.Now;
                db.Entry(cS_ROL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cS_ROL);
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

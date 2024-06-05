using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class UserController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        List<int> listRolAccess = new List<int>() { 1 };
        Utilerias util = new Utilerias();
        // GET: User
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.SP_Listado_usuario_CS().ToList());
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

        // GET: User Detail
        public ActionResult UserDetail()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                if (userID == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var cS_USER = db.SP_Listado_usuario_CS().Where(X => X.ID_USER == userID).FirstOrDefault();
                ViewBag.ROL_ID = new SelectList(db.CS_ROL.Where(a => a.ISACTIVE == true), "ID_ROL", "DESCRIPCION", cS_USER.ID_ROL);

                if (cS_USER == null)
                {
                    return HttpNotFound();
                }
                return View(cS_USER);

            }
            else
            {
                return Redirect("/Login/Index");
            }

        }
        public ActionResult convertirImagen(int id_usuario)
        {
            var imagenUser = db.CS_USER.Where(x => x.ID_USER == id_usuario).FirstOrDefault();
            if (imagenUser.IMAGEN != null)
            {
                return File(imagenUser.IMAGEN, "image/jpeg");
            }
            else
            {
                return File("~/Content/img/doc-strange.png", "image/jpeg");
            }

        }
        [HttpPost]
        public ActionResult UserDetail([Bind(Include = "ID_USER,NOMBRE,CLAVE,USERNAME,IMAGEN,FECHA_REGISTRO,USUARIO_REGISTRO,FECHA_MODIFICACION,USUARIO_MODIFICO,ISACTIVE")] CS_USER cS_USER, HttpPostedFileBase imageload, int? ROL_ID)
        {
            // HttpPostedFileBase fileBase
            if (imageload != null && imageload.ContentLength > 0)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(imageload.InputStream))
                {
                    imageData = binaryReader.ReadBytes(imageload.ContentLength);
                }
                //setear la imagen a la entidad que se creara
                cS_USER.IMAGEN = imageData;
            }
            if (ModelState.IsValid)
            {
                string password = string.Empty;
                CS_USER usuariobd = db.CS_USER.Find(cS_USER.ID_USER);
                if (cS_USER.CLAVE != null)
                {

                    password = util.GetPasswordCrypted(cS_USER.CLAVE);

                }
                else
                {
                    password = usuariobd.CLAVE;
                }

                usuariobd.CLAVE = password;
                usuariobd.USUARIO_MODIFICO = util.GetUser();
                usuariobd.FECHA_MODIFICACION = DateTime.Now;
                usuariobd.IMAGEN = cS_USER.IMAGEN;
                db.SaveChanges();
                return Redirect("/Home/Index");
            }
            return View(cS_USER);
        }
        // GET: User/Details/5
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
                    CS_USER cS_USER = db.CS_USER.Find(id);
                    if (cS_USER == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cS_USER);
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
        // GET: User/Create
        public ActionResult Create()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    ViewBag.ROL_ID = new SelectList(db.CS_ROL.Where(a => a.ISACTIVE == true), "ID_ROL", "DESCRIPCION");
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
        // POST: User/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID_USER,NOMBRE,CLAVE,USERNAME,FECHA_REGISTRO,USUARIO_REGISTRO,FECHA_MODIFICACION,USUARIO_MODIFICO,ISACTIVE")] CS_USER cS_USER, int ROL_ID)
        {
            if (ModelState.IsValid)
            {
                string password = string.Empty;
                password = util.GetPasswordCrypted(cS_USER.CLAVE);

                cS_USER.CLAVE = password;
                cS_USER.USUARIO_REGISTRO = util.GetUser();
                cS_USER.FECHA_REGISTRO = DateTime.Now;
                cS_USER.ISACTIVE = true;

                db.CS_USER.Add(cS_USER);
                db.SaveChanges();

                CS_ROL_USER roluser = new CS_ROL_USER();
                roluser.ID_ROL = ROL_ID;
                roluser.ID_USUARIO = cS_USER.ID_USER;
                roluser.FECHA_REGISTRO = DateTime.Now;
                roluser.USUARIO_REGISTRO = util.GetUser();
                roluser.ISACTIVE = true;
                db.CS_ROL_USER.Add(roluser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cS_USER);
        }
        // GET: User/Edit/5
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
                    var cS_USER = db.SP_Listado_usuario_CS().Where(X => X.ID_USER == id).FirstOrDefault();
                    ViewBag.ROL_ID = new SelectList(db.CS_ROL.Where(a => a.ISACTIVE == true), "ID_ROL", "DESCRIPCION", cS_USER.ID_ROL);

                    if (cS_USER == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cS_USER);
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
        // POST: User/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID_USER,NOMBRE,CLAVE,USERNAME,FECHA_REGISTRO,USUARIO_REGISTRO,FECHA_MODIFICACION,USUARIO_MODIFICO,ISACTIVE")] CS_USER cS_USER, int? ROL_ID)
        {
            if (ModelState.IsValid)
            {
                string password = string.Empty;
                CS_USER usuariobd = db.CS_USER.Find(cS_USER.ID_USER);
                if (cS_USER.CLAVE != null)
                {

                    password = util.GetPasswordCrypted(cS_USER.CLAVE);

                }
                else
                {
                    password = usuariobd.CLAVE;
                }

                usuariobd.CLAVE = password;
                usuariobd.USUARIO_MODIFICO = util.GetUser();
                usuariobd.FECHA_MODIFICACION = DateTime.Now;
                db.SaveChanges();


                if (ROL_ID != null)
                {
                    CS_ROL_USER roluserbd = db.CS_ROL_USER.Where(x => x.ID_USUARIO == cS_USER.ID_USER).FirstOrDefault();
                    db.CS_ROL_USER.Remove(roluserbd);
                    db.SaveChanges();

                    CS_ROL_USER roluser = new CS_ROL_USER();
                    roluser.ID_ROL = Convert.ToInt32(ROL_ID);
                    roluser.ID_USUARIO = cS_USER.ID_USER;
                    roluser.FECHA_REGISTRO = DateTime.Now;
                    roluser.USUARIO_REGISTRO = util.GetUser();
                    roluser.ISACTIVE = true;
                    db.CS_ROL_USER.Add(roluser);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(cS_USER);
        }
        // GET: User/Delete/5
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
                    CS_USER cS_USER = db.CS_USER.Find(id);
                    if (cS_USER == null)
                    {
                        return HttpNotFound();
                    }
                    return View(cS_USER);
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
        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CS_USER cS_USER = db.CS_USER.Find(id);
            db.CS_USER.Remove(cS_USER);
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

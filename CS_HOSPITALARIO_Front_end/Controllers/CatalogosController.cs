using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    // [Authorize]
    //[Authorize(Roles = "Administrator")]
    public class CatalogosController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1 };
        // *** CATÁLOGOS MAESTROS ***
        [HttpGet]
        public ActionResult Maestros()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_CATALOGO.ToList().Where(x => x.ACTIVO == true));
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
        public ActionResult NuevoCatalogoMaestro()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    ViewBag.Title = "Nuevo Catálogo Maestro";
                    return View("NuevoEditarCatalogoMaestro");
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
        public ActionResult NuevoCatalogoMaestro([Bind(Include = "DESCRIPCION")] CS_CATALOGO catalogo)
        {
            if (ModelState.IsValid)
            {
                catalogo.USUARIO_REGISTRO = util.GetUser();
                catalogo.FECHA_REGISTRO = DateTime.Now;
                catalogo.ACTIVO = true;

                db.CS_CATALOGO.Add(catalogo);
                db.SaveChanges();

                return RedirectToAction("Maestros");
            }

            return View(catalogo);
        }
        [HttpGet]
        public ActionResult EditarCatalogoMaestro(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("Maestros");
                    }
                    CS_CATALOGO catalogo = db.CS_CATALOGO.Find(id);
                    if (catalogo == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Editar Catálogo Maestro";
                    ViewBag.Accion = "Editar";
                    return View("NuevoEditarCatalogoMaestro", catalogo);
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
        public ActionResult EditarCatalogoMaestro([Bind(Include = "ID_CATALOGO, DESCRIPCION")] CS_CATALOGO catalogo)
        {
            if (ModelState.IsValid)
            {
                CS_CATALOGO cs_cat = db.CS_CATALOGO.Find(catalogo.ID_CATALOGO);
                cs_cat.DESCRIPCION = catalogo.DESCRIPCION;
                cs_cat.FECHA_MODIFICACION = DateTime.Now;
                cs_cat.USUARIO_MODIFICO = util.GetUser();
                db.Entry(cs_cat).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Maestros");
            }

            return View(catalogo);
        }
        [HttpGet]
        public ActionResult BorrarCatalogoMaestro(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("Maestros");
                    }
                    CS_CATALOGO catalogo = db.CS_CATALOGO.Find(id);
                    if (catalogo == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Borrar Catálogo Maestro";
                    return View("BorrarCatalogoMaestro", catalogo);
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
        public ActionResult BorrarCatalogoMaestro(int id)
        {
            CS_CATALOGO cs_cat = db.CS_CATALOGO.Find(id);
            cs_cat.FECHA_MODIFICACION = DateTime.Now;
            cs_cat.USUARIO_MODIFICO = util.GetUser();
            cs_cat.ACTIVO = false;
            db.Entry(cs_cat).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Maestros");
        }
        // *** ITEMS CATÁLOGOS ***
        [HttpGet]
        public ActionResult ItemsCatalogos()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    
                    return View(db.CS_CATALOGO_DETALLE.Include(x=> x.CS_CATALOGO).ToList().Where(x => x.ACTIVO == true));
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
        public ActionResult NuevoItemCatalogo()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    ViewBag.Title = "Nuevo Ítem de Catálogo";
                    ViewBag.ID_CATALOGO = new SelectList(db.CS_CATALOGO.Where(c => c.ACTIVO == true).OrderBy(c => c.DESCRIPCION), "ID_CATALOGO", "DESCRIPCION");
                    return View("NuevoEditarItemCatalogo");
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
        public ActionResult NuevoItemCatalogo([Bind(Include = "ID_CAT_DETALLE, ID_CATALOGO, DESCRIPCION")] CS_CATALOGO_DETALLE itemCat)
        {
            if (ModelState.IsValid)
            {
                itemCat.USUARIO_REGISTRO = util.GetUser();
                itemCat.FECHA_REGISTRO = DateTime.Now;
                itemCat.ACTIVO = true;

                db.CS_CATALOGO_DETALLE.Add(itemCat);
                db.SaveChanges();

                return RedirectToAction("ItemsCatalogos");
            }

            return View(itemCat);
        }
        [HttpGet]
        public ActionResult EditarItemCatalogo(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("ItemsCatalogos");
                    }
                    CS_CATALOGO_DETALLE itemCat = db.CS_CATALOGO_DETALLE.Find(id);
                    if (itemCat == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Editar Ítem de Catálogo";
                    ViewBag.Accion = "Editar";
                    ViewBag.ID_CATALOGO = new SelectList(db.CS_CATALOGO.Where(c => c.ACTIVO == true).OrderBy(c => c.DESCRIPCION), "ID_CATALOGO", "DESCRIPCION", itemCat.ID_CATALOGO);
                    return View("NuevoEditarItemCatalogo", itemCat);
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
        public ActionResult EditarItemCatalogo([Bind(Include = "ID_CAT_DETALLE, ID_CATALOGO, DESCRIPCION")] CS_CATALOGO_DETALLE itemCat)
        {
            if (ModelState.IsValid)
            {
                CS_CATALOGO_DETALLE cs_cat_det = db.CS_CATALOGO_DETALLE.Find(itemCat.ID_CAT_DETALLE);
                cs_cat_det.ID_CATALOGO = itemCat.ID_CATALOGO;
                cs_cat_det.DESCRIPCION = itemCat.DESCRIPCION;
                cs_cat_det.FECHA_MODIFICACION = DateTime.Now;
                cs_cat_det.USUARIO_MODIFICO = util.GetUser();
                db.Entry(cs_cat_det).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ItemsCatalogos");
            }

            return View(itemCat);
        }
        [HttpGet]
        public ActionResult BorrarItemCatalogo(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("ItemsCatalogos");
                    }
                    CS_CATALOGO_DETALLE itemCat = db.CS_CATALOGO_DETALLE.Find(id);
                    if (itemCat == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Borrar Ítem de Catálogo";
                    return View("BorrarItemCatalogo", itemCat);
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
        public ActionResult BorrarItemCatalogo(int id)
        {
            CS_CATALOGO_DETALLE itemCat = db.CS_CATALOGO_DETALLE.Find(id);
            itemCat.FECHA_MODIFICACION = DateTime.Now;
            itemCat.USUARIO_MODIFICO = util.GetUser();
            itemCat.ACTIVO = false;
            db.Entry(itemCat).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ItemsCatalogos");
        }
        // *** SEGUROS ***
        [HttpGet]
        public ActionResult Seguros()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.CS_SEGURO.ToList().Where(x => x.ACTIVO == true));
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
        public ActionResult NuevoSeguro()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    ViewBag.Title = "Nuevo Seguro";
                    ViewBag.ID_ASEGURADORA = new SelectList(db.CS_CATALOGO_DETALLE.Where(s => (s.ACTIVO == true) && (s.ID_CATALOGO == 13)).OrderBy(c => c.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION");
                    return View("NuevoEditarSeguro");
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
        public ActionResult NuevoSeguro([Bind(Include = "ID_ASEGURADORA, DESCRIPCION")] CS_SEGURO seguro)
        {
            if (ModelState.IsValid)
            {
                seguro.USUARIO_REGISTRO = util.GetUser();
                seguro.FECHA_REGISTRO = DateTime.Now;
                seguro.ACTIVO = true;

                db.CS_SEGURO.Add(seguro);
                db.SaveChanges();

                return RedirectToAction("Seguros");
            }

            return View(seguro);
        }
        [HttpGet]
        public ActionResult EditarSeguro(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("Seguros");
                    }
                    CS_SEGURO seguro = db.CS_SEGURO.Find(id);
                    if (seguro == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Editar Seguro";
                    ViewBag.Accion = "Editar";
                    ViewBag.ID_ASEGURADORA = new SelectList(db.CS_CATALOGO_DETALLE.Where(s => (s.ACTIVO == true) && (s.ID_CATALOGO == 13)).OrderBy(c => c.DESCRIPCION), "ID_CAT_DETALLE", "DESCRIPCION", seguro.ID_ASEGURADORA);
                    return View("NuevoEditarSeguro", seguro);
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
        public ActionResult EditarSeguro([Bind(Include = "ID_SEGURO, ID_ASEGURADORA, DESCRIPCION")] CS_SEGURO seguro)
        {
            if (ModelState.IsValid)
            {
                CS_SEGURO cs_seg = db.CS_SEGURO.Find(seguro.ID_SEGURO);
                cs_seg.ID_ASEGURADORA = seguro.ID_ASEGURADORA;
                cs_seg.DESCRIPCION = seguro.DESCRIPCION;
                cs_seg.FECHA_MODIFICACION = DateTime.Now;
                cs_seg.USUARIO_MODIFICO = util.GetUser();
                db.Entry(cs_seg).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Seguros");
            }

            return View(seguro);
        }
        [HttpGet]
        public ActionResult BorrarSeguro(int? id)
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    if (id == null)
                    {
                        return RedirectToAction("Seguros");
                    }
                    CS_SEGURO seguro = db.CS_SEGURO.Find(id);
                    if (seguro == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.Title = "Borrar Seguro";
                    return View("BorrarSeguro", seguro);
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
        public ActionResult BorrarSeguro(int id)
        {
            CS_SEGURO cs_seg = db.CS_SEGURO.Find(id);
            cs_seg.FECHA_MODIFICACION = DateTime.Now;
            cs_seg.USUARIO_MODIFICO = util.GetUser();
            cs_seg.ACTIVO = false;
            db.Entry(cs_seg).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Seguros");
        }

    }
}
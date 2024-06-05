using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace CS_HOSPITALARIO.Controllers
{
    [AutorizeController]
    public class CatalogosController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();

        // *** CATÁLOGOS MAESTROS ***

        [HttpGet]
        public ActionResult Maestros()
        {
            return View(db.CS_CATALOGO.ToList().Where(x => x.ACTIVO == true));
        }

        [HttpGet]
        public ActionResult NuevoCatalogoMaestro()
        {
            ViewBag.Title = "Nuevo Catálogo Maestro";
            return View("NuevoEditarCatalogoMaestro");
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
            return View(db.CS_CATALOGO_DETALLE.ToList().Where(x => x.ACTIVO == true));
        }
        
        [HttpGet]
        public ActionResult NuevoItemCatalogo()
        {
            ViewBag.Title = "Nuevo Ítem de Catálogo";
            ViewBag.ID_CATALOGO = new SelectList(db.CS_CATALOGO.Where(c => c.ACTIVO == true).OrderBy(c => c.DESCRIPCION), "ID_CATALOGO", "DESCRIPCION");
            return View("NuevoEditarItemCatalogo");
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
            return View(db.CS_SEGURO.ToList().Where(x => x.ACTIVO == true));
        }

        [HttpGet]
        public ActionResult NuevoSeguro()
        {
            ViewBag.Title = "Nuevo Seguro";
            return View("NuevoEditarSeguro");
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
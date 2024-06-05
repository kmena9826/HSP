using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        private readonly HospitalarioBD _bd = new HospitalarioBD();
        List<int> listRolAccess = new List<int>() { 1 };
        Utilerias util = new Utilerias();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            return View(await _bd.CS_PERSONAL.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");
            List<CS_CATALOGO_DETALLE> ESPECIALIDADES = await _bd.CS_CATALOGO_DETALLE.Where(e => e.ID_CATALOGO == 16).ToListAsync();
            ViewBag.ESPECIALIDAD = new SelectList(ESPECIALIDADES, "ID_CAT_DETALLE", "DESCRIPCION");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            List<CS_CATALOGO_DETALLE> ESPECIALIDADES = await _bd.CS_CATALOGO_DETALLE.Where(e => e.ID_CATALOGO == 16).ToListAsync();
            ViewBag.ESPECIALIDAD = new SelectList(ESPECIALIDADES, "ID_CAT_DETALLE", "DESCRIPCION");
            CS_PERSONAL personal = await _bd.CS_PERSONAL.Where(p => p.PERSONAL_ID == id).FirstOrDefaultAsync();
            return View(personal);
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "PUESTO,NOMBRES,LIC,ESPECIALIDAD")] CS_PERSONAL personal)
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            if (!ModelState.IsValid)
            {
                List<CS_CATALOGO_DETALLE> ESPECIALIDADES = await _bd.CS_CATALOGO_DETALLE.Where(e => e.ID_CATALOGO == 16).ToListAsync();
                ViewBag.ESPECIALIDAD = new SelectList(ESPECIALIDADES, "ID_CAT_DETALLE", "DESCRIPCION");
                return View(personal);
            }

            personal.ACTIVO = true;
            _bd.CS_PERSONAL.Add(personal);
            await _bd.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "PERSONAL_ID,PUESTO,NOMBRES,LIC,ESPECIALIDAD")] CS_PERSONAL personal)
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            if (id != personal.PERSONAL_ID)
            {
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                List<CS_CATALOGO_DETALLE> ESPECIALIDADES = await _bd.CS_CATALOGO_DETALLE.Where(e => e.ID_CATALOGO == 16).ToListAsync();
                ViewBag.ESPECIALIDAD = new SelectList(ESPECIALIDADES, "ID_CAT_DETALLE", "DESCRIPCION");
                return View(personal);
            }

            var storedPersonal = await _bd.CS_PERSONAL.Where(p => p.PERSONAL_ID == id).FirstOrDefaultAsync();
            if (storedPersonal == null)
            {
                return HttpNotFound();
            }

            storedPersonal.ESPECIALIDAD = personal.ESPECIALIDAD;
            storedPersonal.NOMBRES = personal.NOMBRES;
            storedPersonal.PUESTO = personal.PUESTO;
            storedPersonal.LIC = personal.LIC;

            try
            {
                _bd.Entry(storedPersonal).State = EntityState.Modified;
                await _bd.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e);
                return View(personal);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            if (id == null) return RedirectToAction(nameof(Index));

            CS_PERSONAL personal = await _bd.CS_PERSONAL.Where(p => p.PERSONAL_ID == id).FirstOrDefaultAsync();

            if (personal == null) return HttpNotFound();

            ViewData["Title"] = "Eliminar Personal";

            return PartialView("_BorrarPersonal", personal);
        }

        [HttpGet]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            int userID = util.GetUser();
            if (userID == 0) return Redirect("/Login/Index");

            int rol = util.GetRol(userID);
            if (!listRolAccess.Contains(rol)) return Redirect("/Home/AccessDeneg");

            CS_PERSONAL personal = await _bd.CS_PERSONAL.Where(p => p.PERSONAL_ID == id).FirstOrDefaultAsync();

            _bd.Entry(personal).State = EntityState.Deleted;
            await _bd.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
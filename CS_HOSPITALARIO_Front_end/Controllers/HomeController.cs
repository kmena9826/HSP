using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class HomeController : Controller
    {
        private HospitalarioBD db = new HospitalarioBD();
        private Utilerias util = new Utilerias();
        List<int> listRolAccess = new List<int>() { 1, 2, 3, 4, 5 };
        public ActionResult Index()
        {
            int userID = util.GetUser();
            if (userID != 0)
            {
                int rol = util.GetRol(userID);
                if (listRolAccess.Contains(rol))
                {
                    return View(db.SpListadoTotalesAdmisionesDB().ToList());
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Acceso()
        {
            return View();
        }
        public ActionResult error()
        {
            return View();
        }
        public ActionResult AccessDeneg()
        {
            return View();
        }
        public JsonResult ListarMenuAcceso()
        {
            using (HospitalarioBD dc = new HospitalarioBD())
            {
                try
                {
                    var datos = util.GetMenuByUser(Convert.ToInt32(User.Identity.GetUserId())).ToList();
                    //return Json(datos.Select(x => new {
                    //    MenuNavegacion=x.MenuNavegacion,
                    //    LinkText=x.MenuItems[0].LinkText,
                    //    ActionName=x.MenuItems[0].ActionName,
                    //    ControllerName=x.MenuItems[0].ControllerName

                    //} ), JsonRequestBehavior.AllowGet);
                    return Json(datos, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }

            }
        }
    }
}
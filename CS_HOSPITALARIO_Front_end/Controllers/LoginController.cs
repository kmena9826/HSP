using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class LoginController : Controller
    {

        private HospitalarioBD db = new HospitalarioBD();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Acceso([Bind(Include = "USERNAME, CLAVE")] CS_USER usuario)
        {
            Utilerias util = new Utilerias();

            string password = string.Empty;

            if (usuario.CLAVE != null && usuario.CLAVE.Length > 0)
                password = util.GetPasswordCrypted(usuario.CLAVE);

            CS_USER user = (from a in db.CS_USER.Where(z => z.USERNAME == usuario.USERNAME && z.CLAVE == password) select a).FirstOrDefault();

            if (user == null)
            {
                TempData["Message"] = "El usuario o la contraseña es incorrecta";

            }
            else if (user.ISACTIVE == false)
            {
                TempData["Message"] = "El usuario o la contraseña es incorrecta";
            }
            else
            {
                var claims = new List<Claim>();

                // create required claims
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.ID_USER.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.NOMBRE));

                // custom – my serialized AppUserState object
                claims.Add(new Claim("userState", user.ToString()));

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                AuthenticationManager.SignIn(new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                }, identity);
                //List<CS_HOSPITALARIO.Models.CustomModels.MenuItem> listMenus = util.GetMenuByUser(user.ID_USER);

                //Session["MenuMaster"] = listMenus;
                TempData["Message"] = string.Empty;
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }

        public ActionResult IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                            DefaultAuthenticationTypes.ExternalCookie);
            return RedirectToAction("Index", "Login");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }

}
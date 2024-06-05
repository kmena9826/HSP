using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CS_HOSPITALARIO.Models.CustomMethods
{
    public class AutorizeController : AuthorizeAttribute
    {
        Utilerias util = new Utilerias();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var rd = httpContext.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            string currentArea = rd.Values["area"] as string;

            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                // The user is not authenticated
                return false;
            }

            int userIDLogin = util.GetUser();

            var user = httpContext.User;
            HospitalarioBD context = new HospitalarioBD();

            var usuario = (from a in context.CS_USER.Where(z => z.ID_USER == userIDLogin) select a).FirstOrDefault();

            var roles = (from a in context.CS_ROL_USER
                         join b in context.CS_ROL on a.ID_ROL equals b.ID_ROL
                         where
                         a.ID_USUARIO == userIDLogin
                         select new
                         {
                             userID = a.ID_USUARIO,
                             roleName = b.DESCRIPCION,
                             roleID = a.ID_ROL
                         }).ToList();

            if (roles.Where(z => z.roleName == "Administrador").Count() > 0)
            {
                // Administrator => let him in
                return true;
            }
            else
            {
                if (roles.Count > 0 && roles != null)
                {
                    int? idrol = roles.FirstOrDefault().roleID;

                    var habilitarAccion = (from a in context.CS_CONTROLADOR
                                           join b in context.CS_CONTROL_ACCION_DETALLE on a.ID equals b.ID_CONTROLADOR
                                           join c in context.CS_ACCIONES on b.ID_ACCION equals c.ID
                                           where
                                           a.NOMBRE == currentController &&
                                           b.ID_ROL_USER == idrol &&
                                           c.LINK == currentAction
                                           select b).FirstOrDefault();
                    if (habilitarAccion != null)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsOwnerOfPost(string username, int postId)
        {
            // TODO: you know what to do here
            throw new NotImplementedException();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;
using CS_HOSPITALARIO.Models.CustomMethods;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CS_HOSPITALARIO.Controllers
{
    [AutorizeController]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        
    }
}
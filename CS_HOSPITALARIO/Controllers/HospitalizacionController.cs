using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO.Controllers
{
    public class HospitalizacionController : Controller
    {
        CS_HOSPITALARIO.Models.HospitalarioBD db = new CS_HOSPITALARIO.Models.HospitalarioBD();

        // GET: Hospitalizacion
        public ActionResult Index()
        {
            return View();
        }

     

        [ValidateInput(false)]
        public ActionResult HospitalCardViewPartial()
        {
            var area_hsp = db.CS_PARAMETROS.FirstOrDefault();
            var model = db.CS_ESTACION_ENFERMERIA.Where(y => y.AREA_SERVICIO==area_hsp.AREA_HSP);
            return PartialView("_HospitalCardViewPartial", model.ToList());
        }
    }
}
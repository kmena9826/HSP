using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO_Front_end.Models.ViewModel;

namespace CS_HOSPITALARIO_Front_end.Controllers
{
    public class HospitalizacionController : Controller
    {
        CS_HOSPITALARIO.Models.HospitalarioBD db = new CS_HOSPITALARIO.Models.HospitalarioBD();

      
        public ActionResult Index()
        {
          
            var area_hsp = db.CS_PARAMETROS.FirstOrDefault();

            var listado_enf = db.CS_ESTACION_ENFERMERIA.Where(y => y.AREA_SERVICIO == area_hsp.AREA_HSP && y.ACTIVO==true);

            //var listado_count = db.CS_ESTACION
            //    .Join(listado_enf, e => e.ESTACION_ENFERMERIA, l => l.ESTACION_ID, (e, l) => new { e, l })
            //    .GroupBy(x => new { x.l.ESTACION_ID, x.l.DESCRIPCION, x.l.DESCRIPCORTA, x.e.CLIENTE_ID, x.e.ESTACION_ENFERMERIA })
            //    .Select(g => new {
            //        g.Key.ESTACION_ID,
            //        g.Key.ESTACION_ENFERMERIA,
            //        g.Key.DESCRIPCORTA,
            //        g.Key.DESCRIPCION,
            //        CANTIDAD_PACIENTES = g.Count()
            //    }).ToList();

            var listado_enfermeria = (from list in db.CS_ESTACION_ENFERMERIA.Where(y => y.AREA_SERVICIO == area_hsp.AREA_HSP && y.ACTIVO == true).AsEnumerable()
                                      select new EstacionViewModel()
                                      {
                                          ESTACION_ID=list.ESTACION_ID,
                                          DESCRIPCION=list.DESCRIPCION,
                                          DESCRIPCORTA=list.DESCRIPCORTA,
                                          CANTIDAD_PACIENTES= EstacionViewModel.GetCantidadPac(list.ESTACION_ID)
                                      }
                                      
                                      
                                      ).ToList();

          
            return View(listado_enfermeria);
        }

        
       public ActionResult Estacion_Piso(int id)
       {
            var list = db.CS_ESTACION.Where(x => x.ESTACION_ENFERMERIA == id).ToList();
            var datos_estacion= db.CS_ESTACION_ENFERMERIA.Where(x => x.ESTACION_ID == id).FirstOrDefault();
            ViewBag.Nom_Estacion = datos_estacion.DESCRIPCION;
            ViewBag.Nom_Estacion_Corta = datos_estacion.DESCRIPCORTA;

            /*Datos para el modal de Imagen*/

            var examen_imagen = (from e in db.CS_DEFINICION_PROC_IMAGEN.Where(x => x.ACTIVO == true)
                          join a in db.ARTICULO on e.ARTICULO equals a.ARTICULO1
                          select new { ARTICULO = e.ARTICULO, DESCRIPCION = a.DESCRIPCION }).ToList();

            ViewBag.ID_EXAMEN_IMAGEN = new SelectList(examen_imagen, "ARTICULO", "DESCRIPCION");
            ViewBag.CANTIDAD_IMAGEN = 1;

            return View(list);
       }
        
        

    }
}
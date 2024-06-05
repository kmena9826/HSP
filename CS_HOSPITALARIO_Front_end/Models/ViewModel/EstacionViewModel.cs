using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models.ViewModel
{

    public class EstacionViewModel
    {
        public int ESTACION_ID { get; set; }
        public string DESCRIPCION { get; set; }
        public string DESCRIPCORTA { get; set; }
        public int AREA_SERVICIO { get; set; }
        public string BODEGA { get; set; }
        public bool ACTIVO { get; set; }
        public int CANTIDAD_PACIENTES { get; set; }



        public static int GetCantidadPac(int area)
        {
            CS_HOSPITALARIO.Models.HospitalarioBD db = new CS_HOSPITALARIO.Models.HospitalarioBD();

            int cant = 0;

            var list = db.CS_ESTACION.Where(y => y.ESTACION_ENFERMERIA == area).Count();

            cant = list;

            return cant;
        }
    }

    public class EstacionPisoViewModel
    {
        public string CLIENTE { get; set; }
        public int CAMA { get; set; }
        public int CUARTO { get; set; }
        public int ESTACION_ENFERMERIA { get; set; }
        public DateTime FECHA_INGRESO { get; set; }
        public DateTime ? FECHA_ALTA { get; set; }
        public bool EMERGENCIA { get; set; }
        public bool SOLICITUD { get; set; }
    }
}
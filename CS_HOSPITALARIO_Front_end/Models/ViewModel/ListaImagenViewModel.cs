using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models.ViewModel
{
    public class ListaImagenViewModel
    {
        public DateTime DESDE { get; set; }
        public DateTime HASTA { get; set; }
        //public Boolean PRIORIDAD { get; set; }
    }


    public class  PlantillaViewModel
    {
        public int ID_PLANTILLA { get; set; }
        public string ARTICULO { get; set; }
        public string DESCRIPCION_ARTICULO { get; set; }
        public int DOCTOR { get; set; }
        public string NOMBRE_DOCTOR { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class OdenesHistViewModel
    {
        public int ORDEN { get; set; }
        public string ARTICULO { get; set; }
        public string DESCRIPCION { get; set; }
        public string LECTURA { get; set; }
        public string ANULADO { get; set; }
        public DateTime? FECHA_ANULACION { get; set; }
        public DateTime? FECHA_LECTURA { get; set; }
        public string PEDIDO { get; set; }
        public int PEDIDO_LINEA { get; set; }
        public string ESTADO { get; set; }
        public bool STAT { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int CANTIDAD { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models
{
	public class ImagenologiaBusinessLogic
	{
		public CS_HOSPITALARIO.Models.CS_IMAGENOLOGIA encabezado_Orden { get; set; }
		public CS_HOSPITALARIO.Models.CS_RESULTADO_IMAGENOLOGIA encabezado_resultado_Imagen {get; set;}
		public List<CS_HOSPITALARIO.Models.CS_RESULT_IMAGENOLOGIA_DETALLE> detalle_resultado_Imagen { get; set; }

        public static DateTime validateDatetime(string value)
        {
            string mensaje = "";
            DateTime date;

            try
            {
                date = Convert.ToDateTime(value);
                return DateTime.ParseExact(date.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception EX)
            {
                mensaje=EX.Message;

                return DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
          
        }
    }
}
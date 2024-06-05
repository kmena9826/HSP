using CS_HOSPITALARIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models
{
    public class RegistrarResultadoLaboratorio
    {
        public int TEST { get; set; }
        public string NOMBRE { get; set; }
        public string VALOR { get; set; }
        public Sp_Listado_Limite_Registrar_Resultado_Result LIMITE { get; set; }
        public string DESCRIPCION { get; set; }
        public string RESULTADO { get; set; }
    }
}
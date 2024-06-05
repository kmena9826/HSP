using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models.ViewModel
{
    public class ResultadoImagenViewModel
    {
        public string CLIENTE_ID { get; set; }
        public string PACIENTE_ID { get; set; }
        public string NOMBRE_PACIENTE { get; set; }
        public int IMAGENOLOGIA_ID { get; set; }
        public int PLANTILLA_ID { get; set; }
        public DateTime FECHA_RESULTADO { get; set; }
        public DateTime ? FECHA_ALTA { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FECHA_NACIMIENTO { get; set; }
        public string TRANSCRIPCION { get; set; }

        public int RADIOLOGO_ID { get; set; }
        public string NOMBRE_RADIOLOGO { get; set; }
        public int ADMISION_ID { get; set; }
        
        public int TIPO_INGRESO_ID { get; set; }
        public string DESC_TIPO_INGRESO { get; set; }

        public int CAUSA_ADMISION_ID { get; set; }
        public string DESC_CAUSA_ADMISION { get; set; }

        public int PRIORIDAD_ID { get; set; }
        public string DESC_PRIORIDAD { get; set; }

        public int? AREA_SERVICIO_ID { get; set; }
        public string DESC_AREA_SERVICIO { get; set; }

        public bool ASGURADO { get; set; }
        public string PEDIDO { get; set; }

        public int TIPO_SEGURO { get; set; }
        public string DESC_TIPO_SEGURO { get; set; }

        public int DOCTOR_ID { get; set; }
        public string NOMBRE_DOCTOR { get; set; }

        public string PROCEDIMIENTO { get; set; }
        public string DESC_PROCEDIMIENTO { get; set; }

        public string SEXO { get; set; }
        public int EDAD { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public DateTime ? FECHA_REGISTRO_PROC { get; set; }
        public string STATUS { get; set; }
        public string STAT { get; set; }

        List<CS_HOSPITALARIO.Models.CS_RESULT_IMAGENOLOGIA_DETALLE> detalle { get; set; }

    }
}
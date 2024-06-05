//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CS_HOSPITALARIO.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CS_SIGNOS_VITALES
    {
        public int ID_SIGNOS_VITALES { get; set; }
        public int ADMISION_ID { get; set; }
        public Nullable<System.DateTime> FECHA_HORA_TOMA { get; set; }
        public Nullable<decimal> ESTATURA { get; set; }
        public Nullable<decimal> PESOKG { get; set; }
        public Nullable<decimal> IMC { get; set; }
        public Nullable<decimal> TEMPERATURAC { get; set; }
        public Nullable<int> PSISTOLICA { get; set; }
        public Nullable<int> PDIASTOLICA { get; set; }
        public Nullable<int> PULSO { get; set; }
        public Nullable<int> RESPIRACION { get; set; }
        public Nullable<decimal> SATURACION { get; set; }
        public Nullable<decimal> GLUCOMETRIA { get; set; }
        public Nullable<decimal> PERIMETRO_TORAXICO { get; set; }
        public Nullable<decimal> PERIMETRO_MUNECA { get; set; }
        public Nullable<decimal> PERIMETRO_CEFALICO { get; set; }
        public Nullable<decimal> PERIMETRO_ABDOMINAL { get; set; }
        public string HALLAZGO { get; set; }
        public Nullable<int> USUARIO_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
    
        public virtual CS_ADMISION CS_ADMISION { get; set; }
        public virtual CS_USER CS_USER { get; set; }
    }
}

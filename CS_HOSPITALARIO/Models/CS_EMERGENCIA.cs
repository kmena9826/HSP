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
    
    public partial class CS_EMERGENCIA
    {
        public int EMERGENCIA { get; set; }
        public string PEDIDO { get; set; }
        public string CLIENTE_ID { get; set; }
        public int NIVEL_EMERGENCIA { get; set; }
        public Nullable<int> ADMISION_ID { get; set; }
        public System.DateTime FECHA_INGRESO { get; set; }
        public string TRIAGE { get; set; }
        public string TRANSFERENCIA { get; set; }
        public Nullable<int> TIPO_ALTA { get; set; }
        public Nullable<System.DateTime> FECHA_ALTA { get; set; }
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public Nullable<int> ADMISION_EGY { get; set; }
        public Nullable<int> PEDIDO_EGY { get; set; }
        public Nullable<int> CLIENTE_EGY { get; set; }
    }
}

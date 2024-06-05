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
    
    public partial class CS_PACIENTES
    {
        public int ID_PACIENTE { get; set; }
        public string CLIENTE_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string SEXO { get; set; }
        public Nullable<int> ESTADO_CIVIL_ID { get; set; }
        public Nullable<System.DateTime> FECHA_NACIMIENTO { get; set; }
        public Nullable<int> TIPO_SANGRE_ID { get; set; }
        public Nullable<int> ESCOLARIDAD_ID { get; set; }
        public Nullable<int> PROFESION_ID { get; set; }
        public Nullable<int> RELIGION_ID { get; set; }
        public string CONTACTO_1 { get; set; }
        public string TEL_CONT_1 { get; set; }
        public string CONTACTO_2 { get; set; }
        public string TEL_CONT_2 { get; set; }
        public string OBSERVACIONES { get; set; }
        public Nullable<int> USUARIO_ID { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
        public string DEPARTAMENTO { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE1 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE2 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE3 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE4 { get; set; }
        public virtual CS_PACIENTES CS_PACIENTES1 { get; set; }
        public virtual CS_PACIENTES CS_PACIENTES2 { get; set; }
    }
}

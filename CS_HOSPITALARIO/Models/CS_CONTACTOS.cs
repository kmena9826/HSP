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
    
    public partial class CS_CONTACTOS
    {
        public int ID_CONTACTO { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string TEL_CONT_1 { get; set; }
        public string TEL_CONT_2 { get; set; }
        public string CORREO { get; set; }
        public string CLIENTE_ID { get; set; }
        public int ID_PARENTESCO { get; set; }
        public bool ACTIVO { get; set; }
        public int USUARIO_REGISTRA { get; set; }
        public System.DateTime FECHA_REGISTRO { get; set; }
        public Nullable<int> USUARIO_MODIFICA { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICA { get; set; }
    
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
    }
}
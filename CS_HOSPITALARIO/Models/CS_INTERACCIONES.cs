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
    
    public partial class CS_INTERACCIONES
    {
        public string ARTICULO_1 { get; set; }
        public int TIPO_INTERACCION { get; set; }
        public string ARTICULO_2 { get; set; }
        public string DESCRIPCION { get; set; }
    
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
    }
}
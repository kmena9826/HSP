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
    
    public partial class SP_Listado_precio_articulo_Result
    {
        public string NIVEL_PRECIO { get; set; }
        public string MONEDA { get; set; }
        public int VERSION { get; set; }
        public string ARTICULO { get; set; }
        public int VERSION_ARTICULO { get; set; }
        public decimal PRECIO { get; set; }
        public string ESQUEMA_TRABAJO { get; set; }
        public Nullable<decimal> MARGEN_MULR { get; set; }
        public decimal MARGEN_UTILIDAD { get; set; }
        public System.DateTime FECHA_INICIO { get; set; }
        public System.DateTime FECHA_FIN { get; set; }
        public Nullable<System.DateTime> FECHA_ULT_MODIF { get; set; }
        public string USUARIO_ULT_MODIF { get; set; }
        public Nullable<decimal> MARGEN_UTILIDAD_MIN { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
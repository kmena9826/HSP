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
    
    public partial class CONSECUTIVO
    {
        public string CONSECUTIVO1 { get; set; }
        public string DESCRIPCION { get; set; }
        public string ACTIVO { get; set; }
        public decimal LONGITUD { get; set; }
        public string ENTIDAD { get; set; }
        public string DOCUMENTO { get; set; }
        public string FORMATO_IMPRESION_DETALLADO { get; set; }
        public string FORMATO_IMPRESION_RESUMIDO { get; set; }
        public string MASCARA { get; set; }
        public string VALOR_INICIAL { get; set; }
        public string VALOR_FINAL { get; set; }
        public string ULTIMO_VALOR { get; set; }
        public string ULTIMO_USUARIO { get; set; }
        public System.DateTime FECHA_HORA_ULT { get; set; }
        public string CONSECUTIVO_NC { get; set; }
        public string USA_IMP_FISCAL { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual CONSECUTIVO_FA CONSECUTIVO_FA { get; set; }
    }
}
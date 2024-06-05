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
    
    public partial class BODEGA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BODEGA()
        {
            this.EXISTENCIA_BODEGA = new HashSet<EXISTENCIA_BODEGA>();
            this.PEDIDO = new HashSet<PEDIDO>();
            this.PEDIDO_LINEA = new HashSet<PEDIDO_LINEA>();
        }
    
        public string BODEGA1 { get; set; }
        public string NOMBRE { get; set; }
        public string TIPO { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string CONSEC_TRASLADOS { get; set; }
        public string CODIGO_ESTABLECIMIENTO { get; set; }
        public string MERCADO_LIBRE { get; set; }
        public string U_SUCURSAL { get; set; }
        public string U_COORDINADAS { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string NO_STOCK_NEGATIVO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EXISTENCIA_BODEGA> EXISTENCIA_BODEGA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO_LINEA> PEDIDO_LINEA { get; set; }
    }
}

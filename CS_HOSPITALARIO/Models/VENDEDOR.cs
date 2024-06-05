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
    
    public partial class VENDEDOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VENDEDOR()
        {
            this.CLIENTE = new HashSet<CLIENTE>();
            this.CLIENTE_VENDEDOR = new HashSet<CLIENTE_VENDEDOR>();
            this.PEDIDO = new HashSet<PEDIDO>();
        }
    
        public string VENDEDOR1 { get; set; }
        public string NOMBRE { get; set; }
        public string EMPLEADO { get; set; }
        public decimal COMISION { get; set; }
        public string CTR_COMISION { get; set; }
        public string CTA_COMISION { get; set; }
        public string E_MAIL { get; set; }
        public string ACTIVO { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string TELEFONO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_VENDEDOR> CLIENTE_VENDEDOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO { get; set; }
    }
}

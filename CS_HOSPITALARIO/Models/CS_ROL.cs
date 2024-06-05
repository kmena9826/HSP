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
    
    public partial class CS_ROL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_ROL()
        {
            this.CS_CONTROL_ACCION_DETALLE = new HashSet<CS_CONTROL_ACCION_DETALLE>();
            this.CS_ROL_USER = new HashSet<CS_ROL_USER>();
        }
    
        public int ID_ROL { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public Nullable<int> USUARIO_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public Nullable<int> USUARIO_MODIFICO { get; set; }
        public bool ISACTIVE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_CONTROL_ACCION_DETALLE> CS_CONTROL_ACCION_DETALLE { get; set; }
        public virtual CS_USER CS_USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ROL_USER> CS_ROL_USER { get; set; }
        public virtual CS_USER CS_USER1 { get; set; }
    }
}

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
    
    public partial class CS_PLANTILLA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_PLANTILLA()
        {
            this.CS_RESULTADO_IMAGENOLOGIA = new HashSet<CS_RESULTADO_IMAGENOLOGIA>();
        }
    
        public int ID_PLANTILLA { get; set; }
        public string ARTICULO { get; set; }
        public string DESCRIPCION { get; set; }
        public int DOCTOR_ID { get; set; }
        public string COMENTARIO { get; set; }
        public bool ACTIVO { get; set; }
    
        public virtual CS_PERSONAL CS_PERSONAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_RESULTADO_IMAGENOLOGIA> CS_RESULTADO_IMAGENOLOGIA { get; set; }
    }
}

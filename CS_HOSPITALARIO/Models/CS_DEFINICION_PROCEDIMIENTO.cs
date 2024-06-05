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
    
    public partial class CS_DEFINICION_PROCEDIMIENTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_DEFINICION_PROCEDIMIENTO()
        {
            this.CS_DEFINICION_PROCEDIMIENTO_DET = new HashSet<CS_DEFINICION_PROCEDIMIENTO_DET>();
        }
    
        public int ID_EXAMEN { get; set; }
        public string ARTICULO { get; set; }
        public string OBSERVACION { get; set; }
        public int AREA_SERVICIO { get; set; }
        public int COLOR_TUBO { get; set; }
        public bool IMPRIME_ETIQUETA { get; set; }
        public bool ENVIAR_CORREO { get; set; }
        public bool ACTIVO { get; set; }
    
        public virtual ARTICULO ARTICULO1 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_DEFINICION_PROCEDIMIENTO_DET> CS_DEFINICION_PROCEDIMIENTO_DET { get; set; }
    }
}
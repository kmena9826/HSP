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
    
    public partial class CS_CATALOGO_DETALLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_CATALOGO_DETALLE()
        {
            this.CS_ADMISION = new HashSet<CS_ADMISION>();
            this.CS_ADMISION1 = new HashSet<CS_ADMISION>();
            this.CS_ADMISION2 = new HashSet<CS_ADMISION>();
            this.CS_ADMISION3 = new HashSet<CS_ADMISION>();
            this.CS_ADMISION4 = new HashSet<CS_ADMISION>();
            this.CS_ADT = new HashSet<CS_ADT>();
            this.CS_ADT1 = new HashSet<CS_ADT>();
            this.CS_SEGURO = new HashSet<CS_SEGURO>();
            this.CS_DEFINICION_PROCEDIMIENTO = new HashSet<CS_DEFINICION_PROCEDIMIENTO>();
            this.CS_PERSONAL = new HashSet<CS_PERSONAL>();
            this.CS_CONTACTOS = new HashSet<CS_CONTACTOS>();
            this.CS_DEFINICION_PROCEDIMIENTO1 = new HashSet<CS_DEFINICION_PROCEDIMIENTO>();
            this.CS_PACIENTES = new HashSet<CS_PACIENTES>();
            this.CS_PACIENTES1 = new HashSet<CS_PACIENTES>();
            this.CS_PACIENTES2 = new HashSet<CS_PACIENTES>();
            this.CS_PACIENTES3 = new HashSet<CS_PACIENTES>();
            this.CS_PACIENTES4 = new HashSet<CS_PACIENTES>();
            this.CS_TEST = new HashSet<CS_TEST>();
            this.CS_IMAGENOLOGIA = new HashSet<CS_IMAGENOLOGIA>();
            this.CS_INTERACCIONES = new HashSet<CS_INTERACCIONES>();
            this.CS_ADMISION5 = new HashSet<CS_ADMISION>();
            this.CS_CITA = new HashSet<CS_CITA>();
        }
    
        public int ID_CAT_DETALLE { get; set; }
        public int ID_CATALOGO { get; set; }
        public string DESCRIPCION { get; set; }
        public System.DateTime FECHA_REGISTRO { get; set; }
        public int USUARIO_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public Nullable<int> USUARIO_MODIFICO { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
        public string ICONO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADT> CS_ADT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADT> CS_ADT1 { get; set; }
        public virtual CS_CATALOGO CS_CATALOGO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_SEGURO> CS_SEGURO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_DEFINICION_PROCEDIMIENTO> CS_DEFINICION_PROCEDIMIENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PERSONAL> CS_PERSONAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_CONTACTOS> CS_CONTACTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_DEFINICION_PROCEDIMIENTO> CS_DEFINICION_PROCEDIMIENTO1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_TEST> CS_TEST { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_IMAGENOLOGIA> CS_IMAGENOLOGIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_INTERACCIONES> CS_INTERACCIONES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_CITA> CS_CITA { get; set; }
    }
}
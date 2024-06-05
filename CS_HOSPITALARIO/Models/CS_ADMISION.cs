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
    
    public partial class CS_ADMISION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_ADMISION()
        {
            this.CS_ESTACION = new HashSet<CS_ESTACION>();
            this.CS_CONSULTAS = new HashSet<CS_CONSULTAS>();
            this.CS_SIGNOS_VITALES = new HashSet<CS_SIGNOS_VITALES>();
            this.CS_IMAGENOLOGIA = new HashSet<CS_IMAGENOLOGIA>();
            this.CS_LABORATORIO = new HashSet<CS_LABORATORIO>();
        }
    
        public int ID_ADMISION { get; set; }
        public string CLIENTE_ID { get; set; }
        public Nullable<int> PRIORIDAD_ID { get; set; }
        public Nullable<int> CAUSA_ADMISION_ID { get; set; }
        public string MOTIVO { get; set; }
        public Nullable<int> TIPO_INGRESO_ID { get; set; }
        public Nullable<int> NIVEL_EMERGENCIA { get; set; }
        public Nullable<int> AREA_SERVICIO_ID { get; set; }
        public Nullable<System.DateTime> FECHA_RECEPCION { get; set; }
        public bool ASEGURADO { get; set; }
        public string PEDIDO { get; set; }
        public string FINANCIADOR { get; set; }
        public Nullable<int> TIPO_SEGURO { get; set; }
        public string NUM_AFILIADO { get; set; }
        public Nullable<bool> ATENDIDO { get; set; }
        public Nullable<int> USUARIO_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_REGISTRO { get; set; }
        public Nullable<System.DateTime> FECHA_ALTA { get; set; }
        public Nullable<int> TIPO_ALTA { get; set; }
        public Nullable<bool> SIGNOS_VITALES { get; set; }
        public Nullable<int> DOCTOR_ID { get; set; }
        public bool ACTIVO { get; set; }
        public Nullable<int> REMITIDO { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ESTACION> CS_ESTACION { get; set; }
        public virtual CS_MEDICOS CS_MEDICOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_CONSULTAS> CS_CONSULTAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_SIGNOS_VITALES> CS_SIGNOS_VITALES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_IMAGENOLOGIA> CS_IMAGENOLOGIA { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE1 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE2 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE3 { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_LABORATORIO> CS_LABORATORIO { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE5 { get; set; }
    }
}

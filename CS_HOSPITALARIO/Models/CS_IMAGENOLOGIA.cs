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
    
    public partial class CS_IMAGENOLOGIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CS_IMAGENOLOGIA()
        {
            this.CS_RESULTADO_IMAGENOLOGIA = new HashSet<CS_RESULTADO_IMAGENOLOGIA>();
        }
    
        public int ID_IMAGENOLOGIA { get; set; }
        public Nullable<int> RADIOLOGO_ID { get; set; }
        public string CLIENTE_ID { get; set; }
        public string ARTICULO { get; set; }
        public int CANTIDAD { get; set; }
        public int ID_ADMISION { get; set; }
        public string PEDIDO { get; set; }
        public int PEDIDO_LINEA { get; set; }
        public string SEXO { get; set; }
        public int EDAD { get; set; }
        public Nullable<bool> IMPRESO { get; set; }
        public Nullable<bool> ENV_POR_CORREO { get; set; }
        public int USUARIO_REGISTRO { get; set; }
        public System.DateTime FECHA_REGISTRO { get; set; }
        public int STATUS { get; set; }
        public bool STAT { get; set; }
        public string LECTURA { get; set; }
        public Nullable<System.DateTime> FECHA_LECTURA { get; set; }
        public string USUARIO_LECTURA { get; set; }
        public string ANULADO { get; set; }
        public Nullable<int> USUARIO_ANULACION { get; set; }
        public Nullable<System.DateTime> FECHA_ANULACION { get; set; }
    
        public virtual ARTICULO ARTICULO1 { get; set; }
        public virtual CS_ADMISION CS_ADMISION { get; set; }
        public virtual CS_PERSONAL CS_PERSONAL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_RESULTADO_IMAGENOLOGIA> CS_RESULTADO_IMAGENOLOGIA { get; set; }
        public virtual CS_CATALOGO_DETALLE CS_CATALOGO_DETALLE { get; set; }
    }
}
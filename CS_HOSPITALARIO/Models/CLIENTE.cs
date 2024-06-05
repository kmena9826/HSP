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
    
    public partial class CLIENTE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENTE()
        {
            this.CS_ADMISION = new HashSet<CS_ADMISION>();
            this.CS_PACIENTES = new HashSet<CS_PACIENTES>();
            this.CLIENTE11 = new HashSet<CLIENTE>();
            this.CLIENTE_VENDEDOR = new HashSet<CLIENTE_VENDEDOR>();
            this.DIRECC_EMBARQUE = new HashSet<DIRECC_EMBARQUE>();
            this.PEDIDO = new HashSet<PEDIDO>();
            this.PEDIDO1 = new HashSet<PEDIDO>();
            this.PEDIDO2 = new HashSet<PEDIDO>();
            this.PEDIDO3 = new HashSet<PEDIDO>();
        }
    
        public string CLIENTE1 { get; set; }
        public string NOMBRE { get; set; }
        public Nullable<int> DETALLE_DIRECCION { get; set; }
        public string ALIAS { get; set; }
        public string CONTACTO { get; set; }
        public string CARGO { get; set; }
        public string DIRECCION { get; set; }
        public string DIR_EMB_DEFAULT { get; set; }
        public string TELEFONO1 { get; set; }
        public string TELEFONO2 { get; set; }
        public string FAX { get; set; }
        public string CONTRIBUYENTE { get; set; }
        public System.DateTime FECHA_INGRESO { get; set; }
        public string MULTIMONEDA { get; set; }
        public string MONEDA { get; set; }
        public decimal SALDO { get; set; }
        public decimal SALDO_LOCAL { get; set; }
        public decimal SALDO_DOLAR { get; set; }
        public decimal SALDO_CREDITO { get; set; }
        public Nullable<decimal> SALDO_NOCARGOS { get; set; }
        public Nullable<decimal> LIMITE_CREDITO { get; set; }
        public string EXCEDER_LIMITE { get; set; }
        public decimal TASA_INTERES { get; set; }
        public decimal TASA_INTERES_MORA { get; set; }
        public System.DateTime FECHA_ULT_MORA { get; set; }
        public System.DateTime FECHA_ULT_MOV { get; set; }
        public string CONDICION_PAGO { get; set; }
        public string NIVEL_PRECIO { get; set; }
        public decimal DESCUENTO { get; set; }
        public string MONEDA_NIVEL { get; set; }
        public string ACEPTA_BACKORDER { get; set; }
        public string PAIS { get; set; }
        public string ZONA { get; set; }
        public string RUTA { get; set; }
        public string VENDEDOR { get; set; }
        public string COBRADOR { get; set; }
        public string ACEPTA_FRACCIONES { get; set; }
        public string ACTIVO { get; set; }
        public string EXENTO_IMPUESTOS { get; set; }
        public decimal EXENCION_IMP1 { get; set; }
        public decimal EXENCION_IMP2 { get; set; }
        public string COBRO_JUDICIAL { get; set; }
        public string CATEGORIA_CLIENTE { get; set; }
        public string CLASE_ABC { get; set; }
        public short DIAS_ABASTECIMIEN { get; set; }
        public string USA_TARJETA { get; set; }
        public string TARJETA_CREDITO { get; set; }
        public string TIPO_TARJETA { get; set; }
        public Nullable<System.DateTime> FECHA_VENCE_TARJ { get; set; }
        public string E_MAIL { get; set; }
        public string REQUIERE_OC { get; set; }
        public string ES_CORPORACION { get; set; }
        public string CLI_CORPORAC_ASOC { get; set; }
        public string REGISTRARDOCSACORP { get; set; }
        public string USAR_DIREMB_CORP { get; set; }
        public string APLICAC_ABIERTAS { get; set; }
        public string VERIF_LIMCRED_CORP { get; set; }
        public string USAR_DESC_CORP { get; set; }
        public string DOC_A_GENERAR { get; set; }
        public string RUBRO1_CLIENTE { get; set; }
        public string RUBRO2_CLIENTE { get; set; }
        public string RUBRO3_CLIENTE { get; set; }
        public string TIENE_CONVENIO { get; set; }
        public string NOTAS { get; set; }
        public short DIAS_PROMED_ATRASO { get; set; }
        public string RUBRO1_CLI { get; set; }
        public string RUBRO2_CLI { get; set; }
        public string RUBRO3_CLI { get; set; }
        public string RUBRO4_CLI { get; set; }
        public string RUBRO5_CLI { get; set; }
        public string ASOCOBLIGCONTFACT { get; set; }
        public string RUBRO4_CLIENTE { get; set; }
        public string RUBRO5_CLIENTE { get; set; }
        public string RUBRO6_CLIENTE { get; set; }
        public string RUBRO7_CLIENTE { get; set; }
        public string RUBRO8_CLIENTE { get; set; }
        public string RUBRO9_CLIENTE { get; set; }
        public string RUBRO10_CLIENTE { get; set; }
        public string USAR_PRECIOS_CORP { get; set; }
        public string USAR_EXENCIMP_CORP { get; set; }
        public string DIAS_DE_COBRO { get; set; }
        public string AJUSTE_FECHA_COBRO { get; set; }
        public string GLN { get; set; }
        public string UBICACION { get; set; }
        public string CLASE_DOCUMENTO { get; set; }
        public string LOCAL { get; set; }
        public string TIPO_CONTRIBUYENTE { get; set; }
        public string RUBRO11_CLIENTE { get; set; }
        public string RUBRO12_CLIENTE { get; set; }
        public string RUBRO13_CLIENTE { get; set; }
        public string RUBRO14_CLIENTE { get; set; }
        public string RUBRO15_CLIENTE { get; set; }
        public string RUBRO16_CLIENTE { get; set; }
        public string RUBRO17_CLIENTE { get; set; }
        public string RUBRO18_CLIENTE { get; set; }
        public string RUBRO19_CLIENTE { get; set; }
        public string RUBRO20_CLIENTE { get; set; }
        public string MODELO_RETENCION { get; set; }
        public string ACEPTA_DOC_ELECTRONICO { get; set; }
        public string CONFIRMA_DOC_ELECTRONICO { get; set; }
        public string EMAIL_DOC_ELECTRONICO { get; set; }
        public string EMAIL_PED_EDI { get; set; }
        public string ACEPTA_DOC_EDI { get; set; }
        public string NOTIFICAR_ERROR_EDI { get; set; }
        public string EMAIL_ERROR_PED_EDI { get; set; }
        public string CODIGO_IMPUESTO { get; set; }
        public string DIVISION_GEOGRAFICA1 { get; set; }
        public string DIVISION_GEOGRAFICA2 { get; set; }
        public string REGIMEN_TRIB { get; set; }
        public string MOROSO { get; set; }
        public string MODIF_NOMB_EN_FAC { get; set; }
        public decimal SALDO_TRANS { get; set; }
        public decimal SALDO_TRANS_LOCAL { get; set; }
        public decimal SALDO_TRANS_DOLAR { get; set; }
        public string PERMITE_DOC_GP { get; set; }
        public string PARTICIPA_FLUJOCAJA { get; set; }
        public string CURP { get; set; }
        public string USUARIO_CREACION { get; set; }
        public Nullable<System.DateTime> FECHA_HORA_CREACION { get; set; }
        public string USUARIO_ULT_MOD { get; set; }
        public Nullable<System.DateTime> FCH_HORA_ULT_MOD { get; set; }
        public string EMAIL_DOC_ELECTRONICO_COPIA { get; set; }
        public string DETALLAR_KITS { get; set; }
        public string XSLT_PERSONALIZADO { get; set; }
        public string NOMBRE_ADDENDA { get; set; }
        public Nullable<decimal> GEO_LATITUD { get; set; }
        public Nullable<decimal> GEO_LONGITUD { get; set; }
        public string DIVISION_GEOGRAFICA3 { get; set; }
        public string DIVISION_GEOGRAFICA4 { get; set; }
        public string OTRAS_SENAS { get; set; }
        public string SUBTIPODOC { get; set; }
        public string API_RECEPCION_DE { get; set; }
        public string USA_API_RECEPCION { get; set; }
        public string USER_API_RECEPCION { get; set; }
        public string PASS_API_RECEPCION { get; set; }
        public string TIPO_IMPUESTO { get; set; }
        public string TIPO_TARIFA { get; set; }
        public Nullable<decimal> PORC_TARIFA { get; set; }
        public string TIPIFICACION_CLIENTE { get; set; }
        public string AFECTACION_IVA { get; set; }
        public string ES_EXTRANJERO { get; set; }
        public string ITEM_HACIENDA { get; set; }
        public string XSLT_PERSONALIZADO_CREDITO { get; set; }
        public string USO_CFDI { get; set; }
        public string TIPO_GENERAR { get; set; }
        public string TIPO_PERSONERIA { get; set; }
        public string METODO_PAGO { get; set; }
        public string BANCO_NACION { get; set; }
        public string ES_AGENTE_PERCEPCION { get; set; }
        public string ES_BUEN_CONTRIBUYENTE { get; set; }
        public string SUJETO_PORCE_SUNAT { get; set; }
        public string U_TIPO_RECEPTOR { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string PDB_EXPORTADORES { get; set; }
    
        public virtual CATEGORIA_CLIENTE CATEGORIA_CLIENTE1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_ADMISION> CS_ADMISION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CS_PACIENTES> CS_PACIENTES { get; set; }
        public virtual CONDICION_PAGO CONDICION_PAGO1 { get; set; }
        public virtual DETALLE_DIRECCION DETALLE_DIRECCION1 { get; set; }
        public virtual COBRADOR COBRADOR1 { get; set; }
        public virtual CS_ESTACION CS_ESTACION { get; set; }
        public virtual NIVEL_PRECIO NIVEL_PRECIO1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTE11 { get; set; }
        public virtual CLIENTE CLIENTE2 { get; set; }
        public virtual NIT NIT { get; set; }
        public virtual RUTA RUTA1 { get; set; }
        public virtual PAIS PAIS1 { get; set; }
        public virtual VENDEDOR VENDEDOR1 { get; set; }
        public virtual ZONA ZONA1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE_VENDEDOR> CLIENTE_VENDEDOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DIRECC_EMBARQUE> DIRECC_EMBARQUE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PEDIDO> PEDIDO3 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS_HOSPITALARIO.Models;

namespace CS_HOSPITALARIO.Controllers
{
    public class FuncionesVarias : Controller
    {

        //tabla donde hay que ir actualizar el consecutivo del pedido
        //HM.CONSECUTIVO_FA


        //FUNCION PARA TOMAR EL ULTIMO CONSECUTIVO Y EL PROXIMO, TOMANDO EN CUENTA QUE LA MASCARA ES 10 NUMEROS


        public static string ProximoCodigo(string psCodigoOriginal, int pnLongitudMaxima)
        {
            char[] chArray = new char[50];
            string s = "";
            if (string.IsNullOrEmpty(psCodigoOriginal))
            {
                chArray = "1".ToCharArray();
            }
            else
            {
                try
                {
                    char ch;
                    bool flag2 = false;
                    bool flag = true;
                    chArray = psCodigoOriginal.ToCharArray();
                    int length = psCodigoOriginal.Length;
                    int index = length - 1;
                    if (pnLongitudMaxima >= length)
                    {
                        goto Label_01DC;
                    }
                    chArray = psCodigoOriginal.ToCharArray();
                    goto Label_01EC;
                Label_0052:
                    ch = psCodigoOriginal[index];
                    string str = ch.ToString()[0].ToString();
                    if (char.IsDigit(str[0]))
                    {
                        flag2 = false;
                        if (str.Equals("9"))
                        {
                            s = "0";
                            flag = true;
                        }
                        else
                        {
                            s = (Convert.ToInt32(str) + 1).ToString();
                            flag = false;
                        }
                        chArray[index] = char.Parse(s);
                    }
                    else if (char.IsLetter(str[0]))
                    {
                        flag2 = false;
                        if (str.Equals("Z"))
                        {
                            chArray[index] = 'A';
                            flag = true;
                        }
                        else if (str.Equals("z"))
                        {
                            chArray[index] = 'a';
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                            chArray[index] = (char)(chArray[index] + '\x0001');
                        }
                    }
                    else if (str.Equals(Environment.NewLine))
                    {
                        flag2 = true;
                        flag = false;
                        if (char.IsLower(str[0]))
                        {
                            s = "A";
                        }
                        else
                        {
                            s = "a";
                        }
                        chArray[index] = char.Parse(s);
                    }
                    else if (index == (length - 1))
                    {
                        flag = true;
                    }
                    if ((index == 0) && flag)
                    {
                        if (chArray.Length == pnLongitudMaxima)
                        {
                            chArray = psCodigoOriginal.ToCharArray();
                        }
                        else if (!flag2)
                        {
                            string str3;
                            if (s.Equals("0"))
                            {
                                str3 = "1" + new string(chArray);
                            }
                            else if (s.Equals("A"))
                            {
                                str3 = "A" + new string(chArray);
                            }
                            else
                            {
                                str3 = "a" + new string(chArray);
                            }
                            chArray = str3.ToCharArray();
                        }
                    }
                    index--;
                Label_01DC:
                    if ((index >= 0) && flag)
                    {
                        goto Label_0052;
                    }
                }
                catch
                {
                }
            }
        Label_01EC:
            return new string(chArray);
        }

        //public static void GetUserX(string user, out string xUser, out string xPwd)
        //{
        //    EDK obj = new EDK();
        //    string usuarioProxy;
        //    string passProxy;
        //    bool flag = obj.GetProxyInfo(user, out usuarioProxy, out passProxy);
        //    xUser = usuarioProxy;
        //    xPwd = passProxy;
        //}

        // IsNumeric Function
        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;

        }

        ///INSSERCION EN LA TABLA HM.PEDIDO
        ///INSERT INTO [HM].[PEDIDO]
  //      ([PEDIDO]
  //      ,[ESTADO]
  //      ,[FECHA_PEDIDO]
  //      ,[FECHA_PROMETIDA]
  //      ,[FECHA_PROX_EMBARQU]
  //      ,[FECHA_ULT_EMBARQUE]
  //      ,[FECHA_ULT_CANCELAC]
  //      ,[ORDEN_COMPRA]
  //      ,[FECHA_ORDEN]
  //      ,[TARJETA_CREDITO]
  //      ,[EMBARCAR_A]
  //      ,[DIREC_EMBARQUE]
  //      ,[DIRECCION_FACTURA]
  //      ,[RUBRO1]
  //      ,[RUBRO2]
  //      ,[RUBRO3]
  //      ,[RUBRO4]
  //      ,[RUBRO5]
  //      ,[OBSERVACIONES]
  //      ,[COMENTARIO_CXC]
  //      ,[TOTAL_MERCADERIA]
  //      ,[MONTO_ANTICIPO]
  //      ,[MONTO_FLETE]
  //      ,[MONTO_SEGURO]
  //      ,[MONTO_DOCUMENTACIO]
  //      ,[TIPO_DESCUENTO1]
  //      ,[TIPO_DESCUENTO2]
  //      ,[MONTO_DESCUENTO1]
  //      ,[MONTO_DESCUENTO2]
  //      ,[PORC_DESCUENTO1]
  //      ,[PORC_DESCUENTO2]
  //      ,[TOTAL_IMPUESTO1]
  //      ,[TOTAL_IMPUESTO2]
  //      ,[TOTAL_A_FACTURAR]
  //      ,[PORC_COMI_VENDEDOR]
  //      ,[PORC_COMI_COBRADOR]
  //      ,[TOTAL_CANCELADO]
  //      ,[TOTAL_UNIDADES]
  //      ,[IMPRESO]
  //      ,[FECHA_HORA]
  //      ,[DESCUENTO_VOLUMEN]
  //      ,[TIPO_PEDIDO]
  //      ,[MONEDA_PEDIDO]
  //      ,[VERSION_NP]
  //      ,[AUTORIZADO]
  //      ,[DOC_A_GENERAR]
  //      ,[CLASE_PEDIDO]
  //      ,[MONEDA]
  //      ,[NIVEL_PRECIO]
  //      ,[COBRADOR]
  //      ,[RUTA]
  //      ,[USUARIO]
  //      ,[CONDICION_PAGO]
  //      ,[BODEGA]
  //      ,[ZONA]
  //      ,[VENDEDOR]
  //      ,[CLIENTE]
  //      ,[CLIENTE_DIRECCION]
  //      ,[CLIENTE_CORPORAC]
  //      ,[CLIENTE_ORIGEN]
  //      ,[PAIS]
  //      ,[SUBTIPO_DOC_CXC]
  //      ,[TIPO_DOC_CXC]
  //      ,[BACKORDER]
  //      ,[CONTRATO]
  //      ,[PORC_INTCTE]
  //      ,[DESCUENTO_CASCADA]
  //      ,[TIPO_CAMBIO]
  //      ,[FIJAR_TIPO_CAMBIO]
  //      ,[ORIGEN_PEDIDO]
  //      ,[DESC_DIREC_EMBARQUE]
  //      ,[DIVISION_GEOGRAFICA1]
  //      ,[DIVISION_GEOGRAFICA2]
  //      ,[BASE_IMPUESTO1]
  //      ,[BASE_IMPUESTO2]
  //      ,[NOMBRE_CLIENTE]
  //      ,[FECHA_PROYECTADA]
  //      ,[FECHA_APROBACION]
  //      ,[TIPO_DOCUMENTO]
  //      ,[VERSION_COTIZACION]
  //      ,[RAZON_CANCELA_COTI]
  //      ,[DES_CANCELA_COTI]
  //      ,[CAMBIOS_COTI]
  //      ,[COTIZACION_PADRE]
  //      ,[TASA_IMPOSITIVA]
  //      ,[TASA_IMPOSITIVA_PORC]
  //      ,[TASA_CREE1]
  //      ,[TASA_CREE1_PORC]
  //      ,[TASA_CREE2]
  //      ,[TASA_CREE2_PORC]
  //      ,[TASA_GAN_OCASIONAL_PORC]
  //      ,[CONTRATO_AC]
  //      ,[TIPO_CONTRATO_AC]
  //      ,[PERIODICIDAD_CONTRATO_AC]
  //      ,[FECHA_CONTRATO_AC]
  //      ,[FECHA_INICIO_CONTRATO_AC]
  //      ,[FECHA_PROXFAC_CONTRATO_AC]
  //      ,[FECHA_FINFAC_CONTRATO_AC]
  //      ,[FECHA_ULTAUMENTO_CONTRATO_AC]
  //      ,[FECHA_PROXFACSIST_CONTRATO_AC]
  //      ,[DIFERIDO_CONTRATO_AC]
  //      ,[TOTAL_CONTRATO_AC]
  //      ,[CONTRATO_REVENTA]
  //      ,[NoteExistsFlag]
  //      ,[RecordDate]
  //      ,[RowPointer]
  //      ,[CreatedBy]
  //      ,[UpdatedBy]
  //      ,[CreateDate]
  //      ,[USR_NO_APRUEBA]
  //      ,[FECHA_NO_APRUEBA]
  //      ,[RAZON_DESAPRUEBA]
  //      ,[MODULO]
  //      ,[CORREOS_ENVIO]
  //      ,[U_REFERIDO]
  //      ,[U_MODULO]
  //      ,[U_CLINICA]
  //      ,[EXPLORADO]
  //      ,[CONTRATO_VIGENCIA_DESDE]
  //      ,[CONTRATO_VIGENCIA_HASTA]
  //      ,[USO_CFDI]
  //      ,[FORMA_PAGO]
  //      ,[CLAVE_REFERENCIA_DE]
  //      ,[FECHA_REFERENCIA_DE])
  //VALUES
  //      ('P0000772'

  //      ,'N'
  //         , GETDATE()

  //      , GETDATE()

  //      , GETDATE()

  //      ,'1980-01-01'
  //         ,'1980-01-01'
  //         , NULL

  //      , GETDATE()

  //      , NULL

  //      ,'ACA VA EL NOMBRE DEL CLIENTE'
  //         ,'ND'
  //         ,'DIRECCION DE DONDE VA LA FACTURA'
  //         , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      ,''
  //         , NULL

  //      ,0  --TOTAL MERCADERIA SE VA ACTUALIZANDO CADA VEZ QUE AGREGUEN LINEAS AL PEDIDO
  //      ,0
  //         ,0
  //         ,0
  //         ,0
  //         ,'P'
  //         ,'P'
  //         ,0
  //         ,0
  //         ,0
  //         ,0
  //         ,0
  //         ,0
  //         ,0---TOTALÑ FACTURA SE ACTUALIZA CUANDO SE AGREGA LINEAS AL PEDIDO
  //      ,0
  //         ,0
  //         ,0
  //         ,0
  //         ,'N'
  //         , GETDATE()

  //      ,0
  //         ,'N'
  //         ,'L'
  //         ,'1'
  //         ,'N'
  //         ,'F'
  //         ,'N'
  //         ,'L'
  //         ,'NIVEL DE PRECIO DEL CLIENTE EN LA TABLA CLIENTE'
  //         ,'COBRADOR EN LA TABLA LCIENTE'
  //         ,'RUTA ESTA EN LA TABLA DEL CLIENTE'
  //         ,'ERPADMIN'
  //         ,'CONDICION DE PAGO SE EXTRAE DEL CLIENTE SI ES CONTADO ES 0 SI ES CREDITO VENDRA DEL CLIENTE'
  //         ,'BODEGA SERIA LA ETACION DE ENFERMERIA'
  //         ,'ZONA ESTA EN LA TABLA DEL CLIENTE'
  //         ,'VENDEDOR ESTA EN LA TABLA DEL CLIENTE'
  //         ,'CODIGO  CLIENTE 0000-0000-00001'
  //         ,'CLIENTE DIRECCION '
  //         ,'CODIGO DL CLIENTE'
  //         ,'CODIGO DEL CLIENTE'
  //         ,'PAIS SE EXTRAE DEL CLIENTE'
  //         ,0
  //         ,'FAC'
  //         ,'N'
  //         , NULL

  //      ,0
  //         ,'N'
  //         , NULL

  //      ,'N'
  //         ,'F'
  //         , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      ,'NOMBRE DEL CLIENTE DE LA TABLA CLIENTE'
  //         , NULL

  //      , NULL

  //      ,'P'
  //         , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      ,0
  //         , NULL

  //      ,0
  //         , NULL

  //      ,0
  //         ,0
  //         , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      ,'N'
  //         ,0
  //         , GETDATE()

  //      ,'ROWPOINTER SE GENERA SOLO'
  //         ,'ERPADMIN'
  //         ,'ERPADMIN'
  //         , GETDATE()

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      ,'CAMPOS UDF'
  //         ,'CAMPOS UDF'
  //         ,'CAMPOS UDF'
  //         , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL

  //      , NULL)
    }
}
using CS_HOSPITALARIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models
{
    public static class Pedido_logic
    {

        public static int Ult_Consect(string pedido, int listado, bool edit_line)
        {
            CS_HOSPITALARIO.Models.HospitalarioBD db = new CS_HOSPITALARIO.Models.HospitalarioBD();
            int consecu = 0;
            int maximo_contador = 0;
            var pedido_exist = db.PEDIDO_LINEA.Where(y => y.PEDIDO == pedido).ToList();

            if (pedido_exist.Count > 0)//Quiere decir que ya hay un registro en base de datos del documento
            {
                int maxp = Convert.ToInt32(pedido_exist.Max(obj => obj.PEDIDO_LINEA1));

                maximo_contador = maximo_contador + maxp;

            }
            else
            {
                if (listado > 0)
                {
                    maximo_contador = maximo_contador + 1;
                }
                else
                {
                    maximo_contador = 1;
                }
            }




            consecu = maximo_contador;


            return consecu;
        }
    }
}
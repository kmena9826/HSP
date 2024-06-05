using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS_HOSPITALARIO.Models.CustomModels;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System;
using System.Text;

namespace CS_HOSPITALARIO.Models.CustomMethods
{
    public class Utilerias
    {
        private HospitalarioBD dbContext = new HospitalarioBD();

        public List<MenuItem> GetMenuByUser(int userID)
        {
            List<MenuItem> menuInformation = new List<MenuItem>();

            var userRoleInformation = (from a in dbContext.CS_ROL_USER.Where(z => z.ID_USUARIO == userID && z.ISACTIVE == true) select a).FirstOrDefault();

            if (userRoleInformation != null)
            {
                var menusItems = (from a in dbContext.CS_CONTROL_ACCION_DETALLE.Where(z => z.ID_ROL_USER == userRoleInformation.ID).GroupBy(x => x.ID_CAT_DETALLE) select a);
                if (menusItems != null)
                {
                    foreach (var i in menusItems)
                    {
                        MenuItem menu = new MenuItem();
                        menu.MenuItems = new List<SubMenuItem>();
                        menu.MenuNavegacion = (from a in dbContext.CS_CATALOGO_DETALLE.Where(z => z.ID_CAT_DETALLE == i.Key) select a).FirstOrDefault().DESCRIPCION;
                        menu.Icono = (from a in dbContext.CS_CATALOGO_DETALLE.Where(z => z.ID_CAT_DETALLE == i.Key) select a).FirstOrDefault().ICONO;
                        var subMenu = (from a in dbContext.CS_CONTROL_ACCION_DETALLE.Where(z => z.ID_ROL_USER == userRoleInformation.ID && z.ID_CAT_DETALLE == i.Key).GroupBy(x => x.ID_CONTROLADOR) select a);
                        if (subMenu != null)
                            foreach (var j in subMenu)
                            {
                                SubMenuItem subMenuItem = new SubMenuItem();
                                var controller = (from a in dbContext.CS_CONTROLADOR.Where(z => z.ID == j.Key) select a).FirstOrDefault();
                                if (controller != null)
                                {
                                    subMenuItem.ControllerName = controller.CONTROLADOR;
                                    subMenuItem.ActionName = controller.CARPETA;
                                    subMenuItem.LinkText = controller.NOMBRE;
                                }
                                menu.MenuItems.Add(subMenuItem);

                            }
                        menuInformation.Add(menu);
                    }
                }
            }

            return menuInformation;

        }

        public int GetUser()
        {
            int userID = 0;          
            try
            {
                userID = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            }
            catch { userID = 0; }
            return userID;
        }
        public int? GetUsuario()
        {
            int? userID = 0;
            try
            {
                userID = int.Parse(HttpContext.Current.User.Identity.GetUserId());
            }
            catch { userID = 0; }
            return userID;
        }
        public int GetRol(int user)
        {
            int rol = 0;
            using (HospitalarioBD context = new HospitalarioBD())
            {
                rol = Convert.ToInt32(context.CS_ROL_USER.Where(x => x.ID_USUARIO == user).FirstOrDefault().ID_ROL);
            }
            return rol;
        }
        public string GetPasswordCrypted(string stringToEncrypt)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(stringToEncrypt));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

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

        public int GetVersionPrecio(string nivel)
        {
            int version = 0;
            var inicio = DateTime.Now;
         
            var dat_ver = (from p in dbContext.VERSION_NIVEL where p.NIVEL_PRECIO == nivel && DateTime.Now >= p.FECHA_INICIO &&   DateTime.Now <= p.FECHA_CORTE && p.ESTADO== "A" select p).FirstOrDefault();
            if (dat_ver != null)
            {
                version = Convert.ToInt32(dat_ver.VERSION);
            }

            return version;
        }
    }
}
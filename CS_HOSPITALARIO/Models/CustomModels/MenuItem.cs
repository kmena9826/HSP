using CS_HOSPITALARIO.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO.Models.CustomModels
{
    public class MenuItem
    {
        public string MenuNavegacion { get; set; }
        public string Icono { get; set; }
        public List<SubMenuItem> MenuItems { get; set; }
    }
}
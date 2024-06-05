using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CS_HOSPITALARIO_Front_end.Enums
{
    public static class PuestoPersonalEnum
    {
        public static SelectListItem Radiologo { get { return new SelectListItem { Text = "Radiologo", Value = "1" }; } }
        public static SelectListItem Enfermero { get { return new SelectListItem { Text = "Enfermero/Enfermera", Value = "2" }; } }
        public static SelectListItem Laboratorista { get { return new SelectListItem { Text = "Laboratorista", Value = "3" }; } }
    }
}
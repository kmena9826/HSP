using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CS_HOSPITALARIO_Front_end.Models.ViewModel
{
    public class LaboratorioViewModel
    {
        public virtual DateTime? desde { get; set; }
        public virtual DateTime? hasta { get; set; }

        public virtual ICollection<CS_HOSPITALARIO.Models.SpListadoExamenLaboratorioRecepcionado_Result> ListadoExamenLaboratorioRecepcionado_Result { get; set; }
    }
}
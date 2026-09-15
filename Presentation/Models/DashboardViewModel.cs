using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class DashboardViewModel
    {
        public int EstudiantesEvaluados { get; set; }

        public int EvaluacionesRealizadas { get; set; }

        public int RequierenSeguimiento { get; set; }

        public DateTime? UltimaEvaluacion { get; set; }
    }
}
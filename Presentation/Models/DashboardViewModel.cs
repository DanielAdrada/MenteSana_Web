using Data.Models;
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

        public List<DassDashboardStatisticDTO> EstadisticasDASS { get; set; }

        public List<NotificacionDTO> Notificaciones { get; set; } = new List<NotificacionDTO>();

        public int NotificacionesPendientes { get; set; }
    }
}
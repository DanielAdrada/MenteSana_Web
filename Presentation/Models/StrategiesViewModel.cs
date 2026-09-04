using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class StrategiesViewModel
    {
        public int EstrategiaId { get; set; }

        public string Dimension { get; set; }

        public string Area { get; set; }

        public string Nivel { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public bool Activa { get; set; }

        public string UsuId { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}
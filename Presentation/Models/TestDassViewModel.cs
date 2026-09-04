using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class TestDassViewModel
    {
        public List<string> Preguntas { get; set; }

        public int Bloque { get; set; }

        public int InicioPregunta { get; set; }

        public bool UltimoBloque { get; set; }

        public int Progreso { get; set; }
    }
}
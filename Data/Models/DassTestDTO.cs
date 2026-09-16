using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class DassTestDTO
    {
        public int TestId { get; set; }

        public string EstudianteId { get; set; }

        public string Estudiante { get; set; }

        public string GradoCurso { get; set; }

        public string NivelDepresion { get; set; }

        public string NivelAnsiedad { get; set; }

        public string NivelEstres { get; set; }

        public DateTime Fecha { get; set; }
    }
}

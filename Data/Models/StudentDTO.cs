using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class StudentDTO
    {
        public string Id { get; set; }

        public string Usuario { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Estado { get; set; }

        public string Grado { get; set; }

        public string Curso { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}

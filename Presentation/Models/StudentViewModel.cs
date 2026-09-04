using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class StudentViewModel
    {
        // ===== DATOS DEL ESTUDIANTE =====

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }

        // ===== INFORMACIÓN PERSONAL =====

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        // ===== ESTADO =====

        public string Estado { get; set; }

        public bool IsEditing { get; set; }

        // ===== LISTADO =====

        public List<StudentViewModel> Students { get; set; }
            = new List<StudentViewModel>();
    }
}

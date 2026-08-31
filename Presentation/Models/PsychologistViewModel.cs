using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class PsychologistViewModel
    {
        // ===== DATOS DE ACCESO =====

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(20)]
        public string Id { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(50, MinimumLength = 8,
            ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; }

        // ===== INFORMACIÓN PERSONAL =====

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; }

        // ===== INFORMACIÓN PROFESIONAL =====

        [Required(ErrorMessage = "La formación académica es obligatoria.")]
        public string Formacion { get; set; }

        [Required(ErrorMessage = "El horario de atención es obligatorio.")]
        public string Horario { get; set; }
        public bool IsEditing { get; set; }
        public string Estado { get; set; }


        // ===== LISTADO =====

        public List<PsychologistViewModel> Psychologists { get; set; }
            = new List<PsychologistViewModel>();

        
    }
}
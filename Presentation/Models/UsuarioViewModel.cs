using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class UsuarioViewModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(100, MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }


        // CAMPOS PARA ESTUDIANTE
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
    }
}

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
        [Display(Name = "Identificación / Contraseña")]
        public string Identificacion { get; set; }

        // CAMPOS PARA ESTUDIANTE
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese el nombre del usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        [DataType(DataType.Password)]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de usuario")]
        [RegularExpression("^(ESTUDIANTE|PSICOLOGO)$",
            ErrorMessage = "Rol inválido")]
        public string Rol { get; set; } // ESTUDIANTE | PSICOLOGO
    }
}
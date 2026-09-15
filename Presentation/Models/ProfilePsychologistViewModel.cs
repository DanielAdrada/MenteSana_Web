using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class ProfilePsychologistViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Formación")]
        public string Formacion { get; set; }

        [Display(Name = "Horario")]
        public string Horario { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        public string FotoRuta { get; set; }

        public HttpPostedFileBase Foto { get; set; }

    }
}
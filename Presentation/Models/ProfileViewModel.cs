using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        //Ruta de la imagen (BD)
        public string FotoRuta { get; set; }

        //Archivo recibido del formulario
        public HttpPostedFileBase Foto { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class ResourcesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresar el título ")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El título debe tener entre 5 y 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Ingrese una descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Seleccionar el tipo de recurso")]
        public string Tipo { get; set; }

        public HttpPostedFileBase Archivo { get; set; }
        public string ArchivoActual { get; set; }       // archivo anterior
        public string Url { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public List<Data.Models.ResourcesDTO> ListaRecursos { get; set; }
    }
}
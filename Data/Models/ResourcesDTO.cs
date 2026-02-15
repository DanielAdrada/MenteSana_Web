using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class ResourcesDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Archivo { get; set; }
        public string Url { get; set; }
        public DateTime Fecha { get; set; }
    }
}
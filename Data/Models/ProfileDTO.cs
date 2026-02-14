using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class ProfileDTO
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string FotoRuta { get; set; }
    }
}
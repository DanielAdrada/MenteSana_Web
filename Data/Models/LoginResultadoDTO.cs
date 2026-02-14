using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class LoginResultadoDTO
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public UsuarioSesionDTO Sesion { get; set; }
    }
}
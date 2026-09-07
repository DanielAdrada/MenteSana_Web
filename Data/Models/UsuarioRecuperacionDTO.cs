using System;

namespace Data.Models
{
    public class UsuarioRecuperacionDTO
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
    }
}
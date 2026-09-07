using System;

namespace Data.Models
{
    public class RecuperacionContrasenaDTO
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class CommentViewModel
    {
        public int UserId { get; set; }

        // Identidad visual
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string FotoPerfil { get; set; }

        // Info del comentario
        public string Usuario { get; set; }
        public string Contenido { get; set; }
        public DateTime Fecha { get; set; }

        // Helper 
        public string Iniciales =>
            (!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Apellido))
            ? $"{Nombre[0]}{Apellido[0]}".ToUpper()
            : Usuario?.Substring(0, 1).ToUpper();
    }
}
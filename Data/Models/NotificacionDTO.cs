using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Models
{
    public class NotificacionDTO
    {
        public int Id { get; set; }

        public int TestId { get; set; }

        public string EstudianteId { get; set; }

        public string Tipo { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public bool Leida { get; set; }

        public bool Activa { get; set; }

    }
}
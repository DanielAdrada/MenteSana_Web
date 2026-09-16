using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Models
{
    public class StudentHomeViewModel
    {
        public ProfileDTO Perfil { get; set; }

        public List<CommentDTO> Comentarios { get; set; }

        public List<ResourcesDTO> Recursos { get; set; }

        // LISTADO DE PSICÓLOGOS
        public List<PsychologistDTO> Psicologos { get; set; }
            = new List<PsychologistDTO>();

    }
}
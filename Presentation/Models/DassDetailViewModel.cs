using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class DassDetailViewModel
    {
        public DassTestDTO Test { get; set; }

        public List<DassAnswerDTO> Respuestas { get; set; }
    }
}

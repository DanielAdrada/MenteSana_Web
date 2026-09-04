using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic.Models
{
    public class EmotionResult
    {
        public string depresion { get; set; }
        public string ansiedad { get; set; }
        public string estres { get; set; }
        public string dimension_prioritaria { get; set; }
        public Dictionary<string, Dictionary<string, double>> areas { get; set; }
        public List<AreaPrioritaria> areas_prioritarias { get; set; }
        public List<Estrategia> estrategias { get; set; }
    }

    public class AreaPrioritaria 
    { 
        public string area { get; set; } 
        public double puntaje { get; set; }
    }

}
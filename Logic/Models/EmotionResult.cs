using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic.Models
{
    public class EmotionResult
    {
        public string emocion_principal { get; set; }
        public string emocion_secundaria { get; set; }

        public Dictionary<string, double> scores { get; set; }

    }
}
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Services
{
    public class EstrategiaService
    {
        private readonly StrategiesLogic _strategiesLogic =
            new StrategiesLogic();

        public List<Estrategia> ObtenerEstrategias(EmotionResult resultado)
        {
            var estrategias = new List<Estrategia>();

            if (resultado == null)
                return estrategias;

            if (resultado.areas_prioritarias == null ||
                resultado.areas_prioritarias.Count == 0)
            {
                return estrategias;
            }

            // Dimensión prioritaria obtenida del resultado del DASS-42
            string dimension =
                resultado.dimension_prioritaria?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(dimension))
                return estrategias;

            // Obtener el nivel correspondiente a la dimensión prioritaria
            string nivel = ObtenerNivel(dimension, resultado);

            if (string.IsNullOrWhiteSpace(nivel))
                return estrategias;

            // Buscar las estrategias correspondientes a cada área prioritaria
            foreach (var area in resultado.areas_prioritarias)
            {
                if (area == null ||
                    string.IsNullOrWhiteSpace(area.area))
                {
                    continue;
                }

                List<Dictionary<string, object>> estrategiasBD =
                    _strategiesLogic.ObtenerEstrategiasPorResultado(
                        dimension,
                        area.area,
                        nivel
                    );

                foreach (var item in estrategiasBD)
                {
                    estrategias.Add(new Estrategia
                    {
                        Titulo = item.ContainsKey("estrategia_titulo")
                            ? item["estrategia_titulo"]?.ToString()
                            : "",

                        Descripcion = item.ContainsKey("estrategia_descripcion")
                            ? item["estrategia_descripcion"]?.ToString()
                            : "",

                        Area = item.ContainsKey("estrategia_area")
                            ? item["estrategia_area"]?.ToString()
                            : ""
                    });
                }
            }

            // Evitar estrategias repetidas
            return estrategias
                .GroupBy(e => e.Titulo)
                .Select(g => g.First())
                .Take(5)
                .ToList();
        }


        private string ObtenerNivel(
            string dimension,
            EmotionResult resultado)
        {
            switch (dimension)
            {
                case "DEPRESION":
                    return resultado.depresion;

                case "ANSIEDAD":
                    return resultado.ansiedad;

                case "ESTRES":
                    return resultado.estres;

                default:
                    return null;
            }
        }
    }
}
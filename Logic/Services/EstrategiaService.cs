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
            var estrategiasFinales = new List<Estrategia>();

            if (resultado == null)
                return estrategiasFinales;

            // =====================================================
            // 1. OBTENER LOS RESULTADOS DE LAS TRES DIMENSIONES
            // =====================================================

            var dimensiones = new List<ResultadoDimension>
            {
                new ResultadoDimension
                {
                    Dimension = "DEPRESION",
                    Nivel = NormalizarNivel(resultado.depresion)
                },

                new ResultadoDimension
                {
                    Dimension = "ANSIEDAD",
                    Nivel = NormalizarNivel(resultado.ansiedad)
                },

                new ResultadoDimension
                {
                    Dimension = "ESTRES",
                    Nivel = NormalizarNivel(resultado.estres)
                }
            };

            // =====================================================
            // 2. NORMAL NO GENERA ESTRATEGIAS
            // =====================================================

            dimensiones = dimensiones
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Nivel) &&
                    x.Nivel != "Normal")
                .OrderByDescending(x =>
                    ObtenerPrioridadNivel(x.Nivel))
                .ToList();

            // Si las tres dimensiones están en Normal,
            // no se generan estrategias.
            if (!dimensiones.Any())
                return estrategiasFinales;


            // =====================================================
            // 3. OBTENER ESTRATEGIAS DE CADA DIMENSIÓN
            // =====================================================

            var estrategiasPorDimension =
                new List<List<Estrategia>>();

            foreach (var dimension in dimensiones)
            {
                var estrategias =
                    ObtenerEstrategiasDimension(
                        dimension.Dimension,
                        dimension.Nivel
                    );

                if (estrategias.Any())
                {
                    estrategiasPorDimension.Add(estrategias);
                }
            }

            if (!estrategiasPorDimension.Any())
                return estrategiasFinales;


            // =====================================================
            // 4. DISTRIBUIR LAS ESTRATEGIAS
            // =====================================================
            //
            // Ejemplo:
            //
            // DEPRESION  = Severo
            // ANSIEDAD   = Severo
            // ESTRES     = Moderado
            //
            // En lugar de tomar:
            //
            // 1. Depresión
            // 2. Depresión
            // 3. Depresión
            // 4. Depresión
            // 5. Depresión
            //
            // se van alternando las dimensiones.
            // =====================================================

            int posicion = 0;

            while (
                estrategiasFinales.Count < 5 &&
                estrategiasPorDimension.Any(lista =>
                    posicion < lista.Count))
            {
                foreach (var lista in estrategiasPorDimension)
                {
                    if (estrategiasFinales.Count >= 5)
                        break;

                    if (posicion < lista.Count)
                    {
                        estrategiasFinales.Add(lista[posicion]);
                    }
                }

                posicion++;
            }


            // =====================================================
            // 5. ELIMINAR DUPLICADOS
            // =====================================================

            return estrategiasFinales
                .Where(e =>
                    e != null &&
                    e.EstrategiaId > 0 &&
                    !string.IsNullOrWhiteSpace(e.Titulo))
                .GroupBy(e => e.EstrategiaId)
                .Select(g => g.First())
                .Take(5)
                .ToList();
        }


        // =========================================================
        // OBTENER ESTRATEGIAS PARA UNA DIMENSIÓN Y NIVEL
        // =========================================================

        private List<Estrategia> ObtenerEstrategiasDimension(
            string dimension,
            string nivel)
        {
            var resultado = new List<Estrategia>();

            if (string.IsNullOrWhiteSpace(dimension) ||
                string.IsNullOrWhiteSpace(nivel))
            {
                return resultado;
            }

            List<Dictionary<string, object>> estrategiasBD =
                _strategiesLogic.ObtenerEstrategiasPorResultado(
                    dimension,
                    nivel
                );

            if (estrategiasBD == null ||
                estrategiasBD.Count == 0)
            {
                return resultado;
            }

            foreach (var item in estrategiasBD)
            {
                int estrategiaId = 0;

                if (item.ContainsKey("estrategia_id") &&
                    item["estrategia_id"] != null)
                {
                    int.TryParse(
                        item["estrategia_id"].ToString(),
                        out estrategiaId
                    );
                }

                resultado.Add(
                    new Estrategia
                    {
                        EstrategiaId = estrategiaId,

                        Titulo =
                            item.ContainsKey("estrategia_titulo")
                                ? item["estrategia_titulo"]?.ToString()
                                : "",

                        Descripcion =
                            item.ContainsKey("estrategia_descripcion")
                                ? item["estrategia_descripcion"]?.ToString()
                                : ""
                    }
                );
            }

            return resultado;
        }


        // =========================================================
        // NORMALIZAR LOS NIVELES DEL DASS
        // =========================================================

        private string NormalizarNivel(string nivel)
        {
            if (string.IsNullOrWhiteSpace(nivel))
                return "";

            string valor =
                nivel.Trim()
                     .Replace("_", " ")
                     .ToLower();

            switch (valor)
            {
                case "normal":
                    return "Normal";

                case "leve":
                    return "Leve";

                case "moderado":
                    return "Moderado";

                case "severo":
                    return "Severo";

                case "extremadamente severo":
                    return "Extremadamente Severo";

                default:
                    return "";
            }
        }


        // =========================================================
        // PRIORIDAD DE LOS NIVELES
        // =========================================================

        private int ObtenerPrioridadNivel(string nivel)
        {
            switch (NormalizarNivel(nivel))
            {
                case "Normal":
                    return 1;

                case "Leve":
                    return 2;

                case "Moderado":
                    return 3;

                case "Severo":
                    return 4;

                case "Extremadamente Severo":
                    return 5;

                default:
                    return 0;
            }
        }


        // =========================================================
        // CLASE AUXILIAR
        // =========================================================

        private class ResultadoDimension
        {
            public string Dimension { get; set; }

            public string Nivel { get; set; }
        }


        // =========================================================
        // GUARDAR RELACIÓN TEST - ESTRATEGIA
        // =========================================================

        public bool GuardarTestEstrategia(
            int testId,
            int estrategiaId)
        {
            if (testId <= 0 ||
                estrategiaId <= 0)
            {
                return false;
            }

            return _strategiesLogic.GuardarTestEstrategia(
                testId,
                estrategiaId
            );
        }
    }
}
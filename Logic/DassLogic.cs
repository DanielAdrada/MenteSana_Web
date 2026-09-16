using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class DassLogic
    {
        private DassDat dassDat = new DassDat();

        //Guarda el test completo
        public int GuardarTest(string estudianteId,string nivelDepresion,string nivelAnsiedad,string nivelEstres)
        {
            // Validar estudiante
            if (string.IsNullOrWhiteSpace(estudianteId))
                return 0;

            estudianteId = estudianteId.Trim();

            // Validar resultados
            if (string.IsNullOrWhiteSpace(nivelDepresion))
                return 0;

            if (string.IsNullOrWhiteSpace(nivelAnsiedad))
                return 0;

            if (string.IsNullOrWhiteSpace(nivelEstres))
                return 0;

            return dassDat.SaveTest(
                estudianteId,
                nivelDepresion.Trim(),
                nivelAnsiedad.Trim(),
                nivelEstres.Trim()
            );
        }


        //Guarda las 42 respuestas 
        public bool GuardarRespuestas(int testId, List<int> respuestas)
        {
            // Valida que el test exista 
            if (testId <= 0)
                return false;

            // Tener exactamente 42 respuestas
            if (respuestas == null || respuestas.Count != 42)
                return false;

            // Validar cada respuesta
            for (int i = 0; i < respuestas.Count; i++)
            {
                int valor = respuestas[i];

                // Las respuestas válidas son 0, 1, 2 y 3
                if (valor < 0 || valor > 3)
                    return false;

                bool guardado = dassDat.SaveAnswer(
                    testId,
                    i + 1,
                    valor
                );

                if (!guardado)
                    return false;
            }
            return true;
        }


        // DATOS PARA EL DASHBOARD
        public int ObtenerEstudiantesEvaluados()
        {
            return dassDat.GetTotalStudentsEvaluated();
        }

        public int ObtenerEvaluacionesRealizadas()
        {
            return dassDat.GetTotalTests();
        }

        public int ObtenerEstudiantesSeguimiento()
        {
            return dassDat.GetStudentsFollowUp();
        }

        public DateTime? ObtenerUltimaEvaluacion()
        {
            return dassDat.GetLastTestDate();
        }

        // RESULTADOS DASS - PANTALLA PRINCIPAL
        public List<DassTestDTO> ObtenerUltimosTests()
        {
            return dassDat.ListLatestTests();
        }

        // HISTORIAL DE UN ESTUDIANTE
        public List<DassTestDTO> ObtenerHistorial(string estudianteId)
        {
            if (string.IsNullOrWhiteSpace(estudianteId))
                return new List<DassTestDTO>();

            return dassDat.GetTestHistory(estudianteId.Trim());
        }

        // RESPUESTAS DE UN TEST
        public List<DassAnswerDTO> ObtenerRespuestas(int testId)
        {
            if (testId <= 0)
                return new List<DassAnswerDTO>();

            return dassDat.GetAnswers(testId);
        }

        // RESUMEN DEL DASHBOARD
        public DassDashboardSummaryDTO ObtenerResumenDashboard()
        {
            return dassDat.GetDashboardSummary();
        }

        // ESTADÍSTICAS DEL DASHBOARD
        public List<DassDashboardStatisticDTO> ObtenerEstadisticasDashboard()
        {
            return dassDat.GetDashboardStatistics();
        }

        public DassTestDTO ObtenerTestPorId(int testId)
        {
            if (testId <= 0)
                return null;

            return dassDat.GetTestById(testId);
        }
    }
}
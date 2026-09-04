using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class StrategiesLogic
    {
        private StrategiesDat strategiesDat = new StrategiesDat();


        // Guarda una estrategia
        public int GuardarEstrategia(
            string dimension,
            string area,
            string nivel,
            string titulo,
            string descripcion,
            string usuId)
        {
            // Validar datos
            if (string.IsNullOrWhiteSpace(dimension))
                return 0;

            if (string.IsNullOrWhiteSpace(area))
                return 0;

            if (string.IsNullOrWhiteSpace(nivel))
                return 0;

            if (string.IsNullOrWhiteSpace(titulo))
                return 0;

            if (string.IsNullOrWhiteSpace(descripcion))
                return 0;

            if (string.IsNullOrWhiteSpace(usuId))
                return 0;

            return strategiesDat.SaveEstrategia(
                dimension.Trim(),
                area.Trim(),
                nivel.Trim(),
                titulo.Trim(),
                descripcion.Trim(),
                usuId.Trim()
            );
        }


        // Obtiene todas las estrategias
        public List<Dictionary<string, object>> ObtenerEstrategias()
        {
            return strategiesDat.GetEstrategias();
        }


        // Actualiza una estrategia
        public bool ActualizarEstrategia(
            int estrategiaId,
            string dimension,
            string area,
            string nivel,
            string titulo,
            string descripcion)
        {
            if (estrategiaId <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(dimension))
                return false;

            if (string.IsNullOrWhiteSpace(area))
                return false;

            if (string.IsNullOrWhiteSpace(nivel))
                return false;

            if (string.IsNullOrWhiteSpace(titulo))
                return false;

            if (string.IsNullOrWhiteSpace(descripcion))
                return false;

            return strategiesDat.UpdateEstrategia(
                estrategiaId,
                dimension.Trim(),
                area.Trim(),
                nivel.Trim(),
                titulo.Trim(),
                descripcion.Trim()
            );
        }


        // Activa o desactiva una estrategia
        public bool CambiarEstadoEstrategia(
            int estrategiaId,
            int activa)
        {
            if (estrategiaId <= 0)
                return false;

            if (activa != 0 && activa != 1)
                return false;

            return strategiesDat.CambiarEstadoEstrategia(
                estrategiaId,
                activa
            );
        }


        // Guarda una estrategia asignada a un test
        public bool GuardarTestEstrategia(
            int testId,
            int estrategiaId)
        {
            if (testId <= 0)
                return false;

            if (estrategiaId <= 0)
                return false;

            return strategiesDat.SaveTestEstrategia(
                testId,
                estrategiaId
            );
        }


        // Obtiene las estrategias asignadas a un test
        public List<Dictionary<string, object>> ObtenerTestEstrategias(
            int testId)
        {
            if (testId <= 0)
                return new List<Dictionary<string, object>>();

            return strategiesDat.GetTestEstrategias(testId);
        }


        // Obtiene el historial de estrategias de un estudiante
        public List<Dictionary<string, object>> ObtenerHistorialEstrategias(
            string estudianteId)
        {
            if (string.IsNullOrWhiteSpace(estudianteId))
                return new List<Dictionary<string, object>>();

            return strategiesDat.GetHistorialEstrategias(
                estudianteId.Trim()
            );
        }

    }
}
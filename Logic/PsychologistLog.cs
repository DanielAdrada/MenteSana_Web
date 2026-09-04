using Data;
using Data.Models;
using System;
using System.Collections.Generic;

namespace Logic
{
    public class PsychologistLog
    {
        private readonly PsychologistDat psychologistDat = new PsychologistDat();

        // ================= REGISTRAR PSICÓLOGO =================
        public bool InsertPsychologist(
            string id,
            string nombre,
            string apellido,
            string correo,
            string telefono,
            string formacion,
            string horario)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            if (string.IsNullOrWhiteSpace(apellido))
                return false;

            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(formacion))
                return false;

            if (string.IsNullOrWhiteSpace(horario))
                return false;

            return psychologistDat.InsertPsychologist(
                id.Trim(),
                nombre.Trim(),
                apellido.Trim(),
                correo.Trim(),
                telefono?.Trim(),
                formacion.Trim(),
                horario.Trim());
        }

        // ================= OBTENER POR ID =================
        public PsychologistDTO GetPsychologistById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return psychologistDat.GetPsychologistById(id);
        }

        // ================= LISTAR =================
        public List<PsychologistDTO> ListPsychologists()
        {
            return psychologistDat.ListPsychologists();
        }

        // ================= ACTUALIZAR =================
        public bool UpdatePsychologist(
            string id,
            string nombre,
            string apellido,
            string correo,
            string telefono,
            string formacion,
            string horario)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            if (string.IsNullOrWhiteSpace(apellido))
                return false;

            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(formacion))
                return false;

            if (string.IsNullOrWhiteSpace(horario))
                return false;

            return psychologistDat.UpdatePsychologist(
                id.Trim(),
                nombre.Trim(),
                apellido.Trim(),
                correo.Trim(),
                telefono?.Trim(),
                formacion.Trim(),
                horario.Trim());
        }

        // ================= CAMBIAR ESTADO =================
        public bool UpdatePsychologistStatus(string id, string estado)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            if (estado != "ACTIVO" && estado != "INACTIVO")
                return false;

            return psychologistDat.UpdatePsychologistStatus(id, estado);
        }

        public int CountPsychologists()
        {
            return psychologistDat.CountPsychologists();
        }
    }
}
using Data.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic
{
    public class ProfileLog
    {
        private readonly PsychologistDat psychologistDat = new PsychologistDat();
        private readonly StudentDat studentDat = new StudentDat();
        private readonly ProfileDat profileDat = new ProfileDat();
        private readonly UserDat userDat = new UserDat();


        // Perfil del estudiante
        public ProfileDTO GetProfile(string id)
        {
            ProfileDTO perfil = studentDat.GetProfile(id);

            if (perfil == null)
                return null;

            perfil.FotoRuta = profileDat.GetProfilePhoto(id);
            perfil.Usuario = userDat.GetUsernameById(id);
            return perfil;
        }

        public bool ExistsProfile(string id)
        {
            return studentDat.ExistsStudent(id);
        }


        public bool SaveProfile(
            string id,
            string nombre,
            string apellido,
            string grado,
            string curso,
            DateTime? fechaNacimiento)
        {
            if (!studentDat.ExistsStudent(id))
                return false;

            return studentDat.UpdateStudent(
                id,
                nombre,
                apellido,
                grado,
                curso,
                fechaNacimiento);
        }


        // Perfil del psicologo
        public PsychologistDTO GetPsychologistProfile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return psychologistDat.GetPsychologistById(id);
        }

        public bool SavePsychologistProfile(
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

            if (psychologistDat.GetPsychologistById(id) == null)
                return false;

            return psychologistDat.UpdatePsychologist(
                id,
                nombre,
                apellido,
                correo,
                telefono,
                formacion,
                horario);
        }


        // Foto
        public bool SaveProfilePhoto(string userId, string rutaFoto)
        {
            if (!profileDat.ExistsProfile(userId))
            {
                
                profileDat.InsertProfile(userId, userId);
            }

            return profileDat.UpdateProfilePhoto(userId, rutaFoto);
        }
        

        // Usuario
        public bool UpdateUsername(string id, string nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario))
                return false;

            return userDat.UpdateUsername(id, nuevoUsuario);
        }

        public string GetUsername(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return "";

            return userDat.GetUsernameById(id);
        }

        public string GetProfilePhoto(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return "";

            return profileDat.GetProfilePhoto(id);
        }



    }
}
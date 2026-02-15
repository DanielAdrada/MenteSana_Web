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
        private readonly StudentDat studentDat = new StudentDat();
        private readonly ProfileDat profileDat = new ProfileDat();
        private readonly UserDat userDat = new UserDat();

        public ProfileDTO GetProfile(string id)
        {
            // Obtener nombre y apellido desde estudiantes
            ProfileDTO perfil = studentDat.GetProfile(id);

            if (perfil == null)
                return null;

            // Obtener foto desde perfiles
            perfil.FotoRuta = profileDat.GetProfilePhoto(id);
            perfil.Usuario = userDat.GetUsernameById(id);
            return perfil;
        }

        public bool ExistsProfile(string id)
        {
            return studentDat.ExistsStudent(id);
        }


        public bool SaveProfile(string id, string nombre, string apellido)
        {
            // SOLO actualiza datos personales
            if (!studentDat.ExistsStudent(id))
                return false;

            return studentDat.UpdateStudent(id, nombre, apellido);
        }

        public bool SaveProfilePhoto(string userId, string rutaFoto)
        {
            if (!profileDat.ExistsProfile(userId))
            {
                
                profileDat.InsertProfile(userId, userId);
            }

            return profileDat.UpdateProfilePhoto(userId, rutaFoto);
        }
        
        public bool UpdateUsername(string id, string nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario))
                return false;

            return userDat.UpdateUsername(id, nuevoUsuario);
        }
    }
}
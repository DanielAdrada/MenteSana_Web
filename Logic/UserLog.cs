using System.Data;
using Data.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;




namespace Logic
{
    public class UserLog
    {
        private readonly UserDat userDat = new UserDat();

        // ================= SEGURIDAD =================

        private string HashPassword(string texto)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private string GenerateSalt(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        // ================= USUARIO =================
        public LoginResultadoDTO IniciarSesion(string usuario, string identificacion, string rolSeleccionado)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(identificacion) ||
        string.IsNullOrWhiteSpace(rolSeleccionado))
            {
                return new LoginResultadoDTO
                {
                    Exitoso = false,
                    Mensaje = "Por favor completa todos los campos."
                };
            }

            string salt = userDat.GetSalt(usuario);

            if (salt == null)
            {
                return new LoginResultadoDTO
                {
                    Exitoso = false,
                    Mensaje = "El usuario no existe."
                };
            }

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(identificacion + salt);
                string hash = BitConverter
                    .ToString(sha.ComputeHash(bytes))
                    .Replace("-", "")
                    .ToLower();

                string rol = userDat.Login(usuario, hash);

                if (rol == null)
                {
                    return new LoginResultadoDTO
                    {
                        Exitoso = false,
                        Mensaje = "La contraseña es incorrecta."
                    };
                }

                //  VALIDACIÓN DE ROL (REQUERIMIENTO CLAVE)
                if (!rol.Equals(rolSeleccionado, StringComparison.OrdinalIgnoreCase))
                {
                    return new LoginResultadoDTO
                    {
                        Exitoso = false,
                        Mensaje = "El tipo de usuario seleccionado no corresponde a la cuenta."
                    };
                }

                return new LoginResultadoDTO
                {
                    Exitoso = true,
                    Mensaje = "¡Bienvenido! Inicio de sesión exitoso.",
                    Sesion = new UsuarioSesionDTO
                    {
                        Id = identificacion,
                        Usuario = usuario,
                        Rol = rol
                    }
                };
            }
        }

        public bool RegistrarUsuario(
            string id,
            string usuario,
            string identificacion,
            string nombre,
            string apellido
        )
        {
            string salt = GenerateSalt();
            string hash = HashPassword(identificacion + salt);

            bool creado = userDat.RegistrarUsuarioConSalt(
                id,
                usuario,
                hash,
                salt,
                "ESTUDIANTE"
            );

            if (!creado)
                return false;

            // Crear estudiante 
            StudentDat studentDat = new StudentDat();
            bool creadoEstudiante = studentDat.InsertStudent(id, nombre, apellido);

            if (!creadoEstudiante)
                return false;

            // crear perfil vacío
            ProfileDat profileDat = new ProfileDat();
            profileDat.InsertProfile(id, id);

            return true;
        }
        public bool UpdateUsername(string userId, string nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario))
                return false;

            return userDat.UpdateUsername(userId, nuevoUsuario);
        }
    }
}
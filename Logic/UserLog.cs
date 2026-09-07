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

        // ================= TOKEN DE RECUPERACIÓN =================

        private string GenerateRecoveryToken()
        {
            byte[] tokenBytes = new byte[32];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }
    

        // ================= USUARIO =================
        public LoginResultadoDTO IniciarSesion(string usuario, string password, string rolSeleccionado)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password) ||
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

            
                string hash = HashPassword(password + salt);

                UsuarioSesionDTO sesion = userDat.Login(usuario, hash);

                if (sesion == null)
                {
                    return new LoginResultadoDTO
                    {
                        Exitoso = false,
                        Mensaje = "La contraseña es incorrecta."
                    };
                }

                //  VALIDACIÓN DE ROL (REQUERIMIENTO CLAVE)
                if (!sesion.Rol.Equals(rolSeleccionado, StringComparison.OrdinalIgnoreCase))
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
                    Sesion = sesion
                };
            }


        // ================= REGISTRO ESTUDIANTE =================

        public bool RegistrarUsuario(
            string id,
            string usuario,
            string correo,
            string password,
            string nombre,
            string apellido)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password + salt);

            bool creado = userDat.RegistrarUsuarioConSalt(
                id,
                usuario,
                correo,
                hash,
                salt,
                "ESTUDIANTE"
            );

            if (!creado)
                return false;

            // Crear estudiante
            StudentDat studentDat = new StudentDat();

            bool creadoEstudiante = studentDat.InsertStudent(
                id,
                nombre,
                apellido
            );

            if (!creadoEstudiante)
                return false;

            // Crear perfil vacío
            ProfileDat profileDat = new ProfileDat();

            profileDat.InsertProfile(id, id);

            return true;
        }

        // ================= ACTUALIZAR USUARIO =================

        public bool UpdateUsername(
            string userId,
            string nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario))
                return false;

            return userDat.UpdateUsername(
                userId,
                nuevoUsuario
            );
        }

        // ================= REGISTRO DE USUARIO =================

        public bool RegisterUser(
            string id,
            string usuario,
            string correo,
            string password,
            string rol)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password + salt);

            return userDat.RegistrarUsuarioConSalt(
                id,
                usuario,
                correo,
                hash,
                salt,
                rol
            );
        }

        // ================= SOLICITAR RECUPERACIÓN =================

        public bool SolicitarRecuperacion(
            string correo,
            out string token)
        {
            token = null;

            if (string.IsNullOrWhiteSpace(correo))
                return false;

            UsuarioRecuperacionDTO usuario =
                userDat.GetUsuarioByCorreo(correo);

            if (usuario == null)
                return false;

            token = GenerateRecoveryToken();

            DateTime fechaExpiracion =
                DateTime.Now.AddMinutes(30);

            return userDat.InsertRecuperacion(
                usuario.Id,
                token,
                fechaExpiracion
            );
        }

        // ================= VALIDAR TOKEN =================

        public RecuperacionContrasenaDTO
            ValidarTokenRecuperacion(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            return userDat.GetRecuperacionValida(token);
        }

        // ================= CAMBIAR CONTRASEÑA =================

        public bool CambiarContrasena(
            string token,
            string nuevaContrasena)
        {
            if (string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                return false;
            }

            if (nuevaContrasena.Length < 8)
                return false;

            RecuperacionContrasenaDTO recuperacion =
                userDat.GetRecuperacionValida(token);

            if (recuperacion == null)
                return false;

            string salt = GenerateSalt();

            string hash =
                HashPassword(nuevaContrasena + salt);

            bool actualizada = userDat.UpdatePassword(
                recuperacion.UsuarioId,
                hash,
                salt
            );

            if (!actualizada)
                return false;

            return userDat.MarcarRecuperacionUsada(
                recuperacion.Id
            );
        }
    }
}
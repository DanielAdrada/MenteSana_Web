using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Data.Models;
using System.Data.SqlClient;


namespace Data
{
    public class UserDat
    {
        public UsuarioSesionDTO Login(string usuario, string passwordHash)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proLoginUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_nombre_usuario", usuario);
                    cmd.Parameters.AddWithValue("v_contrasena", passwordHash);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UsuarioSesionDTO
                            {
                                Id = reader["usu_id"].ToString(),
                                Usuario = reader["usu_nombre_usuario"].ToString(),
                                Rol = reader["usu_rol"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
        }


        public bool RegistrarUsuarioConSalt(
            string id,
            string nombreUsuario,
            string correo,
            string hashContrasena,
            string salt,
            string rol
        )
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);
                    cmd.Parameters.AddWithValue("v_nombre_usuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("v_correo", correo);
                    cmd.Parameters.AddWithValue("v_contrasena", hashContrasena);
                    cmd.Parameters.AddWithValue("v_salt", salt);
                    cmd.Parameters.AddWithValue("v_rol", rol);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public string GetSalt(string usuario)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT usu_salt FROM tbl_usuarios WHERE usu_nombre_usuario = @usuario";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }
        public bool UpdateUsername(string userId, string nuevoUsuario)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = @"
            UPDATE tbl_usuarios 
            SET usu_nombre_usuario = @usuario 
            WHERE usu_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", nuevoUsuario);
                    cmd.Parameters.AddWithValue("@id", userId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public string GetUsernameById(string userId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT usu_nombre_usuario FROM tbl_usuarios WHERE usu_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }
        // ================= RECUPERACIÓN DE CONTRASEÑA =================

        public UsuarioRecuperacionDTO GetUsuarioByCorreo(string correo)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetUsuarioByCorreo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_correo", correo);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UsuarioRecuperacionDTO
                            {
                                Id = reader["usu_id"].ToString(),
                                Usuario = reader["usu_nombre_usuario"].ToString(),
                                Correo = reader["usu_correo"].ToString(),
                                Rol = reader["usu_rol"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public bool InsertRecuperacion(
            string usuarioId,
            string token,
            DateTime fechaExpiracion)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertRecuperacion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_usu_id", usuarioId);
                    cmd.Parameters.AddWithValue("v_token", token);
                    cmd.Parameters.AddWithValue("v_fecha_expiracion", fechaExpiracion);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public RecuperacionContrasenaDTO GetRecuperacionValida(string token)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetRecuperacionValida", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_token", token);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RecuperacionContrasenaDTO
                            {
                                Id = Convert.ToInt32(reader["rec_id"]),
                                UsuarioId = reader["rec_usu_id"].ToString(),
                                Token = reader["rec_token"].ToString(),
                                FechaExpiracion = Convert.ToDateTime(
                                    reader["rec_fecha_expiracion"]
                                ),
                                Usado = Convert.ToBoolean(reader["rec_usado"])
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public bool UpdatePassword(
            string usuarioId,
            string hashContrasena,
            string salt)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdateUsuarioPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_id", usuarioId);
                    cmd.Parameters.AddWithValue("v_contrasena", hashContrasena);
                    cmd.Parameters.AddWithValue("v_salt", salt);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool MarcarRecuperacionUsada(int recuperacionId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "proMarcarRecuperacionUsada",
                    conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_rec_id", recuperacionId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
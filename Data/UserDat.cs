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
        public string Login(string usuario, string passwordHash)
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
                            return reader["usu_rol"].ToString();

                        return null;
                    }
                }
            }
        }

        public bool RegistrarUsuarioConSalt(
            string id,
            string nombreUsuario,
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

    }
}
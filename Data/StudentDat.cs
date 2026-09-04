using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Data
{
    public class StudentDat
    {
        public bool InsertStudent(string id, string nombre, string apellido)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);
                    cmd.Parameters.AddWithValue("v_nombre", nombre);
                    cmd.Parameters.AddWithValue("v_apellido", apellido);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public ProfileDTO GetProfile(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                // Obtener datos del estudiante (SP existente)
                using (MySqlCommand cmd = new MySqlCommand("proGetEstudianteById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        ProfileDTO perfil = new ProfileDTO
                        {
                            Id = reader["est_id"].ToString(),
                            Nombre = reader["est_nombre"].ToString(),
                            Apellido = reader["est_apellido"].ToString()
                        };

                        reader.Close();

                        // Obtener nombre de usuario
                        using (MySqlCommand cmdUser = new MySqlCommand(
                            "SELECT usu_nombre_usuario FROM tbl_usuarios WHERE usu_id = @id",
                            conn))
                        {
                            cmdUser.Parameters.AddWithValue("@id", id);
                            perfil.Usuario = cmdUser.ExecuteScalar()?.ToString();
                        }

                        return perfil;
                    }
                }
            }
        }

        public bool UpdateStudent(string id, string nombre, string apellido)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdateEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);
                    cmd.Parameters.AddWithValue("v_nombre", nombre);
                    cmd.Parameters.AddWithValue("v_apellido", apellido);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool ExistsStudent(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT COUNT(*) FROM tbl_estudiantes WHERE est_id = @id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
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
        public bool InsertStudent(string id, string nombre, string apellido, string grado, string curso, DateTime? fechaNacimiento)
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
                    cmd.Parameters.AddWithValue("v_grado", grado);
                    cmd.Parameters.AddWithValue("v_curso", curso);
                    cmd.Parameters.AddWithValue(
                        "v_fecha_nacimiento",
                        fechaNacimiento.HasValue
                            ? (object)fechaNacimiento.Value
                            : DBNull.Value
                    );

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public ProfileDTO GetProfile(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetEstudianteById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new ProfileDTO
                        {
                            Id = reader["est_id"].ToString(),
                            Usuario = reader["usu_nombre_usuario"].ToString(),

                            Nombre = reader["est_nombre"].ToString(),
                            Apellido = reader["est_apellido"].ToString(),

                            Grado = reader["est_grado"].ToString(),
                            Curso = reader["est_curso"].ToString(),

                            FechaNacimiento = reader["est_fecha_nacimiento"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["est_fecha_nacimiento"])
                        };
                    }
                }
            }
        }

        public bool UpdateStudent(string id, string nombre, string apellido, string grado, string curso, DateTime? fechaNacimiento)
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
                    cmd.Parameters.AddWithValue("v_grado", grado);
                    cmd.Parameters.AddWithValue("v_curso", curso);
                    cmd.Parameters.AddWithValue(
                        "v_fecha_nacimiento",
                        fechaNacimiento.HasValue
                            ? (object)fechaNacimiento.Value
                            : DBNull.Value
                    );

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
        public List<StudentDTO> ListStudents()
        {
            List<StudentDTO> list = new List<StudentDTO>();

            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proListEstudiantes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new StudentDTO
                            {
                                Id = reader["est_id"].ToString(),
                                Usuario = reader["usu_nombre_usuario"].ToString(),
                                Nombre = reader["est_nombre"].ToString(),
                                Apellido = reader["est_apellido"].ToString(),
                                Estado = reader["est_estado"].ToString(),
                                Grado = reader["est_grado"].ToString(),
                                Curso = reader["est_curso"].ToString(),
                                FechaNacimiento = reader["est_fecha_nacimiento"] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["est_fecha_nacimiento"]),
                            });
                        }
                    }

                }
            }

            return list;
        }
        public StudentDTO GetStudentById(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetEstudianteById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO
                            {
                                Id = reader["est_id"].ToString(),
                                Usuario = reader["usu_nombre_usuario"].ToString(),
                                Nombre = reader["est_nombre"].ToString(),
                                Apellido = reader["est_apellido"].ToString(),
                                Estado = reader["est_estado"].ToString(),
                                Grado = reader["est_grado"].ToString(),
                                Curso = reader["est_curso"].ToString(),
                                FechaNacimiento = reader["est_fecha_nacimiento"] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["est_fecha_nacimiento"])
                            };
                        }
                    }
                }
            }

            return null;
        }
        public bool ChangeStudentStatus(string id, string estado)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proChangeStudentStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_id", id);
                    cmd.Parameters.AddWithValue("v_estado", estado);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool DeleteStudent(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proDeleteEstudiante", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_id", id);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public int CountStudents()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT COUNT(*) FROM tbl_estudiantes";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
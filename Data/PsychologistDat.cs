using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PsychologistDat
    {
        public bool InsertPsychologist(string id, string nombre, string apellido, string correo, string telefono, string formacion,string horario)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertPsicologo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                        
                    cmd.Parameters.Add("v_id", MySqlDbType.VarChar).Value = id;
                    cmd.Parameters.Add("v_nombre", MySqlDbType.VarChar).Value = nombre;
                    cmd.Parameters.Add("v_apellido", MySqlDbType.VarChar).Value = apellido;
                    cmd.Parameters.Add("v_correo", MySqlDbType.VarChar).Value = correo;
                    cmd.Parameters.Add("v_telefono", MySqlDbType.VarChar).Value = telefono;
                    cmd.Parameters.Add("v_formacion", MySqlDbType.VarChar).Value = formacion;
                    cmd.Parameters.Add("v_horario", MySqlDbType.VarChar).Value = horario;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        return false;
                    }
                }
            }
        }
        public PsychologistDTO GetPsychologistById(string id)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetPsicologoById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_id", MySqlDbType.VarChar).Value = id;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PsychologistDTO
                            {
                                Id = reader["psi_id"].ToString(),
                                Nombre = reader["psi_nombre"].ToString(),
                                Apellido = reader["psi_apellido"].ToString(),
                                Correo = reader["psi_correo"].ToString(),
                                Telefono = reader["psi_telefono"].ToString(),
                                Formacion = reader["psi_formacion"].ToString(),
                                Horario = reader["psi_horario"].ToString(),
                                Estado = reader["psi_estado"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<PsychologistDTO> ListPsychologists()
        {
            Persistence db = new Persistence();
            List<PsychologistDTO> lista = new List<PsychologistDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proListPsicologos", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new PsychologistDTO
                                {
                                    Id = reader["psi_id"].ToString(),
                                    Nombre = reader["psi_nombre"].ToString(),
                                    Apellido = reader["psi_apellido"].ToString(),
                                    Correo = reader["psi_correo"].ToString(),
                                    Telefono = reader["psi_telefono"].ToString(),
                                    Formacion = reader["psi_formacion"].ToString(),
                                    Horario = reader["psi_horario"].ToString(),
                                    Estado = reader["psi_estado"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return lista;
        }
        public bool UpdatePsychologist(string id, string nombre, string apellido, string correo, string telefono, string formacion, string horario)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdatePsicologo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_id", MySqlDbType.VarChar).Value = id;
                    cmd.Parameters.Add("v_nombre", MySqlDbType.VarChar).Value = nombre;
                    cmd.Parameters.Add("v_apellido", MySqlDbType.VarChar).Value = apellido;
                    cmd.Parameters.Add("v_correo", MySqlDbType.VarChar).Value = correo;
                    cmd.Parameters.Add("v_telefono", MySqlDbType.VarChar).Value = telefono;
                    cmd.Parameters.Add("v_formacion", MySqlDbType.VarChar).Value = formacion;
                    cmd.Parameters.Add("v_horario", MySqlDbType.VarChar).Value = horario;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        return false;
                    }
                }
            }
        }
        public bool UpdatePsychologistStatus(string id, string estado)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdateEstadoPsicologo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("v_id", MySqlDbType.VarChar).Value = id;
                    cmd.Parameters.Add("v_estado", MySqlDbType.VarChar).Value = estado;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                        return false;
                    }
                }
            }
        }

        public int CountPsychologists()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT COUNT(*) FROM tbl_psicologos";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}

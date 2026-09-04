using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Data
{
    public class ResourcesDat
    {
        // Metodo para agregar un recurso educativo
        public bool saveResource(string _titulo, string _descripcion, string _tipo, string _archivo, string _url, string _psiId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertResource", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Vincula las variables de entrada con los parametros del procedimiento almacenado
                    cmd.Parameters.Add("p_titulo", MySqlDbType.VarChar).Value = _titulo;
                    cmd.Parameters.Add("p_descripcion", MySqlDbType.Text).Value = _descripcion;
                    cmd.Parameters.Add("p_tipo", MySqlDbType.VarChar).Value = _tipo;
                    cmd.Parameters.Add("p_archivo", MySqlDbType.VarChar).Value = _archivo;
                    cmd.Parameters.Add("p_url", MySqlDbType.VarChar).Value = _url;
                    cmd.Parameters.Add("p_psi_id", MySqlDbType.VarChar).Value = _psiId;
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message); // error exacto de MySQL
                        return false;
                    }
                }
            } // cierra la conexión automaticamente
        }

        // Método para mostrar los recursos educativos 
        public List<ResourcesDTO> ShowResources()
        {
            Persistence db = new Persistence();
            List<ResourcesDTO> lista = new List<ResourcesDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proSelectResource", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new ResourcesDTO
                                {
                                    Id = Convert.ToInt32(reader["rec_id"]),
                                    Titulo = reader["rec_titulo"].ToString(),
                                    Descripcion = reader["rec_descripcion"].ToString(),
                                    Tipo = reader["rec_tipo"].ToString(),
                                    Archivo = reader["rec_archivo"].ToString(),
                                    Url = reader["rec_url"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["rec_fecha"]),
                                    IdPsicologo = reader["rec_psi_id"] == DBNull.Value
                                        ? null
                                        : reader["rec_psi_id"].ToString(),

                                    NombrePsicologo = reader["psi_nombre"] == DBNull.Value
                                        ? null
                                        : reader["psi_nombre"].ToString(),

                                    ApellidoPsicologo = reader["psi_apellido"] == DBNull.Value
                                        ? null
                                        : reader["psi_apellido"].ToString()

                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener recursos: " + e.Message);
            }
            return lista;
        }

        //Método para actualizar los recursos educativos
        public bool updateResource(int _id, string _titulo, string _descripcion, string _tipo, string _archivo, string _url)
        {
            Persistence db = new Persistence();
            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdateResource", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Vincula las variables de entrada con los parametros del procedimiento almacenado
                    cmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;
                    cmd.Parameters.Add("p_titulo", MySqlDbType.VarChar).Value = _titulo;
                    cmd.Parameters.Add("p_descripcion", MySqlDbType.Text).Value = _descripcion;
                    cmd.Parameters.Add("p_tipo", MySqlDbType.VarChar).Value = _tipo;
                    cmd.Parameters.Add("p_archivo", MySqlDbType.Text).Value = _archivo;
                    cmd.Parameters.Add("p_url", MySqlDbType.Text).Value = _url;
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message); // error exacto de MySQL
                        return false;
                    }
                }

            }

        }

        //Metodo para eliminar un recurso educativo
        public bool deleteResource(int _id)
        {
            Persistence db = new Persistence();
            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proDeleteResource", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Solo vinculamos el ID
                    cmd.Parameters.Add("p_id", MySqlDbType.Int32).Value = _id;

                    try
                    {
                        // Retorna true si se eliminó al menos una fila
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al eliminar: " + e.Message);
                        return false;
                    }
                }
            }
        }

        public int CountResources()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT COUNT(*) FROM tbl_recursos";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
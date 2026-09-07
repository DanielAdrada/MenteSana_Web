using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Data
{
    public class StrategiesDat
    {
        // Agrega una estrategia
        public int SaveEstrategia(string _dimension, string _area, string _nivel, string _titulo, string _descripcion, string _usuId)
        {
            Persistence db = new Persistence();
            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertEstrategia", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_dimension", MySqlDbType.VarChar).Value = _dimension;
                    cmd.Parameters.Add("p_area", MySqlDbType.VarChar).Value = _area;
                    cmd.Parameters.Add("p_nivel", MySqlDbType.VarChar).Value = _nivel;
                    cmd.Parameters.Add("p_titulo", MySqlDbType.VarChar).Value = _titulo;
                    cmd.Parameters.Add("p_descripcion", MySqlDbType.Text).Value = _descripcion;
                    cmd.Parameters.Add("p_usu_id", MySqlDbType.VarChar).Value = _usuId;

                    try
                    {
                        object resultado = cmd.ExecuteScalar();
                        return Convert.ToInt32(resultado);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al guardar estrategia: " + e.Message);
                        return 0;
                    }
                }
            }
        }


        // Obtiene todas las estrategias
        public List<Dictionary<string, object>> GetEstrategias()
        {
            Persistence db = new Persistence();
            List<Dictionary<string, object>> estrategias =
                new List<Dictionary<string, object>>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetEstrategias", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> estrategia =
                                    new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    estrategia[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }

                                estrategias.Add(estrategia);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener estrategias: " + e.Message);
                    }
                }
            }

            return estrategias;
        }


        // Actualiza una estrategia
        public bool UpdateEstrategia( int _estrategiaId, string _dimension, string _area, string _nivel, string _titulo, string _descripcion)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proUpdateEstrategia", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_estrategia_id", MySqlDbType.Int32).Value = _estrategiaId;
                    cmd.Parameters.Add("p_dimension", MySqlDbType.VarChar).Value = _dimension;
                    cmd.Parameters.Add("p_area", MySqlDbType.VarChar).Value = _area;
                    cmd.Parameters.Add("p_nivel", MySqlDbType.VarChar).Value = _nivel;
                    cmd.Parameters.Add("p_titulo", MySqlDbType.VarChar).Value = _titulo;
                    cmd.Parameters.Add("p_descripcion", MySqlDbType.Text).Value = _descripcion;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al actualizar estrategia: " + e.Message);
                        return false;
                    }
                }
            }
        }


        // Activa o desactiva una estrategia
        public bool CambiarEstadoEstrategia(int _estrategiaId, int _activa)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proEstadoEstrategia", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_estrategia_id", MySqlDbType.Int32).Value =
                        _estrategiaId;

                    cmd.Parameters.Add("p_activa", MySqlDbType.Int32).Value =
                        _activa;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al cambiar estado de estrategia: " + e.Message);
                        return false;
                    }
                }
            }
        }


        // Guarda una estrategia asignada a un test
        public bool SaveTestEstrategia(int _testId, int _estrategiaId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proInsertTestEstrategia", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_test_id", MySqlDbType.Int32).Value =
                        _testId;

                    cmd.Parameters.Add("p_estrategia_id", MySqlDbType.Int32).Value =
                        _estrategiaId;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al guardar estrategia del test: " + e.Message);
                        return false;
                    }
                }
            }
        }


        // Obtiene las estrategias asignadas a un test
        public List<Dictionary<string, object>> GetTestEstrategias(
            int _testId)
        {
            Persistence db = new Persistence();
            List<Dictionary<string, object>> estrategias =
                new List<Dictionary<string, object>>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proGetTestEstrategias", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_test_id", MySqlDbType.Int32).Value =
                        _testId;

                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> estrategia =
                                    new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    estrategia[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }

                                estrategias.Add(estrategia);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener estrategias del test: " + e.Message);
                    }
                }
            }

            return estrategias;
        }


        // Obtiene el historial de estrategias de un estudiante
        public List<Dictionary<string, object>> GetHistorialEstrategias(
            string _estudianteId)
        {
            Persistence db = new Persistence();
            List<Dictionary<string, object>> historial =
                new List<Dictionary<string, object>>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proGetHistorialEstrategias", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_est_id", MySqlDbType.VarChar).Value =
                        _estudianteId;

                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> estrategia =
                                    new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    estrategia[reader.GetName(i)] =
                                        reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }

                                historial.Add(estrategia);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener historial de estrategias: " + e.Message);
                    }
                }
            }
            return historial;
        }


        // Obtiene las estrategias activas que corresponden al resultado del DASS-42
        public List<Dictionary<string, object>> GetEstrategiasPorResultado(
            string _dimension,
            string _area,
            string _nivel)
        {
            Persistence db = new Persistence();

            List<Dictionary<string, object>> estrategias =
                new List<Dictionary<string, object>>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proGetEstrategiasRecomendadas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_dimension", MySqlDbType.VarChar).Value =
                        _dimension;

                    cmd.Parameters.Add("p_area", MySqlDbType.VarChar).Value =
                        _area;

                    cmd.Parameters.Add("p_nivel", MySqlDbType.VarChar).Value =
                        _nivel;

                    try
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> estrategia =
                                    new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    estrategia[reader.GetName(i)] =
                                        reader.IsDBNull(i)
                                            ? null
                                            : reader.GetValue(i);
                                }

                                estrategias.Add(estrategia);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(
                            "Error al obtener estrategias por resultado: "
                            + e.Message);
                    }
                }
            }

            return estrategias;
        }

    }
}
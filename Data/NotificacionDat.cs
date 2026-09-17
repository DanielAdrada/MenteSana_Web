using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Data
{
    public class NotificacionDat
    {
        public List<NotificacionDTO> ObtenerNotificaciones()
        {
            Persistence db = new Persistence();
            List<NotificacionDTO> lista = new List<NotificacionDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    string sql = @"
                        SELECT
                            notificacion_id,
                            notificacion_test_id,
                            notificacion_est_id,
                            notificacion_tipo,
                            notificacion_mensaje,
                            notificacion_fecha,
                            notificacion_leida,
                            notificacion_activa,
                            notificacion_estado
                        FROM tbl_notificaciones
                        WHERE notificacion_activa = 1
                          AND notificacion_estado = 'Pendiente'
                        ORDER BY notificacion_fecha DESC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new NotificacionDTO
                                {
                                    Id = Convert.ToInt32(reader["notificacion_id"]),
                                    TestId = Convert.ToInt32(reader["notificacion_test_id"]),
                                    EstudianteId = reader["notificacion_est_id"].ToString(),
                                    Tipo = reader["notificacion_tipo"].ToString(),
                                    Mensaje = reader["notificacion_mensaje"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["notificacion_fecha"]),
                                    Leida = Convert.ToBoolean(reader["notificacion_leida"]),
                                    Activa = Convert.ToBoolean(reader["notificacion_activa"]),
                                    Estado = reader["notificacion_estado"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener notificaciones: " + e.Message);
            }

            return lista;
        }


        public int ContarNotificacionesPendientes()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM tbl_notificaciones
                    WHERE notificacion_activa = 1
                      AND notificacion_estado = 'Pendiente'";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        public bool MarcarComoAtendida(int notificacionId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "proMarcarNotificacionAtendida", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "p_notificacion_id",
                        notificacionId
                    );

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
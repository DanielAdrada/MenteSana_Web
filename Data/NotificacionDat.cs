using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
                            notificacion_activa
                        FROM tbl_notificaciones
                        WHERE notificacion_activa = 1
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
                                    Activa = Convert.ToBoolean(reader["notificacion_activa"])
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
    }
}
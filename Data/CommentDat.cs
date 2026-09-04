using Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Data
{
    public class CommentDat
    {
        // ================= COMENTARIOS =================

        public bool InsertComentario(string usuId, string contenido, bool positivo)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertComentario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_usu_id", usuId);
                    cmd.Parameters.AddWithValue("v_contenido", contenido);
                    cmd.Parameters.AddWithValue("v_positivo", positivo);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<CommentDTO> GetComentarios()
        {
            Persistence db = new Persistence();
            List<CommentDTO> lista = new List<CommentDTO>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetComentarios", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CommentDTO
                            {
                                Id = Convert.ToInt32(reader["com_id"]),
                                Usuario = reader["usu_nombre_usuario"].ToString(),

                                Nombre = reader["est_nombre"].ToString(),
                                Apellido = reader["est_apellido"].ToString(),

                                FotoRuta = reader["per_foto_ruta"] == DBNull.Value
                                    ? null
                                    : reader["per_foto_ruta"].ToString(),

                                Contenido = reader["com_contenido"].ToString(),
                                Fecha = Convert.ToDateTime(reader["com_fecha"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public List<CommentDTO> GetComentariosPositivos()
        {
            Persistence db = new Persistence();
            List<CommentDTO> lista = new List<CommentDTO>();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proGetComentariosPositivos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CommentDTO
                            {
                                Usuario = reader["usu_nombre_usuario"].ToString(),
                                Contenido = reader["com_contenido"].ToString(),
                                Fecha = Convert.ToDateTime(reader["com_fecha"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public bool DesactivarComentario(int comentarioId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proDesactivarComentario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_com_id", comentarioId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
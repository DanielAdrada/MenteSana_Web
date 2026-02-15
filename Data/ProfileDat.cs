using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Data
{
    public class ProfileDat
    {
        public bool InsertProfile(string perId, string usuId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand("proInsertPerfil", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("v_per_id", perId);
                cmd.Parameters.AddWithValue("v_usu_id", usuId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public string GetProfilePhoto(string usuId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand("proGetPerfilByUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("v_usu_id", usuId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader["per_foto_ruta"]?.ToString();

                    return null;
                }
            }
        }

        public bool UpdateProfilePhoto(string usuId, string rutaFoto)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            using (MySqlCommand cmd = new MySqlCommand("proUpdateFotoPerfil", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("v_usu_id", usuId);
                cmd.Parameters.AddWithValue("v_foto_ruta", rutaFoto);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool ExistsProfile(string usuId)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                string sql = "SELECT COUNT(*) FROM tbl_perfil WHERE usu_id = @usuId";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuId", usuId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Web;

namespace Data
{
    public class DassDat
    {

        // Guarda el test DASS
        public int SaveTest(string _estudianteId, string _nivelDepresion, string _nivelAnsiedad, string _nivelEstres)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertTestDASS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Vincula las variables de entrada con los parametros del procedimiento almacenado
                    cmd.Parameters.Add("p_est_id", MySqlDbType.VarChar).Value = _estudianteId;
                    cmd.Parameters.Add("p_nivel_depresion", MySqlDbType.Text).Value = _nivelDepresion;
                    cmd.Parameters.Add("p_nivel_ansiedad", MySqlDbType.VarChar).Value = _nivelAnsiedad;
                    cmd.Parameters.Add("p_nivel_estres", MySqlDbType.VarChar).Value = _nivelEstres;
             
                    try
                    {
                        object resultado = cmd.ExecuteScalar();
                        return Convert.ToInt32(resultado);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message); // error exacto de MySQL
                        return 0;
                    }
                }
            } 
        }

        // Guarda una respuesta del test
        public bool SaveAnswer(int _testId, int _numeroPregunta, int _valorRespuesta)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand("proInsertRespuestaDASS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Vincula las variables de entrada con los parametros del procedimiento almacenado
                    cmd.Parameters.Add("p_test_id", MySqlDbType.Int32).Value = _testId;
                    cmd.Parameters.Add("p_numero_pregunta", MySqlDbType.Text).Value = _numeroPregunta;
                    cmd.Parameters.Add("p_valor_respuesta", MySqlDbType.VarChar).Value = _valorRespuesta;

                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al guardar respuesta DASS: " + e.Message); // error exacto de MySQL
                        return false;
                    }
                }
            }
        }
    }
}
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

        public int GetTotalStudentsEvaluated()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    @"SELECT COUNT(DISTINCT test_est_id)
              FROM tbl_tests_dass", conn))
                {
                    try
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener estudiantes evaluados: " + e.Message);
                        return 0;
                    }
                }
            }
        }


        public int GetTotalTests()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    @"SELECT COUNT(test_id)
              FROM tbl_tests_dass", conn))
                {
                    try
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener total de evaluaciones: " + e.Message);
                        return 0;
                    }
                }
            }
        }


        public DateTime? GetLastTestDate()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    @"SELECT MAX(test_fecha)
              FROM tbl_tests_dass", conn))
                {
                    try
                    {
                        object resultado = cmd.ExecuteScalar();

                        if (resultado == null || resultado == DBNull.Value)
                            return null;

                        return Convert.ToDateTime(resultado);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener última evaluación: " + e.Message);
                        return null;
                    }
                }
            }
        }

        public int GetStudentsFollowUp()
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    @"SELECT COUNT(DISTINCT test_est_id)
                    FROM tbl_tests_dass
                 WHERE test_nivel_depresion IN ('Moderado', 'Severo', 'Extremadamente_Severo')
                 OR test_nivel_ansiedad IN ('Moderado', 'Severo', 'Extremadamente_Severo')
                 OR test_nivel_estres IN ('Moderado', 'Severo', 'Extremadamente_Severo')",
                    conn))
                {
                    try
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error al obtener estudiantes en seguimiento: " + e.Message);
                        return 0;
                    }
                }
            }
        }

    }
}
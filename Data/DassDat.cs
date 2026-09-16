using Data.Models;
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

                        int testId = Convert.ToInt32(resultado);

                        CrearNotificacionDASS(
                            testId,
                            _estudianteId,
                            _nivelDepresion,
                            _nivelAnsiedad,
                            _nivelEstres
                        );

                        return testId;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message); // error exacto de MySQL
                        return 0;
                    }
                }
            } 
        }

        // Crea una notificación cuando el resultado DASS requiere seguimiento
        public bool CrearNotificacionDASS(
            int _testId,
            string _estudianteId,
            string _nivelDepresion,
            string _nivelAnsiedad,
            string _nivelEstres)
        {
            Persistence db = new Persistence();

            using (MySqlConnection conn = db.OpenConnection())
            {
                using (MySqlCommand cmd =
                    new MySqlCommand("proInsertNotificacionDASS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_test_id", MySqlDbType.Int32)
                        .Value = _testId;

                    cmd.Parameters.Add("p_est_id", MySqlDbType.VarChar)
                        .Value = _estudianteId;

                    cmd.Parameters.Add("p_nivel_depresion", MySqlDbType.VarChar)
                        .Value = _nivelDepresion;

                    cmd.Parameters.Add("p_nivel_ansiedad", MySqlDbType.VarChar)
                        .Value = _nivelAnsiedad;

                    cmd.Parameters.Add("p_nivel_estres", MySqlDbType.VarChar)
                        .Value = _nivelEstres;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(
                            "Error al crear notificación DASS: " + e.Message);

                        return false;
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
        public List<DassTestDTO> ListLatestTests()
        {
            Persistence db = new Persistence();
            List<DassTestDTO> lista = new List<DassTestDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proListUltimosTestsDASS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new DassTestDTO
                                {
                                    TestId = Convert.ToInt32(reader["test_id"]),
                                    EstudianteId = reader["test_est_id"].ToString(),
                                    Estudiante = reader["estudiante"].ToString(),
                                    GradoCurso = reader["grado_curso"].ToString(),
                                    NivelDepresion = reader["test_nivel_depresion"].ToString(),
                                    NivelAnsiedad = reader["test_nivel_ansiedad"].ToString(),
                                    NivelEstres = reader["test_nivel_estres"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["test_fecha"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al listar últimos tests DASS: " + e.Message);
            }

            return lista;
        }

        public List<DassTestDTO> GetTestHistory(string estudianteId)
        {
            Persistence db = new Persistence();
            List<DassTestDTO> lista = new List<DassTestDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proGetHistorialTestsDASS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_est_id", MySqlDbType.VarChar).Value = estudianteId;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new DassTestDTO
                                {
                                    TestId = Convert.ToInt32(reader["test_id"]),
                                    EstudianteId = reader["test_est_id"].ToString(),
                                    Estudiante = reader["estudiante"].ToString(),
                                    GradoCurso = reader["grado_curso"].ToString(),
                                    NivelDepresion = reader["test_nivel_depresion"].ToString(),
                                    NivelAnsiedad = reader["test_nivel_ansiedad"].ToString(),
                                    NivelEstres = reader["test_nivel_estres"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["test_fecha"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener historial DASS: " + e.Message);
            }

            return lista;
        }

        public List<DassAnswerDTO> GetAnswers(int testId)
        {
            Persistence db = new Persistence();
            List<DassAnswerDTO> lista = new List<DassAnswerDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proGetRespuestasDASS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_test_id", MySqlDbType.Int32).Value = testId;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new DassAnswerDTO
                                {
                                    Pregunta = Convert.ToInt32(reader["respuesta_pregunta"]),
                                    Valor = Convert.ToInt32(reader["respuesta_valor"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener respuestas DASS: " + e.Message);
            }

            return lista;
        }

        public DassDashboardSummaryDTO GetDashboardSummary()
        {
            Persistence db = new Persistence();
            DassDashboardSummaryDTO resumen = new DassDashboardSummaryDTO();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proGetResumenDashboardDASS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                resumen.TotalEstudiantesEvaluados =
                                    Convert.ToInt32(reader["total_estudiantes_evaluados"]);

                                resumen.TotalTestsRealizados =
                                    Convert.ToInt32(reader["total_tests_realizados"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener resumen del dashboard DASS: " + e.Message);
            }

            return resumen;
        }

        public List<DassDashboardStatisticDTO> GetDashboardStatistics()
        {
            Persistence db = new Persistence();
            List<DassDashboardStatisticDTO> lista = new List<DassDashboardStatisticDTO>();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proGetEstadisticasDashboardDASS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(new DassDashboardStatisticDTO
                                {
                                    Dimension = reader["dimension"].ToString(),
                                    Nivel = reader["nivel"].ToString(),
                                    Cantidad = Convert.ToInt32(reader["cantidad"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener estadísticas del dashboard DASS: " + e.Message);
            }

            return lista;
        }

        public DassTestDTO GetTestById(int testId)
        {
            Persistence db = new Persistence();

            try
            {
                using (MySqlConnection conn = db.OpenConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand("proGetTestDASSById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_test_id", MySqlDbType.Int32).Value = testId;

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DassTestDTO
                                {
                                    TestId = Convert.ToInt32(reader["test_id"]),
                                    EstudianteId = reader["test_est_id"].ToString(),
                                    Estudiante = reader["estudiante"].ToString(),
                                    GradoCurso = reader["grado_curso"].ToString(),
                                    NivelDepresion = reader["test_nivel_depresion"].ToString(),
                                    NivelAnsiedad = reader["test_nivel_ansiedad"].ToString(),
                                    NivelEstres = reader["test_nivel_estres"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["test_fecha"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al obtener test DASS: " + e.Message);
            }

            return null;
        }
    }
}
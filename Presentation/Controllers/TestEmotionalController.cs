using Logic;
using Logic.Models;
using Logic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Presentation.Controllers
{
    public class TestEmotionalController : Controller
    {
        private readonly EmotionService _emotionService = new EmotionService();
        private readonly DassLogic _dassLog = new DassLogic();
        private readonly EstrategiaService _estrategiaService = new EstrategiaService();

        // =========================================================
        // GET: SURVEY
        // =========================================================
        [HttpGet]
        public ActionResult Survey(int bloque = 1)
        {
            // Verificar que exista una sesión de usuario
            string estudianteId = Session["UserId"]?.ToString();

            if (string.IsNullOrWhiteSpace(estudianteId))
            {
                TempData["Error"] =
                    "Tu sesión ha finalizado. Inicia sesión nuevamente para continuar.";

                return RedirectToAction("Index", "Login");
            }

            // Validar bloque
            if (bloque < 1 || bloque > 3)
            {
                bloque = 1;
            }

            ViewBag.Error = TempData["Error"];

            string[] preguntas =
            {
                "Me di cuenta de que me molestaba por cosas bastante insignificantes",
                "Noté sequedad en mi boca",
                "Sentía que no podía experimentar ningún sentimiento positivo",
                "Experimenté dificultad para respirar (por ejemplo, respiración excesivamente rápida o falta de aire sin esfuerzo físico)",
                "Simplemente no lograba ponerme en marcha",
                "Tendía a reaccionar exageradamente ante las situaciones",
                "Tuve sensación de temblor o inestabilidad (por ejemplo, sentir que las piernas iban a fallarme)",
                "Me resultaba difícil relajarme",
                "Me encontré en situaciones que me generaban tanta ansiedad que sentía gran alivio cuando terminaban",
                "Sentía que no tenía nada que esperar con ilusión",
                "Me molestaba con facilidad",
                "Sentía que utilizaba mucha energía nerviosa",
                "Me sentía triste y deprimido/a",
                "Me impacientaba cuando sufría retrasos de cualquier tipo (ejemplo: ascensores, semáforos, esperar demasiado)",
                "Tuve sensación de desmayo",
                "Sentía que había perdido interés en casi todo",
                "Sentía que no valía mucho como persona",
                "Sentía que estaba muy susceptible",
                "Sudaba notablemente (por ejemplo, manos sudorosas) sin calor elevado ni esfuerzo físico",
                "Sentía miedo sin una buena razón",
                "Sentía que la vida no valía la pena",
                "Me resultaba difícil tranquilizarme o desconectarme",
                "Tenía dificultad para tragar",
                "No lograba disfrutar de las cosas que hacía",
                "Era consciente del latido de mi corazón sin realizar esfuerzo físico (ejemplo: aumento del ritmo cardíaco, sensación de latidos irregulares)",
                "Me sentía desanimado/a y triste",
                "Descubrí que estaba muy irritable",
                "Sentía que estaba cerca del pánico",
                "Me costaba calmarme después de que algo me alteraba",
                "Temía que alguna tarea insignificante pero poco familiar me descontrolara",
                "Era incapaz de entusiasmarme con nada",
                "Me resultaba difícil tolerar interrupciones mientras hacía algo",
                "Estaba en un estado de tensión nerviosa",
                "Sentía que no valía prácticamente nada",
                "Era intolerante con cualquier cosa que me impidiera continuar con lo que estaba haciendo",
                "Me sentía aterrorizado/a",
                "No veía nada esperanzador en el futuro",
                "Sentía que la vida no tenía sentido",
                "Me encontraba agitado/a",
                "Me preocupaba las situaciones en las que pudiera entrar en pánico y hacer el ridículo",
                "Experimenté temblores (por ejemplo, en las manos)",
                "Me resultaba difícil tomar la iniciativa para hacer las cosas"
            };

            int preguntasPorBloque = 14;
            int inicio = (bloque - 1) * preguntasPorBloque;

            var modelo = new Presentation.Models.TestDassViewModel
            {
                Preguntas = preguntas
                    .Skip(inicio)
                    .Take(preguntasPorBloque)
                    .ToList(),

                Bloque = bloque,
                InicioPregunta = inicio,
                UltimoBloque = bloque == 3,
                Progreso = (int)((bloque / 3.0) * 100)
            };

            return View(modelo);
        }


        // =========================================================
        // POST: ANALIZAR
        // =========================================================
        [HttpPost]
        public async Task<ActionResult> Analizar(int bloque)
        {
            // -----------------------------------------------------
            // 1. VALIDAR SESIÓN
            // -----------------------------------------------------
            string estudianteId = Session["UserId"]?.ToString();

            if (string.IsNullOrWhiteSpace(estudianteId))
            {
                TempData["Error"] =
                    "Tu sesión ha finalizado. Inicia sesión nuevamente para realizar la evaluación.";

                return RedirectToAction("Index", "Login");
            }


            // -----------------------------------------------------
            // 2. VALIDAR BLOQUE
            // -----------------------------------------------------
            if (bloque < 1 || bloque > 3)
            {
                TempData["Error"] =
                    "El bloque de preguntas no es válido.";

                return RedirectToAction("Survey", new { bloque = 1 });
            }


            // -----------------------------------------------------
            // 3. RECUPERAR RESPUESTAS
            // -----------------------------------------------------
            List<int> respuestas =
                Session["RespuestasDASS"] as List<int>;

            if (respuestas == null)
            {
                respuestas = new List<int>();
            }


            // -----------------------------------------------------
            // 4. LEER LAS 14 RESPUESTAS DEL BLOQUE
            // -----------------------------------------------------
            int inicio = (bloque - 1) * 14 + 1;

            for (int i = inicio; i < inicio + 14; i++)
            {
                string valor = Request.Form["p" + i];

                // Pregunta sin responder
                if (string.IsNullOrWhiteSpace(valor))
                {
                    TempData["Error"] =
                        "Debes responder todas las preguntas antes de continuar.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = bloque }
                    );
                }

                // Validar que sea un número
                int respuesta;

                if (!int.TryParse(valor, out respuesta))
                {
                    TempData["Error"] =
                        "Se encontró una respuesta no válida. Intenta nuevamente.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = bloque }
                    );
                }

                // Validar rango DASS
                if (respuesta < 0 || respuesta > 3)
                {
                    TempData["Error"] =
                        "Una de las respuestas no es válida. Intenta nuevamente.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = bloque }
                    );
                }

                respuestas.Add(respuesta);
            }


            // -----------------------------------------------------
            // 5. GUARDAR RESPUESTAS EN SESSION
            // -----------------------------------------------------
            Session["RespuestasDASS"] = respuestas;


            // -----------------------------------------------------
            // 6. SI NO ES EL ÚLTIMO BLOQUE
            // -----------------------------------------------------
            if (bloque < 3)
            {
                return RedirectToAction(
                    "Survey",
                    new { bloque = bloque + 1 }
                );
            }


            // -----------------------------------------------------
            // 7. VERIFICAR LAS 42 RESPUESTAS
            // -----------------------------------------------------
            if (respuestas.Count != 42)
            {
                TempData["Error"] =
                    "No fue posible completar correctamente las 42 preguntas. Intenta nuevamente.";

                Session.Remove("RespuestasDASS");

                return RedirectToAction(
                    "Survey",
                    new { bloque = 1 }
                );
            }


            // -----------------------------------------------------
            // 8. PROCESAR EL TEST
            // -----------------------------------------------------
            try
            {
                EmotionResult resultado =
                    await _emotionService.DetectarEmocionAsync(respuestas);


                // -------------------------------------------------
                // 9. VALIDAR RESULTADO DE LA API
                // -------------------------------------------------
                if (resultado == null)
                {
                    TempData["Error"] =
                        "No fue posible procesar la evaluación. Intenta nuevamente.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = 3 }
                    );
                }


                // -------------------------------------------------
                // 10. OBTENER ESTRATEGIAS PERSONALIZADAS
                // -------------------------------------------------
                resultado.estrategias =
                    _estrategiaService.ObtenerEstrategias(resultado);


                // -------------------------------------------------
                // 11. GUARDAR EL TEST
                // -------------------------------------------------
                int testId = _dassLog.GuardarTest(
                    estudianteId,
                    resultado.depresion,
                    resultado.ansiedad,
                    resultado.estres
                );


                if (testId <= 0)
                {
                    TempData["Error"] =
                        "No fue posible guardar el resultado de la evaluación.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = 3 }
                    );
                }


                // -------------------------------------------------
                // 12. GUARDAR ESTRATEGIAS
                // -------------------------------------------------
                if (resultado.estrategias != null)
                {
                    foreach (var estrategia in resultado.estrategias)
                    {
                        if (estrategia != null &&
                            estrategia.EstrategiaId > 0)
                        {
                            _estrategiaService.GuardarTestEstrategia(
                                testId,
                                estrategia.EstrategiaId
                            );
                        }
                    }
                }


                // -------------------------------------------------
                // 13. GUARDAR LAS 42 RESPUESTAS
                // -------------------------------------------------
                bool respuestasGuardadas =
                    _dassLog.GuardarRespuestas(
                        testId,
                        respuestas
                    );


                if (!respuestasGuardadas)
                {
                    TempData["Error"] =
                        "La evaluación fue procesada, pero ocurrió un problema al guardar las respuestas.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = 3 }
                    );
                }


                // -------------------------------------------------
                // 14. PREPARAR RESULTADO
                // -------------------------------------------------
                TempData["Resultado"] =
                    JsonConvert.SerializeObject(resultado);


                // -------------------------------------------------
                // 15. LIMPIAR RESPUESTAS
                // -------------------------------------------------
                Session.Remove("RespuestasDASS");


                // -------------------------------------------------
                // 16. MOSTRAR RESULTADO
                // -------------------------------------------------
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                // Registrar el error solamente para desarrollo
                System.Diagnostics.Debug.WriteLine(
                    "ERROR AL PROCESAR DASS: " + ex.ToString()
                );

                // Mensaje amigable para el estudiante
                TempData["Error"] =
                    "No fue posible procesar tu evaluación en este momento. " +
                    "Por favor intenta nuevamente.";

                return RedirectToAction(
                    "Survey",
                    new { bloque = 3 }
                );
            }
        }


        // =========================================================
        // GET: RESULT
        // =========================================================
        [HttpGet]
        public ActionResult Result()
        {
            if (TempData["Resultado"] == null)
            {
                return RedirectToAction(
                    "Survey",
                    new { bloque = 1 }
                );
            }

            try
            {
                EmotionResult resultado =
                    JsonConvert.DeserializeObject<EmotionResult>(
                        TempData["Resultado"].ToString()
                    );

                if (resultado == null)
                {
                    TempData["Error"] =
                        "No fue posible cargar el resultado de la evaluación.";

                    return RedirectToAction(
                        "Survey",
                        new { bloque = 1 }
                    );
                }

                return View(resultado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "ERROR AL CARGAR RESULTADO DASS: " + ex.ToString()
                );

                TempData["Error"] =
                    "No fue posible cargar el resultado de la evaluación.";

                return RedirectToAction(
                    "Survey",
                    new { bloque = 1 }
                );
            }
        }
    }
}
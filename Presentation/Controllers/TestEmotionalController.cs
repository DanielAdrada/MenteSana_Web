    using Logic.Models;
    using Logic.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Text;
    using Newtonsoft.Json;

    namespace Presentation.Controllers
    {
        public class TestEmotionalController : Controller
        {
            private readonly EmotionService _emotionService = new EmotionService();

            // ====== GET: SURVEY ======
            [HttpGet]
            public ActionResult Survey(int bloque = 1)
            {
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

            // ====== POST: ANALIZAR ======
            [HttpPost]
            public async Task<ActionResult> Analizar(int bloque)
            {
                List<int> respuestas = Session["RespuestasDASS"] as List<int>;
                
                if (respuestas == null)
                {
                    respuestas = new List<int>();
                }

                int inicio = (bloque - 1) * 14 + 1;

                for (int i = inicio; i < inicio + 14; i++)
                {
                    string valor = Request.Form["p" + i];

                    if (string.IsNullOrEmpty(valor))
                    {
                        TempData["Error"] = "Debes responder todas las preguntas del bloque actual.";
                        return RedirectToAction("Survey");
                    }

                    respuestas.Add(int.Parse(valor));
                }


                // Guardar respuestas acumuladas
                Session["RespuestasDASS"] = respuestas;
                
                if (bloque < 3)
                {
                    return RedirectToAction("Survey",
                        new { bloque = bloque + 1 });
                }

                
                 if (respuestas.Count != 42)
                 {
                     TempData["Error"] = "No se completaron las 42 preguntas.";
                     return RedirectToAction("Survey");
                 }

                 try
                 {
                     EmotionResult resultado =
                         await _emotionService.DetectarEmocionAsync(respuestas);

                     TempData["Resultado"] =
                         JsonConvert.SerializeObject(resultado);
               
                     Session.Remove("RespuestasDASS");
                     return RedirectToAction("Result");
                 }
                 catch (Exception ex)
                 {
                       TempData["Error"] = ex.Message;
                       return RedirectToAction("Survey");
                 }

            }

            // ====== GET: RESULT ======
            [HttpGet]
            public ActionResult Result()
            {
                if (TempData["Resultado"] == null)
                {
                    return RedirectToAction("Survey");
                }

                EmotionResult resultado =
                    JsonConvert.DeserializeObject<EmotionResult>(
                        TempData["Resultado"].ToString());

                return View(resultado);
            }
        }
    }
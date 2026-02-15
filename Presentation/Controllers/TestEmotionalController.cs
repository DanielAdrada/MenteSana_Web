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
            public ActionResult Survey()
            {
                ViewBag.Error = TempData["Error"];
                return View();
            }

            // ====== POST: ANALIZAR ======
            [HttpPost]
            public async Task<ActionResult> Analizar()
            {
                List<int> respuestas = new List<int>();

                for (int i = 1; i <= 21; i++)
                {
                    string valor = Request.Form["p" + i];

                    if (string.IsNullOrEmpty(valor))
                    {
                        TempData["Error"] = "Debes responder todas las preguntas.";
                        return RedirectToAction("Survey");
                    }

                    respuestas.Add(int.Parse(valor));
                }

                try
                {
                    EmotionResult resultado =
                        await _emotionService.DetectarEmocionAsync(respuestas);

                    if (resultado == null)
                    {
                        TempData["Error"] = "No se pudo interpretar el resultado emocional.";
                        return RedirectToAction("Survey");
                    }

                    // GUARDAR RESULTADO
                    TempData["Resultado"] =
                        JsonConvert.SerializeObject(resultado);

                    // REDIRECCIÓN CORRECTA
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

                var resultado =
                    JsonConvert.DeserializeObject<EmotionResult>(
                        TempData["Resultado"].ToString()
                    );

                return View(resultado);
            }
        }
    }
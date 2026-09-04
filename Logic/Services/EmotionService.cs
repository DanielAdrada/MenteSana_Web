using Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logic.Services
{
    public class EmotionService
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:5000/")
        };

        public async Task<EmotionResult> DetectarEmocionAsync(List<int> respuestas)
        {
            var payload = new
            {
                respuestas = respuestas
            };

            string json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await client.PostAsync("predecir_emocion", content);

            string resultJson = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine("JSON RECIBIDO DESDE PYTHON:");
            System.Diagnostics.Debug.WriteLine(resultJson);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Error API ({response.StatusCode}): {resultJson}"
                );
            }

            if (string.IsNullOrWhiteSpace(resultJson))
            {
                throw new Exception("La API devolvió una respuesta vacía.");
            }

            dynamic raw = JsonConvert.DeserializeObject(resultJson);

            var result = new EmotionResult
            {
                emocion_principal = raw.emocion_principal,
                emocion_secundaria = raw.emocion_secundaria,
                scores = new Dictionary<string, double>()
            };

            foreach (var item in raw.scores)
            {
                string nombre = item[0];
                double valor = Convert.ToDouble(item[1]);
                result.scores.Add(nombre, valor);
            }

            if (result == null)
            {
                throw new Exception("No se pudo deserializar EmotionResult.");
            }

            return result;
        }
    }
}
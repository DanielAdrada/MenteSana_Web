using Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine("=================================");
            Debug.WriteLine("ENVIANDO DASS-42 A FLASK");
            Debug.WriteLine("Cantidad de respuestas: " + respuestas.Count);
            Debug.WriteLine("JSON enviado: " + json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await client.PostAsync("predecir_dass42", content);

            string resultJson = await response.Content.ReadAsStringAsync();

            Debug.WriteLine("=================================");
            Debug.WriteLine("RESPUESTA DE FLASK");
            Debug.WriteLine("HTTP: " + (int)response.StatusCode);
            Debug.WriteLine("JSON recibido: " + resultJson);
            Debug.WriteLine("=================================");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(resultJson);
            }

            EmotionResult resultado =
                JsonConvert.DeserializeObject<EmotionResult>(resultJson);

            if (resultado == null)
            {
                throw new Exception(
                    "Flask respondió correctamente, pero no fue posible convertir la respuesta."
                );
            }
            return resultado;


        }
    }
}
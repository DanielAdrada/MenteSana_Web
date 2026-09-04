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
                await client.PostAsync("predecir_dass42", content);

            string resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(resultJson);
            }

            EmotionResult resultado =
                JsonConvert.DeserializeObject<EmotionResult>(resultJson);

            return resultado;


        }
    }
}
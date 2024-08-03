using System.Text;
using AlexAPI.Models;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace AlexAPI.Services
{
    public class GeminiAIService : IGeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string URL;
        private readonly GoogleCredential _credential;

        public GeminiAIService(IConfiguration configuration)
        {
            URL = $"{configuration.GetValue<string>("Gemini:BaseURL")}?key={configuration.GetValue<string>("Gemini:Key")}";
            _httpClient = new HttpClient();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var data = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(URL, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                return responseString.Candidates.First().Content.Parts.First().Text;
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}

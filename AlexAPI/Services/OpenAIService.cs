using AlexAPI.ResponseModels;
using AlexAPI.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace AlexAPI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public OpenAIService(string key, string endpoint)
        {
            this.endpoint = endpoint;
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var message = $"In a single 250 word paragraph: {prompt}";
            var requestBody = new
            {
                model = "gpt-3.5-turbo-0125",
                messages = new[]
                {
                            new { role = "system", content = message }
                        },
                max_tokens = 2048,
                temperature = 0.7
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(endpoint, content);
            var responseJson = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

            return apiResponse.Choices[0].Message.Content.Trim();
        }
    }
}

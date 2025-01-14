using AlexAPI.Library.Mail;
using AlexAPI.Library.OpenAI;
using AlexAPI.ResponseModels;
using AlexAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AlexAPI.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _endpoint;

        public OpenAIService(IOptions<OpenAISettings> mailSettings)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {mailSettings.Value.Key}");
            _endpoint = mailSettings.Value.BaseURL;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                var message = prompt;
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
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

                var response = await _httpClient.PostAsync(_endpoint, content);
                var responseJson = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseJson);

                return apiResponse.Choices[0].Message.Content.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> TestConnection()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
                return $"Response: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

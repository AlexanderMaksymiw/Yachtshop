using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System.Text;
using Microsoft.Extensions.Options;
using AlexAPI.Library.Llama;

namespace AlexAPI.Services
{
    public class LlamaService : ILlamaService
    {
        Kernel kernel;
        IChatCompletionService aiChatService;
        ChatHistory chatHistory = new ChatHistory();

        public LlamaService(IOptions<LlamaSettings> settings)
        {
            #pragma warning disable SKEXP0010
            kernel = Kernel.CreateBuilder()
                    .AddOpenAIChatCompletion(
                        modelId: settings.Value.Model,
                        endpoint: new Uri(settings.Value.BaseURL),
                        apiKey: settings.Value.Key)
                    .Build();

            aiChatService = kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            chatHistory.Add(new ChatMessageContent(AuthorRole.User, prompt));

            var responseBuilder = new StringBuilder();

            await foreach (var item in aiChatService.GetStreamingChatMessageContentsAsync(chatHistory))
            {
                responseBuilder.Append(item.Content);
            }
            var response = responseBuilder.ToString();
            chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, response));
            return response;
        }
    }
}

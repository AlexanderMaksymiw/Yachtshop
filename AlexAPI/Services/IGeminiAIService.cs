namespace AlexAPI.Services
{
    public interface IGeminiAIService
    {
        Task<string?> GetResponseAsync(string prompt);
    }
}

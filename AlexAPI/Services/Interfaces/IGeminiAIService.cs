namespace AlexAPI.Services.Interfaces
{
    public interface IGeminiAIService
    {
        Task<string?> GetResponseAsync(string prompt);
    }
}

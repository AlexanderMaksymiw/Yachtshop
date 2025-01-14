namespace AlexAPI.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<string?> GetResponseAsync(string prompt);
        Task<string> TestConnection();
    }
}

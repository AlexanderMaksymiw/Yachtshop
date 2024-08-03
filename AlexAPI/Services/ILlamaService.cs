namespace AlexAPI.Services
{
    public interface ILlamaService
    {
        Task<string> GetResponseAsync(string prompt);
    }
}

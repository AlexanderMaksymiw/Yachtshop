namespace AlexAPI.Services.Interfaces
{
    public interface ILlamaService
    {
        Task<string> GetResponseAsync(string prompt);
    }
}

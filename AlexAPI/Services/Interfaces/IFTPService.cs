namespace AlexAPI.Services.Interfaces
{
    public interface IFTPService
    {
        Task<string> UploadFile(IFormFile file, string directory, string filename);
        void DeleteDirectory(string directory);
    }
}

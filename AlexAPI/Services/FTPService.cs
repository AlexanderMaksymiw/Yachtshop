using AlexAPI.Models;
using AlexAPI.Services.Interfaces;
using FluentFTP;

namespace AlexAPI.Services
{
    public class FTPService : IFTPService
    {
        private readonly FtpClient ftpClient;
        public FTPService(FtpClient ftpClient)
        {
            this.ftpClient = ftpClient;
        }

        public async Task<string> UploadFile(IFormFile file, string directory, string filename)
        {
            var extension = Path.GetExtension(file.FileName);

            // Save the uploaded file temporarily
            using (var stream = new FileStream(Path.Combine(Path.GetTempPath(), file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ftpClient.Connect();
            if (!ftpClient.DirectoryExists(directory))
            {
                ftpClient.CreateDirectory(directory);
            }
            ftpClient.UploadFile(Path.Combine(Path.GetTempPath(), file.FileName), $"/u118215671/Images/{directory}/{filename}{extension}");
            ftpClient.Disconnect();

            // Optionally, delete the temporary file after uploading
            File.Delete(Path.Combine(Path.GetTempPath(), file.FileName));

            return $"images.yachtshop.com/{directory}/{filename}{extension}";
        }

        public void DeleteDirectory(string directory)
        {

            ftpClient.Connect();
            if (ftpClient.DirectoryExists(directory))
            {
                ftpClient.DeleteDirectory($"/u118215671/Images/{directory}");
            }
            ftpClient.Disconnect();
        }
    }
}

using AlexAPI.Library.FTP;
using AlexAPI.Services.Interfaces;
using FluentFTP;
using Microsoft.Extensions.Options;
using System.Net;

namespace AlexAPI.Services
{
    public class FTPService : IFTPService
    {
        private readonly FtpClient ftpClient;
        public FTPService(IOptions<FTPSettings> ftpSettings)
        {
            ftpClient = new FtpClient
            {
                Host = ftpSettings.Value.Host,
                Credentials = new NetworkCredential(ftpSettings.Value.Username, ftpSettings.Value.Password)
            };
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
            if (!ftpClient.DirectoryExists($"/u118215671/Images/{directory}"))
            {
                ftpClient.CreateDirectory($"/u118215671/Images/{directory}");
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

using AlexAPI.Services.Interfaces;

namespace AlexAPI.Services
{
    public class FileTemplateService : IFileTemplateService
    {
        private readonly IConfiguration configuration;
        public FileTemplateService(IConfiguration IConfig)
        {
            configuration = IConfig;
        }

        public string? GetPasswordResetLinkBody(string username, string resetLink)
        {
            StreamReader str = new StreamReader(Directory.GetCurrentDirectory() + "\\wwwroot\\EmailTemplates\\PasswordResetEmailTemplate.html");
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[Username]", username);
            mailText = mailText.Replace("[resetLink]", resetLink);
            return mailText;
        }

        public string? GetNewUserBody(string username, string password)
        {
            StreamReader str = new StreamReader(Directory.GetCurrentDirectory() + "\\wwwroot\\EmailTemplates\\NewUserEmailTemplate.html");
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[Username]", username);
            mailText = mailText.Replace("[Password]", password);
            return mailText;
        }
    }
}

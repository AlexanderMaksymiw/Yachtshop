namespace AlexAPI.Services
{
    public interface IFileTemplateService
    {
        string GetPasswordResetLinkBody(string Username, string resetLink);
        string GetNewUserBody(string Username, string password);
    }
}
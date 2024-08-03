namespace AlexAPI.Authentication
{
    public class PasswordResetModel
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? Password { get; set; }
    }
}

namespace AlexAPI.Authentication
{
    public class AddTwoFactorModel
    {
        public string? Email { get; set; }
    }
    public class CheckTwoFactorModel
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
    }
}

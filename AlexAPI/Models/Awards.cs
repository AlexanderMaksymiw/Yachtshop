namespace AlexAPI.Models
{
    public class Awards
    {
        public Guid Id { get; set; }
        public string? Competition { get; set; }
        public string? Class { get; set; }
        public string? Result { get; set; }
    }
}
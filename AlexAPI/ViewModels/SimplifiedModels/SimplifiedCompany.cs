namespace AlexAPI.ViewModels.SimplifiedModels
{
    public class SimplifiedCompany
    {
        public int id { get; set; }
        public string? name { get; set; }
        public List<SimplifiedUser> users { get; set; }
    }
}

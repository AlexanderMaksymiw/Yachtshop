using System.ComponentModel.DataAnnotations;

namespace AlexAPI.ViewModels.SimplifiedModels
{
    public class SimplifiedUser
    {
        public string? id { get; set; }
        public string? username { get; set; }
        [EmailAddress]
        public string? email { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }
        public List<int>? Companies { get; set; }
    }
}

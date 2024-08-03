using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Broker
    {
        [Key]
        public Guid Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class PriceTerm
    {
        [Key]
        public Guid Id { get; set; }
        public int MinRate { get; set; }
        public int MaxRate { get; set; }
        public string? Currency { get; set; }
        public string? Terms { get; set; }
    }
}

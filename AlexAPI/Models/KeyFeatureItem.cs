using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class KeyFeatureItem
    {
        [Key]
        public Guid Id { get; set; }
        public string? Content { get; set; }
    }
}

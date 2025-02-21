using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class KeyFeature
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}

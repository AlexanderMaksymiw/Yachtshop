using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class DealDay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        [Required]
        public decimal FromLat { get; set; }
        [Required]
        public decimal FromLong { get; set; }
        [Required]
        public decimal ToLat { get; set; }
        [Required]
        public decimal ToLong { get; set; }
    }
}

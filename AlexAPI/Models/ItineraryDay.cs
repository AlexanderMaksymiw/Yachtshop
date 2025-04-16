using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class ItineraryDay
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image {  get; set; }
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

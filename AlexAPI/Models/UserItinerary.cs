using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class UserItinerary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int Guests { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Departure { get; set; }
        [Required]
        public DateTime Arrival { get; set; }
        public virtual ICollection<Yacht>? Yachts { get; set; }
        public virtual ICollection<ItineraryDay>? Days { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("FromLocationId")]
        public virtual Location? FromLocation { get; set; }
        [ForeignKey("ToLocationId")]
        public virtual Location? ToLocation { get; set; }
    }
}

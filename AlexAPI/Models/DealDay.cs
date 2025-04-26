using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("FromLocationId")]
        public virtual Location? FromLocation { get; set; }
        [ForeignKey("ToLocationId")]
        public virtual Location? ToLocation { get; set; }
    }
}

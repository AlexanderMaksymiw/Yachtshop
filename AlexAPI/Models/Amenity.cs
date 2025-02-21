using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Amenity
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<Toy>? Toys { get; set; }
        public virtual ICollection<Equipment>? Equipment { get; set; }
    }
}
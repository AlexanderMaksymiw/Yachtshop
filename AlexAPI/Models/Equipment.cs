using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class Equipment
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
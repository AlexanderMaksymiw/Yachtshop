using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class Toy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Amenity> Amenities { get; set; }
    }
}
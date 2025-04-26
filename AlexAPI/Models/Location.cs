using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [JsonIgnore]
        public virtual ICollection<Yacht> Yachts { get; set; } = new List<Yacht>();
    }
}
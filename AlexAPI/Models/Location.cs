using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Yacht> Yachts { get; set; } = new List<Yacht>();
    }
}
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Yacht> Yachts { get; set; }
    }
}
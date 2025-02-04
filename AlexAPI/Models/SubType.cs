using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class SubType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Specification> Specifications { get; set; } = new List<Specification>();
    }
}

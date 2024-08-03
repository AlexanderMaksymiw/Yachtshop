using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class YachtListResponse
    {
        public List<Yacht> Data { get; set; }
        public List<object> Errors { get; set; }
    }

    public class Yacht
    {
        [Key]
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public string? RegistryPort { get; set; }
        public int Id { get; set; }
        public virtual YachtDetail? Detail { get; set; }
        public virtual YachtBrochure? Brochure { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class OperatingAreas
    {
        [Key]
        public Guid Id { get; set; }
        public virtual OperatingAreaItem? _25 { get; set; }
    }
    public class OperatingAreaItem
    {
        [Key]
        public Guid Id { get; set; }
        public List<int> Areas { get; set; }
    }
}

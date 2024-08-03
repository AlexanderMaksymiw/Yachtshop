using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class OperatingAreaNew
    {
        [Key]
        public Guid Id { get; set; }
        public int SeasonId { get; set; }
        public List<int> Areas { get; set; }
    }
}
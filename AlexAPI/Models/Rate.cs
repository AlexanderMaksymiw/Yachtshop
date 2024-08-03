using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Rate
    {
        [Key]
        public Guid Id { get; set; }
        public int? MinRate { get; set; }
        public int? MaxRate { get; set; }
        public string? Currency { get; set; }
        public int SeasonId { get; set; }
        public string? Term { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class SpecialRequest
    {
        [Key]
        public Guid Id { get; set; }
        public int SeasonId { get; set; }
        public List<int> SpecialRequests { get; set; }
    }

}
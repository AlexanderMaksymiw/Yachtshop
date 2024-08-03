using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Crew
    {
        [Key]
        public Guid Id { get; set; }
        public bool CrewPhotosSwitch { get; set; }
    }
}

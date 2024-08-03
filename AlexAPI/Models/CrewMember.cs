using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class CrewMember
    {
        [Key]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Position { get; set; }
        public string? Nationality { get; set; }
        public bool Tba { get; set; }
        public virtual Photo? Photo { get; set; }
    }
}

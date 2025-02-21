using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexAPI.Models
{
    public class Award
    {
        [Key]
        public Guid Id { get; set; }
        public string? Competition { get; set; }
        public string? Class { get; set; }
        public string? Result { get; set; }

        [NotMapped]
        public string? Description => $"{Result} in the {Class} class of {Competition}";
    }
}
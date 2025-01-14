using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace AlexAPI.Models
{
    public class Award
    {
        public Guid Id { get; set; }
        public string? Competition { get; set; }
        public string? Class { get; set; }
        public string? Result { get; set; }

        [NotMapped]
        public string? Description => $"{Result} in the {Class} class of {Competition}";
    }
}
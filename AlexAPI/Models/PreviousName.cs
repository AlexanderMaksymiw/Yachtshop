using System.ComponentModel.DataAnnotations;
namespace AlexAPI.Models
{
    public class PreviousName
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Photo
    {
        [Key]
        public Guid Id { get; set; }
        public int IdFile { get; set; }
        public DateTime DateAdded { get; set; }
        public string? Filename { get; set; }
        public string? Url { get; set; }
    }
}

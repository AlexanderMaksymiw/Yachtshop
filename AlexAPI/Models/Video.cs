using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Video
    {
        [Key]
        public Guid Id { get; set; }
        public string? Filename { get; set; }
        public string Url { get; set; }
    }
}
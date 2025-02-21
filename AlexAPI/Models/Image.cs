using AlexAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }
        public string? Filename { get; set; }
        public string? PhotographerName { get; set; }
        public ImageTypeEnum Type { get; set; }
        public string Url { get; set; }
    }
}
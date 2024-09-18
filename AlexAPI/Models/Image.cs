using AlexAPI.Enums;

namespace AlexAPI.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string? Filename { get; set; }
        public string? PhotographerName { get; set; }
        public ImageTypeEnum Type { get; set; }
        public string Url { get; set; }
    }
}
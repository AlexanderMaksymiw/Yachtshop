using AlexAPI.Enums;
using AlexAPI.Models;

namespace AlexAPI.RequestModels
{
    public class ImageDto
    {
        public string Filename { get; set; }
        public string? PhotographerName { get; set; }
        public ImageTypeEnum Type { get; set; }
        public string? Url { get; set; }
        public IFormFile Image { get; set; }
    }
}

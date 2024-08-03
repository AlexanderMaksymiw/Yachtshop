using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Video
    {
        [Key]
        public Guid Id { get; set; }
        public string? VideoId { get; set; }
        public string? VideoType { get; set; }
        public string? VideoUrl { get; set; }
        public bool VideoBrokerFriendly { get; set; }
        public virtual VideoInfo? VideoInfo { get; set; }
    }
}

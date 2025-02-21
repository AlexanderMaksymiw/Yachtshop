using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Media
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<Video>? Videos { get; set; }
    }
}
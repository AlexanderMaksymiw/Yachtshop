using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Gallery
    {
        [Key]
        public Guid Id { get; set; }
        public virtual List<GalleryItem>? Full { get; set; }
        public virtual List<GalleryItem>? Interior { get; set; }
        public virtual List<GalleryItem> Layout { get; set; }
        public virtual List<GalleryItem> Pdf { get; set; }
    }

    public class GalleryItem
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateAdded { get; set; }
        public int Order { get; set; }
        public int FileId { get; set; }
        public string? Filename { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
    }
}

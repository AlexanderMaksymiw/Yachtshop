using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Yacht
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SYTUrl { get; set; }
        public decimal? Price { get; set; }
        public bool OnSale { get; set; }
        public bool IsFeatured { get; set; }
        public string? HeroImageUrl { get; set; }
        public virtual Specification Specification { get; set; } = new Specification();
        public virtual Amenity Amenities { get; set; } = new Amenity();
        public virtual ICollection<Award>? Awards { get; set; }
        public virtual Media Media { get; set; } = new Media();
        public virtual ICollection<Location>? Locations { get; set; }
        public virtual ICollection<KeyFeature>? KeyFeatures { get; set; }
    }
}

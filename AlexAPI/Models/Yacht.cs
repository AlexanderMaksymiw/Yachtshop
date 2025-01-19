namespace AlexAPI.Models
{
    public class Yacht
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SYTUrl { get; set; }
        public virtual Price Price { get; set; } = new Price();
        public virtual decimal? PriceValue { get; set; }
        public virtual Specification Specification { get; set; } = new Specification();
        public virtual Amenity Amenities { get; set; } = new Amenity();
        public bool OnSale { get; set; }
        public virtual ICollection<Award>? Awards { get; set; }
        public virtual Media Media { get; set; } = new Media();
        public virtual ICollection<Location>? Locations { get; set; }
        public virtual ICollection<KeyFeature>? KeyFeatures { get; set; }
    }
}

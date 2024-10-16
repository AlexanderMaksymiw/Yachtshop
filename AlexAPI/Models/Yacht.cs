namespace AlexAPI.Models
{
    public class Yacht
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SYTUrl { get; set; }
        public virtual Price Price { get; set; } = new Price();
        public virtual Specification Specification { get; set; } = new Specification();
        public virtual Amenities Amenities { get; set; } = new Amenities();
        public virtual Awards Awards { get; set; } = new Awards();
        public virtual Media Media { get; set; } = new Media();
        public virtual ICollection<Location>? Locations { get; set; }
    }
}

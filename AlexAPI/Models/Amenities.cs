namespace AlexAPI.Models
{
    public class Amenities
    {
        public Guid Id { get; set; }
        public virtual ICollection<Toy>? Toys { get; set; }
        public virtual ICollection<Equipment>? Equipment { get; set; }
    }
}
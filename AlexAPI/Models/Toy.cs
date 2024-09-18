namespace AlexAPI.Models
{
    public class Toy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Amenities> Amenities { get; set; }
    }
}
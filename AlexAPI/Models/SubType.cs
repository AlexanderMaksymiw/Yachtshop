namespace AlexAPI.Models
{
    public class SubType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Specification> Specifications { get; set; }
    }
}

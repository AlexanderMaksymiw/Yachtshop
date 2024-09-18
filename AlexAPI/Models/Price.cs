namespace AlexAPI.Models
{
    public class Price
    {
        public Guid Id { get; set; }
        public decimal? Standard { get; set; }
        public decimal? Summer { get; set; }
        public decimal? Winter { get; set; }
    }
}
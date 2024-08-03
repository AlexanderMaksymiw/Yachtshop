using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Prices
    {
        [Key]
        public Guid Id { get; set; }
        public virtual List<PriceTerm>? _25 { get; set; }
    }
}

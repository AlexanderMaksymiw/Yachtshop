using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class General
    {
        [Key]
        public Guid Id { get; set; }
        public bool Available { get; set; }
        public string? DataSource { get; set; }
        public string? GeneralDescription { get; set; }
    }
}

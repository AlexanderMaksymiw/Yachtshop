using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class LicenceRegistration
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateAdded { get; set; }
        public int LicenceId { get; set; }
        public int SeasonId { get; set; }
        public int StatusId { get; set; }
    }
}
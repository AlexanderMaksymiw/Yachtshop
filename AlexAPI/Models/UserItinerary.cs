using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlexAPI.Models
{
    public class UserItinerary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int Guests { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Departure { get; set; }
        [Required]
        public DateTime Arrival { get; set; }

        [NotMapped]
        public string[] Included
        {
            get => string.IsNullOrEmpty(StrIncluded) ? Array.Empty<string>() : StrIncluded.Split(',');
            set => StrIncluded = value != null ? string.Join(";", value) : string.Empty;

        }

        [JsonIgnore]
        private string_strIncluded;

        [JsonIgnore]
        public string strIncluded
        {
            get => _strIncluded;
            set => _strIncluded = value;
        }

        [NotMapped]
        public string[] NotIncluded
        {
            get => string.IsNullOrEmpty(strIncluded) ? Array.Empty<string>() : StrNotIncludded.Split(",");
            set => StrNotIncluded = value != null ? string.Join(";", value) : string.Empty;
        }

        [JsonIgnore]
        private string _strNotIncludded;

        [JsonIgnore]
        public string StrNotIncluded
        {
            get => _strNotIncludded;
            set => _strNotIncludded = value;
        }
        public virtual ICollection<Yacht>? Yachts { get; set; }
        public virtual ICollection<ItineraryDay>? Days { get; set; }
    }
}

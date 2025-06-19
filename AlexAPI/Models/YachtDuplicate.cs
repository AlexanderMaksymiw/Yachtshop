using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class YachtDuplicateLog
    {
        public Guid Id { get; set; }
        public string YachtName { get; set; }
        public double ConfidenceScore { get; set; }
        public string MatchedFields { get; set; }
        public DateTime DateDetected { get; set; }

        public virtual Yacht OriginalYacht { get; set; }
        public virtual string DuplicateYacht { get; set; }
    }
}
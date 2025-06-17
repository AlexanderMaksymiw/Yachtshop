// YachtDuplicateReport.cs
using AlexAPI.Models;

namespace AlexAPI.Models
{
    public class YachtDuplicateReport
    {
        public string Name { get; set; }
        public double ConfidenceScore { get; set; }
        public string MatchedFields { get; set; }
        public int ExistingYachtId { get; set; }
    }
}

// YachtValidationResult.cs
namespace AlexAPI.Models
{
    public class YachtValidationResult
    {
        public Yacht Yacht { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
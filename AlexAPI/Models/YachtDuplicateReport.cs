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
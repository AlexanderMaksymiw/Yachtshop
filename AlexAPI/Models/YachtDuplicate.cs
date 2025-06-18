public class YachtDuplicate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string YachtName { get; set; }
    public double ConfidenceScore { get; set; }
    public string MatchedFields { get; set; }
    public DateTime DateDetected { get; set; }

    public Guid? OriginalYachtId { get; set; }
    public string IncomingYachtData { get; set; }
}

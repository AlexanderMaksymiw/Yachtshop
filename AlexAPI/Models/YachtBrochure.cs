using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class YachtBrochure
    {
        [Key]
        public Guid Id { get; set; }
        public virtual Auto? Auto { get; set; }
        public virtual Broker? Broker { get; set; }
        public virtual OperatingAreas? OperatingAreas { get; set; }
        public virtual List<CrewMember>? CrewMembers { get; set; }
        public virtual Crew? Crew { get; set; }
        public virtual Video? Video { get; set; }
        public virtual Specifications? Specifications { get; set; }
        public virtual Gallery? Galleries { get; set; }
        public virtual General? General { get; set; }
        public virtual Prices? Prices { get; set; }
        public virtual List<KeyFeatureItem>? KeyFeatures { get; set; }
    }
}

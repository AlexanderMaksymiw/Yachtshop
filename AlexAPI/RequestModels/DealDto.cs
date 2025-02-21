using AlexAPI.Models;

namespace AlexAPI.RequestModels
{
    public class DealDto
    {
        public CharterDeal Deal { get; set; }
        public Guid[] YachtIds { get; set; }
        public ICollection<DealDayDto> Days { get; set; }
    }
}

using AlexAPI.Models;

namespace AlexAPI.RequestModels
{
    public class ItineraryDto
    {
        public UserItinerary Itinerary { get; set; }
        public Guid[]? YachtIds { get; set; }
        public ICollection<ItineraryDayDto> Days { get; set; }
    }
}

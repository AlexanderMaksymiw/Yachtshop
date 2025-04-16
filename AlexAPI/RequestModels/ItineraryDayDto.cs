namespace AlexAPI.RequestModels
{
    public class ItineraryDayDto
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal FromLat { get; set; }
        public decimal FromLong { get; set; }
        public decimal ToLat { get; set; }
        public decimal ToLong { get; set; }
    }
}

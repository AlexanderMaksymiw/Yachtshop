namespace AlexAPI.RequestModels
{
    public class DealDayDto
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public Guid FromLoc { get; set; }
        public Guid ToLoc { get; set; }
    }
}

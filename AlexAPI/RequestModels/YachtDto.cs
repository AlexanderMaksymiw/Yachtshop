namespace AlexAPI.RequestModels
{
    public class YachtDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsFeatured { get; set; }
        public string? Type { get; set; }
        public decimal? Length { get; set; }
        public int? Guests { get; set; }
        public decimal? Price { get; set; }
        public string? HeroImageUrl { get; set; }
        public bool OnSale { get; set; }
        public SpecificationDto Specification { get; set; }
        public AmenityDto Amenities { get; set; }
        public bool UserAccess { get; set; } = false;
        public bool RequiresUserAccess { get; set; }


    }
}

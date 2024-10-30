namespace AlexAPI.ViewModels
{
    public class YachtDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? HullType { get; set; }
        public int? YearBuilt { get; set; }
        public string? Builder { get; set; }
        public decimal? Length { get; set; }
        public int? Guests { get; set; }
        public int? Cabins { get; set; }
        public string? Flag { get; set; }
        public string? Port { get; set; }
        public string? Superstructure { get; set; }
        public string? InteriorDesigner { get; set; }
        public string? ExteriorDesigner { get; set; }
        public int? Crew { get; set; }
        public decimal? Beam { get; set; }
        public decimal? Draft { get; set; }
        public int? GrossTonnage { get; set; }
        public decimal? MaxSpeed { get; set; }
        public decimal? CruisingSpeed { get; set; }
        public string? EnginePowerOutput { get; set; }
        public string? Model { get; set; }
        public string? PropulsionType { get; set; }
        public string? FuelCapacity { get; set; }
        public string? Filename { get; set; }
        public string? PhotographerName { get; set; }
        public int ImageType { get; set; }
        public string Url { get; set; }
    }

}

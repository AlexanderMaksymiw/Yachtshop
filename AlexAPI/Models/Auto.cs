using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Auto
    {
        [Key]
        public Guid Id { get; set; }
        public string? Length { get; set; }
        public string? LengthMetres { get; set; }
        public string? LengthFeet { get; set; }
        public string? Beam { get; set; }
        public string? BeamMetres { get; set; }
        public string? BeamFeet { get; set; }
        public string? Draft { get; set; }
        public string? DraftMetres { get; set; }
        public string? DraftFeet { get; set; }
        public int YearBuilt { get; set; }
        public int YearRefit { get; set; }
        public string? Flag { get; set; }
        public int TotalCrew { get; set; }
        public string? Builder { get; set; }
        public string? HullConstruction { get; set; }
        public string? HullConfiguration { get; set; }
        public string? Superstructure { get; set; }
        public string? Equipment { get; set; }
        public int GuestsSleeping { get; set; }
        public string? BedConfig { get; set; }
        public string? CabinConfig { get; set; }
        public int? Cabins { get; set; }
        public string? Engines { get; set; }
        public string? Toys { get; set; }
        public string? FuelConsumption { get; set; }
        public string? FuelConsumptionUnits { get; set; }
        public string? CruisingSpeed { get; set; }
    }
}

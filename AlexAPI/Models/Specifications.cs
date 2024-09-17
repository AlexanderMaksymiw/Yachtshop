using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Specifications
    {
        [Key]
        public Guid Id { get; set; }
        public string? ExteriorDesigner { get; set; }
        public string? InteriorDesigner { get; set; }
        public int KingBeds { get; set; }
        public int TwinCabins { get; set; }
        public int YearBuilt { get; set; }
        public string? Model { get; set; }
        public string? GrossTonnage { get; set; }
        public int? GrossTonnageNumeric { get; set; }
        public decimal? CruisingSpeed { get; set; }
        public int GuestsSleeping { get; set; }
        public string? FuelConsumption { get; set; }
        public int YearRefit { get; set; }
        public int DoubleCabins { get; set; }
        public string? CaptainsNationality { get; set; }
        public string? RegistryPort { get; set; }
        public int DoubleBeds { get; set; }
        public string? Beam { get; set; }
        public decimal? BeamMetres { get; set; }
        public string? MaxSpeed { get; set; }
        public decimal? MaxSpeedNumeric { get; set; }
        public string? Builder { get; set; }
        public string? SailPower { get; set; }
        public int SingleBeds { get; set; }
        public string? Length { get; set; }
        public decimal? LengthMetres { get; set; }
        public int QueenBeds { get; set; }
        public int GuestsCruising { get; set; }
        public int? Cabins { get; set; }
        public string? Flag { get; set; }
        public int PullmanBeds { get; set; }
        public bool GymEquipment { get; set; }
        public string? Draft { get; set; }
        public decimal? DraftMetres { get; set; }
        public string? Toys { get; set; }
        public string? CrewProfiles { get; set; }
        public string? Type { get; set; }
        public string? Port { get; set; }
        public string? TotalPowerOutput { get; set; }
        public string? PropulsionType { get; set; }
        public string? FuelCapacity { get; set; }
    }
}

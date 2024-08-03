using System.ComponentModel.DataAnnotations;

namespace AlexAPI.Models
{
    public class Specifications
    {
        [Key]
        public Guid Id { get; set; }
        public int KingBeds { get; set; }
        public int TwinCabins { get; set; }
        public int YearBuilt { get; set; }
        public int CruisingSpeed { get; set; }
        public int GuestsSleeping { get; set; }
        public string? FuelConsumption { get; set; }
        public int YearRefit { get; set; }
        public int DoubleCabins { get; set; }
        public string? CaptainsNationality { get; set; }
        public string? RegistryPort { get; set; }
        public int DoubleBeds { get; set; }
        public string? Beam { get; set; }
        public string? BeamMetres { get; set; }
        public int MaxSpeed { get; set; }
        public string? Builder { get; set; }
        public string? SailPower { get; set; }
        public int SingleBeds { get; set; }
        public string? Length { get; set; }
        public string? LengthMetres { get; set; }
        public int QueenBeds { get; set; }
        public int GuestsCruising { get; set; }
        public int? Cabins { get; set; }
        public string? Flag { get; set; }
        public int PullmanBeds { get; set; }
        public bool GymEquipment { get; set; }
        public string? Draft { get; set; }
        public string? DraftMetres { get; set; }
        public string? Toys { get; set; }
        public string? CrewProfiles { get; set; }
    }
}

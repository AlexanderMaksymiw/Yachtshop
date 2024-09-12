using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlexAPI.Authentication;

namespace AlexAPI.Models
{
    public class YachtDetailResponse
    {
        public List<YachtDetail> Data { get; set; }
        public List<object> Errors { get; set; }
    }

    public class YachtDetail
    {
        [Key]
        public Guid Guid { get; set; }
        public int Id { get; set; } // Missing from JSON
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime? DateWentPublic { get; set; } // Changed to nullable DateTime
        public string? YachtName { get; set; }
        public string? PreviousName { get; set; }
        public string? ListingType { get; set; }
        public string? SailPower { get; set; }
        public int YearBuilt { get; set; }
        public string? Builder { get; set; }
        public string? OtherBuilder { get; set; }
        public string? Flag { get; set; }
        public string? YachtAdmin { get; set; }
        public string? YachtAdminEmail { get; set; }
        public string? YachtAdminPhone { get; set; }
        public int Cabins { get; set; }
        public int SingleCabins { get; set; }
        public int TwinCabins { get; set; }
        public int DoubleCabins { get; set; }
        public int? TripleCabins { get; set; } // Nullable
        public int? ConvertibleCabins { get; set; } // Nullable
        public int GuestsSleeping { get; set; }
        public int GuestsCruising { get; set; }
        public string? FuelConsumption { get; set; }
        public int CruisingSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public int TotalCrew { get; set; }
        public string? Captain { get; set; }
        public string? CaptainsNationality { get; set; }
        public string? Chef { get; set; }
        public string? ChiefStewardess { get; set; }
        public string? ChiefEngineer { get; set; }
        public int MybaTippingPolicy { get; set; }
        public int? YearRefit { get; set; } // Nullable
        public int? GrossTons { get; set; } // Nullable
        public int MasterCabinOnMainDeck { get; set; }
        public string? Range { get; set; } // Nullable
        public int? BulkBeds { get; set; } // Nullable
        public int Beds { get; set; }
        public int KingBeds { get; set; }
        public int QueenBeds { get; set; }
        public int DoubleBeds { get; set; }
        public int SingleBeds { get; set; }
        public int PullmanBeds { get; set; }
        public string? Rig { get; set; }
        public string? Classification { get; set; }
        public string? InteriorDesigner { get; set; }
        public string? NavalArchitect { get; set; }
        public string? HullConfiguration { get; set; }
        public string? HullConstruction { get; set; }
        public string? SummerBasePort { get; set; }
        public string? WinterBasePort { get; set; }
        public string? RegistryPort { get; set; }
        public string? GuestsAccommodation { get; set; }
        public string? LocationDet { get; set; }
        public string? RateDet { get; set; }
        public string? TaxComments { get; set; }
        public string? SpecialConditions { get; set; }
        public string? Contracts { get; set; }
        public string? EnginesGenerators { get; set; }
        public string? RefitDet { get; set; }
        public string? Toys { get; set; }
        public string? AvFacilities { get; set; }
        public string? Communications { get; set; }
        public string? CommercialStatus { get; set; }
        public string? CrewProfiles { get; set; }
        public DateTime CrewModified { get; set; }
        public string? Notes { get; set; }
        public string? Superstructure { get; set; }
        public string? IndependentStakeholder { get; set; }
        public int OwnerCaOperated { get; set; }
        public int OwnerCaptainOperated { get; set; }
        public string? LengthMetric { get; set; }
        public string? BeamMetric { get; set; }
        public string? DraftMetric { get; set; }
        public string? LengthImperial { get; set; }
        public string? BeamImperial { get; set; }
        public string? DraftImperial { get; set; }
        public string? Price { get; set; }
        public string? SummerRates { get; set; }
        public string? WinterRates { get; set; }
        public string? AwardNominations { get; set; }
        public List<int>? Equipment { get; set; } // Check JSON data type
        public List<string>? OperatingAreas { get; set; } // Check JSON data type
        public virtual List<Rate>? Rates { get; set; } // Check JSON data type
        public virtual List<OperatingAreaNew>? OperatingAreasNew { get; set; } // Check JSON data type
        public virtual List<SpecialRequest>? SpecialRequests { get; set; } // Check JSON data type
        public List<int>? SeasonsUnavailable { get; set; } // Check JSON data type
        public virtual List<LicenceRegistration>? LicencesRegistrations { get; set; } // Check JSON data type
    }

}

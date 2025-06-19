using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.Data;
using AlexAPI.RequestModels;
using Microsoft.AspNetCore.Mvc;
using AlexAPI.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Drawing.Text;
using AlexAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using CsvHelper;
using Azure.Identity;
using System.Reflection.Metadata.Ecma335;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly IFTPService ftpService;
        private readonly ApplicationDbContext _dbContext;
        private readonly DeduplicationService deduplicationService;



        public YachtController(ILogger<YachtController> logger, ApplicationDbContext dbContext, YachtWorkUnit workUnit, IFTPService ftpService, DeduplicationService deduplicationService)
        {
            this.logger = logger;
            this._dbContext = dbContext;
            this.workUnit = workUnit;
            this.ftpService = ftpService;
            this.deduplicationService = deduplicationService;
        }

        private AmenityDto MapToAmenityDto(Amenity amenity)
        {
            return new AmenityDto
            {
                Id = amenity.Id,

                ToyNames = amenity.Toys?.Select(t => t.Name).Distinct().ToList() ?? new List<string>(),
                EquipmentNames = amenity.Equipment?.Select(e => e.Name).Distinct().ToList() ?? new List<string>()
            };
        }

        private SpecificationDto MapToSpecDto(Specification spec)
        {
            return new SpecificationDto
            {
                Id = spec.Id,
                Type = spec.Type,
                HullType = spec.HullType,
                YearBuilt = spec.YearBuilt,
                Builder = spec.Builder,
                Length = spec.Length,
                Guests = spec.Guests,
                Cabins = spec.Cabins,
                Flag = spec.Flag,
                Port = spec.Port,
                Superstructure = spec.Superstructure,
                InteriorDesigner = spec.InteriorDesigner,
                ExteriorDesigner = spec.ExteriorDesigner,
                Crew = spec.Crew,
                Beam = spec.Beam,
                Draft = spec.Draft,
                GrossTonnage = spec.GrossTonnage,
                MaxSpeed = spec.MaxSpeed,
                CruisingSpeed = spec.CruisingSpeed,
                EnginePowerOutput = spec.EnginePowerOutput,
                Model = spec.Model,
                PropulsionType = spec.PropulsionType,
                FuelCapacity = spec.FuelCapacity,
                Class = spec.Class
            };
        }

        private string GenerateCsv(List<YachtDuplicateReport> duplicates)
        {
             var sb = new StringBuilder();
             sb.AppendLine("Name,ConfidenceScore,MatchedFields");
             foreach (var dup in duplicates)
             {
                sb.AppendLine($"\"{dup.Name}\",{dup.ConfidenceScore},\"{dup.MatchedFields}\"");
             }
             return sb.ToString();
        }



            [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var yachtDict = new Dictionary<Guid, Yacht>();

                string sql = @"
                    SELECT y.*, s.*, m.*, i.*, a.*, l.*, kf.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN Images i ON m.Id = i.MediaId
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    LEFT JOIN AmenityToys at ON a.Id = at.AmenityId
                    LEFT JOIN Toys t ON at.ToyId = t.Id
                    LEFT JOIN AmenityEquipment ae ON a.Id = AmenityId
                    LEFT JOIN Equipment e ON ae.EquipmentId = e.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    LEFT JOIN KeyFeatures kf ON y.Id = kf.YachtId
                    WHERE y.Id = @Id";

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, AlexAPI.Models.Image, Amenity, Location, KeyFeature>(
                    sql,
                    (y, s, m, i, a, l, kf) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Amenities = a;
                            yacht.Media.Images = new List<AlexAPI.Models.Image>();
                            yacht.Locations = new List<Location>();
                            yacht.KeyFeatures = new List<KeyFeature>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        if (kf != null && !yacht.KeyFeatures.Any(x => x.Id == kf.Id))
                            yacht.KeyFeatures.Add(kf);

                        if (i != null && !yacht.Media.Images.Any(x => x.Id == i.Id))
                            yacht.Media.Images.Add(i);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id,Id,Id,Id",
                    parameters: new { Id = id }
                );

                var yacht = result.FirstOrDefault();

                return Ok(yacht);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("GetByIds")]
        public IActionResult GetByIds(Guid[] ids)
        {
            try
            {
                var yachtDict = new Dictionary<Guid, Yacht>();

                string sql = @"
                    SELECT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE y.Id IN @Ids";

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new { Ids = ids }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult SearchYachts(
            string? name = null,
            string? type = null,
            string? destination = null,
            int page = 0,
            int numResults = 25,
            int? minGuests = null,
            int? maxGuests = null,
            int? minPrice = null,
            int? maxPrice = null,
            int? length = null,
            int? yearBuilt = null,
            int? cabins = null,
            int? maxSpeed = null,
            int? grossTonnage = null,
            int? cruisingSpeed = null,
            string? subType = null,
            string? hullType = null,
            string? builder = null,
            string[]? equipment = null,
            bool? onSale = null
        )
        {
            try
            {
                bool userHasAccess = User.IsInRole("User");

                var parameters = new List<SqlParameter>();
                var conditions = new List<string>();

                if (!string.IsNullOrEmpty(name))
                {
                    conditions.Add("y.Name LIKE @Name");
                    parameters.Add(new SqlParameter("@Name", $"%{name}%"));
                }

                if (!string.IsNullOrEmpty(type))
                {
                    conditions.Add("s.Type = @Type");
                    parameters.Add(new SqlParameter("@Type", type));
                }

                if (!string.IsNullOrEmpty(destination))
                {
                    conditions.Add("EXISTS (SELECT 1 FROM locationYacht yl JOIN Locations l ON yl.LocationsId = l.Id WHERE yl.YachtsId = y.Id AND l.Name = @Destination)");
                    parameters.Add(new SqlParameter("@Destination", destination));
                }

                if (minPrice.HasValue)
                {
                    conditions.Add("y.Price >= @MinPrice");
                    parameters.Add(new SqlParameter("@MinPrice", minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    conditions.Add("y.Price <= @MaxPrice");
                    parameters.Add(new SqlParameter("@MaxPrice", maxPrice.Value));
                }

                if (length.HasValue)
                {
                    conditions.Add("s.Length >= @Length");
                    parameters.Add(new SqlParameter("@Length", length.Value));
                }

                if (minGuests.HasValue)
                {
                    conditions.Add("s.Guests >= @MinGuests");
                    parameters.Add(new SqlParameter("@MinGuests", minGuests.Value));
                }

                if (maxGuests.HasValue)
                {
                    conditions.Add("s.Guests <= @MaxGuests");
                    parameters.Add(new SqlParameter("@MaxGuests", maxGuests.Value));
                }

                if (yearBuilt.HasValue)
                {
                    conditions.Add("s.YearBuilt >= @YearBuilt");
                    parameters.Add(new SqlParameter("@YearBuilt", yearBuilt.Value));
                }

                if (cabins.HasValue)
                {
                    conditions.Add("s.Cabins >= @Cabins");
                    parameters.Add(new SqlParameter("@Cabins", cabins.Value));
                }

                if (maxSpeed.HasValue)
                {
                    conditions.Add("s.MaxSpeed >= @MaxSpeed");
                    parameters.Add(new SqlParameter("@MaxSpeed", maxSpeed.Value));
                }

                if (grossTonnage.HasValue)
                {
                    conditions.Add("s.GrossTonnage >= @GrossTonnage");
                    parameters.Add(new SqlParameter("@GrossTonnage", grossTonnage.Value));
                }

                if (cruisingSpeed.HasValue)
                {
                    conditions.Add("s.CruisingSpeed >= @CruisingSpeed");
                    parameters.Add(new SqlParameter("@CruisingSpeed", cruisingSpeed.Value));
                }

                if (!string.IsNullOrEmpty(subType))
                {
                    conditions.Add("EXISTS (SELECT 1 FROM SpecificationSubType sst JOIN SubTypes st ON sst.SubTypesId = st.Id WHERE sst.SpecificationsId = s.Id AND st.Name = @SubType)");
                    parameters.Add(new SqlParameter("@SubType", subType));
                }

                if (!string.IsNullOrEmpty(hullType))
                {
                    conditions.Add("s.HullType = @HullType");
                    parameters.Add(new SqlParameter("@HullType", hullType));
                }

                if (!string.IsNullOrEmpty(builder))
                {
                    conditions.Add("s.Builder = @Builder");
                    parameters.Add(new SqlParameter("@Builder", builder));
                }

                if (equipment != null && equipment.Any())
                {
                    conditions.Add("EXISTS (SELECT 1 FROM AmenityEquipment ae JOIN Equipment e ON ae.EquipmentId = e.Id WHERE ae.AmenityId = a.Id AND e.Name IN @Equipment)");
                    parameters.Add(new SqlParameter("@Equipment", equipment));
                }

                if (onSale.HasValue)
                {
                    conditions.Add("y.OnSale = @OnSale");
                    parameters.Add(new SqlParameter("@OnSale", onSale.Value));
                }

                string whereClause = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

                string sql = $@"
                    SELECT 
                        y.*, s.*, a.*, t.*, e.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    LEFT JOIN AmenityToy at ON a.Id = at.AmenitiesId
                    LEFT JOIN Toys t ON at.ToysId = t.Id
                    LEFT JOIN AmenityEquipment ae ON a.Id = ae.AmenitiesId
                    LEFT JOIN Equipment e ON ae.EquipmentId = e.Id
                    {whereClause}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var sqlParams = new DynamicParameters();

                // Add paging params
                sqlParams.Add("@Page", page);
                sqlParams.Add("@NumResults", numResults);

                // Add all your collected SqlParameters
                foreach (var param in parameters)
                {
                    sqlParams.Add(param.ParameterName, param.Value);
                }

                var yachtDict = new Dictionary<Guid, Yacht>();

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Amenity, Toy, Equipment>(
                    sql,
                    map: (yacht, spec, amenity, toy, equipment) =>
                    {
                        if (!yachtDict.TryGetValue(yacht.Id, out var yachtEntry))
                        {
                            yachtEntry = yacht;
                            yachtEntry.Specification = spec;

                            if (amenity == null)
                                amenity = new Amenity();

                            if (amenity.Toys == null)
                                amenity.Toys = new List<Toy>();
                            if (amenity.Equipment == null)
                                amenity.Equipment = new List<Equipment>();

                            yachtEntry.Amenities = amenity;

                            yachtDict.Add(yachtEntry.Id, yachtEntry);
                        }

                        if (toy != null && !yachtEntry.Amenities.Toys.Any(t => t.Id == toy.Id))
                        {
                            yachtEntry.Amenities.Toys.Add(toy);
                        }

                        if (equipment != null && !yachtEntry.Amenities.Equipment.Any(e => e.Id == equipment.Id))
                        {
                            yachtEntry.Amenities.Equipment.Add(equipment);
                        }

                        return yachtEntry;
                    },

                    splitOn: "Id,Id,Id,Id",
                    parameters: sqlParams

                );
                // Map entity yachts to DTOs
                var resultYachts = yachtDict.Values.ToList();
                // Filter yachts based on access requirement and user role
                var filteredYachts = resultYachts
                    .ToList();

                var yachtDtos = filteredYachts.Select(y => new YachtDto
                {
                    Id = y.Id,
                    Name = y.Name,
                    Description = y.Description,
                    Price = y.Price,
                    OnSale = y.OnSale,
                    IsFeatured = y.IsFeatured,
                    HeroImageUrl = y.HeroImageUrl,
                    Specification = y.Specification != null ? MapToSpecDto(y.Specification) : null,
                    Amenities = y.Amenities != null ? MapToAmenityDto(y.Amenities) : new AmenityDto

                    {
                        ToyNames = new List<string>(),
                        EquipmentNames = new List<string>()
                    },

                    UserAccess = userHasAccess
                }).ToList();

                return Ok(yachtDtos);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetYachts(int page = 0, int numResults = 25)
        {
            try
            {
                string sql = @"
                    SELECT 
                        y.*, s.*,a.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    ORDER BY y.Id
                    OFFSET @Page ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var sqlParams = new DynamicParameters();
                sqlParams.Add("@Page", page * numResults);
                sqlParams.Add("@NumResults", numResults);

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Amenity>(
                    sql,
                    map: (yacht, spec, Amenity) =>
                    {
                        yacht.Specification = spec;
                        yacht.Amenities = Amenity;
                        return yacht;
                    },
                    splitOn: "Id,Id", // split on Specification.Id
                    parameters: sqlParams
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHeroImage")]
        public IActionResult GetHeroImage(Guid id)
        {
            try
            {
                var yachtDict = new Dictionary<Guid, Yacht>();

                string sql = @"
                    SELECT y.*, s.*, m.*, i.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN Images i ON m.Id = i.MediaId AND i.[Type] = 0
                    WHERE y.Id = @Id";

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, AlexAPI.Models.Image>(
                    sql,
                    (y, s, m, i) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Media.Images = new List<AlexAPI.Models.Image>();
                            yachtDict[yacht.Id] = yacht;
                        }

                        if (i != null && !yacht.Media.Images.Any(x => x.Id == i.Id))
                            yacht.Media.Images.Add(i);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id",
                    parameters: new { Id = id }
                );

                var yacht = result.FirstOrDefault();

                return Ok(yacht.Media.Images.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDropdownChoices")]
        public IActionResult GetDropdownChoices()
        {
            try
            {
                var destinations = workUnit.LocationRepository.Get();
                return Ok(new DropdownChoices
                {
                    Destinations = destinations,

                });
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllYachtNames")]
        public IActionResult GetAllYachtNames()
        {
            try
            {
                var sql = @"
                    SELECT y.*
                    FROM Yachts y
                ";

                return Ok(workUnit.YachtRepository.ExecuteSqlQuery<Yacht>(sql, new { }).Select(x => new
                {
                    x.Id,
                    x.Name
                }));
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetYachtsUnder50K")]
        public IActionResult GetYachtsUnder50K(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    WHERE y.Price <= 50000
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetYachtsOver50K")]
        public IActionResult GetYachtsOver50K(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    WHERE y.Price >= 50000
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCharterYachts")]
        public IActionResult Get(
            string? name = null,
            string? type = null,
            string? destination = null,
            int page = 0,
            int numResults = 25,
            int? minPrice = null,
            int? maxPrice = null,
            int? length = null,
            int? guests = null,
            int? yearBuilt = null,
            int? cabins = null,
            int? maxSpeed = null,
            int? grossTonnage = null,
            int? cruisingSpeed = null,
            string? subType = null,
            string? hullType = null,
            string? builder = null,
            string[]? equipment = null
        )
        {
            try
            {
                var parameters = new DynamicParameters();
                var conditions = new List<string>();

                if (!string.IsNullOrEmpty(name))
                {
                    conditions.Add("y.Name LIKE @Name");
                    parameters.Add("@Name", $"%{name}%");
                }

                if (!string.IsNullOrEmpty(type))
                {
                    conditions.Add("s.Type = @Type");
                    parameters.Add("@Type", type);
                }

                if (!string.IsNullOrEmpty(destination))
                {
                    conditions.Add("EXISTS (SELECT 1 FROM LocationYacht ly JOIN Locations l ON ly.LocationsId = l.Id WHERE ly.YachtsId = y.Id AND l.Name = @Destination)");
                    parameters.Add("@Destination", destination);
                }

                if (minPrice.HasValue)
                {
                    conditions.Add("y.Price >= @MinPrice");
                    parameters.Add("@MinPrice", minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    conditions.Add("y.Price <= @MaxPrice");
                    parameters.Add("@MaxPrice", maxPrice.Value);
                }

                if (length.HasValue)
                {
                    conditions.Add("s.Length >= @Length");
                    parameters.Add("@Length", length.Value);
                }

                if (guests.HasValue)
                {
                    conditions.Add("s.Guests >= @Guests");
                    parameters.Add("@Guests", guests.Value);
                }

                if (yearBuilt.HasValue)
                {
                    conditions.Add("s.YearBuilt >= @YearBuilt");
                    parameters.Add("@YearBuilt", yearBuilt.Value);
                }

                if (cabins.HasValue)
                {
                    conditions.Add("s.Cabins >= @Cabins");
                    parameters.Add("@Cabins", cabins.Value);
                }

                if (maxSpeed.HasValue)
                {
                    conditions.Add("s.MaxSpeed >= @MaxSpeed");
                    parameters.Add("@MaxSpeed", maxSpeed.Value);
                }

                if (grossTonnage.HasValue)
                {
                    conditions.Add("s.GrossTonnage >= @GrossTonnage");
                    parameters.Add("@GrossTonnage", grossTonnage.Value);
                }

                if (cruisingSpeed.HasValue)
                {
                    conditions.Add("s.CruisingSpeed >= @CruisingSpeed");
                    parameters.Add("@CruisingSpeed", cruisingSpeed.Value);
                }

                if (!string.IsNullOrEmpty(subType))
                {
                    conditions.Add("EXISTS (SELECT 1 FROM SpecificationSubTypes sst JOIN SubTypes st ON sst.SubTypeId = st.Id WHERE sst.SpecificationId = s.Id AND st.Name = @SubType)");
                    parameters.Add("@SubType", subType);
                }

                if (!string.IsNullOrEmpty(hullType))
                {
                    conditions.Add("s.HullType = @HullType");
                    parameters.Add("@HullType", hullType);
                }

                if (!string.IsNullOrEmpty(builder))
                {
                    conditions.Add("s.Builder = @Builder");
                    parameters.Add("@Builder", builder);
                }

                if (equipment != null && equipment.Any())
                {
                    conditions.Add("EXISTS (SELECT 1 FROM AmenityEquipment ae JOIN Equipment e ON ae.EquipmentId = e.Id WHERE ae.AmenityId = a.Id AND e.Name IN @Equipment)");
                    parameters.Add("@Equipment", equipment);
                }

                // Always filter for non-sale yachts
                conditions.Add("y.OnSale = 0");

                string whereClause = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    {whereClause}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                parameters.Add("@Page", page);
                parameters.Add("@NumResults", numResults);

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: parameters
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetFeatured")]
        public IActionResult GetFeatured()
        {
            try
            {
                var yachtDict = new Dictionary<Guid, Yacht>();

                string sql = @"
                SELECT 
                y.*,
                s.*,
                m.*,
                i.*
            FROM Yachts y
            LEFT JOIN Specifications s ON y.SpecificationId = s.Id
            LEFT JOIN Media m ON y.MediaId = m.Id
            LEFT JOIN Images i ON m.Id = i.MediaId
            WHERE y.IsFeatured = 1
            ORDER BY y.Id";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, AlexAPI.Models.Image>(
                    sql,
                    (y, s, m, i) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m ?? new Media();
                            yacht.Media.Images = new List<AlexAPI.Models.Image>();
                            yachtDict[y.Id] = yacht;
                        }

                        if (i != null && i.Id != Guid.Empty)
                        {
                            var images = yachtDict[y.Id].Media.Images;
                            if (!images.Any(img => img.Id == i.Id))
                                images.Add(i);
                        }

                        return yacht;
                    },
                    splitOn: "Id,Id,Id"
                );

                return Ok(yachtDict.Values.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetSalesYachts")]
        public IActionResult GetSalesYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                var sql = @"
                    SELECT y.*
                    FROM Yachts y
                    WHERE y.OnSale = 1
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY
                ";

                return Ok(workUnit.YachtRepository.ExecuteSqlQuery<Yacht>(sql, new
                {
                    Page = page,
                    NumResults = numResults
                }));
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSailingYachts")]
        public IActionResult GetYachtsByType(
            string type,
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    WHERE s.Type = @Type
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Type = type,
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCatamarans")]
        public IActionResult GetCatamarans(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    WHERE s.HullType = 'Catamaran'
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetYachtBySubType")]
        public IActionResult GetYachtBySubType(
            string subType,
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN SpecificationSubTypes sst ON s.Id = sst.SpecificationId
                    LEFT JOIN SubTypes st ON sst.SubTypeId = st.Id
                    WHERE st.Name = @SubType
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        SubType = subType,
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMediterraneanYachts")]
        public IActionResult GetMediterraneanYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> mediterraneanLocations = LocationHelper.MediterraneanLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", mediterraneanLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCaribbeanYachts")]
        public IActionResult GetCaribbeanYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> caribbeanLocations = LocationHelper.CaribbeanLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", caribbeanLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAsiaYachts")]
        public IActionResult GetAsiaYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> asiaLocations = LocationHelper.AsiaLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", asiaLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMiddleEastYachts")]
        public IActionResult GetMiddleEastYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> middleEastLocations = LocationHelper.MiddleEastLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", middleEastLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetIndianOceanYachts")]
        public IActionResult GetIndianOceanYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> indianOceanLocations = LocationHelper.IndianOceanLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", indianOceanLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetOceaniaYachts")]
        public IActionResult GetOceaniaYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> oceaniaLocations = LocationHelper.OceaniaLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", oceaniaLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetNorthAmericaYachts")]
        public IActionResult GetNorthAmericaYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> northAmericaLocations = LocationHelper.NorthAmericaLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", northAmericaLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSouthAmericaYachts")]
        public IActionResult GetSouthAmericaYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> southAmericaLocations = LocationHelper.SouthAmericaLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", southAmericaLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetEuropeanYachts")]
        public IActionResult GetEuropeanYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                List<string> europeanLocations = LocationHelper.EuropeanLocations.Select(x => x.Name).ToList();
                string locationCondition = string.Join(" OR ", europeanLocations.Select(loc => $"l.Name LIKE '%{loc}%'"));

                string sql = $@"
                    SELECT DISTINCT y.*, s.*, m.*, l.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    WHERE {locationCondition}
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachtDict = new Dictionary<Guid, Yacht>();

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Location>(
                    sql,
                    (y, s, m, l) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Locations = new List<Location>();
                            yachtDict[yacht.Id] = yacht;
                        }
                        if (l != null && !yacht.Locations.Any(x => x.Id == l.Id))
                            yacht.Locations.Add(l);

                        return yacht;
                    },
                    splitOn: "Id,Id,Id,Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(yachtDict.Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("Delete")]
        public IActionResult DeleteYacht(Guid id)
        {
            try
            {
                workUnit.YachtRepository.Delete(id);
                workUnit.Save();
                ftpService.DeleteDirectory($"Yacht/{id}");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("UploadLocations")]
        public IActionResult UploadLocations()
        {
            try
            {
                LocationHelper.GetAllLocations().ForEach(x =>
                {
                    workUnit.LocationRepository.Insert(x);
                });
                workUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("YachtLocationUpdate")]
        public IActionResult YachtLoationUpdate(IFormFile csv)
        {
            try
            {
                if (csv == null || csv.Length == 0)
                    return BadRequest("CSV file is empty or missing.");

                using var stream = csv.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csvReader.GetRecords<YachtLocation>().ToList();

                foreach (var x in records)
                {
                    var yacht = workUnit.YachtRepository.GetByID(new Guid(x.Id));
                    var location = workUnit.LocationRepository.Get().FirstOrDefault(y => x.LocationName == y.Name);
                    if(location == null)
                    {
                        continue;
                    }
                    yacht.Locations.Add(location);
                };
                workUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("UpdateHeroImage")]
        public IActionResult UpdateHeroImage(Guid yachtId, string url)
        {
            try
            {
                var yacht = workUnit.YachtRepository.GetByID(yachtId);
                if (yacht.Media.Images.Any(x => x.Type == 0))
                {
                    yacht.Media.Images.First(x => x.Type == 0).Url = url;
                }
                else
                {
                    yacht.Media.Images.Add(new AlexAPI.Models.Image
                    {
                        Filename = "Hero Image",
                        Type = 0,
                        Url = url
                    });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("AddImages")]
        public async Task<IActionResult> AddImages(Guid yachtId, [FromForm] ImageDto[] imageDtos)
        {
            try
            {
                var yacht = workUnit.YachtRepository.GetByID(yachtId);
                if (!yacht.Media.Images.Any())
                {
                    yacht.Media.Images = new List<AlexAPI.Models.Image>();
                }
                foreach (var item in imageDtos)
                {
                    yacht.Media.Images.Add(new AlexAPI.Models.Image
                    {
                        Filename = $"{item.Filename}{Path.GetExtension(item.Image.FileName)}",
                        PhotographerName = item.PhotographerName,
                        Type = item.Type,
                        Url = item.Url ?? await ftpService.UploadFile(item.Image, $"Yacht/{yacht.Id}", item.Filename)
                    });
                }
                workUnit.YachtRepository.Update(yacht);
                workUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("UploadAllWebpImagesUsingFtpService")]
        public async Task<IActionResult> UploadAllWebpImagesUsingFtpService(
            [FromServices] IServiceScopeFactory scopeFactory)
        {
            int uploaded = 0;
            int skipped = 0;
            int failed = 0;
            int processed = 0;

            await using var writerScope = scopeFactory.CreateAsyncScope();
            var writerDb = writerScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await using var readerScope = scopeFactory.CreateAsyncScope();
            var readerDb = readerScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await foreach (var image in readerDb.Images
                .Where(i => i.WebpData != null)
                .AsAsyncEnumerable())
            {
                processed++;

                try
                {
                    // Fetch yacht on demand for this image's MediaId
                    var yacht = await writerDb.Yachts
                        .Include(y => y.Media)
                        .ThenInclude(m => m.Images)
                        .FirstOrDefaultAsync(y => y.Media != null && y.Media.Id == image.MediaId);

                    if (yacht == null)
                    {
                        skipped++;
                        continue;
                    }

                    // --- Slugify helper function ---
                    static string Slugify(string input)
                    {
                        string normalized = input.Normalize(NormalizationForm.FormD);
                        var sb = new StringBuilder();
                        foreach (var c in normalized)
                            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                                sb.Append(c);

                        string slug = sb.ToString().Normalize(NormalizationForm.FormC)
                                        .ToLowerInvariant();
                        slug = Regex.Replace(slug, @"\s+", "-");
                        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
                        slug = Regex.Replace(slug, @"-+", "-");
                        return slug.Trim('-');
                    }

                    string slugYachtName = Slugify(yacht.Name ?? "unknown");
                    string slugFileName = Slugify(Path.GetFileNameWithoutExtension(image.Filename ?? $"image-{image.Id}"));
                    string typeFolder = image.Type.ToString();

                    string destPath = Path.Combine("Website", "Images", "Yachts",
                                                    yacht.Id.ToString(), slugYachtName, typeFolder);

                    if (!Directory.Exists(destPath))
                        Directory.CreateDirectory(destPath);

                    await using var ms = new MemoryStream(image.WebpData);
                    var formFile = new FormFile(ms, 0, ms.Length, image.Id.ToString(), image.Filename)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/webp"
                    };

                    await ftpService.UploadFile(formFile, destPath, slugFileName);

                    string publicUrl = $"https://yachtshop.com/images/yachts/{yacht.Id}/{slugYachtName}/{typeFolder}/{slugFileName}.webp"
                                        .Replace("\\", "/");

                    var existing = yacht.Media.Images?
                        .FirstOrDefault(i => i.Filename == image.Filename &&
                                             i.Type == image.Type &&
                                             i.PhotographerName == image.PhotographerName);

                    if (existing != null)
                        existing.Url = publicUrl;
                    else
                    {
                        yacht.Media.Images ??= new List<AlexAPI.Models.Image>();
                        yacht.Media.Images.Add(new AlexAPI.Models.Image
                        {
                            Filename = image.Filename,
                            PhotographerName = image.PhotographerName,
                            Type = image.Type,
                            Url = publicUrl
                        });
                    }

                    await writerDb.SaveChangesAsync();

                    // Detach all entities after save to reduce memory usage
                    foreach (var entry in writerDb.ChangeTracker.Entries())
                        entry.State = EntityState.Detached;

                    uploaded++;
                    Console.WriteLine($"✅ Uploaded {uploaded} (ID {image.Id})");
                }
                catch (Exception ex)
                {
                    failed++;
                    Console.WriteLine($"❌ Failed  (ID {image.Id}) – {ex.Message}");
                }
            }

            return Ok(new
            {
                Message = $"{uploaded} images uploaded & linked.",
                Skipped = skipped,
                Failed = failed,
                TotalProcessed = processed
            });
        }
    }
}
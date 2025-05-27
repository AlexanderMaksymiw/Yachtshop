using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using Microsoft.AspNetCore.Mvc;
using AlexAPI.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly IFTPService ftpService;

        public YachtController(ILogger<YachtController> logger, YachtWorkUnit workUnit, IFTPService ftpService)
        {
            this.logger = logger;
            this.workUnit = workUnit;
            this.ftpService = ftpService;
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
                    LEFT JOIN LocationYacht ly ON y.Id = ly.YachtsId
                    LEFT JOIN Locations l ON ly.LocationsId = l.Id
                    LEFT JOIN KeyFeatures kf ON y.Id = kf.YachtId
                    WHERE y.Id = @Id";

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Image, Amenity, Location, KeyFeature>(
                    sql,
                    (y, s, m, i, a, l, kf) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Amenities = a;
                            yacht.Media.Images = new List<Image>();
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
            string[]? equipment = null,
            bool? onSale = null
        )
        {
            try
            {
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
                    conditions.Add("EXISTS (SELECT 1 FROM YachtLocations yl JOIN Locations l ON yl.LocationId = l.Id WHERE yl.YachtId = y.Id AND l.Name = @Destination)");
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

                if (guests.HasValue)
                {
                    conditions.Add("s.Guests >= @Guests");
                    parameters.Add(new SqlParameter("@Guests", guests.Value));
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
                    conditions.Add("EXISTS (SELECT 1 FROM SpecificationSubTypes sst JOIN SubTypes st ON sst.SubTypeId = st.Id WHERE sst.SpecificationId = s.Id AND st.Name = @SubType)");
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
                        y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    {whereClause}
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
                        y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    ORDER BY y.Id
                    OFFSET @Page ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification>(
                    sql,
                    map: (yacht, spec) =>
                    {
                        yacht.Specification = spec;
                        return yacht;
                    },
                    splitOn: "Id", // split on Specification.Id
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

                var result = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media, Image>(
                    sql,
                    (y, s, m, i) =>
                    {
                        if (!yachtDict.TryGetValue(y.Id, out var yacht))
                        {
                            yacht = y;
                            yacht.Specification = s;
                            yacht.Media = m;
                            yacht.Media.Images = new List<Image>();
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
        public IActionResult GetFeatured(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT y.*, s.*, m.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Media m ON y.MediaId = m.Id
                    WHERE y.IsFeatured = 1
                    ORDER BY y.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var yachts = workUnit.YachtRepository.ExecuteMultiMapQuery<Yacht, Specification, Media>(
                    sql,
                    map: (yacht, s, m) =>
                    {
                        yacht.Specification = s;
                        yacht.Media = m;
                        return yacht;
                    },
                    splitOn: "Id,Id",
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

                return Ok(workUnit.YachtRepository.ExecuteSqlQuery<Yacht>(sql, new {
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

        [HttpGet]
        [Route("GetScubaYachts")]
        public IActionResult GetScubaYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                string sql = @"
                    SELECT DISTINCT y.*, s.*
                    FROM Yachts y
                    LEFT JOIN Specifications s ON y.SpecificationId = s.Id
                    LEFT JOIN Amenities a ON y.AmenitiesId = a.Id
                    LEFT JOIN AmenityEquipment ae ON a.Id = ae.AmenityId
                    LEFT JOIN Equipment e ON ae.EquipmentId = e.Id
                    LEFT JOIN AmenityToys at ON a.Id = at.AmenityId
                    LEFT JOIN Toys t ON at.ToyId = t.Id
                    WHERE e.Name LIKE '%scuba%' OR t.Name LIKE '%scuba%'
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

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("Update")]
        public IActionResult UpdateYacht(Yacht yacht)
        {
            try
            {
                workUnit.YachtRepository.Update(yacht);
                workUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateYacht(Yacht yacht)
        {
            try
            {
                workUnit.YachtRepository.Insert(yacht);
                workUnit.Save();
                return Ok();
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

        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [HttpPost]
        [Route("BulkUpdate")]
        public IActionResult UpdateYachts(Yacht[] yachts)
        {
            try
            {
                Array.ForEach(yachts, yacht =>
                {
                    workUnit.YachtRepository.Update(yacht);
                    workUnit.Save();
                });
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
                    yacht.Media.Images.Add(new Image
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
                    yacht.Media.Images = new List<Image>();
                }
                foreach (var item in imageDtos)
                {
                    yacht.Media.Images.Add(new Image
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
    }
}

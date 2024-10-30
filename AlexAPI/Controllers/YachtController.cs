using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Enums;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.Services.Interfaces;
using AlexAPI.ViewModels;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly ICSVImportService csvImportService;
        private readonly ILlamaService llamaAI;
        private readonly TelemetryClient telemetryClient;

        public YachtController(ILogger<YachtController> logger, IConfiguration configuration, YachtWorkUnit workUnit, ICSVImportService csvImportService, ILlamaService llamaAI, TelemetryClient telemetryClient)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.workUnit = workUnit;
            this.csvImportService = csvImportService;
            this.llamaAI = llamaAI;
            this.telemetryClient = telemetryClient;
        }

        [HttpPost]
        [Route("Get")]
        public IActionResult Get(
            string? name = null,
            string? type = null,
            string? destination = null,
            int? numResults = null,
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
            string[]? equipment = null)
        {
            try
            {
                return Ok(GetYachts(name, type, destination, numResults, minPrice, maxPrice, length, guests, yearBuilt, cabins, maxSpeed, grossTonnage, cruisingSpeed, subType, hullType, builder, equipment));
            }
            catch(Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest();
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
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllYachtNames")]
        public IActionResult GetAllYachtNames()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get().Select(x => new Tuple<Guid, string>(
                    x.Id,
                    x.Name
                )));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetYachtsUnder50K")]
        public IActionResult GetYachtsUnder50K()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard <= 50000));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetYachtsOver50K")]
        public IActionResult GetYachtsOver50K()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard >= 50000));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCharterYachts")]
        public IActionResult GetCharterYachts()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard >= 0));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetMotorYachts")]
        public IActionResult GetMotorYachts()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.Type == "Motor"));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSailingYachts")]
        public IActionResult GetSailingYachts()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.Type == "Sailing"));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCatamarans")]
        public IActionResult GetCatamarans()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Catamaran"));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetGulets")]
        public IActionResult GetGulets()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Gulets")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetExplorers")]
        public IActionResult GetExplorers()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Explorer")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSportFisherman")]
        public IActionResult GetSportFisherman()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Sport Fisherman")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMonoHull")]
        public IActionResult GetMonoHull()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Mono Hull"));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetTrimaran")]
        public IActionResult GetTrimaran()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Trimaran"));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetFlybridge")]
        public IActionResult GetFlybridge()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Flybridge")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSportBoat")]
        public IActionResult GetSportBoat()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Sport Boat")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMaxi")]
        public IActionResult GetMaxi()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Maxi")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetJClass")]
        public IActionResult GetJClass()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "J Class")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMotorSailers")]
        public IActionResult GetMotorSailers()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Motor Sailer")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSupportYachts")]
        public IActionResult GetSupportYachts()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Support Yacht")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetConversion")]
        public IActionResult GetConversion()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Conversion")));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return Ok(workUnit.YachtRepository.GetByID(id));
            }
            catch (Exception ex)
                {
                    telemetryClient.TrackException(ex);
                    return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ImportTitleUrl")]
        public IActionResult ImportTitleUrl(IFormFile file)
        {
            var import = csvImportService.ReadSYTimesCSV(file);
            foreach (var item in import)
            {
                var yachts = workUnit.YachtRepository.Get(y => y.Name == item.Title && y.Specification.Length == ConvertToMeters(item.Length) && y.Specification.Type == item.Yacht_type && y.Specification.YearBuilt == ConvertToInt(item.Year_Built));
                Yacht yacht;
                if(yachts == null)
                {
                    return BadRequest(item);
                }
                else if(yachts.Count() > 1)
                {
                    for (int i = 1; i < yachts.Count(); i++)
                    {
                        workUnit.YachtRepository.Delete(yachts.ElementAt(i));
                    }
                }
                yacht = yachts.First();
                yacht.SYTUrl = item.Title_URL;
                workUnit.YachtRepository.Update(yacht);
            }
            workUnit.Save();
            return Ok();
        }

        [HttpGet]
        [Route("GetMediterraneanYachts")]
        public IActionResult GetMediterraneanYachts()
        {
            try
            {
                List<string> mediterraneanLocations = LocationHelper.MediterraneanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                        location => mediterraneanLocations.Any(
                            medLocation => location.Name.Contains(medLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCaribbeanYachts")]
        public IActionResult GetCaribbeanYachts()
        {
            try
            {
                List<string> caribbeanLocations = LocationHelper.CaribbeanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                        location => caribbeanLocations.Any(
                            caribLocation => location.Name.Contains(caribLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAsiaYachts")]
        public IActionResult GetAsiaYachts()
        {
            try
            {
                List<string> AsiaLocations = LocationHelper.AsiaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => AsiaLocations.Any(
                                AsiaLocation => location.Name.Contains(AsiaLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMiddleEastYachts")]
        public IActionResult GetMiddleEastYachts()
        {
            try
            {
                List<string> MiddleEastLocations = LocationHelper.MiddleEastLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => MiddleEastLocations.Any(
                                MiddleEastLocation => location.Name.Contains(MiddleEastLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetIndianOceanYachts")]
        public IActionResult GetIndianOceanYachts()
        {
            try
            {
                List<string> IndianOceanLocations = LocationHelper.IndianOceanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => IndianOceanLocations.Any(
                                IndianOceanLocation => location.Name.Contains(IndianOceanLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetOceaniaYachts")]
        public IActionResult GetOceaniaYachts()
        {
            try
            {
                List<string> OceaniaLocations = LocationHelper.OceaniaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => OceaniaLocations.Any(
                                OceaniaLocation => location.Name.Contains(OceaniaLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetNorthandSouthAmericaYachts")]
        public IActionResult GetNorthandSouthAmericaYachts()
        {
            try
            {
                List<string> NorthandSouthAmericaLocations = LocationHelper.NorthandSouthAmericaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => NorthandSouthAmericaLocations.Any(
                                NorthandSouthAmericaLocation => location.Name.Contains(NorthandSouthAmericaLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetEuropeanYachts")]
        public IActionResult GetEuropeanYachts()
        {
            try
            {
                List<string> EuropeanLocations = LocationHelper.EuropeanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => EuropeanLocations.Any(
                                EuropeanLocation => location.Name.Contains(EuropeanLocation)
                            )
                        )
                    )
                );
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ImportHullType")]
        public IActionResult ImportHullType(IFormFile file)
        {
            var import = csvImportService.ReadShortSYTimesCSV(file);
            List<string> missingURLs = new List<string>();
            foreach (var item in import)
            {
                var yacht = workUnit.YachtRepository.Get(filter: y => y.SYTUrl == item.Title_URL, includes: y => y.Specification).FirstOrDefault();
                if (yacht != null)
                {
                    var subTypes = ConvertToCleanSubTypes(item.SubType).Select(x => workUnit.SubTypeRepository.Get(st => st.Name == x).FirstOrDefault() ?? new SubType { Name = x }).ToList();
                    yacht.Specification.SubTypes = subTypes;
                    yacht.Specification.HullType = item.HullType;
                    yacht.Specification.Class = item.Class;
                    workUnit.YachtRepository.Update(yacht);
                    workUnit.Save();
                }
                else
                {
                    missingURLs.Add(item.Title_URL);
                }
            }
            return Ok(missingURLs);
        }

        [HttpPost]
        [Route("YachtCharterFleetImport")]
        public IActionResult YachtCharterFleetIngest(IFormFile file)
        {
            var yachtsFromCSV = csvImportService.ReadYachtCharterFleetCSV(file);
            foreach (var csvYacht in yachtsFromCSV)
            {
                var yacht = workUnit.YachtRepository.Get(
                        filter: x => x.Name.ToLower() == csvYacht.Title.ToLower(),
                        includes: y => y.Specification
                    )
                    .Where(x => 
                        Math.Round(x.Specification.Length.Value) + 1 == Math.Round(ConvertToMeters(csvYacht.Length)) 
                        || Math.Round(x.Specification.Length.Value) - 1 == Math.Round(ConvertToMeters(csvYacht.Length)) 
                        || Math.Round(x.Specification.Length.Value) == Math.Round(ConvertToMeters(csvYacht.Length))
                    ).FirstOrDefault();

                List<string> locations = new List<string>();
                AddLocations(csvYacht.Cruising_Regions_Summer, locations);
                AddLocations(csvYacht.Cruising_Regions_Winter, locations);

                locations.ForEach(location =>
                {
                    if (workUnit.LocationRepository.Get(x => x.Name.ToLower() == location.ToLower()).FirstOrDefault() == null)
                    {
                        workUnit.LocationRepository.Insert(new Location
                        {
                            Name = location
                        });
                    }
                });
                workUnit.Save();


                if (yacht != null)
                {
                    yacht.Locations = workUnit.LocationRepository.Get(x => locations.Select(location => location.ToLower()).Contains(x.Name.ToLower())).ToList();
                    yacht.Price.Standard = ConvertToDecimal(csvYacht.Price);
                    yacht.Price.Summer = ConvertToDecimal(csvYacht.Summer_Charter_Rates);
                    yacht.Price.Winter = ConvertToDecimal(csvYacht.Winter_Charter_Rates);

                }
            }
            
            workUnit.Save();
            return Ok(yachtsFromCSV);
        }

        void AddLocations(string regions, List<string> locations)
        {
            regions.Split("\n").ToList().ForEach(x =>
            {
                x = x.Replace(",", "").Trim();
                if (!string.IsNullOrWhiteSpace(x) && x != "Cruising Regions" && x != "HOT SPOTS:" && !locations.Contains(x))
                {
                    locations.Add(x);
                }
            });
        }

        private decimal ConvertToMeters(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            value = value.Trim();

            if (value.EndsWith(" m"))
            {
                if (decimal.TryParse(value.Replace(" m", ""), out var meters))
                    return meters;
            }
            else if (value.EndsWith(" in"))
            {
                if (decimal.TryParse(value.Replace(" in", ""), out var inches))
                    return inches * 0.0254m;
            }
            else if (value.Contains("'"))
            {
                var feetIndex = value.IndexOf('\'');
                var feet = value.Substring(0, feetIndex);
                var inches = value.Substring(feetIndex + 1).Replace(" in", "");

                if (decimal.TryParse(feet, out var feetDecimal) && decimal.TryParse(inches, out var inchesDecimal))
                    return (feetDecimal * 12 + inchesDecimal) * 0.0254m;
            }
            else if (decimal.TryParse(value, out var plainInches))
            {
                return plainInches * 0.0254m;
            }

            return 0;
        }

        private int ConvertToInt(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            value = value.Trim();
            if (value.Equals("N/A", StringComparison.OrdinalIgnoreCase)) return 0;

            if (int.TryParse(value, out var result))
                return result;

            return 0;
        }

        private decimal ConvertToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            value = value.Trim();

            if (value.Equals("N/A", StringComparison.OrdinalIgnoreCase)) return 0;

            var cleanedValue = System.Text.RegularExpressions.Regex.Replace(value, @"[^\d.,]", "");

            cleanedValue = cleanedValue.Replace(",", "");

            if (decimal.TryParse(cleanedValue, out var result))
                return result;

            return 0;
        }


        private List<string> ConvertToCleanSubTypes(string value)
        {
            var result = value.Replace("SUBTYPES", "").Replace("SUBTYPE", "").Trim().Split(", ").Select(x => x.Trim()).ToList();
            result.Remove("");
            return result.Contains("N/A") ? new List<string>() : result;
        }

        private IEnumerable<Yacht> GetYachts(
            string? name = null,
            string? type = null,
            string? destination = null,
            int? numResults = null,
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
            string[]? equipment = null)
        {
            var sqlQuery = new StringBuilder(@"
                SELECT    
	                y.*,
                    s.[Type],
                    s.[HullType],
                    s.[YearBuilt],
                    s.[Builder],
                    s.[Length],
                    s.[Guests],
                    s.[Cabins],
                    s.[Flag],
                    s.[Port],
                    s.[Superstructure],
                    s.[InteriorDesigner],
                    s.[ExteriorDesigner],
                    s.[Crew],
                    s.[Beam],
                    s.[Draft],
                    s.[GrossTonnage],
                    s.[MaxSpeed],
                    s.[CruisingSpeed],
                    s.[EnginePowerOutput],
                    s.[Model],
                    s.[PropulsionType],
                    s.[FuelCapacity],
                    i.[Filename],
                    i.[PhotographerName],
                    i.[Type] AS [ImageType],
                    i.[Url]
                FROM 
	                Yachts y 
            ");

            if (
                name == null &&
                type == null &&
                destination == null &&
                minPrice == null &&
                maxPrice == null &&
                length == null &&
                guests == null &&
                yearBuilt == null &&
                cabins == null &&
                maxSpeed == null &&
                grossTonnage == null &&
                grossTonnage == null &&
                cruisingSpeed == null &&
                subType == null &&
                builder == null &&
                equipment == null
            )
            {
                sqlQuery.Append(@"
                    INNER JOIN 
	                    (SELECT TOP(@numResults) * FROM Yachts) featuredYachts ON featuredYachts.Id = y.Id 
                ");
            }

            sqlQuery.Append(@"
                LEFT JOIN 
                    Specifications s ON y.SpecificationId = s.Id
                LEFT JOIN 
                    Media m ON y.MediaId = m.Id
                LEFT JOIN 
                    Images i ON i.MediaId = m.Id
                LEFT JOIN 
                    Prices p ON y.PriceId = p.Id
                LEFT JOIN 
                    LocationYacht ly ON y.Id = ly.YachtsId
                LEFT JOIN 
                    Locations l ON ly.LocationsId = l.Id
                WHERE 1=1
            ");

            // List to hold SQL parameters
            var parameters = new List<SqlParameter>([new SqlParameter("@numResults", numResults ?? int.MaxValue)]);

            // Append conditions based on parameters
            if (!string.IsNullOrEmpty(name))
            {
                sqlQuery.Append(" AND y.Name LIKE @name");
                parameters.Add(new SqlParameter("@name", $"%{name}%"));
            }

            if (!string.IsNullOrEmpty(type))
            {
                sqlQuery.Append(" AND s.Type = @type");
                parameters.Add(new SqlParameter("@type", type));
            }

            if (!string.IsNullOrEmpty(destination))
            {
                sqlQuery.Append(" AND l.Name = @destination");
                parameters.Add(new SqlParameter("@destination", destination));
            }

            if (minPrice.HasValue)
            {
                sqlQuery.Append(" AND p.Standard >= @minPrice");
                parameters.Add(new SqlParameter("@minPrice", minPrice));
            }

            if (maxPrice.HasValue)
            {
                sqlQuery.Append(" AND p.Standard <= @maxPrice");
                parameters.Add(new SqlParameter("@maxPrice", maxPrice));
            }

            if (length.HasValue)
            {
                sqlQuery.Append(" AND s.Length >= @length");
                parameters.Add(new SqlParameter("@length", length));
            }


            if (guests.HasValue)
            {
                sqlQuery.Append(" AND s.Guests >= @guests");
                parameters.Add(new SqlParameter("@guests", guests));
            }

            if (yearBuilt.HasValue)
            {
                sqlQuery.Append(" AND s.YearBuilt >= @yearBuilt");
                parameters.Add(new SqlParameter("@yearBuilt", yearBuilt));
            }

            if (cabins.HasValue)
            {
                sqlQuery.Append(" AND s.Cabins >= @cabins");
                parameters.Add(new SqlParameter("@cabins", cabins));
            }

            if (maxSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.MaxSpeed >= @maxSpeed");
                parameters.Add(new SqlParameter("@maxSpeed", maxSpeed));
            }

            if (grossTonnage.HasValue)
            {
                sqlQuery.Append(" AND s.GrossTonnage >= @grossTonnage");
                parameters.Add(new SqlParameter("@grossTonnage", grossTonnage));
            }

            if (cruisingSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.CruisingSpeed >= @cruisingSpeed");
                parameters.Add(new SqlParameter("@cruisingSpeed", cruisingSpeed));
            }

            if (!string.IsNullOrEmpty(subType))
            {
                sqlQuery.Append(" AND s.SubType = @subType");
                parameters.Add(new SqlParameter("@subType", subType));
            }

            if (!string.IsNullOrEmpty(builder))
            {
                sqlQuery.Append(" AND s.Builder = @builder");
                parameters.Add(new SqlParameter("@builder", builder));
            }

            if (equipment != null && equipment.Length > 0)
            {
                var equipmentConditions = string.Join(" OR ", equipment.Select((e, i) => $"y.Equipment LIKE @equipment{i}"));
                sqlQuery.Append($" AND ({equipmentConditions})");
                parameters.AddRange(equipment.Select((e, i) => new SqlParameter($"@equipment{i}", $"%{e}%")));
            }

            // Execute the query and get the results using raw SQL
            var yachtDtos = workUnit.YachtRepository.ExecuteSqlQuery<YachtDto>(sqlQuery.ToString(), parameters.ToArray());
            // Group and map the results
            return yachtDtos.GroupBy(y => y.Id)
                .Select(group => new Yacht
                {
                    Id = group.First().Id,
                    Name = group.First().Name,
                    Specification = new Specification
                    {
                        Type = group.First().Type,
                        HullType = group.First().HullType,
                        YearBuilt = group.First().YearBuilt,
                        Builder = group.First().Builder,
                        Length = group.First().Length,
                        Guests = group.First().Guests,
                        Cabins = group.First().Cabins,
                        Flag = group.First().Flag,
                        Port = group.First().Port,
                        Superstructure = group.First().Superstructure,
                        InteriorDesigner = group.First().InteriorDesigner,
                        ExteriorDesigner = group.First().ExteriorDesigner,
                        Crew = group.First().Crew,
                        Beam = group.First().Beam,
                        Draft = group.First().Draft,
                        GrossTonnage = group.First().GrossTonnage,
                        MaxSpeed = group.First().MaxSpeed,
                        CruisingSpeed = group.First().CruisingSpeed,
                        EnginePowerOutput = group.First().EnginePowerOutput,
                        Model = group.First().Model,
                        PropulsionType = group.First().PropulsionType,
                        FuelCapacity = group.First().FuelCapacity
                    },
                    Media = new Media
                    {
                        Images = group.Select(y => new Image
                        {
                            Filename = y.Filename,
                            PhotographerName = y.PhotographerName,
                            Type = (ImageTypeEnum)y.ImageType,
                            Url = y.Url
                        }).ToList()
                    }
                });
        }
    }
}

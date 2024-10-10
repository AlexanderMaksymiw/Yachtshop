using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AlexAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using AlexAPI.Enums;
using AlexAPI.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text;
using AlexAPI.ViewModels;
using Microsoft.ApplicationInsights;

namespace AlexAPI.Controllers
{
    [Route("api/[controller]")]
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
            string? builder = null,
            string[]? equipment = null)
        {
            try
            {
                var sqlQuery = new StringBuilder(@"
                    SELECT    
	                    y.*,
                        s.[Type],
                        s.[SubType],
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
                return Ok(yachtDtos.GroupBy(y => y.Id)
                    .Select(group => new Yacht
                    {
                        Id = group.First().Id,
                        Name = group.First().Name,
                        Specification = new Specification
                        {
                            Type = group.First().Type,
                            SubType = group.First().SubType,
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
                    }));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Price.Standard <= 50000));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Price.Standard >= 50000));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Price.Standard >= 0));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.Type == "Motor"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.Type == "Sailing"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.HullType == "Catamaran"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Gulets"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Explorer"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Sport Fisherman"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.HullType == "Mono Hull"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.HullType == "Trimaran"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Flybridge"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Sport Boat"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Maxi"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "J Class"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Motor Sailer"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Support Yacht"));
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
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Specification.SubType == "Conversion"));
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

        [HttpGet]
        [Route("GetMediterraneanYachts")]
        public IActionResult GetMediterraneanYachts()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get().Where(yacht => yacht.Price.Standard >= 0));
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                return BadRequest(ex);
            }
        }


        /*
        //TODO: Delete me!
        [HttpPost]
        [Route("CharterWorldImport")]
        public IActionResult CharterWorldImport(IFormFile file)
        {
            var yachtDetails = csvImportService.ReadCWYachtsCSV(file);
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            foreach (var r in yachtDetails)
            {
                var locationsText = r.Text.ToLower().Split('\n').FirstOrDefault(x => x.StartsWith("locations:"));
                if (locationsText == null) continue;

                var locations = locationsText.Replace("locations:", "").Split(", ").ToList();
                var yacht = workUnit.YachtRepository.Get(y => y.Name.ToLower() == r.caption.ToLower()).FirstOrDefault();

                if (yacht != null)
                {
                    if (yacht.Locations == null)
                    {
                        yacht.Locations = new List<Location>();
                    }

                    foreach (var location in locations)
                    {
                        yacht.Locations.Add(new Location
                        {
                            Name = textInfo.ToTitleCase(location).Replace("Us ", "US ").Replace("Bvi", "BVI"),
                        });
                    }
                }
            }

            workUnit.Save();
            return Ok(yachtDetails);
        }

        //TODO: Delete me!
        [HttpPost]
        [Route("YachtCharterFleetImport")]
        public IActionResult YachtCharterFleetIngest(IFormFile file)
        {
            var yachtsFromCSV = csvImportService.ReadYachtCharterFleetCSV(file);
            foreach (var csvYacht in yachtsFromCSV)
            {
                var yacht = workUnit.YachtRepository.Get(x => x.Name.ToLower() == csvYacht.Title.ToLower()).FirstOrDefault();
                List<string> locations = new List<string>();
                csvYacht.Cruising_Regions_Summer.Split("\n").ToList().ForEach(x =>
                {
                    x = x.Replace(",", "").Trim();
                    if (x != "" && x != "Cruising Regions" && x != "HOT SPOTS:" && !locations.Contains(x))
                    {
                        locations.Add(x);
                    }
                });
                csvYacht.Cruising_Regions_Winter.Split("\n").ToList().ForEach(x =>
                {
                    x = x.Replace(",", "").Trim();
                    if (x.Replace(" ", "") != "" && x.Replace(" ", "") != "Cruising Regions" && x.Replace(" ", "") != "HOT SPOTS:" && !locations.Contains(x))
                    {
                        locations.Add(x);
                    }
                });
                if (yacht == null)
                {
                    workUnit.YachtRepository.Insert(new Yacht
                    {
                        Name = csvYacht.Title,
                        Locations = locations.Select(x => new Location { Name = x }).ToList(),
                        Brochure = new YachtBrochure
                        {
                            Auto = new Auto
                            {
                                Equipment = csvYacht.Amenities_Entertainment.IsNullOrEmpty() ? "" : csvYacht.Amenities_Entertainment,
                            },
                            Specifications = new Specifications
                            {
                                Length = csvYacht.Crew.IsNullOrEmpty() ? "" : csvYacht.Length,
                                Beam = csvYacht.Beam.IsNullOrEmpty() ? "" : csvYacht.Beam,
                                Draft = csvYacht.Draft.IsNullOrEmpty() ? "" : csvYacht.Draft,
                                GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Guests),
                                GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? "" : csvYacht.Gross_Tonnage,
                                CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? 0 : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]),
                                YearBuilt = csvYacht.Built.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Built),
                                Builder = csvYacht.Builder.IsNullOrEmpty() ? "" : csvYacht.Builder,
                                Model = csvYacht.Model.IsNullOrEmpty() ? "" : csvYacht.Model,
                                ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? "" : csvYacht.Exterior_Designer,
                                InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? "" : csvYacht.Interior_Designer,
                                Toys = csvYacht.Toys.IsNullOrEmpty() ? "" : csvYacht.Toys,
                            }
                        },
                        Detail = new YachtDetail
                        {
                            TotalCrew = csvYacht.Crew.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Crew),
                            Price = csvYacht.Price.IsNullOrEmpty() ? "" : csvYacht.Price,
                            SummerRates = csvYacht.Summer_Charter_Rates.IsNullOrEmpty() ? "" : csvYacht.Summer_Charter_Rates.Split("\n")[0],
                            WinterRates = csvYacht.Winter_Charter_Rates.IsNullOrEmpty() ? "" : csvYacht.Winter_Charter_Rates.Split("\n")[0],
                            AwardNominations = csvYacht.Awards_Nominations.IsNullOrEmpty() ? "" :csvYacht.Awards_Nominations,
                            Cabins = csvYacht.Cabins.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Cabins)
                        }
                    });
                }
                else
                {
                    yacht.Locations = locations.Select(x => new Location { Name = x }).ToList();

                    if(yacht.Brochure == null)
                    {
                        yacht.Brochure = new YachtBrochure
                        {
                            Auto = new Auto
                            {
                                Equipment = csvYacht.Amenities_Entertainment.IsNullOrEmpty() ? "" : csvYacht.Amenities_Entertainment
                            },
                            Specifications = new Specifications
                            {
                                Length = csvYacht.Crew.IsNullOrEmpty() ? "" : csvYacht.Length,
                                Beam = csvYacht.Beam.IsNullOrEmpty() ? "" : csvYacht.Beam,
                                Draft = csvYacht.Draft.IsNullOrEmpty() ? "" : csvYacht.Draft,
                                GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Guests),
                                GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? "" : csvYacht.Gross_Tonnage,
                                CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? 0 : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]),
                                YearBuilt = csvYacht.Built.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Built),
                                Builder = csvYacht.Builder.IsNullOrEmpty() ? "" : csvYacht.Builder,
                                Model = csvYacht.Model.IsNullOrEmpty() ? "" : csvYacht.Model,
                                ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? "" : csvYacht.Exterior_Designer,
                                InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? "" : csvYacht.Interior_Designer,
                                Toys = csvYacht.Toys.IsNullOrEmpty() ? "" : csvYacht.Toys,
                            }
                        };
                    }
                    else
                    {
                        if(yacht.Brochure.Auto == null)
                        {
                            yacht.Brochure.Auto = new Auto
                            {
                                Equipment = csvYacht.Amenities_Entertainment.IsNullOrEmpty() ? "" : csvYacht.Amenities_Entertainment
                            };
                        }
                        else
                        {
                            yacht.Brochure.Auto.Equipment = csvYacht.Amenities_Entertainment.IsNullOrEmpty() ? yacht.Brochure.Auto.Equipment : csvYacht.Amenities_Entertainment;
                        }
                        if(yacht.Specification == null)
                        {
                            yacht.Specification = new Specifications
                            {
                                Length = csvYacht.Crew.IsNullOrEmpty() ? "" : csvYacht.Length,
                                Beam = csvYacht.Beam.IsNullOrEmpty() ? "" : csvYacht.Beam,
                                Draft = csvYacht.Draft.IsNullOrEmpty() ? "" : csvYacht.Draft,
                                GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Guests),
                                GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? "" : csvYacht.Gross_Tonnage,
                                CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? 0 : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]),
                                YearBuilt = csvYacht.Built.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Built),
                                Builder = csvYacht.Builder.IsNullOrEmpty() ? "" : csvYacht.Builder,
                                Model = csvYacht.Model.IsNullOrEmpty() ? "" : csvYacht.Model,
                                ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? "" : csvYacht.Exterior_Designer,
                                InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? "" : csvYacht.Interior_Designer,
                                Toys = csvYacht.Toys.IsNullOrEmpty() ? "" : csvYacht.Toys,
                            };
                        }
                        else
                        {
                            yacht.Specification.Length = csvYacht.Crew.IsNullOrEmpty() ? yacht.Specification.Length : csvYacht.Length;
                            yacht.Specification.Beam = csvYacht.Beam.IsNullOrEmpty() ? yacht.Specification.Beam : csvYacht.Beam;
                            yacht.Specification.Draft = csvYacht.Draft.IsNullOrEmpty() ? yacht.Specification.Draft : csvYacht.Draft;
                            yacht.Specification.GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? yacht.Specification.GuestsCruising : int.Parse(csvYacht.Guests);
                            yacht.Specification.GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? yacht.Specification.GrossTonnage : csvYacht.Gross_Tonnage;
                            yacht.Specification.CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? yacht.Specification.CruisingSpeed : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]);
                            yacht.Specification.YearBuilt = csvYacht.Built.IsNullOrEmpty() ? yacht.Specification.YearBuilt : int.Parse(csvYacht.Built);
                            yacht.Specification.Builder = csvYacht.Builder.IsNullOrEmpty() ? yacht.Specification.Builder : csvYacht.Builder;
                            yacht.Specification.Model = csvYacht.Model.IsNullOrEmpty() ? yacht.Specification.Model : csvYacht.Model;
                            yacht.Specification.ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? yacht.Specification.ExteriorDesigner : csvYacht.Exterior_Designer;
                            yacht.Specification.InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? yacht.Specification.InteriorDesigner : csvYacht.Interior_Designer;
                            yacht.Specification.Toys = csvYacht.Toys.IsNullOrEmpty() ? yacht.Specification.Toys : csvYacht.Toys;
                        }
                    }
                    if(yacht.Detail == null)
                    {
                        yacht.Detail = new YachtDetail
                        {
                            TotalCrew = csvYacht.Crew.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Crew),
                            Price = csvYacht.Price.IsNullOrEmpty() ? "" : csvYacht.Price,
                            SummerRates = csvYacht.Summer_Charter_Rates.IsNullOrEmpty() ? "" : csvYacht.Summer_Charter_Rates.Split("\n")[0],
                            WinterRates = csvYacht.Winter_Charter_Rates.IsNullOrEmpty() ? "" : csvYacht.Winter_Charter_Rates.Split("\n")[0],
                            AwardNominations = csvYacht.Awards_Nominations.IsNullOrEmpty() ? "" : csvYacht.Awards_Nominations,
                            Cabins = csvYacht.Cabins.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Cabins),
                        };
                    }
                    else
                    {
                        yacht.Detail.TotalCrew = csvYacht.Crew.IsNullOrEmpty() ? yacht.Detail.TotalCrew : int.Parse(csvYacht.Crew);
                        yacht.Detail.Price = csvYacht.Price.IsNullOrEmpty() ? yacht.Detail.Price : csvYacht.Price;
                        yacht.Detail.SummerRates = csvYacht.Summer_Charter_Rates.IsNullOrEmpty() ? yacht.Detail.SummerRates : csvYacht.Summer_Charter_Rates.Split("\n")[0];
                        yacht.Detail.WinterRates = csvYacht.Winter_Charter_Rates.IsNullOrEmpty() ? yacht.Detail.WinterRates : csvYacht.Winter_Charter_Rates.Split("\n")[0];
                        yacht.Detail.AwardNominations = csvYacht.Awards_Nominations.IsNullOrEmpty() ? yacht.Detail.AwardNominations : csvYacht.Awards_Nominations;
                        yacht.Detail.Cabins = csvYacht.Cabins.IsNullOrEmpty() ? 0 : int.Parse(csvYacht.Cabins);
                    }
                    workUnit.YachtRepository.Update(yacht);
                }
            }
            workUnit.Save();
            return Ok(yachtsFromCSV);
        }
        */
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

            var parts = value.Split(' ');
            if (decimal.TryParse(parts[0], out var result))
                return result;

            return 0;
        }
    }
}

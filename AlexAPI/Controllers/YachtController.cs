using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Enums;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.ResponseModels;
using AlexAPI.Services;
using AlexAPI.Services.Interfaces;
using AlexAPI.Services.Models;
using AlexAPI.ViewModels;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly ICSVService csvService;
        private readonly IOpenAIService openAIService;

        public YachtController(ILogger<YachtController> logger, IConfiguration configuration, YachtWorkUnit workUnit, ICSVService csvService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.workUnit = workUnit;
            this.csvService = csvService;
            this.openAIService = new OpenAIService();
        }

        [HttpPost]
        [Route("Get")]
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
                var includes = new Expression<Func<Yacht, object>>[]
                {
                    x => x.Specification,
                    x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
                };

                // Build filter dynamically
                Expression<Func<Yacht, bool>> filter = x =>
                    (name == null || x.Name.ToLower().Contains(name.ToLower())) &&
                    (type == null || x.Specification.Type == type) &&
                    (destination == null || x.Locations.Any(l => l.Name == destination)) &&
                    (minPrice == null || x.Price.Standard >= minPrice) &&
                    (maxPrice == null || x.Price.Standard <= maxPrice) &&
                    (length == null || x.Specification.Length >= length) &&
                    (guests == null || x.Specification.Guests >= guests) &&
                    (yearBuilt == null || x.Specification.YearBuilt >= yearBuilt) &&
                    (cabins == null || x.Specification.Cabins >= cabins) &&
                    (maxSpeed == null || x.Specification.MaxSpeed >= maxSpeed) &&
                    (grossTonnage == null || x.Specification.GrossTonnage >= grossTonnage) &&
                    (cruisingSpeed == null || x.Specification.CruisingSpeed >= cruisingSpeed) &&
                    (subType == null || x.Specification.SubTypes.Select(a => a.Name).Contains(subType)) &&
                    (hullType == null || x.Specification.HullType == hullType) &&
                    (builder == null || x.Specification.Builder == builder) &&
                    (equipment == null || equipment.All(e => x.Amenities.Equipment.Select(a => a.Name).Contains(e)));

                // Apply pagination and execute query
                var result = workUnit.YachtRepository
                    .Get(filter: filter, includes: includes)
                    .Skip(page * numResults)
                    .Take(numResults)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard <= 50000).Skip(page * 25).Take(numResults));
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard >= 50000).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCharterYachts")]
        public IActionResult GetCharterYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price.Standard >= 0).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetMotorYachts")]
        public IActionResult GetMotorYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.Type == "Motor").Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSailingYachts")]
        public IActionResult GetSailingYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.Type == "Sailing").Skip(page * 25).Take(numResults));
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Catamaran").Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetGulets")]
        public IActionResult GetGulets(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Gulets")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetExplorers")]
        public IActionResult GetExplorers(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Explorer")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSportFisherman")]
        public IActionResult GetSportFisherman(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Sport Fisherman")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMonoHull")]
        public IActionResult GetMonoHull(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Mono Hull").Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetTrimaran")]
        public IActionResult GetTrimaran(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.HullType == "Trimaran").Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetFlybridge")]
        public IActionResult GetFlybridge(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Flybridge")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSportBoat")]
        public IActionResult GetSportBoat(

            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Sport Boat")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMaxi")]
        public IActionResult GetMaxi(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Maxi")).Skip(page * numResults).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetJClass")]
        public IActionResult GetJClass(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "J Class")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetMotorSailers")]
        public IActionResult GetMotorSailers(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Motor Sailer")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetSupportYachts")]
        public IActionResult GetSupportYachts(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Support Yacht")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetConversion")]
        public IActionResult GetConversion(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Specification.SubTypes!.Any(t => t.Name == "Conversion")).Skip(page * 25).Take(numResults));
            }
            catch (Exception ex)
            {
                
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
                
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ImportTitleUrl")]
        public IActionResult ImportTitleUrl(IFormFile file)
        {
            var import = csvService.ReadSYTimesCSV(file);
            foreach (var item in import)
            {
                var yachts = workUnit.YachtRepository.Get(y => y.Name == item.Title && y.Specification.Length == ConvertToMeters(item.Length) && y.Specification.Type == item.Yacht_type && y.Specification.YearBuilt == ConvertToInt(item.Year_Built));
                Yacht yacht;
                if (yachts == null)
                {
                    return BadRequest(item);
                }
                else if (yachts.Count() > 1)
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
        public IActionResult GetMediterraneanYachts(
            int page = 0,
            int numResults = 25
        )
        {
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> mediterraneanLocations = LocationHelper.MediterraneanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                        location => mediterraneanLocations.Any(
                            medLocation => location.Name.Contains(medLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> caribbeanLocations = LocationHelper.CaribbeanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                        location => caribbeanLocations.Any(
                            caribLocation => location.Name.Contains(caribLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> AsiaLocations = LocationHelper.AsiaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => AsiaLocations.Any(
                                AsiaLocation => location.Name.Contains(AsiaLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> MiddleEastLocations = LocationHelper.MiddleEastLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => MiddleEastLocations.Any(
                                MiddleEastLocation => location.Name.Contains(MiddleEastLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> IndianOceanLocations = LocationHelper.IndianOceanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => IndianOceanLocations.Any(
                                IndianOceanLocation => location.Name.Contains(IndianOceanLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> OceaniaLocations = LocationHelper.OceaniaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => OceaniaLocations.Any(
                                OceaniaLocation => location.Name.Contains(OceaniaLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
                List<string> NorthAmericaLocations = LocationHelper.NorthAmericaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => NorthAmericaLocations.Any(
                                NorthAmericaLocation => location.Name.Contains(NorthAmericaLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> SouthAmericaLocations = LocationHelper.SouthAmericaLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => SouthAmericaLocations.Any(
                                SouthAmericaLocation => location.Name.Contains(SouthAmericaLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
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
            var includes = new Expression<Func<Yacht, object>>[]
            {
                x => x.Specification,
                x => x.Locations, x => x.Media, x => x.Awards, x => x.Amenities, x => x.Price,
            };

            try
            {
                List<string> EuropeanLocations = LocationHelper.EuropeanLocations;
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Locations.Any(
                            location => EuropeanLocations.Any(
                                EuropeanLocation => location.Name.Contains(EuropeanLocation)
                            )
                        )
                    ).Skip(page * 25).Take(numResults)
                );
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ImportHullType")]
        public IActionResult ImportHullType(IFormFile file)
        {
            var import = csvService.ReadShortSYTimesCSV(file);
            List<string> missingURLs = new List<string>();
            foreach (var item in import)
            {
                var yacht = workUnit.YachtRepository.Get(filter: y => y.SYTUrl == item.Title_URL, includes: [y => y.Specification]).FirstOrDefault();
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
            var yachtsFromCSV = csvService.ReadYachtCharterFleetCSV(file);
            foreach (var csvYacht in yachtsFromCSV)
            {
                var yacht = workUnit.YachtRepository.Get(
                        filter: x => x.Name.ToLower() == csvYacht.Title.ToLower(),
                        includes: [y => y.Specification]
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
                    if (yacht.Locations != workUnit.LocationRepository.Get(x => locations.Select(location => location.ToLower()).Contains(x.Name.ToLower())).ToList())
                    {
                        yacht.Locations = workUnit.LocationRepository.Get(x => locations.Select(location => location.ToLower()).Contains(x.Name.ToLower())).ToList();
                    }
                    yacht.Price.Standard = ConvertToDecimal(csvYacht.Price);
                    yacht.Price.Summer = ConvertToDecimal(csvYacht.Summer_Charter_Rates);
                    yacht.Price.Winter = ConvertToDecimal(csvYacht.Winter_Charter_Rates);

                }
            }

            workUnit.Save();
            return Ok(yachtsFromCSV);
        }

        [HttpPost]
        [Route("GenerateCMSCSV")]
        public async Task<IActionResult> GenerateCMSCSV(string path, IFormFile file)
        {
            try
            {
                var header = csvService.GetHeader(file);
                var importCSV = csvService.ReadCMSCSV(file);
                var rows = new List<List<string>>();
                foreach (var importRow in importCSV)
                {
                    var questions = new string[]{
                        importRow.AccordionQ1 = $"How much does it cost to charter a yacht in {importRow.Title}?",
                        importRow.AccordionQ2 = $"Timing & Weather: {importRow.Title} Yachting Season",
                        importRow.AccordionQ3 = $"What Medical and Health Considerations should be made in {importRow.Title}?",
                        importRow.AccordionQ4 = $"What are the Languages Spoken in {importRow.Title}?",
                        importRow.AccordionQ5CL = $"What are the must see locations when chartering a yacht in {importRow.Title}?",
                    };

                    int counter = 0;

                    foreach (var question in questions)
                    {
                        var answer = await openAIService.GetResponseAsync(question);
                        switch (counter)
                        {
                            case 0:
                                importRow.AccordionQ1 = question;
                                importRow.Q1Answer = answer;
                                break;
                            case 1:
                                importRow.AccordionQ2 = question;
                                importRow.Q2Answer = answer;
                                break;
                            case 2:
                                importRow.AccordionQ3 = question;
                                importRow.Q3Answer = answer;
                                break;
                            case 3:
                                importRow.AccordionQ4 = question;
                                importRow.Q4Answer = answer;
                                break;
                            case 4:
                                importRow.AccordionQ5CL = question;
                                importRow.Q5Answer = answer;
                                break;
                        }
                        counter++;
                    }
                    rows.Add(ConvertRowToStringList(importRow));
                }
                csvService.CreateCSV(path, header, rows);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private void AddLocations(string regions, List<string> locations)
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

            // Regular expression to find the first numeric sequence that represents a price
            var match = System.Text.RegularExpressions.Regex.Match(value, @"\b\d{1,3}(?:[.,]\d{3})*(?:[.,]\d{1,2})?\b");

            if (match.Success)
            {
                // Get the matched price string and remove any commas
                var cleanedValue = match.Value.Replace(",", "");

                // Try parsing the cleaned value to decimal
                if (decimal.TryParse(cleanedValue, out var result))
                    return result;
            }

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

        private List<string> ConvertRowToStringList(CMSCSV row)
        {
            return new List<string>{
                    row.YachtCharterDestinations,
                    row.Title,
                    row.TagLine,
                    row.Region,
                    row.LocationTags,
                    row.CollapseText,
                    row.ReasonsToVisit,
                    row.GoodFor,
                    row.MustSeeLocationsTitle,
                    row.LocationsPara1,
                    row.LocationsPara2,
                    row.HeroImage,
                    row.YachtCharterDestinationsItem,
                    row.YachtCharterDestinationsList,
                    row.CharterDestinationsItem,
                    row.TheMediterraneanItem,
                    row.AccordionQ1,
                    row.Q1Answer,
                    row.Q1Image,
                    row.AccordionQ2,
                    row.Q2Answer,
                    row.Q2Image,
                    row.AccordionQ3,
                    row.Q3Answer,
                    row.Q3Image,
                    row.AccordionQ4,
                    row.Q4Answer,
                    row.Q4Image,
                    row.AccordionQ5CL,
                    row.Q5Answer,
                    row.Q5Image,
                    row.Accordion2L1,
                    row.Accordion2L1A,
                    row.Accordion2L1I,
                    row.Accordion2L2,
                    row.Accordion2L2A,
                    row.Accordion2L2I,
                    row.Accordion2L3,
                    row.Accordion2L3A,
                    row.Accordion2L3I,
                    row.Accordion2L4,
                    row.Accordion2L4A,
                    row.Accordion2L4I,
                    row.Accordion2L5,
                    row.Accordion2L5A,
                    row.Accordion2L5I,
                    row.Accordion2L6,
                    row.Accordion2L6A,
                    row.Accordion2L6I,
                    row.Accordion2L7,
                    row.Accordion2L7A,
                    row.Accordion2L7I,
                    row.Accordion2L8,
                    row.Accordion2L8A,
                    row.Accordion2L8I,
                    row.Accordion2L9,
                    row.Accordion2L9A,
                    row.Accordion2L9I,
                    row.LocationActivitiesTitle,
                    row.CAP1,
                    row.CAP2,
                    row.LocationItineraries,
                    row.ID,
                    row.CreatedDate,
                    row.UpdatedDate,
                    row.Owner,
                    row.LIP1,
                    row.LIP2,
                    row.NewsLocationTitle,
                    row.LNP1,
                    row.LocationEventsTitle,
                    row.LEP1,
                    row.LEP2,
                    row.YachtsInLocationTitle,
                    row.LYP1,
                    row.LYP2,
                };
        }
    }
}

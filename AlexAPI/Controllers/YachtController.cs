using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.Services.Interfaces;
using AlexAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly ICSVService csvService;
        private readonly IOpenAIService openAIService;

        public YachtController(ILogger<YachtController> logger, YachtWorkUnit workUnit, ICSVService csvService, IOpenAIService openAIService)
        {
            this.logger = logger;
            this.workUnit = workUnit;
            this.csvService = csvService;
            this.openAIService = openAIService;
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                string[] resultOrder = [
                    "1st Place",
                    "Winner",
                    "Joint Winner",
                    "2nd Place",
                    "3rd Place",
                    "Finalist",
                    "Judges' Special Award",
                    "Special Commendation",
                    "Nomination",
                    "NULL",
                ];
                var yacht = workUnit.YachtRepository.GetByID(id);
                yacht.Awards = yacht.Awards
                    .OrderBy(x => Array.IndexOf(resultOrder, x.Result))
                    .ToList();
                return Ok(yacht);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
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
                    x => x.Locations,
                    x => x.Media,
                    x => x.Price,
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
                    .Take(numResults);

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

        [HttpGet]
        [Route("PopulateDescriptions")]
        public async Task<IActionResult> PopulateDescriptions()
        {
            try
            {
                var includes = new Expression<Func<Yacht, object>>[]
                {
                    x => x.Specification,
                    x => x.Specification.SubTypes,
                    x => x.Amenities.Equipment,
                    x => x.Amenities.Toys,
                };

                Expression<Func<Yacht, bool>> filter = x => string.IsNullOrEmpty(x.Description);

                var allYachts = workUnit.YachtRepository.Get(filter: filter, includes: includes);
                for (int x = 0; x <= allYachts.Count() / 100; x++)
                {
                    logger.Log(LogLevel.Information, "---------------------------------------------------------------------");
                    logger.Log(LogLevel.Information, $"x: {x}");
                    var yachts = allYachts.Skip(x*100).Take(100);
                    for (int i = 0; i < yachts.Count(); i++)
                    {
                        var yacht = yachts.ElementAt(i);
                        logger.Log(LogLevel.Information, yacht.Name);
                        var prompt = $"Given the following information, write a 500 word summary about the following yacht, complete with headings and paragraphs:\n" +
                            $"Name: {yacht.Name}\n" +
                            $"Type: {yacht.Specification.Type}\n" +
                            $"Sub Type: {string.Join(", ", yacht.Specification.SubTypes.Select(x => x.Name))}\n" +
                            $"Cabins: {yacht.Specification.Cabins}\n" +
                            $"Interior designer: {yacht.Specification.InteriorDesigner}\n" +
                            $"Builder: {yacht.Specification.Builder}\n" +
                            $"Toys: {string.Join(", ", yacht.Amenities.Toys.Select(x => x.Name))}\n" +
                            $"Equipment: {string.Join(", ", yacht.Amenities.Equipment.Select(x => x.Name))}\n";
                        yacht.Description = await openAIService.GetResponseAsync(prompt);
                    }
                    workUnit.Save();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
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
    }
}

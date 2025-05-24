using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Library.Locations;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using FluentFTP;
using AlexAPI.Services.Interfaces;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
        [Route("GetByIds")]
        public IActionResult GetByIds(Guid[] ids)
        {
            try
            {
                var includes = new Expression<Func<Yacht, object>>[]
                {
                    x => x.Specification,
                    x => x.Locations,
                    x => x.Media,
                };

                // Build filter dynamically
                Expression<Func<Yacht, bool>> filter = x => ids.Contains(x.Id);

                var yacht = workUnit.YachtRepository.Get(filter: filter, includes: includes);
                return Ok(yacht);
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
                var includes = new Expression<Func<Yacht, object>>[]
                {
            x => x.Specification,
            x => x.Locations,
            x => x.Media,
            x => x.Amenities  // needed if you filter by equipment
                };

                Expression<Func<Yacht, bool>> filter = x =>
                    (name == null || x.Name.ToLower().Contains(name.ToLower())) &&
                    (type == null || x.Specification.Type == type) &&
                    (destination == null || x.Locations.Any(l => l.Name == destination)) &&
                    (minPrice == null || x.Price >= minPrice) &&
                    (maxPrice == null || x.Price <= maxPrice) &&
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
                    (equipment == null || equipment.All(e => x.Amenities.Equipment.Select(a => a.Name).Contains(e))) &&
                    (onSale == null || x.OnSale == onSale);

                // Fetch, page, and project into the lightweight DTO
                var slimResult = workUnit.YachtRepository
                    .Get(filter: filter, includes: includes)
                    .Skip(page * numResults)
                    .Take(numResults)
                    .Select(y => new YachtDto
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Type = y.Specification.Type,
                        Price = y.Price,
                        Length = y.Specification.Length,
                        Guests = y.Specification.Guests,
                        HeroImageUrl = y.HeroImageUrl,
                        OnSale = y.OnSale
                    })
                    .ToList();

                return Ok(slimResult);
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
                var yachts = workUnit.YachtRepository
                    .Get() // no filter, or maybe just simple filter like OnSale if needed
                    .Skip(page * numResults)
                    .Take(numResults)
                    .Select(y => new
                    {
                        y.Id,
                        y.Name,
                        y.OnSale,
                        y.Price,
                        Length = y.Specification.Length,
                        Guests = y.Specification.Guests,
                        HeroImageUrl = y.HeroImageUrl

                    })
                    .ToList();

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
                return Ok(workUnit.YachtRepository.GetByID(id).Media.Images?.First(x => x.Type == 0));
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price <= 50000).Skip(page * 25).Take(numResults));
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.Price >= 50000).Skip(page * 25).Take(numResults));
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
                var includes = new Expression<Func<Yacht, object>>[]
                {
                    x => x.Specification,
                    x => x.Locations,
                    x => x.Media,
                };

                // Build filter dynamically
                Expression<Func<Yacht, bool>> filter = x =>
                    (name == null || x.Name.ToLower().Contains(name.ToLower())) &&
                    (type == null || x.Specification.Type == type) &&
                    (destination == null || x.Locations.Any(l => l.Name == destination)) &&
                    (minPrice == null || x.Price >= minPrice) &&
                    (maxPrice == null || x.Price <= maxPrice) &&
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
                    (equipment == null || equipment.All(e => x.Amenities.Equipment.Select(a => a.Name).Contains(e))) &&
                    (x.OnSale == false);

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
        [Route("GetFeatured")]
        public IActionResult GetFeatured(
    int page = 0,
    int numResults = 25
)
        {
            try
            {
                // 1. Specify which navigation properties to include:
                var includes = new Expression<Func<Yacht, object>>[]
                {
            x => x.Specification,
            x => x.Media,        // so we can access Media.Images
                                 // x => x.Locations   // include only if you actually need Locations
                };

                // 2. Filter for featured yachts
                Expression<Func<Yacht, bool>> filter = yacht => yacht.IsFeatured;

                // 3. Get (with paging) and project into a slim DTO
                var featuredYachts = workUnit.YachtRepository
                    .Get(filter: filter, includes: includes)
                    .Skip(page * numResults)
                    .Take(numResults)
                    .Select(yacht => new
                    {
                        yacht.Id,
                        yacht.Name,
                        yacht.OnSale,
                        yacht.Price,
                        yacht.Specification.Length,
                        yacht.Specification.Guests,
                        yacht.HeroImageUrl

                    })
                    .ToList();

                return Ok(featuredYachts);
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
                return Ok(workUnit.YachtRepository.Get(yacht => yacht.OnSale).Skip(page * 25).Take(numResults));
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
        [Route("GetScubaYachts")]
        public IActionResult GetScubaYachts(
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

                return Ok(workUnit.YachtRepository.Get(yacht => 
                    yacht.Amenities.Equipment!.Any(x => x.Name.ToLower().Contains("scuba")) ||
                    yacht.Amenities.Toys!.Any(x => x.Name.ToLower().Contains("scuba"))
                ).Skip(page * 25).Take(numResults));
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

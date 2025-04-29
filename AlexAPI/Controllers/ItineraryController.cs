using Microsoft.AspNetCore.Mvc;
using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using AlexAPI.Services.Interfaces;
using FluentFTP;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItineraryController : Controller
    {
        private readonly ILogger<ItineraryController> logger;
        private readonly ItineraryWorkUnit workUnit;
        private readonly IFTPService ftpService;
        private readonly UserManager<ApplicationUser> userManager;

        public ItineraryController(ILogger<ItineraryController> logger, ItineraryWorkUnit workUnit, IFTPService ftpService, UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.workUnit = workUnit;
            this.ftpService = ftpService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Get")]
        public async Task<IActionResult> Get(
            int page = 0,
            int numResults = 25
        )
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                return Ok(user?.Itineraries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Roles(UserRoles.User, UserRoles.Broker)]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ItineraryDto ItineraryDto)
        {
            try
            {
                var yachts = workUnit.YachtRepository.Get(filter: x => ItineraryDto.YachtIds.Contains(x.Id)).ToList();
                ItineraryDto.Itinerary.Yachts = yachts;
                var itinerary = ItineraryDto.Itinerary;
                ItineraryDto.Itinerary.Days = ItineraryDto.Days.Select(x => new ItineraryDay
                {
                    Number = x.Number,
                    Description = x.Description,
                    Image = x.Image == null ? null : ftpService.UploadFile(x.Image, $"Itinerary/{itinerary.Id}", $"{x.Number}").Result,
                    FromLocation = workUnit.LocationRepository.GetByID(x.FromLoc),
                    ToLocation = workUnit.LocationRepository.GetByID(x.ToLoc),
                }).ToList();
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                user?.Itineraries.Add(itinerary);
                workUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Roles(UserRoles.User, UserRoles.Broker)]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if(user.Itineraries.FirstOrDefault(x => x.Id == Id) != null)
                {
                    user.Itineraries.Remove(user.Itineraries.FirstOrDefault(x => x.Id == Id));
                    var itinerary = workUnit.UserItineraryRepository.GetByID(Id);
                    ftpService.DeleteDirectory($"Itinerary/{Id}");
                    workUnit.UserItineraryRepository.Delete(itinerary);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLocations")]
        public IActionResult GetLocations()
        {
            try
            {
                return Ok(workUnit.LocationRepository.Get());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

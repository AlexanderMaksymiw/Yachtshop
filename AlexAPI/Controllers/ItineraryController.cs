using Microsoft.AspNetCore.Mvc;
using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using AlexAPI.Services.Interfaces;
using FluentFTP;
using System.Linq.Expressions;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItineraryController : Controller
    {
        private readonly ILogger<ItineraryController> logger;
        private readonly ItineraryWorkUnit workUnit;
        private readonly IFTPService ftpService;

        public ItineraryController(ILogger<ItineraryController> logger, ItineraryWorkUnit workUnit, IFTPService ftpService)
        {
            this.logger = logger;
            this.workUnit = workUnit;
            this.ftpService = ftpService;
        }

        [HttpPost]
        [Route("Get")]
        public IActionResult Get(
            int page = 0,
            int numResults = 25
            )
        {
            try
            {
                var includes = new Expression<Func<UserItinerary, object>>[]
                {
                    x => x.Yachts,
                };

                var result = workUnit.UserItineraryRepository
                    .Get(includes: includes)
                    .Skip(page * numResults)
                    .Take(numResults);

                return Ok(result);
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
                workUnit.UserItineraryRepository.Insert(itinerary);
                ItineraryDto.Itinerary.Days = ItineraryDto.Days.Select(x => new ItineraryDay
                {
                    Number = x.Number,
                    Description = x.Description,
                    Image = x.Image == null ? null : ftpService.UploadFile(x.Image, $"Itinerary/{itinerary.Id}", $"{x.Number}").Result,
                    FromLat = x.FromLat,
                    FromLong = x.FromLong,
                    ToLat = x.ToLat,
                    ToLong = x.ToLong,
                }).ToList();
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
        public IActionResult Delete(Guid Id)
        {
            try
            {
                var itinerary = workUnit.UserItineraryRepository.GetByID(Id);
                ftpService.DeleteDirectory($"Itinerary/{Id}");
                workUnit.UserItineraryRepository.Delete(itinerary);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}

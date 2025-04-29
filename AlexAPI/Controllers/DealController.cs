using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using AlexAPI.Services.Interfaces;
using FluentFTP;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AlexAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DealController : ControllerBase
    {
        private readonly ILogger<DealController> logger;
        private readonly DealWorkUnit workUnit;
        private readonly IFTPService ftpService;

        public DealController(ILogger<DealController> logger, DealWorkUnit workUnit, IFTPService ftpService)
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
                var includes = new Expression<Func<CharterDeal, object>>[]
                {
                    x => x.Yachts,
                };

                var result = workUnit.CharterDealRepository
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
        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] DealDto dealDto)
        {
            try
            {
                var yachts = workUnit.YachtRepository.Get(filter: x => dealDto.YachtIds.Contains(x.Id)).ToList();
                dealDto.Deal.Yachts = yachts;
                var deal = dealDto.Deal;
                workUnit.CharterDealRepository.Insert(deal);
                dealDto.Deal.Days = dealDto.Days.Select(x => new DealDay
                {
                    Number = x.Number,
                    Description = x.Description,
                    Image = x.Image == null ? null : ftpService.UploadFile(x.Image, $"Deal/{deal.Id}", $"{x.Number}").Result,
                    FromLocation = workUnit.LocationRepository.GetByID(x.FromLoc),
                    ToLocation = workUnit.LocationRepository.GetByID(x.ToLoc)
                }).ToList();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [Route("Delete")]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                var deal = workUnit.CharterDealRepository.GetByID(Id);
                ftpService.DeleteDirectory($"Deal/{Id}");
                workUnit.CharterDealRepository.Delete(deal);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

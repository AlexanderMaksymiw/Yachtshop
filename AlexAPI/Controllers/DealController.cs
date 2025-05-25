using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using AlexAPI.RequestModels;
using AlexAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                var dealDict = new Dictionary<Guid, CharterDeal>();

                var sql = @"
                    SELECT d.*, y.*
                    FROM CharterDeals d
                    LEFT JOIN CharterDealYachts dy ON d.Id = dy.CharterDealId
                    LEFT JOIN Yachts y ON dy.YachtId = y.Id
                    ORDER BY d.Id
                    OFFSET @Page * @NumResults ROWS
                    FETCH NEXT @NumResults ROWS ONLY";

                var deals = workUnit.CharterDealRepository.ExecuteMultiMapQuery<CharterDeal, Yacht>(
                    sql,
                    map: (deal, yacht) =>
                    {
                        deal.Yachts ??= new List<Yacht>();
                        if (yacht != null && !deal.Yachts.Any(y => y.Id == yacht.Id))
                            deal.Yachts.Add(yacht);
                        return deal;
                    },
                    splitOn: "Id",
                    parameters: new
                    {
                        Page = page,
                        NumResults = numResults
                    }
                );

                return Ok(deals);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Roles(UserRoles.Admin, UserRoles.Broker)]
        [Route("Create")]
        public IActionResult Create([FromBody] DealDto dealDto)
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

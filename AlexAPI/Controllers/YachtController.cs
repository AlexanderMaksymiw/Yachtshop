using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AlexAPI.Services.Interfaces;
using System.Globalization;
using Microsoft.OpenApi.Extensions;
using CsvHelper.Configuration.Attributes;

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
        private readonly IGeminiAIService geminiAI;
        private readonly ILlamaService llamaAI;

        public YachtController(ILogger<YachtController> logger, IConfiguration configuration, YachtWorkUnit workUnit, ICSVImportService csvImportService, IGeminiAIService geminiAI, ILlamaService llamaAI)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.workUnit = workUnit;
            this.csvImportService = csvImportService;
            this.geminiAI = geminiAI;
            this.llamaAI = llamaAI;
        }

        [HttpGet]
        [Route("Populate")]
        public async Task<IActionResult> PopulateDatabase()
        {
            HttpClient client = new HttpClient();
            var baseAddress = configuration.GetValue<string>("Yachtfolio:BaseAddress");
            var brochureBaseAddress = configuration.GetValue<string>("Yachtfolio:BrochureBaseAddress");
            var apiKey = configuration.GetValue<string>("Yachtfolio:APIKey");

            // Construct the full URL directly
            var yachtListURL = $"{baseAddress}?type=list&passkey={apiKey}";
            var response = await client.GetAsync(yachtListURL);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                try
                {
                    var yachtListResponse = JsonSerializer.Deserialize<YachtListResponse>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    foreach(var yacht in yachtListResponse.Data)
                    {
                        //Store Yacht Details
                        var yachtDetailsURL = $"{baseAddress}?type=yachts&id_yacht={yacht.Id}&passkey={apiKey}";
                        response = await client.GetAsync(yachtDetailsURL);
                        var yachtDetail = new YachtDetail();
                        var yachtBrochure = new YachtBrochure();
                        if (response.IsSuccessStatusCode)
                        {
                            responseData = await response.Content.ReadAsStringAsync();
                            var yachtDetailsResponse = JsonSerializer.Deserialize<YachtDetailResponse>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (yachtDetailsResponse.Data.Count > 0) {
                                yachtDetail = yachtDetailsResponse.Data.First();
                                workUnit.YachtDetailRepository.Insert(yachtDetail);
                            }
                        }
                        else
                        {
                            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                        }

                        //Store Yacht Brochure
                        var yachtBrochureURL = $"{brochureBaseAddress}?&id_yacht={yacht.Id}&passkey={apiKey}";
                        response = await client.GetAsync(yachtBrochureURL);
                        if (response.IsSuccessStatusCode)
                        {
                            responseData = await response.Content.ReadAsStringAsync();
                            yachtBrochure = JsonSerializer.Deserialize<YachtBrochure>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (yachtBrochure != null)
                            {
                                workUnit.YachtBrochureRepository.Insert(yachtBrochure);
                            }
                        }
                        else
                        {
                            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                        }

                        var yachtResult = new Yacht
                        {
                            Name = yacht.Name,
                            RegistryPort = yacht.RegistryPort,
                            Id = yacht.Id,
                            Detail = yachtDetail,
                            Brochure = yachtBrochure
                        };
                        //Store Yacht List
                        workUnit.YachtRepository.Insert(yachtResult);
                    }
                    workUnit.Save();
                    return Ok("Successfully populated the Database.");
                }
                catch (JsonException ex)
                {
                    logger.LogError(ex, "Error deserializing the JSON response.");
                    return StatusCode(500, ex);
                }
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        [HttpGet]
        [Route("GetAllYachtNames")]
        public async Task<IActionResult> GetAllYachtNames()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get().Select(x => x.Name));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(x => x.Name.ToLower() == name.ToLower()).First());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetId(Guid id)
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

        [HttpGet]
        [Route("GetBrochureById")]
        public async Task<IActionResult> GetBrochure(Guid id)
        {
            return Ok(workUnit.YachtRepository.GetByID(id).Brochure);
        }

        [HttpGet]
        [Route("GetYachtImagesById")]
        public async Task<IActionResult> GetYachtImagesById(Guid id)
        {
            return Ok(workUnit.YachtRepository.GetByID(id).Brochure.Galleries.Full.Select(x => x.Url));
        }

        [HttpGet]
        [Route("GetYachtToys")]
        public async Task<IActionResult> GetYachtToys(Guid id)
        {
            var yacht = workUnit.YachtRepository.GetByID(id);
            var result1 = await geminiAI.GetResponseAsync($"Can you create a description of the available toys of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Toys}");
            return Ok(result1.Replace("```html", "").Replace("```", ""));
        }

        [HttpGet]
        [Route("GetYachtEquipment")]
        public async Task<IActionResult> GetYachtEquipment(Guid id)
        {
            var yacht = workUnit.YachtRepository.GetByID(id);
            var result1 = await geminiAI.GetResponseAsync($"Can you create a description of the available equipment of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Equipment}");
            return Ok(result1.Replace("```html", "").Replace("```", ""));
        }

        [HttpGet]
        [Route("SYTest")]
        public IActionResult SYTest(string yachtName)
        {
            var yachtDetails = csvImportService.ReadSYTimesCSV("C:\\Users\\cmon-\\Downloads\\wetransfer_superyacht-reviews-csv_2024-08-19_1349\\SY Times A-Z Yacht Specs.csv");
            var result = yachtDetails.Where(x => x.Title.ToLower() == yachtName.ToLower()).Select(x => x.Text).First().Split('\n');
            return Ok(result);
        }

        [HttpGet]
        [Route("CWTest")]
        public IActionResult CWTest()
        {
            var yachtDetails = csvImportService.ReadCWYachtsCSV("C:\\Users\\cmon-\\Downloads\\wetransfer_superyacht-reviews-csv_2024-08-19_1349\\Charterworld A-Z Yachts.csv").ToList();
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
    }
}

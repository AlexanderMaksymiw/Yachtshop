using AlexAPI.Authentication;
using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Linq;
using AlexAPI.Services;

namespace AlexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YachtController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<YachtController> logger;
        private readonly YachtWorkUnit workUnit;
        private readonly IGeminiAIService geminiAI;
        private readonly ILlamaService llamaAI;

        public YachtController(ILogger<YachtController> logger, IConfiguration configuration, YachtWorkUnit workUnit, IGeminiAIService geminiAI, ILlamaService llamaAI)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.workUnit = workUnit;
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
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(workUnit.YachtRepository.GetByID(id));
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
            var result1 = await llamaAI.GetResponseAsync($"Can you create a description of the available toys of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Toys}");
            //var result2 = await llamaAI.GetResponseAsync($"Can you convert this markdown into a html div: \n{result1}");
            return Ok(result1.Replace("```html", "").Replace("```", ""));
        }

        [HttpGet]
        [Route("GetYachtEquipment")]
        public async Task<IActionResult> GetYachtEquipment(Guid id)
        {
            var yacht = workUnit.YachtRepository.GetByID(id);
            var result1 = await geminiAI.GetResponseAsync($"Can you create a description of the available equipment of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Equipment}");
            var result2 = await geminiAI.GetResponseAsync($"Can you convert this markdown into a html div: \n{result1}");
            return Ok(result2.Replace("```html", "").Replace("```", ""));
        }

        [HttpGet]
        [Route("OllamaTest")]
        public async Task<IActionResult> GetOllamaResponse(string prompt)
        {
            return Ok(await llamaAI.GetResponseAsync(prompt));
        }
    }
}

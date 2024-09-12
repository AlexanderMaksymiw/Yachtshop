using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AlexAPI.Services.Interfaces;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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

        //TODO: Delete this endpoint
        [HttpGet]
        [Route("GetAllYachtsWithDetails")]
        public async Task<IActionResult> GetAllYachtsWithDetails()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get(x => x.Detail.Id > 0).Select(x => new Tuple<string?, Guid>(
                    x.Name,
                    x.Guid
                )));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllYachtNames")]
        public async Task<IActionResult> GetAllYachtNames()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get().Select(x => new Tuple<string?, Guid>(
                    x.Name,
                    x.Guid
                )));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
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

        [HttpPost]
        [Route("SYTest")]
        public IActionResult SYTest(IFormFile file)
        {
            var yachtDetails = csvImportService.ReadSYTimesCSV(file);
            var result = yachtDetails.Take(10);
            return Ok(result);
        }

        [HttpPost]
        [Route("CWTest")]
        public IActionResult CWTest(IFormFile file)
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


        [HttpPost]
        [Route("YCFTest")]
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
                        if(yacht.Brochure.Specifications == null)
                        {
                            yacht.Brochure.Specifications = new Specifications
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
                            yacht.Brochure.Specifications.Length = csvYacht.Crew.IsNullOrEmpty() ? yacht.Brochure.Specifications.Length : csvYacht.Length;
                            yacht.Brochure.Specifications.Beam = csvYacht.Beam.IsNullOrEmpty() ? yacht.Brochure.Specifications.Beam : csvYacht.Beam;
                            yacht.Brochure.Specifications.Draft = csvYacht.Draft.IsNullOrEmpty() ? yacht.Brochure.Specifications.Draft : csvYacht.Draft;
                            yacht.Brochure.Specifications.GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? yacht.Brochure.Specifications.GuestsCruising : int.Parse(csvYacht.Guests);
                            yacht.Brochure.Specifications.GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? yacht.Brochure.Specifications.GrossTonnage : csvYacht.Gross_Tonnage;
                            yacht.Brochure.Specifications.CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? yacht.Brochure.Specifications.CruisingSpeed : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]);
                            yacht.Brochure.Specifications.YearBuilt = csvYacht.Built.IsNullOrEmpty() ? yacht.Brochure.Specifications.YearBuilt : int.Parse(csvYacht.Built);
                            yacht.Brochure.Specifications.Builder = csvYacht.Builder.IsNullOrEmpty() ? yacht.Brochure.Specifications.Builder : csvYacht.Builder;
                            yacht.Brochure.Specifications.Model = csvYacht.Model.IsNullOrEmpty() ? yacht.Brochure.Specifications.Model : csvYacht.Model;
                            yacht.Brochure.Specifications.ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? yacht.Brochure.Specifications.ExteriorDesigner : csvYacht.Exterior_Designer;
                            yacht.Brochure.Specifications.InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? yacht.Brochure.Specifications.InteriorDesigner : csvYacht.Interior_Designer;
                            yacht.Brochure.Specifications.Toys = csvYacht.Toys.IsNullOrEmpty() ? yacht.Brochure.Specifications.Toys : csvYacht.Toys;
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
    }
}

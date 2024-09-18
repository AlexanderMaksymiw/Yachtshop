using AlexAPI.Data.DAL.WorkUnits;
using AlexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AlexAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using AlexAPI.Enums;
using AlexAPI.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text;
using AlexAPI.ViewModels;

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
        private readonly ILlamaService llamaAI;

        public YachtController(ILogger<YachtController> logger, IConfiguration configuration, YachtWorkUnit workUnit, ICSVImportService csvImportService, ILlamaService llamaAI)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.workUnit = workUnit;
            this.csvImportService = csvImportService;
            this.llamaAI = llamaAI;
        }
        /*
        //TODO: Delete me!
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
        */

        [HttpPost]
        [Route("Get")]
        public IActionResult Get(
    string? name = null,
    string? type = null,
    string? destination = null,
    int? minPrice = null,
    int? maxPrice = null,
    int? minLength = null,
    int? maxLength = null,
    int? minGuests = null,
    int? maxGuests = null,
    int? minYearBuilt = null,
    int? maxYearBuilt = null,
    int? minCabins = null,
    int? maxCabins = null,
    int? minMaxSpeed = null,
    int? maxMaxSpeed = null,
    int? minGrossTonnage = null,
    int? maxGrossTonnage = null,
    int? minCruisingSpeed = null,
    int? maxCruisingSpeed = null,
    string? builder = null,
    string[]? equipment = null)
        {
            // Start building the SQL query
            var sqlQuery = new StringBuilder(@"
                SELECT 
                    y.*,
                    s.[Type],
                    s.[SubType],
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
            var parameters = new List<SqlParameter>();

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

            if (minLength.HasValue)
            {
                sqlQuery.Append(" AND s.Length >= @minLength");
                parameters.Add(new SqlParameter("@minLength", minLength));
            }

            if (maxLength.HasValue)
            {
                sqlQuery.Append(" AND s.Length <= @maxLength");
                parameters.Add(new SqlParameter("@maxLength", maxLength));
            }

            if (minGuests.HasValue)
            {
                sqlQuery.Append(" AND s.Guests >= @minGuests");
                parameters.Add(new SqlParameter("@minGuests", minGuests));
            }

            if (maxGuests.HasValue)
            {
                sqlQuery.Append(" AND s.Guests <= @maxGuests");
                parameters.Add(new SqlParameter("@maxGuests", maxGuests));
            }

            if (minYearBuilt.HasValue)
            {
                sqlQuery.Append(" AND s.YearBuilt >= @minYearBuilt");
                parameters.Add(new SqlParameter("@minYearBuilt", minYearBuilt));
            }

            if (maxYearBuilt.HasValue)
            {
                sqlQuery.Append(" AND s.YearBuilt <= @maxYearBuilt");
                parameters.Add(new SqlParameter("@maxYearBuilt", maxYearBuilt));
            }

            if (minCabins.HasValue)
            {
                sqlQuery.Append(" AND s.Cabins >= @minCabins");
                parameters.Add(new SqlParameter("@minCabins", minCabins));
            }

            if (maxCabins.HasValue)
            {
                sqlQuery.Append(" AND s.Cabins <= @maxCabins");
                parameters.Add(new SqlParameter("@maxCabins", maxCabins));
            }

            if (minMaxSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.MaxSpeed >= @minMaxSpeed");
                parameters.Add(new SqlParameter("@minMaxSpeed", minMaxSpeed));
            }

            if (maxMaxSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.MaxSpeed <= @maxMaxSpeed");
                parameters.Add(new SqlParameter("@maxMaxSpeed", maxMaxSpeed));
            }

            if (minGrossTonnage.HasValue)
            {
                sqlQuery.Append(" AND s.GrossTonnage >= @minGrossTonnage");
                parameters.Add(new SqlParameter("@minGrossTonnage", minGrossTonnage));
            }

            if (maxGrossTonnage.HasValue)
            {
                sqlQuery.Append(" AND s.GrossTonnage <= @maxGrossTonnage");
                parameters.Add(new SqlParameter("@maxGrossTonnage", maxGrossTonnage));
            }

            if (minCruisingSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.CruisingSpeed >= @minCruisingSpeed");
                parameters.Add(new SqlParameter("@minCruisingSpeed", minCruisingSpeed));
            }

            if (maxCruisingSpeed.HasValue)
            {
                sqlQuery.Append(" AND s.CruisingSpeed <= @maxCruisingSpeed");
                parameters.Add(new SqlParameter("@maxCruisingSpeed", maxCruisingSpeed));
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
            return Ok(yachtDtos.GroupBy(y => y.Id)
                .Select(group => new Yacht
                {
                    Id = group.First().Id,
                    Name = group.First().Name,
                    Specification = new Specification
                    {
                        Type = group.First().Type,
                        SubType = group.First().SubType,
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
                }));
        }



        [HttpGet]
        [Route("GetAllYachtNames")]
        public IActionResult GetAllYachtNames()
        {
            try
            {
                return Ok(workUnit.YachtRepository.Get().Select(x => new Tuple<string?, Guid>(
                    x.Name,
                    x.Id
                )));
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
        /*

        //TODO: Delete me!
        [HttpGet]
        [Route("GetBrochureById")]
        public IActionResult GetBrochure(Guid id)
        {
            return Ok(workUnit.YachtRepository.GetByID(id).Brochure);
        }

        [HttpGet]
        [Route("GetYachtImagesById")]
        public IActionResult GetYachtImagesById(Guid id)
        {
            return Ok(workUnit.YachtRepository.GetByID(id).Brochure.Galleries.Full.Select(x => x.Url));
        }

        //TODO: Delete me!
        [HttpGet]
        [Route("GetYachtToys")]
        public async Task<IActionResult> GetYachtToys(Guid id)
        {
            var yacht = workUnit.YachtRepository.GetByID(id);
            var result1 = await geminiAI.GetResponseAsync($"Can you create a description of the available toys of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Toys}");
            return Ok(result1.Replace("```html", "").Replace("```", ""));
        }

        //TODO: Delete me!
        [HttpGet]
        [Route("GetYachtEquipment")]
        public async Task<IActionResult> GetYachtEquipment(Guid id)
        {
            var yacht = workUnit.YachtRepository.GetByID(id);
            var result1 = await geminiAI.GetResponseAsync($"Can you create a description of the available equipment of the yacht {yacht.Name.ToUpper()} using these key features: \n{yacht.Brochure.Auto.Equipment}");
            return Ok(result1.Replace("```html", "").Replace("```", ""));
        }
        */

        //TODO: Delete me!
        [HttpPost]
        [Route("SuperYachtTimesImport")]
        public async Task<IActionResult> SuperYachtTimesImport(IFormFile file)
        {
            var yachtsFromCSV = csvImportService.ReadSYTimesCSV(file).ToList();
            foreach (var csvYacht in yachtsFromCSV)
            {
                var yacht = new Yacht
                {
                    Name = csvYacht.Title,
                    Specification = new Specification
                    {
                        Type = csvYacht.Yacht_type.IsNullOrEmpty() ? "" : csvYacht.Yacht_type,
                        SubType = csvYacht.Hull_Type.IsNullOrEmpty() ? "" : csvYacht.Hull_Type,
                        YearBuilt = csvYacht.Year_Built.IsNullOrEmpty() ? 0 : csvYacht.Year_Built == "N/A" ? 0 : int.Parse(csvYacht.Year_Built),
                        Builder = csvYacht.Builder.IsNullOrEmpty() ? "" : csvYacht.Builder,
                        Length = csvYacht.Crew.IsNullOrEmpty() ? 0 : ConvertToMeters(csvYacht.Length),
                        Guests = csvYacht.Guests.IsNullOrEmpty() ? 0 : csvYacht.Guests == "N/A" ? 0 : int.Parse(csvYacht.Guests),
                        Cabins = csvYacht.Crew_Cabins.IsNullOrEmpty() ? 0 : csvYacht.Crew_Cabins == "N/A" ? 0 : int.Parse(csvYacht.Crew_Cabins),
                        Flag = csvYacht.Flag_Country.IsNullOrEmpty() ? "" : csvYacht.Flag_Country,
                        Port = csvYacht.Port.IsNullOrEmpty() ? "" : csvYacht.Port,
                        Superstructure = csvYacht.Superstructure.IsNullOrEmpty() ? "" : csvYacht.Superstructure,
                        InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? "" : csvYacht.Interior_Designer,
                        ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? "" : csvYacht.Exterior_Designer,
                        Crew = csvYacht.Crew.IsNullOrEmpty() ? 0 : csvYacht.Crew == "N/A" ? 0 : int.Parse(csvYacht.Crew),
                        Beam = csvYacht.Beam.IsNullOrEmpty() ? 0 : ConvertToMeters(csvYacht.Beam),
                        Draft = csvYacht.Draft.IsNullOrEmpty() ? 0 : ConvertToMeters(csvYacht.Draft),
                        GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? 0 : ConvertToInt(csvYacht.Gross_Tonnage),
                        MaxSpeed = csvYacht.Max_Speed.IsNullOrEmpty() ? 0 : ConvertToDecimal(csvYacht.Max_Speed),
                        CruisingSpeed = csvYacht.Cruise_Speed.IsNullOrEmpty() ? 0 : csvYacht.Cruise_Speed == "N/A" ? 0 : decimal.Parse(csvYacht.Cruise_Speed.Split(" ")[0]),
                        EnginePowerOutput = csvYacht.Total_Power_Output.IsNullOrEmpty() ? "" : csvYacht.Total_Power_Output,
                        PropulsionType = csvYacht.Propulsion_Type.IsNullOrEmpty() ? "" : csvYacht.Propulsion_Type,
                        FuelCapacity = csvYacht.Fuel_Capacity.IsNullOrEmpty() ? "" : csvYacht.Fuel_Capacity,
                        PreviousNames = csvYacht.Previous_Names.IsNullOrEmpty() ? new List<PreviousName>() : csvYacht.Previous_Names.Split(", ").Select(x => new PreviousName { Name = x }).ToList(),
                    }
                };

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");

                var imageAddress = $"https://www.superyachttimes.com/api/yacht-photos/{csvYacht.Title_URL.Split("/yachts/")[1]}";

                var response = await client.GetAsync(imageAddress);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    try
                    {
                        List<Image> images = new List<Image>();
                        var imageListResponse = JsonSerializer.Deserialize<ImageListResponse>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if(imageListResponse != null)
                        {

                            if (imageListResponse.Primary != null)
                            {
                                images.Add(new Image
                                {
                                    Type = ImageTypeEnum.Primary,
                                    Url = $"https://photos.superyachtapi.com/download/{imageListResponse.Primary.Urls.ExtraLarge.Split("/")[1]}/large",
                                    PhotographerName = imageListResponse.Primary.PhotographerName,
                                    Filename = imageListResponse.Primary.Title
                                });
                            }

                            if (imageListResponse.Interior != null)
                            {
                                imageListResponse.Interior.ForEach(image =>
                                {
                                    images.Add(new Image
                                    {
                                        Type = ImageTypeEnum.Interior,
                                        Url = $"https://photos.superyachtapi.com/download/{image.Urls.ExtraLarge.Split("/")[1]}/large",
                                        PhotographerName = image.PhotographerName,
                                        Filename = image.Title
                                    });
                                });
                            }

                            if (imageListResponse.Exterior != null)
                            {
                                imageListResponse.Exterior.ForEach(image =>
                                {
                                    images.Add(new Image
                                    {
                                        Type = ImageTypeEnum.Exterior,
                                        Url = $"https://photos.superyachtapi.com/download/{image.Urls.ExtraLarge.Split("/")[1]}/large",
                                        PhotographerName = image.PhotographerName,
                                        Filename = image.Title
                                    });
                                });
                            }

                            if(imageListResponse.Other != null)
                            {
                                imageListResponse.Other.ForEach(image =>
                                {
                                    images.Add(new Image
                                    {
                                        Type = ImageTypeEnum.Other,
                                        Url = $"https://photos.superyachtapi.com/download/{image.Urls.ExtraLarge.Split("/")[1]}/large",
                                        PhotographerName = image.PhotographerName,
                                        Filename = image.Title
                                    });
                                });
                            }
                        }
                        yacht.Media = new Media
                        {
                            Images = images
                        };
                    }
                    catch (Exception ex)
                    {
                    }
                }
                workUnit.YachtRepository.Insert(yacht);

            };
            workUnit.Save();
            return Ok(yachtsFromCSV);
        }

        /*
        //TODO: Delete me!
        [HttpPost]
        [Route("CharterWorldImport")]
        public IActionResult CharterWorldImport(IFormFile file)
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

        //TODO: Delete me!
        [HttpPost]
        [Route("YachtCharterFleetImport")]
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
                        if(yacht.Specification == null)
                        {
                            yacht.Specification = new Specifications
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
                            yacht.Specification.Length = csvYacht.Crew.IsNullOrEmpty() ? yacht.Specification.Length : csvYacht.Length;
                            yacht.Specification.Beam = csvYacht.Beam.IsNullOrEmpty() ? yacht.Specification.Beam : csvYacht.Beam;
                            yacht.Specification.Draft = csvYacht.Draft.IsNullOrEmpty() ? yacht.Specification.Draft : csvYacht.Draft;
                            yacht.Specification.GuestsCruising = csvYacht.Guests.IsNullOrEmpty() ? yacht.Specification.GuestsCruising : int.Parse(csvYacht.Guests);
                            yacht.Specification.GrossTonnage = csvYacht.Gross_Tonnage.IsNullOrEmpty() ? yacht.Specification.GrossTonnage : csvYacht.Gross_Tonnage;
                            yacht.Specification.CruisingSpeed = csvYacht.Cruising_Speed.IsNullOrEmpty() ? yacht.Specification.CruisingSpeed : csvYacht.Cruising_Speed == "-" ? 0 : decimal.Parse(csvYacht.Cruising_Speed.Split(" ")[0]);
                            yacht.Specification.YearBuilt = csvYacht.Built.IsNullOrEmpty() ? yacht.Specification.YearBuilt : int.Parse(csvYacht.Built);
                            yacht.Specification.Builder = csvYacht.Builder.IsNullOrEmpty() ? yacht.Specification.Builder : csvYacht.Builder;
                            yacht.Specification.Model = csvYacht.Model.IsNullOrEmpty() ? yacht.Specification.Model : csvYacht.Model;
                            yacht.Specification.ExteriorDesigner = csvYacht.Exterior_Designer.IsNullOrEmpty() ? yacht.Specification.ExteriorDesigner : csvYacht.Exterior_Designer;
                            yacht.Specification.InteriorDesigner = csvYacht.Interior_Designer.IsNullOrEmpty() ? yacht.Specification.InteriorDesigner : csvYacht.Interior_Designer;
                            yacht.Specification.Toys = csvYacht.Toys.IsNullOrEmpty() ? yacht.Specification.Toys : csvYacht.Toys;
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
        */
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
            value = value.Trim();
            if (value.Equals("N/A", StringComparison.OrdinalIgnoreCase)) return 0;

            var parts = value.Split(' ');
            if (decimal.TryParse(parts[0], out var result))
                return result;

            return 0;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using AlexAPI.Data;  // Your DbContext namespace
using Microsoft.EntityFrameworkCore; // For Include if needed


namespace AlexAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;


        public AdminController(ApplicationDbContext dbContext,IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [Route("MigrateHeroImages")]
        public IActionResult MigrateHeroImages()
        {
            var yachts = _dbContext.Yachts
                .Include(y => y.Media)
                    .ThenInclude(m => m.Images)
                .ToList();

            foreach (var yacht in yachts)
            {
                var heroImage = yacht.Media?.Images?.FirstOrDefault(img => img.Type == 0);
                if (heroImage != null && string.IsNullOrEmpty(yacht.HeroImageUrl))
                {
                    yacht.HeroImageUrl = heroImage.Url;
                }
            }

            _dbContext.SaveChanges();

            return Ok("Hero images migrated successfully.");
        }
        [HttpPost("DownloadAndConvertAllImages")]
        public async Task<IActionResult> DownloadAndConvertAllImages()
        {
            const int batchSize = 50;

            var totalImages = await _dbContext.Images
                .Where(i => i.WebpData == null && i.Url != null)
                .ToListAsync();

            var client = _httpClientFactory.CreateClient();
            int convertedCount = 0;
            var failedImages = new List<Guid>();

            Console.WriteLine($"🔍 Found {totalImages.Count} images to process in batches of {batchSize}");

            for (int i = 0; i < totalImages.Count; i += batchSize)
            {
                var batch = totalImages.Skip(i).Take(batchSize).ToList();

                foreach (var image in batch)
                {
                    try
                    {
                        Console.WriteLine($"⬇️ Downloading image: {image.Id}");

                        var response = await client.GetAsync(image.Url);
                        if (!response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"❌ Failed to download image: {image.Id} - Status: {response.StatusCode}");
                            failedImages.Add(image.Id);
                            continue;
                        }

                        using var originalStream = await response.Content.ReadAsStreamAsync();
                        using var imageSharpImage = await SixLabors.ImageSharp.Image.LoadAsync(originalStream);
                        using var webpStream = new MemoryStream();

                        await imageSharpImage.SaveAsWebpAsync(webpStream, new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                        {
                            Quality = 80
                        });

                        image.WebpData = webpStream.ToArray();
                        image.Url = null;
                        image.Filename = $"{image.Id}.webp";

                        Console.WriteLine($"✅ Converted image: {image.Id}");
                        convertedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"💥 Error converting image {image.Id}: {ex.Message}");
                        failedImages.Add(image.Id);
                    }
                }

                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"💾 Saved batch {i / batchSize + 1}");
            }

            return Ok(new
            {
                Message = $"{convertedCount} images converted and stored.",
                Failed = failedImages
            });
        }
    }
}

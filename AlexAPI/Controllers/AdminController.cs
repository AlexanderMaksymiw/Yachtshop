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

        public AdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}

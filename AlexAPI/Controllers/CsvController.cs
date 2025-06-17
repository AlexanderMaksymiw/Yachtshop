using Microsoft.AspNetCore.Mvc;

namespace AlexAPI.Controllers
{
    [ApiController]
    [Route("api/csv")]
    public class CsvController : ControllerBase
    {
        private readonly CsvCleaner _csvCleaner;

        public CsvController(CsvCleaner csvCleaner)
        {
            _csvCleaner = csvCleaner;
        }

        [HttpPost("clean")]
        public async Task<IActionResult> CleanCsv([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var cleanedCsvStream = await _csvCleaner.CleanCsvAsync(stream);

            return File(cleanedCsvStream, "text/csv", $"cleaned_{file.FileName}");
        }
    }
}

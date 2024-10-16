using AlexAPI.Services.Models;

namespace AlexAPI.Services.Interfaces
{
    public interface ICSVImportService
    {
        public IEnumerable<SYTimesCSV> ReadSYTimesCSV(IFormFile file);
        public IEnumerable<CWYachtsCSV> ReadCWYachtsCSV(IFormFile file);
        public IEnumerable<YCFYachtsCSV> ReadYachtCharterFleetCSV(IFormFile file);
        public IEnumerable<ShortSYTimesCSV> ReadShortSYTimesCSV(IFormFile file);
    }
}

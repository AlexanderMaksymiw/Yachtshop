using AlexAPI.Services.Models;

namespace AlexAPI.Services.Interfaces
{
    public interface ICSVImportService
    {
        public IEnumerable<SYTimesCSV> ReadSYTimesCSV(string filePath);
        public IEnumerable<CWYachtsCSV> ReadCWYachtsCSV(string filePath);
    }
}

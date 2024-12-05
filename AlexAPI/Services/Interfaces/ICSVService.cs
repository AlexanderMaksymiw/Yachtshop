using AlexAPI.Services.Models;

namespace AlexAPI.Services.Interfaces
{
    public interface ICSVService
    {
        public IEnumerable<SYTimesCSV> ReadSYTimesCSV(IFormFile file);
        public IEnumerable<CWYachtsCSV> ReadCWYachtsCSV(IFormFile file);
        public IEnumerable<YCFYachtsCSV> ReadYachtCharterFleetCSV(IFormFile file);
        public IEnumerable<ShortSYTimesCSV> ReadShortSYTimesCSV(IFormFile file);
        public IEnumerable<CMSCSV> ReadCMSCSV(IFormFile file);
        public IEnumerable<string> GetHeader(IFormFile file);
        public void CreateCSV(string path, IEnumerable<string> header, List<List<string>> rows);
    }
}

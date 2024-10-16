using AlexAPI.Services.Interfaces;
using AlexAPI.Services.Models;
using CsvHelper;
using System.Globalization;

namespace AlexAPI.Services
{
    public class CSVImportService : ICSVImportService
    {
        public IEnumerable<SYTimesCSV> ReadSYTimesCSV(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<SYTimesCSV>().ToList();
            }
        }

        public IEnumerable<CWYachtsCSV> ReadCWYachtsCSV(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<CWYachtsCSV>().ToList();
            }
        }

        public IEnumerable<YCFYachtsCSV> ReadYachtCharterFleetCSV(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<YCFYachtsCSV>().ToList();
            }
        }
        public IEnumerable<ShortSYTimesCSV> ReadShortSYTimesCSV(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<ShortSYTimesCSV>().ToList();
            }
        }
    }
}

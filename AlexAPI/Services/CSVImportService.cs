using AlexAPI.Services.Interfaces;
using AlexAPI.Services.Models;
using CsvHelper;
using System.Globalization;

namespace AlexAPI.Services
{
    public class CSVImportService : ICSVImportService
    {
        public IEnumerable<SYTimesCSV> ReadSYTimesCSV(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<SYTimesCSV>().ToList();
            }
        }

        public IEnumerable<CWYachtsCSV> ReadCWYachtsCSV(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<CWYachtsCSV>().ToList();
            }
        }
    }
}

using AlexAPI.Services.Interfaces;
using AlexAPI.Services.Models;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace AlexAPI.Services
{
    public class CSVService : ICSVService
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

        public IEnumerable<CMSCSV> ReadCMSCSV(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<CMSCSV>().ToList();
            }
        }

        public IEnumerable<string> GetHeader(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                return csv.HeaderRecord.ToList();
            }
        }

        public void CreateCSV(string path, IEnumerable<string> header, List<List<string>> rows)
        {
            var csv = new StringBuilder();
            csv.AppendLine(string.Join(",", header.Select(h => $"\"{h}\"")));

            rows.ForEach(row =>
            {
                csv.AppendLine(string.Join(",", row.Select(value => $"\"{value.Replace("\"", "\"\"")}\"")));
            });

            File.WriteAllText(path, csv.ToString());
        }
    }
}

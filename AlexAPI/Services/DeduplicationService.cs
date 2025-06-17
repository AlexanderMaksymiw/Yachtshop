using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlexAPI.Models;  // Using the model namespace
using CsvHelper;
using CsvHelper.Configuration;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using AlexAPI.Data;

namespace AlexAPI.Services
{
    public class DeduplicationService
    {
        private readonly ApplicationDbContext _context;

        public DeduplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool isDuplicate, int confidenceScore, List<string> matchedFields, Yacht? duplicateYacht)> CheckDuplicateAsync(YachtInputModel newYacht)
        {
            var possibleMatches = await _context.Yachts
                .Include(y => y.Specification)
                .Where(y => y.Name == newYacht.Name)
                .ToListAsync();

            foreach (var yacht in possibleMatches)
            {
                var spec = yacht.Specification;
                var input = newYacht.Specification;
                int score = 0;
                var reasonList = new List<string>();

                if (Fuzz.Ratio(spec.Builder, input.Builder) > 85)
                {
                    score += 20;
                    reasonList.Add("Builder");
                }

                if (spec.YearBuilt == input.YearBuilt)
                {
                    score += 15;
                    reasonList.Add("YearBuilt");
                }

                if (Math.Abs((double)(spec.Length ?? 0) - input.Length) < 0.5)
                {
                    score += 15;
                    reasonList.Add("Length");
                }

                if (spec.Guests == input.Guests)
                {
                    score += 15;
                    reasonList.Add("Guests");
                }

                if (spec.Cabins == input.Cabins)
                {
                    score += 15;
                    reasonList.Add("Cabins");
                }

                if (Fuzz.Ratio(spec.Type, input.Type) > 85)
                {
                    score += 20;
                    reasonList.Add("Type");
                }

                if (score >= 85)
                {
                    await LogDuplicateAsync(newYacht, reasonList);
                    return (true, score, reasonList, yacht);
                }
            }

            return (false, 0, new List<string>(), null);
        }

        private async Task LogDuplicateAsync(YachtInputModel duplicate, List<string> reasons)
        {
            var log = new DuplicateYachtLog
            {
                Name = duplicate.Name,
                ConflictFields = string.Join(", ", reasons),
                DateFlagged = DateTime.UtcNow
            };

            _context.DuplicateYachtLog.Add(log);
            await _context.SaveChangesAsync();

            using var writer = new StreamWriter("duplicates.csv", append: true);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            });

            csv.WriteField(log.Name);
            csv.WriteField(log.ConflictFields);
            csv.WriteField(log.DateFlagged);
            csv.NextRecord();
        }

        // CSV generator method for duplicate reports
        // Using YachtDuplicateReport from AlexAPI.Models
        public string GenerateCsv(List<YachtDuplicateReport> duplicates)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name,ConfidenceScore,MatchedFields");
            foreach (var dup in duplicates)
            {
                sb.AppendLine($"\"{dup.Name}\",{dup.ConfidenceScore},\"{dup.MatchedFields}\"");
            }
            return sb.ToString();
        }
    }

    // DTO classes (if not already defined elsewhere)
    public class YachtInputModel
    {
        public string Name { get; set; }
        public SpecificationInputModel Specification { get; set; }
    }

    public class SpecificationInputModel
    {
        public string Type { get; set; }
        public int YearBuilt { get; set; }
        public string Builder { get; set; }
        public double Length { get; set; }
        public int Guests { get; set; }
        public int Cabins { get; set; }
    }
}

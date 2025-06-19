using AlexAPI.Models;  // Using the model namespace
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
                    return (true, score, reasonList, yacht);
                }
            }

            return (false, 0, new List<string>(), null);
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

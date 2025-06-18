using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// 1. Cleaner Interface
public interface IFieldCleaner
{
    string Clean(string input);
}

// 2. Concrete Cleaners
public class IntCleaner : IFieldCleaner
{
    public string Clean(string input)
        => Regex.Match(input, @"\d+").Value;
}

public class FloatCleaner : IFieldCleaner
{
    public string Clean(string input)
        => Regex.Match(input, @"\d+(\.\d+)?").Value;
}

public class MoneyCleaner : IFieldCleaner
{
    public string Clean(string input)
        => Regex.Match(input, @"\d[\d,]*").Value.Replace(",", "");
}

public class ListCleaner : IFieldCleaner
{
    public string Clean(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";
        return string.Join(';',
            input.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct());
    }
}

public class NoOpCleaner : IFieldCleaner
{
    public string Clean(string input) => input.Trim();
}

// 3. The main CsvCleaner class
public class CsvCleaner
{
    private readonly Dictionary<string, string> _headerMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Title", "Name" }, { "Length (m)", "Length" }, { "image", "HeroImageUrl" },
        { "Main Image", "HeroImageUrl" }, { "Charter Price", "Price" }, { "Sale Price", "Price" },
        { "Hull", "HullType" }, { "Country Flag", "Flag" }, { "hero", "HeroImageUrl" },
        { "Rooms", "Cabins" }, { "Crew Members", "Crew" }, { "Built", "YearBuilt" },
        { "Toys List", "Toys" }, { "Equipment List", "Equipment" },
        { "Subtype", "SubTypes" }, { "Sub-Type", "SubTypes" }, { "Specification SubType", "SubTypes" },
        { "Yacht Type", "YachtType" }, { "Guests", "Guests" }, { "Beam", "Beam" }, { "Draft", "Draft" },
        { "Gross Tonnage", "GrossTonnage" }, { "Cruising speed", "CruisingSpeed" }, { "Model", "Model" },
        { "Builder", "Builder" }
    };

    private readonly Dictionary<string, List<string>> _preferredSources = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Length", new() { "length_1", "js_compare_length", "Length" } },
        { "Guests", new() { "guests_1", "Guests" } },
        { "Cabins", new() { "cabins_1", "Cabins" } },
        { "Builder", new() { "Builder", "js_compare_builder" } },
        { "Price", new() { "perweek", "Price" } }
    };

    private readonly Dictionary<string, IFieldCleaner> _fieldCleaners = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Guests", new IntCleaner() },
        { "Cabins", new IntCleaner() },
        { "Crew", new IntCleaner() },
        { "Length", new FloatCleaner() },
        { "Beam", new FloatCleaner() },
        { "Draft", new FloatCleaner() },
        { "Price", new MoneyCleaner() },
        { "Toys", new ListCleaner() },
        { "Equipment", new ListCleaner() },
        { "SubTypes", new ListCleaner() },
        { "GrossTonnage", new IntCleaner() },
        { "CruisingSpeed", new FloatCleaner() },
        { "YachtType", new NoOpCleaner() },
        { "Model", new NoOpCleaner() },
        { "Builder", new NoOpCleaner() }
    };

    private readonly List<string> _targetHeaders;
    private readonly List<string> _outputHeaderOrder;

    public CsvCleaner()
    {
        _targetHeaders = _headerMap.Values.Distinct().ToList();
        _outputHeaderOrder = _targetHeaders;
    }

    public async Task<MemoryStream> CleanCsvAsync(Stream inputCsvStream)
    {
        using var reader = new StreamReader(inputCsvStream, Encoding.UTF8, leaveOpen: true);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null,
            PrepareHeaderForMatch = args => args.Header.Trim()
        });

        await csv.ReadAsync();
        csv.ReadHeader();
        var originalHeaders = csv.HeaderRecord ?? Array.Empty<string>();

        var headerMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var usedTargets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var target in _targetHeaders)
        {
            if (_preferredSources.TryGetValue(target, out var preferred))
            {
                foreach (var candidate in preferred)
                {
                    if (originalHeaders.Contains(candidate, StringComparer.OrdinalIgnoreCase))
                    {
                        headerMapping[candidate] = target;
                        usedTargets.Add(target);
                        break;
                    }
                }
            }
        }

        foreach (var header in originalHeaders)
        {
            if (headerMapping.ContainsKey(header)) continue;

            if (_headerMap.TryGetValue(header, out var mapped) && !usedTargets.Contains(mapped))
            {
                headerMapping[header] = mapped;
                usedTargets.Add(mapped);
            }
            else
            {
                var bestMatch = FindBestFuzzyMatch(header, _targetHeaders.Except(usedTargets).ToList());
                if (bestMatch != null)
                {
                    headerMapping[header] = bestMatch;
                    usedTargets.Add(bestMatch);
                }
                else
                {
                    Console.WriteLine($"⚠️  Unmapped header: {header}");
                }
            }
        }

        var cleanedRows = new List<Dictionary<string, string>>();

        while (await csv.ReadAsync())
        {
            var row = new Dictionary<string, string>();

            foreach (var originalHeader in originalHeaders)
            {
                if (!headerMapping.TryGetValue(originalHeader, out var mappedHeader))
                    continue;

                var rawValue = csv.GetField(originalHeader) ?? "";
                rawValue = Regex.Replace(rawValue.Trim(), @"\s+", " ");
                var cleaner = _fieldCleaners.GetValueOrDefault(mappedHeader, new NoOpCleaner());

                var cleanedValue = cleaner.Clean(rawValue);

                if (!row.TryGetValue(mappedHeader, out var existingValue) || string.IsNullOrWhiteSpace(existingValue))
                {
                    if (!string.IsNullOrWhiteSpace(cleanedValue))
                        row[mappedHeader] = cleanedValue;
                }
            }

            if (row.TryGetValue("Name", out var nameVal) && nameVal.Contains("metres", StringComparison.OrdinalIgnoreCase))
            {
                if (row.TryGetValue("HeroImageUrl", out var urlVal) && !string.IsNullOrWhiteSpace(urlVal))
                {
                    row["Name"] = ExtractNameFromUrl(urlVal);
                }
            }

            cleanedRows.Add(row);
        }

        var outputStream = new MemoryStream();
        using var writer = new StreamWriter(outputStream, Encoding.UTF8, leaveOpen: true);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

        foreach (var header in _outputHeaderOrder)
            csvWriter.WriteField(header);
        await csvWriter.NextRecordAsync();

        foreach (var row in cleanedRows)
        {
            foreach (var header in _outputHeaderOrder)
                csvWriter.WriteField(row.TryGetValue(header, out var val) ? val : "");
            await csvWriter.NextRecordAsync();
        }

        await writer.FlushAsync();
        outputStream.Position = 0;
        return outputStream;
    }

    private string? FindBestFuzzyMatch(string input, List<string> candidates, int maxDistance = 3)
    {
        string? best = null;
        int bestDist = maxDistance + 1;

        foreach (var candidate in candidates)
        {
            int dist = LevenshteinDistance(input.ToLower(), candidate.ToLower());
            if (dist < bestDist)
            {
                bestDist = dist;
                best = candidate;
            }
        }

        return best;
    }

    private int LevenshteinDistance(string s, string t)
    {
        int[,] d = new int[s.Length + 1, t.Length + 1];
        for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= t.Length; j++) d[0, j] = j;

        for (int i = 1; i <= s.Length; i++)
            for (int j = 1; j <= t.Length; j++)
            {
                int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }

        return d[s.Length, t.Length];
    }

    private string ExtractNameFromUrl(string url)
    {
        var match = Regex.Match(url, @"/([\w\-]+)-Superyacht", RegexOptions.IgnoreCase);
        if (!match.Success) return "Unknown";

        var raw = match.Groups[1].Value;
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(raw.Replace("-", " "));
    }
}

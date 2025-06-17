using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

public class CsvCleaner
{
    // Primary header map - can be extended or loaded externally
    private readonly Dictionary<string, string> _headerMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Title", "Name" },
        { "Length (m)", "Length" },
        { "image", "HeroImageUrl" },
        { "hero", "HeroImageUrl" },
        { "Rooms", "Cabins" },
        { "Crew Members", "Crew" },
        { "Built", "YearBuilt" },
        { "Toys List", "Toys" },
        { "Equipment List", "Equipment" },
        { "Subtype", "SubTypes" },
        { "Sub-Type", "SubTypes" },
        { "Specification SubType", "SubTypes" },

    };

    // The canonical headers you want in your output CSV
    private readonly List<string> _targetHeaders;
    private readonly List<string> _outputHeaderOrder = GetDtoPropertyOrder<RawYachtCsvRow>();

    private static List<string> GetDtoPropertyOrder<T>() =>
        typeof(T).GetProperties()
                 .Select(p => p.Name)
                 .ToList();

    public CsvCleaner()
    {
        _targetHeaders = _headerMap.Values.Distinct().ToList();
        _outputHeaderOrder = GetDtoPropertyOrder<RawYachtCsvRow>();

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
        var originalHeaders = csv.HeaderRecord;

        var mappedHeaders = new List<string>();
        var headerMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // First pass: direct mapping from dictionary
        foreach (var h in originalHeaders)
        {
            if (_headerMap.TryGetValue(h, out var mapped))
            {
                mappedHeaders.Add(mapped);
                headerMapping[h] = mapped;
            }
        }

        // Second pass: fuzzy match for headers not mapped yet
        var unmappedHeaders = originalHeaders.Except(headerMapping.Keys).ToList();
        foreach (var header in unmappedHeaders)
        {
            var bestMatch = FindBestFuzzyMatch(header, _targetHeaders);
            if (bestMatch != null)
            {
                mappedHeaders.Add(bestMatch);
                headerMapping[header] = bestMatch;
            }
            // else skip unknown headers
        }

        mappedHeaders = mappedHeaders.Distinct().ToList();

        var cleanedRows = new List<Dictionary<string, string>>();

        while (await csv.ReadAsync())
        {
            var cleanRow = new Dictionary<string, string>();

            foreach (var originalHeader in originalHeaders)
            {
                if (!headerMapping.TryGetValue(originalHeader, out var mappedHeader))
                    continue;

                var rawValue = csv.GetField(originalHeader)?.Trim() ?? "";

                // Normalize list fields
                if (mappedHeader == "Toys" || mappedHeader == "Equipment" || mappedHeader == "SubTypes")
                {
                    cleanRow[mappedHeader] = NormalizeList(rawValue);
                }
                else
                {
                    cleanRow[mappedHeader] = rawValue;
                }
            }

            cleanedRows.Add(cleanRow);
        }

        // Output to MemoryStream
        var outputStream = new MemoryStream();
        using var writer = new StreamWriter(outputStream, Encoding.UTF8, leaveOpen: true);
        using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Write headers
        foreach (var header in _outputHeaderOrder)
            csvWriter.WriteField(header);
        await csvWriter.NextRecordAsync();

        // Write rows
        foreach (var row in cleanedRows)
        {
        foreach (var header in _outputHeaderOrder)
            {
                row.TryGetValue(header, out var val);
                csvWriter.WriteField(val ?? "");
            }
            await csvWriter.NextRecordAsync();
        }

        await writer.FlushAsync();
        outputStream.Position = 0;
        return outputStream;
    }

    private string NormalizeList(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";

        var items = input.Split(';', StringSplitOptions.RemoveEmptyEntries)
                         .Select(i => i.Trim())
                         .Where(i => !string.IsNullOrWhiteSpace(i))
                         .Distinct();

        return string.Join(';', items);
    }

    // Simple fuzzy matching using Levenshtein distance threshold
    private string? FindBestFuzzyMatch(string input, List<string> candidates, int maxDistance = 3)
    {
        string? bestMatch = null;
        int bestDistance = maxDistance + 1;

        foreach (var candidate in candidates)
        {
            int dist = LevenshteinDistance(input.ToLowerInvariant(), candidate.ToLowerInvariant());
            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestMatch = candidate;
            }
        }

        return bestMatch;
    }

    // Classic Levenshtein distance implementation
    private int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        var d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = s[i - 1] == t[j - 1] ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1,    // deletion
                             d[i, j - 1] + 1),   // insertion
                    d[i - 1, j - 1] + cost       // substitution
                );
            }
        }

        return d[n, m];
    }
}

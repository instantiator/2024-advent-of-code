using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class ReactorReports : List<List<int>> {}

public class Day2 : AbstractDay<ReactorReports>
{
    public override string DataFilename => "02-red-nosed-reports.txt";

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(ReactorReports data)
    {
        return new Tuple<bool, string?>(data.All(report => report.Count() > 0), "All reports should have at least 1 level");
    }

    public override async Task<ReactorReports> ParseDataAsync(IEnumerable<string> data)
    {
        var parsed = new ReactorReports();
        foreach (var line in data)
        {
            var levels = line.Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => int.Parse(word.Trim()))
                .ToList();

            parsed.Add(levels);
        }
        return parsed;
    }

    private bool IsSafe(IEnumerable<int> levels)
    {
        if (levels.Count() == 1) { return true; }
        var direction = levels.ElementAt(0) < levels.ElementAt(1) ? 1 : -1;

        for (int i = 0; i < levels.Count(); i++)
        {
            if (i == 0) { continue; }
            if (direction == 1 && levels.ElementAt(i) < levels.ElementAt(i - 1)) { return false; }
            if (direction == -1 && levels.ElementAt(i) > levels.ElementAt(i - 1)) { return false; }
            var diff = Math.Abs(levels.ElementAt(i) - levels.ElementAt(i - 1));
            if (diff < 1 || diff > 3) { return false; }
        }
        return true;
    }

    private bool IsSafeWithDampener(IEnumerable<int> levels)
    {
        var safe = IsSafe(levels);
        if (safe) { return true; }
        for (int x = 0; x < levels.Count(); x++)
        {
            var dampenedLevels = levels.Take(x).Concat(levels.Skip(x+1));
            if (IsSafe(dampenedLevels)) { return true; }
        }
        return false;
    }

    public override async Task<PuzzleResult?> RunImplementation1(ReactorReports data)
    {
        var result = new PuzzleResult();
        var safeReports = data.Count(report => IsSafe(report));
        result.Result = safeReports;
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation2(ReactorReports data)
    {
        var result = new PuzzleResult();
        var safeReports = data.Count(report => IsSafeWithDampener(report));
        result.Result = safeReports;
        return result;
    }
}
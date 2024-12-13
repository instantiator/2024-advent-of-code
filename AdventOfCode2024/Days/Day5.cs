using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class RulesAndRuns
{
    public IEnumerable<Tuple<int, int>> Rules { get; set; } = new List<Tuple<int, int>>();
    public IEnumerable<IEnumerable<int>> Runs { get; set; } = new List<IEnumerable<int>>();
}

public class Day5 : AbstractDay<RulesAndRuns>
{
    public override string DataFilename => "05-print-queue.txt";

    public override async Task<RulesAndRuns> ParseDataAsync(IEnumerable<string> data)
    {
        var rules = new List<Tuple<int, int>>();
        var runs = new List<IEnumerable<int>>();
        foreach (var line in data.Where(line => !string.IsNullOrWhiteSpace(line)))
        {
            if (line.Contains('|'))
            {
                var parts = line.Trim().Split('|').Select(part => int.Parse(part.Trim()));
                if (parts.Count() != 2) { throw new Exception("More than 2 parts found in rule: " + line); }
                rules.Add(new Tuple<int, int>(parts.ElementAt(0), parts.ElementAt(1)));
            }
            else if (line.Contains(','))
            {
                var parts = line.Trim().Split(',').Select(part => int.Parse(part.Trim()));
                runs.Add(parts);
            }
        }
        return new RulesAndRuns() { Rules = rules, Runs = runs };
    }

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(RulesAndRuns data)
    {
        var rulesAreNumbers = data.Rules.All(rule => rule.Item1 >= 0 && rule.Item2 >= 0);
        var runsAreNumbers = data.Runs.All(run => run.All(num => num >= 0));
        var runsNotEmpty = data.Runs.All(run => run.Count() > 0);
        return new Tuple<bool, string?>(rulesAreNumbers && runsAreNumbers && runsNotEmpty, "All values must be numbers");
    }

    public bool Safe(int position, IEnumerable<int> run, IEnumerable<Tuple<int, int>> rules)
    {
        var subject = run.ElementAt(position);

        // check those that go before
        for (int i = 0; i < position; i++)
        {
            var against = run.ElementAt(i);
            var conflicts = rules.Any(r => r.Item1 == subject && r.Item2 == against);
            // var acceptance = rules.Any(r => r.Item1 == against && r.Item2 == subject);
            if (conflicts) { return false; }
        }

        for (int i = position + 1; i < run.Count(); i++)
        {
            var against = run.ElementAt(i);
            var conflicts = rules.Any(r => r.Item1 == against && r.Item2 == subject);
            // var acceptance = rules.Any(r => r.Item1 == subject && r.Item2 == against);
            if (conflicts) { return false; }
        }
        return true;
    }

    private int MiddleOf(IEnumerable<int> run)
    {
        var middle = (int)(run.Count() / 2);
        return run.ElementAt(middle);
    }

    private IEnumerable<int> OrderRun(IEnumerable<int> source, IEnumerable<Tuple<int, int>> rules)
    {
        var result = new List<int>();
        foreach (var value in source)
        {
            if (result.Count() == 0) { result.Add(value); continue; }

            var inserted = false;
            for (int i = 0; i < result.Count(); i++)
            {
                var against = result.ElementAt(i);
                if (rules.Any(r => r.Item1 == value && r.Item2 == against))
                {
                    result.Insert(i, value); inserted = true; break;
                }
            }
            if (!inserted) { result.Add(value); }
        }
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation1(RulesAndRuns data)
    {
        var safeRuns = new List<IEnumerable<int>>();
        foreach (var run in data.Runs)
        {
            var safe = true;
            for (int i = 0; i < run.Count(); i++)
            {
                if (!Safe(i, run, data.Rules)) { safe = false; break; }
            }
            if (safe) { safeRuns.Add(run); }
        }

        var result = new PuzzleResult()
        {
            Result = safeRuns.Select(run => MiddleOf(run)).Sum()
        };
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation2(RulesAndRuns data)
    {
        var madeSafeRuns = new List<IEnumerable<int>>();
        foreach (var run in data.Runs)
        {
            var safe = true;
            for (int i = 0; i < run.Count(); i++)
            {
                if (!Safe(i, run, data.Rules))
                {
                    safe = false; break;
                }
            }
            if (!safe)
            {
                var reordered = OrderRun(run, data.Rules);
                madeSafeRuns.Add(reordered);
            }
        }

        var result = new PuzzleResult()
        {
            Result = madeSafeRuns.Select(run => MiddleOf(run)).Sum()
        };
        return result;

    }
}
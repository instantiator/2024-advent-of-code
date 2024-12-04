using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class Day1 : AbstractDay<Tuple<IEnumerable<int>, IEnumerable<int>>>
{
    public override string DataFilename => "01-historian-hysteria.txt";

    public override async Task<Tuple<IEnumerable<int>, IEnumerable<int>>> ParseDataAsync(IEnumerable<string> data)
    {
        var items = data
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line
                .Split(' ')
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Select(word => int.Parse(word.Trim())));

        return new Tuple<IEnumerable<int>, IEnumerable<int>>(
            items.Select(pair => pair.ElementAt(0)), 
            items.Select(pair => pair.ElementAt(1)));
    }

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(Tuple<IEnumerable<int>, IEnumerable<int>> data)
    {
        return new Tuple<bool, string?>(data.Item1.Count() == data.Item2.Count(), "Column length mismatch");
    }

    public override async Task<PuzzleResult?> RunImplementation1(Tuple<IEnumerable<int>, IEnumerable<int>> data)
    {
        var result = new PuzzleResult();
        var sorted = new Tuple<IEnumerable<int>, IEnumerable<int>>(
            data.Item1.OrderBy(i => i),
            data.Item2.OrderBy(i => i));

        int diff = 0;
        foreach (var pair in sorted.Item1.Zip(sorted.Item2))
        {
            diff += Math.Abs(pair.First - pair.Second);
        }

        result.Result = diff;
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation2(Tuple<IEnumerable<int>, IEnumerable<int>> data)
    {
        var result = new PuzzleResult();

        var left = data.Item1;
        var right = data.Item2;

        long similarity = 0;
        foreach (var item in left)
        {
            var appears = right.Count(i => i == item);
            similarity += (item * appears);
        }

        result.Result = similarity;
        return result;
    }
}
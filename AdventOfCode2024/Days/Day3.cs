using System.Text.RegularExpressions;
using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class Day3 : AbstractDay<IEnumerable<string>>
{
    private Regex MulRegex = new Regex(@"mul\((\d+),(\d+)\)");
    private Regex CmdRegex = new Regex(@"(mul\((\d+),(\d+)\)|do\(\)|don't\(\))");

    public override string DataFilename => "03-mull-it-over.txt";

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(IEnumerable<string> data)
    {
        var nonEmpty = data.All(line => !string.IsNullOrWhiteSpace(line));
        var mulRegexOk = MulRegex.Matches("mul(1,2) and mul(3,4)").Count == 2;
        var cmdRegexOk = CmdRegex.Matches("mul(1,2) and do() and don't()").Count == 3;
        return new Tuple<bool, string?>(nonEmpty && mulRegexOk && cmdRegexOk, "Lines and regex");
    }

    public override async Task<IEnumerable<string>> ParseDataAsync(IEnumerable<string> data)
    {
        return data.Where(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.Trim());
    }

    public override async Task<PuzzleResult?> RunImplementation1(IEnumerable<string> data)
    {
        var result = new PuzzleResult();
        
        var muls = data.SelectMany(line => MulRegex.Matches(line)).Select(match => match.Value);
        var mulSum = 0;
        foreach (var mul in muls)
        {
            var nums = mul.Substring("mul".Length).Trim('(', ')').Split(',').Select(int.Parse);
            mulSum += nums.ElementAt(0) * nums.ElementAt(1);
        }
        result.Result = mulSum;
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation2(IEnumerable<string> data)
    {
        var cmds = data.SelectMany(line => CmdRegex.Matches(line)).Select(match => match.Value);

        var enabled = true;
        var muls = new List<string>();
        foreach (var cmd in cmds)
        {
            if (cmd.StartsWith("mul") && enabled) { muls.Add(cmd);}
            if (cmd == "do()") { enabled = true; }
            if (cmd == "don't()") { enabled = false; }
        }

        var result = await RunImplementation1(muls);
        return result;
    }
}
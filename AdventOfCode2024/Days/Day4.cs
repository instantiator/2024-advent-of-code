using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class Day4 : AbstractDay<char[][]>
{
    public override string DataFilename => "04-ceres-search.txt";

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(char[][] data)
    {
        var nonEmpty = data.All(row => row.Length > 0);
        var sameLength = data.All(row => row.Length == data[0].Length);
        return new Tuple<bool, string?>(nonEmpty && sameLength, "All rows must be the same length");
    }

    public override async Task<char[][]> ParseDataAsync(IEnumerable<string> data)
    {
        return data
            .Where(row => !string.IsNullOrWhiteSpace(row))
            .Select(row => row.Trim().ToArray<char>())
            .ToArray<char[]>();
    }

    private bool CheckWord(char[][] data, int c, int r, string word, int dc, int dr)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (c < 0 || c >= data.Length || r < 0 || r >= data[0].Length) { return false; }
            if (data[c][r] != word[i]) { return false; }
            c += dc;
            r += dr;
        }
        return true;
    }

    public override async Task<PuzzleResult?> RunImplementation1(char[][] data)
    {
        var word = "XMAS";
        var result = new PuzzleResult();
        int count = 0;
        for (int r = 0; r < data[0].Length; r++)
        {
            for (int c = 0; c < data[r].Length; c++)
            {
                count += CheckWord(data, c, r, word, 1, 0) ? 1 : 0; // right
                count += CheckWord(data, c, r, word, 1, 1) ? 1 : 0; // down-right
                count += CheckWord(data, c, r, word, 0, 1) ? 1 : 0; // down
                count += CheckWord(data, c, r, word, -1, 1) ? 1 : 0; // down-left
                count += CheckWord(data, c, r, word, -1, 0) ? 1 : 0; // left
                count += CheckWord(data, c, r, word, -1, -1) ? 1 : 0; // up-left
                count += CheckWord(data, c, r, word, 0, -1) ? 1 : 0; // up
                count += CheckWord(data, c, r, word, 1, -1) ? 1 : 0; // up-right
            }
        }
        result.Result = count;
        return result;
    }

    public override async Task<PuzzleResult?> RunImplementation2(char[][] data)
    {
        var word = "MAS";
        var result = new PuzzleResult();
        var count = 0;
        for (int r = 1; r < data[0].Length - 1; r++)
        {
            for (int c = 1; c < data[r].Length - 1; c++)
            {
                if ((CheckWord(data, c - 1, r - 1, word, 1, 1) || CheckWord(data, c + 1, r + 1, word, -1, -1)) &&
                    (CheckWord(data, c + 1, r - 1, word, -1, 1) || CheckWord(data, c - 1, r + 1, word, 1, -1)))
                {
                    count++;
                }

            }
        }
        result.Result = count;
        return result;
    }
}
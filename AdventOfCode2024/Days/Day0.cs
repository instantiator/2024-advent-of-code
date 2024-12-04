using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days
{
    public class Day0 : AbstractDay<string>
    {
        public override string DataFilename => "00-self-test.txt";

        public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(string data)
        {
            return new Tuple<bool, string?>(true, null);
        }

        public override async Task<string> ParseDataAsync(IEnumerable<string> data)
        {
            return string.Join(" ", data);
        }

        public override async Task<PuzzleResult?> RunImplementation1(string data)
        {
            var result = new PuzzleResult();
            result.Add(data);
            return result;
        }

        public override async Task<PuzzleResult?> RunImplementation2(string data)
        {
            var result = new PuzzleResult();
            result.Add(data);
            return result;
        }
    }
}
using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days
{
    public abstract class AbstractDay<D>
    {
        public async Task<PuzzleResult?> Run(int part)
        {
            var data = await LoadDataAsync();
            if (part == 1) { return await RunImplementation1(data); }
            if (part == 2) { return await RunImplementation2(data); }
            throw new Exception("Invalid part");
        }


        public async Task<D> LoadDataAsync()
        {
            Console.Write("Loading data... ");
            var binPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var binDir = System.IO.Path.GetDirectoryName(binPath)!;
            var data = await File.ReadAllLinesAsync(Path.Combine(binDir, "Data", DataFilename));
            Console.WriteLine($"{data.Count()} lines loaded.");
            Console.Write("Parsing data... ");
            var parsed = await ParseDataAsync(data);
            Console.WriteLine("Parsed.");
            Console.Write("Checking data... ");
            var (ok, message) = await CheckParsedDataAsync(parsed);
            if (ok)
            {
                Console.WriteLine("Ready.");
                return parsed;
            }
            else
            {
                Console.WriteLine($"Error: {message}");
                throw new Exception("Data check failed");
            }
        }

        public abstract string DataFilename { get; }
        public abstract Task<D> ParseDataAsync(IEnumerable<string> data);
        public abstract Task<Tuple<bool, string?>> CheckParsedDataAsync(D data);
        public abstract Task<PuzzleResult?> RunImplementation1(D data);
        public abstract Task<PuzzleResult?> RunImplementation2(D data);
    }
}
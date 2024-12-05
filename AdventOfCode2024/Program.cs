using AdventOfCode2024.Days;

namespace AdventOfCode2024
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (args.Length != 2) { PrintUsage(); return 1; }
            var dayOk = int.TryParse(args[0], out var day);
            var partOk = int.TryParse(args[1], out var part);
            Console.WriteLine($"Day: {day}, Part: {part}");
            await RunDay(day, part);
            return 0;
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Parameters: <day> <part>");
        }

        public static async Task RunDay(int day, int part)
        {
            var result = day switch
            {
                0 => await new Day0().Run(0),
                1 => await new Day1().Run(part),
                2 => await new Day2().Run(part),
                3 => await new Day3().Run(part),
                _ => throw new NotImplementedException($"Day {day} not implemented")
            };

            Console.WriteLine();
            Console.WriteLine($"Result: {result?.Result}");
        }
    }
}
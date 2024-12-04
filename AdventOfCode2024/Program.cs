namespace AdventOfCode2024
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 2) { PrintUsage(); return 1; }
            var dayOk = int.TryParse(args[0], out var day);
            var partOk = int.TryParse(args[1], out var part);


            return 0;
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Parameters: <day> <part>");
        }
    }
}
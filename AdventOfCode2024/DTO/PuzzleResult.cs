namespace AdventOfCode2024.DTO;

public class PuzzleResult
{
    public void Add(string output) { Output.Add(output); Console.WriteLine(output); }
    public List<string> Output { get; } = new List<string>();
    public long? Result { get; set; } = null;
}
using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class Day6Guard
{
    public int Row;
    public int Col;
    public char Direction;

    public bool ClearAhead(char[][] map)
    {
        var theoreticalNext = Move();
        return map[theoreticalNext.Row][theoreticalNext.Col] != '#';
    }

    public Day6Guard Move()
    {
        var dc = Direction == '>' ? 1 : Direction == '<' ? -1 : 0;
        var dr = Direction == 'v' ? 1 : Direction == '^' ? -1 : 0;
        return new Day6Guard() { Row = Row + dr, Col = Col + dc, Direction = Direction };
    }

    public Day6Guard RotateClockwise()
    {
        return new Day6Guard() { Row = Row, Col = Col, Direction = ClockwiseFrom(Direction) };
    }

    public char ClockwiseFrom(char dir)
    {
        switch (dir)
        {
            case '^': return '>';
            case '>': return 'v';
            case 'v': return '<';
            case '<': return '^';
            default: throw new InvalidOperationException();
        }
    }

    public string PositionString => $"{Row},{Col}";
}

public class Day6 : AbstractDay<char[][]>
{
    public override string DataFilename => "06-guard-gallivant.txt";

    public override async Task<char[][]> ParseDataAsync(IEnumerable<string> data)
    {
        return data
            .Where(row => !string.IsNullOrWhiteSpace(row))
            .Select(row => row.Trim().ToArray<char>())
            .ToArray<char[]>();
    }

    private bool IsDirection(char c) => new[] { '^', '>', 'v', '<' }.Contains(c);

    public override async Task<Tuple<bool, string?>> CheckParsedDataAsync(char[][] data)
    {
        var nonEmpty = data.All(row => row.Length > 0);
        var sameLength = data.All(row => row.Length == data[0].Length);
        var oneStart = data.Sum(row => row.Count(c => IsDirection(c))) == 1;
        return new Tuple<bool, string?>(nonEmpty && sameLength && oneStart, "All rows must be the same length, only 1 start position");
    }

    public override async Task<PuzzleResult?> RunImplementation1(char[][] data)
    {
        var startRow = data.TakeWhile(row => row.All(c => !IsDirection(c))).Count();
        var startCol = data[startRow].TakeWhile(c => !IsDirection(c)).Count();

        var history = new List<Day6Guard>();

        var guard = new Day6Guard() { Row = startRow, Col = startCol, Direction = data[startRow][startCol] };
        history.Add(guard);

        while (!AtEdge(guard.Row, guard.Col, data))
        {
            while (!guard.ClearAhead(data)) { guard = guard.RotateClockwise(); }
            guard = guard.Move();
            history.Add(guard);
        }
        var distinctPositions = history.Select(g => g.PositionString).Distinct().Count();
        return new PuzzleResult() { Result = distinctPositions };
    }

    private bool AtEdge(int row, int col, char[][] map) => row == 0 || row == map.Length - 1 || col == 0 || col == map[0].Length - 1;

    public override async Task<PuzzleResult?> RunImplementation2(char[][] data)
    {
        // opportunities occur when:
        // 1. a guard is crossing their own path, and
        // 2. a single rotation would match the previous direction they were taking

        var startRow = data.TakeWhile(row => row.All(c => !IsDirection(c))).Count();
        var startCol = data[startRow].TakeWhile(c => !IsDirection(c)).Count();

        var history = new List<Day6Guard>();
        var opportunities = new List<Day6Guard>();

        var guard = new Day6Guard() { Row = startRow, Col = startCol, Direction = data[startRow][startCol] };
        history.Add(guard);

        while (!AtEdge(guard.Row, guard.Col, data))
        {
            while (!guard.ClearAhead(data)) { guard = guard.RotateClockwise(); }
            guard = guard.Move();

            // search the history for a guard that was already in this position with a single rotation
            var afterRotation = guard.RotateClockwise();
            if (history.Any(g => g.Col == afterRotation.Col && g.Row == afterRotation.Row && g.Direction == afterRotation.Direction))
            {
                opportunities.Add(guard.Move());
            }

            history.Add(guard);
        }

        var distinctPositions = opportunities.Select(g => g.PositionString).Distinct().Count();
        return new PuzzleResult() { Result = distinctPositions };
    }
}
using AdventOfCode2024.DTO;

namespace AdventOfCode2024.Days;

public class Guard
{
    public int Row;
    public int Col;
    public char Direction;

    public bool ClearAhead(char[][] map)
    {
        var theoreticalNext = Move();
        return map[theoreticalNext.Row][theoreticalNext.Col] != '#';
    }

    public Guard Move()
    {
        var dc = Direction == '>' ? 1 : Direction == '<' ? -1 : 0;
        var dr = Direction == 'v' ? 1 : Direction == '^' ? -1 : 0;
        if (dc == 0 && dr == 0) { throw new Exception("Guard direction unrecognised"); }
        return new Guard() { Row = Row + dr, Col = Col + dc, Direction = Direction };
    }

    public Guard RotateUntilClear(char[][] data)
    {
        var guard = this;
        var rotations = 0;
        if (Day6.AtEdge(guard, data))
        {
            return guard;
        }
        while (!guard.ClearAhead(data) && rotations < 4)
        {
            guard = guard.RotateClockwise();
            rotations++;
        }
        if (rotations == 4) { throw new Exception("Guard is trapped"); }
        return guard;
    }


    public Guard RotateClockwise()
    {
        return new Guard() { Row = Row, Col = Col, Direction = ClockwiseFrom(Direction) };
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

    public bool Matches(Guard other)
    {
        return this.Row == other.Row && this.Col == other.Col && this.Direction == other.Direction;
    }
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
        var start = FindStartRowCol(data);
        var startOk = IsDirection(data[start.Item1][start.Item2]);
        return new Tuple<bool, string?>(nonEmpty && sameLength && oneStart && startOk, "All rows must be the same length, only 1 start position");
    }

    private Tuple<int, int> FindStartRowCol(char[][] data)
    {
        var startRow = data.TakeWhile(row => row.All(c => !IsDirection(c))).Count();
        var startCol = data[startRow].TakeWhile(c => !IsDirection(c)).Count();
        return new Tuple<int, int>(startRow, startCol);
    }

    private struct PathCrossCheckResult
    {
        public IEnumerable<Guard> Path;
        public bool CrossesPrevious;
    }

    private PathCrossCheckResult TracePath(char[][] data, Guard start, IEnumerable<Guard>? previousPath = null)
    {
        var current = start.RotateUntilClear(data);
        var path = new List<Guard>();
        if (previousPath != null) { path.AddRange(previousPath); }
        path.Add(current);
        while (!AtEdge(current, data))
        {
            current = current.Move();
            current = current.RotateUntilClear(data);

            if (previousPath != null && (path.Any(g => g.Matches(current))))
            { 
                return new PathCrossCheckResult() { Path = path, CrossesPrevious = true };
            }

            path.Add(current);
        }
        return new PathCrossCheckResult() { Path = path, CrossesPrevious = false };
    }

    public override async Task<PuzzleResult?> RunImplementation1(char[][] data)
    {
        var start = FindStartRowCol(data);
        var guard = new Guard() { Row = start.Item1, Col = start.Item2, Direction = data[start.Item1][start.Item2] };
        var path = TracePath(data, guard);
        var distinctPositions = path.Path.Select(g => g.PositionString).Distinct().Count();
        return new PuzzleResult() { Result = distinctPositions };
    }

    public static bool AtEdge(Guard guard, char[][] map)
        => guard.Row == 0
        || guard.Row == map.Length - 1
        || guard.Col == 0
        || guard.Col == map[0].Length - 1;

    public override async Task<PuzzleResult?> RunImplementation2(char[][] data)
    {
        // opportunities occur when:
        // a single rotation would cause the guard to rejoin their path if they continue following the rules
        // place obstacles directly in front of the guard to identify these opportunities

        var start = FindStartRowCol(data);
        var initial = new Guard() { Row = start.Item1, Col = start.Item2, Direction = data[start.Item1][start.Item2] };
        var current = initial.RotateUntilClear(data);
        var path = new List<Guard>() { current };

        var opportunities = new List<Guard>();
        while (!AtEdge(current, data))
        {
            var nextStep = current.Move();
            if (data[nextStep.Row][nextStep.Col] != '#')
            {
                var newData = data.Select(row => row.ToArray()).ToArray();
                newData[nextStep.Row][nextStep.Col] = '#';
                var projected = TracePath(newData, current, path);
                if (projected.CrossesPrevious)
                {
                    opportunities.Add(nextStep);
                }
                Console.Write(projected.CrossesPrevious ? '*' : '.');
            }
            
            current = current.Move();
            current = current.RotateUntilClear(data);
            path.Add(current);
        }

        var distinctOpportunities = opportunities.Select(g => g.PositionString).Distinct().Count();
        return new PuzzleResult() { Result = distinctOpportunities };
    }
}
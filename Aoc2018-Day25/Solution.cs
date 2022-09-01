using System.Text.RegularExpressions;

namespace Aoc2018_Day25;

internal class Solution
{
    public string Title => "Day 25: Four-Dimensional Adventure";

    public object PartOne()
    {
        var positions = ReadPositions();

        var nextConstellationId = 1;
        var constellationMap = new Dictionary<Position, int>();

        for (var i1 = 0; i1 < positions.Count; i1++)
        for (var i2 = 1; i2 < positions.Count; i2++)
        {
            var distance = CalculateManhattanDistance(positions[i1], positions[i2]);
            if (distance <= 3)
            {
                var (p1, p2) = (positions[i1], positions[i2]);
                var (t1, t2) = (constellationMap.GetValueOrDefault(p1), constellationMap.GetValueOrDefault(p2));

                switch (t1, t2)
                {
                    case (0, 0):
                        constellationMap[p2] = constellationMap[p1] = nextConstellationId++;
                        break;
                    case (>0, 0):
                        constellationMap[p2] = t1;
                        break;
                    case (0, >0):
                        constellationMap[p1] = t2;
                        break;
                    case (>0, >0):
                        foreach (var c in constellationMap.Where(t => t.Value == t2))
                        {
                            constellationMap[c.Key] = t1;
                        }
                        break;
                }
            }
        }

        var constellationCount = constellationMap.GroupBy(t => t.Value).Count();
        var singletonCount = positions.Count(p => !constellationMap.ContainsKey(p));
        return constellationCount + singletonCount;
    }

    public object PartTwo()
    {
        return "Merry Christmas";
    }

    private static int CalculateManhattanDistance(Position p1, Position p2)
        => Math.Abs(p1.X - p2.X) +
           Math.Abs(p1.Y - p2.Y) +
           Math.Abs(p1.Z - p2.Z) +
           Math.Abs(p1.T - p2.T);

    private static IReadOnlyList<Position> ReadPositions()
        => InputFile.ReadAllLines()
                    .Where(Position.CanRead)
                    .Select(Position.Read)
                    .ToList()
                    .AsReadOnly();
}

internal readonly record struct Position(int X, int Y, int Z, int T)
{
    public static implicit operator Position((int X, int Y, int Z, int T) position)
        => new(position.X, position.Y, position.Z, position.T);
    
    private static readonly Regex Pattern = new(@"^(?<X>-?\d+),\s*(?<Y>-?\d+),\s*(?<Z>-?\d+),\s*(?<T>-?\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public static bool CanRead(string input) => Pattern.IsMatch(input);

    public static Position Read(string input)
    {
        var match = Pattern.Match(input);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        return new(Convert.ToInt32(match.Groups["X"].Value),
                   Convert.ToInt32(match.Groups["Y"].Value),
                   Convert.ToInt32(match.Groups["Z"].Value),
                   Convert.ToInt32(match.Groups["T"].Value));
    }
}

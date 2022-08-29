using System.Text;

namespace Aoc2018_Day20;

internal class Map
{
    private readonly Dictionary<Position, char> _data;

    public Map(Dictionary<Position, char> data)
    {
        _data = data;
    }

    public Dictionary<Position, int> CalculateDistancesToEachRoom()
    {
        var distancesByPosition = new Dictionary<Position, int>{ [(0, 0)] = 0 };
        var queue = new PriorityQueue<Position, int>();
        queue.Enqueue((0, 0), 0);

        while (queue.Count > 0)
        {
            var room = queue.Dequeue();
            foreach (var neighbour in GetAdjoiningRoomPositions(room))
            {
                var alt = distancesByPosition[room] + 1;
                if (alt < distancesByPosition.GetValueOrDefault(neighbour, int.MaxValue))
                {
                    distancesByPosition[neighbour] = alt;
                    queue.Enqueue(neighbour, alt);
                }
            }
        }

        return distancesByPosition;
    }

    private IEnumerable<Position> GetAdjoiningRoomPositions(Position position)
    {
        var (x, y) = position;
        if (_data[(x - 1, y)] == '|') yield return (x - 2, y);
        if (_data[(x + 1, y)] == '|') yield return (x + 2, y);
        if (_data[(x, y - 1)] == '-') yield return (x, y - 2);
        if (_data[(x, y + 1)] == '-') yield return (x, y + 2);
    }

    public override string ToString()
    {
        var (minX, minY, maxX, maxY) = (_data.Keys.Min(p => p.X), _data.Keys.Min(p => p.Y), _data.Keys.Max(p => p.X), _data.Keys.Max(p => p.Y));
        var result = new StringBuilder();
    
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var tile = _data.GetValueOrDefault((x, y), ' ');
                result.Append(tile);
            }
    
            if (y < maxY) result.AppendLine();
        }
    
        return result.ToString();
    }

    public static Map ReadMap()
    {
        var expression = InputFile.ReadAllText()
                                  .Trim();
        var map = new MapBuilder();
        var junctions = new Stack<Position>();
        foreach (var ch in expression)
        {
            switch (ch)
            {
                case '^':
                    break;
                case 'N':
                    map.North();
                    break;
                case 'E':
                    map.East();
                    break;
                case 'W':
                    map.West();
                    break;
                case 'S':
                    map.South();
                    break;
                case '(':
                    junctions.Push(map.CurrentPosition);
                    break;
                case '|':
                    map.SetPosition(junctions.Pop());
                    junctions.Push(map.CurrentPosition);
                    break;
                case ')':
                    map.SetPosition(junctions.Pop());
                    break;
                case '$':
                    return map.Build();
                default:
                    throw new FormatException($"Unexpected character {ch} in expression.");
            }
        }
        throw new FormatException($"End character not found in expression.");
    }
}

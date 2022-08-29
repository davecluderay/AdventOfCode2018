namespace Aoc2018_Day22;

internal class Cavern
{
    private readonly int _depth;
    private readonly Position _mouth;
    private readonly Position _target;

    private readonly Dictionary<Position, Region> _layoutCache = new();

    public Cavern(int depth, Position mouth, Position target)
    {
        _depth = depth;
        _mouth = mouth;
        _target = target;
    }

    public int CalculateRiskLevel()
    {
        var riskLevel = 0;
        var (minX, minY, maxX, maxY) = (Math.Min(_mouth.X, _target.X),
                                        Math.Min(_mouth.Y, _target.Y),
                                        Math.Max(_mouth.X, _target.X),
                                        Math.Max(_mouth.Y, _target.Y));
        for (var y = minY; y <= maxY; y++)
        for (var x = minX; x <= maxX; x++)
        {
            riskLevel += GetRegion((x, y)).Type switch { '=' => 1, '|' => 2, _ => 0 };
        }

        return riskLevel;
    }

    public int CalculateFastestTimeToTarget()
    {
        var dist = new Dictionary<(Position Position, Equipment Equipment), int>();
        var queue = new PriorityQueue<(Position Position, Equipment Equipment), int>();

        dist.Add((_mouth, Equipment.Torch), 0);
        foreach (var option in GetMovementOptions(_mouth, Equipment.Torch))
        {
            dist.Add((option.Position, option.Equipment), option.Cost);
            queue.Enqueue((option.Position, option.Equipment), option.Cost);
        }

        while (queue.Count > 0)
        {
            var u = queue.Dequeue();
            foreach (var v in GetMovementOptions(u.Position, u.Equipment))
            {
                var alt = dist[(u.Position, u.Equipment)] + v.Cost;
                if (alt < dist.GetValueOrDefault((v.Position, v.Equipment), int.MaxValue))
                {
                    dist[(v.Position, v.Equipment)] = alt;
                    if (dist.GetValueOrDefault((_target, Equipment.Torch), int.MaxValue) >= alt)
                        queue.Enqueue((v.Position, v.Equipment), alt);
                }
            }
        }

        return dist[(_target, Equipment.Torch)];
    }

    private Region GetRegion(Position position)
    {
        if (!_layoutCache.ContainsKey(position))
        {
            var geologicIndex = CalculateGeologicIndex(position);
            var erosionLevel = CalculateErosionLevel(geologicIndex, _depth);
            var type = IdentifyType(erosionLevel);
            _layoutCache[position] = new Region(erosionLevel, type);
        }
        return _layoutCache[position];
    }

    private int CalculateGeologicIndex(Position position)
    {
        var (x, y) = position;
        if (position == _mouth || position == _target) return 0;
        if (y == 0) return unchecked(16807 * x);
        if (x == 0) return unchecked(48271 * y);
        return unchecked(GetRegion((x - 1, y)).ErosionLevel * GetRegion((x, y - 1)).ErosionLevel);
    }

    private int CalculateErosionLevel(int geologicIndex, int depth)
    {
        return (geologicIndex + depth) % 20183;
    }

    private char IdentifyType(int erosionLevel)
    {
        return (erosionLevel % 3) switch { 0 => '.', 1 => '=', 2 => '|', _ => ' ' };
    }

    private IEnumerable<(Position Position, Equipment Equipment, int Cost)> GetMovementOptions(Position from, Equipment equipped)
    {
        foreach (var to in GetAdjacentPositions(from))
        {
            var equip = SelectEquipment(from, equipped, to);

            var cost = equip == equipped ? 1 : 8;
            if (to == _target && equip != Equipment.Torch)
            {
                equip = Equipment.Torch;
                cost += 7;
            }

            yield return (to, equip, cost);
        }
    }

    private static IEnumerable<Position> GetAdjacentPositions(Position position)
    {
        if (position.X > 0)
            yield return (position.X - 1, position.Y);
        if (position.Y > 0)
            yield return (position.X, position.Y - 1);
        yield return (position.X + 1, position.Y);
        yield return (position.X, position.Y + 1);
    }

    private Equipment SelectEquipment(Position from, Equipment currentEquipment, Position to)
    {
        var (fromType, toType) = (GetRegion(from).Type, GetRegion(to).Type);
        if (fromType == toType) return currentEquipment;

        switch (fromType, toType)
        {
            case ('.', '|'):
            case ('|', '.'):
                return Equipment.Torch;
            case ('.', '='):
            case ('=', '.'):
                return Equipment.ClimbingGear;
            case ('=', '|'):
            case ('|', '='):
                return Equipment.Neither;
            default:
                return currentEquipment;
        }
    }
}

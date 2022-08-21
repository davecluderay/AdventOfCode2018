using System.Text;

namespace Aoc2018_Day15;

internal class Cavern
{
    private readonly HashSet<Position> _layout;
    private readonly Dictionary<Position, Unit> _units;
    private readonly Bounds _bounds;

    private Cavern(HashSet<Position> layout, Dictionary<Position, Unit> units)
        => (_layout, _units, _bounds) = (layout, units, CalculateBounds(layout));

    public override string ToString()
    {
        var result = new StringBuilder();

        for (var y = _bounds.Min.Y - 1; y <= _bounds.Max.Y + 1; y++)
        {
            var units = new List<Unit>();
            for (var x = _bounds.Min.X - 1; x <= _bounds.Max.X + 1; x++)
            {
                var layoutTile = _layout.Contains((x, y)) ? '.' : '#';
                var unit = _units.GetValueOrDefault((x, y));
                var tile = unit?.Value ?? layoutTile;
                result.Append(tile);
                if (unit is not null)
                    units.Add(unit);
            }

            if (units.Any())
            {
                result.Append("   ").AppendJoin(", ", units.Select(u => $"{u.Value}({u.HitPoints})"));
            }

            if (y <= _bounds.Max.Y)
                result.AppendLine();
        }

        return result.ToString();
    }

    public Unit[] GetRemainingElfUnits()
        => GetRemainingUnits().Where(u => u.Value == 'E')
                              .ToArray();

    public Unit[] GetRemainingUnits()
        => _units.Values
                 .OrderBy(u => (u.Position.Y, u.Position.X))
                 .ToArray();
    
    public Unit[] GetRemainingEnemyUnits(Unit unit)
        => _units.Values
                 .Where(u => u.Value != unit.Value)
                 .OrderBy(u => (u.Position.Y, u.Position.X))
                 .ToArray();

    public void RemoveUnit(Unit enemy)
        => _units.Remove(enemy.Position);

    public bool IsEmptySpace(Position position)
        => _layout.Contains(position) && !_units.ContainsKey(position);

    public void MoveUnit(Unit unit, Position to)
    {
        var from = unit.Position;
        _units.Remove(from);
        unit.Position = to;
        _units[to] = unit;
    }

    public static Cavern ReadInitialState(int elfAttackPower = 3)
    {
        var layout = new HashSet<Position>();
        var units = new Dictionary<Position, Unit>();
        
        var lines = InputFile.ReadAllLines();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            var tile = lines[y][x];
            if (tile != '#')
                layout.Add((x, y));
            if (tile is 'E')
                units[(x, y)] = new Unit((x, y), tile, elfAttackPower);
            if (tile is 'G')
                units[(x, y)] = new Unit((x, y), tile, 3);
        }

        return new Cavern(layout, units);
    }

    private static Bounds CalculateBounds(HashSet<Position> layout)
        => new(new Position(layout.Min(k => k.X), layout.Min(k => k.Y)),
               new Position(layout.Max(k => k.X), layout.Max(k => k.Y)));
    
    private record struct Bounds(Position Min, Position Max);
}

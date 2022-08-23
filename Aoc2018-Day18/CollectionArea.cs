using System.Text;

namespace Aoc2018_Day18;

internal class CollectionArea
{
    private readonly Dictionary<Position, char> _layout;
    private readonly Dictionary<Position, char> _uncommitted = new();

    public Bounds Bounds { get; }

    private CollectionArea(Dictionary<Position, char> layout)
    {
        _layout = layout;
        Bounds = new Bounds((layout.Keys.Min(k => k.X), layout.Keys.Min(k => k.Y)),
                            (layout.Keys.Max(k => k.X), layout.Keys.Max(k => k.Y)));

    }

    public char GetAt(Position position)
        => _layout.GetValueOrDefault(position);

    public void SetOnCommit(Position position, char contents)
        => _uncommitted[position] = contents;

    public void Commit()
    {
        foreach (var change in _uncommitted)
        {
            _layout[change.Key] = change.Value;
        }
        _uncommitted.Clear();
    }

    public int CalculateResourceValue()
        => _layout.Count(t => t.Value == '|') * _layout.Count(t => t.Value == '#');

    public int CalculateLayoutHash()
        => ToString().GetHashCode();

    public override string ToString()
    {
        var result = new StringBuilder();

        for (var y = Bounds.Min.Y; y <= Bounds.Max.Y; y++)
        {
            for (var x = Bounds.Min.X; x <= Bounds.Max.X; x++)
            {
                result.Append(_layout[(x, y)]);
            }

            if (y < Bounds.Max.Y)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }

    public static CollectionArea ReadInitialLayout()
    {
        var layout = new Dictionary<Position, char>();

        var lines = InputFile.ReadAllLines();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            layout[(x, y)] = lines[y][x];
        }

        return new CollectionArea(layout);
    }
}
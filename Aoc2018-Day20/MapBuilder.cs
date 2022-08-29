namespace Aoc2018_Day20;

internal class MapBuilder
{
    public Position CurrentPosition { get; private set; }
    private readonly Dictionary<Position, char> _data = new();

    public MapBuilder()
    {
        SetPosition((0, 0));
    }

    public void North()
    {
        _data[CurrentPosition with { Y = CurrentPosition.Y - 1 }] = '-';
        SetPosition(CurrentPosition with { Y = CurrentPosition.Y - 2 });
    }

    public void East()
    {
        _data[CurrentPosition with { X = CurrentPosition.X + 1 }] = '|';
        SetPosition(CurrentPosition with { X = CurrentPosition.X + 2 });
    }

    public void South()
    {
        _data[CurrentPosition with { Y = CurrentPosition.Y + 1 }] = '-';
        SetPosition(CurrentPosition with { Y = CurrentPosition.Y + 2 });
    }

    public void West()
    {
        _data[CurrentPosition with { X = CurrentPosition.X - 1 }] = '|';
        SetPosition(CurrentPosition with { X = CurrentPosition.X - 2 });
    }

    public void SetPosition(Position position)
    {
        var (x, y) = position;

        _data[(x, y)] = _data.Count == 0 ? 'X' : '.';

        _data[(x - 1, y - 1)] = '#';
        _data[(x - 1, y + 1)] = '#';
        _data[(x + 1, y - 1)] = '#';
        _data[(x + 1, y + 1)] = '#';

        if (!_data.ContainsKey((x - 1, y))) _data[(x - 1, y)] = '?';
        if (!_data.ContainsKey((x, y - 1))) _data[(x, y - 1)] = '?';
        if (!_data.ContainsKey((x + 1, y))) _data[(x + 1, y)] = '?';
        if (!_data.ContainsKey((x, y + 1))) _data[(x, y + 1)] = '?';
        
        CurrentPosition = position;
    }

    public Map Build()
    {
        var uncertainPositions = _data.Where(entry => entry.Value == '?')
                                      .Select(entry => entry.Key);
        foreach (var position in uncertainPositions)
        {
            _data[position] = '#';
        }

        return new Map(_data);
    }
}

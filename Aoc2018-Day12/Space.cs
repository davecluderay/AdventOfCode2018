using System.Text;

namespace Aoc2018_Day12;

internal class Space
{
    private const char EmptyValue = '.';
        
    private Dictionary<long, char> _spaceData = new();
    private Dictionary<long, char> _uncommitted = new();
    private (long Low, long High)? _bounds;
        
    public Space(string initialData)
    {
        foreach (var item in initialData.Select((c, i) => (Value: c, Index: i)))
        {
            if (item.Value == EmptyValue) continue;
            _spaceData[item.Index] = item.Value;
        }
    }

    public (long Low, long High) GetBounds()
    {
        if (_bounds == null)
        {
            var low = long.MaxValue;
            var high = long.MinValue;
            foreach (var item in _spaceData)
            {
                low = Math.Min(low, item.Key);
                high = Math.Max(high, item.Key);
            }
            _bounds = (low, high);
        }
        return _bounds.Value;
    }

    public char GetAt(long position)
    {
        return _spaceData.GetValueOrDefault(position, EmptyValue);
    }

    public IEnumerable<long> GetNonEmptyPositions()
    {
        return _spaceData.Where(x => x.Value != EmptyValue)
                         .Select(x => x.Key);
    }

    public void SetOnCommit(long position, char value)
    {
        if (GetAt(position) != value)
        {
            _uncommitted[position] = value;
        }
    }

    public void Commit()
    {
        foreach (var item in _uncommitted)
        {
            if (item.Value == EmptyValue && _spaceData.ContainsKey(item.Key))
            {
                _spaceData.Remove(item.Key);
                ResetBounds();
            }

            if (item.Value != EmptyValue)
            {
                _spaceData[item.Key] = item.Value;
                ResetBounds();
            }
        }
        _uncommitted.Clear();
    }

    public void Shift(long shift)
    {
        _spaceData = _spaceData.ToDictionary(x => x.Key + shift, x => x.Value);
        _uncommitted = _uncommitted.ToDictionary(x => x.Key + shift, x => x.Value);
        ResetBounds();
    }

    private void ResetBounds()
    {
        _bounds = null;
    }

    public string Render(long startIndex, int count)
    {
        var result = new StringBuilder(count);
        for (var i = startIndex; i < startIndex + count; i++)
            result.Append(GetAt(i));
        return result.ToString();
    }

    public (long StartIndex, string Output) Render()
    {
        var bounds = GetBounds();
        return (bounds.Low - 1,
                Render(startIndex: bounds.Low - 1,
                       count: (int)(bounds.High - bounds.Low + 2)));
    }
}

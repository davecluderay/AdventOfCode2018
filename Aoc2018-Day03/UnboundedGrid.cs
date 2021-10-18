using System.Collections.Generic;
using System.Linq;

namespace Aoc2018_Day03
{
    internal class UnboundedGrid<T>
    {
        private readonly Dictionary<(int X, int Y), T> _data = new Dictionary<(int X, int Y), T>();

        public void Set((int X, int Y) position, T value) => _data[position] = value;
        public T? Get((int X, int Y) position) => _data.TryGetValue(position, out var value) ? value : default;

        public (int X, int Y)[] GetOccupiedPositions() => _data.Keys.ToArray();
    }
}

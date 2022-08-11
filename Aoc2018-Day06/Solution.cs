namespace Aoc2018_Day06
{
    internal class Solution
    {
        public string Title => "Day 6: Chronal Coordinates";

        public object PartOne()
        {
            var coordinates = ReadCoordinates();
            var bounds = CalculateBounds(coordinates);

            var nearestCoordinatesByPosition = new Dictionary<Location, Location?>();
            var coordinatesWithInfiniteZones = new HashSet<Location>();

            for (var y = bounds.Min.Y; y <= bounds.Max.Y; y++)
            for (var x = bounds.Min.X; x <= bounds.Max.X; x++)
            {
                var position = new Location(x, y);
                var nearest = FindNearest(position, coordinates);
                nearestCoordinatesByPosition[position] = nearest;
                if (nearest is not null && IsEdge(position, bounds))
                {
                    coordinatesWithInfiniteZones.Add(nearest);
                }
            }

            return nearestCoordinatesByPosition
                   .Where(x => x.Value is not null &&
                               !coordinatesWithInfiniteZones.Contains(x.Value))
                   .GroupBy(x => x.Value)
                   .Max(x => x.Count());
        }

        public object PartTwo()
        {
            var coordinates = ReadCoordinates();
            var bounds = CalculateBounds(coordinates);

            var safeZoneSize = 0;

            for (var y = bounds.Min.Y; y <= bounds.Max.Y; y++)
            for (var x = bounds.Min.X; x <= bounds.Max.X; x++)
            {
                var position = new Location(x, y);
                if (IsInSafeZone(position, coordinates, 10000))
                {
                    safeZoneSize++;
                }
            }

            return safeZoneSize;
        }

        private Location? FindNearest(Location position, IEnumerable<Location> coordinates)
        {
            var ordered = coordinates.Select(coord => (coord, distance: CalculateManhattanDistance(coord, position)))
                                     .OrderBy(x => x.distance)
                                     .Take(2)
                                     .ToList();
            return ordered[0].distance == ordered[1].distance
                       ? null
                       : ordered[0].coord;
        }

        private bool IsInSafeZone(Location position, IEnumerable<Location> coordinates, int threshold)
        {
            var total = 0;
            foreach (var coord in coordinates)
            {
                total += CalculateManhattanDistance(position, coord);
                if (total >= threshold)
                    return false;
            }
            return true;
        }

        private static IReadOnlyList<Location> ReadCoordinates()
            => InputFile.ReadAllLines()
                        .Select(x => x.Split(','))
                        .Select(x => new Location(Convert.ToInt32(x.First()),
                                                  Convert.ToInt32(x.Last())))
                        .ToList();

        private static Bounds CalculateBounds(IReadOnlyList<Location> coordinates)
            => new(new Location(coordinates.Min(c => c.X), coordinates.Min(c => c.Y)),
                   new Location(coordinates.Max(c => c.X), coordinates.Max(c => c.Y)));

        private int CalculateManhattanDistance(Location first, Location second)
            => Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);

        private bool IsEdge(Location position, Bounds bounds)
            => position.X == bounds.Min.X ||
               position.Y == bounds.Min.Y ||
               position.X == bounds.Max.X ||
               position.Y == bounds.Max.Y;

        private record Location(int X, int Y);
        private record Bounds(Location Min, Location Max);
    }
}

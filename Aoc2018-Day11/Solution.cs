namespace Aoc2018_Day11;

internal class Solution
{
    private const int GridSize = 300;

    public string Title => "Day 11: Chronal Charge";

    public object PartOne()
    {
        var serialNumber = ReadSerialNumber();
        var areaSums = CalculateSummedPowerLevelAreas(serialNumber);
        var result = FindSquaresAndTotalPowerLevels(areaSums, fixedSize: 3).MaxBy(l => l.PowerLevel);
        return $"{result.Origin.X},{result.Origin.Y}";
    }

    public object PartTwo()
    {
        var serialNumber = ReadSerialNumber();
        var areaSums = CalculateSummedPowerLevelAreas(serialNumber);
        var result = FindSquaresAndTotalPowerLevels(areaSums).MaxBy(l => l.PowerLevel);
        return $"{result.Origin.X},{result.Origin.Y},{result.Size}";
    }

    private IEnumerable<(Position Origin, int Size, int PowerLevel)> FindSquaresAndTotalPowerLevels(Dictionary<Position, int> areaSums, int? fixedSize = default)
    {
        // Iterate over all possible square positions and sizes.
        var minSize = fixedSize ?? 1;
        for (var x = 0; x <= GridSize - minSize; x++)
        for (var y = 0; y <= GridSize - minSize; y++)
        {
            var maxSize = fixedSize ?? Math.Min(GridSize - x, GridSize - y);
            for (var size = minSize; size <= maxSize; size++)
            {
                var totalPowerLevel = CalculateTotalPowerLevel(areaSums, (x, y), size);
                yield return ((x, y), size, totalPowerLevel);
            }
        }
    }

    private static int CalculateTotalPowerLevel(Dictionary<Position, int> areaSums, Position position, int squareSize)
    {
        // Calculate the area using the pre-calculated summed-area table.
        var (x, y) = position;
        var totalPowerLevel = areaSums[(x + squareSize - 1, y + squareSize - 1)];
        totalPowerLevel -= areaSums.GetValueOrDefault((x - 1, y + squareSize - 1));
        totalPowerLevel -= areaSums.GetValueOrDefault((x + squareSize - 1, y - 1));
        totalPowerLevel += areaSums.GetValueOrDefault((x - 1, y - 1));
        return totalPowerLevel;
    }

    private static Dictionary<Position, int> CalculateSummedPowerLevelAreas(int serialNumber)
    {
        // The value at each position in the result is the sum of all power levels in the
        // rectangle that extends back to the top-left of the grid.
        var result = new Dictionary<Position, int>();
        for (var y = 0; y < GridSize; y++)
        for (var x = 0; x < GridSize; x++)
        {
            var powerLevel = (x * x * y + 20 * x * y + 100 * y + serialNumber * x + 10 * serialNumber) % 1000 / 100 - 5;
            var sum = powerLevel;
            sum += (x > 0 ? result[(x - 1, y)] : 0); 
            sum += (y > 0 ? result[(x, y - 1)] : 0);
            sum -= (x > 0 && y > 0 ? result[(x - 1, y - 1)] : 0);
            result[(x, y)] = sum;
        }
        return result;
    }

    private static int ReadSerialNumber()
        => InputFile.ReadAllLines().Select(s => Convert.ToInt32(s))
                    .First();

    private record struct Position(int X, int Y)
    {
        public static implicit operator Position((int X, int Y) position)
            => new(position.X, position.Y);
    }
}

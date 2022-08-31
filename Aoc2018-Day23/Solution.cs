namespace Aoc2018_Day23;

internal class Solution
{
    public string Title => "Day 23: Experimental Emergency Teleportation";

    public object PartOne()
    {
        var bots = ReadNanobots();
        var strongest = bots.MaxBy(b => b.Range)!;
        return bots.Count(b => IsInRange(strongest.Position, b.Position, strongest.Range));
    }
        
    public object PartTwo()
    {
        const int subdivisions = 8;

        var bots = ReadNanobots().OrderByDescending(b => b.Range)
                                 .ToArray();

        var sampleRegion = new Box(MinX: bots.Min(b => b.Position.X - b.Range),
                                   MinY: bots.Min(b => b.Position.Y - b.Range),
                                   MinZ: bots.Min(b => b.Position.Z - b.Range),
                                   MaxX: bots.Max(b => b.Position.X + b.Range),
                                   MaxY: bots.Max(b => b.Position.Y + b.Range),
                                   MaxZ: bots.Max(b => b.Position.Z + b.Range));
        var sampleSpacing = new[] { sampleRegion.SizeX, sampleRegion.SizeY, sampleRegion.SizeZ }.Max() / subdivisions;

        while (true)
        {
            var optimalPosition = Position.MaxValue;
            var maxInRangeCount = 0;
            var minDistance = int.MaxValue;
            
            for (var x = sampleRegion.MinX; x < sampleRegion.MaxX; x += sampleSpacing)
            for (var y = sampleRegion.MinY; y < sampleRegion.MaxY; y += sampleSpacing)
            for (var z = sampleRegion.MinZ; z < sampleRegion.MaxZ; z += sampleSpacing)
            {
                var inRangeCount = bots.Count(bot => IsInRange(bot.Position, (x, y, z), bot.Range));
                var distance = CalculateManhattanDistance((0, 0, 0), (x, y, z));

                if (inRangeCount > maxInRangeCount || inRangeCount == maxInRangeCount && distance < minDistance)
                {
                    maxInRangeCount = inRangeCount;
                    optimalPosition = (x, y, z);
                    minDistance = distance;
                }
            }

            if (sampleSpacing == 1)
                return minDistance;

            sampleRegion = sampleRegion.WithCentre(optimalPosition)
                                       .ScaledDownBy(2);
            sampleSpacing /= 2;
        }
    }

    private bool IsInRange(Position p1, Position p2, int range)
        => CalculateManhattanDistance(p1, p2) <= range;

    private int CalculateManhattanDistance(Position p1, Position p2)
        => Math.Abs(p1.X - p2.X) +
           Math.Abs(p1.Y - p2.Y) +
           Math.Abs(p1.Z - p2.Z);

    private IReadOnlyCollection<Nanobot> ReadNanobots()
        => InputFile.ReadAllLines()
                    .Where(Nanobot.CanRead)
                    .Select(Nanobot.Read)
                    .ToList()
                    .AsReadOnly();
}

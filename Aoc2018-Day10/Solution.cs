using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018_Day10;

internal class Solution
{
    public string Title => "Day 10: The Stars Align";

    public object PartOne()
    {
        var stars = ReadStars();
        return FindMessage(stars).Message;
    }

    public object PartTwo()
    {
        var stars = ReadStars();
        return FindMessage(stars).TimeTaken;
    }

    private (int TimeTaken, string Message) FindMessage(IReadOnlyList<Star> stars)
    {
        var currentArea = CalculateBoundingBox(stars).Area;
        var time = 0;
        while (true)
        {
            AdjustAlignmentBy(stars, seconds: 1);

            var newArea = CalculateBoundingBox(stars).Area;
            if (newArea > currentArea)
            {
                AdjustAlignmentBy(stars, seconds: -1);
                return (TimeTaken: time, Message: Render(stars));
            }

            time++;
            currentArea = newArea;
        }
    }

    private void AdjustAlignmentBy(IReadOnlyList<Star> stars, int seconds)
    {
        foreach (var star in stars)
        {
            star.Position = new Position(star.Position.X + star.Velocity.X * seconds,
                                         star.Position.Y + star.Velocity.Y * seconds);
        }
    }

    private BoundingBox CalculateBoundingBox(IReadOnlyList<Star> stars)
    {
        var min = new Position(stars.Min(s => s.Position.X), stars.Min(s => s.Position.Y));
        var max = new Position(stars.Max(s => s.Position.X), stars.Max(s => s.Position.Y));
        return new BoundingBox(min, max);
    }

    private IReadOnlyList<Star> ReadStars()
    {
        var pattern = new Regex(@"^position=<\s*(?<PX>-?\d+),\s*(?<PY>-?\d+)> velocity=<\s*(?<VX>-?\d+),\s*(?<VY>-?\d+)>$",
                                RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        var stars = InputFile.ReadAllLines()
                             .Select(s => pattern.Match(s))
                             .Where(m => m.Success)
                             .Select(m => new Star
                                          {
                                              Position = new Position(Convert.ToInt32(m.Groups["PX"].Value),
                                                                      Convert.ToInt32(m.Groups["PY"].Value)),
                                              Velocity = new Velocity(Convert.ToInt32(m.Groups["VX"].Value),
                                                                      Convert.ToInt32(m.Groups["VY"].Value))
                                          })
                             .ToList();
        return stars;
    }

    private string Render(IReadOnlyList<Star> stars)
    {
        var box = CalculateBoundingBox(stars);
        var positions = stars.Select(s => s.Position).ToHashSet();

        var result = new StringBuilder();
        for (var y = box.Min.Y; y <= box.Max.Y; y++)
        {
            result.AppendLine();
            for (var x = box.Min.X; x <= box.Max.X; x++)
            {
                result.Append(positions.Contains(new Position(x, y)) ? '#' : '.');
            }
        }

        return result.ToString();
    }

    private class Star
    {
        public Position Position { get; set; }
        public Velocity Velocity { get; init; }
    }

    private record struct Position(int X, int Y);
    private record struct Velocity(int X, int Y);

    private readonly record struct BoundingBox(Position Min, Position Max)
    {
        public long Area => (Max.X - Min.X + 1L) * (Max.Y - Min.Y + 1L);
    }
}

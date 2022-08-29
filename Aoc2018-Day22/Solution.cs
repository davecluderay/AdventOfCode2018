using System.Text.RegularExpressions;

namespace Aoc2018_Day22;

internal class Solution
{
    public string Title => "Day 22: Mode Maze";

    public object PartOne()
    {
        var (depth, target) = ReadInput();
        var cavern = new Cavern(depth, mouth: (0, 0), target);
        return cavern.CalculateRiskLevel();
    }

    public object PartTwo()
    {
        var (depth, target) = ReadInput();
        var cavern = new Cavern(depth, mouth: (0, 0), target);
        return cavern.CalculateFastestTimeToTarget();
    }

    private (int Depth, Position Target) ReadInput()
    {
        var text = InputFile.ReadAllText();

        var depthMatch = Regex.Match(text, @"depth: (?<Depth>\d+)");
        if (!depthMatch.Success) throw new FormatException("Expected to find depth specifier.");

        var targetMatch = Regex.Match(text, @"target: (?<X>\d+),\s*(?<Y>\d+)");
        if (!targetMatch.Success) throw new FormatException("Expected to find target specifier.");

        var depth = Convert.ToInt32(depthMatch.Groups["Depth"].Value);
        var target = new Position(Convert.ToInt32(targetMatch.Groups["X"].Value),
                                  Convert.ToInt32(targetMatch.Groups["Y"].Value));
        return (depth, target);
    }
}

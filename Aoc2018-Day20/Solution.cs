namespace Aoc2018_Day20;

internal class Solution
{
    public string Title => "Day 20: A Regular Map";

    public object PartOne()
    {
        var map = Map.ReadMap();
        var distancesByPosition = map.CalculateDistancesToEachRoom();
        return distancesByPosition.Values.Max();
    }
        
    public object PartTwo()
    {
        var map = Map.ReadMap();
        var distancesByPosition = map.CalculateDistancesToEachRoom();
        return distancesByPosition.Count(d => d.Value >= 1000);
    }
}

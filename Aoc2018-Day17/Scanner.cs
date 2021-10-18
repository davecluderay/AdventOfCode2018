using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2018_Day17
{
    internal static class Scanner
    {
        public static ((int x, int y) offset, char[,] data) Scan(string? fileName = null)
        {
            var pattern =
                new Regex(@"^" +
                          @"(?<AXIS1>x|y)=(?<AXIS1_VALUE1>\d+)(\.\.(?<AXIS1_VALUE2>\d+))?" +
                          @",\s*" +
                          @"(?<AXIS2>x|y)=(?<AXIS2_VALUE1>\d+)(\.\.(?<AXIS2_VALUE2>\d+))?" +
                          @"$");
            var clayPositions = new HashSet<(int x, int y)>();
            foreach (var line in InputFile.ReadAllLines(fileName))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var match = pattern.Match(line);
                if (!match.Success) throw new Exception($"COULDN'T RECOGNISE LINE: {line}");

                var axis1       = match.Groups["AXIS1"].Value;
                var axis1Value1 = int.Parse(match.Groups["AXIS1_VALUE1"].Value);
                var axis1Value2 = match.Groups["AXIS1_VALUE2"].Success ? int.Parse(match.Groups["AXIS1_VALUE2"].Value) : axis1Value1;
                var axis2Value1 = int.Parse(match.Groups["AXIS2_VALUE1"].Value);
                var axis2Value2 = match.Groups["AXIS2_VALUE2"].Success ? int.Parse(match.Groups["AXIS2_VALUE2"].Value) : axis2Value1;

                var (x1, x2, y1, y2) = axis1 == "x"
                    ? (axis1Value1, axis1Value2, axis2Value1, axis2Value2)
                    : (axis2Value1, axis2Value2, axis1Value1, axis1Value2);

                for (var x = x1; x <= x2; x++)
                for (var y = y1; y <= y2; y++)
                    clayPositions.Add((x, y));
            }

            var minX = clayPositions.Min(p => p.x) - 1;
            var maxX = clayPositions.Max(p => p.x) + 1;
            var minY = clayPositions.Min(p => p.y);
            var maxY = clayPositions.Max(p => p.y);

            var data = new char[maxY - minY + 1, maxX - minX + 1];
            for (var y = minY; y <= maxY; y++)
            for (var x = minX; x <= maxX; x++)
                data[y - minY, x - minX] = clayPositions.Contains((x, y)) ? '#' : '.';

            return ((minX, minY), data);
        }
    }
}

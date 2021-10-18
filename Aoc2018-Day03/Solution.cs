using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2018_Day03
{
    internal class Solution
    {
        public string Title => "Day 3: No Matter How You Slice It";

        public object? PartOne()
        {
            var claims = LoadClaims();
            var material = ApplyClaims(claims);

            return material.GetOccupiedPositions().Count(p => material.Get(p)!.Count() > 1);
        }

        public object? PartTwo()
        {
            var claims = LoadClaims();
            var material = ApplyClaims(claims);

            var conflictingClaimIds = material.GetOccupiedPositions()
                                              .Where(p => material.Get(p)!.Skip(1).Any())
                                              .SelectMany(p => material.Get(p)!)
                                              .Distinct()
                                              .ToArray();

            return claims.Select(c => c.Id)
                         .Single(id => !conflictingClaimIds.Contains(id));
        }

        private Claim[] LoadClaims(string fileName = "input.txt")
        {
            return InputFile.ReadAllLines(fileName)
                            .Select(Claim.Parse)
                            .ToArray();
        }

        private UnboundedGrid<IEnumerable<int>> ApplyClaims(IEnumerable<Claim> claims)
        {
            var material = new UnboundedGrid<IEnumerable<int>>();

            foreach (var claim in claims)
            {
                for (var x = claim.Position.X; x < claim.Position.X + claim.Size.Width; x++)
                for (var y = claim.Position.Y; y < claim.Position.Y + claim.Size.Height; y++)
                {
                    material.Set((x, y), (material.Get((x, y)) ?? Enumerable.Empty<int>()).Append(claim.Id));
                }
            }

            return material;
        }

        private class Claim
        {
            private static readonly Regex Pattern = new Regex(@"^#(?<Id>\d+)\s*@\s*(?<X>\d+),(?<Y>\d+):\s*(?<W>\d+)x(?<H>\d+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

            public int Id { get; }
            public (int X, int Y) Position { get; }
            public (int Width, int Height) Size { get; }

            private Claim(int id, int x, int y, int width, int height)
            {
                Id = id;
                Position = (x, y);
                Size = (width, height);
            }

            public static Claim Parse(string text)
            {
                var match = Pattern.Match(text);
                if (!match.Success) throw new ArgumentException($"Unable to parse from text: {text}");

                return new Claim(Convert.ToInt32(match.Groups["Id"].Value),
                                 Convert.ToInt32(match.Groups["X"].Value),
                                 Convert.ToInt32(match.Groups["Y"].Value),
                                 Convert.ToInt32(match.Groups["W"].Value),
                                 Convert.ToInt32(match.Groups["H"].Value));
            }
        }
    }
}

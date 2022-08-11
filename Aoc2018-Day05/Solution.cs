namespace Aoc2018_Day05
{
    internal class Solution
    {
        public string Title => "Day 5: Alchemical Reduction";

        public object PartOne()
        {
            var polymer = InputFile.ReadAllText().Trim();
            var result = ReactPolymer(polymer);
            return result.Count();
        }

        public object PartTwo()
        {
            var originalPolymer = InputFile.ReadAllText().Trim();
            var polymers = ModifyPolymer(originalPolymer);
            var results = polymers.Select(p => ReactPolymer(p).Count());
            return results.Min();
        }

        private static IEnumerable<string> ModifyPolymer(string polymer)
        {
            for (var i = 0; i < 26; i++)
            {
                var result = polymer.Where(u => u != (char)('a' + i) && u != (char)('A' + i))
                                    .ToArray();
                yield return new string(result);
            }
        }

        private static IEnumerable<char> ReactPolymer(IEnumerable<char> polymer)
        {
            var units = new LinkedList<char>(polymer);

            var node = units.First;
            while (node != null)
            {
                if (node.Previous is null)
                {
                    node = node.Next;
                    continue;
                }

                if (AreReactive(node.Value, node.Previous.Value))
                {
                    var temp = node.Previous.Previous ?? node.Next;
                    units.Remove(node.Previous);
                    units.Remove(node);
                    node = temp;
                    continue;
                }

                node = node.Next;
            }

            return units;
        }

        private static bool AreReactive(char a, char b)
        {
            var result = char.IsLetter(a) && Math.Abs(a - b) == 'a' - 'A';
            return result;
        }
    }
}

namespace Aoc2018_Day02
{
    internal class Solution
    {
        public string Title => "Day 2: Inventory Management System";

        public object? PartOne()
        {
            var lines = InputFile.ReadAllLines();
            
            return NumberOfWordsWithOccurrences(2, lines) * NumberOfWordsWithOccurrences(3, lines);
        }

        public object? PartTwo()
        {
            var lines = InputFile.ReadAllLines();
            for (var i = 0; i < lines.Length; i++)
            for (var j = i + 1; j < lines.Length; j++)
            {
                var (differenceCount, matches) = CompareWords(lines[i], lines[j]);
                if (differenceCount == 1)
                    return new string(matches);
            }
            return null;
        }

        private static int NumberOfWordsWithOccurrences(int numberOfOccurrences, IEnumerable<string> words)
        {
            return words.Count(w => w.GroupBy(ch => ch)
                .Any(g => g.Count() == numberOfOccurrences));
        }

        private static (int differenceCount, char[] matches) CompareWords(string left, string right)
        {
            if (left.Length != right.Length) throw new InvalidOperationException("Words must be of equal length.");

            int differenceCount = 0;
            var matches = new List<char>(left.Length);
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] == right[i])
                    matches.Add(left[i]);
                else
                    differenceCount++;
            }

            return (differenceCount, matches.ToArray());
        }
    }
}

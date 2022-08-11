namespace Aoc2018_Day01
{
    internal class Solution
    {
        public string Title => "Day 1: Chronal Calibration";

        public object PartOne()
        {
            return ReadAdjustments().Sum();
        }
        
        public object PartTwo()
        {
            var sums = new HashSet<long>();
            var currentFrequency = 0L;
            
            var adjustments = ReadAdjustments();
            for (var adjustmentIndex = 0; ; adjustmentIndex = (adjustmentIndex + 1) % adjustments.Count)
            {
                var adjustment = adjustments[adjustmentIndex];

                currentFrequency += adjustment;
                if (sums.Contains(currentFrequency))
                {
                    return currentFrequency;
                }

                sums.Add(currentFrequency);
            }
        }

        private IList<int> ReadAdjustments() => InputFile.ReadAllLines()
                                                         .Select(int.Parse)
                                                         .ToList();
    }
}

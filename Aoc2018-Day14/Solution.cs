namespace Aoc2018_Day14;

internal class Solution
{
    public string Title => "Day 14: Chocolate Charts";

    public object PartOne()
    {
        var input = Convert.ToInt32(ReadInput());
        var board = new List<int>(new [] { 3, 7, 1, 0 });
        var currentIndexes = new[] { 0, 1 };

        while (board.Count < input + 10)
        {
            SimulateRound(board, currentIndexes);
        }

        return string.Join("", board.Skip(input).Take(10));
    }

    public object PartTwo()
    {
        var input = ReadInput().Select(c => c - '0').ToArray();
        var board = new List<int>(new [] { 3, 7, 1, 0 });
        var currentIndexes = new[] { 0, 1 };

        while (true)
        {
            var recipesAdded = SimulateRound(board, currentIndexes);
            var matchedAfter = FindSequenceNearEnd(board, input, trackBack: recipesAdded > 1);
            if (matchedAfter >= 0)
            {
                return matchedAfter;
            }
        }
    }

    private static int SimulateRound(List<int> board, int[] currentIndexes)
    {
        // Add new recipes.
        var sum = currentIndexes.Sum(i => board[i]);
        if (sum >= 10)
        {
            board.Add(sum / 10);
        }
        board.Add(sum % 10);

        // Advance elves' current recipes.
        for (var i = 0; i < currentIndexes.Length; i++)
        {
            var recipe = board[currentIndexes[i]];
            currentIndexes[i] = (currentIndexes[i] + recipe + 1) % board.Count;
        }
        
        // Return the number of recipes added.
        return sum >= 10 ? 2 : 1;
    }

    private int FindSequenceNearEnd(List<int> board, int[] input, bool trackBack)
    {
        if (board.Count < input.Length) return -1;

        if (trackBack && input.SequenceEqual(board.TakeLast(input.Length + 1).Take(input.Length)))
        {
            return board.Count - input.Length - 1;
        }

        if (input.SequenceEqual(board.TakeLast(input.Length)))
        {
            return board.Count - input.Length;
        }

        return -1;
    }

    private static string ReadInput()
    {
        return InputFile.ReadAllText().Trim();
    }
}

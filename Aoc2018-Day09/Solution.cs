using System.Text.RegularExpressions;

namespace Aoc2018_Day09;

internal class Solution
{
    public string Title => "Day 9: Marble Mania";

    public object PartOne()
    {
        var (players, lastMarble) = ReadParameters();
        return PlayMarbles(players, lastMarble);
    }

    public object PartTwo()
    {
        var (players, lastMarble) = ReadParameters();
        return PlayMarbles(players, lastMarble * 100);
    }

    private long PlayMarbles(int players, int lastMarble)
    {
        const int magicNumber = 23;

        var playerScores = Enumerable.Repeat(0L, players).ToArray();
        var nextPlayer = 0;
        var playArea = new LinkedList<int>(new[] { 0 });
        var currentMarble = playArea.First!;
        var nextMarble = 1;

        while (nextMarble <= lastMarble)
        {
            if (nextMarble % magicNumber == 0)
            {
                var remove = SelectPreviousNodeCircular(currentMarble, 7);
                playerScores[nextPlayer] += nextMarble + remove.Value;
                currentMarble = remove.Next ?? playArea.First!;
                playArea.Remove(remove);
            }
            else
            {
                var insertAfter = currentMarble.Next ?? playArea.First!;
                currentMarble = playArea.AddAfter(insertAfter, nextMarble);
            }

            nextMarble++;
            nextPlayer = ++nextPlayer % players;
        }

        return playerScores.Max();
    }

    private LinkedListNode<int> SelectPreviousNodeCircular(LinkedListNode<int> currentMarble, int count)
    {
        while (count-- > 0)
        {
            currentMarble = currentMarble.Previous ?? currentMarble.List!.Last!;
        }
        return currentMarble;
    }

    private (int Players, int LastMarble) ReadParameters()
        => InputFile.ReadAllLines()
                    .Select(s => Regex.Match(s, @"(?<Players>\d+) players; last marble is worth (?<LastMarble>\d+) points"))
                    .Where(m => m.Success)
                    .Select(m => (Convert.ToInt32(m.Groups["Players"].Value),
                                  Convert.ToInt32(m.Groups["LastMarble"].Value)))
                    .Single();
}

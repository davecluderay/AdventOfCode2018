namespace Aoc2018_Day18;

internal class Solution
{
    public string Title => "Day 18: Settlers of The North Pole";

    public object PartOne()
    {
        var area = CollectionArea.ReadInitialLayout();
        return Simulate(area, 10);
    }

    public object PartTwo()
    {
        var area = CollectionArea.ReadInitialLayout();
        return Simulate(area, 1_000_000_000);
    }

    private static int Simulate(CollectionArea area, int toMinute)
    {
        var hashes = new List<int>();
        var recurringFromMinute = -1;
        var recurringInterval = -1;

        for (var minute = 0; minute < toMinute; minute++)
        {
            // Stop and return an extrapolated value when a recurring sequence of layouts is found.
            var hash = area.CalculateLayoutHash();
            if (hashes.Contains(hash))
            {
                if (recurringFromMinute == -1)
                {
                    recurringFromMinute = hashes.IndexOf(hash);
                    recurringInterval = minute - recurringFromMinute;
                }

                if ((toMinute - minute) % recurringInterval == 0)
                {
                    break;
                }
            }
            else
            {
                hashes.Add(hash);
            }

            var ((minX, minY), (maxX, maxY)) = area.Bounds;
            for (var y = minY; y <= maxY; y++)
            for (var x = minX; x <= maxX; x++)
            {
                var position = new Position(x, y);
                var contents = area.GetAt(position);
                var adjacentContents = GetAdjacentAcreContents(area, position);

                switch (contents)
                {
                    case '.' when adjacentContents.TreeCount >= 3:
                        area.SetOnCommit(position, '|');
                        break;
                    case '|' when adjacentContents.LumberYardCount >= 3:
                        area.SetOnCommit(position, '#');
                        break;
                    case '#' when adjacentContents.LumberYardCount < 1 || adjacentContents.TreeCount < 1:
                        area.SetOnCommit(position, '.');
                        break;
                }
            }

            area.Commit();
        }

        return area.CalculateResourceValue();
    }

    private static (int TreeCount, int OpenCount, int LumberYardCount) GetAdjacentAcreContents(CollectionArea area, Position position)
    {
        var treeCount = 0;
        var openCount = 0;
        var lumberYardCount = 0;

        void Count(Position pos)
        {
            switch (area.GetAt(pos))
            {
                case '|': treeCount++;
                    break;
                case '#': lumberYardCount++;
                    break;
                case '.':
                    openCount++;
                    break;
            }
        }

        var (x, y) = position;

        Count((x - 1, y - 1));
        Count((x - 1, y    ));
        Count((x - 1, y + 1));
        Count((x    , y + 1));
        Count((x + 1, y + 1));
        Count((x + 1, y    ));
        Count((x + 1, y - 1));
        Count((x    , y - 1));

        return (treeCount, openCount, lumberYardCount);
    }
}

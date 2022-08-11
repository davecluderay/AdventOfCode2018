namespace Aoc2018_Day17
{
    internal static class WaterSpring
    {
        internal static void SimulateSteadyStateFlow(((int x, int y) offset, char[,] data) scanResult, (int x, int y) entryPoint)
        {
            FlowDownFromPoint(scanResult.data, (x: entryPoint.x - scanResult.offset.x, y: 0));
        }

        private static void FlowDownFromPoint(char[,] data, (int x, int y) entryPoint)
        {
            if (IsFallingWaterAt(entryPoint)) return; // Already handled this path of flow.

            bool IsSandAt((int x, int y) p) => data[p.y, p.x] == '.' || data[p.y, p.x] == '|';
            bool IsFallingWaterAt((int x, int y) p) => data[p.y, p.x] == '|';
            bool IsClayAt((int x, int y) p) => data[p.y, p.x] == '#';
            bool IsOutOfBounds((int x, int y) p) => p.x < 0 || p.x >= data.GetLength(1) || p.y < 0 || p.y >= data.GetLength(0);
            
            var pos = entryPoint;
            
            // Fall until we hit clay or water.
            while (true)
            {
                data[pos.y, pos.x] = '|';

                var nextPos = (pos.x, y: pos.y + 1);
                
                if (IsOutOfBounds(nextPos)) return;
                if (!IsSandAt(nextPos)) break;

                pos = nextPos;
            }

            bool SpreadLaterally()
            {
                (int x, bool isPrecipice) FindSpreadLimit(int xIncrement)
                {
                    var (x, y) = pos;
                    while (true)
                    {
                        if (IsSandAt((x, y + 1)) || IsFallingWaterAt((x, y + 1))) return (x, true);
                        if (IsClayAt((x + xIncrement, y))) return (x, false);
                        x += xIncrement;
                    }
                }

                var limit1 = FindSpreadLimit(-1);
                var limit2 = FindSpreadLimit(1);
                var fillChar = limit1.isPrecipice || limit2.isPrecipice ? '|' : '~';

                for (var x = limit1.x; x <= Math.Min(limit2.x, data.GetLength(1) - 1); x++)
                    data[pos.y, x] = fillChar;

                if (limit1.isPrecipice) FlowDownFromPoint(data, (limit1.x, pos.y + 1));
                if (limit2.isPrecipice) FlowDownFromPoint(data, (limit2.x, pos.y + 1));
                return fillChar == '~';
            }

            var isContained = SpreadLaterally();
            while (isContained)
            {
                pos.y--;
                isContained = SpreadLaterally();
            }
        }
    }
}

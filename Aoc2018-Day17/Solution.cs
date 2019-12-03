namespace Aoc2018_Day17
{
    internal class Solution
    {
        public string Title => "Day 17: Reservoir Research";

        public object PartOne()
        {
            var scanData = Scanner.Scan();
            
            WaterSpring.SimulateSteadyStateFlow(scanData, (500, 0));

            ScanRenderer.RenderToFile("completed-map.txt", scanData);

            return CalculateWaterVolume(scanData.data, includeFlowingWater: true);
        }
        
        public object PartTwo()
        {
            var scanData = Scanner.Scan();
            
            WaterSpring.SimulateSteadyStateFlow(scanData, (500, 0));

            ScanRenderer.RenderToFile("completed-map.txt", scanData);

            return CalculateWaterVolume(scanData.data, includeFlowingWater: false);
        }
        
        private static int CalculateWaterVolume(char[,] data, bool includeFlowingWater)
        {
            var result = 0;
            
            for (var y = 0; y < data.GetLength(0); y++)
            for (var x = 0; x < data.GetLength(1); x++)
            {
                var c = data[y, x];
                if (c == '~') result++;
                if (includeFlowingWater && c == '|') result++;
            }

            return result;
        }
    }
}
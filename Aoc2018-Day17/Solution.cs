using System.IO;

namespace Aoc2018_Day17
{
    internal class Solution
    {
        public string Title => "Day 17: Reservoir Research";

        public object PartOne()
        {
            var scanData = Scanner.Scan("example.txt");
            
            // Run a simulation of the water flow from the spring until steady state is reached.
            using (var writer = new StringWriter())
            {
                ScanRenderer.Render(writer, scanData);
                OutputFile.WriteAllText(writer.ToString(), "dry-map.txt");
            }

            return null;
        }
        
        public object PartTwo()
        {
            return null;
        }
    }
}
using System.IO;

namespace Aoc2018_Day17
{
    internal static class ScanRenderer
    {
        public static void RenderToFile(string fileName, ((int x, int y) offset, char[,] data) scanResult)
        {
            using var writer = new StringWriter();
            Render(writer, scanResult);
            OutputFile.WriteAllText(writer.ToString(), fileName);
        }

        private static void Render(TextWriter writer, ((int x, int y) offset, char[,] data) scanResult)
        {
            // Render the y=0 line - with the spring at position 500
            for (var x = 0; x < scanResult.data.GetLength(1); x++)
            {
                writer.Write((x + scanResult.offset.x == 500) ? '+' : '.');
            }
            writer.WriteLine();
            
            // Render any blank lines before the first line with data
            for (var y = 1; y < scanResult.offset.y; y++)
            {
                for (var x = 0; x < scanResult.data.GetLength(1); x++)
                {
                    writer.Write((x + scanResult.offset.x == 500) ? '|' : '.');
                }
            }
            
            // Render the data itself
            for (var y = 0; y < scanResult.data.GetLength(0); y++)
            {
                for (var x = 0; x < scanResult.data.GetLength(1); x++)
                {
                    writer.Write(scanResult.data[y, x]);
                }
                writer.WriteLine();
            }
        }
    }
}
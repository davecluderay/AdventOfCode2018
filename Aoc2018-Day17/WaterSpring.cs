namespace Aoc2018_Day17
{
    internal static class WaterSpring
    {
        internal static void SimulateSteadyStateFlow(((int x, int y) offset, char[,] data) scanResult, (int x, int y) springPosition)
        {
            // TODO: Start from the spring and flow downwards.
            //       When clay is beneath, flow sideways.
            //       If clay is encountered on either side, this is contained water - raise the level.
            //       If the wall stops on either side, let the water flow over the edge to continue downwards.
            //       If a hole is encountered in the clay beneath, let the water flow through it to continue downwards.
        }
    }
}
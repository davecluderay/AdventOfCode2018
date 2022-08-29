namespace Aoc2018_Day22;

internal class Region
{
    public Region(int erosionLevel, char type)
        => (ErosionLevel, Type) = (erosionLevel, type);

    public int ErosionLevel { get; }
    public char Type { get; }
}

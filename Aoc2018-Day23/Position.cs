namespace Aoc2018_Day23;

internal readonly record struct Position(int X, int Y, int Z)
{
    public static implicit operator Position((int X, int Y, int Z) position)
        => new(position.X, position.Y, position.Z);

    public static Position MaxValue = (int.MaxValue, int.MaxValue, int.MaxValue);
}

namespace Aoc2018_Day13;

internal record struct Position(int X, int Y)
{
    public static implicit operator Position((int X, int Y) position)
        => new(position.X, position.Y);
}

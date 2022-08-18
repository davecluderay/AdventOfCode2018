namespace Aoc2018_Day13;

internal class MineCart
{
    public MineCart(Position position, char orientation) => (Position, Orientation, NumberOfTurnsMade) = (position, orientation, 0);
    public Position Position { get; set; }
    public char Orientation { get; set; }
    public int NumberOfTurnsMade { get; set; }
}

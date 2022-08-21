namespace Aoc2018_Day15;

internal class Unit
{
    public Unit(Position position, char value, int attackPower)
    {
        Position = position;
        Value = value;
        AttackPower = attackPower;
    }
    
    public Position Position { get; set; }
    public char Value { get; }
    public int AttackPower { get; }
    public int HitPoints { get; set; } = 200;
}

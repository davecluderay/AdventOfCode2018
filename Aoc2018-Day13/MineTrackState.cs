namespace Aoc2018_Day13;

internal class MineTrackState
{
    public Dictionary<Position, char> TrackLayout { get; }
    public IReadOnlyList<MineCart> Carts { get; private set; }

    private MineTrackState(Dictionary<Position, char> tracks, List<MineCart> mineCarts)
        => (TrackLayout, Carts) = (tracks, mineCarts);

    public IReadOnlyList<MineCart> SelectCartsToMove()
        => Carts.OrderBy(p => (p.Position.Y, p.Position.X))
                .ToList()
                .AsReadOnly();

    public void RemoveCart(MineCart cart)
        => Carts = Carts.Where(c => c != cart)
                        .ToList();

    public static MineTrackState ReadInitialState()
    {
        var data = new Dictionary<Position, char>();
        var carts = new List<MineCart>();

        var lines = InputFile.ReadAllLines();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            var position = new Position(x, y);
            var value = lines[y][x];
            switch (value)
            {
                case '<':
                case '>':
                case 'v':
                case '^':
                    carts.Add(new(position, value));
                    break;
            }

            if (value != ' ')
            {
                data[position] = value;
            }
        }

        foreach (var cart in carts)
        {
            data[cart.Position] = InferTrackValueAt(cart.Position, data);
        }

        return new MineTrackState(data, carts);
    }

    private static char InferTrackValueAt(Position position, Dictionary<Position, char> data)
    {
        return (IsConnectedToLeft(position, data),
                IsConnectedToRight(position, data),
                IsConnectedToAbove(position, data),
                IsConnectedToBelow(position, data)) switch
               {
                   (true, true, true, true)   => '+',
                   (true, true, false, false) => '-',
                   (false, false, true, true) => '|',
                   (true, false, true, false) => '/',
                   (false, true, false, true) => '/',
                   (true, false, false, true) => '\\',
                   (false, true, true, false) => '\\',
                   _                          => throw new Exception($"Unable to infer track value at position {position}.")
               };
    }

    private static bool IsConnectedToRight(Position position, Dictionary<Position, char> data)
    {
        var otherPosition = position with { X = position.X + 1 };
        var otherValue = data.GetValueOrDefault(otherPosition, ' ');

        switch (otherValue)
        {
            case '+':
            case '-':
                return true;
            case ' ':
            case '|':
                return false;
            case '\\':
            case '/':
                return (IsConnectedToAbove(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToBelow(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToRight(otherPosition, data) ? 1 : 0) < 2;
            default:
                throw new Exception($"Unable to infer track value at position {position}.");
        }
    }

    private static bool IsConnectedToLeft(Position position, Dictionary<Position, char> data)
    {
        var otherPosition = position with { X = position.X - 1 };
        var otherValue = data.GetValueOrDefault(otherPosition, ' ');

        switch (otherValue)
        {
            case '+':
            case '-':
                return true;
            case ' ':
            case '|':
                return false;
            case '\\':
            case '/':
                return (IsConnectedToAbove(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToBelow(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToLeft(otherPosition, data) ? 1 : 0) < 2;
            default:
                throw new Exception($"Unable to infer track value at position {position}.");
        }
    }

    private static bool IsConnectedToAbove(Position position, Dictionary<Position, char> data)
    {
        var otherPosition = position with { Y = position.Y - 1 };
        var otherValue = data.GetValueOrDefault(otherPosition, ' ');

        switch (otherValue)
        {
            case '+':
            case '|':
                return true;
            case ' ':
            case '-':
                return false;
            case '\\':
            case '/':
                return (IsConnectedToLeft(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToRight(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToAbove(otherPosition, data) ? 1 : 0) < 2;
            default:
                throw new Exception($"Unable to infer track value at position {position}.");
        }
    }

    private static bool IsConnectedToBelow(Position position, Dictionary<Position, char> data)
    {
        var otherPosition = position with { Y = position.Y + 1 };
        var otherValue = data.GetValueOrDefault(otherPosition, ' ');

        switch (otherValue)
        {
            case '+':
            case '|':
                return true;
            case ' ':
            case '-':
                return false;
            case '\\':
            case '/':
                return (IsConnectedToLeft(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToRight(otherPosition, data) ? 1 : 0) +
                       (IsConnectedToBelow(otherPosition, data) ? 1 : 0) < 2;
            default:
                throw new Exception($"Unable to infer track value at position {position}.");
        }
    }
}

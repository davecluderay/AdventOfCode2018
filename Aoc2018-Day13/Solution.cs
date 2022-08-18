namespace Aoc2018_Day13;

internal class Solution
{
    public string Title => "Day 13: Mine Cart Madness";

    public object PartOne()
    {
        var state = MineTrackState.ReadInitialState();
        return PerformSimulation(state, true);
    }

    public object PartTwo()
    {
        var state = MineTrackState.ReadInitialState();
        return PerformSimulation(state, false);
    }

    private static string PerformSimulation(MineTrackState trackState, bool stopOnFirstCollision)
    {
        while (trackState.Carts.Count > 1)
        {
            var carts = trackState.SelectCartsToMove();
            foreach (var cart in carts)
            {
                if (!trackState.Carts.Contains(cart)) continue;

                var position = cart.Position;
                var orientation = cart.Orientation;
                var newPosition = orientation switch
                                  {
                                      '<' => position with { X = position.X - 1 },
                                      '>' => position with { X = position.X + 1 },
                                      '^' => position with { Y = position.Y - 1 },
                                      'v' => position with { Y = position.Y + 1 },
                                      _   => throw new Exception($"Expected a cart at {position}, but found '{orientation}'.")
                                  };
                if (carts.Any(c => c.Position == newPosition))
                {
                    if (stopOnFirstCollision)
                    {
                        return $"{newPosition.X},{newPosition.Y}";
                    }

                    trackState.RemoveCart(cart);
                    trackState.RemoveCart(carts.Single(c => c.Position == newPosition));
                    continue;
                }
        
                var newOrientation = (trackState.TrackLayout[newPosition], orientation) switch
                                     {
                                         ('/', '>')  => '^',
                                         ('/', '<')  => 'v',
                                         ('/', '^')  => '>',
                                         ('/', 'v')  => '<',
                                         ('\\', '>') => 'v',
                                         ('\\', '<') => '^',
                                         ('\\', '^') => '<',
                                         ('\\', 'v') => '>',
                                         ('+', '>') => (cart.NumberOfTurnsMade++ % 3) switch
                                                       {
                                                           0 => '^',
                                                           1 => '>',
                                                           _ => 'v'
                                                       },
                                         ('+', '<') => (cart.NumberOfTurnsMade++ % 3) switch
                                                       {
                                                           0 => 'v',
                                                           1 => '<',
                                                           _ => '^'
                                                       },
                                         ('+', '^') => (cart.NumberOfTurnsMade++ % 3) switch
                                                       {
                                                           0 => '<',
                                                           1 => '^',
                                                           _ => '>'
                                                       },
                                         ('+', 'v') => (cart.NumberOfTurnsMade++ % 3) switch
                                                       {
                                                           0 => '>',
                                                           1 => 'v',
                                                           _ => '<'
                                                       },
                                         _ => orientation
                                     };
                cart.Position = newPosition;
                cart.Orientation = newOrientation;
            }
        }

        var result = trackState.Carts.Single().Position;
        return $"{result.X},{result.Y}";
    }
}

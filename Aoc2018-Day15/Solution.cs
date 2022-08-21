namespace Aoc2018_Day15;

internal class Solution
{
    public string Title => "Day 15: Beverage Bandits";

    public object PartOne()
    {
        var cavern = Cavern.ReadInitialState();
        var outcome = SimulateCombat(cavern);
        return outcome;
    }

    public object PartTwo()
    {
        var elfAttackPower = 3;
        while (true)
        {
            var cavern = Cavern.ReadInitialState(elfAttackPower: ++elfAttackPower);

            var initialElfCount = cavern.GetRemainingElfUnits().Length;
            var outcome = SimulateCombat(cavern);
            var finalElfCount = cavern.GetRemainingElfUnits().Length;
            
            if (initialElfCount == finalElfCount)
            {
                return outcome;
            }
        }
    }

    private int SimulateCombat(Cavern cavern)
    {
        var rounds = 0;
        while (true)
        {
            foreach (var unit in cavern.GetRemainingUnits())
            {
                if (unit.HitPoints < 1) continue;

                var remainingEnemyUnits = cavern.GetRemainingEnemyUnits(unit);
                if (!remainingEnemyUnits.Any())
                {
                    return rounds * cavern.GetRemainingUnits().Sum(u => u.HitPoints);
                }

                TakeNextStep(cavern, unit, remainingEnemyUnits);
                PerformNextAttack(cavern, unit, remainingEnemyUnits);
            }

            ++rounds;
        }
    }

    private void TakeNextStep(Cavern cavern, Unit unit, Unit[] remainingEnemyUnits)
    {
        var potentialTargets = remainingEnemyUnits.SelectMany(e => GetAdjacentPositions(e.Position))
                                                  .Where(p => cavern.IsEmptySpace(p) || p == unit.Position)
                                                  .ToHashSet();
        var nextStep = FindNextStepTowardsAny(cavern, unit.Position, potentialTargets);
        if (nextStep is not null)
        {
            cavern.MoveUnit(unit, nextStep.Value);
        }
    }

    private void PerformNextAttack(Cavern cavern, Unit unit, Unit[] remainingEnemyUnits)
    {
        var enemy = GetAdjacentPositions(unit.Position)
                    .Select(p => remainingEnemyUnits.SingleOrDefault(e => e.Position == p))
                    .Where(e => e is not null)
                    .MinBy(e => (e!.HitPoints, e.Position.Y, e.Position.X));
        if (enemy is null) return;

        enemy.HitPoints -= unit.AttackPower;
        if (enemy.HitPoints < 1)
        {
            cavern.RemoveUnit(enemy);
        }
    }

    private Position? FindNextStepTowardsAny(Cavern cavern, Position from, ISet<Position> to)
    {
        if (to.Contains(from))
        {
            return null;
        }

        var minSteps = int.MaxValue;
        var routes = new List<(Position FirstStep, Position Destination, int StepCount)>();

        var explored = new HashSet<Position>();
        var toExplore = new Queue<(Position Position, int StepCount, Position? FirstStep)>();
        toExplore.Enqueue((from, 0, null));
        while (toExplore.Any())
        {
            var (position, stepCount, firstStep) = toExplore.Dequeue();

            if (explored.Contains(position)) continue;
            explored.Add(position);

            if (stepCount > minSteps)
            {
                continue;
            }
        
            if (to.Contains(position))
            {
                routes.Add((firstStep ?? position, position, stepCount));
                minSteps = Math.Min(minSteps, stepCount);
                continue;
            }

            foreach (var adjacent in GetAdjacentPositions(position).Where(cavern.IsEmptySpace))
            {
                toExplore.Enqueue((adjacent, stepCount + 1, firstStep ?? adjacent));
            }
        }

        if (!routes.Any())
        {
            return null;
        }

        var route = routes.MinBy(r => (r.StepCount, r.Destination.Y, r.Destination.X, r.FirstStep.Y, r.FirstStep.X));
        return route.FirstStep;
    }

    private IEnumerable<Position> GetAdjacentPositions(Position position)
    {
        yield return position with { Y = position.Y - 1 };
        yield return position with { X = position.X - 1 };
        yield return position with { X = position.X + 1 };
        yield return position with { Y = position.Y + 1 };
    }
}

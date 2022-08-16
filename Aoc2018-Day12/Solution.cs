using System.Text.RegularExpressions;

namespace Aoc2018_Day12;

internal class Solution
{
    public string Title => "Day 12: Subterranean Sustainability";

    public object PartOne()
    {
        return PerformPlantSimulation(generationsToSimulate: 20);
    }

    public object PartTwo()
    {
        return PerformPlantSimulation(generationsToSimulate: 50_000_000_000);
    }

    private long PerformPlantSimulation(long generationsToSimulate)
    {
        var (space, rules) = ReadInput();

        var lastRender = space.Render();
        long currentGeneration = 0L;
        while (true)
        {
            currentGeneration++;
            var bounds = space.GetBounds();
            for (var i = bounds.Low - 2; i <= bounds.High + 2; i++)
            {
                var match = space.Render(i - 2, 5);
                var rule = rules.SingleOrDefault(r => r.Match == match);
                space.SetOnCommit(i, rule?.Outcome ?? '.');
            }
            space.Commit();

            if (currentGeneration == generationsToSimulate)
            {
                // Already reached the required generation, so the simulation is over.
                break;
            }

            var render = space.Render();
            if (render.Output == lastRender.Output)
            {
                // The rendered output is the same, so work out how much it shifts by in each generation,
                // then extrapolate forward to the final generation in the simulation.
                var generationalShift = render.StartIndex - lastRender.StartIndex;
                space.Shift((generationsToSimulate - currentGeneration) * generationalShift);
                break;
            }
            
            lastRender = render;
        }

        return space.GetNonEmptyPositions().Sum();
    }

    private (Space Space, IReadOnlyList<Rule> Rules) ReadInput()
    {
        var initialStatePattern = new Regex(@"^initial state: (?<InitialState>[#\.]+)$", RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        var rulePattern = new Regex(@"(?<Match>[#\.]{5}) => (?<Outcome>[#\.])", RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        Space? space = default;
        var rules = new List<Rule>();
        
        foreach (var line in InputFile.ReadAllLines())
        {
            var match = initialStatePattern.Match(line);
            if (match.Success)
            {
                space = new Space(match.Groups["InitialState"].Value);
            }

            match = rulePattern.Match(line);
            if (match.Success)
            {
                rules.Add(new Rule(match.Groups["Match"].Value,
                                   match.Groups["Outcome"].Value.Single()));
            }
        }

        return (space!, rules);
    }

    private record Rule(string Match, char Outcome);
}

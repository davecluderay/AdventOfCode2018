using System.Text.RegularExpressions;

namespace Aoc2018_Day07;

internal class Solution
{
    public string Title => "Day 7: The Sum of Its Parts";

    public object PartOne()
    {
        var result = AssembleSleigh(numberOfElves: 1, baseStepDuration: 60);
        return result.StepsTaken;
    }

    public object PartTwo()
    {
        var result = AssembleSleigh(numberOfElves: 5, baseStepDuration: 60);
        return result.TimeTaken;
    }

    private SleighAssemblyOutcome AssembleSleigh(int numberOfElves, int baseStepDuration)
    {
        var rules = ReadRules().ToList();

        var remainingSteps = GetSteps(rules);
        var inProgressSteps = new HashSet<(string Step, int CompletionTime)>(numberOfElves);
        var completedSteps = new List<string>(remainingSteps.Count);

        var timeTaken = 0;
        while (remainingSteps.Any() || inProgressSteps.Any())
        {
            // Track completion of steps.
            foreach (var completingStep in inProgressSteps.Where(s => timeTaken >= s.CompletionTime).ToList())
            {
                inProgressSteps.Remove(completingStep);
                completedSteps.Add(completingStep.Step);
                rules.RemoveAll(r => r.DependsOnStep == completingStep.Step);
            }

            // Identify and begin the next available steps.
            var nextSteps = remainingSteps.Where(s => rules.All(r => r.Step != s))
                                          .OrderBy(s => s)
                                          .Take(numberOfElves - inProgressSteps.Count);
            foreach (var nextStep in nextSteps)
            {
                remainingSteps.Remove(nextStep);
                inProgressSteps.Add((Step: nextStep,
                                     CompletionTime: timeTaken + baseStepDuration + (nextStep[0] - 'A') + 1));
            }
            
            // Skip ahead to the next completion time.
            if (inProgressSteps.Any())
            {
                timeTaken = inProgressSteps.Min(s => s.CompletionTime);
            }
        }

        return new SleighAssemblyOutcome(string.Concat(completedSteps), timeTaken);
    }

    private IReadOnlyList<SleighAssemblyRule> ReadRules()
    {
        var pattern = new Regex("^Step (?<DependsOnStep>[A-Z]) must be finished before step (?<Step>[A-Z]) can begin.$",
                                RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
        return InputFile.ReadAllLines()
                        .Select(s => pattern.Match(s))
                        .Where(m => m.Success)
                        .Select(match => new SleighAssemblyRule(match.Groups["Step"].Value,
                                                                match.Groups["DependsOnStep"].Value))
                        .ToList();
    }

    private HashSet<string> GetSteps(IEnumerable<SleighAssemblyRule> rules)
    {
        var steps = new HashSet<string>();
        foreach (var rule in rules)
        {
            steps.Add(rule.Step);
            steps.Add(rule.DependsOnStep);
        }
        return steps;
    }

    private record SleighAssemblyRule(string Step, string DependsOnStep);
    private record SleighAssemblyOutcome(string StepsTaken, int TimeTaken);
}

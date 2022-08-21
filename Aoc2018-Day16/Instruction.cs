using System.Text.RegularExpressions;

namespace Aoc2018_Day16;

internal record Instruction(int Op, int A, int B, int C)
{
    private static readonly Regex Pattern = new(@"^(?<Op>-?\d+)\s+(?<A>-?\d+)\s+(?<B>-?\d+)\s+(?<C>-?\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public static Instruction Parse(string input)
    {
        var match = Pattern.Match(input);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        return new(Convert.ToInt32(match.Groups["Op"].Value),
                   Convert.ToInt32(match.Groups["A"].Value),
                   Convert.ToInt32(match.Groups["B"].Value),
                   Convert.ToInt32(match.Groups["C"].Value));
    }
}
using System.Text.RegularExpressions;

namespace Aoc2018_Day19;

internal record Instruction(string OpCode, int A, int B, int C)
{
    private static readonly Regex Pattern = new(@"^(?<OpCode>[^\s]+)\s+(?<A>-?\d+)\s+(?<B>-?\d+)\s+(?<C>-?\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public override string ToString() => $"{OpCode} {A} {B} {C}";

    public static bool CanRead(string input) => Pattern.IsMatch(input);

    public static Instruction Read(string input)
    {
        var match = Pattern.Match(input);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        return new(match.Groups["OpCode"].Value,
                   Convert.ToInt32(match.Groups["A"].Value),
                   Convert.ToInt32(match.Groups["B"].Value),
                   Convert.ToInt32(match.Groups["C"].Value));
    }
}

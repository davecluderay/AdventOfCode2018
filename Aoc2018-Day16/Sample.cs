using System.Text.RegularExpressions;

namespace Aoc2018_Day16;

internal record Sample(Instruction Instruction, int[] RegistersBefore, int[] RegistersAfter)
{
    private static readonly Regex SeparatorPattern = new(@"(\r?\n)+",
                                                         RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private static readonly Regex RegistersPattern = new(@"^(Before|After):\s*\[(?<R0>-?\d+),\s*(?<R1>-?\d+),\s*(?<R2>-?\d+),\s*(?<R3>-?\d+)\]",
                                                         RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public static Sample Parse(string input)
    {
        var parts = SeparatorPattern.Split(input);
        if (parts.Length != 3) throw new FormatException($"String did not match the expected format: {input}");

        var match = RegistersPattern.Match(parts[0]);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        var beforeRegisters = new[] { Convert.ToInt32(match.Groups["R0"].Value), Convert.ToInt32(match.Groups["R1"].Value), Convert.ToInt32(match.Groups["R2"].Value), Convert.ToInt32(match.Groups["R3"].Value) };

        match = RegistersPattern.Match(parts[2]);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        var afterRegisters = new[] { Convert.ToInt32(match.Groups["R0"].Value), Convert.ToInt32(match.Groups["R1"].Value), Convert.ToInt32(match.Groups["R2"].Value), Convert.ToInt32(match.Groups["R3"].Value) };

        var instruction = Instruction.Parse(parts[1]);
        return new(instruction,
                   beforeRegisters,
                   afterRegisters);
    }
}
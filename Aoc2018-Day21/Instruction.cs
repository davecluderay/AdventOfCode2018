using System.Text.RegularExpressions;

namespace Aoc2018_Day21;

internal record Instruction(string OpCode, int A, int B, int C)
{
    private static readonly Regex Pattern = new(@"^(?<OpCode>[^\s]+)\s+(?<A>-?\d+)\s+(?<B>-?\d+)\s+(?<C>-?\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public override string ToString() => $"{OpCode} {A} {B} {C}";

    public string Describe() =>
        OpCode switch
        {
            "addr" => $"R{C} = R{A} + R{B}",
            "addi" => $"R{C} = R{A} + {B}",
            "mulr" => $"R{C} = R{A} * R{B}",
            "muli" => $"R{C} = R{A} * {B}",
            "banr" => $"R{C} = R{A} & R{B}",
            "bani" => $"R{C} = R{A} & {B}",
            "borr" => $"R{C} = R{A} | R{B}",
            "bori" => $"R{C} = R{A} | {B}",
            "setr" => $"R{C} = R{A}",
            "seti" => $"R{C} = {A}",
            "gtir" => $"R{C} = ({A} > R{B}) ? 1 : 0",
            "gtri" => $"R{C} = (R{A} > {B}) ? 1 : 0",
            "gtrr" => $"R{C} = (R{A} > R{B}) ? 1 : 0",
            "eqir" => $"R{C} = ({A} == R{B}) ? 1 : 0",
            "eqri" => $"R{C} = (R{A} == {B}) ? 1 : 0",
            "eqrr" => $"R{C} = (R{A} == R{B}) ? 1 : 0",
            _      => "ERROR"
        };

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

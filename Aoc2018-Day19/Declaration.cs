using System.Text.RegularExpressions;

namespace Aoc2018_Day19;

internal record Declaration(int Value)
{
    private static readonly Regex Pattern = new(@"^#ip\s+(?<Value>\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public override string ToString() => $"#ip {Value}";

    public static bool CanRead(string input) => Pattern.IsMatch(input);

    public static Declaration Read(string input)
    {
        var match = Pattern.Match(input);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        return new(Convert.ToInt32(match.Groups["Value"].Value));
    }
}

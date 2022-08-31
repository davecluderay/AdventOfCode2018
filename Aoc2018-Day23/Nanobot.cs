using System.Text.RegularExpressions;

namespace Aoc2018_Day23;

internal record Nanobot(Position Position, int Range)
{
    private static readonly Regex Pattern = new(@"^pos=<(?<X>-?\d+),\s*(?<Y>-?\d+),\s*(?<Z>-?\d+)>,\s*r=(?<R>\d+)$",
                                                RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public static bool CanRead(string input) => Pattern.IsMatch(input);

    public static Nanobot Read(string input)
    {
        var match = Pattern.Match(input);
        if (!match.Success) throw new FormatException($"String did not match the expected format: {input}");
        return new((Convert.ToInt32(match.Groups["X"].Value),
                    Convert.ToInt32(match.Groups["Y"].Value),
                    Convert.ToInt32(match.Groups["Z"].Value)),
                   Convert.ToInt32(match.Groups["R"].Value));
    }
}

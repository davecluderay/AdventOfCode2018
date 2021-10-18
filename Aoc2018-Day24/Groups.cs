using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2018_Day24
{
    internal static class Groups
    {
        public static IEnumerable<Group> ReadAll(string? fileName = null)
        {
            GroupType? currentGroupType = null;
            var currentSequence = 0;
            foreach (var line in InputFile.ReadAllLines(fileName))
            {
                if (line == "Immune System:")
                {
                    currentGroupType = GroupType.ImmuneSystem;
                    currentSequence = 0;
                    continue;
                }

                if (line == "Infection:")
                {
                    currentGroupType = GroupType.Infection;
                    currentSequence = 0;
                    continue;
                }

                if (string.IsNullOrEmpty(line)) continue;
                if (currentGroupType == null) throw new Exception("Expected group type header.");

                yield return ParseGroup(line, currentGroupType.Value, ++currentSequence);
            }
        }

        private static Group ParseGroup(string line, GroupType groupType, int sequenceNumber)
        {
            var pattern = new Regex(
                @"(?<Units>\d+) units each with (?<HpPerUnits>\d+) hit points (\((?<WeakOrImmune1>weak|immune) to (?<attackType1>\S+(,\s*\S+)*)(; (?<WeakOrImmune2>weak|immune) to (?<attackType2>\S+(,\s*\S+)*))?\) )?with an attack that does (?<DealsDamage>\d+) (?<DamageType>\S+) damage at initiative (?<Initiative>\d+)",
                RegexOptions.ExplicitCapture);
            var match = pattern.Match(line);
            if (!match.Success)
                throw new Exception($"CANNOT MATCH: {line}");

            var group = new Group(groupType, sequenceNumber, Convert.ToInt32(match.Groups["Units"].Value), Convert.ToInt32(match.Groups["HpPerUnits"].Value), (match.Groups["WeakOrImmune1"].Value == "immune" ? match.Groups["attackType1"].Value : match.Groups["WeakOrImmune2"].Value == "immune" ? match.Groups["attackType2"].Value : "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray(), (match.Groups["WeakOrImmune1"].Value == "weak" ? match.Groups["attackType1"].Value : match.Groups["WeakOrImmune2"].Value == "weak" ? match.Groups["attackType2"].Value : "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray(), Convert.ToInt32(match.Groups["DealsDamage"].Value), match.Groups["DamageType"].Value, Convert.ToInt32(match.Groups["Initiative"].Value));
            return group;
        }
    }
}

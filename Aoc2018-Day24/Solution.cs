using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2018_Day24
{
    internal class Solution
    {
        public string Title => "Day 24: Immune System Simulator 20XX";

        public object PartOne()
        {
            var (immuneSystemUnitsRemaining, infectionUnitsRemaining) = Simulate();
            return Math.Max(immuneSystemUnitsRemaining, infectionUnitsRemaining);
        }

        public object PartTwo()
        {
            var boost = 0;
            while (true)
            {
                var (immuneSystemUnitsRemaining, infectionUnitsRemaining) = Simulate(boost++);
                if (immuneSystemUnitsRemaining > 0 && infectionUnitsRemaining == 0)
                    return immuneSystemUnitsRemaining;
            }
        }

        private static (int immuneSystemUnitsRemaining, int infectionUnitsRemaining) Simulate(int immunityBoost = 0)
        {
            var allGroups = Groups.ReadAll().ToList();
            foreach (var g in allGroups.Where(g => g.GroupType == GroupType.ImmuneSystem))
            {
                g.AttackDamage += immunityBoost;
            }

            var stalemate = false;
            while (allGroups.GroupBy(g => g.GroupType).Count() > 1 && !stalemate)
            {
                stalemate = true;

                // Decide a pecking order for target selection.
                var selectorOrder = allGroups.OrderByDescending(x => (x.EffectivePower, x.Initiative))
                                             .ToList();

                // Select targets
                var pairs = new List<(Group attacker, Group? target)>();
                foreach (var attacker in selectorOrder)
                {
                    var target = allGroups
                                 .Where(g => g.GroupType != attacker.GroupType && pairs.All(p => p.target != g))
                                 .OrderByDescending(t => (attacker.InflictsDamage(t), t.EffectivePower, t.Initiative))
                                 .FirstOrDefault(t => attacker.InflictsDamage(t) > 0);
                    pairs.Add((attacker, target));
                }

                // Fight
                pairs = pairs.Where(p => p.target != null)
                             .OrderByDescending(p => p.attacker.Initiative)
                             .ToList();
                foreach (var p in pairs)
                {
                    var attacker = p.attacker;
                    var target   = p.target;

                    var damage        = attacker.InflictsDamage(target!);
                    var numberOfKills = Math.Min(target!.NumberOfUnits, damage / target.HitPoints);

                    target.NumberOfUnits -= numberOfKills;
                    if (numberOfKills > 0)
                        stalemate = false;
                }

                allGroups = allGroups.Where(x => x.NumberOfUnits > 0).ToList();
            }

            return (allGroups.Where(g => g.GroupType == GroupType.ImmuneSystem)
                             .Sum(g => g.NumberOfUnits),
                    allGroups.Where(g => g.GroupType == GroupType.Infection)
                             .Sum(g => g.NumberOfUnits));
        }
    }
}

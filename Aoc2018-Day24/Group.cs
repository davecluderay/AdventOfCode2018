using System.Linq;

namespace Aoc2018_Day24
{
    internal class Group
    {
        public string    GroupIdentifier { get; }
        public GroupType GroupType       { get; }
        public int       NumberOfUnits   { get; set; }
        public int       HitPoints   { get; }
        public string[]  ImmuneTo        { get; }
        public string[]  WeakTo          { get; }
        public int       AttackDamage { get; set; }
        public string    AttackType      { get; }
        public int       Initiative      { get; }

        public int EffectivePower => (NumberOfUnits > 0 ? NumberOfUnits : 0) * AttackDamage;

        public Group(GroupType groupType, int sequenceNumber, int numberOfUnits, int hitPoints,
            string[]           immuneTo,
            string[]           weakTo, int attackDamage, string attackType, int initiative)
        {
            GroupType = groupType;
            NumberOfUnits = numberOfUnits;
            HitPoints = hitPoints;
            ImmuneTo = immuneTo;
            WeakTo = weakTo;
            AttackDamage = attackDamage;
            AttackType = attackType;
            Initiative = initiative;

            GroupIdentifier = groupType == GroupType.Infection
                ? $"Infection group {sequenceNumber}"
                : $"Immune System group {sequenceNumber}";
        }

        public int InflictsDamage(Group target)
        {
            if (target == null) return 0;

            var isImmune = target.ImmuneTo.Contains(AttackType);
            if (isImmune) return 0;

            var isWeak = target.WeakTo.Contains(AttackType);
            if (isWeak) return EffectivePower * 2;

            return EffectivePower;
        }

        public override string ToString()
        {
            return
                $"{GroupIdentifier} {nameof(NumberOfUnits)}: {NumberOfUnits}, {nameof(HitPoints)}: {HitPoints}, {nameof(ImmuneTo)}: {string.Join(", ", ImmuneTo)}, {nameof(WeakTo)}: {string.Join(", ", WeakTo)}, {nameof(AttackDamage)}: {AttackDamage}, {nameof(AttackType)}: {AttackType}, {nameof(Initiative)}: {Initiative}";
        }
    }
}
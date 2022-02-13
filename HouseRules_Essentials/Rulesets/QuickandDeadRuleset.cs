namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class QuickandDeadRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Quick and the Dead";
            const string description = "A Skirmish mode with a small hand but fast turn around time on cards means you need to not hesitate.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(1.5f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new CardLimitModifiedRule(7));
        }
    }
}

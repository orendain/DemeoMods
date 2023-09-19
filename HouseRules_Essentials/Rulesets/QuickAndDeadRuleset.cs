namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class QuickAndDeadRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Quick and the Dead";
            const string description = "A mode with a small hand but fast turnaround time on cards means you need to not hesitate.";
            const string longdesc = "";

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                new CardEnergyFromAttackMultipliedRule(1.5f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new CardLimitModifiedRule(7));
        }
    }
}

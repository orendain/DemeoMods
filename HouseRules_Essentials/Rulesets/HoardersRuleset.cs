namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HoardersRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hoarders - Skirmish Only!";
            const string description = "A large hand size but you may not get them fast enough.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(0.9f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new CardLimitModifiedRule(22));
        }
    }
}

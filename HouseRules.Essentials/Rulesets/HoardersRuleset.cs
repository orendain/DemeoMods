namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class HoardersRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hoarders - Skirmish Only!";
            const string description = "A large hand size but you may not get them fast enough.";
            const string longdesc = "";

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                new CardEnergyFromAttackMultipliedRule(0.9f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new CardLimitModifiedRule(22));
        }
    }
}

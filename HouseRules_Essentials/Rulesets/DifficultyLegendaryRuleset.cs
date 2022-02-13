namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyLegendaryRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Legendary";
            const string description = "This Multiplayer mode increases the default game difficulty for those who want to be a legend.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(0.6f),
                new CardEnergyFromRecyclingMultipliedRule(0.6f),
                new EnemyHealthScaledRule(1.4f),
                new EnemyAttackScaledRule(1.4f));
        }
    }
}

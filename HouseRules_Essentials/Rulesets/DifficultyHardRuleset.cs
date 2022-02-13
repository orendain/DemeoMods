namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyHardRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Hard";
            const string description = "This mode increases the default game difficulty for a greater challenge.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(0.8f),
                new CardEnergyFromRecylcingMultipliedRule(0.8f),
                new EnemyHealthScaledRule(1.2f),
                new EnemyAttackScaledRule(1.2f));
        }
    }
}

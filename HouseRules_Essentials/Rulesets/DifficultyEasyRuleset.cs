namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyEasyRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Easy";
            const string description = "This mode decreases the default game difficulty for a more casual playstyle.";

            return Ruleset.NewInstance(
                name,
                description,
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true),
                new CardEnergyFromAttackMultipliedRule(1.5f),
                new EnemyHealthScaledRule(0.8f),
                new EnemyAttackScaledRule(0.6f));
        }
    }
}

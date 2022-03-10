namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyEasyRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Easy";
            const string description = "Decreased game difficulty for a more casual playstyle.";

            return Ruleset.NewInstance(
                name,
                description,
                new EnemyDoorOpeningDisabledRule(true),
                new EnemyRespawnDisabledRule(true),
                new CardEnergyFromAttackMultipliedRule(1.5f),
                new CardEnergyFromRecyclingMultipliedRule(1.5f),
                new EnemyHealthScaledRule(0.8f),
                new LevelPropertiesModifiedRule("BigGoldPileChance", 100, "FloorOneHealingFountains", 2, "FloorOneLootChests", 4, "FloorTwoHealingFountains", 3, "FloorTwoLootChests", 5, "FloorThreeHealingFountains", 2, "FloorThreeLootChests", 2));
        }
    }
}

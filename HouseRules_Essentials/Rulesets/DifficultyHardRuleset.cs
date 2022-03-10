namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyHardRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Hard";
            const string description = "Increased game difficulty for a greater challenge.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(0.8f),
                new CardEnergyFromRecyclingMultipliedRule(0.8f),
                new EnemyHealthScaledRule(1.8f),
                new EnemyAttackScaledRule(1.5f),
                new LevelPropertiesModifiedRule("BigGoldPileChance", 30, "FloorOneHealingFountains", 1, "FloorOneLootChests", 2, "FloorTwoHealingFountains", 1, "FloorTwoLootChests", 2, "FloorThreeHealingFountains", 1, "FloorThreeLootChests", 1));                
        }
    }
}

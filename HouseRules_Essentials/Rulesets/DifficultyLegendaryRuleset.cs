namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyLegendaryRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Legendary";
            const string description = "Increased game difficulty for those who want to be a legend.";

            return Ruleset.NewInstance(
                name,
                description,
                new CardEnergyFromAttackMultipliedRule(0.5f),
                new CardEnergyFromRecyclingMultipliedRule(0.5f),
                new EnemyHealthScaledRule(2.2f),
                new EnemyAttackScaledRule(2.0f);
                new LevelPropertiesModifiedRule("BigGoldPileChance", 10, "FloorOneHealingFountains", 0, "FloorOneLootChests", 1, "FloorTwoHealingFountains", 1, "FloorTwoLootChests", 1, "FloorThreeHealingFountains", 0, "FloorThreeLootChests", 1));                
        }
    }
}

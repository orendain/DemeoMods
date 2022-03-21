namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyLegendaryRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Legendary";
            const string description = "Increased game difficulty for those who want to be a legend.";

            var levelProperties = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 10 },
                { "FloorOneHealingFountains", 0 },
                { "FloorOneLootChests", 1 },
                { "FloorOneGoldMaxAmount", 504 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 1 },
                { "FloorTwoGoldMaxAmount", 252 },
                { "FloorThreeHealingFountains", 0 },
                { "FloorThreeLootChests", 1 },
            });

            var cardEnergyAttack = new CardEnergyFromAttackMultipliedRule(0.5f);
            var cardEnergyRecycle = new CardEnergyFromRecyclingMultipliedRule(0.5f);
            var enemyScaleHealth = new EnemyHealthScaledRule(2.2f);
            var enemyScaleAttack = new EnemyAttackScaledRule(2.0f);

            return Ruleset.NewInstance(
                name,
                description,
                cardEnergyAttack,
                cardEnergyRecycle,
                enemyScaleAttack,
                enemyScaleHealth,
                levelProperties);
        }
    }
}

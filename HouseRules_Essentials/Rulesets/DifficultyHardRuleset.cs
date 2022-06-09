namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyHardRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Hard";
            const string description = "Increased game difficulty for a greater challenge.";

            var cardEnergyAttack = new CardEnergyFromAttackMultipliedRule(0.8f);
            var cardEnergyRecycle = new CardEnergyFromRecyclingMultipliedRule(0.8f);
            var enemyScaleHealth = new EnemyHealthScaledRule(1.8f);
            var enemyScaleAttack = new EnemyAttackScaledRule(1.5f);

            var levelPropertiesModified = new LevelPropertiesModifiedRule(new LevelPropertiesModifiedRule.ConfigData
            {
                Adjustments = new Dictionary<string, int>
                {
                  { "BigGoldPileChance", 30 },
                  { "FloorOneHealingFountains", 1 },
                  { "FloorOneLootChests", 2 },
                  { "FloorOneGoldMaxAmount", 672 },
                  { "FloorTwoHealingFountains", 1 },
                  { "FloorTwoLootChests", 2 },
                  { "FloorTwoGoldMaxAmount", 420 },
                  { "FloorThreeHealingFountains", 1 },
                  { "FloorThreeLootChests", 1 },
                },
                Limit = new List<Boardgame.GameConfigType> { Boardgame.GameConfigType.None },
            });

            return Ruleset.NewInstance(
                name,
                description,
                cardEnergyAttack,
                cardEnergyRecycle,
                enemyScaleAttack,
                enemyScaleHealth,
                levelPropertiesModified);
        }
    }
}

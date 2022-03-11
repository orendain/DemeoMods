﻿namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyHardRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Hard";
            const string description = "Increased game difficulty for a greater challenge.";

            var cardEnergyAttck = new CardEnergyFromAttackMultipliedRule(0.8f);
            var cardEnegyRecycle = new CardEnergyFromRecyclingMultipliedRule(0.8f);
            var EnemyScaleHealth = new EnemyHealthScaledRule(1.8f);
            var EnemyScaleAttack = new EnemyAttackScaledRule(1.5f);

            var levelPropertiesModified = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
              { "BigGoldPileChance", 30 },
              { "FloorOneHealingFountains", 1 },
              { "FloorOneLootChests", 2 },
              { "FloorOneGoldMaxAmount", 672},
              { "FloorTwoHealingFountains", 1 },
              { "FloorTwoLootChests", 2 },
              { "FloorTwoGoldMaxAmount", 420},
              { "FloorThreeHealingFountains", 1 },
              { "FloorThreeLootChests", 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                cardEneryAttack,
                cardEnergyRecycle,
                EnemyScaleAttack,
                EnemyScaleHealth,
                levelPropertiesRule);
        }
    }
}

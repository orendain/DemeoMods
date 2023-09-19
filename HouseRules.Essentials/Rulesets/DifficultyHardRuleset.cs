﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class DifficultyHardRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Hard";
            const string description = "Increased game difficulty for a greater challenge.";
            const string longdesc = "Less fountains on floor 2 and less chests and gold per floor!\n\n80% energy gained for attacks and recycled cards!\n\nEnemies at 180% normal health and they hit 50% harder!";

            var cardEnergyAttack = new CardEnergyFromAttackMultipliedRule(0.8f);
            var cardEnergyRecycle = new CardEnergyFromRecyclingMultipliedRule(0.8f);
            var enemyScaleHealth = new EnemyHealthScaledRule(1.8f);
            var enemyScaleAttack = new EnemyAttackScaledRule(1.5f);

            var levelPropertiesModified = new LevelPropertiesModifiedRule(new Dictionary<string, int>
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
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                cardEnergyAttack,
                cardEnergyRecycle,
                enemyScaleAttack,
                enemyScaleHealth,
                levelPropertiesModified);
        }
    }
}

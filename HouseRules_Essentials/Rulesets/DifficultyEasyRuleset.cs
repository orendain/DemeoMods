namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DifficultyEasyRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Easy";
            const string description = "Decreased game difficulty for a more casual playstyle.";

            var cardEnergyAttack = new CardEnergyFromAttackMultipliedRule(1.5f);
            var cardEnergyRecycle = new CardEnergyFromRecyclingMultipliedRule(1.5f);
            var enemyScaleHealth = new EnemyHealthScaledRule(0.8f);
            var enemyDoorDisabled = new EnemyDoorOpeningDisabledRule(true);
            var enemyRespawnDisabled = new EnemyRespawnDisabledRule(true);

            var levelPropertiesModified = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
              { "BigGoldPileChance", 100 },
              { "FloorOneHealingFountains", 2 },
              { "FloorOneLootChests", 4 },
              { "FloorTwoHealingFountains", 3 },
              { "FloorTwoLootChests", 5 },
              { "FloorThreeHealingFountains", 2 },
              { "FloorThreeLootChests", 2 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                cardEnergyAttack,
                cardEnergyRecycle,
                enemyScaleHealth,
                enemyDoorDisabled,
                enemyRespawnDisabled,
                levelPropertiesModified);
        }
    }
}

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

            var cardEnergyAttck = new CardEnergyFromAttackMultipliedRule(1.5f);
            var cardEnegyRecycle = new CardEnergyFromRecyclingMultipliedRule(1.5f);
            var EnemyScaleHealth = new EnemyHealthScaledRule(0.8f);
            var EnemyDoorDisabled = new EnemyDoorOpeningDisabledRule(true);
            var EnemyRespawnDisabled = new EnemyRespawnDisabledRule(true);

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
                cardEneryAttack,
                cardEnergyRecycle,
                EnemyScaleHealth,
                EnemyDoorDisabled,
                EnemyRespawnDisabled,
                levelPropertiesRule);

        }
    }
}

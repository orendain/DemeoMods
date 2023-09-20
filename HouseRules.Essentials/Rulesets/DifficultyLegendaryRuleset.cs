namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class DifficultyLegendaryRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Difficulty: Legendary";
            const string description = "Increased game difficulty for those who want to be a legend.";
            const string longdesc = "No fountains except 1 on floor 2 and less chests and gold per floor!\n\n50% energy gained for attacks and recycled cards!\n\nEnemies at 220% normal health and they hit 100% harder!";

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
                longdesc,
                cardEnergyAttack,
                cardEnergyRecycle,
                enemyScaleAttack,
                enemyScaleHealth,
                levelProperties);
        }
    }
}

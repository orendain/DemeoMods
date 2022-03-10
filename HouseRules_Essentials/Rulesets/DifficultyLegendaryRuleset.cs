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

            var levelProperties = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 10 },
                { "FloorOneHealingFountains", 0 },
                { "FloorOneLootChests", 1 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 1 },
                { "FloorThreeHealingFountains", 0 },
                { "FloorThreeLootChests", 1 },
            });

            var cardEneryAttack = new CardEnergyFromAttackMultipliedRule(0.5f);
            var cardEnergyRecycle = new CardEnergyFromRecyclingMultipliedRule(0.5f);
            var EnemyScaleHealth = new EnemyHealthScaledRule(2.2f);
            var EnemyScaleAttack = new EnemyAttackScaledRule(2.0f);

            return Ruleset.NewInstance(
                name,
                description,
                cardEneryAttack,
                cardEnergyRecycle,
                EnemyScaleAttack,
                EnemyScaleHealth,
                levelProperties);
        }
    }
}

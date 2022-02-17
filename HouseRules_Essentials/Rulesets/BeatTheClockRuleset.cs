namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class BeatTheClockRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Beat The Clock!";
            const string description = "Ultra health. Ultra card recycling. Only 15 rounds to escape...";

            var healthRule = new StartHealthAdjustedRule(new Dictionary<string, int>
            {
                { "HeroGuardian", 190 },
                { "HeroSorcerer", 190 },
                { "HeroRouge", 190 },
                { "HeroHunter", 190 },
                { "HeroBard", 190 },
            });
            var recyclingRule = new CardEnergyFromRecyclingMultipliedRule(5);
            var roundLimitRule = new RoundCountLimitedRule(15);
            var levelRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneLootChests", 15 },
                { "FloorTwoLootChests", 15 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                healthRule,
                recyclingRule,
                roundLimitRule,
                levelRule);
        }
    }
}

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

            var healthRule = new Essentials.Rules.PieceConfigAdjustedRule(new List<List<string>>
            {
                new List<string> { "HeroGuardian", "StartHealth", "200" },
                new List<string> { "HeroSorcerer", "StartHealth", "200" },
                new List<string> { "HeroRouge", "StartHealth", "200" },
                new List<string> { "HeroHunter", "StartHealth", "200" },
                new List<string> { "HeroBard", "StartHealth", "200" },
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

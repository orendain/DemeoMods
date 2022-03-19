namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class BeatTheClockRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Beat The Clock!";
            const string description = "Ultra health. Ultra card recycling. Only 15 rounds to escape...";

            var healthRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 200 },
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
